/* ------------------------------------------------------
 * App.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * --------------------------------------------------- */

package n2f.blogger;

import genetibase.java.microedition.componentmodel.Resources;

import java.util.Enumeration;
import java.util.Vector;

import javax.microedition.lcdui.Display;
import javax.microedition.midlet.MIDlet;
import javax.microedition.rms.RecordStoreException;

import genetibase.java.microedition.Deallocatable;
import n2f.blogger.core.Settings;
import n2f.blogger.ui.UIManager;
import n2f.blogger.webservice.WebServiceInteractor;

/**
 * @author Alex Nesterov
 */
public final class App
	extends MIDlet
{
    /**
     * Message to display when resource load operation fails.
     */
    public static final String MESSAGE_CANNOT_LOAD_RESOURCES =
	    "Cannot load resources. Please, contact Next2Friends support team.";
    private Vector _deallocatables;
    private Settings _settings;
    private WebServiceInteractor _webService;

    /**
     * Creates a new instance of the <tt>App</tt> class.
     */
    public App()
    {
	_currentApp = this;
    }

    public void addToDeallocatableList(Deallocatable dealloc)
    {
	if (!_deallocatables.contains(dealloc))
	    _deallocatables.addElement(dealloc);
    }

    public void destroyApp(boolean unconditional)
    {
	fullDestroy();
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

    public WebServiceInteractor getWebServiceIteractor()
    {
	if (_webService == null)
	{
	    _webService = new WebServiceInteractor();
	    _webService.addErrorListener(UIManager.getInstance());
	}

	return _webService;
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

	for (Enumeration en = _deallocatables.elements(); en.hasMoreElements();)
	    ((Deallocatable)en.nextElement()).free();
	_deallocatables.removeAllElements();
	_settings = null;
	_deallocatables = null;
	_webService = null;

	notifyDestroyed();
    }
}
