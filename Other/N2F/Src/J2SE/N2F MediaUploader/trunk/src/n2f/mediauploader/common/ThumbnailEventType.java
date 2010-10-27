/* ------------------------------------------------
 * ThumbnailEventType.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader.common;

/**
 * Defines the types of events a {@link ThumbnailItem} can initiate.
 * @author Alex Nesterov
 */
public enum ThumbnailEventType
{
    /** Thumbnail deselected. */
    Deselected,
    /** Thumbnail rotated 90 degrees clock-wise. */
    Rotated90,
    /** Thumbnail rotated 180 degrees clock-wise. */
    Rotated180,
    /** Thumbnail rotated 270 degress clock-wise. */
    Rotated270,
    /** Thumbnail selected. */
    Selected
}
