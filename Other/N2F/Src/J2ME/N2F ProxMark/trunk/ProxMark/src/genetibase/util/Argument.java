/* ------------------------------------------------
 * Argument.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.util;

/**
 * Provides functionality to validate arguments.
 *
 * @author Alex Nesterov
 */
public final class Argument
{
    /* ------ Methods.Public.Static ------ */
    
    /**
     * Returns the value indicating whether the specified string is empty or
     * contains only of spaces.
     * 
     * @return	<code>true</code> if the specified string is empty or contains
     *		only of spaces; otherwise, <code>false</code>.
     *
     * @throws NullPointerException if the specified is <code>null</code>.
     */
    public static boolean isEmpty(String stringToCheck)
    {
	if (stringToCheck == null)
	{
	    throw new NullPointerException("stringToCheck");
	}
	
	return stringToCheck.trim().length() == 0;
    }
    
    /**
     * Returns the value indicating whether the specified string is
     * <code>null</code>, empty, or contains only of spaces.
     *
     * @return	<code>true</code> if the specified string is <code>null</code>,
     *		empty, or contains only of spaces; otherwise,
     *		<code>false</code>.
     */
    public static boolean isNullOrEmpty(String stringToCheck)
    {
	return stringToCheck == null || isEmpty(stringToCheck);
    }
    
    /* ------ Constructors ------ */
    
    /**
     * This class is immutable.
     */
    private Argument()
    {
    }
}
