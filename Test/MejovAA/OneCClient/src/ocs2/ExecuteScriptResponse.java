
package ocs2;

import javax.xml.bind.JAXBElement;
import javax.xml.bind.annotation.XmlAccessType;
import javax.xml.bind.annotation.XmlAccessorType;
import javax.xml.bind.annotation.XmlElementRef;
import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.bind.annotation.XmlType;


/**
 * <p>Java class for anonymous complex type.
 * 
 * <p>The following schema fragment specifies the expected content contained within this class.
 * 
 * <pre>
 * &lt;complexType>
 *   &lt;complexContent>
 *     &lt;restriction base="{http://www.w3.org/2001/XMLSchema}anyType">
 *       &lt;sequence>
 *         &lt;element name="ExecuteScriptResult" type="{http://OneCService2/types}ResultSet" minOccurs="0"/>
 *       &lt;/sequence>
 *     &lt;/restriction>
 *   &lt;/complexContent>
 * &lt;/complexType>
 * </pre>
 * 
 * 
 */
@XmlAccessorType(XmlAccessType.FIELD)
@XmlType(name = "", propOrder = {
    "executeScriptResult"
})
@XmlRootElement(name = "ExecuteScriptResponse")
public class ExecuteScriptResponse {

    @XmlElementRef(name = "ExecuteScriptResult", namespace = "http://OneCService2", type = JAXBElement.class)
    protected JAXBElement<ResultSet> executeScriptResult;

    /**
     * Gets the value of the executeScriptResult property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link ResultSet }{@code >}
     *     
     */
    public JAXBElement<ResultSet> getExecuteScriptResult() {
        return executeScriptResult;
    }

    /**
     * Sets the value of the executeScriptResult property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link ResultSet }{@code >}
     *     
     */
    public void setExecuteScriptResult(JAXBElement<ResultSet> value) {
        this.executeScriptResult = ((JAXBElement<ResultSet> ) value);
    }

}
