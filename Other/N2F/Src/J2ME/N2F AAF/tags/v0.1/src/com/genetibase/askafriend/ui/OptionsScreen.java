package com.genetibase.askafriend.ui;

import javax.microedition.lcdui.AlertType;
import javax.microedition.lcdui.ChoiceGroup;
import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.Form;
import javax.microedition.lcdui.Item;
import javax.microedition.lcdui.ItemStateListener;
import javax.microedition.lcdui.Spacer;

import com.genetibase.askafriend.common.Resoursable;
import com.genetibase.askafriend.core.Question;

class OptionsScreen extends AbstractWindow implements ItemStateListener{
	private Form mainForm;
	private Command cmdBack, cmdSubmit; 
	private ChoiceGroup choiceOptions, choiceDuration, choicePrivacy;
	

	OptionsScreen(String id, Resoursable resoursable) {
		super(id, resoursable);
		
		cmdBack = new Command(getLocalizedText(LocaleUI.OPTIONS_COMMAND_BACK), Command.BACK, 1);
		cmdSubmit = new Command(getLocalizedText(LocaleUI.OPTIONS_COMMAND_SUBMIT), Command.SCREEN, 1);
		String[] answerTypes = new String[Question.answerTypes.length];
		for (int i = 0; i < Question.answerTypes.length; i++) {
			answerTypes[i] = getLocalizedText(Question.answerTypes[i]);
		}
		String[] answerDures = new String[Question.answerDurations.length];
		for (int i = 0; i < Question.answerDurations.length; i++) {
			answerDures[i] = getLocalizedText(Question.answerDurations[i]);
		}
		choiceOptions = new ChoiceGroup(getLocalizedText(LocaleUI.OPTIONS_CHOICEGROUP_1), ChoiceGroup.POPUP, answerTypes, null);
		choiceDuration = new ChoiceGroup(getLocalizedText(LocaleUI.OPTIONS_CHOICEGROUP_2), ChoiceGroup.POPUP, answerDures, null);
		choicePrivacy = new ChoiceGroup(null, ChoiceGroup.MULTIPLE, new String[]{getLocalizedText(LocaleUI.OPTIONS_CHOICEGROUP_3)}, null);
		
		
		mainForm = new Form(getLocalizedText(LocaleUI.FORM_OPTIONS_CAPTION));
		
		mainForm.setItemStateListener(this);
		
		mainForm.append(new Spacer(getFormWidth(),getFormHeight()/20));
		mainForm.append(choiceOptions);
		choiceOptions.setSelectedIndex(Question.getInstance().getSelAnswerTypeIdx(), true);

		mainForm.append(new Spacer(getFormWidth(),getFormHeight()/20));
		
		mainForm.append(choiceDuration);
		choiceDuration.setSelectedIndex(Question.getInstance().getSelAnswerDurationIdx(), true);

		mainForm.append(new Spacer(getFormWidth(),getFormHeight()/20));
		
		choicePrivacy.setSelectedFlags(new boolean[]{Question.getInstance().isPrivateQuestion()});
		mainForm.append(choicePrivacy);
		
		
		
		mainForm.addCommand(cmdBack);
		mainForm.addCommand(cmdSubmit);
		
		cmdResponses = new Command(getLocalizedText(LocaleUI.START_CITEM_RESPONSES), Command.SCREEN, 2);
		cmdPrivateQuestions = new Command(getLocalizedText(LocaleUI.START_CITEM_PRIVATE_QUESTIONS), Command.SCREEN, 5);
		mainForm.addCommand(cmdResponses);
		mainForm.addCommand(cmdPrivateQuestions);
		
		mainForm.setCommandListener(this);
	}

	public void show() {		
        getDisplay().setCurrent(mainForm);
	}
	
	public int getFormHeight() {
		return mainForm.getHeight();
	}

	public int getFormWidth() {
		return mainForm.getWidth();
	}

	protected void refresh() {}

	public void commandAction(Command c, Displayable d)
	{
		super.commandAction(c, d);
		if (c == cmdSubmit) 
		{
			Question.getInstance().setSelAnswerTypeIdx(choiceOptions.getSelectedIndex());
			Question.getInstance().setSelAnswerDurationIdx(choiceDuration.getSelectedIndex());
			boolean[] flags = new boolean[1];
			choicePrivacy.getSelectedFlags(flags);
			if (flags[0]) 
				Question.getInstance().setPrivateQuestion(true);
			if (getLocalizedText(LocaleUI.OPTIONS_CHOICEGROUP_1_2).equals(choiceOptions.getString(choiceOptions.getSelectedIndex())) &&
						Question.getInstance().getUserAnswers()== null) {
				showAlert(getLocalizedText(LocaleUI.OPTIONS_ALERT_TEXT), AlertType.CONFIRMATION, this);
			} else {
				UIManager.getInstance().show(UIManager.SCREEN_WAIT);
			}
		} else if (c == cmdBack)  {
			UIManager.getInstance().showPrevious();
		}
	}
	
	public void itemStateChanged(Item item) 
	{
		if (item == choiceOptions) 
		{
			int index = choiceOptions.getSelectedIndex();
			Question.getInstance().setSelAnswerTypeIdx(index);
			Question.getInstance().setSelAnswerDurationIdx(choiceDuration.getSelectedIndex());
			boolean[] flags = new boolean[1];
			choicePrivacy.getSelectedFlags(flags);
			if (flags[0]) 
				Question.getInstance().setPrivateQuestion(true);
			if (getLocalizedText(LocaleUI.OPTIONS_CHOICEGROUP_1_2).equals(choiceOptions.getString(index)))
			{
				UIManager.getInstance().show(UIManager.SCREEN_ANSWER_OPTIONS);
			}
		}
	}

	protected Displayable getScreen() {
		return mainForm;
	}

}
