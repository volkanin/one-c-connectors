package ocs2.util;

import javax.xml.bind.JAXBElement;
import javax.xml.namespace.QName;
import ocs2.Parameters;

/**
 *
 * @author Andrei Mejov <andrei.mejov>
 */
public class ParamsProcessor
{
    public static Parameters prepareParameters()
    {
        Parameters pa = new Parameters();
        pa.setParams(
                     new JAXBElement<Parameters.Params>(
                                                        new QName("http://OneCService2/types", "Params"),
                                                        Parameters.Params.class,
                                                        new Parameters.Params()
                                                       )
                    );
        return pa;
    }
}
