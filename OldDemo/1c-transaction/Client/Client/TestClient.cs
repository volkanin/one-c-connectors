/*
 * Created by SharpDevelop.
 * User: Andrei Mejov <andrei.mejov@gmail.com>
 * Date: 15.10.2009
 * Time: 19:11 
 */
using System;
using System.Transactions;
using System.Data.Common;
using FirebirdSql.Data.FirebirdClient;
using NUnit.Framework;
using OneCServiceStub;

namespace Client
{	
	[TestFixture]
	public class TestClient
	{
		[Test]		
		[Ignore]
		public void SimpleTest()
		{
			using (TransactionScope t = new TransactionScope(TransactionScopeOption.Required))
			{
				onecserviceClient client = new onecserviceClient();
			
				ResultSet resultSet = 
					client.ExecuteRequest(@"C:\Work\OneCService\Base\First", "", "", "ВЫБРАТЬ 100, Истина");
			
				Assert.AreEqual(resultSet.Error, "");
			
				Assert.AreEqual(resultSet.ColumnTypes.Length, 2);
			
				Assert.AreEqual(resultSet.Rows.Length, 1);
			
				Assert.AreEqual(resultSet.Rows[0].Values[0].InnerText, "100");
			
				Assert.AreEqual(resultSet.Rows[0].Values[1].InnerText, "True");
				
				t.Complete();
			
				client.Close();
								
			}
		}
		
		[Test]
		[Ignore]
		public void SimpleTransactionTest()
		{
			using (TransactionScope t = new TransactionScope(TransactionScopeOption.Required))
			{
				onecserviceClient first = new onecserviceClient();
				try
				{				
						ResultSet resultSet = first.ExecuteScript(
								@"C:\Work\OneCService\Base\First",
								"", "", 
								"сотр = Справочники.Сотрудники.НайтиПоКоду(10);\n" +
								"Если Не сотр.Пустая() Тогда сотр.ПолучитьОбъект().Удалить(); КонецЕсли;" +
								"сотр = Справочники.Сотрудники.СоздатьЭлемент();\n" +
								"сотр.Код = 10; " +
			                	"сотр.Наименование = \"Сиськин\"; " +
			                	"сотр.Записать();" 
			        	                                      );
						Console.WriteLine("Error: "+resultSet.Error);
						Assert.AreEqual(resultSet.Error, "");
						
						//Transaction.Current.Rollback();
						t.Complete();						
				}
				finally
				{
					first.Close();
				}
			}//using			
		}
		
		[Test]
		//[Ignore]
		public void GlobalTransactionTest()
		{
			using (TransactionScope t = new TransactionScope(TransactionScopeOption.Required))			
			{				
				using (FbConnection connection = new FbConnection(@"Database=C:\Work\Database\maptest.fdb;DataSource=localhost; Port=3050; Dialect=3; Charset=WIN1251;User=SYSDBA;Password=masterkey"))
				{												
					connection.Open();						
					//К сожалению, Firebird'овцы не поддерживают интеграцию с TransactionScope
					//поэтому придется использоватья явное управление транзакцией
					//connection.EnlistTransaction(Transaction.Current);
					onecserviceClient client = new onecserviceClient();	
					try
					{		
						//Первая база 1С через веб-сервис
						ResultSet resultSet = client.ExecuteScript(
								@"C:\Work\OneCService\Base\First",
								"", "", 
								"сотр = Справочники.Сотрудники.НайтиПоКоду(10);\n" +
								"Если Не сотр.Пустая() Тогда сотр.ПолучитьОбъект().Удалить(); КонецЕсли;" +
								"сотр = Справочники.Сотрудники.СоздатьЭлемент();\n" +
								"сотр.Код = 10; " +
			                	"сотр.Наименование = \"Сиськин\"; " +
			                	"сотр.Записать();" 
			        	                                      );
						Console.WriteLine("Error: "+resultSet.Error);
						Assert.AreEqual(resultSet.Error, "");
						
						//Вторая база 1С через веб-сервис
						resultSet = client.ExecuteScript(
								@"C:\Work\OneCService\Base\Second",
								"", "", 
								"сотр = Справочники.Сотрудники.НайтиПоКоду(10);\n" +
								"Если Не сотр.Пустая() Тогда сотр.ПолучитьОбъект().Удалить(); КонецЕсли;" +
								"сотр = Справочники.Сотрудники.СоздатьЭлемент();\n" +
								"сотр.Код = 10; " +
			                	"сотр.Наименование = \"Сиськин\"; " +
			                	"сотр.Записать();" 
			        	                                      );
						Console.WriteLine("Error: "+resultSet.Error);
						Assert.AreEqual(resultSet.Error, "");

						//СУБД через ADO.NET																								
						DbCommand cmd = (DbCommand)connection.CreateCommand();
						cmd.Transaction = connection.BeginTransaction();						
												
						Assert.IsNotNull(cmd.Transaction);
						
						cmd.CommandText = "DELETE FROM MARKERS";
						cmd.ExecuteNonQuery();						
						
						cmd.CommandText = "INSERT INTO MARKERS (CODE, TITLE, X, Y) VALUES (1, 'ЙЦУКЕН', 10, 10)";
						cmd.ExecuteNonQuery();												
						
						//К сожалению, Firebird'овцы не поддерживают интеграцию с TransactionScope
						//поэтому придется использоватья явное управление транзакцией
						cmd.Transaction.Commit();
						
						//Завершение транзакции
						t.Complete();						
					}
					finally
					{
						client.Close();
					}
				}//using connection
			}//using TransactionScope
		}
	}
}
