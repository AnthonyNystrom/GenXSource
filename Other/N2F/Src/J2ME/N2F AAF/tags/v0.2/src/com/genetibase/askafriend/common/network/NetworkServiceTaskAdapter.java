package com.genetibase.askafriend.common.network;

import com.genetibase.askafriend.common.network.stub.AskAFriendWSSoap;
import com.genetibase.askafriend.common.network.stub.AskAFriendWSSoap_Stub;
import com.genetibase.askafriend.common.utils.RunnableTaskAdapter;
import com.genetibase.askafriend.ui.GUIListener;

public abstract class NetworkServiceTaskAdapter extends RunnableTaskAdapter {
	
	public static final int TYPE_SUBMIT_QUESTION = 30;
	public static final int TYPE_ATTACH_PHOTO = 31;
	public static final int TYPE_COMPLETE_QUESTION = 32;
	public static final int TYPE_GET_MYAAFQUESTIONS = 33;
	public static final int TYPE_GET_MYAAFRESPONSE_DETAILS = 34;
	public static final int TYPE_GET_MYAAFCOMMENTS = 35;
	public static final int TYPE_GET_MYAAFPRIVATE_QUEST = 36;
	public static final int TYPE_GET_NEWCOMMENTS_QUEST = 37;
	public static final int TYPE_GET_NEWCOMMENTS = 38;
	public static final int TYPE_GET_CREDENTIALS = 39;
	
	protected AskAFriendWSSoap soap;
	protected NetworkServiceLogics handler;
	protected int operationType;
	protected String questionId;
	
	protected NetworkServiceTaskAdapter(int operationType, GUIListener listener, NetworkServiceLogics logic, String questionId) {
		this.operationType = operationType;
		this.listener = listener;
		this.handler = logic;
		this.questionId = questionId;
		this.soap = new AskAFriendWSSoap_Stub();
	}
	

	public int getType() {
		return operationType;
	}

}
