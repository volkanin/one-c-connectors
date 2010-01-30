/*
 * Created by SharpDevelop.
 * User: Andrei Mejov <andrei.mejov@gmail.com>
 * Date: 29.01.2010
 * Time: 22:10
 *  
 */

using System;
using NUnit.Framework;

namespace OneCService2
{
	[TestFixture]
	public class TestPool
	{		
		
		[Test]
		public void TestLifecycle()
		{
			ConnectionPool pool = new ConnectionPool();
			
			pool.Parameters.Add(ConnectionPool.AdapterTypeParam, "OneCService2.V81Adapter");
			pool.Parameters.Add(ConnectionPool.NameParam, "TestName");
			pool.Parameters.Add(ConnectionPool.PoolSizeParam, "3");
			pool.Parameters.Add(ConnectionPool.PoolUserNameParam, "PoolUserName");
			pool.Parameters.Add(ConnectionPool.PoolPasswordParam, "PoolPassword");
			
			pool.Parameters.Add(V81Adapter.ModeParam, "File");
			pool.Parameters.Add(V81Adapter.FileParam, @"C:\Work\1C\Test");
		    pool.Parameters.Add(V81Adapter.UserNameParam, "");
			pool.Parameters.Add(V81Adapter.PasswordParam, "");

			try
			{
				pool.Init();			
			}
			finally
			{
				pool.Done();
			}
			
			Assert.IsTrue(true);
		}
		
		[Test]
		public void TestGetAndReleaseConnections()
		{												
			ConnectionPool pool = new ConnectionPool();
			pool.Logger = new ConsoleLogger();
			
			pool.Parameters.Add(ConnectionPool.AdapterTypeParam, "OneCService2.V81Adapter");
			pool.Parameters.Add(ConnectionPool.NameParam, "TestName");
			pool.Parameters.Add(ConnectionPool.PoolSizeParam, "3");
			pool.Parameters.Add(ConnectionPool.PoolUserNameParam, "PoolUserName");
			pool.Parameters.Add(ConnectionPool.PoolPasswordParam, "PoolPassword");
			
			pool.Parameters.Add(V81Adapter.ModeParam, "File");
			pool.Parameters.Add(V81Adapter.FileParam, @"C:\Work\1C\Test");
			pool.Parameters.Add(V81Adapter.UserNameParam, "");
			pool.Parameters.Add(V81Adapter.PasswordParam, "");

			try
			{
				pool.Init();			
				
				/*Неправильная аутентификация*/
				try
				{
					AbstractAdapter adapter = pool.GetConnection("AAA", "BBB");
					Assert.IsTrue(false);
				}
				catch (Exception _e)
				{
					Console.WriteLine(_e.ToString());
				}
				
				AbstractAdapter adapter1 = pool.GetConnection("PoolUserName", "PoolPassword");
				AbstractAdapter adapter2 = pool.GetConnection("PoolUserName", "PoolPassword");
				AbstractAdapter adapter3 = pool.GetConnection("PoolUserName", "PoolPassword");
				
				try
				{
					/*Должно отвалится по таймауту*/
					pool.GetConnection("PoolUserName", "PoolPassword");
					Assert.IsTrue(false);
				}
				catch (Exception _e)
				{
					Console.WriteLine(_e.ToString());
				}
				
				pool.ReleaseConnection(adapter1);
				pool.ReleaseConnection(adapter2);
				pool.ReleaseConnection(adapter3);		
				
				/*Берем и не отдаем назад соединение - должно все равно закрыться*/
				adapter1 = pool.GetConnection("PoolUserName", "PoolPassword");
				
				Assert.IsTrue(true);
			}
			finally
			{
				pool.Done();
			}
			
			Assert.IsTrue(true);
		}
	}		
	
}
