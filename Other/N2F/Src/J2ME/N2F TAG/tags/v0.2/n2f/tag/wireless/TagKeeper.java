/* ------------------------------------------------
 * TagWriter.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.tag.wireless;

import java.util.Enumeration;
import java.util.Hashtable;

import n2f.tag.App;
import n2f.tag.core.Deallocatable;
import n2f.tag.core.TagStorage;
import n2f.tag.webservice.stub.ArrayOfString;
import n2f.tag.webservice.stub.BTTagUpdate;

/**
 * Provides functionality to save tags locally on the divice.
 *
 * @author Alex Nesterov
 */
public class TagKeeper implements Deallocatable
{
    private TagStorage tagStorage;
    private volatile int numReads, numWrites;
    
    /* ------ Methods.Public ------ */
    public synchronized void write(String deviceTagID, String tagValidationString)
    {
	/*
	 * NOTE: Suppose synchronized keyword is needed if the server and client
	 * will intend to write a tag simultaneously.
	 */
	
	System.out.println("TagWriter::write(" + deviceTagID + ", " + tagValidationString + ")");

	tagStorage.write(deviceTagID, tagValidationString);
	numWrites++;
	numReads = 0;
    }
    
    public synchronized BTTagUpdate read()
    {
	BTTagUpdate btTagUpd = new BTTagUpdate();
	Hashtable table = tagStorage.readAllTags();
	String[] keys = new String[table.size()];
	String[] values = new String[table.size()];
	String key = null, value = null;
	int i = 0;
	for (Enumeration en = table.keys(); en.hasMoreElements();i++)
	{
	    key = (String) en.nextElement();
	    value = (String) table.get(key);
	    keys[i] = key;
	    values[i] = value;
//System.out.println("read:tagId"+keys[i]);
//System.out.println("read:syncId"+values[i]);
	}
	btTagUpd.setDeviceTagID(new ArrayOfString(keys));
	btTagUpd.setTagValidationString(new ArrayOfString(values));
	numReads++;
	numWrites = 0;
	return btTagUpd;
    }
    
    public synchronized boolean hasRecords()
    {
	return (numWrites>0) && (numReads==0);
    }
    
    /* ------ Methods.Public.Static ------ */
    
    private static TagKeeper _instance;
    private static final Object _lock = new Object();
    
    /**
     * Gets an instance of TagWriter.
     */
    public static TagKeeper getInstance()
    {
	if (_instance == null)
	{
	    synchronized (_lock)
	    {
		if (_instance == null)
		{
		    _instance = new TagKeeper();
		}
	    }
	}
	
	return _instance;
    }
    
    /* ------ Constructors ------ */
    
    /**
     * Creates a new instance of TagWriter.
     */
    private TagKeeper()
    {
	this.tagStorage = new TagStorage("-TagStorage-");
	App.getCurrentApp().addToDeallocatableList(this);
    }
    
    public synchronized void free()
    {
	this.tagStorage.free();
	this.tagStorage = null;
	_instance = null;
    }
}
