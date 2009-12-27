/*
 * Created by SharpDevelop.
 * User: Mejov Andrei <andrei.mejov@gmail.com>
 * Date: 05.07.2009
 * Time: 14:18
 *  
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace MSMQAdapter
{	
	[ComVisible(true)]	
	[ComSourceInterfacesAttribute(typeof(ILogger))]	
	[ClassInterface(ClassInterfaceType.AutoDual)]	
	[ProgId("FileLogger")]	
	[Guid("0339E962-6744-4844-9408-1A795828CCBD")]		
	public class FileLogger : ILogger
	{
		public static readonly string FileName = "FileName";
		public static readonly string UseUTF8 = "UseUTF8";
		
		private string   loggerName = null;
		private FileStream   stream = null;
		private StreamWriter writer = null;
		
		private Dictionary<string, string> parameters = new Dictionary<string, string>();
		
		[ComVisible(false)]
		public Dictionary<string, string> Parameters
		{
			get {return parameters;}
		}
		
		public void SetParameter(string _name, string _value)
		{
			parameters.Add(_name, _value);
		}
		
		public void ClearParameters()
		{
			parameters.Clear();
		}
		
		public FileLogger()
		{			
		}
		
		public void Start(string _loggerName)
		{				
			ValidateParameters();			
			loggerName = _loggerName;
			stream = new FileStream(Parameters[FileName], FileMode.Append, FileAccess.Write, FileShare.Read);
			if (Parameters[UseUTF8].Equals("true") || Parameters[UseUTF8].Equals("True") || Parameters[UseUTF8].Equals("1"))
			{
				writer = new StreamWriter(stream, System.Text.Encoding.UTF8);
			}
			else
			{
				writer = new StreamWriter(stream, System.Text.Encoding.Default);
			}						
		}
		
		private void ValidateParameters()
		{
			if (!
			   	(
			    	Parameters.ContainsKey(FileName) && 
			    	Parameters.ContainsKey(UseUTF8)			    	
			    )
			   )
			{
				throw new Exception("FileName and UseUTF8 must be set");
			}
		}
				
		private void Rotate()
		{
			
		}
		
		private void WriteLine(string _level, string _text)
		{		
			//Преполагает использование в многопоточной среде
			lock (this)
			{
				writer.WriteLine("{0} {1} {2}: {3}", DateTime.Now.ToString(), _level, loggerName, _text);
				writer.Flush();
			}
		}				
		
		public void Info(string _text)
		{			
			WriteLine("INFO", _text);
		}
		
		public void Warn(string _text)
		{
			WriteLine("WARN", _text);
		}
		
		public void Error(string _text)
		{
			WriteLine("ERROR", _text);
		}
		
		public void Dispose()
		{			
			Stop();
		}
		
		public void Stop()
		{			
			try
			{				
				writer.Close();
			}
			catch (Exception _e) {}			
			
			try
			{				
				stream.Close();
			}
			catch (Exception _e) {}			
		}					
	}
}

