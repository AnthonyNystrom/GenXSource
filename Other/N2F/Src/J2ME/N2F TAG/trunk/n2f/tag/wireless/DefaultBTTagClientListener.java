/* ------------------------------------------------
 * DefaultBTTagClientListener.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.tag.wireless;

/**
 * Default IBTTagClientListener implementation that is used when no other
 * listeners exist for BTTagClient related events.
 *
 * @author Alex Nesterov
 */
public final class DefaultBTTagClientListener
	implements IBTTagClientListener
{
    /* ------ Methods.Public ------ */
    
    public void clientCannotInitializeBluetooth()
    {
    }
    
    public void clientCannotStartDeviceSearch()
    {
    }
    
    public void clientDeviceDiscoveryError()
    {
    }
    
    public void clientServiceSearchCompleted(ServiceRecordList discoveredServices)
    {
    }
    
    /* ------ Constructors ------ */
    
    /**
     * Creates a new instance of DefaultBTTagClientListener.
     */
    public DefaultBTTagClientListener()
    {
    }
}
