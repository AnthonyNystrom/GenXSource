package n2f.tag.webservice;

import n2f.tag.ui.GUIListener;
import n2f.tag.ui.SettingsBean;
import n2f.tag.ui.UIManager;
import n2f.tag.webservice.stub.EditBlockListSoap;
import n2f.tag.webservice.stub.EditBlockListSoap_Stub;

public class GetBlockListTask extends NetworkServiceTaskAdapter {

	protected GetBlockListTask(int operationType, GUIListener listener, WebServiceInteractor logic) {
		super(operationType, listener, logic);
	}

	protected void logic() throws Exception 
	{
		SettingsBean bean = (SettingsBean) UIManager.getInstance().getBean(UIManager.SCREEN_SETTINGS);
		if (bean != null) {
			EditBlockListSoap soap = new EditBlockListSoap_Stub();
			Object resultObj = soap.getBlockList(bean.getWebMemberId(), bean.getPassword());
			if (handler != null) {
				handler.analyzeServiceResponse(resultObj, this);
			}
		}
	}

}
