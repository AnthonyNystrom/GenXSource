package App;

import gui.GUIListItem;
import gui.GUIHeader;
import gui.GUIFooter;
import gui.GUIText;
import gui.GUIList;
import gui.GUIScreen;
import gui.GUISkin;
import gui.GUIImage;
import javax.microedition.io.ConnectionNotFoundException;
import javax.microedition.lcdui.*;

public class CommonScreen extends GUIScreen implements Clonable
{
    static public GUIHeader commonHeader = new CommonHeader();
    static public GUIFooter commonFooter = new GUIFooter(Core.bigFont);
    static private GUIImage back = new GUIImage(Const.GUIIMAGE_IMAGE, "/back.png");
    static public GUISkin footerSkin = new GUISkin("/footer", true, Const.SKIN_TYPE_3H);
    static public GUIImage combobox = new GUIImage(Const.GUIIMAGE_SPRITE, "/combobox");
    static public GUISkin itemSkin = new GUISkin("/item", false, Const.SKIN_TYPE_1H);
    static public GUISkin itemBigSkin = new GUISkin("/itembig", false, Const.SKIN_TYPE_1H);
    static public GUIImage shift = new GUIImage(Const.GUIIMAGE_SPRITE, "/shift");
    static public GUIImage checkIcon = new GUIImage(Const.GUIIMAGE_SPRITE, "/checkbox");
    static public GUIImage noImage = new GUIImage(Const.GUIIMAGE_IMAGE, "/noimage.png");
    public boolean drawBack;
    static public Image blackImage;
    public static Question currentQuestion;

    public CommonScreen()
    {
        super();
        header = commonHeader;
        footer = commonFooter;
        footer.skin = footerSkin;
        footer.fontColor = 0xffffff;
        drawBack = true;

        if (blackImage == null)
        {
            int[] black;
            black = new int[Core.SCREEN_WIDTH * Core.SCREEN_HEIGHT];
            int pixel = 0xaa000000;
            for (int i = 0; i < Core.SCREEN_WIDTH * Core.SCREEN_HEIGHT; ++i)
            {
                black[i] = pixel;
            }
            blackImage = Image.createRGBImage(black, Core.SCREEN_WIDTH, Core.SCREEN_HEIGHT, true);
            black = null;
        }
    }

    public Object clone()
    {
        CommonScreen copy = new CommonScreen();
        return copy(copy);
    }

    public Object copy(Object from)
    {
        CommonScreen copy = (CommonScreen) from;
        copy.drawBack = drawBack;

        return super.copy(copy);
    }

    public void onHide()
    {
        items.removeAllElements();
        popup = null;
    }

    public void drawHeader()
    {
        super.drawHeader();

        if (GUIText.isCapital)
        {
            Core.staticG.setClip(0, 0, Core.SCREEN_WIDTH, header.skin.sprite.spr_FrameHeight);
            shift.render(Core.SCREEN_WIDTH - shift.sprite.spr_FrameWidth, 0);
        }
    }

