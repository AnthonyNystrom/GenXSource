/* ------------------------------------------------
 * ExtendedDigest.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.java.security;

public interface ExtendedDigest extends Digest
{
    /**
     * Return the size in bytes of the internal buffer the digest applies
     * it's compression function to.
     * 
     * @return	Byte length of the digest's internal buffer.
     */
    public int getByteLength();
}
