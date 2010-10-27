/* ------------------------------------------------
 * LatestList.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark.wireless;

import java.util.logging.Level;
import java.util.logging.LogManager;
import java.util.logging.Logger;

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
    private static final Logger _logger =
	    Logger.getLogger(LatestList.class.getName());
    private static final long _discoveryLatency = 60000L;

    static
    {
	LogManager.getLogManager().addLogger(_logger);
    }

    private DeviceTimeList _latestDeviceBuckets;

    /**
     * Creates a new instance of the <tt>LatestList</tt> class.
     */
    public LatestList()
    {
	_logger.log(Level.INFO, "LatestList CREATED.");
	_latestDeviceBuckets = new DeviceTimeList();
    }

    /**
     * Filters the devices which were discovered a short period of time ago to
     * avoid tag trashing.
     *
     * @param	discoveredDeviceBuckets
     *		Specifies the discovered devices to filter along with the time stamp
     *		indicating when the devices were discovered.
     * @throws	IllegalArgumentException
     *		If the specified <tt>discoveredDeviceBuckets</tt> is <code>null</code>.
     */
    public void filterDiscoveredDevices(DeviceTimeList discoveredDeviceBuckets)
    {
	if (discoveredDeviceBuckets == null)
	    throw new IllegalArgumentException("discoveredDeviceBuckets cannot be null.");

	filterOldBuckets();

	DeviceTimeBucket[] discoveredDeviceArray =
		new DeviceTimeBucket[discoveredDeviceBuckets.size()];
	discoveredDeviceBuckets.copyTo(discoveredDeviceArray);

	_logger.log(Level.INFO,
		    "discoveredDeviceBuckets.size() = {0}",
		    discoveredDeviceBuckets.size());
	_logger.log(Level.INFO, "Before traversing discovered devices --> _latestDeviceBuckets.size() = {0}", _latestDeviceBuckets.size());

	for (int i = 0; i < discoveredDeviceArray.length; i++)
	{
	    DeviceTimeBucket xBucket = discoveredDeviceArray[i];
	    DeviceTimeBucket[] likeXBuckets = getBucketsLike(xBucket);

	    if (likeXBuckets.length == 0)
	    {
		_logger.log(Level.INFO,
			    "{0} is considered a NEW device.",
			    xBucket.getRemoteDevice());
		_latestDeviceBuckets.addBucket(xBucket);
		_logger.log(Level.INFO, "_latestDeviceBuckets.size() = {0}", _latestDeviceBuckets.size());
		continue;
	    }

	    _logger.log(Level.INFO,
			"{0} is considered a LATEST device and is filtered.",
			xBucket.getRemoteDevice());
	    discoveredDeviceBuckets.removeBucket(xBucket);
	}
    }

    /**
     * Gets an array of DeviceTimeBucket instances with the service records
     * that map to the same device specified by the <tt>targetBucket</tt>.
     */
    private DeviceTimeBucket[] getBucketsLike(DeviceTimeBucket targetBucket)
    {
	DeviceTimeList buffer = new DeviceTimeList();
	IDeviceTimeBucketEnumeration enumeration =
		_latestDeviceBuckets.buckets();
	
	_logger.log(Level.INFO, "_latestDeviceBuckets.size() = {0}", _latestDeviceBuckets.size());

	while (enumeration.hasMoreElements())
	{
	    DeviceTimeBucket currentBucket = enumeration.nextElement();

	    if (BTService.areDevicesEqual(
		    currentBucket.getRemoteDevice(),
		    targetBucket.getRemoteDevice()))
	    {
		_logger.log(Level.INFO, "Devices are considered EQUAL. Adding to LIKE buckets...");
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
	_logger.log(Level.INFO, "Filtering old buckets...");
	_logger.log(Level.INFO, "_latestDeviceBuckets.size() = {0}", _latestDeviceBuckets.size());
	DeviceTimeBucket[] bucketsArray = new DeviceTimeBucket[_latestDeviceBuckets.size()];
	_latestDeviceBuckets.copyTo(bucketsArray);
	long currentMillis = System.currentTimeMillis();
	_logger.log(Level.FINE, "currentMillis = {0}", currentMillis);
	
	for (int i = 0; i < bucketsArray.length; i++)
	{
	    DeviceTimeBucket currentBucket = bucketsArray[i];
	    long discoveryTime = currentBucket.getDiscoveryTime();
	    _logger.log(Level.FINE, "currentBucket --> discoveryTime = {0}", discoveryTime);

	    if (currentMillis - discoveryTime > _discoveryLatency)
	    {
		_logger.log(Level.INFO,
			    "Old record removed: {0}",
			    currentBucket.getRemoteDevice().getBluetoothAddress());
		_latestDeviceBuckets.removeBucket(currentBucket);
	    }
	}
    }

}
