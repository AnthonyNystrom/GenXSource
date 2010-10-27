package gui;

import java.util.Vector;

public class GUIListItem extends GUIControl implements App.Clonable
{
    public Vector   items;
    public int	    clickAction;
    public int	    selectedColor;
    public int	    unselectedColor;
    public boolean  isCheckbox;
    public boolean  isSelectable;
    public boolean  isVisible;

    public GUIListItem()
    {
	super();
	
	clickAction = Integer.MAX_VALUE;
	items = new Vector();
	isCheckbox = false;
	isSelectable = true;
	isVisible = true;
    }
    
    public Object clone()
    {
	GUIListItem newObj = new GUIListItem();
	return copy(newObj);
    }
    
    public Object copy(Object from)
    {
	GUIListItem copy = (GUIListItem)from;
	copy.clickAction = clickAction;
	//copy.skin = skin;
	copy.selectedColor = selectedColor;
	copy.unselectedColor = unselectedColor;
	copy.isCheckbox = isCheckbox;
	copy.isSelectable = isSelectable;
	copy.isVisible = isVisible;
	
	int size = items.size();
	copy.items.ensureCapacity(size);
	for(int i = 0; i < size; ++i)
	{
	    copy.items.addElement(((GUIControl)items.elementAt(i)).clone());
	}
	
	return super.copy(copy);
    }
    
    public void addItem(GUIControl item)
    {
	items.addElement(item);
    }
    
    public void onSelect()
    {
	super.onSelect();
	int size = items.size();
	for(int i = 0; i < size; ++i)
	{
	    ((GUIControl)items.elementAt(i)).onSelect();
	}
    }
    
    public void onDeselect()
    {
	super.onDeselect();
	int size = items.size();
	for(int i = 0; i < size; ++i)
	{
	    ((GUIControl)items.elementAt(i)).onDeselect();
	}
    }
  
    public void update()
    {
	int size = items.size();
	for(int i = 0; i < size; i++)
	{
	    ((GUIControl)(items.elementAt(i))).update();
	}
    }
    
    public void render(int offsetX, int offsetY)
    {
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
	    skin.draw(x+offsetX, y+offsetY, dx, dy, frameOffset);
	}
	
	int size = items.size();
	for(int i = 0; i < size; i++)
	{
	    int oldX = App.Core.staticG.getClipX();
	    int oldY = App.Core.staticG.getClipY();
	    int oldW = App.Core.staticG.getClipWidth();
	    int oldH = App.Core.staticG.getClipHeight();
	    GUIControl control = ((GUIControl)(items.elementAt(i)));
	    control.render(x+offsetX, y+offsetY);
	    offsetX += control.dx;
	    App.Core.staticG.setClip(oldX, oldY, oldW, oldH);
	}
	App.Core.staticG.setColor(oldColor);
    }
    
    public void onClick()
    {
	if(isCheckbox)
	{
	    isChecked = !isChecked;
	    int size = items.size();
	    for(int i = 0; i < size; ++i)
	    {
		((GUIControl)items.elementAt(i)).isChecked = isChecked;
	    }
	}
	else
	{
	    App.Core.screenManager.activeScreen.onAction(clickAction);
	}
    }
}
