/*
 * Created by SharpDevelop.
 * User: Andrei Mejov <andrei.mejov@gmail.com>
 * Date: 07.01.2010
 * Time: 18:14
 *  
 */
using System;
using System.Xml;

namespace OneCService2
{
	public class V81Adapter : AbstractAdapter
	{
		public static readonly string     ModeParam = "Mode";
		public static readonly string UserNameParam = "UserName";
		public static readonly string PasswordParam = "Password";
		public static readonly string     FileParam = "File";
		public static readonly string   ServerParam = "Server";
		public static readonly string     BaseParam = "Base";
		
		private string   fileConnectionStringTemplate = "File=${File}; Usr=${UserName}; Pwd=${Password}";		
		private string serverConnectionStringTemplate = "Srvr=${Server}; Ref=${Base}; Usr=${UserName}; Pwd=${Password}";
		
		private object comConnector = null;
		
		public string FileConnectionStringTemplate
		{
			set {fileConnectionStringTemplate = value;}
			get {return fileConnectionStringTemplate;}
		}
		
		public string ServerConnectionStringTemplate
		{
			set {serverConnectionStringTemplate = value;}
			get {return serverConnectionStringTemplate;}
		}
						
		public override void Init()
		{
			comConnector = CreateInstanceByProgId("V81.ComConnector");
			string connectionString = null;
			if (Parameters[ModeParam].Equals("Server"))
			{
				ValidateParameters(new string[] {"Server", "Base", "UserName", "Password"});
				connectionString = PrepareConnectionString(serverConnectionStringTemplate);
			}
			else
			{
				ValidateParameters(new string[] {"File", "UserName", "Password"});
				connectionString = PrepareConnectionString(fileConnectionStringTemplate);
			}
			
			Connection = Invoke(comConnector, "Connect", new object[] {connectionString});			
		}
		
		public override void Done()
		{
			RestoreConnectionRCW();			
			try
			{
				ReleaseRCW(comConnector);
			}
			catch (Exception _e)
			{	
				Logger.Severe("Exception in adapter Done: " + _e.ToString());
			}
			
			RestoreConnectionRCW();			
			try
			{
				ReleaseRCW(Connection);
			}
			catch (Exception _e)
			{	
				Logger.Severe("Exception in adapter Done: " + _e.ToString());
			}
		}				
		
		protected override XmlNode Serialize(object _o)
		{
			throw new NotImplementedException();
		}
		
		protected override object DeSerialize(XmlNode _node)
		{
			throw new NotImplementedException();
		}
		
		protected override SupportedType GetTypeForValue(object _o)
		{
			throw new NotImplementedException();
		}
		
		public override object ExecuteScript(string _script)
		{			
			return Invoke(Connection, "OneCService_ВыполнитьСтроку", new object[] {_script});						    
		}		
		
		public override ResultSet ExecuteRequest(string _request)
		{
			throw new NotImplementedException();
		}
		
		public override object ExecuteMethod(string _methodName, object[] _parameters)
		{
			throw new NotImplementedException();
		}
	}
}
