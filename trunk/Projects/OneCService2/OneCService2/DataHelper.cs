/*
 * Created by SharpDevelop.
 * User: Andrei Mejov <andrei.mejov@gmail.com>
 * Date: 11.04.2010
 * Time: 23:33
 *  
 */
using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml;

namespace OneCService2
{
	[ComVisible(true)]
	public class DataHelper
	{	
		public static readonly string  OneCServiceArrayElementName = "onecservice-array";
		public static readonly string OneCServiceStructElementName = "onecservice-struct";
		public static readonly string     StructItemKeyElementName = "struct-item-key";
		public static readonly string   StructItemValueElementName = "struct-item-value";
		public static readonly string        RemoveThisElementName = "remove-this";
		
		
		private static Regex isDoubleRegex = new Regex("^\\s*\\d+(\\.\\d+)?\\s*$");		
		private static Regex   isBoolRegex = new Regex("^(true)|(false)$");
		
		/*Оформлено в виде методов, чтобы можно было вызывать из 1С*/
		public string GetArrayElementName()       {return OneCServiceArrayElementName;}
		public string GetStructElementName()      {return OneCServiceStructElementName;}
		public string GetStructKeyElementName()   {return StructItemKeyElementName;}
		public string GetStructValueElementName() {return StructItemValueElementName;}
		public string GetRemoveThisElementName()  {return RemoveThisElementName;}
		
		/*Операции, которые выгоднее выполнять в .NET*/		
		
		public XmlNode XmlNodeFromString(string _s)
		{						
			using (StringReader sr = new StringReader(_s))
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(sr);
				return doc.DocumentElement;
			}			
		}
		
		public string StringFromXmlNode(XmlNode _node)
		{
			using (StringWriter sw = new StringWriter())
			{
				XmlWriter writer = new XmlTextWriter(sw);				
				XmlDocument doc = new XmlDocument();				
				XmlNode importedNode = doc.ImportNode(_node, true);
				doc.AppendChild(importedNode);				
				doc.WriteContentTo(writer);
				writer.Flush();
				writer.Close();	
				return sw.ToString();
			}
		}
		
		public string FormatDate(DateTime _date)
		{
			return _date.ToString("yyyy-MM-dd'T'HH:mm:ss.fffffffZ");
		}
		
		public XmlNode XmlNodeFromDate(DateTime _date)
		{
			XmlDocument doc = new XmlDocument();	
			XmlNode resultNode = doc.CreateNode(XmlNodeType.Text, null, null);				
			resultNode.Value = FormatDate(_date);
			return resultNode;
		}				
		
		public XmlNode XmlNodeFromSimple(object _simpleTypeValue)
		{
			XmlDocument doc = new XmlDocument();	
			XmlNode resultNode = doc.CreateNode(XmlNodeType.Text, null, null);				
			resultNode.Value = _simpleTypeValue.ToString();
			return resultNode;
		}
		
		/*Десериализация для простых типов*/		
		public static bool isDouble(string _s)
		{
			return (isDoubleRegex.IsMatch(_s.Trim().Replace(',','.'))) && (_s.Replace(',','.').Contains("."));
		}
		
		public static bool isInt(string _s)
		{
			return (isDoubleRegex.IsMatch(_s.Trim())) && (!_s.Contains("."));
		}
		
		public static bool isBool(string _s)
		{
			return isBoolRegex.IsMatch(_s.Trim().ToLower());
		}
		
		public static bool isDate(string _s)
		{
			try
			{
				DateTime.ParseExact(
								_s, 
								"yyyy-MM-dd'T'HH:mm:ss.fffffffZ", 
								new CultureInfo("en-US", true)
								  );
				return true;
			}
			catch (Exception _e)
			{
				return false;
			}
		}
		
		public bool IsSimple(XmlNode _node)
		{
			return _node.NodeType.Equals(XmlNodeType.Text);			
		}
		
		public object DeSerializeSimpleString(string _s)
		{
			if (isBool(_s))
			{
				return bool.Parse(_s);					
			}
			else if (isDouble(_s))
			{
				return double.Parse(_s);
			}
			else if (isInt(_s))
			{
				return int.Parse(_s);
			}
			else if (isDate(_s))
			{
				return DateTime.ParseExact(
											_s, 
											"yyyy-MM-dd'T'HH:mm:ss.fffffffZ", 
											new CultureInfo("en-US", true)
										  );
			}
			else
			{
				return _s;
			}
		}
		
		public object DeSerializeSimple(XmlNode _node)
		{
			string s = _node.Value.Trim();
			return DeSerializeSimpleString(s);
		}
		
		//Для отладки
		public void ForDebug(object _o)
		{
			Console.WriteLine(_o);
		}
	}
}
