/*
 * Created by SharpDevelop.
 * User: Andrei Mejov <andrei.mejov@gmail.com>
 * Date: 03.01.2010
 * Time: 19:58
 *  
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;

using System.ServiceModel;

namespace OneCService2
{
	public class OneCService2 : ServiceBase
	{
		public const string MyServiceName = "OneCService2";
		
		private ServiceHost serviceHost = null;
		
		public OneCService2()
		{
			InitializeComponent();
		}
		
		private void InitializeComponent()
		{
			this.ServiceName = MyServiceName;
		}
		
		protected override void Dispose(bool disposing)
		{			
			base.Dispose(disposing);
		}
				
		protected override void OnStart(string[] args)
		{		
			SimpleLogger.CreateDefaultLogger(@"D:\out.log");
			try
			{
				string host = Config.ServiceConfig.Host;
				int port = Config.ServiceConfig.Port;				
				SimpleLogger.DefaultLogger.Info("Starting OneCService2: host="+host+" port="+port);
				
				ConnectionPool.PoolsPrepare(SimpleLogger.DefaultLogger);								
				ConnectionPool.PoolsInit(SimpleLogger.DefaultLogger);
				
				serviceHost = OneCWebService2.CreateServiceHost(host, port);
				serviceHost.Open();

			}
			catch (Exception _e)
			{
				SimpleLogger.DefaultLogger.Severe(_e.ToString());
			}
		}
		
		
		protected override void OnStop()
		{	
			try
			{
				serviceHost.Close();
			}
			catch (Exception _e)
			{
				SimpleLogger.DefaultLogger.Severe("Exception in OnStop: "+_e.ToString());
			}						
			ConnectionPool.PoolsDone(SimpleLogger.DefaultLogger);			
			SimpleLogger.DefaultLogger.Dispose();
		}
	}
}
