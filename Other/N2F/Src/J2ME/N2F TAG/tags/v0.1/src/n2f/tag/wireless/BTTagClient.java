/* ------------------------------------------------
 * BTTagClient.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.tag.wireless;

import genetibase.java.security.CryptoException;

import java.io.DataInputStream;
import java.io.DataOutputStream;

import javax.bluetooth.BluetoothStateException;
import javax.bluetooth.DeviceClass;
import javax.bluetooth.DiscoveryAgent;
import javax.bluetooth.DiscoveryListener;
import javax.bluetooth.LocalDevice;
import javax.bluetooth.RemoteDevice;
import javax.bluetooth.ServiceRecord;
import javax.bluetooth.UUID;
import javax.microedition.io.Connector;
import javax.microedition.io.StreamConnection;

import n2f.tag.App;
import n2f.tag.debug.Debug;
import n2f.tag.security.TagIDEncryptor;

/**
 * Represents a client for Bluetooth Tagging Service.
 *
 * @author Alex Nesterov
 */
public final class BTTagClient
	implements DiscoveryListener, Runnable
{
    /* ------ Methods.Public ------ */
    
    /**
     * If the client is searching devices, then device search is canceled.
     * If the client is searching services, then service search is canceled.
     * Nothing happens otherwise.
     */
    public void cancelSearch()
    {
	System.out.println("BTTagClient::cancelSearch()");
	
	synchronized (this)
	{
	    if (_currentState == DEVICE_SEARCH)
	    {
		System.out.println("BTTagClient::cancelSearch - _currentState == DEVICE_SEARCH");
		System.out.println("BTTagClient::cancelSearch - _discoveryAgent.cancelInqurity(this);");
		_discoveryAgent.cancelInquiry(this);
	    }
	    else if (_currentState == SERVICE_SEARCH)
	    {
		System.out.println("BTTagClient::cancelSearch - _currentState == SERVICE_SEARCH");
		System.out.println("BTTagClient::cancelSearch - _searchIDs.length = " + _searchIDs.length);
		
		for (int i = 0; i < _searchIDs.length; i++)
		{
		    System.out.println("BTTagClient::cancelSearch - Cancelling search with transID = " + _searchIDs[i]);
		    _discoveryAgent.cancelServiceSearch(_searchIDs[i]);
		}
	    }
	}
    }
    
    /**
     * Exit the accepting thread and close notifier.
     */
    public void destroy()
    {
	System.out.println("BTTagClient::destroy()");
	
	
	synchronized (this)
	{
	    System.out.println("BTTagClient::destroy - _isClosed = true;");
	    _isClosed = true;
	    isRunning = false;
	    waitProcess = true;
	    waitSearchDevices = true;
	    waitSearchServices = true;
	    
	    notifyAll();
	}
	if (_discoveryAgent != null) {
		try {
			cancelSearch();
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
	this._listener = null;
	this._encryptor = null;
	_discoveryAgent = null;
	this._clientThread = null;
	if (_discoveredDevices != null)
		this._discoveredDevices.removeAllDevices();
	this._discoveredDevices = null;
	if (_discoveredServices != null)
		this._discoveredServices.removeAllRecords();
	this._discoveredServices = null;
    }
    
    public void deviceDiscovered(RemoteDevice remoteDevice, DeviceClass deviceClass)
    {
	_discoveredDevices.addDevice(remoteDevice);
    }
    
    /**
     * Invoked when the DiscoveryAgent implementation finishes device discovery cycle.
     */
    public void inquiryCompleted(int inquiryResult)
    {
	System.out.println("BTTagClient::inquiryCompleted(" + inquiryResult + ")");
	_inquiryResult = inquiryResult;
	
	synchronized (this)
	{
		waitSearchDevices = false;
	    notifyAll();
	}
    }
    
    /**
     * Sets the request to search devices/services.
     */
    public void requestSearch()
    {
    System.out.println("BTTagClient::requestSearch:");
    synchronized(this) 
    {
    	while(!isRunning) {
        	try {
        		System.out.println("WAIT as thread's not running!");
        		wait(1000);	
        	} catch (InterruptedException e) {
        		// TODO Auto-generated catch block
        		e.printStackTrace();
        	}	
        }
    	System.out.println("BTTagClient::requestSearch:notify");
    	waitProcess = false;
	    notifyAll();
    }
    }
    
    /**
     * Runs the client portion of the Bluetooth Tagging Service.
     */
    public void run()
    {
	boolean bluetoothIsReady = false;
	
	try
	{
	    _discoveryAgent = LocalDevice.getLocalDevice().getDiscoveryAgent();
	    bluetoothIsReady = true;
	}
	catch (Exception e)
	{
	    System.out.println("BTTagClient::run - Cannot initialize Bluetooth.");
	}
	
	/* Nothing to do if the Bluetooth radio is not available. */
	if (!bluetoothIsReady)
	{
	    return;
	}
	
	try
	{
	    processRequest();
	}
	catch (Exception e)
	{
	    e.printStackTrace();
	}
	System.out.println("CLIENT THREAD ENDED!!!!!!!!!!!");
	
    }
    
    /**
     * Invoked by the DiscoveryAgent implementation when a service search finds services.
     *
     * @param transID	Identifies the service search that returned results.
     * @param serviceRecords	Holds records to the services found.
     */
    public void servicesDiscovered(int transID, ServiceRecord[] serviceRecords)
    {
	System.out.println("BTTagClient::servicesDiscovered(" + transID + ", ...)");
	
	if (serviceRecords != null)
	{
	    int length = serviceRecords.length;
	    System.out.println("BTTagClient::servicesDiscovered - length = " + length);
	    
	    for (int i = 0; i < length; i++)
	    {
		_discoveredServices.addRecord(serviceRecords[i]);
	    }
	}
    }
    
    /**
     * Invoked by the DiscoveryAgent implementation when a service search finishes.
     *
     * @param transID	Identifies a particular service search.
     * @param responseCode Indicates why the service search ended.
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
	
	System.out.println("BTTagClient::serviceSearchCompleted - _searchIDs.length = " + _searchIDs.length);
	
	if (index == -1)
	{
	    System.out.println("BTTagClient::serviceSearchCompleted - Unexpected transID: " + transID);
	}
	else
	{
	    _searchIDs[index] = -1;
	}
	
	synchronized (this)
	{
		waitSearchServices = false;
	    notifyAll();
	}
    }
    
    /**
     * Tags a remote device.
     * @param serviceRecord Specifies the the service to use for tagging.
     */
    public synchronized boolean tagDevice(ServiceRecord serviceRecord)
    {
	Debug.println(":::::::::::: tag device ::::::::::");
	String url = serviceRecord.getConnectionURL(ServiceRecord.NOAUTHENTICATE_NOENCRYPT, false);
	StreamConnection connection = null;
	DataOutputStream out = null;
	
	try
	{
	    Debug.println("BTTagClient::tagDevice - URL from service record:"+url)	;
	    connection = (StreamConnection)Connector.open(url);
	    out = connection.openDataOutputStream();
	    out.writeUTF(getTagID());
	    out.flush(); /* Notify the server that it can read client's tagID. */
	    /* Keep output stream open until the next write session. */
	    Debug.println("BTTagClient::tagDevice - Wrote tagId successfully");
	}
	catch (Exception e)
	{
	    Debug.println("BTTagClient::tagDevice - Cannot write to the output stream for " + url);
	    e.printStackTrace();
	    BTTagService.closeConnection(connection);
	    BTTagService.closeOutputStream(out);
	    
	    return false;
	}
	
	String remoteTagID = null;
	DataInputStream in = null;
	
	try
	{
	    in = connection.openDataInputStream();
	    String tagValidationString = in.readUTF();
	    remoteTagID = in.readUTF();
	    Debug.println("BTTagClient::tagDevice - tagValidationString = " + tagValidationString);
	    TagKeeper.getInstance().write(remoteTagID, tagValidationString);
	}
	catch (Exception e)
	{
	    Debug.println("BTTagClient::tagDevice - Cannot read from server " + url);
	    e.printStackTrace();
	    
	    BTTagService.closeInputStream(in);
	    BTTagService.closeOutputStream(out);
	    BTTagService.closeConnection(connection);
	    
	    return false;
	}
	
	try
	{
	    String encryptedTagValidationString = null;
	    
	    try
	    {
		encryptedTagValidationString =
			_encryptor.encryptString(
			BTTagService.getRemoteValidationString(remoteTagID)
			);
	    }
	    catch (CryptoException e)
	    {
		Debug.println("BTTagClient::tagDevice - Error occurred while encrypting Tag Validation String.");
		e.printStackTrace();
		encryptedTagValidationString = "";
	    }
	    
	    out.writeUTF(encryptedTagValidationString);
	    out.flush();
	}
	catch (Exception e)
	{
	    System.out.println("MainForm::tagDevice - Cannot write to the output stream for " + url);
	    e.printStackTrace();
	    
	    BTTagService.closeInputStream(in);
	    BTTagService.closeOutputStream(out);
	    BTTagService.closeConnection(connection);
	    
	    return false;
	}
	
	BTTagService.closeInputStream(in);
	BTTagService.closeOutputStream(out);
	BTTagService.closeConnection(connection);
	
	return true;
    }
    
    /* ------ Methods.Private ------ */
    
    private String getTagID()
    {
	return App.getCurrentApp().getWebServiceIteractor().getTagId();//"A9B6FC1F-2D48-43d1-BM64-6386A6619132";
    }
    
    /**
     * Prints out the contents of the specified <tt>remoteDevices</tt>
     * enumeration.
     */
    private static void printOutRemoteDevices(
	    RemoteDeviceList remoteDevices
	    )
    {
	if (remoteDevices.size() == 0)
	{
	    System.out.println("BTTagClient::searchDevices - No devices found.");	    
	}
	else
	{
	    System.out.println("BTTagClient::searchDevices - Found devices:");
	    IRemoteDeviceEnumeration remoteDeviceEnumeration = remoteDevices.devices();
	    
	    while (remoteDeviceEnumeration.hasMoreElements())
	    {
		System.out.println(
			" * " + BTTagService.getRemoteDeviceName(remoteDeviceEnumeration.nextElement())
			);
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
	    System.out.println("BTTagClient::searchServices - Bluetooth Tagging Service NOT found.");
	}
	else
	{
	    System.out.println("BTTagClient::serviceSearchCompleted - Services found for the following devices:");
	    
	    IServiceRecordEnumeration records = services.records();
	    
	    while (records.hasMoreElements())
	    {
		System.out.println(
			" * "
			+ BTTagService.getRemoteDeviceName(records.nextElement().getHostDevice())
			);
	    }
	}
    }
    
    
    private synchronized void processRequest()
    {
	while (!_isClosed)
	{
	    _currentState = READY;
		
		synchronized (this) {
			while (waitProcess)
			{
				System.out.println("WAIT in PROCESSREQUEST");
				try
				{
					isRunning = true;
					wait();	
				} catch (InterruptedException interruptedException)	{
					System.out.println(
							"BTTagClient::processRequest - Unexpected interruption: "
							+ interruptedException
					);
//					_isClosed = true;
				}
			}
			System.out.println("MOVE FROM WAIT in PROCESSREQUEST");
			waitProcess = true;
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
		_listener.clientServiceSearchCompleted(_discoveredServices);
//		_isClosed = true;
//		return;
	    }
	    else if (_discoveredDevices.size() == 0)
	    {
		_listener.clientServiceSearchCompleted(_discoveredServices);
		continue;
	    }
	    
	    if (_isClosed)
	    {
		return;
	    }
	    
	    /* Continue only if device search completed successfully. */
	    
	    IRemoteDeviceEnumeration newDevicesEnumeration = _discoveredDevices.devices();
	    
	    /* Collect RemoteDevice instances along with time stamp for further filtering. */
	    DeviceTimeList devicesToFilter = new DeviceTimeList();
	    long currentTime = System.currentTimeMillis();
	    
	    while (newDevicesEnumeration.hasMoreElements())
	    {
		devicesToFilter.addBucket(
			new DeviceTimeBucket(newDevicesEnumeration.nextElement(), currentTime)
			);
	    }
	    
	    _discoveredDevices.removeAllDevices();
	    _latestList.filterDiscoveredDevices(devicesToFilter);
	    IDeviceTimeBucketEnumeration filteredDevices = devicesToFilter.buckets();
	    
	    System.out.println(
		    "BTTagClient::processRequest - devicesToFilter.size (after filtering) = "
		    + devicesToFilter.size()
		    );
	    
	    while (filteredDevices.hasMoreElements())
	    {
		_discoveredDevices.addDevice(
			filteredDevices.nextElement().getRemoteDevice()
			);
	    }
	    
	    if (_discoveredDevices.size() == 0)
	    {
		System.out.println("BTTagClient::processRequest - All devices filtered.");
		_listener.clientServiceSearchCompleted(_discoveredServices);
		continue;
	    }
	    
	    /* Start searching for services on the discovered Bluetooth devices. */
	    
	    if (!searchServices())
	    {
		_listener.clientServiceSearchCompleted(_discoveredServices);
		System.out.println("!!!!!!!!!NOT SEARCHSERVICES!!!!!!!!!");
	    }
	    else if (_discoveredServices.size() == 0)
	    {
		_listener.clientServiceSearchCompleted(_discoveredServices);
		continue;
	    }
	    System.out.println("END PROCESS REQUEST ON CLIENT");
	    _listener.clientServiceSearchCompleted(_discoveredServices);
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
		waitSearchDevices = true;	
	    _discoveryAgent.startInquiry(DiscoveryAgent.GIAC, this);
	}
	catch (BluetoothStateException e)
	{
	    System.out.println("BTTagClient::searchDevices - Inquiry failed: " + e);
	    return true;
	}
	synchronized (this) 
	{
		while (waitSearchDevices)
		{
			System.out.println("WAIT in SEARCHDEVICES");
			try
			{
			    wait(); /* ... until devices are found. */
			}
			catch (InterruptedException e)
			{
			    System.out.println("BTTagClient::searchDevices - Interrupted: " + e);
			    return false;
			}	
		}
	    System.out.println("MOVE FROM WAIT in SEARCHDEVICES");;

	}
	
	
	/* This "wake up" may be caused by component destruction. */
	if (_isClosed)
	{
	    return false;
	}
	
	/* Check the return code otherwise... */
	switch (_inquiryResult)
	{
	    case INQUIRY_ERROR:
	    {
		Debug.println("BTTagClient::searchDevices - Device discovering error...");
		break;
	    }
	    case INQUIRY_TERMINATED:
	    {
		Debug.println("BTTagClient::searchDevices - Discovery terminted.");
		break;
	    }
	    case INQUIRY_COMPLETED:
	    {
		printOutRemoteDevices(_discoveredDevices);
		break;
	    }
	    default:
	    {
		/* Unexpected return code. */
		Debug.println("BTTagClient::searchDevices - Unexpected device discovery return code.");
		destroy();
		return false;
	    }
	}
	
	return true;
    }
    
    /**
     * Searches for BTTagService on the specified devices.
     *
     * @return <code>true</code> if the operation completed successfully;
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
	    final int ii = i;
	    new Thread() {
	    	public void run() {
	    		try
	    	    {
	    		waitSearchServices = true;	
	    		_searchIDs[ii] = _discoveryAgent.searchServices(null, _uuidSet, remoteDevice, BTTagClient.this);
	    		
	    	    } catch (Exception e) {
	    		Debug.println("BTTagClient::searchServices - Cannot search services for "
	    			+ remoteDevice.getBluetoothAddress()
	    			+ " due to "
	    			+ e
	    			);
	    		_searchIDs[ii] = -1;
//	    		continue;
	    	    }
	    	}
	    }.start();
		
	    
	    if (_searchIDs[ii] != -1) {
	    	isSearchStarted = true;
	    	 synchronized (this)
	    	 {
	    		 while(waitSearchServices) {
	    			 System.out.println("WAIT in SEARCHSERVICES");
	    			 try
		 	    	 {
		 	    		wait(); /* Until services are found. */
		 	    		
		 	    	 }
		 	    	 catch (InterruptedException e)
		 	    	 {
		 	    		_discoveryAgent.cancelServiceSearch(_searchIDs[i]);
		 	    		System.out.println("BTTagClient::searchServices - Unexpected interruption: " + e);
		 	    		return false;
		 	    	 }
	    		 } 
	    		 System.out.println("MOVE FROM WAIT in SEARCHSERVICES");
	 	    }
	 	}
	}
	
	/* At least one of the services search should be found. */
	if (!isSearchStarted)
	{
	    System.out.println("BTTagClient::searchServices - Bluetooth Tagging Service NOT found.");
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
    
    /* ------ Declarations ------ */
    
    /**
     * Used as a prepared input parameter for the _discoveryAgent.searchServices
     * method. Allows to create an appropriate array only once.
     */
    private static final UUID[] _uuidSet = new UUID[]
    {
	BTTagService.TAG_SERVICE_ID
    };
    
    /** Shows the engine is ready to work. */
    private static final int READY = 0;
    
    /** Shows the engine is searching Bluetooth devices. */
    private static final int DEVICE_SEARCH = 1;
    
    /** Shows the engine is searching Bluetooth services. */
    private static final int SERVICE_SEARCH = 1;
    
    private int			    _currentState;
    private Thread		    _clientThread;
    private DiscoveryAgent	    _discoveryAgent;
    private int			    _inquiryResult;
    private volatile boolean	_isClosed;
    private volatile boolean	isRunning;
    private volatile boolean waitProcess = true; 
    private volatile boolean waitSearchDevices = true;
    private volatile boolean waitSearchServices = true;
    private IBTTagClientListener    _listener;
    private LatestList		    _latestList;
    private RemoteDeviceList	    _discoveredDevices;
    private ServiceRecordList	    _discoveredServices;
    private TagIDEncryptor	    _encryptor;
    
    /** Keeps the services search IDs (just to be able to cancel them). */
    private int[] _searchIDs;
    
    /* ------ Constructors ------ */
    
    /**
     * Creates a new instance of BTTagClient. Uses DefaultBTTagClientListener
     * as a Null Object listener.
     */
    public BTTagClient()
    {
	this(new DefaultBTTagClientListener());
    }
    
    /**
     * Creates a new instance of BTTagClient.
     *
     * @param listener	Specifies the instance that implements the
     *			IBTTagClientListener interface and wants to be notified
     *			of BTTagClient related events. If the specified value
     *			is <code>null</code>, DefaultBTTagClientListener is
     *			used instead.
     */
    public BTTagClient(IBTTagClientListener listener)
    {
	if (listener == null)
	{
	    listener = new DefaultBTTagClientListener();
	}
	
	_encryptor = new TagIDEncryptor(App.getCurrentApp().getWebServiceIteractor().getEncryptionKey());
	
	_listener = listener;
	_currentState = READY;
	
	_discoveredDevices  = new RemoteDeviceList();
	_discoveredServices = new ServiceRecordList();
	_latestList	    = new LatestList();
	
	_clientThread = new Thread(this);
	_clientThread.start();
    }
}
