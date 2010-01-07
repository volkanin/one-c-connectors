/*
 * Created by SharpDevelop.
 * User: Andrei Mejov <andrei.mejov@gmail.com>
 * Date: 07.01.2010
 * Time: 18:10
 *  
 */
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;

namespace OneCService2
{
	public abstract class AbstractAdapter : IDisposable
	{	
		private Dictionary<string, string> parameters = new Dictionary<string, string>();
		private ILogger                        logger = null;
		private IntPtr                  connectionPtr = IntPtr.Zero;
		private object                     connection = null;
		
		/*Настройки*/
		public Dictionary<string, string> Parameters
		{
			get {return parameters;}
		}
		
		/*Жрунал по умолчанию*/
		public ILogger Logger
		{
			set {logger = value;}
			get {
					if (logger == null)
					{
						logger = SimpleLogger.DefaultLogger;
					}
					return logger;
				}
		}
		
		/*Внутренние свойства и методы доступные наследникам*/
		protected object Connection
		{
			set {
					connection = value;
					connectionPtr = Marshal.GetIUnknownForObject(connection);
				}
			get {return connection;}
		}				
		
		protected object CreateInstanceByProgId(string _progId)
		{
			Type type = Type.GetTypeFromProgID(_progId);
			return Activator.CreateInstance(type);
		}
		
		protected void ReleaseRCW(object _o)
		{
			try
			{
				Marshal.Release(Marshal.GetIDispatchForObject(_o));
				Marshal.ReleaseComObject(_o);
			}
			catch (Exception _e)
			{			
				logger.Severe("Exception in ReleaseRCW: " +_e.ToString());
			}
		}
		
		protected void RestoreConnectionRCW()
		{
			if (!connectionPtr.Equals(IntPtr.Zero))
			{
				connection = Marshal.GetObjectForIUnknown(connectionPtr);
			}
			else
			{
				throw new Exception("Illegal state for RestoreRCWr: connectionPtr is null");
			}
		}
		
		protected object Invoke(object _o, string _method, object[] _args)
		{
			 return _o.GetType().InvokeMember(
									_method, 
									BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, 
									null, 
									_o, 
									_args
											);
		}
		
		protected object GetProperty(object _o, string _name)
		{
			return _o.GetType().InvokeMember(
									_name, 
									BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty, 
									null, 
									_o, 
									null
											);
		}
		
		protected void SetProperty(object _o, string _name, object _value)
		{
			_o.GetType().InvokeMember(
									_name, 
									BindingFlags.Public | BindingFlags.Static | BindingFlags.SetProperty, 
									null, 
									_o, 
									new object[] {_value}
									);
		}
		
		protected string PrepareConnectionString(string _template)
		{
			String s = _template;
			foreach (string key in parameters.Keys)
			{
				s = s.Replace("${"+key+"}", parameters[key]);
			}
			return s;
		}
		
		protected void ValidateParameters(string[] _mustBeSet)
		{
			foreach (string s in _mustBeSet)
			{
				if (!Parameters.ContainsKey(s))
				{
					throw new Exception("Paramters not found: "+s);
				}
			}
		}				

		/*Управление жизненным циклом*/
		public abstract void Init();		
		public abstract void Done();
		
		/*Работа с типами*/
		protected abstract void PrepareSupportedType();		
		/*Полезные методы*/		
		
		
		public void Dispose()
		{
			Done();
		}
	}
}
