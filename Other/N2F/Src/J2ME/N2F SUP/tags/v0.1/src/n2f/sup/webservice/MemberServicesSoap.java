/* ------------------------------------------------
 * MemberServicesSoap.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.sup.webservice;

import java.rmi.*;

/**
 * @author Alex Nesterov
 */
public interface MemberServicesSoap
	extends Remote
{
    public String getMemberID(String nickName, String webPassword) throws RemoteException;
}
