package App;

import gui.GUICombobox;
import gui.GUIImage;
import gui.GUIListItem;
import gui.GUIList;
import gui.GUIText;
import javax.microedition.lcdui.*;

public class ScreenTag extends CommonScreen
{
    public void onShow()
    {
	footer.posText.setText("Menu");
	footer.posAction = Const.ACTION_POPUP;
	footer.negText.setText("Back");
	footer.negAction = Const.ACTION_BACK;
	
	CommonPopup popup = new CommonPopup();
	GUIListItem popupItem = CommonScreen.createPopupItem();
	
	((GUIText)popupItem.items.elementAt(0)).setText("Select");
	popupItem.clickAction = Const.ACTION_SELECT;
	popup.list.addItem(popupItem);
	
	popupItem = (GUIListItem)popupItem.clone();
	((GUIText)popupItem.items.elementAt(0)).setText("Exit");
	popupItem.clickAction = Const.ACTION_POPUP;
	popup.list.addItem(popupItem);
	
	this.popup = popup;
	
	GUIList list = CommonScreen.createList();
	addControl(list);
	
	GUIListItem item = CommonScreen.createItem();
	item.addItem(checkIcon);
	
	GUIText combotext = new GUIText(Core.smallFont, false, false);
	combotext.dx = item.dx;
	combotext.dy = item.dy;
	combotext.setText("Enable tagging");
	combotext.anchor = Graphics.TOP | Graphics.LEFT;
	item.addItem(combotext);
	item.isCheckbox = true;
	item.isChecked = tag.BTLocator.isEnabled;
	list.addItem(item);
	
//	CommonCombobox combobox = new CommonCombobox();
//	combobox.skin = CommonScreen.itemSkin;
//	combobox.sprite = CommonScreen.combobox;
//	combobox.dx = item.dx;
//	combobox.dy = item.dy;
//	combobox.selectedColor = 0xffffff;
//	combobox.unselectedColor = 0x000000;
//	combobox.hideFirst = true;
//	
//	combotext = (GUIText) combotext.clone();
//	combotext.anchor = Graphics.TOP | Graphics.HCENTER;
//	combotext.setText("Select period");
//	combobox.addControl(combotext, Const.ACTION_NULL);
//	
//	combotext = (GUIText) combotext.clone();
//	combotext.setText("5 min");
//	combobox.addControl(combotext, Const.ACTION_NULL);
//	
//	combotext = (GUIText) combotext.clone();
//	combotext.setText("1 hour");
//	combobox.addControl(combotext, Const.ACTION_NULL);
//	
//	combotext = (GUIText) combotext.clone();
//	combotext.setText("1 day");
//	combobox.addControl(combotext, Const.ACTION_NULL);
//	
//	combotext = (GUIText) combotext.clone();
//	combotext.setText("Manually");
//	combobox.addControl(combotext, Const.ACTION_NULL);
//
//	item = CommonScreen.createItem();
//	item.addItem(combobox);
//	list.addItem(item);
    }
    
    public void onHide()
    {
	boolean isEnabled = ((GUIListItem)((GUIList)items.elementAt(0)).items.elementAt(0)).isChecked;
	tag.BTLocator.isEnabled = isEnabled;
//	Core.btServer.isEnabled = isEnabled;
//	GUICombobox combobox = (GUICombobox) ((GUIListItem) ((GUIList) items.elementAt(0)).items.elementAt(1)).items.elementAt(0);
//	int index = combobox.items.indexOf(combobox.activeItem);
//	long sleepTime = 0;
//	switch(index)
//	{
//	    case 0://5 min
//		sleepTime = 5*60*1000;
//		break;
//	    case 1://1 hour
//		sleepTime = 60*60*1000;
//		break;
//	    case 2://1 day
//		sleepTime = 24*60*60*1000;
//		break;
//	}
//	Core.btLocator.sleepTime = sleepTime;
	
	super.onHide();
    }
}
