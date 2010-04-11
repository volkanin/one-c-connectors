/*
 * Created by SharpDevelop.
 * User: Andrei Mejov <andrei.mejov@gmail.com>
 * Date: 07.01.2010
 * Time: 18:14
 *  
 */
using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml;

namespace OneCService2
{
	public class V81Adapter : AbstractAdapter
	{
		public static readonly string     RequestForGetTypes = "ВЫБРАТЬ \"А\", 1, Истина, ДатаВремя(1,1,1)";
		public static readonly string  ScriptForGetArrayType = "результат=Тип(\"Массив\");";
		public static readonly string ScriptForGetStructType = "результат=Тип(\"Структура\")";		
		
		public static readonly string     ModeParam = "Mode";
		public static readonly string UserNameParam = "UserName";
		public static readonly string PasswordParam = "Password";
		public static readonly string     FileParam = "File";
		public static readonly string   ServerParam = "Server";
		public static readonly string     BaseParam = "Base";
		
		private string   fileConnectionStringTemplate = "File=${File}; Usr=${UserName}; Pwd=${Password}";		
		private string serverConnectionStringTemplate = "Srvr=${Server}; Ref=${Base}; Usr=${UserName}; Pwd=${Password}";
		
		private object comConnector = null;				
		
		//Сериализтор
		private object     xdtoSer = null;
		
		public string FileConnectionStringTemplate
		{
			set {fileConnectionStringTemplate = value;}
			get {return fileConnectionStringTemplate;}
		}
		
		public string ServerConnectionStringTemplate
		{
			set {serverConnectionStringTemplate = value;}
			get {return serverConnectionStringTemplate;}
		}
		
		protected virtual string GetProgId()
		{
			return "V81.ComConnector";
		}
						
		public override void Init()
		{
			comConnector = CreateInstanceByProgId(GetProgId());
			string connectionString = null;
			if (Parameters[ModeParam].Equals("Server"))
			{
				ValidateParameters(new string[] {"Server", "Base", "UserName", "Password"});
				connectionString = PrepareConnectionString(serverConnectionStringTemplate);
			}
			else
			{
				ValidateParameters(new string[] {"File", "UserName", "Password"});
				connectionString = PrepareConnectionString(fileConnectionStringTemplate);
			}
			
			Connection = Invoke(comConnector, "Connect", new object[] {connectionString});	
			
			Prepare();
		}
		
		public override void Done()
		{						
			RestoreConnectionRCW();			
			try
			{
				ReleaseRCW(Connection);
			}
			catch (Exception _e)
			{	
				Logger.Severe("Exception in adapter Done: " + _e.ToString());
			}
			
			RestoreConnectionRCW();			
			try
			{
				ReleaseRCW(comConnector);
			}
			catch (Exception _e)
			{	
				Logger.Severe("Exception in adapter Done: " + _e.ToString());
			}
		}				
		
		//Подготовка
		private void Prepare()
		{			
			xdtoSer = Invoke(
							Connection, 
							"NewObject", 
							new object[] {"СериализаторXDTO", GetProperty(Connection, "ФабрикаXDTO")}
							);
						
		}				
		
		//Получить тип по значению
		public override SupportedType GetTypeForValue(object _o)
		{
			if (_o is string)
			{
				return SupportedType.STRING;
			}
			else if (_o is int)
			{
				return SupportedType.INTEGER;
			}
			else if (_o is double)
			{
				return SupportedType.DOUBLE;
			}
			else if (_o is bool)
			{
				return SupportedType.BOOLEAN;
			}
			else if (_o is DateTime)
			{
				return SupportedType.DATE;
			}
			else
			{
				string typeName = (string)Invoke(Connection, "OneCService_ПринадлежитСложномуТипу", new object[] {_o});
				if (typeName.Equals("ARRAY"))
				{
					return SupportedType.ARRAY;
				}
				else if (typeName.Equals("STRUCT"))
				{
					return SupportedType.STRUCT;
				}
				else
				{
					return SupportedType.OBJECT;
				}
			}			
		}
		
		//Получаем объект по ссылке
		public object GetObjectByRef(object _o)
		{
			IntPtr ptr = Marshal.GetIUnknownForObject(_o);
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
				return Marshal.GetObjectForIUnknown(ptr);
			}			
		}
		
