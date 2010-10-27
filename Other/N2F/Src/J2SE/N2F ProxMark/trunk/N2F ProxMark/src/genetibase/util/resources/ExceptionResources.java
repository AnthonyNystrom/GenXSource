/* ------------------------------------------------
 * ExceptionResources.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.util.resources;

import java.util.ResourceBundle;

/**
 * @author Alex Nesterov
 */
public final class ExceptionResources
{
    private static final ResourceBundle _resources;
    public static final String ArgumentCannotBeNull;
    public static final String CannotLoadResource;
    
    static
    {
	_resources = ResourceBundle.getBundle("ExceptionResources");
	ArgumentCannotBeNull = _resources.getString("ArgumentCannotBeNull");
	CannotLoadResource = _resources.getString("CannotLoadResource");
    }
    
    /**
     * This class is immutable.
     */
    private ExceptionResources()
    {
    }
}
