/*
 * Created by SharpDevelop.
 * User: Mejov Andrei <mini_root@freemail.ru>
 * Date: 29.05.2009
 * Time: 1:30
 *  
 */
using System;
using System.ServiceModel;
using System.Xml;

namespace OneCService		
{		

	[ServiceContract(Name="onecservice", Namespace="http://onecservice")]
	public interface IOneCWebService
	{		
		[OperationContract(Name="ExecuteRequest")]		
		ResultSet ExecuteRequest(string _file, string _usr, string _pwd, string _request);
		
		[OperationContract(Name="ExecuteScript")]
		ResultSet ExecuteScript(string _file, string _usr, string _pwd, string _script);
		
		[OperationContract(Name="ExecuteMethodWithXDTO")]
		ResultSet ExecuteMethodWithXDTO(string _file, string _usr, string _pwd, string _methodName, XmlNode[] _parameters);
	}
}
