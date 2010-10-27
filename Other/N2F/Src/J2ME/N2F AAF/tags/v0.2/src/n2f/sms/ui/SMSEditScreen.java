package n2f.sms.ui;

import javax.microedition.lcdui.Alert;
import javax.microedition.lcdui.AlertType;
import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.TextBox;
import javax.microedition.lcdui.TextField;

import com.genetibase.askafriend.common.Resoursable;
import com.genetibase.askafriend.core.Engine;
import com.genetibase.askafriend.ui.AbstractWindow;
import com.genetibase.askafriend.ui.LocaleUI;
import com.genetibase.askafriend.ui.UIManager;

public class SMSEditScreen extends AbstractWindow {

	private TextBox textBox;
//	private Command cancel;
	private Command back;
	private Command submit;

	public SMSEditScreen(String id, Resoursable resoursable) {
		super(id, resoursable);
//		cancel = new Command(getLocalizedText(LocaleUI.SMS_EDIT_COMMAND_CANCEL), Command.CANCEL, 1);
		back = new Command(getLocalizedText(LocaleUI.SMS_EDIT_COMMAND_BACK), Command.BACK, 1);
		submit = new Command(getLocalizedText(LocaleUI.SMS_EDIT_COMMAND_SUBMIT), Command.SCREEN, 1);
		SMSBean bean = (SMSBean) Engine.getEngine().getBean(UIManager.SCREEN_SMS);
		textBox = new TextBox(getLocalizedText(LocaleUI.FORM_SMS_EDIT_CAPTION), bean== null? null: SMSBean.getInfo(), 255, TextField.ANY);
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
				SMSBean bean = (SMSBean) Engine.getEngine().getBean(UIManager.SCREEN_SMS);
				if (bean != null) {
					SMSBean.setInfo(textBox.getString());
				}
				UIManager.getInstance().showPrevious();//(UIManager.SCREEN_SMS, false);
			} else 
				showAlert(getLocalizedText(LocaleUI.ALERT_SMS_EDIT_TEXT), AlertType.CONFIRMATION, this);
			
		} else if (c == back)  {
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

