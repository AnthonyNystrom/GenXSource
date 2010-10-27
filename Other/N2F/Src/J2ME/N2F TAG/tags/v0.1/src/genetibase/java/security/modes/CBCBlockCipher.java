/* ------------------------------------------------
 * CBCBlockCipher.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.java.security.modes;

import genetibase.java.security.BlockCipher;
import genetibase.java.security.CipherParameters;
import genetibase.java.security.DataLengthException;
import genetibase.java.security.params.ParametersWithIV;

/**
 * Implements Cipher-Block-Chaining (CBC) mode on top of a simple cipher.
 */
public class CBCBlockCipher
    implements BlockCipher
{
    private byte[]          IV;
    private byte[]          cbcV;
    private byte[]          cbcNextV;

    private int             blockSize;
    private BlockCipher     cipher = null;
    private boolean         encrypting;

    /**
     * Creates a new instance of CBCBlockCipher.
     *
     * @param cipher the block cipher to be used as the basis of chaining.
     */
    public CBCBlockCipher(
        BlockCipher cipher)
    {
        this.cipher = cipher;
        this.blockSize = cipher.getBlockSize();

        this.IV = new byte[blockSize];
        this.cbcV = new byte[blockSize];
        this.cbcNextV = new byte[blockSize];
    }

    /**
     * Returns the underlying block cipher that we are wrapping.
     * @return	The underlying block cipher that we are wrapping.
     */
    public BlockCipher getUnderlyingCipher()
    {
        return cipher;
    }

    /**
     * Initialise the cipher and, possibly, the initialisation vector (IV).
     * If an IV isn't passed as part of the parameter, the IV will be all zeros.
     *
     * @param encrypting    If <code>true</code> the cipher is initialized for
     *			    encryption, if <code>false</code> for decryption.
     * @param params	The key and other data required by the cipher.
     * @exception   IllegalArgumentException if <tt>params</tt> is
     *		    inappropriate.
     */
    public void init(
        boolean             encrypting,
        CipherParameters    params)
        throws IllegalArgumentException
    {
        this.encrypting = encrypting;
        
        if (params instanceof ParametersWithIV)
        {
                ParametersWithIV ivParam = (ParametersWithIV)params;
                byte[]      iv = ivParam.getIV();

                if (iv.length != blockSize)
                {
                    throw new IllegalArgumentException("initialisation vector must be the same length as block size");
                }

                System.arraycopy(iv, 0, IV, 0, iv.length);

                reset();

                cipher.init(encrypting, ivParam.getParameters());
        }
        else
        {
                reset();

                cipher.init(encrypting, params);
        }
    }

    /**
     * Returns the algorithm name and mode.
     * @return	The name of the underlying algorithm followed by "/CBC".
     */
    public String getAlgorithmName()
    {
        return cipher.getAlgorithmName() + "/CBC";
    }

    /**
     * Returns the block size of the underlying cipher.
     * @return	The block size of the underlying cipher.
     */
    public int getBlockSize()
    {
        return cipher.getBlockSize();
    }

    /**
     * Processes one block of input from the array in and write it to
     * the out array.
     *
     * @param in	The array containing the input data.
     * @param inOff	The offset into the in array the data starts at.
     * @param out	The array the output data will be copied into.
     * @param outOff	The offset into the out array the output will start at.
     * @exception   DataLengthException if there isn't enough data in
     *		    <tt>in</tt>, or space in <tt>out</tt>.
     * @exception   IllegalStateException if the cipher isn't initialised.
     * @return	The number of bytes processed and produced.
     */
    public int processBlock(
        byte[]      in,
        int         inOff,
        byte[]      out,
        int         outOff)
        throws DataLengthException, IllegalStateException
    {
        return (encrypting) ? encryptBlock(in, inOff, out, outOff) : decryptBlock(in, inOff, out, outOff);
    }

    /**
     * Resets the chaining vector back to the IV and reset the underlying
     * cipher.
     */
    public void reset()
    {
        System.arraycopy(IV, 0, cbcV, 0, IV.length);

        cipher.reset();
    }

    /**
     * Do the appropriate chaining step for CBC mode encryption.
     *
     * @param in	The array containing the data to be encrypted.
     * @param inOff	The offset into the in array the data starts at.
     * @param out	The array the encrypted data will be copied into.
     * @param outOff	The offset into the out array the output will start at.
     * @exception   DataLengthException if there isn't enough data in
     *		    <tt>in</tt>, or space in <tt>out</tt>.
     * @exception   IllegalStateException if the cipher isn't initialised.
     * @return	The number of bytes processed and produced.
     */
    private int encryptBlock(
        byte[]      in,
        int         inOff,
        byte[]      out,
        int         outOff)
        throws DataLengthException, IllegalStateException
    {
        if ((inOff + blockSize) > in.length)
        {
            throw new DataLengthException("input buffer too short");
        }

        /*
         * XOR the cbcV and the input,
         * then encrypt the cbcV
         */
        for (int i = 0; i < blockSize; i++)
        {
            cbcV[i] ^= in[inOff + i];
        }

        int length = cipher.processBlock(cbcV, 0, out, outOff);

        /*
         * copy ciphertext to cbcV
         */
        System.arraycopy(out, outOff, cbcV, 0, cbcV.length);

        return length;
    }

    /**
     * Does the appropriate chaining step for CBC mode decryption.
     *
     * @param in	The array containing the data to be decrypted.
     * @param inOff	The offset into the in array the data starts at.
     * @param out	The array the decrypted data will be copied into.
     * @param outOff	The offset into the out array the output will start at.
     * @exception   DataLengthException if there isn't enough data in
     *		    <tt>in</tt>, or space in <tt>out</tt>.
     * @exception   IllegalStateException if the cipher isn't initialised.
     * @return	The number of bytes processed and produced.
     */
    private int decryptBlock(
        byte[]      in,
        int         inOff,
        byte[]      out,
        int         outOff)
        throws DataLengthException, IllegalStateException
    {
        if ((inOff + blockSize) > in.length)
        {
            throw new DataLengthException("input buffer too short");
        }

        System.arraycopy(in, inOff, cbcNextV, 0, blockSize);

        int length = cipher.processBlock(in, inOff, out, outOff);

        /*
         * XOR the cbcV and the output
         */
        for (int i = 0; i < blockSize; i++)
        {
            out[outOff + i] ^= cbcV[i];
        }

        /*
         * swap the back up buffer into next position
         */
        byte[]  tmp;

        tmp = cbcV;
        cbcV = cbcNextV;
        cbcNextV = tmp;

        return length;
    }
}
