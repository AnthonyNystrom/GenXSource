package App;


//import com.nokia.mid.ui.*;
import java.io.InputStream;
import javax.microedition.lcdui.*;

public class Sprite
{

    public Sprite(/*InputStream is*/String fileName, boolean isIndexed)
    {// creating sprite from input stream or filename

	spr_MaxWidth = 0;
	spr_MaxHeight = 0;
	spr_IsIndexed = isIndexed;

	//    - - - - -
	InputStream is = null;
	//    - - - - -
	try
	{
	    //    - - - - -
	    is = getClass().getResourceAsStream(fileName + ".spa");
	    //    - - - - -

	    int ver = SPR_VER_HI * 100 + SPR_VER_MED * 10 + SPR_VER_LO;
	    if (ver != is.read())
	    {
		return;
	    }
	    spr_Format = (byte) is.read(); // color format

	    byte ColNum = (byte) is.read(); // number of colors

	    short NumberOfPalettes = (short) is.read(); // number of palettes

	    spr_IsTransp = is.read() > 0; // transparence

	    spr_MaxFrames = is.read(); // Number of Frames

	    spr_FrameWidth = (short) is.read(); // Full frame width

	    spr_FrameHeight = (short) is.read(); // Full frame height

	    int i, dpi;
	    spr_XSm = new byte[spr_MaxFrames]; // x offsets for frames

	    spr_YSm = new byte[spr_MaxFrames]; // y offsets for frames

	    spr_Widths = new short[spr_MaxFrames]; // spr_Widths for frames

	    spr_Heights = new short[spr_MaxFrames]; // spr_Heights for frames

	    spr_Reffs = new byte[spr_MaxFrames]; // frames refferences

	    spr_Manips = new byte[spr_MaxFrames]; // frames manipulations

	    spr_Offsets = new int[spr_MaxFrames]; // offsets for frames

	    spr_Frame = spr_MaxFrames;

	    // reading delta and size of frames
	    is.read(spr_XSm);
	    is.read(spr_YSm);
	    for (i = 0; i < spr_MaxFrames; i++)
	    {
		spr_Widths[i] = (short) is.read();
	    }
	    for (i = 0; i < spr_MaxFrames; i++)
	    {
		spr_Heights[i] = (short) is.read();
	    }
	    is.read(spr_Reffs);
	    is.read(spr_Manips);

	    int maxSize = 0;
	    for (i = 0; i < spr_MaxFrames; i++)
	    { // reading offsets for frames and calculates maximum frame
		// width and height

		spr_Offsets[i] = (short) (is.read() | (is.read() << 8) | (is.read() << 16) | (is.read() << 24));
		if (spr_Reffs[i] == i)
		{
		    int size = spr_Widths[i] * spr_Heights[i];
		    if (size > maxSize)
		    {
			maxSize = size;
		    }

		    if (spr_Widths[i] > spr_MaxWidth)
		    {
			spr_MaxWidth = spr_Widths[i];
		    }
		    if (spr_Heights[i] > spr_MaxHeight)
		    {
			spr_MaxHeight = spr_Heights[i];
		    }
		}
	    }

	    // creating and reading sprite palettes
	    spr_Palettes = new short[ColNum]; //Colors

	    for (i = 0; i < ColNum; i++)
	    {
		spr_Palettes[i] = (short) (is.read() | (is.read() << 8));
	    }
	    for (i = 0; i < NumberOfPalettes - 1; i++)
	    {
		is.skip(ColNum << 1);
	    }

	    // read sprite data size
	    int DataSize = (is.read() | (is.read() << 8) | (is.read() << 16) | (is.read() << 24));

	    if (Const.SPA_TYPE == Const.SPA_TP_DG)
	    { //+ + + + + only for DirectGraphics

		if (spr_IsIndexed)
		{ // sprite in packed format

		    if (Const.SPA_IS_ZRLE)
		    { //+ + + + + only if sprite used ZRLE compression
			// we must compress data

			// array for compressed data
			byte newStoredData[] = new byte[DataSize + DataSize / 2];
			int start, end;
			short newEnd = 0;
			boolean isZero;
			int numOfZ;
			int storedData;
			for (int fr = 0; fr < spr_MaxFrames; fr++)
			{
			    if (spr_Reffs[fr] != fr)
			    {
				spr_Offsets[fr] = spr_Offsets[spr_Reffs[fr]];
				continue;
			    }

			    start = spr_Offsets[fr];
			    if (fr < spr_MaxFrames - 1)
			    {
				end = spr_Offsets[fr + 1];
			    }
			    else
			    {
				end = (short) DataSize;
			    }
			    spr_Offsets[fr] = newEnd;
			    isZero = false;
			    numOfZ = 0;
			    i = start;
			    do
			    {
				storedData = is.read();
				if (storedData != 0)
				{
				    if (!isZero)
				    {
					newStoredData[newEnd] = (byte) storedData;
					newEnd++;
				    }
				    else
				    {
					isZero = false;
					newStoredData[newEnd] = (byte) numOfZ;
					newEnd++;
					newStoredData[newEnd] = (byte) storedData;
					newEnd++;
				    }
				}
				else
				{
				    if (isZero)
				    {
					numOfZ++;
				    }
				    else
				    {
					isZero = true;
					numOfZ = 1;
					newStoredData[newEnd] = 0;
					newEnd++;
				    }
				}
			    }
			    while (++i < end);
			    if (isZero)
			    {
				newStoredData[newEnd] = (byte) numOfZ;
			    }
			    newEnd++;
			}
			DataSize = newEnd;
			spr_Data = new byte[DataSize];
			System.arraycopy(newStoredData, 0, spr_Data, 0, newEnd);
			//            for (i = 0; i < newEnd; i++)
			//            {
			//              spr_Data[i] = newStoredData[i];
			//            }
			newStoredData = null;
		    }
		    else
		    { //+ + + + + sprites not use ZRLE

			spr_Data = new byte[DataSize];
			is.read(spr_Data);
		    }

		    // cach buffer for curent frame
		    spr_SpriteData = new short[maxSize + 4];
		}
		else
		{ // sprite stored in unpacked format. we must unpack it now

		    spr_Data = new byte[DataSize + 4];
		    is.read(spr_Data, 0, DataSize);

		    int size = 0;
		    int nextFrame;
		    for (i = 0; i < spr_MaxFrames; i++)
		    { //calculate unpacked data size

			if (spr_Reffs[i] == i)
			{
			    size += spr_Widths[i] * spr_Heights[i];
			    nextFrame = i + 1;
			    while (nextFrame < spr_MaxFrames && spr_Reffs[nextFrame] != nextFrame)
			    {
				nextFrame++;
			    }

			    if (nextFrame < spr_MaxFrames)
			    {
				if (size % spr_Widths[nextFrame] != 0)
				{
				    size +=
					    (spr_Widths[nextFrame] - (size % spr_Widths[nextFrame]));
				}
			    }
			}
		    }
		    spr_SpriteData = new short[size + 4];

		    size = 0;
		    for (int n = 0; n < spr_MaxFrames; n++)
		    { //unpack data

			if (spr_Reffs[n] != n)
			{
			    spr_Offsets[n] = spr_Offsets[spr_Reffs[n]];
			    continue;
			}
			int di = spr_Offsets[n], dataByte = 0;
			int iSize = spr_Widths[n] * spr_Heights[n];
			spr_Offsets[n] = (short) size;
			int endOfSize = size + iSize;
			i = size;
			if (spr_Format == 16)
			{ // it's 16 colors format

			    do
			    {
				dataByte = spr_Data[di] & 0xFF;
				di++;
				spr_SpriteData[i++] = spr_Palettes[dataByte & 0x0F];
				spr_SpriteData[i++] = spr_Palettes[dataByte >> 4];
			    }
			    while (i < endOfSize);
			}
			else
			{ // it's 4 colors format

			    do
			    {
				dataByte = spr_Data[di] & 0xFF;
				di++;
				spr_SpriteData[i++] = spr_Palettes[dataByte & 0x03];
				dataByte >>= 2;
				spr_SpriteData[i++] = spr_Palettes[dataByte & 0x03];
				dataByte >>= 2;
				spr_SpriteData[i++] = spr_Palettes[dataByte & 0x03];
				dataByte >>= 2;
				spr_SpriteData[i++] = spr_Palettes[dataByte & 0x03];
			    }
			    while (i < endOfSize);
			}
			size = endOfSize;
			nextFrame = n + 1;
			while (nextFrame < spr_MaxFrames && spr_Reffs[nextFrame] != nextFrame)
			{
			    nextFrame++;
			}

			if (nextFrame < spr_MaxFrames)
			{
			    if (size % spr_Widths[nextFrame] != 0)
			    {
				size +=
					(spr_Widths[nextFrame] - (size % spr_Widths[nextFrame]));
			    }
			}
		    }
		    spr_Palettes = null;
		    spr_Data = null;
		    System.gc();
		}
	    }
	    else
	    {
		if (Const.SPA_TYPE == Const.SPA_TP_CLIP)
		{ //+ + + + + only for sprites with clipping

		    int width = 0, height = 0;
		    int oldOffsets[] = new int[spr_MaxFrames];
		    for (i = 0; i < spr_MaxFrames; i++)
		    { // find width and height of whole picture

			oldOffsets[i] = spr_Offsets[spr_Reffs[i]];
			if (spr_Reffs[i] != i)
			{
			    boolean b = false;
			    for (int n = 0; n < i; n++)
			    {
				if (spr_Reffs[i] == spr_Reffs[n] && spr_Manips[i] == spr_Manips[n])
				{
				    b = true;
				    break;
				}
			    }
			    if (b)
			    {
				continue;
			    }
			}
			width += spr_Widths[i];
			if (spr_Heights[i] > height)
			{
			    height = spr_Heights[i];
			}
		    }

		    int scanlineSize = width + 1;
		    int imageSize = height * (scanlineSize);
		    int imageDataSize = imageSize + ColNum * 3 + 80;
		    if (spr_IsTransp)
		    { // transparent PNGs has more 13 bytes

			imageDataSize += 13;
		    }
		    byte imageData[] = new byte[imageDataSize];
//----   IHDR
		    byte pngPreset[] =
		    {
			(byte) 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0, 0, 0, 0x0D, 0x49, 0x48, 0x44, 0x52
		    };
		    System.arraycopy(pngPreset, 0, imageData, 0, 16);
		    int imageIndex = 16;
		    int chunkStart = 12;
		    imageData[imageIndex++] = (byte) ((width >> 24) & 0xFF);
		    imageData[imageIndex++] = (byte) ((width >> 16) & 0xFF);
		    imageData[imageIndex++] = (byte) ((width >> 8) & 0xFF);
		    imageData[imageIndex++] = (byte) ((width) & 0xFF);

		    imageData[imageIndex++] = (byte) ((height >> 24) & 0xFF);
		    imageData[imageIndex++] = (byte) ((height >> 16) & 0xFF);
		    imageData[imageIndex++] = (byte) ((height >> 8) & 0xFF);
		    imageData[imageIndex++] = (byte) ((height) & 0xFF);

		    imageData[imageIndex++] = 8;
		    imageData[imageIndex++] = 3;
		    imageData[imageIndex++] = 0;
		    imageData[imageIndex++] = 0;
		    imageData[imageIndex++] = 0;

		    imageIndex = writeCRC(imageData, chunkStart, imageIndex);
//----   PLTE
		    chunkStart = imageIndex;
		    imageData[imageIndex++] = 0x50;
		    imageData[imageIndex++] = 0x4C;
		    imageData[imageIndex++] = 0x54;
		    imageData[imageIndex++] = 0x45;
		    for (i = 0; i < ColNum; i++)
		    { // writing palette

			imageData[imageIndex++] = (byte) ((spr_Palettes[i] & 0x0F00) >> 4);
			imageData[imageIndex++] = (byte) (spr_Palettes[i] & 0x00F0);
			imageData[imageIndex++] = (byte) ((spr_Palettes[i] & 0x000F) << 4);
		    }
		    imageIndex = writeCRC(imageData, chunkStart, imageIndex);
//----   tRNS
		    if (spr_IsTransp)
		    { // writing transparence data

			chunkStart = imageIndex;
			imageData[imageIndex++] = 0x74;
			imageData[imageIndex++] = 0x52;
			imageData[imageIndex++] = 0x4E;
			imageData[imageIndex++] = 0x53;

			imageData[imageIndex++] = 0x00;

			imageIndex = writeCRC(imageData, chunkStart, imageIndex);
		    }
//----   IDAT
		    chunkStart = imageIndex;
		    imageData[imageIndex++] = 0x49;
		    imageData[imageIndex++] = 0x44;
		    imageData[imageIndex++] = 0x41;
		    imageData[imageIndex++] = 0x54;

		    imageData[imageIndex++] = 0x78;
		    imageData[imageIndex++] = (byte) 0xDA;

		    imageData[imageIndex++] = (byte) 0x01;

		    imageData[imageIndex++] = (byte) (imageSize & 0xFF);
		    imageData[imageIndex++] = (byte) ((imageSize >> 8) & 0xFF);
		    int invertSize = 65535 - imageSize;
		    imageData[imageIndex++] = (byte) (invertSize & 0xFF);
		    imageData[imageIndex++] = (byte) ((invertSize >> 8) & 0xFF);
		    int dataStart = imageIndex;
		    i = dataStart + imageSize;
		    do
		    { // fill all image space by the 0

			i--;
			imageData[i] = 0;
		    }
		    while (i > dataStart);

		    int dopWidth = 0;
		    spr_Data = new byte[DataSize + 4];
		    is.read(spr_Data, 0, DataSize);
		    imageIndex++;
		    for (dpi = 0; dpi < spr_MaxFrames; dpi++)
		    {
			if (spr_Reffs[dpi] != dpi)
			{
			    boolean b = false;
			    for (int n = 0; n < dpi; n++)
			    {
				if (spr_Reffs[dpi] == spr_Reffs[n] && spr_Manips[dpi] == spr_Manips[n])
				{
				    b = true;
				    spr_Offsets[dpi] = spr_Offsets[n];
				    break;
				}
			    }
			    if (b)
			    {
				continue;
			    }
			}

			int di = oldOffsets[dpi], dataByte;
			int x = 0, y = 0;
			int mX = 0, mY = 0;
			boolean isHFlip = false;
			boolean isVFlip = false;
			boolean isRot90 = false;
			if ((spr_Manips[dpi] & Const.SPA_MP_ROT90) != 0)
			{
			    isRot90 = true;
			}
			if ((spr_Manips[dpi] & Const.SPA_MP_HFLIP) != 0)
			{
			    isHFlip = true;
			}
			if ((spr_Manips[dpi] & Const.SPA_MP_VFLIP) != 0)
			{
			    isVFlip = true;
			}

			spr_TempHeight = spr_Heights[dpi] - 1;
			spr_TempWidth = spr_Widths[dpi] - 1;
			width = spr_Widths[spr_Reffs[dpi]] - 1;
			height = spr_Heights[spr_Reffs[dpi]] - 1;
			iSize = (spr_TempHeight + 1) * (spr_TempWidth + 1);
			int tmpX;
			int n;
			if (spr_Format == 16)
			{
			    i = iSize;
			    do
			    {
				dataByte = (spr_Data[di++] & 0xFF);
				n = 2;
				do
				{

				    mX = x;
				    mY = y;
				    if (isRot90)
				    {
					tmpX = mX;
					mX = spr_TempWidth - mY;
					mY = tmpX;
				    }
				    if (isHFlip)
				    {
					mX = spr_TempWidth - mX;
				    }
				    if (isVFlip)
				    {
					mY = spr_TempHeight - mY;
				    }

				    imageData[imageIndex + dopWidth + mX + mY * scanlineSize] = (byte) (dataByte & 0x0F);
				    dataByte >>= 4;
				    x++;
				    if (x > width)
				    {
					x = 0;
					y++;
					if (y > height)
					{
					    break;
					}
				    }
				}
				while (--n > 0);
				i -= 2;
			    }
			    while (i > 0);

			}
			else
			{
			    i = iSize;
			    do
			    {
				dataByte = spr_Data[di++] & 0xFF;
				n = 4;
				do
				{
				    mX = x;
				    mY = y;
				    if (isRot90)
				    {
					tmpX = mX;
					mX = spr_TempWidth - mY;
					mY = tmpX;
				    }
				    if (isHFlip)
				    {
					mX = spr_TempWidth - mX;
				    }
				    if (isVFlip)
				    {
					mY = spr_TempHeight - mY;
				    }

				    imageData[imageIndex + dopWidth + mX + mY * scanlineSize] = (byte) (dataByte & 0x03);
				    x++;
				    if (x > width)
				    {
					x = 0;
					y++;
					if (y > height)
					{
					    break;
					}
				    }
				    dataByte >>= 2;
				}
				while (--n > 0);
				i -= 4;
			    }
			    while (i > 0);
			}

			spr_Offsets[dpi] = (short) dopWidth;
			dopWidth += spr_Widths[dpi];
		    }
		    oldOffsets = null;
		    spr_Palettes = null;
		    spr_Data = null;
		    spr_SpriteData = null;
		    System.gc();

		    imageIndex += imageSize - 1;
		    //  adler 32
		    long s1 = 1;
		    long s2 = 0;
		    i = dataStart;
		    int len = imageSize;
		    int k;
		    while (len > 0)
		    {
			k = len < 5552 ? len : 5552;
			len -= k;
			do
			{
			    s1 += imageData[i++];
			    s2 += s1;
			}
			while (--k > 0);
			s1 %= 65521L;
			s2 %= 65521L;
		    }
		    s2 = (s2 << 16) | s1;
		    imageData[imageIndex++] = (byte) ((s2 >> 24) & 0xFF);
		    imageData[imageIndex++] = (byte) ((s2 >> 16) & 0xFF);
		    imageData[imageIndex++] = (byte) ((s2 >> 8) & 0xFF);
		    imageData[imageIndex++] = (byte) ((s2) & 0xFF);
		    imageIndex = writeCRC(imageData, chunkStart, imageIndex);
//----   IEND
		    chunkStart = imageIndex;
		    imageData[imageIndex++] = 0x49;
		    imageData[imageIndex++] = 0x45;
		    imageData[imageIndex++] = 0x4E;
		    imageData[imageIndex++] = 0x44;

		    imageIndex = writeCRC(imageData, chunkStart, imageIndex);

		    clp_Sprite = Image.createImage(imageData, 0, imageDataSize);

		    imageData = null;
		    System.gc();

		}
		else
		{
		    if (Const.SPA_TYPE == Const.SPA_TP_SIMPLE)
		    {
			//        int DataByte=0;
			smp_Sprite = new Image[spr_MaxFrames];
			spr_Data = new byte[DataSize + 4];
			is.read(spr_Data, 0, DataSize);
			for (dpi = 0; dpi < spr_MaxFrames; dpi++)
			{
			    if (spr_Reffs[dpi] != dpi)
			    {
				boolean b = false;
				for (int n = 0; n < dpi; n++)
				{
				    if (spr_Reffs[dpi] == spr_Reffs[n] && spr_Manips[dpi] == spr_Manips[n])
				    {
					b = true;
					smp_Sprite[dpi] = smp_Sprite[n];
					break;
				    }
				}
				if (b)
				{
				    continue;
				}
			    }
			    int width = spr_Widths[dpi], height = spr_Heights[dpi];

			    int scanlineSize = width + 1;
			    int imageSize = height * (scanlineSize);
			    int imageDataSize = imageSize + ColNum * 3 + 80;
			    if (spr_IsTransp)
			    {
				imageDataSize += 13;
			    }
			    byte imageData[] = new byte[imageDataSize];
//----   IHDR
			    byte pngPreset[] =
			    {
				(byte) 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0, 0, 0,
				0x0D, 0x49, 0x48, 0x44, 0x52
			    };
			    System.arraycopy(pngPreset, 0, imageData, 0, 16);
			    int imageIndex = 16;
			    int chunkStart = 12;
			    imageData[imageIndex++] = (byte) ((width >> 24) & 0xFF);
			    imageData[imageIndex++] = (byte) ((width >> 16) & 0xFF);
			    imageData[imageIndex++] = (byte) ((width >> 8) & 0xFF);
			    imageData[imageIndex++] = (byte) ((width) & 0xFF);

			    imageData[imageIndex++] = (byte) ((height >> 24) & 0xFF);
			    imageData[imageIndex++] = (byte) ((height >> 16) & 0xFF);
			    imageData[imageIndex++] = (byte) ((height >> 8) & 0xFF);
			    imageData[imageIndex++] = (byte) ((height) & 0xFF);

			    imageData[imageIndex++] = 8;
			    imageData[imageIndex++] = 3;
			    imageData[imageIndex++] = 0;
			    imageData[imageIndex++] = 0;
			    imageData[imageIndex++] = 0;

			    imageIndex = writeCRC(imageData, chunkStart, imageIndex);
//----   PLTE
			    chunkStart = imageIndex;
			    imageData[imageIndex++] = 0x50;
			    imageData[imageIndex++] = 0x4C;
			    imageData[imageIndex++] = 0x54;
			    imageData[imageIndex++] = 0x45;
			    for (i = 0; i < ColNum; i++)
			    {
				imageData[imageIndex++] = (byte) ((spr_Palettes[i] & 0x0F00) >> 4);
				imageData[imageIndex++] = (byte) (spr_Palettes[i] & 0x00F0);
				imageData[imageIndex++] = (byte) ((spr_Palettes[i] & 0x000F) << 4);
			    }
			    imageIndex = writeCRC(imageData, chunkStart, imageIndex);
//----   tRNS
			    if (spr_IsTransp)
			    {
				chunkStart = imageIndex;
				imageData[imageIndex++] = 0x74;
				imageData[imageIndex++] = 0x52;
				imageData[imageIndex++] = 0x4E;
				imageData[imageIndex++] = 0x53;

				imageData[imageIndex++] = 0x00;

				imageIndex = writeCRC(imageData, chunkStart, imageIndex);
			    }
//----   IDAT
			    chunkStart = imageIndex;
			    imageData[imageIndex++] = 0x49;
			    imageData[imageIndex++] = 0x44;
			    imageData[imageIndex++] = 0x41;
			    imageData[imageIndex++] = 0x54;

			    imageData[imageIndex++] = 0x78;
			    imageData[imageIndex++] = (byte) 0xDA;

			    imageData[imageIndex++] = (byte) 0x01;

			    imageData[imageIndex++] = (byte) (imageSize & 0xFF);
			    imageData[imageIndex++] = (byte) ((imageSize >> 8) & 0xFF);
			    int invertSize = 65535 - imageSize;
			    imageData[imageIndex++] = (byte) (invertSize & 0xFF);
			    imageData[imageIndex++] = (byte) ((invertSize >> 8) & 0xFF);
			    int dataStart = imageIndex;
			    i = dataStart + imageSize;
			    do
			    { // fill all image space by the 0

				i--;
				imageData[i] = 0;
			    }
			    while (i > dataStart);

			    iSize = width * height;
			    int di = spr_Offsets[spr_Reffs[dpi]], dataByte;
			    int x = 0, y = 0;
			    int mX, mY;
			    boolean isHFlip = false;
			    boolean isVFlip = false;
			    boolean isRot90 = false;
			    if ((spr_Manips[dpi] & Const.SPA_MP_ROT90) != 0)
			    {
				isRot90 = true;
			    }
			    if ((spr_Manips[dpi] & Const.SPA_MP_HFLIP) != 0)
			    {
				isHFlip = true;
			    }
			    if ((spr_Manips[dpi] & Const.SPA_MP_VFLIP) != 0)
			    {
				isVFlip = true;
			    }

			    spr_TempHeight = spr_Heights[dpi] - 1;
			    spr_TempWidth = spr_Widths[dpi] - 1;
			    width = spr_Widths[spr_Reffs[dpi]] - 1;
			    height = spr_Heights[spr_Reffs[dpi]] - 1;
			    int tmpX;
			    imageIndex++;
			    int n;

			    if (spr_Format == 16)
			    {
				i = iSize;
				do
				{
				    dataByte = (spr_Data[di++] & 0xFF);
				    n = 2;
				    do
				    {

					mX = x;
					mY = y;
					if (isRot90)
					{
					    tmpX = mX;
					    mX = spr_TempWidth - mY;
					    mY = tmpX;
					}
					if (isHFlip)
					{
					    mX = spr_TempWidth - mX;
					}
					if (isVFlip)
					{
					    mY = spr_TempHeight - mY;
					}

					imageData[imageIndex + mX + mY * scanlineSize] = (byte) (dataByte & 0x0F);
					dataByte >>= 4;
					x++;
					if (x > width)
					{
					    x = 0;
					    y++;
					    if (y > height)
					    {
						break;
					    }
					}
				    }
				    while (--n > 0);
				    i -= 2;
				}
				while (i > 0);

			    }
			    else
			    {
				i = iSize;
				do
				{
				    dataByte = spr_Data[di++] & 0xFF;
				    n = 4;
				    do
				    {
					mX = x;
					mY = y;
					if (isRot90)
					{
					    tmpX = mX;
					    mX = spr_TempWidth - mY;
					    mY = tmpX;
					}
					if (isHFlip)
					{
					    mX = spr_TempWidth - mX;
					}
					if (isVFlip)
					{
					    mY = spr_TempHeight - mY;
					}

					imageData[imageIndex + mX + mY * scanlineSize] = (byte) (dataByte & 0x03);
					x++;
					if (x > width)
					{
					    x = 0;
					    y++;
					    if (y > height)
					    {
						break;
					    }
					}
					dataByte >>= 2;
				    }
				    while (--n > 0);
				    i -= 4;
				}
				while (i > 0);
			    }

			    imageIndex += imageSize - 1;
			    //  adler 32
			    long s1 = 1;
			    long s2 = 0;
			    i = dataStart;
			    int len = imageSize;
			    int k2;
			    while (len > 0)
			    {
				k2 = len < 5552 ? len : 5552;
				len -= k2;
				do
				{
				    s1 += imageData[i++];
				    s2 += s1;
				}
				while (--k2 > 0);
				s1 %= 65521L;
				s2 %= 65521L;
			    }
			    s2 = (s2 << 16) | s1;
			    imageData[imageIndex++] = (byte) ((s2 >> 24) & 0xFF);
			    imageData[imageIndex++] = (byte) ((s2 >> 16) & 0xFF);
			    imageData[imageIndex++] = (byte) ((s2 >> 8) & 0xFF);
			    imageData[imageIndex++] = (byte) ((s2) & 0xFF);
			    imageIndex = writeCRC(imageData, chunkStart, imageIndex);
//----   IEND
			    chunkStart = imageIndex;
			    imageData[imageIndex++] = 0x49;
			    imageData[imageIndex++] = 0x45;
			    imageData[imageIndex++] = 0x4E;
			    imageData[imageIndex++] = 0x44;

			    imageIndex = writeCRC(imageData, chunkStart, imageIndex);

			    smp_Sprite[dpi] = Image.createImage(imageData, 0, imageDataSize);

			    imageData = null;
			    System.gc();
			}
			spr_Palettes = null;
			spr_Data = null;
			spr_SpriteData = null;
			System.gc();
		    }
		}
	    }
	    is.close();
	}
	catch (Exception e)
	{
	    //            System.out.println("Error in sprite: "+fileName+"    "+e);
	    if (Const.SPA_TYPE == Const.SPA_TP_DG)
	    {
		if (!spr_IsIndexed)
		{
		    spr_Palettes = null;
		    spr_Data = null;
		}
	    }
	}
	setFrame(0);
	//    - - - - - -
	is = null;
	//    - - - - - -
	System.gc();
    }

