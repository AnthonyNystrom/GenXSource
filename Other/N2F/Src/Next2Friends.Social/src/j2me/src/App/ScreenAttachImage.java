package App;

import gui.*;
import java.io.IOException;
import java.io.InputStream;
import javax.microedition.lcdui.*;
import javax.microedition.io.file.*;
import javax.microedition.io.*;

public class ScreenAttachImage extends CommonScreen
{

    public GUIList list;
    private boolean isView;
    private Image preview;

    public ScreenAttachImage()
    {
	super();
	list = CommonScreen.createList();
    }

    public void onShow()
    {
	isView = false;
	drawBack = true;
	footer.posText.setText("Menu");
	footer.posAction = Const.ACTION_POPUP;
	footer.negText.setText("Back");
	footer.negAction = Const.ACTION_BACK;

	CommonPopup popup = new CommonPopup();
	GUIListItem popupItem = CommonScreen.createPopupItem();

	((GUIText) popupItem.items.elementAt(0)).setText("Attach from camera");
	popupItem.clickAction = Const.ACTION_SHOWCAPTURE;
	popup.list.addItem(popupItem);

	popupItem = (GUIListItem) popupItem.clone();
	((GUIText) popupItem.items.elementAt(0)).setText("Attach from file");
	popupItem.clickAction = Const.ACTION_SHOWATTACHFROMFILE;
	popup.list.addItem(popupItem);

	popupItem = (GUIListItem) popupItem.clone();
	((GUIText) popupItem.items.elementAt(0)).setText("Delete image");
	popupItem.clickAction = Const.ACTION_DELETECURRENT;
	popup.list.addItem(popupItem);

	popupItem = (GUIListItem) popupItem.clone();
	((GUIText) popupItem.items.elementAt(0)).setText("Save to drafts");
	popupItem.clickAction = Const.ACTION_SAVETODRAFTS;
	popup.list.addItem(popupItem);

	popupItem = (GUIListItem) popupItem.clone();
	((GUIText) popupItem.items.elementAt(0)).setText("Save to outbox");
	popupItem.clickAction = Const.ACTION_SAVETOOUTBOX;
	popup.list.addItem(popupItem);

	popupItem = (GUIListItem) popupItem.clone();
	((GUIText) popupItem.items.elementAt(0)).setText("Send now");
	popupItem.clickAction = Const.ACTION_SEND;
	popup.list.addItem(popupItem);

	popupItem = (GUIListItem) popupItem.clone();
	((GUIText) popupItem.items.elementAt(0)).setText("Exit");
	popupItem.clickAction = Const.ACTION_POPUP;
	popup.list.addItem(popupItem);

	this.popup = popup;
	addControl(list);

	int count = CommonScreen.currentQuestion.photoNames.size();
	for(int i = 0; i < count; ++i)
	{
	    String fileName = (String) CommonScreen.currentQuestion.photoNames.elementAt(i);
//#ifdef BLACKBERRY
//#             //addThumbnail(GUICamera.scale(Core.storage.readImage(fileName), Const.THUMBNAIL_DY), fileName);
//#else
	    addThumbnail(GUICamera.scale(Core.storage.readImage(fileName), Const.THUMBNAIL_DY), fileName);
            //addThumbnail(noImage.image, fileName);
//#endif
	}
    }

    public void addThumbnail(Image img, String fileName)
    {
	GUIListItem item = CommonScreen.createItem();
	item.skin = CommonScreen.itemBigSkin;
	item.dy = item.skin.sprite.spr_MaxHeight;
	GUIImage thumbnail = new GUIImage(img);
	thumbnail.y = 1;
	item.addItem(thumbnail);
	list.addItem(item);
	item.clickAction = Const.ACTION_IMAGEVIEW;//Const.ACTION_NULL;

	GUIText name = new GUIText(Core.smallFont, false, false);
	name.x = Const.COMMON_MARGIN;
	name.dx = item.dx - img.getWidth();
	name.dy = item.dy;
	name.setText(fileName);
	item.selectedColor = 0xffffff;
	item.unselectedColor = 0x424242;
	item.addItem(name);

	if(list.items.size() == 1)
	{
	    list.activeItem = item;
	}
    }

