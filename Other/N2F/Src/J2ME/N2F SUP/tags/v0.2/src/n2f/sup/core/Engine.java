package n2f.sup.core;

import java.io.InputStream;
import java.util.Enumeration;
import java.util.Hashtable;
import java.util.Vector;

import javax.microedition.io.ConnectionNotFoundException;
import javax.microedition.lcdui.Canvas;
import javax.microedition.lcdui.Image;
import javax.microedition.midlet.MIDlet;

import n2f.sup.common.AbstractErrorManager;
import n2f.sup.common.ActionListener;
import n2f.sup.common.Deallocatable;
import n2f.sup.common.PauseListener;
import n2f.sup.common.file.FileUtils;
import n2f.sup.common.network.NetworkServiceLogics;
import n2f.sup.common.utils.GraphicsUtils;
import n2f.sup.common.utils.RunnableTask;
import n2f.sup.common.utils.Utils;
import n2f.sup.ui.GUIListener;
import n2f.sup.ui.SUP_GUIListener;
import n2f.sup.ui.UIManager;
import n2f.sup.ui.bean.AbstractBean;

//import com.genetibase.askafriend.camera.SnapshotEncodingWrapper;
//import com.genetibase.askafriend.camera.VideoImpl;
//import com.genetibase.askafriend.camera.VideoPlayer;
//import com.genetibase.askafriend.common.network.stub.AskAFriendConfirm;

public class Engine extends AbstractErrorManager{
	private Vector pauseList = new Vector();
//	private VideoImpl player = null;
	private Vector deallocatableList = new Vector();
	
	private MIDlet midlet;
	private boolean initNetworkTimer = true;
	
	private static Engine INSTANCE;
	private ResourceManager resManager;
	private NetworkServiceLogics netManager;
//	private AskAFriendConfirm submitConfirm;
	private boolean submissionSucceeded;
	private Hashtable beans = new Hashtable();
//	private byte[] capturedImage;
//	private ResponseHolder responseHolder;
	
	public void addDeallocatable(Deallocatable deallocatable) {
		if (!this.deallocatableList.contains(deallocatable)) {
			this.deallocatableList.addElement(deallocatable);
		}
	}
	
	private Engine() {
		getResourceManager();
		netManager = new NetworkServiceLogics(getResourceManager());
		netManager.addErrorListener(UIManager.getInstance());
		
//		responseHolder = new ResponseHolder();
		
		addDeallocatable(UIManager.getInstance());		
		addPauseListener(netManager);
		addDeallocatable(netManager);
	}
	
	private ResourceManager getResourceManager() {
		if (resManager == null) {
			resManager = new ResourceManager();
			addPauseListener(resManager);
			UIManager.getInstance().setResoursable(resManager);
			addDeallocatable(resManager);
			resManager.addErrorListener(UIManager.getInstance());
		}
		return resManager;
	}
	
	public static Engine getEngine() {
		if (INSTANCE == null) {
			INSTANCE = new Engine();
		}
		return INSTANCE;
	}
	
	public void requestWebMemberId(GUIListener lis) {
		netManager.getWebMemberId(lis);
	}
	
	
	public Image getImage(String name) {
		return resManager.getImage(name);
	}
	
	public Image getImage(String name, boolean cacheImage) {
		return resManager.getImage(name, cacheImage);
	}
	
	public ImageWrapper getImageFromFile(String id, int idOnForm) throws Exception {
		return resManager.getImageFromFile(id, idOnForm);
	}
	
	public byte[] getImageToSend(String imageName) {
		return resManager.getImageForSend(imageName);
	}
	
	public void loadPreview(String name, int indexOnForm, GUIListener lis) {
		resManager.preview(new ImageWrapper(name, indexOnForm), lis);
	}
	
	public MIDlet getMidlet() {
		return midlet;
	}
	
	public void setMIDlet(MIDlet midlet, boolean initNetworkTimer) {
		this.midlet = midlet;
		this.initNetworkTimer = initNetworkTimer;
	}
	
