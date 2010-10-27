package App;
import gui.*;
import javax.microedition.lcdui.Graphics;

public class ScreenView extends CommonScreen
{
    public DashboardItem dash;
    
    public void onShow()
    {
	drawBack = false;
	footer.posText.setText("");
	footer.posAction = Const.ACTION_NULL;
	footer.negText.setText("Back");
	footer.negAction = Const.ACTION_BACK;
	
	GUIText text = CommonScreen.createText();
	addControl(text);

	
	text.setText(dash.date);
	text.x = text.dx-text.font.charsWidth(text.text, 0, text.textLen);
	
	GUIText bigText = new GUIText(Core.smallFont, true, false);
	bigText.dx = text.dx;
	bigText.dy = Core.SCREEN_HEIGHT-commonHeader.getHeight()-commonFooter.getHeight()-text.dy;
	bigText.setText(dash.text);
	addControl(bigText);
    }
}
