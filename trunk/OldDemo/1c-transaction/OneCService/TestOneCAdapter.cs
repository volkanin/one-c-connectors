/*
 * Created by SharpDevelop.
 * User: Mejov Andrei <mini_root@freemail.ru>
 * Date: 27.05.2009
 * Time: 18:53 
 */

using System;
using System.IO;
using System.ServiceModel;
using System.Transactions;
using System.Xml;

using NUnit.Framework;

namespace OneCService
{
	[TestFixture]
	public class TestOneCAdapter
	{
		[Test]
		[Ignore]
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
		[Ignore]
		public void TestServiceStartAndStop()
		{
			ServiceHost host = OneCWebService.CreateHost("localhost", 9001, @"C:\OneCService.log");
			host.Open();
			host.Close();
		}
		
		[Test]
		[Ignore]
		public void TestTransactionWithOneCAdapter()
		{
			OneCAdapter a = new OneCAdapter();
			a.Open(@"C:\Work\OneCService\Base\First", null, null);	
			a.Begin();
			a.ExecuteScript(
							"сотр = Справочники.Сотрудники.СоздатьЭлемент();\n" +
							"сотр.Код = 10; " +
			                "сотр.Наименование = \"Сиськин\"; " +
			                "сотр.Записать(); "
			               );
			a.Commit();
			//a.Rollback();
			a.Close();
		}
		
		[Test]
		//[Ignore]
		public void TestTransactionWithManager()
		{			
			using (V8Pool pool = new V8Pool())
			{				
				OneCAdapter a = new OneCAdapter();			
				a.Open(@"C:\Work\OneCService\Base\First", null, null);	
				
				V8ResourceManager firstManager = new V8ResourceManager();				
				firstManager.Domain = AppDomain.CurrentDomain;
				firstManager.Adapter = a;	
				
				OneCAdapter b = new OneCAdapter();
				b.Open(@"C:\Work\OneCService\Base\Second", null, null);	
				
				V8ResourceManager secondManager = new V8ResourceManager();
				secondManager.Domain = AppDomain.CurrentDomain;
				secondManager.Adapter = b;
				
				Assert.AreNotEqual(firstManager.ResourceGuid, secondManager.ResourceGuid);
				Console.WriteLine("First GUID: " + firstManager.ResourceGuid);
				Console.WriteLine("Second GUID: " + secondManager.ResourceGuid);
											
			
				//Глобальная транзакция с участием двух разных баз 1С
				using (TransactionScope scope = new TransactionScope())
				{
					Assert.IsNotNull(Transaction.Current);					
					
					Transaction.Current.EnlistDurable(firstManager.ResourceGuid, firstManager, EnlistmentOptions.None);
					Transaction.Current.EnlistDurable(secondManager.ResourceGuid, secondManager, EnlistmentOptions.None);
					//Transaction.Current.EnlistVolatile(manager, EnlistmentOptions.None);										
					
					a.Begin();
					b.Begin();
					
					a.ExecuteScript(
							"сотр = Справочники.Сотрудники.НайтиПоКоду(10);\n" +
							"Если Не сотр.Пустая() Тогда сотр.ПолучитьОбъект().Удалить(); КонецЕсли"			                
			        	          );
					
					b.ExecuteScript(
							"сотр = Справочники.Сотрудники.НайтиПоКоду(10);\n" +
							"Если Не сотр.Пустая() Тогда сотр.ПолучитьОбъект().Удалить(); КонецЕсли"
			        	          );
					
					a.ExecuteScript(
							"сотр = Справочники.Сотрудники.СоздатьЭлемент();\n" +
							"сотр.Код = 10; " +
			                "сотр.Наименование = \"Сиськин\"; " +
			                "сотр.Записать(); "
			        	          );									
					
					b.ExecuteScript(
							"сотр = Справочники.Сотрудники.СоздатьЭлемент();\n" +
							"сотр.Код = 10; " +
			                "сотр.Наименование = \"Сиськин\"; " +
			                "сотр.Записать();"
			        	          );	
					
					scope.Complete();				
				}//using scope			
			}//using pool
		}
	}
}
