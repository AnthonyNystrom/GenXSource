package App;

import gui.GUICamera;
import gui.GUIImage;
import gui.GUIListItem;
import gui.GUIText;
import gui.GUIList;
import java.io.IOException;
import javax.microedition.lcdui.Image;
import service.AskComment;
import service.AskResponse;
import service.AskService;

public class ScreenResponse extends CommonScreen
{

    public int currentQuestionId;

    public void onShow()
    {
	ScreenWriteComment.currentQuestionId = currentQuestionId;
	
	Core.isNetwork = true;
	Core.forceRepaint();

	AskService service = new AskService();
	AskResponse currentResponse = null;
	AskComment[] comments = null;
	try
	{
	    currentResponse = service.GetResponse(Core.storage.login, Core.storage.password, currentQuestionId);

	    int[] commentIds = service.GetCommentIDs(Core.storage.login, Core.storage.password, currentQuestionId, 0);
	    int size = commentIds.length;
	    comments = new AskComment[size];
	    for(int i = 0; i < size; ++i)
	    {
		comments[i] = service.GetComment(Core.storage.login, Core.storage.password, commentIds[i]);
	    }
	}
	catch(IOException ex)
	{
	    ex.printStackTrace();
	    onAction(Const.ACTION_BACK);
	    return;
	}
	Core.isNetwork = false;

	footer.posText.setText("Menu");
	footer.posAction = Const.ACTION_POPUP;
	footer.negText.setText("Back");
	footer.negAction = Const.ACTION_BACK;

	CommonPopup popup = new CommonPopup();
	GUIListItem popupItem = CommonScreen.createPopupItem();

	popupItem = (GUIListItem) popupItem.clone();
	((GUIText) popupItem.items.elementAt(0)).setText("Write comment");
	popupItem.clickAction = Const.ACTION_SHOWCOMMENT;
	popup.list.addItem(popupItem);

	popupItem = (GUIListItem) popupItem.clone();
	((GUIText) popupItem.items.elementAt(0)).setText("Exit");
	popupItem.clickAction = Const.ACTION_POPUP;
	popup.list.addItem(popupItem);

	this.popup = popup;
	GUIList list = CommonScreen.createList();
	addControl(list);

	GUIText text = CommonScreen.createText();
	text.setText(currentResponse.question);

	GUIListItem itemQuestion = CommonScreen.createItem();
	itemQuestion.addItem(text);
	list.addItem(itemQuestion);
	GUIListItem item = (GUIListItem) itemQuestion.clone();
	itemQuestion.skin = null;

	Image bigImage = Image.createImage(currentResponse.photobase64binary, 0, currentResponse.photobase64binary.length);
	Image smallImage = GUICamera.scale(bigImage, Core.SCREEN_HEIGHT / 2);
	currentResponse.photobase64binary = null;
	GUIImage imageControl = new GUIImage(smallImage);
	imageControl.x = (Core.SCREEN_WIDTH - imageControl.dx) >> 1;
	imageControl.y = Const.COMMON_MARGIN;

	GUIListItem itemImage = CommonScreen.createItem();
	itemImage.dy = imageControl.dy + (Const.COMMON_MARGIN << 1);
	itemImage.addItem(imageControl);
	itemImage.isSelectable = false;
	list.addItem(itemImage);

	
	item.isSelectable = false;
	switch(currentResponse.responsetype + 1)
	{
	    case Const.QUESTION_RESPONSE_YESNO:
		{
		    item = (GUIListItem) item.clone();
		    ((GUIText) item.items.elementAt(0)).setText("Yes: " + currentResponse.responsevalues[0]);
		    list.addItem(item);

		    item = (GUIListItem) item.clone();
		    ((GUIText) item.items.elementAt(0)).setText("No: " + currentResponse.responsevalues[1]);
		    list.addItem(item);
		}
		break;
	    case Const.QUESTION_RESPONSE_AB:
		{
		    item = (GUIListItem) item.clone();
		    ((GUIText) item.items.elementAt(0)).setText(currentResponse.customresponses[0] + " " + currentResponse.responsevalues[0]);
		    list.addItem(item);

		    item = (GUIListItem) item.clone();
		    ((GUIText) item.items.elementAt(0)).setText(currentResponse.customresponses[1] + " " + currentResponse.responsevalues[1]);
		    list.addItem(item);
		}
		break;
	    case Const.QUESTION_RESPONSE_MULTIPLE:
		{
		    //TODO: ???
		}
		break;
	    case Const.QUESTION_RESPONSE_RATE:
		{
		    //TODO: ???
		}
		break;
	}

	//comments
	item = (GUIListItem) item.clone();
	item.isSelectable = true;
	int size = comments.length;
	for(int i = 0; i < size; ++i)
	{
	    item = (GUIListItem) item.clone();
	    ((GUIText) item.items.elementAt(0)).setText(comments[i].nickname + ": " + comments[i].text);
	    list.addItem(item);
	}

	Core.isNetwork = false;
    }
}
