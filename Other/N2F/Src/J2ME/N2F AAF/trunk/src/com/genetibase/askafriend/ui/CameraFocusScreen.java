package com.genetibase.askafriend.ui;

import javax.microedition.lcdui.Alert;
import javax.microedition.lcdui.AlertType;
import javax.microedition.lcdui.Canvas;
import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.Graphics;

import com.genetibase.askafriend.camera.MediaUtils;
import com.genetibase.askafriend.common.ActionListener;
import com.genetibase.askafriend.common.Deallocatable;
import com.genetibase.askafriend.common.Resoursable;
import com.genetibase.askafriend.core.Engine;

class CameraFocusScreen extends AbstractWindow implements Deallocatable {
	private Command backCommand, captureCommand, saveCommand, saveBackCommand, settingsCommand;
	private Displayable displayable;
	private boolean isCameraEnabled = true;
	private byte[] capturedImage = null;
	
	CameraFocusScreen(String id, Resoursable resoursable) {
		super(id, resoursable);
		CameraBean bean = (CameraBean)Engine.getEngine().getBean(UIManager.SCREEN_CAMERA_SETTINGS);
        if (bean == null) {
        	bean = new CameraBean(getResoursable());
        	Engine.getEngine().putBean(getId(), bean);
        } 
		Engine.getEngine().setPrefferedSettings(bean.getWrapper());
		CustomCanvas.getCanvas().reset();
	    backCommand = new Command(getLocalizedText(LocaleUI.FOCUS_COMMAND_BACK), Command.BACK, 0);
	    captureCommand = new Command(getLocalizedText(LocaleUI.CAMERA_COMMAND_CAPTURE), Command.SCREEN, 0);
		
	    if (MediaUtils.supportsCapturing()) {
		    displayable = getDisplayable();
	    	displayable.setTitle(getLocalizedText(LocaleUI.FORM_FOCUS_CAPTION));
			displayable.addCommand(captureCommand);
		    addBack2Screen(displayable);
		    settingsCommand = new Command(getLocalizedText(LocaleUI.FOCUS_COMMAND_SET), Command.SCREEN, 0);
		    displayable.addCommand(settingsCommand);
//	    	Engine.getEngine().showCamera((Canvas)getDisplayable());
	    } else {
			isCameraEnabled = false;
		    addBack2Screen(getDisplayable());
	    }
		
	}
	
	public void show() {
		if (isCameraEnabled) {
	    	Engine.getEngine().showCamera((Canvas)getDisplayable());
			getDisplay().setCurrent(getDisplayable());
		} else {
	        Alert a = new Alert(getLocalizedText(LocaleUI.FOCUS_ALERT_TITLE), getLocalizedText(LocaleUI.FOCUS_ALERT_TEXT), null, AlertType.WARNING);
	        a.setTimeout(Alert.FOREVER);
		    a.addCommand(backCommand);
	        getDisplay().setCurrent(a);
		}
	}
	
	private void addBack2Screen(Displayable screen) {
		if (screen != null) {
		    screen.addCommand(backCommand);
		    screen.setCommandListener(this);
		}
	}

	protected void refresh() {
	    getDisplay().setCurrent(getDisplayable());
		if (isCameraEnabled) {
		    ((Canvas)getDisplayable()).repaint();
		}
	}
	
	private Displayable getDisplayable() {
		if (displayable == null) {
			if (isCameraEnabled) {
				CustomCanvas.getCanvas().setCanvasable(new CanvasEx());
				displayable = CustomCanvas.getCanvas();
//				displayable = new CanvasEx();
//			} else {
//				displayable = new Form("Video");
			}
		}
		return displayable;
	}

//	private static final void removeCommand(Canvas canvas, Command command){
//		if (command != null)
//			canvas.removeCommand(command);
//	}
	
	public void free() {
		CustomCanvas.getCanvas().reset();
		displayable = null;
		capturedImage = null;
	}
	
	class CanvasEx extends CanvasableAdapter {
		public void paint(Graphics g) {
			Canvas canvas = CustomCanvas.getCanvas();
		    int width = canvas.getWidth();
		    int height = canvas.getHeight();
//			g.setColor(0xFFFFFF);
//			g.drawRect(0, 0, width, height);
		    // Draw a green border around the VideoControl.
		    g.setColor(0x00ff00);
		    g.drawRect(0, 0, width - 1, height - 1);
		    g.drawRect(1, 1, width - 3, height - 3);
		}

		public void keyPressed(int keyCode) {
			int action = CustomCanvas.getCanvas().getGameAction(keyCode);
			if (action == Canvas.FIRE){
				if (isSnapshotAvailable)
					capture();
			}
		}

		public boolean getFullScreenMode() {
			return false;
		}
	}
	
	
	private boolean isSnapshotAvailable = true;
	private void capture() {
		isSnapshotAvailable = false;
		Engine.getEngine().capture(new ActionListener (){
			public void actionPerformed(Object obj, int status) {
				capturedImage = (byte[]) obj;
				Alert a = new Alert(null, getLocalizedText(LocaleUI.FOCUS_ALERT_SAVE_TEXT), null, AlertType.CONFIRMATION);
			    a.setTimeout(Alert.FOREVER);
				a.addCommand(saveBackCommand = new Command(getLocalizedText(LocaleUI.FOCUS_ALERT_COMMAND_DECLINE), Command.SCREEN, 1));
				a.addCommand(saveCommand = new Command(getLocalizedText(LocaleUI.FOCUS_ALERT_COMMAND_SAVE), Command.SCREEN, 0));
			    a.setCommandListener(CameraFocusScreen.this);
				Engine.getEngine().stopCamera();
				getDisplay().setCurrent(a);
			}
		});		
	}
	
	public void commandAction(Command c, Displayable d) {
		super.commandAction(c, d);		
		if (c.getCommandType() == Command.EXIT) {
			Engine.getEngine().stopCamera();
			free();
		    Engine.getEngine().exit();
		} else if (c == backCommand) {
			Engine.getEngine().stopCamera();
			UIManager.getInstance().showPrevious();
			free();
		}else if (c == captureCommand) {
			if (isSnapshotAvailable)
				capture();
		} else if (c == saveCommand) {
			isSnapshotAvailable = true;
//			Debug.println("captured imag = " + this.capturedImage.length);
			Engine.getEngine().setCapturedImage(this.capturedImage);
			UIManager.getInstance().show(UIManager.SCREEN_IMAGE_SAVER);
			free();
		} else if (c == saveBackCommand) {
			capturedImage = null;
			isSnapshotAvailable = true;
			show();
		} else if (c == settingsCommand) {
			Engine.getEngine().stopCamera();
			free();
			UIManager.getInstance().show(UIManager.SCREEN_CAMERA_SETTINGS, false);
		} else if (c == Alert.DISMISS_COMMAND) {
			Engine.getEngine().stopCamera();
			UIManager.getInstance().showPrevious();
			free();
		}
		else {
			free();
//			throw new IllegalArgumentException("Unknown command");
		}
	}

	public int getFormHeight() {
		return displayable.getHeight();
	}

	public int getFormWidth() {
		return displayable.getWidth();
	}

	protected Displayable getScreen() {
		return displayable;
	}
}