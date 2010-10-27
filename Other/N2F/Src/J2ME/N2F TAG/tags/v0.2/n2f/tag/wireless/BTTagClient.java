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
public final class BTTagClient implements DiscoveryListener, Runnable
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
    
    /**
     * Exit the accepting thread and close notifier.
     */
    public void destroy()
    {
    	System.out.println("BTTagClient::destroy()");
    	synchronized (this)
    	{
    		_isClosed = true;
    		isRunning = false;
    		inProcess = false;
    		inSearchDevices = false;
    		inSearchServices = false;
    		Debug.println("BTTagClient::destroy - _isClosed = true;");
    		notifyAll();
    	}
    	this._listener = null;
    	this._encryptor = null;
    	this._clientThread = null;
    	this._discoveryAgent = null;
    	
    	if (_discoveredDevices != null) _discoveredDevices.removeAllDevices();
    	_discoveredDevices = null;
    	
    	if (_discoveredServices != null) _discoveredServices.removeAllRecords();
    	_discoveredServices = null;
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
    		inSearchDevices = false;
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
    				//WAIT till the search thread's running
    				wait(1000);	
    			} catch (InterruptedException e) {
    				e.printStackTrace();
    			}	
    		}
    		System.out.println("BTTagClient::requestSearch:notify");
    		//TODO: do smth
    		inProcess = false;
    		inSearchDevices = true;
    		inSearchServices = true;
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
    	} catch (Exception e) {
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
    	} catch (Exception e) {
    		Debug.println("can't process request");
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
    		inSearchServices = false;
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
    	String remoteTagID = null;
    	DataInputStream in = null;
	
    	try
    	{
Debug.println("BTTagClient::tagDevice - URL from service record:"+url)	;
    		connection = (StreamConnection)Connector.open(url, Connector.READ_WRITE, true);
Debug.println("opened connection");
    		out = connection.openDataOutputStream();
Debug.println("opened OutputStream");
    		out.writeUTF(getTagID());
    		out.flush(); 
    		/* Notify the server that it can read client's tagID. */
    		/* Keep output stream open until the next write session. */
Debug.println("BTTagClient::tagDevice - Wrote tagId successfully");
    	} catch (Exception e) {
Debug.println("BTTagClient::tagDevice " + e.getMessage());
    		e.printStackTrace();
    		
    		BTTagService.closeOutputStream(out);
    		BTTagService.closeConnection(connection);
    		connection = null;
    		out = null;
    		
    		return false;
    	}
    	try
    	{
    		in = connection.openDataInputStream();
    		String tagValidationString = in.readUTF();
    		remoteTagID = in.readUTF();
Debug.println("BTTagClient::tagDevice - tagValidationString = " + tagValidationString);
    		TagKeeper.getInstance().write(remoteTagID, tagValidationString);
    	} catch (Exception e) {
Debug.println("BTTagClient::tagDevice - Cannot read from server " + url);
    		e.printStackTrace();
	    
    		BTTagService.closeInputStream(in);
    		BTTagService.closeOutputStream(out);
    		BTTagService.closeConnection(connection);
    		connection = null;
    		out = null;
    		in = null;
    		
    		return false;
    	}
	
    	try
    	{
    		String encryptedTagValidationString = null;
    		try
    		{
    			encryptedTagValidationString = _encryptor.encryptString(BTTagService.getRemoteValidationString(remoteTagID));
    		} catch (CryptoException e)	{
Debug.println("BTTagClient::tagDevice - Error occurred while encrypting Tag Validation String.");
    			e.printStackTrace();
    			encryptedTagValidationString = "";
    		}
	    
    		if (encryptedTagValidationString != null) {
    			out.writeUTF(encryptedTagValidationString);
    			out.flush();
    		}
    	} catch (Exception e) {
Debug.println("tagDevice - Cannot write to the output stream for " + url);
    		e.printStackTrace();
	    
    		BTTagService.closeInputStream(in);
    		BTTagService.closeOutputStream(out);
    		BTTagService.closeConnection(connection);
    		connection = null;
    		out = null;
    		in = null;
	    
    		return false;
    	}
	
    	BTTagService.closeInputStream(in);
    	BTTagService.closeOutputStream(out);
    	BTTagService.closeConnection(connection);
    	connection = null;
		out = null;
		in = null;
	
    	return true;

    }
    
    /* ------ Methods.Private ------ */
    
    private String getTagID()
    {
    	return App.getCurrentApp().getWebServiceIteractor().getTagId();
    	//"A9B6FC1F-2D48-43d1-BM64-6386A6619132";
    }
    
    /**
     * Prints out the contents of the specified <tt>remoteDevices</tt>
     * enumeration.
     */
    private static void printOutRemoteDevices(RemoteDeviceList remoteDevices)
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
    			System.out.println(" * " + BTTagService.getRemoteDeviceName(remoteDeviceEnumeration.nextElement()));
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
    			System.out.println(" * "+ BTTagService.getRemoteDeviceName(records.nextElement().getHostDevice()));
    		}
    	}
    }
    
    
    private synchronized void processRequest()
    {
    	while (!_isClosed)
    	{
    		_currentState = READY;
		
    		synchronized (this) {
    			while (inProcess)
    			{
    				System.out.println("WAIT in PROCESSREQUEST");
    				try
    				{
    					isRunning = true;
    					wait();	
    				} catch (InterruptedException interruptedException)	{
    					
    					interruptedException.printStackTrace();
//					_isClosed = true;
    				}
    			}
    			System.out.println("MOVE FROM WAIT in PROCESSREQUEST");
    			inProcess = true;
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
    		}
    		else if (_discoveredDevices !=null &&_discoveredDevices.size() == 0)
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
    			devicesToFilter.addBucket(new DeviceTimeBucket(newDevicesEnumeration.nextElement(), currentTime));
    		}
	    
    		_discoveredDevices.removeAllDevices();
    		_latestList.filterDiscoveredDevices(devicesToFilter);
    		IDeviceTimeBucketEnumeration filteredDevices = devicesToFilter.buckets();
	    
    		System.out.println("BTTagClient::processRequest - devicesToFilter.size (after filtering) = " + devicesToFilter.size());
	    
    		while (filteredDevices.hasMoreElements())	    
    		{
    			_discoveredDevices.addDevice(filteredDevices.nextElement().getRemoteDevice());
    		}		    
    		if (_discoveredDevices.size() == 0)
    		{
    			System.out.println("BTTagClient::processRequest - All devices filtered.");
    			_listener.clientServiceSearchCompleted(_discoveredServices);
    			continue;
    		}
	    
	    /* Start searching for services on the discovered Bluetooth devices. */
	    
    		if (_isClosed)
    			return;
    		if (!searchServices())
    		{
    			_listener.clientServiceSearchCompleted(_discoveredServices);
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
    		inSearchDevices = _discoveryAgent.startInquiry(DiscoveryAgent.GIAC, this);
    	}
    	catch (BluetoothStateException e)
    	{
    		System.out.println("BTTagClient::searchDevices - Inquiry failed: " + e);
    		inSearchDevices = false;
    	}
    	synchronized (this) 
    	{
    		while (inSearchDevices)
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
	    		_listener.clientCannotInitializeBluetooth();
	    		cancelSearch();
	    		return false;
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
    		
			try	{
				inSearchServices = true;	
       			_searchIDs[i] = _discoveryAgent.searchServices(null, _uuidSet, remoteDevice, BTTagClient.this);
    	    		
       		} catch (Exception e) {
       			e.printStackTrace();
       			Debug.println("BTTagClient::searchServices - Cannot search services for "
       			+ remoteDevice.getBluetoothAddress()+ " due to "+ e);
				_searchIDs[i] = -1;
				inSearchServices = false;
				serviceSearchCompleted(-1, -1);
			}
    		if (_searchIDs[i] != -1) {
    			isSearchStarted = true;
    			synchronized (this)
    			{
    				while(inSearchServices) {
    					System.out.println("==========wait in search services==========");
    					try
		 	    		 {
    						wait(); /* Until services are found. */
    						System.out.println("left wait !!!!!!!!!!!");
		 	    		 }
    					catch (InterruptedException e)
    					{
    						_discoveryAgent.cancelServiceSearch(_searchIDs[i]);
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
    private volatile boolean inProcess = true; 
    private volatile boolean inSearchDevices = true;
    private volatile boolean inSearchServices = true;
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
