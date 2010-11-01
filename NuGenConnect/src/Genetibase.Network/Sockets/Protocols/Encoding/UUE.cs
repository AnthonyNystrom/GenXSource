using System;

namespace Genetibase.Network.Sockets.Protocols.Encoding
{
	public sealed class UUE
	{
		private UUE()
		{
		}
		
		static UUE()
		{
		  Decoder3To4.ConstructDecodeTable(CodeTable, out DecodeTable);
		  DecodeTable[(byte)' '] = DecodeTable[(byte)'`'];
		}
		
		// note the embedded "
		public const string CodeTable = @"`!""#$%&'()*+,-./0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_";
	  public static readonly byte[] DecodeTable;
	}
}
