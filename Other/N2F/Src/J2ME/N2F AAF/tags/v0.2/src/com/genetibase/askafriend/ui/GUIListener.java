package com.genetibase.askafriend.ui;

public interface GUIListener 
{
	int ACTION_BUSY = 0;
	int ACTION_HIDE = 1;
	
	void fireAction(int actionType);
}
