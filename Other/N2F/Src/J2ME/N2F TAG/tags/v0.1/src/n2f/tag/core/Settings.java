/* ------------------------------------------------
 * Settings.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.tag.core;

import javax.microedition.rms.RecordStoreException;

import n2f.tag.App;

import genetibase.java.Argument;
import genetibase.java.microedition.componentmodel.*;

/**
 * Manages the application settings.
 */
public final class Settings implements Deallocatable
{
    /* ------ Methods.Public ------ */
    
    /*
     * Synchronize
     */
    
    private static final String _synchronizeKey = "Synchronize";
    
    /**
     * Returns synchronization mode.
     * @return Synchronization mode.
     */
    public int getSynchronize()
    {
	String synchronizeValue = get(_synchronizeKey);
	
	if (Argument.isNullOrEmpty(synchronizeValue))
	{
	    return 0;
	}
	
	int value = 0;
	
	try
	{
	    value = Integer.parseInt(synchronizeValue);
	}
	catch (Exception e)
	{
	}
	
	return value;
    }
    
    /**
     * Sets synchronization mode.
     * @param value Specifies synchronization mode.
     */
    public void setSynchronize(int value)
    {
	put(_synchronizeKey, Integer.toString(value));
    }
    
    /**
     * Sets synchronization mode.
     * @param value Specifies synchronization mode.
     */
    public void setHaveCredentials()
    {
	put(_credentialsKey, "yes");
    }

    public boolean hasCredentials()
    {
    return get(_credentialsKey)!=null;
    }
    
    /*
     * TaggingOn
     */
    
    private static final String _taggingOnKey = "TaggingOn";
    
    /**
     * Returns <code>true</code> if tagging is on, <code>false</code>
     * otherwise.
     *
     * @return	<code>true</code> if tagging is on, <code>false</code>
     *		otherwise.
     */
    public boolean getTaggingOn()
    {
	String taggingOnValue = get(_taggingOnKey);
	
	if (Argument.isNullOrEmpty(taggingOnValue))
	{
	    return false;
	}
	
	int value = 0;
	
	try
	{
	    value = Integer.parseInt(taggingOnValue);
	}
	catch (Exception e)
	{
	}
	
	return value == 1;
    }
    
    /**
     * Sets the value indicating whether the device can initiate
     * tagging process.
     *
     * @param value Specify <code>true</code> if tagging is on,
     *		    <code>false</code> otherwise.
     */
    public void setTaggingOn(boolean value)
    {
	put(_taggingOnKey, value ? "1" : "0");
    }
    
    /*
     * Visible
     */
    
    private static final String _visibleKey = "Visible";
    private static final String _credentialsKey = "Credentials";
    
    /**
     * Returns <code>true</code> of the device is visible to other devices;
     * <code>false</code> otherwise.
     *
     * @return	<code>true</code> if the device is visible to other devices;
     *		<code>false</code> otherwise.
     */
    public boolean getVisible()
    {
	String visibleValue = get(_visibleKey);
	
	if (Argument.isNullOrEmpty(visibleValue))
	{
	    return false;
	}
	
	int value = Integer.parseInt(visibleValue);
	return value == 1;
    }
    
    /**
     * Sets the value indicating whether the device is visible to other devices.
     *
     * @param value Specify <code>true</code> if the device is visible to other
     *		    devices; <code>false</code> otherwise.
     */
    public void setVisible(boolean value)
    {
	put(_visibleKey, value ? "1" : "0");
    }
    
    /* ------ Methods.Public ------ */
    
    public void save() throws RecordStoreException
    {
	if (_recordStoreWrapper != null)
	{
	    _recordStoreWrapper.save();
	}
    }
   
    /* ------ Methods.Private ------ */
    
    public synchronized String get(String propertyName)
    {
	if (_recordStoreWrapper != null)
	{
	    return _recordStoreWrapper.get(propertyName);
	}
	
	return null;
    }
    
    public synchronized void put(String propertyName, String value)
    {
	if (_recordStoreWrapper != null)
	{
	    _recordStoreWrapper.put(propertyName, value);
	}
    }
    
    /* ------ Declarations ------ */
    
    private RecordStoreWrapper _recordStoreWrapper;
    
    /* ------ Constructors ------ */
    
    /**
     * Creates an instance of Settings class.
     */
    public Settings()
    {
	try
	{
	    _recordStoreWrapper = new RecordStoreWrapper("N2F_TAG_SETTINGS");
	    App.getCurrentApp().addToDeallocatableList(this);
	}
	catch (RecordStoreException recordStoreException)
	{
	    _recordStoreWrapper = null;
	}
    }

	public void free() {
		_recordStoreWrapper.free();
		
	}
}
