/*
 * Created by SharpDevelop.
 * User: Andrei Mejov <andrei.mejov@gmail.com>
 * Date: 11.02.2010
 * Time: 21:17
 *  
 */

using System;
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
				
				int loopCount = 1000;
			
				for (int i=0; i<loopCount; i++)
				{
					AbstractAdapter adapter = pool.GetConnection("PoolUserName", "PoolPassword");
					try
					{
						ResultSet r = adapter.ExecuteRequest("ВЫБРАТЬ Ссылка ИЗ Справочник.Номенклатура");
						Assert.AreEqual(r.Error, "");
						Assert.IsTrue(r.Rows.Count>0);
						Console.WriteLine("Loop: "+i);
					}
					finally
					{
						pool.ReleaseConnection(adapter);
					}
					
				}
			}
			finally
			{
				pool.Done();
			}
		}
	}
}
