package com.genetibase.askafriend.ui;

import java.util.Enumeration;
import java.util.Vector;

import javax.microedition.lcdui.Alert;
import javax.microedition.lcdui.AlertType;
import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.Image;
import javax.microedition.lcdui.List;

import com.genetibase.askafriend.common.Resoursable;
import com.genetibase.askafriend.common.network.stub.PrivateAAFQuestion;
import com.genetibase.askafriend.common.utils.StringTokenizer;
import com.genetibase.askafriend.core.Engine;
import com.genetibase.askafriend.core.ResponseHolder;

public class PrivateQuestionListScreen extends AbstractWindow {
	private List mainForm;
	private Command cmdRefresh, cmdBack, cmdGoWap;
	private final static Image SEL_IMAGE = Engine.getEngine().getImage("/selected.png");
	
	public PrivateQuestionListScreen(String id, Resoursable resoursable) 
	{
		super(id, resoursable);
		cmdRefresh = new Command(getLocalizedText(LocaleUI.RESPONSE_COMMAND_REFRESH),Command.SCREEN, 3);  
		cmdBack = new Command(getLocalizedText(LocaleUI.RESPONSE_COMMAND_BACK),Command.BACK, 3);
		cmdGoWap = new Command(getLocalizedText(LocaleUI.PRIVATE_QUESTIONS_COMMAND_GOWAP), Command.SCREEN,3);
		
		mainForm = new List(getLocalizedText(LocaleUI.FORM_PRIVATE_QUESTIONS_CAPTION), List.IMPLICIT);
		mainForm.addCommand(cmdRefresh);
		mainForm.addCommand(cmdBack);
		mainForm.setSelectCommand(cmdGoWap);
		mainForm.setCommandListener(this);
	}
	
	private ResponseHolder getHolder() {
		return Engine.getEngine().getResponseHolder();
	}

	public void show()
	{		
		if (getHolder().getPrivateQuestions() == null) {
			showBusy(null);
			Engine.getEngine().getAAFPrivateQuestions(this);
		} else {
			refresh();
		}
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
		Vector questions = getHolder().getPrivateQuestions();
		if (questions != null && (!questions.isEmpty())) {
			StringBuffer sb = new StringBuffer();
			PrivateAAFQuestion q = null;
			for (Enumeration en = questions.elements(); en.hasMoreElements();) {
				q = (PrivateAAFQuestion) en.nextElement();
				sb.append(q.getNickName()).
				append("(").append(StringTokenizer.parseDate(q.getDateTimePosted())).append(")").
				append("\n").append(q.getQuestion());
				mainForm.append(sb.toString(), SEL_IMAGE);
				sb.setLength(0);
			}
		}
		getDisplay().setCurrent(mainForm);
	}

	public void commandAction(Command c, Displayable d) {
		super.commandAction(c, d);		
		if (c == cmdGoWap) {
			Vector questions = getHolder().getPrivateQuestions();
			if (questions != null && (!questions.isEmpty())) {
				String seleString = mainForm.getString(mainForm.getSelectedIndex());
				PrivateAAFQuestion q = null;
				for (Enumeration en = questions.elements(); en.hasMoreElements();) {
					q = (PrivateAAFQuestion) en.nextElement();
					if (seleString.indexOf(q.getNickName())>-1 && seleString.indexOf(q.getQuestion())>-1) {
						break;
					}
				}
				if (q != null){
					if (!Engine.getEngine().goWAP(q.getURL())){
						showAlert(getLocalizedText(LocaleUI.PRIVATE_QUESTIONS_ALERT_NOWAP_TEXT), AlertType.ERROR, this);
					}
				}
			}
		}
		if (c == cmdRefresh) {
			Engine.getEngine().getAAFQuestions(this);
		}
		if (c == cmdBack) {
			Engine.getEngine().breakNetworkConnection();
			UIManager.getInstance().showPrevious();
		}
		if (c == Alert.DISMISS_COMMAND) {
			getDisplay().setCurrent(mainForm);
		}
	}

	protected Displayable getScreen() {
		return mainForm;
	}

}
