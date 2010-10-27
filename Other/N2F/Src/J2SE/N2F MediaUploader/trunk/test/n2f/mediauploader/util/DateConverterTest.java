/* ------------------------------------------------
 * DateConverterTest.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader.util;

import org.junit.*;
import static org.junit.Assert.*;

/**
 * @author Alex Nesterov
 */
public final class DateConverterTest
{
    @Test
    public void subtract_Mismatch()
    {
	TimeInterval timeInterval = DateConverter.subtract(1000, 2000);
	assertEquals(0, timeInterval.getHours());
	assertEquals(0, timeInterval.getMinutes());
	assertEquals(-1, timeInterval.getSeconds());
    }
    
    @Test(expected = IllegalArgumentException.class)
    public void subtract_NegativeArg0()
    {
	DateConverter.subtract(-1, 1000);
    }

    @Test(expected = IllegalArgumentException.class)
    public void subtract_NegativeArg1()
    {
	DateConverter.subtract(1000, -1);
    }

    @Test
    public void subtract_ZeroSeconds()
    {
	TimeInterval timeInterval = DateConverter.subtract(1000, 999);
	assertEquals(0, timeInterval.getHours());
	assertEquals(0, timeInterval.getMinutes());
	assertEquals(0, timeInterval.getSeconds());
    }

    @Test
    public void subtract_Seconds()
    {
	TimeInterval timeInterval = DateConverter.subtract(5000, 3000);
	assertEquals(0, timeInterval.getHours());
	assertEquals(0, timeInterval.getMinutes());
	assertEquals(2, timeInterval.getSeconds());
    }

    @Test
    public void subtract_Minutes()
    {
	TimeInterval timeInterval = DateConverter.subtract(120 * 1000 + 1000, 60 * 1000);
	assertEquals(0, timeInterval.getHours());
	assertEquals(1, timeInterval.getMinutes());
	assertEquals(1, timeInterval.getSeconds());
    }

    public void subtract_Hours()
    {
	TimeInterval timeInterval = DateConverter.subtract(3 * 3600 * 1000 + 120 * 1000 + 1000, 2 * 3600 * 1000 + 60 * 1000);
	assertEquals(1, timeInterval.getHours());
	assertEquals(1, timeInterval.getMinutes());
	assertEquals(1, timeInterval.getSeconds());
    }
}
