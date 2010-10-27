package com.genetibase.askafriend.ui;

import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.Image;
import javax.microedition.lcdui.List;
import javax.microedition.lcdui.Ticker;

import com.genetibase.askafriend.common.Resoursable;
import com.genetibase.askafriend.core.Engine;
import com.genetibase.askafriend.core.Question;

class CannedQuestionsScreen extends AbstractWindow {
	private List mainForm;
	private Command cmdBack, cmdNext;
	private final static Image OLD_IMAGE = Engine.getEngine().getImage("/unselected.png");
	private final static Image NEW_IMAGE = Engine.getEngine().getImage("/own_question.png");
	private static String[] questions; 

	public CannedQuestionsScreen(String id, Resoursable resoursable) {
		super(id, resoursable);
		
		questions  = new String[] {getLocalizedText(LocaleUI.CAN_QUESTIONS_1),
				getLocalizedText(LocaleUI.CAN_QUESTIONS_2),
				getLocalizedText(LocaleUI.CAN_QUESTIONS_3),
				getLocalizedText(LocaleUI.CAN_QUESTIONS_4),
				getLocalizedText(LocaleUI.CAN_QUESTIONS_5),
				getLocalizedText(LocaleUI.CAN_QUESTIONS_6),
				getLocalizedText(LocaleUI.CAN_QUESTIONS_7)};
		
		cmdBack = new Command(getLocalizedText(LocaleUI.CAN_QUESTIONS_COMMAND_BACK),Command.BACK, 3);
		cmdNext = new Command(getLocalizedText(LocaleUI.CAN_QUESTIONS_COMMAND_NEXT), Command.SCREEN,3);
		
		mainForm = new List(getLocalizedText(id), List.IMPLICIT);
		
		for (int i = 0; i < 7; i++) {
			mainForm.append(questions[i], i==0? NEW_IMAGE: OLD_IMAGE);
		}
		mainForm.addCommand(cmdBack);
		mainForm.setSelectCommand(cmdNext);
		mainForm.setCommandListener(this);
		mainForm.setTicker(new Ticker(getLocalizedText(LocaleUI.FORM_CAN_QUESTIONS_TICKER)));

	}

	public int getFormHeight() {
		return mainForm.getHeight();
	}

	public int getFormWidth() {
		return mainForm.getWidth();
	}

	protected Displayable getScreen() {
		return mainForm;
	}

	protected void refresh() {}

	public void show() {
		getDisplay().setCurrent(mainForm);

	}
	
	public void commandAction(Command c, Displayable d)
	{
		super.commandAction(c, d);
		if (c == cmdBack) {
			UIManager.getInstance().showPrevious();
		} else  if (c == cmdNext) {
			int index = mainForm.getSelectedIndex();
			if (index > 0)
				Question.getInstance().setQuestionText(questions[index]);
			UIManager.getInstance().show(UIManager.SCREEN_QUESTION);
		}
	}

}
