/* ------------------------------------------------
 * HttpRequestBuilder.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader.webservice;

import java.io.UnsupportedEncodingException;
import java.net.URLEncoder;
import static n2f.mediauploader.resources.ExceptionResources.*;

/**
 * Provides functionality to build HTTP request in a chaining manner. Both
 * parameter and value are encoded in <tt>UTF-8</tt> using {@link URLEncoder}.
 * <br/>
 * Example output:
 * <br/>
 * <code>
 * param=value&param=value2&param3=value3
 * </code>
 * 
 * @author Alex Nesterov
 */
class HttpRequestBuilder
{
    private StringBuffer _request;

    /**
     * Creates a new instance of the <tt>HttpRequestBuilder</tt> class.
     */
    public HttpRequestBuilder()
    {
	_request = new StringBuffer();
    }

    /**
     * Appends the specified parameter and value to the request.
     * 
     * @param	param
     *		Specifies the name of the parameter.
     * @param	value
     *		Specifies the value for the parameter.
     * @return	HttpRequestBuilder instance to make available chaining calls.
     * @throws	IllegalArgumentException
     *		If the specified <tt>param</tt> is <code>null</code>, or 
     *		if the specified <tt>value</tt> is <code>null</code>.
     * @throws	UnsupportedEncodingException
     *		If UTF-8 encoding is not supported.
     */
    public HttpRequestBuilder addParam(String param, String value) throws UnsupportedEncodingException
    {
	if (param == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull, "param"));
	if (value == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull, "value"));

	if (_request.length() > 0)
	    _request.append("&");

	_request.append(getEncodedString(param)).
		append("=").
		append(getEncodedString(value));
	
	return this;
    }

    /**
     * Returns the concantenated HTTP request.
     * @return	Concatenated HTTP request.
     */
    public String getRequest()
    {
	return _request.toString();
    }
    
    /**
     * 
     * @return
     */
    @Override
    public String toString()
    {
	return getRequest();
    }

    /**
     * Returns UTF-8 encoded string that is valid for HTTP request.
     * @param	str
     *		Specifies the string to encode.
     * @return	UTF-8 encoded string that is valid for HTTP request.
     * @throws	UnsupportedEncodingException
     *		If UTF-8 encoding is not supported.
     */
    private static String getEncodedString(String str) throws UnsupportedEncodingException
    {
	return URLEncoder.encode(str, "UTF-8");
    }

}