		public override XmlNode Serialize(object _o)
		{
			XmlNode resultNode = null;			
			Object o = GetObjectByRef(_o);
			SupportedType type = GetTypeForValue(o);
			
			//!!!!!!!!!!!!!!			
			/*Object o = _o;
			IntPtr ptr = Marshal.GetIUnknownForObject(o);
			
			SupportedType type = GetTypeForValue(o);			
									
			Console.WriteLine("OOO "+o.ToString()+" PTR="+ptr);									
			
			o = Marshal.GetObjectForIUnknown(ptr);
			
			o = GetObjectByRef(o);*/
			
			//!!!!!!!!!!!!!!
			if (type.Equals(SupportedType.ARRAY))
			{
				XmlDocument doc = new XmlDocument();
				XmlElement arrayElement = doc.CreateElement(OneCServiceArrayElement);
				doc.AppendChild(arrayElement);				
				int count = (int)Invoke(o, "Количество", new object[] {});
				for (int i=0; i<count; i++)
				{
					object item = Invoke(o, "Получить", new object[] {i});
					if (item != null)
					{
						XmlElement itemElement = doc.CreateElement(OneCServiceArrayElement+"-item");
						arrayElement.AppendChild(itemElement);
						XmlNode itemValueElement = (XmlNode)Serialize(item);
						itemElement.AppendChild(doc.ImportNode(itemValueElement, true));
					}
				}
				resultNode = doc.DocumentElement;
			}
			else if (type.Equals(SupportedType.STRUCT))
			{
				XmlDocument doc = new XmlDocument();
				XmlElement structElement = doc.CreateElement(OneCServiceStructElement);
				doc.AppendChild(structElement);
				object arrayOfStructElements = Invoke(Connection, "OneCService_ЭлементыСтруктурыВМассив", new object[] {o});
				int count = (int)Invoke(arrayOfStructElements, "Количество", new object[] {});
				for (int i=0; i<count; i++)
				{
					object keyAndValue = Invoke(arrayOfStructElements, "Получить", new object[] {i});
					if (keyAndValue != null)
					{
						//Поле структры
						XmlElement itemElement = doc.CreateElement(OneCServiceStructElement+"-item");
						structElement.AppendChild(itemElement);
						
						//Название (ключ)
						XmlElement keyElement = doc.CreateElement("struct-item-key");
						keyElement.AppendChild(
										doc.CreateTextNode(
												GetProperty(keyAndValue, "Ключ").ToString()
														  )
											  );
						
						itemElement.AppendChild(keyElement);												
						
						//Значение
						XmlNode valueContent = (XmlNode)Serialize(
												 			GetProperty(keyAndValue, "Значение")
									  		 		             );
						
						XmlElement valueElement = doc.CreateElement("struct-item-value");
						valueElement.AppendChild(doc.ImportNode(valueContent, true));
						
						itemElement.AppendChild(valueElement);												
					}
				}
				resultNode = doc.DocumentElement;
			}
			else if (type.Equals(SupportedType.OBJECT))
			{				
				object writeXml = Invoke(Connection, "OneCService_СоздатьЗаписьXML", new object[] {});
				try
				{
					Invoke(writeXml, "УстановитьСтроку", new object[] {});
					//Объект из импортированной схемы 
					if ((bool)Invoke(Connection, "OneCService_ЭтоXDTO", new object[] {o}))
					{
						//ФабрикаXDTO.Пакеты.Получить("http://goods").КорневыеСвойства.Получить(0).ЛокальноеИмя;
						//Получим список кореневых элементов, чтобы скорректировать их неправильную геренацию 1С'ом
						string ns = (string)GetProperty(Invoke(o, "Тип", new object[] {}), "URIПространстваИмен");
						object rootElements = 
							GetProperty(
								Invoke(
										GetProperty(
												GetProperty(Connection, "ФабрикаXDTO"), "Пакеты"
								  	   		   	   ),
										"Получить",
										new object[] {ns}
									 ),
								"КорневыеСвойства"
								  	   );
						string rootElementName = null;
						if (((int)Invoke(rootElements, "Количество", new object[] {}))>0)
						{
							rootElementName = (string)GetProperty(
												Invoke(rootElements, "Получить", new object[] {0}),
												"ЛокальноеИмя"
														 		);
						}
						if (rootElementName == null)
						{
							Invoke(
									GetProperty(xdtoSer, "Фабрика"), 
									"ЗаписатьXML", new object[] {writeXml, o}
							  	  );
						}
						else
						{
							Invoke(
									GetProperty(xdtoSer, "Фабрика"), 
									"ЗаписатьXML", new object[] {writeXml, o, rootElementName}
							  	  );
						}
					}
					//Внутренние объекты 1С
					else
					{
						Invoke(xdtoSer, "ЗаписатьXML", new object[] {writeXml, o});
					}
					//Заполнем буфер текстом xml представления 1с'овского объекта
					string xmlString = (string)Invoke(writeXml, "Закрыть", new object[] {});				
					using (StringReader sr = new StringReader(xmlString))					
					{
						XmlDocument doc = new XmlDocument();					
						doc.Load(sr);							
						resultNode = doc.DocumentElement;
					}
				}
				finally
				{					
					//ReleaseRCW(writeXml);
				}
			}
			else if (type.Equals(SupportedType.DATE))
			{
				XmlDocument doc = new XmlDocument();	
				resultNode = doc.CreateNode(XmlNodeType.Text, null, null);				
				resultNode.Value = ((DateTime)o).ToString("yyyy-MM-dd'T'HH:mm:ss.fffffffZ");
			}
			else
			{
				XmlDocument doc = new XmlDocument();	
				resultNode = doc.CreateNode(XmlNodeType.Text, null, null);				
				resultNode.Value = o.ToString();
			}
			
			return resultNode;
		}
		
