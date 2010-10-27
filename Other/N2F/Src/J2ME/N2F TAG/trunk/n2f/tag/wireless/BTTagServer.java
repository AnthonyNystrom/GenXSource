/* ------------------------------------------------
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.tag.wireless;

import genetibase.java.collections.StreamConnectionQueue;
import genetibase.java.security.CryptoException;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;

import javax.bluetooth.DataElement;
import javax.bluetooth.DiscoveryAgent;
import javax.bluetooth.LocalDevice;
import javax.bluetooth.ServiceRecord;
import javax.microedition.io.Connector;
import javax.microedition.io.PushRegistry;
import javax.microedition.io.StreamConnection;
import javax.microedition.io.StreamConnectionNotifier;

import n2f.tag.App;
import n2f.tag.core.Settings;
import n2f.tag.debug.Debug;
import n2f.tag.security.TagIDEncryptor;
import n2f.tag.ui.UIManager;

/**
 * Represents a server for Bluetooth Tagging Service.
 *
 * @author Alex Nesterov
 */
public final class BTTagServer implements Runnable
{
    private class ClientProcessor implements Runnable
    {
	/**
	 * Adds the specified connection to the queue and notifies the thread.
	 *
	 * @param connection	Specifies the connection to add.
	 * @throws  NullPointerException if the specified <tt>connection</tt>
	 *	    is <code>null</code>.
	 */
    	public void addConnection(StreamConnection connection)
    	{
    		if (connection == null)
    		{
    			throw new NullPointerException("connection");
    		}	
	    
    		synchronized (this)
    		{
    			_queue.enqueue(connection);
    			notifyAll();
    		}
    	}
	
	/**
	 * Closes the connections and destroys the processor thread.
	 */
    	public void destroy()
    	{
    		StreamConnection connection = null;
    		Debug.println("BTTAGSERVERS: ClientProcessor::destroy...");
    		synchronized (this)
    		{	
	    
    			System.out.println("QUEUE SIZE:"+_queue.size());
    			while (_queue.size() != 0)
    			{
    				connection = _queue.dequeue();
    				BTTagService.closeConnection(connection);
    			}
    			_isClosed = true;
    			notifyAll();
		
    		}
    	}
	
    	public void run()
    	{
    		while (!_isClosed)
    		{
		/* Wait for the new task to be processed. */
    			synchronized (this)
    			{
    				while (_queue.size() == 0)
    				{
    					try
    					{
    						wait();
    					}
    					catch (InterruptedException e)
    					{
    						System.out.println(
    								"BTTagServer::run - Unexpected interruption: "
    								+ e
    						);
    					}
    				}
    			}
		
    			StreamConnection connection = null;
    			
    			synchronized (this)
    			{
    				if (_isClosed)
    				{
    					return;
    				}
		    
    				if (!_queue.isEmpty())
    				{
    					connection = _queue.dequeue();
    					processConnection(connection);
    				}
    			}
    		}
    		Debug.println("CLIENT Processor ended2");
    	}
	
    	private Thread _processorThread;
    	private volatile StreamConnectionQueue _queue;
	
    	public ClientProcessor()
    	{
    		_queue = new StreamConnectionQueue();
    		_processorThread = new Thread(this);
    		_processorThread.start();
    	}
    }
    
    private void registerPushActivity() {
//    	 register push connections if not registered
    	Settings settings = App.getCurrentApp().getSettings();

    	if (settings.get("registeredPush") == null || "no".equals(settings.get("registeredPush"))) {
    		try {
    			PushRegistry.registerConnection(pushUrl, App.getCurrentApp().getClass().getName(), "*");
				settings.put("registeredPush", "yes");
				Debug.println("push has been registered at:"+pushUrl);
    		} catch( Exception e ) {
    			Debug.println("push registration failed:"+e.getMessage());
    			pushUrl = _url;
    			e.printStackTrace();
    			try {
					PushRegistry.registerConnection(pushUrl, App.getCurrentApp().getClass().getName(), "*");
					settings.put("registeredPush", "yes");
					Debug.println("push has been registered at:"+pushUrl);
				} catch (Exception e1)  {
					e1.printStackTrace();
					Debug.println("finally unable to register push activity");

				}
    			
    		}
		}
    }
    
