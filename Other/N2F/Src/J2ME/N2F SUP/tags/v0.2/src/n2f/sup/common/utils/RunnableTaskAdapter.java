package n2f.sup.common.utils;

import n2f.sup.ui.GUIListener;

public abstract class RunnableTaskAdapter implements RunnableTask {
	
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
		// TODO Auto-generated method stub
	}

	public void setListener(GUIListener lis) {
		this.listener = lis;
	}

	public GUIListener getListener() {
		return listener;
	}
}
