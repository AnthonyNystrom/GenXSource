/* ------------------------------------------------
 * LatestList.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.tag.wireless;


/**
 * Represents a class that keeps a list of discovered services along with
 * the time stamp when the services were discovered. Provides functionality
 * to filter discovered devices to get rid of tag trashing when the same
 * devices are discovered within a very little period of time.
 *
 * @author Alex Nesterov
 */
public class LatestList
{
    /* ------ Methods.Public ------ */
    
    /**
     * Filters the devices which were discovered a short period of time ago to
     * avoid tag trashing.
     *
     * @param discoveredDeviceBuckets	Specifies the discovered devices to
     *					filter along with the time stamp
     *					indicating when the devices were
     *					discovered.
     *
     * @throws	NullPointerException if the specified
     *		<tt>discoveredDeviceBuckets</tt> is <code>null</code>.
     */
    public void filterDiscoveredDevices(DeviceTimeList discoveredDeviceBuckets)
    {
	if (discoveredDeviceBuckets == null)
	{
	    throw new NullPointerException("discoveredDeviceBuckets");
	}
	
	filterOldBuckets();
	
//	IDeviceTimeBucketEnumeration discoveredEnum = discoveredDeviceBuckets.buckets();
	
	DeviceTimeBucket[] discoveredDeviceArray = new DeviceTimeBucket[discoveredDeviceBuckets.size()];
	discoveredDeviceBuckets.copyTo(discoveredDeviceArray);
	
	System.out.println(
		"LatestList::filterDiscoveredDevices - discoveredDeviceBuckets.size() = "
		+ discoveredDeviceBuckets.size()
		);
	
	for (int i = 0; i < discoveredDeviceArray.length; i++)
	{
	    DeviceTimeBucket xBucket = discoveredDeviceArray[i];
	    DeviceTimeBucket[] likeXBuckets = getBucketsLike(xBucket);
	    
	    if (likeXBuckets.length == 0)
	    {
		System.out.println("LatestList::filterDiscoveredDevices - "
			+ xBucket.getRemoteDevice()
			+ " is considered a NEW device.");
		_latestDeviceBuckets.addBucket(xBucket);
		continue;
	    }
	    
	    System.out.println(
		    "LatestList::filterDiscoveredDevices - "
		    + xBucket.getRemoteDevice()
		    + " is considered a LATEST device and is filtered."
		    );
	    discoveredDeviceBuckets.removeBucket(xBucket);
	}
    }
    
    /* ------ Methods.Private ------ */
    
    /**
     * Gets an array of DeviceTimeBucket instances with the service records
     * that map to the same device specified by the <tt>targetBucket</tt>.
     */
    private DeviceTimeBucket[] getBucketsLike(DeviceTimeBucket targetBucket)
    {
	DeviceTimeList buffer = new DeviceTimeList();
	IDeviceTimeBucketEnumeration enumeration = _latestDeviceBuckets.buckets();
	
	while (enumeration.hasMoreElements())
	{
	    DeviceTimeBucket currentBucket = enumeration.nextElement();
	    
	    if (BTTagService.areDevicesEqual(
		    currentBucket.getRemoteDevice()
		    , targetBucket.getRemoteDevice())
		    )
	    {
		buffer.addBucket(currentBucket);
	    }
	}
	
	DeviceTimeBucket[] array = new DeviceTimeBucket[buffer.size()];
	buffer.copyTo(array);
	return array;
    }
    
    /**
     * Filters old entries, i.e. not actually latest, those with the time stamp
     * older than 1 minute ago.
     */
    private void filterOldBuckets()
    {
	DeviceTimeBucket[] bucketsArray = new DeviceTimeBucket[_latestDeviceBuckets.size()];
	_latestDeviceBuckets.copyTo(bucketsArray);
	long currentMillis = System.currentTimeMillis();
	
	for (int i = 0; i < bucketsArray.length; i++)
	{
	    DeviceTimeBucket currentBucket = bucketsArray[i];
	    
	    if (currentMillis - currentBucket.getDiscoveryTime() > _discoveryLatency)
	    {
		System.out.println(
			"LatestList::filterOldBuckets - Old record removed: "
			+ currentBucket.getRemoteDevice().getBluetoothAddress()
			);
		_latestDeviceBuckets.removeBucket(currentBucket);
	    }
	}
    }
    
    /* ------ Declarations ------ */
    
    private static final long _discoveryLatency = 60000L;
    private DeviceTimeList _latestDeviceBuckets;
    
    /* ------ Constructors ------ */
    
    /**
     * Creates a new instance of LatestList.
     */
    public LatestList()
    {
	_latestDeviceBuckets = new DeviceTimeList();
    }
}
