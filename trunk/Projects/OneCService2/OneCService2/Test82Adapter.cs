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
		
		[Test]
		public void TestAdapterLifeCycle()		
		{
			V82Adapter adapter = new V82Adapter();
			adapter.Logger = new ConsoleLogger();
			adapter.Parameters.Add(V81Adapter.ModeParam, "File");
			adapter.Parameters.Add(V81Adapter.FileParam, @"C:\Work\1C\Test82");
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
		public void TestComplexDeSerialize()
		{
			V82Adapter adapter = new V82Adapter();
			adapter.Logger = new ConsoleLogger();
			adapter.Parameters.Add(V81Adapter.ModeParam, "File");
			adapter.Parameters.Add(V81Adapter.FileParam, @"C:\Work\1C\Test82");
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
		public void TestExecuteRequest()
		{
			V82Adapter adapter = new V82Adapter();
			adapter.Logger = new ConsoleLogger();
			adapter.Parameters.Add(V81Adapter.ModeParam, "File");
			adapter.Parameters.Add(V81Adapter.FileParam, @"C:\Work\1C\Test82");
			adapter.Parameters.Add(V81Adapter.UserNameParam, "");
			adapter.Parameters.Add(V81Adapter.PasswordParam, "");
			
			adapter.Init();
			try
			{
				ResultSet rs = adapter.ExecuteRequest("ВЫБРАТЬ Код, Наименование ИЗ Справочник.Номенклатура");		
				Assert.AreEqual(rs.ColumnNames[0], "Код");
				Assert.IsTrue(rs.Rows.Count>0);
				Assert.AreEqual(rs.Rows[0].Values[0].Value, "1");
				Assert.AreEqual(rs.Rows[0].Values[1].Value, "Товар 1");
			}
			finally
			{
				adapter.Done();
			}	
		}
		
		[Test]
		public void TestExecuteMethod()
		{
			V82Adapter adapter = new V82Adapter();
			adapter.Logger = new ConsoleLogger();
			adapter.Parameters.Add(V81Adapter.ModeParam, "File");
			adapter.Parameters.Add(V81Adapter.FileParam, @"C:\Work\1C\Test82");
			adapter.Parameters.Add(V81Adapter.UserNameParam, "");
			adapter.Parameters.Add(V81Adapter.PasswordParam, "");
			
			adapter.Init();
			try
			{
				XmlNode firstNode = adapter.Serialize("1");
				XmlNode secondNode = adapter.Serialize("ЙЦУКЕН");
				XmlNode[] na = new XmlNode[] {firstNode, secondNode};
				
				ResultSet rs = adapter.ExecuteMethodForResultSet("СобратьМассив", na);	
				Assert.AreEqual(rs.ColumnNames[0], "value");
				Assert.AreEqual(rs.ColumnTypes[0], SupportedType.ARRAY.ToString());
				Assert.IsTrue(rs.Rows.Count>0);
				object o = adapter.DeSerialize(rs.Rows[0].Values[0]);
				Assert.AreEqual(Invoke(o, "Получить", new object[] {1}), "ЙЦУКЕН");
			}
			finally
			{
				adapter.Done();
			}	
		}
		
		[Test]
		public void TestExternalXSD()
		{
			/*V82Adapter adapter = new V82Adapter();
			adapter.Logger = new ConsoleLogger();
			adapter.Parameters.Add(V81Adapter.ModeParam, "File");
			adapter.Parameters.Add(V81Adapter.FileParam, @"C:\Work\1C\Test82");
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
	}
}
