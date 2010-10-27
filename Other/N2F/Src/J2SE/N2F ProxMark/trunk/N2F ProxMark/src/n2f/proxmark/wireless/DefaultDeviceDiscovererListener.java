/* ------------------------------------------------
 * DefaultDeviceDiscovererListener.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark.wireless;

import javax.bluetooth.ServiceRecord;

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
     * Creates a new instance of the <tt>DefaultDeviceDiscovererListener</tt> class.
     */
    public DefaultDeviceDiscovererListener()
    {
    }

    public void cannotInitializeBluetooth()
    {
    }
    
    public void cannotStartDeviceSearch()
    {
    }

    public void tagDevice(ServiceRecord serviceRecord)
    {
    }
}
