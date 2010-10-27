/* ------------------------------------------------
 * Program.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark;

import genetibase.microedition.componentmodel.Resources;
import javax.microedition.lcdui.Display;
import javax.microedition.midlet.MIDlet;
import javax.microedition.midlet.MIDletStateChangeException;

/**
 * @author Alex Nesterov
 */
public class Program
	extends MIDlet
{
    private static Program _currentApp;
    private static Resources _res;
    
    public Program()
    {
	_res = new Resources("res", "Resources", "properties");
	_currentApp = this;
    }
    
    /**
     * Returns the currently active application instance.
     * @return The currently active application instance.
     */
    public static Program getCurrentApp()
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
    
    public static Resources getResources()
    {
	return _res;
    }

    protected void startApp() throws MIDletStateChangeException
    {
	getDisplay().setCurrent(new MainForm(getResources().get("ProductName")));
    }

    protected void pauseApp()
    {
	fullDestroy();
    }

    protected void destroyApp(boolean arg0) throws MIDletStateChangeException
    {
	fullDestroy();
	notifyDestroyed();
    }
    
    private void fullDestroy()
    {
    }
}
