/*
 * Created by SharpDevelop.
 * User: Andrei Mejov <andrei.mejov@gmail.com>
 * Date: 07.01.2010
 * Time: 18:14
 *  
 */
using System;
using System.IO;
using System.Xml;

namespace OneCService2
{
	public class V81Adapter : AbstractAdapter
	{
		public static readonly string    RequestForGetTypes = "ВЫБРАТЬ \"А\", 1, Истина, ДатаВремя(1,1,1)";
		public static readonly string ScriptForGetArrayType = "результат=Тип(\"Массив\");";
		
		public static readonly string     ModeParam = "Mode";
		public static readonly string UserNameParam = "UserName";
		public static readonly string PasswordParam = "Password";
		public static readonly string     FileParam = "File";
		public static readonly string   ServerParam = "Server";
		public static readonly string     BaseParam = "Base";
		
		private string   fileConnectionStringTemplate = "File=${File}; Usr=${UserName}; Pwd=${Password}";		
		private string serverConnectionStringTemplate = "Srvr=${Server}; Ref=${Base}; Usr=${UserName}; Pwd=${Password}";
		
		private object comConnector = null;
		
		//Для быстрого определения типов
		private object  stringType = null;
		private object  doubleType = null;
		private object    boolType = null;
		private object    dateType = null;
		private object   arrayType = null;
		
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
						
		public override void Init()
		{
			comConnector = CreateInstanceByProgId("V81.ComConnector");
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
			object request = Invoke(Connection, "NewObject", new object[] {"Запрос", RequestForGetTypes});
			object result = Invoke(request, "Выполнить", new object[] {});
			object row = Invoke(result, "Выбрать", new object[] {});
			
			stringType = Invoke(
							Invoke(
									GetProperty(
											Invoke(GetProperty(result, "Колонки"), "Получить", new object[] {0}),
											"ТипЗначения"
												), 
									"Типы", new object[] {}
								 ),
							"Получить", new object[] {0}
							   );
		
			doubleType = Invoke(
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
			arrayType = ExecuteScript(ScriptForGetArrayType);
			
			Invoke(row, "Следующий", new object[] {});
			ReleaseRCW(row);
			ReleaseRCW(result);
			ReleaseRCW(request);
			
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
			else if ((bool)(Invoke(Connection, "OneCService_ПринадлежитТипу", new object[] {_o, arrayType})))
			{
				return SupportedType.ARRAY;	
			}
			else
			{
				return SupportedType.OBJECT;
			}
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
		
		public override XmlNode Serialize(object _o)
		{
			XmlNode resultNode = null;						
			
			Object o = GetObjectByRef(_o);
			
			SupportedType type = GetTypeForValue(o);
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
						XmlElement itemValueElement = (XmlElement)Serialize(item);
						itemElement.AppendChild(doc.ImportNode(itemValueElement, true));
					}
				}
				resultNode = doc.DocumentElement;
			}
			else if (type.Equals(SupportedType.OBJECT))
			{
				object writeXml = Invoke(Connection, "NewObject", new object[] {"ЗаписьXML"});
				try
				{
					Invoke(writeXml, "УстановитьСтроку", new object[] {});
					Invoke(xdtoSer, "ЗаписатьXML", new object[] {writeXml, o});
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
					ReleaseRCW(writeXml);
				}
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
			throw new NotImplementedException();
		}
		
		public override object ExecuteScript(string _script)
		{			
			return Invoke(Connection, "OneCService_ВыполнитьСтроку", new object[] {_script});						    
		}		
		
		public override ResultSet ExecuteRequest(string _request)
		{
			throw new NotImplementedException();
		}
		
		public override object ExecuteMethod(string _methodName, object[] _parameters)
		{
			throw new NotImplementedException();
		}
	}
}
