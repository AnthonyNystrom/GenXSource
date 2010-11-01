using System;
using System.Collections;

namespace Genetibase.Network.Sockets.Protocols.Encoding
{
	public class DecoderBinHex4: Decoder3To4
	{
	  
	  public DecoderBinHex4()
	  {
	    
	  }
	  
	  public override void Decode(string AIn, int AStartPos, int ABytes)
	  {
	    int LCopyToPos = -1;
	    byte[] LIn;
      byte[] LOut;
	    int LRepetition;
	    int LForkLength;
	    int LN;
	    if (AIn == "")
	    {
	      return;
	    }
      LIn = Encoding.GetBytes(AIn);
	    for (int i = 0; i < LIn.Length; i++)
	    {
	      if ((byte)LIn[i] == (byte)58)
	      {
	        if (LCopyToPos == -1)
	        {
	          LCopyToPos = 0;
	        }
	        else
	        {
            Array.Resize<byte>(ref LIn, LIn.Length - LCopyToPos);
	          LCopyToPos = -1;
	          break;
	        }
	      }
	      else
	      {
	        if (LCopyToPos > -1)
	        {
	          if (((byte)LIn[i] != '\r')
	            &&((byte)LIn[i] != '\n'))
            {
              LIn[LCopyToPos] = LIn[i];
              LCopyToPos ++;
            }
	        }
	      }
	    }
      if (LCopyToPos == -1)
      {
        throw new BinHex4.MissingColonException("Block passed to DecoderBinHex4.Decode is missing a starting colon :");
      }
      else
      {
        if (LCopyToPos != -2)
        {
          throw new BinHex4.MissingColonException("Block passed to DecoderBinHex4.Decode is missing a terminating colon :");
        }
      }
      
  	  if (LIn.Length == 0)
  	  {
  	    return;
  	  }
  	  LOut = InternalDecode(LIn, AStartPos, ABytes);
	    LN = 0;
      while (LN < LOut.Length)
	    {
	      if ((byte)LOut[LN] == 144)
	      {
	        if ((byte)LOut[LN + 1] == 4)
	        {
            for (int i = LN + 1; i < LOut.Length - 1; i++)
	          {
	            LOut[i] = LOut[i + 1];
	          }
            Array.Resize<byte>(ref LOut, LOut.Length - 1);
	          LN ++;
	        }
	        else
	        {
	          LRepetition = LOut[LN + 1];
	          if (LRepetition == 1)
	          {
              for (int i = LN; i < LOut.Length - 2; i++)
	            {
	              LOut[i] = LOut[i + 2];
	            }
              Array.Resize<byte>(ref LOut, LOut.Length - 2);
	          }
	          else
	          {
	            if (LRepetition == 2)
	            {
	              LOut[LN] = LOut[LN - 1];
                for (int i = LN + 1; i < LOut.Length - 1; i++)
	              {
	                LOut[i] = LOut[i + 1];
	              }
                Array.Resize<byte>(ref LOut, LOut.Length - 1);
	              LN ++;
	            }
	            else
	            {
	              if (LRepetition == 3)
	              {
	                LOut[LN] = LOut[LN - 1];
	                LOut[LN + 1] = LOut[LN - 1];
	                LN += 2;
	              }
	              else
	              {
                  Array.Resize<byte>(ref LOut, LOut.Length + LRepetition - 3);
                  for (int i = LOut.Length - 1; i > LN + 1; i--)
	                {
	                  LOut[i] = LOut[i - (LRepetition - 3)];
	                }
	                for (int i = LN; i < LN + LRepetition - 1; i++)
	                {
	                  LOut[i] = LOut[LN - 1];
	                }
	                LN += LRepetition - 1;
	              }
	            }
	          }
	        }
	      }
        else
        {
          LN++;
        }
	    }
	    LN = 1 + (byte)LOut[0];
	    LN += 11;
	    LForkLength = ((((((byte)LOut[LN] * 256) + (byte)LOut[LN + 1])*256) + (byte)LOut[LN + 2]) * 256) + (byte)LOut[LN + 3];
	    LN += 4;
	    if (LForkLength == 0)
	    {
	      LForkLength = ((((((byte)LOut[LN] * 256) + (byte)LOut[LN + 1]) * 256) + (byte)LOut[LN + 2]) * 256) + (byte)LOut[LN + 3];
	    }
	    LN += 4;
	    LN += 2;
	    for (int i = 0; i < LForkLength; i++)
	    {
	      LOut[i] = LOut[LN + i];
	    }
      Array.Resize<byte>(ref LOut, LForkLength);
	    _Stream.Write(LOut, 0, LForkLength);
	  }
	}
}
