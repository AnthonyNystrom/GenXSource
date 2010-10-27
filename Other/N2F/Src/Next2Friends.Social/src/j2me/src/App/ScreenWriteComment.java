package App;

import gui.GUIListItem;
import gui.GUIText;
import service.AskComment;

public class ScreenWriteComment extends CommonScreen
{
    public static int currentQuestionId;
    
    public void onShow()
    {
	footer.posText.setText("Menu");
	footer.posAction = Const.ACTION_POPUP;
//#ifdef BLACKBERRY
//#         footer.negText.setText("Back");
//# 	footer.negAction = Const.ACTION_BACK;
//#else
	footer.negText.setText("Clear");
	footer.negAction = Const.ACTION_NULL;
//#endif

	CommonPopup popup = new CommonPopup();
	GUIListItem popupItem = CommonScreen.createPopupItem();

	popupItem = (GUIListItem) popupItem.clone();
	((GUIText) popupItem.items.elementAt(0)).setText("Save to outbox");
	popupItem.clickAction = Const.ACTION_SAVETOOUTBOX;
	popup.list.addItem(popupItem);

//#ifndef BLACKBERRY
	popupItem = (GUIListItem) popupItem.clone();
	((GUIText) popupItem.items.elementAt(0)).setText("Back");
	popupItem.clickAction = Const.ACTION_BACK;
	popup.list.addItem(popupItem);
//#endif
        
	popupItem = (GUIListItem) popupItem.clone();
	((GUIText) popupItem.items.elementAt(0)).setText("Exit");
	popupItem.clickAction = Const.ACTION_POPUP;
	popup.list.addItem(popupItem);

	this.popup = popup;

	GUIText textHeader = new GUIText(Core.smallFont, false, false);
	textHeader.dx = Core.SCREEN_WIDTH;
	textHeader.dy = Core.smallFont.getHeight();
	textHeader.setText("Enter comment:");
	addControl(textHeader);

	GUIText enterText = new GUIText(Core.smallFont, true, true);
	enterText.dx = Core.SCREEN_WIDTH;
	enterText.dy = Core.SCREEN_HEIGHT - header.getHeight() - footer.getHeight() - textHeader.dy;
	enterText.isSelected = true;
	addControl(enterText);
    }

    public void onAction(int action)
    {
	switch(action)
	{
	    case Const.ACTION_SAVETOOUTBOX:
		{
		    GUIText text = (GUIText)items.elementAt(1);
		    String commentString = new String(text.text, 0, text.textLen);
		    
		    Comment comment = new Comment();
		    comment.questionId = currentQuestionId;
		    comment.text = commentString;
		    
		    comment.save();
		    
		    Core.screenManager.backScreen();
		}
		break;
	    default:
		super.onAction(action);
		break;
	}
    }
}
