/*
 * Created by SharpDevelop.
 * User: Andrei Mejov  <andrei.mejov@gmail.com>
 * Date: 07.01.2010
 * Time: 19:18
 *  
 */
using System;

namespace OneCService2
{
	public class ConsoleLogger : ILogger
	{
		public void Info(string _message)
		{			
			Console.WriteLine("INFO:"+DateTime.Now.ToString()+":"+_message);
		}
		
		public void Severe(string _message)
		{
			Console.WriteLine("SEVERE:"+DateTime.Now.ToString()+":"+_message);			
		}
		
		public void Dispose()
		{		
		}
	}
}