    public void setFrame(int newFrame)
    {// sets new sprite frame

	if (Const.SPA_TYPE == Const.SPA_TP_DG)
	{ //+ + + + + only for DirectGraphics Sprites

	    if (spr_IsIndexed)
	    { // sprite in packed format, we must unpack it

		if (spr_Frame != newFrame)
		{ // we need to create new frame

		    dg_Manipulation = SPA_MANIPULATION[spr_Manips[newFrame]];
		    if (spr_Frame >= spr_MaxFrames || spr_Reffs[spr_Frame] != spr_Reffs[newFrame])
		    {
			spr_TempOffset = 0;
			spr_TempWidth = spr_Widths[spr_Reffs[newFrame]];
			spr_TempHeight = spr_Heights[spr_Reffs[newFrame]];
			iSize = spr_TempWidth * spr_TempHeight;
			int di = spr_Offsets[spr_Reffs[newFrame]], dataByte = 0;
			int numOfZ = 0;
			int i = 0;
			if (spr_Format == 16)
			{ // it's 16 colors format

			    do
			    {
				if (Const.SPA_IS_ZRLE)
				{ //+ + + + + only if ZRLE used

				    if (numOfZ == 0)
				    {
					dataByte = spr_Data[di] & 0xFF;
					di++;
					if (dataByte == 0)
					{
					    numOfZ = spr_Data[di] - 1;
					    di++;
					}
				    }
				    else
				    {
					numOfZ--;
				    }
				}
				else
				{ //+ + + + + standart unpacking

				    dataByte = spr_Data[di] & 0xFF;
				    di++;
				}
				spr_SpriteData[i] = spr_Palettes[dataByte & 0x0F];
				i++;
				spr_SpriteData[i] = spr_Palettes[dataByte >> 4];
			    }
			    while (++i < iSize);
			}
			else
			{ // it's 4 colors format

			    do
			    {
				if (Const.SPA_IS_ZRLE)
				{ //+ + + + + only if ZRLE used

				    if (numOfZ == 0)
				    {
					dataByte = spr_Data[di] & 0xFF;
					di++;
					if (dataByte == 0)
					{
					    numOfZ = spr_Data[di] - 1;
					    di++;
					}
				    }
				    else
				    {
					numOfZ--;
				    }
				}
				else
				{ //+ + + + + standart unpacking

				    dataByte = spr_Data[di] & 0xFF;
				    di++;
				}
				spr_SpriteData[i] = spr_Palettes[dataByte & 0x03];
				dataByte >>= 2;
				i++;
				spr_SpriteData[i] = spr_Palettes[dataByte & 0x03];
				dataByte >>= 2;
				i++;
				spr_SpriteData[i] = spr_Palettes[dataByte & 0x03];
				dataByte >>= 2;
				i++;
				spr_SpriteData[i] = spr_Palettes[dataByte & 0x03];
			    }
			    while (++i < iSize);
			}
		    }
		}
	    }
	    else
	    { // sprite in unpacked format. Just find new offset

		spr_TempOffset = spr_Offsets[spr_Reffs[newFrame]];
		spr_TempWidth = spr_Widths[spr_Reffs[newFrame]];
		spr_TempHeight = spr_Heights[spr_Reffs[newFrame]];
		dg_Manipulation = SPA_MANIPULATION[spr_Manips[newFrame]];
	    }
	}

	spr_Frame = newFrame;

    }

