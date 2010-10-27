/* ------------------------------------------------
 * DeviceDiscoverer.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark.wireless;

public class DeviceDiscoverer
{
    private class BTServerListener
	    implements IBTServerListener
    {
	public void advertReceived(String advertText, String imageBase64String)
	{
	    System.out.println("DeviceDiscoverer::advertReceived");
	    _listener.advertReceived(advertText, imageBase64String);
	}

	public void cannotInitializeBluetooth()
	{
	    System.out.println("DeviceDiscoverer::serverCannotInitializeBluetooth");
	    _listener.cannotStartTaggingService();
	}

    }

    private boolean _isVisible;
    private BTServer _server;
    private IDeviceDiscovererListener _listener;

    /**
     * Creates a new instance of DeviceDiscoverer. Uses
     * DefaultDeviceDiscovererListener as a Null Object listener.
     */
    public DeviceDiscoverer()
    {
	this(new DefaultDeviceDiscovererListener());
    }

    /**
     * Creates a new instance of DeviceDiscoverer.
     *
     * @param listener	Specifies the instance that implements the
     *			IDeviceDiscovererListener interface and wants to be
     *			notified of DeviceDiscoverer related events. If the
     *			specified value is <code>null</code>,
     *			DefaultDeviceDiscovererListener is used instead.
     */
    public DeviceDiscoverer(IDeviceDiscovererListener listener)
    {
	if (listener == null)
	    listener = new DefaultDeviceDiscovererListener();

	_listener = listener;
	_server = new BTServer(new BTServerListener());
    }

    public void free()
    {
	setVisible(false);
	_server = null;
	_listener = null;
    }

    /**
     * Sets the value indicating whether the device is visible to other devices.
     */
    public void setVisible(boolean value)
    {
	if (_isVisible == value)
	    return;
	System.out.println("DeviceDiscoverer::setVisible(" + value + ")");

	_isVisible = value;

	if (value)
	{
	    System.out.println("DeviceDiscoverer::setVisible - Starting server...");
	    _server.start();
	}
	else
	{
	    System.out.println("DeviceDiscoverer::setVisible - Stopping server...");
	    _server.stop();
	}
    }

}
