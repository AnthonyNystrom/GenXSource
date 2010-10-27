/* ------------------------------------------------
 * PKCS7Padding.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.java.security.paddings;

import genetibase.java.security.SecureRandom;
import genetibase.java.security.InvalidCipherTextException;

/**
 * A padder that adds PKCS7/PKCS5 padding to a block.
 */
public class PKCS7Padding
    implements BlockCipherPadding
{
    /**
     * Initializes the padder.
     * @param random	Specifies a SecureRandom if available.
     */
    public void init(SecureRandom random)
        throws IllegalArgumentException
    {
        // nothing to do.
    }

    /**
     * Returns the name of the algorithm the padder implements.
     * @return	The name of the algorithm the padder implements.
     */
    public String getPaddingName()
    {
        return "PKCS7";
    }

    /**
     * Addes the pad bytes to the passed in block, returning the
     * number of bytes added.
     */
    public int addPadding(
        byte[]  in,
        int     inOff)
    {
        byte code = (byte)(in.length - inOff);

        while (inOff < in.length)
        {
            in[inOff] = code;
            inOff++;
        }

        return code;
    }

    /**
     * Returns the number of pad bytes present in the block.
     * @return	The number of pad bytes present in the block.
     */
    public int padCount(byte[] in)
        throws InvalidCipherTextException
    {
        int count = in[in.length - 1] & 0xff;

        if (count > in.length)
        {
            throw new InvalidCipherTextException("pad block corrupted");
        }
        
        for (int i = 1; i <= count; i++)
        {
            if (in[in.length - i] != count)
            {
                throw new InvalidCipherTextException("pad block corrupted");
            }
        }

        return count;
    }
}
