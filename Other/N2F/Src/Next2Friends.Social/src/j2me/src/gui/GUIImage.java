package gui;

import javax.microedition.lcdui.*;

public class GUIImage extends GUIControl implements App.Clonable
{

    public Image image;
    public App.Sprite sprite;
    public int type;
    public int frame;

    public GUIImage()
    {
    }

    public GUIImage(Image image)
    {
	dx = image.getWidth();
	dy = image.getHeight();
	this.image = image;
	type = App.Const.GUIIMAGE_IMAGE;
    }

    public GUIImage(int type, String fileName)
    {
	this.type = type;
	frame = 0;
	if(App.Const.GUIIMAGE_IMAGE == type)
	{
	    try
	    {
		image = Image.createImage(fileName);
		dx = image.getWidth();
		dy = image.getHeight();
	    }
	    catch(Exception ex)
	    {
		ex.printStackTrace();
	    }
	}
	else
	    if(App.Const.GUIIMAGE_SPRITE == type)
	    {
		sprite = new App.Sprite(fileName, false);
		dx = sprite.spr_FrameWidth;
		dy = sprite.spr_FrameHeight;
	    }
    }

    public void render(int offsetX, int offsetY)
    {
	super.render(offsetX, offsetY);

	if(type == App.Const.GUIIMAGE_IMAGE)
	{
	    if(image != null)
	    {
		App.Core.staticG.drawImage(image, x + offsetX, y + offsetY, 0);
	    }
	}
	else
	    if(type == App.Const.GUIIMAGE_SPRITE)
	    {
		if(sprite != null)
		{
		    int numFrame = frame;
		    if(isChecked)
		    {
			numFrame++;
		    }
		    sprite.setFrame(numFrame);

		    sprite.drawClip(x + offsetX, y + offsetY, x + offsetX, y + offsetY, dx, dy);
		}
	    }
    }

    public Object copy(Object from)
    {
	GUIImage copy = (GUIImage) from;

	//image and sprite are not copied
	copy.image = image;
	copy.sprite = sprite;
	copy.type = type;
	copy.frame = frame;

	return super.copy(copy);
    }

    public Object clone()
    {
	GUIImage copy = new GUIImage();

	return copy(copy);
    }
}
