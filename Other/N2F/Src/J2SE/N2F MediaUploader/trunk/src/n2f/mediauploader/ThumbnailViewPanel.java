/* ------------------------------------------------
 * ThumbnailViewPanel.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

import java.awt.Component;
import java.awt.Dimension;
import java.io.File;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.logging.LogManager;
import java.util.logging.Logger;
import javax.swing.JPanel;
import n2f.mediauploader.common.*;
import static n2f.mediauploader.resources.ExceptionResources.*;

/**
 * Represents a panel that displays thumbnails.
 * 
 * @author Alex Nesterov
 */
final class ThumbnailViewPanel
	extends JPanel
{
    private static Logger _logger =
	    Logger.getLogger(ThumbnailViewPanel.class.getName());

    static
    {
	LogManager.getLogManager().addLogger(_logger);
    }

    private Dimension _thumbnailDimension;
    private IPhotoProcessor _photoProcessor;
    private List<KeyValuePair<File>> _selectedLeafs;
    private Map<KeyValuePair<File>, ThumbnailItem> _leafThumbnailMap;

    /**
     * Creates a new instance of the <tt>ThumbnailViewPanel</tt> class.
     * 
     * @param	thumbnailDimension
     *		Specifies the dimension for thumbnail items.
     * @throws	IllegalArgumentException
     *		If the specified <tt>thumbnailDimension</tt> is <code>null</code>, or
     *		if the specified <tt>photoProcessor</tt> is <code>null</code>.
     */
    public ThumbnailViewPanel(Dimension thumbnailDimension,
			       IPhotoProcessor photoProcessor)
    {
	if (thumbnailDimension == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "thumbnailDimension"));
	if (photoProcessor == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "photoProcessor"));

	_thumbnailDimension = thumbnailDimension;
	_photoProcessor = photoProcessor;
	_leafThumbnailMap = new HashMap<KeyValuePair<File>, ThumbnailItem>();
	_selectedLeafs = new ArrayList<KeyValuePair<File>>();

	setLayout(new ThumbnailFlowLayout(ThumbnailFlowLayout.LEADING));
    }

    private IThumbnailListener _thumbnailListener;

    /**
     * Sets the specified thumbnail listener to receive thumbnail related
     * events from this component.
     * @param	l
     *		Specifies the thumbnail listener to receive thumbnail related
     *		events from this component. <code>null</code> to remove the
     *		listener.
     */
    public void setThumbnailListener(IThumbnailListener l)
    {
	if (_thumbnailListener != l)
	{
	    IThumbnailListener oldThumbnailListener = _thumbnailListener;
	    _thumbnailListener = l;
	    
	    for (final Component component : getComponents())
	    {
		if (component instanceof ThumbnailItem)
		{
		    ThumbnailItem thumbnail = (ThumbnailItem)component;
		    thumbnail.removeThumbnailListener(oldThumbnailListener);
		    thumbnail.addThumbnailListener(_thumbnailListener);
		}
	    }
	}
    }

    /**
     * Returns the list of selected leafs.
     * @return Selected leafs.
     */
    public List<KeyValuePair<File>> getSelectedLeafs()
    {
	return _selectedLeafs;
    }

    /**
     * Sets the current entries to show. The current contents of the panel are
     * discarded.
     * @param	leafs
     *		Specifies the entries to show on the panel.
     */
    public void setFolder(final List<KeyValuePair<File>> leafs)
    {
	_photoProcessor.cancelCreateThumbnail();
	
	for (final Component component : getComponents())
	{
	    if (component instanceof ThumbnailItem)
	    {
		ThumbnailItem thumbnail = (ThumbnailItem)component;
		thumbnail.removeThumbnailListener(_thumbnailListener);
	    }
	}
	
	removeAll();

	_leafThumbnailMap.clear();
	_selectedLeafs.clear();

	for (final KeyValuePair<File> leaf : leafs)
	{
	    ThumbnailItem thumbnailItem = new ThumbnailItem(_thumbnailDimension,
							    leaf,
							    _photoProcessor);
	    
	    if (_thumbnailListener != null)
		thumbnailItem.addThumbnailListener(_thumbnailListener);
	    _leafThumbnailMap.put(leaf, thumbnailItem);
	    add(thumbnailItem);
	}

	doLayout();
	updateUI();
    }
    
    /**
     * Selects all thumbnail items.
     */
    public void selectAll()
    {
	for (final ThumbnailItem thumbnail : _leafThumbnailMap.values())
	{
	    thumbnail.setSelected(true);
	    KeyValuePair<File> leaf = thumbnail.getAssociatedLeaf();
	    if (!_selectedLeafs.contains(leaf))
		_selectedLeafs.add(leaf);
	}
    }
    
    /**
     * Removes selection from all thumbnanil items.
     */
    public void selectNone()
    {
	_selectedLeafs.clear();
	for (final ThumbnailItem thumbnail : _leafThumbnailMap.values())
	    thumbnail.setSelected(false);
    }

    /**
     * Selects or deselects the thumbnail item associated with the specified <tt>leaf</tt>.
     * @param	leaf
     *		Specifies the leaf to determine the thumbnail item to select or deselect.
     * @param	value
     *		<code>true</code> to select the thumbnail item;
     *		<code>false</code> otherwise.
     */
    public void setSelected(KeyValuePair<File> leaf, boolean value)
    {
	if (_leafThumbnailMap.containsKey(leaf))
	{
	    _leafThumbnailMap.get(leaf).setSelected(value);

	    if (value)
		if (!_selectedLeafs.contains(leaf))
		    _selectedLeafs.add(leaf);
		else
		    _selectedLeafs.remove(leaf);
	}
    }

}
