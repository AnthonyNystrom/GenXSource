/* ------------------------------------------------
 * AdvertPublishModel.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark;

import java.awt.Dimension;
import java.beans.PropertyChangeListener;
import java.beans.PropertyChangeSupport;
import java.io.DataOutputStream;
import java.io.IOException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.logging.Level;
import java.util.logging.LogManager;
import java.util.logging.Logger;
import javax.bluetooth.ServiceRecord;
import javax.microedition.io.Connector;
import javax.microedition.io.StreamConnection;
import n2f.proxmark.wireless.BTService;
import n2f.proxmark.wireless.DeviceDiscoverer;
import n2f.proxmark.wireless.IDeviceDiscovererListener;
import static genetibase.util.resources.ExceptionResources.*;

/**
 * Keeps a list of advertisements to publish to remote devices via Bluetooth radio.
 * @author Alex Nesterov
 */
final class AdvertPublishModel
{
    /** Occurs when the return value of the <tt>canPublish</tt> method changes. */
    public static final String CAN_PUBLISH_CHANGED =
	    "PublisherModel.canPublishChanged";
    private static final Logger _logger =
	    Logger.getLogger(AdvertPublishModel.class.getName());

    static
    {
	LogManager.getLogManager().addLogger(_logger);
    }

    private DeviceDiscoverer _deviceDiscoverer;
    private Dimension _thumbnailMaximumSize;
    private List<Advert> _advertList;
    private Map<ServiceRecord, Advert> _knownServiceRecords;
    private PropertyChangeSupport _changeSupport;

    /**
     * Creates a new instance of the <tt>AdvertPublishModel</tt> class.
     * 
     * @param	l
     *		Specifies the listener to receive <tt>DeviceDiscoverer</tt>
     *		related events.
     * @param	thumbnailMaximumSize
     *		Specifies the maximum size of the thumbnail to be sent to devices
     *		via Bluetooth radio.
     * @throws	IllegalArgumentException
     *		If the specified <tt>l</tt> is <code>null</code>, or
     *		if the specified <tt>thumbnailMaximumSize</tt> is <code>null</code>.
     */
    public AdvertPublishModel(IDeviceDiscovererListener l,
			       Dimension thumbnailMaximumSize)
    {
	if (l == null)
	    throw new IllegalArgumentException(
		    String.format(ArgumentCannotBeNull, "l"));
	if (thumbnailMaximumSize == null)
	    throw new IllegalArgumentException(
		    String.format(ArgumentCannotBeNull, "thumbnailMaximumSize"));
	_advertList = new ArrayList<Advert>();
	_changeSupport = new PropertyChangeSupport(this);
	_deviceDiscoverer = new DeviceDiscoverer(l);
	_knownServiceRecords = new HashMap<ServiceRecord, Advert>();
	_thumbnailMaximumSize = thumbnailMaximumSize;

    /** 
     * @todo DeviceDiscoverer retrieves the next advert (text + image in Base64 format) for
     * the specified device. Then sends the data to the device over Bluetooth connection.
     * A hashtable of already known devices is expected to be implemented.
     * Each device will be sent a sequence of different adverts until the adverts queue
     * is not empty. Then the queue is refilled and the device will be send the first
     * advert again. Until publisher is not stopped.
     */
    }

    /**
     * Adds the specified listener to receive property change related events
     * from this component.
     * @param	l
     *		Specifies the listener to add.
     * @throws	IllegalArgumentException
     *		If the specified <tt>l</tt> is <code>null</code>.
     */
    public void addPropertyChangeListener(PropertyChangeListener l)
    {
	if (l == null)
	    throw new IllegalArgumentException(
		    String.format(ArgumentCannotBeNull, "l"));
	_changeSupport.addPropertyChangeListener(l);
    }

    /**
     * Removes the specified listener so that it no longer receives property
     * change related events from this component.
     * @param	l
     *		Specifies the listener to remove.
     */
    public void removePropertyChangeListener(PropertyChangeListener l)
    {
	_changeSupport.removePropertyChangeListener(l);
    }

    /**
     * Adds a new advertisement to the list of advertisements to publish to remote devices.
     * @param	advertToAdd
     *		Specifies the advertisement to be published.
     * @throws	IllegalArgumentException
     *		If the specified <tt>advertToAdd</tt> is <code>null</code>.
     */
    public void addAdvert(Advert advertToAdd)
    {
	if (advertToAdd == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "advertToAdd"));

