/* ------------------------------------------------
 * WidgetResources.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader.resources;

import java.awt.Image;
import java.io.IOException;
import java.util.MissingResourceException;
import java.util.ResourceBundle;
import javax.imageio.ImageIO;

/**
 * @author Alex Nesterov
 */
public final class AppletResources
{
    public static final Image RotateCW_Normal;
    public static final Image RotateCW_Hot;
    public static final Image RotateCCW_Normal;
    public static final Image RotateCCW_Hot;
    public static final Image ThumbnailEmptyImage;
    public static final Image TreeCollapsed;
    public static final Image TreeExpanded;
    public static final String ArrayItemDelimiter;
    public static final String Byte;
    public static final String CancelButtonText;
    public static final String CompanyName;
    public static final String DefaultGallery;
    public static final String ElapsedTimeText;
    public static final String GalleryID;
    public static final String GalleryName;
    public static final String KByte;
    public static final String MByte;
    public static final String NothingLinkText;
    public static final String PerSecond;
    public static final String RemainingTimeText;
    public static final String SelectAllLinkText;
    public static final String TokenNotFound;
    public static final String TokenParam;
    public static final String TransferredText;
    public static final String UploadSelectedLinkText;
    public static final String UnknownError;
    public static final String UploadRateText;
    public static final String[] ValidExtensions = new String[] {
	"bmp", "gif", "jpg", "jpeg", "jpe", "jfif", "png", "tif", "tiff"
    };
    private static final ResourceBundle _resources;

    static
    {
	_resources = ResourceBundle.getBundle("AppletResources");
	
	ArrayItemDelimiter = _resources.getString("ArrayItemDelimiter");
	Byte = _resources.getString("Byte");
	CancelButtonText = _resources.getString("CancelButtonText");
	CompanyName = _resources.getString("CompanyName");
	DefaultGallery = _resources.getString("DefaultGallery");
	ElapsedTimeText = _resources.getString("ElapsedTimeText");
	GalleryID = _resources.getString("GalleryID");
	GalleryName = _resources.getString("GalleryName");
	KByte = _resources.getString("KByte");
	MByte = _resources.getString("MByte");
	NothingLinkText = _resources.getString("NothingLinkText");
	PerSecond = _resources.getString("PerSecond");
	RemainingTimeText = _resources.getString("RemainingTimeText");
	SelectAllLinkText = _resources.getString("SelectAllLinkText");
	TransferredText = _resources.getString("TransferredText");
	TokenNotFound = _resources.getString("TokenNotFound");
	TokenParam = _resources.getString("TokenParam");
	UploadRateText = _resources.getString("UploadRateText");
	UploadSelectedLinkText = _resources.getString("UploadSelectedLinkText");
	UnknownError = _resources.getString("UnknownError");

	RotateCW_Normal = loadImageResource("images/RotateCW_Normal.png");
	RotateCW_Hot = loadImageResource("images/RotateCW_Hot.png");
	RotateCCW_Normal = loadImageResource("images/RotateCCW_Normal.png");
	RotateCCW_Hot = loadImageResource("images/RotateCCW_Hot.png");
	ThumbnailEmptyImage = loadImageResource("images/ThumbnailEmpty.png");
	TreeCollapsed = loadImageResource("images/TreeCollapsed.png");
	TreeExpanded = loadImageResource("images/TreeExpanded.png");
    }

    /**
     * This class is immutable.
     */
    private AppletResources()
    {
    }

    private static Image loadImageResource(String key)
    {
	Image image = null;

	try
	{
	    image = ImageIO.read(AppletResources.class.getResource(key));
	}
	catch (IOException e)
	{
	    throw new MissingResourceException(String.format("Cannot load %s.",
							     key),
					       "AppletResources",
					       key);
	}

	return image;
    }

}
