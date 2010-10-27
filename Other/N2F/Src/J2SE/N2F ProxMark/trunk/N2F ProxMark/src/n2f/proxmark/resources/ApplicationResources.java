/* ------------------------------------------------
 * ApplicationResources.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark.resources;

import genetibase.util.ResourceLoader;
import java.awt.Image;
import java.util.ResourceBundle;

/**
 * @author Alex Nesterov
 */
public final class ApplicationResources
{
    public static final Image Delete_Normal;
    public static final Image Delete_Disabled;
    public static final Image Delete_Hot;
    public static final Image New_Normal;
    public static final Image New_Disabled;
    public static final Image New_Hot;
    public static final Image Publish_Normal;
    public static final Image Publish_Disabled;
    public static final Image Publish_Hot;
    public static final String AdParams_Empty;
    public static final String AdParams_OnlyImage;
    public static final String Browse_Text;
    public static final String Browse_Tooltip;
    public static final String CannotInitializeBluetooth;
    public static final String CannotStartDeviceSearch;
    public static final String CompanyName;
    public static final String Delete_Text;
    public static final String Delete_Tooltip;
    public static final String ImageFilterDescription;
    public static final String Image_Text;
    public static final String Multiply_Text;
    public static final String New_Text;
    public static final String New_Tooltip;
    public static final String Open_Text;
    public static final String ProductName;
    public static final String Publish_Text;
    public static final String Publish_Tooltip;
    public static final String Text_Text;
    public static final String UnknownError;
    public static final String[] ValidExtensions = new String[] {
	"bmp", "gif", "jpg", "jpeg", "jpe", "jfif", "png", "tif", "tiff"
    };
    private static final ResourceBundle _resources;
    
    static
    {
	_resources = ResourceBundle.getBundle("ApplicationResources");
	
	AdParams_Empty = _resources.getString("AdParams_Empty");
	AdParams_OnlyImage = _resources.getString("AdParams_OnlyImage");
	Browse_Text = _resources.getString("Browse_Text");
	Browse_Tooltip = _resources.getString("Browse_Tooltip");
	CannotInitializeBluetooth = _resources.getString("CannotInitializeBluetooth");
	CannotStartDeviceSearch = _resources.getString("CannotStartDeviceSearch");
	CompanyName = _resources.getString("CompanyName");
	Delete_Text = _resources.getString("Delete_Text");
	Delete_Tooltip = _resources.getString("Delete_Tooltip");
	Image_Text = _resources.getString("Image_Text");
	ImageFilterDescription = _resources.getString("ImageFilterDescription");
	Multiply_Text = _resources.getString("Multiply_Text");
	New_Text = _resources.getString("New_Text");
	New_Tooltip = _resources.getString("New_Tooltip");
	Open_Text = _resources.getString("Open_Text");
	ProductName = _resources.getString("ProductName");
	Publish_Text = _resources.getString("Publish_Text");
	Publish_Tooltip = _resources.getString("Publish_Tooltip");
	Text_Text = _resources.getString("Text_Text");
	UnknownError = _resources.getString("UnknownError");
	
	Class<?> resourceProvider = ApplicationResources.class;
	
	Delete_Normal = ResourceLoader.loadImage(resourceProvider, "images/Delete_Normal.png");
	Delete_Disabled = ResourceLoader.loadImage(resourceProvider, "images/Delete_Disabled.png");
	Delete_Hot = ResourceLoader.loadImage(resourceProvider, "images/Delete_Hot.png");
	New_Normal = ResourceLoader.loadImage(resourceProvider, "images/New_Normal.png");
	New_Disabled = ResourceLoader.loadImage(resourceProvider, "images/New_Disabled.png");
	New_Hot = ResourceLoader.loadImage(resourceProvider, "images/New_Hot.png");
	Publish_Normal = ResourceLoader.loadImage(resourceProvider, "images/Publish_Normal.png");
	Publish_Disabled = ResourceLoader.loadImage(resourceProvider, "images/Publish_Disabled.png");
	Publish_Hot = ResourceLoader.loadImage(resourceProvider, "images/Publish_Hot.png");
    }
    
    /**
     * This class is immutable.
     */
    private ApplicationResources()
    {
    }
}
