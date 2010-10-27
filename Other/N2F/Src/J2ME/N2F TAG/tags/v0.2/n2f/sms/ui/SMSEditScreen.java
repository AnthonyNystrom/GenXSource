package n2f.sms.ui;

import genetibase.java.microedition.componentmodel.Resources;

import javax.microedition.lcdui.Alert;
import javax.microedition.lcdui.AlertType;
import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.TextBox;
import javax.microedition.lcdui.TextField;

import n2f.tag.ui.AbstractWindow;
import n2f.tag.ui.UIManager;

public class SMSEditScreen extends AbstractWindow {

	private TextBox textBox;
//	private Command cancel;
	private Command back;
	private Command submit;

	public SMSEditScreen(String id, Resources resoursable) {
		super(id, resoursable);
//		cancel = new Command(getLocalizedText(LocaleUI.SMS_EDIT_COMMAND_CANCEL), Command.CANCEL, 1);
		back = new Command(resoursable.get("smsEditBack"), Command.BACK, 1);
		submit = new Command(resoursable.get("smsEditSubmit"), Command.SCREEN, 1);
		SMSBean bean = (SMSBean) UIManager.getInstance().getBean(UIManager.SCREEN_SMS);
		textBox = new TextBox(resoursable.get("smsEditCaption"), bean== null? null: SMSBean.getInfo(), 255, TextField.ANY);
//		textBox.addCommand(cancel);
		textBox.addCommand(back);
		textBox.addCommand(submit);
		textBox.setCommandListener(this);
	}

	public void show() {		
        getDisplay().setCurrent(textBox);
	}
	
	public int getFormHeight() {
		return textBox.getHeight();
	}

	public int getFormWidth() {
		return textBox.getWidth();
	}

	protected void refresh() {}

	public void commandAction(Command c, Displayable d) {
		super.commandAction(c, d);
		if (c == submit) {
			if (textBox.size() > 0) {
				SMSBean bean = (SMSBean) UIManager.getInstance().getBean(UIManager.SCREEN_SMS);
				if (bean != null) {
					SMSBean.setInfo(textBox.getString());
				}
				UIManager.getInstance().showPrevious();//(UIManager.SCREEN_SMS, false);
			} else 
				showAlert(resoursable.get("smsEditAlert"), AlertType.CONFIRMATION, this);
			
		} else
//			if (c == cancel) {
//			UIManager.getInstance().showDefault();
//		}
		if (c == back)  {
//			Question.getInstance().setQuestionText(textBox.getString().trim());
			UIManager.getInstance().showPrevious();
		}
		if (c == Alert.DISMISS_COMMAND) {
			getDisplay().setCurrent(textBox);
		}		
	}

	protected Displayable getScreen() {
		return textBox;
	}
}

