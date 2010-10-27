/* ------------------------------------------------
 * MediaUploaderModelTest.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

import org.junit.*;
import static org.junit.Assert.*;

/**
 * @author Alex Nesterov
 */
public class DefaultPhotoProcessorTest
{
    private DefaultPhotoProcessor _photoProcessor;

    @Before
    public void setUp()
    {
	_photoProcessor = new DefaultPhotoProcessor(new MemberAccount("", new GalleryModel()));
    }

    @Test(expected = IllegalArgumentException.class)
    public void ctorIllegalArgumentException()
    {
	new DefaultPhotoProcessor(null);
    }
    
    @Test(expected = IllegalArgumentException.class)
    public void addPropertyChangeListener_NullArg()
    {
	_photoProcessor.addPropertyChangeListener(null);
    }

    @Test(expected = IllegalArgumentException.class)
    public void uploadPhotosIllegalArgumentException() throws PhotoProcessorException
    {
	_photoProcessor.uploadPhotos(null);
    }
}
