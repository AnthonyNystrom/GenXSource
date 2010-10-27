/* ------------------------------------------------
 * GalleryModelTest.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

import java.util.List;
import org.junit.*;
import org.jmock.*;
import static org.junit.Assert.*;
import static org.hamcrest.CoreMatchers.*;

/**
 * @author Alex Nesterov
 */
public final class GalleryModelTest
{
    private GalleryModel _model;

    @Before
    public void setUp()
    {
	_model = new GalleryModel();
    }

    @Test
    public void addGallery_String_StdCase()
    {
	_model.addGallery("Gallery", "123");
	assertEquals(1, _model.getGalleries().size());
    }

    public void addGallery_GalleryDescriptor_StdCase()
    {
	GalleryDescriptor descriptor = new GalleryDescriptor("Gallery", "123");
	_model.addGallery(descriptor);
	assertEquals(1, _model.getGalleries().size());
    }

    @Test(expected = IllegalArgumentException.class)
    public void addGallery_String_NullArg0()
    {
	_model.addGallery(null, "123");
    }

    @Test(expected = IllegalArgumentException.class)
    public void addGallery_String_NullArg1()
    {
	_model.addGallery("Gallery", null);
    }

    @Test(expected = IllegalArgumentException.class)
    public void addGallery_GalleryDescriptor_NullArg()
    {
	_model.addGallery(null);
    }

    @Test
    public void addGalleryMany_StdCase()
    {
	String[] galleryNames = new String[] { "Gallery", "Another Gallery" };
	String[] galleryIDs = new String[] { "123", "345" };

	_model.addGalleryMany(galleryNames, galleryIDs);
	List<GalleryDescriptor> galleries = _model.getGalleries();

	assertEquals(2, galleries.size());
	assertEquals("Gallery", galleries.get(0).getGalleryName());
	assertEquals("123", galleries.get(0).getGalleryID());
	assertEquals("Another Gallery", galleries.get(1).getGalleryName());
	assertEquals("345", galleries.get(1).getGalleryID());
    }

    @Test(expected = IllegalArgumentException.class)
    public void addGalleryMany_NullArg0()
    {
	_model.addGalleryMany(null, new String[] {});
    }

    @Test(expected = IllegalArgumentException.class)
    public void addGalleryMany_NullArg1()
    {
	_model.addGalleryMany(new String[] {}, null);
    }

    @Test(expected = IllegalArgumentException.class)
    public void addGalleryMany_DifferentArrays()
    {
	String[] galleryNames =
		new String[] { "Gallery", "Another Gallery" };
	String[] galleryIDs = new String[] { "123" };
	_model.addGalleryMany(galleryNames, galleryIDs);
    }

    @Test
    public void addGalleryModelListener_StdCase()
    {
	_model.addGalleryModelListener(
		new IGalleryModelListener()
		{
		    public void galleryModelChanged(GalleryEvent e)
		    {
			GalleryEventType eventType = e.getEventType();

			switch (eventType)
			{
			    case NewGalleryAdded:
			    {
			    }
			    case DefaultGalleryChanged:
			    {
			    }
			}
		    }

		});
    }

    @Test(expected = IllegalArgumentException.class)
    public void addGalleryModelListener_NullArg()
    {
	_model.addGalleryModelListener(null);
    }

    @Test
    public void getCurrentGallery_DefaultState()
    {
	assertNull(_model.getCurrentGallery());
    }
    
    @Test
    public void getCurrentGallery_DefaultNotSet()
    {
	GalleryDescriptor descriptor = new GalleryDescriptor("Gallery", "123");
	_model.addGallery(descriptor);
	assertEquals(descriptor, _model.getCurrentGallery());
    }
    
