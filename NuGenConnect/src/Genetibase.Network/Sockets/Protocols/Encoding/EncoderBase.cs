using System;
using System.IO;

namespace Genetibase.Network.Sockets.Protocols.Encoding
{
	public abstract class EncoderBase
	{
    private System.Text.Encoding mEncoding = System.Text.Encoding.ASCII;
	  public string Encode(string ASrc)
	  {
      using (MemoryStream ms = new MemoryStream(this.Encoding.GetBytes(ASrc)))
	    {
	      return Encode(ms);
	    }
	  }
	  
	  public string Encode(Stream ASrcStream)
	  {
	    return Encode(ASrcStream, Int32.MaxValue);
	  }
	  
	  public abstract string Encode(Stream ASrcStream, int ABytes);

    public System.Text.Encoding Encoding {
      get {
        return mEncoding;
      }
      set {
        mEncoding = value;
      }
    }
	}
}
