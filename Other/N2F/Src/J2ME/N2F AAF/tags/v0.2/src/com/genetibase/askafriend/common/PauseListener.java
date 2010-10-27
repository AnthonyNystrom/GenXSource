package com.genetibase.askafriend.common;

public interface PauseListener {
	int STATE_PAUSE = -10000;
	int STATE_RESTORE = 10000;
	
	void notify(int state);
}
