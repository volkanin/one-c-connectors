/*
 * Created by SharpDevelop.
 * User: Andrei Mejov <andrei.mejov@gmail.com>
 * Date: 07.01.2010
 * Time: 18:15
 *  
 */

using System;
using NUnit.Framework;

namespace OneCService2
{
	[TestFixture]
	public class TestAdapter
	{
		[Test]
		public void TestAdapterLifeCycle()
		{			
			V81Adapter adapter = new V81Adapter();
			adapter.Logger = new ConsoleLogger();
			adapter.Parameters.Add(V81Adapter.ModeParam, "File");
			adapter.Parameters.Add(V81Adapter.FileParam, @"C:\Work\1C\Test2");
			adapter.Parameters.Add(V81Adapter.UserNameParam, "");
			adapter.Parameters.Add(V81Adapter.PasswordParam, "");
			
			adapter.Init();
			adapter.Done();
		}
	}
}
