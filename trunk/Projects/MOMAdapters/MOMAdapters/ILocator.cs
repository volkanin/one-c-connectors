/*
 * Created by SharpDevelop.
 * User: Mejov Andrei <andrei.mejov@gmail.com>
 * Date: 03.01.2010
 * Time: 16:01
 *  
 */
using System;
using System.Runtime.InteropServices;

namespace MOMAdapters
{

	[ComVisible(true)]
	public interface ILocator
	{
		string[] GetComponentsList();
		object GetComponent(String _name);
	}
}
