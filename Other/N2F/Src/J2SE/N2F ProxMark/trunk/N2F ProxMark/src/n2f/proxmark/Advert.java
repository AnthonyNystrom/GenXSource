/* ------------------------------------------------
 * Advert.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark;

import java.awt.Dimension;
import java.beans.PropertyChangeListener;
import java.beans.PropertyChangeSupport;
import java.util.logging.LogManager;
import java.util.logging.Logger;
import n2f.proxmark.resources.ApplicationResources;
import static genetibase.util.resources.ExceptionResources.*;

/**
 * Encapsulates advertisement related data and tracks its state.
 * @author Alex Nesterov
 */
final class Advert
{
    public static final String IMAGE_PATH_CHANGED =
	    "AdParamsModel.imagePathChanged";
    public static final String THUMBNAIL_MAXSIZE_CHANGED =
	    "AdParamsModel.thumbnailMaxSizeChanged";
    public static final String TEXT_CHANGED = "AdParamsModel.textChanged";
    private static final Logger _logger =
	    Logger.getLogger(Advert.class.getName());

    static
    {
	LogManager.getLogManager().addLogger(_logger);
    }

    private PropertyChangeSupport _changeSupport;

    /**
     * Creates a new instance of the <tt>Advert</tt> class.
     * @param	thumbnailMaximumSize
     *		Specifies the maximum size for the thumbnail image the 
     *		advertisement to contain.
     * @throws	IllegalArgumentException
     *		If the specified <tt>thumbnailMaximumSize</tt> is <code>null</code>.
     */
    public Advert(Dimension thumbnailMaximumSize)
    {
	this("", "", thumbnailMaximumSize);
    }
    
    /**
     * Creates a new instance of the <tt>Advert</tt> class.
     * @param	text
     *		Specifies the text for the advertisement.
     * @param	imagePath
     *		Specifies the path to the image for the advertisement.
     * @param	thumbnailMaximumSize
     *		Specifies the maximum size for the thumbnail image the
     *		advertisement to contain.
     * @throws	IllegalArgumentException
     *		If the specified <tt>text</tt> is <code>null</code>, or
     *		if the specified <tt>imagePath</tt> is <code>null</code>, or
     *		if the specified <tt>thumbnailMaximumSize</tt> is <code>null</code>.
     */
    public Advert(String text, String imagePath, Dimension thumbnailMaximumSize)
    {
	if (text == null)
	    throw new IllegalArgumentException(
		    String.format(ArgumentCannotBeNull, "text"));
	if (imagePath == null)
	    throw new IllegalArgumentException(
		    String.format(ArgumentCannotBeNull, "imagePath"));
	if (thumbnailMaximumSize == null)
	    throw new IllegalArgumentException(
		    String.format(ArgumentCannotBeNull, "thumbnailMaximumSize"));
	
	_text = text;
	_imagePath = imagePath;
	_thumbnailMaximumSize = thumbnailMaximumSize;
	
	_changeSupport = new PropertyChangeSupport(this);
    }

    /**
     * Adds the specified listener to receive property change events from this model.
     * @param	l
     *		Specifies the property change listener to add.
     * @throws	IllegalArgumentException
     *		If the specified <tt>l</tt> is <code>null</code>.
     */
    public void addPropertyChangeListener(PropertyChangeListener l)
    {
	if (l == null)
	    throw new IllegalArgumentException(
		    String.format(ArgumentCannotBeNull, "l"));
	_changeSupport.addPropertyChangeListener(l);
    }

    /**
     * Removes the specified listener so that it no longer receives property
     * change events from this model.
     * @param	l
     *		Specifies the property change listener to remove.
     */
    public void removePropertyChangeListener(PropertyChangeListener l)
    {
	_changeSupport.removePropertyChangeListener(l);
    }

    private String _imagePath;

    /**
     * Gets the current image for the advertisement.
     * @return	Current image path.
     */
    public String getImagePath()
    {
	return _imagePath;
    }

    /**
     * Sets the current image for the advertisement.
     * @param	value
     *		Specifies the current image path.
     * @throws	IllegalArgumentException
     *		If the specified <tt>value</tt> is <code>null</code>.
     */
    public void setImagePath(String value)
    {
	if (value == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "value"));

	if (!value.equals(_imagePath))
	{
	    String previousValue = _imagePath;
	    _imagePath = value;
	    _changeSupport.firePropertyChange(IMAGE_PATH_CHANGED,
					      previousValue,
					      value);
	}
    }

    private String _text;

    /**
     * Gets the current text for the advertisement.
     * @return	Current text for the advertisement.
     */
    public String getText()
    {
	return _text;
    }

    /**
     * Sets the current text for the advertisement.
     * @param	value
     *		Specifies the current text for the advertisement.
     * @throws	IllegalArgumentException
     *		If the specified <tt>value</tt> is <code>null</code>.
     */
    public void setText(String value)
    {
	if (value == null)
	    throw new IllegalArgumentException(
		    String.format(ArgumentCannotBeNull, "value"));

	if (!value.equals(_text))
	{
	    String previousValue = _text;
	    _text = value;
	    _changeSupport.firePropertyChange(TEXT_CHANGED, previousValue, value);
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
     *		Specifies the maximum size for the thumbnail.
     * @throws	IllegalArgumentException
     *		If the specified <tt>value</tt> is <code>null</code>.
     */
    public void setThumbnailMaximumSize(Dimension value)
    {
	if (value == null)
	    throw new IllegalArgumentException(
		    String.format(ArgumentCannotBeNull, "value"));

	if (!value.equals(_thumbnailMaximumSize))
	{
	    Dimension previousValue = _thumbnailMaximumSize;
	    _thumbnailMaximumSize = value;
	    _changeSupport.firePropertyChange(THUMBNAIL_MAXSIZE_CHANGED,
					      previousValue,
					      value);
	}
    }

    /**
     * Returns string representation of this component.
     * @return	String representation of this component.
     */
    @Override
    public String toString()
    {
	String text = getText();
	String imagePath = getImagePath();
	
	if (text != null && !text.equals(""))
	    return text;
	if (imagePath != null && !imagePath.equals(""))
	    return ApplicationResources.AdParams_OnlyImage;
	
	return ApplicationResources.AdParams_Empty;
    }
}
