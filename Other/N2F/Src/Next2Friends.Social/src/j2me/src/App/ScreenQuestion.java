package App;

import gui.GUIListItem;
import gui.GUIText;

public class ScreenQuestion extends CommonScreen
{
    private GUIText enterText;

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
	drawBack = true;

	CommonPopup questionPopup = new CommonPopup();
	GUIListItem popupItem = CommonScreen.createPopupItem();

	((GUIText) popupItem.items.elementAt(0)).setText("Next");
	popupItem.clickAction = Const.ACTION_QUOPTIONS;
	questionPopup.list.addItem(popupItem);

	popupItem = (GUIListItem) popupItem.clone();
	((GUIText) popupItem.items.elementAt(0)).setText("Save to drafts");
	popupItem.clickAction = Const.ACTION_SAVETODRAFTS;
	questionPopup.list.addItem(popupItem);

//#ifndef BLACKBERRY
	popupItem = (GUIListItem) popupItem.clone();
	((GUIText) popupItem.items.elementAt(0)).setText("Back");
	popupItem.clickAction = Const.ACTION_BACK;
	questionPopup.list.addItem(popupItem);
//#endif
        
	popupItem = (GUIListItem) popupItem.clone();
	((GUIText) popupItem.items.elementAt(0)).setText("Exit");
	popupItem.clickAction = Const.ACTION_POPUP;
	questionPopup.list.addItem(popupItem);
	popup = questionPopup;

	GUIText textHeader = new GUIText(Core.smallFont, false, false);
	textHeader.dx = Core.SCREEN_WIDTH;
	textHeader.dy = Core.smallFont.getHeight();
	textHeader.setText("Enter your question");
	addControl(textHeader);

	enterText = new GUIText(Core.smallFont, true, true);
	enterText.dx = Core.SCREEN_WIDTH;
	enterText.dy = Core.SCREEN_HEIGHT - header.getHeight() - footer.getHeight() - textHeader.dy;
	enterText.isSelected = true;
	addControl(enterText);

	enterText.setText(CommonScreen.currentQuestion.question);
    }

    public void onHide()
    {
	save();

	super.onHide();
    }

    private void save()
    {
	CommonScreen.currentQuestion.question = new String(enterText.text, 0, enterText.textLen);
    }

    public void onAction(int action)
    {
	switch(action)
	{
	    case Const.ACTION_SAVETODRAFTS:
		save();
		super.onAction(Const.ACTION_POPUP);
		super.onAction(action);
		break;
	    default:
		super.onAction(action);
		break;
	}
    }
}
