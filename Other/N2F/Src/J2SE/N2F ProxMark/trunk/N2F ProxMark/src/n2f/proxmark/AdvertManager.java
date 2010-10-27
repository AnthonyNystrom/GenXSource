/* ------------------------------------------------
 * AdvertManager.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark;

import java.awt.Dimension;
import java.beans.PropertyChangeListener;
import java.beans.PropertyChangeSupport;
import java.util.ArrayList;
import java.util.List;
import java.util.logging.LogManager;
import java.util.logging.Logger;
import n2f.proxmark.AdvertListEvent.EventType;
import static genetibase.util.resources.ExceptionResources.*;

/**
 * Keeps a list of advertisements and allows to add new advertisements to the list
 * and remove specified advertisements from the list. In addition fires events
 * when the state of the list changes.
 * 
 * @author Alex Nesterov
 */
final class AdvertManager
{
    /** Occurs when the return value of the <tt>canCreateAdvert</tt> method changes. */
    public static final String CAN_CREATE_ADVERT_CHANGED =
	    "AdManager.canCreateAdvert";
    /** Occurs when the return value of the <tt>canDeleteAdvert</tt> method changes. */
    public static final String CAN_DELETE_ADVERT_CHANGED =
	    "AdManager.canDeleteAdvert";
    /** Occurs when the current advertisement changes. */
    public static final String CURRENT_ADVERT_CHANGED =
	    "AdManager.currentAdChanged";
    private static final Logger _logger =
	    Logger.getLogger(AdvertManager.class.getName());

    static
    {
	LogManager.getLogManager().addLogger(_logger);
    }

    private List<IAdvertListListener> _advertListListeners;
    private List<Advert> _advertList;
    private PropertyChangeSupport _changeSupport;

    /**
     * Creates a new instance of the <tt>AdvertManager</tt> class.
     * @param	thumbnailMaximumSize
     *		Specifies the maximum size for the thumbnail image to appear
     *		in advertisements.
     * @throws	IllegalArgumentException
     *		If the specified <tt>thumbnailMaximumSize</tt> is <code>null</code>.
     */
    public AdvertManager(Dimension thumbnailMaximumSize)
    {
	if (thumbnailMaximumSize == null)
	    throw new IllegalArgumentException(
		    String.format(ArgumentCannotBeNull, "thumbnailMaximumSize"));
	_advertListListeners = new ArrayList<IAdvertListListener>();
	_advertList = new ArrayList<Advert>();
	_changeSupport = new PropertyChangeSupport(this);
	_thumbnailMaximumSize = thumbnailMaximumSize;
	setCanCreateAdvert(true);
    }

