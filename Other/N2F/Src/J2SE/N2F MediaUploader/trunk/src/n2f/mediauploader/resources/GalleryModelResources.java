/* ------------------------------------------------
 * GalleryModelResources.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader.resources;

import java.util.ResourceBundle;

/**
 * @author Alex Nesterov
 */
public final class GalleryModelResources
{
    private static final ResourceBundle _resources;
    public static final String GalleryIDNotMapped;
    public static final String GalleryDoesNotExist;
    public static final String ArrayLengthNotEqual;
    
    static
    {
	_resources = ResourceBundle.getBundle("GalleryModelResources");
	ArrayLengthNotEqual = _resources.getString("ArrayLengthNotEqual");
	GalleryIDNotMapped = _resources.getString("GalleryIDNotMapped");
	GalleryDoesNotExist = _resources.getString("GalleryDoesNotExist");
    }
    
    /**
     * This class is immutable.
     */
    private GalleryModelResources()
    {
    }
}
