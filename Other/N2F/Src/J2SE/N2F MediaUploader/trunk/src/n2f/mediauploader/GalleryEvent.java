/* ------------------------------------------------
 * GalleryEvent.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

import java.util.EventObject;
import static n2f.mediauploader.resources.ExceptionResources.*;

/**
 * Encapsulates GalleryModel events related data.
 * @author Alex Nesterov
 */
public final class GalleryEvent
	extends EventObject
{
    /**
     * Creates a new instance of the <tt>GalleryEvent</tt> class.
     * @param	source
     *		Specifies the source of this event.
     * @param	eventType
     *		Specifies the type of this event.
     * @param	galleryDescriptor
     *		Specifies the associated gallery descriptor.
     * @throws	IllegalArgumentException
     *		If the specified <tt>source</tt> is <code>null</code>, or
     *		if the specified <tt>eventType</tt> is <code>null</code>, or
     *		if the specified <tt>galleryDescriptor</tt> is <code>null</code>.
     */
    public GalleryEvent(Object source, GalleryEventType eventType,
			 GalleryDescriptor galleryDescriptor)
    {
	super(source);

	if (eventType == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "eventType"));
	if (galleryDescriptor == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "galleryDescriptor"));

	_eventType = eventType;
	_galleryDescriptor = galleryDescriptor;
    }

    private GalleryEventType _eventType;

    /**
     * Gets the type of this event.
     * @return	The type of this event.
     */
    public GalleryEventType getEventType()
    {
	return _eventType;
    }

    private GalleryDescriptor _galleryDescriptor;

    /**
     * Gets the associated gallery descriptor.
     * @return	The associated gallery descriptor.
     */
    public GalleryDescriptor getGalleryDescriptor()
    {
	return _galleryDescriptor;
    }

    /**
     * Checks this <tt>GalleryEvent</tt> instance and the specified object for equality.
     * @param	o
     *		Specifies the object to check for equality.
     * @return	<code>true</code> if the specified object is an instance of the
     *		<tt>GalleryEvent</tt> class and all the fields are equal;
     *		<code>false</code> otherwise.
     */
    @Override
    public boolean equals(Object o)
    {
	if (o == null)
	    return false;

	if (o instanceof GalleryEvent)
	{
	    GalleryEvent event = (GalleryEvent)o;
	    if (event.getEventType() == getEventType() && event.getSource() == getSource())
		return event.getGalleryDescriptor() == getGalleryDescriptor();
	}
	
	return false;
    }

    @Override
    public int hashCode()
    {
	int hash = 3;

	hash = 53 * hash + (_eventType != null
		? _eventType.hashCode()
		: 0);
	hash = 53 * hash + (_galleryDescriptor != null
		? _galleryDescriptor.hashCode()
		: 0);

	return hash;
    }

}
