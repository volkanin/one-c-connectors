/*
 * Created by SharpDevelop.
 * User: Mejov Andrei <mini_root@freemail.ru>
 * Date: 29.05.2009
 * Time: 1:36
 * 
 */
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Xml;

namespace OneCService
{	
	[ServiceBehavior(IncludeExceptionDetailInFaults=true, Name="onecservice", Namespace="http://onecservice")]
	public class OneCWebService : IOneCWebService
	{
		public OneCWebService()
		{			
		}
		
		ResultSet IOneCWebService.ExecuteRequest(string _file, string _usr, string _pwd, string _request)
		{
			try
			{
				OneCAdapter adapter = new OneCAdapter();
				try
				{
					if (_usr.Equals(""))
					{
						adapter.Open(_file, null, null);
					}
					else
					{
						adapter.Open(_file, _usr, _pwd);	
					}				
					if (adapter.ExecuteRequest(_request))
					{
						return adapter.ToResultSet();
					}
					else
					{
						return new ResultSet();
					}
				}
				finally
				{
					try
					{
						adapter.Close();
					}
					catch (Exception _e) {}
				}
			}
			catch (Exception _e)
			{
				ResultSet r = new ResultSet();
				r.Error = _e.ToString();
				return r;
			}
		}
		
		ResultSet IOneCWebService.ExecuteScript(string _file, string _usr, string _pwd, string _script)
		{
			try
			{
				OneCAdapter adapter = new OneCAdapter();
				try
				{
					if (_usr.Equals(""))
					{
						adapter.Open(_file, null, null);
					}
					else
					{
						adapter.Open(_file, _usr, _pwd);	
					}															
										
					return adapter.ExecuteScriptForResultSet(_script);
				}
				finally
				{
					try
					{
						adapter.Close();
					}
					catch (Exception _e) {}
				}
			}
			catch (Exception _e)
			{
				ResultSet r = new ResultSet();
				r.Error = _e.ToString();
				return r;
			}
		}
		
		ResultSet IOneCWebService.ExecuteMethodWithXDTO(
														string    _file, 
														string    _usr, 
														string    _pwd, 
														string    _methodName, 
														XmlNode[] _parameters
														)
		{
			try
			{
				OneCAdapter adapter = new OneCAdapter();
				try
				{
					if (_usr.Equals(""))
					{
						adapter.Open(_file, null, null);
					}
					else
					{
						adapter.Open(_file, _usr, _pwd);	
					}															
										
					return adapter.ExecuteMethodWithXDTO(_methodName, _parameters);
				}
				finally
				{
					try
					{
						adapter.Close();
					}
					catch (Exception _e) {}
				}
			}
			catch (Exception _e)
			{
				ResultSet r = new ResultSet();
				r.Error = _e.ToString();
				return r;
			}
		}
		
		public static ServiceHost CreateHost(string _hostName, int _port)
		{			
			Uri uri = new Uri("http://"+_hostName+":"+_port+"/onecservice");
			ServiceHost host = new ServiceHost(typeof(OneCWebService), uri);
			
			BasicHttpBinding binding = new BasicHttpBinding();			
			binding.Namespace = "http://onecservice";
			
			host.AddServiceEndpoint(typeof(IOneCWebService), binding, uri);
			
			//Добавим возможность получать WSDL				
			ServiceMetadataBehavior b = host.Description.Behaviors.Find<ServiceMetadataBehavior>();
			
			if (b == null)
			{
				b = new ServiceMetadataBehavior();
				
				b.HttpGetEnabled = true;				
				host.Description.Behaviors.Add(b);
			}
			else
			{				
				b.HttpGetEnabled = true;
			}
						
			
			return host;			
		}
	}
}
