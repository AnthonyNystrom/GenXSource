/* ------------------------------------------------
 * IDeviceDiscovererListener.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark.wireless;

/**
 * The IDeviceDiscovererListener should be implemented by any class whose
 * instances are intended to be notified of DeviceDiscoverer related events.
 * 
 * @author Alex Nesterov
 */
public interface IDeviceDiscovererListener
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
     * Invoked when an error occured during Bluetooth radio initialization to
     * make the device visible for tagging.
     */
    void cannotStartTaggingService();
}
