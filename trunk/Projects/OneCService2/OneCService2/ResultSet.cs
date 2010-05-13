/*
 * Created by SharpDevelop.
 * User: Andrei Mejov <andrei.mejov@gmail.com>
 * Date: 08.01.2010
 * Time: 18:22
 *  
 */
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace OneCService2
{	
	[DataContract(Namespace="http://OneCService2/types")]
	[Guid("DF847E1B-8263-461F-9626-9C1F9F591360")]
	[ComVisible(true)]
	public class Parameters
	{
		private List<XmlNode> paramsList = new List<XmlNode>();
		
		public void AddParameter(XmlNode _p)
		{
			paramsList.Add(_p);
		}
		
		[DataMember]
		public XmlNode[] Params
		{
			get {return paramsList.ToArray();}
			set {paramsList = new List<XmlNode>(value);}
		}
	}
	
	public class Value : IXmlSerializable
	{
		private XmlNode anyElement;

    	public XmlNode AnyElement
    	{
        	get { return anyElement; }
        	set { anyElement = value; }
    	}    
        
    	public XmlSchema GetSchema()
    	{
        	return null;
    	}

    	public void ReadXml(XmlReader reader)
    	{
        	XmlDocument document = new XmlDocument();
        	anyElement = document.ReadNode(reader);
    	}

    	public void WriteXml(XmlWriter writer)
    	{
        	anyElement.WriteTo(writer);
    	}
	}
	
	[DataContract(Namespace="http://OneCService2/types")]
	[Guid("2B181871-8F21-4B7E-8D93-8C4AFC37E5BF")]
	[ComVisible(true)]
	public class Row
	{
		private List<XmlNode> values = new List<XmlNode>();		
		
		public List<XmlNode> ValuesList
		{
			get {return values;}
		}
		
		public void AddValue(XmlNode _value, DataHelper _dataHelper)
		{
			if (_value.NodeType.Equals(XmlNodeType.Element))
			{
				if (_value.LocalName.Equals(_dataHelper.GetRemoveThisElementName()))
				{
					string currentValue = _value.ChildNodes[0].Value;
					ValuesList.Add(_dataHelper.XmlNodeFromSimple(currentValue));
				}
				else
				{
					ValuesList.Add(_value);
				}
			}
			else
			{
				ValuesList.Add(_value);
			}
			
		}
		
		[DataMember]
		public XmlNode[] Values
		{
			get {return values.ToArray();}
			set {}
		}				
	}
	
	[DataContract(Namespace="http://OneCService2/types")]
	[Guid("9008747A-1971-45D9-B752-E4B6856B521D")]
	[ComVisible(true)]
	public class ResultSet
	{
		private List<string> columnNames = new List<string>();
		private List<string> columnTypes = new List<string>();
		private List<Row> rows = new List<Row>();		
		
		private string   error = "";
		
		[DataMember]
		public string Error
		{
			get {return error;}
			set {error = value;}
		}
		
		[DataMember]
		public List<string> ColumnNames
		{
			get {return columnNames;}
		}
		
		public void AddColumnName(string _name)
		{
			ColumnNames.Add(_name);
		}
		
		[DataMember]
		public List<string> ColumnTypes
		{
			get {return columnTypes;}
		}
		
		public void AddColumnType(string _type)
		{
			ColumnTypes.Add(_type);
		}
		
		[DataMember]
		public List<Row> Rows
		{
			get {return rows;}
		}
		
		public ResultSet()
		{		
		}
		
		public Row CreateRow()
		{
			Row row = new Row();
			Rows.Add(row);
			return row;
		}
	}
}
