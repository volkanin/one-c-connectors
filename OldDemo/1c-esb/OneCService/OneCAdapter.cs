/*
 * Created by SharpDevelop.
 * User: Mejov Andrei <mini_root@freemail.ru>
 * Date: 27.05.2009
 * Time: 18:41
 *  
 */
using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml;

namespace OneCService
{	
	public class OneCAdapter : IDisposable
	{
		public const string OneCProgId = "V81.Application";
		public const string RequestForGetTypes = "ВЫБРАТЬ 1, \"А\", Истина, ДатаВремя(1, 1, 1)";
		public const string RequestForGetArrayType = "результат = Тип(\"Массив\");";
		public const string OneCServiceArrayElement = "onecservice-array";
		
		private object connection = null;
		private string connectionString = null;
		private object result = null;
		private object resultSet = null;
		private object request = null;
		
		private object stringType = null;
		private object doubleType = null;
		private object   boolType = null;
		private object   dateType = null;
		private object  arrayType = null;
		
		private object    xdtoSer = null;				
		
		private List<Type>  types = new List<Type>();
				
		public OneCAdapter()
		{			
		}
		
		private object Invoke(object _o, string _method, object[] _args)
		{
			return _o.GetType().InvokeMember(
											_method, 
											BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod, 
											null, 
											_o, 
											_args
											);
		}
		
		private object GetProperty(object _o, string _name)
		{
			return _o.GetType().InvokeMember(
											_name, 
											BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty, 
											null, 
											_o, 
											null
											);
		}		
		
		public void Open(string _file, string _userName, string _password)
		{
			Type t = Type.GetTypeFromProgID(OneCProgId);			
			connection = Activator.CreateInstance(t);
			connectionString = "File="+_file;
			if (_userName != null)
			{
				connectionString = connectionString + "; Usr="+_userName; 
			}
			if (_password != null)
			{
				connectionString = connectionString + "; Pwd="+_password; 
			}		
			//System.Console.WriteLine(connectionString);
			Invoke(
					connection, 
					"Connect", 
					new object[] {connectionString}
				   );
			
			GetSupportedTypes();
			result = null;
			resultSet = null;
			
			xdtoSer = Invoke(
							connection, 
							"NewObject", 
							new object[] {"СериализаторXDTO", GetProperty(connection, "ФабрикаXDTO")}
							);
		}
		
		private void GetSupportedTypes()
		{
			Release(resultSet);
			Release(result);
			Release(request);
						
			request = Invoke(connection, "NewObject", new object[] {"Запрос",  RequestForGetTypes});
			result = Invoke(request, "Выполнить", new object[] {});	
			resultSet = Invoke(result, "Выбрать", new object[] {});	
			
			doubleType = Invoke(
							Invoke(
									GetProperty(
											Invoke(GetProperty(result, "Колонки"), "Получить", new object[] {0}),
											"ТипЗначения"
												), 
									"Типы", new object[] {}
								 ),
							"Получить", new object[] {1}
							   );
		
			stringType = Invoke(
							Invoke(
									GetProperty(
											Invoke(GetProperty(result, "Колонки"), "Получить", new object[] {1}),
											"ТипЗначения"
												), 
									"Типы", new object[] {}
								 ),
							"Получить", new object[] {0}
							   );
			
			boolType = Invoke(
							Invoke(
									GetProperty(
											Invoke(GetProperty(result, "Колонки"), "Получить", new object[] {2}),
											"ТипЗначения"
												), 
									"Типы", new object[] {}
								 ),
							"Получить", new object[] {0}
							   );
			dateType = Invoke(
							Invoke(
									GetProperty(
											Invoke(GetProperty(result, "Колонки"), "Получить", new object[] {3}),
											"ТипЗначения"
												), 
									"Типы", new object[] {}
								 ),
							"Получить", new object[] {0}
							   );
			
			Invoke(resultSet, "Следующий", new object[] {});			
			Release(resultSet);
			
			arrayType = Invoke(connection, "ВыполнитьСтроку", new object[] {RequestForGetArrayType});
		}		
		
		//Получаем объект по ссылке
		public object GetObjectByRef(object _o)
		{
			object o1 = null;
			try
			{
				o1 = Invoke(_o, "ПолучитьОбъект", new object[] {});
			}
			catch (Exception _e) {}
			if (o1 != null) 
			{
				return o1;
			}
			else 
			{
				return _o;
			}			
		}
		
		//Выполняем запрос
		public bool ExecuteRequest(string _request)
		{	
			Release(resultSet);
			Release(result);
			Release(request);
			
			request = Invoke(connection, "NewObject", new object[] {"Запрос", _request});			
			try
			{
				result = Invoke(request, "Выполнить", new object[] {});
				if ((bool)Invoke(result, "Пустой", new object[] {}) == false)
				{
					resultSet = Invoke(result, "Выбрать", new object[] {});				
					return true;
				}
				else
				{
					resultSet = null;
					return false;
				}				
			}
			finally
			{
				Release(request);
			}
		}
		
