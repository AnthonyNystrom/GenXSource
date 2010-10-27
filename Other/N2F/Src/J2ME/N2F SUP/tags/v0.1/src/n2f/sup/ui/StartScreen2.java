package n2f.sup.ui;

import java.util.Enumeration;

import javax.microedition.lcdui.Command;
import javax.microedition.lcdui.Displayable;
import javax.microedition.lcdui.Image;
import javax.microedition.lcdui.List;

import n2f.sup.common.Resoursable;
import n2f.sup.core.ActionItem;
import n2f.sup.core.Engine;
import n2f.sup.core.ItemBasis;
import n2f.sup.ui.bean.CartBean;
import n2f.sup.ui.bean.StartBean;

//import com.genetibase.askafriend.core.CommonKeys;

class StartScreen2 extends AbstractWindow {
	protected static final byte ACTION_EXIT = -1;
	protected static final byte ACTION_BROWSER = 1;
	private static final byte ACTION_QUESTION = 2;
	private static final byte ACTION_RESPONSE = 3;
	protected static final byte ACTION_SETTINGS = 4;
//	private static final byte ACTION_CAMERA = 5;
	private static final byte ACTION_ASK_A_QUESTION = 6;
	protected static final byte ACTION_CARD = 7;
	
	private final List list;
	private final Command select;
	protected StartBean bean;
	
	protected void addListItems() {
		bean = (StartBean)Engine.getEngine().getBean(UIManager.SCREEN_START);
		if (bean == null) {
			Engine.getEngine().putBean(UIManager.SCREEN_START, bean = new StartBean());
			createItem(getLocalizedText(LocaleUI.START_CITEM_ASK), "/ask.png", ACTION_ASK_A_QUESTION, bean);//ask a question 			
//			if (MediaUtils.supportsCapturing()) {
//				createItem(getLocalizedText(LocaleUI.START_CITEM_CAMERA), "/camera.png", ACTION_CAMERA, bean);
//			}
			createItem(getLocalizedText(LocaleUI.START_CITEM_EXPLORE), "/folder.png", ACTION_BROWSER, bean);//browser
			createItem(getLocalizedText(LocaleUI.START_CITEM_CART), "/cart.png", ACTION_CARD, bean);//Card			

			createItem(getLocalizedText(LocaleUI.START_CITEM_PRIVATE_QUESTIONS), "/private.png", ACTION_QUESTION, bean);//provate questions
			createItem(getLocalizedText(LocaleUI.START_CITEM_RESPONSES), "/questions.png", ACTION_RESPONSE, bean);//responces
			createItem(getLocalizedText(LocaleUI.START_CITEM_SETTINGS), "/settings.png", ACTION_SETTINGS, bean); //settings

			createItem(getLocalizedText(LocaleUI.START_CITEM_EXIT), "/exit.png", ACTION_EXIT, bean);//exit
		} 
		
	}
	
	protected String getListCaption() {
		return getLocalizedText(LocaleUI.FORM_START_CAPTION);		
	}
	
	StartScreen2(String id, Resoursable resoursable) {
		super(id, resoursable);
		this.list = new List(getListCaption(), List.IMPLICIT);
		this.select = new Command(getLocalizedText(LocaleUI.START_COMMAND_SELECT), Command.ITEM, 1);
		
		addListItems();
		
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
	
	protected static void createItem(String text, String imageId, byte action, StartBean bean){
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

	public void show() {
        getDisplay().setCurrent(this.list);
        Engine.getEngine().initNetworkDispatch();
	}
	
	protected void doSettings(){
		UIManager.getInstance().show(UIManager.SCREEN_SETTINGS);		
	}
	
	protected void doBrowser(){
//		UIManager.getInstance().show(UIManager.SCREEN_FILE_SYSTEM);		
	}
	
	private void doAction(byte action){
		if (action == ACTION_EXIT) {
			Engine.getEngine().exit();
//		} else if (action == ACTION_RESPONSE) {
//			UIManager.getInstance().show(UIManager.SCREEN_RESPONSES);
		} else if (action == ACTION_SETTINGS) {
			doSettings();
//		} else if (action == ACTION_QUESTION) {
//			UIManager.getInstance().show(UIManager.SCREEN_PRIVATE_QUESTIONS);
		} else if (action == ACTION_BROWSER) {
			doBrowser();
		} else if (action == ACTION_CARD) {
			openPictureCart(false);
		} else if (action == ACTION_ASK_A_QUESTION) {
			openPictureCart(true);
//		} else if (action == ACTION_CAMERA) {
//			String settings = getResoursable().getProperty(CommonKeys.KEY_SETTINGS_CAMERA);
//			if (settings == null) {
//				UIManager.getInstance().show(UIManager.SCREEN_CAMERA_SETTINGS);
//			} else {
//				UIManager.getInstance().show(UIManager.SCREEN_CAMERA);
//			}			
		}		
	} 
	
	protected void openPictureCart(boolean next) {
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
