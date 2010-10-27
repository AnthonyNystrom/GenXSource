/* ------------------------------------------------
 * GalleryModel.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import static n2f.mediauploader.resources.ExceptionResources.*;
import static n2f.mediauploader.resources.GalleryModelResources.*;

/**
 * Encapsulates N2F PhotoOrganise Web-Service galleries.
 * @author Alex Nesterov
 */
public final class GalleryModel
{
    private List<GalleryDescriptor> _galleries;

    /**
     * Creates a new instance of the <tt>GalleryModel</tt> class.
     */
    public GalleryModel()
    {
	_galleries = new ArrayList<GalleryDescriptor>();
    }

    /**
     * Adds a new gallery to the model with the specified name and ID.
     * 
     * @param	galleryName
     *		Specifies the name for the gallery.
     * @param	galleryID
     *		Specifies the ID for the gallery.
     * @throws	IllegalArgumentException
     *		If the specified <tt>galleryName</tt> is <code>null</code>, or
     *		if the specified <tt>galleryID</tt> is <code>null</code>.
     */
    public void addGallery(String galleryName, String galleryID)
    {
	addGallery(new GalleryDescriptor(galleryName, galleryID));
    }

    /**
     * Adds a new gallery to the model.
     * @param	gallery
     *		Specifies the gallery to add to the model.
     * @throws	IllegalArgumentException
     *		If the specified <tt>gallery</tt> is <code>null</code>.
     */
    public void addGallery(GalleryDescriptor gallery)
    {
	if (gallery == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "gallery"));
	_galleries.add(gallery);

	if (_currentGallery == null)
	    _currentGallery = gallery;

	invokeGalleryEvent(GalleryEventType.NewGalleryAdded, gallery);
    }

    /**
     * Adds a set of galleries to the model. The method assumes both arrays are
     * of equal length. Also each gallery name from the <tt>galleryNames</tt>
     * array is assoicated with an appropriate ID item at the same index in the
     * <tt>galleryIDs</tt> array.
     * @param	galleryNames
     *		Specifies a set of names for new galleries.
     * @param	galleryIDs
     *		Specifies a set of IDs for new galleries.
     * @throws	IllegalArgumentException
     *		If the specified <tt>galleryNames</tt> is <code>null</code>, or
     *		if the specified <tt>galleryIDs</tt> is <code>null</code>, or
     *		if the specified arrays have different length.
     */
    public void addGalleryMany(String[] galleryNames, String[] galleryIDs)
    {
	if (galleryNames == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "galleryNames"));
	if (galleryIDs == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "galleryIDs"));

	if (galleryNames.length != galleryIDs.length)
	    throw new IllegalArgumentException(String.format(ArrayLengthNotEqual,
							     "galleryNames",
							     "galleryIDs"));

	for (int i = 0; i < galleryNames.length; i++)
	    addGallery(galleryNames[i], galleryIDs[i]);
    }

    private List<IGalleryModelListener> _galleryModelListeners;

    private List<IGalleryModelListener> getGalleryModelListeners()
    {
	if (_galleryModelListeners == null)
	    _galleryModelListeners = new ArrayList<IGalleryModelListener>();
	return _galleryModelListeners;
    }

    /**
     * Adds the specified listener to receive <tt>GalleryModel</tt> related
     * events from this component.
     * @param	l
     *		Specifies the listener to add.
     * @throws	IllegalArgumentException
     *		If the specified <tt>l</tt> is <code>null</code>.
     */
    public void addGalleryModelListener(IGalleryModelListener l)
    {
	if (l == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "l"));
	getGalleryModelListeners().add(l);
    }

    /**
     * Removes the specified listener so that it no longer receives
     * <tt>GalleryModel</tt> related events from this component.
     * @param	l
     *		Specifies the listener to remove.
     */
    public void removeGalleryModelListener(IGalleryModelListener l)
    {
	getGalleryModelListeners().remove(l);
    }

    private void invokeGalleryEvent(GalleryEventType eventType,
				     GalleryDescriptor descriptor)
    {
	GalleryEvent event = new GalleryEvent(this, eventType, descriptor);

	for (final IGalleryModelListener l : getGalleryModelListeners())
	    l.galleryModelChanged(event);
    }

    /**
     * Returns all currently associated galleries.
     * @return	All currently associated galleries.
     */
    public List<GalleryDescriptor> getGalleries()
    {
	return Collections.unmodifiableList(_galleries);
    }

    private GalleryDescriptor _currentGallery;

    /**
     * Returns the currently selected gallery.
     * @return	Currently selected gallery.
     */
    public GalleryDescriptor getCurrentGallery()
    {
	return _currentGallery;
    }

    /**
     * Sets the currently selected gallery. This value can change upon combo box
     * selected item change for example.
     * @param	currentGallery
     *		Specifies the currently selected gallery.
     * @throws	IllegalArgumentException
     *		If the specified <tt>currentGallery</tt> is <code>null</code>.
     */
    public void setCurrentGallery(GalleryDescriptor currentGallery)
    {
	if (currentGallery == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "currentGallery"));
	if (!_galleries.contains(currentGallery))
	    throw new IllegalArgumentException(String.format(GalleryDoesNotExist, "currentGallery"));
	
	_currentGallery = currentGallery;
    }

    private GalleryDescriptor _defaultGallery;

    /**
     * Gets the gallery that was specified via <tt>defaultGalleryID</tt> param in
     * HTML markup.
     * @return	Default gallery.
     */
    public GalleryDescriptor getDefaultGallery()
    {
	return _defaultGallery;
    }

    /**
     * Sets the gallery with the specified ID as default. The gallery should
     * already be added to the model before this method is called.
     * @param	defaultGalleryID
     *		Specifies the ID of the default gallery.
     * @throws	IllegalArgumentException
     *		If the specified <tt>defaultGalleryID</tt> is <code>null</code>, or
     *		if the gallery with the specified <tt>defaultGalleryID</tt> was
     *		not found in the model.
     */
    public void setDefaultGallery(String defaultGalleryID)
    {
	if (defaultGalleryID == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "defaultGalleryID"));

	for (final GalleryDescriptor gallery : _galleries)
	{
	    if (gallery.getGalleryID().equals(defaultGalleryID))
	    {
		_defaultGallery = gallery;
		_currentGallery = _defaultGallery;
		invokeGalleryEvent(GalleryEventType.DefaultGalleryChanged,
				   _defaultGallery);
		return;
	    }
	}

	throw new IllegalArgumentException(String.format(GalleryIDNotMapped,
							 defaultGalleryID));
    }

}
