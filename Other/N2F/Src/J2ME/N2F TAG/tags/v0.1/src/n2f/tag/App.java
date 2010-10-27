/* ------------------------------------------------------
 * App.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * --------------------------------------------------- */

package n2f.tag;

import java.util.Enumeration;
import java.util.Vector;

import genetibase.java.microedition.componentmodel.Resources;

import javax.microedition.lcdui.Display;
import javax.microedition.midlet.MIDlet;
import javax.microedition.rms.RecordStoreException;

import n2f.tag.core.Deallocatable;
import n2f.tag.core.Settings;
import n2f.tag.ui.UIManager;
import n2f.tag.webservice.WebServiceInteractor;
import n2f.tag.wireless.DeviceDiscoverer;
import n2f.tag.wireless.IDeviceDiscovererListener;

/**
 * @author Alex Nesterov
 */
public final class App
	extends MIDlet
{
    /* ------ Methods.Public ------ */
    
    public void addToDeallocatableList(Deallocatable dealloc)
    {
	if (!_deallocatables.contains(dealloc))
	    _deallocatables.addElement(dealloc);
    }
    
    public void destroyApp(boolean unconditional)
    {
	fullDestroy();
    }
    
    private void fullDestroy()
    {
	try
	{
	    getSettings().save();
	}
	catch (RecordStoreException e)
	{
	    e.printStackTrace();
	}
	
	for (Enumeration en = _deallocatables.elements();en.hasMoreElements();)
	    ((Deallocatable)en.nextElement()).free();
	_deallocatables.removeAllElements();
	
	_discoverer = null;
	_settings = null;
	_deallocatables = null;
	_webService = null;
	
	notifyDestroyed();
    }
    
    public void pauseApp()
    {
	fullDestroy();
    }
    
    public void startApp()
    {
	_deallocatables = new Vector();
	
	Resources res = createResources();
	res.load();
	
	UIManager.getInstance().setResoursable(res);
	
	UIManager.getInstance().show(UIManager.SCREEN_SPLASH, false);
    }
    
    /* ------ Methods.Public.Static ------ */
    
    /**
     * Returns a new Resources instance initialized for the current project.
     * @return Resources instance initialized for the current project.
     */
    public static Resources createResources()
    {
	return new Resources("res", "Resources", "properties");
    }
    
    private static App _currentApp;
    
    /**
     * Returns the currently active application instance.
     * @return The currently active application instance.
     */
    public static App getCurrentApp()
    {
	return _currentApp;
    }
    
    /**
     * Returns the display for the currently active application instance.
     * @return The Display for the currently active application instance.
     */
    public static Display getDisplay()
    {
	return Display.getDisplay(getCurrentApp());
    }
    
    /**
	 * Returns _settings for the application.
	 * 
	 * @return Settings for the application.
	 */
    public Settings getSettings()
    {
	if (_settings == null)
	    _settings = new Settings();
	return _settings;
    }
    
    public DeviceDiscoverer getDeviceDiscoverer(IDeviceDiscovererListener listener)
    {
	if (_discoverer == null)
	    _discoverer = new DeviceDiscoverer(listener);
	return _discoverer;
    }
    
    public WebServiceInteractor getWebServiceIteractor()
    {
	if (_webService == null)
	{
	    _webService = new WebServiceInteractor();
	    _webService.addErrorListener(UIManager.getInstance());
	}
	
	return _webService;
    }
    
    /* ------ Declarations ------ */
    
    /**
     * Message to display when resource load operation fails.
     */
    public static final String MESSAGE_CANNOT_LOAD_RESOURCES = "Cannot load resources. Please, contact Next2Friends support team.";
    
    private Vector _deallocatables;
    private Settings _settings;
    private DeviceDiscoverer _discoverer;
    private WebServiceInteractor _webService;
    
    /* ------ Constructors ------ */
    
    /**
     * Creates a new instance of App.
     */
    public App()
    {
	_currentApp = this;
    }
}