		public ResultSet SingleResultToResultSet(object _o)
		{
			if (_o == null)
			{
				return new ResultSet();
			}
			else
			{
				ResultSet r = new ResultSet();
				r.ColumnNames.Add("Поле1");
				//Используется другой механизм определения типов, так как метаданные как в ТЗ недоступны
				if (_o is string)
				{
					r.ColumnTypes.Add("string");
				}
				else if ((_o is double) || (_o is int))
				{
					r.ColumnTypes.Add("double");
				}
				else if (_o is bool)
				{
					r.ColumnTypes.Add("boolean");
				}
				else if (_o is DateTime)
				{
					r.ColumnTypes.Add("datetime");
				}
				else
				{
					r.ColumnTypes.Add("object");
					_o = OneCObjectToXML(GetObjectByRef(_o));
				}
			
				Row row = new Row();
			
				XmlDocument doc = new XmlDocument();	
				XmlElement e = doc.CreateElement("value");					
				if (_o is XmlNode)
				{						
					XmlNode n = doc.ImportNode((XmlNode)_o, true);
					e.AppendChild(n);
					row.ValuesList.Add((XmlNode)e);
				}
				else
				{												
					e.AppendChild(doc.CreateTextNode(_o.ToString()));
					row.ValuesList.Add(e);
				}
				
				r.Rows.Add(row);
			
				return r;
			}
		}
		
		//Вызываем метод с параметрами в виде сериализованных объектов
		public ResultSet ExecuteMethodWithXDTO(string _methodName, XmlNode[] _parameters)
		{
			
			if (_parameters == null)
			{
				_parameters = new XmlNode[]{};
			}
			//Десериализуем параметры
			object[] oneCParameters = new object[_parameters.Length];
			try
			{
				for (int i=0; i<_parameters.Length; i++)
				{
					oneCParameters[i] = XmlToOneCObject(_parameters[i]);
				}
				//Вызовем метод
				object o = Invoke(connection, _methodName, oneCParameters);
				//Подготовим результат
				try
				{
					return SingleResultToResultSet(o);
				}
				finally
				{
					Release(o);
				}
			}
			finally
			{
				foreach (object o in oneCParameters)
				{
					Release(o);
				}
			}
			
		}				
		
		//Выполняем произвольный скрипт на внутреннем языке 1С
		public object ExecuteScript(string _script)
		{			
			return Invoke(connection, "ВыполнитьСтроку", new object[] {_script});
		}
		
		public ResultSet ExecuteScriptForResultSet(string _script)
		{
			object o = Invoke(connection, "ВыполнитьСтроку", new object[] {_script});
			try
			{
				if (o == null)
				{
					return new ResultSet();
				}
				else
				{
					return SingleResultToResultSet(o);
				}
			}
			finally
			{
				Release(o);
			}
						
		}
		
		public bool Next()
		{
			return (bool)Invoke(resultSet, "Следующий", new object[] {});
		}
		
		public int GetColumnCount()
		{
			return (int)Invoke(GetProperty(result, "Колонки"), "Количество", new object[] {});
		}
						
		public string GetColumnName(int _index)
		{
			return (string)GetProperty(
										Invoke(GetProperty(result, "Колонки"), "Получить", new object[] {_index}), 
										"Имя"
							  		  );
		}				
		
		public Type GetColumType(int _index)
		{			
			object currentType = GetProperty(
										Invoke(GetProperty(result, "Колонки"), "Получить", new object[] {_index}), 
										"ТипЗначения"
							  	  			);
			try
			{
				if ((bool)Invoke(currentType, "СодержитТип", new object[] {doubleType}))
				{				
					return typeof(double);
				}
				else if ((bool)Invoke(currentType, "СодержитТип", new object[] {stringType}))
				{				
					return typeof(string);
				}
				else if ((bool)Invoke(currentType, "СодержитТип", new object[] {boolType}))
				{
					return typeof(bool);
				}
				else if ((bool)Invoke(currentType, "СодержитТип", new object[] {dateType}))
				{
					return typeof(DateTime);
				}
				else
				{
					return null;
				}
			}
			finally
			{
				Release(currentType);
			}
		}
		
