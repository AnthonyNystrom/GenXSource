package gui;

import javax.microedition.lcdui.*;
import java.io.*;
import java.util.Vector;

public class GUIScreen implements App.Clonable
{

    public Vector items;
    public GUIControl activeItem;
    public GUIHeader header;
    public GUIFooter footer;
    public GUIPopup popup;
    public int offsetX;
    public int offsetY;

    public GUIScreen()
    {
	items = new Vector(App.Const.MAX_CONTROLS);
    }

    public void addControl(GUIControl control)
    {
	items.addElement(control);
    }

    public Object clone()
    {
	GUIScreen copy = new GUIScreen();
	return copy(copy);
    }

    public Object copy(Object from)
    {
	GUIScreen copy = (GUIScreen) from;
	copy.header = header;
	copy.footer = (GUIFooter) footer.clone();
	copy.popup = (GUIPopup) popup.clone();
	copy.offsetX = offsetX;
	copy.offsetY = offsetY;

	int size = items.size();
	copy.items.ensureCapacity(size);
	for(int i = 0; i < size; ++i)
	{
	    copy.items.addElement(((GUIControl) items.elementAt(i)).clone());
//	    if(i == 0)
//	    {
//		((GUIControl)copy.items.elementAt(i)).onSelect();
//	    }
	}

	//int num = items.indexOf(activeItem);
	//copy.activeItem = (GUIItemList)copy.items.elementAt(num);

	return copy;
    }

    public void onShow()
    {
	if(activeItem == null)
	{
	    if(!items.isEmpty())
	    {
		activeItem = (GUIControl) items.elementAt(0);
	    }
	}
	if(popup != null)
	{
	    popup.isActive = false;
	}
    }

    public void onHide()
    {
    }

    public void update()
    {
//	switch(Core.keyAction)
//	{
//	    case Const.KEY_OK:
//		onKeyOk();
//		break;
//	}
	if(popup == null || (popup != null && !popup.isActive))
	{
	    int size = items.size();
	    for(int i = 0; i < size; i++)
	    {
		((GUIControl) (items.elementAt(i))).update();
	    }
	}

	if(footer != null)
	{
	    footer.update();
	}

	if(popup != null)
	{
	    popup.update();
	}

    }

    public void render()
    {
	offsetX = 0;
	offsetY = 0;

	drawHeader();
	drawBack(offsetX, offsetY);
	
	if(footer != null)
	{
	    footer.draw();
	}

	App.Core.staticG.setClip(0, 0, App.Core.SCREEN_WIDTH, App.Core.SCREEN_HEIGHT);
	drawBody();

	
	if(popup != null)
	{
	    popup.render(0, App.Core.SCREEN_HEIGHT - footer.getHeight());
	}
    }

    public void drawBack(int x, int y)
    {
    }

    public void drawHeader()
    {
	if(header != null)
	{
	    header.draw();
	    offsetY += header.getHeight();
	}
    }

    public void drawBody()
    {
	int size = items.size();
	for(int i = 0; i < size; i++)
	{
	    App.Core.staticG.setClip(0, 0, App.Core.SCREEN_WIDTH, App.Core.SCREEN_HEIGHT);
	    GUIControl item = ((GUIControl) (items.elementAt(i)));
	    item.render(offsetX, offsetY);
	    offsetY += item.dy;

	}
    }

    void onKeyPositive()
    {
    }

    void onKeyNegative()
    {
    }

    void onKeyOk()
    {
    }

    public void onAction(int action)
    {
    }
}
