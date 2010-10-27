package n2f.sup.common.network;

import java.util.Timer;
import java.util.TimerTask;

import n2f.sup.common.AbstractErrorManager;
import n2f.sup.common.Deallocatable;
import n2f.sup.common.ErrorListener;
import n2f.sup.common.PauseListener;
import n2f.sup.common.utils.RunnableTask;
import n2f.sup.common.utils.WorkerThread;
import n2f.sup.core.Engine;
import n2f.sup.core.ResourceManager;
import n2f.sup.ui.GUIListener;
import n2f.sup.ui.SUP_GUIListener;
import n2f.sup.ui.UIManager;
//import java.util.Vector;
import n2f.sup.ui.bean.SettingsBean;

//import com.genetibase.askafriend.common.ErrorEvent;
//import com.genetibase.askafriend.common.network.stub.ArrayOfAskAFriendComment;
//import com.genetibase.askafriend.common.network.stub.ArrayOfAskAFriendQuestion;
//import com.genetibase.askafriend.common.network.stub.ArrayOfPrivateAAFQuestion;
//import com.genetibase.askafriend.common.network.stub.AskAFriendConfirm;
//import com.genetibase.askafriend.common.network.stub.AskAFriendQuestion;
//import com.genetibase.askafriend.common.network.stub.AskAFriendResponse;
//import com.genetibase.askafriend.core.Question;
//import com.genetibase.askafriend.core.QuestionInfoBean;

public class NetworkServiceLogics extends AbstractErrorManager implements Deallocatable, PauseListener {
	
//	private String currentQuestionId;
	private Timer timer;
	private TimerTask task;
//	private static final int FIVE_MIN = 300000;
	private WorkerThread wt;
	
	public void init()
	{
		if (Engine.getEngine().isInitNetworkTimer())
		{
//			if (task == null) {
//				this.task = new TimerTask()
//				{
//					private long counter;
//					public void run() {
//						if (counter++/2 == 0)
//							getAAFPrivateQuestions(null);
//						else
//							getAllNewComments();
//					}
//				};
//				this.timer = new Timer();
//				timer.scheduleAtFixedRate(task, FIVE_MIN, FIVE_MIN);
//			}
		}
	}
	
	public NetworkServiceLogics(ResourceManager manager) {
		wt = new WorkerThread();
	}
	
	public void analyzeServiceResponse(ServiceResponse response, NetworkServiceTaskAdapter owner)
	{
		switch (response.getRequestType()) 
		{
		case NetworkServiceTaskAdapter.TYPE_GET_CREDENTIALS:
			String userId = (String) response.getSource();
			SettingsBean cartBean = (SettingsBean) Engine.getEngine().getBean(UIManager.SCREEN_SETTINGS);
			cartBean.setWebMemberId(userId);
			break;
			
		case NetworkServiceTaskAdapter.TYPE_SUP_UPLOAD_PHOTO:
//			SUP
			
			break;
		}
	}
	
	private void executeInWorkerThread(RunnableTask runnableTask) {
		if (wt != null) {
			wt.put(runnableTask);
		}
	}
	
	public void getWebMemberId(GUIListener lis) 
	{
		GetUserIdTask task = new GetUserIdTask(GetUserIdTask.TYPE_GET_CREDENTIALS, lis, this);
		executeInWorkerThread(task);
	}
	
	//new function for SUP version 
	public void uploadPhoto(SUP_GUIListener listener, byte[] image, long creationTime) {
		SUPServiceTask nsl = new SUPServiceTask(NetworkServiceTaskAdapter.TYPE_SUP_UPLOAD_PHOTO, listener, this, image, creationTime);
		executeInWorkerThread(nsl);
	} 
	
	
	public void free() 
	{
		if (this.task != null) {
			this.task.cancel();
		}
		if (this.timer != null) {
			this.timer.cancel();
		} 
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
