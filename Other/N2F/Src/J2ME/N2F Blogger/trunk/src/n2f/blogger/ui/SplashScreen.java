
package n2f.blogger.ui;

import genetibase.java.microedition.componentmodel.Resources;

import java.io.InputStream;

import javax.microedition.lcdui.Canvas;
import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.Display;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.Graphics;
import javax.microedition.lcdui.Image;

import n2f.blogger.App;
import n2f.blogger.utils.Utils;

class SplashScreen
	extends AbstractWindow
	implements Runnable
{
    protected Display getDisplay()
    {
	return Display.getDisplay(App.getCurrentApp());
    }

    public void show()
    {
	Canvas canvas = new Canvas()
	{
	    public void paint(Graphics g)
	    {
		g.setColor(0xffffff);
		width = getWidth();
		height = getHeight();
		g.fillRect(0, 0, width, height);
		if (image != null)
		    g.drawImage(image, 0, (height - image.getHeight()) / 2, 0);
	    }
	};
	canvas.setFullScreenMode(true);
	getDisplay().setCurrent(canvas);
	getDisplay().callSerially(this);
	canvas.setCommandListener(this);
    }

    protected void refresh()
    {
    }

    public int getFormWidth()
    {
	return width;
    }

    public int getFormHeight()
    {
	return height;
    }

    private Image image = null;
    private int width, height;

    /**
     * The Constructor
     */
    SplashScreen(String id, Resources resoursable)
    {
	super(id, resoursable);
	InputStream is = getClass().getResourceAsStream("/res/commercelogo.png");
	if (is != null)
	{
	    this.image = Utils.createImage(is);
	    is = null;
	}
    }

    public void commandAction(Command c, Displayable d)
    {
	super.commandAction(c, d);
    }

    public void run()
    {
	new Thread(new Runnable()
	   {
	       public void run()
	       {
		   try
		   {
		       try
		       {
			   Thread.sleep(2000);
		       }
		       catch (InterruptedException ie)
		       {
			   ie.printStackTrace();
		       }

		       if (App.getCurrentApp().getSettings().hasCredentials())
			   UIManager.getInstance().showDefault();
		       else
			   UIManager.getInstance().show(UIManager.SCREEN_SETTINGS,
							false);
		       free();

		   }
		   catch (Exception e)
		   {
		       e.printStackTrace();
		   }
	       }

	   }).start();
    }

    protected Displayable getScreen()
    {
	return null;
    }

    private void free()
    {
	image = null;
    }
}
