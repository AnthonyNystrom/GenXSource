package n2f.tag.ui;

import genetibase.java.microedition.componentmodel.Resources;

import java.io.InputStream;

import javax.microedition.io.PushRegistry;
import javax.microedition.lcdui.Canvas;
import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.Display;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.Graphics;
import javax.microedition.lcdui.Image;

import n2f.tag.App;
import n2f.tag.debug.Debug;
import n2f.tag.utils.Utils;
import n2f.tag.wireless.DeviceDiscoverer;
import n2f.tag.wireless.IDeviceDiscovererListener;

class SplashScreen extends AbstractWindow implements Runnable, IDeviceDiscovererListener {

	
	
	protected Display getDisplay() {
		return Display.getDisplay(App.getCurrentApp());
	}
	
	public void show() {
		Canvas canvas = new Canvas(){
			public void paint(Graphics g) {
			    g.setColor(0xffffff);
			    width = getWidth();
			    height = getHeight();			    
			    g.fillRect(0,0,width,height);
			    if (image != null){
			        g.drawImage(image,0,(height-image.getHeight())/2,0);
			    }
			}
		};
		canvas.setFullScreenMode(true);
        getDisplay().setCurrent(canvas);
		getDisplay().callSerially(this);
//		console = new Command(CONSOLE, Command.SCREEN, 4);
//		canvas.addCommand(console);
		canvas.setCommandListener(this);
	}
	
	protected void refresh() {
	}
	public int getFormWidth() {
		return width;
	}
	public int getFormHeight() {
		return height;
	}

	private Image image = null; //the logo image displayed
	private int width, height; //the sccren's width and height 
	/**
	 * The Constructor
	 */
	SplashScreen(String id, Resources resoursable) {
		super(id, resoursable);
		InputStream is = getClass().getResourceAsStream("/res/commercelogo.png");
		if (is != null) {
			this.image = Utils.createImage(is);
//			Utils.close(is);
			is = null;
		}
		
		
	}
	
		
	/* 
	 * the run method
	 * (non-Javadoc)
	 * @see java.lang.Runnable#run()
	 */
	  public void run() {
		  new Thread(new Runnable() {
				public void run() {
					try {
						Debug.println("list connections!!");
						String[] inboundConnections = PushRegistry.listConnections(true);
						Debug.println("list connections:"+inboundConnections.length);
						if (inboundConnections != null && inboundConnections.length > 0) {
							// midlet has been started by push
							DeviceDiscoverer discoverer = App.getCurrentApp().getDeviceDiscoverer(SplashScreen.this);
							discoverer.processPushConnection(inboundConnections);
							
						} else {
							try {
								Thread.sleep(2000);
							} catch (InterruptedException ie) {
								ie.printStackTrace();
							}
							if (App.getCurrentApp().getSettings().hasCredentials())
							{
							    UIManager.getInstance().showDefault();
							} else
							    UIManager.getInstance().show(UIManager.SCREEN_SETTINGS, false);

						}
						free();
						
					} catch (Exception e) {
						e.printStackTrace();
					}
				}
			}).start();
	  }

	private void free() {
		image = null;
	}

	protected Displayable getScreen() {
		return null;
	}

	public void discovererCannotStartDeviceSearch() {
		// TODO Auto-generated method stub
		
	}

	public void discovererCannotStartTaggingService() {
		// TODO Auto-generated method stub
		
	}
	
	public void commandAction(Command c, Displayable d) {
		super.commandAction(c, d);
	}

}