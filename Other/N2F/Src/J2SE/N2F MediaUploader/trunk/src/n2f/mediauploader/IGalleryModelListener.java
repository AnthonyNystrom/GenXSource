/* ------------------------------------------------
 * IGalleryModelListener.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

/**
 * @author Alex Nesterov
 */
public interface IGalleryModelListener
{
    /** 
     * Invoked when a gallery model state changes.
     * @param	e
     *		Event data.
     */
    void galleryModelChanged(GalleryEvent e);
}
