/* ------------------------------------------------
 * DeviceDiscoverer.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.tag.wireless;

import javax.bluetooth.RemoteDevice;
import javax.bluetooth.ServiceRecord;

import n2f.tag.App;
import n2f.tag.core.Deallocatable;
import n2f.tag.debug.Debug;
import n2f.tag.ui.UIManager;
import n2f.tag.webservice.utils.RunnableTaskAdapter;
import n2f.tag.webservice.utils.WorkerThread;

public class DeviceDiscoverer
	implements IBTTagClientListener, IBTTagServerListener, Deallocatable
{
    private class RequestSearchTask
	    extends RunnableTaskAdapter
    {
	
	protected void logic() throws Exception
	{
	    _client.requestSearch();
	}
	
	public int getType()
	{
	    return BLUETOOTH_DEVICE_SEARCH;
	}
    }
    
    private WorkerThread wt;
    
    
    /* ------ Methods.Public ------ */
    
    public void clientCannotInitializeBluetooth()
    {
	System.out.println("DeviceDiscoverer::clientCannotInitializeBluetooth");
	_listener.discovererCannotStartDeviceSearch();
    }
    
    public void clientCannotStartDeviceSearch()
    {
	System.out.println("DeviceDiscoverer::clientCannotStartDeviceSearch");
	_listener.discovererCannotStartDeviceSearch();
    }
    
    public void clientDeviceDiscoveryError()
    {
	System.out.println("DeviceDiscoverer::clientDeviceDiscoveryError");
    }
    
    public void clientServiceSearchCompleted(ServiceRecordList discoveredServices)
    {
	System.out.println("DeviceDiscoverer::clientServiceSearchCompleted");
	
	if (discoveredServices.size() == 0)
	{
	    makeNewSearchRequest();
	    return;
	}
	
	_blockList.filterDiscoveredServices(discoveredServices);
	printOutDiscoveredServices(discoveredServices);
	
	IServiceRecordEnumeration readyForTaggingServices = discoveredServices.records();

	/*boolean restart = false;
	if (discoveredServices.size() > 0) {
		restart = true;
		_server.stop();
	}*/
	while (readyForTaggingServices.hasMoreElements())
	{
	    ServiceRecord currentRecord = readyForTaggingServices.nextElement();
	    _client.tagDevice(currentRecord);
	}

	
	Debug.println("::::::::::::finished tagging::::::::::");
	/*if (restart) {
		_server = new BTTagServer(this);
		_server.start();
	}*/
	
	
	makeNewSearchRequest();
    }
    
    public void free()
    {
    	new Thread() {
    		public void run(){
    			setVisible(false);
    			_server = null; 	
    		}
    	}.start();
    	
    	wt.free();
    	setTaggingOn(false);
    	if (_client != null) {
    		_client.destroy();
    	}
    	_client = null;
    	wt = null;
//    	if (_server != null)
//    		_server.stop();
    	this._blockList = null;
    	this._listener = null;
    	
    }
    
    /**
     * Sets the value indicating whether the device is visible to other devices.
     */
    public void setVisible(boolean value)
    {
	if (_isVisible == value) return;
	System.out.println("DeviceDiscoverer::setVisible(" + value + ")");
	
	this._isVisible = value;
	
	if (value)
	{
	    System.out.println("DeviceDiscoverer::setVisible - Starting server...");
	    _server.start();
	} else {
	    System.out.println("DeviceDiscoverer::setVisible - Stopping server...");
	    _server.stop();
	}
    }
    
    /**
     * Sets the value indicating whether the device can initiate
     * tagging process.
     */
    public void setTaggingOn(boolean value)
    {
    if (_isTaggingOn == value) 
    	return;
	System.out.println("DeviceDiscoverer::setTaggingOn(" + value + ")");
	_isTaggingOn = value;
	
	if (value)
	{
	    System.out.println("DeviceDiscoverer::setTaggingOn - _requestSearchTask");
	    _requestSearchTask = new RequestSearchTask();
	    wt.put(_requestSearchTask);
	}
	else
	{
	    if (_requestSearchTask != null)
	    {
		wt.interruptTask();
	    }
	    _client.cancelSearch();
	}
    }
    
    public void serverCannotInitializeBluetooth()
    {
	System.out.println("DeviceDiscoverer::serverCannotInitializeBluetooth");
	_listener.discovererCannotStartTaggingService();
    }
    
    /* ------ Methods.Private ------ */
    
    private void printOutDiscoveredServices(ServiceRecordList discoveredServices)
    {
	System.out.println("DeviceDiscoverer::printOutDiscoveredServices");
	
	if (discoveredServices.size() == 0)
	{
	    System.out.println("No services discovered.");
	    return;
	}
	
	System.out.println("Bluetooth Tagging Service discovered for:");
	
	IServiceRecordEnumeration enumeration = discoveredServices.records();
	
	while (enumeration.hasMoreElements())
	{
	    RemoteDevice device = enumeration.nextElement().getHostDevice();
	    System.out.println(" * " + device);
	}
    }
    
    /* ------ Methods.Private ------ */
    
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
		System.out.println("Worker thread can't sleep!!");
		e.printStackTrace();
	    }
	    if (wt != null && _requestSearchTask !=null)
	    	wt.put(_requestSearchTask);
	}
    }
    
    /* ------ Declarations ------ */
    
    private static final long _requestSearchTimeOut = 5000L;
    
    private boolean _isTaggingOn;
    private boolean _isVisible;
    
//    private ServiceTimeList _discoveredServiceBuckets;
    private RequestSearchTask _requestSearchTask;
    
    private BTTagClient _client;
    private BTTagServer _server;
    
    private BlockList _blockList;
    private IDeviceDiscovererListener _listener;
    
    /* ------ Constructors ------ */
    
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
	{
	    listener = new DefaultDeviceDiscovererListener();
	}
	
	App.getCurrentApp().addToDeallocatableList(this);
	wt = new WorkerThread();
	wt.addErrorListener(UIManager.getInstance());
	_listener = listener;
	
	_blockList = new BlockList(App.getCurrentApp().getWebServiceIteractor().getBlockList());
//	_discoveredServiceBuckets = new ServiceTimeList();
	
	_client = new BTTagClient(this);
	_server = new BTTagServer(this);
	
	
    }
}
