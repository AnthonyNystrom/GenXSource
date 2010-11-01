using System;

namespace Genetibase.Network.Sockets.Protocols.Encoding
{
	public class EncoderUUE: Encoder00E
	{
		public EncoderUUE()
		{
		  _CodingTable = UUE.CodeTable;
      _FillChar = _CodingTable[0];
		}
	}
}
