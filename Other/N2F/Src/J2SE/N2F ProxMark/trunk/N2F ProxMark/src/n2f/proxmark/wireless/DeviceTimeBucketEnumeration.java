/* ------------------------------------------------
 * DeviceTimeBucketEnumeration.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark.wireless;

import java.util.Enumeration;

/**
 * @author Alex Nesterov
 */
public final class DeviceTimeBucketEnumeration
	implements IDeviceTimeBucketEnumeration
{
    /* ------ Methods.Public ------ */
    
    /**
     * Tests if this enumeration contains more elements.
     *
     * @return	<code>true</code> if and only if this enumeration object
     *		contains at least one more element to provide;
     *		<code>false</code> otherwise.
     */
    public boolean hasMoreElements()
    {
	return _originalEnumeration.hasMoreElements();
    }
    
    /**
     * Returns the next element of this enumeration if this enumeration object
     * has at least one more element to provide.
     *
     * @return The next element of this enumeration.
     * @throws NoSuchElementException if no more elements exist.
     */
    public DeviceTimeBucket nextElement()
    {
	return (DeviceTimeBucket)_originalEnumeration.nextElement();
    }
    
    /* ------ Declarations ------ */
    
    private Enumeration _originalEnumeration;
    
    /* ------ Constructors ------ */
    
    /**
     * Creates a new instance of the <tt>DeviceTimeBucketEnumeration</tt> class.
     *
     * @throws	IllegalArgumentException
     *		If the specified <tt>originalEnumeration</tt> is <code>null</code>.
     */
    public DeviceTimeBucketEnumeration(Enumeration originalEnumeration)
    {
	if (originalEnumeration == null)
	    throw new IllegalArgumentException("originalEnumeration cannot be null.");
	_originalEnumeration = originalEnumeration;
    }
}
