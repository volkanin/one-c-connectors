
package ocs2;

import java.math.BigDecimal;
import java.math.BigInteger;
import javax.xml.bind.JAXBElement;
import javax.xml.bind.annotation.XmlElementDecl;
import javax.xml.bind.annotation.XmlRegistry;
import javax.xml.datatype.Duration;
import javax.xml.datatype.XMLGregorianCalendar;
import javax.xml.namespace.QName;


/**
 * This object contains factory methods for each 
 * Java content interface and Java element interface 
 * generated in the ocs2 package. 
 * <p>An ObjectFactory allows you to programatically 
 * construct new instances of the Java representation 
 * for XML content. The Java representation of XML 
 * content can consist of schema derived interfaces 
 * and classes representing the binding of schema 
 * type definitions, element declarations and model 
 * groups.  Factory methods for each of these are 
 * provided in this class.
 * 
 */
@XmlRegistry
public class ObjectFactory {

    private final static QName _AnyURI_QNAME = new QName("http://schemas.microsoft.com/2003/10/Serialization/", "anyURI");
    private final static QName _DateTime_QNAME = new QName("http://schemas.microsoft.com/2003/10/Serialization/", "dateTime");
    private final static QName _Char_QNAME = new QName("http://schemas.microsoft.com/2003/10/Serialization/", "char");
    private final static QName _QName_QNAME = new QName("http://schemas.microsoft.com/2003/10/Serialization/", "QName");
    private final static QName _UnsignedShort_QNAME = new QName("http://schemas.microsoft.com/2003/10/Serialization/", "unsignedShort");
    private final static QName _Float_QNAME = new QName("http://schemas.microsoft.com/2003/10/Serialization/", "float");
    private final static QName _Long_QNAME = new QName("http://schemas.microsoft.com/2003/10/Serialization/", "long");
    private final static QName _Short_QNAME = new QName("http://schemas.microsoft.com/2003/10/Serialization/", "short");
    private final static QName _ArrayOfRow_QNAME = new QName("http://OneCService2/types", "ArrayOfRow");
    private final static QName _Base64Binary_QNAME = new QName("http://schemas.microsoft.com/2003/10/Serialization/", "base64Binary");
    private final static QName _Byte_QNAME = new QName("http://schemas.microsoft.com/2003/10/Serialization/", "byte");
    private final static QName _Parameters_QNAME = new QName("http://OneCService2/types", "Parameters");
    private final static QName _Boolean_QNAME = new QName("http://schemas.microsoft.com/2003/10/Serialization/", "boolean");
    private final static QName _ResultSet_QNAME = new QName("http://OneCService2/types", "ResultSet");
    private final static QName _UnsignedByte_QNAME = new QName("http://schemas.microsoft.com/2003/10/Serialization/", "unsignedByte");
    private final static QName _AnyType_QNAME = new QName("http://schemas.microsoft.com/2003/10/Serialization/", "anyType");
    private final static QName _UnsignedInt_QNAME = new QName("http://schemas.microsoft.com/2003/10/Serialization/", "unsignedInt");
    private final static QName _Int_QNAME = new QName("http://schemas.microsoft.com/2003/10/Serialization/", "int");
    private final static QName _Decimal_QNAME = new QName("http://schemas.microsoft.com/2003/10/Serialization/", "decimal");
    private final static QName _ArrayOfstring_QNAME = new QName("http://schemas.microsoft.com/2003/10/Serialization/Arrays", "ArrayOfstring");
    private final static QName _Double_QNAME = new QName("http://schemas.microsoft.com/2003/10/Serialization/", "double");
    private final static QName _Row_QNAME = new QName("http://OneCService2/types", "Row");
    private final static QName _Guid_QNAME = new QName("http://schemas.microsoft.com/2003/10/Serialization/", "guid");
    private final static QName _Duration_QNAME = new QName("http://schemas.microsoft.com/2003/10/Serialization/", "duration");
    private final static QName _String_QNAME = new QName("http://schemas.microsoft.com/2003/10/Serialization/", "string");
    private final static QName _UnsignedLong_QNAME = new QName("http://schemas.microsoft.com/2003/10/Serialization/", "unsignedLong");
    private final static QName _RowValues_QNAME = new QName("http://OneCService2/types", "Values");
    private final static QName _ExecuteRequestRequest_QNAME = new QName("http://OneCService2", "_request");
    private final static QName _ExecuteRequestConnectionName_QNAME = new QName("http://OneCService2", "_connectionName");
    private final static QName _ExecuteRequestPoolPassword_QNAME = new QName("http://OneCService2", "_poolPassword");
    private final static QName _ExecuteRequestPoolUserName_QNAME = new QName("http://OneCService2", "_poolUserName");
    private final static QName _ParametersParams_QNAME = new QName("http://OneCService2/types", "Params");
    private final static QName _ExecuteMethodMethodName_QNAME = new QName("http://OneCService2", "_methodName");
    private final static QName _ExecuteMethodParameters_QNAME = new QName("http://OneCService2", "_parameters");
    private final static QName _ExecuteMethodResponseExecuteMethodResult_QNAME = new QName("http://OneCService2", "ExecuteMethodResult");
    private final static QName _ResultSetColumnTypes_QNAME = new QName("http://OneCService2/types", "ColumnTypes");
    private final static QName _ResultSetColumnNames_QNAME = new QName("http://OneCService2/types", "ColumnNames");
    private final static QName _ResultSetError_QNAME = new QName("http://OneCService2/types", "Error");
    private final static QName _ResultSetRows_QNAME = new QName("http://OneCService2/types", "Rows");
    private final static QName _ExecuteScriptScript_QNAME = new QName("http://OneCService2", "_script");
    private final static QName _ExecuteRequestResponseExecuteRequestResult_QNAME = new QName("http://OneCService2", "ExecuteRequestResult");
    private final static QName _ExecuteScriptResponseExecuteScriptResult_QNAME = new QName("http://OneCService2", "ExecuteScriptResult");

