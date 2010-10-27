package com.genetibase.askafriend.common.network;

import com.genetibase.askafriend.common.network.stub.MemberServicesSoap;
import com.genetibase.askafriend.common.network.stub.MemberServicesSoap_Stub;
import com.genetibase.askafriend.core.Engine;
import com.genetibase.askafriend.ui.GUIListener;
import com.genetibase.askafriend.ui.SettingsBean;
import com.genetibase.askafriend.ui.UIManager;

public class GetUserIdTask extends NetworkServiceTaskAdapter {

	protected GetUserIdTask(int operationType, GUIListener listener, NetworkServiceLogics logic) {
		super(operationType, listener, logic, null);
	}

	protected void logic() throws Exception {
		MemberServicesSoap soap = new MemberServicesSoap_Stub();
		SettingsBean bean = (SettingsBean) Engine.getEngine().getBean(UIManager.SCREEN_SETTINGS);
		Object resultObj = null;
		System.out.println("login to encrypt="+bean.getLogin());
		System.out.println("pass="+bean.getPassword());
		resultObj = soap.getMemberID(bean.getLogin(), bean.getPassword());
		if (handler != null)
			handler.analyzeServiceResponse(new ServiceResponse(operationType, resultObj), this);
	}
}
