package com.genetibase.askafriend.common.utils;

import com.genetibase.askafriend.ui.GUIListener;

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
	}

	public void setListener(GUIListener lis) {
		this.listener = lis;
	}

	public GUIListener getListener() {
		return listener;
	}
}
