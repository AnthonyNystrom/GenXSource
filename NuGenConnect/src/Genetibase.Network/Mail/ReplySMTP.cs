using System;
using System.Collections.Generic;
using System.Text;

using Genetibase.Network.Sockets;

namespace Genetibase.Network.Mail
{
  public class ReplySMTP: ReplyRFC
  {
    protected SMTPEnhancedCode mEnhancedCode = new SMTPEnhancedCode();

    public void SetEnhancedReply(int numericCode, string enhancedReply, string text)
    {
    }

    public SMTPEnhancedCode EnhancedCode
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

    public override byte[] FormattedReply
    {
      get
      {
        StringBuilder resultBuilder = new StringBuilder();
        if (Code > 0)
        {
          string lCode = Code.ToString();
          if (Text.Count > 0)
          {
            for (int i = 0; i < Text.Count; i++)
            {
              if (i < (Text.Count - 1))
              {
                if (EnhancedCode.Available)
                {
                  resultBuilder.AppendLine(lCode + "-" + EnhancedCode.ReplyAsString + " " + Text[i]);
                }
                else
                {
                  resultBuilder.AppendLine(lCode + "-" + Text[i]);
                }
              }
              else
              {
                if (EnhancedCode.Available)
                {
                  resultBuilder.AppendLine(lCode + " " + EnhancedCode.ReplyAsString + " " + Text[i]);
                }
                else
                {
                  resultBuilder.AppendLine(lCode + " " + Text[i]);
                }
              }
            }
          }
          else
          {
            if (EnhancedCode.Available)
            {
              resultBuilder.AppendLine(lCode + " " + EnhancedCode.ReplyAsString);
            }
            else
            {
              resultBuilder.AppendLine(lCode);
            }
          }
        }
        else
        {
          foreach (string s in Text)
          {
            resultBuilder.AppendLine(s);
          }
        }
        return Encoding.ASCII.GetBytes(resultBuilder.ToString());
      }
      set
      {
        Clear();
        string[] ReplyLines = Encoding.ASCII.GetString(value).Split('\r');
        if (ReplyLines.Length > 0)
        {
          // in Indy 10, there was POP3 stuff in here?
          string CodeText = ReplyLines[0].Substring(0, 3);
          Code = Int32.Parse(CodeText);
          foreach(string s in ReplyLines)
          {
            string TempString = s.Substring(4);
            if (mEnhancedCode.IsValidReplyCode(TempString))
            {
              mEnhancedCode.ReplyAsString = TempString;
            }
            Text.Add(TempString);
          }
        }
      }
    }
  }
}