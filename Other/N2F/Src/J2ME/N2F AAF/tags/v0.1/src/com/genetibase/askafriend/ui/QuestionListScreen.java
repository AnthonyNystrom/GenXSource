package com.genetibase.askafriend.ui;

import java.util.Enumeration;
import java.util.Vector;

import javax.microedition.lcdui.Alert;
import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.Image;
import javax.microedition.lcdui.List;


import com.genetibase.askafriend.common.Resoursable;
import com.genetibase.askafriend.core.Engine;
import com.genetibase.askafriend.core.QuestionInfoBean;
import com.genetibase.askafriend.core.ResponseHolder;

class QuestionListScreen extends AbstractWindow {
	
	private List mainForm;
	private Command cmdRefresh, cmdBack, cmdDetails, cmdComments;
	private final static Image OLD_IMAGE = Engine.getEngine().getImage("/unselected.png");
	private final static Image NEW_IMAGE = Engine.getEngine().getImage("/selected.png");	
	
	QuestionListScreen(String id, Resoursable resoursable) {
		super(id, resoursable);
		
		cmdRefresh = new Command(getLocalizedText(LocaleUI.RESPONSE_COMMAND_REFRESH),Command.SCREEN, 3);  
		cmdBack = new Command(getLocalizedText(LocaleUI.RESPONSE_COMMAND_BACK),Command.BACK, 3);
		cmdDetails = new Command(getLocalizedText(LocaleUI.RESPONSE_COMMAND_DETAILS), Command.SCREEN,3);
		cmdComments = new Command(getLocalizedText(LocaleUI.ANSWER_DETAILS_COMMAND_COMMENTS), Command.SCREEN,3);
		
		mainForm = new List(getLocalizedText(LocaleUI.FORM_RESPONSE_CAPTION), List.IMPLICIT);
		mainForm.addCommand(cmdRefresh);
		mainForm.addCommand(cmdBack);
		mainForm.setSelectCommand(cmdDetails);
		mainForm.setSelectCommand(cmdComments);
		mainForm.setCommandListener(this);
	}
	
	private ResponseHolder getHolder() {
		return Engine.getEngine().getResponseHolder();
	}

	public void show() 
	{
		showBusy(null);
		if (getHolder().getQuestions() == null) { 
			getQuestionsUpdate();
		} else 
			refresh();
	}

	private void getQuestionsUpdate() {
		Engine.getEngine().getAAFQuestions(null);
	    Engine.getEngine().getQuestionsWithComments(this);
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
		mainForm.deleteAll();
		Vector v = getHolder().getQuestions();
		if (v != null) {
			QuestionInfoBean bean = null;
			if (v.size() > 0) {
				for (Enumeration en = v.elements(); en.hasMoreElements();) {
					bean = (QuestionInfoBean) en.nextElement();
					mainForm.append(bean.getQuestionTxt(), bean.hasNewComments()?NEW_IMAGE: OLD_IMAGE);
				}
				bean = (QuestionInfoBean) Engine.getEngine().getBean(UIManager.SCREEN_RESPONSE_DETAILS);
				if (v.indexOf(bean) >-1) {
					mainForm.setSelectedIndex(v.indexOf(bean), true);
				}
			} else {
				mainForm.append(getLocalizedText(LocaleUI.FORM_RESPONSE_TEXT_EMPTY), null);
			}
		}
        getDisplay().setCurrent(mainForm);
	}

	public void commandAction(Command c, Displayable d)
	{
		super.commandAction(c, d);
		if (c == Alert.DISMISS_COMMAND) {
			getDisplay().setCurrent(mainForm);
		}else if (c == cmdDetails) {
			Vector v = getHolder().getQuestions();
			if (v != null) {
				String seleString = mainForm.getString(mainForm.getSelectedIndex());
				QuestionInfoBean bean = null;
				for (Enumeration en = v.elements(); en.hasMoreElements();) {
					if (seleString.equals((bean = (QuestionInfoBean)en.nextElement()).getQuestionTxt()))
					{
						Engine.getEngine().putBean(UIManager.SCREEN_RESPONSE_DETAILS, bean);
						UIManager.getInstance().show(UIManager.SCREEN_RESPONSE_DETAILS);
						break;
					}
				}
			}
		} else if (c == cmdComments) {
			Vector v = getHolder().getQuestions();
				if (v != null) {
					String seleString = mainForm.getString(mainForm.getSelectedIndex());
					QuestionInfoBean bean = null;
					for (Enumeration en = v.elements(); en.hasMoreElements();) {
						if (seleString.equals((bean = (QuestionInfoBean)en.nextElement()).getQuestionTxt()))
						{
							Engine.getEngine().putBean(UIManager.SCREEN_RESPONSE_DETAILS, bean);
							UIManager.getInstance().show(UIManager.SCREEN_RESPONSE_COMMENTS);
							break;
						}
					}
				}
		} else if (c == cmdRefresh) {
			getQuestionsUpdate();
		} else if (c == cmdBack) {
			Engine.getEngine().breakNetworkConnection();
			UIManager.getInstance().showPrevious();
		}
	}

	protected Displayable getScreen() {
		return mainForm;
	}

}
