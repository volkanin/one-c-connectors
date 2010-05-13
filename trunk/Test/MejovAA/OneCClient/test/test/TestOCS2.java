package test;

import java.net.URL;
import java.util.Date;
import javax.xml.bind.JAXBElement;
import javax.xml.namespace.QName;
import javax.xml.ws.Service;
import ocs2.OneCService2;
import ocs2.OneCService2_Service;
import ocs2.Parameters;
import ocs2.ResultSet;
import ocs2.util.ParamsProcessor;
import ocs2.util.ResultSetProcessor;
import org.junit.After;
import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Ignore;
import org.junit.Test;
import static org.junit.Assert.*;

/**
 *
 * @author Andrei Mejov <andrei.mejov@gmail.com>
 */
public class TestOCS2
{

    public static class WorkThread extends Thread
    { 
        @Override
        public void run()
        {
            try
            {
                Service service =
                OneCService2_Service.create(
                                            new URL("http://127.0.0.1:9000/OneCService2?wsdl"),
                                            new QName("http://OneCService2","OneCService2")
                                            );
                OneCService2 ocs2 = service.getPort(OneCService2.class);

                while (true)
                {
                    ResultSet r = ocs2.executeRequest("Test81", "PoolUserName", "PoolPassword", "ВЫБРАТЬ 1");
                    ResultSetProcessor rp = new ResultSetProcessor(r);
                    System.out.println(""+Thread.currentThread().getName()+" "+(new Date()).toString()+" "+r.getError().getValue());
                    
                    assertFalse(rp.hasError());
                    assertTrue(rp.next());
                    assertEquals(rp.getValue(0), "1");                    
                }
            }
            catch (Throwable _t)
            {
                throw new RuntimeException(_t);
            }
        }
    }

    public TestOCS2()
    {
    }

    @BeforeClass
    public static void setUpClass() throws Exception
    {
    }

    @AfterClass
    public static void tearDownClass() throws Exception
    {
    }

    @Before
    public void setUp()
    {
    }

    @After
    public void tearDown()
    {
    }
    
    @Test
    @Ignore
    public void testRequest() throws Throwable
    {
        Service service =
                   OneCService2_Service.create(
                                                new URL("http://127.0.0.1:9000/OneCService2?wsdl"),
                                                new QName("http://OneCService2","OneCService2")
                                              );
        OneCService2 ocs2 = service.getPort(OneCService2.class);

        ResultSet r = ocs2.executeRequest("Test81", "PoolUserName", "PoolPassword", "ВЫБРАТЬ 1");
        ResultSetProcessor rp = new ResultSetProcessor(r);
        assertFalse(rp.hasError());
        assertTrue(rp.next());
        assertEquals(rp.getValue(0), "1");

        r = ocs2.executeScript("Test81", "PoolUserName", "PoolPassword", "результат=2+3;");
        rp = new ResultSetProcessor(r);
        assertFalse(rp.hasError());
        assertTrue(rp.next());
        assertEquals(rp.getValue(0), "5");
        
        
        Parameters pa = ParamsProcessor.prepareParameters();
        pa.getParams().getValue().getContent().add("3");
        pa.getParams().getValue().getContent().add("Ц");
        r = ocs2.executeMethod("Test81", "PoolUserName", "PoolPassword", "OneCService2_ТестовыйМетод", pa);
        rp = new ResultSetProcessor(r);
        System.out.println(""+rp.getError());
        assertFalse(rp.hasError());
        assertTrue(rp.next());
        assertEquals(rp.getValue(0), "3|Й");
    }

    @Test
    public void testLoad() throws Throwable
    {
        WorkThread firstThread = new WorkThread();
        WorkThread secondThread = new WorkThread();

        firstThread.start();
        secondThread.start();
        secondThread.join();
    }

}