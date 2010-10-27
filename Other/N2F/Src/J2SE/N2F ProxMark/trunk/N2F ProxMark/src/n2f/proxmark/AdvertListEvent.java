/* ------------------------------------------------
 * AdvertListEvent.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark;

import java.util.EventObject;
import static genetibase.util.resources.ExceptionResources.*;

/**
 * Encapsulates data related to the changes to the advert list.
 * @author Alex Nesterov
 */
final class AdvertListEvent
	extends EventObject
{
    public static enum EventType
    {
	/** Occurs when a new avertisement was created. */
	Created,
	/** Occurs when the advertisement was deleted. */
	Deleted
    }

    /**
     * Creates a new instance of the <tt>AdvertListEvent</tt> class.
     * @param	source
     *		Specifies the source of the event.
     * @param	eventType
     *		Specifies the type of the event.
     * @param	advert
     *		Specifies the target <tt>Advert</tt> instance.
     * @throws	IllegalArgumentException
     *		If the specified <tt>source</tt> is <code>null</code>, or
     *		if the specified <tt>eventType</tt> is <code>null</code>, or
     *		if the specified <tt>advert</tt> is <code>null</code>.
     */
    public AdvertListEvent(Object source, EventType eventType,
			    Advert advert)
    {
	super(source);
	if (advert == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "advert"));
	if (eventType == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "eventType"));
	_advert = advert;
	_eventType = eventType;
    }

    private Advert _advert;

    /**
     * Returns the associated advert.
     * @return	Associated advert.
     */
    public Advert getAdvert()
    {
	return _advert;
    }

    private EventType _eventType;

    /**
     * Returns the type of the event.
     * @return	Type of the event.
     */
    public EventType getEventType()
    {
	return _eventType;
    }

}
