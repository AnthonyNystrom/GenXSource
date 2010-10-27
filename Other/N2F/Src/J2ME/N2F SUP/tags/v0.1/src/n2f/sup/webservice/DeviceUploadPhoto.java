/* ------------------------------------------------
 * DeviceUploadPhoto.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.sup.webservice;

/**
 * @author Alex Nesterov
 */
public class DeviceUploadPhoto
{
    /* ------ Methods.Public ------ */
    
    public String getWebMemberID()
    {
	return _webMemberID;
    }
    
    public void setWebMemberID(String webMemberID)
    {
	_webMemberID = webMemberID;
    }
    
    public String getWebPassword()
    {
	return _webPassword;
    }
    
    public void setWebPassword(String webPassword)
    {
	_webPassword = webPassword;
    }
    
    public String getBase64StringPhoto()
    {
	return _base64StringPhoto;
    }
    
    public void setBase64StringPhoto(String base64StringPhoto)
    {
	_base64StringPhoto = base64StringPhoto;
    }
    
    public String getDateTime()
    {
	return _dateTime;
    }
    
    public void setDateTime(String dateTime)
    {
	_dateTime = dateTime;
    }
    
    /* ------ Declarations ------ */
    
    private String _webMemberID;
    private String _webPassword;
    private String _base64StringPhoto;
    private String _dateTime;
    
    /* ------ Constructors ------ */
    
    public DeviceUploadPhoto()
    {
    }
    
    public DeviceUploadPhoto(String webMemberID, String webPassword, String base64StringPhoto, String dateTime)
    {
	_webMemberID = webMemberID;
	_webPassword = webPassword;
	_base64StringPhoto = base64StringPhoto;
	_dateTime = dateTime;
    }
}
