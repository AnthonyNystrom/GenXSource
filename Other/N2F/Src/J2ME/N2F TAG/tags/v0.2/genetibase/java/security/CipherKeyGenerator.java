/* ------------------------------------------------
 * CipherKeyGenerator.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.java.security;

/**
 * The base class for symmetric, or secret, cipher key generators.
 */
public class CipherKeyGenerator
{
    protected SecureRandom  random;
    protected int           strength;

    /**
     * Initialise the key generator.
     *
     * @param param The parameters to be used for key generation
     */
    public void init(
        KeyGenerationParameters param)
    {
        this.random = param.getRandom();
        this.strength = (param.getStrength() + 7) / 8;
    }

    /**
     * Generates a secret key.
     * @return	A byte array containing the key value.
     */
    public byte[] generateKey()
    {
        byte[]  key = new byte[strength];
        random.nextBytes(key);
        return key;
    }
}
