/* ------------------------------------------------
 * MainForm.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.blogger.ui;

import genetibase.java.microedition.componentmodel.Resources;

import javax.microedition.lcdui.Alert;
import javax.microedition.lcdui.AlertType;
import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.CommandListener;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.Form;

import n2f.blogger.App;
import n2f.blogger.core.Settings;
import n2f.blogger.webservice.WebServiceInteractor;

/**
 * @author Alex Nesterov
 */
final class MainForm
	extends AbstractWindow
	implements CommandListener
{
    private Command _exitCommand;
    private Command _settingsCommand;
    private Form _form;

    /**
     * Creates a new instance of the <tt>MainForm</tt> class.
     */
    public MainForm(String id, Resources res)
    {
	super(id, res);
    }

    public void commandAction(Command command, Displayable displayable)
    {
	super.commandAction(command, displayable);
	if (command == _exitCommand)
	{
	    App currentApp = App.getCurrentApp();
	    currentApp.destroyApp(false);
	}
	else if (command == _settingsCommand)
	{
	    UIManager.getInstance().show(UIManager.SCREEN_SETTINGS);
	}
	else if (command == Alert.DISMISS_COMMAND)
	{
	    refresh();
	}
    }

    public void saveSettings()
    {
	Settings settings = App.getCurrentApp().getSettings();
    }

    private void initializeComponent()
    {
	Resources resources = getResoursable();
	_form = new Form("N2F Blogger");

	_settingsCommand = new Command(resources.get("settingsCommand"),
				       Command.SCREEN, 50);
	_form.addCommand(_settingsCommand);
	addConsole(_form);

	_exitCommand = new Command(resources.get("exitCommand"),
				   Command.EXIT,
				   100);
	_form.addCommand(_exitCommand);
	_form.setCommandListener(this);
    }
    
    public void show()
    {
	WebServiceInteractor ws = App.getCurrentApp().getWebServiceIteractor();
	if (ws != null)
	    refresh();
    }

    private void showAlert(
	    String alertTitleResourceString,
	    String alertTextResourceString)
    {
	Resources resources = getResoursable();

	String alertTitle = null;
	String alertText = null;

	alertTitle = resources.get(alertTitleResourceString);
	alertText = resources.get(alertTextResourceString);

	Alert alert = new Alert(alertTitle, alertText, null, AlertType.ERROR);
	alert.setTimeout(Alert.FOREVER);
	App.getDisplay().setCurrent(alert);
    }
    
    protected Displayable getScreen()
    {
	return _form;
    }

    protected void refresh()
    {
	hideBusy();

	App currentApp = App.getCurrentApp();
	currentApp.getWebServiceIteractor().init();
	initializeComponent();
	App.getDisplay().setCurrent(_form);
	System.out.println("MainForm::refresh");
    }

}
