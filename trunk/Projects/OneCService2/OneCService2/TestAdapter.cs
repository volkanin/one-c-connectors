/*
 * Created by SharpDevelop.
 * User: Andrei Mejov <andrei.mejov@gmail.com>
 * Date: 07.01.2010
 * Time: 18:15
 *  
 */

using System;
using System.Xml;
using NUnit.Framework;

namespace OneCService2
{
	[TestFixture]
	public class TestAdapter
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
		
		[Test]
		public void TestAdapterLifeCycle()
		{			
			V81Adapter adapter = new V81Adapter();
			adapter.Logger = new ConsoleLogger();
			adapter.Parameters.Add(V81Adapter.ModeParam, "File");
			adapter.Parameters.Add(V81Adapter.FileParam, @"C:\Work\1C\Test");
			adapter.Parameters.Add(V81Adapter.UserNameParam, "");
			adapter.Parameters.Add(V81Adapter.PasswordParam, "");
			
			adapter.Init();
			try
			{
				Assert.AreEqual(adapter.ExecuteScript("результат=2+3;"), 5);
			}
			finally
			{
				adapter.Done();
			}
		}
		
		[Test]
		public void TestTypes()
		{
			V81Adapter adapter = new V81Adapter();
			adapter.Logger = new ConsoleLogger();
			adapter.Parameters.Add(V81Adapter.ModeParam, "File");
			adapter.Parameters.Add(V81Adapter.FileParam, @"C:\Work\1C\Test");
			adapter.Parameters.Add(V81Adapter.UserNameParam, "");
			adapter.Parameters.Add(V81Adapter.PasswordParam, "");
			
			adapter.Init();			
			try
			{
				object o = adapter.ExecuteScript("результат=2+3;");
				SupportedType type = adapter.GetTypeForValue(o);
				Assert.AreEqual(type, SupportedType.INTEGER);
				
				o = adapter.ExecuteScript("результат=2+3.1;");
				type = adapter.GetTypeForValue(o);
				Assert.AreEqual(type, SupportedType.DOUBLE);
				
				o = adapter.ExecuteScript("результат=\"ЙЦУКЕН\";");
				type = adapter.GetTypeForValue(o);
				Assert.AreEqual(type, SupportedType.STRING);
				
				o = adapter.ExecuteScript("результат=Истина;");
				type = adapter.GetTypeForValue(o);
				Assert.AreEqual(type, SupportedType.BOOLEAN);
				
				o = adapter.ExecuteScript("результат=Новый Массив();");
				type = adapter.GetTypeForValue(o);
				Assert.AreEqual(type, SupportedType.ARRAY);
				
				o = adapter.ExecuteScript("результат=Справочники.Номенклатура.НайтиПоКоду(1);");				
				Assert.NotNull(o);
				o = adapter.GetObjectByRef(o);
				type = adapter.GetTypeForValue(o);
				Assert.AreEqual(type, SupportedType.OBJECT);
			}
			finally
			{
				adapter.Done();
			}
		}
		
		[Test]
		public void TestSerialize()
		{
			V81Adapter adapter = new V81Adapter();
			adapter.Logger = new ConsoleLogger();
			adapter.Parameters.Add(V81Adapter.ModeParam, "File");
			adapter.Parameters.Add(V81Adapter.FileParam, @"C:\Work\1C\Test");
			adapter.Parameters.Add(V81Adapter.UserNameParam, "");
			adapter.Parameters.Add(V81Adapter.PasswordParam, "");
			
			adapter.Init();
			try
			{
				object o = adapter.ExecuteScript("результат=2+3;");
				XmlNode node = adapter.Serialize(o);
				Assert.AreEqual(node.Value, "5");
				
				o = adapter.ExecuteScript("результат=Справочники.Номенклатура.НайтиПоКоду(1);");
				node = adapter.Serialize(o);
				
				WriteXml(node);
				
				o = adapter.ExecuteScript("э=Справочники.Номенклатура.НайтиПоКоду(1); м = Новый Массив(); м.Добавить(э);м.Добавить(э); результат=м;");
				node = adapter.Serialize(o);
				
				WriteXml(node);
				
				o = adapter.ExecuteScript("с = Новый Структура(); с.Вставить(\"ЭтоКлюч\", \"ЭтоЗначение\"); с.Вставить(\"ЭтоКлюч1\", \"ЭтоЗначение1\"); результат=с;");
				node = adapter.Serialize(o);
				
				WriteXml(node);
			}
			finally
			{
				adapter.Done();
			}
		}
		
		[Test]
		public void TestTypeCheck()
		{
			Assert.IsTrue(V81Adapter.isDouble("0.1"));
			Assert.IsTrue(V81Adapter.isInt("12"));						
			Assert.IsTrue(V81Adapter.isBool("True"));
			Assert.IsTrue(V81Adapter.isBool("False"));
			
			V81Adapter adapter = new V81Adapter();
			XmlNode node = adapter.Serialize(DateTime.Now);
			Assert.IsTrue(V81Adapter.isDate(node.Value));
			Console.WriteLine(node.Value);
		}
		
		[Test]
		public void TestSimpleDeSerialize()
		{			
			V81Adapter adapter = new V81Adapter();
			XmlNode dateString = adapter.Serialize(DateTime.Now);
			XmlNode intString = adapter.Serialize(12);
			XmlNode doubleString = adapter.Serialize(0.1);
			XmlNode boolString = adapter.Serialize(false);
			
			Assert.IsTrue(adapter.DeSerialize(dateString) is DateTime);
			Assert.IsTrue(adapter.DeSerialize(intString) is int);
			Console.WriteLine("QQQ: "+doubleString.Value);
			Assert.IsTrue(adapter.DeSerialize(doubleString) is double);
			Assert.IsTrue(adapter.DeSerialize(boolString) is bool);
		}
		
		[Test]
		public void TestComplexDeSerialize()
		{
			V81Adapter adapter = new V81Adapter();
			adapter.Logger = new ConsoleLogger();
			adapter.Parameters.Add(V81Adapter.ModeParam, "File");
			adapter.Parameters.Add(V81Adapter.FileParam, @"C:\Work\1C\Test");
			adapter.Parameters.Add(V81Adapter.UserNameParam, "");
			adapter.Parameters.Add(V81Adapter.PasswordParam, "");
			
			adapter.Init();
			try
			{
				object o = adapter.ExecuteScript(
								"м = Новый Массив(); м.Добавить(\"ЙЦУКЕН\"); результат=м;"
												);
				XmlNode node = adapter.Serialize(o);
				
				o = adapter.DeSerialize(node);
				Assert.NotNull(o);
			}
			finally
			{
				adapter.Done();
			}
		}
	}
}
