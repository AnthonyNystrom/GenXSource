package gui;

import javax.microedition.lcdui.*;

public class GUIFooter implements App.Clonable
{
    public GUISkin  skin;
    public GUIText  posText;
    public GUIText  negText;
    public int	    posAction;
    public int	    negAction;
    public int	    fontColor;
    
    public GUIFooter(Font font)
    { 
	posText = new GUIText(font, false, false);

	posText.dx = App.Core.SCREEN_WIDTH/2;
	posText.anchor = Graphics.TOP | Graphics.HCENTER;
	
	negText = (GUIText)posText.clone();
	negText.x = App.Core.SCREEN_WIDTH/2;
    }
    
    public Object copy(Object from)
    {
	GUIFooter copy = (GUIFooter)from;
	copy.skin = (GUISkin)skin.clone();
	copy.posText = (GUIText)posText.clone();
	copy.negText = (GUIText)negText.clone();
	copy.posAction = posAction;
	copy.negAction = negAction;
	copy.fontColor = fontColor;
	
	return copy;
    }
    
    public Object clone()
    {
	GUIFooter newFooter = new GUIFooter(posText.font);
	
	return copy(newFooter);
    }
    
    public void update()
    {
	switch(App.Core.keyAction)
	{
	    case App.Const.KEY_POSITIVE:
		App.Core.screenManager.activeScreen.onAction(posAction);
		break;
	    case App.Const.KEY_NEGATIVE:
		App.Core.screenManager.activeScreen.onAction(negAction);
		break;	
	}
	if(posText.dy == 0)
	{
	    posText.dy = skin.sprite.spr_FrameHeight;
	    negText.dy = skin.sprite.spr_FrameHeight;
	}
    }
    
    public void draw()
    {
	if(skin != null)
	{
	    
	    int oldColor = App.Core.staticG.getColor();
	    App.Core.staticG.setColor(fontColor);
	    
	    App.Core.staticG.setClip(0, 0, App.Core.SCREEN_WIDTH, App.Core.SCREEN_HEIGHT);
	    skin.draw(0, App.Core.SCREEN_HEIGHT-getHeight(), App.Core.SCREEN_WIDTH/2, App.Core.SCREEN_HEIGHT);
	    //Core.staticG.setClip(0, Core.SCREEN_HEIGHT-getHeight(), Core.SCREEN_WIDTH/2, Core.SCREEN_HEIGHT);
	    drawPostSkin(true);
	    
	    App.Core.staticG.setClip(0, 0, App.Core.SCREEN_WIDTH, App.Core.SCREEN_HEIGHT);
	    skin.draw(App.Core.SCREEN_WIDTH/2, App.Core.SCREEN_HEIGHT-getHeight(), App.Core.SCREEN_WIDTH/2, App.Core.SCREEN_HEIGHT);
	    //Core.staticG.setClip(Core.SCREEN_WIDTH/2, Core.SCREEN_HEIGHT-getHeight(), Core.SCREEN_WIDTH/2, Core.SCREEN_HEIGHT);
	    drawPostSkin(false);
	    
	    App.Core.staticG.setColor(oldColor);
	}
    }
    
    public int getHeight()
    {
	if(skin != null)
	{    
	    return skin.sprite.spr_MaxHeight;
	}
	else
	{
	    return 0;
	}
    }
    
    public void drawPostSkin(boolean left)
    {
	if(left)
	{
	    if(posText != null)
	    {
		posText.render(0, App.Core.SCREEN_HEIGHT-getHeight());
	    }
	}
	else
	{
	    if(negText != null)
	    {
		negText.render(0, App.Core.SCREEN_HEIGHT-getHeight());
	    }
	}
    }
}
