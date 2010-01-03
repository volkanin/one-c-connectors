/*
 * Created by SharpDevelop.
 * User: Mejov Andrei <andrei.mejov@gmail.com>
 * Date: 18.08.2009
 * Time: 21:35
 * 
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Apache.NMS;
using Apache.NMS.ActiveMQ;

namespace MOMAdapters
{
	[ComVisible(true)]	
	[ComSourceInterfacesAttribute(typeof(ILocator))]	
	[ClassInterface(ClassInterfaceType.AutoDual)]
	[ProgId("MOMAdapters.Locator")]	
	[Guid("463A7B50-502B-4896-8120-E3950C4990D5")]	
	public class Locator : ILocator
	{
		private static Dictionary<string, Type> components = new Dictionary<string, Type>();
		
		static Locator()
		{			
			Type thisType = typeof(Locator);
			foreach (Type type in thisType.Assembly.GetTypes())
			{
				object[] attrs = type.GetCustomAttributes(typeof(AutoLocate), false);
				if (attrs.Length > 0)
				{
					components.Add(((AutoLocate)attrs[0]).Name, type);					
				}
			}
		}
		
		public string[] GetComponentsList()
		{			
			lock (components)
			{
				string[] res = new string[components.Keys.Count];
				int index = 0;
				foreach (String name in components.Keys)
				{
					res[index] = name;
					index++;
				}
				return res;
			}
		}
		
		public object GetComponent(String _name)
		{
			object res = null;
			lock (components)
			{
				if (components.ContainsKey(_name))
				{
					Type type = components[_name];					
					res = Activator.CreateInstance(type);					
				}
				else
				{
					throw new Exception("Component not found: "+_name);
				}
			}
			return res;
		}
		
	}
}
