package com.genetibase.askafriend.ui;

import java.util.Enumeration;
import java.util.Vector;

import javax.microedition.lcdui.AlertType;
import javax.microedition.lcdui.ChoiceGroup;
import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.CommandListener;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.Form;
import javax.microedition.lcdui.Image;
import javax.microedition.lcdui.Item;
import javax.microedition.lcdui.ItemStateListener;
import javax.microedition.lcdui.List;
import javax.microedition.lcdui.Ticker;

import com.genetibase.askafriend.common.Resoursable;
import com.genetibase.askafriend.core.ActionItem;
import com.genetibase.askafriend.core.Engine;
import com.genetibase.askafriend.core.ItemLight2;
import com.genetibase.askafriend.core.Question;

class CartScreen extends AbstractWindow {
	private final List form;
	private final Command backCommand, nextCommand, browseCommand;
	private Command previewCommand, deleteCommand, deleteAllCommand, selectCommand;
	
	private final Vector items = new Vector();
	private final Image IMAGE_SEL, IMAGE_DESEL;
	private Ticker ticker;
	
	CartScreen(String id, Resoursable resoursable) {
		super(id, resoursable);
		IMAGE_DESEL = Engine.getEngine().getImage("/file.png");
		IMAGE_SEL = Engine.getEngine().getImage("/check.png");
		CartBean bean = (CartBean)Engine.getEngine().getBean(getId());
		if (bean == null) {
			Engine.getEngine().putBean(UIManager.SCREEN_CARD, bean = new CartBean(getResoursable()));
		}
		this.form = new List(getLocalizedText(LocaleUI.FORM_IMAGE_CART_CAPTION), List.IMPLICIT);
		this.backCommand = new Command(getLocalizedText(LocaleUI.OPTIONS_COMMAND_BACK), Command.BACK, 9);
		this.nextCommand = new Command(bean.nextIsProceedCommandName()? 
				getLocalizedText(LocaleUI.IMAGE_CART_COMMAND_NEXT):getLocalizedText(LocaleUI.IMAGE_CART_COMMAND_ASK), Command.SCREEN, 1);
		this.browseCommand =  new Command(getLocalizedText(LocaleUI.START_CITEM_EXPLORE), Command.SCREEN, 3);

		int index = 0;
		for (Enumeration e = bean.getItems(); e.hasMoreElements(); index++) {
			ItemLight2 item = (ItemLight2)e.nextElement();
			item.setIdOnForm(form.append(item.getTitle(), item.isSelected()?IMAGE_SEL: IMAGE_DESEL));
		}
		
		this.form.addCommand(backCommand);
		if (form.size()> 0) {
			this.previewCommand = new Command(getLocalizedText(LocaleUI.FILE_SELECTOR_COMMAND_PREVIEW), Command.SCREEN, 4);
			this.deleteCommand = new Command (getLocalizedText(LocaleUI.IMAGE_CART_COMMAND_DELETE), Command.SCREEN, 5);
			this.deleteAllCommand = new Command (getLocalizedText(LocaleUI.IMAGE_CART_COMMAND_DELETEALL), Command.SCREEN, 5);
			this.selectCommand = new Command(getLocalizedText(LocaleUI.IMAGE_CART_COMMAND_SELECT), Command.SCREEN, 9);

			form.setSelectCommand(selectCommand);
			if (!getResoursable().isTouch()) {
				this.form.addCommand(previewCommand);
				this.form.addCommand(deleteCommand);
			}
			this.form.addCommand(deleteAllCommand);
		}
//		if (bean.getSize() == 0) {
//			form.append(getLocalizedText(LocaleUI.IMAGE_CART_TEXT), null);
//		}
		this.form.setTicker(ticker = new Ticker(bean.getSize()>0? 
				(bean.nextIsProceedCommandName()? getLocalizedText(LocaleUI.IMAGE_CART_TEXT3): getLocalizedText(LocaleUI.IMAGE_CART_TEXT2)):
					getLocalizedText(LocaleUI.IMAGE_CART_TEXT)));
		this.form.addCommand(nextCommand);
		this.form.addCommand(browseCommand);
		this.form.setCommandListener(this);
	}

