using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using InTheHand;
using InTheHand.Net;

namespace N2FMobile
{
    // State object for reading client data asynchronously
    public class StateObject
    {
        // Client  socket.
        public BluetoothClient workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }
}
