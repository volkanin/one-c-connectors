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
		public void TestAdapterLifecycle()
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
				ResultSet r = adapter.ExecuteScript("результат=2+3;");				
				Assert.IsTrue(r.Rows[0].Values[0].Value.Equals("5"));
			}
			finally
			{
				adapter.Done();
			}
		}
		
		[Test]
		//[Ignore]
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
				ResultSet r = adapter.ExecuteScript("результат=2+3;");				
				Assert.IsTrue(r.Rows[0].Values[0].Value.Equals("5"));
				
				//Объект
				r = adapter.ExecuteScript("результат=Справочники.Номенклатура.НайтиПоКоду(1);");				
				WriteXml(r.Rows[0].Values[0]);
				
				//Массив
				r = adapter.ExecuteScript("э=Справочники.Номенклатура.НайтиПоКоду(1); м = Новый Массив(); м.Добавить(э);м.Добавить(э); результат=м;");
				WriteXml(r.Rows[0].Values[0]);								
				
				//Структура
				r = adapter.ExecuteScript("с = Новый Структура(); с.Вставить(\"ЭтоКлюч\", \"ЭтоЗначение\"); с.Вставить(\"ЭтоКлюч1\", \"ЭтоЗначение1\"); результат=с;");
				WriteXml(r.Rows[0].Values[0]);								
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
			/*V81Adapter adapter = new V81Adapter();
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
			}*/
		}
		
		[Test]
		public void TestDesirialize()
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
				
				r = adapter.ExecuteScript("м = Новый Массив(); м.Добавить(\"ЙЦУКЕН\"); результат = м;");
				XmlNode arrayNode = r.Rows[0].Values[0];
				
				r = adapter.ExecuteScript("с = Новый Структура(); с.Вставить(\"Ключ1\", \"Значение1\"); с.Вставить(\"Ключ2\",Справочники.Номенклатура.НайтиПоКоду(1)); результат = с;");
				XmlNode structNode = r.Rows[0].Values[0];
				
				r = adapter.ExecuteRequest("ВЫБРАТЬ 1, \"Й\", Истина, ДатаВремя(1, 1, 1)");
				Assert.IsTrue(r.ColumnTypes[0].Equals("DOUBLE"));
				Assert.IsTrue(r.ColumnTypes[1].Equals("STRING"));
				Assert.IsTrue(r.ColumnTypes[2].Equals("BOOLEAN"));
				Assert.IsTrue(r.ColumnTypes[3].Equals("DATE"));
				
				r = adapter.ExecuteMethod("OneCService2_ТестовыйМетод", new XmlNode[] {r.Rows[0].Values[0], r.Rows[0].Values[1]});
				Assert.IsTrue(r.Rows[0].Values[0].Value.Equals("1|Й"));
				
				r = adapter.ExecuteMethod("OneCService2_ТестовыйМетод2", new XmlNode[] {r.Rows[0].Values[0], arrayNode});
				Console.WriteLine(r.Rows[0].Values[0].Value);
				Assert.IsTrue(r.Rows[0].Values[0].Value.Equals("1|Й|ЙЦУКЕН"));
				
				r = adapter.ExecuteMethod("OneCService2_ТестовыйМетод3", new XmlNode[] {r.Rows[0].Values[0], structNode});
				Console.WriteLine(r.Rows[0].Values[0].Value);
				Assert.IsTrue(r.Rows[0].Values[0].Value.Equals("1|Й|ЙЦУКЕН|Значение1|Товар 1"));
			}
			finally
			{
				adapter.Done();
			}
		}
	}
}
