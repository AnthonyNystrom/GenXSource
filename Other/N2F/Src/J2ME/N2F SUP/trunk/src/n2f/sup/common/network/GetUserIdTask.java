package n2f.sup.common.network;

import n2f.sup.core.Engine;
import n2f.sup.ui.GUIListener;
import n2f.sup.ui.UIManager;
import n2f.sup.ui.bean.SettingsBean;
import n2f.sup.webservice.MemberServicesSoap;
import n2f.sup.webservice.MemberServicesStub;


public class GetUserIdTask extends NetworkServiceTaskAdapter {
	protected GetUserIdTask(int operationType, GUIListener listener, NetworkServiceLogics logic) {
		super(operationType, listener, logic);
	}

	protected void logic() throws Exception {
		MemberServicesSoap soap = new MemberServicesStub();
		SettingsBean bean = (SettingsBean) Engine.getEngine().getBean(UIManager.SCREEN_SETTINGS);
		Object resultObj = null;
		System.out.println("login to encrypt="+bean.getLogin());
		System.out.println("pass="+bean.getPassword());
		resultObj = soap.getMemberID(bean.getLogin(), bean.getPassword());
		if (handler != null)
			handler.analyzeServiceResponse(new ServiceResponse(operationType, resultObj), this);
	}
}
