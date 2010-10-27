/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package n2f.mediauploader.util;

import java.util.HashMap;
import java.util.Map;
import org.junit.*;
import static org.junit.Assert.*;

/**
 * @author Administrator
 */
public class ArgumentTest
{
    private static final Map<String, String> _getExtensionTestData =
	    new HashMap<String, String>();
    private static final Map<String, String> _getShortFileNameTestData =
	    new HashMap<String, String>();

    static
    {
	_getExtensionTestData.put("C:\\image.jpg", "jpg");
	_getExtensionTestData.put("C:\\image", "");
	_getExtensionTestData.put(".", "");
	_getExtensionTestData.put("", "");

	_getShortFileNameTestData.put("C:\\image.jpg", "image");
	_getShortFileNameTestData.put("C:\\image", "image");
	_getShortFileNameTestData.put("image.jpg", "image");
	_getShortFileNameTestData.put("image", "image");
	_getShortFileNameTestData.put("image.jpg.png", "image.jpg");
	_getShortFileNameTestData.put("", "");
	_getShortFileNameTestData.put(".", "");
	_getShortFileNameTestData.put("C:\\", "");
    }
    
    @Test
    public void getExtension()
    {
	for (final String key : _getExtensionTestData.keySet())
	{
	    assertEquals(_getExtensionTestData.get(key),
			 Argument.getExtension(key));
	}
    }

    @Test(expected = IllegalArgumentException.class)
    public void getExtensionIllegalArgumentException()
    {
	Argument.getExtension(null);
    }

    @Test
    public void getShortFileName()
    {
	for (final String key : _getShortFileNameTestData.keySet())
	{
	    assertEquals(_getShortFileNameTestData.get(key),
			 Argument.getShortFileName(key));
	}
    }

    @Test(expected = IllegalArgumentException.class)
    public void getShortFileNameIllegalArgumentException()
    {
	Argument.getShortFileName(null);
    }
}
