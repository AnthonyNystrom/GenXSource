/* ------------------------------------------------
 * SecureRandom.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package genetibase.java.security;

import java.util.Random;

import genetibase.java.security.digests.SHA1Digest;
import genetibase.java.security.digests.SHA256Digest;
import genetibase.java.security.prng.RandomGenerator;
import genetibase.java.security.prng.DigestRandomGenerator;

/**
 * An implementation of SecureRandom specifically for the light-weight API, JDK
 * 1.0, and the J2ME. Random generation is based on the traditional SHA1 with
 * counter. Calling setSeed will always increase the entropy of the hash.
 */
public class SecureRandom extends Random
{
    /* ------ Methods.Public ------ */
    
    public byte[] generateSeed(int numBytes)
    {
        byte[] rv = new byte[numBytes];
        nextBytes(rv);
        return rv;
    }

    public void setSeed(byte[] inSeed)
    {
        _generator.addSeedMaterial(inSeed);
    }

    public void nextBytes(byte[] bytes)
    {
        _generator.nextBytes(bytes);
    }

    public void setSeed(long rSeed)
    {
        if (rSeed != 0)    // To avoid problems with Random calling setSeed in construction.
        {
            _generator.addSeedMaterial(rSeed);
        }
    }

    public int nextInt()
    {
        byte[] intBytes = new byte[4];
        nextBytes(intBytes);
        int result = 0;

        for (int i = 0; i < 4; i++)
        {
            result = (result << 8) + (intBytes[i] & 0xff);
        }

        return result;
    }
    
    /* ------ Methods.Public.Static ------ */
    
    public static SecureRandom getInstance(String algorithm)
    {
        if (algorithm.equals("SHA1PRNG"))
        {
            return new SecureRandom(new DigestRandomGenerator(new SHA1Digest()));
        }
	
        if (algorithm.equals("SHA256PRNG"))
        {
            return new SecureRandom(new DigestRandomGenerator(new SHA256Digest()));
        }
	
        return new SecureRandom();    // follow old behaviour
    }

    public static SecureRandom getInstance(String algorithm, String provider)
    {
        return getInstance(algorithm);
    }

    public static byte[] getSeed(int numBytes)
    {
        byte[] rv = new byte[numBytes];

        _rand.setSeed(System.currentTimeMillis());
        _rand.nextBytes(rv);

        return rv;
    }
    
    /* ------ Methods.Protected ------ */

    protected final int next(int numBits)
    {
        int size = (numBits + 7) / 8;
        byte[] bytes = new byte[size];

        nextBytes(bytes);

        int result = 0;

        for (int i = 0; i < size; i++)
        {
            result = (result << 8) + (bytes[i] & 0xff);
        }

        return result & ((1 << numBits) - 1);
    }
    
    /* ------ Declarations ------ */
    
    private static SecureRandom _rand = new SecureRandom(new DigestRandomGenerator(new SHA1Digest()));
    protected RandomGenerator _generator;

    /* ------ Constructors ------ */
    
    /**
     * Creates a new instance of SecureRandom.
     */
    public SecureRandom()
    {
        super(0);
        _generator = new DigestRandomGenerator(new SHA1Digest());
        setSeed(System.currentTimeMillis());
    }

    /**
     * Creates a new instance of SecureRandom.
     */
    public SecureRandom(byte[] inSeed)
    {
        super(0);
        _generator = new DigestRandomGenerator(new SHA1Digest());
        setSeed(inSeed);
    }

    /**
     * Creates a new instance of SecureRandom.
     */
    protected SecureRandom(
        RandomGenerator generator)
    {
        super(0);
        this._generator = generator;
    }
}
