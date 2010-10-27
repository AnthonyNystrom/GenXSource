/* ------------------------------------------------
 * IPhotoProcessor.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

import java.awt.Dimension;
import java.io.File;
import java.util.List;
import n2f.mediauploader.common.KeyValuePair;

/**
 * Indicates that the implementor can create thumbnails and upload photos.
 * @author Alex Nesterov
 */
public interface IPhotoProcessor
{
    /**
     * Interrupts the current photo upload task.
     */
    void cancelUploadPhotos();
    
    /**
     * Interrupts all current thumbnail generation tasks.
     */
    void cancelCreateThumbnail();

    /**
     * Starts thumbnail generated in a dedicated thread and notifies the listener
     * when the process completed using the specified <tt>callback</tt>.
     * 
     * @param	imageFile
     *		Specifies the file that contains the full-sized image.
     * @param	callback
     *		Specifies the instance that is used to notify the listener
     *		when the operation is completed.
     * 
     * @throws	IllegalArgumentException
     *		If the specified <tt>imageFile</tt> is <code>null</code>, or
     *		if the specified <tt>thumbnailDimension</tt> is <code>null</code>, or
     *		if the specified <tt>callback</tt> is <code>null</code>.
     */
    void createThumbnail(File imageFile,
			 Dimension thumbnailDimension,
			 ICreateThumbnailCallback callback);

    /**
     * Uploads photos from the specified list.
     * 
     * @param	fileList
     *		Specifies the list of photos to upload.
     * @throws	PhotoProcessorException
     *		If error occurs during photo upload.
     */
    void uploadPhotos(List<KeyValuePair<File>> fileList)
	    throws PhotoProcessorException;

}