    public void addAdvertListListener(IAdvertListListener l)
    {
	if (l == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "l"));
	_advertListListeners.add(l);
    }

    public void removeAdvertListListener(IAdvertListListener l)
    {
	_advertListListeners.remove(l);
    }

    private void invokeAdvertListChangedEvent(EventType eventType,
					   Advert adParamsModel)
    {
	AdvertListEvent event = new AdvertListEvent(this, eventType, adParamsModel);
	for (final IAdvertListListener l : _advertListListeners)
	    l.advertListChanged(event);
    }

    /**
     * Adds the specified listener to receive property change events from this component.
     * @param	l
     *		Specifies the property change listener to add.
     * @throws	IllegalArgumentException
     *		If the specified <tt>l</tt> is <code>null</code>.
     */
    public void addPropertyChangeListener(PropertyChangeListener l)
    {
	if (l == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "l"));
	_changeSupport.addPropertyChangeListener(l);
    }

    /**
     * Removes the specified listener so that it no longer receives property
     * change events from this component.
     * @param	l
     *		Specifies the property change listener to remove.
     */
    public void removePropertyChangeListener(PropertyChangeListener l)
    {
	_changeSupport.removePropertyChangeListener(l);
    }

    private boolean _canCreateAdvert;

    /**
     * Returns the value indicating whether this <tt>AdvertManager</tt> can
     * create new advertisement and add it to the list.
     * @return	The value indicating whether this <tt>AdvertManager</tt> can
     *		create new advertisement and add it to the list.
     */
    public boolean canCreateAdvert()
    {
	return _canCreateAdvert;
    }

    private void setCanCreateAdvert(boolean value)
    {
	boolean oldValue = _canCreateAdvert;
	_canCreateAdvert = value;
	_changeSupport.firePropertyChange(CAN_CREATE_ADVERT_CHANGED,
					  oldValue,
					  value);
    }

    private boolean _canDeleteAdvert;

    /**
     * Returns the value indicating whether this <tt>AdvertManager</tt> can
     * delete any advertisement from the list.
     * @return	The value indicating whether this <tt>AdvertManager</tt> can
     *		delete any advertisement from the list.
     */
    public boolean canDeleteAdvert()
    {
	return _canDeleteAdvert;
    }

    private void setCanDeleteAdvert(boolean value)
    {
	boolean oldValue = _canDeleteAdvert;
	_canDeleteAdvert = value;
	_changeSupport.firePropertyChange(CAN_DELETE_ADVERT_CHANGED,
					  oldValue,
					  value);
    }

    /**
     * Creates a new advertisement, adds it to the list of advertisements and
     * returns the created instance.
     * @return	Created <tt>Advert</tt> instance.
     */
    public Advert createAdvert()
    {
	Advert model = new Advert(getThumbnailMaximumSize());
	_advertList.add(model);
	setCurrentAdvert(model);
	invokeAdvertListChangedEvent(EventType.Created, _currentAdvert);
	return model;
    }

    /**
     * Removes the specified advertisement from the list of advertisements.
     * @param	advertToDelete
     *		Specifies the advertisement to remove from the list of advertisements.
     */
    public void deleteAdvert(Advert advertToDelete)
    {
	if (advertToDelete != null)
	{
	    _advertList.remove(advertToDelete);

	    if (!_advertList.isEmpty())
	    {
		List<Advert> paramsModelList = _advertList;
		setCurrentAdvert(paramsModelList.get(paramsModelList.size() - 1));
	    }
	    else
	    {
		setCurrentAdvert(null);
	    }
	    
	    invokeAdvertListChangedEvent(EventType.Deleted, advertToDelete);
	}
    }

    private Advert _currentAdvert;

    /**
     * Gets the currently selected advertisement.
     * @return	Currently selected advertisement.
     */
    public Advert getCurrentAdvert()
    {
	return _currentAdvert;
    }

    /**
     * Sets the currently selected advertisement.
     * @param	value
     *		Specifies the currently selected advertisement. Can be <code>null</code>.
     */
    public void setCurrentAdvert(Advert value)
    {
	if (_currentAdvert != value)
	{
	    Advert oldValue = _currentAdvert;
	    _currentAdvert = value;
	    _changeSupport.firePropertyChange(CURRENT_ADVERT_CHANGED,
					      oldValue,
					      value);

	    setCanDeleteAdvert(_currentAdvert != null);
	}
    }

    private Dimension _thumbnailMaximumSize;

    /**
     * Gets the maximum size for the thumbnail to appear in the advertisement.
     * @return	Maximum size for the thumbnail to appear in the advertisement.
     */
    public Dimension getThumbnailMaximumSize()
    {
	return _thumbnailMaximumSize;
    }

    /**
     * Sets the maximum size for the thumbnail to appear in the advertisement.
     * @param	value
     *		Specifies the maximum size for the thumbnail to appear in the advertisement.
     * @throws	IllegalArgumentException
     *		If the specified <tt>value</tt> is <code>null</code>.
     */
    public void setThumbnailMaximumSize(Dimension value)
    {
	if (value == null)
	    throw new IllegalArgumentException(
		    String.format(ArgumentCannotBeNull, "value"));
	_thumbnailMaximumSize = value;
    }

}
