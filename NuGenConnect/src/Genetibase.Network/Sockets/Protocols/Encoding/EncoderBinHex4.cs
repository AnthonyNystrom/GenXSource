using System;
using System.Collections;
using System.IO;

namespace Genetibase.Network.Sockets.Protocols.Encoding
{
  /// <summary>
  ///   Encoder implementation based on RFC 1741
  /// </summary>
	public class EncoderBinHex4: Encoder3To4
	{
	  protected int GetCRC(byte[] ABlock)
	  {
      int LCRC = 0;
	    for (int i = 0; i < ABlock.Length; i++)
	    {
	      AddByteCRC(ref LCRC, (byte)ABlock[i]);
	    }
	    return LCRC;
	  }
	  protected void AddByteCRC(ref int ACRC, byte AByte)
	  {
	    bool LWillSheftedOutBitBeA1 = false;
	    for (int i = 1; i <= 8; i++)
	    {
	      LWillSheftedOutBitBeA1 = (ACRC & 32768) != 0;
	      ACRC = (ACRC << 1) | (AByte >> 7);
	      if (LWillSheftedOutBitBeA1)
	      {
	        ACRC = ACRC ^ 4129;
	      }
	      AByte = (byte)((AByte << 1) & 255);
	    }
	  }
	  
	  public EncoderBinHex4()
	  {
	    _CodingTable = BinHex4.BinHex4CodeTable;
      _FillChar = '=';
	  }
	  
	  public void EncodeFile(string AFileName, Stream ASrcStream, Stream ADestStream)
	  {
	    int LN;
	    int LM;
	    int LN64;
	    int LM64;
	    int LOffset;
	    int LBlocks;
	    byte[] LOut;
	    int LSSize = (int)ASrcStream.Length;
	    int LTemp;
      byte[] LFile;
	    string LFileName;
	    int LCRC;
      byte[] LOutgoing;
	    int LRemainder;
	    byte [] TempLFile = new byte[LSSize];
	    ASrcStream.Read(TempLFile, 0, LSSize);
      LFile = new byte[TempLFile.Length];
      TempLFile.CopyTo(LFile, 0);
	    if (AFileName.Length > 255)
	    {
	      LFileName = AFileName.Substring(0, 255);
	    }
	    else
	    {
	      LFileName = AFileName;
	    }
	    LOut = new byte[LFileName.Length + 20];
	    LOut[0] = (byte)LFileName.Length;
	    for (int i = 0; i < LFileName.Length; i++)
	    {
	      LOut[1 + i] = (byte)LFileName[i];
	    }
	    LOffset = LFileName.Length + 1;
	    LOut[LOffset] = (byte)0;
	    for (int i = 1; i <= 8; i++)
	    {
	      LOut[LOffset + i] = (byte)32;
	    }
	    LOut[LOffset + 9] = (byte)0;
	    LOut[LOffset + 10] = (byte)0;
	    LTemp = LSSize;
	    LOut[LOffset + 14] = (byte)(LTemp % 256);
	    LTemp = LTemp / 256;
	    LOut[LOffset + 13] = (byte)(LTemp % 256);
	    LTemp = LTemp / 256;
	    LOut[LOffset + 12] = (byte)(LTemp % 256);
	    LTemp = LTemp / 256;
	    LOut[LOffset + 11] = (byte)LTemp;
	    LOut[LOffset + 15] = (byte)0;
	    LOut[LOffset + 16] = (byte)0;
	    LOut[LOffset + 17] = (byte)0;
	    LOut[LOffset + 18] = (byte)0;
	    LCRC = GetCRC(LOut);
      Array.Resize<byte>(ref LOut, LOut.Length + 2);
	    LOut[LOffset + 20] = (byte)(LCRC % 256);
	    LCRC = LCRC / 256;
      LOut[LOffset + 19] = (byte)LCRC;
      Array.Resize<byte>(ref LOut, LOut.Length + LSSize + 2);
	    LOffset += 21;
	    LN64 = 0;
	    while (LN64 < LSSize)
	    {
	      LOut[LN64 + LOffset] = LFile[LN64];
	      LN64++;
	    }
	    LCRC = GetCRC(LFile);
      Array.Resize<byte>(ref LFile, 0);
	    LOffset += LSSize;
	    LOut[LOffset + 1] = (byte)(LCRC % 256);
	    LCRC = LCRC / 256;
	    LOut[LOffset] = (byte)LCRC;
	    LOffset = LOut.Length % 3;
	    if (LOffset > 0)
	    {
        Array.Resize<byte>(ref LOut, LOut.Length + (3 - LOffset));
	    }
	    LOutgoing = EncodeBytes(LOut);
      Array.Resize<byte>(ref LOut, 0);
	    LSSize = LOutgoing.Length;
      Array.Resize<byte>(ref LOutgoing, LOutgoing.Length + 2);
	    LN64 = LSSize;
	    while (LN64 > 0)
	    {
	      LOutgoing[LN64] = LOutgoing[LN64 -1];
	      LN64--;
	    }
	    LOutgoing[0] = (byte)':';
	    LOutgoing[LOutgoing.Length - 1] = (byte)':';
	    LN64 = 0;
      while (LN64 < LOutgoing.Length)
	    {
	      if ((byte)LOutgoing[LN64] == 144)
	      {
          Array.Resize<byte>(ref LOutgoing, LOutgoing.Length + 1);
          LM64 = LOutgoing.Length - 1;
	        while (LM64 > LN64)
	        {
	          LOutgoing[LM64] = LOutgoing[LM64 - 1];
	          LM64 --;
	        }
	        LOutgoing[LN64 + 1] = (byte)0;
	      }
	      LN64++;
	    }
      byte[] TempBytes = Encoding.GetBytes(BinHex4.BinHex4IdentificationString + Genetibase.Network.Sockets.Global.EOL);
      ADestStream.Write(TempBytes, 0, TempBytes.Length);
      LBlocks = LOutgoing.Length;
	    LBlocks = LBlocks / 64;
      Array.Resize<byte>(ref LOut, 66);
	    for (LN = 0; LN < LBlocks; LN++)
	    {
	      LOffset = LN * 64;
	      for (LM = 0; LM < 64; LM++)
	      {
	        LOut[LM] = LOutgoing[LM + LOffset];
	      }
	      LOut[64] = (byte)13;
	      LOut[65] = (byte)10;
	      ADestStream.Write(LOut, 0, 66);
	    }
	    LRemainder = LOutgoing.Length % 64;
	    if (LRemainder > 0)
	    {
        Array.Resize<byte>(ref LOut, LOut.Length + LRemainder + 2);
	      LOffset = LBlocks * 64;
	      for (LM = 0; LM < LRemainder; LM++)
	      {
	        LOut[LM] = LOutgoing[LM + LOffset];
	      }
	      LOut[LRemainder] = (byte)13;
	      LOut[LRemainder + 1] = (byte)10;
	      ADestStream.Write(LOut, 0, LRemainder + 2);
	    }
	  }
	}
}
