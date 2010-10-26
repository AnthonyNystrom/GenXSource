using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Genetibase.NuGenRenderCore.Settings;
using Timer=System.Windows.Forms.Timer;

namespace Genetibase.NuGenDEMVis
{
    public partial class SplashDlg : Form
    {
        Timer timer;
        bool loaded = false;

        delegate void UpdateText(Label label, string text, bool add);
        delegate void _DoneLoading();
        Delegate UpdateTextDelegate;
        Delegate DoneLoadingDelegate;

        bool loadedForm = false;

        public SplashDlg()
        {
            InitializeComponent();

            UpdateTextDelegate = new UpdateText(DoUpdateText);
            DoneLoadingDelegate = new _DoneLoading(DoneLoading);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            loadedForm = true;
        }

        private static void DoUpdateText(Label label, string text, bool add)
        {
            if (add)
                label.Text += text;
            else
                label.Text = text;
        }

        public static HashTableSettings LoadConfig(SplashDlg dlg, bool devSettings)
        {
            if (dlg != null)
            {
                while (!dlg.loadedForm)
                {
                    Thread.Sleep(100);
                }
            }

            HashTableSettings gSettings;

            if (dlg != null)
                dlg.BeginInvoke(dlg.UpdateTextDelegate, dlg.label4, "global settings", false);

            Version version = Assembly.GetExecutingAssembly().GetName().Version;
            if (dlg != null)
            {
                string text;
                if (version.Major > 0)
                    text = version.ToString();
                else if (version.Minor > 0)
                    text = string.Format("Beta Build {0}.{1}.{2}", version.Minor, version.Build, version.Revision);
                else
                    text = string.Format("Alpha Build {0}.{1}", version.Build, version.Revision);
                dlg.BeginInvoke(dlg.UpdateTextDelegate, dlg.label1, text, false);
            }

            // load global settings
            gSettings = HashTableSettings.LoadFromXml(NuGenDEMVis.Properties.Resource1.DefaultGlobalSettings, false);
            gSettings.GlobalOnly = true;
            gSettings["Assembly.Version"] = version;
            gSettings["Base.Path"] = Path.GetFullPath(ConfigurationManager.AppSettings[(devSettings ? "dev@" : "") + "Base.Path.Relative"].Replace("%STARTUP%", Application.StartupPath));

            if (dlg != null)
                dlg.BeginInvoke(dlg.DoneLoadingDelegate);

            return gSettings;
        }

        private void DoneLoading()
        {
            loaded = true;
            label4.Text += " ...done";

            if (ConfigurationManager.AppSettings["Splash.CloseMode"] == "timer")
            {
                timer = new Timer();
                timer.Interval = 4000;
                timer.Tick += timer_Tick;
                timer.Enabled = true;
            }
            else
            {
                label2.Visible = true;
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (loaded)
                base.OnFormClosing(e);
            else if (e.CloseReason != CloseReason.UserClosing)
                e.Cancel = true;
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            // centre window
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            Location = new Point((screen.Width / 2) - (Width / 2), (screen.Height / 2) - (Height / 2));
        }

        void timer_Tick(object sender, System.EventArgs e)
        {
            timer.Enabled = false;
            Close();
        }

        private void pictureBox1_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void label1_Click(object sender, System.EventArgs e)
        {
            //Clipboard.SetText(label1.Text);
        }
    }
}
