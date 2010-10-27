package App;

import gui.GUIListItem;
import gui.GUIText;
import gui.GUICamera;
import javax.microedition.lcdui.*;

//#ifdef BLACKBERRY
//# import net.rim.blackberry.api.invoke.Invoke;
//# import net.rim.device.api.io.file.*;
//# import net.rim.device.api.system.Application;
//# 
//# public class ScreenCapture extends CommonScreen
//# {
//#     boolean hasCaptured;
//#     static public boolean isUpload;
//# 
//#     public ScreenCapture()
//#     {
//#         super();
//#         
//#         hasCaptured = false;
//#     }
//# 
//#     public void onShow()
//#     {
//#         Invoke.invokeApplication(Invoke.APP_TYPE_CAMERA, null);
//#     }
//# 
//#     public void onHide()
//#     {
//#         super.onHide();
//# 
//#         Application.getApplication().requestForeground();
//#     }
//#     
//#     public void render()
//#     {
//#         hasCaptured = true;
//#     }
//# 
//#     public void update()
//#     {
//#         super.update();
//# 
//#         long nextUSN = FileSystemJournal.getNextUSN();
//#         hasCaptured = false;
//#         String path = null;
//#         while (!hasCaptured)
//#         {
//# 
//#             FileSystemJournalEntry entry = FileSystemJournal.getEntry(nextUSN);
//# 
//#             if (entry == null)
//#             {
//#                 try
//#                 {
//#                     Thread.sleep(100);
//#                 } catch (Exception ex)
//#                 {
//#                 }
//#                 continue;
//# 
//#             }            //if (entry.getEvent() == FileSystemJournalEntry.FILE_ADDED)
//#             {
//#                 int event = entry.getEvent();
//#                 path = entry.getPath();
//#                 System.out.println("path=" + path);
//#                 hasCaptured = true;
//#             }
//#         }
//#         if (path != null)
//#         {
//#             int index = path.lastIndexOf('/') + 1;
//#             String showPath = path.substring(index, path.length());
//#                 if(isUpload)
//#                 {
//#                     UploadedImage newImage = new UploadedImage();
//#                     newImage.photoName = showPath;
//#                     newImage.save();
//#                     ScreenMainmenu.outboxCount++;
//#                     Core.screenManager.screenStack.removeAllElements();
//#                     Core.screenManager.setScreen(Core.mainMenu);
//#                     return;
//#                 }
//#             Image img = Core.storage.readImage(showPath, Const.THUMBNAIL_DY);
//#             ((ScreenAttachImage) Core.attachScreen).addThumbnail(img, showPath);
//#         }
//#         super.onAction(Const.ACTION_BACK);
//#     }
//# }
//# 
//#else

public class ScreenCapture extends CommonScreen
{
    public GUICamera camera;
    Image image;
    static public boolean isUpload;

    public void onShow()
    {
        drawBack = false;
        if (isUpload)
        {
            footer.posText.setText("Upload");
            footer.posAction = Const.ACTION_UPLOAD;
        } else
        {
            footer.posText.setText("Capture");
            footer.posAction = Const.ACTION_CAPTURE;
        }
        footer.negText.setText("Back");
        footer.negAction = Const.ACTION_BACK;

//	CommonPopup popup = new CommonPopup();
//	GUIListItem popupItem = CommonScreen.createPopupItem();
//	
//	((GUIText)popupItem.items.elementAt(0)).setText("Capture");
//	popupItem.clickAction = Const.ACTION_CAPTURE;
//	popup.list.addItem(popupItem);
//	
//	popupItem = (GUIListItem)popupItem.clone();
//	((GUIText)popupItem.items.elementAt(0)).setText("Exit");
//	popupItem.clickAction = Const.ACTION_POPUP;
//	popup.list.addItem(popupItem);
//	
//	this.popup = popup;

        camera = new GUICamera();
        camera.y = header.getHeight();
        camera.dx = Core.SCREEN_WIDTH;
        camera.dy = Core.SCREEN_HEIGHT - header.getHeight() - footer.getHeight();
        camera.init();

        Core.staticG.setClip(0, 0, Core.SCREEN_WIDTH, Core.SCREEN_HEIGHT);
        Core.staticG.setColor(0xaaaaaa);
        Core.staticG.fillRect(0, 0, Core.SCREEN_WIDTH, Core.SCREEN_HEIGHT);
        Core.forceRepaint();
    }

    public void onHide()
    {
        super.onHide();
        camera.release();
        camera = null;
        image = null;
    }

    public void drawBack(int x, int y)
    {
    }

    public void drawBody()
    {
        if (image != null)
        {
            Core.staticG.drawImage(image, offsetX, offsetY, 0);
        }
    }

    public void update()
    {
        super.update();

        if (Core.keyAction == Const.KEY_OK)
        {
            onAction(Const.ACTION_CAPTURE);
        }
    }

    public void onAction(int action)
    {
        switch (action)
        {
            case Const.ACTION_POPUP:
                if (popup.isActive)
                {
                    image = null;
                    camera.init();
                } else
                {
                    byte buf[] = camera.getSnapShot();
                    image = Image.createImage(buf, 0, buf.length);
                    camera.release();
                }
                break;
            case Const.ACTION_UPLOAD:
                {
                    byte buf[] = null;
                    if (image == null)
                    {
                        buf = camera.getSnapShot();
                        image = Image.createImage(buf, 0, buf.length);
                    }
                    String fileName = Core.storage.writeImage(buf);

                    UploadedImage newImage = new UploadedImage();
                    newImage.photoName = fileName;
                    newImage.save();
                    ScreenMainmenu.outboxCount++;
                    Core.screenManager.screenStack.removeAllElements();
                    Core.screenManager.setScreen(Core.mainMenu);
                }
                break;
            case Const.ACTION_CAPTURE:
                {
                    Image thumb;
                    byte buf[] = null;
                    if (image == null)
                    {
                        buf = camera.getSnapShot();
                        image = Image.createImage(buf, 0, buf.length);
                    }

                    String fileName = Core.storage.writeImage(buf);

                    thumb = camera.scale(image, Const.THUMBNAIL_DY);

                    ((ScreenAttachImage) Core.attachScreen).addThumbnail(thumb, fileName);
                    super.onAction(Const.ACTION_BACK);
                }
                break;
            default:
                super.onAction(action);
                break;
        }
    }
}

//#endif
