package n2f.sup.common.network;

import n2f.sup.common.utils.RunnableTaskAdapter;
import n2f.sup.ui.GUIListener;


public abstract class NetworkServiceTaskAdapter extends RunnableTaskAdapter {
	
	public static final int TYPE_GET_CREDENTIALS = 39;
	public static final int TYPE_SUP_UPLOAD_PHOTO = 40;	
	
	protected NetworkServiceLogics handler;
	protected int operationType;
	
	protected NetworkServiceTaskAdapter(int operationType, GUIListener listener, NetworkServiceLogics logic) {
		this.operationType = operationType;
		this.listener = listener;
		this.handler = logic;
	}
	

	public int getType() {
		return operationType;
	}

}
