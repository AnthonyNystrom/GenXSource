package App;

import gui.*;
import javax.microedition.lcdui.*;
import java.util.*;
import javax.microedition.io.file.*;
import javax.microedition.io.*;
import java.io.InputStream;

public class ScreenAttachFromFile extends CommonScreen implements Runnable
{
    private boolean isView;
    private Image preview;
    private int oldFirstVisible;
    public boolean isActive;
    private Thread thread;
    static public boolean isUpload;

    public ScreenAttachFromFile()
    {
        super();
        oldFirstVisible = -1;
        isActive = false;
    }

    public void run()
    {
        while (isActive)
        {
            updateThumbs();
        }
    }

    public void updateThumbs()
    {
        GUIList list = (GUIList) items.elementAt(0);
        if ((list.firstVisible != oldFirstVisible) && (list.scrollOffset == list.needScrollOffset))
        {
            int count = list.items.size();
            for (int i = 0; i < count; ++i)
            {
                GUIListItem item = ((GUIListItem) list.items.elementAt(i));
                if ((i < list.firstVisible) || (i > list.firstVisible + list.visibleCount + 2))
                {
                    if (((GUIImage) item.items.elementAt(0)).image != noImage.image)
                    {
                        item.items.setElementAt(noImage, 0);
                        return;
                    }
                } else
                {
                    if (((GUIImage) item.items.elementAt(0)).image == noImage.image)
                    {
                        GUIText text = ((GUIText) item.items.elementAt(1));
                        String fileName = new String(text.text, 0, text.textLen);
//#ifdef BLACKBERRY
//#                         GUIImage thumb = new GUIImage(Core.storage.readImage(fileName, Const.THUMBNAIL_DY));
//#else
                        GUIImage thumb = new GUIImage(GUICamera.scale(Core.storage.readImage(fileName), Const.THUMBNAIL_DY));
//#endif
                        item.items.setElementAt(thumb, 0);
                        return;
                    }
                }
                if (i == count - 1)
                {
                    oldFirstVisible = list.firstVisible;
                }
            }
        }
    }

    public void update()
    {
        super.update();
        if (isView && Core.keyAction != Const.ACTION_NULL)
        {
            isView = false;
            preview = null;
            Core.keyAction = Const.ACTION_NULL;
        }
    }

    public void drawBody()
    {
        super.drawBody();

        if (isView && preview != null)
        {
            Core.staticG.setClip(0, 0, Core.SCREEN_WIDTH, Core.SCREEN_HEIGHT);
            Core.staticG.drawImage(CommonScreen.blackImage, 0, 0, 0);
            Core.staticG.drawImage(preview, Core.SCREEN_WIDTH / 2, Core.SCREEN_HEIGHT / 2, Graphics.HCENTER | Graphics.VCENTER);
        }

        if (((GUIList) items.elementAt(0)).items.size() == 0)
        {
            Core.staticG.setColor(0x000000);
            Core.staticG.setFont(Core.smallFont);
            Core.staticG.drawString("No photos", Core.SCREEN_WIDTH / 2, Core.SCREEN_HEIGHT / 2, Graphics.HCENTER | Graphics.BASELINE);
        }
    }

    public void onShow()
    {
        GUIList list = CommonScreen.createList();
        addControl(list);
        GUIListItem item = CommonScreen.createItem();
        item.skin = CommonScreen.itemBigSkin;
        item.dy = item.skin.sprite.spr_MaxHeight;
        item.clickAction = /*Const.ACTION_NULL;*/ Const.ACTION_IMAGEVIEW;
        item.addItem(noImage);
        GUIText name = new GUIText(Core.smallFont, false, false);
        name.x = Const.COMMON_MARGIN;
        name.dx = item.dx;
        name.dy = item.dy;
        name.setText("");
        item.selectedColor = 0xffffff;
        item.unselectedColor = 0x424242;
        item.addItem(name);

        for (Enumeration e = Core.storage.getImagesList(); e.hasMoreElements();)
        {
            item = (GUIListItem) item.clone();
            String fileName = (String) e.nextElement();

            ((GUIText) item.items.elementAt(1)).setText(fileName);
            list.addItem(item);
        }

        isView = false;
        drawBack = true;
        footer.posText.setText("Menu");
        footer.posAction = Const.ACTION_POPUP;
        footer.negText.setText("Back");
        footer.negAction = Const.ACTION_BACK;

        CommonPopup popup = new CommonPopup();
        GUIListItem popupItem = CommonScreen.createPopupItem();

        if (isUpload)
        {
            ((GUIText) popupItem.items.elementAt(0)).setText("Upload");
            popupItem.clickAction = Const.ACTION_UPLOAD;
            popup.list.addItem(popupItem);
        } else
        {
            ((GUIText) popupItem.items.elementAt(0)).setText("Attach");
            popupItem.clickAction = Const.ACTION_CAPTURE;
            popup.list.addItem(popupItem);
        }
        popupItem = (GUIListItem) popupItem.clone();
        ((GUIText) popupItem.items.elementAt(0)).setText("Exit");
        popupItem.clickAction = Const.ACTION_POPUP;
        popup.list.addItem(popupItem);

        this.popup = popup;

        thread = new Thread(this);
        thread.start();
        thread.setPriority(Thread.MIN_PRIORITY);
        isActive = true;
    }

