/*
 * Created by SharpDevelop.
 * User: Andrei Mejov <andrei.mejov@gmail.com>
 * Date: 13.01.2010
 * Time: 21:35
 *  
 */
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;

namespace OneCService2
{
	public class ServiceConfig : ConfigurationSection
	{
		[ConfigurationProperty("port")]
		public int Port
		{
			get{return (int)this["port"];}
             set{this["port"] = value; }
		}
		
		[ConfigurationProperty("host")]
		public string Host
		{
			get{return (string)this["host"];}
            set{this["host"] = value; }
		}
	}	
	
	public class Connection 
	{
		private string name;
		
		public string Name
		{
			get {return name;}
			set {name = value;}
		}
	}
	
	public class Connections : IConfigurationSectionHandler
	{
		public object Create(
								object _parent,
               					object _configContext, 
               					XmlNode _section
               				)
        {
            Dictionary<string, Connection> connections = new Dictionary<string, Connection>();            
            XmlNodeList nodes = _section.SelectNodes("Connection");
                        
            foreach (XmlNode node in nodes){
            	Connection connection = new Connection();
                connection.Name = node.Attributes["name"].InnerText;                
                connections.Add(connection.Name, connection);
            }
            return connections;            
        }
	}
	
	public class Config
	{
		private static ServiceConfig                serviceConfig = null;
		private static Dictionary<string, Connection> connections = null;
		
		public static ServiceConfig ServiceConfig
		{
			get
			{
				if (serviceConfig == null)
				{
					serviceConfig = (ServiceConfig)ConfigurationSettings.GetConfig("ServiceConfig");
				}
				return serviceConfig;
			}
		}				
		
		public static Dictionary<string, Connection> Connections
		{
			get
			{
				if (connections == null)
				{
					connections = (Dictionary<string, Connection>)ConfigurationSettings.GetConfig("Connections");
				}
				return connections;
			}
		}
	}
}
