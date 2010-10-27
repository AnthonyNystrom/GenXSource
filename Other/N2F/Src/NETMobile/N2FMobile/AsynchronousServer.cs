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
    /// <summary>
    /// This is BOB
    /// </summary>
    public class AsynchronousServer
    {
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        public static Guid _N2FServiceGUID = new Guid("{E075D486-E23D-4887-8AF5-DAA1F6A5B172}");
        private static MainForm mainForm;
        public static Thread ListenThread;
        public static bool Listen = true;
        public static BluetoothListener BTListener;
        public static bool TerminateAllThreads = false;

        public AsynchronousServer(MainForm mainForm)
        {
            AsynchronousServer.mainForm = mainForm;
            ListenThread = new Thread(new ThreadStart(StartListening));
            ListenThread.Priority = ThreadPriority.Lowest;
            ListenThread.Start();
        }

        public static void StartListening()
        {
            BluetoothRadio.PrimaryRadio.Mode = RadioMode.Discoverable;

            BTListener = new BluetoothListener(_N2FServiceGUID);

            BTListener.Start();

            while (!TerminateAllThreads)
            {
                while (Listen)
                {
                    allDone.Reset();
                    BTListener.BeginAcceptBluetoothClient(new AsyncCallback(AcceptCallback), BTListener);
                    allDone.WaitOne();
                }

                // sleep for 500ms so when the listener is paused it doesnt munch too much cpu
                Thread.Sleep(500);
            }
        }

        public static void AcceptCallback(IAsyncResult ar)
        {

            try
            {
                // Signal the main thread to continue.
                allDone.Set();

                // Get the socket that handles the client request.
                BluetoothListener listener = (BluetoothListener)ar.AsyncState;
                BluetoothClient handler = listener.EndAcceptBluetoothClient(ar);

                // Create the state object.
                StateObject state = new StateObject();
                state.workSocket = handler;

                handler.Client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
            }
            catch { }
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            BluetoothClient handler = state.workSocket;

            // Read data from the client socket. 
            int bytesRead = handler.Client.EndReceive(ar);

            if (bytesRead > 0)
            {
                //There  might be more data, so store the data received so far.
                state.sb.Append(Encoding.ASCII.GetString(
                    state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read 
                // more data.
                content = state.sb.ToString();
                if (content.IndexOf("<EOF>") > -1)
                {
                    // All the data has been read from the 
                    // client. Display it on the console.
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}", content.Length, content);

                    try
                    {
                        mainForm.Invoke(new EventHandler(mainForm.Update_txtDevices), new object[] { new object(), EventArgs.Empty });
                    }
                    catch (Exception ex) { }

                    // Echo the data back to the client.
                    //Send(handler, content);

                    handler.Client.Shutdown(SocketShutdown.Both);
                    handler.Client.Close();
                }
                else
                {
                    // Not all data received. Get more.
                    handler.Client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }
            }
        }
    }
}