using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Network.Sockets {
  public class ReplyRFC : Reply<int> {
    private List<string> mText = new List<string>();

    protected override bool IsCodeValid(int code) {
      return ((code >= 100)
        && (code < 1000))
        || Code == 0;
    }

    public override void ThrowReplyError() {
      throw new IndyException(mText + " Code: " + Code.ToString());
    }

    public override void Clear() {
      base.Clear();
      mText.Clear();
    }

    public List<string> Text {
      get {
        return mText;
      }
    }

    public override byte[] FormattedReply {
      get {
        StringBuilder resultBuilder = new StringBuilder();
        if (Code > 0) {
          if (Text.Count > 0) {
            string LCodeStr = Code.ToString();
            for (int i = 0; i < Text.Count; i++) {
              if (i < (Text.Count - 1)) {
                resultBuilder.Append(LCodeStr + "-" + Text[i] + Global.EOL);
              } else {
                resultBuilder.Append(LCodeStr + " " + Text[i] + Global.EOL);
              }
            }
          } else {
            resultBuilder.Append(Code.ToString() + Global.EOL);
          }
        } else {
          foreach (string s in Text) {
            resultBuilder.Append(s + Global.EOL);
          }
        }
        return Encoding.ASCII.GetBytes(resultBuilder.ToString());
      }
      set {
        Clear();
        byte[] input = value;
        if (input[input.Length - 1] == Global.LF) {
          Array.Resize<byte>(ref input, input.Length - 1);
          if (input[input.Length - 1] == Global.CR) {
            Array.Resize<byte>(ref input, input.Length - 1);
          }
        }
        if (input.Length <= 3) {
          this.Code = Int32.Parse(Encoding.ASCII.GetString(input));
          return;
        }
        string[] ReplyLines = Encoding.ASCII.GetString(input).Split(new char[] { '\n' }, StringSplitOptions.None);
        if (ReplyLines.Length > 0) {
          Code = Int32.Parse(ReplyLines[0].Substring(0, 3));
          foreach (string s in ReplyLines) {
            if (s.Length > 4) {
              if (s.EndsWith("\r")) {
                Text.Add(s.Substring(4, s.Length - 5));
              } else {
                Text.Add(s.Substring(4));
              }
            } else {
              Text.Add("");
            }
          }
        }
      }
    }

    public override void ReadFromSocket(Socket socket) {
      Clear();
      string xLine = socket.ReadLn();
      while (xLine.Length > 3 
        && xLine[3] == ' ') {
        if (Code == 0) {
          Code = Int32.Parse(xLine.Substring(0, 3));
        }
        Text.Add(xLine.Substring(4));
        xLine = socket.ReadLn();
      }
      Text.Add(xLine.Substring(4));
    }
  }
}