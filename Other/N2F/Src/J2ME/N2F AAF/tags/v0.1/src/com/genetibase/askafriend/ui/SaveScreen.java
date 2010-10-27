package com.genetibase.askafriend.ui;

import javax.microedition.lcdui.Alert;
import javax.microedition.lcdui.AlertType;
import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.Form;
import javax.microedition.lcdui.TextField;

import com.genetibase.askafriend.common.Resoursable;
import com.genetibase.askafriend.core.Engine;
import com.genetibase.askafriend.core.ItemLight2;
import com.genetibase.askafriend.ui.bean.SaveBean;

class SaveScreen extends AbstractWindow {
	private final Form form;
	private SaveBean save;
	private final Command saveCommand;
	private final Command backCommand;
	private final Command proceed;
	private final Command camera;
	
	private final TextField textField; 
	
	SaveScreen(String id, Resoursable resoursable) {
		super(id, resoursable);
		save = (SaveBean)Engine.getEngine().getBean(getId());
		this.form = new Form(save.getPath());
		saveCommand = new Command(getLocalizedText(LocaleUI.FILE_SAVER_COMMAND_SAVE), Command.ITEM, 3);
		backCommand = new Command(getLocalizedText(LocaleUI.FILE_SELECTOR_COMMAND_CANCEL), Command.BACK, 1);
		textField = new TextField(null, ".jpg", 10, TextField.ANY);
		
		proceed = new Command(getLocalizedText(LocaleUI.ALERT_FILE_SAVER_COMMAND_ASK), Command.OK, 2);
		camera = new Command(getLocalizedText(LocaleUI.ALERT_FILE_SAVER_COMMAND_PHOTO), Command.BACK, 2);
		
		this.form.addCommand(saveCommand);
		this.form.addCommand(backCommand);
		this.form.append(this.textField);
		this.form.setCommandListener(this);
	}

	private static boolean isValid(String text) {
		boolean valid = true;
		if (text == null || text.length() == 0 || !text.trim().endsWith(".jpg")) {
			valid = false;
		}
		return valid;
	}
	
	public void show() {
		getDisplay().setCurrent(form);
	}

	protected Displayable getScreen() {
		return form;
	}

	protected void refresh() {}

	public int getFormWidth() {
		return form.getWidth();
	}

	public int getFormHeight() {
		return form.getHeight();
	}
	
	private void add2Card(ItemLight2 item){
		if (item != null) {
			CartBean bean = (CartBean)Engine.getEngine().getBean(UIManager.SCREEN_CARD);
			if (bean == null) {
				Engine.getEngine().putBean(UIManager.SCREEN_CARD, bean = new CartBean(getResoursable()));
			}
			bean.addItem(item);
			bean.saveBean(getResoursable());
		}
	}


	public void commandAction(Command c, Displayable d) {
		super.commandAction(c, d);
		if (c == saveCommand) {
			String filename = ((TextField)textField).getString();
			if (isValid(filename)) {
				byte[] data = Engine.getEngine().getCapturedImage();
				Engine.getEngine().save(filename, data, null);
				Engine.getEngine().setCapturedImage(null);
				add2Card(new ItemLight2(filename, save.getPath(), ItemLight2.IMAGE_FILE, ItemLight2.TYPE_FILE));
				
				Alert a = new Alert("Info", getLocalizedText(LocaleUI.ALERT_FILE_SAVER_TEXT),
						null, AlertType.CONFIRMATION);
				a.setTimeout(Alert.FOREVER);
			    a.addCommand(camera);
			    a.addCommand(proceed);
			    a.setCommandListener(this);
		        getDisplay().setCurrent(a);
			} else {
				showAlert(getLocalizedText(LocaleUI.ALERT_FILE_SAVER_TEXT2), AlertType.CONFIRMATION, this);
			}
		} else if (c == backCommand) {
			UIManager.getInstance().showPrevious();
		} else if (c == camera) {
			UIManager.getInstance().show(UIManager.SCREEN_CAMERA);
		} else if (c == proceed) {
			CartBean bean = (CartBean)Engine.getEngine().getBean(UIManager.SCREEN_CARD);
			if (bean != null) {
				bean.setProceedCommandName(true);
			}
			UIManager.getInstance().cleanStack();
			UIManager.getInstance().show(UIManager.SCREEN_CARD, false);
		}
	}

}
