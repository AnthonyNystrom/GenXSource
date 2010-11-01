using System;

namespace Genetibase.Network.Sockets.Protocols.Encoding
{
	public class DecoderUUE: Decoder00E
	{
    public DecoderUUE()
    {
      _DecodeTable = UUE.DecodeTable;
      _FillChar = (char)UUE.DecodeTable[0];
    }
	}
}
