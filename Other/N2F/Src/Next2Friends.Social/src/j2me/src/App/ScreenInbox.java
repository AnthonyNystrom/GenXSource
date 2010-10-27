package App;

import gui.GUIControl;
import gui.GUIListItem;
import gui.GUIText;
import gui.GUIList;
import java.io.IOException;
import java.util.Vector;
import service.AskQuestion;
import service.AskResponse;
import service.AskService;

public class ScreenInbox extends CommonScreen
{

    public GUIList list;
    private Vector questionIds;

    public ScreenInbox()
    {
	super();
	questionIds = new Vector();
	list = CommonScreen.createList();
    }

    public void onShow()
    {
	footer.posText.setText("Menu");
	footer.posAction = Const.ACTION_POPUP;
	footer.negText.setText("Back");
	footer.negAction = Const.ACTION_BACK;

	CommonPopup popup = new CommonPopup();
	GUIListItem popupItem = CommonScreen.createPopupItem();

	popupItem = (GUIListItem) popupItem.clone();
	((GUIText) popupItem.items.elementAt(0)).setText("View");
	popupItem.clickAction = Const.ACTION_VIEW;
	popup.list.addItem(popupItem);

	popupItem = (GUIListItem) popupItem.clone();
	((GUIText) popupItem.items.elementAt(0)).setText("Exit");
	popupItem.clickAction = Const.ACTION_POPUP;
	popup.list.addItem(popupItem);

	this.popup = popup;
	addControl(list);
    }

    public void addQuestion(String question, int id)
    {
	GUIListItem item = CommonScreen.createItem();
	GUIText text = CommonScreen.createText();
	item.addItem(text);
	item.clickAction = Const.ACTION_VIEW;
	list.addItem(item);

	String shortQuestion = question.substring(0, Math.min(question.length(), 15));
	text.setText(shortQuestion);

	questionIds.addElement(new Integer(id));
    }

    public void onAction(int action)
    {
	switch(action)
	{
	    case Const.ACTION_VIEW:
		{
		    int questionId = ((Integer) questionIds.elementAt(list.items.indexOf(list.activeItem))).intValue();
		    ((ScreenResponse) Core.response).currentQuestionId = questionId;
		    Core.screenManager.setScreen(Core.response);
		}
		break;
	    default:
		super.onAction(action);
		break;
	}
    }
}
