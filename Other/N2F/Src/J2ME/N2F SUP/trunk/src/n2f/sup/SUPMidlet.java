package n2f.sup;

import n2f.sup.core.Engine;
import n2f.sup.ui.UIManager;


public class SUPMidlet extends AbstractMidlet {
	private boolean isPause = false;
	private static SUPMidlet instance = null;
	
	public SUPMidlet(){
		super();
		instance = this;
	}
	
	public static SUPMidlet getMIDlet() {
		return instance;
	}
	
    /**  
     * Signals the MIDlet to start and enter the Active state.
     */
    protected void startApp() {
		if (!isPause) {
			UIManager.getInstance().show(UIManager.SCREEN_SUP_SPLASH);
		} else {
			Engine.getEngine().notifyResume();
			Engine.getEngine().setMIDlet(this, false);
			UIManager.getInstance().showCurrent();
		}
		isPause = false;			
    }

}
