package App;

import gui.GUIControl;
import gui.GUIListItem;
import gui.GUIText;
import gui.GUIList;
import gui.GUIImage;

public class ScreenOutbox extends CommonScreen
{

    public void onShow()
    {
	footer.posText.setText("");
	footer.posAction = Const.ACTION_NULL;
	footer.negText.setText("Back");
	footer.negAction = Const.ACTION_BACK;

//	CommonPopup popup = new CommonPopup();
//	GUIListItem popupItem = CommonScreen.createPopupItem();
//
//	popupItem = (GUIListItem) popupItem.clone();
//	((GUIText) popupItem.items.elementAt(0)).setText("Delete");
//	popupItem.clickAction = Const.ACTION_DELETECURRENT;
//	popup.list.addItem(popupItem);
//
//	popupItem = (GUIListItem) popupItem.clone();
//	((GUIText) popupItem.items.elementAt(0)).setText("Exit");
//	popupItem.clickAction = Const.ACTION_POPUP;
//	popup.list.addItem(popupItem);
//
//	this.popup = popup;

	GUIList list = CommonScreen.createList();
	addControl(list);

	GUIListItem item = CommonScreen.createItem();
	GUIText text = CommonScreen.createText();
	item.addItem(text);
	String[] headers = Question.getQuestionHeaders("outbox");
	for(int i = 0; i < headers.length; ++i)
	{
	    item = (GUIListItem) item.clone();
	    ((GUIText) item.items.elementAt(0)).setText(headers[i]);
	    list.addItem(item);
	}
	
	headers = Comment.getCommentsHeaders();
	for(int i = 0; i < headers.length; ++i)
	{
	    item = (GUIListItem) item.clone();
	    ((GUIText) item.items.elementAt(0)).setText(headers[i]);
	    list.addItem(item);
	}
        
        headers = UploadedImage.getHeaders();
	for(int i = 0; i < headers.length; ++i)
	{
	    item = (GUIListItem) item.clone();
	    ((GUIText) item.items.elementAt(0)).setText(headers[i]);
	    list.addItem(item);
	}
    }

    public void onAction(int action)
    {
	switch(action)
	{
	    case Const.ACTION_DELETECURRENT:
		{
		    GUIList list = (GUIList) items.elementAt(0);
		    if(list.activeItem != null)
		    {
			GUIControl deletedControl = list.activeItem;
			int recordId = Core.storage.getRecordId("outbox", list.items.indexOf(deletedControl));
			Core.storage.deleteData("outbox", recordId);
			list.up();
			list.items.removeElement(deletedControl);
			onAction(Const.ACTION_POPUP);
		    }
		}
		break;
	    default:
		super.onAction(action);
		break;
	}
    }
}