	public int getFormHeight() {
		return form.getHeight();
	}

	public int getFormWidth() {
		return form.getWidth();
	}

	protected Displayable getScreen() {
		return form;
	}

	protected void refresh() {}

	public void show() {
        getDisplay().setCurrent(this.form);
	}
	
	/**
	 * updates item state and returns first checked item index. if no selected then -1 
	 * @param flags
	 * @return
	 */
	private void updateItems() {
		items.removeAllElements();
		CartBean bean = (CartBean)Engine.getEngine().getBean(getId());
		Vector v = bean.getItemsVector();
		for (int size = form.size(), i = 0; i < size && v.size() > 0; i++) {
			((ItemLight2)v.elementAt(i)).setSelected(form.getImage(i) == IMAGE_SEL);
			if (form.getImage(i) == IMAGE_SEL)
				items.addElement(v.elementAt(i));
		}
	}
	
	private void deleteItem() {
		CartBean bean = (CartBean)Engine.getEngine().getBean(getId());
		int index = form.getSelectedIndex();
		form.delete(index);
		bean.removeItem(index);
		updateItems();
		bean.saveBean(getResoursable());
		if (form.size() == 0) {
			form.setSelectCommand(null);
			this.form.removeCommand(previewCommand);
			this.form.removeCommand(deleteCommand);			
			this.form.removeCommand(deleteAllCommand);
//			this.form.append(getLocalizedText(LocaleUI.IMAGE_CART_TEXT), null);
			this.ticker.setString(getLocalizedText(LocaleUI.IMAGE_CART_TEXT));
		}		
	}
	
	private void previewItem() {
		updateItems();
		PreviewBean bean = (PreviewBean)Engine.getEngine().getBean(UIManager.SCREEN_PREVIEW);
		if (bean == null) {
			Engine.getEngine().putBean(UIManager.SCREEN_PREVIEW, bean = new PreviewBean());
		}
		bean.clean();
		int selectedIndex = form.getSelectedIndex();//
		if (selectedIndex != -1) {
			ItemLight2 cItem = ((CartBean)Engine.getEngine().getBean(getId())).getItem(selectedIndex);
			bean.setItem(cItem);
			UIManager.getInstance().show(UIManager.SCREEN_PREVIEW);
		} else {
			showAlert(getLocalizedText(LocaleUI.ALERT_IMAGE_CART_TEXT), AlertType.ERROR, this);
		}		
	}

	public void commandAction(Command c, Displayable d) {
		super.commandAction(c, d);
		if (c == backCommand) {
			UIManager.getInstance().showPrevious();
		} else if (c == previewCommand) {//alert
			previewItem();
		} else if (c == nextCommand) {
			updateItems();
			if (items.size() > 3 || (items.size()== 0)) {
				showAlert(getLocalizedText(LocaleUI.FILE_SELECTOR_ALERT_TEXT), AlertType.CONFIRMATION, this);
			} else {
				if (items.size() > 0) {
					Question.getInstance().setPictureNames(items);
				}
				UIManager.getInstance().show(UIManager.SCREEN_CAN_QUESTIONS);
			}
		} else if (c == deleteCommand) {//alert
			deleteItem();
		} else if (c == browseCommand) {
			UIManager.getInstance().show(UIManager.SCREEN_FILE_SYSTEM);
		} else if (c == deleteAllCommand) {
			CartBean bean = (CartBean)Engine.getEngine().getBean(getId());
			form.deleteAll();
			bean.removeAll();
			items.removeAllElements();
			bean.saveBean(getResoursable());
			form.setSelectCommand(null);
			this.form.removeCommand(previewCommand);
			this.form.removeCommand(deleteCommand);			
			this.form.removeCommand(deleteAllCommand);
			this.ticker.setString(getLocalizedText(LocaleUI.IMAGE_CART_TEXT));
//			this.form.append(getLocalizedText(LocaleUI.IMAGE_CART_TEXT), null);
			
		} else if (c == selectCommand) {//alert
			if (getResoursable().isTouch()) {
				new CardOptionsScreen(getResoursable()).show();
			} else {
				markItem();				
			}
		}
	}
	
