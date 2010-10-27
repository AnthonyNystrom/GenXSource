using System;
using System.Threading;

using InTheHand.Net;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;

namespace goAbout.Engine
{
	/// <summary>
	/// Summary description for BluetoothCom.
	/// </summary>
	public class BluetoothCom
	{
		#region properties

        /// <summary>
        /// The GUID of this program
        /// </summary>
 		private Guid serviceGUID = new Guid("{E075D486-E23D-4887-8AF5-DAA1F6A5B172}");
   
        /// <summary>
        /// The Main BluetoothClient for bluetooth connectivity
        /// </summary>
		private BluetoothClient btClient;

        /// <summary>
        /// Bluetooth listener for imcoming connections
        /// </summary>
		public BluetoothListener  btListener;

        /// <summary>
        /// An array of all bluetooth devices that have been discovered
        /// </summary>
		public BluetoothDeviceInfo[] bTDevices=new BluetoothDeviceInfo[0];

		//protocol
        private int majorVersion = 0;
		private int minorVersion = 1;
		private string mACAddress = "";

        /// <summary>
        /// False when threads are to be terminated
        /// </summary>
        public static bool run=true;

        /// <summary>
        /// The Major version number of the this program
        /// </summary>
		public int MajorVersion
		{
            get { return majorVersion; }
		}

        /// <summary>
        /// The Minor version number of the this program
        /// </summary>
		public int MinorVersion
		{
            get { return minorVersion; }
		}

        /// <summary>
        /// The Bluetooth MAC address of the device
        /// </summary>
		public string MACAddress
		{
			get{return mACAddress;}
			set{mACAddress=value;}
		}

        /// <summary>
        /// Returns an array of all bluetooth devices that have been discovered
        /// </summary>
		public BluetoothDeviceInfo[] BTDevices
		{
			get{return bTDevices;}
		}

		#endregion

