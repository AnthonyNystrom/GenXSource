package com.genetibase.askafriend.core;

import com.genetibase.askafriend.common.network.stub.AskAFriendResponse;

public class ResponseDetailsWrapper {

	private AskAFriendResponse details;
	private QuestionInfoBean bean;
	
	public ResponseDetailsWrapper(AskAFriendResponse details, QuestionInfoBean bean) {
		this.details = details;
		this.bean = bean;
	}

	public QuestionInfoBean getBean() {
		return bean;
	}

	public AskAFriendResponse getDetails() {
		return details;
	}
}
