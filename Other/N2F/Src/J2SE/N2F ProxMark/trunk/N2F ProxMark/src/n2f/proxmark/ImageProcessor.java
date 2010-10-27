/* ------------------------------------------------
 * ImageProcessor.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.proxmark;

import genetibase.drawing.GrfxUtils;
import genetibase.util.Base64;
import java.awt.Dimension;
import java.awt.image.BufferedImage;
import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.IOException;
import java.util.logging.Level;
import java.util.logging.LogManager;
import java.util.logging.Logger;
import javax.imageio.ImageIO;
import static genetibase.util.resources.ExceptionResources.*;

/**
 * @author Alex Nesterov
 */
final class ImageProcessor
{
    private static final Logger _logger =
	    Logger.getLogger(ImageProcessor.class.getName());

    static
    {
	LogManager.getLogManager().addLogger(_logger);
    }

    /**
     * This class is immutable.
     */
    private ImageProcessor()
    {
    }

    /**
     * If the specified <tt>imagePath</tt> is valid generates image thumbnail.
     * @param	imagePath
     *		Specifies the path to the image to create thumbnail for.
     * @param	thumbnailMaximumSize
     *		Specifies the maximum width and height for the thumbnail.
     * @return	Thumbnail image. <code>null</code> if the image at the specified
     *		path does not exist or image could not be loaded.
     * @throws	IllegalArgumentException
     *		If the specified <tt>imagePath</tt> is <code>null</code>, or
     *		if the specified <tt>thumbnailMaximumSize</tt> is <code>null</code>.
     */
    public static BufferedImage createThumbnail(String imagePath,
						  Dimension thumbnailMaximumSize)
    {
	if (imagePath == null)
	    throw new IllegalArgumentException(
		    String.format(ArgumentCannotBeNull, "imagePath"));
	if (thumbnailMaximumSize == null)
	    throw new IllegalArgumentException(
		    String.format(ArgumentCannotBeNull, "thumbnailMaximumSize"));

	File imageFile = new File(imagePath);

	if (imageFile.exists())
	{
	    BufferedImage originalImage = null;

	    try
	    {
		originalImage = ImageIO.read(imageFile);
	    }
	    catch (IOException e)
	    {
		_logger.log(Level.SEVERE, e.getMessage(), e);
	    }

	    if (originalImage != null)
		return GrfxUtils.createThumbnail(originalImage,
						 thumbnailMaximumSize.width);
	}

	return null;
    }

    /**
     * If the specified <tt>imagePath</tt> is valid generates thumbnail using
     * given parameters and returns its Base64 string representation.
     * @param	imagePath
     *		Specifies the path to the image to create thumbnail for.
     * @param	thumbnailMaximumSize
     *		Specifies the maximum width and height for the thumbnail.
     * @return	Base64 string representation of the generated thumbnail, or
     *		<code>null</code> if thumbnail generation failed.
     * @throws	java.io.IOException
     *		If writing the thumbnail to a <tt>ByteArrayOutputStream</tt> failed.
     */
    public static String imageToBase64(String imagePath,
					 Dimension thumbnailMaximumSize)
	    throws IOException
    {
	BufferedImage thumbnail = createThumbnail(imagePath, thumbnailMaximumSize);
	
	if (thumbnail == null)
	    return null;
	
	ByteArrayOutputStream outputStream = new ByteArrayOutputStream();
	_logger.log(Level.INFO,
		    "Writing the image into memory buffer in JPEG format...");
	ImageIO.write(thumbnail, "jpg", outputStream);
	_logger.log(Level.INFO, "Wrote to memory buffer successfully.");
	_logger.log(Level.INFO, "Converting the image to a Base64 string...");
	byte[] imageByteArray = outputStream.toByteArray();
	String base64PhotoString = Base64.encode(imageByteArray);
	_logger.log(Level.INFO, "Base64 conversion completed successfully.");
	return base64PhotoString;
    }

}
