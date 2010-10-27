/* ------------------------------------------------
 * DefaultBTTagServerListener.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.tag.wireless;

/**
 * Default IBTTagServerListener implementation that is used when no other
 * listeners exist for BTTagServer related events.
 *
 * @author Alex Nesterov
 */
public class DefaultBTTagServerListener
	implements IBTTagServerListener
{
    /* ------ Methods.Public ------ */
    
    public void serverCannotInitializeBluetooth()
    {
    }
    
    /* ------ Constructors ------ */
    
    /**
     * Creates a new instance of DefaultBTTagServerListener.
     */
    public DefaultBTTagServerListener()
    {
    }
}
