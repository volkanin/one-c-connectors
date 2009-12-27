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
using System.Transactions;

namespace OneCService
{	
	[ServiceBehavior(
					IncludeExceptionDetailInFaults=true, 
					Name="onecservice", 
					Namespace="http://onecservice", 
					TransactionTimeout="00:00:45"
					)
	]
	public class OneCWebService : IOneCWebService
	{				
		
		public OneCWebService()
		{
			
		}
		
		private void EnlistToTransaction(OneCAdapter _adapter)
		{
			if (Transaction.Current != null)
			{
				V8ResourceManager manager = new V8ResourceManager();
				manager.Domain = AppDomain.CurrentDomain;
				manager.Adapter = _adapter;				
					
				Transaction.Current.EnlistDurable(manager.ResourceGuid, manager, EnlistmentOptions.None);
				
				_adapter.Begin();
			}
			else
			{
				Exception e = new Exception("Ambient transaction not found!");
				SimpleLogger.DefaultLogger.Severe(e.ToString());
				throw e;
			}
		}
		
		[OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]		
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
										
					EnlistToTransaction(adapter);
					
					//Выполняем запрос
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
		
		[OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]		
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
					
					EnlistToTransaction(adapter);
										
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
		
		[OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
		[TransactionFlow(TransactionFlowOption.Mandatory)]
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
					
					EnlistToTransaction(adapter);
										
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
		
		public static ServiceHost CreateHost(string _hostName, int _port, string _logFileName)
		{			
			SimpleLogger.CreateDefaultLogger(_logFileName);
			
			Uri uri = new Uri("http://"+_hostName+":"+_port+"/onecservice");
			ServiceHost host = new ServiceHost(typeof(OneCWebService), uri);
						
			//BasicHttpBinding binding = new BasicHttpBinding();
			//Для поддержки транзакций используем другую привязку
			WSHttpBinding binding = new WSHttpBinding();			
			//Разрешим использования потока транзакций
			binding.TransactionFlow=true;			
			binding.Namespace = "http://onecservice";			
			//Другие настройки, под девизом "ничего лишнего"
			binding.AllowCookies = false;
			binding.MaxBufferPoolSize = 20;
			binding.Security.Mode = SecurityMode.None;			
			
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
