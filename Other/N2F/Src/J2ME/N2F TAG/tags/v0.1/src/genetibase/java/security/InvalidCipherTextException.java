/* ------------------------------------------------
 * InvalidCipherTextException.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.java.security;

/**
 * This exception is thrown whenever we find something we don't expect in a
 * message.
 */
public class InvalidCipherTextException 
    extends CryptoException
{
    /**
     * Creates a new instance of InvalidCipherTextException.
     */
    public InvalidCipherTextException()
    {
    }

    /**
     * Creates a new instance of InvalidCipherTextException.
     * @param message	Specifies the message for the exception.
     */
    public InvalidCipherTextException(
        String  message)
    {
        super(message);
    }
}
