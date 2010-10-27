/* ------------------------------------------------
 * GalleryDescriptorTest.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

import org.junit.*;
import static org.junit.Assert.*;

/**
 * @author Alex Nesterov
 */
public class GalleryDescriptorTest
{
    private GalleryDescriptor _descriptor;

    @Before
    public void setUp()
    {
	_descriptor = new GalleryDescriptor("gallery", "123");
    }

    @Test(expected = IllegalArgumentException.class)
    public void ctorIllegalArgumentExceptionParam0()
    {
	new GalleryDescriptor(null, "123");
    }

    @Test(expected = IllegalArgumentException.class)
    public void ctorIllegalArgumentExceptionParam1()
    {
	new GalleryDescriptor("gallery", null);
    }

    @Test
    public void galleryName()
    {
	assertEquals("gallery", _descriptor.getGalleryName());
    }

    @Test
    public void galleryID()
    {
	assertEquals("123", _descriptor.getGalleryID());
    }

}
