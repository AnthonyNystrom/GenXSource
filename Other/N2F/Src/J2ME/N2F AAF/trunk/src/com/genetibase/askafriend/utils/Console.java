package com.genetibase.askafriend.utils;

import javax.microedition.lcdui.*;

import com.genetibase.askafriend.core.Engine;
import com.genetibase.askafriend.ui.AbstractWindow;
import com.genetibase.askafriend.ui.UIManager;


/**
 * This class is used for work concole representation on handset.

 */
public class Console extends AbstractWindow implements CommandListener{
	private static final int MAX_ITEM_SIZE = 50;
	private static Font font = Font.getFont(Font.FACE_MONOSPACE, Font.STYLE_PLAIN, Font.SIZE_SMALL);
	private Command clear, back, memory, gc;
//	private Displayable previousForm;
	private Display display;
	private static Console console = null;
	private static int elementCount = 0;
	private static Object mutex = new Object();
	private Form form = new Form("Console");
	
	/**
	 * default constructor
	 */
	private Console() {
		super(null, null);		
		form.addCommand(clear = new Command("Clear", Command.ITEM, 0));
		form.addCommand(back = new Command("Back", Command.BACK, 1));
		form.addCommand(memory = new Command("Memory...", Command.ITEM, 2));
		form.addCommand(gc = new Command("Garbage collection...", Command.ITEM, 3));
		
		form.setCommandListener(this);
	}
	
	/**
	 * This method creates StringItem object from string.
	 * @param text - String.
	 * @return instance of StringItem.
	 */
	private static StringItem getStringItem(String text) {
		StringItem stringItem = new StringItem(null, text);
		stringItem.setFont(font);
		return stringItem;
	}

	/**
	 * Thismethod prints text to concole and returns carriage
	 * 
	 * @param s
	 */
	static void println(String s) {
		print(s + '\n');
	}

	/**
	 * This method prints text to concole
	 * 
	 * @param s
	 */
	static void print(String s) {
		synchronized(mutex) {
			try {
				if (elementCount >= MAX_ITEM_SIZE) {
					getInstance().form.delete(--elementCount);
				}
				getInstance().form.insert(0, getStringItem(s));
				elementCount++;
			} catch (Exception e) {
				e.printStackTrace();
				Debug.println("java.lang.IndexOutOfBoundsException in Console");
			}
		}
	}

	/**
	 * This method show Display object
	 * 
	 * @param display
	 */
	public void show() {
		getInstance().display = Display.getDisplay(Engine.getEngine().getMidlet());
//		getInstance().previousForm = display.getCurrent();
		getInstance().display.setCurrent(getInstance().form);
	}

	/**
	 * method dispatches command to Displayable object
	 */
	public void commandAction(Command c, Displayable d) {

		if (c == back) {
			UIManager.getInstance().showCurrent();
		} else if (c == clear) {
			form.deleteAll();
			elementCount = 0;
		}  else if (c == memory) {
			printMemory();
		} else if (c == gc) {
			System.gc();
		} 
	}
	
	
	
	/**
	 * This method prints to concole total memory, free memory and active thread
	 * counts
	 */
	public void printMemory() {
		Runtime r = Runtime.getRuntime();
		println("Total memory: " + r.totalMemory());
		println("Free memory: " + r.freeMemory());
		println("Active threads: " + Thread.activeCount());
	}

	protected void hideBusy() {
		// TODO Auto-generated method stub
		
	}

	protected void refresh() {
		// TODO Auto-generated method stub
		
	}

	public int getFormWidth() {
		return getInstance().form.getWidth();
	}

	public int getFormHeight() {
		return getInstance().form.getHeight();
	}
	
	public static Console getInstance(){
		if (console == null) {
			console = new Console();
		}
		return console;
	}

	protected Displayable getScreen() {
		return form;
	}
	
}
