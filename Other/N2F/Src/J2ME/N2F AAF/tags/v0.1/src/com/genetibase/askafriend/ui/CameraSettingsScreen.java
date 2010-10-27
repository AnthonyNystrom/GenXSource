package com.genetibase.askafriend.ui;

import java.util.Enumeration;
import java.util.Hashtable;

import javax.microedition.lcdui.Alert;
import javax.microedition.lcdui.AlertType;
import javax.microedition.lcdui.Choice;
import javax.microedition.lcdui.ChoiceGroup;
import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.Form;

import com.genetibase.askafriend.camera.MediaUtils;
import com.genetibase.askafriend.camera.SnapshotEncodingWrapper;
import com.genetibase.askafriend.common.Resoursable;
import com.genetibase.askafriend.core.Engine;

class CameraSettingsScreen extends AbstractWindow {
	private final Form form;
    private ChoiceGroup resolutionChoice;
    private Command backCommand, applyCommand; // these are the strings for the choices.
	
	CameraSettingsScreen(String id, Resoursable resoursable) {
		super(id, resoursable);
		this.form = new Form(getLocalizedText(LocaleUI.FORM_CAMERA_SETTINGS_CAPTION));
		addConsole(this.form);
		
		this.resolutionChoice = new ChoiceGroup(getLocalizedText(LocaleUI.CAMERA_SETTINGS_IMAGE_RESOLUTION), Choice.EXCLUSIVE);
		
		CameraBean bean = (CameraBean)Engine.getEngine().getBean(getId());
    	if (bean == null) {
    		bean = new CameraBean(getResoursable());
    		Engine.getEngine().putBean(getId(), bean);
    	} 
    	String chosen = (bean.getWrapper()== null)? "": bean.getWrapper().getDescription();
		
		Hashtable wrappers = MediaUtils.ENCODING_WRAPPERS;
		if (!wrappers.isEmpty()) {
			Enumeration en = wrappers.keys();
			while (en.hasMoreElements()) {
				String descr = (String) en.nextElement();
				int index = resolutionChoice.append(descr, null);
				if (descr.equals(chosen))
					resolutionChoice.setSelectedIndex(index, true);
			}
			en = null;
			
		    applyCommand = new Command(getLocalizedText(LocaleUI.CAMERA_SETTINGS_COMMAND_APPLY), Command.OK, 0);
		    form.addCommand(applyCommand);  
		} 
	    form.append(resolutionChoice);
    	form.addCommand(applyCommand);        
    	form.setCommandListener(this);
 	}

	public int getFormHeight() {
		return getDisplay().getCurrent().getHeight();
	}

	public int getFormWidth() {
		return getDisplay().getCurrent().getWidth();
	}

	public void show() {
		getDisplay().setCurrent(form);
	}

	protected void refresh() {
	}
	
	private boolean isValid(int size) {
		if (size == -1) {
			Alert a = new Alert(null, getLocalizedText(LocaleUI.ALERT_CAMERA_SETTINGS_TEXT), null, AlertType.ERROR);
	        a.setTimeout(Alert.FOREVER);
	        getDisplay().setCurrent(a);
	        return false;
		}
		return true;
	}
	
	public void commandAction(Command c, Displayable d) {
		super.commandAction(c, d);	
		if (c == Alert.DISMISS_COMMAND) {
			UIManager.getInstance().showPrevious();
		} else if (c == backCommand) {
			UIManager.getInstance().showPrevious();
		} else if (c == applyCommand) {
		   	CameraBean bean = (CameraBean)Engine.getEngine().getBean(getId());
	        int size = this.resolutionChoice.getSelectedIndex();
	        if (isValid(size)) {
	        	String chosen = resolutionChoice.getString(size);
	        	Hashtable wrappers = MediaUtils.ENCODING_WRAPPERS;
	        	SnapshotEncodingWrapper wrapper = (SnapshotEncodingWrapper) wrappers.get(chosen);
	        	if (wrapper != null) {
	        		bean.setWrapper(wrapper);
					bean.saveBean(getResoursable());
					Engine.getEngine().setPrefferedSettings(wrapper);
					UIManager.getInstance().showPrevious();	
	        	}
	        }
		} 	
	}

	protected Displayable getScreen() {
		return form;
	}
}
