/* ------------------------------------------------
 * RemoteDeviceHashtable.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark.wireless;

import java.util.Hashtable;
import javax.bluetooth.*;

import genetibase.collections.*;

/**
 * @author Alex Nesterov
 */
public class RemoteDeviceHashtable
{
    /* ------ Methods.Public ------ */
    
    /**
     * Clears this hashtable so that it contains no entries.
     */
    public void clear()
    {
	_internalHashtable.clear();
    }
    
    /**
     * Tests if some bluetooth address maps to the specified
     * <tt>remoteDevice</tt> in this hashtable. This operation is more expensive
     * than the <tt>containsKey</tt> method.
     *
     * @param remoteDevice Specifies the device to search for.
     * @return	<code>true</code> if some bluetooth address maps to the
     *		<tt>remoteDevice</tt> argument in this hashtable;
     *		<code>false</code> otherwise.
     * @throws	NullPointerException if the specified <tt>remoteDevice</tt> is
     *		<code>null</code>.
     */
    public boolean contains(RemoteDevice remoteDevice)
    {
	return _internalHashtable.contains(remoteDevice);
    }
    
    /**
     * Tests if the specified <tt>bluetoothAddress</tt> is a key in this hashtable.
     *
     * @param bluetoothAddress	Possible bluetooth address.
     * @return	<code>true</code> if the specified <tt>bluetoothAddress</tt> is
     *		a key in this hashtable; <code>false</code> otherwise.
     */
    public boolean containsKey(String bluetoothAddress)
    {
	return _internalHashtable.containsKey(bluetoothAddress);
    }
    
    /**
     * Returns an enumeration of the remote devices in this hashtable. Use the
     * IRemoteDeviceEnumeration methods on the returned object to fetch the
     * elements sequentially.
     *
     * @return Enumeration of the remote devices in this hashtable.
     */
    public IRemoteDeviceEnumeration elements()
    {
	return new RemoteDeviceEnumeration(_internalHashtable.elements());
    }
    
    /**
     * Returns the RemoteDevice to which the specified bluetooth address is
     * mapped in this hashtable.
     *
     * @param bluetoothAddress	Specifies the bluetooth address in the hashtable.
     * @return	The RemoteDevice to which the <tt>bluetoothAddress</tt> is
     *		mapped in this hashtable; <code>null</code> if the
     *		<tt>bluetoothAddress</tt> is not mapped to any RemoteDevice in
     *		this hashtable.
     */
    public RemoteDevice get(String bluetoothAddress)
    {
	return (RemoteDevice)_internalHashtable.get(bluetoothAddress);
    }
    
    /**
     * Tests if this hashtable maps no bluetooth addresses to remote devices.
     *
     * @return	<code>true</code> if this hashtable maps no bluetooth addresses
     *		to remote devices; <code>false</code> otherwise.
     */
    public boolean isEmpty()
    {
	return _internalHashtable.isEmpty();
    }
    
    /**
     * Returns an enumeration of the bluetooth addresses in this hashtable.
     *
     * @return Enumeration of the bluetooth addresses in this hashtable.
     */
    public IStringEnumeration keys()
    {
	return new StringEnumeration(_internalHashtable.keys());
    }
    
    /**
     * Maps the specified <tt>bluetoothAddress</tt> to the specified
     * RemoteDevice in this hashtable. Neither the key nor the value can be
     * <code>null</code>.
     *
     * @return	The previous RemoteDevice for the specified
     *		<tt>bluetoothAddress</tt> in this hashtable, or
     *		<code>null</code> if it did not have one.
     * @throws	NullPointerException if the <tt>bluetoothAddress</tt> or
     *		<tt>remoteDevice</tt> is <code>null</code>.
     */
    public RemoteDevice put(String bluetoothAddress, RemoteDevice remoteDevice)
    {
	return (RemoteDevice)_internalHashtable.put(bluetoothAddress, remoteDevice);
    }
    
    /**
     * Removes the <tt>bluetoothAddress</tt> (and the corresponding
     * RemoteDevice) from this hashtable. This method does nothing if the
     * specified <tt>bluetoothAddress</tt> is not in the hashtable.
     *
     * @param bluetoothAddress	The bluetooth address that needs to be removed.
     * @return	The RemoteDevice to which the <tt>bluetoothAddress</tt> had been
     *		mapped in this hashtable, or <code>null</code> if the
     *		<tt>bluetoothAddress</tt> did not have a mapping.
     */
    public RemoteDevice remove(String bluetoothAddress)
    {
	return (RemoteDevice)_internalHashtable.remove(bluetoothAddress);
    }
    
    /**
     * Returns the number of entries in this hashtable.
     *
     * @returns The number of entries in this hashtable.
     */
    public int size()
    {
	return _internalHashtable.size();
    }
    
    /**
     * Returns a rather long string representation of this hashtable.
     *
     * @returns String representation of this hashtable.
     */
    public String toString()
    {
	return _internalHashtable.toString();
    }
    
    /* ------ Declarations ------ */
    
    private Hashtable _internalHashtable;
    
    /* ------ Constructors ------ */
    
    /**
     * Creates a new instance of RemoteDeviceHashtable.
     */
    public RemoteDeviceHashtable()
    {
	_internalHashtable = new Hashtable();
    }
    
    /** Creates a new instance of StringHashtable.
     *
     * @param initialCapacity	Specifies the initial capacity for the hashtable.
     *
     * @throws	IllegalArgumentException if the specified
     *		<tt>initialCapacity</tt> is less than zero.
     */
    public RemoteDeviceHashtable(int initialCapacity)
    {
	_internalHashtable = new Hashtable(initialCapacity);
    }
}
