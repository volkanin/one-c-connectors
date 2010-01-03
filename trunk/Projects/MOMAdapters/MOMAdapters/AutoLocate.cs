/*
 * Created by SharpDevelop.
 * User: Mejov Andrei <andrei.mejov@gmail.com>
 * Date: 03.01.2010
 * Time: 16:14
 * 
 */
using System;

namespace MOMAdapters
{
	public class AutoLocate : Attribute
	{
		private string name;

		public string Name
		{
			get {return name;}
		}
		
		public AutoLocate(string _name)
		{			
			name = _name;
		}		
	}
}
