/* ------------------------------------------------
 * GrfxUtils.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.drawing;

import java.awt.*;
import java.awt.image.BufferedImage;

import javax.swing.plaf.*;

import static genetibase.util.resources.ExceptionResources.*;

/**
 * @author Alex Nesterov
 */
public class GrfxUtils
{
    /**
     * Creates a thumbnail with the specified width.
     * 
     * @param	image
     *          The original image.
     * @param	requestedThumbWidth
     *          The width of the resulting thumbnail.
     * @return	Thumbnail of the specified width.
     * @throws	IllegalArgumentException
     *		If the specified <tt>image</tt> is <code>null</code>.
     * @author Romain Guy
     * @author Alex Nesterov
     */
    public static BufferedImage createThumbnail(BufferedImage image,
						  int requestedThumbWidth)
    {
	if (image == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "image"));

	float imageWidth = image.getWidth();
	float imageHeight = image.getHeight();

	if (imageWidth < requestedThumbWidth)
	    return image;

	float ratio = imageWidth / imageHeight;
	int width = image.getWidth();
	BufferedImage thumb = image;

	do
	{
	    width /= 2;
	    if (width < requestedThumbWidth)
		width = requestedThumbWidth;

	    BufferedImage temp = new BufferedImage(width,
						   (int)(width / ratio),
						   BufferedImage.TYPE_INT_ARGB);
	    Graphics2D g2 = temp.createGraphics();
	    g2.setRenderingHint(RenderingHints.KEY_INTERPOLATION,
				RenderingHints.VALUE_INTERPOLATION_BILINEAR);
	    g2.drawImage(thumb, 0, 0, temp.getWidth(), temp.getHeight(), null);
	    g2.dispose();

	    thumb = temp;
	}
	while (width != requestedThumbWidth);

	return thumb;
    }

    /**
     * Rotates the specified image at the specified angle.
     * 
     * @param	image
     *		Specifies the image to rotate.
     * @param	rotateType
     *		Specifies the angle to rotate the image at.
     * @return	Rotated image.
     * @throws	IllegalArgumentException
     *		If the specified <tt>image</tt> is <code>null</code>, or
     *		if the specified <tt>rotateType</tt> is <code>null</code>.
     */
    public static BufferedImage rotateImage(BufferedImage image,
					      RotateType rotateType)
    {
	if (image == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "image"));

	if (rotateType == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "rotateType"));

	int angle = 0;

	int width = image.getWidth();
	int height = image.getHeight();

	int newWidth = width;
	int newHeight = height;

	switch (rotateType)
	{
	    case Rotate90:
	    {
		angle = 90;
		newWidth = height;
		newHeight = width;
		break;
	    }
	    case Rotate180:
	    {
		angle = 180;
		break;
	    }
	    case Rotate270:
	    {
		angle = 270;
		newWidth = height;
		newHeight = width;
		break;
	    }
	}

	BufferedImage result =
		new BufferedImage(newWidth,
				  newHeight,
				  BufferedImage.TYPE_INT_RGB);
	Graphics2D g = result.createGraphics();
	g.translate((newWidth - width) / 2, (newHeight - height) / 2);
	g.rotate(Math.toRadians(angle), width / 2, height / 2);
	g.drawRenderedImage(image, null);
	g.dispose();

	return result;
    }

    /**
     * Calculates the destination rectangle for the image of the specified size
     * to fit the specified rectangle.
     * 
     * @throws	IllegalArgumentException
     *		If the specified <tt>targetRect</tt> is <code>null</code>, or
     *		if the specified <tt>imageSize</tt> is <code>null</code>.
     */
    public static Rectangle scaleToFit(Rectangle targetRect,
					 Dimension imageSize)
    {
	if (targetRect == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "targetRect"));
	if (imageSize == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "imageSize"));

	if (!targetRect.isEmpty())
	{
	    if (imageSize.width < targetRect.width && imageSize.height < targetRect.height)
	    {
		return new Rectangle((targetRect.width - imageSize.width) / 2,
				     (targetRect.height - imageSize.height) / 2,
				     imageSize.width,
				     imageSize.height);
	    }

	    Rectangle resultRect = new Rectangle(targetRect.getLocation(),
						 targetRect.getSize());

	    if (resultRect.getHeight() * imageSize.getWidth() > resultRect.getWidth() * imageSize.getHeight())
	    {
		resultRect.setSize((int)resultRect.getWidth(),
				   (int)(resultRect.getWidth() * imageSize.getHeight() / imageSize.getWidth()));
		resultRect.setLocation((int)resultRect.getX(),
				       (int)(resultRect.getY() + (targetRect.getHeight() - resultRect.getHeight()) / 2));

	    }
	    else
	    {
		resultRect.setSize((int)(resultRect.getHeight() * imageSize.getWidth() / imageSize.getHeight()),
				   (int)resultRect.getHeight());
		resultRect.setLocation((int)(resultRect.getX() + (targetRect.getWidth() - resultRect.getWidth()) / 2),
				       (int)resultRect.getY());
	    }

	    return resultRect;
	}

	return new Rectangle(0, 0, 0, 0);
    }

}
