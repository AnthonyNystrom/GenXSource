/* ------------------------------------------------
 * ThumbnailEvent.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader.common;

import java.util.EventObject;
import static n2f.mediauploader.resources.ExceptionResources.*;

/**
 * Encapsulates thumbnail event related data.
 * @author Alex Nesterov
 */
public class ThumbnailEvent
	extends EventObject
{
    /**
     * Creates a new instance of the <tt>ThumbnailEvent</tt> class.
     * @param	source
     *		Specifies the source of this event.
     * @param	eventType
     *		Specifies the type of this event.
     * @throws	IllegalArgumentException
     *		If the specified <tt>source</tt> is <code>null</code>, or
     *		if the specified <tt>eventType</tt> is <code>null</code>.
     */
    public ThumbnailEvent(Object source, ThumbnailEventType eventType)
    {
	super(source);
	
	if (eventType == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull, eventType));
	
	_eventType = eventType;
    }

    private ThumbnailEventType _eventType;
    
    /**
     * Gets the type of this event.
     * @return	The type of this event.
     */
    public ThumbnailEventType getEventType()
    {
	return _eventType;
    }
}
