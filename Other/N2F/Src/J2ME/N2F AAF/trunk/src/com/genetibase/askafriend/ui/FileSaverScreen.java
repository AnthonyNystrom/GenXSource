package com.genetibase.askafriend.ui;

import java.util.Enumeration;
import java.util.Vector;

import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.Displayable;

import com.genetibase.askafriend.common.Resoursable;
import com.genetibase.askafriend.common.file.FileInfoWrapper;
import com.genetibase.askafriend.core.Engine;
import com.genetibase.askafriend.core.ItemLight2;
import com.genetibase.askafriend.ui.bean.SaveBean;

class FileSaverScreen extends FileSelectorScreen2 {
	private Command saveCommand;
	
	FileSaverScreen(String id, Resoursable resoursable) {
		super(id, resoursable);
		saveCommand = new Command(getLocalizedText(LocaleUI.FILE_SAVER_COMMAND_SAVE), Command.ITEM, 3);
		this.list.addCommand(saveCommand);
	}
	
	protected void addCommands() {
		this.list.addCommand(selectCommand);
		this.list.addCommand(cancelCommand);
	}

	protected void populateList(Vector files) {
		ItemLight2 item = null;
		for (Enumeration en = files.elements(); en.hasMoreElements();) {
			FileInfoWrapper file = (FileInfoWrapper) en.nextElement();
			if (!file.isDirectory()) {
				continue;
			}

			item = new ItemLight2(!file.isRoot()? file.getFileName() :file.getFileName().substring(1), 
					file.getPath(), 
						file.isDirectory()? ItemLight2.IMAGE_FOLDER: ItemLight2.IMAGE_FILE, 
								file.isDirectory() || file.isRoot() ? ItemLight2.TYPE_FOLDER: ItemLight2.TYPE_FILE
										);
			int index = list.append(item.getTitle(), item.getIcon());
			item.setIdOnForm(index);
			items.addElement(item);
		}
	}
	
	public void commandAction(Command c, Displayable d) {
		super.commandAction(c, d);
		if (c == saveCommand) {
			SaveBean bean = (SaveBean)Engine.getEngine().getBean(UIManager.SCREEN_SAVE);
			if (bean == null) {
				Engine.getEngine().putBean(UIManager.SCREEN_SAVE, bean = new SaveBean());
			}
			ItemLight2 item = getSelectedItem(false);
			bean.setPath(item.getPath());
			UIManager.getInstance().show(UIManager.SCREEN_SAVE);
		}	

	}
}


/*package com.genetibase.askafriend.ui;

import java.util.Enumeration;
import java.util.Vector;

import javax.microedition.lcdui.Alert;
import javax.microedition.lcdui.AlertType;
import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.Item;
import javax.microedition.lcdui.TextField;

import com.genetibase.askafriend.common.Resoursable;
import com.genetibase.askafriend.common.file.FileInfoWrapper;
import com.genetibase.askafriend.core.Engine;

class FileSaverScreen extends FileSelectorScreen {
	private Command saveCommand, camera, proceed;
	
	FileSaverScreen(String id, Resoursable resoursable) {
		super(id, resoursable);
		saveCommand = new Command(getLocalizedText(LocaleUI.FILE_SAVER_COMMAND_SAVE), Command.ITEM, 3);
		proceed = new Command(getLocalizedText(LocaleUI.ALERT_FILE_SAVER_COMMAND_ASK), Command.OK, 2);
		camera = new Command(getLocalizedText(LocaleUI.ALERT_FILE_SAVER_COMMAND_PHOTO), Command.BACK, 2);
	}

	protected void populateList(Vector files) {
		Item item = null;
		int idOnForm = 0;
		for (Enumeration en = files.elements(); en.hasMoreElements();) {
			FileInfoWrapper file = (FileInfoWrapper) en.nextElement();
			if (!file.isDirectory()) {
				continue;
			}
			item = new CustomListItem(file.isRoot()? file.getFileName().substring(1): file.getFileName(), file.getPath(), 
					   0, file.isDirectory()? CustomListItem.ACTION_TYPE_OPEN_FOLDER: CustomListItem.ACTION_TYPE_SELECTABLE, this, getResoursable());
			idOnForm = mainForm.append(item);
			((CustomListItem)item).setIdOnForm(idOnForm);
		}
//		CustomListItem.MAX_ELEMENTS = index;		
		idOnForm = mainForm.append(item = new TextField("", getLocalizedText(LocaleUI.FILE_SAVER_TEXTFIELD_DEFTEXT), 13, TextField.ANY));
		item.addCommand(saveCommand);
		item.setItemCommandListener(this);
	}
	
	public void commandAction(Command c, Item item) {
		super.commandAction(c, item);
		if (c == saveCommand) {
			String filename = ((TextField)item).getString();
			byte[] data = Engine.getEngine().getCapturedImage();
			Engine.getEngine().save(filename, data, null);
			Engine.getEngine().setCapturedImage(null);
			Alert a = new Alert("Alert", getLocalizedText(LocaleUI.ALERT_FILE_SAVER_TEXT), null, AlertType.CONFIRMATION);
			a.setTimeout(Alert.FOREVER);
		    a.addCommand(camera);
		    a.addCommand(proceed);
		    a.setCommandListener(this);
	        getDisplay().setCurrent(a);
		}	

	}
	
	public void commandAction(Command c, Displayable d) {
		super.commandAction(c, d);
		if (c == proceed) {
			UIManager.getInstance().show(UIManager.SCREEN_FILE_SYSTEM);
		}if (c == camera) {
			UIManager.getInstance().showPrevious();
		}
	}
}*/