    public void draw(int x, int y)
    {
	int oldX = Core.staticG.getClipX();
	int oldY = Core.staticG.getClipY();
	int oldW = Core.staticG.getClipWidth();
	int oldH = Core.staticG.getClipHeight();
	
	if (Const.SPA_TYPE != Const.SPA_TP_DG)
	{ //+ + + + + only for Sprites without dirct graphics

	    spr_DopX = x + spr_XSm[spr_Frame];
	    spr_DopY = y + spr_YSm[spr_Frame];
	}

	if (Const.SPA_TYPE == Const.SPA_TP_CLIP)
	{ //+ + + + + only for Sprites with clipping
	    // set clip area

	    Core.staticG.setClip(spr_DopX, spr_DopY, spr_Widths[spr_Frame], spr_Heights[spr_Frame]);
	    // draw
	    Core.staticG.drawImage(clp_Sprite, spr_DopX - spr_Offsets[spr_Frame], spr_DopY, 0);
	}

	if (Const.SPA_TYPE == Const.SPA_TP_SIMPLE)
	{ //+ + + + + only for Sprites without clipping
	    // draw

	    Core.staticG.drawImage(smp_Sprite[spr_Frame], spr_DopX, spr_DopY, 0);
	}
	
	Core.staticG.setClip(oldX, oldY, oldW, oldH);
    }

