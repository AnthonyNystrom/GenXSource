using System;
using System.Threading;
using System.Drawing;
using System.Xml;
using System.Text;
using System.IO;
using System.Net;
using System.Collections;
using System.Security.Cryptography;

using InTheHand.Net;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;

namespace goAbout.Engine
{
    /// <summary>
    /// Enumeration represents both Genders
    /// </summary>
	public enum eGender{Female, Male};

    /// <summary>
    /// Enumeration represents the Loadstate of the XML storage file
    /// </summary>
	public enum eLoadState{NotTried,NoXMLFile, NoLogin, Loaded};

	public class Member
	{
		#region field variables

		private string memberID="";
		private string password="";
		private string email;
		private string introduction;
		private string image;
        private Image imageFile;
		private static int age=18;
		private static eGender gender;
		private static int seekAge;
		private static eGender seekGender;
		private int option1;
		private int option3;
		private int option2;
		private int option4;
		private int syncVersion;
		private DateTime syncDT;
		private DateTime modifiedDT;
		private DateTime createdDT;
		private bool authorised = false;
		private eLoadState loadState = eLoadState.NotTried;
		private ArrayList matches = new ArrayList();
		private BluetoothCom bluetoothCom;
        private string serverAddress;
        private string defaultServerAddress = "http://127.0.0.1/goabout/sync.aspx";

        public Thread listenThread;
        public Thread discoverThead;

        public static string EncryptionKey = "DSDSA4546fes8fdsf";

        /// <summary>
        /// The MemberID of the Member
        /// </summary>
		public string MemberID
		{
			get{return memberID;}
			set{memberID = value;}

		}

        /// <summary>
        /// The server authentication Password for the Member
        /// </summary>
		public string Password
		{
			get{return password;}
			set{this.password = value;}

		}

        /// <summary>
        /// The Email address of the Member
        /// </summary>
        public string Email
        {
            get { return email; }
            set { this.email = value; }

        }

        /// <summary>
        /// The text introduction of the Member
        /// </summary>
        public string Introduction
        {
            get { return introduction; }
            set { this.introduction = value; }
        }

        /// <summary>
        /// The string name of the image
        /// </summary>
        public string Image
        {
            get { return image; }
            set { this.image = value; }

        }

        /// <summary>
        /// The Image file for this Member
        /// </summary>
        public Image ImageFile
        {
            get { return imageFile; }
            set { this.imageFile = value; }

        }

        /// <summary>
        /// The age of this Member
        /// </summary>
        public static int Age
        {
            get { return age; }
            set { age = value; }
        }

        /// <summary>
        /// The Gender of the Member
        /// </summary>
        public static eGender Gender
        {
            get { return gender; }
            set { gender = value; }
        }


        /// <summary>
        /// The Filter Age setting for Matches
        /// </summary>
        public static int SeekAge
        {
            get { return seekAge; }
            set { seekAge = value; }
        }

        /// <summary>
        /// The Filter Gender setting for Matches
        /// </summary>
        public static eGender SeekGender
        {
            get { return seekGender; }
            set { seekGender = value; }
        }


        /// <summary>
        /// Option 1 setting
        /// </summary>
        public int Option1
        {
            get { return option1; }
            set { this.option1 = value; }
        }

        /// <summary>
        /// Option 2 setting
        /// </summary>
        public int Option2
        {
            get { return option2; }
            set { this.option2 = value; }
        }

        /// <summary>
        /// Option 3 setting
        /// </summary>
        public int Option3
        {
            get { return option3; }
            set { this.option3 = value; }
        }

        /// <summary>
        /// Option 4 setting
        /// </summary>
        public int Option4
        {
            get { return option4; }
            set { this.option4 = value; }
        }

        /// <summary>
        /// The current synchronisation status version of the device 
        /// </summary>
        public int SyncVersion
        {
            get { return syncVersion; }
            set { this.syncVersion = value; }

        }

        /// <summary>
        /// The BluetoothCom object for the Member
        /// </summary>
        public BluetoothCom BluetoothCom
        {
            get { return bluetoothCom; }
            set { this.bluetoothCom = value; }

        }
       


        /// <summary>
        /// The of Date & Time the last device synchonisation
        /// </summary>
        public DateTime SyncDT
        {
            get { return syncDT; }
            set { this.syncDT = value; }
        }

        /// <summary>
        /// The Date & Time the user last logged onto the system via the web
        /// </summary>
        public DateTime ModifiedDT
        {
            get { return modifiedDT; }
            set { this.modifiedDT = value; }
        }

