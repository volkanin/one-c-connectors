package ocs2.util;

import java.util.Iterator;
import ocs2.ResultSet;

/**
 *
 * @author Andrei Mejov <andrei.mejov@gmail.com>
 */
public class ResultSetProcessor
{
    private ResultSet     resultSet = null;
    private Integer currentPosition = -1;

    public ResultSetProcessor(ResultSet _resultSet)
    {
        resultSet = _resultSet;
    }

    public Integer getColumnsCount()
    {
        return resultSet.getColumnNames().getValue().getString().size();
    }

    public String getColumnName(Integer _index)
    {
        return resultSet.getColumnNames().getValue().getString().get(_index);
    }

    public String getColumnType(Integer _index)
    {
        return resultSet.getColumnTypes().getValue().getString().get(_index);
    }

    public boolean hasError()
    {
        if (resultSet.getError().isNil())
        {
            return false;
        }
        else if (resultSet.getError().getValue().trim().equals(""))
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    public String getError()
    {
        return resultSet.getError().getValue();
    }

    public boolean next()
    {
        currentPosition++;
        if (currentPosition < resultSet.getRows().getValue().getRow().size())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Object getValue(Integer _index)
    {
        return resultSet.getRows().getValue().getRow().get(currentPosition).getValues().getValue().getContent().get(_index);
    }
}
