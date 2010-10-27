package com.genetibase.askafriend.ui;

import javax.microedition.lcdui.Alert;

import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.Form;
import javax.microedition.lcdui.Gauge;
import javax.microedition.lcdui.ImageItem;
import javax.microedition.lcdui.Item;
import javax.microedition.lcdui.Spacer;

import com.genetibase.askafriend.common.Resoursable;
import com.genetibase.askafriend.common.network.stub.AskAFriendConfirm;
import com.genetibase.askafriend.common.utils.Base64;
import com.genetibase.askafriend.common.utils.Utils;
import com.genetibase.askafriend.core.Engine;
import com.genetibase.askafriend.core.Question;


class WaitScreen extends AbstractWindow {
	private Form mainForm;
	private Gauge progress;
	private ImageItem advertSpace;
	private Command back, done;
	private int gaugeIndex = 0; 
	private AskAFriendConfirm confirm = null;
	
	WaitScreen(String id, Resoursable resoursable) {
		super(id, resoursable);
		
		mainForm = new Form(getLocalizedText(LocaleUI.FORM_WAIT_CAPTION));
				
		mainForm.append(new Spacer(getFormWidth(),getFormHeight()/20));
		
		advertSpace = new ImageItem(null, Engine.getEngine().getImage("/res/commercelogo.png"), Item.LAYOUT_TOP|Item.LAYOUT_CENTER, "Advertising space");
		mainForm.append(advertSpace);
		
		mainForm.append(new Spacer(getFormWidth(),getFormHeight()/10));
		
		progress = new Gauge(getLocalizedText(LocaleUI.FORM_WAIT_CAPTION), false, Gauge.INDEFINITE, Gauge.CONTINUOUS_RUNNING);
		gaugeIndex = mainForm.append(progress);	
		progress.setLayout(Item.LAYOUT_EXPAND);
		
		back = new Command(getLocalizedText(LocaleUI.WAIT_COMMAND_BACK), Command.BACK, 1);
		done = new Command(getLocalizedText(LocaleUI.WAIT_COMMAND_DONE), Command.OK, 1);
		mainForm.addCommand(back);
		mainForm.setCommandListener(this);
	}

	
	public void show() {		
        getDisplay().setCurrent(mainForm);
        Engine.getEngine().submitQuestion(Question.getInstance(), this);
	}
	
	public int getFormHeight() {
		return mainForm.getHeight();
	}

	public int getFormWidth() {
		return mainForm.getWidth();
	}

	protected void refresh() {
		confirm = Engine.getEngine().getSubmitConfirm();
		if (confirm != null ) {
			String imageStr = confirm.getAdvertImage();
			if (imageStr != null && imageStr.length() > 0)
				advertSpace.setImage(Utils.createImage(Base64.decode(imageStr)));
		}
		mainForm.delete(gaugeIndex);
		mainForm.removeCommand(back);
		mainForm.addCommand(done);
		mainForm.append(getLocalizedText(LocaleUI.WAIT_TEXT));
		Question.getInstance().clear();
	}


	public void commandAction(Command c, Displayable d) {
		super.commandAction(c, d);
		if (c == done) {
			UIManager.getInstance().showDefault();
		} 
		if (c == back) {
			Engine.getEngine().breakNetworkConnection();
			UIManager.getInstance().showPrevious();
		}
		if (c == Alert.DISMISS_COMMAND) {
			UIManager.getInstance().showDefault();
		}

	}


	protected Displayable getScreen() {
		return mainForm;
	}
}
