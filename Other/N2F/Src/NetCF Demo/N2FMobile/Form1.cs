#region Using directives

using System;
using System.Threading;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Data;

#endregion

using goAbout.Engine;

namespace goAbout
{
    /// <summary>
    /// Summary description for form.
    /// </summary>
    public partial class MainForm : System.Windows.Forms.Form
    {
        #region Properties

        private MainMenu mainMenu;
        public static MenuItem LeftMenu;
        private MenuItem miOptions;
        private MenuItem miSlientMode;
        private MenuItem miVibrateOnly;
        private MenuItem miVibrateRing;
        private MenuItem miSaveExit;

        private Label lblNickName;
        private Label lblSeekGender;
        private Label label1;
        private Label label2;
        public static Label lblMatches;

        public static Panel pnlLogin;

        private TextBox txtMemberID;
        private TextBox txtPassword;

        public static Member member;

        #endregion

        public MainForm()
        {
            InitializeComponent();

            member = new Member();

            eLoadState loadstate = member.LoadFromXML();

            this.lblNickName.Text = member.MemberID;

            if (member.Option4 == 0)
                miSlientMode.Checked = true;
            else if (member.Option4 == 1)
                miVibrateOnly.Checked = true;
            else if (member.Option4 == 2)
                miVibrateRing.Checked = true;

            // if the setting have been loaded then start the bluetooth service
            if (loadstate == eLoadState.Loaded)
            {
                // Start the bluetooth service
                member.StartBluetoothService();
            }
        }

        /// <summary>
        ///  When the application exits this method is called and the variable  BluetoothCom.run is 
        /// set to false. This allows the threads within the class to terminate
        /// </summary>
        private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            BluetoothCom.run = false;
        }

        /// <summary>
        /// The divice is set to Silent mode
        /// </summary>
        private void miSlientMode_Click(object sender, EventArgs e)
        {
            // set the check boxes in the menu to display the current Alert mode setting
            miSlientMode.Checked = true;
            miVibrateOnly.Checked = false;
            miVibrateRing.Checked = false;

            // update the option in the member object 
            member.Option4 = 0;

            // save the new option to the XML storage file
            member.SaveToXML();
        }

        /// <summary>
        /// The divice is set to Vibrate Only
        /// </summary>
        private void miVibrateOnly_Click(object sender, EventArgs e)
        {
            // set the check boxes in the menu to display the current Alert mode setting
            miSlientMode.Checked = false;
            miVibrateOnly.Checked = true;
            miVibrateRing.Checked = false;

            // update the option in the member object 
            member.Option4 = 1;

            // save the new option to the XML storage file
            member.SaveToXML();
        }

        /// <summary>
        /// The divice is set to Vibrate 7 Ring
        /// </summary>
        private void miVibrateRing_Click(object sender, EventArgs e)
        {
            // set the check boxes in the menu to display the current Alert mode setting
            miSlientMode.Checked = false;
            miVibrateOnly.Checked = false;
            miVibrateRing.Checked = true;

            // update the option in the member object 
            member.Option4 = 2;

            // save the new option to the XML storage file
            member.SaveToXML();
        }

        /// <summary>
        /// Store the current state of the device to the XML storage file
        /// </summary>
        private void miSaveExit_Click(object sender, EventArgs e)
        {
            member.SaveToXML();
            Application.Exit();
        }

