/* ------------------------------------------------
 * GetMemberID.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.sup.webservice;

/**
 * @author Alex Nesterov
 */
public class GetMemberID
{   
    private String _nickName;
    
    public String getNickName()
    {
	return _nickName;
    }
    
    public void setNickName(String nickName)
    {
	_nickName = nickName;
    }
    
    private String _webPassword;
    
    public String getWebPassword()
    {
	return _webPassword;
    }
    
    public void setWebPassword(String webPassword)
    {
	_webPassword = webPassword;
    }
    
    public GetMemberID()
    {
    }
    
    public GetMemberID(String nickName, String webPassword)
    {
	_nickName = nickName;
	_webPassword = webPassword;
    }
}
