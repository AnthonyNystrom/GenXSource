package n2f.sup.ui;

import n2f.sup.common.Resoursable;
import n2f.sup.core.Engine;
import n2f.sup.ui.bean.SettingsBean;


class SUPSettingsScreen extends SettingsScreen {

	SUPSettingsScreen(String id, Resoursable resoursable) {
		super(id, resoursable);
	}

	protected void showDefault() {
		UIManager.getInstance().show(UIManager.SCREEN_SUP_START);
	}

	protected void refresh() {
		hideBusy();
		bean = (SettingsBean) Engine.getEngine().getBean(UIManager.SCREEN_SETTINGS);
		bean.saveBean(getResoursable());
		Engine.getEngine().setAuthorized(true);
		
		UIManager.getInstance().show(UIManager.SCREEN_SUP_START, false);
	}
	
	
	

}
