/* ------------------------------------------------
 * PhotoOrganiseSoap.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.sup.webservice;

import java.rmi.*;

public interface PhotoOrganiseSoap extends Remote
{
    public DeviceUploadPhotoResponse deviceUploadPhoto(
	    String webMemberID
	    , String webPassword
	    , String base64StringPhoto
	    , String dateTime
	    ) throws RemoteException;   
}
