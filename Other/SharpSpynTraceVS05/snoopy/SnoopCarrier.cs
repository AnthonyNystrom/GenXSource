using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Genetibase.Debug
{
	/// <summary>
	/// SnoopCarrier sole purpose is connect SnoopControl to its caller (which resides in another process)
	/// </summary>
	public class NuGenSnoopCarrier : System.Windows.Forms.UserControl, Genetibase.Debug.IHookInstall 		
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private NuGenSnoopControl snoop_;
		private System.Windows.Forms.Timer timer1;
		private IntPtr parentWindow;
		private IntPtr spyWindow;
		private bool is_recurse = false;
		private int recurse_depth = 10; 

		public NuGenSnoopCarrier()
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
			this.components = new System.ComponentModel.Container();
			this.snoop_     = new NuGenSnoopControl (); 
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// snoop_
			// 
			this.snoop_.Dock = System.Windows.Forms.DockStyle.Fill;			
			// 
			// timer1
			// 
			this.timer1.Interval = 10;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// WindowPropertiesView
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.snoop_});
			this.Name = "SnoopCarrier";
			this.Size = new System.Drawing.Size(280, 384);
			this.ResumeLayout(false);

		}
		#endregion

		protected override CreateParams CreateParams
		{
			get
			{
				System.Windows.Forms.CreateParams cp = base.CreateParams;
				cp.Parent = parentWindow;
				RECT rc = new RECT();
				UnmanagedMethods.GetClientRect(parentWindow, ref rc);
				
				cp.X = rc.left;
				cp.Y = rc.top;
				cp.Width = rc.right - rc.left;
				cp.Height = rc.bottom - rc.top;
		
				return cp;
			}
		}
	

		/// <summary>
		/// We are in - let's do what we gotta do
		/// </summary>
		/// <param name="data"></param>
		public void OnInstallHook(byte[] data)
		{
			parentWindow = (IntPtr)BitConverter.ToInt32(data, 0);
			spyWindow = (IntPtr)BitConverter.ToInt32(data, 4);
			is_recurse = BitConverter.ToInt32(data, 8) > 0 ? true : false;
			recurse_depth = BitConverter.ToInt32(data, 12);

			timer1.Start();
		}	

		private void timer1_Tick(object sender, System.EventArgs e)
		{
			try
			{
				timer1.Stop();				

				CreateControl();
				
				Control ctl = Control.FromHandle(spyWindow);
				
				if (ctl != null)
				{
					snoop_.RecurseIntoSubControls = is_recurse;
					snoop_.RecursionDepth = recurse_depth;
					snoop_.SelectedObject = ctl; 
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.StackTrace, "SnoopCarrier could not attach to control!");
			}
		}
	}

	public struct RECT
	{
		public int left;
		public int top;
		public int right;
		public int bottom;
	}

	public enum GetWindowCmd : int
	{
		GW_HWNDFIRST = 0,
		GW_HWNDLAST,
		GW_HWNDNEXT,
		GW_HWNDPREV,
		GW_OWNER,
		GW_CHILD    
	}

	public class UnmanagedMethods
	{
		[DllImport("user32.dll")]
		public static extern IntPtr GetClientRect(IntPtr hwnd, ref RECT rc);

		[DllImport("user32.dll")]
		public static extern IntPtr GetWindow(IntPtr hwnd, int cmd);

		[DllImport("user32.dll")]
		public static extern IntPtr SendMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

		[DllImport("user32.dll")]
		public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int processID);

		[DllImport("user32.dll")]
		public static extern bool MoveWindow(IntPtr hwnd, int x, int y, int width, int height, bool repaint);
	}
}
