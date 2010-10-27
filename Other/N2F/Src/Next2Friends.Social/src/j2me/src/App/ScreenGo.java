package App;


import gui.GUIListItem;
import gui.GUIText;
import gui.GUIList;

public class ScreenGo extends CommonScreen
{
    public void onShow()
    {
	CommonScreen.currentQuestion = new Question();
	((ScreenAttachImage)Core.attachScreen).list.items.removeAllElements();
	
	footer.posText.setText("Menu");
	footer.posAction = Const.ACTION_POPUP;
	footer.negText.setText("Back");
	footer.negAction = Const.ACTION_BACK;
	
	GUIList golist = CommonScreen.createList();
	
	GUIListItem goitem = CommonScreen.createItem();
	
	GUIText gotext = new GUIText(Core.smallFont, false, false);
	gotext.x = Const.COMMON_MARGIN;
	gotext.dx = golist.dx-2*Const.COMMON_MARGIN;
	gotext.dy = CommonScreen.itemSkin.sprite.spr_FrameHeight;
	gotext.setText("Ask a question");
	goitem.addItem(gotext);
	goitem.clickAction = Const.ACTION_QUESTION;
	golist.addItem(goitem);
	
	GUIListItem goitem2 = (GUIListItem)goitem.clone();
	((GUIText)goitem2.items.elementAt(0)).setText("Tag");
	golist.addItem(goitem2);
	goitem2.clickAction = Const.ACTION_SHOWTAG;
        
        goitem2 = (GUIListItem)goitem.clone();
	((GUIText)goitem2.items.elementAt(0)).setText("SnapUp");
	golist.addItem(goitem2);
	goitem2.clickAction = Const.ACTION_SHOWUPLOAD;
        
        goitem2 = (GUIListItem)goitem.clone();
	((GUIText)goitem2.items.elementAt(0)).setText("Update my Status");
	golist.addItem(goitem2);
	goitem2.clickAction = Const.ACTION_SHOWSTATUS;
	
	addControl(golist);
	
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
    }
    
}
