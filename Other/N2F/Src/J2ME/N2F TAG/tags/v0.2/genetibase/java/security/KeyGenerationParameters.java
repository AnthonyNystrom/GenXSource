/* ------------------------------------------------
 * KeyGenerationParameters.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.java.security;

/**
 * The base class for parameters to key generators.
 */
public class KeyGenerationParameters
{
    private SecureRandom    random;
    private int             strength;

    /**
     * Creates a new instance of KeyGenerationParameters.
     * 
     * @param random	The random byte source.
     * @param strength	The size, in bits, of the keys we want to produce.
     */
    public KeyGenerationParameters(
        SecureRandom    random,
        int             strength)
    {
        this.random = random;
        this.strength = strength;
    }

    /**
     * Return the random source associated with this generator.
     * @return	The generator's random source.
     */
    public SecureRandom getRandom()
    {
        return random;
    }

    /**
     * Returns the bit strength for keys produced by this generator.
     * @return	The strength of the keys this generator produces (in bits).
     */
    public int getStrength()
    {
        return strength;
    }
}