		public override object DeSerialize(XmlNode _node)
		{
			//Десериализация простых типов
			if (_node.NodeType.Equals(XmlNodeType.Text))
			{
				string s = _node.Value.Trim();
				if (isBool(s))
				{
					return bool.Parse(s);					
				}
				else if (isDouble(s))
				{
					return double.Parse(s);
				}
				else if (isInt(s))
				{
					return int.Parse(s);
				}
				else if (isDate(s))
				{
					return DateTime.ParseExact(
											s, 
											"yyyy-MM-dd'T'HH:mm:ss.fffffffZ", 
											new CultureInfo("en-US", true)
											  );
				}
				else
				{
					return s;
				}
			}
			//Десереализация сложных типов
			else if (_node.NodeType.Equals(XmlNodeType.Element))
			{
				//Массив
				if (_node.LocalName.Equals(OneCServiceArrayElement))
				{
					object array = Invoke(Connection, "NewObject", new object[] {"Массив"});
					foreach (XmlNode n in _node.ChildNodes)
					{
						if (n.NodeType.Equals(XmlNodeType.Element))
						{
							if (n.LocalName.Equals(OneCServiceArrayElement+"-item"))
							{
								if (n.ChildNodes.Count > 0)
								{
									XmlNode itemNode = n.ChildNodes.Item(0);
									object item = DeSerialize(itemNode);
									Invoke(array, "Добавить", new object[] {item});
								}
							}//if item
						}//if element
					}//foreach
					return array;
				}
				//Структура
				else if (_node.LocalName.Equals(OneCServiceStructElement))
				{
					object strct = Invoke(Connection, "NewObject", new object[] {"Структура"});
					
					foreach (XmlNode n in _node.ChildNodes)
					{
						if (n.NodeType.Equals(XmlNodeType.Element))
						{
							if (n.LocalName.Equals(OneCServiceStructElement+"-item"))
							{
								string key = null;
								object val = null;
								foreach (XmlNode i in n.ChildNodes)
								{
									if (i.NodeType.Equals(XmlNodeType.Element))
									{
										if (i.LocalName.Equals("struct-item-key") && (i.ChildNodes.Count>0))
										{
											key = i.ChildNodes.Item(0).Value;											
										}
										else if (i.LocalName.Equals("struct-item-value") && (i.ChildNodes.Count>0))
										{
											val = DeSerialize(i.ChildNodes.Item(0));
										}
									}
								}//foreach key and value
								if (key == null)
								{
									throw new Exception("Exception in DeSerialize: key not found in structure");
								}
								
								Invoke(strct, "Вставить", new object[] {key, val});
							}
						}//if element
					}//foreach
					
					return strct;
				}
				//Объект
				else
				{
					using (StringWriter sw = new StringWriter())
					{
						using (XmlWriter xw = XmlTextWriter.Create(sw, new XmlWriterSettings()))
						{
							_node.WriteTo(xw);
						}
						
						object readXml = Invoke(Connection, "NewObject", new object[] {"ЧтениеXML"});
						try
						{
							Invoke(readXml, "УстановитьСтроку", new object[] {sw.ToString()});	
							object o = null;							
						    try
							{	
								o = Invoke(xdtoSer, "ПрочитатьXML", new object[] {readXml});
							}
							catch (Exception _e)
							{			
								Invoke(readXml, "Закрыть", new object[] {});	
								ReleaseRCW(readXml);
								readXml = Invoke(Connection, "NewObject", new object[] {"ЧтениеXML"});
								Invoke(readXml, "УстановитьСтроку", new object[] {sw.ToString()});	
								
								o = Invoke(
										   GetProperty(xdtoSer, "Фабрика"), 
										   "ПрочитатьXML", new object[] {readXml}
										  );								
							}
							Invoke(readXml, "Закрыть", new object[] {});					
							return o;
						}
						finally
						{
							ReleaseRCW(readXml);
						}						
					}//using sw
				}
				return null;
			}
			else
			{
				throw new Exception("Unsupported node type: " + _node.NodeType.ToString());
			}			
		}
		
