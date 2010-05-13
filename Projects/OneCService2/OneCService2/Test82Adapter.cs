/*
 * Created by SharpDevelop.
 * User: Andrei Mejov <andrei.mejov@gmail.com>
 * Date: 30.01.2010
 * Time: 17:53
 *  
 */

using System;
using System.Reflection;
using System.Xml;

using NUnit.Framework;

namespace OneCService2
{
	[TestFixture]
	public class Test82Adapter
	{
		private void WriteXml(XmlNode _node)
		{
			Console.WriteLine("XML:");
			XmlWriter writer = new XmlTextWriter(System.Console.Out);				
			XmlDocument doc = new XmlDocument();				
			XmlNode importedNode = doc.ImportNode(_node, true);
			doc.AppendChild(importedNode);				
			doc.WriteContentTo(writer);
			writer.Flush();
			writer.Close();		
		}
		
		private object GetProperty(object _o, string _name)
		{
			return _o.GetType().InvokeMember(
									_name, 
									BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty, 
									null, 
									_o, 
									null
											);
		}
		
		private object Invoke(object _o, string _method, object[] _args)
		{
			 return _o.GetType().InvokeMember(
									_method, 
									BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, 
									null, 
									_o, 
									_args
											);
		}
				
	}
}