        /// <summary>
        /// A device synchonisation to the central server has been requested  
        /// or A new member has entered their MemberID and password
        /// </summary>
        private void LeftMenu_Click(object sender, EventArgs e)
        {
            // if the device is labelled "Synchronise" then synchronise the device
            if (LeftMenu.Text == "Synchronise")
            {
                // Synchronise the device
                member.SynchronizeFromDevice();

                // update the display status
                lblMatches.Text = "Device Synchonised";

                // reflect the server settings returned from the central server
                if (member.Option4 == 0)
                {
                    miSlientMode.Checked = true;
                    miVibrateOnly.Checked = false;
                    miVibrateRing.Checked = false;
                }
                else if (member.Option4 == 1)
                {
                    miSlientMode.Checked = false;
                    miVibrateOnly.Checked = true;
                    miVibrateRing.Checked = false;
                }
                else if (member.Option4 == 2)
                {
                    miSlientMode.Checked = false;
                    miVibrateOnly.Checked = false;
                    miVibrateRing.Checked = true;
                }
            }
            else
            {
                // get the MemberID from the user input box
                member.MemberID = this.txtMemberID.Text;

                // get the password from the user input box
                member.Password = this.txtPassword.Text;

                // Set the label of the LeftMenu to display "Synchronise"
                LeftMenu.Text = "Synchronise";

                // hide the login panel
                pnlLogin.Visible = false;

                // now, start the bluetooth service
                member.StartBluetoothService();
            }

            // save the member object back to the XML storage file
            member.SaveToXML();
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblSeekGender = new System.Windows.Forms.Label();
            lblMatches = new System.Windows.Forms.Label();
            this.mainMenu = new System.Windows.Forms.MainMenu();
            LeftMenu = new System.Windows.Forms.MenuItem();
            this.miOptions = new System.Windows.Forms.MenuItem();
            this.miSlientMode = new System.Windows.Forms.MenuItem();
            this.miVibrateOnly = new System.Windows.Forms.MenuItem();
            this.miVibrateRing = new System.Windows.Forms.MenuItem();
            this.miSaveExit = new System.Windows.Forms.MenuItem();
            this.lblNickName = new System.Windows.Forms.Label();
            pnlLogin = new System.Windows.Forms.Panel();
            this.txtMemberID = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            // 
            // lblSeekGender
            // 
            this.lblSeekGender.Location = new System.Drawing.Point(-4, 206);
            this.lblSeekGender.Size = new System.Drawing.Size(184, 16);
            // 
            // lblMatches
            // 
            lblMatches.Location = new System.Drawing.Point(3, 27);
            lblMatches.Size = new System.Drawing.Size(152, 22);
            lblMatches.Text = "You have 0 new matches!";
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.Add(LeftMenu);
            this.mainMenu.MenuItems.Add(this.miOptions);
            // 
            // LeftMenu
            // 
            LeftMenu.Text = "Synchronise";
            LeftMenu.Click += new System.EventHandler(this.LeftMenu_Click);
            // 
            // miOptions
            // 
            this.miOptions.MenuItems.Add(this.miSlientMode);
            this.miOptions.MenuItems.Add(this.miVibrateOnly);
            this.miOptions.MenuItems.Add(this.miVibrateRing);
            this.miOptions.MenuItems.Add(this.miSaveExit);
            this.miOptions.Text = "Control";
            // 
            // miSlientMode
            // 
            this.miSlientMode.Text = "Silent Mode";
            this.miSlientMode.Click += new System.EventHandler(this.miSlientMode_Click);
            // 
            // miVibrateOnly
            // 
            this.miVibrateOnly.Text = "Vibrate only";
            this.miVibrateOnly.Click += new System.EventHandler(this.miVibrateOnly_Click);
            // 
            // miVibrateRing
            // 
            this.miVibrateRing.Text = "Vibrate and Ring";
            this.miVibrateRing.Click += new System.EventHandler(this.miVibrateRing_Click);
            // 
            // menuItem6
            // 
            this.miSaveExit.Text = "Shutdown and Exit";
            this.miSaveExit.Click += new System.EventHandler(this.miSaveExit_Click);
            // 
            // lblNickName
            // 
            this.lblNickName.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lblNickName.Location = new System.Drawing.Point(3, 5);
            this.lblNickName.Size = new System.Drawing.Size(152, 22);
            // 
            // pnlLogin
            // 
            pnlLogin.Controls.Add(this.label2);
            pnlLogin.Controls.Add(this.label1);
            pnlLogin.Controls.Add(this.txtPassword);
            pnlLogin.Controls.Add(this.txtMemberID);
            pnlLogin.Location = new System.Drawing.Point(3, 52);
            pnlLogin.Size = new System.Drawing.Size(167, 127);
            pnlLogin.Visible = false;
            // 
            // txtMemberID
            // 
            this.txtMemberID.Location = new System.Drawing.Point(21, 25);
            this.txtMemberID.Size = new System.Drawing.Size(123, 24);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(21, 71);
            this.txtPassword.Size = new System.Drawing.Size(123, 24);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Nina", 9F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(21, 6);
            this.label1.Size = new System.Drawing.Size(88, 19);
            this.label1.Text = "Member ID";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Nina", 9F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(21, 54);
            this.label2.Size = new System.Drawing.Size(88, 14);
            this.label2.Text = "Password";
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(176, 180);
            this.Controls.Add(lblMatches);
            this.Controls.Add(this.lblSeekGender);
            this.Controls.Add(this.lblNickName);
            this.Controls.Add(pnlLogin);
            this.Menu = this.mainMenu;
            this.Closing += new System.ComponentModel.CancelEventHandler(MainForm_Closing);

        }

        #endregion
    }
}

