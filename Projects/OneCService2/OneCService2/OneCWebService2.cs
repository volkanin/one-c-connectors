/*
 * Created by SharpDevelop.
 * User: Andrei Mejov <andrei.mejov@gmail.com>
 * Date: 07.02.2010
 * Time: 13:47
 *  
 */
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Xml;

namespace OneCService2
{
	[ServiceBehavior(
					IncludeExceptionDetailInFaults=true, 
					Name="OneCService2", 
					Namespace="http://OneCService2"
				    )]
	public class OneCWebService2 : IOneCWebService2
	{
		public OneCWebService2()
		{
		}
		
		
		ResultSet IOneCWebService2.ExecuteRequest(
												string _connectionName, 
												string _poolUserName, 
												string _poolPassword, 
												string _request
							    				 )
		{
			return null;
		}
		
		
		ResultSet IOneCWebService2.ExecuteScript(
												string _connectionName, 
												string _poolUserName, 
												string _poolPassword, 
												string _script
							   					)
		{
			return null;
		}
		
		
		ResultSet IOneCWebService2.ExecuteMethod(
												string _connectionName, 
												string _poolUserName, 
												string _poolPassword, 
												string _methodName,
												XmlNode[] _parameters
							   					)
		{
			return null;
		}
		
		public static ServiceHost CreateServiceHost(string _hostName, int _port)
		{
			Uri uri = new Uri("http://"+_hostName+":"+_port+"/OneCService2");
			ServiceHost serviceHost = new ServiceHost(typeof(OneCWebService2), uri);
			
			BasicHttpBinding binding = new BasicHttpBinding();
			binding.Namespace = "http://OneCService2";
			
			ServiceMetadataBehavior b = serviceHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
			
			serviceHost.AddServiceEndpoint(typeof(IOneCWebService2), binding, uri);
			
			if (b == null)
			{
				b = new ServiceMetadataBehavior();
				b.HttpGetEnabled = true;
				
				serviceHost.Description.Behaviors.Add(b);
			}
			else
			{
				b.HttpGetEnabled = true;
			}
			
			return serviceHost;
		}
	}
}
