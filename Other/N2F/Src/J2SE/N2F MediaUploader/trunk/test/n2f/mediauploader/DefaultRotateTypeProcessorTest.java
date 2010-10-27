/* ------------------------------------------------
 * DefaultRotateTypeProcessorTest.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

import n2f.mediauploader.drawing.RotateType;
import org.junit.*;
import static org.junit.Assert.*;

/**
 * @author Alex Nesterov
 */
public class DefaultRotateTypeProcessorTest
{
    private IRotateTypeProcessor _processor;

    @Before
    public void setUp()
    {
	_processor = new DefaultRotateTypeProcessor();
    }

    @Test
    public void combineRotateTypes()
    {
	verifyCombine(RotateType.Rotate180, RotateType.RotateNone,
		      RotateType.Rotate180);
	verifyCombine(RotateType.Rotate180, RotateType.Rotate90,
		      RotateType.Rotate90);
	verifyCombine(RotateType.RotateNone, RotateType.Rotate90,
		      RotateType.Rotate270);
	verifyCombine(RotateType.Rotate90, RotateType.Rotate270,
		      RotateType.Rotate180);
    }

    private void verifyCombine(RotateType expected, RotateType x, RotateType y)
    {
	assertEquals(expected, _processor.combineRotateTypes(x, y));
    }

    @Test(expected = IllegalArgumentException.class)
    public void combineRotateTypesIllegalArgumentExceptionParam0()
    {
	_processor.combineRotateTypes(null, RotateType.RotateNone);
    }

    @Test(expected = IllegalArgumentException.class)
    public void combineRotateTypesIllegalArgumentExceptionParam1()
    {
	_processor.combineRotateTypes(RotateType.RotateNone, null);
    }

}
