package com.genetibase.askafriend.common.network;

import java.util.Timer;
import java.util.TimerTask;
import java.util.Vector;

import com.genetibase.askafriend.common.AbstractErrorManager;
import com.genetibase.askafriend.common.Deallocatable;
import com.genetibase.askafriend.common.ErrorEvent;
import com.genetibase.askafriend.common.ErrorListener;
import com.genetibase.askafriend.common.PauseListener;
import com.genetibase.askafriend.common.network.stub.ArrayOfAskAFriendComment;
import com.genetibase.askafriend.common.network.stub.ArrayOfAskAFriendQuestion;
import com.genetibase.askafriend.common.network.stub.ArrayOfPrivateAAFQuestion;
import com.genetibase.askafriend.common.network.stub.AskAFriendConfirm;
import com.genetibase.askafriend.common.network.stub.AskAFriendQuestion;
import com.genetibase.askafriend.common.network.stub.AskAFriendResponse;
import com.genetibase.askafriend.common.utils.RunnableTask;
import com.genetibase.askafriend.common.utils.WorkerThread;
import com.genetibase.askafriend.core.Engine;
import com.genetibase.askafriend.core.Question;
import com.genetibase.askafriend.core.QuestionInfoBean;
import com.genetibase.askafriend.core.ResourceManager;
import com.genetibase.askafriend.ui.GUIListener;
import com.genetibase.askafriend.ui.SettingsBean;
import com.genetibase.askafriend.ui.UIManager;
import com.genetibase.askafriend.utils.Debug;

public class NetworkServiceLogics extends AbstractErrorManager implements Deallocatable, PauseListener {
	
	private String currentQuestionId;
	private Timer timer;
	private TimerTask task;
	private static final int FIVE_MIN = 300000;
	private WorkerThread wt;
	
	public void init()
	{
		if (task == null) {
			this.task = new TimerTask()
			{
				private long counter;
				public void run() {
					if (counter++/2 == 0)
						getAAFPrivateQuestions(null);
					else
						getAllNewComments(null);
				}
			};
			this.timer = new Timer();
			timer.scheduleAtFixedRate(task, FIVE_MIN, FIVE_MIN);
		}
	}
	
	public NetworkServiceLogics(ResourceManager manager) {
		wt = new WorkerThread();
	}
	
	public void analyzeServiceResponse(ServiceResponse response, NetworkServiceTaskAdapter owner)
	{
		switch (response.getRequestType()) 
		{
		case SubmitQuestionTask.TYPE_SUBMIT_QUESTION:
			AskAFriendConfirm aafConfirm = (AskAFriendConfirm) response.getSource();
			
			if (aafConfirm != null) {
				Engine.getEngine().setSubmitConfirm(aafConfirm);
				Debug.println("question was submitted correctly");
				currentQuestionId = aafConfirm.getWebAskAFriendID();
				if (Question.getInstance().getPictureNames() != null)
					attachPhoto(Question.getInstance(), currentQuestionId, 1, null);
				else 
					completeQuestion(submitListener, currentQuestionId);
			}
			break;
		case SubmitQuestionTask.TYPE_ATTACH_PHOTO:
			Integer result = (Integer) response.getSource();
			if (result != null) 
			{
				Debug.println("attach was submitted correctly:"+result.intValue());
				if (result.intValue() < Question.getInstance().getPicturesCount())
				{
					attachPhoto(Question.getInstance(), currentQuestionId, result.intValue() +1, null);
				} else {
					completeQuestion(submitListener, currentQuestionId);
				}
			}
			break;
		case SubmitQuestionTask.TYPE_COMPLETE_QUESTION:
			
			if (response.getSource() != null) {
				Debug.println("complete was submitted correctly");
				Engine.getEngine().setSubmissionSucceeded(true);
			}
			submitListener = null;
			break;
		case GetResponseTask.TYPE_GET_MYAAFQUESTIONS:
			ArrayOfAskAFriendQuestion arrayQ = (ArrayOfAskAFriendQuestion) response.getSource();
			Vector v = Engine.getEngine().getResponseHolder().getQuestions();
			if (arrayQ != null) {
				AskAFriendQuestion[] questions = arrayQ.getAskAFriendQuestion();
				if (v == null || (v.size() != questions.length))
					v = new Vector(questions.length);
				QuestionInfoBean bean = null;
				for (int i = 0, size = questions.length; i < size; i++) {
					bean = new QuestionInfoBean(questions[i].getWebAskAFriendID(), questions[i].getQuestion(), null, 0);
					if (!v.contains(bean)) {
						v.addElement(bean);
					}
				}
			}
			Engine.getEngine().getResponseHolder().setQuestions(v);
			break;
		case GetResponseTask.TYPE_GET_MYAAFRESPONSE_DETAILS:
			AskAFriendResponse mobiResponse = (AskAFriendResponse) response.getSource();
			if (mobiResponse != null) {
				Engine.getEngine().getResponseHolder().setResponseDetails(mobiResponse, ((GetResponseTask)owner).getBean());
			} 
			break;
		case GetResponseTask.TYPE_GET_MYAAFCOMMENTS:
			ArrayOfAskAFriendComment array = (ArrayOfAskAFriendComment)response.getSource();
			Vector vect = Engine.getEngine().getResponseHolder().getQuestions();
			
			if (array != null && array.getAskAFriendComment().length > 0) {
				int arrayLen = array.getAskAFriendComment().length;
				QuestionInfoBean bean = ((GetResponseTask)owner).getBean();
				bean.setLastCommentId(array.getAskAFriendComment()[arrayLen-1].getWebAskAFriendCommentID());
				bean.setLastTimeCommentPosted(array.getAskAFriendComment()[arrayLen-1].getDateTimePosted().trim());
				Engine.getEngine().getResponseHolder().setResponseComments(bean, array.getAskAFriendComment());
				int index = vect.indexOf(bean);
				if (index >-1) {
					QuestionInfoBean b = (QuestionInfoBean)vect.elementAt(index);
					b.setHasNewComments(bean.hasNewComments());
					b.setLastCommentId(bean.getLastCommentId());
					b.setLastTimeCommentPosted(String.valueOf(b.getLastTimeCommentPosted()));
				}
				System.out.println("update:"+bean.getQuestionId() +"-"+ bean.getLastCommentId());
			}
			break;
		case GetResponseTask.TYPE_GET_MYAAFPRIVATE_QUEST:
			ArrayOfPrivateAAFQuestion privateQuestions =(ArrayOfPrivateAAFQuestion) response.getSource();
			if (privateQuestions != null && 
					privateQuestions.getPrivateAAFQuestion() != null && 
					privateQuestions.getPrivateAAFQuestion().length > 0) {
				if (Engine.getEngine().getResponseHolder().setPrivateQuestions(privateQuestions.getPrivateAAFQuestion()))
				{
//				TODO:check if we have new messages 
					fireError("New private question!", new ErrorEvent(this, 
						new RuntimeException("New private question!"),
						"New private question!", ErrorEvent.NEW_PRIVATE_QUESTION));
				}
			} 
			break;
		case GetResponseTask.TYPE_GET_NEWCOMMENTS_QUEST:
			Boolean newComments = (Boolean) response.getSource();
			System.out.println("analyze!!!!! new rsponses");
			if (newComments!= null && newComments.booleanValue() && owner.getListener() == null) {
				fireError("New comment!", new ErrorEvent(this, new RuntimeException("New comment!"), "New comment!", ErrorEvent.NEW_REPLY));	
			}
			break;
		case GetUserIdTask.TYPE_GET_CREDENTIALS:
			String userId = (String) response.getSource();
			SettingsBean cartBean = (SettingsBean) Engine.getEngine().getBean(UIManager.SCREEN_SETTINGS);
			cartBean.setWebMemberId(userId);
			break;
		}
	}
	
