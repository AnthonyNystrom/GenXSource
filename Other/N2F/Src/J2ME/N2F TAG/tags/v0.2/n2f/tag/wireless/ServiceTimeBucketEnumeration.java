/* ------------------------------------------------
 * ServiceTimeBucketEnumeration.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.tag.wireless;

import java.util.Enumeration;

/**
 * @author Alex Nesterov
 */
public final class ServiceTimeBucketEnumeration
	implements IServiceTimeBucketEnumeration
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
    public ServiceTimeBucket nextElement()
    {
	return (ServiceTimeBucket)_originalEnumeration.nextElement();
    }
    
    /* ------ Declarations ------ */
    
    private Enumeration _originalEnumeration;
    
    /* ------ Constructors ------ */
    
    /**
     * Creates a new instance of ServiceTimeBucketEnumeration.
     *
     * @throws	NullPointerException if the specified
     *		<tt>originalEnumeration</tt> is <code>null</code>.
     */
    public ServiceTimeBucketEnumeration(Enumeration originalEnumeration)
    {
	if (originalEnumeration == null)
	{
	    throw new NullPointerException("originalEnumeration");
	}
	
	_originalEnumeration = originalEnumeration;
    }
}
