using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Network.Sockets
{
  public static class Global
  {
    public const int InfiniteTimeOut = -1;
    public static bool IsInt32(string text)
    {
      int TempInt = 0;
      return Int32.TryParse(text, out TempInt);
    }

    public const char CR = '\r';
    public const char LF = '\n';
    public static readonly string EOL = new String(new char[] { CR, LF });
    public const string FetchDelimDefault = " ";
    public const bool FetchDeleteDefault = true;
    public const bool FetchCaseSensitiveDefault = true;
    public const int HoursPerDay = 24;
    public const int MinsPerHour = 60;
    public const int SecsPerMin = 60;
    public const int MSecsPerSec = 1000;
    public const int MinsPerDay = HoursPerDay * MinsPerHour;
    public const int SecsPerDay = MinsPerDay * SecsPerMin;
    public const int MSecsPerDay = SecsPerDay * MSecsPerSec;
    
    public static int StrToInt32Def(string text, int def) {
      int TempInt = 0;
      if (Int32.TryParse(text, out TempInt)) {
        return TempInt;
      } else {
        return def;
      }
    }

    public static long StrToInt64Def(string text, long def) {
      long TempInt = 0;
      if (Int64.TryParse(text, out TempInt)) {
        return TempInt;
      } else {
        return def;
      }
    }

    public static short StrToInt16Def(string text, short def) {
      short TempInt = 0;
      if (Int16.TryParse(text, out TempInt)) {
        return TempInt;
      } else {
        return def;
      }
    }

    public static string Fetch(ref string AInput, string ADelim, bool ADelete, bool ACaseSensitive) {
      int LPos = 0;
      string TempResult;
      if (ACaseSensitive) {
        LPos = AInput.IndexOf(ADelim);
        if (LPos == -1) {
          TempResult = AInput;
          if (ADelete) {
            AInput = "";
          }
        } else {
          TempResult = AInput.Substring(0, LPos);
          if (ADelete) {
            AInput = AInput.Substring(LPos + ADelim.Length);
          }
        }
      } else {
        TempResult = FetchCaseInsensitive(ref AInput, ADelim, ADelete);
      }
      return TempResult;
    }

    public static string Fetch(ref string AInput) {
      return Fetch(ref AInput, Global.FetchDelimDefault, Global.FetchDeleteDefault, Global.FetchCaseSensitiveDefault);
    }

    public static string Fetch(ref string AInput, string ADelim) {
      return Fetch(ref AInput, ADelim, Global.FetchDeleteDefault, Global.FetchCaseSensitiveDefault);
    }

    public static string Fetch(ref string AInput, string ADelim, bool ADelete) {
      return Fetch(ref AInput, ADelim, ADelete, Global.FetchCaseSensitiveDefault);
    }

    public static string FetchCaseInsensitive(ref string AInput) {
      return FetchCaseInsensitive(ref AInput, Global.FetchDelimDefault, Global.FetchDeleteDefault);
    }

    public static string FetchCaseInsensitive(ref string AInput, string ADelim) {
      return FetchCaseInsensitive(ref AInput, ADelim, Global.FetchDeleteDefault);
    }

    public static string FetchCaseInsensitive(ref string AInput, string ADelim, bool ADelete) {
      int LPos = 0;
      string TempResult;
      LPos = AInput.IndexOf(ADelim);
      if (LPos == -1) {
        TempResult = AInput;
        if (ADelete) {
          AInput = "";
        }
      } else {
        TempResult = AInput.Substring(0, LPos);
        if (ADelete) {
          AInput = AInput.Substring(LPos + 1);
        }
      }
      return TempResult;
    }
  }
}