    public void drawBody()
    {
	super.drawBody();

	if(isView && preview != null)
	{
	    Core.staticG.setClip(0, 0, Core.SCREEN_WIDTH, Core.SCREEN_HEIGHT);
	    Core.staticG.drawImage(CommonScreen.blackImage, 0, 0, 0);
	    Core.staticG.drawImage(preview, Core.SCREEN_WIDTH / 2, Core.SCREEN_HEIGHT / 2, Graphics.HCENTER | Graphics.VCENTER);
	}

	if(list.items.size() == 0)
	{
	    Core.staticG.setColor(0x000000);
	    Core.staticG.setFont(Core.smallFont);
	    Core.staticG.drawString("Select \"Attach\" from Menu", Core.SCREEN_WIDTH / 2, Core.SCREEN_HEIGHT / 2, Graphics.HCENTER | Graphics.BASELINE);
	}
    }

    public void update()
    {
	if(isView && Core.keyAction != Const.ACTION_NULL)
	{
	    isView = false;
	    preview = null;
	    Core.keyAction = Const.ACTION_NULL;
	}

	super.update();
    }

    public void onHide()
    {
	save();
	super.onHide();
	preview = null;
	list.items.removeAllElements();
    }

    public void save()
    {
	int count = list.items.size();
	CommonScreen.currentQuestion.photoNames.removeAllElements();
	for(int i = 0; i < count; ++i)
	{
	    GUIText photoName = (GUIText) (((GUIListItem) list.items.elementAt(i)).items.elementAt(1));
	    CommonScreen.currentQuestion.photoNames.addElement(new String(photoName.text, 0, photoName.textLen));
	}
    }

    boolean checkImageCountMin()
    {
	int count = list.items.size();
	if(count > 0)
	{
	    return true;
	}
	else
	{
	    Core.message = "Attach image first";
	    onAction(Const.ACTION_POPUP);
	    return false;
	}
    }
    
    boolean checkImageCountMax()
    {
	int count = list.items.size();
	if(count < 3)
	{
	    return true;
	}
	else
	{
	    Core.message = "Too many images";
	    onAction(Const.ACTION_POPUP);
	    return false;
	}
    }
    
    boolean checkHasQuestion()
    {
	if(!CommonScreen.currentQuestion.question.equals(""))
	{
	    return true;
	}
	else
	{
	    Core.message = "Enter question first";
	    onAction(Const.ACTION_POPUP);
	    return false;
	}
    }

    public void onAction(int action)
    {
	switch(action)
	{
	    case Const.ACTION_IMAGEVIEW:
		if(isView == false)
		{
		    Core.message = "Loading...";
		    Core.forceRepaint();
		    isView = true;
		    GUIText text = (GUIText) (list.activeItem.items.elementAt(1));
		    String fileName = new String(text.text, 0, text.textLen);
                    
//#ifdef BLACKBERRY
//#                     preview = Core.storage.readImage(fileName, 3*Core.SCREEN_HEIGHT/4);
//#else
		    try
		    {
			FileConnection file = null;
    //#ifdef NOKIA                        
    //# 		file = (FileConnection) Connector.open(System.getProperty("fileconn.dir.photos") + fileName);
    //#elifdef MOTOROLA                        
//#                     file = (FileConnection) Connector.open("file:///c/mobile/picture/"+fileName, Connector.READ);
    //#endif                        
			if(!file.exists())
			{
			    file.create();
			}
			InputStream is = file.openInputStream();
			preview = GUICamera.scale(Image.createImage(is), Core.SCREEN_HEIGHT / 2);
			is.close();
			file.close();
		    }
		    catch(Exception ex)
		    {
			ex.printStackTrace();
		    }
		    Core.message = null;
//#endif
		}
		else
		{
		    preview = null;
		    isView = false;
		}
		break;
	    case Const.ACTION_DELETECURRENT:
		if(list.activeItem != null)
		{
		    GUIControl deletedControl = list.activeItem;
		    list.up();
		    list.items.removeElement(deletedControl);
		    onAction(Const.ACTION_POPUP);
		}
		break;
	    case Const.ACTION_SAVETODRAFTS:
		save();
		super.onAction(Const.ACTION_POPUP);
		super.onAction(action);
		break;
	    case Const.ACTION_SAVETOOUTBOX:
		if(checkImageCountMin() && checkHasQuestion())
		{
		    CommonScreen.currentQuestion.sendNow = false;
		    save();
		    super.onAction(Const.ACTION_SAVETOOUTBOX);
		}
		break;
	    case Const.ACTION_SEND:
		if(checkImageCountMin() && checkHasQuestion())
		{
		    CommonScreen.currentQuestion.sendNow = true;
		    save();
		    super.onAction(Const.ACTION_SAVETOOUTBOX);
		}
		break;
	    case Const.ACTION_SHOWATTACHFROMFILE:
	    case Const.ACTION_SHOWCAPTURE:
		if(checkImageCountMax())
		{
		    super.onAction(action);
		}
		break;
	    default:
		super.onAction(action);
		break;
	}
    }
}
