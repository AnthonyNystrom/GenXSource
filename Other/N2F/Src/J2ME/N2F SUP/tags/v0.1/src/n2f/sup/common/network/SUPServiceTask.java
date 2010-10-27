package n2f.sup.common.network;

import n2f.sup.common.utils.Base64;
import n2f.sup.common.utils.StringTokenizer;
import n2f.sup.core.CommonKeys;
import n2f.sup.core.Engine;
import n2f.sup.ui.SUP_GUIListener;
import n2f.sup.utils.Debug;
import n2f.sup.utils.MemoryDispatcher;
import n2f.sup.webservice.DeviceUploadPhotoResponse;
import n2f.sup.webservice.PhotoOrganiseSoapStub;

//import com.genetibase.askafriend.common.network.stub.DeviceUploadPhotoResponse;
//import com.genetibase.askafriend.common.network.stub.PhotoOrganiseSoap_Stub;

public class SUPServiceTask extends NetworkServiceTaskAdapter {
	private byte[] image;
	private long creationDate;
	
	public SUPServiceTask(int operationType, SUP_GUIListener listener, NetworkServiceLogics logic, byte[] image, long createDate) {
		super(operationType, listener, logic);
		this.image = image;
		this.creationDate = createDate!=0? createDate: System.currentTimeMillis();
	}

	protected void logic() throws Exception {
//		PhotoOrganiseSoap_Stub soap = new PhotoOrganiseSoap_Stub();
		PhotoOrganiseSoapStub soap = new PhotoOrganiseSoapStub();		
		String webMemberID = Engine.getEngine().getSystemProperty(CommonKeys.WEBMEMBERID);
		String webPassword = Engine.getEngine().getSystemProperty(CommonKeys.PASSWORD);
		System.out.println("-=START=-");		
		Object resultObj = null;
		switch (operationType) {
		case TYPE_SUP_UPLOAD_PHOTO:
			//TODO?: getImages array and encode them
			String s = Base64.encode(image);
			image = null;
			String currTime = StringTokenizer.getServiceDate(creationDate);
			Debug.println("send image creation date to serv:"+StringTokenizer.parseDate(currTime));
//			DeviceUploadPhotoResponse res = null;
			DeviceUploadPhotoResponse res = null;
			try {
				System.out.println("-=TRY=-");				
				res = soap.deviceUploadPhoto(webMemberID, webPassword, s, currTime);
//				res = new DeviceUploadPhotoResponse();
				System.out.println("-=DONE=-");				
			} catch (OutOfMemoryError e) {
				resultObj = new Integer(SUP_GUIListener.ACTION_FAILED_MEMORY_ISSUE);
				MemoryDispatcher.gc();
				System.out.println("OUTOFMEMORY");				
				e.printStackTrace();
			} finally {
				System.out.println("FINALLY");
				if (res != null && resultObj == null) {
//					resultObj = new Integer(indexOrder);
					resultObj = new Integer(SUP_GUIListener.ACTION_OK);
				} else {
					resultObj = new Integer(SUP_GUIListener.ACTION_FAILED_GENERAL);				
				}
			}
			break;
		}
		if (handler != null)
			handler.analyzeServiceResponse(new ServiceResponse(operationType, resultObj), this);
		System.out.println("-=NOTIFY=-");		
		listener.fireAction(((Integer)resultObj).intValue());
	}

}
