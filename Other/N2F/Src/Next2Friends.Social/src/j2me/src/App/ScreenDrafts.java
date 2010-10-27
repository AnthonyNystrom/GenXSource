package App;

import gui.GUIControl;
import gui.GUIListItem;
import gui.GUIText;
import gui.GUIList;
import gui.GUIImage;

public class ScreenDrafts extends CommonScreen
{

    public void onShow()
    {
	footer.posText.setText("Menu");
	footer.posAction = Const.ACTION_POPUP;
	footer.negText.setText("Back");
	footer.negAction = Const.ACTION_BACK;

	CommonPopup popup = new CommonPopup();
	GUIListItem popupItem = CommonScreen.createPopupItem();

	((GUIText) popupItem.items.elementAt(0)).setText("Edit");
	popupItem.clickAction = Const.ACTION_EDIT;
	popup.list.addItem(popupItem);

	popupItem = (GUIListItem) popupItem.clone();
	((GUIText) popupItem.items.elementAt(0)).setText("Delete");
	popupItem.clickAction = Const.ACTION_DELETECURRENT;
	popup.list.addItem(popupItem);

	popupItem = (GUIListItem) popupItem.clone();
	((GUIText) popupItem.items.elementAt(0)).setText("Exit");
	popupItem.clickAction = Const.ACTION_POPUP;
	popup.list.addItem(popupItem);

	this.popup = popup;

	GUIList list = CommonScreen.createList();
	addControl(list);

	GUIListItem item = CommonScreen.createItem();
	GUIText text = CommonScreen.createText();
	item.addItem(text);
	item.clickAction = Const.ACTION_EDIT;
	String[] headers = Question.getQuestionHeaders("drafts");
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
	    case Const.ACTION_EDIT:
		super.onAction(Const.ACTION_QUESTION);
		CommonScreen.currentQuestion = new Question();
		CommonScreen.currentQuestion.read(true, ((GUIList) items.elementAt(0)).items.indexOf(((GUIList) items.elementAt(0)).activeItem));
		break;
	    case Const.ACTION_DELETECURRENT:
		{
		    GUIList list = (GUIList) items.elementAt(0);
		    if(list.activeItem != null)
		    {
			GUIControl deletedControl = list.activeItem;
			int recordId = Core.storage.getRecordId("drafts", list.items.indexOf(deletedControl));
			Core.storage.deleteData("drafts", recordId);
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
