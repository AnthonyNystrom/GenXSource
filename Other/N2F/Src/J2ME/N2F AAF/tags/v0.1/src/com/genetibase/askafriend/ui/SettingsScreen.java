package com.genetibase.askafriend.ui;

import javax.microedition.lcdui.Alert;
import javax.microedition.lcdui.AlertType;
import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.Form;
import javax.microedition.lcdui.Spacer;
import javax.microedition.lcdui.TextField;

import com.genetibase.askafriend.common.Resoursable;
import com.genetibase.askafriend.core.Engine;

class SettingsScreen extends AbstractWindow {
	private Form mainForm;
	private Command cmdBack, cmdSubmit;
	private TextField edLogin, edPassword;
	private SettingsBean bean;
	
	SettingsScreen(String id, Resoursable resoursable) {
		super(id, resoursable);
		bean = (SettingsBean)Engine.getEngine().getBean(getId());
		if (bean == null) {
			bean = new SettingsBean(getResoursable());
		}
		cmdSubmit = new Command(getLocalizedText(LocaleUI.SETINGS_COMMAND_PROCEED), Command.SCREEN, 1);
		edLogin = new TextField(getLocalizedText(LocaleUI.SETINGS_TEXTFIELD_LOGIN), bean.getLogin(), 32, TextField.ANY);
		edPassword = new TextField(getLocalizedText(LocaleUI.SETINGS_TEXTFIELD_PASSWORD), bean.getPassword(), 32, TextField.ANY | TextField.PASSWORD);

		this.mainForm = new Form(getLocalizedText(LocaleUI.FORM_SETTINGS_CAPTION));
		
		this.mainForm.append(new Spacer(getFormWidth(),getFormHeight()/10));
		this.mainForm.append(edLogin);
		this.mainForm.append(edPassword);
		boolean exit = (bean.getLogin()== null && bean.getPassword() == null);
		
		cmdBack = new Command(exit? getLocalizedText(LocaleUI.SETINGS_COMMAND_QUIT): getLocalizedText(LocaleUI.SETINGS_COMMAND_CANCEL), exit?Command.EXIT: Command.CANCEL, 1);
		this.mainForm.addCommand(cmdBack);
		this.mainForm.addCommand(cmdSubmit);
		Engine.getEngine().putBean(getId(), bean);
		this.mainForm.setCommandListener(this);
	}
	
	public void show() {		
        getDisplay().setCurrent(mainForm);
	}

	
	public int getFormHeight() {
		return mainForm.getHeight();
	}

	public int getFormWidth() {
		return mainForm.getWidth();
	}

	protected void refresh() {
		hideBusy();
		bean = (SettingsBean) Engine.getEngine().getBean(UIManager.SCREEN_SETTINGS);
		bean.saveBean(getResoursable());
		Engine.getEngine().setAuthorized(true);
		
		UIManager.getInstance().show(UIManager.SCREEN_START, false);
	}

	private static boolean isValid(String login, String pass) {
//		System.out.println("login=" + login + " pass=" + pass);
		return login != null && pass != null && login.length() > 0 && pass.length() > 0;
	}
	
	public void commandAction(Command c, Displayable d) {
		if (c == cmdBack) {
			if (c.getCommandType() == Command.EXIT)
				Engine.getEngine().exit();
			else {
				Engine.getEngine().openInit(null);
				UIManager.getInstance().show(UIManager.SCREEN_START, false);
			}
		} else if (c == cmdSubmit) {
			String login, pass;
			if (!isValid(login = edLogin.getString(), pass = edPassword.getString())) {
				showAlert(getLocalizedText(LocaleUI.SETTINGS_ALERT_TEXT), AlertType.CONFIRMATION, this);
			} else {
				
				bean.setLogin(login);
				bean.setPassword(pass);
				showBusy(null);
				Engine.getEngine().requestWebMemberId(this);
//				refresh();
			}
		} else if (c == Alert.DISMISS_COMMAND) {
			getDisplay().setCurrent(mainForm);
		}
	}

	protected Displayable getScreen() {
		return mainForm;
	}
}