    /**
     * Create a new ObjectFactory that can be used to create new instances of schema derived classes for package: ocs2
     * 
     */
    public ObjectFactory() {
    }

    /**
     * Create an instance of {@link ExecuteRequest }
     * 
     */
    public ExecuteRequest createExecuteRequest() {
        return new ExecuteRequest();
    }

    /**
     * Create an instance of {@link Parameters.Params }
     * 
     */
    public Parameters.Params createParametersParams() {
        return new Parameters.Params();
    }

    /**
     * Create an instance of {@link ArrayOfRow }
     * 
     */
    public ArrayOfRow createArrayOfRow() {
        return new ArrayOfRow();
    }

    /**
     * Create an instance of {@link Row.Values }
     * 
     */
    public Row.Values createRowValues() {
        return new Row.Values();
    }

    /**
     * Create an instance of {@link ExecuteRequestResponse }
     * 
     */
    public ExecuteRequestResponse createExecuteRequestResponse() {
        return new ExecuteRequestResponse();
    }

    /**
     * Create an instance of {@link ArrayOfstring }
     * 
     */
    public ArrayOfstring createArrayOfstring() {
        return new ArrayOfstring();
    }

    /**
     * Create an instance of {@link Row }
     * 
     */
    public Row createRow() {
        return new Row();
    }

    /**
     * Create an instance of {@link Parameters }
     * 
     */
    public Parameters createParameters() {
        return new Parameters();
    }

    /**
     * Create an instance of {@link ExecuteMethod }
     * 
     */
    public ExecuteMethod createExecuteMethod() {
        return new ExecuteMethod();
    }

    /**
     * Create an instance of {@link ExecuteMethodResponse }
     * 
     */
    public ExecuteMethodResponse createExecuteMethodResponse() {
        return new ExecuteMethodResponse();
    }

    /**
     * Create an instance of {@link ResultSet }
     * 
     */
    public ResultSet createResultSet() {
        return new ResultSet();
    }

    /**
     * Create an instance of {@link ExecuteScript }
     * 
     */
    public ExecuteScript createExecuteScript() {
        return new ExecuteScript();
    }

