/*
 * Created by SharpDevelop.
 * User: Andrei Mejov <andrei.mejov@gmail.com>
 * Date: 07.01.2010
 * Time: 19:00
 *  
 */
using System;

namespace OneCService2
{	
	public interface ILogger : IDisposable
	{
		void Info(string _message);
		void Severe(string _message);		
	}		
}