    public void onHide()
    {
        super.onHide();
        preview = null;
        isActive = false;
        try
        {
            thread.join();
        } catch (InterruptedException ex)
        {
            ex.printStackTrace();
        }
    }

    public void onAction(int action)
    {
        switch (action)
        {
            case Const.ACTION_UPLOAD:
                {
                    UploadedImage newImage = new UploadedImage();
                    GUIList list = (GUIList) items.elementAt(0);
                    if (list.items.size() > 0)
                    {
                        GUIText text = (GUIText) (list.activeItem.items.elementAt(1));
                        String fileName = new String(text.text, 0, text.textLen);
                        newImage.photoName = fileName;
                        newImage.save();
                        ScreenMainmenu.outboxCount++;
                        Core.screenManager.screenStack.removeAllElements();
                        Core.screenManager.setScreen(Core.mainMenu);
                    }
                }
                break;
            case Const.ACTION_CAPTURE:
                {
                    GUIList list = (GUIList) items.elementAt(0);
                    if (list.items.size() > 0)
                    {
                        GUIText text = (GUIText) (list.activeItem.items.elementAt(1));
                        String fileName = new String(text.text, 0, text.textLen);
                        Image thumb = null;

//#ifdef BLACKBERRY
//#                         Image img = Core.storage.readImage(fileName, Const.THUMBNAIL_DY);
//#                         ((ScreenAttachImage) Core.attachScreen).addThumbnail(img, fileName);
//#else
                        try
                        {
                            FileConnection file = null;
                            //#ifdef NOKIA                            
//#                             file = (FileConnection) Connector.open(System.getProperty("fileconn.dir.photos") + fileName);
                            //#elifdef MOTOROLA                            
//#                         file = (FileConnection) Connector.open("file:///c/mobile/picture/"+fileName, Connector.READ);
                            //#endif                            
                            if (!file.exists())
                            {
                                file.create();
                            }
                            InputStream is = file.openInputStream();
                            thumb = GUICamera.scale(Image.createImage(is), Const.THUMBNAIL_DY);
                            is.close();
                            file.close();
                        } catch (Exception ex)
                        {
                            ex.printStackTrace();
                        }
                        //                        Image thumb = noImage.image;
                        ((ScreenAttachImage) Core.attachScreen).addThumbnail(thumb, fileName);
//#endif                        
                        super.onAction(Const.ACTION_BACK);
                    }
                }
                break;
            case Const.ACTION_IMAGEVIEW:
                if (isView == false)
                {
                    isActive = false;
                    try
                    {
                        thread.join();
                    } catch (InterruptedException ex)
                    {
                        ex.printStackTrace();
                    }

                    Core.message = "Loading...";
                    Core.forceRepaint();
                    isView = true;
                    GUIList list = (GUIList) items.elementAt(0);
                    GUIText text = (GUIText) (list.activeItem.items.elementAt(1));
                    String fileName = new String(text.text, 0, text.textLen);

                    try
                    {
//#ifdef BLACKBERRY
//#                         preview = Core.storage.readImage(fileName, 3 * Core.SCREEN_HEIGHT / 4);
//#else
                        FileConnection file = null;
                        //#ifdef NOKIA                        
//#                         file = (FileConnection) Connector.open(System.getProperty("fileconn.dir.photos") + fileName);
                        //#elifdef MOTOROLA                         
//#                     file = (FileConnection) Connector.open("file:///c/mobile/picture/"+fileName, Connector.READ);
                        //#endif                        
                        if (!file.exists())
                        {
                            file.create();
                        }
                        InputStream is = file.openInputStream();
                        preview = GUICamera.scale(Image.createImage(is), Core.SCREEN_HEIGHT / 2);
                        is.close();
                        file.close();
//#endif
                        Core.keyAction = Const.ACTION_NULL;
                    } catch (Exception ex)
                    {
                        ex.printStackTrace();
                    }
                    Core.message = null;
                } else
                {
                    preview = null;
                    isView = false;
                    thread = new Thread(this);
                    thread.start();
                    thread.setPriority(Thread.MIN_PRIORITY);
                    isActive = true;
                }
                break;
            default:
                super.onAction(action);
        }
    }
}
