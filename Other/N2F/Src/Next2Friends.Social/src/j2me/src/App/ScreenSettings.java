package App;

import gui.GUIListItem;
import gui.GUIText;
import gui.GUIList;
import javax.microedition.lcdui.*;

public class ScreenSettings extends CommonScreen
{
    public void onShow()
    {
	footer.posText.setText("Menu");
	footer.posAction = Const.ACTION_POPUP;
	footer.negText.setText("Clear");
	footer.negAction = Const.ACTION_NULL;
	
	CommonPopup loginPopup = new CommonPopup();
	GUIListItem popupItem = CommonScreen.createPopupItem();
	
	((GUIText)popupItem.items.elementAt(0)).setText("Save");
	popupItem.clickAction = Const.ACTION_SAVE;
	loginPopup.list.addItem(popupItem);
        
        popupItem = (GUIListItem)popupItem.clone();
	((GUIText)popupItem.items.elementAt(0)).setText("Back");
	popupItem.clickAction = Const.ACTION_BACK;
	loginPopup.list.addItem(popupItem);
	
	popupItem = (GUIListItem)popupItem.clone();
	((GUIText)popupItem.items.elementAt(0)).setText("Exit");
	popupItem.clickAction = Const.ACTION_POPUP;
	loginPopup.list.addItem(popupItem);	
	popup = loginPopup;
	
	GUIList list = CommonScreen.createList();
	
	GUIText login = new GUIText(Core.smallFont, false, false);
	login.dx = Core.SCREEN_WIDTH;
	login.dy = CommonScreen.itemSkin.sprite.spr_FrameHeight;
	login.anchor = Graphics.TOP | Graphics.HCENTER;
	login.setText("Login");
	
	GUIListItem item = CommonScreen.createItem();
	
	item.addItem(login);
	list.addItem(item);
	
	int leftMargin = Core.smallFont.charWidth('w');
	CommonEdit loginEdit = new CommonEdit();
	loginEdit.x = leftMargin;
	loginEdit.dx = login.dx-2*leftMargin;
	loginEdit.dy = login.dy;
	loginEdit.y = login.y;
	loginEdit.setText(Core.storage.login);
	
	GUIListItem item2 = (GUIListItem)item.clone();
	item2.items.removeAllElements();
	item2.addItem(loginEdit);
	list.addItem(item2);
	
	item.isSelectable = false;
	item = item = (GUIListItem)item.clone();
	((GUIText)item.items.elementAt(0)).setText("Password");
	list.addItem(item);
	
	item2 = (GUIListItem)item2.clone();
	((GUIText)item2.items.elementAt(0)).isPassword = true;
	list.addItem(item2);
	((GUIText)item2.items.elementAt(0)).setText(Core.storage.password);
	
	list.down();
	addControl(list);
    }
    
    public void onAction(int action)
    {
        switch(action)
        {
            case Const.ACTION_SAVE:
                save();
                onAction(Const.ACTION_POPUP);
                break;
            default:
                super.onAction(action);
                break;
        }
    }
    
    public void onHide()
    {
	save();
	super.onHide();
    }
    
    private void save()
    {
        GUIText login = ((GUIText)((GUIListItem)((GUIList)items.elementAt(0)).items.elementAt(1)).items.elementAt(0));
	Core.storage.login = new String(login.text, 0, login.textLen);
	
	GUIText password = ((GUIText)((GUIListItem)((GUIList)items.elementAt(0)).items.elementAt(3)).items.elementAt(0));
	Core.storage.password = new String(password.text, 0, password.textLen);
        
        Core.storage.readWriteSettings(false);
    }
}
