/* ------------------------------------------------
 * IDeviceDiscovererListener.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark.wireless;

import javax.bluetooth.ServiceRecord;

/**
 * This interface should be implemented by the objects that are intended to
 * be notified of <tt>DeviceDiscoverer</tt> related events.
 * 
 * @author Alex Nesterov
 */
public interface IDeviceDiscovererListener
{
    /**
     * Invoked when an error occurs while trying to initialize Bluetooth.
     */
    void cannotInitializeBluetooth();
    
    /**
     * Invoked when an error occurs that does not allow the search process to 
     * start.
     */
    void cannotStartDeviceSearch();
    
    /**
     * Send and receive data over Bluetooth radio channel.
     * @param serviceRecord
     */
    void tagDevice(ServiceRecord serviceRecord);
}
