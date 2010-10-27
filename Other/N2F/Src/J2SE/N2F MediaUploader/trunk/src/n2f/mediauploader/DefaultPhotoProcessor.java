/* ------------------------------------------------
 * DefaultPhotoProcessor.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

import java.awt.Dimension;
import java.awt.image.BufferedImage;
import java.beans.PropertyChangeListener;
import java.beans.PropertyChangeSupport;
import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.IOException;
import java.util.List;
import java.util.logging.Level;
import java.util.logging.LogManager;
import java.util.logging.Logger;
import javax.imageio.ImageIO;
import n2f.mediauploader.common.KeyValuePair;
import n2f.mediauploader.drawing.GrfxUtils;
import n2f.mediauploader.drawing.RotateType;
import n2f.mediauploader.util.Base64;
import n2f.mediauploader.util.WorkerThread;
import n2f.mediauploader.webservice.PhotoOrganiseService;
import n2f.mediauploader.webservice.WebServiceException;
import n2f.mediauploader.webservice.WebServiceInterop;

import static n2f.mediauploader.resources.DefaultPhotoProcessorResources.*;
import static n2f.mediauploader.resources.ExceptionResources.*;

/**
 * Default <tt>IPhotoProcessor</tt> interface implementation. It creates
 * thumbnails in a dedicated thread sequentially. It means all images are
 * processed in a single thread one by one. Images are upload using a
 * {@link PhotoOrganiseService} instance, which is instantiated internally, not
 * injected that is.
 * 
 * @author Alex Nesterov
 */
