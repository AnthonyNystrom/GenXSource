/* ------------------------------------------------
 * ServiceTimeBucket.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark.wireless;

import javax.bluetooth.*;

/**
 * Associates a ServiceRecord to the time when it was discovered.
 *
 * @author Alex Nesterov
 */
public class ServiceTimeBucket
{
    private ServiceRecord _serviceRecord;
    
    /**
     * Returns the associated ServiceRecord.
     * @return	The associated ServiceRecord.
     */
    public ServiceRecord getServiceRecord()
    {
	return _serviceRecord;
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
     * Creates a new instance of ServiceTimeBucket.
     *
     * @param serviceRecord Specifies the ServiceRecord to associated with this
     *			    bucket.
     * @param discoveryTime Specifies the time when the service was discovered.
     */
    public ServiceTimeBucket(ServiceRecord serviceRecord, long discoveryTime)
    {
	_serviceRecord = serviceRecord;
	_discoveryTime = discoveryTime;
    }
}
