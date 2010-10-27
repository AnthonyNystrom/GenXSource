/* ------------------------------------------------
 * PhotoOrganiseServiceResources.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader.webservice.resources;

import java.util.ResourceBundle;

/**
 * @author Alex Nesterov
 */
public final class PhotoOrganiseServiceResources
{
    private static final ResourceBundle _resources;
    public static final String CannotAccessWebService;
    public static final String CannotOpenConnection;
    public static final String PhotoOrganiseURL;
    
    static
    {
	_resources = ResourceBundle.getBundle("PhotoOrganiseServiceResources");
	CannotAccessWebService = _resources.getString("CannotAccessWebService");
	CannotOpenConnection = _resources.getString("CannotOpenConnection");
	PhotoOrganiseURL = _resources.getString("PhotoOrganiseURL");
    }
    
    /**
     * This class is immutable.
     */
    private PhotoOrganiseServiceResources()
    {
    }
}