public final class DefaultPhotoProcessor
	implements IPhotoProcessor
{
    public static final String PHOTO_UPLOADED =
	    "DefaultPhotoProcessor.photoUploaded";
    public static final String THUMBNAIL_CHANGED =
	    "DefaultPhotoProcessor.thumbnailChanged";
    private static final Logger _logger =
	    Logger.getLogger(DefaultPhotoProcessor.class.getName());

    static
    {
	LogManager.getLogManager().addLogger(_logger);
    }
        
    private boolean _shouldCancelUpload;
    private PhotoOrganiseService _service;
    private MemberAccount _memberAccount;
    private PropertyChangeSupport _changeSupport;
    private WorkerThread _workerThread;

    /**
     * Creates a new instance of the <tt>DefaultPhotoProcessor</tt> class.
     * @param	memberAccount
     *		Specifies the account that will be used to upload photos.
     * @throws	IllegalArgumentException
     *		If the specified <tt>memberAccount</tt> is <code>null</code>.
     */
    public DefaultPhotoProcessor(MemberAccount memberAccount)
    {
	if (memberAccount == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "memberAccount"));

	_changeSupport = new PropertyChangeSupport(this);
	_memberAccount = memberAccount;
	_service = new PhotoOrganiseService();
	_workerThread = new WorkerThread();
    }

    /**
     * Adds the specified listener to receive property change events from this
     * component.
     * @param	l
     *		Specifies the property change listener.
     * @throws	IllegalArgumentException
     *		If the specified <tt>l</tt> is <code>null</code>.
     */
    public void addPropertyChangeListener(PropertyChangeListener l)
    {
	if (l == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "l"));
	_changeSupport.addPropertyChangeListener(l);
    }

    /**
     * Interrupts photo uploading process. In the current implementation it
     * waits until the current photo is uploaded and then exits. If no current
     * photo uploading process is running returns silently.
     */
    public void cancelUploadPhotos()
    {
	_logger.log(Level.INFO, "Cancelling upload...");
	_shouldCancelUpload = true;
    }

    /**
     * Creating thumbnails can be a long process. It should be stopped when
     * there is no need in thumbnails, for example the current folder is changed.
     */
    public void cancelCreateThumbnail()
    {
	_logger.log(Level.INFO, "Cancelling current thumbnail generation tasks.");
	_workerThread.removeAllTasks();
    }

    /**
     * Loads the specified <tt>imageFile</tt>, generates a thumbnail, and
     * notifies the process has finished via the specified <tt>callback</tt>
     * instance.
     * 
     * @param	imageFile
     *		Specifies the file containing an image.
     * @param	thumbnailDimension
     *		Specifies the dimension for the future thumbnail.
     * @param	callback
     *		Listeners will be notified when the thumbnail generation
     *		process is complete successfully or not.
     * @throws	IllegalArgumentException
     *		If the specified <tt>imageFile</tt> is <code>null</code>, or
     *		if the specified <tt>thumbnailDimension</tt> is <code>null</code>, or
     *		if the specified <tt>callback</tt> is <code>null</code>.
     */
    public void createThumbnail(final File imageFile,
				 final Dimension thumbnailDimension,
				 final ICreateThumbnailCallback callback)
    {
	if (imageFile == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "imageFile"));
	if (thumbnailDimension == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "thumbnailDimension"));
	if (callback == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "callback"));

	Runnable runnable = new Runnable()
	{
	    public void run()
	    {
		BufferedImage image = null;

		try
		{
		    image = ImageIO.read(imageFile);
		}
		catch (IOException e)
		{
		    callback.createThumbnailFailed(e.getMessage());
		    return;
		}

		if (image == null)
		{
		    _logger.log(Level.WARNING, "image is null. Won't create thumbnail.");
		    return;
		}
		
		callback.thumbnailCreated(GrfxUtils.createThumbnail(image,
								    thumbnailDimension.width));
	    }

	};

	_workerThread.put(runnable);
    }

    /**
     * Uploads the specified list of images using the internal
     * {@link PhotoOrganiseService} instance.
     * 
     * @param	fileList
     *		Specifies the list of images to upload.
     * @throws	n2f.mediauploader.PhotoProcessorException
     *		If an error occurred during upload. Stack trace as well as inner exception
     *		are provided.
     * @throws	IllegalArgumentException
     *		If the specified <tt>fileList</tt> is <code>null</code>.
     */
    public void uploadPhotos(List<KeyValuePair<File>> fileList) throws PhotoProcessorException
    {
	if (fileList == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "fileList"));

	_shouldCancelUpload = false;
	_logger.log(Level.INFO, "Intending to upload {0} files.", fileList.size());

	for (final KeyValuePair<File> item : fileList)
	{
	    if (_shouldCancelUpload)
	    {
		_logger.log(Level.INFO, "Upload cancelled.");
		break;
	    }

	    File photoFile = item.value;
	    _logger.log(Level.INFO, "Intending to upload \"{0}\"...", photoFile.getAbsolutePath());
	    
	    if (photoFile.exists())
	    {
		_logger.log(Level.INFO, "File exists. Loading...");
		BufferedImage image = null;

		try
		{
		    image = ImageIO.read(photoFile);
		    _logger.log(Level.INFO, "File \"{0}\" was loaded successfully.", photoFile.getAbsolutePath());
		}
		catch (IOException e)
		{
		    throw new PhotoProcessorException(String.format(CannotReadFile,
								    photoFile),
						      e);
		}

		_logger.log(Level.INFO, "Retrieving thumbnail...");
		BufferedImage thumbnail =
			(BufferedImage)item.get(MediaUploaderModel.THUMBNAIL_PROPERTY);
		_logger.log(Level.INFO, "Thumbnail retrieved successfully. Can be null.");
		_changeSupport.firePropertyChange(THUMBNAIL_CHANGED, null,
						  thumbnail);

		RotateType rotateType =
			(RotateType)item.get(MediaUploaderModel.FILE_ROTATE_TYPE_PROPERTY);

		if (rotateType == null)
		    rotateType = RotateType.RotateNone;
		_logger.log(Level.INFO, "rotateType = {0}", rotateType);
		_logger.log(Level.INFO, "Intending to rotate the image.");
		image = GrfxUtils.rotateImage(image, rotateType);
		_logger.log(Level.INFO, "Image rotated successfully.");
		
		ByteArrayOutputStream outputStream =
			new ByteArrayOutputStream();
		_logger.log(Level.INFO, "Writing the image into memory buffer in JPEG format...");

		try
		{
		    ImageIO.write(image, "jpg", outputStream);
		    _logger.log(Level.INFO, "Wrote to memory buffer successfully.");
		}
		catch (IOException e)
		{
		    throw new PhotoProcessorException(ImageWriteFailed, e);
		}

		_logger.log(Level.INFO, "Converting the image to a Base64 string...");
		byte[] imageByteArray = outputStream.toByteArray();
		String base64PhotoString = Base64.encode(imageByteArray);
		_logger.log(Level.INFO, "Base64 conversion completed successfully.");

		try
		{
		    _logger.log(Level.INFO, "Uploading using the following member associated data:");
		    
		    String encryptedMemberID = _memberAccount.getEncryptedMemberID();
		    String galleryID = _memberAccount.getGalleryModel().getCurrentGallery().getGalleryID();
		    String lastModified = WebServiceInterop.getServiceDate(photoFile.lastModified());
		    
		    _logger.log(Level.INFO, "encryptedMemberID = {0}", encryptedMemberID);
		    _logger.log(Level.INFO, "galleryID = {0}", galleryID);
		    _logger.log(Level.INFO, "lastModified = {0}", lastModified);
		    
		    _service.uploadPhoto(encryptedMemberID,
					 galleryID,
					 base64PhotoString,
					 lastModified);
		}
		catch (WebServiceException e)
		{
		    throw new PhotoProcessorException(e.getMessage(), e);
		}

		_logger.log(Level.INFO, "Uploaded successfully.");
		_changeSupport.firePropertyChange(PHOTO_UPLOADED, null,
						  photoFile);

	    }
	}
    }
}
