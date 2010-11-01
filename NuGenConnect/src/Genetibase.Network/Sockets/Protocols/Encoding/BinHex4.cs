using System;

namespace Genetibase.Network.Sockets.Protocols.Encoding
{
	/// <summary>
	/// Description of BinHex.	
	/// </summary>
	public sealed class BinHex4
	{
	  public class MissingColonException: Genetibase.Network.Sockets.IndyException
	  {
	    public MissingColonException(string AMsg):base(AMsg)
	    {
	    }
	  }
	  
		private BinHex4()
		{
		}
		
		// Note the 2nd characeter is a " which is represented in a string as ""
		public const string BinHex4CodeTable = @"!""#$%&'()*+,-012345689@ABCDEFGHIJKLMNPQRSTUVXYZ[`abcdefhijklmpqr";
	  public const string BinHex4IdentificationString = "(This file must be converted with BinHex 4.0)";
	}
}
