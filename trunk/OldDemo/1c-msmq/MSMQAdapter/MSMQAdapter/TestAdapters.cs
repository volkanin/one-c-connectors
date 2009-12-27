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
	}
}
