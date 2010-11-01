using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Network.Mail
{
  public class ReplyPOP3Exception: Exception
  {
    protected string mErrorCode;
    protected string mEnhancedCode;

    public ReplyPOP3Exception(string errorCode, string replyMessage, string enhancedCode):base(replyMessage)
    {
      mErrorCode = errorCode;
      mEnhancedCode = EnhancedCode;
    }

    public string ErrorCode
    {
      get{
        return mErrorCode;
      }
    }

    public string EnhancedCode
    {
      get{
        return mEnhancedCode;
      }
    }
  }
}