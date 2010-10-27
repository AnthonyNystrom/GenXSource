/* ------------------------------------------------
 * Digest.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.java.security;

/**
 * Interface that a message digest conforms to.
 */
public interface Digest
{
    /**
     * Returns the algorithm name.
     * @return	The algorithm name.
     */
    public String getAlgorithmName();

    /**
     * Returns the size, in bytes, of the digest produced by this message digest.
     * @return the size, in bytes, of the digest produced by this message digest.
     */
    public int getDigestSize();

    /**
     * Updates the message digest with a single byte.
     * @param in the input byte to be entered.
     */
    public void update(byte in);

    /**
     * Updates the message digest with a block of bytes.
     *
     * @param in    The byte array containing the data.
     * @param inOff The offset into the byte array where the data starts.
     * @param len   The length of the data.
     */
    public void update(byte[] in, int inOff, int len);

    /**
     * Closes the digest, producing the final digest value. The doFinal
     * call leaves the digest reset.
     *
     * @param out	The array the digest is to be copied into.
     * @param outOff	The offset into the out array the digest is to start at.
     */
    public int doFinal(byte[] out, int outOff);

    /**
     * Resets the digest back to it's initial state.
     */
    public void reset();
}
