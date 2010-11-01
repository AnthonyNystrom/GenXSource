using System;

namespace Genetibase.Network.Sockets {
  //Kudzu
  //TCPClient is a common client base for connection based clients. Not
  //all clients will descend from this. Clients such as HTTP are not
  //connection based and instead may choose to use Socket directly, or use
  //TCPClient internally.
  public class TCPClient<TReplyCode, TReply>: TcpConnection<TReplyCode, TReply>
    where TReply: Reply<TReplyCode>, new()
    where TReplyCode: IEquatable<TReplyCode>
  {
  
    #region Connect Method
    //Kudzu
    // I purposefully did not add Connect(Host) and Connect(Host, Port) since
    // C# does not support default arguments. They just add clutter
    // and only serve basic but not functional syntactic sugar
    public void Connect() {
      Connect(new SocketTCP(mHost, mPort));
    }
    public virtual void Connect(
      Socket aSocket
    ) {
      mSocket = aSocket;
      mSocket.Open();
    }
    #endregion

    //Kudzu
    // I thought about this implementation of host and port for a long time with EndPoint.
    //
    // There are two basic needs:
    // 1) 99% of the time the user is dealing with a socket, so we want it to 
    // act like a socket and not make the user jump through hoops. 
    // 2) 1% of the time it needs to be abstracted because it could be a file, or really anything. 
    //
    // The compromise I chose is this. Sockets have an EndPoint but
    // can also expose other properties such as SocketTCP does with Host and Property. But externally at the
    // base level only EndPoint is exposed. TCPClient however exposes only Host and Port. If the user wants a 
    // custom socket they will need to create and assign it to TCPClient. Thus the user will interact with
    // the socket class directly anyways. The host and port in TCPClient serve the purpose that 99% of the time
    // the socket class is created implicitly, and thus the user does not want to deal with the socket class
    // directly, at least before connection. After connection the user may possibly call IO methods of the socket,
    // but Host and Port are needed before its creation. So this whole scheme boils down to flexibility and
    // ease of use in the 99% usage pattern. EndPoint was not mirrored in TCPClient as it is a big redundant, again if the user wants to use a custom
    // socket they will already be interacting with it directly.
    //
    // Another option was to extract EndPoint out as a class as well, but that just adds too many extra layers
    // and it would end up in all practical use of 1:1 EndPoint classes per socket class anyways.

    #region Host Property
    protected string mHost;
    public string Host {
      get { return mHost; }
      //TODO - dont allow setting while connected
      set { mHost = value; }
    }
    #endregion

    #region Port Property
    protected int mPort;
    public int Port {
      get { return mPort; }
      //TODO - dont allow setting while connected
      set { mPort = value; }
    }
    #endregion

  }
}