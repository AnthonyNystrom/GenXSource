package App;

import gui.GUIListItem;
import gui.GUIText;
import gui.GUIList;
import gui.GUIImage;

public class ScreenMainmenu extends CommonScreen
{

    public GUIImage icons;
    public static int inboxCount;
    public static int dashboardCount;
    public static int draftsCount;
    public static int outboxCount;

    public void updateCounts()
    {
        GUIList list = (GUIList) items.elementAt(0);

        GUIListItem item = (GUIListItem) list.items.elementAt(1);
        GUIText text = (GUIText) item.items.elementAt(1);
        text.setText("Inbox (" + inboxCount + ")");

        item = (GUIListItem) list.items.elementAt(2);
        text = (GUIText) item.items.elementAt(1);
        text.setText("Dashboard (" + dashboardCount + ")");

        item = (GUIListItem) list.items.elementAt(3);
        text = (GUIText) item.items.elementAt(1);
        text.setText("Drafts (" + draftsCount + ")");

        item = (GUIListItem) list.items.elementAt(4);
        text = (GUIText) item.items.elementAt(1);
        text.setText("Outbox (" + outboxCount + ")");
    }

    public void onHide()
    {
        super.onHide();
        icons = null;
    }

    public void onShow()
    {
        if (Core.backNet == null)
        {
            Core.backNet = new BackgroundThread();
        }

        icons = new GUIImage(Const.GUIIMAGE_SPRITE, "/icons");

        footer.posText.setText("Menu");
        footer.posAction = Const.ACTION_POPUP;
        footer.negText.setText("Exit");
        footer.negAction = Const.ACTION_EXIT;

        GUIList list = CommonScreen.createList();

        GUIListItem item = CommonScreen.createItem();
        item.items.addElement(icons);

        GUIText text = CommonScreen.createText();
        text.setText("Go!");
        item.addItem(text);
        list.addItem(item);

        GUIListItem item2 = (GUIListItem) item.clone();
        ((GUIImage) item2.items.elementAt(0)).frame = 1;
        list.addItem(item2);
        item2.clickAction = Const.ACTION_SHOWINBOX;

        //dashboardCount = Core.storage.countData("dashboard");
        item2 = (GUIListItem) item.clone();
        ((GUIImage) item2.items.elementAt(0)).frame = 2;
        list.addItem(item2);
        item2.clickAction = Const.ACTION_DASHBOARD;

        draftsCount = Core.storage.countData("drafts");
        item2 = (GUIListItem) item.clone();
        ((GUIImage) item2.items.elementAt(0)).frame = 3;
        list.addItem(item2);
        item2.clickAction = Const.ACTION_DRAFTS;

        outboxCount = Core.storage.countData("outbox") + Core.storage.countData("comments") + Core.storage.countData("upload");
        item2 = (GUIListItem) item.clone();
        ((GUIImage) item2.items.elementAt(0)).frame = 4;
        list.addItem(item2);
        item2.clickAction = Const.ACTION_OUTBOX;

        item2 = (GUIListItem) item.clone();
        ((GUIImage) item2.items.elementAt(0)).frame = 5;
        ((GUIText) item2.items.elementAt(1)).setText("Settings");
        list.addItem(item2);
        item2.clickAction = Const.ACTION_SETTINGS;

        item.clickAction = Const.ACTION_GO;
        addControl(list);

        CommonPopup popup = new CommonPopup();
        GUIListItem popupItem = CommonScreen.createPopupItem();

        ((GUIText) popupItem.items.elementAt(0)).setText("Select");
        popupItem.clickAction = Const.ACTION_SELECT;
        popup.list.addItem(popupItem);

        popupItem = (GUIListItem) popupItem.clone();
        ((GUIText) popupItem.items.elementAt(0)).setText("Go to web");
        popupItem.clickAction = Const.ACTION_HOMEURL;
        popup.list.addItem(popupItem);

        popupItem = (GUIListItem) popupItem.clone();
        ((GUIText) popupItem.items.elementAt(0)).setText("Get & send now");
        popupItem.clickAction = Const.ACTION_GETSEND;
        popup.list.addItem(popupItem);

        popupItem = (GUIListItem) popupItem.clone();
        ((GUIText) popupItem.items.elementAt(0)).setText("Remind password");
        popupItem.clickAction = Const.ACTION_REMIND;
        popup.list.addItem(popupItem);

        popupItem = (GUIListItem) popupItem.clone();
        ((GUIText) popupItem.items.elementAt(0)).setText("Exit");
        popupItem.clickAction = Const.ACTION_POPUP;
        popup.list.addItem(popupItem);

        this.popup = popup;

        updateCounts();
    }
}