package App;


import gui.GUIPopup;


public class CommonPopup extends GUIPopup implements Clonable
{

    public CommonPopup()
    {
    }
    
    public void drawBack(int x, int y)
    {
	int oldColor = Core.staticG.getColor();
	
//	Core.staticG.setColor(0x0062e4);

//	for(int i = 0; i < Core.SCREEN_HEIGHT; i += 2)
//	{
//	    Core.staticG.drawLine(0, i, Core.SCREEN_WIDTH, i);
//	}
	Core.staticG.setClip(0, 0, Core.SCREEN_WIDTH, Core.SCREEN_HEIGHT);
	Core.staticG.drawImage(CommonScreen.blackImage, 0, 0, 0);
	
	Core.staticG.setColor(0xffffff);
	Core.staticG.fillRect(x, y, dx, dy);
	
	Core.staticG.setColor(0x2695f6);
	Core.staticG.drawRect(x, y, dx-1, dy-1);

	Core.staticG.setColor(oldColor);
    }
    
    public Object clone()
    {
	CommonPopup copy = new CommonPopup();
	
	return copy(copy);
    }
}
