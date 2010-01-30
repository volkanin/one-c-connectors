/*
 * Created by SharpDevelop.
 * User: Andrei Mejov <andrei.mejov@gmail.com>
 * Date: 30.01.2010
 * Time: 17:52
 *  
 */
using System;

namespace OneCService2
{
	public class V82Adapter : V81Adapter
	{	
		protected override string GetProgId()
		{
			return "V82.ComConnector";
		}
	}
}
