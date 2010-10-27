package com.genetibase.askafriend.ui;

import javax.microedition.lcdui.Alert;
import javax.microedition.lcdui.AlertType;
import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.CommandListener;
import javax.microedition.lcdui.Display;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.Form;
import javax.microedition.lcdui.Gauge;
import javax.microedition.lcdui.Spacer;

import com.genetibase.askafriend.common.Resoursable;
import com.genetibase.askafriend.core.Engine;
import com.genetibase.askafriend.utils.Debug;

public abstract class AbstractWindow implements GUIListener, CommandListener {
	private static final String ABSTRACT_WINDOW_WAIT_LOADING = "Loading...";
	private static final String ABSTRACT_WINDOW_FORM_TITLE_WAIT = "Wait";
	private static final String INTERRUPTABLE_BUSY_COMMAND_STOP = "Stop";
	private static final String CONSOLE = "Console";
	private static String ABSTRACT_WINDOW_ALERT_TITLE = null;
	private Command console = null; 
	public static final int COMMAND_REPLIES = 1;
	public static final int COMMAND_QUESTIONS = 2;
	public static final int COMMAND_CANCEL = 3;
	public static final int COMMAND_NONAME = -3;
	private Command cmdOpenQuestions, cmdOpenResponses, cmdBack;
	protected Command cmdResponses, cmdPrivateQuestions;
	
	private final String id;
	
	public abstract void show();
	
	protected void hide() {}
	
	abstract protected Displayable getScreen(); 
	
	public void fireAction (int actionType) {
		switch (actionType) {
		case ACTION_BUSY:
//			showBusy(null);
			break;
		case ACTION_HIDE:
			refresh();
			break;
		}
	}

	public void commandAction(Command c, Displayable d) {
		if (c == console) {
			UIManager.getInstance().showConsole();
		} else if (c == Alert.DISMISS_COMMAND) {
			if (getScreen() != null) {
				getDisplay().setCurrent(getScreen());
			}
		} else if (c == interruptCommand) {
			hideBusy();
		} else if (c == cmdOpenQuestions) {
			UIManager.getInstance().show(UIManager.SCREEN_PRIVATE_QUESTIONS);
		} else if (c == cmdOpenResponses) {
			UIManager.getInstance().show(UIManager.SCREEN_RESPONSES);
		} else if (c == cmdBack) {
			if (getScreen() != null) {
				getDisplay().setCurrent(getScreen());
			}
		} else if (c == cmdResponses) {
			UIManager.getInstance().show(UIManager.SCREEN_RESPONSES);
		} else if (c == cmdPrivateQuestions) {
			UIManager.getInstance().show(UIManager.SCREEN_PRIVATE_QUESTIONS);
		}
	}
	
	public void showAlert(String text, int confirmCmd, int declineCmd, CommandListener listener) 
	{
		if (getScreen() == null)
			return;
		if (ABSTRACT_WINDOW_ALERT_TITLE == null) {
			ABSTRACT_WINDOW_ALERT_TITLE = resoursable.getLocale(LocaleUI.ALERT_TITLE);
		}
		Alert alert = new Alert(ABSTRACT_WINDOW_ALERT_TITLE, text, null, AlertType.CONFIRMATION);
		alert.setTimeout(Alert.FOREVER);
		if (confirmCmd == COMMAND_QUESTIONS) {
			cmdOpenQuestions = new Command(getLocalizedText(LocaleUI.COMMAND_YES), Command.OK, 1);
			alert.addCommand(cmdOpenQuestions);
		} else if (confirmCmd == COMMAND_REPLIES) {
			cmdOpenResponses = new Command(getLocalizedText(LocaleUI.COMMAND_YES), Command.OK, 1);
			alert.addCommand(cmdOpenResponses);
		}
		if (declineCmd == COMMAND_CANCEL) {
			cmdBack = new Command(getLocalizedText(LocaleUI.COMMAND_NO), Command.CANCEL, 3);
			alert.addCommand(cmdBack);
		}
		alert.setCommandListener(listener);
        getDisplay().setCurrent(alert);
        AlertType.CONFIRMATION.playSound(getDisplay());
	}

	
	public void showAlert(String text, AlertType type, CommandListener listener) {
		if (ABSTRACT_WINDOW_ALERT_TITLE == null) {
			ABSTRACT_WINDOW_ALERT_TITLE = resoursable.getLocale(LocaleUI.ALERT_TITLE);
		}
		Alert alert = new Alert(ABSTRACT_WINDOW_ALERT_TITLE, text, null, type);
		alert.setTimeout(Alert.FOREVER);
		alert.setCommandListener(listener);
        getDisplay().setCurrent(alert);
	}
	
	protected void addConsole(Form form){
		if (Debug.isDebug()) {
			console = new Command(CONSOLE, Command.SCREEN, 4);
			form.addCommand(console);
		}
	}

//	private Displayable current;
	private Interruptable interruptable;
	
	public void showBusy(Interruptable interruptable)
	{
		 
		this.interruptable = interruptable;
		if (interruptable != null) {
			interruptCommand = new Command(INTERRUPTABLE_BUSY_COMMAND_STOP, Command.OK, 0);
			busyForm.addCommand(interruptCommand);
			busyForm.setCommandListener(this);
		}
//		current = getDisplay().getCurrent();
		getDisplay().setCurrent(busyForm);
//		System.out.println("---------show busy form-------------");
	}
	
	protected void hideBusy() {
//System.out.println("HIDE BUSY1");		
		if (interruptable != null) {
			interruptable.interrupted();
//		getDisplay().setCurrent(current);
		
			busyForm.removeCommand(interruptCommand);
			interruptCommand = null;
			busyForm.setCommandListener(null);
		}
//		current = null;
		interruptable = null;
//System.out.println("------------HIDE BUSY2------------");
	}
	
	abstract protected void refresh();

	protected String getId() {
		return id;
	}

	public abstract int getFormWidth();
	
	public abstract int getFormHeight();
	
	private Resoursable resoursable;
	
	protected Display getDisplay() {
		return Display.getDisplay(Engine.getEngine().getMidlet());
	}

	protected Resoursable getResoursable() {
		return resoursable;
	}
	
	protected String getLocalizedText(String key) {
		String retStr = null;
		if ((retStr = getResoursable().getLocale(key)) == null) {
			Debug.println("KEY=" + key);
			retStr = "UNKNOWN " + key;
		}
		return retStr;
	}

	private static Form busyForm;
	
	private Command interruptCommand;
	
	public AbstractWindow(String id, Resoursable resoursable) {
		this.resoursable = resoursable;
		this.id = id;
		if (busyForm == null) 
		{
			busyForm = new Form(ABSTRACT_WINDOW_FORM_TITLE_WAIT);
			busyForm.append(new Spacer(getFormWidth(),30));
			busyForm.append(new Gauge(ABSTRACT_WINDOW_WAIT_LOADING, false, Gauge.INDEFINITE, Gauge.CONTINUOUS_RUNNING));
		}
	}	
}
