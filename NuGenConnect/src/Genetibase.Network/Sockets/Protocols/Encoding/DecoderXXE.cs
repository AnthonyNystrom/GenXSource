using System;

namespace Genetibase.Network.Sockets.Protocols.Encoding
{
	public class DecoderXXE: Decoder00E
	{
		public DecoderXXE()
		{
		  _DecodeTable = XXE.DecodeTable;
		  _FillChar = (char)_DecodeTable[0];
		}
	}
}
