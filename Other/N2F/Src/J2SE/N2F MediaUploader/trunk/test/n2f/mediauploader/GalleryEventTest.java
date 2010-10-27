/* ------------------------------------------------
 * GalleryEventTest.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

import org.junit.*;
import static org.junit.Assert.*;

/**
 * @author Alex Nesterov
 */
public final class GalleryEventTest
{
    private GalleryDescriptor _descriptor;

    @Before
    public void setUp()
    {
	_descriptor = new GalleryDescriptor("Gallery", "123");
    }

    @Test(expected = IllegalArgumentException.class)
    public void ctor_NullArg0()
    {
	new GalleryEvent(null,
			 GalleryEventType.NewGalleryAdded,
			 _descriptor);
    }

    @Test(expected = IllegalArgumentException.class)
    public void ctor_NullArg1()
    {
	new GalleryEvent(this, null, _descriptor);
    }

    @Test(expected = IllegalArgumentException.class)
    public void ctor_NullArg2()
    {
	new GalleryEvent(this, GalleryEventType.NewGalleryAdded, null);
    }

    @Test
    public void getEventType_StdCall()
    {
	GalleryEvent event = new GalleryEvent(this,
					      GalleryEventType.NewGalleryAdded,
					      _descriptor);
	assertEquals(GalleryEventType.NewGalleryAdded, event.getEventType());

    }

    @Test
    public void getGalleryDescriptor_StdCall()
    {
	GalleryEvent event = new GalleryEvent(this,
					      GalleryEventType.NewGalleryAdded,
					      _descriptor);
	assertEquals(_descriptor, event.getGalleryDescriptor());
    }

    @Test
    public void equals_StdCall()
    {
	GalleryEvent event =
		new GalleryEvent(this, GalleryEventType.DefaultGalleryChanged,
				 _descriptor);
	GalleryEvent event2 =
		new GalleryEvent(this, GalleryEventType.DefaultGalleryChanged,
				 _descriptor);

	assertEquals(event, event2);
    }

    @Test
    public void equals_Mistype()
    {
	Object o = new Object();
	GalleryEvent event =
		new GalleryEvent(this, GalleryEventType.DefaultGalleryChanged,
				 _descriptor);
	assertFalse(event.equals(o));
    }

    @Test
    public void equals_NullArg()
    {
	GalleryEvent event =
		new GalleryEvent(this, GalleryEventType.DefaultGalleryChanged,
				 _descriptor);
	assertFalse(event.equals(null));
    }

}
