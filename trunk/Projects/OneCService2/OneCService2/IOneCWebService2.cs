/*
 * Created by SharpDevelop.
 * User: Andrei Mejov <andrei.mejov>
 * Date: 07.02.2010
 * Time: 13:41
 *  
 */
using System;
using System.ServiceModel;
using System.Xml;

namespace OneCService2
{
	[ServiceContract(Name="OneCService2", Namespace="http://OneCService2")]
	public interface IOneCWebService2
	{
		[OperationContract(Name="ExecuteRequest")]
		ResultSet ExecuteRequest(
								string _connectionName, 
								string _poolUserName, 
								string _poolPassword, 
								string _request
							    );
		
		[OperationContract(Name="ExecuteScript")]
		ResultSet ExecuteScript(
								string _connectionName, 
								string _poolUserName, 
								string _poolPassword, 
								string _script
							   );
		
		[OperationContract(Name="ExecuteMethod")]
		ResultSet ExecuteMethod(
								string _connectionName, 
								string _poolUserName, 
								string _poolPassword, 
								string _methodName,
								Parameters _parameters
							   );		
	}
}
