using System;
using System.Collections.Generic;
using System.Text;

using Genetibase.Network.Sockets;

namespace Genetibase.Network.Mail
{
  public class ReplyPOP3: Reply<string>
  {
    public const string ST_OK = "+OK";
    public const string ST_ERR = "-ER";
    public const string ST_SASLCONTINUE = "+";
    public static readonly string[] VALID_POP3_STR;

    private static int CaseInsensitiveIndexInStringArray(string[] array, string item)
    {
      for (int i = 0; i < array.Length; i++)
      {
        if (array[i].Equals(item, StringComparison.InvariantCultureIgnoreCase))
        {
          return i;
        }
      }
      return -1;
    }

    private static int ExtractTextPosArray(string text)
    {
      return CaseInsensitiveIndexInStringArray(VALID_POP3_STR, text.Substring(0, FindCodeTextDelim(text)));
      //      PosInStrArray(Copy(AStr, 1, FindCodeTextDelim(AStr) - 1), VALID_POP3_STR, False);
    }

    private static int FindCodeTextDelim(string text)
    {
      int LMin;
      int LSpace;
      string LBuf = text;
      bool LAddBackFlag = false;
      if (LBuf.StartsWith("-"))
      {
        LBuf = LBuf.Substring(1);
        LAddBackFlag = true;
      }
      LMin = LBuf.IndexOf(" ");
      LSpace = LBuf.IndexOf("-");
      int TempResult;
      if (LMin > -1)
      {
        if ((LSpace != -1)
          && (LMin > LSpace))
        {
          TempResult = LSpace;
        }
        else
        {
          TempResult = LMin;
        }
      }
      else
      {
        if (LSpace != 0)
        {
          TempResult = LSpace;
        }
        else
        {
          TempResult = text.Length;
        }
      }
      if (LAddBackFlag)
      {
        TempResult += 1;
      }
      return TempResult;
    }

    static ReplyPOP3()
    {
      VALID_POP3_STR = new string[] { ST_OK, ST_ERR, ST_SASLCONTINUE };
    }

    private List<string> mText = new List<string>();
    private string mEnhancedCode;

    protected override bool IsCodeValid(string code) {
      return true;
      // TODO: 
    }

    public override void ReadFromSocket(Socket socket) {
      string TempStrings = "";
      while (true) {
        string tempLine = socket.ReadLn();
        TempStrings += tempLine + Global.EOL;
        int xPos = FindCodeTextDelim(tempLine);
        if (xPos > -1) {
          if (xPos >= tempLine.Length) {
            break;
          } else {
            if (tempLine[xPos] != '-') {
              break;
            }
          }
        }
      }
      FormattedReply = Encoding.ASCII.GetBytes(TempStrings);

    }

    /*public override int IndexAfterEndMarker(Genetibase.Network.Sockets.Genetibase.Network.Sockets.Buffer buffer)
    {
      if (buffer.Size == 0)
      {
        return -1;
      }
      int TempResult = -1;
//      TempResult = Math.Max(TempResult, buffer.IndexOf(ST_OK));
//      TempResult = Math.Max(TempResult, buffer.IndexOf(ST_ERR));
//      TempResult = Math.Max(TempResult, buffer.IndexOf(ST_SASLCONTINUE));
//
//      if (TempResult > -1)
//      {
      
        TempResult = buffer.IndexOf("\n", TempResult) + 1;
//      }
      return TempResult;
    }   */

    public List<string> Text
    {
      get
      {
        return mText;
      }
    }

    public string TextAsString
    {
      get
      {
        StringBuilder result = new StringBuilder();
        foreach (string s in mText)
        {
          result.AppendLine(s);
        }
        return result.ToString();
      }
    }

    public override void ThrowReplyError()
    {
      throw new ReplyPOP3Exception(Code, TextAsString, mEnhancedCode);
    }

    public string EnhancedCode
    {
      get
      {
        return mEnhancedCode;
      }
      set
      {
        mEnhancedCode = value;
      }
    }

    private static bool IsValidEnhancedCode(string text, bool strict)
    {
      return false;
    }

    public override byte[] FormattedReply
    {
      get
      {
        StringBuilder result = new StringBuilder();
        if (!String.IsNullOrEmpty(Code))
        {
          if (Text.Count > 0)
          {
            for (int i = 0; i < Text.Count; i++)
            {
              if (i < (Text.Count - 1))
              {
                if (Code == ST_ERR
                  && !String.IsNullOrEmpty(mEnhancedCode))
                {
                  result.AppendLine(Code + "-" + mEnhancedCode + " " + Text[i]);
                }
                else
                {
                  result.AppendLine(Code + "-" + Text[i]);
                }
              }
              else
              {
                if (Code == ST_ERR
                  && !String.IsNullOrEmpty(mEnhancedCode))
                {
                  result.AppendLine(Code + " " + mEnhancedCode + " " + Text[i]);
                }
                else
                {
                  result.AppendLine(Code + " " + Text[i]);
                }
              }
            }
          }
          else
          {
            result.AppendLine(Code);
          }
        }
        else
        {
          if (Text.Count > 0)
          {
            return Encoding.ASCII.GetBytes(TextAsString);
          }
        }
        return Encoding.ASCII.GetBytes(result.ToString());
      }
      set
      {
        Clear();
        if (value.Length > 0)
        {
          string[] Values = Encoding.ASCII.GetString(value).Split('\n');
          int LPos = ExtractTextPosArray(Values[0]);
          if (LPos > -1)
          {
            Code = VALID_POP3_STR[LPos];
          }
          for (int i = 0; i < Values.Length - 1; i++)
          {
            if (LPos == -1)
            {
              LPos = CaseInsensitiveIndexInStringArray(VALID_POP3_STR, Values[i]);
            }
            int Idx = FindCodeTextDelim(Values[i]);
            string LBuf = Values[i].Substring(Idx);
            if (Code == ST_ERR
              && IsValidEnhancedCode(LBuf.Substring(0, LBuf.IndexOf(" ")), false))
            {
              mEnhancedCode = LBuf.Substring(0, LBuf.IndexOf(" "));
              LBuf = LBuf.Remove(0, LBuf.IndexOf(" ") + 1);
            }
            mText.Add(LBuf);
          }
          if (LPos == -1)
          {
            Code = ST_ERR;
          }
        }
      }
    }
  }
}