/* ------------------------------------------------------
 * App.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * --------------------------------------------------- */

package n2f.proxmark.wireless;

import genetibase.collections.StreamConnectionQueue;

import java.io.DataInputStream;

import javax.bluetooth.DiscoveryAgent;
import javax.bluetooth.LocalDevice;
import javax.microedition.io.Connector;
import javax.microedition.io.StreamConnection;
import javax.microedition.io.StreamConnectionNotifier;

/**
 * Represents a server for Bluetooth Tagging Service.
 *
 * @author Alex Nesterov
 */
public final class BTServer
	implements Runnable
{
    private static final String _url =
	    "btspp://localhost:" + BTService.PROXMARK_SERVICE_ID.toString() + ";name=" + BTService.PROXMARK_SERVICE_NAME + ";authorize=false";
    
     private class ClientProcessor
	    implements Runnable
    {
	/**
	 * Adds the specified connection to the queue and notifies the thread.
	 *
	 * @param   connection	Specifies the connection to add.
	 * @throws  NullPointerException if the specified <tt>connection</tt>
	 *	    is <code>null</code>.
	 */
	public void addConnection(StreamConnection connection)
	{
	    if (connection == null)
		throw new NullPointerException("connection");

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
	    System.out.println("ClientProcessor::destroy");
	    synchronized (this)
	    {
		System.out.println("ClientProcessor::destroy - _queue.size() = " + _queue.size());
		while (_queue.size() != 0)
		{
		    connection = _queue.dequeue();
		    BTService.closeConnection(connection);
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
				    "BTTagServer::run - Unexpected interruption: " + e);
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

	    System.out.println("ClientProcessor::run - Thread ended.");
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
    
    private ClientProcessor _clientProcessor;
    private volatile boolean _isClosed;
    private IBTServerListener _listener;
    private LocalDevice _localDevice;
    private StreamConnectionNotifier _notifier;
    private Thread _serverThread;
    
    /**
     * Creates a new instance of BTServer. Uses DefaultBTTagServerListener
     * as a Null Object listener.
     */
    public BTServer()
    {
	this(new DefaultBTServerListener());
    }

    /**
     * Creates a new instance of BTServer.
     *
     * @param listener	Specifies the instance that implements the
     *			IBTServerListener interface and wants to be notified
     *			of BTServer related events. If the specified value
     *			is <code>null</code>, DefaultBTTagServerListener is
     *			used instead.
     */
    public BTServer(IBTServerListener listener)
    {
	if (listener == null)
	    listener = new DefaultBTServerListener();

	_listener = listener;
    }
    
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
	    /* 
	     * Some implementations always return false,
	     * even if setDiscoverable successful.
	     */
	    }
	    System.out.println("BTServer::run - _url = " + _url);
	    _notifier =
		    (StreamConnectionNotifier)Connector.open(_url,
							     Connector.READ,
							     true);
	    System.out.println("BTServer::run - Opened server connection.");
	    isBTReady = true;
	}
	catch (Exception e)
	{
	    System.out.println("BTServer::run - Cannot initialize Bluetooth.");
	}

	if (!isBTReady)
	{
	    if (_listener != null)
		_listener.cannotInitializeBluetooth();
	    _isClosed = true;
	    return;
	}

	_clientProcessor = new ClientProcessor();

	while (!_isClosed)
	{
	    StreamConnection connection = null;

	    try
	    {
		connection = _notifier.acceptAndOpen();
		System.out.println("BTServer::run - ACCEPTED new connection.");
	    }
	    catch (Exception e)
	    {
		/* Wrong client or interrupted. Continue anyway. */
		System.out.println("BTServer::run - Cannot accept new connection due to EXCEPTION:");
		e.printStackTrace();
		_isClosed = true;
	    }

	    if (connection != null)
		_clientProcessor.addConnection(connection);
	}

	System.out.println("BTServer::run - Thread ended, _isClosed = true");
    }

    private volatile boolean _isStarted;

    /**
     * Starts the server. The device becomes available for tagging. If the
     * server is already started makes no effect.
     */
    public void start()
    {
	System.out.println("BTTagServer::start - _isStarted = " + _isStarted);

	if (!_isStarted)
	{
	    _isStarted = true;
	    System.out.println("BTTagServer::start - _isStarted = true, initializing...");
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
    
    private void destroy()
    {
	_isClosed = true;
	System.out.println("BTServer::destroy...");

	if (_clientProcessor != null)
	{
	    _clientProcessor.destroy();
	}
	_clientProcessor = null;

	if (_notifier != null)
	{
	    try
	    {
		System.out.println("BTServer::destroy - Trying to close notifier...");
		_notifier.close();
		System.out.println("BTServer::destroy - Notifier closed successfully.");
	    }
	    catch (Exception ignored)
	    {
		ignored.printStackTrace();
	    }
	    finally
	    {
		_notifier = null;
	    }
	}

	_serverThread = null;
	_listener = null;
	_localDevice = null;

	System.out.println("BTTagServer::destroy - Finished.");

    }

    private void initialize()
    {
	_clientProcessor = null;
	_isClosed = false;
	_localDevice = null;
	_notifier = null;

	System.out.println("BTServer::initialize - _serverThread = new Thread(this);");
	_serverThread = new Thread(this);

	System.out.println("BTServer::initialize - _serverThread.start();");
	_serverThread.start();
    }

    private void processConnection(StreamConnection connection)
    {
	if (connection == null)
	    return;

	System.out.println("BTServer::processConnection - New connection is being processed");

	DataInputStream in = null;
	String advertText = null;
	String advertImage = null;

	try
	{
	    in = connection.openDataInputStream();
	    advertText = in.readUTF();
	    System.out.println("BTServer::processConnection - advertText = " + advertText);
	    advertImage = in.readUTF();
	    System.out.println("BTServer::processConnection - advertImage read successfully.");
	    _listener.advertReceived(advertText, advertImage);
	}
	catch (Exception e)
	{
	    System.out.println("BTServer::processConnection - Cannot read from the stream.");
	    e.printStackTrace();
	}
	finally
	{
	    BTService.closeInputStream(in);
	    BTService.closeConnection(connection);
	    in = null;
	    connection = null;
	}
    }
}