        /// <summary>
        /// The Date & Time that the Member joined
        /// </summary>
        public DateTime CreatedDT
        {
            get { return createdDT; }
            set { this.createdDT = value; }
        }

        /// <summary>
        /// Has the member been authorised from the main server
        /// </summary>
		public bool Authorised
		{
			get{return authorised;}
		}

        /// <summary>
        /// An array list of all current Matches
        /// </summary>
		public ArrayList Matches
		{
			get{return matches;}
		}

		#endregion

        /// <summary>
        /// serialise the Members profile into a byte array for bluetooth transmission
        /// </summary>
        /// <returns>The byte array of the Members profile</returns>
		public byte[] SerializeProfileforBT()
		{
			string p =  age.ToString()+",";

            // replace female for 0 and 1 for Male
			if(gender==eGender.Female)	p += 0+",";
			else p += 1+",";

            //store the seek age
			p += seekAge.ToString()+",";

            // replace female for 0 and 1 for Male
			if(SeekGender==eGender.Female)p += 0+",";
			else p += 1+",";

            // Convert the string to a byte array with UTF8 encoding
			return Encoding.UTF8.GetBytes(p);
		}

        /// <summary>
        /// Member constructor
        /// </summary>
		public Member()
		{
			// blank
		}

        /// <summary>
        /// Instanciates a new bluetoothCom for the device
        /// </summary>
        public void StartBluetoothService()
        {
            bluetoothCom = new BluetoothCom();

            // start the listener in a threaded loop
            listenThread = new Thread(new ThreadStart(BluetoothCom.ListenLoop));
            listenThread.Start();

            // start the discovery service in a threaded loop
            discoverThead = new Thread(new ThreadStart(BluetoothCom.DiscoverLoop));
            discoverThead.Start();
        }

        /// <summary>
        /// Adds a new Bluetooth Match into the system
        /// </summary>
        /// <param name="m"></param>
		public void AddMatch(Match m)
		{
            // add the match to the arraylist
			this.matches.Add(m);

            // update the display
            DisplayMatches();

            // save the Match to the XML storage file
            SaveToXML();
		}

        /// <summary>
        /// This method iterates through the array member variable matches and check 
        /// if username of the match that just occurred has already been matches. 
        /// </summary>
        /// <param name="macaddress">Bluetooth address of the potential match</param>
        /// <returns>if a match already exists</returns>
		public bool MatchExists(string macaddress)
		{
			Match t;

            // loop through Matches
			for(int i=0;i<matches.Count;i++)
			{
				t=(Match)matches[i];
                // if the MAC address of the Match is the same as the method parameter 
                if (t.MACAddress == macaddress)
					return true; // the match already exists so return true;
			}

            // A matching MAC address was not found so the device is new
			return false;
		}

