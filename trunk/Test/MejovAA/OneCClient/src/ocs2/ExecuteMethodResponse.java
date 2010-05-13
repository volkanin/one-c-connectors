
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
 *         &lt;element name="ExecuteMethodResult" type="{http://OneCService2/types}ResultSet" minOccurs="0"/>
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
    "executeMethodResult"
})
@XmlRootElement(name = "ExecuteMethodResponse")
public class ExecuteMethodResponse {

    @XmlElementRef(name = "ExecuteMethodResult", namespace = "http://OneCService2", type = JAXBElement.class)
    protected JAXBElement<ResultSet> executeMethodResult;

    /**
     * Gets the value of the executeMethodResult property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link ResultSet }{@code >}
     *     
     */
    public JAXBElement<ResultSet> getExecuteMethodResult() {
        return executeMethodResult;
    }

    /**
     * Sets the value of the executeMethodResult property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link ResultSet }{@code >}
     *     
     */
    public void setExecuteMethodResult(JAXBElement<ResultSet> value) {
        this.executeMethodResult = ((JAXBElement<ResultSet> ) value);
    }

}
