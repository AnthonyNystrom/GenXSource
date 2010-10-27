package n2f.sup.ui;

import javax.microedition.lcdui.Display;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.Graphics;
import javax.microedition.lcdui.Image;

import n2f.sup.SUPMidlet;
import n2f.sup.common.Resoursable;
import n2f.sup.core.Engine;


class SUPSplash extends AbstractWindow implements Runnable {
	
	protected Display getDisplay() {
		return Display.getDisplay(SUPMidlet.getMIDlet());
	}
	
	public void show() {
		CustomCanvas.getCanvas().setCanvasable(new CanvasableAdapter(){
			public void paint(Graphics g) {
			    g.setColor(0xffffff);
			    width = CustomCanvas.getCanvas().getWidth();
			    height = CustomCanvas.getCanvas().getHeight();			    
			    g.fillRect(0,0,width,height);
			    if (image != null){
			        g.drawImage(image,0,(height-image.getHeight())/2,0);
			    }
			}

			public boolean getFullScreenMode() {
				return true;
			}
			
		});
        getDisplay().setCurrent(CustomCanvas.getCanvas());
		getDisplay().callSerially(this);
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
	SUPSplash(String id, Resoursable resoursable) {
		super(id, resoursable);
		this.image = Engine.getEngine().getImage("/res/commercelogo.png", false);
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
						Engine.getEngine().setMIDlet(SUPMidlet.getMIDlet(), false);						
						Engine.getEngine().openInit(null);
						Engine.getEngine().setTouch(CustomCanvas.getCanvas().isPointerDevice());
						UIManager.getInstance().setDefaultID(UIManager.SCREEN_SUP_START);
						try {
							Thread.sleep(2000);
						} catch(InterruptedException ie) {
							ie.printStackTrace();
						}
						if (!Engine.getEngine().isAuthorized()) {
							UIManager.getInstance().show(UIManager.SCREEN_SUP_SETTINGS);
						} else {
							UIManager.getInstance().show(UIManager.SCREEN_SUP_START);
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
		CustomCanvas.getCanvas().setFullScreenMode(false);
		CustomCanvas.getCanvas().reset();
	}

	protected Displayable getScreen() {
		return null;
	}
}