package App;

import gui.GUICombobox;
import gui.GUIControl;
import gui.GUIListItem;
import gui.GUIText;
import gui.GUIList;
import gui.GUIImage;
import javax.microedition.lcdui.*;

public class ScreenQuOptions extends CommonScreen
{

    public void onShow()
    {
	footer.posText.setText("Menu");
	footer.posAction = Const.ACTION_POPUP;
	footer.negText.setText("Back");
	footer.negAction = Const.ACTION_BACK;
	drawBack = true;

	CommonPopup quOptionsPopup = new CommonPopup();
	GUIListItem popupItem = CommonScreen.createPopupItem();

	((GUIText) popupItem.items.elementAt(0)).setText("Next");
	popupItem.clickAction = Const.ACTION_SHOWATTACH;
	quOptionsPopup.list.addItem(popupItem);
	
	popupItem = (GUIListItem) popupItem.clone();
	((GUIText) popupItem.items.elementAt(0)).setText("Save to drafts");
	popupItem.clickAction = Const.ACTION_SAVETODRAFTS;
	quOptionsPopup.list.addItem(popupItem);

	popupItem = (GUIListItem) popupItem.clone();
	((GUIText) popupItem.items.elementAt(0)).setText("Exit");
	popupItem.clickAction = Const.ACTION_POPUP;
	quOptionsPopup.list.addItem(popupItem);
	popup = quOptionsPopup;

	GUIList optList = CommonScreen.createList();

	GUIListItem item = CommonScreen.createItem();
	GUIText text = new GUIText(Core.smallFont, false, false);
	text.x = Const.COMMON_MARGIN;
	text.dx = optList.dx - 2 * Const.COMMON_MARGIN;
	text.dy = CommonScreen.itemSkin.sprite.spr_FrameHeight;
	text.setText("!");
	item.addItem(text);

	CommonCombobox combobox = new CommonCombobox();
	combobox.skin = CommonScreen.itemSkin;
	combobox.sprite = CommonScreen.combobox;
	combobox.dx = item.dx;
	combobox.dy = item.dy;
	combobox.selectedColor = 0xffffff;
	combobox.unselectedColor = 0x000000;
	combobox.hideFirst = true;

	GUIText combotext = new GUIText(Core.smallFont, false, false);
	combotext.dx = combobox.dx;
	combotext.dy = combobox.dy;
	combotext.setText("Choose response");
	combotext.anchor = Graphics.TOP | Graphics.HCENTER;
	combobox.addControl(combotext, Const.ACTION_HIDEAB);

	combotext = (GUIText) combotext.clone();
	combotext.setText("Yes/No");
	combobox.addControl(combotext, Const.ACTION_HIDEAB);

	combotext = (GUIText) combotext.clone();
	combotext.setText("A/B");
	combobox.addControl(combotext, Const.ACTION_SHOWAB);

	combotext = (GUIText) combotext.clone();
	combotext.setText("Rate");
	combobox.addControl(combotext, Const.ACTION_HIDEAB);

	combotext = (GUIText) combotext.clone();
	combotext.setText("Image select");
	combobox.addControl(combotext, Const.ACTION_HIDEAB);

	int responseType = CommonScreen.currentQuestion.responseType;
	while(responseType > 0)
	{
	    combobox.right();
	    responseType--;
	}

	GUIListItem item2 = (GUIListItem) item.clone();
	item2.skin = null;
	item2.items.removeAllElements();
	item2.addItem(combobox);
	item2.isCheckbox = false;
	item2.isSelectable = true;
	item2.clickAction = Const.ACTION_NULL;
	optList.addItem(item2);
	optList.down();

	GUIListItem editItem = (GUIListItem) item.clone();
	editItem.isSelectable = true;
	((GUIText) editItem.items.elementAt(0)).setText("A:");
	int aWidth = Core.smallFont.stringWidth("A:");
	((GUIText) editItem.items.elementAt(0)).dx = aWidth;
	CommonEdit edit = new CommonEdit();
	edit.dx = optList.dx - aWidth - 2 * Const.COMMON_MARGIN - ((GUIText) editItem.items.elementAt(0)).x;
	edit.x = aWidth;
	edit.dy = CommonScreen.itemSkin.sprite.spr_FrameHeight;
	edit.setText(CommonScreen.currentQuestion.responseA);
	editItem.addItem(edit);
	editItem.isVisible = false;
	optList.addItem(editItem);

	editItem = (GUIListItem) editItem.clone();
	((GUIText) editItem.items.elementAt(0)).setText("B:");
	((GUIText) editItem.items.elementAt(1)).setText(CommonScreen.currentQuestion.responseB);
	optList.addItem(editItem);

	item2 = (GUIListItem) item2.clone();
	item2.items.removeAllElements();
	combobox = (CommonCombobox) combobox.clone();
	combobox.items.removeAllElements();
	combobox.activeItem = null;

	combotext = (GUIText) combotext.clone();
	combotext.setText("Choose duration");
	combobox.addControl(combotext);
	combotext = (GUIText) combotext.clone();
	combotext.setText("3 minutes");
	combobox.addControl(combotext);
	combotext = (GUIText) combotext.clone();
	combotext.setText("15 minutes");
	combobox.addControl(combotext);
	combotext = (GUIText) combotext.clone();
	combotext.setText("1 hour");
	combobox.addControl(combotext);
	combotext = (GUIText) combotext.clone();
	combotext.setText("1 day");
	combobox.addControl(combotext);
	item2.addItem(combobox);

	int duration = CommonScreen.currentQuestion.duration;
	while(duration > 0)
	{
	    combobox.right();
	    duration--;
	}

	optList.addItem(item2);

	item = (GUIListItem) item.clone();
	item.items.removeAllElements();
	item.addItem(checkIcon);
	combotext = (GUIText) combotext.clone();
	combotext.setText("Private question");
	combotext.anchor = Graphics.TOP | Graphics.LEFT;
	item.addItem(combotext);
	item.isCheckbox = true;
	item.isSelectable = true;
	item.isChecked = CommonScreen.currentQuestion.isPrivate;
	optList.addItem(item);
	addControl(optList);
    }

    public void onHide()
    {
	save();

	super.onHide();
    }

    public void save()
    {
	GUIText text = ((GUIText) ((GUIListItem) ((GUIList) items.elementAt(0)).items.elementAt(1)).items.elementAt(1));
	CommonScreen.currentQuestion.responseA = new String(text.text, 0, text.textLen);
	text = ((GUIText) ((GUIListItem) ((GUIList) items.elementAt(0)).items.elementAt(2)).items.elementAt(1));
	CommonScreen.currentQuestion.responseB = new String(text.text, 0, text.textLen);

	GUICombobox combobox = (GUICombobox) ((GUIListItem) ((GUIList) items.elementAt(0)).items.elementAt(0)).items.elementAt(0);
	int index = combobox.items.indexOf(combobox.activeItem);
//	if(combobox.items.size() < Const.QUESTION_RESPONSE_MULTIPLE + 1)
//	{
//	    index++;
//	}
	CommonScreen.currentQuestion.responseType = index;

	combobox = (GUICombobox) ((GUIListItem) ((GUIList) items.elementAt(0)).items.elementAt(3)).items.elementAt(0);
	index = combobox.items.indexOf(combobox.activeItem);
//	if(combobox.items.size() < Const.QUESTION_DURATION_1D + 1)
//	{
//	    index++;
//	}
	CommonScreen.currentQuestion.duration = index;

	CommonScreen.currentQuestion.isPrivate = ((GUIListItem) ((GUIList) items.elementAt(0)).items.elementAt(4)).isChecked;
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
