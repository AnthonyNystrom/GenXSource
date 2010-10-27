/* ------------------------------------------------
 * ExtensionFilterTest.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

import org.junit.*;
import static org.junit.Assert.*;

/**
 * @author Alex Nesterov
 */
public class ExtensionFilterTest
{
    private static final String[] _invalidFiles = new String[] {
	"C:\\image", "C:\\image.gif", "C:\\image.jpg.gif"
    };
    private static final String[] _validFiles = new String[] {
	"C:\\image.jpg", "C:\\image.JPG", "C:\\image.JpG", "C:\\image.gif.jpg"
    };
    private static final String[] _validExtensions = new String[] { "jpg" };

    @Test
    public void checkFileNullExtensions()
    {
	String[] nullExtensions = new String[] { null };

	for (final String file : _validFiles)
	{
	    assertFalse(ExtensionFilter.checkFile(file, nullExtensions));
	}
    }

    @Test(expected = IllegalArgumentException.class)
    public void checkFileIllegalArgumentExceptionParam0()
    {
	ExtensionFilter.checkFile(null, _validExtensions);
    }

    @Test(expected = IllegalArgumentException.class)
    public void checkFileIllegalArgumentExceptionParam1()
    {
	ExtensionFilter.checkFile("image.jpg", null);
    }

    @Test
    public void checkFileValid()
    {
	for (final String file : _validFiles)
	{
	    assertTrue(ExtensionFilter.checkFile(file, _validExtensions));
	}
    }

    @Test
    public void checkFileInvalid()
    {
	for (final String file : _invalidFiles)
	{
	    assertFalse(ExtensionFilter.checkFile(file, _validExtensions));
	}
    }

}
