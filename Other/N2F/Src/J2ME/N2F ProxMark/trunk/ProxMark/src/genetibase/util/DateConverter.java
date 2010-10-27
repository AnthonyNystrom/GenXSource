/* ------------------------------------------------
 * DateConverter.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.util;

/**
 * Provides functionality to convert dates to different formats.
 *
 * @author Alex Nesterov
 */
public final class DateConverter
{
    /* ------ Methods.Public.Static ------ */
    
    /**
     * Determines the number of ticks for 0:00.00 01/01/1970.
     */
    private static final long _initialTicks = 621355968000000000L;
    
    /**
     * Converts the specified date in milliseconds to ticks that are standard
     * for Microsoft .NET Framework. Take into consideration that .NET
     * Framework expects the date to start from 1 year while J2ME expects it to
     * start from 1970 year.
     *
     * @return	Ticks that are standard for Microsoft .NET Framework.
     */
    public static long getDotNetTicks(long millis)
    {
	return millis * 10000 + _initialTicks;
    }
    
    /* ------ Constructors ------ */
    
    /**
     * This class is immutable.
     */
    private DateConverter()
    {
    }
}
