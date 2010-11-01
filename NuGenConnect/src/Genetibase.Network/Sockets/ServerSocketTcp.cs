using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Genetibase.Network.Sockets
{
  public class ServerSocketTcp: ServerSocket
  {
    private int mPort;
    private System.Net.Sockets.Socket mSocket;
    private static bool Select(System.Net.Sockets.Socket socket, int timeout)
    {
      List<System.Net.Sockets.Socket> sockets = new List<System.Net.Sockets.Socket>();
      sockets.Add(socket);
      System.Net.Sockets.Socket.Select(sockets, null, null, timeout * 1000);
      return sockets.Count == 1;
    }

    public override void StartListening()
    {
      mSocket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      mSocket.Bind(new IPEndPoint(IPAddress.Any, mPort));
      mSocket.Listen(5);      
    }

    public override void StopListening()
    {
      mSocket.Close();      
    }

    public override int Port
    {
      get
      {
        return mPort;
      }
      set
      {
        mPort = value;
      }
    }

    protected override Socket DoAccept()
    {
      while (true)
      {
        if (Select(mSocket, 250))
        {
          Socket s = new SocketTCP(mSocket.Accept());
          return s;
        }
      }
    }
  }
}