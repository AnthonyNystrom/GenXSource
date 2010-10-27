package n2f.sup;

import javax.microedition.midlet.MIDlet;

import n2f.sup.core.Engine;


public abstract class AbstractMidlet extends MIDlet {
	protected boolean isPause = false;
	
    /**  
     * Signals the MIDlet to start and enter the Active state.
     */
    protected abstract void startApp();

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