	public void executeInWorkerThread(RunnableTask runnableTask) {
		if (wt != null) {
			wt.put(runnableTask);
		}
	}
	
	private GUIListener submitListener;
	public void submitQuestion(Question question, GUIListener listener) 
	{
		submitListener = listener;
		SubmitQuestionTask task = new SubmitQuestionTask(SubmitQuestionTask.TYPE_SUBMIT_QUESTION, null, this, question, null, 0);
		executeInWorkerThread(task);
	}

	public void attachPhoto(Question question, String questId,int indexOrder, GUIListener listener) 
	{
		SubmitQuestionTask task = new SubmitQuestionTask(SubmitQuestionTask.TYPE_ATTACH_PHOTO, listener, this, question, questId, indexOrder);
		executeInWorkerThread(task);
	}
	
	public void completeQuestion(GUIListener listener, String questId) 
	{
		SubmitQuestionTask task = new SubmitQuestionTask(SubmitQuestionTask.TYPE_COMPLETE_QUESTION, listener, this, null, questId, 0);
		executeInWorkerThread(task);
	}

	public void getAAFResponseDetails(GUIListener listener, QuestionInfoBean bean) 
	{
		GetResponseTask task = new GetResponseTask(GetResponseTask.TYPE_GET_MYAAFRESPONSE_DETAILS, listener, this, bean);
		executeInWorkerThread(task);
	}
	
	public void getAAFQuestions(GUIListener listener) 
	{
		GetResponseTask task = new GetResponseTask(GetResponseTask.TYPE_GET_MYAAFQUESTIONS, listener, this, null);
		executeInWorkerThread(task);
	}
	
	public void getAAFResponseComments(GUIListener listener, QuestionInfoBean bean) 
	{
		GetResponseTask task = new GetResponseTask(GetResponseTask.TYPE_GET_MYAAFCOMMENTS, listener, this, bean);
		executeInWorkerThread(task);
	}
	
	public void getAAFPrivateQuestions(GUIListener listener) 
	{
		GetResponseTask task = new GetResponseTask(GetResponseTask.TYPE_GET_MYAAFPRIVATE_QUEST, listener, this, null);
		executeInWorkerThread(task);
	}
	
	public synchronized void getAllNewComments(GUIListener lis) 
	{
		GetResponseTask task = new GetResponseTask(GetResponseTask.TYPE_GET_NEWCOMMENTS_QUEST, lis, this, null);
		executeInWorkerThread(task);
	}
	
	public void getWebMemberId(GUIListener lis) 
	{
		GetUserIdTask task = new GetUserIdTask(GetUserIdTask.TYPE_GET_CREDENTIALS, lis, this);
		executeInWorkerThread(task);
	}
	
	public void free() 
	{
		this.task.cancel();
		this.timer.cancel();
		task = null;
		timer = null;
		wt.free();
		wt = null;
	}
	
	public void notify(int state) {
		if (timer != null) {
			timer.cancel();
		}
		if (wt != null) {
			wt.free();
			wt = null;
		}
		//no need to check if the application goes to pause
		if (state == STATE_RESTORE) {
			wt = new WorkerThread();
			init();
		} 
	}
	
	public void addErrorListener(ErrorListener errorListener) {
		super.addErrorListener(errorListener);
		if (wt != null) {
			wt.addErrorListener(errorListener);
		}
	}

	public void breakNetworkConnection() {
		wt.removeAllTasks();
	}
}
