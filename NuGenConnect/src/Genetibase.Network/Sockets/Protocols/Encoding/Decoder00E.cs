using System;

namespace Genetibase.Network.Sockets.Protocols.Encoding
{
	public class Decoder00E: Decoder3To4
	{
	  public override void Decode(string AIn, int AStartPos, int ABytes)
	  {
	    if (ABytes != -1)
	    {
	      base.Decode(AIn, AStartPos, ABytes);
	    }
	    else
	    {
	      if (AIn != "")
	      {
	        base.Decode(AIn, 1, _DecodeTable[(byte)AIn[1]]);
	      }
	    }
	  }
  }
}
