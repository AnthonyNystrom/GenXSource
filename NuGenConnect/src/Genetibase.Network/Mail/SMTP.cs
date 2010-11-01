/*
 * Basic SMTP Client Class
 * Revision No  Date        Notes
 *  01          12 Feb 06   Initial development. Very basic! A.Neillans
 * 
*/

using System;
using System.Collections.Generic;
using System.Text;
using Genetibase.Network.Sockets;

namespace Genetibase.Network.Mail {
  public class SMTP : TCPClient<int, ReplySMTP> {
      protected string mHELO;

      public SMTP()
      {
          // Constructor.
          mPort = 25;
      }

      ~SMTP()
      {
          // Destructor.
      }

      #region HELO Property
      public string HELO
      {
          get { return mHELO; }
          //TODO - dont allow setting while connected
          set { mHELO = value; }
      }
      #endregion

      public new void Connect()
      {
          base.Connect();
          GetResponse(220);
      }

      public void Quit()
      {
          base.Socket.Write("QUIT");
          Disconnect();
      }

      public new void Disconnect()
      {
          // This function will disconnect, without correctly 'Quiting'.
          // There are instances where this is helpful.
          base.Disconnect();
      }

      public bool SendMessage(Message AMessage)
      {
          string lString = "";
          if (AMessage != null)
          {
              AMessage.GenerateHeaders();
              // Helo / EHLO, expect 220
              base.Socket.WriteLn("HELO " + mHELO);
              lString = base.Socket.ReadLn();
              // Note, does not handle multi line responses ...
              if (lString.StartsWith("220") == false)
              {
                  throw new IndyException("Invalid HELO Response: " + lString);
              }

              // Mail From, expect 250
              base.Socket.WriteLn("MAIL FROM: " + AMessage.FromAddress);
              lString = base.Socket.ReadLn();
              if (lString.StartsWith("250") == false)
              {
                  throw new IndyException("Invalid MAIL FROM Response: " + lString);
              }
              // Rcpt To, expect 250
              base.Socket.WriteLn("RCPT TO: " + AMessage.ToAddress);
              lString = base.Socket.ReadLn();
              if (lString.StartsWith("250") == false)
              {
                  throw new IndyException("Invalid RCPT TO Response: " + lString);
              }
              // Data, expect 354
              base.Socket.WriteLn("DATA");
              // Headers
              base.Socket.WriteLn(AMessage.Headers.ToString());
              // CRLF
              base.Socket.WriteLn("");
              // Message
              base.Socket.WriteLn(AMessage.Body.ToString());
              base.Socket.WriteLn("");
              base.Socket.WriteLn(".");
              System.Diagnostics.Debug.WriteLine(base.Socket.ReadLn());
              //
          }
          return false; // MtW: Is this implemented fully yet?
      }
  }
}
