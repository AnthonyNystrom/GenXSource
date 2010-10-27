package gui;

import java.util.Vector;

public class GUICombobox extends GUIControl implements App.Clonable
{

    public Vector items;
    public GUIControl activeItem;
    public GUIControl oldItem;
    public int selectedColor;
    public int unselectedColor;
    public boolean hideFirst;
    private int offset;
    public GUIControl targetItem;
    private boolean toRight;

    public GUICombobox()
    {
	super();

	items = new Vector();
	offset = 0;
	hideFirst = false;
    }

    public void addControl(GUIControl control)
    {
	items.addElement(control);
	if(activeItem == null)
	{
	    activeItem = targetItem = control;
	}
    }

    public void update()
    {
	if(targetItem != activeItem || offset != 0)
	{
	    int absSpeed = 2 * Math.abs(dx - (dx - offset)) / 3 + 2;
	    int speed = toRight ? absSpeed : -absSpeed;
	    offset += speed;
	    if(activeItem == targetItem)
	    {
		if(toRight && offset > 0)
		{
		    offset = 0;
		}
		else
		    if(!toRight && offset < 0)
		    {
			offset = 0;
		    }
	    }
	    if(offset >= dx)
	    {
		activeItem = targetItem;
		offset = -dx;
		if(hideFirst)
		{
		    items.removeElementAt(0);
		    hideFirst = false;
		}
	    }
	    else
		if(offset <= -dx)
		{
		    activeItem = targetItem;
		    offset = dx;
		}
	}

	if(isSelected)
	{
	    switch(App.Core.keyAction)
	    {
		case App.Const.KEY_LEFT:
		{
		    left();
		    break;
		}
		case App.Const.KEY_RIGHT:
		{
		    right();
		    break;
		}
	    }
	}
    }

    public void right()
    {
	int num = items.indexOf(targetItem);
	num = num == (items.size() - 1) ? num : num + 1;
	targetItem = (GUIControl) items.elementAt(num);
	toRight = true;
    }

    public void left()
    {
	int num = items.indexOf(targetItem);
	num = num == 0 ? num : num - 1;
	targetItem = (GUIControl) items.elementAt(num);
	toRight = false;
    }

    public void render(int offsetX, int offsetY)
    {
	super.render(offsetX, offsetY);

	super.render(offsetX, offsetY);

	int oldColor = App.Core.staticG.getColor();
	if(skin != null)
	{
	    int frameOffset;
	    if(isSelected)
	    {
		App.Core.staticG.setColor(selectedColor);
		frameOffset = 0;
	    }
	    else
	    {
		App.Core.staticG.setColor(unselectedColor);
		frameOffset = 1;
	    }
	    skin.draw(x + offsetX, y + offsetY, dx, dy, frameOffset);
	}
	if(activeItem != null)
	{
	    activeItem.render(offsetX + offset, offsetY);
	}
	App.Core.staticG.setColor(oldColor);

    }

    public Object clone()
    {
	GUICombobox copy = new GUICombobox();
	return copy(copy);
    }

    public Object copy(Object from)
    {
	GUICombobox copy = (GUICombobox) from;
	copy.selectedColor = selectedColor;
	copy.unselectedColor = unselectedColor;
	copy.hideFirst = hideFirst;

	int size = items.size();
	copy.items.ensureCapacity(size);
	for(int i = 0; i < size; ++i)
	{
	    copy.items.addElement(((GUIControl) items.elementAt(i)).clone());
	}

	int num = items.indexOf(activeItem);
	copy.activeItem = (GUIControl) copy.items.elementAt(num);

	return super.copy(copy);
    }
}
