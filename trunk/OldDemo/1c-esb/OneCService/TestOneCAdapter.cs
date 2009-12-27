/*
 * Created by SharpDevelop.
 * User: Mejov Andrei <mini_root@freemail.ru>
 * Date: 27.05.2009
 * Time: 18:53 
 */

using System;
using NUnit.Framework;
using System.ServiceModel;
using System.IO;
using System.Xml;

namespace OneCService
{
	[TestFixture]
	public class TestOneCAdapter
	{
		[Test]
		public void TestConnection()
		{			
			try
			{
				OneCAdapter a = new OneCAdapter();
				a.Open(@"C:\Work\Base1", null, null);
				try
				{
					Assert.IsTrue(a.ExecuteRequest("ВЫБРАТЬ 1 КАК К1, \"A\" КАК К2, Истина КАК К3, ДатаВремя(1, 1, 1) КАК К4"));
					Assert.IsTrue(a.Next());
					//Количество колонок
					Assert.AreEqual(a.GetColumnCount(), 4);
					//Названия
					Assert.AreEqual(a.GetColumnName(0), "К1");
					Assert.AreEqual(a.GetColumnName(1), "К2");				
					Assert.AreEqual(a.GetColumnName(2), "К3");				
					Assert.AreEqual(a.GetColumnName(3), "К4");
					//Значения
					Assert.AreEqual(a.GetValueByIndex(0), 1);
					Assert.AreEqual(a.GetValueByIndex(1), "A");
					Assert.AreEqual(a.GetValueByIndex(2), true);
					Assert.AreEqual(a.GetValueByIndex(3), DateTime.MinValue.AddYears(99));
					//Типы
					Assert.AreEqual(a.GetColumType(0), typeof(double));
					Assert.AreEqual(a.GetColumType(1), typeof(string));
					Assert.AreEqual(a.GetColumType(2), typeof(bool));
					Assert.AreEqual(a.GetColumType(3), typeof(DateTime));
					
					//Выполнение кода
					//Assert.IsNotNull(a.ExecuteScript("результат=ПолучитьТекущуюДату()"));
					//a.ExecuteScript("СоздатьЧленаСемьи(\"Шарик\");");					
					//a.ExecuteScript("УдалитьЧленаСемьи(\"Шарик\");");
					
					/*a.ExecuteRequest("ВЫБРАТЬ С.Ссылка ИЗ Справочник.ЧленыСемьи КАК С");
					a.Next();
					XmlElement e = (XmlElement)a.GetValueByIndex(0);										
					StringWriter sw = new StringWriter();
					using (XmlWriter writer = XmlTextWriter.Create(sw , new XmlWriterSettings()))
					{
						e.WriteTo(writer);
					}					
					System.Console.WriteLine(sw.ToString());
					
					a.ExecuteRequest("ВЫБРАТЬ С.Ссылка ИЗ Справочник.ЧленыСемьи КАК С");
					a.ToResultSet();*/
				}
				finally
				{
					a.Close();
				}
			}
			catch (Exception _e)
			{
				System.Console.WriteLine(_e.ToString());
				throw _e;
			}
		}
		
		[Test]
		public void TestServiceStartAndStop()
		{
			ServiceHost host = OneCWebService.CreateHost("localhost", 9001);
			host.Open();
			host.Close();
		}
	}
}
