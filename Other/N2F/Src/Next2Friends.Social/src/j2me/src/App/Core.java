package App;

//#ifdef NOKIA
//# import com.nokia.mid.ui.*;
//#endif

//#ifdef BLACKBERRY
//# import net.rim.device.api.system.Application;
//# import net.rim.device.api.system.KeyListener;
//# import net.rim.device.api.system.TrackwheelListener;
//# import net.rim.device.api.ui.Keypad;
//#endif

import gui.GUIScreenManager;
import javax.microedition.lcdui.*;
import javax.microedition.lcdui.game.*;
import tag.*;

//#ifdef NOKIA
//# public class Core extends FullCanvas implements Runnable
//#elifdef BLACKBERRY
//# public class Core extends Canvas implements Runnable, TrackwheelListener, KeyListener
//#else
public class Core extends Canvas implements Runnable
//#endif
{
    public App app;
    Thread thread;
    static boolean isRunning;
    static boolean isInited;
    public static boolean isKeyPressed;
    public static boolean isNetwork;
    public static int keyCode;
    public static int keyAction;
    public static int SCREEN_HEIGHT;
    public static int SCREEN_WIDTH;
    public static Graphics staticG;
    public static Storage storage;
    public static Font smallFont;
    public static Font bigFont;
    public static GUIScreenManager screenManager;
    public static BackgroundThread backNet;
    static CommonScreen mainMenu;
    static CommonScreen goScreen;
    static CommonScreen questionScreen;
    static CommonScreen quOptionsScreen;
    static CommonScreen loginScreen;
    static CommonScreen settingsScreen;
    static CommonScreen captureScreen;
    static CommonScreen attachScreen;
    static CommonScreen attachFromFile;
    static CommonScreen drafts;
    static CommonScreen outbox;
    static CommonScreen remind;
    static CommonScreen dashboard;
    static CommonScreen view;
    static CommonScreen tag;
    static CommonScreen inbox;
    static CommonScreen response;
    static CommonScreen comment;
    static CommonScreen status;
    static CommonScreen upload;
    
    public Title titleScreen;
    public static String message;
    
    public static TagEncryptor encryptor;
    
    public static BTLocator btLocator;
    public static BTServer  btServer;
    
//#ifdef BLACKBERRY
//#     public static boolean isAlt;
//#endif

    public Core()
    {
//#ifdef BLACKBERRY
//#     Application.getApplication().addTrackwheelListener(this);
//#endif
    
        setFullScreenMode(true);
	isRunning = true;
	isKeyPressed = false;
	isNetwork = false;
	keyAction = Integer.MAX_VALUE;

//#ifdef MOTOROLA
    //#ifdef V9
//# 	SCREEN_HEIGHT = 300;
//#         SCREEN_WIDTH = 240;
    //#elifdef V3
//#         SCREEN_HEIGHT = 205;
//#         SCREEN_WIDTH = 176;
    //#endif
//#else
        SCREEN_HEIGHT = getHeight();
        SCREEN_WIDTH = getWidth();
//#endif
    }


     public void start()
     {
 	thread = new Thread(this);
 	thread.start();
 	thread.setPriority(Thread.MAX_PRIORITY);
     }
     
//#ifdef BLACKBERRY     
//#         public boolean keyChar(char key, int status, int time)
//#     {
//#         return false;
//#     }
//#     
//#     public boolean keyDown(int keycode, int time)
//#     {
//#        // System.out.println("pressed "+keycode);
//#         if(isAlt)
//#         {
//#             keycode--;
//#         }
//#         
//#         keyAction = keycode;
//#         
//#         return true;
//#     }
//#     
//#     public boolean keyUp(int keycode, int time)
//#     {
//#         return false;
//#     }
//#     
//#     public boolean keyRepeat(int keycode, int time)
//#     {
//#         return false;
//#     }
//#     
//#     public boolean keyStatus(int keycode, int time)
//#     {
//#         if(keycode == Const.KEY_ALT_ON)
//#         {
//#             isAlt = true;
//#         }
//#         else if(keycode == Const.KEY_ALT_OFF)
//#         {
//#             isAlt = false;
//#         }
//#         return true;
//#     }
//# 
//#     public boolean trackwheelRoll(int amount, int status, int time)
//#     {
//#         return false;
//#     }
//# 
//#     public boolean trackwheelClick(int status, int time)
//#     {
//#         keyAction = Const.KEY_OK;
//#         return true;
//#     }
//# 
//#     public boolean trackwheelUnclick(int status, int time)
//#     {
//#         return true;
//#     }
//#endif

    void init()
    {
	smallFont = Font.getFont(Font.FACE_MONOSPACE, Font.STYLE_BOLD, Font.SIZE_SMALL);
	bigFont = Font.getFont(Font.FACE_SYSTEM, Font.STYLE_BOLD, Font.SIZE_SMALL);
	storage = new Storage();

	screenManager = new GUIScreenManager();

	titleScreen = new Title();
	screenManager.setScreen(titleScreen);

	mainMenu = new ScreenMainmenu();
	goScreen = new ScreenGo();
	loginScreen = new ScreenLogin();
	questionScreen = new ScreenQuestion();
	quOptionsScreen = new ScreenQuOptions();
	settingsScreen = new ScreenSettings();
	captureScreen = new ScreenCapture();
	attachScreen = new ScreenAttachImage();
	attachFromFile = new ScreenAttachFromFile();
	drafts = new ScreenDrafts();
	outbox = new ScreenOutbox();
	remind = new ScreenRemind();
	dashboard = new ScreenDashboard();
	view = new ScreenView();
	tag = new ScreenTag();
	inbox = new ScreenInbox();
	response = new ScreenResponse();
	comment = new ScreenWriteComment();
        status = new ScreenStatus();
        upload = new ScreenUpload();
        
	btServer = new BTServer();
	btLocator = new BTLocator();
        
        isInited = true;
    }

    public static void forceRepaint()
    {
	App.instance.core.repaint();
    }

    public void run()
    {
	init();
	while(isRunning)
	{
	    if(message != null && keyAction != Const.ACTION_NULL)
	    {
		message = null;
		keyAction = Const.ACTION_NULL;
	    }
	    else
	    {
		screenManager.update();
	    }
	    keyAction = Integer.MAX_VALUE;
	    repaint();
	    try
	    {
		Thread.sleep(50);
	    }
	    catch(InterruptedException ex)
	    {
	    }

	}
	App.quitApp();
    }

    public void paint(Graphics g)
    {
        if(!isInited)
            return;
        
	staticG = g;
	screenManager.render();

	if(isNetwork)
	{
	    g.setClip(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT);
	    g.drawImage(CommonScreen.blackImage, 0, 0, 0);
	    g.setColor(0xffffff);
	    g.setFont(bigFont);
	    g.drawString("Please wait", Core.SCREEN_WIDTH / 2, Core.SCREEN_HEIGHT / 2, Graphics.HCENTER | Graphics.BASELINE);
	}

	if(message != null)
	{
	    g.setClip(0, 0, SCREEN_WIDTH, SCREEN_HEIGHT);
	    g.drawImage(CommonScreen.blackImage, 0, 0, 0);
	    g.setColor(0xffffff);
	    g.setFont(bigFont);
	    g.drawString(message, Core.SCREEN_WIDTH / 2, Core.SCREEN_HEIGHT / 2, Graphics.HCENTER | Graphics.BASELINE);
	}
    }

    protected void keyPressed(int keyCode)
    {
//#ifdef BLACKBERRY
//#         if(keyCode == Const.KEY_UP || 
//#         keyCode == Const.KEY_DOWN ||
//#         keyCode == Const.KEY_LEFT ||
//#         keyCode == Const.KEY_RIGHT)
//#else
	isKeyPressed = true;
	this.keyCode = keyCode;
	if(keyAction != keyCode)
//#endif
	{
	    keyAction = keyCode;
	}
    //System.out.println("pressed "+keyCode);
    }

    public static void clearAction()
    {
	keyAction = Integer.MAX_VALUE;
    }

    protected void keyReleased(int keyCode)
    {
	isKeyPressed = false;
    //System.out.println("unpressed "+keyCode);
    }
}
