package n2f.tag.webservice;

import genetibase.java.collections.IStringEnumeration;
import genetibase.java.collections.StringHashtable;

import java.util.Vector;

import n2f.tag.App;
import n2f.tag.core.AbstractErrorManager;
import n2f.tag.core.Deallocatable;
import n2f.tag.core.ErrorListener;
import n2f.tag.ui.GUIListener;
import n2f.tag.ui.SettingsBean;
import n2f.tag.ui.UIManager;
import n2f.tag.webservice.stub.ArrayOfBTTagConfirmation;
import n2f.tag.webservice.stub.ArrayOfDeviceBlockItem;
import n2f.tag.webservice.stub.BTTagConfirmation;
import n2f.tag.webservice.stub.DeviceBlockItem;
import n2f.tag.webservice.utils.RunnableTask;
import n2f.tag.webservice.utils.WorkerThread;
import n2f.tag.wireless.TagKeeper;

public class WebServiceInteractor extends AbstractErrorManager implements Deallocatable{

	private WorkerThread wt;
	private BTTagConfirmation[] confirmationsList;
	private String encryptionKey;
	private String tagId;
	private StringHashtable blockTable;
	private UploadTimer timer;
	
	public final static long FIVEMIN = 300000;
	public final static long HOURLY = 3600000;
	public final static long DAILY = 24*3600000;
	
	
	public WebServiceInteractor() {
		wt = new WorkerThread();
		App.getCurrentApp().addToDeallocatableList(this);
	}
	
	public void init() {
		if (timer == null) { 	
			timer = new UploadTimer();
			timer.start();
		}
	}
	
	public void changeInstensity(long freq) {
		this.timer.changeIntensity(freq);
	}
	
	private class UploadTimer 
	{
		private boolean running = true;
		private WatcherThread thread = new WatcherThread();
		private volatile long lastExec; 
		private volatile long intensity = FIVEMIN;
		
		public void start() {
			this.running = true;
			(new Thread(thread)).start();
			this.lastExec = System.currentTimeMillis();
			
		}
		
		public void changeIntensity(long newIntensity) {
			if (running) {
				this.intensity = newIntensity;
				this.thread.wakeUp();	
			}
		}
		
		public void stop() {
			running = false;
			thread.wakeUp();
			thread = null;
		}
		
		class WatcherThread implements Runnable	{
			
			public void run()
			{
				while (running) {
					synchronized (this) {
						try {
							//wait if no data has been sent to sensor
							long currentTime = System.currentTimeMillis();
System.out.println("WAIT "+(intensity - (currentTime-lastExec)));
							wait(intensity - (currentTime-lastExec));
System.out.println("WOKE UP "+(intensity - (currentTime-lastExec)));
							//something has been sent to sensor - try to read the reply
							//there's data available from sensor
							currentTime = System.currentTimeMillis();
							if (running && (currentTime-lastExec > intensity))
							{
								if (TagKeeper.getInstance().hasRecords()) 
									uploadTagList(null);
								else 
System.out.println("there's nothing to UPLOAD");
								lastExec = currentTime;
							}
						} catch (Throwable e) {
							e.printStackTrace();
						} 
					}
				}
System.out.println("moved away from webinteractor loop");				
			}		
			
			public void wakeUp() {
				synchronized (this) {
					notifyAll();				
				}
			}
		}
	}
	
	public void addErrorListener(ErrorListener errorListener) {
		super.addErrorListener(errorListener);
		wt.addErrorListener(errorListener);
	}
	
	public void getWebMemberId(GUIListener lis) 
	{
		GetUserIdsTask task = new GetUserIdsTask(GetUserIdsTask.TYPE_GET_CREDENTIALS, lis, this);
		executeInWorkerThread(task);
	}
	
	public void getBlockList(GUIListener lis) {
		GetBlockListTask task = new GetBlockListTask(GetBlockListTask.TYPE_GET_BLOCKLIST, lis, this);
		executeInWorkerThread(task);
	} 
	
	public void getTagId(GUIListener lis) 
	{
		GetUserIdsTask task = new GetUserIdsTask(GetUserIdsTask.TYPE_GET_TAG_ID, lis, this);
		executeInWorkerThread(task);
	}
	
	public void getEncryptionTag(GUIListener lis) 
	{
		GetUserIdsTask task = new GetUserIdsTask(GetUserIdsTask.TYPE_GET_ENCRYPTION_KEY, lis, this);
		executeInWorkerThread(task);
	}

	public void uploadTagList(GUIListener lis) 
	{
		UploadTagTask task = new UploadTagTask(GetUserIdsTask.TYPE_UPLOAD_TAGLIST, lis, this);
		executeInWorkerThread(task);
	}

	
	public void  analyzeServiceResponse(Object resultObj, NetworkServiceTaskAdapter owner) {
		if (owner != null ) {
			switch (owner.getType()) {
			case NetworkServiceTaskAdapter.TYPE_GET_CREDENTIALS:
				if (resultObj != null) {
					SettingsBean bean = (SettingsBean) UIManager.getInstance().getBean(UIManager.SCREEN_SETTINGS);
					bean.setWebMemberId((String)resultObj);
				}
				break;
				
			case NetworkServiceTaskAdapter.TYPE_GET_BLOCKLIST:
				ArrayOfDeviceBlockItem a = (ArrayOfDeviceBlockItem) resultObj;
				if (a != null) {
					DeviceBlockItem[] blockList = a.getDeviceBlockItem();
					blockTable  = new StringHashtable();
					for (int i = 0, size = blockList.length; i < size; i++) {
						blockTable.put(blockList[i].getDeviceTagID(), blockList[i].getMACAddress());
					} 
System.out.println("blocklist:"+blockTable);
				}
				break;
				
			case NetworkServiceTaskAdapter.TYPE_UPLOAD_TAGLIST:
				ArrayOfBTTagConfirmation confirms = (ArrayOfBTTagConfirmation) resultObj;
				if (confirms != null) {
					this.confirmationsList = confirms.getBTTagConfirmation();
System.out.println("upload succeeded:");
					for (int i =0 ; i<confirmationsList.length; i++) {
System.out.println("i="+i + " webid"+confirmationsList[i].getWebMemberID()+" is confirmed"+confirmationsList[i].isConfirmedByServer());
					}
				}
				break;

			case NetworkServiceTaskAdapter.TYPE_GET_TAG_ID:
				this.tagId = (String) resultObj;
System.out.println("retrieved tagKey:"+tagId);				
				break;
				
			case NetworkServiceTaskAdapter.TYPE_GET_ENCRYPTION_KEY:
				this.encryptionKey = (String) resultObj;
System.out.println("retrieved encryptioinKey:"+encryptionKey);				
				break;
				
			default:
				break;
			}
		}
	}
	
	private void executeInWorkerThread(RunnableTask runnableTask) {
		if (wt != null) {
			wt.put(runnableTask);
		}
	}
	
	public StringHashtable getBlockTable() 
	{
		return blockTable;
	}
	
	public Vector getBlockList() {
		Vector v = new Vector();
		for (IStringEnumeration en = blockTable.keys(); en.hasMoreElements();) {
			v.addElement(en.nextElement());
		}
		return v;
	}

	public BTTagConfirmation[] getConfirmationsList() {
		return confirmationsList;
	}

	public String getEncryptionKey() {
		return encryptionKey;
	}

	public String getTagId() {
		return tagId;
	}
	
	public void free() {
		if (wt != null)
			wt.free();
		wt = null;
		if (timer != null)
			timer.stop();
		timer = null;
	}
}
