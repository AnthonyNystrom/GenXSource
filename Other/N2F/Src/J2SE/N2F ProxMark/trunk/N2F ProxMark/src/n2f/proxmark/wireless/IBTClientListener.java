/* ------------------------------------------------
 * IBTTagClientListener.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark.wireless;

/**
 * The IBTClientListener should be implemented by any class whose instances
 * are intended to be notified of BTClient related events.
 * 
 * @author Alex Nesterov
 */
public interface IBTClientListener
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
     * Invoked when an error occurs while Bluetooth devices discovery.
     */
    void deviceDiscoveryError();
    
    /**
     * Invoked when Bluetooth services search completed.
     * @param	discoveredServices
     *		Contains records for discovered ProxMark services on remote devices.
     */
    void serviceSearchCompleted(ServiceRecordList discoveredServices);
}
