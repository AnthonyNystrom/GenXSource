package App;

import gui.GUIListItem;
import gui.GUIText;
import gui.GUIList;
import java.io.IOException;
import javax.microedition.lcdui.*;
import service.MemberService;

public class ScreenRemind extends CommonScreen 
{

    public void onShow() 
    {
        footer.posText.setText("Menu");
        footer.posAction = Const.ACTION_POPUP;
        footer.negText.setText("Clear");
        footer.negAction = Const.ACTION_NULL;

        CommonPopup loginPopup = new CommonPopup();
        GUIListItem popupItem = CommonScreen.createPopupItem();

        ((GUIText) popupItem.items.elementAt(0)).setText("Send");
        popupItem.clickAction = Const.ACTION_SEND;
        loginPopup.list.addItem(popupItem);

        popupItem = (GUIListItem) popupItem.clone();
        ((GUIText) popupItem.items.elementAt(0)).setText("Back");
        popupItem.clickAction = Const.ACTION_BACK;
        loginPopup.list.addItem(popupItem);

        popupItem = (GUIListItem) popupItem.clone();
        ((GUIText) popupItem.items.elementAt(0)).setText("Exit");
        popupItem.clickAction = Const.ACTION_POPUP;
        loginPopup.list.addItem(popupItem);
        popup = loginPopup;
        
        GUIList list = CommonScreen.createList();
        addControl(list);
	
	GUIText login = new GUIText(Core.smallFont, false, false);
	login.dx = Core.SCREEN_WIDTH;
	login.dy = CommonScreen.itemSkin.sprite.spr_FrameHeight;
	login.anchor = Graphics.TOP | Graphics.LEFT;
	login.setText("Email:");
	
	GUIListItem item = CommonScreen.createItem();
        
	
	item.addItem(login);
	list.addItem(item);
	
	int leftMargin = Core.smallFont.charWidth('w');
	CommonEdit loginEdit = new CommonEdit();
	loginEdit.x = leftMargin;
	loginEdit.dx = login.dx-2*leftMargin;
	loginEdit.dy = login.dy;
	loginEdit.y = login.y;
        
        GUIListItem item2 = (GUIListItem)item.clone();
	item2.items.removeAllElements();
	item2.addItem(loginEdit);
	list.addItem(item2); 
        
        item.isSelectable = false;
        list.down();
    }

    public void onAction(int action) 
    {
        switch (action) 
        {
            case Const.ACTION_SEND: 
            {
                Core.isNetwork = true;
                GUIText email = ((GUIText)((GUIListItem)((GUIList)items.elementAt(0)).items.elementAt(1)).items.elementAt(0));
                MemberService service = new MemberService();
                try
                {
                    service.RemindPassword(new String(email.text, 0, email.textLen));
                    Core.message = "Password sent";
                    onAction(Const.ACTION_BACK);
                }
                catch(IOException ex)
                {
                    Core.message = "Cannot send password";
                }
                onAction(Const.ACTION_POPUP);
                Core.isNetwork = false;
            }
                break;
            default:
                super.onAction(action);
                break;
        }
    }
}
