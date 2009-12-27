/*
 * Created by SharpDevelop.
 * User: Mejov Andrei <mini_root@freemail.ru>
 * Date: 27.05.2009
 * Time: 18:39
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

namespace OneCService
{	
	public class OneCService : ServiceBase
	{
		public const string MyServiceName = "OneCService";
		
		private ServiceHost host = null;
		
		public OneCService()
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
			string hostName = "localhost";
			int    port = 9000;
			if (args.Length >= 2)
			{
				hostName = args[0];
				port = Convert.ToInt32(args[1]);
			}
			host = OneCWebService.CreateHost(hostName, port);
			host.Open();
			//System.Console.WriteLine("Start web service");
		}
				
		protected override void OnStop()
		{
			host.Close();
			//System.Console.WriteLine("Stop web service");
		}
	}
}
