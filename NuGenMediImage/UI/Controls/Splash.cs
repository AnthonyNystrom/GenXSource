/*
 * Created by SharpDevelop.
 * User: mjackson
 * Date: 25/10/2006
 * Time: 08:17
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.NuGenMediImage.UI.Controls
{
	/// <summary>
	/// Description of Splash.
	/// </summary>
	internal sealed class Splash  : System.Windows.Forms.Form
	{
		
#region initialisation

		public Splash() : base() {
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//	Size to the image so as to display it fully and position the form in the center screen with no border.
			this.Size = this.BackgroundImage.Size;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

			//	Force the splash to stay on top while the mainform renders but don't show it in the taskbar.
			this.TopMost = true;
			this.ShowInTaskbar = false;
			
			//	Make the backcolour Fuchia and set that to be transparent
			//	so that the image can be shown with funny shapes, round corners etc.
			this.BackColor = System.Drawing.Color.Fuchsia;
			this.TransparencyKey = System.Drawing.Color.Fuchsia;

			//	Initialise a timer to do the fade out
			if (this.components == null) {
				this.components = new System.ComponentModel.Container();
			}
			this.fadeTimer = new System.Windows.Forms.Timer(this.components);
            //	Size to the image so as to display it fully and position the form in the center screen with no border.
            this.Size = this.BackgroundImage.Size;

		}

		private System.Windows.Forms.Timer fadeTimer;

#endregion

#region Static Methods

        internal static Splash Instance = null;
        private Panel versionBox;
        private LinkLabel linkLabel1;

        public bool ShowVersionBox
        {
            get { return versionBox.Visible; }
            set { versionBox.Visible = value; }
        }
		internal static System.Threading.Thread splashThread = null;

		public static void ShowSplash(bool showVersion) {
			//	Show Splash with no fading
			ShowSplash(0,showVersion);
		}

		public static void ShowSplash(int fadeinTime,bool showVersion) {
			//	Only show if not showing already
			if (Instance == null) {
				Instance = new Splash();
                Instance.ShowVersionBox = showVersion;
				
				//	Hide initially so as to avoid a nasty pre paint flicker
				Instance.Opacity = 0;
				Instance.Show();
				
				//	Process the initial paint events
				Application.DoEvents();
				
				// Perform the fade in
				if (fadeinTime > 0) {
					//	Set the timer interval so that we fade out at the same speed.
					int fadeStep = (int)System.Math.Round((double)fadeinTime/20);
					Instance.fadeTimer.Interval = fadeStep;

					for (int i = 0; i <= fadeinTime; i += fadeStep){
						System.Threading.Thread.Sleep(fadeStep);
						Instance.Opacity += 0.05;
					}
				} else {
					//	Set the timer interval so that we fade out instantly.
					Instance.fadeTimer.Interval = 1;
				}
				Instance.Opacity = 1;
			}
		}

		public static void Fadeout() {
			//	Only fadeout if we are currently visible.
			if (Instance != null) {
				Instance.BeginInvoke(new MethodInvoker(Instance.Close));
				
				//	Process the Close Message on the Splash Thread.
				Application.DoEvents();
			}
		}

#endregion

#region Close Splash Methods

		protected override void OnClick(System.EventArgs e) {
			//	If we are displaying as a about dialog we need to provide a way out.
			this.Close();
            Instance.Dispose();
            Instance = null;
		}
		
		protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
		{
			base.OnClosing(e);
			
			//	Close immediatly is the timer interval is set to 1 indicating no fade.
			if (this.fadeTimer.Interval == 1) {
				e.Cancel = false;
				return;
			}

			//	Only use the timer to fade out if we have a mainform running otherwise there will be no message pump
			if (Application.OpenForms.Count > 1) {
				if (this.Opacity > 0) {
					e.Cancel = true;
					this.Opacity -= 0.05;
					
					//	use the timer to iteratively call the close method thereby keeping the GUI thread available for other processes.
					this.fadeTimer.Tick -= new System.EventHandler(this.FadeoutTick);
					this.fadeTimer.Tick += new System.EventHandler(this.FadeoutTick);
					this.fadeTimer.Start();
				} else {
					e.Cancel = false;
					this.fadeTimer.Stop();
					
					//	Clear the instance variable so we can reshow the splash, and ensure that we don't try to close it twice
					Instance = null;
				}
			} else {
				if (this.Opacity > 0) {
					//	Sleep on this thread to slow down the fade as there is no message pump running
					System.Threading.Thread.Sleep(this.fadeTimer.Interval);
					Instance.Opacity -= 0.05;
					
					//	iteratively call the close method
					this.Close();
				} else {
					e.Cancel = false;

					//	Clear the instance variable so we can reshow the splash, and ensure that we don't try to close it twice
					Instance = null;
				}
			}
				
		}	
		
		void FadeoutTick(object sender, System.EventArgs e){
			this.Close();
		}

#endregion

#region Designer stuff

		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Splash));
            this.versionBox = new System.Windows.Forms.Panel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.versionBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // versionBox
            // 
            this.versionBox.BackColor = System.Drawing.Color.Transparent;
            this.versionBox.Controls.Add(this.linkLabel1);
            this.versionBox.Location = new System.Drawing.Point(2, 252);
            this.versionBox.Name = "versionBox";
            this.versionBox.Size = new System.Drawing.Size(174, 33);
            this.versionBox.TabIndex = 0;
            this.versionBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // linkLabel1
            // 
            this.linkLabel1.ActiveLinkColor = System.Drawing.Color.Black;
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel1.DisabledLinkColor = System.Drawing.Color.Black;
            this.linkLabel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.LinkColor = System.Drawing.Color.Black;
            this.linkLabel1.Location = new System.Drawing.Point(4, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(166, 13);
            this.linkLabel1.TabIndex = 1;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "http://www.genetibase.com";
            this.linkLabel1.VisitedLinkColor = System.Drawing.Color.Black;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // Splash
            // 
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(400, 287);
            this.Controls.Add(this.versionBox);
            this.Name = "Splash";
            this.Text = "Splash";
            this.versionBox.ResumeLayout(false);
            this.versionBox.PerformLayout();
            this.ResumeLayout(false);

		}
#endregion

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                Graphics g = e.Graphics;
                //Font font = new System.Drawing.Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                Font font2 = new System.Drawing.Font("Tahoma", 7.25F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                //g.DrawString("http://www.genetibase.com", font, Brushes.Black, new Point(4, 4));
                g.DrawString(Application.ProductVersion, font2, Brushes.Black, new Point(4, 18));

                //font.Dispose();
                font2.Dispose();                
            }
            catch { }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(linkLabel1.Text);
            }
            catch{ }

            try
            {
                this.OnClick(null);
            }catch{}
        }

	}
}
