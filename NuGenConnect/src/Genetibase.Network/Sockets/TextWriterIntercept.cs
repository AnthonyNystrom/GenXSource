using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Genetibase.Network.Sockets {
  public class TextWriterIntercept : ConnectionIntercept {
    private TextWriter mWriter;
    private Encoding mEncoding;
    private string mPrefix = "";

    public static string BytesToBeautyString(byte[] data, Encoding encoding) {
      string TempString = encoding.GetString(data);
      StringBuilder tempResult = new StringBuilder();
      foreach (char c in TempString) {
        if (Char.IsLetterOrDigit(c)
          || Char.IsPunctuation(c)
          || Char.IsSymbol(c)) {
          tempResult.Append(c);
          continue;
        }
        switch (c) {
          case ' ':
            tempResult.Append(" ");
            break;
          case '\r':
            tempResult.Append("\\r");
            break;
          case '\n':
            tempResult.Append("\\n");
            break;
          case '\t':
            tempResult.Append("\\t");
            break;
          case '\b':
            tempResult.Append("\\b");
            break;
          default: {
              byte[] bytes = Encoding.UTF8.GetBytes(new char[] { c });
              string tempString = "";
              foreach (byte b in bytes) {
                tempString += b.ToString("X2");
              }
              tempResult.AppendFormat("\\u{0}", tempString);
              break;
            }
        }
      }
      return tempResult.ToString();
    }

    public TextWriterIntercept(TextWriter writer, Encoding encoding) {
      mWriter = writer;
      mEncoding = encoding;
    }

    public TextWriterIntercept(TextWriter writer, Encoding encoding, string prefix)
      : this(writer, encoding) {
      mPrefix = prefix;
    }

    private void WriteLine(string message) {
      if (String.IsNullOrEmpty(mPrefix)
        || mPrefix.Trim().Length == 0) {
        mWriter.WriteLine(message);
      } else {
        mWriter.WriteLine("{0} - {1}", mPrefix, message);
      }
    }

    private void WriteLine(string format, params object[] parameters) {
      WriteLine(String.Format(format, parameters));
    }

    public override void Connect(object sender) {
      WriteLine("Connect");
    }

    public override void Disconnect() {
      WriteLine("Disconnect");
    }

    public override void Receive(ref byte[] buffer) {
      WriteLine("Receive: '{0}'", BytesToBeautyString(buffer, mEncoding));
    }

    public override void Send(ref byte[] buffer) {
      WriteLine("Send: '{0}'", BytesToBeautyString(buffer, mEncoding));
    }
  }
}