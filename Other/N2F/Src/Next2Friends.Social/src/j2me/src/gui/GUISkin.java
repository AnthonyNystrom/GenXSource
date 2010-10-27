package gui;


public class GUISkin implements App.Clonable
{
    public App.Sprite	sprite;
    public int		skinType; 
    
    public GUISkin(String fileName, boolean isIndexed, int skinType)
    {
	if(fileName != null)
	{
	    sprite = new App.Sprite(fileName, isIndexed);
	}
	this.skinType = skinType;
    }
    
    public Object copy(Object from)
    {
	GUISkin copy = (GUISkin)from;
	
	//sprite is not copied!!!
	copy.sprite = sprite;
	return copy;
    }
    
    public Object clone()
    {
	GUISkin newSkin = new GUISkin(null, false, skinType);
	
	return copy(newSkin);
    }
    
    public void draw(int x, int y, int dx, int dy)
    {
	draw(x, y, dx, dy, 0);
    }
    
    public void draw(int x, int y, int dx, int dy, int frameOffset)
    {
	switch(skinType)
	{
	    case App.Const.SKIN_TYPE_1H:
	    {
		sprite.setFrame(frameOffset);
		int offset = x;
		int times = dx/sprite.spr_FrameWidth+1;
		for(int i = 0; i < times; ++i)
		{
		    sprite.drawClip(offset, y, x, y, dx, dy);
		    offset += sprite.spr_FrameWidth;
		}
	    }
	    break;
	    case App.Const.SKIN_TYPE_3H:
	    {
		sprite.setFrame(0+3*frameOffset);
		sprite.draw(x, y);

		sprite.setFrame(1+3*frameOffset);
		int offset = x + sprite.spr_FrameWidth;
		int times = (dx-2*sprite.spr_FrameWidth)/sprite.spr_FrameWidth+1;
		for(int i = 0; i < times; ++i)
		{
		    sprite.drawClip(offset, y, x, y, dx, dy);
		    offset += sprite.spr_FrameWidth;
		}

		sprite.setFrame(2+3*frameOffset);
		sprite.draw(x+dx-sprite.spr_FrameWidth, y);
	    }
	    break;
	}
    }
}
