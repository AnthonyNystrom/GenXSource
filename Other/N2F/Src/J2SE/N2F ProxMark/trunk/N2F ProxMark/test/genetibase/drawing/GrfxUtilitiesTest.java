/* ------------------------------------------------
 * GrfxUtils.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.drawing;

import java.awt.Dimension;
import java.awt.Rectangle;
import java.awt.image.BufferedImage;
import org.jmock.*;
import org.jmock.lib.legacy.ClassImposteriser;
import org.junit.*;
import static org.junit.Assert.*;

public final class GrfxUtilitiesTest
{
    private Mockery _context;

    @Before
    public void setUp()
    {
	_context = new Mockery()
	{
	    {
		setImposteriser(ClassImposteriser.INSTANCE);
	    }

	};
    }

    @Test(expected = IllegalArgumentException.class)
    public void rotateImageIllegalArgumentExceptionParam0()
    {
	GrfxUtils.rotateImage(null, RotateType.RotateNone);
    }

    @Test(expected = IllegalArgumentException.class)
    public void rotateImageIllegalArgumentExceptionParam1()
    {
	GrfxUtils.rotateImage(_context.mock(BufferedImage.class), null);
    }

    @Test(expected = IllegalArgumentException.class)
    public void scaleToFitIllegalArgumentExceptionParam0()
    {
	GrfxUtils.scaleToFit(null, _context.mock(Dimension.class));
    }

    @Test(expected = IllegalArgumentException.class)
    public void scaleToFitIllegalArgumentExceptionParam1()
    {
	GrfxUtils.scaleToFit(_context.mock(Rectangle.class), null);
    }

    @Test
    public void scaleToFitEmptyRectangle()
    {
	assertTrue(GrfxUtils.scaleToFit(new Rectangle(0, 0, 0, 0),
					new Dimension(640, 480)).isEmpty());
    }

}
