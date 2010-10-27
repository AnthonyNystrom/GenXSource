/* ------------------------------------------------
 * ExtensionFilter.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

import n2f.mediauploader.util.*;
import static n2f.mediauploader.resources.ExceptionResources.*;

/**
 * Provides functionality to filter files upon their extensions.
 * 
 * @author Alex Nesterov
 */
final class ExtensionFilter
{
    /**
     * This class is immutable.
     */
    private ExtensionFilter()
    {
    }

    /**
     * Checks if the specified file has a valid extension. In the example below
     * <tt>jpg</tt> is considered to be a valid extension.
     * <br/>
     * <pre><code>
     * String[] validExtensions = new String[] { "jpg" };
     * ExtensionFilter.checkFile("C:\\image.jpg", validExtensions); // true
     * </code></pre>
     * 
     * @param	filePath
     *		Specifies full file path or file name only.
     * @param	validExtensions
     *		An array of valid extensions.
     * 
     * @return	<code>true</code> if the specified file extension is considered
     *		to be valid; <code>false</code> otherwise.
     * 
     * @throws	IllegalArgumentException
     *		If the specified <tt>filePath</tt> is <code>null</code>, or
     *		if the specified <tt>validExtensions</tt> is <code>null</code>.
     */
    public static boolean checkFile(String filePath, String[] validExtensions)
    {
	if (filePath == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "filePath"));
	if (validExtensions == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull,
							     "validExtensions"));

	String currentExtension = Argument.getExtension(filePath);

	for (final String validExtension : validExtensions)
	{
	    if (validExtension == null)
		continue;
	    if (currentExtension.equalsIgnoreCase(validExtension))
		return true;
	}

	return false;
    }

}
