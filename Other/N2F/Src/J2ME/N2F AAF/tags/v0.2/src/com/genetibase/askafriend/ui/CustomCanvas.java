package com.genetibase.askafriend.ui;

import java.util.Enumeration;
import java.util.Vector;

import javax.microedition.lcdui.Canvas;
import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.Graphics;

class CustomCanvas extends Canvas 
{
	private Canvasable canvasable;
	private boolean isPointerDevice;
	
	public CustomCanvas(){
		super();
		isPointerDevice = hasPointerEvents();		
	}

	public void setCanvasable( Canvasable paintable ) {
		this.canvasable = paintable;
		if (paintable != null) {
			this.setFullScreenMode(paintable.getFullScreenMode());
		}
	}
	
	protected void paint(Graphics g) {
		if (canvasable != null) {
			canvasable.paint(g);
		} else {
		    g.setColor(0xFFFFFF);
			g.fillRect(0, 0, getWidth(), getHeight());
		}
		
	}
	
	protected void keyPressed(int keyCode){
		if (canvasable != null) {
			canvasable.keyPressed(keyCode);
		}
	}
	protected void keyReleased(int keyCode){
		if (canvasable != null) {
			canvasable.keyReleased(keyCode);
		}
	}
	
	private static CustomCanvas canvas = null;
	
	public static CustomCanvas getCanvas() {
		if (canvas == null) {
			canvas = new CustomCanvas();
		}
		return canvas;
	}
	
	public void reset(){
		canvasable = null;
		for (Enumeration e = commands.elements(); e.hasMoreElements(); ) {
			this.removeCommand((Command)e.nextElement());
		}
		commands.removeAllElements();
		setCommandListener(null);
		repaint();
	} 
	
	private Vector commands = new Vector();
	
	public void addCommand(Command cmd) {
		super.addCommand(cmd);
		commands.addElement(cmd);
	}

	public boolean isPointerDevice() {
		return isPointerDevice;
	}
	
	
}