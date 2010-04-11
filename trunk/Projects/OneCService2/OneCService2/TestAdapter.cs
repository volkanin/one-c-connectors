/*
 * Created by SharpDevelop.
 * User: Andrei Mejov <andrei.mejov@gmail.com>
 * Date: 07.01.2010
 * Time: 18:15
 *  
 */

using System;
using System.Reflection;
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
		
		[Test]
		[Ignore]
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
		[Ignore]
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
		[Ignore]
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
				//Примитив
				object o = adapter.ExecuteScript("результат=2+3;");
				XmlNode node = adapter.Serialize(o);
				Assert.AreEqual(node.Value, "5");
				
				//Объект
				o = adapter.ExecuteScript("результат=Справочники.Номенклатура.НайтиПоКоду(1);");
				node = adapter.Serialize(o);
				
				WriteXml(node);
				
				//Массив
				o = adapter.ExecuteScript("э=Справочники.Номенклатура.НайтиПоКоду(1); м = Новый Массив(); м.Добавить(э);м.Добавить(э); результат=м;");
				node = adapter.Serialize(o);
				
				WriteXml(node);
				
				//Структура
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
		[Ignore]
		public void TestTypeCheck()
		{
			//Определение типов
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
		[Ignore]
		public void TestSimpleDeSerialize()
		{			
			//Примитивные типы
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
		[Ignore]
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
				//Массив
				object o = adapter.ExecuteScript(
								"м = Новый Массив(); м.Добавить(\"ЙЦУКЕН\"); результат=м;"
												);
				XmlNode node = adapter.Serialize(o);
				
				o = adapter.DeSerialize(node);
				Assert.NotNull(o);
				Assert.AreEqual(Invoke(o, "Получить", new object[] {0}), "ЙЦУКЕН");

				//Структура
				o = adapter.ExecuteScript(
								"с = Новый Структура(); с.Вставить(\"A\", \"1\"); с.Вставить(\"B\", 2); результат=с;"
										 );
								
				node = adapter.Serialize(o);				
				o = adapter.DeSerialize(node);
				Assert.NotNull(o);
				
				Assert.AreEqual(GetProperty(o, "A"), 1);
				Assert.AreEqual(GetProperty(o, "B"), 2);
				
				//Объект
				o = adapter.ExecuteScript("результат=Справочники.Номенклатура.НайтиПоКоду(1);");
				node = adapter.Serialize(o);
				o = adapter.DeSerialize(node);
				Assert.NotNull(o);
				Assert.AreEqual(GetProperty(o, "Код"), 1);
				
				//Структура с объектом
				o = adapter.ExecuteScript(
								"с = Новый Структура(); с.Вставить(\"A\", Справочники.Номенклатура.НайтиПоКоду(1)); с.Вставить(\"B\", 2); результат=с;"								
										 );
				node = adapter.Serialize(o);				
				o = adapter.DeSerialize(node);
				Assert.NotNull(o);
				
				Assert.AreEqual(GetProperty(GetProperty(o, "A"), "Код"), 1);
				Assert.AreEqual(GetProperty(o, "B"), 2);
			}
			finally
			{
				adapter.Done();
			}
		}
		
		
		[Test]
		[Ignore]
		public void TestExternalXSD()
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
				object o = adapter.ExecuteMethod("OCS2_ТоварВGood", new object[] {1});
				Assert.IsNotNull(o);
				Console.WriteLine(o);
				XmlNode node = adapter.Serialize(o);				
				
				WriteXml(node);
				
				o = adapter.DeSerialize(node);
				
				Assert.AreEqual(Invoke(
									Invoke(o, "Свойства", new object[] {}),
									"Количество",
									new object[]{}
								      ), 1);
				
				o = adapter.ExecuteMethod("OCS2_ИзвлечьGoodCode", new object[] {o});					
				Assert.IsNotNull(o);
				Assert.AreEqual(o, "1");
				
			}
			finally
			{
				adapter.Done();
			}
		}
		
		[Test]
		public void TestGlobalChanges()
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
				ResultSet r = adapter.ExecuteScript("результат=5+2;");				
				Assert.IsTrue(r.Rows[0].Values[0].Value.Equals("7"));
			}
			finally
			{
				adapter.Done();
			}
		}
	}
}
