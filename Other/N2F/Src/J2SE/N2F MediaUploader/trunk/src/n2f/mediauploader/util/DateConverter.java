/* ------------------------------------------------
 * DateConverterTest.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader.util;

import static n2f.mediauploader.util.resources.DateConverterResources.*;

/**
 * Provides functionality to process data and time.
 * @author Alex Nesterov
 */
public final class DateConverter
{
    /**
     * This class is immutable.
     */
    private DateConverter()
    {
    }

    /**
     * Subtracts one date from another date and represents the result as a time
     * interval that identifies the number of hours, minutes, and seconds 
     * between the specified dates.
     * 
     * @param	greaterTimeInMillis
     *		Specifies the greater date in milliseconds.
     * @param	smallerTimeInMillis
     *		Specifies the smaller date in milliseconds.
     * @return	A time interval that identifies the number of hours, minutes,
     *		and seconds between the specified dates.
     * @throws	IllegalArgumentException
     *		If the specified <tt>greaterTimeInMillis</tt> is negative, or
     *		if the specified <tt>smallerTimeInMillis</tt> is negative.
     */
    public static TimeInterval subtract(long greaterTimeInMillis,
					  long smallerTimeInMillis)
    {
	if (greaterTimeInMillis < 0)
	    throw new IllegalArgumentException(String.format(CannotBeNegative,
							     "greaterTimeInMillis"));
	if (smallerTimeInMillis < 0)
	    throw new IllegalArgumentException(String.format(CannotBeNegative,
							     "smallerTimeInMillis"));
	return new TimeInterval(greaterTimeInMillis - smallerTimeInMillis);
    }

}
