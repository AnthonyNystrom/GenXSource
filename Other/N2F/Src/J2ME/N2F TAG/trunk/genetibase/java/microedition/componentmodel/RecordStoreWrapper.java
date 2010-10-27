/* ------------------------------------------------
 * RecordStoreWrapper.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.java.microedition.componentmodel;

import javax.microedition.rms.*;

import n2f.tag.core.Deallocatable;

import genetibase.java.collections.*;

/**
 * Provides basic functionality to read/write settings to/from Record Management System.
 *
 * @author Alex Nesterov
 */
public class RecordStoreWrapper implements Deallocatable
{
    /* ------ Methods.Public ------ */
    
    /**
     * Gets the value of the specified property.
     *
     * @param propertyName  Specifies the name of the property to get the
     *			    value for.
     * @return	The value of the specified property. If the entry for the
     *		specified property does not exist, <code>null</code> will be
     *		returned.
     * @throws	NullPointerException if <tt>propertyName</tt> is
     *		<code>null</code>.
     */
    public String get(String propertyName)
    {
	if (propertyName == null)
	{
	    throw new NullPointerException("propertyName");
	}
	
	return _hashtable.get(propertyName);
    }
    
    /**
     * Adds a new property or modifies the existing one.
     *
     * @param propertyName  Specifies the name of the property to add or
     *			    modify.
     * @param value	    Specifies the value for the property. If value
     *			    is <code>null</code>, it is replaced with an
     *			    empty string.
     *
     * @throws	NullPointerException if the specified <tt>propertyName</tt> is
     *		<code>null</code>.
     */
    public void put(String propertyName, String value)
    {
	if (value == null)
	{
	    value = "";
	}
	
	_hashtable.put(propertyName, value);
    }
    
    /**
     * Saves current settings to the Record Management Store.
     *
     * @throws	RecordStoreException if error occured while accessing the
     *		Record Management Store.
     */
    public void save() throws RecordStoreException
    {
	RecordStore recordStore = null;
	RecordEnumeration recordEnumeration = null;
	
	try
	{
	    recordStore = RecordStore.openRecordStore(_recordStoreName, true);
	    recordEnumeration = recordStore.enumerateRecords(null, null, false);
	    
	    /* Remove all records first. */
	    while (recordEnumeration.hasNextElement())
	    {
		int id = recordEnumeration.nextRecordId();
		recordStore.deleteRecord(id);
	    }
	    
	    /* Now save the preferences. */
	    IStringEnumeration keys = _hashtable.keys();
	    
	    while (keys.hasMoreElements())
	    {
		String key = keys.nextElement();
		String value = get(key);
		String entry = key + "=" + value;
		byte[] raw = entry.getBytes();
		recordStore.addRecord(raw, 0, raw.length);
	    }
	}
	finally
	{
	    if (recordEnumeration != null)
	    {
		recordEnumeration.destroy();
	    }
	    
	    if (recordStore != null)
	    {
		recordStore.closeRecordStore();
	    }
	}
    }
    
    /* ------ Methods.Private ----- */
    
    private void load() throws RecordStoreException
    {
	RecordStore recordStore = null;
	RecordEnumeration recordEnumeration = null;
	
	try
	{
	    recordStore = RecordStore.openRecordStore(_recordStoreName, true);
	    recordEnumeration = recordStore.enumerateRecords(null, null, false);
	    
	    while (recordEnumeration.hasNextElement())
	    {
		byte[] raw = recordEnumeration.nextRecord();
		String entry = new String(raw);
		
		/* Parse name and value for a preference entry. */
		int index = entry.indexOf('=');
		String name = entry.substring(0, index);
		String value = entry.substring(index + 1);
		put(name, value);
	    }
	}
	finally
	{
	    if (recordEnumeration != null)
	    {
		recordEnumeration.destroy();
	    }
	    
	    if (recordStore != null)
	    {
		recordStore.closeRecordStore();
	    }
	}
    }
    
    /* ------ Declarations ----- */
    
    private String _recordStoreName;
    private StringHashtable _hashtable;
    
    /* ------ Constructors ----- */
    
    /**
     * Creates a new instance of RecordStoreWrapper.
     *
     * @param recordStoreName	Specifies the unique name for the record
     *				store.
     * @throws	NullPointerException if the specified <tt>recordStoreName</tt>
     *		is <code>null</code>.
     * @throws	IllegalArgumentException if the specified
     *		<tt>recordStoreName</tt> is an empty string.
     */
    public RecordStoreWrapper(String recordStoreName) throws RecordStoreException
    {
	if (recordStoreName == null)
	{
	    throw new NullPointerException("recordStoreName");
	}
	
	if (recordStoreName == "")
	{
	    throw new IllegalArgumentException("recordStoreName");
	}
	
	_recordStoreName = recordStoreName;
	_hashtable = new StringHashtable(10);
	load();
    }

	public void free() {
		_hashtable.clear();
		_hashtable = null;
		
	}
}
