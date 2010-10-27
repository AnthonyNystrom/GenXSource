/* ------------------------------------------------
 * ThumbnailItem.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader.common;

import java.awt.*;
import java.awt.event.*;
import java.awt.image.BufferedImage;
import java.io.File;
import java.util.ArrayList;
import java.util.List;
import java.util.logging.Level;
import java.util.logging.LogManager;
import java.util.logging.Logger;
import javax.swing.JCheckBox;
import javax.swing.JPanel;
import n2f.mediauploader.*;
import n2f.mediauploader.drawing.GrfxUtils;
import n2f.mediauploader.drawing.RotateType;
import n2f.mediauploader.resources.AppletResources;
import static n2f.mediauploader.resources.ExceptionResources.*;

/**
 * @author Alex Nesterov
 */
public class ThumbnailItem
	extends JPanel
{
    private static final Logger _logger =
	    Logger.getLogger(ThumbnailItem.class.getName());

    static
    {
	LogManager.getLogManager().addLogger(_logger);
    }

    /** 
     * Abstract implementation of the button that is used to rotate thumbnail
     * image.
     */
    private abstract class AbstractRotateButton
	    extends JPanel
    {
	private class RotateButtonMouseListener
		extends MouseListenerAdapter
	{
	    @Override
	    public void mouseClicked(MouseEvent e)
	    {
		ActionEvent event = new ActionEvent(AbstractRotateButton.this,
						    e.getID(),
						    "mouseClick");
		for (final ActionListener l : _actionListeners)
		    l.actionPerformed(event);
	    }

	    @Override
	    public void mouseEntered(MouseEvent e)
	    {
		_currentImage = _hotImage;
		repaint();
	    }

	    @Override
	    public void mouseExited(MouseEvent e)
	    {
		_currentImage = _normalImage;
		repaint();
	    }

	}

	private Image _currentImage;
	private Image _normalImage;
	private Image _hotImage;

	/**
	 * Creates a new instance of the <tt>ThumbnailItem</tt> class.
	 * 
	 * @param   normalImage
	 *	    Specifies the image for normal button state.
	 * @param   hotImage
	 *	    Specifies the image for hot button state.
	 * 
	 * @throws  IllegalArgumentException
	 *	    If the specified <tt>defaultImage</tt> is <code>null</code>, or
	 *	    if the specified <tt>hotImage</tt> is <code>null</code>.
	 */
	public AbstractRotateButton(Image normalImage, Image hotImage)
	{
	    if (normalImage == null)
		throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
								 "normalImage"));
	    if (hotImage == null)
		throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
								 "hotImage"));

	    _normalImage = normalImage;
	    _hotImage = hotImage;
	    _currentImage = _normalImage;

	    addMouseListener(new RotateButtonMouseListener());
	    Dimension rotateButtonDimension = new Dimension(32, 32);
	    setMinimumSize(rotateButtonDimension);
	    setOpaque(false);
	    setPreferredSize(rotateButtonDimension);
	}

	private List<ActionListener> _actionListeners;

	/**
	 * Adds the specified listener to receive action events from this 
	 * component.
	 * @param   l
	 *	    Specifies the action listener.
	 * @throws  IllegalArgumentException
	 *	    If the specified <tt>l</tt> is <code>null</code>.
	 */
	public void addActionListener(ActionListener l)
	{
	    if (l == null)
		throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
								 "l"));
	    if (_actionListeners == null)
		_actionListeners = new ArrayList<ActionListener>();

	    _actionListeners.add(l);
	}

	@Override
	public void paintComponent(Graphics g)
	{
	    int width = getWidth();
	    int height = getHeight();

	    int imageWidth = _currentImage.getWidth(null);
	    int imageHeight = _currentImage.getHeight(null);

	    int x = (width - imageWidth) / 2;
	    int y = (height - imageHeight) / 2;

	    _logger.log(Level.FINE, "width = {0}", width);
	    _logger.log(Level.FINE, "height = {0}", height);
	    _logger.log(Level.FINE, "x = {0}", x);
	    _logger.log(Level.FINE, "y = {0}", y);

	    g.drawImage(_currentImage, x, y, null);
	}

    }

    private class ContentPanel
	    extends LineBorderPanel
    {
	public ContentPanel(LayoutManager layoutManager)
	{
	    super(layoutManager);
	}

	private BufferedImage _image;

	public BufferedImage getImage()
	{
	    return _image;
	}

	public void setImage(BufferedImage image)
	{
	    _image = image;
	    repaint();
	}

	@Override
	public void paintComponent(Graphics g)
	{
	    super.paintComponent(g);

	    Image currentImage = _image != null
		    ? _image
		    : AppletResources.ThumbnailEmptyImage;

	    int width = getWidth();
	    int height = getHeight();

	    int imageWidth = currentImage.getWidth(null);
	    int imageHeight = currentImage.getHeight(null);
	    
	    Dimension imageSize = new Dimension(imageWidth, imageHeight);
	    Rectangle targetRect = new Rectangle(0, 0, width, height);
	    Rectangle destRect = GrfxUtils.scaleToFit(targetRect, imageSize);

	    g.drawImage(currentImage,
			destRect.x, destRect.y,
			destRect.width, destRect.height,
			null);
	}

    }

    private class RotateCWButton
	    extends AbstractRotateButton
    {
	public RotateCWButton()
	{
	    super(AppletResources.RotateCW_Normal, AppletResources.RotateCW_Hot);
	}

    }

    private class RotateCCWButton
	    extends AbstractRotateButton
    {
	public RotateCCWButton()
	{
	    super(AppletResources.RotateCCW_Normal,
		  AppletResources.RotateCCW_Hot);
	}

    }

    private class CheckBoxItemListener
	    implements ItemListener
    {
	public void itemStateChanged(ItemEvent e)
	{
	    int stateChange = e.getStateChange();

	    if (stateChange == ItemEvent.SELECTED)
		setSelected(true);
	    else if (stateChange == ItemEvent.DESELECTED)
		setSelected(false);
	}

    }

    private class ContentPanelMouseListener
	    extends MouseListenerAdapter
    {
	@Override
	public void mouseEntered(MouseEvent e)
	{
	    _rotateCWButton.setVisible(true);
	    _rotateCCWButton.setVisible(true);
	}

	@Override
	public void mouseExited(MouseEvent e)
	{
	    if (!_contentPanel.contains(e.getPoint()))
	    {
		_rotateCWButton.setVisible(false);
		_rotateCCWButton.setVisible(false);
	    }
	}

    }

    private class CreateThumbnailCallback
	    implements ICreateThumbnailCallback
    {
	public void createThumbnailFailed(String message)
	{
	    _logger.log(Level.INFO, message);
	}

	public void thumbnailCreated(BufferedImage thumbnail)
	{
	    _logger.log(Level.INFO, "Thumbnail created successfully.");
	    _contentPanel.setImage(thumbnail);
	    _associatedLeaf.set(MediaUploaderModel.THUMBNAIL_PROPERTY, thumbnail);
	}

    }

    private class RotateButtonActionListener
	    implements ActionListener
    {
	public void actionPerformed(ActionEvent e)
	{
	    AbstractRotateButton button = (AbstractRotateButton)e.getSource();
	    RotateType rotateType = null;
	    ThumbnailEventType eventType = null;

	    if (button == _rotateCWButton)
	    {
		rotateType = RotateType.Rotate90;
		eventType = ThumbnailEventType.Rotated90;
	    }
	    else if (button == _rotateCCWButton)
	    {
		rotateType = RotateType.Rotate270;
		eventType = ThumbnailEventType.Rotated270;
	    }

	    BufferedImage image = _contentPanel.getImage();

	    if (eventType != null && image != null && rotateType != null)
	    {
		_contentPanel.setImage(
			GrfxUtils.rotateImage(_contentPanel.getImage(),
					      rotateType));
		invokeThumbnailEvent(eventType);
	    }
	}

    }

    private class RotateButtonMouseListener
	    extends MouseListenerAdapter
    {
	@Override
	public void mouseExited(MouseEvent e)
	{
	    _rotateCWButton.setVisible(false);
	    _rotateCCWButton.setVisible(false);
	}

    }

    private IPhotoProcessor _photoProcessor;
    private JCheckBox _checkBox;
    private ContentPanel _contentPanel;
    private RotateCWButton _rotateCWButton;
    private RotateCCWButton _rotateCCWButton;

    /**
     * Creates a new instance of the <tt>ThumbnailItem</tt> class.
     * 
     * @param	thumbnailDimension
     *		Specifies the width and height for this ThumbnailItem.
     * @param	associatedLeaf
     * @throws	IllegalArgumentException
     *		If the specified <tt>thumbnailDimension</tt> is <code>null</code>, or
     *		if the specified <tt>associatedLeaf</tt> is <code>null</code>, or
     *		if the specified <tt>photoProcessor</tt> is <code>null</code>.
     */
    public ThumbnailItem(final Dimension thumbnailDimension,
			  final KeyValuePair<File> associatedLeaf,
			  final IPhotoProcessor photoProcessor)
    {
	super(new BorderLayout());

	if (thumbnailDimension == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "thumbnailDimension"));
	if (associatedLeaf == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "associatedLeaf"));
	if (photoProcessor == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "photoProcessor"));

	_associatedLeaf = associatedLeaf;
	_photoProcessor = photoProcessor;

	_checkBox = new JCheckBox(associatedLeaf.key);
	_checkBox.addItemListener(new CheckBoxItemListener());

	_rotateCWButton = new RotateCWButton();
	_rotateCCWButton = new RotateCCWButton();

	JPanel toolbar = new JPanel(new BorderLayout());
	toolbar.add(_rotateCWButton, BorderLayout.EAST);
	toolbar.add(_rotateCCWButton, BorderLayout.WEST);
	toolbar.setOpaque(false);

	_contentPanel = new ContentPanel(new BorderLayout());
	_contentPanel.add(toolbar, BorderLayout.SOUTH);
	_contentPanel.addMouseListener(new ContentPanelMouseListener());
	_contentPanel.setHotBorderColor(AppletColorScheme.ThumbnailHotBorderColor);
	_contentPanel.setNormalBorderColor(AppletColorScheme.ThumbnailNormalBorderColor);
	_contentPanel.setSelectedBorderColor(AppletColorScheme.ThumbnailSelectedBorderColor);

	ActionListener rotateButtonActionListener =
		new RotateButtonActionListener();
	MouseListener rotateButtonMouseListener =
		new RotateButtonMouseListener();

	for (final AbstractRotateButton rotateButton : new AbstractRotateButton[] {
	    _rotateCWButton,
	    _rotateCCWButton
	})
	{
	    _contentPanel.considerTransparent(rotateButton);
	    rotateButton.addActionListener(rotateButtonActionListener);
	    rotateButton.addMouseListener(rotateButtonMouseListener);
	    rotateButton.setSize(32, 32);
	    rotateButton.setOpaque(false);
	    rotateButton.setVisible(false);
	}

	add(_checkBox, BorderLayout.SOUTH);
	add(_contentPanel, BorderLayout.CENTER);
	setThumbnailDimension(thumbnailDimension);
	_photoProcessor.createThumbnail(associatedLeaf.value,
					thumbnailDimension,
					new CreateThumbnailCallback());
    }

    /* ------ AssociatedLeaf ------ */
    private KeyValuePair<File> _associatedLeaf;

    /**
     * Gets the leaf associated with this thumbnail during initialization. It
     * is an instance that encapsulates thumbnail related data such as image
     * file and short user-friendly name.
     * 
     * @return	The leaf associated with this thumbnail durin initialization.
     */
    public KeyValuePair<File> getAssociatedLeaf()
    {
	return _associatedLeaf;
    }

    /* ------ Selected ------ */
    private boolean _isSelected;

    /**
     * Gets the value indicating whether this thumbnail is selected.
     * @return	<code>true</code> if this thumbnail is selected;
     *		<code>false</code> otherwise.
     */
    public boolean isSelected()
    {
	return _isSelected;
    }

    /**
     * Sets the value indicating whether this thumbnail is selected.
     * @param	value
     *		Specify <code>true</code> to select this thumbnail;
     *		<code>false</code> otherwise.
     */
    public void setSelected(boolean value)
    {
	if (_isSelected != value)
	{
	    _isSelected = value;
	    _contentPanel.setSelected(_isSelected);
	    _checkBox.setSelected(_isSelected);
	    invokeThumbnailEvent(_isSelected
				 ? ThumbnailEventType.Selected
				 : ThumbnailEventType.Deselected);
	}
    }

    /* ------ ThumbnailDimension ------ */
    private Dimension _thumbnailDimension;

    /**
     * Gets the width and height for this thumbnail.
     * @return	The width and height for this thumbnail.
     */
    public Dimension getThumbnailDimension()
    {
	return _thumbnailDimension;
    }

    /**
     * Sets the width and height for this thumbnail.
     * @param	value
     *		Specifies the width and height for this thumbnail.
     * @throws	IllegalArgumentException
     *		If the specified <tt>value</tt> is <code>null</code>.
     */
    public void setThumbnailDimension(Dimension value)
    {
	if (value == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "value"));

	_thumbnailDimension = value;

	setMaximumSize(value);
	setMinimumSize(value);
	setPreferredSize(value);
    }

    /* ------ ThumbnailListeners ------ */
    private List<IThumbnailListener> _thumbnailListeners;

    private List<IThumbnailListener> getThumbnailListeners()
    {
	if (_thumbnailListeners == null)
	    _thumbnailListeners = new ArrayList<IThumbnailListener>();
	return _thumbnailListeners;
    }

    /**
     * Adds the specified thumbnail listener to receive thumbnail related
     * events from this component.
     * @param	l
     *		Specifies the thumbnail listener to receive thumbnail related
     *		events from this component.
     * @throws	IllegalArgumentException
     *		If the specified <tt>l</tt> is <code>null</code>.
     */
    public void addThumbnailListener(IThumbnailListener l)
    {
	if (l == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "l"));

	getThumbnailListeners().add(l);
    }

    /**
     * Removes the specified thumbnail listener so that is no longer receives
     * thumbnail related events from this component.
     * @param	l
     *		Specifies the thumbnail listener to remove.
     */
    public void removeThumbnailListener(IThumbnailListener l)
    {
	getThumbnailListeners().remove(l);
    }

    private void invokeThumbnailEvent(ThumbnailEventType eventType)
    {
	ThumbnailEvent event = new ThumbnailEvent(this, eventType);
	for (final IThumbnailListener l : getThumbnailListeners())
	    l.thumbnailItemChanged(event);
    }

}
