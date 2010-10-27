package App;

import gui.GUIText;


public class CommonEdit extends GUIText implements Clonable
{

    public CommonEdit()
    {
	super(Core.smallFont, false, true);
    }
    
    public Object clone()
    {
	CommonEdit copy = new CommonEdit();
	return copy(copy);
    }
    
    public Object copy(Object from)
    {
	CommonEdit copy = (CommonEdit)from;
	
	return super.copy(copy);
    }
    
    public void render(int offsetX, int offsetY)
    {
	int oldColor = Core.staticG.getColor();
	
	//Core.staticG.setColor(0x888888);
	//Core.staticG.fillRect(x+offsetX, y+offsetY, dx, dy);
	
	Core.staticG.setColor(oldColor);
	
	super.render(offsetX, offsetY);
	
    }
    
}
