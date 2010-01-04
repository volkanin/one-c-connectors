/*
 * Created by SharpDevelop.
 * User: Mejov Andrei <andrei.mejov@gmail.com>
 * Date: 18.08.2009
 * Time: 21:35
 * 
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Apache.NMS;
using Apache.NMS.ActiveMQ;

namespace MOMAdapters
{
	[AutoLocate("ActiveMQAdapter")]
	[ComVisible(true)]	
	[ComSourceInterfacesAttribute(typeof(IAdapter))]	
	[ClassInterface(ClassInterfaceType.AutoDual)]	
	[ProgId("MOMAdapters.ActiveMQAdapter")]
	[Guid("4B8D93BC-304F-4A5D-AC5E-9D91F3240A78")]	
	public class ActiveMQAdapter : IAdapter
	{
		private static readonly string 					  AdapterType = "ActiveMQ";
		
		public static readonly string         ReceiveDestinationName = "ReceiveDestinationName";
		public static readonly string            SendDestinationName = "SendDestinationName";
		public static readonly string       ActiveMQConnectionString = "ActiveMQConnectionString";
		public static readonly string                       UserName = "UserName";
		public static readonly string                       Password = "Password";
		public static readonly string 					      UseZip = "UseZip";
		
		private IConnection          connection = null;
		private ISession                session = null;
		private IDestination    sendDestination = null;
		private IDestination receiveDestination = null;
		private IMessageProducer       producer = null;
		private IMessageConsumer       consumer = null;
		private Queue<string>     messageBuffer = null;
		private bool                 useZipFlag = false;
		
		private Dictionary<string, string>      parameters = new Dictionary<string, string>();				
	
		[ComVisible(false)]
		public Dictionary<string, string> Parameters
		{
			get {return parameters;}			
		}
		
		public string GetAdapterType() 
		{
			return AdapterType;
		}
		
		public void SetParameter(string _name, string _value)
		{
			parameters.Add(_name, _value);
		}
		
		public void ClearParameters()
		{
			parameters.Clear();
		}		
		
		private IDestination CreateDestination(string _s)
		{
			string[] sa = _s.Split(new char[]{'/'});
			if (sa.Length != 2)
			{
				throw new Exception("Illegal destination format: "+_s);
			}
			else
			{
				if (sa[0].Equals("queue"))
				{					
					return session.GetQueue(sa[1]);
				}
				else
				{
					return session.GetTopic(sa[1]);
				}
			}
		}
		
		public void Start()
		{			
			ValidateParameters();
			ConnectionFactory factory = new ConnectionFactory(Parameters[ActiveMQConnectionString]);
			if ((Parameters.ContainsKey("UserName")) && (Parameters.ContainsKey("Password")))
			{
				connection = factory.CreateConnection(Parameters["UserName"], Parameters["Password"]);
			}
			else
			{
				connection = factory.CreateConnection();
			}						
			connection.Start();
			
			session = connection.CreateSession(AcknowledgementMode.Transactional);					
						
			if (Parameters.ContainsKey(SendDestinationName))
			{
				sendDestination = CreateDestination(Parameters[SendDestinationName]);				
				producer = session.CreateProducer(sendDestination);
			}
			
			if (Parameters.ContainsKey(ReceiveDestinationName))
			{
				receiveDestination = CreateDestination(Parameters[ReceiveDestinationName]);
				consumer = session.CreateConsumer(receiveDestination);	
				((Apache.NMS.ActiveMQ.MessageConsumer)consumer).RedeliveryTimeout = 500;
			}
		
			if (Parameters.ContainsKey(UseZip))
			{
				if (
					(Parameters[UseZip].Equals("true")) || (Parameters[UseZip].Equals("True")) ||
				    (Parameters[UseZip].Equals("1"))
				   )
				{
					useZipFlag = true;				
				}
			}			
			
			messageBuffer = new Queue<string>();
		}		
		
		private void ValidateParameters()
		{
			if (!((Parameters.ContainsKey(SendDestinationName)) || (Parameters.ContainsKey(ReceiveDestinationName))))
			{
				throw new Exception("SendDestinationName and/or ReceiveDestinationName must be set");
			}
			if ((Parameters.ContainsKey(UserName)) && (!Parameters.ContainsKey(Password)))
			{
				throw new Exception("Password must not be empty");
			}
		}

		public void Stop()
		{	
			if (session != null)
			{				
				if (producer != null)
				{
					producer.Close();
				}
				producer = null;
				if (consumer != null)
				{
					consumer.Close();
				}
				consumer = null;
				session.Close();
				session = null;
				if (messageBuffer != null)
				{
					lock (messageBuffer)
					{
						messageBuffer.Clear();
						messageBuffer = null;
					}
				}
				connection.Stop();
				connection.Close();
				connection = null;
				
				useZipFlag = false;
			}			
		}
		
		public void SendFile(string _fileName)
		{
			using (FileStream fs = new FileStream(_fileName, FileMode.Open))
			{
				using (StreamReader sr = new StreamReader(fs, System.Text.UTF8Encoding.UTF8, true))
				{
					string buf = "";
					while (!sr.EndOfStream) 
					{
						buf += sr.ReadLine();
					}
					Send(buf);
				}
			}
		}

		public void Send(string _text)
		{			
			if (producer == null)
			{
				throw new Exception("This connector not configured for sending");
			}
			IMessage message = null;
			if (useZipFlag)
			{				
				message = session.CreateBytesMessage(MSMQAdapter.CompressString(_text));
			}
			else
			{
				message = session.CreateTextMessage(_text);
			}			
			message.NMSPersistent = true;
			message.NMSTimeToLive = TimeSpan.MaxValue;			
			producer.Send(message);			
		}
		
		private string ReceiveMessage(bool _noWait)
		{		
			IMessage message = null;
			
			if (_noWait)
			{
				message = consumer.ReceiveNoWait();
				if (message == null)
				{
					return null;
				}
			}
			else
			{
				message = consumer.Receive();
			}
			
			if (useZipFlag)
			{
				return MSMQAdapter.DecompressString(((IBytesMessage)message).Content);
			}
			else if (message is ITextMessage)
			{
				return ((ITextMessage)message).Text;
			}
			else if (message is IBytesMessage)			
			{				
				byte[] buf = ((IBytesMessage)message).Content;
				char[] chars = System.Text.UTF8Encoding.UTF8.GetChars(buf);
				return new String(chars);
			}
			else
			{
				return message.ToString();
			}
		}
		
		public string Receive()
		{		
			if (consumer == null)
			{
				throw new Exception("This connector not configured for receiving");
			}
			lock (messageBuffer)
			{
				if (messageBuffer.Count > 0)
				{
					return messageBuffer.Dequeue();
				}
				else
				{
					return ReceiveMessage(false);
				}
			}
		}
		
		public bool HasMessage()
		{			
			if (consumer == null)
			{
				throw new Exception("This connector not configured for receiving");
			}
			lock (messageBuffer)
			{
				if (messageBuffer.Count > 0)
				{
					return true;
				}
				else
				{
					string s = ReceiveMessage(true);
					if (s != null)
					{				
						messageBuffer.Enqueue(s);
						return true;
					}
					else
					{
						return false;
					}
				}
			}
		}
		
		public void Begin()
		{			
		}
		
		public void Commit()
		{			
			lock (messageBuffer)
			{
				messageBuffer.Clear();
				session.Commit();
			}
		}
		
		public void Rollback()
		{	
			lock (messageBuffer)
			{
				messageBuffer.Clear();
				session.Rollback();
				((Apache.NMS.ActiveMQ.MessageConsumer)consumer).RedeliverRolledBackMessages();
			}
		}
		
		public void Dispose()
		{
			Stop();			
		}
	}
}
