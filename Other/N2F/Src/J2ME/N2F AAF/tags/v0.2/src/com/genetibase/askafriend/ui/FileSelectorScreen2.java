package com.genetibase.askafriend.ui;

import java.util.Enumeration;
import java.util.Vector;

import javax.microedition.lcdui.AlertType;
import javax.microedition.lcdui.ChoiceGroup;
import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.CommandListener;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.Form;
import javax.microedition.lcdui.Item;
import javax.microedition.lcdui.ItemStateListener;
import javax.microedition.lcdui.List;
import javax.microedition.lcdui.Ticker;

import com.genetibase.askafriend.common.Resoursable;
import com.genetibase.askafriend.common.file.FileInfoWrapper;
import com.genetibase.askafriend.core.ActionItem;
import com.genetibase.askafriend.core.Engine;
import com.genetibase.askafriend.core.ItemLight2;
import com.genetibase.askafriend.ui.bean.FileSelectorBean;

public class FileSelectorScreen2 extends AbstractWindow {
	private static final String FOLDER_UP = "..";
	private static final String FILE_PREFIX = "file:/";
	
	private final Command askCommand;
	protected final Command cancelCommand;
	private final Command previewCommand;
	protected final Command selectCommand;
	private Command add2CartCommand;
	protected final List list;
	
	//final
	protected String getTitle() {
		String path = Engine.getEngine().gerFileRootPath();
		if (path == null) {
			path = getLocalizedText(LocaleUI.FORM_FILE_SELECTOR_CAPTION);
		}else {
			int index = path.indexOf(FILE_PREFIX);
			if (index >-1)
				path = path.substring(index + FILE_PREFIX.length());	
		}
		return path;
	}
	
	private FileSelectorBean getFileSelectorBean() {
		FileSelectorBean bean = (FileSelectorBean)Engine.getEngine().getBean(getId());
		if (bean == null) {
			Engine.getEngine().putBean(getId(), bean = new FileSelectorBean());
		}
		return bean;
	}
	
	public FileSelectorScreen2(String id, Resoursable resoursable) {
		super(id, resoursable);

		this.list = new List(getTitle(), List.IMPLICIT);
		this.selectCommand = new Command(getLocalizedText(LocaleUI.FILE_SELECTOR_COMMAND_SELECT), Command.SCREEN, 9);
		this.list.setSelectCommand(selectCommand);		
		this.list.setCommandListener(this);		
		this.askCommand = new Command(getLocalizedText(LocaleUI.FILE_SELECTOR_COMMAND_ASK_A_QUESTION), Command.SCREEN, 1);
		this.cancelCommand = new Command(getLocalizedText(LocaleUI.FILE_SELECTOR_COMMAND_CANCEL), Command.CANCEL, 9);
		this.previewCommand = new Command (getLocalizedText(LocaleUI.FILE_SELECTOR_COMMAND_PREVIEW), Command.SCREEN, 4);
		this.add2CartCommand = new Command (getLocalizedText(LocaleUI.FILE_SELECTOR_COMMAND_ADD2CART), Command.SCREEN, 2);
		this.cmdResponses = new Command(getLocalizedText(LocaleUI.START_CITEM_RESPONSES), Command.SCREEN, 5);
		this.cmdPrivateQuestions = new Command(getLocalizedText(LocaleUI.START_CITEM_PRIVATE_QUESTIONS), Command.SCREEN, 5);
		this.list.setTicker(new Ticker(getLocalizedText(LocaleUI.FILE_SELECTOR_TEXT)));
		addCommands();
	}
	
	protected void addCommands() {
		this.list.addCommand(cmdResponses);
		this.list.addCommand(cmdPrivateQuestions);
		this.list.addCommand(askCommand);
		this.list.addCommand(cancelCommand);
		addFileCommands();
	}
	
	private void addFileCommands() {
		if (!getResoursable().isTouch()) {
			this.list.addCommand(previewCommand);
			this.list.addCommand(add2CartCommand);
		}
	}
	
	private void doPreview(ItemLight2 item){
		if (item != null) {
			if (item.getType() != ItemLight2.TYPE_FILE) {
				showAlert(getLocalizedText(LocaleUI.ALERT_SELECTOR_COMMAND_PREVIEW_TEXT), AlertType.CONFIRMATION, this);
			} else {
				PreviewBean bean = (PreviewBean)Engine.getEngine().getBean(UIManager.SCREEN_PREVIEW);
				if (bean == null) {
					Engine.getEngine().putBean(UIManager.SCREEN_PREVIEW, bean = new PreviewBean());
				}
				bean.clean();
				getFileSelectorBean().setSelected(item);
				bean.setItem(item);
				UIManager.getInstance().show(UIManager.SCREEN_PREVIEW);
			}
		}
	}
	
	protected void doOpen(ItemLight2 item) {
		if (item != null) {
			if (item.getType() != ItemLight2.TYPE_FOLDER) {
				showAlert(getLocalizedText(LocaleUI.ALERT_SELECTOR_THIS_ITEM_CAN_T_BE_OPENED), AlertType.CONFIRMATION, this);
			} else {
				if (!FOLDER_UP.equals(item.getTitle())) {
					getFileSelectorBean().setSelected(item);
				}
				Engine.getEngine().open(item.getTitle(), this);				
			}
		}
	}
	
	private void add2Card(ItemLight2 item){
		if (item != null){ 
			if (item.getType() == ItemLight2.TYPE_FILE) {
				CartBean bean = (CartBean)Engine.getEngine().getBean(UIManager.SCREEN_CARD);
				if (bean == null) {
					Engine.getEngine().putBean(UIManager.SCREEN_CARD, bean = new CartBean(getResoursable()));
				}
				bean.addItem(item);
				bean.saveBean(getResoursable());
			} else {
				showAlert(getLocalizedText(LocaleUI.ALERT_SELECTOR_PLEASE_SELECT_AN_IMAGE), AlertType.ERROR, this);
			}
		}
	}
	
