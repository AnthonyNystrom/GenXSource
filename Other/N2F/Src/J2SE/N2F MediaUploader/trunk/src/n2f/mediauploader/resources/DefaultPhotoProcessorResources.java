/* ------------------------------------------------
 * DefaultPhotoProcessorResources.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader.resources;

import java.util.ResourceBundle;

/**
 * @author Alex Nesterov
 */
public final class DefaultPhotoProcessorResources
{
    private static final ResourceBundle _resources;
    public static final String CannotReadFile;
    public static final String ImageWriteFailed;
    
    static
    {
	_resources = ResourceBundle.getBundle("DefaultPhotoProcessorResources");
	CannotReadFile = _resources.getString("CannotReadFile");
	ImageWriteFailed = _resources.getString("ImageWriteFailed");
    }
    
    /**
     * This class is immutable.
     */
    private DefaultPhotoProcessorResources()
    {
    }
}
