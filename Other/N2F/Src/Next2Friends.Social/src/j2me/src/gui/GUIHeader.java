package gui;


public class GUIHeader
{   
    public GUISkin skin;
    
    public GUIHeader()
    {
    }
    
    public void draw()
    {
	if(skin != null)
	{
	    skin.draw(0, 0, App.Core.SCREEN_WIDTH, App.Core.SCREEN_HEIGHT);
	}
	App.Core.staticG.setClip(0, 0, App.Core.SCREEN_WIDTH, App.Core.SCREEN_HEIGHT);
	drawPostSkin();
    }
    
    public int getHeight()
    {
	return skin.sprite.spr_MaxHeight;
    }
    
    public void drawPostSkin()
    {
	
    }
}
