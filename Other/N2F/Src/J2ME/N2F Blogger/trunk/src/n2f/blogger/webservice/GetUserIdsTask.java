
package n2f.blogger.webservice;

import n2f.blogger.ui.GUIListener;
import n2f.blogger.ui.SettingsBean;
import n2f.blogger.ui.UIManager;
import n2f.blogger.webservice.stub.MemberServicesSoap;
import n2f.blogger.webservice.stub.MemberServicesSoap_Stub;

public class GetUserIdsTask
	extends NetworkServiceTaskAdapter
{
    protected GetUserIdsTask(int operationType, GUIListener listener,
			      WebServiceInteractor logic)
    {
	super(operationType, listener, logic);
    }

    protected void logic() throws Exception
    {

	Object resultObj = null;
	MemberServicesSoap soap = new MemberServicesSoap_Stub();

	SettingsBean bean =
		(SettingsBean)UIManager.getInstance().getBean(UIManager.SCREEN_SETTINGS);
	System.out.println("login:" + bean.getLogin());
	System.out.println("passw:" + bean.getPassword());
	System.out.println("webid:" + bean.getWebMemberId());
	if (bean != null)
	{
	    switch (getType())
	    {
		case TYPE_GET_CREDENTIALS:
		    resultObj = soap.getMemberID(bean.getLogin(),
						 bean.getPassword());
		    break;
		default:
		    break;
	    }
	}
	if (handler != null)
	    handler.analyzeServiceResponse(resultObj, this);
    }

}
