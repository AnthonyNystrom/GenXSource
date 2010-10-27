package com.genetibase.askafriend.ui;

import java.util.Enumeration;
import java.util.Vector;

import javax.microedition.lcdui.Alert;
import javax.microedition.lcdui.AlertType;
import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.Form;
import javax.microedition.lcdui.StringItem;

import com.genetibase.askafriend.common.Resoursable;
import com.genetibase.askafriend.common.network.stub.AskAFriendComment;
import com.genetibase.askafriend.common.utils.StringTokenizer;
import com.genetibase.askafriend.core.Engine;
import com.genetibase.askafriend.core.QuestionInfoBean;
import com.genetibase.askafriend.core.ResponseHolder;

public class CommentsByUserScreen extends AbstractWindow {
	private Form mainForm;
	private Command cmdBack;
	private Command cmdRefresh;
	
	public CommentsByUserScreen(String id, Resoursable resoursable) {
		super(id, resoursable);
		this.mainForm = new Form(getLocalizedText(id));
		this.cmdBack = new Command(getLocalizedText(LocaleUI.ASK_QUESTION_COMMAND_BACK),Command.CANCEL,1);
		this.cmdRefresh = new Command(getLocalizedText(LocaleUI.COMMENTS_COMMAND_REFRESH),Command.SCREEN,2);
		mainForm.addCommand(cmdBack);
		mainForm.addCommand(cmdRefresh);
		mainForm.setCommandListener(this);
	}
	
	private ResponseHolder getHolder() {
		return Engine.getEngine().getResponseHolder();
	}

	public void show()
	{
		
		QuestionInfoBean bean = (QuestionInfoBean)Engine.getEngine().getBean(UIManager.SCREEN_RESPONSE_DETAILS);
		if (bean != null) {
//		if (getHolder().getResponseComments(bean) != null && !bean.hasNewComments()) {
			showBusy(null);
			Engine.getEngine().loadComments(this, bean);
//		}
		}
//		} else 
//			refresh();
	}

	public int getFormHeight() {
		return mainForm.getHeight();
	}

	public int getFormWidth() {
		return mainForm.getWidth();
	}

	protected void refresh() {
		hideBusy();
		mainForm.deleteAll();
		QuestionInfoBean bean = (QuestionInfoBean)Engine.getEngine().getBean(UIManager.SCREEN_RESPONSE_DETAILS);
		Vector comments = getHolder().getResponseComments(bean);
		if (comments != null && comments.size() > 0) 
		{
			StringBuffer sb = new StringBuffer();
			AskAFriendComment comment = null;
			for (Enumeration en = comments.elements(); en.hasMoreElements();) {
				comment = (AskAFriendComment) en.nextElement();
				sb.append(comment.getNickName()).
				append("(").append(StringTokenizer.parseDate(comment.getDateTimePosted())).append(")").
				append("\n").append(comment.getText());
				mainForm.append(new StringItem(null, sb.toString()));
				sb.setLength(0);
			}
			getDisplay().setCurrent(mainForm);
		} else {
//			mainForm.append("No comments are available");
			getDisplay().setCurrent(mainForm);
			Alert a = new Alert(null, getLocalizedText(LocaleUI.ALERT_COMMENTS_TEXT), null, AlertType.CONFIRMATION);
			a.addCommand(cmdRefresh);
			a.addCommand(cmdBack);
			a.setCommandListener(this);
			getDisplay().setCurrent(a);
		}
		
	}
	
	public void commandAction(Command c, Displayable d) {
		
		QuestionInfoBean bean = (QuestionInfoBean)Engine.getEngine().getBean(UIManager.SCREEN_RESPONSE_DETAILS);
		int index = getHolder().getQuestions().indexOf(bean);
		if (index > -1)
			((QuestionInfoBean)getHolder().getQuestions().elementAt(index)).setHasNewComments(false);
		bean.setHasNewComments(false);
		if (c == Alert.DISMISS_COMMAND) {
			UIManager.getInstance().showPrevious();
		} else if (c == cmdBack) {
			UIManager.getInstance().showPrevious();
		} else if (c == cmdRefresh) {
			showBusy(null);
			bean.setLastCommentId(null);
			Engine.getEngine().loadComments(this, bean);
		}
	}

	protected Displayable getScreen() {
		return mainForm;
	}

}
