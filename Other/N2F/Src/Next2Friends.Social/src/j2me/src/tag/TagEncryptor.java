package tag;

public class TagEncryptor
{
    private DES des;

    public TagEncryptor(byte[] key)
    {
	des = new DES(key);
    }

    public String encrypt(String data)
    {
	int blockSize = App.Const.TAG_BLOCKSIZE;

	byte[] cbc = new byte[blockSize];
	byte[] endBuf = new byte[blockSize];
	for(int i = 0; i < blockSize; ++i)
	{
	    endBuf[i] = (byte) blockSize;
	}

	byte[] strData = data.getBytes();
	int len = strData.length;
	byte[] out = new byte[len + blockSize];

	int offset = 0;
	for(; offset < len;)
	{
	    for(int i = 0; i < blockSize; i++)
	    {
		cbc[i] ^= strData[offset + i];
	    }
	    int length = des.processBlock(cbc, 0, out, offset);
	    System.arraycopy(out, offset, cbc, 0, blockSize);
	    
	    offset += length;
	}
	des.processBlock(endBuf, 0, out, offset);

	return App.Base64.encode(out);
    }

    public String makeValidationString(String tagId)
    {
	long ticks = System.currentTimeMillis() * 10000 + App.Const.TICKS_DOTNETINIT;
	return tagId + Long.toString(ticks);
    }
}
