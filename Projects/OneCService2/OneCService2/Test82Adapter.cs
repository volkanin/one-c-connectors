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
	}
}
