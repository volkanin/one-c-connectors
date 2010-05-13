/*
 * Created by SharpDevelop.
 * User: Andrei Mejov <andrei.mejov@gmail.com>
 * Date: 07.01.2010
 * Time: 18:14
 *  
 */
using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
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
		
		//Сериализтор
		private object     xdtoSer = null;
		
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
		
		protected virtual string GetProgId()
		{
			return "V81.ComConnector";
		}
						
		public override void Init()
		{
			comConnector = CreateInstanceByProgId(GetProgId());
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
				ReleaseRCW(Connection);								
			}
			catch (Exception _e)
			{	
				Logger.Severe("Exception in adapter Done: " + _e.ToString());
			}
				
			ReleaseRCW(ConnectionPtr);
			try
			{
				ReleaseRCW(comConnector);
			}
			catch (Exception _e)
			{	
				Logger.Severe("Exception in adapter Done: " + _e.ToString());
			}
		}				
									
		/*Полезные методы*/
		public override ResultSet ExecuteScript(string _script)
		{	
			RestoreConnectionRCW();
			ResultSet resultSet = new ResultSet();
			DataHelper dataHelper = new DataHelper();
			Invoke(Connection, "OneCService2_ВыполнитьСкрипт", new object[] {_script, resultSet, dataHelper});
			return resultSet;
		}		
		
		public override ResultSet ExecuteRequest(string _request)
		{
			RestoreConnectionRCW();			
			ResultSet resultSet = new ResultSet();
			DataHelper dataHelper = new DataHelper();
			Invoke(Connection, "OneCService2_ВыполнитьЗапрос", new object[] {_request, resultSet, dataHelper});
			return resultSet;					
		}
		
		public override ResultSet ExecuteMethod(string _methodName, XmlNode[] _parameters)
		{
			RestoreConnectionRCW();			
			ResultSet resultSet = new ResultSet();
			DataHelper dataHelper = new DataHelper();
			Invoke(Connection, "OneCService2_ВыполнитьМетод", new object[] {_methodName, _parameters, resultSet, dataHelper});
			return resultSet;			
		}
	}
}
