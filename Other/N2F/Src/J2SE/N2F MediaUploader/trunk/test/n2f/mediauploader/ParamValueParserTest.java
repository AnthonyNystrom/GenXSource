/* ------------------------------------------------
 * ParamValueParserTest.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

import java.util.HashMap;
import java.util.Map;
import org.jmock.*;
import org.junit.*;
import static org.junit.Assert.*;

/**
 * @author Alex Nesterov
 */
public final class ParamValueParserTest
{
    private static final Map<String, String> _paramValueMap =
	    new HashMap<String, String>();

    static
    {
	_paramValueMap.put("galleryName::0", "Gallery0");
	_paramValueMap.put("galleryName::1", "Gallery1");
	_paramValueMap.put("galleryID::0", "0");
	_paramValueMap.put("galleryID::1", "1");
    }

    private IParamValueProvider _stub;

    @Before
    public void setUp()
    {
	_stub = new IParamValueProvider()
	{
	    public String getValueFor(String param)
	    {
		return _paramValueMap.get(param);
	    }

	};
    }

    @Test
    public void getGenericParamValues()
    {
	String[] galleryNames =
		ParamValueParser.getGenericParamValues(_stub,
						       "galleryName",
						       "::");

	int galleryNameCount = 2;
	assertEquals(galleryNameCount, galleryNames.length);

	for (int i = 0; i < galleryNameCount; i++)
	{
	    assertEquals(String.format("Gallery%s", i), galleryNames[i]);
	}
    }

    @Test(expected = IllegalArgumentException.class)
    public void getGenericParamValuesIllegalArgumentExceptionParam0()
    {
	ParamValueParser.getGenericParamValues(null, "galleryName", "::");
    }

    @Test(expected = IllegalArgumentException.class)
    public void getGenericParamValuesIllegalArgumentExceptionParam1()
    {
	ParamValueParser.getGenericParamValues(_stub, null, "::");
    }

    @Test(expected = IllegalArgumentException.class)
    public void getGenericParamValuesIllegalArgumentExceptionParam2()
    {
	ParamValueParser.getGenericParamValues(_stub, "galleryName", null);
    }

}
