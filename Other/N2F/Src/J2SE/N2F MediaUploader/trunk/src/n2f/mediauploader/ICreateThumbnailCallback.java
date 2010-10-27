/* ------------------------------------------------
 * ICreateThumbnailCallback.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

import java.awt.image.BufferedImage;

/**
 * Allows to notify the listener when thumbnail generation process is complete.
 * @author Alex Nesterov
 */
public interface ICreateThumbnailCallback
{
    /** Invoked when thumbnail generation process failed or was interrupted. */
    void createThumbnailFailed(String message);
    /** Invoked when thumbnail generation process completed successfully. */
    void thumbnailCreated(BufferedImage thumbnail);
}
