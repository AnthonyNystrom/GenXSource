package com.genetibase.askafriend.ui;

import javax.microedition.lcdui.Alert;
import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.Form;
import javax.microedition.lcdui.Image;
import javax.microedition.lcdui.ImageItem;
import javax.microedition.lcdui.Item;
import javax.microedition.lcdui.Spacer;
import javax.microedition.lcdui.StringItem;

import com.genetibase.askafriend.common.Resoursable;
import com.genetibase.askafriend.common.network.stub.AskAFriendResponse;
import com.genetibase.askafriend.common.utils.Base64;
import com.genetibase.askafriend.common.utils.Utils;
import com.genetibase.askafriend.core.Engine;
import com.genetibase.askafriend.core.QuestionInfoBean;
import com.genetibase.askafriend.core.ResponseHolder;

class AnswerDetailsScreen extends AbstractWindow {
	private StringItem result;
	private ImageItem photo;
	private Command cmdBack;
	private Command cmdComments;
	private Form mainForm;
	
	
	AnswerDetailsScreen(String id, Resoursable resoursable) {
		super(id, resoursable);
		cmdBack = new Command(getLocalizedText(LocaleUI.ANSWER_DETAILS_COMMAND_BACK),Command.BACK, 3);
		cmdComments = new Command(getLocalizedText(LocaleUI.ANSWER_DETAILS_COMMAND_COMMENTS), Command.SCREEN,3);
		photo = new ImageItem(null, null, Item.LAYOUT_TOP|Item.LAYOUT_LEFT|Item.LAYOUT_EXPAND|Item.LAYOUT_VEXPAND, "Advertising space");
		mainForm = new Form(getLocalizedText(LocaleUI.FORM_ANSWER_DETAILS_CAPTION));
		mainForm.addCommand(cmdBack);
		mainForm.addCommand(cmdComments);
		mainForm.setCommandListener(this);
	}
	
	private ResponseHolder getHolder() {
		return Engine.getEngine().getResponseHolder();
	}
	
	public void show()
	{	
		showBusy(null);
		QuestionInfoBean bean = (QuestionInfoBean)Engine.getEngine().getBean(getId());
		QuestionInfoBean storedBean = null;
//		System.out.println("bean1="+bean);
		if (getHolder().getResponseDetails() != null ) 
			storedBean = getHolder().getResponseDetails().getBean();
		
		if ((bean != null) && bean.equals(storedBean)) {
			refresh();
		} else
			Engine.getEngine().getAAFResponseDetails(this, bean);
	}
	
	public int getFormHeight() {
		return mainForm.getHeight();
	}

	public int getFormWidth() {
		return mainForm.getWidth();
	}

	protected void refresh() 
	{
		hideBusy();
		if (getHolder().getResponseDetails() != null) {
			AskAFriendResponse resp = getHolder().getResponseDetails().getDetails();
			mainForm.append(resp.getQuestion());
			mainForm.append(new Spacer(getFormWidth(),10));
			if (resp.getAverage() > 0) 
			{
				result = new StringItem(null, String.valueOf(resp.getAverage()));
				mainForm.append(result);
			}
			mainForm.append(new Spacer(getFormWidth(),10));
			
			String imageString = resp.getPhotoBase64Binary();
			if (imageString != null && imageString.length() > 0 ) 
			{
				Image im = Utils.createImage(Base64.decode(imageString));
				photo.setImage(im);
				mainForm.append(photo);
			}

		} else {
			mainForm.append(getLocalizedText(LocaleUI.ALERT_DETAILS_TEXT));
		}
		getDisplay().setCurrent(mainForm);
	}

	public void commandAction(Command c, Displayable d) {
		super.commandAction(c, d);
		if (c == cmdComments) {
			UIManager.getInstance().show(UIManager.SCREEN_RESPONSE_COMMENTS);
		} else if (c == cmdBack) {
			UIManager.getInstance().showPrevious();
		} else if (c == Alert.DISMISS_COMMAND) {
			getDisplay().setCurrent(mainForm);
		}
	}

	protected Displayable getScreen() {
		return mainForm;
	}
}
