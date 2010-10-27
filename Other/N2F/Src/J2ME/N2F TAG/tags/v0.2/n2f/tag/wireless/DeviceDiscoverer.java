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
//	static int AWAKEN_CODE = 100;
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
	
	/*final */IServiceRecordEnumeration readyForTaggingServices = discoveredServices.records();
	tagDevices(discoveredServices, readyForTaggingServices);
	makeNewSearchRequest();
    }
   //TODO:1 
    private void tagDevices(/*final*/ ServiceRecordList discoveredServices,/* final */IServiceRecordEnumeration readyForTaggingServices) {
//    	if (wt != null) {
//    		wt.put(new RunnableTaskAdapter() {
//
//				protected void logic() throws Exception 
//				{
					while (readyForTaggingServices.hasMoreElements())
					{
			    	    final ServiceRecord currentRecord = readyForTaggingServices.nextElement();
			    	    _client.tagDevice(currentRecord);
					}

	
	Debug.println("::::::::::::finished tagging::::::::::");
//boolean restart = false;
//if (_isVisible && discoveredServices.size() > 0) {
//	restart = true;
//	_server.stop();
//}
//try {
//	wait(2000);
//} catch(Exception exc) {
//	exc.printStackTrace();
//}
//Debug.println("sever reconnects");
//if (_isVisible && restart) {
//	_server = new BTTagServer(DeviceDiscoverer.this);
//	_server.start(DeviceDiscoverer.this);
//   	}    	
//				}
//				public int getType() {
//					return 1021234;
//				}
//    			
//    		});	
//    	}
	
    }
    
    public void free()
    {
    	setVisible(false);
    	wt.free();
    	setTaggingOn(false);
    	if (_client != null) {
    		_client.destroy();
    	}
    	_client = null;
		_server = null; 	
    	
    	wt = null;
    	this._blockList = null;
    	this._listener = null;
    	
    }
    
    /**
     * Sets the value indicating whether the device is visible to other devices.
     */
    public void setVisible(boolean value)
    {
	if (_isVisible == value) return;
	Debug.println("DeviceDiscoverer::setVisible(" + value + ")");
	
	this._isVisible = value;
	
	if (value)
	{
	    Debug.println("DeviceDiscoverer::setVisible - Starting server...");
	    _server.start(this);
	} else {
		//TODO: this method blocks
				Debug.println("DeviceDiscoverer::setVisible - Stopping server...");
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
	
	public void unregisterPushConnection() {
		_server.unregisterPushConnection();
	}
	
	public void processPushConnection(String[] connections) {
		_server.processPushConnection(connections, this);
	}
}