    public void onAction(int action)
    {
        switch (action)
        {
            case Const.ACTION_GO:
                Core.screenManager.setScreen(Core.goScreen);
                break;
            case Const.ACTION_EXIT:
                Core.isRunning = false;
                break;
            case Const.ACTION_POPUP:
                if (popup != null)
                {
                    popup.toggle();
                }
                break;
            case Const.ACTION_SELECT:
                if (popup != null)
                {
                    popup.toggle();
                }
                onAction(((GUIList) items.elementAt(0)).activeItem.clickAction);
                break;
            case Const.ACTION_BACK:
                Core.screenManager.backScreen();
                break;
            case Const.ACTION_HOMEURL:
                try
                {
                    App.instance.platformRequest("http://www.next2friends.com");
                } catch (ConnectionNotFoundException ex)
                {
                }
                Core.isRunning = false;
                break;
            case Const.ACTION_QUESTION:
                Core.screenManager.setScreen(Core.questionScreen);
                break;
            case Const.ACTION_SETTINGS:
                Core.screenManager.setScreen(Core.settingsScreen);
                break;

            case Const.ACTION_MAINMENU:
                Core.screenManager.setScreen(Core.mainMenu);
                break;
            case Const.ACTION_SHOWCAPTURE:
                ScreenCapture.isUpload = false;
                Core.screenManager.setScreen(Core.captureScreen);
                break;
            case Const.ACTION_SHOWUPLOAD_CAPTURE:
                ScreenCapture.isUpload = true;
                Core.screenManager.setScreen(Core.captureScreen);
                break;
            case Const.ACTION_SHOWATTACH:
                Core.screenManager.setScreen(Core.attachScreen);
                break;
            case Const.ACTION_QUOPTIONS:
                Core.screenManager.setScreen(Core.quOptionsScreen);
                break;
            case Const.ACTION_REMIND:
                Core.screenManager.setScreen(Core.remind);
                break;
            case Const.ACTION_SHOWATTACHFROMFILE:
                ScreenAttachFromFile.isUpload = false;
                Core.screenManager.setScreen(Core.attachFromFile);
                break;
            case Const.ACTION_SHOWUPLOAD_ATTACHFROMFILE:
                ScreenAttachFromFile.isUpload = true;
                Core.screenManager.setScreen(Core.attachFromFile);
                break;
            case Const.ACTION_SHOWUPLOAD:
                Core.screenManager.setScreen(Core.upload);
                break;
            case Const.ACTION_DRAFTS:
                Core.screenManager.setScreen(Core.drafts);
                break;
            case Const.ACTION_OUTBOX:
                Core.screenManager.setScreen(Core.outbox);
                break;
            case Const.ACTION_DASHBOARD:
                Core.screenManager.setScreen(Core.dashboard);
                break;
            case Const.ACTION_VIEW:
                Core.screenManager.setScreen(Core.view);
                break;
            case Const.ACTION_SHOWTAG:
                Core.screenManager.setScreen(Core.tag);
                break;
            case Const.ACTION_SHOWINBOX:
                Core.screenManager.setScreen(Core.inbox);
                break;
            case Const.ACTION_SAVETODRAFTS:
                CommonScreen.currentQuestion.save(true);
                break;
            case Const.ACTION_SHOWCOMMENT:
                Core.screenManager.setScreen(Core.comment);
                break;
            case Const.ACTION_SHOWSTATUS:
                Core.screenManager.setScreen(Core.status);
                break;
            case Const.ACTION_GETSEND:
                Core.backNet.getSend = true;
                onAction(Const.ACTION_POPUP);
                break;
            case Const.ACTION_SAVETOOUTBOX:
                CommonScreen.currentQuestion.save(false);
                Core.screenManager.screenStack.removeAllElements();
                Core.screenManager.setScreen(Core.mainMenu);
                break;
            case Const.ACTION_SHOWAB:
                ((GUIListItem) ((GUIList) items.elementAt(0)).items.elementAt(1)).isVisible = true;
                ((GUIListItem) ((GUIList) items.elementAt(0)).items.elementAt(2)).isVisible = true;
                break;
            case Const.ACTION_HIDEAB:
                ((GUIListItem) ((GUIList) items.elementAt(0)).items.elementAt(1)).isVisible = false;
                ((GUIListItem) ((GUIList) items.elementAt(0)).items.elementAt(2)).isVisible = false;
                break;
        }
        Core.keyAction = Const.ACTION_NULL;
    }

    public void drawBack(int x, int y)
    {
        Core.staticG.setClip(0, header.getHeight(), Core.SCREEN_WIDTH, Core.SCREEN_HEIGHT);
        int oldColor = Core.staticG.getColor();
        Core.staticG.setColor(0xffffff);
        Core.staticG.fillRect(0, 0, Core.SCREEN_WIDTH, Core.SCREEN_HEIGHT);
        Core.staticG.setColor(oldColor);
        Core.staticG.setClip(0, 0, Core.SCREEN_WIDTH, Core.SCREEN_HEIGHT);
        if (drawBack)
        {
            back.render(x, y);
        }
    }

    static public GUIList createList()
    {
        GUIList list = new GUIList();
        list.dx = Core.SCREEN_WIDTH;
        list.dy = Core.SCREEN_HEIGHT - commonHeader.getHeight() - commonFooter.getHeight();

        return list;
    }

    static public GUIListItem createItem()
    {
        GUIListItem item = new GUIListItem();

        item.skin = CommonScreen.itemSkin;
        item.dx = Core.SCREEN_WIDTH;
        item.dy = CommonScreen.itemSkin.sprite.spr_MaxHeight;
        item.selectedColor = 0xffffff;
        item.unselectedColor = 0x424242;

        return item;
    }

    static public GUIText createText()
    {
        GUIText text = new GUIText(Core.smallFont, false, false);
        text.x = 0;//Const.COMMON_MARGIN;
        text.dx = Core.SCREEN_WIDTH - 2 * Const.COMMON_MARGIN;
        text.dy = CommonScreen.itemSkin.sprite.spr_FrameHeight;

        return text;
    }

    static public GUIListItem createPopupItem()
    {
        GUIListItem popupItem = new GUIListItem();
        popupItem.skin = CommonScreen.itemSkin;
        popupItem.dx = 7 * Core.SCREEN_WIDTH / 8;
        popupItem.dy = CommonScreen.itemSkin.sprite.spr_FrameHeight;
        popupItem.selectedColor = 0xffffff;
        popupItem.unselectedColor = 0x424242;
        GUIText popupText = new GUIText(Core.smallFont, false, false);
        popupText.x = Const.COMMON_MARGIN;
        popupText.dx = popupItem.dx - 2 * Const.COMMON_MARGIN;
        popupText.dy = CommonScreen.itemSkin.sprite.spr_FrameHeight;
        popupText.setText("");
        popupItem.addItem(popupText);
        popupItem.clickAction = Const.ACTION_SELECT;

        return popupItem;
    }
}
