/*
 * Created by SharpDevelop.
 * User: Mejov Andrei <andrei.mejov@gmail.com>
 * Date: 18.08.2009
 * Time: 21:35
 * 
 */
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Apache.NMS;
using Apache.NMS.ActiveMQ;

namespace MOMAdapters
{
	[ComVisible(true)]	
	[ProgId("MOMAdapters.Utils")]	
	[Guid("463A7B50-502B-4896-8120-E3950C4990D5")]	
	public class Utils
	{
	
		public string GetAdapterTypes()
		{
			return "there is a COMArray soon";
		}
		
	}
}
