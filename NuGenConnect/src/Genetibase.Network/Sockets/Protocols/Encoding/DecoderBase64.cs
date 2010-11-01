using System;

namespace Genetibase.Network.Sockets.Protocols.Encoding
{
	public class DecoderBase64: Decoder3To4
	{
		public DecoderBase64()
		{
		  _DecodeTable = Base64.DecodeTable;
		  _CodingTable = Base64.CodeTable;
		  _FillChar = '=';
		}
	}
}
