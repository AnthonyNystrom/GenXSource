package App;

import gui.*;
import java.util.Vector;

public class ScreenDashboard extends CommonScreen
{
    public GUIImage icons;
    public Vector dashItems;

    public ScreenDashboard()
    {
	dashItems = new Vector();
    }

    public void onShow()
    {
	icons = new GUIImage(Const.GUIIMAGE_SPRITE, "/dashicons");
	
	footer.posText.setText("Menu");
	footer.posAction = Const.ACTION_POPUP;
	footer.negText.setText("Back");
	footer.negAction = Const.ACTION_BACK;

	CommonPopup popup = new CommonPopup();
	GUIListItem popupItem = CommonScreen.createPopupItem();

	((GUIText) popupItem.items.elementAt(0)).setText("View");
	popupItem.clickAction = Const.ACTION_VIEW;
	popup.list.addItem(popupItem);

	popupItem = (GUIListItem) popupItem.clone();
	((GUIText) popupItem.items.elementAt(0)).setText("Exit");
	popupItem.clickAction = Const.ACTION_POPUP;
	popup.list.addItem(popupItem);

	this.popup = popup;

	GUIList list = CommonScreen.createList();
	addControl(list);

	GUIListItem item = CommonScreen.createItem();
	item.items.addElement(icons);
	GUIText text = CommonScreen.createText();
        text.useAutoscroll = false;
	item.addItem(text);
	item.clickAction = Const.ACTION_VIEW;
        
        int size = dashItems.size();
        //sort dashboard
        for(int j = 0; j < size; ++j)
        {
            for(int k = 0; k < size-1; ++k)
            {
                DashboardItem item1 = (DashboardItem)dashItems.elementAt(k);
                DashboardItem item2 = (DashboardItem)dashItems.elementAt(k+1);
                long val1 = Long.parseLong(item1.fullDate);
                long val2 = Long.parseLong(item2.fullDate);
                if(val1 < val2)
                {
                    //swap
                    dashItems.setElementAt(item2, k);
                    dashItems.setElementAt(item1, k+1);
                }
            }
        }

	
	for(int i = 0; i < size; i++)
	{
	    item = (GUIListItem) item.clone();
	    DashboardItem dashItem = (DashboardItem) dashItems.elementAt(i);
	    ((GUIImage) item.items.elementAt(0)).frame = dashItem.iconFrame;
	    ((GUIText) item.items.elementAt(1)).setText(dashItem.title + ", " + dashItem.date);
	    list.addItem(item);
	}

    }

    public void onAction(int action)
    {
	switch(action)
	{
	    case Const.ACTION_VIEW:
		{
		    GUIList list = (GUIList)items.elementAt(0);
		    ((ScreenView) Core.view).dash = (DashboardItem) dashItems.elementAt(list.items.indexOf(list.activeItem));
		    super.onAction(action);
		}
		break;
	    default:
		super.onAction(action);
		break;
	}
    }
}
