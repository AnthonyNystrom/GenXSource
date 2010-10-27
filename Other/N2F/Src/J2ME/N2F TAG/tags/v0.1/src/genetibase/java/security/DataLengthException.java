/* ------------------------------------------------
 * DataLengthException.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.java.security;

/**
 * This exception is thrown if a buffer that is meant to have output
 * copied into it turns out to be too short, or if we've been given 
 * insufficient input. In general this exception will get thrown rather
 * than an ArrayOutOfBounds exception.
 */
public class DataLengthException 
    extends RuntimeCryptoException
{
    /**
     * Creates a new instance of DataLengthException.
     */
    public DataLengthException()
    {
    }

    /**
     * Creates a new instance of DataLengthException.
     * @param message	Specifies the message for the exception.
     */
    public DataLengthException(
        String  message)
    {
        super(message);
    }
}