    @Test
    public void getCurrentGallery_DefaultWasSet()
    {
	GalleryDescriptor descriptor = new GalleryDescriptor("Gallery", "123");
	GalleryDescriptor descriptor2 = new GalleryDescriptor("Another Gallery", "345");
	_model.addGallery(descriptor);
	_model.addGallery(descriptor2);
	_model.setDefaultGallery("345");
	assertEquals(descriptor2, _model.getCurrentGallery());
    }
    
    @Test
    public void getCurrentGallery_CurrentWasSet()
    {
	GalleryDescriptor descriptor = new GalleryDescriptor("Gallery", "123");
	GalleryDescriptor descriptor2 = new GalleryDescriptor("Another Gallery", "345");
	_model.addGallery(descriptor);
	_model.addGallery(descriptor2);
	assertEquals(descriptor, _model.getCurrentGallery());
	_model.setCurrentGallery(descriptor2);
	assertEquals(descriptor2, _model.getCurrentGallery());
    }
    
    @Test
    public void getDefaultGallery_DefaultState()
    {
	assertNull(_model.getDefaultGallery());
    }

    @Test
    public void getDefaultGallery_StdCase()
    {
	_model.addGallery("Gallery", "123");
	_model.addGallery("Another Gallery", "345");
	_model.setDefaultGallery("123");
	GalleryDescriptor defaultGallery = _model.getDefaultGallery();
	assertEquals("Gallery", defaultGallery.getGalleryName());
	assertEquals("123", defaultGallery.getGalleryID());
    }

    @Test
    public void galleryModelChanged_DefaultGalleryChanged()
    {
	Mockery context = new Mockery();

	final GalleryDescriptor galleryDescriptor =
		new GalleryDescriptor("Gallery",
				      "123");
	final IGalleryModelListener listener =
		context.mock(IGalleryModelListener.class);
	final Sequence sequence = context.sequence("galleryModelChanged");

	context.checking(
		new Expectations()
		{
		    {
			one(listener).galleryModelChanged(
				new GalleryEvent(_model,
						 GalleryEventType.NewGalleryAdded,
						 galleryDescriptor));
			inSequence(sequence);
			one(listener).galleryModelChanged(
				new GalleryEvent(_model,
						 GalleryEventType.DefaultGalleryChanged,
						 galleryDescriptor));
			inSequence(sequence);
		    }

		});

	_model.addGalleryModelListener(listener);
	_model.addGallery(galleryDescriptor);
	_model.setDefaultGallery("123");

	context.assertIsSatisfied();
    }

    @Test
    public void galleryModelChanged_GalleryAdded()
    {
	Mockery context = new Mockery();

	final IGalleryModelListener listener =
		context.mock(IGalleryModelListener.class);
	final GalleryDescriptor descriptor = new GalleryDescriptor("Gallery",
								    "123");
	final GalleryDescriptor descriptor2 = new GalleryDescriptor("Another Gallery",
								     "345");

	context.checking(
		new Expectations()
		{
		    {
			one(listener).galleryModelChanged(
				new GalleryEvent(_model,
						 GalleryEventType.NewGalleryAdded,
						 descriptor));
		    }

		});

	_model.addGalleryModelListener(listener);
	_model.addGallery(descriptor);
	_model.removeGalleryModelListener(listener);
	_model.addGallery(descriptor2);

	context.assertIsSatisfied();
    }
    
    @Test(expected = IllegalArgumentException.class)
    public void setCurrentGallery_NullArg()
    {
	_model.setCurrentGallery(null);
    }
    
    @Test(expected = IllegalArgumentException.class)
    public void setCurrentGallery_ThatDoesNotExist()
    {
	_model.setCurrentGallery(new GalleryDescriptor("Gallery", "123"));
    }

    @Test(expected = IllegalArgumentException.class)
    public void setDefaultGallery_NullArg()
    {
	_model.setDefaultGallery(null);
    }

    @Test(expected = IllegalArgumentException.class)
    public void setDefaultGallery_ThatDoesNotExist()
    {
	_model.setDefaultGallery("123");
    }

}
