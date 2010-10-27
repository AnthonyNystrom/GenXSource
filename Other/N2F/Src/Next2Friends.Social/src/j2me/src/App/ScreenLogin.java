package App;

import gui.GUIListItem;
import gui.GUIText;
import gui.GUIList;
import java.io.IOException;
import javax.microedition.lcdui.*;
import service.*;
import tag.*;

public class ScreenLogin extends CommonScreen
{

    public void onShow()
    {
	Core.storage.readWriteSettings(true);

	footer.posText.setText("Menu");
	footer.posAction = Const.ACTION_POPUP;
	footer.negText.setText("Clear");
	footer.negAction = Const.ACTION_NULL;

	CommonPopup loginPopup = new CommonPopup();
	GUIListItem popupItem = CommonScreen.createPopupItem();

	((GUIText) popupItem.items.elementAt(0)).setText("Login");
	popupItem.clickAction = Const.ACTION_LOGIN;
	loginPopup.list.addItem(popupItem);

	popupItem = (GUIListItem) popupItem.clone();
	((GUIText) popupItem.items.elementAt(0)).setText("Exit Application");
	popupItem.clickAction = Const.ACTION_EXIT;
	loginPopup.list.addItem(popupItem);

	popupItem = (GUIListItem) popupItem.clone();
	((GUIText) popupItem.items.elementAt(0)).setText("Exit");
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
	loginEdit.dx = login.dx - 2 * leftMargin;
	loginEdit.dy = login.dy;
	loginEdit.y = login.y;
	loginEdit.setText(Core.storage.login);

	GUIListItem item2 = (GUIListItem) item.clone();
	item2.items.removeAllElements();
	item2.addItem(loginEdit);
	list.addItem(item2);

	item.isSelectable = false;
	item = item = (GUIListItem) item.clone();
	((GUIText) item.items.elementAt(0)).setText("Password");
	list.addItem(item);

	item2 = (GUIListItem) item2.clone();
	((GUIText) item2.items.elementAt(0)).isPassword = true;
	((GUIText) item2.items.elementAt(0)).setText(Core.storage.password);
	list.addItem(item2);

	list.down();
	addControl(list);
    }

    public void onAction(int action)
    {
	switch(action)
	{
	    case Const.ACTION_LOGIN:
		{
		    Core.isNetwork = true;
		    Core.forceRepaint();
		    MemberService memberService = new service.MemberService();
		    GUIText loginText = ((GUIText) ((GUIListItem) ((GUIList) items.elementAt(0)).items.elementAt(1)).items.elementAt(0));
		    GUIText passwordText = ((GUIText) ((GUIListItem) ((GUIList) items.elementAt(0)).items.elementAt(3)).items.elementAt(0));
		    String loginString = new String(loginText.text, 0, loginText.textLen);
		    String passwordString = new String(passwordText.text, 0, passwordText.textLen);
		    try
		    {
			if(true == memberService.CheckUserExists(loginString, passwordString))
			{
			    Core.storage.tagId = memberService.GetTagID(loginString, passwordString);
			    Core.storage.key = memberService.GetEncryptionKey(loginString, passwordString);
			    Core.storage.login = loginString;
			    Core.storage.password = passwordString;
			    Core.storage.readWriteSettings(false);

			    Core.encryptor = new TagEncryptor(Core.storage.key.getBytes());

			    super.onAction(Const.ACTION_MAINMENU);
			}
			else
			{
			    Core.message = "Wrong login/password";
			}
		    }
		    catch(IOException ex)
		    {
			Core.message = "Network error";
			super.onAction(Const.ACTION_MAINMENU);
		    }
		    Core.isNetwork = false;

		}
		break;
	    default:
		super.onAction(action);
	}
    }

    public void update()
    {
	if(Core.keyAction == Const.KEY_OK)
	{
	    if(((GUIList) items.elementAt(0)).activeItem == ((GUIList) items.elementAt(0)).items.elementAt(1))
	    {
		((GUIList) items.elementAt(0)).down();
	    }
	    else
	    {
		onAction(Const.ACTION_LOGIN);
	    }
	}

//	if(!Core.storage.login.equals("") && !Core.storage.password.equals(""))
//	{
//	    onAction(Const.ACTION_LOGIN);
//	}

	super.update();
    }
}