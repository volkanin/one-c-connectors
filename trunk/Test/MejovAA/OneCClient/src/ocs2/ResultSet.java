
package ocs2;

import javax.xml.bind.JAXBElement;
import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElementRef;
import javax.xml.bind.annotation.XmlType;


/**
 * <p>Java class for ResultSet complex type.
 * 
 * <p>The following schema fragment specifies the expected content contained within this class.
 * 
 * <pre>
 * &lt;complexType name="ResultSet">
 *   &lt;complexContent>
 *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *       &lt;sequence>
 *         &lt;element name="ColumnNames" type="{http://schemas.microsoft.com/2003/10/Serialization/Arrays}ArrayOfstring" minOccurs="0"/>
 *         &lt;element name="ColumnTypes" type="{http://schemas.microsoft.com/2003/10/Serialization/Arrays}ArrayOfstring" minOccurs="0"/>
 *         &lt;element name="Error" type="{http://www.w3.org/2001/XMLSchema}string" minOccurs="0"/>
 *         &lt;element name="Rows" type="{http://OneCService2/types}ArrayOfRow" minOccurs="0"/>
 *       &lt;/sequence>
 *     &lt;/restriction>
 *   &lt;/complexContent>
 * &lt;/complexType>
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "ResultSet", namespace = "http://OneCService2/types", propOrder = {
    "columnNames",
    "columnTypes",
    "error",
    "rows"
})
public class ResultSet {

    @XmlElementRef(name = "ColumnNames", namespace = "http://OneCService2/types", type = JAXBElement.class)
    protected JAXBElement<ArrayOfstring> columnNames;
    @XmlElementRef(name = "ColumnTypes", namespace = "http://OneCService2/types", type = JAXBElement.class)
    protected JAXBElement<ArrayOfstring> columnTypes;
    @XmlElementRef(name = "Error", namespace = "http://OneCService2/types", type = JAXBElement.class)
    protected JAXBElement<String> error;
    @XmlElementRef(name = "Rows", namespace = "http://OneCService2/types", type = JAXBElement.class)
    protected JAXBElement<ArrayOfRow> rows;

    /**
     * Gets the value of the columnNames property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link ArrayOfstring }{@code >}
     *     
     */
    public JAXBElement<ArrayOfstring> getColumnNames() {
        return columnNames;
    }

    /**
     * Sets the value of the columnNames property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link ArrayOfstring }{@code >}
     *     
     */
    public void setColumnNames(JAXBElement<ArrayOfstring> value) {
        this.columnNames = ((JAXBElement<ArrayOfstring> ) value);
    }

    /**
     * Gets the value of the columnTypes property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link ArrayOfstring }{@code >}
     *     
     */
    public JAXBElement<ArrayOfstring> getColumnTypes() {
        return columnTypes;
    }

    /**
     * Sets the value of the columnTypes property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link ArrayOfstring }{@code >}
     *     
     */
    public void setColumnTypes(JAXBElement<ArrayOfstring> value) {
        this.columnTypes = ((JAXBElement<ArrayOfstring> ) value);
    }

    /**
     * Gets the value of the error property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public JAXBElement<String> getError() {
        return error;
    }

    /**
     * Sets the value of the error property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public void setError(JAXBElement<String> value) {
        this.error = ((JAXBElement<String> ) value);
    }

    /**
     * Gets the value of the rows property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link ArrayOfRow }{@code >}
     *     
     */
    public JAXBElement<ArrayOfRow> getRows() {
        return rows;
    }

    /**
     * Sets the value of the rows property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link ArrayOfRow }{@code >}
     *     
     */
    public void setRows(JAXBElement<ArrayOfRow> value) {
        this.rows = ((JAXBElement<ArrayOfRow> ) value);
    }

}
