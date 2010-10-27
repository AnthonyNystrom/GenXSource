/* ------------------------------------------------
 * RuntimeCryptoException.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.java.security;

/**
 * The foundation class for the exceptions thrown
 * by the cryptographic algorithms.
 */
public class RuntimeCryptoException 
    extends RuntimeException
{
    /**
     * Creates a new instance of RuntimeCryptoException.
     */
    public RuntimeCryptoException()
    {
    }

    /**
     * Creates a new instance of RuntimeCryptoException.
     * @param message	Specifies the message for the exception.
     */
    public RuntimeCryptoException(
        String  message)
    {
        super(message);
    }
}
