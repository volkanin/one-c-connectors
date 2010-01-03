/*
 * Created by SharpDevelop.
 * User: Mejov Andrei <andrei.mejov@gmail.com>
 * Date: 01.01.2009
 * Time: 15:34
 *  
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Messaging;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

namespace MOMAdapters
{			
	[AutoLocate("MSMQAdapter")]
	[ComVisible(true)]	
	[ComSourceInterfacesAttribute(typeof(IAdapter))]	
	[ClassInterface(ClassInterfaceType.AutoDual)]	
	[ProgId("MOMAdapters.MSMQAdapter")]
	[Guid("F89F8898-CE26-4A69-833F-D7F11F81B894")]
	public class MSMQAdapter : IAdapter
	{
		private static readonly string AdapterType = "MSMQ";
		
		public static readonly string ReceiveQueueName = "ReceiveQueueName";
		public static readonly string    SendQueueName = "SendQueueName";
		public static readonly string         UserName = "UserName";
		public static readonly string         Password = "Password";
		public static readonly string           UseZip = "UseZip";		
		
		private MessageQueue                  receiveQueue = null;
		private MessageQueue                     sendQueue = null;
		private MessageQueueTransaction        transaction = null;	
		private bool                    transactionStarted = false;
		private bool                            useZipFlag = false;	
		
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
			if (sendQueue == null)
			{
				throw new Exception("This connector not configured for sending");
			}
			if (!transactionStarted)
			{
				Begin();
			}
			if (useZipFlag)
			{
				sendQueue.Send(CompressString(_text), transaction);
			}
			else
			{
				sendQueue.Send(_text, transaction);
			}
		}
		
		public bool HasMessage()
		{			
			if (receiveQueue == null)
			{
				throw new Exception("This connector not configured for receiving");
			}
			IAsyncResult r = receiveQueue.BeginPeek();
			Thread.Sleep(20);			
			return r.IsCompleted;			
		}
		
		public string Receive()
		{
			if (receiveQueue == null)
			{
				throw new Exception("This connector not configured for receiving");
			}
			if (!transactionStarted)
			{
				Begin();
			}
			
			Message message = receiveQueue.Receive(transaction);			
			if (useZipFlag)
			{				
				return DecompressString((byte[])(message.Body));
			}				
			else
			{
				return message.Body.ToString();
			}			
		}
		
		private void ValidateParameters()
		{
			if (!((Parameters.ContainsKey(SendQueueName)) || (Parameters.ContainsKey(ReceiveQueueName))))
			{
				throw new Exception("SendQueueName and/or ReceiveQueueName must be set");
			}
			if ((Parameters.ContainsKey(UserName)) && (!Parameters.ContainsKey(Password)))
			{
				throw new Exception("Password must not be empty");
			}
		}
		
		public void Start()
		{
			ValidateParameters();
			transaction = new MessageQueueTransaction();
			
			if (MessageQueue.Exists(this.Parameters[ReceiveQueueName]))
			{
				receiveQueue = new MessageQueue(this.Parameters[ReceiveQueueName], true);
			}
			else
			{
				receiveQueue = MessageQueue.Create(this.Parameters[ReceiveQueueName], true);
			}
			if (MessageQueue.Exists(this.Parameters[SendQueueName]))
			{
				sendQueue = new MessageQueue(this.Parameters[SendQueueName], true);
			}
			else
			{
				sendQueue = MessageQueue.Create(this.Parameters[SendQueueName], true);
			}
			if (Parameters.ContainsKey(UseZip))
			{
				if (
					(Parameters[UseZip].Equals("true")) || (Parameters[UseZip].Equals("True")) ||
				    (Parameters[UseZip].Equals("1"))
				   )
				{
					useZipFlag = true;
					sendQueue.Formatter = new BinaryMessageFormatter();
					receiveQueue.Formatter = new BinaryMessageFormatter();					
				}
			}			
			if (!useZipFlag)
			{
				sendQueue.Formatter = new XmlMessageFormatter(new Type[]{typeof(string)});
				receiveQueue.Formatter = new XmlMessageFormatter(new Type[]{typeof(string)});
			}
		}
		
		public void Stop()
		{			
			if (transactionStarted)
			{
				transaction.Abort();
			}
			receiveQueue.Dispose();			
			sendQueue.Dispose();
			
			receiveQueue = null;
			sendQueue = null;
			
			useZipFlag = false;
		}
		
		public void Begin()
		{
			transaction.Begin();
			transactionStarted = true;
		}
		
		public void Commit()
		{
			if (transactionStarted)
			{
				transaction.Commit();
				transactionStarted = false;
			}
		}
		
		public void Rollback()
		{
			if (transactionStarted)
			{
				transaction.Abort();
				transactionStarted = false;
			}
		}

		public static byte[] CompressString(string _s)
		{
			byte[] buf = UTF8Encoding.UTF8.GetBytes(_s);
			using (MemoryStream ms = new MemoryStream())
			{
				using (DeflateStream ds = new DeflateStream(ms, CompressionMode.Compress))
				{					
					ds.Write(buf,0, buf.Length);
					ds.Close();			
					return ms.GetBuffer();
				}
			}
		}
						
		public static string DecompressString(byte[] _bytes)
		{			
			using (MemoryStream ms = new MemoryStream(_bytes))
			{				
				using (DeflateStream ds = new DeflateStream(ms, CompressionMode.Decompress))
				{					
					using (MemoryStream resStream = new MemoryStream())
					{						
						int b = ds.ReadByte();
						while (b>0)
						{							
							resStream.WriteByte((byte)b);
							b = ds.ReadByte();
						}						
						byte[] buf = new byte[resStream.Position];						
						resStream.Seek(0L, SeekOrigin.Begin);
						resStream.Read(buf, 0, buf.Length);
						return new string(UTF8Encoding.UTF8.GetChars(buf));
					}//using resStream
				}//using ds
			}//using ms		
		}
		
		public void Dispose()
		{
			Stop();
		}
	}
}
