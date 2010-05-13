/*
 * Created by SharpDevelop.
 * User: Andrei Mejov <andrei.mejov@gmail.com>
 * Date: 07.02.2010
 * Time: 13:47
 *  
 */
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
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
			try
			{				
				if (ConnectionPool.Pools.ContainsKey(_connectionName))
				{							
					AbstractAdapter adapter = ConnectionPool.Pools[_connectionName].GetConnection(_poolUserName, _poolPassword);
					try
					{
						return adapter.ExecuteRequest(_request);						
					}
					finally
					{
						ConnectionPool.Pools[_connectionName].ReleaseConnection(adapter);
					}
				}
				else
				{
					throw new Exception("Connection with name '"+_connectionName+"' not found");
				}
			}
			catch (Exception _e)
			{
				SimpleLogger.DefaultLogger.Severe("Exception in ExecuteRequest: "+_e.ToString());
				ResultSet resultSet = new ResultSet();
				resultSet.Error = _e.ToString();
				return resultSet;
			}
		}
		
		
		ResultSet IOneCWebService2.ExecuteScript(
												string _connectionName, 
												string _poolUserName, 
												string _poolPassword, 
												string _script
							   					)
		{
			try
			{				
				if (ConnectionPool.Pools.ContainsKey(_connectionName))
				{							
					AbstractAdapter adapter = ConnectionPool.Pools[_connectionName].GetConnection(_poolUserName, _poolPassword);
					try
					{
						return adapter.ExecuteScript(_script);						
					}
					finally
					{
						ConnectionPool.Pools[_connectionName].ReleaseConnection(adapter);
					}
				}
				else
				{
					throw new Exception("Connection with name '"+_connectionName+"' not found");
				}
			}
			catch (Exception _e)
			{
				SimpleLogger.DefaultLogger.Severe("Exception in ExecuteRequest: "+_e.ToString());
				ResultSet resultSet = new ResultSet();
				resultSet.Error = _e.ToString();
				return resultSet;
			}
		}
		
		
		ResultSet IOneCWebService2.ExecuteMethod(
												string     _connectionName, 
												string     _poolUserName, 
												string     _poolPassword, 
												string     _methodName,
												Parameters _parameters
							   					)
		{
			try
			{				
				if (ConnectionPool.Pools.ContainsKey(_connectionName))
				{							
					AbstractAdapter adapter = ConnectionPool.Pools[_connectionName].GetConnection(_poolUserName, _poolPassword);
					try
					{
						return adapter.ExecuteMethod(_methodName, _parameters.Params);						
					}
					finally
					{
						ConnectionPool.Pools[_connectionName].ReleaseConnection(adapter);
					}
				}
				else
				{
					throw new Exception("Connection with name '"+_connectionName+"' not found");
				}
			}
			catch (Exception _e)
			{
				SimpleLogger.DefaultLogger.Severe("Exception in ExecuteRequest: "+_e.ToString());
				ResultSet resultSet = new ResultSet();
				resultSet.Error = _e.ToString();
				return resultSet;
			}
		}
		
		public static ServiceHost CreateServiceHost(string _hostName, int _port)
		{
			Uri uri = new Uri("http://"+_hostName+":"+_port+"/OneCService2");
			ServiceHost serviceHost = new ServiceHost(typeof(OneCWebService2), uri);						
			
			BasicHttpBinding binding = new BasicHttpBinding();
			//WSHttpBinding binding = new WSHttpBinding();
			binding.Namespace = "http://OneCService2";		
			//binding.Security.Mode = SecurityMode.None;	
			binding.MaxBufferPoolSize = 10*1000*1000;
			binding.MaxReceivedMessageSize = 10*1000*1000;
			
			ServiceMetadataBehavior b = serviceHost.Description.Behaviors.Find<ServiceMetadataBehavior>();			
			
			serviceHost.AddServiceEndpoint(typeof(IOneCWebService2), binding, uri, uri);			
			
			if (b == null)
			{
				b = new ServiceMetadataBehavior();				
								
				serviceHost.Description.Behaviors.Add(b);
			}			
			
			b.HttpGetEnabled = true;			
			
			ServiceThrottlingBehavior t = serviceHost.Description.Behaviors.Find<ServiceThrottlingBehavior>();
			
			if (t == null)
			{
				t = new ServiceThrottlingBehavior();
				
				serviceHost.Description.Behaviors.Add(t);
			}
			
			t.MaxConcurrentCalls = 20;
			t.MaxConcurrentInstances = 20;
			t.MaxConcurrentSessions = 20;			
			
			return serviceHost;
		}
	}
}
