using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Genetibase.Debug
{
    /// <summary>
    /// Simple Diagnostics Listener Control
    /// </summary>
    public class NuGenOInternal : UserControl
    {
        private delegate void UpdateUIHandler (string message);

        private Container components = null;

        private DebugListener _debugListener;
        private NuGenTViewCtrl NuGenTViewCtrl;

        public NuGenOInternal ()
        {
            InitializeComponent ();

            _debugListener = new DebugListener ();
            _debugListener.DebugMessageAvailable += new DebugMessageAvailable (_debugListener_DebugMessageAvailable);
        }

        /// <summary>
        /// Starts logging system dianostic messages
        /// </summary>
        public void StartLogging ()
        {
            _debugListener.Start ();
        }

        /// <summary>
        /// Stop logging system diagnostic message
        /// </summary>
        public void StopLogging ()
        {
            _debugListener.Stop ();
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
                _debugListener.Stop ();

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
            this.NuGenTViewCtrl = new Genetibase.Debug.NuGenTViewCtrl ();
            this.SuspendLayout ();
            // 
            // NuGenTViewCtrl
            // 
            this.NuGenTViewCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NuGenTViewCtrl.Font =
                new System.Drawing.Font ("Courier New", 8.25F, System.Drawing.FontStyle.Regular,
                                         System.Drawing.GraphicsUnit.Point, ((System.Byte) (0)));
            this.NuGenTViewCtrl.Location = new System.Drawing.Point (0, 0);
            this.NuGenTViewCtrl.Name = "NuGenTViewCtrl";
            this.NuGenTViewCtrl.NuGenTSelectorBackColor = System.Drawing.Color.Gray;
            this.NuGenTViewCtrl.NuGenTSelectorForeColor = System.Drawing.Color.White;
            this.NuGenTViewCtrl.Size = new System.Drawing.Size (344, 216);
            this.NuGenTViewCtrl.TabIndex = 0;
            // 
            // NuGenOInternal
            // 
            this.Controls.Add (this.NuGenTViewCtrl);
            this.Name = "NuGenOInternal";
            this.Size = new System.Drawing.Size (344, 216);
            this.ResumeLayout (false);
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
        }

        private void _debugListener_DebugMessageAvailable (object sender, DebugMessageArgs e)
        {
            Invoke (new UpdateUIHandler (UpdateUIProc), new object[] {e.Message});
        }
    }
}