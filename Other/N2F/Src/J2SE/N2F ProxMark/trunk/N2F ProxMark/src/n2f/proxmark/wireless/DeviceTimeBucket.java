/* ------------------------------------------------
 * DeviceTimeBucket.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark.wireless;

import javax.bluetooth.*;

/**
 * Associates a RemoteDevice to the time when it was discovered.
 *
 * @author Alex Nesterov
 */
public class DeviceTimeBucket
{
    private RemoteDevice _remoteDevice;
    
    /**
     * Returns the associated ServiceRecord.
     * @return	The associated ServiceRecord.
     */
    public RemoteDevice getRemoteDevice()
    {
	return _remoteDevice;
    }
    
    private long _discoveryTime;
    
    /**
     * Returns the time when the service was discovered.
     * @return	The time when the service was discovered.
     */
    public long getDiscoveryTime()
    {
	return _discoveryTime;
    }
    
    /**
     * Creates a new instance of the <tt>DeviceTimeBucket</tt> class.
     *
     * @param	remoteDevice 
     *		Specifies the RemoteDevice to associate with this bucket.
     * @param	discoveryTime
     *		Specifies the time when the device was discovered.
     */
    public DeviceTimeBucket(RemoteDevice remoteDevice, long discoveryTime)
    {
	_remoteDevice = remoteDevice;
	_discoveryTime = discoveryTime;
    }
}