    /**
     * Create an instance of {@link ExecuteScriptResponse }
     * 
     */
    public ExecuteScriptResponse createExecuteScriptResponse() {
        return new ExecuteScriptResponse();
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://schemas.microsoft.com/2003/10/Serialization/", name = "anyURI")
    public JAXBElement<String> createAnyURI(String value) {
        return new JAXBElement<String>(_AnyURI_QNAME, String.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link XMLGregorianCalendar }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://schemas.microsoft.com/2003/10/Serialization/", name = "dateTime")
    public JAXBElement<XMLGregorianCalendar> createDateTime(XMLGregorianCalendar value) {
        return new JAXBElement<XMLGregorianCalendar>(_DateTime_QNAME, XMLGregorianCalendar.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Integer }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://schemas.microsoft.com/2003/10/Serialization/", name = "char")
    public JAXBElement<Integer> createChar(Integer value) {
        return new JAXBElement<Integer>(_Char_QNAME, Integer.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link QName }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://schemas.microsoft.com/2003/10/Serialization/", name = "QName")
    public JAXBElement<QName> createQName(QName value) {
        return new JAXBElement<QName>(_QName_QNAME, QName.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Integer }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://schemas.microsoft.com/2003/10/Serialization/", name = "unsignedShort")
    public JAXBElement<Integer> createUnsignedShort(Integer value) {
        return new JAXBElement<Integer>(_UnsignedShort_QNAME, Integer.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Float }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://schemas.microsoft.com/2003/10/Serialization/", name = "float")
    public JAXBElement<Float> createFloat(Float value) {
        return new JAXBElement<Float>(_Float_QNAME, Float.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Long }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://schemas.microsoft.com/2003/10/Serialization/", name = "long")
    public JAXBElement<Long> createLong(Long value) {
        return new JAXBElement<Long>(_Long_QNAME, Long.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Short }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://schemas.microsoft.com/2003/10/Serialization/", name = "short")
    public JAXBElement<Short> createShort(Short value) {
        return new JAXBElement<Short>(_Short_QNAME, Short.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ArrayOfRow }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://OneCService2/types", name = "ArrayOfRow")
    public JAXBElement<ArrayOfRow> createArrayOfRow(ArrayOfRow value) {
        return new JAXBElement<ArrayOfRow>(_ArrayOfRow_QNAME, ArrayOfRow.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link byte[]}{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://schemas.microsoft.com/2003/10/Serialization/", name = "base64Binary")
    public JAXBElement<byte[]> createBase64Binary(byte[] value) {
        return new JAXBElement<byte[]>(_Base64Binary_QNAME, byte[].class, null, ((byte[]) value));
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Byte }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://schemas.microsoft.com/2003/10/Serialization/", name = "byte")
    public JAXBElement<Byte> createByte(Byte value) {
        return new JAXBElement<Byte>(_Byte_QNAME, Byte.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Parameters }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://OneCService2/types", name = "Parameters")
    public JAXBElement<Parameters> createParameters(Parameters value) {
        return new JAXBElement<Parameters>(_Parameters_QNAME, Parameters.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Boolean }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://schemas.microsoft.com/2003/10/Serialization/", name = "boolean")
    public JAXBElement<Boolean> createBoolean(Boolean value) {
        return new JAXBElement<Boolean>(_Boolean_QNAME, Boolean.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ResultSet }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://OneCService2/types", name = "ResultSet")
    public JAXBElement<ResultSet> createResultSet(ResultSet value) {
        return new JAXBElement<ResultSet>(_ResultSet_QNAME, ResultSet.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Short }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://schemas.microsoft.com/2003/10/Serialization/", name = "unsignedByte")
    public JAXBElement<Short> createUnsignedByte(Short value) {
        return new JAXBElement<Short>(_UnsignedByte_QNAME, Short.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Object }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://schemas.microsoft.com/2003/10/Serialization/", name = "anyType")
    public JAXBElement<Object> createAnyType(Object value) {
        return new JAXBElement<Object>(_AnyType_QNAME, Object.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Long }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://schemas.microsoft.com/2003/10/Serialization/", name = "unsignedInt")
    public JAXBElement<Long> createUnsignedInt(Long value) {
        return new JAXBElement<Long>(_UnsignedInt_QNAME, Long.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Integer }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://schemas.microsoft.com/2003/10/Serialization/", name = "int")
    public JAXBElement<Integer> createInt(Integer value) {
        return new JAXBElement<Integer>(_Int_QNAME, Integer.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link BigDecimal }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://schemas.microsoft.com/2003/10/Serialization/", name = "decimal")
    public JAXBElement<BigDecimal> createDecimal(BigDecimal value) {
        return new JAXBElement<BigDecimal>(_Decimal_QNAME, BigDecimal.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ArrayOfstring }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://schemas.microsoft.com/2003/10/Serialization/Arrays", name = "ArrayOfstring")
    public JAXBElement<ArrayOfstring> createArrayOfstring(ArrayOfstring value) {
        return new JAXBElement<ArrayOfstring>(_ArrayOfstring_QNAME, ArrayOfstring.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Double }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://schemas.microsoft.com/2003/10/Serialization/", name = "double")
    public JAXBElement<Double> createDouble(Double value) {
        return new JAXBElement<Double>(_Double_QNAME, Double.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Row }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://OneCService2/types", name = "Row")
    public JAXBElement<Row> createRow(Row value) {
        return new JAXBElement<Row>(_Row_QNAME, Row.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://schemas.microsoft.com/2003/10/Serialization/", name = "guid")
    public JAXBElement<String> createGuid(String value) {
        return new JAXBElement<String>(_Guid_QNAME, String.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Duration }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://schemas.microsoft.com/2003/10/Serialization/", name = "duration")
    public JAXBElement<Duration> createDuration(Duration value) {
        return new JAXBElement<Duration>(_Duration_QNAME, Duration.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://schemas.microsoft.com/2003/10/Serialization/", name = "string")
    public JAXBElement<String> createString(String value) {
        return new JAXBElement<String>(_String_QNAME, String.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link BigInteger }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://schemas.microsoft.com/2003/10/Serialization/", name = "unsignedLong")
    public JAXBElement<BigInteger> createUnsignedLong(BigInteger value) {
        return new JAXBElement<BigInteger>(_UnsignedLong_QNAME, BigInteger.class, null, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Row.Values }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://OneCService2/types", name = "Values", scope = Row.class)
    public JAXBElement<Row.Values> createRowValues(Row.Values value) {
        return new JAXBElement<Row.Values>(_RowValues_QNAME, Row.Values.class, Row.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://OneCService2", name = "_request", scope = ExecuteRequest.class)
    public JAXBElement<String> createExecuteRequestRequest(String value) {
        return new JAXBElement<String>(_ExecuteRequestRequest_QNAME, String.class, ExecuteRequest.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://OneCService2", name = "_connectionName", scope = ExecuteRequest.class)
    public JAXBElement<String> createExecuteRequestConnectionName(String value) {
        return new JAXBElement<String>(_ExecuteRequestConnectionName_QNAME, String.class, ExecuteRequest.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://OneCService2", name = "_poolPassword", scope = ExecuteRequest.class)
    public JAXBElement<String> createExecuteRequestPoolPassword(String value) {
        return new JAXBElement<String>(_ExecuteRequestPoolPassword_QNAME, String.class, ExecuteRequest.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://OneCService2", name = "_poolUserName", scope = ExecuteRequest.class)
    public JAXBElement<String> createExecuteRequestPoolUserName(String value) {
        return new JAXBElement<String>(_ExecuteRequestPoolUserName_QNAME, String.class, ExecuteRequest.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Parameters.Params }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://OneCService2/types", name = "Params", scope = Parameters.class)
    public JAXBElement<Parameters.Params> createParametersParams(Parameters.Params value) {
        return new JAXBElement<Parameters.Params>(_ParametersParams_QNAME, Parameters.Params.class, Parameters.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://OneCService2", name = "_methodName", scope = ExecuteMethod.class)
    public JAXBElement<String> createExecuteMethodMethodName(String value) {
        return new JAXBElement<String>(_ExecuteMethodMethodName_QNAME, String.class, ExecuteMethod.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://OneCService2", name = "_connectionName", scope = ExecuteMethod.class)
    public JAXBElement<String> createExecuteMethodConnectionName(String value) {
        return new JAXBElement<String>(_ExecuteRequestConnectionName_QNAME, String.class, ExecuteMethod.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://OneCService2", name = "_poolPassword", scope = ExecuteMethod.class)
    public JAXBElement<String> createExecuteMethodPoolPassword(String value) {
        return new JAXBElement<String>(_ExecuteRequestPoolPassword_QNAME, String.class, ExecuteMethod.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link Parameters }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://OneCService2", name = "_parameters", scope = ExecuteMethod.class)
    public JAXBElement<Parameters> createExecuteMethodParameters(Parameters value) {
        return new JAXBElement<Parameters>(_ExecuteMethodParameters_QNAME, Parameters.class, ExecuteMethod.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://OneCService2", name = "_poolUserName", scope = ExecuteMethod.class)
    public JAXBElement<String> createExecuteMethodPoolUserName(String value) {
        return new JAXBElement<String>(_ExecuteRequestPoolUserName_QNAME, String.class, ExecuteMethod.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ResultSet }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://OneCService2", name = "ExecuteMethodResult", scope = ExecuteMethodResponse.class)
    public JAXBElement<ResultSet> createExecuteMethodResponseExecuteMethodResult(ResultSet value) {
        return new JAXBElement<ResultSet>(_ExecuteMethodResponseExecuteMethodResult_QNAME, ResultSet.class, ExecuteMethodResponse.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ArrayOfstring }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://OneCService2/types", name = "ColumnTypes", scope = ResultSet.class)
    public JAXBElement<ArrayOfstring> createResultSetColumnTypes(ArrayOfstring value) {
        return new JAXBElement<ArrayOfstring>(_ResultSetColumnTypes_QNAME, ArrayOfstring.class, ResultSet.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ArrayOfstring }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://OneCService2/types", name = "ColumnNames", scope = ResultSet.class)
    public JAXBElement<ArrayOfstring> createResultSetColumnNames(ArrayOfstring value) {
        return new JAXBElement<ArrayOfstring>(_ResultSetColumnNames_QNAME, ArrayOfstring.class, ResultSet.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://OneCService2/types", name = "Error", scope = ResultSet.class)
    public JAXBElement<String> createResultSetError(String value) {
        return new JAXBElement<String>(_ResultSetError_QNAME, String.class, ResultSet.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ArrayOfRow }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://OneCService2/types", name = "Rows", scope = ResultSet.class)
    public JAXBElement<ArrayOfRow> createResultSetRows(ArrayOfRow value) {
        return new JAXBElement<ArrayOfRow>(_ResultSetRows_QNAME, ArrayOfRow.class, ResultSet.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://OneCService2", name = "_connectionName", scope = ExecuteScript.class)
    public JAXBElement<String> createExecuteScriptConnectionName(String value) {
        return new JAXBElement<String>(_ExecuteRequestConnectionName_QNAME, String.class, ExecuteScript.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://OneCService2", name = "_poolPassword", scope = ExecuteScript.class)
    public JAXBElement<String> createExecuteScriptPoolPassword(String value) {
        return new JAXBElement<String>(_ExecuteRequestPoolPassword_QNAME, String.class, ExecuteScript.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://OneCService2", name = "_poolUserName", scope = ExecuteScript.class)
    public JAXBElement<String> createExecuteScriptPoolUserName(String value) {
        return new JAXBElement<String>(_ExecuteRequestPoolUserName_QNAME, String.class, ExecuteScript.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link String }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://OneCService2", name = "_script", scope = ExecuteScript.class)
    public JAXBElement<String> createExecuteScriptScript(String value) {
        return new JAXBElement<String>(_ExecuteScriptScript_QNAME, String.class, ExecuteScript.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ResultSet }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://OneCService2", name = "ExecuteRequestResult", scope = ExecuteRequestResponse.class)
    public JAXBElement<ResultSet> createExecuteRequestResponseExecuteRequestResult(ResultSet value) {
        return new JAXBElement<ResultSet>(_ExecuteRequestResponseExecuteRequestResult_QNAME, ResultSet.class, ExecuteRequestResponse.class, value);
    }

    /**
     * Create an instance of {@link JAXBElement }{@code <}{@link ResultSet }{@code >}}
     * 
     */
    @XmlElementDecl(namespace = "http://OneCService2", name = "ExecuteScriptResult", scope = ExecuteScriptResponse.class)
    public JAXBElement<ResultSet> createExecuteScriptResponseExecuteScriptResult(ResultSet value) {
        return new JAXBElement<ResultSet>(_ExecuteScriptResponseExecuteScriptResult_QNAME, ResultSet.class, ExecuteScriptResponse.class, value);
    }

}
