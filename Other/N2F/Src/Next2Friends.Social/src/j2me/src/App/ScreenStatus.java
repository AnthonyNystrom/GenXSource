package App;

import gui.*;
import java.io.IOException;
import service.MemberService;

public class ScreenStatus extends CommonScreen
{

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

        ((GUIText) popupItem.items.elementAt(0)).setText("Save");
        popupItem.clickAction = Const.ACTION_SAVE;
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
        textHeader.setText("Status");
        addControl(textHeader);

        GUIText enterText = new GUIText(Core.smallFont, true, true);
        enterText.dx = Core.SCREEN_WIDTH;
        enterText.dy = Core.SCREEN_HEIGHT - header.getHeight() - footer.getHeight() - textHeader.dy;
        enterText.isSelected = true;
        addControl(enterText);

        Core.isNetwork = true;
        Core.forceRepaint();
        MemberService service = new MemberService();
        try
        {
            String status = service.GetMemberStatusText(Core.storage.login, Core.storage.password);
            enterText.setText(status);
        } catch (IOException ex)
        {
            ex.printStackTrace();
        }
        Core.isNetwork = false;
    }

    public void onAction(int action)
    {
        switch (action)
        {
            case Const.ACTION_SAVE:
                {
                    Core.isNetwork = true;
                    Core.forceRepaint();
                    GUIText text = (GUIText) items.elementAt(1);
                    String statusText = new String(text.text, 0, text.textLen);
                    MemberService service = new MemberService();
                    try
                    {
                        service.SetMemberStatusText(Core.storage.login, Core.storage.password, statusText);
                    } catch (Exception ex)
                    {
                    }
                    Core.isNetwork = false;
                    onAction(Const.ACTION_POPUP);
                    onAction(Const.ACTION_BACK);
                }
                break;
            default:
                super.onAction(action);
                break;
        }
    }
}