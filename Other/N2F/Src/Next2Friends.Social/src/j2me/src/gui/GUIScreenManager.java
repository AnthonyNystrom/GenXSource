package gui;

import java.util.Stack;

public class GUIScreenManager
{
    public  Stack	screenStack;
    public  GUIScreen	activeScreen;
    private GUIScreen	newScreen;
    
    
    public GUIScreenManager()
    {
	screenStack = new Stack();
    }
    
    public void setScreen(GUIScreen screen)
    {
	if(screen != activeScreen)
	{
	    newScreen = screen;
	}
    }
    
    public void backScreen()
    {
	activeScreen.onHide();
	
	activeScreen = newScreen = ((GUIScreen)screenStack.pop());
	activeScreen.onShow();	
    }
    
    public void update()
    {
	if(newScreen != activeScreen)
	{
	    if(activeScreen != null)
	    {
		activeScreen.onHide();
		screenStack.push(activeScreen);
	    }
	    activeScreen = newScreen;
	    activeScreen.onShow();
	}
	
	if(activeScreen != null)
	{
	    activeScreen.update();
	}
    }
    
    public void render()
    {
	if(activeScreen != null)
	{
	    activeScreen.render();
	}
    }
}
