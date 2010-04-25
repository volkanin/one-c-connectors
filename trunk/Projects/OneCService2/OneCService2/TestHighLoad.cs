/*
 * Created by SharpDevelop.
 * User: Andrei Mejov <andrei.mejov@gmail.com>
 * Date: 11.02.2010
 * Time: 21:17
 *  
 */

using System;
using System.Threading;
using System.Xml;

using NUnit.Framework;

namespace OneCService2
{
	[TestFixture]
	public class TestHighLoad
	{
		[Test]
		public void TestLoop()
		{
			ConnectionPool pool = new ConnectionPool();
			pool.Logger = new ConsoleLogger();
			
			pool.Parameters.Add(ConnectionPool.AdapterTypeParam, "OneCService2.V81Adapter");
			pool.Parameters.Add(ConnectionPool.NameParam, "TestName");
			pool.Parameters.Add(ConnectionPool.PoolSizeParam, "1");
			pool.Parameters.Add(ConnectionPool.PoolUserNameParam, "PoolUserName");
			pool.Parameters.Add(ConnectionPool.PoolPasswordParam, "PoolPassword");
			
			pool.Parameters.Add(V81Adapter.ModeParam, "File");
			pool.Parameters.Add(V81Adapter.FileParam, @"C:\Work\1C\Test");
			pool.Parameters.Add(V81Adapter.UserNameParam, "");
			pool.Parameters.Add(V81Adapter.PasswordParam, "");
						
			try
			{
				pool.Init();
				
				int loopCount = 10000;
			
				AbstractAdapter adapter = pool.GetConnection("PoolUserName", "PoolPassword");					
				for (int i=0; i<loopCount; i++)
				{
					
					try
					{												
						ResultSet r = adapter.ExecuteScript(
								"с = Новый Структура(); с.Вставить(\"A\", Справочники.Номенклатура.НайтиПоКоду(1)); с.Вставить(\"B\", 2); результат=с;"								
										 				 );																		
						Console.WriteLine("Loop: "+i);
						Thread.Sleep(50);
					}
					finally
					{						
					}
					
				}
				pool.ReleaseConnection(adapter);
			}
			finally
			{
				pool.Done();
			}
		}
	}
}
