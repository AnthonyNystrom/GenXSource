/* ------------------------------------------------
 * IDeviceDiscovererListener.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.tag.wireless;

/**
 * The IDeviceDiscovererListener should be implemented by any class whose
 * instances are intended to be notified of DeviceDiscoverer related events.
 * 
 * @author Alex Nesterov
 */
public interface IDeviceDiscovererListener
{
    /**
     * Invoked when an error occured during Bluetooth radio initialization for
     * device search purpose.
     */
    void discovererCannotStartDeviceSearch();
    
    /**
     * Invoked when an error occured during Bluetooth radio initialization to
     * make the device visible for tagging.
     */
    void discovererCannotStartTaggingService();
}
