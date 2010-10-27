package App;

import javax.microedition.midlet.*;
import javax.microedition.lcdui.*;

public class App extends MIDlet
{
    public static App instance = null;
    public Core core = null;

    public void startApp()
    {
        if(instance == null)
        {
            instance = this;
            core = new Core();
            Display.getDisplay(this).setCurrent(core);
            core.app = this;
            core.start();
        }

    }

    public void pauseApp()
    {
    }

    public void destroyApp(boolean unconditional)
    {
    }
    
    public static void quitApp()
    {
	instance.destroyApp(true);
	instance.notifyDestroyed();
	instance = null;
    }

}
