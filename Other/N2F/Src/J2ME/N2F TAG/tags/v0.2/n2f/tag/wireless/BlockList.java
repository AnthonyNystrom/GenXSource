/* ------------------------------------------------
 * BlockList.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.tag.wireless;

import java.util.Vector;
import javax.bluetooth.DataElement;
import javax.bluetooth.ServiceRecord;

/**
 * Represents a class that provides functionality to check if some of 
 * the discovered devices are in the block list and should be filtered.
 * 
 * @author Alex Nesterov
 */
public class BlockList
{
    /* ------ Methods.Public ------ */
    /**
     * Retrieves the devices associated with the specified
     * <tt>discoveredServices</tt> and checks if some of the devices are
     * in the block list and should be filtered.
     *
     * @param discoveredServices    Specifies the list of services to be filtered.
     * @throws	NullPointerException if the specified <tt>discoveredServices</tt>
     *		is <code>null</code>.
     */
    public void filterDiscoveredDevices(ServiceRecordList discoveredServices)
    {
	if (discoveredServices == null)
	{
	    throw new NullPointerException("discoveredServices");
	}
	
	IServiceRecordEnumeration records = discoveredServices.records();
	
	while (records.hasMoreElements())
	{
	    ServiceRecord currentRecord = records.nextElement();
	    DataElement data = currentRecord.getAttributeValue(BTTagService.DEVICE_TAG_ID);
	    
	    if (data == null)
	    {
		System.out.println("BlockList::filterDiscoveredServices - Cannot find DeviceTagID.");
		continue;
	    }
	    
	    String deviceTagID = (String)data.getValue();
	    
	    if (_deviceTagIDs.contains(deviceTagID))
	    {
		discoveredServices.removeRecord(currentRecord);
	    }
	}
    }

	
	
    /**
     * Retrieves the devices associated with the specified
     * <tt>discoveredServices</tt> and checks if some of the devices are
     * in the block list and should be filtered.
     *
     * @param discoveredServices    Specifies the list of services to be filtered.
     * @throws	NullPointerException if the specified <tt>discoveredServices</tt>
     *		is <code>null</code>.
     */
    public void filterDiscoveredServices(ServiceRecordList discoveredServices)
    {
	if (discoveredServices == null)
	{
	    throw new NullPointerException("discoveredServices");
	}
	
	IServiceRecordEnumeration records = discoveredServices.records();
	
	while (records.hasMoreElements())
	{
	    ServiceRecord currentRecord = records.nextElement();
	    DataElement data = currentRecord.getAttributeValue(BTTagService.DEVICE_TAG_ID);
	    
	    if (data == null)
	    {
		System.out.println("BlockList::filterDiscoveredServices - Cannot find DeviceTagID.");
		continue;
	    }
	    
	    String deviceTagID = (String)data.getValue();
	    
	    if (_deviceTagIDs.contains(deviceTagID))
	    {
		discoveredServices.removeRecord(currentRecord);
	    }
	}
    }
    
    /* ------ Declarations ------ */
    
    private Vector _deviceTagIDs;
    
    /* ------ Constructors ------ */
    
    /**
     * Creates a new instance of BlockList.
     */
    public BlockList(Vector blockVector)
    {
	_deviceTagIDs = new Vector();
    }
}
