/*
 * Created by SharpDevelop.
 * User: Andrei Mejov <andrei.mejov@gmail.com>
 * Date: 15.10.2009
 * Time: 18:36
 *  
 */
using System;
using System.Transactions;
using OneCServiceStub;

namespace Client
{
	class Program
	{
		public static void Main(string[] args)
		{		
			using (TransactionScope t = new TransactionScope(TransactionScopeOption.Required))			
			{								
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
					if (!resultSet.Error.Equals(""))
					{
						Console.WriteLine("Error: "+resultSet.Error);
						throw new Exception(resultSet.Error);
					}
						
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
					if (!resultSet.Error.Equals(""))
					{
						Console.WriteLine("Error: "+resultSet.Error);
						throw new Exception(resultSet.Error);
					}
						
					//Завершение транзакции
					t.Complete();						
				}//try
				finally
				{
					client.Close();					
					Console.ReadKey();
				}				
			}//using TransactionScope			
		}
	}
}