    /* ------ Methods.Public ------ */
    
    /**
     * Runs the server portion of the Bluetooth Tagging Service.
     */
    public void run()
    {
    	if (!_isStarted)
    	{
    		return;
    	}
	
    	boolean isBTReady = false;
		
		try
		{
    		_localDevice = LocalDevice.getLocalDevice();
    	    
    		if (!_localDevice.setDiscoverable(DiscoveryAgent.GIAC))
    		{
    	/* Some implementations always return false,
		 * 	even if setDiscoverable successful.
		 */
    		}
    		Debug.println("_url for notifier:"+_url);
    		_notifier = (StreamConnectionNotifier)Connector.open(_url, Connector.READ_WRITE, true);
    		Debug.println("opened server connection");
    		_serviceRecord = _localDevice.getRecord(_notifier);
    		DataElement data = new DataElement(DataElement.STRING, App.getCurrentApp().getWebServiceIteractor().getTagId());
    		_serviceRecord.setAttributeValue(BTTagService.DEVICE_TAG_ID, data);
    		_localDevice.updateRecord(_serviceRecord);
    		isBTReady = true;
    	} catch (Exception e) {
    		System.out.println("BTTagServer::run - Cannot initialize Bluetooth.");
    	}
	
    	if (!isBTReady)
    	{
    		if (_listener != null)
    			_listener.serverCannotInitializeBluetooth();
    		_isClosed = true;
    		return;
    	}
	
    	_clientProcessor = new ClientProcessor();
    	// register push connections if not registered
    	registerPushActivity();
    	while (!_isClosed)
    	{	
    		StreamConnection connection = null;
    		try
    		{
    			connection = _notifier.acceptAndOpen();
    			Debug.println("ACCEPTED new connection");
    		}
    		catch (Exception e)
    		{
		/* Wrong client or interrupted. Continue anyway. */
    			Debug.println("!!can't accept new connection because of EXCEPTION:");
    			e.printStackTrace();
    			_isClosed = true;
    		}
    		if (connection != null)
    			_clientProcessor.addConnection(connection);
    	}
    	Debug.println("BTSERVER ENDS THREAD, closed=true");
	
    }
    
    private volatile boolean _isStarted;
    
    /**
     * Starts the server. The device becomes available for tagging. IF the
     * server is already started makes no effect.
     */
    public void start(IBTTagServerListener listener)
    {
	Debug.println("BTTagServer::start - _isStarted = " + _isStarted);
	
	if (!_isStarted)
	{
	    _isStarted = true;
	    if (_listener == null) {
	    	_listener = listener;
	    } 
	    if (_encryptor == null) {
	    	_encryptor = new TagIDEncryptor(App.getCurrentApp().getWebServiceIteractor().getEncryptionKey());
	    }
	    Debug.println("BTTagServer::start - _isStarted = TRUE now. Initializing...");
	    initialize();
	}
    }
    
    /**
     * Stops the server. The device becomes inavailable for tagging. If the
     * server is not started makes no effect.
     */
    public void stop()
    {
	if (_isStarted)
	{
	    _isStarted = false;
	    destroy();
	}
    }
    
    /* ------ Methods.Private ------ */
    
