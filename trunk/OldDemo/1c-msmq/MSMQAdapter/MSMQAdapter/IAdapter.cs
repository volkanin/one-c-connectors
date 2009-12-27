/*
 * Created by SharpDevelop.
 * User: Mejov Andrei <andrei.mejov@gmail.com>
 * Date: 01.01.2009
 * Time: 23:25
 *  
 */
using System;
using System.Runtime.InteropServices;

namespace MSMQAdapter
{
	[ComVisible(true)]
	public interface IAdapter
	{
		void SetParameter(string _name, string _value);
		void ClearParameters();
		
		void SendFile(string _text);
		void Send(string _text);
		bool HasMessage();
		string Receive();
		
		void Start();
		void Stop();
		
		void Begin();
		void Commit();
		void Rollback();
	}
}
