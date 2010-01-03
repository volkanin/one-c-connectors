/*
 * Created by SharpDevelop.
 * User: Mejov Andrei <andrei.mejov@gmail.com>
 * Date: 03.01.2010
 * Time: 16:09
 *  
 */

using System;
using NUnit.Framework;

namespace MOMAdapters
{
	[TestFixture]
	public class Test
	{
		[Test]
		public void TestLocator()
		{			
			//Тестирование автоматического обноружения компонентов, помеченных AutoLocate'ом
			Locator locator = new Locator();
			
			Assert.IsTrue(locator.GetComponentsList().Length>0);
			
			locator.GetComponent("MSMQAdapter");
			locator.GetComponent("JabberAdapter");
			locator.GetComponent("ActiveMQAdapter");
		}
	}
}
