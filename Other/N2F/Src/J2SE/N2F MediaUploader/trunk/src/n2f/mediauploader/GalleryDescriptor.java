/* ------------------------------------------------
 * GalleryDescriptor.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

import static n2f.mediauploader.resources.ExceptionResources.*;

/**
 * Encapsulates PhotoOrganise Web-Service Gallery related data.
 * 
 * @author Alex Nesterov
 */
public final class GalleryDescriptor
{
    /**
     * Creates a new instance of the <tt>GalleryDescriptor</tt> class.
     * 
     * @param	galleryName
     *		Specifies the name for the gallery.
     * @param	galleryID
     *		Specifies the gallery identifier.
     * @throws	IllegalArgumentException
     *		If the specified <tt>galleryName</tt> is <code>null</code>, or
     *		if the specified <tt>galleryID</tt> is <code>null</code>.
     */
    public GalleryDescriptor(String galleryName, String galleryID)
    {
	if (galleryName == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull, "galleryName"));
	if (galleryID == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull, "galleryID"));
	
	_galleryName = galleryName;
	_galleryID = galleryID;
    }
    
    private String _galleryID;
    
    /**
     * Returns the associated gallery identifier.
     * @return	The associated gallery identifier.
     */
    public String getGalleryID()
    {
	return _galleryID;
    }
    
    private String _galleryName;
    
    /**
     * Returns the name of this gallery.
     * @return	The name of this gallery.
     */
    public String getGalleryName()
    {
	return _galleryName;
    }
    
    /**
     * Returns the string representation of this <tt>GalleryDescriptor</tt>.
     * @return	String representation of this <tt>GalleryDescriptor</tt>.
     */
    @Override
    public String toString()
    {
	return _galleryName;
    }
}
