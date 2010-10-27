/* ------------------------------------------------
 * ResourceLoader.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.util;

import java.awt.Image;
import java.io.IOException;
import java.util.MissingResourceException;
import javax.imageio.ImageIO;
import static genetibase.util.resources.ExceptionResources.*;

/**
 * Provides functionality to load images from resources.
 * 
 * @author Alex Nesterov
 */
public final class ResourceLoader
{
    /**
     * This class is immutable.
     */
    private ResourceLoader()
    {
    }

    /**
     * Loads the specified image using the specified type as a resource provider.
     * <tt>resourceProvider</tt> is used to determine the package that contains
     * resources. Suppose you have the following project structure:
     * <br/>
     * <code><pre>
     * - org.company.project
     *	    ProjectResources.java
     * - org.company.project.resources
     *	    image.png
     * </pre></code>
     * <br/>
     * Then to retrieve the <tt>image.png</tt> you should call <tt>loadImage</tt>
     * images like:<br/>
     * <code><pre>
     * import java.awt.Image;
     * import genetibase.util.ResourceLoader;
     * import org.company.project.ProjectResources;
     * ...
     * Image image = ResourceLoader.loadImage(ProjectResources.class, "resources/image.png");
     * </pre></code>
     * @param	resourceProvider
     *		Specifies the root package to look for resources.
     * @param	key
     *		Specifies the relative package and the name of the resource. If should be in
     *		the following format: <code>relativePackage/imageFileName.extension</code>.
     * @return	Initialized image.
     * @throws	IllegalArgumentException
     *		If the specified <tt>resourceProvider</tt> is <code>null</code>, or
     *		if the specified <tt>key</tt> is <code>null</code>.
     * @throws	MissingResourceException
     *		If the specified resource cannot be loaded. Make sure the file
     *		with the specified name is an image that exists at the specified
     *		location.
     */
    public static Image loadImage(Class<?> resourceProvider, String key)
    {
	if (resourceProvider == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "resourceProvider"));
	if (key == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "key"));

	Image image = null;

	try
	{
	    image = ImageIO.read(resourceProvider.getResource(key));
	}
	catch (IllegalArgumentException e)
	{
	    throw createMissingResourceException(resourceProvider, key);
	}
	catch (IOException e)
	{
	    throw createMissingResourceException(resourceProvider, key);
	}

	return image;
    }

    private static MissingResourceException createMissingResourceException(Class<?> resourceProvider, String key)
    {
	return new MissingResourceException(String.format(CannotLoadResource,
							     key),
					       resourceProvider.getName(), key);
    }
}
