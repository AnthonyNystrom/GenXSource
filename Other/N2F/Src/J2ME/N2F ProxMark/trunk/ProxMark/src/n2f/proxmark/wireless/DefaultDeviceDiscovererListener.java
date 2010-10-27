/* ------------------------------------------------
 * DefaultDeviceDiscovererListener.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark.wireless;

/**
 * Default IDeviceDiscovererListener implementation that is used when no other
 * listeners exist for DeviceDiscoverer related events.
 * 
 * @author Alex Nesterov
 */
public class DefaultDeviceDiscovererListener
	implements IDeviceDiscovererListener
{
    /**
     * Creates a new instance of DefaultDeviceDiscovererListener.
     */
    public DefaultDeviceDiscovererListener()
    {
    }
    
    public void advertReceived(String advertText, String imageBase64String)
    {
    }
    
    public void cannotStartTaggingService()
    {
    }
}
