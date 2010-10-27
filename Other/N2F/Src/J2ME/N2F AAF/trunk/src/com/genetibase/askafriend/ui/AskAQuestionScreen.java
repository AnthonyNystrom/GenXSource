package com.genetibase.askafriend.ui;

import javax.microedition.lcdui.Alert;
import javax.microedition.lcdui.AlertType;
import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.TextBox;
import javax.microedition.lcdui.TextField;
import javax.microedition.lcdui.Ticker;

import com.genetibase.askafriend.common.Resoursable;
import com.genetibase.askafriend.core.Question;

class AskAQuestionScreen extends AbstractWindow {

	private TextBox textBox;
	private Command cancel;
	private Command back;
	private Command submit;

	AskAQuestionScreen(String id, Resoursable resoursable) {
		super(id, resoursable);
		cancel = new Command(getLocalizedText(LocaleUI.ASK_QUESTION_COMMAND_CANCEL), Command.CANCEL, 1);
		back = new Command(getLocalizedText(LocaleUI.ASK_QUESTION_COMMAND_BACK), Command.BACK, 1);
		submit = new Command(getLocalizedText(LocaleUI.ASK_QUESTION_COMMAND_NEXT), Command.SCREEN, 1);
		textBox = new TextBox(getLocalizedText(LocaleUI.FORM_ASK_A_QUESTION_CAPTION), null, 255, TextField.ANY);
		textBox.addCommand(cancel);
		textBox.addCommand(back);
		textBox.addCommand(submit);
		textBox.setString(Question.getInstance().getQuestionText());
		if (textBox.getString() != null && textBox.getString().length() > 0) {
			textBox.setTicker(new Ticker(getLocalizedText(LocaleUI.ASK_QUESTION_TICKER_EDIT)));
		} else
			textBox.setTicker(new Ticker(getLocalizedText(LocaleUI.ASK_QUESTION_TICKER_WRITE)));
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
				Question.getInstance().setQuestionText(textBox.getString().trim());
				UIManager.getInstance().show(UIManager.SCREEN_DURATION);
			} else 
				showAlert(getLocalizedText(LocaleUI.ALERT_ASK_QUESTION_TEXT), AlertType.CONFIRMATION, this);
			
		}
		if (c == cancel) {
			UIManager.getInstance().showDefault();
		}
		if (c == back)  {
			Question.getInstance().setQuestionText(textBox.getString().trim());
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
