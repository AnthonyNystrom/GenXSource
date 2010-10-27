/* ------------------------------------------------
 * IBTServerListener.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark.wireless;

/**
 * The IBTServerListener should be implemented by any class whose instances
 * are intended to be notified of BTTagServer related events.
 * 
 * @author Alex Nesterov
 */
public interface IBTServerListener
{
    /**
     * Invoked when a new advert is received over Bluetooth connection.
     * @param	advertText
     *		Specifies the text of the advertisement.
     * @param	imageBase64String
     *		Specifies the image for the advertisement in Base64 encoding.
     */
    void advertReceived(String advertText, String imageBase64String);
    
    /**
     * Invoked when an error occures while trying to set up Bluetooth Tagging
     * Service.
     */
    void cannotInitializeBluetooth();
}
