/* ------------------------------------------------
 * HttpRequestBuilderTest.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader.webservice;

import java.io.UnsupportedEncodingException;
import java.net.URLEncoder;
import org.junit.*;
import static org.junit.Assert.*;

/**
 * @author Alex Nesterov
 */
public class HttpRequestBuilderTest
{
    private HttpRequestBuilder _builder;

    @Before
    public void setUp()
    {
	_builder = new HttpRequestBuilder();
    }

    @Test
    public void emptyRequest()
    {
	assertEquals("", _builder.getRequest());
    }

    @Test
    public void addParam() throws UnsupportedEncodingException
    {
	_builder.addParam("param", "value");
	assertEquals("param=value", _builder.getRequest());
    }

    @Test
    public void addParamSome() throws UnsupportedEncodingException
    {
	_builder.addParam("param", "value").addParam("param2", "value2");
	assertEquals("param=value&param2=value2", _builder.getRequest());
    }

    @Test
    public void addParamSome2() throws UnsupportedEncodingException
    {
	_builder.addParam("param", "value");
	_builder.addParam("param2", "value2");
	assertEquals("param=value&param2=value2", _builder.getRequest());
    }

    @Test(expected = IllegalArgumentException.class)
    public void addParamIllegalArgumentExceptionParam0() throws UnsupportedEncodingException
    {
	_builder.addParam(null, "value");
    }

    @Test(expected = IllegalArgumentException.class)
    public void addParamIllegalArgumentExceptionParam1() throws UnsupportedEncodingException
    {
	_builder.addParam("param", null);
    }

    @Test
    public void addParamUTFEncoding() throws UnsupportedEncodingException
    {
	_builder.addParam("параметр", "значение");

	String encodedParam = getEncodedString("параметр");
	String encodedValue = getEncodedString("значение");
	assertEquals(encodedParam + "=" + encodedValue, _builder.getRequest());
    }

    @Test
    public void addParamSomeUTFEncoding() throws UnsupportedEncodingException
    {
	_builder.addParam("параметр", "значение");
	_builder.addParam("параметр2", "значение2");

	String encodedParam = getEncodedString("параметр");
	String encodedParam2 = getEncodedString("параметр2");
	String encodedValue = getEncodedString("значение");
	String encodedValue2 = getEncodedString("значение2");
	assertEquals(encodedParam + "=" + encodedValue + "&" + encodedParam2 + "=" + encodedValue2,
		     _builder.getRequest());
    }

    @Test
    public void toStringVersusGetRequest() throws UnsupportedEncodingException
    {
	_builder.addParam("param", "value");
	_builder.addParam("param2", "value2");
	assertEquals(_builder.toString(), _builder.getRequest());
    }

    private static String getEncodedString(String str) throws UnsupportedEncodingException
    {
	return URLEncoder.encode(str, "UTF-8");
    }

}
