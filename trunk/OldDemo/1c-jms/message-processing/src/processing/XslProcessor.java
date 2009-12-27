package processing;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.InputStream;
import org.apache.camel.Exchange;
import org.apache.camel.Processor;

import javax.xml.transform.*;
import javax.xml.transform.stream.*;
import org.apache.camel.EndpointInject;
import org.apache.camel.Produce;
import org.apache.camel.Producer;
import org.apache.camel.ProducerTemplate;

/**
 * 
 * Класс, представляет собой компонент для Camel'а. Выполняет XSLT преобразование.
 * Дополняет сообщение информацией о контрагенте. В случае возникновения исключения -
 * пересылает сообщение в очередь обработки ошибок.
 *
 * @author Mejov Andrei <mini_root@freemail.ru>
 */
public class XslProcessor implements Processor
{

    private String xslName = null;
    private String contrId = null;

    @EndpointInject(uri="activemq:error")
    private ProducerTemplate errorProducer = null;

    public ProducerTemplate getErrorProducer() 
    {
        return errorProducer;
    }
    public void setErrorProducer(ProducerTemplate errorProducer) 
    {
        this.errorProducer = errorProducer;
    }

    public static class MyErrorListener implements ErrorListener
    {
        private Throwable lastException = null;

        public Throwable getLastException()
        {
            return lastException;
        }
        public void setLastException(Throwable lastException)
        {
            this.lastException = lastException;
        }

        public void error(TransformerException exception) throws TransformerException
        {
            lastException = exception;
        }

        public void fatalError(TransformerException exception) throws TransformerException
        {
            lastException = exception;
        }

        public void warning(TransformerException exception) throws TransformerException
        {
            lastException = exception;
        }
    }

    public String getXslName()
    {
        return xslName;
    }
    public void setXslName(String xslName)
    {
        this.xslName = xslName;
    }

    public String getContrId()
    {
        return contrId;
    }
    public void setContrId(String contrId)
    {
        this.contrId = contrId;
    }

    public void process(Exchange _exchange) throws Exception
    {
        byte[] buf = _exchange.getIn().getBody(byte[].class);
        ByteArrayInputStream xmlStream = new ByteArrayInputStream(buf);
        try
        {
            StreamSource xmlSource = new StreamSource(xmlStream);
            InputStream xslStream = this.getClass().getResourceAsStream(xslName);
            MyErrorListener errorListener = new MyErrorListener();
            try
            {
                StreamSource xslSource = new StreamSource(xslStream);

                TransformerFactory factory = TransformerFactory.newInstance();
                factory.setErrorListener(errorListener);

                Transformer t = factory.newTransformer(xslSource);
                ByteArrayOutputStream resultStream = new ByteArrayOutputStream();
                try
                {
                    StreamResult result = new StreamResult(resultStream);
                    t.setParameter("contrId", contrId);
                    t.transform(xmlSource, result);
                    if (errorListener.lastException != null)
                    {
                        throw new Exception("Exception in xslt: ",errorListener.lastException);
                    }

                    resultStream.flush();
                    _exchange.getOut().setBody(resultStream.toByteArray(), byte[].class);
                }
                finally
                {
                    resultStream.close();
                }
            }
            finally
            {
                xslStream.close();
            }
        }
        catch (Throwable _t)
        {
            errorProducer.sendBody(buf);
        }
        finally
        {
            xmlStream.close();
        }               
    }
}