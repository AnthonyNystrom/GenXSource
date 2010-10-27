
package n2f.blogger.ui;

import n2f.blogger.core.AbstractBean;
import genetibase.java.microedition.Deallocatable;
import genetibase.java.microedition.componentmodel.Resources;

import java.util.Hashtable;
import java.util.Stack;

import javax.microedition.lcdui.AlertType;

import n2f.blogger.App;
import n2f.blogger.core.AbstractErrorManager;
import n2f.blogger.core.ErrorEvent;
import n2f.blogger.core.ErrorListener;
import n2f.blogger.debug.Console;

public class UIManager
	extends AbstractErrorManager
	implements ErrorListener, Deallocatable
{
    public static final String SCREEN_SETTINGS = "Settings";
    public static final String SCREEN_MAIN = "N2F Blogger";
    public static final String SCREEN_CONSOLE = "Console";
    public static final String SCREEN_SPLASH = "Splash";
    public static final String SCREEN_CONTACTS = "Contacts";
    private static final String _defaultID = SCREEN_MAIN;
    private static UIManager _instance = null;
    private AbstractWindow _currentUI;
    private Hashtable _beans;
    private Resources _resoursable;
    private Stack _uiStack;
    private String _currentID;

    private UIManager()
    {
	_beans = new Hashtable();
	_uiStack = new Stack();
	App.getCurrentApp().addToDeallocatableList(this);
    }

    public static UIManager getInstance()
    {
	if (_instance == null)
	    _instance = new UIManager();
	return _instance;
    }

    public void show(String id)
    {
	show(id, true);
    }

    public void show(String id, boolean add2stack)
    {
	boolean clean = true;

	if (SCREEN_MAIN.equals(id))
	{
	    _currentUI = new MainForm(id, getResoursable());
	}
	else if (SCREEN_CONSOLE.equals(id))
	{
	    showConsole();
	    return;
	}
	else if (SCREEN_SETTINGS.equals(id))
	{
	    _currentUI = new SettingsScreen(id, getResoursable());
	}
	else if (SCREEN_SPLASH.equals(id))
	{
	    _currentUI = new SplashScreen(id, null);
	    add2stack = false;
	    clean = false;
	}

	if (_currentUI != null)
	    show(_currentUI, add2stack, clean);
    }

    public void showPrevious()
    {
	if (_uiStack.empty())
	    showDefault();
	else
	    show((String)_uiStack.pop(), false);
    }

    public void showCurrent()
    {
	if (_uiStack.empty())
	    showDefault();
	else
	    show(_currentUI, false, true);
    }

    public void showConsole()
    {
	show(Console.getInstance(), true, false);
    }

    public void cleanStack()
    {
	this._uiStack.removeAllElements();
    }

    private void cleanResources()
    {
	_uiStack.removeAllElements();
    }

    private void show(AbstractWindow ui, boolean add2stack, boolean clean)
    {
	if (clean)
	    cleanResources();
	ui.show();
	if (add2stack && _currentID != null)
	    _uiStack.push(_currentID);
	_currentID = ui.getId();
    }

    public void showDefault()
    {
	show(_defaultID);
    }
    
    public void free()
    {
	getResoursable().free();
	_uiStack.removeAllElements();
	_instance = null;
    }

    /**
     * @return Returns the defaultUI.
     */
    public String getDefaultID()
    {
	return _defaultID;
    }

    public String getCurrentID()
    {
	return _currentID;
    }

    public AbstractWindow getCurrentUI()
    {
	return _currentUI;
    }

    public void actionPerformed(ErrorEvent errorEvent)
    {
	String text = null;
	
	if (getCurrentUI() != null)
	{
	    switch (errorEvent.getErrorId())
	    {
		case ErrorEvent.OPERATION_FAILED:
		    text = getResoursable().get("errorOperationFailed");
		    break;
		case ErrorEvent.GET_CREDENTIALS:
		    text = getResoursable().get("errorGetCredentials");
		    break;
		default:
		    text = errorEvent.getExplanation();
		    break;
	    }

	    if (text == null)
		text = "Not localized message.";

	    getCurrentUI().showAlert(text,
				     AlertType.CONFIRMATION,
				     getCurrentUI());
	}
    }

    private Resources getResoursable()
    {
	return _resoursable;
    }

    public void setResoursable(Resources resoursable)
    {
	_resoursable = resoursable;
    }

    public AbstractBean getBean(String key)
    {
	return (AbstractBean)_beans.get(key);
    }

    public void putBean(String key, AbstractBean bean)
    {
	_beans.put(key, bean);
    }

}
