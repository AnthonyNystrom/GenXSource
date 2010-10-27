/* ------------------------------------------------
 * BlockCipherPadding.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.java.security.paddings;

import genetibase.java.security.SecureRandom;
import genetibase.java.security.InvalidCipherTextException;

/**
 * Block cipher padders are expected to conform to this interface
 */
public interface BlockCipherPadding
{
    /**
     * Initializes the padder.
     * @param random	The source of randomness for the padding, if required.
     */
    public void init(SecureRandom random)
        throws IllegalArgumentException;

    /**
     * Returns the name of the algorithm the cipher implements.
     * @return	The name of the algorithm the cipher implements.
     */
    public String getPaddingName();

    /**
     * Adds the pad bytes to the passed in block, returning the
     * number of bytes added.
     * <p>
     * Note: this assumes that the last block of plain text is always 
     * passed to it inside in. i.e. if inOff is zero, indicating the
     * entire block is to be overwritten with padding the value of in
     * should be the same as the last block of plain text. The reason
     * for this is that some modes such as "trailing bit compliment"
     * base the padding on the last byte of plain text.
     * </p>
     */
    public int addPadding(byte[] in, int inOff);

    /**
     * Returns the number of pad bytes present in the block.
     * @exception   InvalidCipherTextException if the padding is badly formed
     *		    or invalid.
     */
    public int padCount(byte[] in)
        throws InvalidCipherTextException;
}
