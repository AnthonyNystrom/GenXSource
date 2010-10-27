package gui;


public class GUIPopup extends GUIControl implements App.Clonable
{
    public boolean isActive;
    public GUIList  list;
    
    public GUIPopup()
    {
	list = new GUIList();
    }
    
    public void update()
    {
	if(isActive)
	{
	    list.update();
	}
    }
    
    public void drawBack(int x, int y)
    {
	
    }
    
    public void render(int offsetX, int offsetY)
    {
	if(isActive)
	{
	    App.Core.staticG.setClip(offsetX, offsetY-dy, dx, dy);
	    drawBack(offsetX, offsetY-dy);
	    list.render(offsetX, offsetY-dy);
	}
    }
    
    public void toggle()
    {
	if(isActive)
	{
	    isActive = false;
	}
	else
	{
	    if(!list.items.isEmpty())
	    {
		isActive = true;
		//recalc rect
		dx = ((GUIListItem)list.items.elementAt(0)).dx;
		list.dx = dx;
		x = 0;
		y = 0;
		dy = list.getHeight();
		list.dy = dy;
	    }
	}
    }
    
    public Object copy(Object from)
    {
	GUIPopup copy = (GUIPopup)from;

	copy.list = (GUIList)list.clone();
	
	return super.copy(copy);
    }
    
    public Object clone()
    {
	GUIPopup newObj = new GUIPopup();
	return copy(newObj);
    }
}
