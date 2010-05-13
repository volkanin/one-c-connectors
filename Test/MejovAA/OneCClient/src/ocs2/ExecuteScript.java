
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
 *         &lt;element name="_connectionName" type="{http://www.w3.org/2001/XMLSchema}string" minOccurs="0"/>
 *         &lt;element name="_poolUserName" type="{http://www.w3.org/2001/XMLSchema}string" minOccurs="0"/>
 *         &lt;element name="_poolPassword" type="{http://www.w3.org/2001/XMLSchema}string" minOccurs="0"/>
 *         &lt;element name="_script" type="{http://www.w3.org/2001/XMLSchema}string" minOccurs="0"/>
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
    "connectionName",
    "poolUserName",
    "poolPassword",
    "script"
})
@XmlRootElement(name = "ExecuteScript")
public class ExecuteScript {

    @XmlElementRef(name = "_connectionName", namespace = "http://OneCService2", type = JAXBElement.class)
    protected JAXBElement<String> connectionName;
    @XmlElementRef(name = "_poolUserName", namespace = "http://OneCService2", type = JAXBElement.class)
    protected JAXBElement<String> poolUserName;
    @XmlElementRef(name = "_poolPassword", namespace = "http://OneCService2", type = JAXBElement.class)
    protected JAXBElement<String> poolPassword;
    @XmlElementRef(name = "_script", namespace = "http://OneCService2", type = JAXBElement.class)
    protected JAXBElement<String> script;

    /**
     * Gets the value of the connectionName property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public JAXBElement<String> getConnectionName() {
        return connectionName;
    }

    /**
     * Sets the value of the connectionName property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public void setConnectionName(JAXBElement<String> value) {
        this.connectionName = ((JAXBElement<String> ) value);
    }

    /**
     * Gets the value of the poolUserName property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public JAXBElement<String> getPoolUserName() {
        return poolUserName;
    }

    /**
     * Sets the value of the poolUserName property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public void setPoolUserName(JAXBElement<String> value) {
        this.poolUserName = ((JAXBElement<String> ) value);
    }

    /**
     * Gets the value of the poolPassword property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public JAXBElement<String> getPoolPassword() {
        return poolPassword;
    }

    /**
     * Sets the value of the poolPassword property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public void setPoolPassword(JAXBElement<String> value) {
        this.poolPassword = ((JAXBElement<String> ) value);
    }

    /**
     * Gets the value of the script property.
     * 
     * @return
     *     possible object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public JAXBElement<String> getScript() {
        return script;
    }

    /**
     * Sets the value of the script property.
     * 
     * @param value
     *     allowed object is
     *     {@link JAXBElement }{@code <}{@link String }{@code >}
     *     
     */
    public void setScript(JAXBElement<String> value) {
        this.script = ((JAXBElement<String> ) value);
    }

}
