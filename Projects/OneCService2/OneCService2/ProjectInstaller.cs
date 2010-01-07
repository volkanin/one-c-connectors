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
using System.Configuration.Install;
using System.ServiceProcess;

namespace OneCService2
{
	[RunInstaller(true)]
	public class ProjectInstaller : Installer
	{
		private ServiceProcessInstaller serviceProcessInstaller;
		private ServiceInstaller serviceInstaller;
		
		public ProjectInstaller()
		{
			serviceProcessInstaller = new ServiceProcessInstaller();
			serviceInstaller = new ServiceInstaller();
			// Here you can set properties on serviceProcessInstaller or register event handlers
			serviceProcessInstaller.Account = ServiceAccount.LocalService;
			
			serviceInstaller.ServiceName = OneCService2.MyServiceName;
			this.Installers.AddRange(new Installer[] { serviceProcessInstaller, serviceInstaller });
		}
	}
}
