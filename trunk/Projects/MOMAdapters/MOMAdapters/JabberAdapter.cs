/*
 * Created by SharpDevelop.
 * User: Mejov Andrei <andrei.mejov@gmail.com>
 * Date: 21.01.2009
 * Time: 16:47
 *  
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

using agsXMPP;
using agsXMPP.net;
using agsXMPP.protocol.client;
using agsXMPP.Xml.Dom;

namespace MOMAdapters
{
	[ComVisible(true)]	
	[ComSourceInterfacesAttribute(typeof(IAdapter))]	
	[ClassInterface(ClassInterfaceType.AutoDual)]	
	[ProgId("JabberAdapter")]	
	[Guid("0339E962-6744-4844-9408-1A795828CCEE")]	
	public class JabberAdapter : IAdapter
	{
		private static readonly string 					  AdapterType = "Jabber";
			
		public static readonly string                  ReceiveAddress = "ReceiveAddress";
		public static readonly string                     SendAddress = "SendAddress";
		public static readonly string                          Server = "Server";
		public static readonly string                        Password = "Password";	
		public static readonly string IgnoreMessageFromUnknownAddress = "IgnoreMessageFromUnknownAddress";
		public static readonly string                     StorageFile = "StorageFile";
		
		private XmppClientConnection xmpp = null;
		
		private string userName = null;
		private string password = null;
		private string   server = null;
		private Int32?     port = null;
		
		private Queue<Message> messageBuffer = new Queue<Message>();
		
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
		
		public void Start()
		{			
			ValidateParameters();
			bool waitFlag = false;						
			
			xmpp = new XmppClientConnection();			
			xmpp.Server = server;
			xmpp.ConnectServer = server;
			
			if (port != null)
			{
				xmpp.Port = (int)port;
				if ((port == 80) || (port == 7070))
				{				
					xmpp.SocketConnectionType = SocketConnectionType.HttpPolling;					
				}
			}
			
			xmpp.UseSSL = true;
			xmpp.UseStartTLS = true;
			xmpp.Username = userName;
			xmpp.Password = password;
			xmpp.Status = "Online";
			xmpp.UseCompression = false;
			xmpp.KeepAlive = true;
			xmpp.AutoAgents = true;
			xmpp.AutoPresence = true;
			xmpp.AutoResolveConnectServer = false;
			xmpp.AutoRoster = true;					
			//xmpp.ClientLanguage = "Russian";						
			
			xmpp.OnStreamError += delegate(object sender, Element e) 
			{  
				throw new Exception("Xmpp stream error: "+e.ToString());
			};
			xmpp.OnSocketError += delegate(object sender, Exception ex) 
			{  
				throw ex;
			};
			xmpp.OnError += delegate(object sender, Exception ex) 
			{  
				throw ex;
			};									
			xmpp.OnLogin += delegate(object sender) 
			{  								
				waitFlag = true;
			};		
			xmpp.OnXmppConnectionStateChanged += delegate(object sender, XmppConnectionState state) 
			{  
				Console.WriteLine(state.ToString());
			};
			xmpp.OnAuthError += delegate(object sender, Element e) 
			{  
				Console.WriteLine(e.ToString());
			};
						
			xmpp.Open();			
			
			int counter = 0;
			while (!waitFlag) 
			{
				System.Threading.Thread.Sleep(500);
				counter++;
				
				if (counter >= 100)
				{
					throw new Exception("Connection timeout!");
				}
			}						
				
			xmpp.OnMessage += delegate(object sender, Message msg) 
			{ 
				lock (messageBuffer)
				{
					if (Parameters.ContainsKey(IgnoreMessageFromUnknownAddress))
					{
						if (Parameters[IgnoreMessageFromUnknownAddress].Equals("true"))
						{
							if (msg.From.User.ToString().Equals(Parameters[SendAddress]))
							{
								messageBuffer.Enqueue(msg);
							}
						}
						else
						{
							messageBuffer.Enqueue(msg);
						}
					}
					else
					{
						messageBuffer.Enqueue(msg);
					}					
				}
			};
		}
		
		private void ValidateParameters()
		{
			if (!
			   	(
			    	Parameters.ContainsKey(ReceiveAddress) && 
			    	Parameters.ContainsKey(SendAddress) && 
			    	Parameters.ContainsKey(Password) &&
			    	Parameters.ContainsKey(Server)
			    )
			   )
			{
				throw new Exception("ReceiveAddress, SendAddress and Password must be set");
			}
			else
			{
				string[] sa = Parameters[ReceiveAddress].Split(new char[] {'@'});
				if (sa.Length != 2)
				{
					throw new Exception("Illegal address format: "+Parameters[ReceiveAddress]);
				}
				else
				{
					userName = sa[0];
					password = Parameters[Password];
				}			
				sa = Parameters[Server].Split(new char[] {':'});
				if (sa.Length == 2)
				{
					server = sa[0];
					port = Convert.ToInt32(sa[1]);
				}
				else
				{
					server = Parameters[Server];
				}
			}
		}
		
		public void Stop()
		{
			if (xmpp != null)
			{
				xmpp.Close();
				xmpp = null;
				userName = null;
				password = null;
				server = null;
				port = null;
			}
			messageBuffer.Clear();
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
			Message m = new Message(Parameters[SendAddress], _text);
			m.Type = MessageType.chat;
			xmpp.Send(m);
		}
		
		public bool HasMessage()
		{
			lock (messageBuffer)
			{
				return messageBuffer.Count > 0;
			}
		}
		
		
		public string Receive()
		{
			lock (messageBuffer)
			{				
				Message msg = messageBuffer.Dequeue();				
				return msg.Body;
			}
		}
		
		public void Begin()
		{
			
		}
		
		public void Commit()
		{
			
		}
		
		public void Rollback()
		{
			
		}
		
		public void Dispose()
		{
			Stop();
		}
	}
}
