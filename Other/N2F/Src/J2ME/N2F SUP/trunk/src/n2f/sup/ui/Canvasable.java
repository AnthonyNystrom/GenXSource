package n2f.sup.ui;

import javax.microedition.lcdui.Graphics;

public interface Canvasable {
	boolean getFullScreenMode();
	void paint(Graphics g);
	void keyPressed(int keyCode);
	void keyReleased(int keyCode);
}
