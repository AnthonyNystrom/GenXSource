/* ------------------------------------------------
 * DeviceDiscoverer.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark.wireless;

import genetibase.util.WorkerThread;
import java.io.DataOutputStream;
import java.util.logging.Level;
import java.util.logging.LogManager;
import java.util.logging.Logger;
import javax.bluetooth.BluetoothStateException;
import javax.bluetooth.RemoteDevice;
import javax.bluetooth.ServiceRecord;
import javax.microedition.io.Connector;
import javax.microedition.io.StreamConnection;

/**
 * Provides functionality to discover devices via Bluetooth radio and send data
 * to the devices.
 * 
 * @author Administrator
 */
public class DeviceDiscoverer
{
    private static final long _requestSearchTimeOut = 5000L;
    private static final Logger _logger =
	    Logger.getLogger(DeviceDiscoverer.class.getName());

    static
    {
	LogManager.getLogManager().addLogger(_logger);
    }

    private class BTClientListener
	    implements IBTClientListener
    {
	public void cannotInitializeBluetooth()
	{
	    _logger.log(Level.INFO, "Setting _client == null");
	    _client = null;
	    _listener.cannotInitializeBluetooth();
	}

	public void cannotStartDeviceSearch()
	{
	    _logger.log(Level.SEVERE, "Cannot start device search.");
	    _listener.cannotStartDeviceSearch();
	}

	public void deviceDiscoveryError()
	{
	    _logger.log(Level.SEVERE, "Device discovery error.");
	}

	public void serviceSearchCompleted(ServiceRecordList discoveredServices)
	{
	    _logger.log(Level.INFO, "Service search completed.");

	    if (discoveredServices.size() == 0)
	    {
		makeNewSearchRequest();
		return;
	    }

	    printOutDiscoveredServices(discoveredServices);

	    IServiceRecordEnumeration readyForTaggingServices =
		    discoveredServices.records();
	    while (readyForTaggingServices.hasMoreElements())
	    {
		final ServiceRecord currentRecord =
			readyForTaggingServices.nextElement();
		_listener.tagDevice(currentRecord);
	    }

	    makeNewSearchRequest();
	}

    }

    private class RequestSearchTask
	    implements Runnable
    {
	public void run()
	{
	    _logger.log(Level.INFO, "Requesting Bluetooth search...");

	    if (_client == null)
	    {
		try
		{
		    _client = new BTClient(_clientListener);
		}
		catch (BluetoothStateException e)
		{
		    _logger.log(Level.WARNING,
				"Cannot initialize Bluetooth. Setting _client == null");
		    _client = null;
		    _listener.cannotInitializeBluetooth();
		}
	    }

	    if (_client != null)
		_client.requestSearch();

	    _logger.log(Level.INFO, "Request search thread ended.");
	}

    }

    private boolean _isTaggingOn;
    private BTClient _client;
    private BTClientListener _clientListener;
    private IDeviceDiscovererListener _listener;
    private RequestSearchTask _requestSearchTask;
    private WorkerThread _workerThread;

    /**
     * Creates a new instance of the <tt>DeviceDiscoverer</tt>. Uses
     * <tt>DefaultDeviceDiscovererListener</tt> as a <tt>Null Object</tt> listener.
     */
    public DeviceDiscoverer()
    {
	this(new DefaultDeviceDiscovererListener());
    }

    /**
     * Creates a new instance of the <tt>DeviceDiscoverer</tt> class.
     *
     * @param	listener	
     *		Specifies the instance that implements the IDeviceDiscovererListener
     *		interface and wants to be notified of DeviceDiscoverer related events.
     *		If the specified value is <code>null</code>, DefaultDeviceDiscovererListener
     *		is used instead.
     */
    public DeviceDiscoverer(IDeviceDiscovererListener listener)
    {
	if (listener == null)
	    listener = new DefaultDeviceDiscovererListener();

	_listener = listener;
	_clientListener = new BTClientListener();
	_workerThread = new WorkerThread();
    }

    public void destroy()
    {
	setTaggingOn(false);

	_listener = null;
	_clientListener = null;
	
	if (_workerThread != null)
	{
	    _workerThread.removeAllTasks();
	    _workerThread = null;
	}

	if (_client != null)
	{
	    _client.destroy();
	    _client = null;
	}
	
	_requestSearchTask = null;
    }

    /**
     * Sets the value indicating whether the device can initiate tagging process.
     */
    public void setTaggingOn(boolean value)
    {
	if (_isTaggingOn == value)
	    return;

	_logger.log(Level.INFO, "setTaggingOn = {0}", value);
	_isTaggingOn = value;

	if (value)
	{
	    _logger.log(Level.INFO, "Creating new RequestSearchTask...");
	    _requestSearchTask = new RequestSearchTask();
	    _workerThread.put(_requestSearchTask);
	}
	else
	{
	    if (_client != null)
		_client.cancelSearch();
	}
    }

    private void makeNewSearchRequest()
    {
	if (_isTaggingOn)
	{
	    try
	    {
		Thread.sleep(_requestSearchTimeOut);
	    }
	    catch (InterruptedException e)
	    {
		_logger.log(Level.SEVERE, "Worker thread cannot sleep.");
		e.printStackTrace();
	    }

	    if (_workerThread != null && _requestSearchTask != null)
		_workerThread.put(_requestSearchTask);
	}
    }

    private void printOutDiscoveredServices(ServiceRecordList discoveredServices)
    {
	if (discoveredServices.size() == 0)
	{
	    _logger.log(Level.INFO, "No services discovered.");
	    return;
	}

	_logger.log(Level.INFO, "Bluetooth Tagging Service discovered for:");

	IServiceRecordEnumeration enumeration = discoveredServices.records();

	while (enumeration.hasMoreElements())
	{
	    RemoteDevice device = enumeration.nextElement().getHostDevice();
	    _logger.log(Level.INFO, " * {0}", device);
	}
    }
}
