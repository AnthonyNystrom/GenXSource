using System;

namespace Genetibase.Network.Sockets.Protocols.Encoding
{
	public sealed class Base64
	{
		private Base64()
		{
		}
		
		static Base64()
		{
		  Decoder3To4.ConstructDecodeTable(CodeTable, out DecodeTable);
		}
		
		public const string CodeTable = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";
	  public static readonly byte[] DecodeTable = new byte[128];
	}
}
