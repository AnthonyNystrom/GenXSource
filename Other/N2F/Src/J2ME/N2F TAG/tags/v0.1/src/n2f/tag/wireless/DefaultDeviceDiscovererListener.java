/* ------------------------------------------------
 * DefaultDeviceDiscovererListener.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.tag.wireless;

/**
 * Default IDeviceDiscovererListener implementation that is used when no other
 * listeners exist for DeviceDiscoverer related events.
 * 
 * @author Alex Nesterov
 */
public class DefaultDeviceDiscovererListener
	implements IDeviceDiscovererListener
{
    /* ------ Methods.Public ------ */
    
    public void discovererCannotStartDeviceSearch()
    {
    }
    
    public void discovererCannotStartTaggingService()
    {
    }
    
    /* ------ Constructors ------ */
    
    /**
     * Creates a new instance of DefaultDeviceDiscovererListener.
     */
    public DefaultDeviceDiscovererListener()
    {
    }
}
