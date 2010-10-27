package n2f.tag.webservice;

import n2f.tag.ui.GUIListener;
import n2f.tag.webservice.utils.RunnableTaskAdapter;



public abstract class NetworkServiceTaskAdapter extends RunnableTaskAdapter {
	

	public static final int TYPE_GET_CREDENTIALS = 39;
	public static final int TYPE_GET_BLOCKLIST = 38;
	public static final int TYPE_UPLOAD_TAGLIST = 37;
	public static final int TYPE_GET_ENCRYPTION_KEY = 36;
	public static final int TYPE_GET_TAG_ID = 35;
	
	protected WebServiceInteractor handler;
	protected int operationType;
	
	protected NetworkServiceTaskAdapter(int operationType, GUIListener listener, WebServiceInteractor logic) {
		this.operationType = operationType;
		this.listener = listener;
		this.handler = logic;
	}
	

	public int getType() {
		return operationType;
	}

}
