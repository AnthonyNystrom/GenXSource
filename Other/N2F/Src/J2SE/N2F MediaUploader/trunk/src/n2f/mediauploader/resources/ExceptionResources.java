/* ------------------------------------------------
 * ExceptionResources.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader.resources;

import java.util.ResourceBundle;

/**
 * @author Alex Nesterov
 */
public final class ExceptionResources
{
    private static final ResourceBundle _resources;
    public static final String ArgumentCannotBeNull;
    
    static
    {
	_resources = ResourceBundle.getBundle("ExceptionResources");
	ArgumentCannotBeNull = _resources.getString("ArgumentCannotBeNull");
    }
    
    /**
     * This class is immutable.
     */
    private ExceptionResources()
    {
    }
}