	protected ItemLight2 getSelectedItem(boolean keep) {
		int selected = list.getSelectedIndex();
		ItemLight2 item = null;
		if (selected != -1) {
			item = (ItemLight2)items.elementAt(selected);
			if (keep) {
				getFileSelectorBean().setSelected(item);
			}
		}
		return item;
	}
	
	public void commandAction(Command c, Displayable d)	{		
		super.commandAction(c, d);
		if (c == cancelCommand) {
			dispose();
			UIManager.getInstance().showDefault();
		} else if (c == previewCommand) {//alert
			ItemLight2 item = getSelectedItem(true);
			doPreview(item);
		} else if (c == askCommand) {
			UIManager.getInstance().show(UIManager.SCREEN_CARD);
		} else if (c == add2CartCommand) {//alert
			ItemLight2 item = getSelectedItem(false);
			add2Card(item);
		} else if (c == selectCommand) { //alert
			ItemLight2 item = getSelectedItem(false);
			if (item != null) {
				if (item.getType() == ItemLight2.TYPE_FOLDER) {
					doOpen(item);
				} else if (item.getType() == ItemLight2.TYPE_FILE) {
					if (getResoursable().isTouch()) {
						new OptionsScreen(getResoursable()).show();
					} else {
						item = getSelectedItem(true);		
						doPreview(item);
					}
				}
			}			
		}
	}

	/**
	 * Remove the list items and clear image cache of ResourceManager
	 */
	protected void dispose() {
		items.removeAllElements();
		list.deleteAll();
		this.list.removeCommand(add2CartCommand);
		this.list.removeCommand(previewCommand);
	}

	public int getFormHeight() {
		return this.list.getHeight();
	}

	public int getFormWidth() {
		return this.list.getWidth();
	}

	protected Displayable getScreen() {
		return this.list;
	}

	protected void refresh() {
		this.list.setTitle(getTitle());
		dispose();
		populateList(Engine.getEngine().getCurrentFiles(null));
		hideBusy();
	}

	public void show() {
		Engine.getEngine().open(null, this);
        getDisplay().setCurrent(this.list);
	}
	
	
	protected Vector items = new Vector();
	protected void populateList(Vector files) {
		ItemLight2 item = null;
		int filesCounter = 0;
		for (Enumeration en = files.elements(); en.hasMoreElements();) {
			FileInfoWrapper file = (FileInfoWrapper) en.nextElement();
			item = new ItemLight2(!file.isRoot()? file.getFileName() :file.getFileName().substring(1), 
					file.getPath(), 
						file.isDirectory()? ItemLight2.IMAGE_FOLDER: ItemLight2.IMAGE_FILE, 
								file.isDirectory() || file.isRoot() ? ItemLight2.TYPE_FOLDER: ItemLight2.TYPE_FILE
										);
			int index = list.append(item.getTitle(), item.getIcon());
			item.setIdOnForm(index);
			if (item.getType() == ItemLight2.TYPE_FILE) {
				filesCounter++;
			}
			items.addElement(item);
		}
		addFileCommands();
		item = getFileSelectorBean().getSelected();
		int index = -1;
		if (item != null && (index = items.indexOf(item)) != -1) {
			list.setSelectedIndex(index, true);
		}
	}
	
	

	private class OptionsScreen implements CommandListener, ItemStateListener {
		
		private static final byte ACTION_BACK = -1;
		private static final byte ACTION_PREVIEW = 2;
		private static final byte ACTION_ADD_CARD = 3;
		
		private final Form form; 
		private final ChoiceGroup choiceGroup; 
		private final Command selectCommand;
		private Vector actionItems = new Vector();
		
		OptionsScreen(Resoursable resoursable) {
			this.form = new Form(getLocalizedText(LocaleUI.FORM_BROWS_OPTIONS_CAPTION));
			this.choiceGroup = new ChoiceGroup(null, ChoiceGroup.EXCLUSIVE);
			selectCommand = new Command(getLocalizedText(LocaleUI.FORM_BROWS_OPTIONS_COMMAND_SELECT), Command.OK, 0);
			//create;
			
			createItem(getLocalizedText(LocaleUI.FORM_BROWS_OPTIONS_COMMAND_BACK), ACTION_BACK);
			createItem(getLocalizedText(LocaleUI.FORM_BROWS_OPTIONS_COMMAND_PREVIEW), ACTION_PREVIEW);			
			createItem(getLocalizedText(LocaleUI.FORM_BROWS_OPTIONS_COMMAND_ADD_CARD), ACTION_ADD_CARD);
			//add to form
			for (Enumeration e = actionItems.elements(); e.hasMoreElements();) {
				ActionItem item = (ActionItem)e.nextElement();
				int index = this.choiceGroup.append(item.getTitle(), item.getIcon());
				item.setIdOnForm(index);
			}
			form.append(choiceGroup);
			choiceGroup.setDefaultCommand(selectCommand);
			this.form.setItemStateListener(this);
			this.form.setCommandListener(this);		
		}

		public void show() {
			getDisplay().setCurrent(this.form);
		}
		
		private void hide() {
			FileSelectorScreen2.this.show();
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
			case ACTION_PREVIEW:
				doPreview(getSelectedItem(false));
				break;
			case ACTION_ADD_CARD:
				add2Card(getSelectedItem(false));	
				this.hide();
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