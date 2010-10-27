/* ------------------------------------------------
 * DefaultBTClientListener.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark.wireless;

/**
 * Default IBTTagClientListener implementation that is used when no other
 * listeners exist for BTTagClient related events.
 *
 * @author Alex Nesterov
 */
public final class DefaultBTClientListener
	implements IBTClientListener
{
    /**
     * Creates a new instance of the <tt>DefaultBTClientListener</tt> class.
     */
    public DefaultBTClientListener()
    {
    }
    
    public void cannotInitializeBluetooth()
    {
    }
    
    public void cannotStartDeviceSearch()
    {
    }
    
    public void deviceDiscoveryError()
    {
    }
    
    public void serviceSearchCompleted(ServiceRecordList discoveredServices)
    {
    }
}
