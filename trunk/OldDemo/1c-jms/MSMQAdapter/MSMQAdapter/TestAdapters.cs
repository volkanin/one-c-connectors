/*
 * Created by SharpDevelop.
 * User: Mejov Andrei <andrei.mejov@gmail.com>
 * Date: 04.07.2009
 * Time: 13:57
 *  
 */

using System;
using NUnit.Framework;

namespace MSMQAdapter
{
	[TestFixture]
	public class TestAdapters
	{
		[Test]
		[Ignore]
		public void TestJabber()
		{
			JabberAdapter center = new JabberAdapter();
			center.SetParameter("ReceiveAddress", "center@192.168.1.11");			
			center.SetParameter("SendAddress", "exchange@192.168.1.11");			
			center.SetParameter("Server", "192.168.1.11:5222");
			center.SetParameter("Password", "123456");
			
			center.Start();
			center.Send("QQQ");
			center.Stop();
		}
		
		[Test]
		public void TestActiveMQ()
		{
			using (ActiveMQAdapter first = new ActiveMQAdapter())
			{
				using (ActiveMQAdapter second = new ActiveMQAdapter())
				{
					first.SetParameter("ActiveMQConnectionString", "tcp://localhost:61616");
					first.SetParameter("ReceiveDestinationName", "queue/first");
					first.SetParameter("SendDestinationName", "queue/second");
					first.SetParameter("UserName", "test");
					first.SetParameter("Password", "test");					
					second.SetParameter("ActiveMQConnectionString", "tcp://localhost:61616");
					second.SetParameter("ReceiveDestinationName", "queue/second");
					second.SetParameter("SendDestinationName", "queue/first");
					second.SetParameter("UserName", "test");
					second.SetParameter("Password", "test");
					
					first.Start();
					second.Start();
					
					first.Begin();
					first.Send("ЙЦУКЕН");
					first.Commit();
					System.Threading.Thread.Sleep(1000);
					second.Begin();
					Assert.IsTrue(second.HasMessage());
					Assert.AreEqual(second.Receive(), "ЙЦУКЕН");
					second.Commit();
					
				}
			}			
		}
	}		
}
