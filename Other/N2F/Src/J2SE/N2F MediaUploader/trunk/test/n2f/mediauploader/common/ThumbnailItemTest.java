/* ------------------------------------------------
 * ThumbnailItemTest.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader.common;

import java.awt.Dimension;
import java.io.File;
import n2f.mediauploader.IPhotoProcessor;
import n2f.mediauploader.PhotoProcessorStubAdapter;
import org.jmock.*;
import org.junit.*;
import static org.junit.Assert.*;

/**
 * @author Alex Nesterov
 */
public class ThumbnailItemTest
{
    private Dimension _dimension;
    private KeyValuePair<File> _leaf;
    private IPhotoProcessor _photoProcessor;
    private ThumbnailItem _thumbnailItem;

    @Before
    public void setUp()
    {
	_dimension = new Dimension(196, 96);
	_leaf = new KeyValuePair<File>("image", new File("C:\\image.jpg"));
	_photoProcessor = new PhotoProcessorStubAdapter();
	_thumbnailItem = new ThumbnailItem(_dimension,
					   _leaf,
					   _photoProcessor);
    }

    @Test(expected = IllegalArgumentException.class)
    public void ctorIllegalArgumentExceptionParam0()
    {
	new ThumbnailItem(null, _leaf, _photoProcessor);
    }

    @Test(expected = IllegalArgumentException.class)
    public void ctorIllegalArgumentExceptionParam1()
    {
	new ThumbnailItem(_dimension, null, _photoProcessor);
    }

    @Test(expected = IllegalArgumentException.class)
    public void ctorIllegalArgumentExceptionParam2()
    {
	new ThumbnailItem(_dimension, _leaf, null);
    }

    @Test(expected = IllegalArgumentException.class)
    public void addThumbnailListenerIllegalArgumentException()
    {
	_thumbnailItem.addThumbnailListener(null);
    }

    @Test
    public void isSelected()
    {
	assertFalse(_thumbnailItem.isSelected());
	_thumbnailItem.setSelected(true);
	assertTrue(_thumbnailItem.isSelected());
	_thumbnailItem.setSelected(false);
	assertFalse(_thumbnailItem.isSelected());
    }

    @Test
    public void getSetThumbnailDimension()
    {
	Dimension dimension = new Dimension(20, 30);
	_thumbnailItem.setThumbnailDimension(dimension);
	assertEquals(dimension, _thumbnailItem.getThumbnailDimension());
    }

    @Test(expected = IllegalArgumentException.class)
    public void setThumbnailDimensionIllegalArgumentException()
    {
	_thumbnailItem.setThumbnailDimension(null);
    }

    @Test
    public void thumbnailDimensionAffectsMinMaxPreferredSize()
    {
	Dimension dimension = new Dimension(20, 30);
	_thumbnailItem.setThumbnailDimension(dimension);

	assertEquals(dimension, _thumbnailItem.getPreferredSize());
	assertEquals(dimension, _thumbnailItem.getMaximumSize());
	assertEquals(dimension, _thumbnailItem.getMinimumSize());
    }

}
