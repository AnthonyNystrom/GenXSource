package com.genetibase.askafriend.core;

import java.util.Hashtable;
import java.util.Vector;

import com.genetibase.askafriend.common.network.stub.AskAFriendComment;
import com.genetibase.askafriend.common.network.stub.AskAFriendResponse;
import com.genetibase.askafriend.common.network.stub.PrivateAAFQuestion;

public class ResponseHolder 
{
	private Vector questions;
	private Vector privateQuestions;
	private	ResponseDetailsWrapper responseDetails;
	private Hashtable bean2Comments = new Hashtable();
	
	public ResponseHolder() {
	}
	
	public Vector getQuestions() {
		return questions;
	}

	public void setQuestions(Vector questions) {
		this.questions = questions;
	}

	public Vector getPrivateQuestions() {
		return privateQuestions;
	}

	public boolean setPrivateQuestions(PrivateAAFQuestion[] array) {
		boolean ret = false;
		if (array != null && privateQuestions != null) {
			if (array.length != privateQuestions.size())
				ret = true;
		} else {
			ret = true;
			privateQuestions = new Vector(array.length);
		}
		for (int i = 0; i < array.length; i++) {
			this.privateQuestions.addElement(array[i]);
		}
		return ret;
	}

	public ResponseDetailsWrapper getResponseDetails() {
		return responseDetails;
	}

	public void setResponseDetails(AskAFriendResponse responseDetails, QuestionInfoBean bean) {
		this.responseDetails = new ResponseDetailsWrapper(responseDetails, bean);
	}

	public Vector getResponseComments(QuestionInfoBean bean) {
		return (Vector) bean2Comments.get(bean);
	}

	public void setResponseComments(QuestionInfoBean bean, AskAFriendComment[] responseComments) 
	{
		if (responseComments.length  > 0) {
			int len = responseComments.length;
			final Vector comments = new Vector(len);
			for (int i = 0; i < len; i++) {
//System.out.println(responseComments[i].getWebAskAFriendCommentID()+"-"+responseComments[i].getDateTimePosted());				
				comments.addElement(responseComments[i]);
			}		
			
			bean.setLastCommentId(responseComments[0].getWebAskAFriendCommentID());
			bean.setLastTimeCommentPosted(responseComments[0].getDateTimePosted());
			if (questions == null)
				questions = new Vector();
			int index = questions.indexOf(bean);
			if (index > -1) {
				QuestionInfoBean b = (QuestionInfoBean)questions.elementAt(index);
				b.setLastCommentId(responseComments[0].getWebAskAFriendCommentID());
				b.setLastTimeCommentPosted(responseComments[0].getDateTimePosted());
			} else {
				questions.addElement(bean);
			}
			 
			bean2Comments.put(bean, comments);
//			System.out.println("bean2comments:"+bean2Comments);
		} else {
			bean2Comments.remove(bean);
//			System.out.println("now new comments yet");
		}
	}
}
