package n2f.sup.ui;

import javax.microedition.lcdui.AlertType;
import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.List;
import javax.microedition.lcdui.Ticker;

import n2f.sup.common.Resoursable;
import n2f.sup.core.Engine;
import n2f.sup.core.ItemLight2;
import n2f.sup.ui.bean.CartBean;


class SUPCartScreen extends CartScreen implements Interruptable, SUP_GUIListener {
	private boolean interrupted = true;
	protected void setTitle() {
		this.form = new List(getLocalizedText(LocaleUI.FORM_SUP_IMAGE_CART_CAPTION), List.IMPLICIT);
		this.form.setTicker(new Ticker(getLocalizedText(LocaleUI.IMAGE_SUP_CART_TEXT)));
	}

	
	SUPCartScreen(String id, Resoursable resoursable) {
		super(id, resoursable);
	}

	protected void createNextCommand(CartBean bean) {
		this.nextCommand = new Command(getLocalizedText(LocaleUI.IMAGE_SUP_CART_COMMAND_UPLOAD), Command.SCREEN, 1);
		this.form.addCommand(nextCommand);
	}
	
	protected void doNext() {
		updateItems();
		showBusy(this);
		interrupted = false;		
		doUpload();
	}
	
	private void doUpload() {
		if (!interrupted) {
			if (items.size() > 0) {
				ItemLight2 item = (ItemLight2)items.lastElement();
				Engine.getEngine().uploadPhotoSUP(this, item);
			} else {
				refresh();
				showAlert(getLocalizedText(LocaleUI.IMAGE_SUP_CART_ALERT1), AlertType.WARNING, this);
			}
		}
	}
	
	protected void doBrowse() {
		UIManager.getInstance().show(UIManager.SCREEN_SUP_BROWSER);
	}


	protected void refresh() {
		//TODO: update items and their states
		interrupted = true;		
		show();
	}


	public void interrupted() {
		interrupted = true;
		Engine.getEngine().breakNetworkConnection();
		show();
	}
	
	private void removeItem() {
		ItemLight2 item = (ItemLight2)items.lastElement();
		if (item != null) {
			deleteItem(item.getIdOnForm());			
		}
	}
	
	public void fireAction (int actionType) {
		switch (actionType) {
		case ACTION_BUSY:
//			showBusy(null);
			break;
		case ACTION_HIDE:
//			refresh();
			break;
		case ACTION_OK:
			removeItem();
			if (items.size() > 0) {
				doUpload();
			} else {
				//UIManager.getInstance().show(UIManager.SCREEN_SUP_BROWSER);
				refresh();
			}
			break;
		case ACTION_FAILED_MEMORY_ISSUE:
			refresh();
			showAlert(getLocalizedText(LocaleUI.IMAGE_SUP_CART_MEMORY), AlertType.ERROR, this);
			break;
		case ACTION_FAILED_GENERAL:
			refresh();
			showAlert(getLocalizedText(LocaleUI.IMAGE_SUP_CART_GENERAL), AlertType.ERROR, this);
			break;
		}
	}
}