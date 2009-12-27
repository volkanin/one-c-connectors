/*
 * Created by SharpDevelop.
 * User: Mejov Andrei <andrei.mejov@gmail.com>
 * Date: 05.07.2009
 * Time: 14:17 
 */
using System;
using System.Runtime.InteropServices;

namespace MSMQAdapter
{	
	[ComVisible(true)]
	public interface ILogger
	{
		void SetParameter(string _name, string _value);
		void ClearParameters();
		
		void Start(string _loggerName);
		void Stop();
		
		void Info(string _s);
		void Error(string _s);
		void Warn(string _s);
	}
}
