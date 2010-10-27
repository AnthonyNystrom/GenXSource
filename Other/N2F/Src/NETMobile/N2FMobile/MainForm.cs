using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net.Sockets;
using InTheHand.Net;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;

namespace N2FMobile
{
    public partial class MainForm : Form
    {
        private Label lblBluetooth;
        private static bool Run = true;
        private MainMenu mainMenu1;
        private MenuItem menuItem1;
        private MenuItem menuItem2;
        private PictureBox pictureBox1;
        private MenuItem menuItem3;
        private PictureBox BTON;
        private Label lblFound;
        private string OtherName = string.Empty;
        public Sound snd = new Sound(@"\chimes.wav");
        private Thread ThreadClient;
        private System.Windows.Forms.Timer timerResetLabel;
        private int EmptyCounter = 0;
        

        public MainForm()
        {
            InitializeComponent();

            try
            {
                this.pictureBox1.Image = new Bitmap(@"\n2f.bmp");
                this.BTON.Image = new Bitmap(@"\btON.bmp");
            }
            catch
            {

            }

            lblBluetooth.Text = "Bluetooth is ON - Searching...";
            BluetoothRadio br = BluetoothRadio.PrimaryRadio;
            br.Mode = RadioMode.Discoverable;

            if (br == null)
            {
                MessageBox.Show("Bluetooth radio is being used. Restart device for quick fix");
                Application.Exit();
            }

            if (br.LocalAddress.ToString() == AsynchronousClient.BTAddress.ToString())
            {
                // BOB is the server so he needs to have the server listener running
                OtherName = "Alice";
                //MessageBox.Show("I am BOB");

                // set alices address
                AsynchronousClient.BTAddress = new BluetoothAddress(new byte[] { 175, 223, 244, 227, 23, 0, 0, 0 });

                AsynchronousClient AClient = new AsynchronousClient(this);

                ThreadClient = new Thread(new ThreadStart(AClient.StartSending));
                ThreadClient.Start();

                // start the Asynchronous server
                AsynchronousServer AServer = new AsynchronousServer(this);

            }
            else
            {
                OtherName = "Bob";
                //MessageBox.Show("I am ALICE");

                AsynchronousClient AClient = new AsynchronousClient(this);

                ThreadClient = new Thread(new ThreadStart(AClient.StartSending));
                ThreadClient.Start();

                // start the Asynchronous server
                AsynchronousServer AServer = new AsynchronousServer(this);
                
            }
        }


        public void Update_txtDevices(object sender, EventArgs e)
        {
            // let the Tag be displayed for 3 seconds
            EmptyCounter = 3;
            lblFound.Text = "Tagged " + OtherName;
            snd.Play();
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            if (menuItem2.Text == "Bluetooth OFF")
            {
                     // turn off
                    lblBluetooth.Text = "Bluetooth is OFF";
                    menuItem2.Text = "Bluetooth ON";

                    this.BTON.Image = new Bitmap(@"\btOFF.bmp");
                    menuItem2.Enabled = false;

                    AsynchronousServer.BTListener.Stop();
                    AsynchronousServer.BTListener = null ;
                    AsynchronousServer.Listen = false;
                    AsynchronousClient.Sending = false;

                    //BluetoothRadio.PrimaryRadio.Mode = RadioMode.PowerOff;

                    menuItem2.Enabled = true;

            }
            else
            {
                    // turn on
                    BluetoothRadio.PrimaryRadio.Mode = RadioMode.Discoverable;
                    AsynchronousServer.BTListener = new BluetoothListener(AsynchronousServer._N2FServiceGUID);
                    AsynchronousServer.BTListener.Start();
                    AsynchronousServer.Listen = true;
                    
                    AsynchronousClient.Sending = true;
                    
                    lblBluetooth.Text = "Bluetooth is ON - Searching...";
                    menuItem2.Text = "Bluetooth OFF";
                    this.BTON.Image = new Bitmap(@"\btON.bmp");
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // shut down the label timer
            timerResetLabel.Enabled = false;

            // shut down the SERVER
            try
            {
                AsynchronousServer.TerminateAllThreads = true;
                AsynchronousServer.Listen = false;
                AsynchronousServer.BTListener.Stop();
                AsynchronousServer.BTListener = null;
                AsynchronousServer.ListenThread.Abort();
            }
            catch { }

            // shut down the CLIENT
            try
            {
                AsynchronousClient.TerminateAllThreads = true;
                AsynchronousClient.Sending = false;
                ThreadClient.Abort();
            }
            catch { }

            base.OnClosing(e);
        }

        private void InitializeComponent()
        {
            this.lblBluetooth = new System.Windows.Forms.Label();
            this.mainMenu1 = new System.Windows.Forms.MainMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.BTON = new System.Windows.Forms.PictureBox();
            this.lblFound = new System.Windows.Forms.Label();
            this.timerResetLabel = new System.Windows.Forms.Timer();
            this.SuspendLayout();
            // 
            // lblBluetooth
            // 
            this.lblBluetooth.Location = new System.Drawing.Point(10, 63);
            this.lblBluetooth.Name = "lblBluetooth";
            this.lblBluetooth.Size = new System.Drawing.Size(299, 30);
            this.lblBluetooth.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.Add(this.menuItem1);
            this.mainMenu1.MenuItems.Add(this.menuItem3);
            // 
            // menuItem1
            // 
            this.menuItem1.MenuItems.Add(this.menuItem2);
            this.menuItem1.Text = "Bluetooth";
            // 
            // menuItem2
            // 
            this.menuItem2.Text = "Bluetooth OFF";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Text = "Exit";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(8, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(301, 57);
            // 
            // BTON
            // 
            this.BTON.Location = new System.Drawing.Point(254, 6);
            this.BTON.Name = "BTON";
            this.BTON.Size = new System.Drawing.Size(51, 49);
            this.BTON.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // lblFound
            // 
            this.lblFound.Font = new System.Drawing.Font("Segoe Condensed", 20F, System.Drawing.FontStyle.Regular);
            this.lblFound.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblFound.Location = new System.Drawing.Point(10, 123);
            this.lblFound.Name = "lblFound";
            this.lblFound.Size = new System.Drawing.Size(299, 47);
            this.lblFound.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // timerResetLabel
            // 
            this.timerResetLabel.Enabled = true;
            this.timerResetLabel.Interval = 1000;
            this.timerResetLabel.Tick += new System.EventHandler(this.timerResetLabel_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(320, 186);
            this.Controls.Add(this.lblFound);
            this.Controls.Add(this.BTON);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblBluetooth);
            this.KeyPreview = true;
            this.Menu = this.mainMenu1;
            this.Name = "MainForm";
            this.ResumeLayout(false);

        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void timerResetLabel_Tick(object sender, EventArgs e)
        {
            if (--EmptyCounter == 0)
            {
                lblFound.Text = string.Empty;
            }
        }  
    }
}