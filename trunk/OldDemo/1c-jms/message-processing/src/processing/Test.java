package processing;

import javax.jms.Message;
import org.apache.camel.Consume;

/**
 *
 * @author Mejov Andrei <mini_root@freemail.ru>
 */
public class Test
{
     @Consume(uri = "activemq:toStdOut")
     public void toStdOut(byte[] _message)
     {
         System.out.println("QQQ "+new String(_message, 0, _message.length)+"|");
         System.out.println("QQQ1 "+_message.length);
     }

     @Consume(uri = "activemq:toStdOut2")
     public void toStdOut2(byte[] _message) throws Throwable
     {
         System.out.println("WWW "+_message + " | " + _message.length);
         System.out.println("WWW1 "+ (new String(_message, "UTF-8")) + " | " + _message.length);
     }
}

