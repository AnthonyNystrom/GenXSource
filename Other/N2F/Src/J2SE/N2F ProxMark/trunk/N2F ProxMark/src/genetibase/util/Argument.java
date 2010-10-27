/* ------------------------------------------------
 * Argument.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.util;

import java.io.File;
import static genetibase.util.resources.ExceptionResources.*;

/**
 * Provides methods to process arguments.
 * 
 * @author Alex Nesterov
 */
public final class Argument
{
    /**
     * This class is immutable.
     */
    private Argument()
    {
    }
    
    /**
     * Gets extension for the specified <tt>filePath</tt>. Suppose you
     * passed <tt>image.jpg</tt>. Then the return value will be <tt>jpg</tt>.
     * 
     * @param	filePath
     *		Specifies either full path or file name only.
     * @return	Extension for the specified file. If no extension was found
     *		empty string is returned.
     * @throws	IllegalArgumentException
     *		If the specified <tt>filePath</tt> is <code>null</code>.
     */
    public static String getExtension(String filePath)
    {
	if (filePath == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull, "filePath"));

	int extensionSeparatorIndex = filePath.lastIndexOf('.');
	String extension = "";

	if (extensionSeparatorIndex >= 0)
	    extension = filePath.substring(extensionSeparatorIndex + 1);

	return extension;
    }
    
    /**
     * Gets short name without extension for the specified file.
     * 
     * @param	filePath
     *		Specifies the file to get short name for.
     * @return	Short name without extension.
     * @throws	IllegalArgumentException
     *		If the specified <tt>filePath</tt> is <code>null</code>.
     */
    public static String getShortFileName(String filePath)
    {
	if (filePath == null)
	    throw new IllegalArgumentException(String.format(ArgumentCannotBeNull, "filePath"));
	
	int extensionSeparatorIndex = filePath.lastIndexOf('.');
	int backslashIndex = filePath.lastIndexOf(File.separatorChar);
	
	if (extensionSeparatorIndex < 0)
	    extensionSeparatorIndex = filePath.length();
	
	return (extensionSeparatorIndex < 0)
		? filePath.substring(backslashIndex + 1)
		: filePath.substring(backslashIndex + 1, extensionSeparatorIndex);
    }
}
