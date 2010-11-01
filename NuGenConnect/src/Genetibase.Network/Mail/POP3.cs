using System.Collections.Generic;
using Genetibase.Network.Sockets;

namespace Genetibase.Network.Mail {

  public class POP3: TCPClient<string, ReplyPOP3>{
  
    const string ST_OK="+OK";
    const string ST_ERR="-ERR";
  
    private string _username;
    private string _password;
    
    public string Username {
      get { return _username;}
      set { _username = value;}
    }
    
    public string Password {
      get { return _password;}
      set { _password = value;}
    }
    
    public POP3() {
      
      Port = 110;
    }

    public override void Connect(Socket aSocket)
    {
      base.Connect(aSocket);
      GetResponse(ST_OK);
      Greeting.FormattedReply = LastCmdResult.FormattedReply;
    }

    public override void DisconnectNotifyPeer()
    {
      base.DisconnectNotifyPeer();
      SendCmd("QUIT", ST_OK);
    }
    
    public void Login() {    
      SendCmd("USER " + Username, ST_OK);
      SendCmd("PASS " + Password, ST_OK);
    }
    
    public int GetMessageCount() {
    
      SendCmd("STAT", ST_OK);
      return 0;
    }
    
    public List<string> RetrieveRaw(int aMessageNumber) {
      
      SendCmd("RETR " + aMessageNumber.ToString(), ST_OK);
      string xResponse = Socket.ReadLn();
      List<string> xMessage = new List<string>();
      
      while (xResponse != ".") {
        xMessage.Add(xResponse);
        xResponse = Socket.ReadLn();
      }
      return xMessage;
    }
        
  }
}
