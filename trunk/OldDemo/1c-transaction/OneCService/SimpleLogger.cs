/*
 * Created by SharpDevelop.
 * User: Andrei Mejov <andrei.mejov@gmail.com>
 * Date: 15.10.2009
 * Time: 22:01
 *  
 */
using System;
using System.IO;
using System.Text;

namespace OneCService
{
	public class SimpleLogger : IDisposable
	{				
		private StreamWriter writer = null;
		
		private static SimpleLogger defaultLogger = null;
		
		public static void CreateDefaultLogger(string _fileName)
		{
			if (defaultLogger != null)
			{
				defaultLogger.Dispose();
			}
			defaultLogger = new SimpleLogger(_fileName);
		}
		
		public static SimpleLogger DefaultLogger
		{
			get {return defaultLogger;}
		}
		
		public SimpleLogger(string _fileName)
		{			
			writer = new StreamWriter(_fileName, true, Encoding.GetEncoding("windows-1251"));
		}
		
		public void Info(string _message)
		{			
			lock (writer)
			{
				writer.WriteLine("INFO:"+DateTime.Now.ToString()+":"+_message);
				writer.Flush();
			}
		}
		
		public void Severe(string _message)
		{
			lock (writer)
			{
				writer.WriteLine("SEVERE:"+DateTime.Now.ToString()+":"+_message);
				writer.Flush();
			}
		}
		
		public void Dispose()
		{
			try
			{				
				writer.Flush();
			}
			catch (Exception _e) {}
			
			try
			{				
				writer.Close();
			}
			catch (Exception _e) {}
		}
		
	}
}

	