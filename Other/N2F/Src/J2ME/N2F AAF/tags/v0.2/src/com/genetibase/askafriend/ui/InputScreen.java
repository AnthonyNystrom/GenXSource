package com.genetibase.askafriend.ui;
import java.util.Vector;

import javax.microedition.lcdui.Alert;
import javax.microedition.lcdui.AlertType;
import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.Form;
import javax.microedition.lcdui.Spacer;
import javax.microedition.lcdui.TextField;

import com.genetibase.askafriend.common.Resoursable;
import com.genetibase.askafriend.core.Question;

class InputScreen extends AbstractWindow
{
	private final Command okCommand;
	private final Command cancelCommand;
	private Form form;
	private static int ANSWER_SIZE = 64;
	private TextField inputA, inputB;

	public InputScreen(String id, Resoursable resoursable) {
		super(id, resoursable);
		
		okCommand = new Command(getLocalizedText(LocaleUI.INPUT_SCREEN_COMMAND_DONE), Command.OK, 1);
		cancelCommand = new Command(getLocalizedText(LocaleUI.INPUT_SCREEN_COMMAND_CANCEL), Command.OK, 1);
		
		form = new Form(getLocalizedText(LocaleUI.FORM_INPUT_SCREEN_CAPTION));
		form.addCommand(okCommand);
		form.addCommand(cancelCommand);
		form.setCommandListener(this);
		String[] userAnswers = Question.getInstance().getUserAnswers();
		String answerA = null, answerB = null;
		if (userAnswers != null) {
			answerA = userAnswers[0];
			answerB = userAnswers[1];
		}
		form.append(new Spacer(getFormWidth(),getFormHeight()/10));
		form.append(inputA = new TextField(getLocalizedText(LocaleUI.INPUT_SCREEN_TEXTFIELD_LABEL)+1, answerA, ANSWER_SIZE, TextField.ANY));
		form.append(new Spacer(getFormWidth(),getFormHeight()/10));
		form.append(inputB = new TextField(getLocalizedText(LocaleUI.INPUT_SCREEN_TEXTFIELD_LABEL)+2, answerB, ANSWER_SIZE, TextField.ANY));
	}
	
	public void show() {		
        getDisplay().setCurrent(form);
	}


	public void commandAction(Command command, Displayable d)
	{
		super.commandAction(command, d);
		if (command == okCommand)
		{
			Vector answers = new Vector();
			String answerA = inputA.getString();
			if (answerA != null && answerA.length() > 0) {
				answers.addElement(answerA);
			} else {
				showAlert(getLocalizedText(LocaleUI.INPUT_SCREEN_ALERT_TEXT_A), AlertType.CONFIRMATION, this);
				return;
			}
			String answerB = inputB.getString();
			if (answerB != null && answerB.length() > 0) {
				answers.addElement(answerB);
			} else {
				showAlert(getLocalizedText(LocaleUI.INPUT_SCREEN_ALERT_TEXT_B), AlertType.CONFIRMATION, this);
				return;
			}
			Question.getInstance().setUserAnswers(answers);
			UIManager.getInstance().showPrevious();
			
		} else if (command == cancelCommand) {
			UIManager.getInstance().showPrevious();
		} else if (command == Alert.DISMISS_COMMAND) {
			getDisplay().setCurrent(form);
		}
	}

	protected void refresh() {}

	public int getFormHeight() {
		return form.getHeight();
	}

	public int getFormWidth() {
		return form.getWidth();
	}

	protected Displayable getScreen() {
		return form;
	}
}
