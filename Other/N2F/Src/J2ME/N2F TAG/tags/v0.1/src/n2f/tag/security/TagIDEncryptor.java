/* ------------------------------------------------
 * TagIDEncryptor.java
 * Copyright © 2007 Alex Nesterov
 * mailto:alex@next2friends.com
 * ---------------------------------------------- */

package n2f.tag.security;

import n2f.tag.utils.Base64;

import genetibase.java.Argument;
import genetibase.java.security.BufferedBlockCipher;
import genetibase.java.security.CryptoException;
import genetibase.java.security.modes.CBCBlockCipher;
import genetibase.java.security.params.KeyParameter;
import genetibase.java.security.engines.DESEngine;
import genetibase.java.security.paddings.PaddedBufferedBlockCipher;

/**
 * Performs DES encryption/decryption of arbitrary data.
 *
 * @author Alex Nesterov
 */
public class TagIDEncryptor
{
    /* ------ Methods.Public ------ */
    
    /**
     * Encrypts the specified byte array.
     *
     * @param data  Specifies the data to encrypt.
     * @return	The encrypted data in a different byte array.
     */
    public synchronized byte[] encrypt(byte[] data) throws CryptoException
    {
	if (data == null || data.length == 0)
	{
	    return new byte[0];
	}
	
	_cipher.init(true, _key);
	return callCipher(data);
    }
    
    /**
     * Encrypts the specified string data.
     *
     * @param data  Specifies the string to encrypt.
     * @return	The encrypted data.
     */
    public String encryptString(String data) throws CryptoException
    {
	if (Argument.isNullOrEmpty(data))
	{
	    return "";
	}
	
//	return new String(encrypt(data.getBytes()));
	return Base64.encode(encrypt(data.getBytes()));
    }
    
    /* ------ Methods.Private ------ */
    
    private byte[] callCipher(byte[] data) throws CryptoException
    {
	int size = _cipher.getOutputSize(data.length);
	byte[] result = new byte[size];
	int olen = _cipher.processBytes(data, 0, data.length, result, 0);
	olen += _cipher.doFinal(result, olen);
	
	if(olen < size)
	{
	    byte[] tmp = new byte[olen];
	    System.arraycopy(result, 0, tmp, 0, olen);
	    result = tmp;
	}
	
	return result;
    }
    
    /* ------ Declarations ------ */
    
    private BufferedBlockCipher _cipher;
    private KeyParameter _key;
    
    /* ------ Constructors ------ */
    
    /**
     * Creates a new instance of TagIDEncryptor.
     *
     * @param key   Specifies the key used for encryption. Should be at least
     *		    8 bytes long.
     */
    public TagIDEncryptor(byte[] key)
    {
	_cipher = new PaddedBufferedBlockCipher(new CBCBlockCipher(new DESEngine()));
	_key = new KeyParameter(key);
    }
    
    /**
     * Creates a new instance of TagIDEncryptor.
     *
     * @param key   Specifies the key used for encryption. Should be at least
     *		    8 chars long.
     */
    public TagIDEncryptor(String key)
    {
	this(key.getBytes());
    }
}
