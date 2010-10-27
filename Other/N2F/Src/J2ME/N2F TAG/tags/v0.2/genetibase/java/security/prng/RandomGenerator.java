/* ------------------------------------------------
 * RandomGenerator.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.java.security.prng;

/**
 * Generic interface for objects generating random bytes.
 */
public interface RandomGenerator
{
    /**
     * Addes more seed material to the generator.
     * @param seed  Specifies the byte array to be mixed into the generator's state.
     */
    void addSeedMaterial(byte[] seed);

    /**
     * Addes more seed material to the generator.
     * @param seed  Specifies the long value to be mixed into the generator's state.
     */
    void addSeedMaterial(long seed);

    /**
     * Fill bytes with random values.
     * @param bytes Specifies the byte array to be filled.
     */
    void nextBytes(byte[] bytes);

    /**
     * Fill part of bytes with random values.
     *
     * @param bytes	Specifies the byte array to be filled.
     * @param start	Specifies the index to start filling at.
     * @param len	Specifies the length of segment to fill.
     */
    void nextBytes(byte[] bytes, int start, int len);

}
