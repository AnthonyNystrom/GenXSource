package App;

import gui.GUIHeader;
import gui.GUISkin;
import gui.GUIImage;
import javax.microedition.lcdui.*;

public class CommonHeader extends GUIHeader
{

    public GUIImage header;
    static public GUIImage net = new GUIImage(Const.GUIIMAGE_SPRITE, "/net");
    private int	    netFrames;

    public CommonHeader()
    {
	header = new GUIImage(Const.GUIIMAGE_IMAGE, "/logo.png");
	header.x = Core.SCREEN_WIDTH / 2 - header.image.getWidth() / 2;
	header.dx = Core.SCREEN_WIDTH;
	header.dy = Core.SCREEN_HEIGHT;

	skin = new GUISkin("/header", true, Const.SKIN_TYPE_1H);
    }

    public void drawPostSkin()
    {
	header.render(0, 0);

	if(netFrames++ > Const.GUITEXT_CURSOR_FLASH_FRAMES)
	{
	    netFrames = 0;
	}
	
	if(netFrames > Const.GUITEXT_CURSOR_FLASH_FRAMES/2 && Core.backNet != null && Core.backNet.getSend)
	{
	    Core.staticG.setClip(0, 0, Core.SCREEN_WIDTH, Core.SCREEN_HEIGHT);
	    net.render(0, 0);
	}
    }
}
