/* ------------------------------------------------
 * BTTagService.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.tag.wireless;

import genetibase.java.Argument;
import genetibase.java.DateConverter;
import java.io.DataInputStream;
import java.io.DataOutputStream;

import java.io.IOException;

import javax.bluetooth.RemoteDevice;
import javax.bluetooth.ServiceRecord;
import javax.bluetooth.UUID;
import javax.microedition.io.StreamConnection;

/**
 * Ecapuslates Bluetooth Tagging Service data, such as UUID and service name.
 *
 * @author Alex Nesterov
 */
public final class BTTagService
{
    /* ------ Methods.Public.Static ------ */
    
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
	{
	    return true;
	}
	
	if (x == null && y != null)
	{
	    return false;
	}
	
	if (x != null && y == null)
	{
	    return false;
	}
	
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
    public static boolean areServiceRecordsEqual(ServiceRecord x, ServiceRecord y)
    {
	if (x == null && y == null)
	{
	    return true;
	}
	
	if (x == null && y != null)
	{
	    return false;
	}
	
	if (x != null && y == null)
	{
	    return false;
	}
	
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
     * Tests if the specified <tt>tagID</tt> is a valid Tagging ID.
     *
     * @return	<code>true</code> if the specified <tt>tagID</tt> is a valid
     *		Tagging ID; <code>false</code> otherwise.
     */
    public static boolean checkTagID(String tagID)
    {
	if (Argument.isNullOrEmpty(tagID))
	{
	    return false;
	}
	
	if (tagID.length() < 25)
	{
	    return false;
	}
	
	int[] values = new int[]
	{
	    getValueForChar(tagID.charAt(0))
	    , getValueForChar(tagID.charAt(1))
	    , getValueForChar(tagID.charAt(9))
	    , getValueForChar(tagID.charAt(19))
	    , getValueForChar(tagID.charAt(24))
	};
	
	if (values[0] + values[1] == 19)
	{
	    return values[2] + values[3] + values[4] == 19;
	}
	
	return false;
    }
    
    /**
     * Tries to close the specified <tt>connection</tt>. No exceptions are thrown.
     * @param connection    Specifies the StreamConnection to close.
     */
    public static void closeConnection(StreamConnection connection)
    {
	try
	{
	    connection.close();
	}
	catch (Exception ignored)
	{
	}
    }
    
    /**
     * Tries to close the specified <tt>inputStream</tt>. No exceptions are thrown.
     * @param inputStream   Specifies the DataInputStream to close.
     */
    public static void closeInputStream(DataInputStream inputStream)
    {
	try
	{
	    inputStream.close();
	}
	catch (Exception ignored)
	{
	}
    }
    
    /**
     * Tries to close the specified <tt>outputStream</tt>. No exceptions are thrown.
     * @param outputStream  Specifies the DataOutputStream to close.
     */
    public static void closeOutputStream(DataOutputStream outputStream)
    {
	try
	{
	    outputStream.close();
	}
	catch (Exception ignored)
	{
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
	catch (IOException ioException)
	{
	    name = remoteDevice.getBluetoothAddress();
	}
	
	return name;
    }
    
    /**
     * Returns Tag Validation String for the specified <tt>remoteTagID</tt>.
     * @return	Tag Validation String for the specified <tt>remoteTagID</tt>.
     */
    public static String getRemoteValidationString(String remoteTagID)
    {
	long ticksInNow = DateConverter.getDotNetTicks(System.currentTimeMillis());
	return remoteTagID + Long.toString(ticksInNow);
    }
    
    private static int getValueForChar(char c)
    {
	return Character.digit(c, 16);
    }
    
    /* ------ Declarations ------ */
    
    /**
     * Describes this service.
     */
    public static final UUID TAG_SERVICE_ID = new UUID("57ad9d629fc011dc83140800200c9a66", false);
    
    /**
     * Defines a name for this service.
     */
    public static final String TAG_SERVICE_NAME = "BTTagService";
    
    /**
     * Defines the id of the record item with DeviceTag identifier.
     */
    public static final int DEVICE_TAG_ID = 0x0003;
    
    /* ------ Constructors ------ */
    
    /**
     * This class is immutable.
     */
    private BTTagService()
    {
    }
}
