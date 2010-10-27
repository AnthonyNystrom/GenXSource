package service;

//#ifdef BLACKBERRY
//# import net.rim.device.api.system.WLANInfo; 
//#endif

import App.*;
import java.io.IOException;

public class BaseService
{
    protected NetworkSystem ns;
    protected String url;
    protected OutputStream pos;
    protected InputStream pis;
    protected int requestId;

    public BaseService()
    {
	ns = new NetworkSystem();
	//url = "http://69.21.114.101:100/soap2bin.handler";
        url = "http://services.next2friends.com/n2fwebservices/soap2bin.handler";
//#ifdef BLACKBERRY
//#         if(WLANInfo.WLAN_STATE_CONNECTED == WLANInfo.getWLANState())
//#         {
//#             url += ";interface=wifi";
//#         } 
//#endif
    }

    public void prepare(int requestId)
    {
	pos = new OutputStream();
	pos.writeShort(Const.INVOCATION_CODE);
	pos.writeInt(requestId);
    }

    public void commit() throws IOException
    {
	pos.close();
	pis = ns.makeRequest(url, pos);

	short errorCode = pis.readShort();
	if(errorCode != 1)
	{
	    // there was a remote exception
	    String exception = pis.readString();
	    System.out.println(exception);
            throw new IOException();
	}

    }
}
