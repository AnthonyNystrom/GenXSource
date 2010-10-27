/* ------------------------------------------------
 * TimeInterval.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.util;

/**
 * Represents a time interval.
 * @author Alex Nesterov
 */
public class TimeInterval
{
    /**
     * Creates a new instance of the <tt>TimeInterval</tt> class.
     * @param	timeMillis
     *		Time in milliseconds.
     */
    public TimeInterval(long timeMillis)
    {
	_hours = timeMillis / (3600 * 1000);
	_minutes = (timeMillis - _hours * 3600 * 1000) / (60 * 1000);
	_seconds =
		(timeMillis - _hours * 3600 * 1000 - _minutes * 60 * 1000) / 1000;
    }

    private long _hours;

    /**
     * Gets the number of hours in this time interval.
     * @return	Number of hours in this time interval.
     */
    public long getHours()
    {
	return _hours;
    }

    private long _minutes;

    /**
     * Gets the number of minutes in this time interval.
     * @return	Number of minutes in this time interval.
     */
    public long getMinutes()
    {
	return _minutes;
    }

    private long _seconds;

    /**
     * Gets the number of seconds in this time interval.
     * @return	Number of seconds in this time interval.
     */
    public long getSeconds()
    {
	return _seconds;
    }

    @Override
    public String toString()
    {
	return String.format("%02d:%02d:%02d", getHours(), getMinutes(),
			     getSeconds());
    }

}