    public void drawClip(int x, int y, int clipX, int clipY, int clipWidth, int clipHeight)
    {
	int oldX = Core.staticG.getClipX();
	int oldY = Core.staticG.getClipY();
	int oldW = Core.staticG.getClipWidth();
	int oldH = Core.staticG.getClipHeight();
	
	if (Const.SPA_TYPE != Const.SPA_TP_DG)
	{ //+ + + + + only for Sprites without dirct graphics

	    spr_DopX = x + spr_XSm[spr_Frame];
	    spr_DopY = y + spr_YSm[spr_Frame];
	}

	if (Const.SPA_TYPE == Const.SPA_TP_CLIP)
	{ //+ + + + + only for Sprites with clipping
	    // set clip area

	    //Core.staticG.setClip(spr_DopX, spr_DopY, spr_Widths[spr_Frame], spr_Heights[spr_Frame]);
	    Core.staticG.clipRect(spr_DopX, spr_DopY, spr_Widths[spr_Frame], spr_Heights[spr_Frame]);
	    Core.staticG.clipRect(clipX, clipY, clipWidth, clipHeight);
	    // draw
	    Core.staticG.drawImage(clp_Sprite, spr_DopX - spr_Offsets[spr_Frame], spr_DopY, 0);
	}

	if (Const.SPA_TYPE == Const.SPA_TP_SIMPLE)
	{ //+ + + + + only for Sprites without clipping
	    // draw

	    Core.staticG.clipRect(clipX, clipY, clipWidth, clipHeight);
	    Core.staticG.drawImage(smp_Sprite[spr_Frame], spr_DopX, spr_DopY, 0);
	}
	
	Core.staticG.setClip(oldX, oldY, oldW, oldH);
    }

