/* ------------------------------------------------
 * PhotoProcessorException.java
 * Copyright © 2008 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.mediauploader;

/**
 * A checked exception that is thrown by the {@link IPhotoProcessor} interface
 * implementations.
 * 
 * @author Administrator
 */
public class PhotoProcessorException
	extends Exception
{
    /**
     * Creates a new instance of the <tt>PhotoProcessorException</tt> class.
     */
    public PhotoProcessorException()
    {
    }

    /**
     * Creates an new instance of the <tt>PhotoProcessorException</tt> class.
     * @param	msg	Specifies the detail message.
     */
    public PhotoProcessorException(String msg)
    {
	super(msg);
    }

    /**
     * Creates a new instance of the <tt>PhotoProcessorException</tt> class.
     * @param	msg
     *		Specifies the detail message.
     * @param	cause
     *		Specifies the inner exception that caused this exception.
     */
    public PhotoProcessorException(String msg, Throwable cause)
    {
	super(msg, cause);
    }
    
    /**
     * Creates a new instance of the <tt>PhotoProcessorException</tt> class.
     * @param	cause
     *		Specifies the inner exception that caused this exception.
     */
    public PhotoProcessorException(Throwable cause)
    {
	super(cause);
    }
}
