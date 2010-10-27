/* ------------------------------------------------
 * IBTTagServerListener.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.tag.wireless;

/**
 * The IBTTagServerListener should be implemented by any class whose instances
 * are intended to be notified of BTTagServer related events.
 * 
 * @author Alex Nesterov
 */
public interface IBTTagServerListener
{
    /**
     * Invoked when an error occures while trying to set up Bluetooth Tagging
     * Service.
     */
    void serverCannotInitializeBluetooth();
}