    private void destroy()
    {
	_isClosed = true;
	Debug.println("BTTagServer::destroy...");
  
	if (_clientProcessor != null)
	{
	    _clientProcessor.destroy();
	}
	_clientProcessor = null;
	
	if (_notifier != null)
	{
//		new Thread() {
//			public void run() {
				try
			    {
			    Debug.println("try to close notifier");
				_notifier.close();
				Debug.println("!!!!!!!!!closed notifier");
			    } catch (Exception ignored) {
			    	ignored.printStackTrace();
			    } finally {
			    	_notifier = null;
			    }		
//			}
//		}.start();
	    
	}
	_serverThread = null;
	
	this._encryptor = null;
	this._listener = null;
	this._serviceRecord = null; 
	this._localDevice = null;
	Debug.println("BTTagServer::destroy ended");
	
    }
    
    private String getTagID()
    {
	return App.getCurrentApp().getWebServiceIteractor().getTagId();//"A9B6FC1F-2C22-43d1-BD96-6058F6619ECE";
    }
    
    private void initialize()
    {
	_clientProcessor = null;
	_isClosed = false;
	_localDevice = null;
	_notifier = null;
	_serviceRecord = null;
	
	System.out.println("BTTagServer::initialize - _serverThread = new Thread(this);");
	_serverThread = new Thread(this);
	
	Debug.println("BTTagServer::initialize - _serverThread.start();");
	_serverThread.start();
    }
    
    private void processConnection(StreamConnection connection)
    {
	if (connection == null)
	{
	    return;
	}
	
Debug.println("BTTagServer::processConnection-New connection is being processed");
	
	DataInputStream in = null;
	String clientTagID = null;
	
	try
	{
	    in = connection.openDataInputStream();
	    clientTagID = in.readUTF();
Debug.println("BTTagServer::processConnection - clientTagID = " + clientTagID);
	}
	catch (Exception e)
	{
Debug.println("BTTagServer::processConnection - Cannot read from the stream.");
	    e.printStackTrace();
	    
	    BTTagService.closeInputStream(in);
	    BTTagService.closeConnection(connection);
	    in = null;
	    connection = null;
	    return;
	}
	
	DataOutputStream out = null;
	
	try
	{
	    out = connection.openDataOutputStream();
	    String encryptedTagValidationString = null;
	    
	    try
	    {
		encryptedTagValidationString = 
			_encryptor.encryptString(
			    BTTagService.getRemoteValidationString(clientTagID)
			    );
	    }
	    catch (CryptoException e)
	    {
Debug.println("BTTagServer::processConnection - Error occurred while encrypting Tag Validation String.");
		e.printStackTrace();
		encryptedTagValidationString = "";
	    } finally {
Debug.println("encrypted string:"+encryptedTagValidationString);
Debug.println("tag id string:"+getTagID());
	    }
	    out.writeUTF(encryptedTagValidationString);
	    out.writeUTF(getTagID());
	    out.flush();
Debug.println("BTTagServer::processConnection - Wrote tagID successfully");		    
	}
	catch (Exception e)
	{
Debug.println("BTTagServer::processConnection - Cannot write to client:"+e.getMessage());
	    e.printStackTrace();
	    BTTagService.closeInputStream(in);
	    BTTagService.closeOutputStream(out);
	    BTTagService.closeConnection(connection);
	    in = null;
	    out = null;
	    connection = null;
	    return;
	}
	
	try
	{
Debug.println("BTTagServer::processConnection - ready to read tagValidationString");				
	    String tagValidationString = in.readUTF();
Debug.println("BTTagServer::processConnection - tagValidationString = " + tagValidationString);
	    TagKeeper.getInstance().write(clientTagID, tagValidationString);
	}
	catch (Exception e)
	{
		System.out.println("BTTagServer::processConnection - Cannot read tagValidation from server.");
		e.printStackTrace();
	}
	finally
	{
	    BTTagService.closeInputStream(in);
	    BTTagService.closeOutputStream(out);
	    BTTagService.closeConnection(connection);
	    in = null;
	    out = null;
	    connection = null;
	}
    }
    
    /* ------ Declarations ------ */
    
    private static final String _url = "btspp://localhost:"
	    + BTTagService.TAG_SERVICE_ID.toString()
	    + ";name="
	    + BTTagService.TAG_SERVICE_NAME
	    + ";authorize=false"
	    ;
    