    public void draw()
    {
	draw(sprX, sprY);
    }

    public void changeColor(short color, int index)
    {// changing one color with specified index in sprite

	if (Const.SPA_TYPE == Const.SPA_TP_DG)
	{ //+ + + + + only for DirectGraphics

	    if (spr_IsIndexed)
	    {//- - - - - only for indexed sprites

		if (spr_Palettes[index] != color)
		{ // color realy must be changed

		    spr_Palettes[index] = color;
		    spr_Frame = spr_MaxFrames;
		// now if setFrame will be used, they create new frame with changed
		// color
		}
	    }
	}
    }

    private int writeCRC(byte dataArray[], int offsetStart, int offsetEnd)
    { // write crc32 and chunk size to data array for creating PNG,
	// and return new offset to continue writing

	if (Const.SPA_TYPE != Const.SPA_TP_DG)
	{ //+ + + + + only if standart Graphics used

	    if (png_CrcTable == null)
	    { // if table not present - create it

		makeCrcTable();
	    }

	    // calculate crc32
	    long crc = 0xffffffffL;
	    for (int i = offsetStart; i < offsetEnd; i++)
	    {
		byte cd = dataArray[i];
		crc = png_CrcTable[(int) ((crc ^ cd) & 0xff)] ^ (crc >> 8);
	    }
	    crc ^= 0xffffffffL;
	    int chunkLen = offsetEnd - offsetStart - 4;

	    // write crc32
	    dataArray[offsetEnd++] = (byte) ((crc >> 24) & 0xFF);
	    dataArray[offsetEnd++] = (byte) ((crc >> 16) & 0xFF);
	    dataArray[offsetEnd++] = (byte) ((crc >> 8) & 0xFF);
	    dataArray[offsetEnd++] = (byte) ((crc) & 0xFF);

	    // write chunk size
	    dataArray[offsetStart - 4] = (byte) ((chunkLen >> 24) & 0xFF);
	    dataArray[offsetStart - 3] = (byte) ((chunkLen >> 16) & 0xFF);
	    dataArray[offsetStart - 2] = (byte) ((chunkLen >> 8) & 0xFF);
	    dataArray[offsetStart - 1] = (byte) ((chunkLen) & 0xFF);
	}
	return offsetEnd + 4;
    }

