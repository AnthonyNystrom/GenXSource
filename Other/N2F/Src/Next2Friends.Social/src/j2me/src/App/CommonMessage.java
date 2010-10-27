package App;


import gui.GUIMessage;
import gui.GUIText;


public class CommonMessage extends GUIMessage
{
    public CommonMessage()
    {
	super();
	
	message = new GUIText(Core.bigFont, true, false);
	message.x = 0;
	message.y = Core.SCREEN_HEIGHT/3;
	message.dx = Core.SCREEN_WIDTH;
	message.dy = 2*Core.SCREEN_HEIGHT/3;
    }
    
    public void render()
    {
	Core.staticG.drawImage(CommonScreen.blackImage, 0, 0, 0);
	message.render(0, 0);
    }
}
