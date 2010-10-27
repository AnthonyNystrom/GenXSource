package com.genetibase.askafriend.ui;

import java.util.Enumeration;

import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.Image;
import javax.microedition.lcdui.List;

//import com.genetibase.askafriend.camera.MediaUtils;
import com.genetibase.askafriend.common.Resoursable;
import com.genetibase.askafriend.core.ActionItem;
import com.genetibase.askafriend.core.CommonKeys;
import com.genetibase.askafriend.core.Engine;
import com.genetibase.askafriend.core.ItemBasis;
import com.genetibase.askafriend.ui.bean.StartBean;

class StartScreen2 extends AbstractWindow {
	private static final byte ACTION_EXIT = -1;
	private static final byte ACTION_BROWSER = 1;
	private static final byte ACTION_QUESTION = 2;
	private static final byte ACTION_RESPONSE = 3;
	private static final byte ACTION_SETTINGS = 4;
	private static final byte ACTION_CAMERA = 5;
	private static final byte ACTION_ASK_A_QUESTION = 6;
	private static final byte ACTION_CARD = 7;
	private static final byte ACTION_INVITE_A_FRIEND = 8;
	
	private final List list;
	private final Command select;
	private StartBean bean;
	
	StartScreen2(String id, Resoursable resoursable) {
		super(id, resoursable);
		this.list = new List(getLocalizedText(LocaleUI.FORM_START_CAPTION), List.IMPLICIT);
		this.select = new Command(getLocalizedText(LocaleUI.START_COMMAND_SELECT), Command.ITEM, 1);
		
		bean = (StartBean)Engine.getEngine().getBean(getId());
		if (bean == null) {
			Engine.getEngine().putBean(getId(), bean = new StartBean());
			createItem(getLocalizedText(LocaleUI.START_CITEM_ASK), "/ask.png", ACTION_ASK_A_QUESTION, bean);//ask a question 			
//			if (MediaUtils.supportsCapturing()) {
//				createItem(getLocalizedText(LocaleUI.START_CITEM_CAMERA), "/camera.png", ACTION_CAMERA, bean);
//			}
			createItem(getLocalizedText(LocaleUI.START_CITEM_EXPLORE), "/folder.png", ACTION_BROWSER, bean);//browser
			createItem(getLocalizedText(LocaleUI.START_CITEM_CART), "/cart.png", ACTION_CARD, bean);										//Card			

			createItem(getLocalizedText(LocaleUI.START_CITEM_PRIVATE_QUESTIONS), "/private.png", ACTION_QUESTION, bean);//provate questions
			createItem(getLocalizedText(LocaleUI.START_CITEM_RESPONSES), "/questions.png", ACTION_RESPONSE, bean);//responces
			createItem(getLocalizedText(LocaleUI.START_CITEM_SETTINGS), "/settings.png", ACTION_SETTINGS, bean); //settings
			
			createItem(getLocalizedText(LocaleUI.START_CITEM_INVITE), "/sms.png", ACTION_INVITE_A_FRIEND, bean); //settings

			createItem(getLocalizedText(LocaleUI.START_CITEM_EXIT), "/exit.png", ACTION_EXIT, bean);//exit
		} 
		
		for (Enumeration b = bean.getElements(); b.hasMoreElements();) {
			ActionItem item = (ActionItem)b.nextElement();
			int index = list.append(item.getTitle(), item.getIcon());
			item.setIdOnForm(index);
			if (item.isSelected()) {
				list.setSelectedIndex(index, true);
			}
		}
		
		this.list.setSelectCommand(select);
		this.list.setCommandListener(this);		 
		
	}
	
	private static void createItem(String text, String imageId, byte action, StartBean bean){
		ActionItem itemBasis = new ActionItem(text, imageId!= null ? getImage(imageId): null, action);
		bean.addItem(itemBasis);
	}
	
	private static Image getImage( String id) {
		return Engine.getEngine().getImage(id);
	} 

	public int getFormHeight() {
		return this.list.getHeight();
	}

	public int getFormWidth() {
		return this.list.getWidth();
	}

	protected Displayable getScreen() {
		return list;
	}

	protected void refresh() {
	}

	public void show() 
	{
        getDisplay().setCurrent(this.list);
        Engine.getEngine().initNetworkDispatch();
        
		if (UIManager.getInstance().isHasNewResponces()) {
			new Thread() {
				public void run() {
					showAlert(getLocalizedText(LocaleUI.REPLY_READY), COMMAND_REPLIES, COMMAND_CANCEL, StartScreen2.this);					
				}
			}.start();
		} else if (UIManager.getInstance().isHasNewQuestions()) {
			new Thread() {
				public void run() {
					showAlert(getLocalizedText(LocaleUI.QUESTION_READY), COMMAND_QUESTIONS, COMMAND_CANCEL, StartScreen2.this);
				}
			}.start();
		}
	}
	
	private void doAction(byte action) {
		switch (action) {
		case ACTION_EXIT:
			Engine.getEngine().exit();
			break;
		case ACTION_RESPONSE:
			UIManager.getInstance().show(UIManager.SCREEN_RESPONSES);
			break;
		case ACTION_SETTINGS:
			UIManager.getInstance().show(UIManager.SCREEN_SETTINGS);
			break;
		case ACTION_QUESTION:
			UIManager.getInstance().show(UIManager.SCREEN_PRIVATE_QUESTIONS);
			break;
		case ACTION_BROWSER:
			UIManager.getInstance().show(UIManager.SCREEN_FILE_SYSTEM);
			break;
		case ACTION_CARD:
			openPictureCart(false);
			break;
		case ACTION_ASK_A_QUESTION:
			openPictureCart(true);
			break;
		case ACTION_INVITE_A_FRIEND:
			UIManager.getInstance().show(UIManager.SCREEN_CONTACTS);
			break;
		case ACTION_CAMERA:
			String settings = getResoursable().getProperty(CommonKeys.KEY_SETTINGS_CAMERA);
			if (settings == null) {
				UIManager.getInstance().show(UIManager.SCREEN_CAMERA_SETTINGS);
			} else {
				UIManager.getInstance().show(UIManager.SCREEN_CAMERA);
			}			
			break;
		}
	} 
	
	private void openPictureCart(boolean next) {
		CartBean bean = (CartBean)Engine.getEngine().getBean(UIManager.SCREEN_CARD);
		if (bean == null) {
			Engine.getEngine().putBean(UIManager.SCREEN_CARD, bean = new CartBean(getResoursable()));
		}
		bean.setProceedCommandName(next);
		UIManager.getInstance().show(UIManager.SCREEN_CARD);
	}
	
	public void commandAction(Command c, Displayable d) {
		super.commandAction(c, d);
		if (c == select) {
			int index = list.getSelectedIndex();
			
			for (Enumeration e = bean.getElements(); e.hasMoreElements(); ){
				ItemBasis item = (ItemBasis)e.nextElement();
				item.setSelected(false);
			}
			ActionItem item = bean.getItem(index);
			item.setSelected(true);
			doAction(item.getAction());
		}
	}	

}