	public void open(String selected, GUIListener lis) {
		//UIManager.getInstance().getCurrentUI().showBusy(null);
		getResourceManager().open(selected, lis);
	}
	
	public void scaleImage(final String path, final Canvas canvas, final ActionListener actionListener) {
		getResourceManager().executeInWorkerThread(new RunnableTask(){
		    public void execute() throws Exception{
				int status = ActionListener.STATUS_OK;
				Object retValue = null;
				InputStream is = null;
				try {
					ImageWrapper imageWrapper = new ImageWrapper(path, 0);
					FileUtils.loadImage(imageWrapper);
					Image image = imageWrapper.getImage();
					double scale = Math.min((double)canvas.getHeight()/ image.getHeight(), (double)canvas.getWidth()/image.getWidth());
					retValue = GraphicsUtils.scaleImage(image, scale);
				} catch (Throwable e) {
					status = ActionListener.STATUS_FAILED;
					retValue = e;					
					e.printStackTrace();
				} finally {
					Utils.close(is);
					if (actionListener != null) {
						actionListener.actionPerformed(retValue, status);
					}
					System.gc();
				}
		    }

			public void interrupt(){
		    }

			public int getType() {
				return TYPE_SCALE_IMAGE;
			}
		});
	}

	
	//TODO: make callback when it is not possibl to save the file because of some reasons:
	//
	//1. the file alredy exists
	//2. the file can't be created due security reason
	public void save(String filename, byte[] source, GUIListener lis) {
		log("Engine.save:"+ filename);		
		getResourceManager().save(filename, source, lis);
	}
	
	public void delete(String selected, GUIListener lis) {
		getResourceManager().delete(selected, lis);
		
	}

	public void openInit(GUIListener listener) {
		getResourceManager().openInit(listener);
	}

	public Vector getCurrentFiles(String selectedFile) {
		return getResourceManager().getCurrentFiles(selectedFile);
	}
	
//	private VideoPlayer getVideoPlayer() {
//		if (player == null) {
//			player = new VideoImpl();
//			addDeallocatable(player);
//			player.addErrorListener(UIManager.getInstance());
//		}
//		return player;
//	}
//	
//	public void showCamera(Canvas canvas) {
//		getVideoPlayer().showCamera(canvas);
//	}
//	
//	public void stopCamera() {
//		getVideoPlayer().stopCamera();
//	}
		
//	public void setCapturedImage(byte[] capturedImage) {
//		this.capturedImage = capturedImage;
//	}
//	
//	public byte[] getCapturedImage() {
//		return this.capturedImage;
//	}
//	
//	public void capture(ActionListener actionListener) {
//		getVideoPlayer().capture(actionListener);
//	}
//		
//	public Image getSnapShotImage(byte[] imageData) { 
//		return getVideoPlayer().getSnapShotImage(imageData);
//	}

	public void disposePreviewCache() {
		getResourceManager().disposePreviewCache();
	}
	
	public void dispatchAction (ActionHero action) {
		switch (action.getActionType()) {
		case ActionHero.ACTION_TYPE_OPEN_FOLDER:
			Engine.getEngine().open(action.getTitle(), action.getListener());
    		break;
		case ActionHero.ACTION_TYPE_CAMERA:
    		break;
		case ActionHero.ACTION_TYPE_FILE_SYSTEM:
    		break;
		default:
    		break;
		}		
	}
	
	public void exit() {
		getMidlet().notifyDestroyed();
	}
	
//	public void submitQuestion(Question question, GUIListener listener) {
//		netManager.submitQuestion(question, listener);
//	}
	
