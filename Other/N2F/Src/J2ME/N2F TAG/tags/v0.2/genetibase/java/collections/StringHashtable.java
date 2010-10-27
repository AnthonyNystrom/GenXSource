/* ------------------------------------------------
 * StringHashtable.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.java.collections;

import java.util.*;

/**
 * Represents a hashtable that accepts strings for keys and values.
 *
 * @author Alex Nesterov
 */
public class StringHashtable
{
    /* ------ Methods.Public ------ */
    
    /**
     * Clears this hashtable so that it contains no entries.
     */
    public void clear()
    {
	_internalHashtable.clear();
    }
    
    /**
     * Tests if some key maps into the specified value in this hashtable. This
     * operation is more expensive than the <tt>containsKey</tt> method.
     *
     * @param value Specifies the value to search for.
     * @return	<code>true</code> if some key maps to the <tt>value</tt>
     *		argument in this hashtable; <code>false</code> otherwise.
     * @throws	NullPointerException if the specified <tt>value</tt> is <code>null</code>.
     */
    public boolean contains(String value)
    {
	return _internalHashtable.contains(value);
    }
    
    /**
     * Tests if the specified string is a key in this hashtable.
     *
     * @param key   Possible key.
     * @return	<code>true</code> if the specified value is a key in this
     *		hashtable; <code>false</code> otherwise.
     */
    public boolean containsKey(String key)
    {
	return _internalHashtable.containsKey(key);
    }
    
    /**
     * Returns an enumeration of the values in this hashtable. Use the
     * IStringEnumeration methods on the returned object to fetch the elements
     * sequentially.
     *
     * @return Enumeration of the values in this hashtable.
     */
    public IStringEnumeration elements()
    {
	return new StringEnumeration(_internalHashtable.elements());
    }
    
    /**
     * Returns the value to which the specified key is mapped in this hashtable.
     *
     * @param key   Specifies the key in the hashtable.
     * @return	The value to which the <tt>key</tt> is mapped in this hashtable;
     *		<code>null</code> if the <tt>key</tt> is not mapped to any value
     *		in this hashtable.
     */
    public String get(String key)
    {
	return (String)_internalHashtable.get(key);
    }
    
    /**
     * Tests if this hashtable maps no keys to values.
     *
     * @return	<code>true</code> if this hashtable maps no keys to values;
     *		<code>false</code> otherwise.
     */
    public boolean isEmpty()
    {
	return _internalHashtable.isEmpty();
    }
    
    /**
     * Returns an enumeration of the keys in this hashtable.
     *
     * @return Enumeration of the keys in this hashtable.
     */
    public IStringEnumeration keys()
    {
	return new StringEnumeration(_internalHashtable.keys());
    }
    
    /**
     * Maps the specified <tt>key</tt> to the specified <tt>value</tt> in this
     * hashtable. Neither the <tt>key</tt> nor the <tt>value</tt> can be <code>null</code>.
     *
     * @return	The previous value of the specified <tt>key</tt> in this hashtable, or
     *		<code>null</code> if it did not have one.
     * @throws	NullPointerException if the <tt>key</tt> or <tt>value</tt> is <code>null</code>.
     */
    public String put(String key, String value)
    {
	return (String)_internalHashtable.put(key, value);
    }
    
    /**
     * Removes the <tt>key</tt> (and its corresponding value) from this
     * hashtable. This method does nothing if the key is not in the hashtable.
     *
     * @param key   The key that needs to be removed.
     * @return The value to which the <tt>key</tt> had been mapped in this
     *		hashtable, or <code>null</code> if the <tt>key</tt> did not have
     *		a mapping.
     */
    public String remove(String key)
    {
	return (String)_internalHashtable.remove(key);
    }
    
    /**
     * Returns the number of entries in this hashtable.
     *
     * @returns The number of entries in this hashtable.
     */
    public int size()
    {
	return _internalHashtable.size();
    }
    
    /**
     * Returns a rather long string representation of this hashtable.
     *
     * @returns String representation of this hashtable.
     */
    public String toString()
    {
	return _internalHashtable.toString();
    }
    
    /* ------ Declarations ------ */
    
    private Hashtable _internalHashtable;
    
    /* ------ Constructors ------ */
    
    /**
     * Creates a new instance of StringHashtable.
     */
    public StringHashtable()
    {
	_internalHashtable = new Hashtable();
    }
    
    /**
     * Creates a new instance of StringHashtable.
     *
     * @param initialCapacity	Specifies the initial capacity for the hashtable.
     *
     * @throws	IllegalArgumentException if the specified
     *		<tt>initialCapacity</tt> is negative.
     */
    public StringHashtable(int initialCapacity)
    {
	_internalHashtable = new Hashtable(initialCapacity);
    }
}
