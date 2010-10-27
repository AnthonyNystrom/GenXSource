/* ------------------------------------------------
 * Argument.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader.util;

import java.io.File;

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
     * Tests the input parameter against null. If the input is 
     * an array, checks all of its elements as well. Returns the 
     * unchanged parameter if not null, throws a NullPointerException
     * otherwise. <p>
     * 
     * PENDING: type of exception? there are raging debates, some
     *   favour an IllegalArgument? <p>
     *   
     * PENDING: the implementation uses a unchecked type cast to an array.
     *   can we do better, how?
     *     
     * 
     * @param <T> the type of the input parameter
     * @param input the argument to check against null.
     * @param message the text of the exception if the argument is null
     * @return the input if not null
     * @thows NullPointerException if input is null
     */
    @SuppressWarnings("unchecked")
    public static <T> T asNotNull(T input, String message)
    {
	if (input == null)
	    throw new NullPointerException(message);

	if (input.getClass().isArray())
	{
	    if (!input.getClass().getComponentType().isPrimitive())
	    {
		T[] array = (T[])input;
		for (int i = 0; i < array.length; i++)
		{
		    asNotNull(array[i], message);
		}
	    }
	}

	return input;
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
	    throw new IllegalArgumentException("filePath cannot be null.");

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
	    throw new IllegalArgumentException("filePath cannot be null.");
	
	int extensionSeparatorIndex = filePath.lastIndexOf('.');
	int backslashIndex = filePath.lastIndexOf(File.separatorChar);
	
	if (extensionSeparatorIndex < 0)
	    extensionSeparatorIndex = filePath.length();
	
	return (extensionSeparatorIndex < 0)
		? filePath.substring(backslashIndex + 1)
		: filePath.substring(backslashIndex + 1, extensionSeparatorIndex);
    }
}
