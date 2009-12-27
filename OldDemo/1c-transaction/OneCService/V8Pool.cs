/*
 * Created by SharpDevelop.
 * User: Andrei Mejov <andrei.mejov@gmail.com>
 * Date: 13.10.2009
 * Time: 17:49
 *  
 */
using System;
using System.Collections.Generic;

namespace OneCService
{
	public class V8Pool : IDisposable
	{		
		private List<V8ResourceManager> managers = new List<V8ResourceManager>();
		
		public List<V8ResourceManager> Managers
		{			
			get {return managers;}
		}
		
		public void Dispose()
		{
			foreach (V8ResourceManager manager in managers)
			{
				manager.Dispose();
			}
		}
	}
}
