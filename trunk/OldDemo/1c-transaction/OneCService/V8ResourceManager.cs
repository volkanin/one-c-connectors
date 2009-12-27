/*
 * Created by SharpDevelop.
 * User: Andrei Mejov <andrei.mejov@gmail.com>
 * Date: 12.10.2009
 * Time: 20:21
 *  
 */
using System;
using System.Transactions;
using System.Collections.Generic;

namespace OneCService
{
	public class V8ResourceManager : IEnlistmentNotification, IDisposable
	{		
		private Guid        resourceGuid = Guid.NewGuid();	
		private AppDomain         domain = null;				
		
		public AppDomain Domain
		{
			set {
					domain = value;
					if (domain.GetData("globalCOMStorage") == null)
					{
						domain.SetData("globalCOMStorage", new Dictionary<Guid, OneCAdapter>());
					}
				}
			get {return domain;}
		}
		
		public OneCAdapter Adapter
		{
			set {					
					Dictionary<Guid, OneCAdapter> globalCOMStorage = 
						(Dictionary<Guid, OneCAdapter>)domain.GetData("globalCOMStorage");
					globalCOMStorage.Add(resourceGuid, value);
				}
			get {					
					Dictionary<Guid, OneCAdapter> globalCOMStorage =
						(Dictionary<Guid, OneCAdapter>)domain.GetData("globalCOMStorage");
					return globalCOMStorage[resourceGuid];
				}
		}				
		
		public Guid ResourceGuid
		{
			set {resourceGuid = value;}
			get {return resourceGuid;}
		}
		
		public void Prepare(PreparingEnlistment preparingEnlistment)
		{		
			preparingEnlistment.Prepared();
		}
		
		public void Commit(Enlistment enlistment)
		{
			try
			{		
				//Console.WriteLine("Commit GUID:" + resourceGuid);
				Adapter.Commit();				
				enlistment.Done();
			}
			catch (Exception _e)
			{				
				SimpleLogger.DefaultLogger.Severe("Error on commit: "+_e.ToString());
			}
			finally
			{
				TryClose();
			}
		}

		public void Rollback(Enlistment enlistment)
		{
			try
			{								
				Adapter.Rollback();					
				enlistment.Done();
			}
			catch (Exception _e)
			{
				SimpleLogger.DefaultLogger.Severe("Error on rollback: "+_e.ToString());
			}
			finally
			{
				TryClose();
			}
		}
		
		public void InDoubt(Enlistment enlistment)
		{
			try
			{								
				Adapter.Rollback();
				enlistment.Done();
			}
			catch (Exception _e)
			{
				Console.WriteLine(_e);
			}
			finally
			{
				TryClose();
			}
		}
		
		public void Dispose()
		{	
			try
			{				
				Adapter.Rollback();
			}
			catch (Exception _e)
			{
				//Console.WriteLine(_e);
			}	
			finally
			{
				TryClose();
			}
		}
		
		private void TryClose()
		{
			try
			{
				Adapter.Close();
			}
			catch (Exception _e)
			{
				//Console.WriteLine(_e);
			}
		}
	}
}
