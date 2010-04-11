/*
 * Created by SharpDevelop.
 * User: Andrei Mejov <andrei.mejov@gmail.com>
 * Date: 11.04.2010
 * Time: 23:33
 *  
 */
using System;
using System.IO;
using System.Xml;
using System.Runtime.InteropServices;

namespace OneCService2
{
	[ComVisible(true)]
	public class DataHelper
	{		
		public XmlNode XmlNodeFromString(string _s)
		{						
			using (StringReader sr = new StringReader(_s))
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(sr);
				return doc.DocumentElement;
			}			
		}
		
		public XmlNode XmlNodeFromDate(DateTime _date)
		{
			XmlDocument doc = new XmlDocument();	
			XmlNode resultNode = doc.CreateNode(XmlNodeType.Text, null, null);				
			resultNode.Value = _date.ToString("yyyy-MM-dd'T'HH:mm:ss.fffffffZ");
			return resultNode;
		}
		
		public XmlNode XmlNodeFromSimple(object _simpleTypeValue)
		{
			XmlDocument doc = new XmlDocument();	
			XmlNode resultNode = doc.CreateNode(XmlNodeType.Text, null, null);				
			resultNode.Value = _simpleTypeValue.ToString();
			return resultNode;
		}
	}
}
