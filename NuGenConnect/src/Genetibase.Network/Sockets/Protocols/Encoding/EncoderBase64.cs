using System;

namespace Genetibase.Network.Sockets.Protocols.Encoding
{
	public class EncoderBase64: Encoder3To4
	{
		public EncoderBase64()
		{
		  _CodingTable = Base64.CodeTable;
      _FillChar = '=';
		}
	}
}
