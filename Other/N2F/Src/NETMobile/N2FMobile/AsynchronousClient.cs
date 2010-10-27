using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Windows.Forms;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using InTheHand;
using InTheHand.Net;

namespace N2FMobile
{
    /// <summary>
    /// This is ALICE
    /// </summary>
    public class AsynchronousClient
    {
        // ManualResetEvent instances signal completion.
        private ManualResetEvent connectDone = new ManualResetEvent(false);
        private ManualResetEvent sendDone = new ManualResetEvent(false);
        private ManualResetEvent receiveDone = new ManualResetEvent(false);
        private static Guid _N2FServiceGUID = new Guid("{E075D486-E23D-4887-8AF5-DAA1F6A5B172}");
        
        //device
        public static BluetoothAddress BTAddress = new BluetoothAddress(new byte[] { 183, 118, 245, 227, 23, 0, 0, 0 });

        //server
        //public static BluetoothAddress BTAddress = new BluetoothAddress(new byte[] { 38, 0, 176, 114, 2, 0, 0, 0 });

        public static BluetoothEndPoint BTEndPoint = new BluetoothEndPoint(BTAddress, _N2FServiceGUID);
        public static bool Connected;
        private BluetoothClient BTClient;
        public static bool Sending = true;
        private static MainForm mainForm;
        public static bool TerminateAllThreads = false;

        // The response from the remote device.
        private String response = String.Empty;

        public AsynchronousClient(MainForm mainForm) 
        {
            AsynchronousClient.mainForm = mainForm;
        }

        public void StartSending()
        {
            while (!TerminateAllThreads)
            {
                while (Sending)
                {
                    Thread t = new Thread(new ThreadStart(ThreadedSender));
                    t.Priority = ThreadPriority.Lowest;
                    t.Start();

                    Thread.Sleep(2000);
                }

                // sleep for 500ms so when the Sender is paused it doesnt munch too much cpu
                Thread.Sleep(500);
            }
        }

        public void ThreadedSender()
        {
            BTClient = new BluetoothClient();
            // Connect to a remote device.
            try
            {
                BTClient.BeginConnect(BTAddress, _N2FServiceGUID, new AsyncCallback(ConnectCallback), BTClient);
                connectDone.WaitOne();

                // Send test data to the remote device.
                Send(BTClient, "TagValidationString!<EOF>");
                sendDone.WaitOne();

            }
            catch (Exception ex)
            {
                KillEmAll();
            }
            finally
            {
                KillEmAll();
            }
        }

        private void KillEmAll()
        {
            if (BTClient != null)
            {
                if (BTClient.Connected)
                {
                    BTClient.Client.Shutdown(SocketShutdown.Both);
                    BTClient.Client.Close();

                }

                BTClient = null;
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                BluetoothClient socket = (BluetoothClient)ar.AsyncState;

                socket.EndConnect(ar);
                // Complete the connection.

                // Signal that the connection has been made.
                connectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());

                KillEmAll();

                // the server machine was not available
                connectDone.Set();

            }
        }

        private void Send(BluetoothClient client, String data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.
            client.Client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                BluetoothClient client = (BluetoothClient)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = client.Client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                // Signal that all bytes have been sent.
                sendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}