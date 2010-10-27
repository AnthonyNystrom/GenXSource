/* ------------------------------------------------
 * BTService.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark.wireless;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.util.logging.Level;
import java.util.logging.LogManager;
import java.util.logging.Logger;
import javax.bluetooth.RemoteDevice;
import javax.bluetooth.ServiceRecord;
import javax.bluetooth.UUID;
import javax.microedition.io.StreamConnection;

/**
 * Encapsulates ProxMark service data such as UUID and service name.
 * 
 * @author Alex Nesterov
 */
public final class BTService
{
    /** Describes this service. */
    public static final UUID PROXMARK_SERVICE_ID =
	    new UUID("55413750df7411dc95ff0800200c9a66", false);
    /** Defines a name for this service. */
    public static final String PROXMARK_SERVICE_NAME = "ProxMarkService";
    private static final Logger _logger =
	    Logger.getLogger(BTService.class.getName());

    static
    {
	LogManager.getLogManager().addLogger(_logger);
    }

    /**
     * This class is immutable.
     */
    private BTService()
    {
    }
    
    /**
     * Tests if the specified devices are associated with the same bluetooth address.
     *
     * @param x	Specifies the first RemoteDevice to compare.
     * @param y	Specifies the second RemoteDevice to compare.
     *
     * @return	<code>true</code> if both of the specified devices are
     *		<code>null</code>, or the devices are assocated with the same
     *		bluetooth address.
     */
    public static boolean areDevicesEqual(RemoteDevice x, RemoteDevice y)
    {
	if (x == null && y == null)
	    return true;
	if (x == null && y != null)
	    return false;
	if (x != null && y == null)
	    return false;

	boolean result = false;

	try
	{
	    result = x.getBluetoothAddress().equals(y.getBluetoothAddress());
	}
	catch (Exception e)
	{
	    result = false;
	}

	return result;
    }
    
    /**
     * Tests if the specified records are associated with the same device.
     *
     * @param x	Specifies the first ServiceRecord to compare.
     * @param y	Specifies the second ServiceRecord to compare.
     *
     * @return	<code>true</code> if both of the specified records are
     *		<code>null</code>, or the records are assocated with the same
     *		device.
     */
    public static boolean areServiceRecordsEqual(ServiceRecord x,
						   ServiceRecord y)
    {
	if (x == null && y == null)
	    return true;
	if (x == null && y != null)
	    return false;
	if (x != null && y == null)
	    return false;

	boolean result = false;

	try
	{
	    result = areDevicesEqual(x.getHostDevice(), y.getHostDevice());
	}
	catch (Exception e)
	{
	    result = false;
	}

	return result;
    }
    
    /**
     * Tries to close the specified <tt>connection</tt>. No exceptions are thrown.
     * @param	connection
     *		Specifies the <tt>StreamConnection</tt> to close.
     */
    public static void closeConnection(StreamConnection connection)
    {
	if (connection != null)
	{
	    try
	    {
		connection.close();
		_logger.log(Level.INFO, "Closed connection.");
	    }
	    catch (Exception ignored)
	    {
		_logger.log(Level.SEVERE, ignored.getMessage(), ignored);
	    }
	    finally
	    {
		connection = null;
	    }
	}
    }

    /**
     * Tries to close the specified <tt>inputStream</tt>. No exceptions are thrown.
     * @param	inputStream
     *		Specifies the DataInputStream to close.
     */
    public static void closeInputStream(DataInputStream inputStream)
    {
	if (inputStream != null)
	{
	    try
	    {
		inputStream.close();
		_logger.log(Level.INFO, "Closed input.");
	    }
	    catch (Exception ignored)
	    {
		_logger.log(Level.SEVERE, ignored.getMessage(), ignored);
	    }
	    finally
	    {
		inputStream = null;
	    }
	}
    }

    /**
     * Tries to close the specified <tt>outputStream</tt>. No exceptions are thrown.
     * @param	outputStream
     *		Specifies the DataOutputStream to close.
     */
    public static void closeOutputStream(DataOutputStream outputStream)
    {
	if (outputStream != null)
	{
	    try
	    {
		outputStream.close();
		_logger.log(Level.INFO, "Closed output.");
	    }
	    catch (Exception ignored)
	    {
		_logger.log(Level.SEVERE, ignored.getMessage(), ignored);
	    }
	    finally
	    {
		outputStream = null;
	    }
	}
    }

    /**
     * First invokes the <tt>getFriendlyName</tt> method on the specified
     * <tt>remoteDevice</tt>. If an IOException exception is thrown during
     * this operation, the <tt>getBluetoothAddress</tt> method is invoked
     * instead.
     *
     * @return The name for the specified <tt>remoteDevice</tt>.
     */
    public static String getRemoteDeviceName(RemoteDevice remoteDevice)
    {
	String name = null;

	try
	{
	    name = remoteDevice.getFriendlyName(true);
	}
	catch (IOException e)
	{
	    name = remoteDevice.getBluetoothAddress();
	    _logger.log(Level.WARNING,
			"Device friendly name cannot be retrieved. Will use --> {0}",
			name);
	}

	return name;
    }

}
