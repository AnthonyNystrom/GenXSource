/* ------------------------------------------------
 * GalleryEventType.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

/**
 * Defines the types of events {@link GalleryModel} can initiate.
 * @author Alex Nesterov
 */
enum GalleryEventType
{
    /** New gallery added to a <tt>GalleryModel</tt>. */
    NewGalleryAdded,
    /** Default gallery changed on a <tt>GalleryModel</tt>. */
    DefaultGalleryChanged
}
