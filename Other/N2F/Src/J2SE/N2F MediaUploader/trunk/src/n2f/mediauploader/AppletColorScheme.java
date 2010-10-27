/* ------------------------------------------------
 * AppletColorScheme.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

import java.awt.Color;

/**
 * Defines the colors that are common for MediaUploader applet.
 * @author Alex Nesterov
 */
public final class AppletColorScheme
{
    /** Since we want the hyperlink to look identically in both clicked and
     * unclicked state we define one common color. */
    public static final Color HyperlinkColor = new Color(2, 87, 174);
    /** Defines the color for <tt>ThumbnailItem</tt> in its normal state. */
    public static final Color ThumbnailNormalBorderColor =
	    new Color(204, 204, 204);
    /** Defines the color for <tt>ThumbnailItem</tt> in its hot state (mouse is over). */
    public static final Color ThumbnailHotBorderColor =
	    new Color(204, 102, 0);
    /** Defines the color for <tt>ThumbnailItem</tt> in its selected state
     * (<tt>setSelected</tt> method was called with <code>true</code> parameter). */
    public static final Color ThumbnailSelectedBorderColor =
	    new Color(2, 87, 174);

    /**
     * This class is immutable.
     */
    private AppletColorScheme()
    {
    }

}
