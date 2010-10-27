/* ------------------------------------------------
 * RemoteDeviceList.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark.wireless;

import java.util.Vector;
import javax.bluetooth.RemoteDevice;

/**
 * @author Alex Nesterov
 */
public class RemoteDeviceList
{
    /* ------ Methods.Public ------ */
    
    /**
     * Adds the specified <tt>remoteDevice</tt> to the end of this list, increasing
     * its size by one.
     *
     * @param remoteDevice  Specifies the RemoteDevice to add.
     */
    public void addDevice(RemoteDevice remoteDevice)
    {
	_internalList.addElement(remoteDevice);
    }
    
    /**
     * Tests if the specified <tt>remoteDevice</tt> is contained within this list.
     *
     * @return	<code>true</code> if the specified <tt>remoteDevice</tt> is contained
     *		within this list; otherwise, <code>false</code>.
     */
    public boolean contains(RemoteDevice remoteDevice)
    {
	return _internalList.contains(remoteDevice);
    }
    
    public RemoteDevice deviceAt(int index)
    {
	return (RemoteDevice)_internalList.elementAt(index);
    }
    
    /**
     * Returns an enumeration of remote devices in this list.
     * @return	An enumeration of remote devices in this list.
     */
    public IRemoteDeviceEnumeration devices()
    {
	return new RemoteDeviceEnumeration(_internalList.elements());
    }
    
    /**
     * Removes all the remote devices from this list and sets its size to 0.
     */
    public void removeAllDevices()
    {
	_internalList.removeAllElements();
    }
    
    /**
     * Returns the number of remote devices in the list.
     * @return	The number of remote devices in the list.
     */
    public int size()
    {
	return _internalList.size();
    }
    
    /* ------ Declarations ------ */
    
    private Vector _internalList;
    
    /* ------ Constructors ------ */
    
    /**
     * Creates a new instance of RemoteDeviceList.
     */
    public RemoteDeviceList()
    {
	_internalList = new Vector();
    }
}
