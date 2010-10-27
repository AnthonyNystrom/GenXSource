package com.genetibase.askafriend.ui;


import javax.microedition.lcdui.Alert;
import javax.microedition.lcdui.AlertType;
import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.CommandListener;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.Graphics;
import javax.microedition.lcdui.Image;

import com.genetibase.askafriend.common.ActionListener;
import com.genetibase.askafriend.common.Deallocatable;
import com.genetibase.askafriend.common.Resoursable;
import com.genetibase.askafriend.core.Engine;
import com.genetibase.askafriend.core.ItemLight2;

// This class displays a selected image centered in the screen
class PreviewScreen extends AbstractWindow implements ActionListener, Interruptable, Deallocatable {
	private Command backCommand;
	private Command add2CartCommand, delFromCartCommand;	
	private Image previewImage = null;
	private ItemLight2 item;
	private CustomCanvas canvas;
	
	public PreviewScreen(String id, Resoursable resoursable) {
		super(id, resoursable);
	    backCommand = new Command(getLocalizedText(LocaleUI.FORM_PREVIEW_COMMAND_BACK), Command.BACK, 0);
		
		PreviewBean bean = (PreviewBean)Engine.getEngine().getBean(getId());
		if (bean == null) {
			Engine.getEngine().putBean(getId(), bean = new PreviewBean());
		}
		item = bean.getItem();
		if (item != null) {
			Engine.getEngine().scaleImage(item.getPath() + item.getTitle(), CustomCanvas.getCanvas(), this);
		}
	}
	
	private int width, height;
	
	protected String getTitle() {
		String path = item.getPath()+item.getTitle();
		String FILE_PREFIX = "file:/";
		if (path == null) {
			path = getLocalizedText(LocaleUI.FORM_PREVIEW_CAPTION);
		} else {
			int index = path.indexOf(FILE_PREFIX);
			if (index >-1)
				path = path.substring(index + FILE_PREFIX.length());	
		}
		return path;
	}
	
	public void show() {
		showBusy(null);
		canvas = CustomCanvas.getCanvas();
		if (item != null)
			canvas.setTitle(getTitle());
		canvas.setCanvasable(new CanvasableAdapter() {
			public void paint(Graphics g) {
			    g.setColor(0xffffff);
			    width = canvas.getWidth();
			    height = canvas.getHeight();			    
			    g.fillRect(0, 0, width, height);
			    if (previewImage != null) {
			        g.drawImage(previewImage,(width-previewImage.getWidth())/2,(height-previewImage.getHeight())/2,0);
			    }
			}

			public boolean getFullScreenMode() {
				return false;
			}
		});
		
		
		canvas.addCommand(backCommand);
		
		CartBean bean = (CartBean)Engine.getEngine().getBean(UIManager.SCREEN_CARD);
		if (bean == null) {
			Engine.getEngine().putBean(UIManager.SCREEN_CARD, bean = new CartBean(getResoursable()));
		}
		if (bean.containsItem(item)) {
			delFromCartCommand = new Command(getLocalizedText(LocaleUI.FORM_PREVIEW_COMMAND_REMOVE_FROM_CART), Command.SCREEN, 1);
			canvas.addCommand(delFromCartCommand);
		} else {
			add2CartCommand = new Command(getLocalizedText(LocaleUI.FORM_PREVIEW_COMMAND_ADD_2_CART), Command.SCREEN, 1);
			canvas.addCommand(add2CartCommand);
		}
		canvas.setCommandListener(this);
	}
	

	public void commandAction(Command c, Displayable d) {
		super.commandAction(c, d);
		CartBean bean = (CartBean)Engine.getEngine().getBean(UIManager.SCREEN_CARD);
		if (bean == null) {
			Engine.getEngine().putBean(UIManager.SCREEN_CARD, bean = new CartBean(getResoursable()));
		}
		if (c == backCommand) {
			try {
				bean.saveBean(getResoursable());
				hideBusy();
			} catch (Exception e) {
				// TODO: handle exception
			} finally {
				free();
				UIManager.getInstance().showPrevious();
			}
		} else if (c == add2CartCommand) {
			bean.addItem(item);
			canvas.removeCommand(add2CartCommand);
			if (delFromCartCommand == null)
				delFromCartCommand = new Command(getLocalizedText(LocaleUI.FORM_PREVIEW_COMMAND_REMOVE_FROM_CART), Command.SCREEN, 1);
			canvas.addCommand(delFromCartCommand);
		} else if (c == delFromCartCommand) {
			bean.removeItem(item);
			canvas.removeCommand(delFromCartCommand);
			if (add2CartCommand == null)
				add2CartCommand = new Command(getLocalizedText(LocaleUI.FORM_PREVIEW_COMMAND_ADD_2_CART), Command.SCREEN, 1);
			canvas.addCommand(add2CartCommand);
		} 
	}


	protected void refresh() {
		hideBusy();
	}

	public int getFormWidth() {
		return width;
	}

	public int getFormHeight() {
		return height;
	}

	public void actionPerformed(Object action, int statusResult) {
		if (statusResult == ActionListener.STATUS_OK) {
			previewImage = (Image)action;
			hideBusy();
			getDisplay().setCurrent(CustomCanvas.getCanvas());
			CustomCanvas.getCanvas().repaint();
		} else if (statusResult== ActionListener.STATUS_FAILED) {
			Alert a = new Alert(null ,getLocalizedText(LocaleUI.ERROR_PREVIEW_TEXT), null, AlertType.CONFIRMATION);
		    a.setTimeout(Alert.FOREVER);
			a.addCommand(backCommand);
			a.setCommandListener(this);
			getDisplay().setCurrent(a);
		} 
//		else {
//			throw new IllegalArgumentException("Unknown status");
//		}
	}

	public void interrupted() {
//		UIManager.getInstance().showPrevious();
	}


	public void free() {
		CustomCanvas.getCanvas().reset();
	}


	protected Displayable getScreen() {
		return null;
	}
	
	
	public void showAlert(String text, AlertType type, CommandListener listener) {}
}