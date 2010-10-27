/* ------------------------------------------------
 * WebServiceException.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader.webservice;

/**
 * A checked exception that is thrown if error occures while web-service invokation.
 * @author Alex Nesterov
 */
public class WebServiceException
	extends Exception
{
    /**
     * Creates a new instance of the <tt>WebServiceException</tt> class.
     */
    public WebServiceException()
    {
	super();
    }
    
    /**
     * Creates a new instance of the <tt>WebServiceException</tt> class.
     * @param	msg
     *		Specifies the message to associate with this exception.
     */
    public WebServiceException(String msg)
    {
	super(msg);
    }
    
    /**
     * Creates a new instance of the <tt>WebServiceException</tt> class.
     * @param	msg
     *		Specifies the message to associate with this exception.
     * @param	cause
     *		Specifies the inner exception for this exception.
     */
    public WebServiceException(String msg, Throwable cause)
    {
	super(msg, cause);
    }
    
    /**
     * Creates a new instance of the <tt>WebServiceException</tt> class.
     * @param	cause
     *		Specifies the inner exception for this exception.
     */
    public WebServiceException(Throwable cause)
    {
	super(cause);
    }
}
