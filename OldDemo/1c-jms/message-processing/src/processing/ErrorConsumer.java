package processing;

import java.io.File;
import java.io.FileOutputStream;
import java.util.logging.Level;
import java.util.logging.Logger;
import org.apache.camel.Consume;

/**
 * Класс, представляет собой компонент для Camel'а, содержащий методы-приемники
 * для обработки ошибок.
 * 
 * @author Mejov Andrei <mini_root@freemail.ru>
 */
public class ErrorConsumer
{
    private Logger logger = Logger.getLogger(Logger.GLOBAL_LOGGER_NAME);
    private String destinationDir = null;

    public Logger getLogger() 
    {
        return logger;
    }

    public void setLogger(Logger logger) 
    {
        this.logger = logger;
    }

    public String getDestinationDir()
    {
        return destinationDir;
    }

    public void setDestinationDir(String destinationDir)
    {
        this.destinationDir = destinationDir;
    }  
    
    private void mkdirIfNeed()
    {
        File f = new File(this.destinationDir + "/routeError");
        if (!f.exists())
        {
            f.mkdir();
        }
        f = new File(this.destinationDir + "/exchangeError");
        if (!f.exists())
        {
            f.mkdir();
        }
        f = new File(this.destinationDir + "/error");
        if (!f.exists())
        {
            f.mkdir();
        }
    }

    private String getUniqueFileName(String _subdir)
    {
        mkdirIfNeed();
        File f = null;
        do
        {
            String s =
                this.destinationDir +
                "/" +
                _subdir +
                "/" +
                System.currentTimeMillis() +
                "-" +
                Math.round(Math.random()*10);
            f = new File(s);
        } while (f.exists());
        return f.toString();
    }

    private synchronized void saveToFile(String _fileName, byte[] _message) throws Throwable
    {
        FileOutputStream fos = new FileOutputStream(_fileName);
        try
        {
            fos.write(_message);
            fos.flush();
        }
        finally
        {
            fos.close();
        }
    }

    @Consume(uri = "activemq:routeError")
    public void onRouteError(byte[] _message)
    {
        try
        {
            saveToFile(getUniqueFileName("routeError"), _message);
        }
        catch (Throwable _t)
        {
            logger.log(Level.SEVERE, "Fatal exception in ErrorConsumenrt", _t);
        }
    }

    @Consume(uri = "activemq:exchangeError")
    public void onExchangeError(byte[] _message)
    {
        try
        {
            saveToFile(getUniqueFileName("exchangeError"), _message);
        }
        catch (Throwable _t)
        {
            logger.log(Level.SEVERE, "Fatal exception in ErrorConsumenrt", _t);
        }
    }

    @Consume(uri = "activemq:error")
    public void onError(byte[] _message)
    {
        try
        {
            saveToFile(getUniqueFileName("error"), _message);
        }
        catch (Throwable _t)
        {
            logger.log(Level.SEVERE, "Fatal exception in ErrorConsumenrt", _t);
        }
    }
}
