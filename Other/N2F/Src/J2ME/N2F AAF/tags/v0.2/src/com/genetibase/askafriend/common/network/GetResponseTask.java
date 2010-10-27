package com.genetibase.askafriend.common.network;

import java.util.Vector;

import com.genetibase.askafriend.common.network.stub.ArrayOfAskAFriendQuestion;
import com.genetibase.askafriend.common.network.stub.ArrayOfString;
import com.genetibase.askafriend.common.network.stub.AskAFriendQuestion;
import com.genetibase.askafriend.core.CommonKeys;
import com.genetibase.askafriend.core.Engine;
import com.genetibase.askafriend.core.QuestionInfoBean;
import com.genetibase.askafriend.ui.GUIListener;

public class GetResponseTask extends NetworkServiceTaskAdapter {
	private QuestionInfoBean bean;

	protected GetResponseTask(int operationType, GUIListener listener, NetworkServiceLogics logic,
			QuestionInfoBean bean) {
		super(operationType, listener, logic, bean == null? "": bean.getQuestionId());
		this.bean = bean;
	}

	protected void logic() throws Exception {
		handleResponse(questionId, bean == null? "": bean.getLastCommentId());

	}
	
	private void handleResponse(String questionId, String latestCommentId) throws Exception {
		String webMemberId = Engine.getEngine().getSystemProperty(CommonKeys.WEBMEMBERID);
		String password = Engine.getEngine().getSystemProperty(CommonKeys.PASSWORD);
		Object resultObj = null;
		switch (operationType) {
		case TYPE_GET_MYAAFQUESTIONS:
			resultObj = soap.getMyAAFQuestions(webMemberId, password, latestCommentId == null? "": latestCommentId);
			break;
		case TYPE_GET_MYAAFRESPONSE_DETAILS:
			resultObj = soap.getAAFResponse(webMemberId, password, questionId);
			break;
		case TYPE_GET_MYAAFCOMMENTS:
			System.out.println("questionId:"+questionId);
			System.out.println("lastcommentId:"+latestCommentId);
			resultObj = soap.getAAFComments(webMemberId, password, questionId, latestCommentId == null? "": latestCommentId);
			break;
		case TYPE_GET_MYAAFPRIVATE_QUEST:
			resultObj = soap.getPrivateAAFQuestion(webMemberId, password);
			break;
		case TYPE_GET_NEWCOMMENTS_QUEST:
			Vector v = Engine.getEngine().getResponseHolder().getQuestions();
			QuestionInfoBean bean = null;
			boolean hasNewComments = false;
			if (v == null) {
				resultObj = soap.getMyAAFQuestions(webMemberId, password, latestCommentId == null? "": latestCommentId);
				AskAFriendQuestion[] questions = ((ArrayOfAskAFriendQuestion)resultObj).getAskAFriendQuestion();
				v = new Vector(questions.length);
				for (int i = 0, size = questions.length; i < size; i++) {
					bean = new QuestionInfoBean(questions[i].getWebAskAFriendID(), questions[i].getQuestion(), null, 0);
					v.addElement(bean);
				}
			}
			int size = v.size();
			if (size > 0)
			{
				String[] lastIds = new String[size]; 
				String[] ids = new String[size];
				for (int i = 0; i < size; i++) {
					bean = (QuestionInfoBean) v.elementAt(i);
					lastIds[i] = bean.getLastCommentId() == null? "": bean.getLastCommentId();
					ids[i] = bean.getQuestionId(); 
System.out.println("send ***id ="+ids[i]+" lastId="+lastIds[i]);					
				}
				resultObj = soap.getNewAAFQuestionCommentIDs(webMemberId, password, new ArrayOfString(ids), new ArrayOfString(lastIds));
				if (resultObj != null) {
					String[] questions = ((ArrayOfString)resultObj).getString();
					if (questions.length > 0) {
						hasNewComments = true;
						for (int i = 0, len = questions.length; i < len; i++) {
							bean = new QuestionInfoBean(questions[i], "", null, 0);
							int index = v.indexOf(bean);
							if (index >-1) {
								QuestionInfoBean b = (QuestionInfoBean)v.elementAt(index);
								b.setHasNewComments(hasNewComments);
							} else {
								bean.setHasNewComments(hasNewComments);
								v.addElement(bean);
							}
						}
					} 
				}
			}
			System.out.println("has new responses="+ hasNewComments);
			Engine.getEngine().getResponseHolder().setQuestions(v);
			resultObj = new Boolean(hasNewComments);	

			break;
		default:
			break;
		}
		if (handler !=null)
			handler.analyzeServiceResponse(new ServiceResponse(operationType, resultObj), this);

	}

	public QuestionInfoBean getBean() {
		return bean;
	}
}