	private void markItem() {
		int index = form.getSelectedIndex();
		String s = form.getString(index);
		Image i = form.getImage(index);
		if (i == IMAGE_DESEL) {
			form.set(index, s, IMAGE_SEL);
		} else if (i == IMAGE_SEL) {
			form.set(index, s, IMAGE_DESEL);
		}
	}
	
	private class CardOptionsScreen implements CommandListener , ItemStateListener{
		
		private static final byte ACTION_BACK = -1;
		private static final byte ACTION_PREVIEW = 2;
		private static final byte ACTION_MARK = 3;
		private static final byte ACTION_DELETE = 4;
		
		private final Form form; 
		private final ChoiceGroup choiceGroup; 
		private final Command selectCommand;
		private Vector actionItems = new Vector();
		
		CardOptionsScreen(Resoursable resoursable) {
			this.form = new Form(getLocalizedText(LocaleUI.FORM_CARD_OPTIONS_CAPTION));
			this.choiceGroup = new ChoiceGroup(null, ChoiceGroup.EXCLUSIVE);
			selectCommand = new Command(getLocalizedText(LocaleUI.FORM_CARD_OPTIONS_COMMAND_SELECT), Command.OK, 0);
			//create;
			
			createItem(getLocalizedText(LocaleUI.FORM_CARD_OPTIONS_COMMAND_BACK), ACTION_BACK);
			createItem(getLocalizedText(LocaleUI.FORM_CARD_OPTIONS_COMMAND_DELETE), ACTION_DELETE);
			///
			Image i = CartScreen.this.form.getImage(CartScreen.this.form.getSelectedIndex());
			///
			
			createItem(i == IMAGE_DESEL? getLocalizedText(LocaleUI.FORM_CARD_OPTIONS_CHOICE_MARK): getLocalizedText(LocaleUI.FORM_CARD_OPTIONS_CHOICE_UNMARK), ACTION_MARK);
			createItem(getLocalizedText(LocaleUI.FORM_CARD_OPTIONS_COMMAND_PREVIEW), ACTION_PREVIEW);			
			//add to form
			for (Enumeration e = actionItems.elements(); e.hasMoreElements();) {
				ActionItem item = (ActionItem)e.nextElement();
				int index = this.choiceGroup.append(item.getTitle(), item.getIcon());
				item.setIdOnForm(index);
			}
			form.append(choiceGroup);
			
			this.choiceGroup.setDefaultCommand(selectCommand);
			this.form.setCommandListener(this);
			this.form.setItemStateListener(this);
		}

		public void show() {
			getDisplay().setCurrent(this.form);
		}
		
		private void hide() {
			CartScreen.this.show();
		}

		public void commandAction(Command c, Displayable d) {
			if (c == selectCommand) {
				doAction();
			}
		}
		
		private void doAction() {
			int index = choiceGroup.getSelectedIndex();
			ActionItem item = (ActionItem)actionItems.elementAt(index);
			switch (item.getAction()) {
			case ACTION_BACK:
				this.hide();
				break;
			case ACTION_DELETE:
				deleteItem();
				this.hide();
				break;
			case ACTION_MARK:
				markItem();	
				this.hide();
				break;
			case ACTION_PREVIEW:
				previewItem();
				break;
			}
		}
		
		private void createItem(String text, /*String imageId, */byte action){
			ActionItem itemBasis = new ActionItem(text, /*imageId!= null ? getImage(imageId): */null, action);
			actionItems.addElement(itemBasis);
		}

		public void itemStateChanged(Item arg0) {
			doAction();			
		}
		

	}	
	
}
