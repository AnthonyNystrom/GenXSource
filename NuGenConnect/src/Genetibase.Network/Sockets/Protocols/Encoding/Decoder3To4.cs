using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Genetibase.Network.Sockets.Protocols.Encoding
{
	public class Decoder3To4: DecoderBase
	{
	  protected string _CodingTable;
    protected byte[] _DecodeTable;
    protected char _FillChar;
	  
	  protected byte[] InternalDecode(byte[] LIn)
	  {
	    return InternalDecode(LIn, 0, -1);
	  }

    protected byte[] InternalDecode(byte[] LIn, int AStartPos)
	  {
	    return InternalDecode(LIn, AStartPos, -1);
	  }

    protected byte[] InternalDecode(byte[] LIn, int AStartPos, int ABytes)
	  {
	    byte[] LOut;
	    int LEmptyBytes = 0;
	    byte[] LInBytes = new byte[4];
	    int LOutPos = 0;
	    int LOutSize;
	    int LInLimit;
	    int LInPos;
	    byte[] LWorkBytes = new byte[4];
	    long LWhole;
	    // TODO: Change output to a MemoryStream
	    if (ABytes == -1)
	    {
	      LOutSize = (LIn.Length / 4) * 3;
	    }
	    else
	    {
	      if (ABytes % 3 > 0)
	      {
	        LOutSize = (ABytes / 3) * 3 + 3;	        
	      }
	      else
	      {
	        LOutSize = ABytes;
	      }
	    }
	    LOut = new byte[LOutSize];
	    LInPos = AStartPos;
	    LInLimit = LIn.Length - LInBytes.Length;
	    while(LInPos <= LInLimit)
	    {
        Array.Copy(LIn, LInPos, LInBytes, 0, LInBytes.Length);
	      LInPos += LInBytes.Length;
	      LWhole = 
	        (_DecodeTable[(byte)LInBytes[0]] << 18) |
	        (_DecodeTable[(byte)LInBytes[1]] << 12) |
	        (_DecodeTable[(byte)LInBytes[2]] << 6) |
	        _DecodeTable[(byte)LInBytes[3]];
        LWorkBytes = BitConverter.GetBytes(LWhole);
	      LOut[LOutPos] = LWorkBytes[2];
	      LOut[LOutPos + 1] = LWorkBytes[1];
	      LOut[LOutPos + 2] = LWorkBytes[0];
	      LOutPos += 3;
	      if (ABytes == -1)
	      {
	        if ((byte)LInBytes[2] == (byte)FillChar)
	        {
	          LEmptyBytes = 2;
	          break;
	        }
	        else
	        {
	          if ((byte)LInBytes[3] == (byte)FillChar)
	          {
	            LEmptyBytes = 1;
	            break;
	          }
	        }	        
	      }
	      else
	      {
	        if (LOutPos > ABytes)
	        {
	          LEmptyBytes = LOutPos - ABytes;
	          break;
	        }
	      }	      
	    }
      Array.Resize<byte>(ref LOut, LOut.Length - LEmptyBytes);
	    return LOut;
	  }
	  
	  public static void ConstructDecodeTable(string ACodingTable, out byte[] ADecodeArray)
	  {
      //TODO: See if we can find an efficient way, or maybe an option to see if the requested
      //decode char is valid, that is it returns a 255 from the DecodeTable, or maybe
      //check its presence in the encode table.
      ADecodeArray = new byte[128];
      for (int i = 0; i < 128; i++)
      {
        ADecodeArray[i] = 255;
      }
      for (byte i = 0; i < ACodingTable.Length; i++)
      {
        ADecodeArray[ACodingTable[i]] = i;
      }
	  }

    public override void Decode(string AIn, int AStartPos, int ABytes)
    {
      byte[] LIn = null;
      byte[] LOut = null;
      if (AIn == "")
      {
        return;
      }
      LIn = Encoding.GetBytes(AIn);
      LOut = InternalDecode(LIn, AStartPos, ABytes);
      if (ABytes == -1) {
        ABytes = LOut.Length;
      }
      _Stream.Write(LOut, 0, ABytes);
    }
    
    public Char FillChar
    {
      get
      {
        return _FillChar;
      }
      set
      {
        _FillChar = value;
      }
    }
	}
}
