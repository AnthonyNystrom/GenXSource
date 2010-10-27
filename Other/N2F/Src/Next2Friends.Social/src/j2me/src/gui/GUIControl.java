package gui;


abstract public class GUIControl implements App.Clonable
{
    public int x, y, dx, dy;
    public boolean isSelected;
    public boolean isChecked;
    public GUISkin skin;
    
    public GUIControl()
    {
	isSelected = false;
	isChecked = false;
    }
    
    public Object copy(Object from)
    {
	GUIControl copy = (GUIControl)from;
	
	copy.x = x;
	copy.y = y;
	copy.dx = dx;
	copy.dy = dy;
	copy.isSelected = false;
	copy.skin = skin;
	
	return copy;
    }
    
    public abstract Object clone();
    
    public void update()
    {
	
    }
    
    public void render(int offsetX, int offsetY)
    {
	App.Core.staticG.clipRect(x+offsetX, y+offsetY, dx, dy);
    }
    
    public void onSelect()
    {
	isSelected = true;
    }
    
    public void onDeselect()
    {
	isSelected = false;
    }
    
    public void onClick()
    {
    }
}
