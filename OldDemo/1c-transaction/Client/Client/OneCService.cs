﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:2.0.50727.3603
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: System.Runtime.Serialization.ContractNamespaceAttribute("http://onecservice/types", ClrNamespace="OneCServiceStub")]

namespace OneCServiceStub
{
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="ResultSet", Namespace="http://onecservice/types")]
    public partial class ResultSet : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private string[] ColumnNamesField;
        
        private string[] ColumnTypesField;
        
        private string ErrorField;
        
        private OneCServiceStub.Row[] RowsField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] ColumnNames
        {
            get
            {
                return this.ColumnNamesField;
            }
            set
            {
                this.ColumnNamesField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string[] ColumnTypes
        {
            get
            {
                return this.ColumnTypesField;
            }
            set
            {
                this.ColumnTypesField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Error
        {
            get
            {
                return this.ErrorField;
            }
            set
            {
                this.ErrorField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public OneCServiceStub.Row[] Rows
        {
            get
            {
                return this.RowsField;
            }
            set
            {
                this.RowsField = value;
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "3.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Row", Namespace="http://onecservice/types")]
    public partial class Row : object, System.Runtime.Serialization.IExtensibleDataObject
    {
        
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        private System.Xml.XmlNode[] ValuesField;
        
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData
        {
            get
            {
                return this.extensionDataField;
            }
            set
            {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Xml.XmlNode[] Values
        {
            get
            {
                return this.ValuesField;
            }
            set
            {
                this.ValuesField = value;
            }
        }
    }
}




[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
[System.ServiceModel.ServiceContractAttribute(Namespace="http://onecservice", ConfigurationName="onecservice")]
public interface onecservice
{
    
    [System.ServiceModel.OperationContractAttribute(Action="http://onecservice/onecservice/ExecuteRequest", ReplyAction="http://onecservice/onecservice/ExecuteRequestResponse")]
    [System.ServiceModel.TransactionFlowAttribute(System.ServiceModel.TransactionFlowOption.Mandatory)]
    OneCServiceStub.ResultSet ExecuteRequest(string _file, string _usr, string _pwd, string _request);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://onecservice/onecservice/ExecuteScript", ReplyAction="http://onecservice/onecservice/ExecuteScriptResponse")]
    [System.ServiceModel.TransactionFlowAttribute(System.ServiceModel.TransactionFlowOption.Mandatory)]
    OneCServiceStub.ResultSet ExecuteScript(string _file, string _usr, string _pwd, string _script);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://onecservice/onecservice/ExecuteMethodWithXDTO", ReplyAction="http://onecservice/onecservice/ExecuteMethodWithXDTOResponse")]
    [System.ServiceModel.TransactionFlowAttribute(System.ServiceModel.TransactionFlowOption.Mandatory)]
    OneCServiceStub.ResultSet ExecuteMethodWithXDTO(string _file, string _usr, string _pwd, string _methodName, System.Xml.XmlNode[] _parameters);
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
public interface onecserviceChannel : onecservice, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
public partial class onecserviceClient : System.ServiceModel.ClientBase<onecservice>, onecservice
{
    
    public onecserviceClient()
    {
    }
    
    public onecserviceClient(string endpointConfigurationName) : 
            base(endpointConfigurationName)
    {
    }
    
    public onecserviceClient(string endpointConfigurationName, string remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public onecserviceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public onecserviceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(binding, remoteAddress)
    {
    }
    
    public OneCServiceStub.ResultSet ExecuteRequest(string _file, string _usr, string _pwd, string _request)
    {
        return base.Channel.ExecuteRequest(_file, _usr, _pwd, _request);
    }
    
    public OneCServiceStub.ResultSet ExecuteScript(string _file, string _usr, string _pwd, string _script)
    {
        return base.Channel.ExecuteScript(_file, _usr, _pwd, _script);
    }
    
    public OneCServiceStub.ResultSet ExecuteMethodWithXDTO(string _file, string _usr, string _pwd, string _methodName, System.Xml.XmlNode[] _parameters)
    {
        return base.Channel.ExecuteMethodWithXDTO(_file, _usr, _pwd, _methodName, _parameters);
    }
}

