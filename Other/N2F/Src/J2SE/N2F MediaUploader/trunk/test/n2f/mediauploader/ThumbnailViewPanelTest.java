/* ------------------------------------------------
 * ThumbnailViewPanelTest.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

import java.awt.Dimension;
import org.junit.*;
import static org.junit.Assert.*;

/**
 * @author Alex Nesterov
 */
public class ThumbnailViewPanelTest
{
    @Test(expected = IllegalArgumentException.class)
    public void ctorIllegalArgumentExceptionParam0()
    {
	new ThumbnailViewPanel(null, new PhotoProcessorStubAdapter());
    }

    @Test(expected = IllegalArgumentException.class)
    public void ctorIllegalArgumentExceptionParam1()
    {
	new ThumbnailViewPanel(new Dimension(10, 10), null);
    }

}
