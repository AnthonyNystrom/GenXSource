package n2f.tag.webservice.utils;

import n2f.tag.ui.GUIListener;


public abstract class RunnableTaskAdapter implements RunnableTask {
	
	public static final int BLUETOOTH_DEVICE_SEARCH = 11;
	protected GUIListener listener = null;
	
	public void execute() throws Exception {
		if (listener != null)
			listener.fireAction(GUIListener.ACTION_BUSY);
		logic();
		if (listener != null)
			listener.fireAction(GUIListener.ACTION_HIDE);
	}
	
	abstract protected void logic() throws Exception;

	public void interrupt() {
	}

	public void setListener(GUIListener lis) {
		this.listener = lis;
	}

	public GUIListener getListener() {
		return listener;
	}
}
