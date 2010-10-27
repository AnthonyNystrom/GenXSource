package App;

import gui.GUIScreen;
import gui.GUIImage;
import javax.microedition.lcdui.*;
import tag.*;

public class Title extends GUIScreen
{
    GUIImage image;
    long time;
    
    
    Title()
    {
	
	image = new GUIImage(Const.GUIIMAGE_IMAGE, "/title.png");
	time = System.currentTimeMillis();
    }
    
    public void update()
    {	
	if(Core.keyAction != Const.ACTION_NULL || System.currentTimeMillis() - time > Const.TITLE_SHOWTIME)
	{
	    image = null;
	    Core.screenManager.setScreen(Core.loginScreen);
	}
    }
    
    public void render()
    {
	if(image != null)
	{
	    image.render(0, 0);
	}
    }
}
