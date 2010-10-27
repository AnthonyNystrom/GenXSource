
package n2f.blogger.ui;

import genetibase.java.microedition.componentmodel.Resources;

import javax.microedition.lcdui.Alert;
import javax.microedition.lcdui.AlertType;
import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.CommandListener;
import javax.microedition.lcdui.Display;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.Form;
import javax.microedition.lcdui.Gauge;

import n2f.blogger.App;
import n2f.blogger.debug.Debug;

public abstract class AbstractWindow
	implements GUIListener, CommandListener
{
    private static final String ABSTRACT_WINDOW_WAIT_LOADING = "Loading...";
    private static final String ABSTRACT_WINDOW_FORM_TITLE_WAIT = "Wait";
    private static final String INTERRUPTABLE_BUSY_COMMAND_STOP = "Stop";
    protected static final String CONSOLE = "Console";
    private static String ABSTRACT_WINDOW_ALERT_TITLE = null;
    protected Command console = null;
    protected Resources resoursable;
    private final String id;

    public abstract void show();

    protected void hide()
    {
    }

    abstract protected Displayable getScreen();

    public void fireAction(int actionType)
    {
	switch (actionType)
	{
	    case ACTION_BUSY:
		break;
	    case ACTION_HIDE:
		refresh();
		break;
	}
    }

    public void commandAction(Command c, Displayable d)
    {
	if (c == console)
	{
	    UIManager.getInstance().showConsole();
	}
	else if (c == interruptCommand)
	{
	    hideBusy();
	}
    }

    public void showAlert(String text, AlertType type, CommandListener listener)
    {
	if (ABSTRACT_WINDOW_ALERT_TITLE == null)
	{
	    ABSTRACT_WINDOW_ALERT_TITLE = resoursable.get("alertTitle");
	}
	Alert alert = new Alert(ABSTRACT_WINDOW_ALERT_TITLE, text, null, type);
	alert.setTimeout(Alert.FOREVER);
	alert.setCommandListener(listener);
	getDisplay().setCurrent(alert);
    }

    protected void addConsole(Form form)
    {
	if (Debug.isDebug())
	{
	    console = new Command(CONSOLE, Command.SCREEN, 4);
	    form.addCommand(console);
	}
    }

    private Interruptable interruptable;

    public void showBusy(Interruptable interruptable)
    {

	this.interruptable = interruptable;
	if (interruptable != null)
	{
	    interruptCommand = new Command(INTERRUPTABLE_BUSY_COMMAND_STOP,
					   Command.OK, 0);
	    busyForm.addCommand(interruptCommand);
	    busyForm.setCommandListener(this);
	}
	getDisplay().setCurrent(busyForm);
    }

    protected void hideBusy()
    {
	if (interruptable != null)
	{
	    interruptable.interrupted();
	    busyForm.removeCommand(interruptCommand);
	    interruptCommand = null;
	    busyForm.setCommandListener(null);
	}
	interruptable = null;
    }

    abstract protected void refresh();

    protected String getId()
    {
	return id;
    }

    protected Display getDisplay()
    {
	return App.getDisplay();
    }

    protected Resources getResoursable()
    {
	return resoursable;
    }

    private static Form busyForm;
    private Command interruptCommand;

    public AbstractWindow(String id, Resources resoursable)
    {
	this.resoursable = resoursable;
	this.id = id;
	if (busyForm == null)
	{
	    busyForm = new Form(ABSTRACT_WINDOW_FORM_TITLE_WAIT);
	    busyForm.append(new Gauge(ABSTRACT_WINDOW_WAIT_LOADING, false,
				      Gauge.INDEFINITE, Gauge.CONTINUOUS_RUNNING));
	}
    }

}
