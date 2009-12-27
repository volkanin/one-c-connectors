package test;

import java.io.InputStream;
import javax.jms.*;
import org.apache.activemq.ActiveMQConnectionFactory;
import org.junit.After;
import org.junit.AfterClass;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Ignore;
import org.junit.Test;
import static org.junit.Assert.*;

/**
 *
 * @author Mejov Andrei <mini_root@freemail.ru>
 */
public class TestCamel
{

    public TestCamel()
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
    @Ignore("Используется для первоначальной проверки маршрутизации и аутентификации")
    public void simpleTest() throws Throwable
    {
        ActiveMQConnectionFactory factory = new ActiveMQConnectionFactory("tcp://localhost:61616");

        Connection connection = factory.createConnection("test","test");
        connection.start();

        Session session = connection.createSession(true, Session.SESSION_TRANSACTED);

        InputStream is = this.getClass().getResourceAsStream("TestOrder.xml");
        byte[] buf = new byte[is.available()];
        is.read(buf);

        try
        {
            Queue queue = session.createQueue("test");

            MessageProducer producer = session.createProducer(queue);

            BytesMessage message = session.createBytesMessage();
            message.writeBytes(buf);

            producer.send(message);

            session.commit();

            producer.close();
        }
        finally
        {
            connection.close();
        }
    }

    @Test
    @Ignore("Используется для проверки и отладки работы XslProcessor'а")
    public void orderTest() throws Throwable
    {
        ActiveMQConnectionFactory factory = new ActiveMQConnectionFactory("tcp://localhost:61616");

        Connection connection = factory.createConnection("test","test");
        connection.start();

        Session session = connection.createSession(true, Session.SESSION_TRANSACTED);

        InputStream is = this.getClass().getResourceAsStream("TestOrder.xml");
        byte[] buf = new byte[is.available()];
        is.read(buf);

        try
        {
            Queue queue = session.createQueue("contr1In");

            MessageProducer producer = session.createProducer(queue);

            BytesMessage message = session.createBytesMessage();
            message.writeBytes(buf);

            producer.send(message);

            session.commit();

            producer.close();
        }
        finally
        {
            connection.close();
        }
    }

    //Основной тест, используется для проверки входящей и исходщей цепочек
    @Test
    @Ignore("")
    public void mainTest() throws Throwable
    {
        //Заказ
        InputStream is = this.getClass().getResourceAsStream("TestOrder.xml");
        byte[] order = new byte[is.available()];
        is.read(order);
        is.close();

        //Ответ на заказ
        is = this.getClass().getResourceAsStream("TestOrderResponse.xml");
        byte[] orderResponse = new byte[is.available()];
        is.read(orderResponse);
        is.close();

        //Результат на выходе входной цепочки
        is = this.getClass().getResourceAsStream("TestResultIn.xml");
        byte[] resultIn = new byte[is.available()];
        is.read(resultIn);
        is.close();

        //Результат на выходе выходной цепочки
        is = this.getClass().getResourceAsStream("TestResultOut.xml");
        byte[] resultOut = new byte[is.available()];
        is.read(resultOut);
        is.close();


        ActiveMQConnectionFactory factory = new ActiveMQConnectionFactory("tcp://localhost:61616");

        Connection connection = factory.createConnection("test","test");
        connection.start();

        Session session = connection.createSession(true, Session.SESSION_TRANSACTED);
        try
        {
            //Входящая цепочка
            Queue contr1InQueue = session.createQueue("contr1In");            
            MessageProducer producer = session.createProducer(contr1InQueue);            

            BytesMessage message = session.createBytesMessage();
            message.writeBytes(order);
            producer.send(message);
            session.commit();

            Thread.sleep(3000L);

            Queue oneCInQueue = session.createQueue("oneCIn");
            MessageConsumer consumer = session.createConsumer(oneCInQueue);
            
            message = (BytesMessage)consumer.receive(1000L);
            assertNotNull(message);

            session.commit();

            byte[] buf = new byte[(int)message.getBodyLength()];
            message.readBytes(buf);            
            assertEquals((new String(buf)).replaceAll("\\t|\\s", ""), (new String(resultIn)).replaceAll("\\t|\\s", ""));
            
            
            producer.close();
            consumer.close();


            //Исходящая цепочка
            Queue oneCOutQueue = session.createQueue("oneCOut");
            producer = session.createProducer(oneCOutQueue);            

            message = session.createBytesMessage();
            message.writeBytes(orderResponse);
            producer.send(message);
            session.commit();

            Thread.sleep(3000L);

            Queue contr1Out = session.createQueue("contr1Out");
            consumer = session.createConsumer(contr1Out);

            message = (BytesMessage)consumer.receive(1000L);
            assertNotNull(message);

            session.commit();

            buf = new byte[(int)message.getBodyLength()];
            message.readBytes(buf);
            assertEquals((new String(buf)).replaceAll("\\t|\\s", ""), (new String(resultOut)).replaceAll("\\t|\\s", ""));
            

            producer.close();
            consumer.close();

        }
        finally
        {
            connection.close();
        }
    }


    @Test
    @Ignore("")
    public void sendOrder() throws Throwable
    {
         //Заказ
        InputStream is = this.getClass().getResourceAsStream("TestOrder.xml");
        byte[] order = new byte[is.available()];
        is.read(order);
        is.close();

        //Ответ на заказ
        is = this.getClass().getResourceAsStream("TestOrderResponse.xml");
        byte[] orderResponse = new byte[is.available()];
        is.read(orderResponse);
        is.close();

        //Результат на выходе входной цепочки
        is = this.getClass().getResourceAsStream("TestResultIn.xml");
        byte[] resultIn = new byte[is.available()];
        is.read(resultIn);
        is.close();

        //Результат на выходе выходной цепочки
        is = this.getClass().getResourceAsStream("TestResultOut.xml");
        byte[] resultOut = new byte[is.available()];
        is.read(resultOut);
        is.close();

        ActiveMQConnectionFactory factory = new ActiveMQConnectionFactory("tcp://localhost:61616");

        Connection connection = factory.createConnection("test","test");
        connection.start();

        Session session = connection.createSession(true, Session.SESSION_TRANSACTED);
        try
        {
            //Входящая цепочка
            Queue contr1InQueue = session.createQueue("contr1In");
            MessageProducer producer = session.createProducer(contr1InQueue);

            BytesMessage message = session.createBytesMessage();
            message.writeBytes(order);
            producer.send(message);
            session.commit();

            producer.close();
        }
        finally
        {
            connection.close();
        }
    }

    @Test
    //@Ignore("")
    public void receiveOrderResponse() throws Throwable
    {
        ActiveMQConnectionFactory factory = new ActiveMQConnectionFactory("tcp://localhost:61616");

        Connection connection = factory.createConnection("test","test");
        connection.start();

        Session session = connection.createSession(true, Session.SESSION_TRANSACTED);
        try
        {
            //Входящая цепочка
            Queue contr1OutQueue = session.createQueue("contr1Out");
            MessageConsumer consumer = session.createConsumer(contr1OutQueue);

            BytesMessage message = (BytesMessage)consumer.receive(2000L);

            assertNotNull(message);

            byte[] buf = new byte[(int)message.getBodyLength()];
            message.readBytes(buf);

            System.out.println(new String(buf, "UTF-8"));

            session.commit();
            consumer.close();
        }
        finally
        {
            connection.close();
        }
    }
}

