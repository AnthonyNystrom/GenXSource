/* ------------------------------------------------
 * MediaUploaderApplet.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

import java.awt.*;
import java.awt.event.*;
import java.awt.image.BufferedImage;
import java.beans.PropertyChangeEvent;
import java.beans.PropertyChangeListener;
import java.io.File;
import java.lang.Thread.UncaughtExceptionHandler;
import java.util.List;
import java.util.logging.Level;
import java.util.logging.LogManager;
import java.util.logging.Logger;
import javax.swing.*;
import n2f.mediauploader.common.*;
import n2f.mediauploader.common.action.LinkAction;
import n2f.mediauploader.drawing.RotateType;
import n2f.mediauploader.resources.AppletResources;
import n2f.mediauploader.directorytree.DirectoryTreeView;
import n2f.mediauploader.directorytree.IDirectoryTreeListener;
import n2f.mediauploader.util.Environment;
import static n2f.mediauploader.resources.AppletResources.*;

/**
 * MediaUploader for Next2Friends.com user gallery. The following example
 * shows how you can embed the applet into the upload page.
 * <br/>
 * <tt>member-token</tt> identifies the user who is intending to upload images
 * to the gallery.
 * <br/>
 * <code><pre>
 * &lt;applet code="MediaUploaderApplet.class" width="800" height="600"&gt;
 *	&lt;param name="member-token" value="123456789"&gt;
 * &lt;/applet&gt;
 * </pre></code>
 * 
 * @author Alex Nesterov
 */
