/* ------------------------------------------------
 * WebServiceInterop.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader.webservice;

import java.util.Calendar;
import java.util.Date;

/**
 * Provides helper methods for web-service interoperation.
 * @author Andrei Obraztsov
 * @author Alex Nesterov
 */
public final class WebServiceInterop
{
    private static final long _initialTicks = 621355968000000000L;

    /**
     * This class is immutable.
     */
    private WebServiceInterop()
    {
    }
    
    public static String parseDate(String timeStr)
    {
	if (timeStr == null)
	    return "UNKNOWN DATE";
	
	StringBuffer sb = new StringBuffer();
	
	try
	{
	    long time = (Long.parseLong(timeStr) - _initialTicks) / 10000;
	    Calendar c = Calendar.getInstance();
	    c.setTime(new Date(time));
	    return formatString(c);
	}
	catch (Exception ex)
	{
	    sb.setLength(0);
	    sb.append("UNKNOWN DATE");
	}
	
	return sb.toString();
    }

    public static String getServiceDate(long date)
    {
	return String.valueOf(date * 10000 + _initialTicks);
    }

    private static String formatString(Calendar c)
    {
	StringBuffer sb = new StringBuffer();
	String time = c.get(Calendar.DATE) < 10
		? ZERO + c.get(Calendar.DATE)
		: String.valueOf(c.get(Calendar.DATE));

	sb.append(time).append(DOT);
	time = c.get(Calendar.MONTH) < 9
		? ZERO + (c.get(Calendar.MONTH) + 1)
		: String.valueOf(c.get(Calendar.MONTH) + 1);
	
	sb.append(time).append(DOT);
	sb.append(c.get(Calendar.YEAR)).append(SPACE);
	
	time = c.get(Calendar.HOUR_OF_DAY) < 10
		? ZERO + c.get(Calendar.HOUR_OF_DAY)
		: String.valueOf(c.get(Calendar.HOUR_OF_DAY));
	
	sb.append(time).append(COLUMN);
	time = c.get(Calendar.MINUTE) < 10
		? ZERO + c.get(Calendar.MINUTE)
		: String.valueOf(c.get(Calendar.MINUTE));
	
	sb.append(time).append(COLUMN);
	time = c.get(Calendar.SECOND) < 10
		? ZERO + c.get(Calendar.SECOND)
		: String.valueOf(c.get(Calendar.SECOND));
	
	sb.append(time);
	return sb.toString();
    }

    private static final char COLUMN = ':';
    private static final String SPACE = " ";
    private static final String DOT = ".";
    private static final String ZERO = "0";
}
