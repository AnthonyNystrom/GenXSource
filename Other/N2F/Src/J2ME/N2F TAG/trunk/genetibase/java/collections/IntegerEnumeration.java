/* ------------------------------------------------
 * IntegerEnumeration.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.java.collections;

import java.util.NoSuchElementException;

/**
 * Provides functionality to iterate an integer collection.
 * 
 * @author Alex Nesterov
 */
public class IntegerEnumeration
{
    /**
     * Tests if this enumeration contains more elements.
     *
     * @return	<code>true</code> if and only if this enumeration object
     *		contains at least one more element to provide;
     *		<code>false</code> otherwise.
     */
    public boolean hasMoreElements()
    {
	return _index < _array.length;
    }
    
    /**
     * Returns the next element of this enumeration if this enumeration object
     * has at least one more element to provide.
     *
     * @return The next element of this enumeration.
     * @throws NoSuchElementException if no more elements exist.
     */
    public int nextElement()
    {
	if (_index >= _array.length)
	{
	    throw new NoSuchElementException();
	}
	
	return _array[_index++];
    }
    
    /* ------ Declarations ------ */
    
    private int _index;
    private int[] _array;
    
    /* ------ Constructors ------ */
    
    /**
     * Creates a new instance of IntegerEnumeration.
     *
     * @param array  Specifies an array of items in an integer list.
     */
    public IntegerEnumeration(int[] array)
    {
	if (array == null)
	{
	    throw new NullPointerException("array");
	}
	
	_array = array;
    }
}