	_advertList.add(advertToAdd);
	setCanPublish(!_advertList.isEmpty());
    }

    /**
     * Removes the specified <tt>advertToRemove</tt> from the list of advertisements
     * to be published to remote devices.
     * @param	advertToRemove
     *		Specifies the advertisement to be removed and no longer be published.
     */
    public void removeAdvert(Advert advertToRemove)
    {
	_advertList.remove(advertToRemove);
	setCanPublish(!_advertList.isEmpty());
    }

    /**
     * Prepares this component to become garbage.
     */
    public void destroy()
    {
	_deviceDiscoverer.destroy();
    }

    private boolean _canPublish;

    /**
     * Returns the value indicating whether this <tt>AdvertPublishModel</tt>
     * is ready to publish advertisements to remote devices.
     * @return	The value indicating whether this <tt>AdvertPublishModel</tt>
     *		is ready to publish advertisements to remote devices.
     */
    public boolean canPublish()
    {
	return _canPublish;
    }

    private void setCanPublish(boolean value)
    {
	if (_canPublish != value)
	{
	    boolean oldValue = _canPublish;
	    _canPublish = value;
	    _changeSupport.firePropertyChange(CAN_PUBLISH_CHANGED,
					      oldValue,
					      value);
	}
    }

    private boolean _isPublishing;

    /**
     * Gets the value indicating whether this <tt>AdvertPublishModel</tt> is
     * currently publishing advertisements to remote devices.
     * @return	The value indicating whether this <tt>AdvertPublishModel</tt> is
     *		currently publishing advertisements to remote devices.
     */
    public boolean isPublishing()
    {
	return _isPublishing;
    }

    /**
     * Sets the value indicating whether this <tt>AdvertPublishModel</tt> should
     * start or stop publishing advertisements to remove devices.
     * @param	value
     *		<code>true</code> to start publishing; <code>false</code> otherwise.
     */
    public void setIsPublishing(boolean value)
    {
	if (_isPublishing != value)
	{
	    _isPublishing = value;
	    _deviceDiscoverer.setTaggingOn(_isPublishing);
	}
    }

    public synchronized void tagDevice(ServiceRecord serviceRecord)
    {
	_logger.log(Level.INFO, "STARTED TAGGING");
	String url =
		serviceRecord.getConnectionURL(ServiceRecord.NOAUTHENTICATE_NOENCRYPT,
					       false);
	StreamConnection connection = null;
	DataOutputStream out = null;

	try
	{
	    _logger.log(Level.INFO, "URL from service record: {0}", url);
	    connection = (StreamConnection)Connector.open(url,
							  Connector.WRITE,
							  true);

	    _logger.log(Level.INFO, "Opened connection...");
	    out = connection.openDataOutputStream();
	    _logger.log(Level.INFO, "Opened OutputStream...");

	    Advert currentAdvert = getAdvertFor(serviceRecord);
	    
	    if (currentAdvert != null)
	    {
		String text = getAdvertText(currentAdvert);
		if (text == null)
		    text = "";
		String image = getAdvertImage(currentAdvert);
		if (image == null)
		    image = "";
		
		_logger.log(Level.INFO, "TEXT: {0}", text);
		
		out.writeUTF(text);
		_logger.log(Level.INFO, "Wrote advert text SUCCESSFULLY.");
		out.writeUTF(image);
		_logger.log(Level.INFO, "Wrote advert image SUCCESSFULLY.");
		out.flush();
		_logger.log(Level.INFO, "Wrote advert successfully.");
	    }
	    else
	    {
		_logger.log(Level.WARNING, "currentAdvert == null");
	    }
	}
	catch (Exception e)
	{
	    _logger.log(Level.SEVERE, e.getMessage(), e);
	    BTService.closeOutputStream(out);
	    BTService.closeConnection(connection);
	    connection = null;
	    out = null;

	    return;
	}

	BTService.closeOutputStream(out);
	BTService.closeConnection(connection);
	connection = null;
	out = null;

	_logger.log(Level.INFO, "FINISHED TAGGING");
    }

    private String getAdvertText(Advert advert)
    {
	return advert.getText();
    }

    private String getAdvertImage(Advert advert)
    {
	String imageBase64String = null;

	try
	{
	    imageBase64String =
		    ImageProcessor.imageToBase64(advert.getImagePath(),
						 _thumbnailMaximumSize);
	}
	catch (IOException e)
	{
	    _logger.log(Level.SEVERE, e.getMessage(), e);
	}

	return imageBase64String;
    }

    private Advert getAdvertFor(ServiceRecord serviceRecord)
    {
	Advert result = null;

	if (_advertList.isEmpty())
	    return result;

	if (_knownServiceRecords.containsKey(serviceRecord))
	{
	    int currentAdvertIndex =
		    _advertList.indexOf(_knownServiceRecords.get(serviceRecord));
	    int nextAdvertIndex = currentAdvertIndex++;

	    if (nextAdvertIndex < 0 || nextAdvertIndex >= _advertList.size())
		nextAdvertIndex = 0;

	    result = _advertList.get(nextAdvertIndex);
	    _knownServiceRecords.put(serviceRecord, result);
	}
	else
	{
	    result = _advertList.get(0);
	    _knownServiceRecords.put(serviceRecord, result);
	}

	return result;
    }

}
