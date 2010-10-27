/* ------------------------------------------------
 * PhotoProcessorStubAdapter.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

import java.awt.Dimension;
import java.io.File;
import java.util.List;
import n2f.mediauploader.common.KeyValuePair;

/**
 * @author Alex Nesterov
 */
public class PhotoProcessorStubAdapter
	implements IPhotoProcessor
{
    public void cancelUploadPhotos()
    {
    }

    public void cancelCreateThumbnail()
    {
    }

    public void createThumbnail(File imageFile,
				 Dimension thumbnailDimension,
				 ICreateThumbnailCallback callback)
    {
    }

    public void uploadPhotos(List<KeyValuePair<File>> fileList) throws PhotoProcessorException
    {
    }

}
