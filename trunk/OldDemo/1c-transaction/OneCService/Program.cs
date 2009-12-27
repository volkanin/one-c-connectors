/*
 * Created by SharpDevelop.
 * User: Mejov Andrei <mini_root@freemail.ru>
 * Date: 27.05.2009
 * Time: 18:39
 *  
 */
using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace OneCService
{
	static class Program
	{
		static void Main()
		{			
			ServiceBase.Run(new ServiceBase[] { new OneCService() });
		}
	}
}