	public void breakNetworkConnection() {
		netManager.breakNetworkConnection();
	}
	
//	public void getAAFResponseDetails(GUIListener list, QuestionInfoBean bean) {
//		netManager.getAAFResponseDetails(list, bean);
//	}
//	
//	public void getAAFQuestions(GUIListener list) {
//		netManager.getAAFQuestions(list);
//	}
//
//	public void getAAFPrivateQuestions(GUIListener list) {
//		netManager.getAAFPrivateQuestions(list);
//	}
//	
//	public AskAFriendConfirm getSubmitConfirm() {
//		return submitConfirm;
//	}
//	
//	public void setSubmitConfirm(AskAFriendConfirm submitConfirm) {
//		this.submitConfirm = submitConfirm;
//	}

	public String getSystemProperty(String name) {
		return getResourceManager().getProperty(name);
	}
	
	public boolean isAuthorized() {
		return CommonKeys.TRUE.equals(getResourceManager().getProperty(CommonKeys.AUTH));
	}
	
	public void setAuthorized(boolean isAuthorized) {
		getResourceManager().setProperty(CommonKeys.AUTH, isAuthorized ? CommonKeys.TRUE : CommonKeys.FALSE);
	}
	public void destroy() {
		removeAllErrorListeners();
		for (Enumeration dealEnum = this.deallocatableList.elements(); dealEnum.hasMoreElements(); ) {
			((Deallocatable)dealEnum.nextElement()).free();
		}
		this.beans.clear();
		this.pauseList.removeAllElements();
		resManager = null;
	}

	public boolean isSubmissionSucceeded() {
		return submissionSucceeded;
	}

	public void setSubmissionSucceeded(boolean submissionSucceeded) {
		this.submissionSucceeded = submissionSucceeded;
	}
	
	public void putBean(String key, AbstractBean bean) {
		this.beans.put(key, bean);
	} 
	
	public AbstractBean getBean( String key){
		return (AbstractBean)this.beans.get(key);
	}
	
//	public void setPrefferedSettings(SnapshotEncodingWrapper wrapper) {
//		getVideoPlayer().setPrefferedSettings(wrapper);
//	}	
//
//	public void loadComments(GUIListener list, QuestionInfoBean bean) {
//		netManager.getAAFResponseComments(list, bean);
//	}

	public boolean goWAP(String url) {
		boolean ret = false;
		try {

			if (getMidlet().platformRequest(url)) {
				ret = true;
				exit();
			}
			ret = true;
		} catch (ConnectionNotFoundException e) {
			e.printStackTrace();
			ret = false;
		}
		return ret;
	}

	public void addPauseListener(PauseListener pauseListener) {
		pauseList.addElement(pauseListener);
	}
	
	public void removePauseListener(PauseListener pauseListener) {
		pauseList.removeElement(pauseListener);
	}
	
	private void notifyPause(int state) {
		for (Enumeration pauseListEnum = pauseList.elements(); pauseListEnum.hasMoreElements(); ){
			PauseListener pauseListener = (PauseListener)pauseListEnum.nextElement();
			pauseListener.notify(state);
		}
	}
	
	public void notifyPause(){
		notifyPause(PauseListener.STATE_PAUSE);
	}
	
	public void notifyResume(){
		notifyPause(PauseListener.STATE_RESTORE);
	}
	
	public String gerFileRootPath(){
		return resManager.getFileRoot();
	}

//	public ResponseHolder getResponseHolder() {
//		return responseHolder;
//	}
//	
	public boolean isTouch() {
		return resManager.isTouch();
	}
	
	public void setTouch( boolean isTouch) {
		resManager.setTouch(isTouch);
	}

	public void initNetworkDispatch() {
		netManager.init();
	}
	
	public void uploadPhotoSUP(SUP_GUIListener listener, ItemLight2 item) {
		byte[] image = Engine.getEngine().getImageToSend(item.getPath() + item.getTitle());
//		System.out.println("PHOTO UPLOAD=" + item.getPath() + item.getTitle());
		
		netManager.uploadPhoto(listener, image, resManager.getImageCreationDate());
		resManager.setImageCreationDate(0);
	}

	public boolean isInitNetworkTimer() {
		return initNetworkTimer;
	}

}