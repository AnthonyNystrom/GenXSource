/* ------------------------------------------------
 * BlockCipher.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.java.security;

/**
 * Block cipher engines are expected to conform to this interface.
 */
public interface BlockCipher
{
    /**
     * Initialise the cipher.
     *
     * @param forEncryption If <code>true</code> the cipher is initialised for
     *			    encryption, if <code>false</code> for decryption.
     * @param params	    The key and other data required by the cipher.
     * @exception   IllegalArgumentException if <tt>params</tt> argument is
     *		    inappropriate.
     */
    public void init(boolean forEncryption, CipherParameters params)
        throws IllegalArgumentException;

    /**
     * Returns the name of the algorithm the cipher implements.
     * @return	The name of the algorithm the cipher implements.
     */
    public String getAlgorithmName();

    /**
     * Returns the block size for this cipher (in bytes).
     * @return	The block size for this cipher in bytes.
     */
    public int getBlockSize();

    /**
     * Processes one block of input from the array in and write it to
     * the out array.
     *
     * @param in	The array containing the input data.
     * @param inOff	The offset into the in array the data starts at.
     * @param out	The array the output data will be copied into.
     * @param outOff	The offset into the out array the output will start at.
     * @exception   DataLengthException if there isn't enough data
     *		    in <tt>in</tt>, or space in <tt>out</tt>.
     * @exception   IllegalStateException if the cipher isn't initialised.
     * @return	The number of bytes processed and produced.
     */
    public int processBlock(byte[] in, int inOff, byte[] out, int outOff)
        throws DataLengthException, IllegalStateException;

    /**
     * Resets the cipher. After resetting the cipher is in the same state
     * as it was after the last init (if there was one).
     */
    public void reset();
}
