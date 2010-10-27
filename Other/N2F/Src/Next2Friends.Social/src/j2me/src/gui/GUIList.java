package gui;

import java.util.Vector;
import javax.microedition.lcdui.*;

public class GUIList extends GUIControl implements App.Clonable
{

    public Vector items;
    public GUIListItem activeItem;
    private int scrollHeight;
    public  int scrollOffset;
    public int needScrollOffset;
    public int firstVisible;
    public int visibleCount;

    public GUIList()
    {
	super();

	items = new Vector();
    }

    public Object clone()
    {
	GUIList newObj = new GUIList();
	return copy(newObj);
    }

    public Object copy(Object from)
    {
	GUIList copy = (GUIList) from;

	copy.scrollHeight = scrollHeight;

	int size = items.size();
	copy.items.ensureCapacity(size);
	for(int i = 0; i < size; ++i)
	{
	    copy.items.addElement(((GUIListItem) items.elementAt(i)).clone());
	}

	int num = items.indexOf(activeItem);
	copy.activeItem = (GUIListItem) copy.items.elementAt(num);
	copy.activeItem.onSelect();


	return super.copy(copy);
    }

    public void addItem(GUIListItem newItem)
    {
	items.addElement(newItem);
	if(1 == items.size())
	{
	    activeItem = newItem;
	    activeItem.onSelect();
	}
	scrollHeight = dy * dy / getHeight();
	visibleCount = dy/newItem.dy;
    }

    public int getHeight()
    {
	int itemsHeight = 0;
	int size = items.size();
	for(int i = 0; i < size; i++)
	{
	    GUIListItem item = ((GUIListItem) (items.elementAt(i)));
	    itemsHeight += item.dy;
	}
	return itemsHeight;
    }

    public void down()
    {
	int currIndex = items.indexOf(activeItem);
	((GUIControl) items.elementAt(currIndex)).onDeselect();

	GUIListItem item;
	do
	{
	    currIndex = currIndex == items.size() - 1 ? 0 : currIndex + 1;
	    item = ((GUIListItem) items.elementAt(currIndex));
	}
	while(!(item.isSelectable && item.isVisible));

	((GUIControl) items.elementAt(currIndex)).onSelect();

	activeItem = (GUIListItem) items.elementAt(currIndex);
	updateScroll();
    }

    public void up()
    {
	int currIndex = items.indexOf(activeItem);
	((GUIControl) items.elementAt(currIndex)).onDeselect();

	GUIListItem item;
	do
	{
	    currIndex = currIndex == 0 ? items.size() - 1 : currIndex - 1;
	    item = ((GUIListItem) items.elementAt(currIndex));
	}
	while(!(item.isSelectable && item.isVisible));

	((GUIControl) items.elementAt(currIndex)).onSelect();

	activeItem = (GUIListItem) items.elementAt(currIndex);
	updateScroll();
    }

    public void update()
    {
	//scroll
	if(scrollOffset > needScrollOffset)
	{
	    scrollOffset -= 1+(scrollOffset - needScrollOffset) / 3;
	}
	else if(scrollOffset < needScrollOffset)
	{
	    scrollOffset += 1+(needScrollOffset - scrollOffset) / 3;
	}

	//keys processing
	int itemsCount = items.size();
	if(itemsCount == 0)
	{
	    return;
	}
	switch(App.Core.keyAction)
	{
	    case App.Const.KEY_UP:
		up();
		break;
	    case App.Const.KEY_DOWN:
		down();
		break;
	    case App.Const.KEY_OK:
		if(activeItem != null)
		{
		    activeItem.onClick();
		}
		break;
	}

	int size = items.size();
	for(int i = 0; i < size; i++)
	{
	    ((GUIListItem) (items.elementAt(i))).update();
	}
    }

    private void updateScroll()
    {
//	int currIndex = items.indexOf(activeItem);
//	int needY = 0;
//	
//	for(int i = 0; i < currIndex; ++i)
//	{
//	    needY += ((GUIListItem)items.elementAt(i)).dy;
//	}
//	int needDy = needY + ((GUIListItem)items.elementAt(currIndex)).dy;
//	
//	if(needY < scrollOffset)
//	{
//	    needScrollOffset = needY;
//	}
//	else if (needDy > scrollOffset+dy)
//	{
//	    needScrollOffset = needDy-dy;
//	}

	int currIndex = items.indexOf(activeItem);
	int needY = 0;

	for(int i = 0; i < currIndex; ++i)
	{
	    needY += ((GUIListItem) items.elementAt(i)).dy;
	}
	int currHeight = ((GUIListItem) items.elementAt(currIndex)).dy;
	int needDy = needY + currHeight;

	if(needY < scrollOffset)
	{
	    needScrollOffset = needY;
	}
	else if(needDy > needScrollOffset + dy)
	{
	    needScrollOffset = needDy - dy;
	}
	
	firstVisible = needScrollOffset/currHeight;
    }

    public void render(int offsetX, int offsetY)
    {
	super.render(offsetX, offsetY);

	int drawY = offsetY - scrollOffset;

	drawBack();
	drawScroll();

	int size = items.size();
	for(int i = 0; i < size; i++)
	{
	    App.Core.staticG.setClip(x + offsetX, y + offsetY, dx, dy);
	    GUIListItem item = ((GUIListItem) (items.elementAt(i)));
	    if(item.isVisible)
	    {
		if((y + item.y + item.dy + drawY >= offsetY) && (y + item.y + drawY <= offsetY + dy))
		{
		    item.render(x + offsetX, y + drawY);
		}
		drawY += item.dy;
	    }
	}
    }

    protected void drawBack()
    {
    }

    protected void drawScroll()
    {
    }
}
