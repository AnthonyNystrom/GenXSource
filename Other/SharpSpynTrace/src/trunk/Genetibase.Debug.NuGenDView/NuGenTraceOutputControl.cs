using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Genetibase.Debug
{
	public class NuGenTraceOutputControl: UserControl
	{
		private delegate void UpdateUIHandler (string message);

		private Container components = null;

		private NuGenTraceListener _debugListener;
		private NuGenTViewCtrl NuGenTViewCtrl;

		public NuGenTraceOutputControl()
		{
			InitializeComponent ();

			_debugListener = new NuGenTraceListener();
			_debugListener.TraceHappened += new Genetibase.Debug.NuGenTraceListener.TraceDelegate(_debugListener_DebugMessageAvailable);
		}

		/// <summary>
		/// Starts logging system dianostic messages
		/// </summary>
		public void StartLogging ()
		{
			Trace.Listeners.Add(_debugListener);
		}

		/// <summary>
		/// Stop logging system diagnostic message
		/// </summary>
		public void StopLogging ()
		{
			Trace.Listeners.Remove(_debugListener);
		}

		[Category ("Output Window Properties")]
		[Description ("The output window")]
		public NuGenTViewCtrl OutputWindow
		{
			get { return NuGenTViewCtrl; }
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose (bool disposing)
		{
			if (disposing)
			{
				StopLogging();

				if (components != null)
				{
					components.Dispose ();
				}
			}
			base.Dispose (disposing);
		}

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent ()
		{
			this.NuGenTViewCtrl = new Genetibase.Debug.NuGenTViewCtrl();
			this.SuspendLayout();
			// 
			// NuGenTViewCtrl
			// 
			this.NuGenTViewCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
			this.NuGenTViewCtrl.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.NuGenTViewCtrl.Location = new System.Drawing.Point(0, 0);
			this.NuGenTViewCtrl.Name = "NuGenTViewCtrl";
			this.NuGenTViewCtrl.NuGenTSelectorBackColor = System.Drawing.Color.Gray;
			this.NuGenTViewCtrl.NuGenTSelectorForeColor = System.Drawing.Color.White;
			this.NuGenTViewCtrl.Size = new System.Drawing.Size(504, 288);
			this.NuGenTViewCtrl.TabIndex = 0;
			// 
			// NuGenTraceOutputControl
			// 
			this.Controls.Add(this.NuGenTViewCtrl);
			this.Name = "NuGenTraceOutputControl";
			this.Size = new System.Drawing.Size(504, 288);
			this.ResumeLayout(false);

		}

		private void UpdateUIProc (string message)
		{
			if (Regex.IsMatch(message, "^hIgHlItE"))
			{
				NuGenTViewCtrl.Add(message.Substring(8), true);
			}
			else
			{
				NuGenTViewCtrl.Add (message);
			}
			//NuGenTViewCtrl.Add (message);
		}

		private void _debugListener_DebugMessageAvailable (string message)
		{
			UpdateUIProc(message);
			//Invoke (new UpdateUIHandler (UpdateUIProc), new object[] {message});
		}
	}
}