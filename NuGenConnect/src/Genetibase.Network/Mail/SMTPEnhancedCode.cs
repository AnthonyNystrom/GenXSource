using System;
using System.Collections.Generic;
using System.Text;

using Genetibase.Network.Sockets;

namespace Genetibase.Network.Mail
{
  public class SMTPEnhancedCode
  {
    public const string PartSeparator = ".";
    public const string ValidClassChars = "245";
    public const int DefStatusClass = 2;
    public const int DefSubject = 0;
    public const int DefDetails = 0;
    public const bool DefAvailable = false;

    protected int mStatusClass = DefStatusClass;
    protected int mSubject = DefSubject;
    protected int mDetails = DefDetails;
    protected bool mAvailable = DefAvailable;

    public bool IsValidReplyCode(string text)
    {
      string lTmp;
      string lBuf;
      string lValidPart;
      int TempInt;
      bool TempResult = text.Trim().Length == 0;
      if (!TempResult)
      {
        lTmp = text;
        lBuf = lTmp.Substring(0, lTmp.IndexOf(" "));
        lTmp = lTmp.Remove(0, lBuf.Length);
        lValidPart = lBuf.Substring(0, lBuf.IndexOf(PartSeparator));
        lBuf = lBuf.Remove(0, lValidPart.Length);
        bool TempValidation = true;
        foreach(char c in lValidPart)
        {
          if (ValidClassChars.Contains(c.ToString()))
          {
            TempResult = false;
            break;
          }
        }
        if (TempValidation)
        {
          lValidPart = lBuf.Substring(0, lBuf.IndexOf(PartSeparator));
          lBuf = lBuf.Remove(0, lValidPart.Length);
          if ((!String.IsNullOrEmpty(lValidPart)) && Int32.TryParse(lValidPart, out TempInt))
          {
            TempResult = (!String.IsNullOrEmpty(lBuf)) && Int32.TryParse(lBuf, out TempInt);
          }
        }
      }
      return TempResult;
    }

    public int StatusClass
    {
      get
      {
        return mStatusClass;
      }
      set
      {
        if (mStatusClass != value)
        {
          if (!ValidClassChars.Contains(value.ToString()))
          {
            throw new IndyException("Invalid Status Class");
          }
          mStatusClass = value;
        }
      }
    }

    public int Subject
    {
      get
      {
        return mSubject;
      }
      set
      {
        mSubject = value;
      }
    }

    public int Details
    {
      get
      {
        return mDetails;
      }
      set
      {
        mDetails = value;
      }
    }

    public bool Available
    {
      get
      {
        return mAvailable;
      }
      set
      {
         if (mAvailable != value)
         {
           mAvailable = value;
           if (value)
           {
             mStatusClass = DefStatusClass;
             mSubject = DefSubject;
             mDetails = DefDetails;
           }
         }
      }
    }

    public string ReplyAsString
    {
      get
      {
        if (Available)
        {
          return String.Format("{1}{0}{2}{0}{3}",
                                PartSeparator,
                                mStatusClass.ToString().Substring(0, 1),
                                mSubject.ToString().Substring(0, 3),
                                mDetails.ToString().Substring(0, 3));
        }
        return "";
      }
      set
      {
        if (!IsValidReplyCode(value))
        {
          throw new Genetibase.Network.Sockets.IndyException("Invalid Reply String");
        }
        string lTmp;
        string lBuf;
        string lValidPart;
        lTmp = value;
        lBuf = lTmp.Substring(0, lTmp.IndexOf(PartSeparator));
        lTmp = lTmp.Remove(0, lBuf.Length);
        if (!String.IsNullOrEmpty(lBuf))
        {
          lValidPart = lBuf.Substring(0, lBuf.IndexOf(PartSeparator));
          lBuf = lBuf.Remove(0, lValidPart.Length);
          mStatusClass = Global.StrToInt32Def(lValidPart, 0);
          lValidPart = lBuf.Substring(0, lBuf.IndexOf(PartSeparator));
          lBuf = lBuf.Remove(0, lValidPart.Length);
          mSubject = Global.StrToInt32Def(lValidPart, 0);
          lValidPart = lBuf.Substring(0, lBuf.IndexOf(' '));
          lBuf = lBuf.Remove(0, lValidPart.Length);
          mDetails = Global.StrToInt32Def(lValidPart, 0);
          mAvailable = true;
        }
        else
        {
          mAvailable = false;
        }
      }
    }
  }
}