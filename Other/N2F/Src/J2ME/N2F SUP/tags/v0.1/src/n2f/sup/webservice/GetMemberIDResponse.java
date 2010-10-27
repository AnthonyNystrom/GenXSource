/* ------------------------------------------------
 * GetMemberIDResponse.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.sup.webservice;

/**
 * @author Alex Nesterov
 */
public class GetMemberIDResponse
{ 
    private String _getMemberIDResult;
    
    public String getGetMemberIDResult()
    {
	return _getMemberIDResult;
    }
    
    public void setGetMemberIDResult(String getMemberIDResult)
    {
	_getMemberIDResult = getMemberIDResult;
    }
    
    public GetMemberIDResponse()
    {
    }
    
    public GetMemberIDResponse(String getMemberIDResult)
    {
	_getMemberIDResult = getMemberIDResult;
    }
}
