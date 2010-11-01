using System;
using JH.CommBase;
using System.Text;

namespace Genetibase.Debug
{
	/// <summary>
	/// Summary description for miniterminal.
	/// </summary>
	/// 
  public delegate void UpdateRxString(string str);
	public class miniterminal : JH.CommBase.CommBase
	{
    public CommBaseSettings ComSettings;
    private bool ShowHex = false;
    private UInt32 doCR  = 0xFFFF ;  
    private UpdateRxString pCallBackRxUpdate;

		public miniterminal(UpdateRxString pUpdStr)
		{
      this.ComSettings = new CommBase.CommBaseSettings();
      pCallBackRxUpdate = pUpdStr;
		}

    protected override CommBaseSettings CommSettings()
    {
      return ComSettings;
    }

    public void SendByte(byte b)
    {
      if((ASCII)b == ASCII.CR || (ASCII) b == ASCII.LF)
      {
        base.Send((byte)ASCII.CR);
        base.Send((byte)ASCII.LF);
        return;
        
      }
      base.Send(b);
    }

    public void SendString(string str)
    {
      Encoding enc = Encoding.ASCII;
      byte[] s = enc.GetBytes(str);
      base.Send(s);
    }
    protected override void OnRxChar(byte ch)
    {
      string tmp="";
      if(!ShowHex)
      {
        if((ASCII)ch == ASCII.CR || (ASCII)ch == ASCII.LF )
        {
          tmp+=(char)ch;//tmp+="\r\n";
        }
        else
        {
          tmp+=(char)ch;
        }
      }
      else
      {
        tmp = "0x" + String.Format("{0:x2}", ch)+" ";
        if(doCR < 0x100)
        {
          if(ch == (byte)doCR)
           tmp+="\r\n";
        }
      }

      pCallBackRxUpdate(tmp);
    }

    public bool ShowRxHexValues
    {
      set { ShowHex = value;}
    }
    public UInt32 SetCrHexValue
    {
      set {doCR = value;}
    }
	}
}
