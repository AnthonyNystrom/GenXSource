package com.genetibase.askafriend.common.network;

import com.genetibase.askafriend.common.network.stub.ArrayOfString;
import com.genetibase.askafriend.common.utils.Base64;
import com.genetibase.askafriend.core.CommonKeys;
import com.genetibase.askafriend.core.Engine;
import com.genetibase.askafriend.core.Question;
import com.genetibase.askafriend.ui.GUIListener;

public class SubmitQuestionTask extends NetworkServiceTaskAdapter {
	private Question question;
	private int indexOrder;
	
	public SubmitQuestionTask(int operationType, GUIListener listener, NetworkServiceLogics logic,
			Question quest, String questionId, int photoIndexOrder) 
	{
		super(operationType, listener, logic, questionId);
		this.question = quest;
		this.indexOrder = photoIndexOrder;
	}
	
	protected void logic() throws Exception
	{
		handleSumission(question, questionId, indexOrder);
	}
	
	protected void handleSumission(Question que, String questionId, int indexOrder) throws Exception 
	{
		String login = Engine.getEngine().getSystemProperty(CommonKeys.WEBMEMBERID);
		String password = Engine.getEngine().getSystemProperty(CommonKeys.PASSWORD);
		Object resultObj = null;
		if (handler == null)
			throw new RuntimeException("No handler set for nerwork task");
		switch (operationType) {
		case TYPE_SUBMIT_QUESTION:
			String[] answers = que.getUserAnswers();
			resultObj = soap.submitQuestion(login, password,
						que.getQuestionText(), que.getPicturesCount(), que.getSelAnswerTypeIdx(),
						new ArrayOfString(answers), que.getSelAnswerDurationIdx(),que.isPrivateQuestion());
			break;
		case TYPE_ATTACH_PHOTO:
			String s = Base64.encode(Engine.getEngine().getImageToSend(que.getPictureNames()[indexOrder-1]));
//System.out.println("attach: questionId="+questionId +" indexOrder="+indexOrder);			
			if (soap.attachPhoto(login, password, questionId, indexOrder, s) != null)
				resultObj = new Integer(indexOrder);
			break;
		case TYPE_COMPLETE_QUESTION:
			resultObj = soap.completeQuestion(login, password, questionId);
			break;
		default:
			break;
		}
		if (handler !=null)
			handler.analyzeServiceResponse(new ServiceResponse(operationType, resultObj), this);
	}
}
