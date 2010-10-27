package gui;

//#ifdef BLACKBERRY
//# import javax.microedition.media.*;
//# import javax.microedition.media.control.*;
//# import javax.microedition.lcdui.*;
//# import net.rim.blackberry.api.invoke.*;
//# 
//# public class GUICamera extends GUIControl
//# {
//# 
//#     private Player p;
//#     private VideoControl vc;
//# 
//#     public GUICamera()
//#     {
//#         super();
//#     }
//# 
//#     public void update()
//#     {
//#     }
//# 
//#     public void render(int offsetX, int offsetY)
//#     {
//#     }
//# 
//#     public Object clone()
//#     {
//#         return null;
//#     }
//# 
//#     public void init()
//#     {
//#         Invoke.invokeApplication(Invoke.APP_TYPE_CAMERA, new CameraArguments());
//# 
//# //	try
//# //	{
//# //	    p = Manager.createPlayer("capture://video");
//# //	    p.realize();
//# //	    vc = (VideoControl) p.getControl("VideoControl");
//# //
//# //	    if(vc != null)
//# //	    {
//# //		vc.initDisplayMode(vc.USE_DIRECT_VIDEO, App.App.instance.core);
//# //		vc.setDisplayLocation(x, y);
//# //		vc.setDisplaySize(dx, dy);
//# //		vc.setVisible(true);
//# //	    }
//# //	    p.start();
//# //	}
//# //	catch(Exception ex)
//# //	{
//# //	    String msg = ex.getMessage();
//# //	}
//#     }
//# 
//#     public void release()
//#     {
//#         try
//#         {
//#             if (vc != null)
//#             {
//#                 vc.setVisible(false);
//#             }
//#             if (p != null)
//#             {
//#                 p.stop();
//#                 p.close();
//#             }
//#         } catch (MediaException ex)
//# 
//#         {
//#             ex.printStackTrace();
//#         }
//#         p = null;
//#         vc = null;
//#     }
//# 
//#     public byte[] getSnapShot()
//#     {
//#         byte[] image = null;
//#         try
//#         {
//#             image = vc.getSnapshot("encoding=jpeg");
//#         } catch (MediaException ex)
//#         {
//#             ex.printStackTrace();
//#         } finally
//# 
//# 
//#         {
//#             return image;
//#         }
//#     }
//#     
//#          public static Image scale(Image src, int height)
//#      {
//#  	int srcw = src.getWidth();
//#  	int srch = src.getHeight();
//#  	int width = height * srcw / srch;
//#  
//#  	Image thumb = Image.createImage(width, height);
//#  	Graphics g = thumb.getGraphics();
//#  
//#  	for(int y = 0; y < height; y++)
//#  	{
//#  	    for(int x = 0; x < width; x++)
//#  	    {
//#  		g.setClip(x, y, 1, 1);
//#  		int dx = x * srcw / width;
//#  		int dy = y * srch / height;
//#  		g.drawImage(src, x - dx, y - dy, 0);
//#  	    }
//#  	}
//#  
//#  	return thumb;
//#      }
//# }
//# 
//#else

import javax.microedition.media.*;
import javax.microedition.media.control.*;
import javax.microedition.lcdui.*;

public class GUICamera extends GUIControl
{

    private Player p;
    private VideoControl vc;

    public GUICamera()
    {
	super();
    }

    public void update()
    {
    }

    public void render(int offsetX, int offsetY)
    {
    }

    public Object clone()
    {
	return null;
    }

    public void init()
    {
	try
	{
	    p = Manager.createPlayer("capture://video");
	    p.realize();
	    vc = (VideoControl) p.getControl("VideoControl");

	    if(vc != null)
	    {
		vc.initDisplayMode(vc.USE_DIRECT_VIDEO, App.App.instance.core);
		vc.setDisplayLocation(x, y);
		vc.setDisplaySize(dx, dy);
		vc.setVisible(true);
	    }
	    p.start();
	}
	catch(Exception ex)
	{
	    ex.printStackTrace();
	}
    }

    public void release()
    {
	try
	{
	    if(vc != null)
	    {
		vc.setVisible(false);
	    }
	    if(p != null)
	    {
		p.stop();
		p.close();
	    }
	}
	catch(MediaException ex)
	{
	    ex.printStackTrace();
	}
	p = null;
	vc = null;
    }

    public byte[] getSnapShot()
    {
	byte[] image = null;
	try
	{
	    image = vc.getSnapshot("encoding=jpeg");
	}
	catch(MediaException ex)
	{
	    ex.printStackTrace();
	}
	finally
	{
	    return image;
	}
    }

    public static Image scale(Image src, int height)
    {
	int srcw = src.getWidth();
	int srch = src.getHeight();
	int width = height * srcw / srch;

	Image thumb = Image.createImage(width, height);
	Graphics g = thumb.getGraphics();

	for(int y = 0; y < height; y++)
	{
	    for(int x = 0; x < width; x++)
	    {
		g.setClip(x, y, 1, 1);
		int dx = x * srcw / width;
		int dy = y * srch / height;
		g.drawImage(src, x - dx, y - dy, 0);
	    }
	}

	return thumb;
//	int scanline = src.getWidth();
//	int srcw = src.getWidth();
//	int srch = src.getHeight();
//	int height = width*srch/srcw;
//	int buf[] = new int[srcw * srch];
//	src.getRGB(buf, 0, scanline, 0, 0, srcw, srch);
//	int buf2[] = new int[width*height];
//	for (int y=0; y < height; y++)
//	{
//	    int c1 = y*width;
//	    int c2 = (y*srch/height)*scanline;
//	    for (int x=0; x<width; x++)
//	    {
//		buf2[c1 + x] = buf[c2 + x*srcw/width];
//	    }
//	}		
//	Image img = Image.createRGBImage(buf2, width, height, true);
//	return img;
    }
}
//#endif