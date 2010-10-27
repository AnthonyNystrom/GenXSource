/* ------------------------------------------------
 * ThumbnailEventTest.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader.common;

import org.junit.*;
import static org.junit.Assert.*;

/**
 * @author Alex Nesterov
 */
public class ThumbnailEventTest
{
    @Test(expected = IllegalArgumentException.class)
    public void ctorIllegalArgumentExceptionParam0()
    {
	new ThumbnailEvent(null, ThumbnailEventType.Deselected);
    }

    @Test(expected = IllegalArgumentException.class)
    public void ctorIllegalArgumentExceptionParam1()
    {
	new ThumbnailEvent(this, null);
    }

    @Test
    public void eventType()
    {
	ThumbnailEvent event = new ThumbnailEvent(this,
						  ThumbnailEventType.Selected);
	assertEquals(ThumbnailEventType.Selected, event.getEventType());
    }

}