		/*Полезные методы*/
		public override ResultSet ExecuteScript(string _script)
		{			
			ResultSet resultSet = new ResultSet();
			DataHelper dataHelper = new DataHelper();
			Invoke(Connection, "OneCService_ВыполнитьСкрипт", new object[] {_script, resultSet, dataHelper});
			return resultSet;
		}		
		
		public override ResultSet ExecuteRequest(string _request)
		{
			RestoreConnectionRCW();
			
			object request = Invoke(Connection, "NewObject", new object[] {"Запрос", _request});
			object result = Invoke(request, "Выполнить", new object[] {});
			try
			{
				ResultSet resultSet = new ResultSet();
				if ((bool)Invoke(result, "Пустой", new object[] {}))
				{
					return resultSet;
				}
				else
				{
					object row = Invoke(result, "Выбрать", new object[] {});
					try
					{
						int columnCount = (int)Invoke(
												GetProperty(result, "Колонки"), "Количество", new object[] {}
													 );
						/*Заполним названия колонок*/
						for (int i=0; i<columnCount; i++)
						{
							string columnName = 
									(string)GetProperty(
											Invoke(GetProperty(result, "Колонки"), "Получить", new object[] {i}), 
											"Имя"
							  		  				  );
							resultSet.ColumnNames.Add(columnName);							
						}
						/*Заполним данные и типы*/
						while ((bool)Invoke(row, "Следующий", new object[] {}))
						{
							Row newRow = new Row();
							for (int i=0; i<columnCount; i++)
							{
								object o = Invoke(row, "Получить", new object[] {i});
								try
								{
									/*Добавим информацию о типе если нужно и есть возможность*/
									if ((resultSet.ColumnTypes.Count<(i+1)) && (o != null))
									{
										resultSet.ColumnTypes.Add(GetTypeForValue(o).ToString());
									}
									if (o == null)
									{
										newRow.ValuesList.Add(Serialize(""));
									}
									else
									{
										newRow.ValuesList.Add(Serialize(o));
									}
								}
								finally
								{
									ReleaseRCW(o);
								}
							}
							
							resultSet.Rows.Add(newRow);
						}
						
						return resultSet;
					}
					finally
					{
						ReleaseRCW(row);
					}
				}
			}
			finally
			{				
				ReleaseRCW(result);
				ReleaseRCW(request);
			}						
		}
		
		public override object ExecuteMethod(string _methodName, object[] _parameters)
		{
			return Invoke(Connection, _methodName, _parameters);
		}
	}
}
