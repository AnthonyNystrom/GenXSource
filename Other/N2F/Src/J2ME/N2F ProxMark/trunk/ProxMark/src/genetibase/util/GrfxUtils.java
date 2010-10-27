
package genetibase.util;

import java.io.IOException;
import java.io.InputStream;
import javax.microedition.lcdui.Image;

public class GrfxUtils
{
    /**
     * Closes this input stream and releases any system resources associated with the stream
     * 
     * @param is
     */
    public static void close(InputStream is)
    {
	if (is != null)
	{
	    try
	    {
		is.close();
	    }
	    catch (IOException e)
	    {
		e.printStackTrace();
	    }
	    finally
	    {
		is = null;
	    }
	}
    }

    /**
     * Creates an immutable image from decoded image data obtained from an
     * InputStream.
     * 
     * @param is -
     *            the name of the resource containing the image data in one of
     *            the supported image formats
     * @return created image
     */
    public static final Image createImage(InputStream is)
    {
	Image ret = null;

	if (is != null)
	{
	    try
	    {
		ret = Image.createImage(is);
	    }
	    catch (Exception e)
	    {

		e.printStackTrace();
	    }
	    close(is);
	}

	return ret;
    }

    public static final Image createImage(byte[] imageBytes)
    {
	Image ret = null;

	if (imageBytes != null)
	{
	    try
	    {
		ret = Image.createImage(imageBytes, 0, imageBytes.length);
	    }
	    catch (Exception e)
	    {

		e.printStackTrace();
	    }
	}

	return ret;
    }

}
