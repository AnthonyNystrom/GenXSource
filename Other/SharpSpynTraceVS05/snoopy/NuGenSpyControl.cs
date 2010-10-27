using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Genetibase.Debug
{
	/// <summary>
	/// Summary description for NuGenSpyControl.
	/// </summary>
	public class NuGenSpyControl : System.Windows.Forms.UserControl
	{
		private Genetibase.Debug.NuGenWindowFinder nuGenWindowFinder1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.NumericUpDown numericUpDown1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private Genetibase.Debug.NuGenWindowFinder nuGenWindowFinder2;
		private System.Windows.Forms.Button button2;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public NuGenSpyControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitializeComponent call

		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(NuGenSpyControl));
			this.nuGenWindowFinder1 = new Genetibase.Debug.NuGenWindowFinder();
			this.panel1 = new System.Windows.Forms.Panel();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.nuGenWindowFinder2 = new Genetibase.Debug.NuGenWindowFinder();
			this.button2 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
			this.SuspendLayout();
			// 
			// nuGenWindowFinder1
			// 
			this.nuGenWindowFinder1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("nuGenWindowFinder1.BackgroundImage")));
			this.nuGenWindowFinder1.Location = new System.Drawing.Point(8, 8);
			this.nuGenWindowFinder1.Name = "nuGenWindowFinder1";
			this.nuGenWindowFinder1.Size = new System.Drawing.Size(32, 32);
			this.nuGenWindowFinder1.TabIndex = 0;
			this.nuGenWindowFinder1.ActiveWindowChanged += new System.EventHandler(this.windowFinder1_ActiveWindowChanged);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Location = new System.Drawing.Point(0, 48);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(824, 576);
			this.panel1.TabIndex = 1;
			this.panel1.Resize += new System.EventHandler(this.panel1_Resize);
			// 
			// checkBox1
			// 
			this.checkBox1.Location = new System.Drawing.Point(56, 16);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(120, 24);
			this.checkBox1.TabIndex = 2;
			this.checkBox1.Text = "Recursive forms";
			// 
			// numericUpDown1
			// 
			this.numericUpDown1.Location = new System.Drawing.Point(320, 16);
			this.numericUpDown1.Name = "numericUpDown1";
			this.numericUpDown1.Size = new System.Drawing.Size(48, 22);
			this.numericUpDown1.TabIndex = 3;
			this.numericUpDown1.Value = new System.Decimal(new int[] {
																		 10,
																		 0,
																		 0,
																		 0});
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(192, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(120, 23);
			this.label1.TabIndex = 4;
			this.label1.Text = "Recursion Depth";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(384, 16);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(128, 23);
			this.button1.TabIndex = 5;
			this.button1.Text = "Close Spy Window";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// nuGenWindowFinder2
			// 
			this.nuGenWindowFinder2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("nuGenWindowFinder2.BackgroundImage")));
			this.nuGenWindowFinder2.Location = new System.Drawing.Point(520, 8);
			this.nuGenWindowFinder2.Name = "nuGenWindowFinder2";
			this.nuGenWindowFinder2.Size = new System.Drawing.Size(32, 32);
			this.nuGenWindowFinder2.TabIndex = 0;
			this.nuGenWindowFinder2.ActiveWindowChanged += new System.EventHandler(this.windowFinder1_ActiveWindowChanged);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(568, 16);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(128, 23);
			this.button2.TabIndex = 5;
			this.button2.Text = "Close Spy Window";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// NuGenSpyControl
			// 
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.numericUpDown1);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.nuGenWindowFinder1);
			this.Controls.Add(this.nuGenWindowFinder2);
			this.Controls.Add(this.button2);
			this.Name = "NuGenSpyControl";
			this.Size = new System.Drawing.Size(824, 624);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void windowFinder1_ActiveWindowChanged(object sender, System.EventArgs e)
		{
			NuGenWindowFinder windowFinder1 = (NuGenWindowFinder) sender;
			if (windowFinder1.Window.SameProcess)
			{
				closeSpyData();

				Genetibase.Debug.NuGenSnoopControl snoop = new Genetibase.Debug.NuGenSnoopControl();
				snoop.Dock = DockStyle.Fill;
				snoop.RecurseIntoSubControls = checkBox1.Checked;
				snoop.RecursionDepth = (int) numericUpDown1.Value;
				snoop.SelectedObject = Control.FromHandle(windowFinder1.Window.HWND);
				panel1.Controls.Add(snoop);					
			}
			else if (windowFinder1.Window.IsManaged)
			{
				closeSpyData();
				send_spy(windowFinder1.Window.HWND);
			}
		}

		public void closeSpyData()
		{
			IntPtr hwnd = Genetibase.Debug.UnmanagedMethods.GetWindow(panel1.Handle, (int)Genetibase.Debug.GetWindowCmd.GW_CHILD);
			Genetibase.Debug.UnmanagedMethods.SendMessage(hwnd, 0x10, (IntPtr) 0, (IntPtr) 0);
		}

		private void send_spy(IntPtr targetHWND)
		{
			try
			{
				//				this.Text = String.Format(this.Text, targetWindowHandle.ToInt32());
				int processId;
				int threadId = Genetibase.Debug.UnmanagedMethods.GetWindowThreadProcessId(targetHWND, out processId);

				Type type = typeof(Genetibase.Debug.NuGenSnoopCarrier);
				
				int panelHandle = panel1.Handle.ToInt32();
				int targetHandle = targetHWND.ToInt32();

				byte[] b1 = BitConverter.GetBytes(panelHandle);
				byte[] b2 = BitConverter.GetBytes(targetHandle);
				byte[] b3 = BitConverter.GetBytes((int) (checkBox1.Checked ? 1 : 0));
				byte[] b4 = BitConverter.GetBytes((int) numericUpDown1.Value);

				byte[] data = new byte[b1.Length + b2.Length + b3.Length + b4.Length];
				Array.Copy(b1,data, b1.Length);
				Array.Copy(b2, 0, data, b1.Length, b2.Length);
				Array.Copy(b3, 0, data, b1.Length + b2.Length, b3.Length);
				Array.Copy(b4, 0, data, b1.Length + b2.Length + b3.Length, b4.Length);

				Genetibase.Debug.NuGenHookInstaller.InjectAssembly(processId, threadId, type.Assembly.Location, type.FullName, data);
				Genetibase.Debug.UnmanagedMethods.SendMessage(targetHWND, 0, IntPtr.Zero, IntPtr.Zero);			
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message, "sharp-eye could not send a probe");
			}
		}

		private void panel1_Resize(object sender, System.EventArgs e)
		{
			IntPtr hwnd = Genetibase.Debug.UnmanagedMethods.GetWindow(panel1.Handle, (int)Genetibase.Debug.GetWindowCmd.GW_CHILD);
			Genetibase.Debug.UnmanagedMethods.MoveWindow(hwnd, 0, 0, panel1.Width, panel1.Height, true);
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			closeSpyData();
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			Trace.WriteLine ("Tracing...");
		}
	}
}