    private void makeCrcTable()
    {// creating crc32 table

	if (Const.SPA_TYPE != Const.SPA_TP_DG)
	{//+ + + + + only if standart Graphics used

	    png_CrcTable = new long[256];
	    long c;
	    int n, k;

	    for (n = 0; n < 256; n++)
	    {
		c = (long) n;
		for (k = 0; k < 8; k++)
		{
		    if ((c & 1) == 1)
		    {
			c = 0xedb88320L ^ (c >> 1);
		    }
		    else
		    {
			c = (c >> 1);
		    }
		}
		png_CrcTable[n] = c;
		//System.out.println("" + n + " = " + png_CrcTable[n]);
	    }
	}
    }
    int iSize;
    public short[] spr_SpriteData = null;
    private int[] spr_Offsets = null;
    public short[] spr_Widths = null;
    public short[] spr_Heights = null;
    public byte[] spr_Widths60 = null;
    public byte[] spr_Heights60 = null;
    public byte[] spr_XSm = null;
    public byte[] spr_YSm = null;
    public byte[] spr_Reffs = null;
    public byte[] spr_Manips = null;
    public int spr_TempOffset;
    public int spr_TempWidth;
    public int spr_TempHeight;
    public/*private*/ int spr_Frame;
    public int spr_MaxFrames;
    public short spr_MaxWidth;
    public short spr_MaxHeight;
    public short spr_FrameWidth; // Full frame width

    public short spr_FrameHeight; // Full frame height

    public int dg_Manipulation = 0;
    public boolean spr_IsTransp;
    public boolean spr_IsIndexed;
    short[] spr_Palettes = null;
    byte[] spr_Data = null;
    byte spr_Format;
    Image clp_Sprite = null;//image for SPA_TP_CLIP
    Image smp_Sprite[] = null;//image for SPA_TP_SIMPLE
    static long png_CrcTable[] = null;//crc32 table for png
    static final int SPA_MANIPULATION[] =
    {
	0, 0x2000, 0x4000, 0x2000 | 0x4000, 270, 270 | 0x2000, 270 | 0x4000, 270 | 0x2000 | 0x4000
    };
    static final int SPR_VER_HI = 0;
    static final int SPR_VER_MED = 6;
    static final int SPR_VER_LO = 5;
    public int spr_DopX,  spr_DopY;
    public int sprX,  sprY;
}