    private static String pushUrl = "btspp://:"
    + BTTagService.TAG_SERVICE_ID.toString()
    + ";name="
    + BTTagService.TAG_SERVICE_NAME
    + ";authorize=false"
    ;
    
    private ClientProcessor		_clientProcessor;
    private TagIDEncryptor		_encryptor;
    private volatile boolean	_isClosed;
    private IBTTagServerListener	_listener;
    private LocalDevice			_localDevice;
    private StreamConnectionNotifier	_notifier;
    private ServiceRecord		_serviceRecord;
    private Thread			_serverThread;
//    private String URL = _url;
    
    /* ------ Constructors ------ */
    
    /**
     * Creates a new instance of BTTagServer. Uses DefaultBTTagServerListener
     * as a Null Object listener.
     */
    public BTTagServer()
    {
	this(new DefaultBTTagServerListener());
    }
    
    /**
     * Creates a new instance of BTTagServer.
     *
     * @param listener	Specifies the instance that implements the
     *			IBTTagServerListener interface and wants to be notified
     *			of BTTagServer related events. If the specified value
     *			is <code>null</code>, DefaultBTTagServerListener is
     *			used instead.
     */
    public BTTagServer(IBTTagServerListener listener)
    {
	if (listener == null)
	{
	    listener = new DefaultBTTagServerListener();
	}
	
	_listener = listener;
//	_encryptor = new TagIDEncryptor(App.getCurrentApp().getWebServiceIteractor().getEncryptionKey());
    }
    
 	public boolean unregisterPushConnection() {
		boolean status = false;
		try {
			Debug.println("unregister connection:"+pushUrl);
			status = PushRegistry.unregisterConnection(pushUrl);
			App.getCurrentApp().getSettings().put("registeredPush", "no");
		} catch(SecurityException se) {
			Debug.println("SecurityException when ubregister push connection to "+pushUrl);
			se.printStackTrace();
		}
		return status;
	}
 	
 	
	public void processPushConnection(final String[] connections, final IBTTagServerListener listener) {
		new Thread() {
			public void run() {
				for (int i = 0; i < connections.length; i++) {
					Debug.println("connections[i] = " + connections[i]);
//					Debug.println("_url = " + _url);
					if (connections[i].startsWith("btspp://")) {
//						URL = connections[i]; 
						Debug.println("processPushConnection - _isStarted = " + _isStarted);
						
						if (!_isStarted)
						{
						    _isStarted = true;
						    if (_listener == null) {
						    	_listener = listener;
						    } 
						    if (_encryptor == null) {
						    	_encryptor = new TagIDEncryptor(App.getCurrentApp().getWebServiceIteractor().getEncryptionKey());
						    }
						    Debug.println("processPushConnection - _isStarted = TRUE now. Initializing...");
						    StreamConnection c = null;
							try
							{
//					    		Debug.println("url for notifier:"+URL);
					    		_notifier = (StreamConnectionNotifier)Connector.open(connections[i]);
					    		Debug.println("opened url for notifier");
					    		c = _notifier.acceptAndOpen();
					    		Debug.println("connection accepted!!!");
					    		
					    		UIManager.getInstance().showDefault();
					    	}
					    	catch (Exception e)
					    	{
					    		Debug.println("processPushConnection - Cannot initialize Bluetooth.");
					    	} finally {
					    		try {
					    			if (c!=null)
					    				c.close();
									Debug.println("closed connection");
								} catch (IOException e) {
									e.printStackTrace();
								}
								c = null;
					    		try {
					    			if (_notifier != null) {
					    				_notifier.close();	
									}
									
									Debug.println("closed notifier");
								} catch (IOException e) {
									e.printStackTrace();
								}
								_isStarted = false;
								_notifier = null;
								_encryptor = null;
								_listener = null;
					    	}
						}
						break;
					}
				}
			}
		}.start();
	}
}
