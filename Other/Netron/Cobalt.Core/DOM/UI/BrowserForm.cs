using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Netron.Neon.WinFormsUI;

namespace Netron.Cobalt
{
    public partial class BrowserForm : DockContent, ITab
    {
        #region Fields
        /// <summary>
        /// the local base address
        /// </summary>
        private string mServerBaseAddress = "http://localhost:" + Properties.Settings.Default.HttpPort;
        #endregion

        /// <summary>
        /// Gets the local's server base address.
        /// </summary>
        /// <value>The server base address.</value>
        public string ServerBaseAddress
        {
            get { return mServerBaseAddress; }
        } 


        #region Constructor


        /// <summary>
        /// Initializes a new instance of the <see cref="T:BrowserForm"/> class.
        /// </summary>
        /// <include file="CodeDoc\DockContent.xml" path="//CodeDoc/Class[@name=&quot;DockHContent&quot;]/Constructor[@name=&quot;()&quot;]/*"/>
        public BrowserForm()
        {
            InitializeComponent();
            WebBrowser.Navigate("about:blank");
            WebBrowser.ScriptErrorsSuppressed = true;
            
        }
	    #endregion

        #region Browsing tools

        /// <summary>
        /// Handles the Click event of the homeNavigateButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void homeNavigateButton_Click(object sender, EventArgs e)
        {
            //this.rightTabControl.SelectedTab = tabWeb;
            Application.Tabs.WebBrowser.Show();
            //Application.Tabs.WebBrowser.Show(dockPanel, DockState.Document);
            Application.WebBrowser.GoHome();
        }

        /// <summary>
        /// Handles the Click event of the backNavigateButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void backNavigateButton_Click(object sender, EventArgs e)
        {
            //this.rightTabControl.SelectedTab = tabWeb;
            Application.WebBrowser.Show();
            Application.WebBrowser.GoBack();
        }

        /// <summary>
        /// Handles the Click event of the forwardNavigateButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void forwardNavigateButton_Click(object sender, EventArgs e)
        {
            //this.rightTabControl.SelectedTab = tabWeb;
            Application.WebBrowser.Show();
            Application.WebBrowser.GoForward();
        }

        /// <summary>
        /// Handles the Click event of the stopNavigateButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void stopNavigateButton_Click(object sender, EventArgs e)
        {
            Application.WebBrowser.Stop();
        }

        /// <summary>
        /// Handles the Click event of the netronNavigateButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void netronNavigateButton_Click(object sender, EventArgs e)
        {
            //this.rightTabControl.SelectedTab = tabWeb;
            WebBrowser.Show();
            WebBrowser.Navigate("http://www.netronproject.com");
        }

        /// <summary>
        /// Handles the Click event of the feedbackButton control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void feedbackButton_Click(object sender, EventArgs e)
        {
            GotoForum();
        }
        public  void GotoForum()
        {
            //this.rightTabControl.SelectedTab = tabWeb;
                          
            WebBrowser.Navigate("http://sourceforge.net/forum/forum.php?forum_id=237527");
        }

        #endregion

        /// <summary>
        /// Handles the Navigating event of the webBrowser control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.Windows.Forms.WebBrowserNavigatingEventArgs"/> instance containing the event data.</param>
        private void webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            Application.Status.StatusProgressBar.Visible = true;
            Application.Status.StatusProgressBar.Value = 0;
            if (e.Url.ToString().Equals("home", StringComparison.CurrentCultureIgnoreCase))
            {
                Application.WebBrowser.Stop();
                Application.WebBrowser.Navigate(ServerBaseAddress + "/Start/Welcome.htm");  
            }
            else if (e.Url.ToString().ToLower().StartsWith("root") )
            {
                Application.WebBrowser.Navigate(ServerBaseAddress +  e.Url.ToString().Substring(5));
            }
            else if (e.Url.ToString().ToLower().StartsWith("/root"))
            { 
               Application.WebBrowser.Navigate(ServerBaseAddress +  e.Url.ToString().Substring(6));
            }
            else if( e.Url.ToString().ToLower().StartsWith("http://root"))
            {
                Application.WebBrowser.Navigate(ServerBaseAddress + e.Url.ToString().Substring(11));
            }
            if (e.Url.ToString().StartsWith("doc://"))
            {
                // ShowContent(Swa.index.Replace("images/","doc://images/"));
                Application.WebBrowser.Stop();
                Application.WebBrowser.Navigate(ConvertToLocal(e.Url.ToString()));
            }
        }

        /// <summary>
        /// Converts to local a local "doc://" protocol-like prefix. Note that this is not a true protocol but only a masking of the rather ugly adress with 'localhost:port'.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        private string ConvertToLocal(string url)
        {
            if (!Properties.Settings.Default.HttpEnabled)
                Application.ShowContent("The server is not running error...");

            if (url.EndsWith("/"))
                url = url.Remove(url.Length - 1);
            url = url.Remove(0, 6);
            url = ServerBaseAddress + "/" + url;
            return url;
        }

        /// <summary>
        /// Handles the ProgressChanged event of the webBrowser control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.Windows.Forms.WebBrowserProgressChangedEventArgs"/> instance containing the event data.</param>
        private void webBrowser_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            if (e.CurrentProgress <= 0) return;
            Application.Status.StatusProgressBar.Value = Convert.ToInt32(Math.Round(e.CurrentProgress * (double)100 / e.MaximumProgress, 0));
        }

        /// <summary>
        /// Handles the Navigated event of the webBrowser control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.Windows.Forms.WebBrowserNavigatedEventArgs"/> instance containing the event data.</param>
        private void webBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            Application.Status.StatusProgressBar.Visible = false;
            Application.ToolBars.SetUrlAddress(e.Url.ToString());
        }

        private void GuideButton_Click(object sender, EventArgs e)
        {
            Application.WebBrowser.Navigate("doc://index.htm");
        }

        private void GoUrlButton_Click(object sender, EventArgs e)
        {
            Application.WebBrowser.Navigate(UrlBox.Text.Trim());
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            Properties.Settings.Default.BrowserStripLocation = this.BrowserStrip.Location;
        }
        private void urlBox_KeyDown(object sender, KeyEventArgs e)
        {


            if (e.KeyCode == Keys.Enter)
            {
                Application.WebBrowser.Navigate(UrlBox.Text.Trim());
                e.Handled = true;
            }
        }
        /// <summary>
        /// Handles the Click event of the browserToolStripMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void browserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewHideBrowserStrip();
        }
        /// <summary>
        /// View or hide the browser strip.
        /// </summary>
        private void ViewHideBrowserStrip()
        {
            browserToolStripMenuItem.Checked = !browserToolStripMenuItem.Checked;
            BrowserStrip.Visible = browserToolStripMenuItem.Checked;
        }

        private void UrlBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (UrlBox.SelectedIndex)
            { 
                case 0:
                    Application.WebBrowser.Navigate(ServerBaseAddress + "/Start/Welcome.htm");
                    break;
                case 1:
                    Application.WebBrowser.Navigate(ServerBaseAddress + "/Guide/API/");
                    break;
                case 2:
                    Application.WebBrowser.Navigate("http://www.netronproject.com");
                    break;
                case 3:
                    GotoForum();
                    break;
                case 4:
                    Application.WebBrowser.GoHome();
                    break;


            }
        }
    }
}