        /// <summary>
        /// creates Bluetooth connectivity via the InTheHand framework
        /// </summary>
        public BluetoothCom()
        {
            try
            {
                // set this device to discoverable so other devices can see it
                BluetoothRadio.PrimaryRadio.Mode = RadioMode.Discoverable;

                // store this devices bluetooth unique MAC address
                this.mACAddress = BluetoothRadio.PrimaryRadio.LocalAddress.ToString();
            }
            catch (Exception ex)
            {
                // show a messagebox regarding bluetooth devices
                // IE "No bluetooth devices present"
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Send Members profile to another bluetooth device
        /// </summary>
        /// <param name="address">The address object for the remote Member</param>
        /// <param name="gotTheirProfile">1 if this Member has the remote Members profile</param>
		private void SendBluetoothProfile(BluetoothAddress address, int gotTheirProfile)
		{		
			try
			{
                // Create a BluetoothClient object
				BluetoothClient btSend = new BluetoothClient();

                // Connect to the remote member with the serviceGUID for service identification
				btSend.Connect(new BluetoothEndPoint(address, serviceGUID)); 

                // get the stream from the connetion
				System.IO.Stream s = btSend.GetStream();

                // serialise the this Members profile into a string
				string header = "";

				header += this.MajorVersion+",";
				header += this.MinorVersion+",";
				header += Member.Age+",";
				header += ((int)Member.Gender).ToString()+",";
				header += Member.SeekAge+",";
				header += ((int)Member.SeekGender).ToString()+",";
				header += MainForm.member.MemberID+",";
				header += this.MACAddress+",";
				header += gotTheirProfile.ToString();

                // convert the header string into a byte array for transmission
				byte[] bs = System.Text.Encoding.Unicode.GetBytes(header);

                // write the byte array to the steam object
				s.Write(bs, 0, bs.Length);

                // Flush the buffer and transmit the profile
				s.Flush();

                // close the BluetoothClient connection
				btSend.Close();

			}
			catch(Exception ex)
			{
                throw ex;
			}
		}

        /// <summary>
        /// Threaded loop discovers new devices in range and sends this Members profile
        /// if they have not yet received it.
        /// </summary>
		public void DiscoverLoop()
		{
            // on exit, run is set to false to terminate all threads
			while(run)
			{
                // create a new bluetooth client object
				BluetoothClient bt = new BluetoothClient();

                // return a maximum of 10 devices
				bTDevices = bt.DiscoverDevices(10);

                //close the bluetooth client object
				bt.Close();

                // loop through all the devices found
				for(int i=0;i<BTDevices.Length;i++)
				{
                    // check again if the application is exiting and terminate threads
                    // this makes the application end much faster
                    if (!run)
                    {
                        // stop the listener
                        btListener.Stop();
                        // exit the loop
                        break;
                    }

					try
					{
                        // the device isnt looing at itself
                        if (BTDevices[i].DeviceAddress.ToString() != this.MACAddress)
                        {
                            // if the device name is not in the match list
                            if (!MainForm.member.MatchExists(BTDevices[i].DeviceAddress.ToString()))
                            {   //Send profile and request that a profile not be returned by using "0" parameter
                                SendBluetoothProfile(bTDevices[i].DeviceAddress, 0);
                            }

                        }
					}
					catch(Exception e)
					{
						//throw e;
                        // an exception will be thrown if the remote device doesnt reply in time
                        // this is quite a frequent occurance due to the nature of mobile bluetooth
					}
				}
			}
		}

        /// <summary>
        /// Threaded loop constantly listens for imcoming connections
        /// </summary>
		public void ListenLoop()
		{
            // create a new bluetooth listener object
			btListener = new InTheHand.Net.Sockets.BluetoothListener(serviceGUID);

            // start the listener
			btListener.Start();

            // on exit, run is set to false to terminate all threads
            while(run)
			{
                // Call ReceiveBTData to build a valid Match object from the incoming connection
				Match m = ReceiveBTData();

                //if the data returned is valid
				if(m!=null) 
				{

                    // if the Match has not already been registered add
                    if (!MainForm.member.MatchExists(m.MACAddress))
                    { // add the Match into the system
                        MainForm.member.AddMatch(m);
                    }

					// if they dont have my profile then send it
                    if (!m.HasMyProfile)
                    {
                        // retreive the DeviceAddress object from the MACAddress
                        // loop through all the discovered devices
                        for (int i = 0; i < bTDevices.Length; i++)
                        {//if the MAC address match return the DeviceAddress object
                            if(bTDevices[i].DeviceAddress.ToString()==m.MACAddress)
                                SendBluetoothProfile(bTDevices[i].DeviceAddress, 1);
                        }
                    }

                    // Sleep for 100ms
                    //Thread.Sleep(100);
				}
			}
		}

        /// <summary>
        /// Trys to create a Match object from an incoming bluetooth connection stream
        /// </summary>
        /// <returns>A Match object or null if the stream is invalid</returns>
		private Match ReceiveBTData()
		{
            // set bytes read to 0
			int bytesRead = 0;

            // nullify an existing BluetoothClient object
			BluetoothClient client = null;

            // create a new stream object for the incoming data
            System.IO.Stream stream = null;

            // create a byte array buffer of 128
			byte[] Buffer = new byte[128];

            // create a null Match object
            // if the incoming byte stream cannot be successfully converted into a Match
            // object then m is returned as a null
			Match m=null;

			try 
			{
                // Accept an requested connections
                client = btListener.AcceptBluetoothClient();
                
                // return the byte stream from the connection
                stream = client.GetStream();

                // place up 128 bytes of data into the buffer and read the number of bytes read
				bytesRead = stream.Read(Buffer, 0, 128);

                // convert the byte array into a string
				string rs = System.Text.Encoding.Unicode.GetString(Buffer, 0, bytesRead);

                // split the string via the comma delimter
				string[] str = rs.Split(new char[]{','});

                // pass the MemberID into the Match Constructor to create a new instance
				m = new Match(str[6]);

                // set the MAC address of the Match from the connections Address object 
                m.MACAddress = ((BluetoothEndPoint)client.Client.RemoteEndPoint).Address.ToString();

                // determine if the remote Member has my profile already
                if (str[8] == "0") 
                    m.HasMyProfile = false;
                else 
                    m.HasMyProfile = true;

				// Close the bluetooth connect as the radio is released
				stream.Close();
                client.Close();
			} 
			catch (Exception e) 
			{
				//throw e;
				
			} 
			finally 
			{
                // upon terminating the try/catch block, make sure the stream and client object
                // are other incoming connections arent stalled until the connetion times out
				if ((!(stream == null))) 
				{
					stream.Close();
				}
                if ((!(client == null))) 
				{
                    client.Close();
				}
			}

            // return the Match object
			return m;
		}
	}
}