		public bool IsArray(object _o)
		{
			if ((bool)(Invoke(connection, "ПринадлежитТипу", new object[] {_o, arrayType})))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		public object XmlToOneCObject(XmlNode _node)
		{
			if (_node.LocalName.Equals(OneCServiceArrayElement))
			{
				object array = Invoke(connection, "NewObject", new object[] {"Массив"});
				foreach (XmlNode n in _node.ChildNodes)
				{
					if (n.NodeType.Equals(XmlNodeType.Element))
					{
						if (n.LocalName.Equals(OneCServiceArrayElement+"-item"))
						{
							XmlNode itemNode = n.ChildNodes.Item(0);
							
							object o = XmlToOneCObject(itemNode);
							Invoke(array,"Добавить", new object[] {o});
						}
					}
				}
				return array;
			}
			else
			{
				using (StringWriter sw = new StringWriter())
				{
					using (XmlWriter writer = XmlTextWriter.Create(sw , new XmlWriterSettings()))
					{				
						_node.WriteTo(writer);
					}	
			
					object readXml = Invoke(connection, "NewObject", new object[] {"ЧтениеXML"});
					try
					{
						Invoke(readXml, "УстановитьСтроку", new object[] {sw.ToString()});			
						object o = Invoke(xdtoSer, "ПрочитатьXML", new object[] {readXml});
						Invoke(readXml, "Закрыть", new object[] {});	
				
						return o;
					}
					finally
					{
						Release(readXml);
					}
				}
			}
		}
		
		public XmlElement OneCObjectToXML(object _o)
		{			
			if (IsArray(_o))
			{
				XmlDocument doc = new XmlDocument();
				XmlElement arrayElement = doc.CreateElement(OneCServiceArrayElement);
				doc.AppendChild(arrayElement);
				int count = (int)Invoke(_o, "Количество", new object[] {});
				for (int i=0; i<count; i++)
				{
					object item = Invoke(_o, "Получить", new object[] {i});
					if (item != null)
					{
						XmlElement itemElement = doc.CreateElement(OneCServiceArrayElement+"-item");
						arrayElement.AppendChild(itemElement);
						XmlElement itemValueElement = OneCObjectToXML(GetObjectByRef(item));
						itemElement.AppendChild(doc.ImportNode(itemValueElement, true));
					}
				}
				return doc.DocumentElement;
			}
			else
			{
				object writeXml = Invoke(connection, "NewObject", new object[] {"ЗаписьXML"});
				try
				{
					Invoke(writeXml, "УстановитьСтроку", new object[] {});
					Invoke(xdtoSer, "ЗаписатьXML", new object[] {writeXml, _o});				
					//Заполнем буфер текстом xml представления 1с'овского объекта
					string xmlString = (string)Invoke(writeXml, "Закрыть", new object[] {});				
					using (StringReader sr = new StringReader(xmlString))					
					{
						XmlDocument doc = new XmlDocument();					
						doc.Load(sr);							
						return doc.DocumentElement;
					}
				}
				finally
				{
					Release(writeXml);
				}
			}
		}
		
		public object GetValueByIndex(int _index)
		{
			if (GetColumType(_index) == null)
			{
				object o = Invoke(resultSet, "Получить", new object[] {_index});
				o = GetObjectByRef(o);
				
				return OneCObjectToXML(o);
			}
			else
			{
				return Invoke(resultSet, "Получить", new object[] {_index});
			}
		}
		
		public ResultSet ToResultSet()
		{
			ResultSet r = new ResultSet();
			
			//Заполним шапку
			for (int i=0; i<this.GetColumnCount(); i++)
			{
				r.ColumnNames.Add(this.GetColumnName(i));
				Type t = this.GetColumType(i);
				if (t == null)
				{
					r.ColumnTypes.Add("object");
				}
				else if (t.Equals(typeof(string)))
				{
					r.ColumnTypes.Add("string");
				}
				else if (t.Equals(typeof(double)))
				{
					r.ColumnTypes.Add("double");
				}
				else if (t.Equals(typeof(bool)))
				{
					r.ColumnTypes.Add("boolean");
				}
				else if (t.Equals(typeof(DateTime)))
				{
					r.ColumnTypes.Add("datetime");
				}
				else
				{
					r.ColumnTypes.Add("string");
				}
			}
			
			XmlDocument doc = new XmlDocument();
			//Заполним тело
			while (this.Next())
			{
				Row row = new Row();
				for (int i=0; i<this.GetColumnCount(); i++)
				{		
					XmlElement e = doc.CreateElement("value");					
					if (this.GetValueByIndex(i) is XmlNode)
					{						
						XmlNode n = doc.ImportNode((XmlNode)this.GetValueByIndex(i), true);
						e.AppendChild(n);
						row.ValuesList.Add((XmlNode)e);
					}
					else
					{												
						e.AppendChild(doc.CreateTextNode(this.GetValueByIndex(i).ToString()));
						row.ValuesList.Add(e);
					}
				}
				
				r.Rows.Add(row);
			}
			
			return r;
		}
		
		public void Release(object _o)
		{
			try
			{
				Marshal.Release(Marshal.GetIDispatchForObject(_o));
				Marshal.ReleaseComObject(_o);				
			}
			catch (Exception _e) {}
		}
		
		public void Close()
		{			
			Release(xdtoSer);
			Invoke(connection, "Exit", new object[] {});
			Release(connection);
		}
		
		public void Dispose()
		{
			try
			{
				Close();
			}
			catch (Exception _e) {}
		}
		
	}
}
