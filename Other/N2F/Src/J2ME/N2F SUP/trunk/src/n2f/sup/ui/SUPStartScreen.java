package n2f.sup.ui;

import n2f.sup.common.Resoursable;
import n2f.sup.core.Engine;
import n2f.sup.ui.bean.CartBean;
import n2f.sup.ui.bean.StartBean;



class SUPStartScreen extends StartScreen2 {

	SUPStartScreen(String id, Resoursable resoursable) {
		super(id, resoursable);
	}
	
	protected String getListCaption() {
		return getLocalizedText(LocaleUI.FORM_SUP_START_CAPTION);
	}

	protected void doSettings(){
		UIManager.getInstance().show(UIManager.SCREEN_SUP_SETTINGS);		
	}

	protected void doBrowser(){
		UIManager.getInstance().show(UIManager.SCREEN_SUP_BROWSER);		
	}
	
	protected void openPictureCart(boolean next) {
		CartBean bean = (CartBean)Engine.getEngine().getBean(UIManager.SCREEN_CARD);
		if (bean == null) {
			Engine.getEngine().putBean(UIManager.SCREEN_CARD, bean = new CartBean(getResoursable()));
		}
		bean.setProceedCommandName(next);
		UIManager.getInstance().show(UIManager.SCREEN_SUP_CARD);
	}
	
	protected void addListItems() {
		bean = (StartBean)Engine.getEngine().getBean(UIManager.SCREEN_START);
		if (bean == null) {
			Engine.getEngine().putBean(UIManager.SCREEN_START, bean = new StartBean());
			createItem(getLocalizedText(LocaleUI.START_CITEM_UPLOAD), "/camera.png", ACTION_CARD, bean);//browser
			createItem(getLocalizedText(LocaleUI.START_CITEM_EXPLORE), "/folder.png", ACTION_BROWSER, bean);//browser
			createItem(getLocalizedText(LocaleUI.START_CITEM_CART), "/cart.png", ACTION_CARD, bean);//Card			

			createItem(getLocalizedText(LocaleUI.START_CITEM_SETTINGS), "/settings.png", ACTION_SETTINGS, bean); //settings

			createItem(getLocalizedText(LocaleUI.START_CITEM_EXIT), "/exit.png", ACTION_EXIT, bean);//exit
		} 
	}	
	
}
