/* ------------------------------------------------
 * IBTTagClientListener.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.tag.wireless;

/**
 * The IBTTagClientListener should be implemented by any class whose instances
 * are intended to be notified of BTTagClient related events.
 * 
 * @author Alex Nesterov
 */
public interface IBTTagClientListener
{
    /**
     * Invoked when an error occurs while trying to initialize Bluetooth.
     */
    void clientCannotInitializeBluetooth();
    
    /**
     * Invoked when an error occurs that does not allow the search process to 
     * start.
     */
    void clientCannotStartDeviceSearch();
    
    /**
     * Invoked when an error occurs while Bluetooth devices discovery.
     */
    void clientDeviceDiscoveryError();
    
    /**
     * Invoked when Bluetooth services search completed.
     * @param discoveredServices    Contains records for discovered Bluetooth
     *				    Tagging Services on remote devices.
     */
    void clientServiceSearchCompleted(ServiceRecordList discoveredServices);
}
