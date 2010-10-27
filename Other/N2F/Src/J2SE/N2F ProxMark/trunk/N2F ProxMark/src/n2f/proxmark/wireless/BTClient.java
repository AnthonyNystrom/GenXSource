/* ------------------------------------------------
 * BTClient.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark.wireless;

import java.util.logging.Level;
import java.util.logging.LogManager;
import java.util.logging.Logger;
import javax.bluetooth.BluetoothStateException;
import javax.bluetooth.DeviceClass;
import javax.bluetooth.DiscoveryAgent;
import javax.bluetooth.DiscoveryListener;
import javax.bluetooth.LocalDevice;
import javax.bluetooth.RemoteDevice;
import javax.bluetooth.ServiceRecord;
import javax.bluetooth.UUID;

/**
 * Represents a client for ProxMark service.
 * 
 * @author Alex Nesterov
 */
public final class BTClient
	implements DiscoveryListener
{
    /**
     * Used as a prepared input parameter for the _discoveryAgent.searchServices
     * method. Allows to create an appropriate array only once.
     */
    private static final UUID[] _uuidSet = new UUID[] {
	BTService.PROXMARK_SERVICE_ID
    };
    private static final Logger _logger =
	    Logger.getLogger(BTClient.class.getName());
    /** Shows the engine is ready to work. */
    private static final int READY = 0;
    /** Shows the engine is searching Bluetooth devices. */
    private static final int DEVICE_SEARCH = 1;
    /** Shows the engine is searching Bluetooth services. */
    private static final int SERVICE_SEARCH = 1;

    static
    {
	LogManager.getLogManager().addLogger(_logger);
    }

    private class ClientTask
	    implements Runnable
    {
	public void run()
	{
	    try
	    {
		processRequest();
	    }
	    catch (Exception e)
	    {
		_logger.log(Level.SEVERE, "Cannot process request.", e);
	    }

	    _logger.log(Level.INFO, "Client thread ended.");
	}

    }

    private int _currentState;
    private Thread _clientThread;
    private DiscoveryAgent _discoveryAgent;
    private int _inquiryResult;
    private volatile boolean _isClosed;
    private volatile boolean _isRunning;
    private volatile boolean _inProcess = true;
    private volatile boolean _inSearchDevices = true;
    private volatile boolean _inSearchServices = true;
    private IBTClientListener _listener;
    private LatestList _latestList;
    private RemoteDeviceList _discoveredDevices;
    private ServiceRecordList _discoveredServices;
    /** Keeps the services search IDs (just to be able to cancel them). */
    private int[] _searchIDs;

    /**
     * Creates a new instance of the <tt>BTClient</tt> class.
     * Uses <tt>DefaultBTClientListener</tt> as a <tt>Null Object</tt> listener.
     * @throws	BluetoothStateException
     *		If DiscoveryAgent cannot be retrieved.
     */
    public BTClient() throws BluetoothStateException
    {
	this(new DefaultBTClientListener());
    }

    /**
     * Creates a new instance of the <tt>BTClient</tt> class.
     * 
     * @param	listener
     *		Specifies the instance that implements the <tt>IBTTagClientListener</tt>
     *		interface and wants to be notified of BTTagClient related events. If the
     *		specified value is <code>null</code>, <tt>DefaultBTClientListener</tt> is
     *		used instead.
     * @throws	BluetoothStateException
     *		If DiscoveryAgent cannot be retrieved.
     */
    public BTClient(IBTClientListener listener) throws BluetoothStateException
    {
	if (listener == null)
	    listener = new DefaultBTClientListener();
	
	_logger.log(Level.INFO, "BTClient CREATED.");

	_listener = listener;
	_currentState = READY;

	_discoveredDevices = new RemoteDeviceList();
	_discoveredServices = new ServiceRecordList();
	_latestList = new LatestList();

	_discoveryAgent = LocalDevice.getLocalDevice().getDiscoveryAgent();

	_clientThread = new Thread(new ClientTask());
	_clientThread.start();
    }

    /* ------ Methods.Public ------ */
    /**
     * If the client is searching devices, then device search is canceled.
     * If the client is searching services, then service search is canceled.
     * Nothing happens otherwise.
     */
    public void cancelSearch()
    {
	if (_currentState == DEVICE_SEARCH)
	{
	    _logger.log(Level.INFO, "_currentState == DEVICE_SEARCH");
	    _logger.log(Level.INFO, "_discoveryAgent.cancelInquiry(this)");
	    _discoveryAgent.cancelInquiry(this);
	}
	else if (_currentState == SERVICE_SEARCH)
	{
	    _logger.log(Level.INFO, "_currentState == SERVICE_SEARCH");
	    _logger.log(Level.INFO, "_searchIDs.length = {0}", _searchIDs.length);

	    for (int i = 0; i < _searchIDs.length; i++)
	    {
		_logger.log(Level.INFO,
			    "Cancelling search with transID = {0}.",
			    _searchIDs[i]);
		_discoveryAgent.cancelServiceSearch(_searchIDs[i]);
	    }
	}
    }

    /**
     * Exit the accepting thread and close notifier.
     */
    public void destroy()
    {
	synchronized (this)
	{
	    _isClosed = true;
	    _isRunning = false;
	    _inProcess = false;
	    _inSearchDevices = false;
	    _inSearchServices = false;
	    _logger.log(Level.INFO, "_isClosed = true");
	    notifyAll();
	}

	_listener = null;
	_clientThread = null;
	_discoveryAgent = null;

	if (_discoveredDevices != null)
	    _discoveredDevices.removeAllDevices();
	_discoveredDevices = null;

	if (_discoveredServices != null)
	    _discoveredServices.removeAllRecords();
	_discoveredServices = null;
    }

    public void deviceDiscovered(RemoteDevice remoteDevice,
				  DeviceClass deviceClass)
    {
	_discoveredDevices.addDevice(remoteDevice);
    }

    /**
     * Invoked when the DiscoveryAgent implementation finishes device discovery cycle.
     */
    public void inquiryCompleted(int inquiryResult)
    {
	_logger.log(Level.INFO, "inquiryResult = {0}", inquiryResult);
	_inquiryResult = inquiryResult;

	synchronized (this)
	{
	    _inSearchDevices = false;
	    notifyAll();
	}
    }

    /**
     * Sets the request to search devices/services.
     */
    public void requestSearch()
    {
	_logger.log(Level.INFO, "Search was requested.");

	synchronized (this)
	{
	    while (!_isRunning)
	    {
		try
		{
		    /* Wait till the search thread is running... */
		    wait(1000);
		}
		catch (InterruptedException e)
		{
		    _logger.log(Level.SEVERE, "Cannot be interrupted.", e);
		}
	    }

	    _logger.log(Level.INFO, "requestSearch --> Notify");

	    _inProcess = false;
	    _inSearchDevices = true;
	    _inSearchServices = true;

	    notifyAll();
	}
    }

    /**
     * Invoked by the DiscoveryAgent implementation when a service search finds services.
     *
     * @param	transID
     *		Identifies the service search that returned results.
     * @param	serviceRecords
     *		Holds records to the services found.
     */
    public void servicesDiscovered(int transID, ServiceRecord[] serviceRecords)
    {
	_logger.log(Level.INFO, "transID = {0}", transID);

	if (serviceRecords != null)
	{
	    int length = serviceRecords.length;
	    _logger.log(Level.INFO, "serviceRecords.length = {0}", length);

	    for (int i = 0; i < length; i++)
		_discoveredServices.addRecord(serviceRecords[i]);
	}
    }

    /**
     * Invoked by the DiscoveryAgent implementation when a service search finishes.
     *
     * @param	transID
     *		Identifies a particular service search.
     * @param	responseCode
     *		Indicates why the service search ended.
     */
    public void serviceSearchCompleted(int transID, int responseCode)
    {
	int index = -1;

	for (int i = 0; i < _searchIDs.length; i++)
	{
	    if (_searchIDs[i] == transID)
	    {
		index = i;
		break;
	    }
	}

	_logger.log(Level.INFO, "_searchIDs.length = {0}", _searchIDs.length);

	if (index == -1)
	    _logger.log(Level.INFO, "Unexpected transID: {0}", transID);
	else
	    _searchIDs[index] = -1;

	synchronized (this)
	{
	    _inSearchServices = false;
	    notifyAll();
	}
    }

    /**
     * Prints out the contents of the specified <tt>remoteDevices</tt>
     * enumeration.
     */
    private static void printOutRemoteDevices(RemoteDeviceList remoteDevices)
    {
	if (remoteDevices.size() == 0)
	{
	    _logger.log(Level.INFO, "No devices found.");
	}
	else
	{
	    _logger.log(Level.INFO, "Found devices:");
	    IRemoteDeviceEnumeration remoteDeviceEnumeration =
		    remoteDevices.devices();

	    while (remoteDeviceEnumeration.hasMoreElements())
	    {
		_logger.log(Level.INFO, " * {0}",
			    BTService.getRemoteDeviceName(remoteDeviceEnumeration.nextElement()));
	    }
	}
    }

    /**
     * Prints out the services found for Bluetooth devices.
     */
    private static void printOutServices(ServiceRecordList services)
    {
	if (services.size() == 0)
	{
	    _logger.log(Level.WARNING, "ProxMark service NOT found.");
	}
	else
	{
	    _logger.log(Level.INFO,
			"ProxMark services found for the following devices:");

	    IServiceRecordEnumeration records = services.records();

	    while (records.hasMoreElements())
	    {
		_logger.log(Level.INFO,
			    " * {0}",
			    BTService.getRemoteDeviceName(records.nextElement().
							  getHostDevice()));
	    }
	}
    }

    private synchronized void processRequest()
    {
	while (!_isClosed)
	{
	    _currentState = READY;

	    synchronized (this)
	    {
		while (_inProcess)
		{
		    _logger.log(Level.INFO, "Waiting in processRequest...");

		    try
		    {
			_isRunning = true;
			wait();
		    }
		    catch (InterruptedException e)
		    {
			_logger.log(Level.SEVERE, "Cannot be intrrupted.", e);
		    }
		}

		_logger.log(Level.INFO, "Move from wait() in processRequest.");
		_inProcess = true;
	    }

	    /* If the component is destroyed... */
	    if (_isClosed)
	    {
		return;
	    }

	    _discoveredDevices.removeAllDevices();
	    _discoveredServices.removeAllRecords();

	    /* Start searching for Bluetooth devices. */

	    if (!searchDevices())
	    {
		_listener.serviceSearchCompleted(_discoveredServices);
	    }
	    else if (_discoveredDevices != null && _discoveredDevices.size() == 0)
	    {
		_listener.serviceSearchCompleted(_discoveredServices);
		continue;
	    }

	    if (_isClosed)
	    {
		return;
	    }

	    /* Continue only if device search completed successfully. */

	    IRemoteDeviceEnumeration newDevicesEnumeration =
		    _discoveredDevices.devices();

	    /* Collect RemoteDevice instances along with time stamp for further filtering. */
	    DeviceTimeList devicesToFilter = new DeviceTimeList();
	    long currentTime = System.currentTimeMillis();

	    while (newDevicesEnumeration.hasMoreElements())
	    {
		devicesToFilter.addBucket(new DeviceTimeBucket(newDevicesEnumeration.nextElement(),
							       currentTime));
	    }

	    _discoveredDevices.removeAllDevices();
	    _latestList.filterDiscoveredDevices(devicesToFilter);
	    IDeviceTimeBucketEnumeration filteredDevices =
		    devicesToFilter.buckets();

	    _logger.log(Level.INFO,
			"devicesToFilter.size (after filtering) = {0}",
			devicesToFilter.size());

	    while (filteredDevices.hasMoreElements())
	    {
		_discoveredDevices.addDevice(filteredDevices.nextElement().
					     getRemoteDevice());
	    }
	    if (_discoveredDevices.size() == 0)
	    {
		_logger.log(Level.INFO, "All devices filtered.");
		_listener.serviceSearchCompleted(_discoveredServices);
		continue;
	    }

	    /* Start searching for services on the discovered Bluetooth devices. */

	    if (_isClosed)
		return;
	    if (!searchServices())
	    {
		_listener.serviceSearchCompleted(_discoveredServices);
	    }
	    else if (_discoveredServices.size() == 0)
	    {
		_listener.serviceSearchCompleted(_discoveredServices);
		continue;
	    }

	    _logger.log(Level.INFO, "processRequested ended.");
	    _listener.serviceSearchCompleted(_discoveredServices);
	}
    }

    /**
     * Search Bluetooth devices.
     *
     * @return	<code>true</code> if the operation completed successfully;
     *		<code>false</code> otherwise.
     */
    private boolean searchDevices()
    {
	_currentState = DEVICE_SEARCH;
	try
	{
	    _inSearchDevices = _discoveryAgent.startInquiry(DiscoveryAgent.GIAC,
							    this);
	}
	catch (BluetoothStateException e)
	{
	    _logger.log(Level.SEVERE, "Inquiry failed.", e);
	    _inSearchDevices = false;
	}
	synchronized (this)
	{
	    while (_inSearchDevices)
	    {
		_logger.log(Level.INFO, "Waiting in searchDevices...");

		try
		{
		    wait(); /* ... until devices are found. */
		}
		catch (InterruptedException e)
		{
		    _logger.log(Level.SEVERE, "Cannot be interrupted.", e);
		    return false;
		}
	    }

	    _logger.log(Level.INFO, "Move from wait() in searchDevices.");
	}

	/* This "wake up" may be caused by component destruction. */
	if (_isClosed)
	    return false;

	/* Check the return code otherwise... */
	switch (_inquiryResult)
	{
	    case INQUIRY_ERROR:
	    {
		_logger.log(Level.SEVERE, "INQUIRY_ERROR");
		_listener.cannotInitializeBluetooth();
		cancelSearch();
		return false;
	    }
	    case INQUIRY_TERMINATED:
	    {
		_logger.log(Level.SEVERE, "INQUIRY_TERMINATED");
		break;
	    }
	    case INQUIRY_COMPLETED:
	    {
		_logger.log(Level.INFO, "INQUIRY_COMPLETED");
		printOutRemoteDevices(_discoveredDevices);
		break;
	    }
	    default:
	    {
		/* Unexpected return code. */
		_logger.log(Level.SEVERE,
			    "Unexpected device discovery return code.");
		destroy();
		return false;
	    }
	}

	return true;
    }

    /**
     * Searches for BTTagService on the specified devices.
     *
     * @return	<code>true</code> if the operation completed successfully;
     * 		<code>false</code> otherwise.
     */
    private boolean searchServices()
    {
	_currentState = SERVICE_SEARCH;
	int discoveredDevicesCount = _discoveredDevices.size();
	_searchIDs = new int[discoveredDevicesCount];

	boolean isSearchStarted = false;

	for (int i = 0; i < discoveredDevicesCount; i++)
	{
	    final RemoteDevice remoteDevice = _discoveredDevices.deviceAt(i);

	    try
	    {
		_inSearchServices = true;
		_searchIDs[i] = _discoveryAgent.searchServices(null, _uuidSet,
							       remoteDevice,
							       BTClient.this);

	    }
	    catch (Exception e)
	    {
		e.printStackTrace();

		_logger.log(Level.SEVERE,
			    "Cannot search services for " + remoteDevice.getBluetoothAddress() + " due to " + e.getMessage() + ".",
			    e);
		_searchIDs[i] = -1;
		_inSearchServices = false;
		serviceSearchCompleted(-1, -1);
	    }
	    
	    if (_searchIDs[i] != -1)
	    {
		isSearchStarted = true;
		
		synchronized (this)
		{
		    while (_inSearchServices)
		    {
			_logger.log(Level.INFO, "Waiting in searchServices...");
			
			try
			{
			    wait(); /* Until services are found. */
			    _logger.log(Level.INFO, "Left wait.");
			}
			catch (InterruptedException e)
			{
			    _discoveryAgent.cancelServiceSearch(_searchIDs[i]);
			    return false;
			}
		    }
		    
		    _logger.log(Level.INFO, "Move from wait() in searchServices.");
		}
	    }
	}

	/* At least one of the services search should be found. */
	if (!isSearchStarted)
	{
	    _logger.log(Level.WARNING, "ProxMark service NOT found.");
	    return true;
	}

	/* This "wake up" may be caused by destroy call. */
	if (_isClosed)
	{
	    return false;
	}

	printOutServices(_discoveredServices);
	return true;
    }

}
