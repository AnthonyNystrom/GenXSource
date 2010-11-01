using System;
using System.IO;

namespace Genetibase.Network.Sockets.Protocols.Encoding
{
	public abstract class DecoderBase
	{
    protected Stream _Stream;
    private System.Text.Encoding mEncoding = System.Text.Encoding.ASCII;

    public void Decode(string AIn)
    {
      Decode(AIn, 0, -1);
    }
    
    public void Decode(string AIn, int AStartPos)
    {
      Decode(AIn, AStartPos, -1);
    }
    
    public abstract void Decode(string AIn, int AStartPos, int ABytes);    
	  
    public virtual void DecodeBegin(Stream ADestStream)
    {
      _Stream = ADestStream;
    }
    
    public string DecodeString(string AString)
    {
      using (MemoryStream ms = new MemoryStream())
      {
        DecodeBegin(ms);
        Decode(AString);
        DecodeEnd();
        ms.Position = 0;
        using (StreamReader sr = new StreamReader(ms))
        {
          try
          {
            return sr.ReadToEnd();
          }
          finally
          {
            sr.Close();
          }
        }
      }
    }
    
    public virtual void DecodeEnd()
    {
    }

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
