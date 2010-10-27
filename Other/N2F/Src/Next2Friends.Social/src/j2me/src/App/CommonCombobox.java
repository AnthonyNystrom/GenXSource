package App;

import gui.GUIControl;
import gui.GUICombobox;
import gui.GUIImage;
import java.util.Vector;

public class CommonCombobox extends GUICombobox implements Clonable
{
    private int	    animCounter;
    public GUIImage sprite;
    public Vector   selectAction;

    public CommonCombobox()
    {
	super();
	selectAction = new Vector();
    }
    
    public void addControl(GUIControl control, int action)
    {
	super.addControl(control);
	selectAction.addElement(new Integer(action));
    }
    
    public void addControl(GUIControl control)
    {
	super.addControl(control);
	selectAction.addElement(new Integer(Const.ACTION_NULL));
    }
    
    public void update()
    {
	
	GUIControl oldActive = activeItem;
	super.update();
	if(selectAction.size() != items.size())
	{
	    selectAction.removeElementAt(0);
	}
	if(activeItem != oldActive)
	{
	    int num = items.indexOf(activeItem);
	    Core.screenManager.activeScreen.onAction(((Integer)selectAction.elementAt(num)).intValue());
	}

	animCounter = animCounter > (sprite.sprite.spr_MaxFrames-2)/2-2 ? 0 : animCounter+1;
    }
    
    public Object clone()
    {
	CommonCombobox copy = new CommonCombobox();
	
	return copy(copy);
    }
    
    public Object copy(Object from)
    {
	CommonCombobox copy = (CommonCombobox)from;
	copy.sprite = sprite;
	
	return super.copy(copy);
    }
    
    public void render(int offsetX, int offsetY)
    {
	super.render(offsetX, offsetY);
	
	if(sprite != null)
	{
	    //left arrow
	    if(activeItem != items.firstElement())
	    {
		Core.staticG.setClip(0, 0, Core.SCREEN_WIDTH, Core.SCREEN_HEIGHT);
		if(isSelected)
		{
		    sprite.frame = animCounter;   
		}
		else
		{
		    sprite.frame = sprite.sprite.spr_MaxFrames-2;   
		}
		sprite.render(offsetX, offsetY);
	    }
	    
	    if(activeItem != items.lastElement())
	    {
		Core.staticG.setClip(0, 0, Core.SCREEN_WIDTH, Core.SCREEN_HEIGHT);
		if(isSelected)
		{
		    sprite.frame = (sprite.sprite.spr_MaxFrames-2)/2+animCounter;
		}
		else
		{
		    sprite.frame = sprite.sprite.spr_MaxFrames-1;
		}
		sprite.render(offsetX+dx-sprite.sprite.spr_FrameWidth, offsetY);
	    }
	}
    }
    
}
