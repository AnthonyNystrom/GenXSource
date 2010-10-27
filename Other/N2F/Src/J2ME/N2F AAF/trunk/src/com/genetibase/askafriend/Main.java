package com.genetibase.askafriend;

import javax.microedition.midlet.MIDlet;

import com.genetibase.askafriend.core.Engine;
import com.genetibase.askafriend.ui.UIManager;

public class Main extends MIDlet {
	private boolean isPause = false;
	private static Main instance = null;
	
	public Main(){
		super();
		instance = this;
	}
	
	public static Main getMIDlet() {
		return instance;
	}
	
    /**  
     * Signals the MIDlet to start and enter the Active state.
     */
    protected void startApp()
    {
		if (!isPause) {
			UIManager.getInstance().show(UIManager.SCREEN_SPLASH);
		} else {
			Engine.getEngine().notifyResume();
			Engine.getEngine().setMIDlet(this);
			UIManager.getInstance().showCurrent();
		}
		isPause = false;			
    }

    /**
     * Signals the MIDlet to terminate and enter the Destroyed state.
     */
    public void destroyApp(boolean unconditional) {
		Engine.getEngine().destroy();
    }

    /**
     * Signals the MIDlet to stop and enter the Paused state.
     */
    protected void pauseApp() {
    	isPause = true;
		Engine.getEngine().notifyPause();
    }            

}
