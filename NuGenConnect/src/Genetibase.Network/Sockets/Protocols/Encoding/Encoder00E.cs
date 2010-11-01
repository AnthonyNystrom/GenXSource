using System;
using System.IO;

namespace Genetibase.Network.Sockets.Protocols.Encoding
{
	public class Encoder00E: Encoder3To4
	{
	  public override string Encode(Stream ASrcStream, int ABytes)
	  {
	    int LStart = (int)ASrcStream.Position;
	    string TempResult = base.Encode(ASrcStream, ABytes);
	    return _CodingTable[(int)ASrcStream.Position - LStart + 1] + TempResult;
	  }
	}
}
