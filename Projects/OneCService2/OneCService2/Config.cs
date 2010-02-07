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
		[ConfigurationProperty("Port")]
		public int Port
		{
			get{return (int)this["Port"];}
            set{this["Port"] = value; }
		}
		
		[ConfigurationProperty("Host")]
		public string Host
		{
			get{return (string)this["Host"];}
            set{this["Host"] = value; }
		}
	}	
	
	public class Connection 
	{
		private Dictionary<string, string> parameters = new Dictionary<string, string>();		
		
		public Dictionary<string, string> Parameters
		{
			get {return parameters;}			
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
                        
            foreach (XmlNode node in nodes)
            {
            	Connection connection = new Connection();
            	foreach (XmlAttribute attr in node.Attributes)
            	{
            		connection.Parameters.Add(attr.Name, attr.Value);
            	}            	                
            	
            	if (connection.Parameters.ContainsKey("Name"))
            	{
            		connections.Add(connection.Parameters["Name"], connection);
            	}
            	else
            	{
            		throw new Exception("Connection must contain attribute Name");
            	}
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
