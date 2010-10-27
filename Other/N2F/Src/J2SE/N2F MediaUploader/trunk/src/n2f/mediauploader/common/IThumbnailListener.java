/* ------------------------------------------------
 * IThumbnailListener.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader.common;

/**
 * Indicates that the implementors are notified when the state of
 * a {@ThumbnailItem} changes.
 * @author Alex Nesterov
 */
public interface IThumbnailListener
{
    /**
     * Invoked when the state of a <tt>ThumbnailItem</tt> changes.
     * @param	e
     *		Event data.
     */
    void thumbnailItemChanged(ThumbnailEvent e);
}
