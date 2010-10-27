/* ------------------------------------------------
 * DateConverterResources.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader.util.resources;

import java.util.ResourceBundle;

/**
 * @author Alex Nesterov
 */
public final class DateConverterResources
{
    private static final ResourceBundle _resources;
    public static final String CannotBeNegative;
    
    static
    {
	_resources = ResourceBundle.getBundle("DateConverterResources");
	CannotBeNegative = _resources.getString("CannotBeNegative");
    }

    /**
     * This class is immutable.
     */
    private DateConverterResources()
    {
    }

}
