package n2f.tag.webservice;

import n2f.tag.ui.GUIListener;
import n2f.tag.ui.SettingsBean;
import n2f.tag.ui.UIManager;
import n2f.tag.webservice.stub.BTTagSendSoap;
import n2f.tag.webservice.stub.BTTagSendSoap_Stub;
import n2f.tag.wireless.TagKeeper;

public class UploadTagTask extends NetworkServiceTaskAdapter {

	protected UploadTagTask(int operationType, GUIListener listener, WebServiceInteractor logic) {
		super(operationType, listener, logic);
	}

	protected void logic() throws Exception {
		BTTagSendSoap soap = new BTTagSendSoap_Stub();
		SettingsBean bean = (SettingsBean) UIManager.getInstance().getBean(UIManager.SCREEN_SETTINGS);
		if (bean != null) {
			Object resultObj = soap.uploadBTTags(bean.getWebMemberId(), bean.getPassword(), TagKeeper.getInstance().read());
			if (handler != null) {
				handler.analyzeServiceResponse(resultObj, this);
			}
		}
		
	}

}