public class MediaUploaderApplet
	extends JApplet
	implements IParamValueProvider
{
    /** 
     * Represents the main screen that holds all the components like tree-view,
     * thumbnail-view panel and the tool bar.
     */
    private class ImageSelectorScreen
	    extends JSplitPane
    {
	public ImageSelectorScreen()
	{
	    super(JSplitPane.HORIZONTAL_SPLIT);

	    JPanel rightPane = new JPanel(new BorderLayout());
	    rightPane.add(_toolbar, BorderLayout.NORTH);
	    rightPane.add(new JScrollPane(_thumbnailViewPanel),
			  BorderLayout.CENTER);

	    JPanel leftPane = new JPanel(new BorderLayout());
	    leftPane.add(new JScrollPane(_treeView));

	    setLeftComponent(leftPane);
	    setRightComponent(rightPane);
	    setDividerLocation(MediaUploaderApplet.this.getWidth() / 3);
	    setOneTouchExpandable(true);
	}

    }

    /**
     * Holds links for operations on thumbnails and a combo-box populated with
     * gallery names.
     */
    private class Toolbar
	    extends JPanel
    {
	public Toolbar()
	{
	    setLayout(new FlowLayout(FlowLayout.LEFT));

	    Font font = getFont();
	    Font linkFont = new Font(font.getName(), Font.BOLD, font.getSize());

	    for (final Hyperlink hyperlink : new Hyperlink[] { _selectAllLink,
								_selectNoneLink,
								_uploadLink
	    })
	    {
		hyperlink.setClickedColor(AppletColorScheme.HyperlinkColor);
		hyperlink.setUnclickedColor(AppletColorScheme.HyperlinkColor);
		hyperlink.setFont(linkFont);
	    }

	    /** To separate the links visually and make them not like a single line of text. */
	    _selectNoneLink.setBorder(BorderFactory.createEmptyBorder(0, 10,
								      0, 10));

	    add(_selectAllLink);
	    add(_selectNoneLink);
	    add(_uploadLink);
	    add(_galleryComboBox);
	}

    }

    /**
     * All unhanndled exceptions are handled here notifying the user something
     * is wrong with the applet. If you open Java Console you will see the
     * stack trace of the exception.
     */
    private class AppletExceptionHandler
	    implements UncaughtExceptionHandler
    {
	public void uncaughtException(Thread t, Throwable e)
	{
	    if (e instanceof ThreadDeath)
		return;

	    _logger.log(Level.SEVERE, e.getMessage(), e);

	    if (e instanceof Exception)
		showAlertDialog(String.format(AppletResources.UnknownError,
					      Environment.NewLine,
					      e.getMessage()));
	}

    }

    private class DirectoryTreeListener
	    implements IDirectoryTreeListener
    {
	/** Invoked when the user selects a different path on the tree-view. */
	public void pathChanged(List<KeyValuePair<File>> path)
	{
	    _uploaderModel.setFolder(path);
	}

    }

    private class GalleryComboBoxItemListener
	    implements ItemListener
    {
	/** Invoked when the user selects a different item on the combo-box. */
	public void itemStateChanged(ItemEvent e)
	{
	    GalleryDescriptor currentGallery = (GalleryDescriptor)e.getItem();
	    _logger.log(Level.INFO,
			"Setting current gallery --> " + currentGallery.getGalleryName());
	    _galleryModel.setCurrentGallery(currentGallery);
	}

    }

    private class GalleryModelListener
	    implements IGalleryModelListener
    {
	/** Invoked when the state of the gallery model changes. */
	public void galleryModelChanged(GalleryEvent e)
	{
	    GalleryEventType eventType = e.getEventType();

	    switch (eventType)
	    {
		case NewGalleryAdded:
		{
		    GalleryDescriptor descriptor = e.getGalleryDescriptor();
		    _logger.log(Level.INFO,
				"Adding gallery --> " + descriptor.getGalleryName());
		    _galleryComboBox.addItem(descriptor);

		    break;
		}
		case DefaultGalleryChanged:
		{
		    GalleryDescriptor descriptor = e.getGalleryDescriptor();
		    _logger.log(Level.INFO,
				"Default gallery --> " + descriptor.getGalleryName());
		    _galleryComboBox.setSelectedItem(descriptor);

		    break;
		}
	    }
	}

    }

    private class MediaUploaderPropertyChangeListener
	    implements PropertyChangeListener
    {
	public void propertyChange(PropertyChangeEvent e)
	{
	    if (MediaUploaderModel.FILE_LIST_CHANGED_PROPERTY.equals(e.getPropertyName()))
	    {
		_thumbnailViewPanel.setFolder((List<KeyValuePair<File>>)e.getNewValue());
	    }
	}

    }

    private class PhotoProcessorListener
	    implements PropertyChangeListener
    {
	/** PhotoProcessor notifies of the progress. */
	public void propertyChange(PropertyChangeEvent e)
	{
	    String propertyName = e.getPropertyName();

	    if (DefaultPhotoProcessor.PHOTO_UPLOADED.equals(propertyName))
	    {
		_uploadPanel.fileTransferred(((File)e.getNewValue()).length());
	    }
	    else if (DefaultPhotoProcessor.THUMBNAIL_CHANGED.equals(propertyName))
	    {
		_uploadPanel.setThumbnail((BufferedImage)e.getNewValue());
	    }
	}

    }

    private class ThumbnailItemListener
	    implements IThumbnailListener
    {
	/** 
	 * Invoked when the state of a thumbnail item within thumbnail-view panel
	 * changes.
	 * @param   e
	 *	    Event data.
	 */
	public void thumbnailItemChanged(ThumbnailEvent e)
	{
	    ThumbnailItem source = (ThumbnailItem)e.getSource();
	    KeyValuePair<File> leaf = source.getAssociatedLeaf();

	    switch (e.getEventType())
	    {
		case Deselected:
		{
		    _uploaderModel.deselect(leaf);
		    break;
		}
		case Selected:
		{
		    _uploaderModel.select(leaf);
		    break;
		}
		case Rotated90:
		{
		    _uploaderModel.rotate(leaf, RotateType.Rotate90);
		    break;
		}
		case Rotated180:
		{
		    _uploaderModel.rotate(leaf, RotateType.Rotate180);
		    break;
		}
		case Rotated270:
		{
		    _uploaderModel.rotate(leaf, RotateType.Rotate270);
		    break;
		}
	    }
	}

    }

    private class ToolbarActionListener
	    extends LinkAction
    {
	/** Invoked when the user clicks one of the links. */
	public void actionPerformed(ActionEvent e)
	{
	    Object source = e.getSource();

	    if (source == _selectAllLink)
	    {
		_thumbnailViewPanel.selectAll();
	    }
	    else if (source == _selectNoneLink)
	    {
		_thumbnailViewPanel.selectNone();
	    }
	    else if (source == _uploadLink)
	    {
		List<KeyValuePair<File>> selectedFileList =
			_uploaderModel.getSelectedFileList();
		int fileListSize = selectedFileList.size();

		if (fileListSize > 0)
		{
		    _uploadPanel.uploadStarted(getTotalBytesForFiles(selectedFileList),
					       fileListSize);
		    _layout.show(_containerPanel, UPLOAD_PROGRESS_SCREEN);

		    SwingUtilities.invokeLater(
			    new Runnable()
			    {
				public void run()
				{
				    SwingWorker<Void, Void> worker = new SwingWorker<Void, Void>()
				    {
					@Override
					protected Void doInBackground()
					{
					    try
					    {
						_uploaderModel.uploadSelected();
					    }
					    catch (PhotoProcessorException e)
					    {
						_logger.log(Level.SEVERE,
							    e.getMessage(), e);
						showAlertDialog(e.getMessage());
					    }

					    return null;
					}

					@Override
					protected void done()
					{
					    _uploadPanel.uploadFinished();
					    _layout.show(_containerPanel,
							 IMAGE_SELECTOR_SCREEN);
					}

				    };

				    worker.execute();
				}

			    });
		}
	    }
	}

    }

    private class UploadPanelListener
	    implements PropertyChangeListener
    {
	/** Invoked when the user clicks Cancel on the {@link UploadScreen}. */
	public void propertyChange(PropertyChangeEvent e)
	{
	    if (e.getPropertyName().equals(UploadPanel.CANCEL_UPLOAD_PROPERTY))
		_uploaderModel.cancelUploadPhotos();
	}

    }

    private static final String IMAGE_SELECTOR_SCREEN =
	    "imageSelectorScreen";
    private static final String UPLOAD_PROGRESS_SCREEN =
	    "uploadProgressScreen";
    private static final Logger _logger =
	    Logger.getLogger(MediaUploaderApplet.class.getName());

    static
    {
	LogManager.getLogManager().addLogger(_logger);

	try
	{
	    UIManager.setLookAndFeel(UIManager.getCrossPlatformLookAndFeelClassName());
	}
	catch (Exception e)
	{
	    _logger.log(Level.WARNING,
			"Cannot apply CrossPlatformLookAndFeel due to {0}",
			e.getMessage());
	}
    }

    private CardLayout _layout;
    private Dimension _thumbnailDimension;
    private DirectoryTreeView _treeView;
    private GalleryModel _galleryModel;
    private Hyperlink _selectAllLink;
    private Hyperlink _selectNoneLink;
    private Hyperlink _uploadLink;
    private IThumbnailListener _thumbnailListener;
    private JComboBox _galleryComboBox;
    private JPanel _containerPanel;
    private MemberAccount _memberAccount;
    private MediaUploaderModel _uploaderModel;
    private ThumbnailViewPanel _thumbnailViewPanel;
    private Toolbar _toolbar;
    private UploadPanel _uploadPanel;

    /**
     * Creates a new instance of the <tt>MediaUploaderApplet</tt> class.
     */
    public MediaUploaderApplet()
    {
	Thread.setDefaultUncaughtExceptionHandler(new AppletExceptionHandler());
    }

    public String getValueFor(String param)
    {
	return getParameter(param);
    }

    @Override
    public void init()
    {
	String token = getValueFor(TokenParam);
	String defaultGallery = getValueFor(DefaultGallery);
	String[] galleryNames =
		ParamValueParser.getGenericParamValues(this,
						       GalleryName,
						       ArrayItemDelimiter);
	String[] galleryIDs =
		ParamValueParser.getGenericParamValues(this,
						       GalleryID,
						       ArrayItemDelimiter);

	if (token == null)
	{
	    showAlertDialog(TokenNotFound);
	}
	else
	{
	    _thumbnailDimension = new Dimension(160, 160);
	    _thumbnailListener = new ThumbnailItemListener();
	    _galleryModel = new GalleryModel();
	    _memberAccount = new MemberAccount(token, _galleryModel);
	    DefaultPhotoProcessor photoProcessor =
		    new DefaultPhotoProcessor(_memberAccount);
	    photoProcessor.addPropertyChangeListener(new PhotoProcessorListener());

	    _layout = new CardLayout();
	    _containerPanel = new JPanel(_layout);
	    add(_containerPanel, BorderLayout.CENTER);

	    _thumbnailViewPanel = new ThumbnailViewPanel(_thumbnailDimension,
							 photoProcessor);
	    _thumbnailViewPanel.setThumbnailListener(_thumbnailListener);

	    _treeView = new DirectoryTreeView();
	    _treeView.addDirectoryTreeListener(new DirectoryTreeListener());
	    _treeView.expandRow(0);

	    _uploadPanel = new UploadPanel(_thumbnailDimension);
	    _uploadPanel.addPropertyChangeListener(new UploadPanelListener());

	    ToolbarActionListener toolbarActionListener =
		    new ToolbarActionListener();

	    _selectAllLink = new Hyperlink(toolbarActionListener);
	    _selectAllLink.setText(AppletResources.SelectAllLinkText);

	    _selectNoneLink = new Hyperlink(toolbarActionListener);
	    _selectNoneLink.setText(AppletResources.NothingLinkText);

	    _uploadLink = new Hyperlink(toolbarActionListener);
	    _uploadLink.setText(AppletResources.UploadSelectedLinkText);

	    _galleryComboBox = new JComboBox();
	    _galleryComboBox.addItemListener(new GalleryComboBoxItemListener());
	    _toolbar = new Toolbar();

	    _galleryModel.addGalleryModelListener(new GalleryModelListener());
	    _galleryModel.addGalleryMany(galleryNames, galleryIDs);

	    if (defaultGallery != null)
		_galleryModel.setDefaultGallery(defaultGallery);
	    else
		_logger.log(Level.WARNING, "Default gallery not found.");

	    IRotateTypeProcessor rotateTypeProcessor =
		    new DefaultRotateTypeProcessor();
	    _uploaderModel = new MediaUploaderModel(photoProcessor,
						    rotateTypeProcessor);
	    _uploaderModel.addPropertyChangeListener(new MediaUploaderPropertyChangeListener());
	    _uploaderModel.setValidExtensions(AppletResources.ValidExtensions);

	    _containerPanel.add(new ImageSelectorScreen(), IMAGE_SELECTOR_SCREEN);
	    _containerPanel.add(_uploadPanel, UPLOAD_PROGRESS_SCREEN);
	}
    }

    @Override
    public String[][] getParameterInfo()
    {
	return new String[][] { {
			    AppletResources.TokenParam,
			    "DES encrypted MemberID",
			    "Unique encrypted MemberID"
			}
	};
    }

    private void showAlertDialog(String message)
    {
	JOptionPane.showMessageDialog(this,
				      message,
				      AppletResources.CompanyName,
				      JOptionPane.ERROR_MESSAGE);
    }

    private static long getTotalBytesForFiles(List<KeyValuePair<File>> files)
    {
	long totalBytes = 0;

	for (KeyValuePair<File> file : files)
	    totalBytes += file.value.length();

	return totalBytes;
    }

}
