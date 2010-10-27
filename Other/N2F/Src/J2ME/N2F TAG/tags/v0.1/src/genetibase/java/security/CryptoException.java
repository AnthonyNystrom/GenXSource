/* ------------------------------------------------
 * CryptoException.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.java.security;

/**
 * The foundation class for the hard exceptions thrown
 * by the cryptographic algorithms.
 */
public class CryptoException 
    extends Exception
{
    /**
     * Creates a new instance of CryptoException.
     */
    public CryptoException()
    {
    }

    /**
     * Creates a new instance of CryptoException.
     * @param message	Specifies the message for the exception.
     */
    public CryptoException(
        String  message)
    {
        super(message);
    }
}
