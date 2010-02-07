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

namespace OneCService2
{
	public class OneCService2 : ServiceBase
	{
		public const string MyServiceName = "OneCService2";
		
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
				ConnectionPool.PoolsPrepare(SimpleLogger.DefaultLogger);
				try
				{
					ConnectionPool.PoolsInit(SimpleLogger.DefaultLogger);
				}
				finally
				{
					ConnectionPool.PoolsDone(SimpleLogger.DefaultLogger);
				}
			}
			catch (Exception _e)
			{
				SimpleLogger.DefaultLogger.Severe(_e.ToString());
			}
		}
		
		
		protected override void OnStop()
		{
			SimpleLogger.DefaultLogger.Dispose();
		}
	}
}