        /// <summary>
        /// Attempts to load the goAbout config file.
        /// </summary>
        /// <returns>The load state of the device</returns>
        public eLoadState LoadFromXML()
        {
            this.matches.Clear();

            if (!File.Exists(@"goAbout.xml"))
            {
                this.serverAddress = defaultServerAddress;
                loadState = eLoadState.NoLogin;
            }
            else
            {
                XmlTextReader r = new XmlTextReader(@"goAbout.xml");

                while (r.Read())
                {
                    if (r.NodeType == XmlNodeType.Element)
                    {
                        if (r.LocalName.Equals("MemberID"))
                        {
                            string str = decr(r.ReadString());
                            memberID = str;
                        }

                        if (r.LocalName.Equals("Password"))
                        {
                            string str = decr(r.ReadString());
                            password = str;
                        }

                        if (r.LocalName.Equals("MemberID"))
                        {
                            string str = decr(r.ReadString());
                            serverAddress = str;

                            if (str == "")
                                serverAddress = defaultServerAddress;
                        }

                        if (password != "" && memberID!="")
                            loadState = eLoadState.Loaded;

                        if (r.LocalName == "Options")
                        {
                            while (r.Read())
                            {
                                if (r.NodeType == XmlNodeType.Element)
                                {
                                    if (r.LocalName == "Option1")
                                        this.option1 = Int32.Parse(r.ReadString());

                                    if (r.LocalName == "Option2")
                                        this.option2 = Int32.Parse(r.ReadString());

                                    if (r.LocalName == "Option3")
                                        this.option3 = Int32.Parse(r.ReadString());

                                    if (r.LocalName == "Option4")
                                    {
                                        this.option4 = Int32.Parse(r.ReadString());
                                        break;
                                    }
                                }
                            }
                        }

                        if (r.LocalName == "Profile")
                        {
                            while (r.Read())
                            {
                                if (r.NodeType == XmlNodeType.Element)
                                {
                                    //if (r.LocalName == "Age")
                                    //    Member.Age = Int32.Parse(r.ReadString());

                                    ////if (r.LocalName == "Gender")
                                    // //   Member.Gender = (eGender)r.ReadString();

                                    //if (r.LocalName == "AgeSeek")
                                    //    Member.SeekAge = Int32.Parse(r.ReadString());

                                    //if (r.LocalName == "GenderSeek")
                                    //{
                                    //    Member.SeekGender = (eGender)r.ReadString();
                                    //    break;
                                    //}
                                }
                            }
                        }

                        if (r.LocalName == "Matches")
                        {
                            while (r.Read())
                            {
                                if (r.NodeType == XmlNodeType.Element)
                                {
                                    if (r.LocalName == "Match")
                                    {
                                        Match m = null;
                                        while (r.Read())
                                        {
                                            if (r.LocalName == "MatchName")
                                                m = new Match(r.ReadString());

                                            if (r.LocalName == "MAC")
                                                m.MACAddress = r.ReadString();

                                            if (r.LocalName == "MatchTime")
                                                m.MatchDT = Convert.ToDateTime(r.ReadString());

                                            if (r.LocalName == "ConfirmedByServer")
                                            {
                                                m.ConfirmedByServer = Convert.ToBoolean(r.ReadString());
                                                matches.Add(m);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                r.Close();
                DisplayMatches();
            }

            if (this.loadState == eLoadState.NoLogin)
            {
                MainForm.pnlLogin.Visible = true;
                MainForm.LeftMenu.Text = "Done";
                MainForm.pnlLogin.Controls[2].Focus();
            }

            return loadState;
        }

        /// <summary>
        /// Updates the GUI and displays and events such as new match and server sycnhonisation
        /// </summary>
        public void DisplayMatches()
        {
            int count = 0;

            // count all ConfirmedByServer matches (new matches)
            for (int i = 0; i < matches.Count; i++)
                if (!((Match)matches[i]).ConfirmedByServer) count++;

            //MainForm.lblMatches.Text = "You have " + count + " Matches";
        }

        /// <summary>
        /// Saves the current state and matches of the Member to the XML storage file
        /// </summary>
        public void SaveToXML()
        {
            // Create a new XmlTextWriter instance
            XmlTextWriter writer = new XmlTextWriter(@"goAbout.xml", Encoding.UTF8);

            // start writing
            writer.WriteStartDocument();

            // create a Member element
            writer.WriteStartElement("Member");

            // Encrypt the Screen Name & Password and write them as elements values
            writer.WriteElementString("MemberID", enc(memberID));
            writer.WriteElementString("Password", enc(this.Password));
            writer.WriteElementString("ServerAddress", enc(this.serverAddress));

            // create a new element for the options
            writer.WriteStartElement("Options");

            // store the options
            writer.WriteElementString("Option1", this.option1.ToString());
            writer.WriteElementString("Option2", this.option2.ToString());
            writer.WriteElementString("Option3", this.option3.ToString());
            writer.WriteElementString("Option4", this.option4.ToString());

            // close the options element
            writer.WriteEndElement();

            // create a new element for the Profile
            writer.WriteStartElement("Profile");

            // All filter options are encrypted
            writer.WriteElementString("Age", enc(Age.ToString()));
            writer.WriteElementString("Gender", enc(Gender.ToString()));
            writer.WriteElementString("AgeSeek", enc(SeekAge.ToString()));
            writer.WriteElementString("GenderSeek", enc(SeekGender.ToString()));

            // close the profile element
            writer.WriteEndElement();

            // This is where photo storage would be implemented
            //			if(this.image!=null)
            //			{
            //				Byte[] b = this.ImageFile.
            //				writer.WriteStartElement("Photo");
            //
            //					writer.WriteElementString("Name","photo.jpg");  
            //					writer.WriteElementString("Data","");  
            //
            //				writer.WriteEndElement();     
            //			}

            // create a Matches element
            writer.WriteStartElement("Matches");

            // loop though all the Matches
            for (int i = 0; i < Matches.Count; i++)
            {
                // Create a new Match element
                writer.WriteStartElement("Match");

                // Encrypt all Match details
                writer.WriteElementString("MatchName", enc(((Match)Matches[i]).memberID));
                writer.WriteElementString("MAC", enc(((Match)Matches[i]).MACAddress));
                writer.WriteElementString("MatchDT", enc(DateTime.Now.ToLongTimeString()));
                writer.WriteElementString("ConfirmedByServer", enc(((Match)Matches[i]).ConfirmedByServer.ToString()));
                writer.WriteEndElement();
            }

            // close the Matches element
            writer.WriteEndElement();

            // Close the Member element
            writer.WriteEndDocument();

            // Close the writer object
            writer.Close();
        }

        /// <summary>
        /// Encrypts a string
        /// </summary>
        /// <param name="str">The string to be encrypted</param>
        /// <returns>The encrypted string</returns>
        private string enc(string str)
        {
            //ASCIIEncoding e = new ASCIIEncoding();
           // byte[] b = Crypto.Encrypt(Member.EncryptionKey, e.GetBytes(str));
            //return e.GetString(b, 0, b.Length);

            return str;
        }

        /// <summary>
        /// Decrypts a string
        /// </summary>
        /// <param name="str">The string to be decrypted</param>
        /// <returns>The decrypted string</returns>
        private string decr(string str)
        {
            //ASCIIEncoding e = new ASCIIEncoding();
            //byte[] b = Crypto.Decrypt(Member.EncryptionKey, e.GetBytes(str));
            //return e.GetString(b, 0, b.Length);

            return str;
        }

        /// <summary>
        /// Syncrhonises the device with the central server via HTTP
        /// An internet connection can be obtained from a number of sources including
        /// WIFI, bluetooth and cellular.
        /// The connection must use SSL for a secure connection of sensative information could be intercepted
        /// </summary>
		public void SynchronizeFromDevice()
		{
            // serialise the current settings
			string p= "";

            // The username and password for server authentication.
            p += this.memberID + ",";
            p += this.Password + ",";

            // The options for the device
			p += this.option1.ToString()+",";
			p += this.option2.ToString()+",";
			p += this.option3.ToString()+",";
			p += this.option4.ToString()+",";

            // the age of the Member
			p += age.ToString()+",";

            // gender: store 0 for female and 1 for male
			if(gender==eGender.Female)	
                p += 0+",";
			else							
                p += 1+",";

            // the seek age of the Member
			p += seekAge.ToString()+",";

            // seek gender: store 0 for female and 1 for male
			if(SeekGender==eGender.Female)	
                p += 0+",";
			else								
                p += 1+",";

            // loop though all the matches
            for (int i = 0; i < Matches.Count; i++)
            {
                // only Matches not confirmed by the central server
                if(!((Match)Matches[i]).ConfirmedByServer)
                {
                    // get the MemberID of the Match
                    p += ((Match)Matches[i]).memberID+",";

                    // get the MAC address of the Match
                    p += ((Match)Matches[i]).MACAddress.ToString();

                    // if this is the last loop then dont add another comma delimter
                    if (i != (Matches.Count - 1))
                        p += ",";

                }
            }
            
			WebRequest webRequest = WebRequest.Create(serverAddress);

            // set the WebRequest HTTP method to POST
            webRequest.Method = "POST";
		
            //Convert the profile into a byte array
			byte[] byteArray = Encoding.UTF8.GetBytes(p);

            // Set http MIME type
            webRequest.ContentType = "application/x-www-form-urlencoded";

            // Set the WebRequest content length to the profile array length 
            webRequest.ContentLength = byteArray.Length;

            // create the return stream object
            Stream stream;

            try
            {
                // get the WebRequest stream to send the byte array
                stream = webRequest.GetRequestStream();

                // write the array to the stream
                stream.Write(byteArray, 0, byteArray.Length);

                // close the stream
                stream.Close();

                // Create a response object
                WebResponse s = webRequest.GetResponse();

                // return the response into the stream object
                stream = s.GetResponseStream();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Server returned 500");
                return;
            }

            // create a new stream reader to read the returned stream
			StreamReader reader = new StreamReader(stream);

            // read the stream and split it into a string array via the comma delimiter
			string[] l = reader.ReadToEnd().Split(new char[]{','});
					
			// Phone settings
			this.option1 = Int32.Parse(l[0]);
			this.option2 = Int32.Parse(l[1]);
			this.option3 = Int32.Parse(l[2]);
			this.option4 = Int32.Parse(l[3]);

            // Filter settings
			age = Int32.Parse(l[4]);
			gender = (eGender)Int32.Parse(l[5]);
			seekAge = Int32.Parse(l[6]);
			SeekGender = (eGender)Int32.Parse(l[7]);

            // update all the matches objects to confirmed by server
            for (int i = 0; i < Matches.Count; i++)
                ((Match)Matches[i]).ConfirmedByServer = true;           
		}
	}	
}