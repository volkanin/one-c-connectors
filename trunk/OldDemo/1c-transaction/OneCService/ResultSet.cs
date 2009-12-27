/*
 * Created by SharpDevelop.
 * User: Mejov Andrei <mini_root@freemail.ru>
 * Date: 27.05.2009
 * Time: 19:23
 *  
 */
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.ServiceModel;

namespace OneCService
{		
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
	
	[DataContract(Namespace="http://onecservice/types")]
	public class Row
	{
		private List<XmlNode> values = new List<XmlNode>();		
		
		public List<XmlNode> ValuesList
		{
			get {return values;}
		}
		
		[DataMember]
		public XmlNode[] Values
		{
			get {return values.ToArray();}
			set {}
		}				
	}
	
	[DataContract(Namespace="http://onecservice/types")]
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
		
		[DataMember]
		public List<string> ColumnTypes
		{
			get {return columnTypes;}
		}
		
		[DataMember]
		public List<Row> Rows
		{
			get {return rows;}
		}
		
		public ResultSet()
		{		
		}
	}
}
