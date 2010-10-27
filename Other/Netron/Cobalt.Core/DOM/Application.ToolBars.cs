using System;
using System.Collections.Generic;
using System.Text;
using Netron.Neon.WinFormsUI;
using System.Windows.Forms;                     
namespace Netron.Cobalt
{
    static partial class Application
    {
        public static class ToolBars
        {

            public static ToolStrip BrowserStrip
            {
                get { return Application.Tabs.WebBrowser.BrowserStrip; }
            }

            public static ToolStripComboBox UrlBox
            {
                get { return Application.Tabs.WebBrowser.BrowserStrip.Items["UrlBox"] as ToolStripComboBox; }
            }


            public static void SetUrlAddress(string address)
            {
                if (address.StartsWith(Application.Tabs.WebBrowser.ServerBaseAddress)) ;
                address = address.Replace(Application.Tabs.WebBrowser.ServerBaseAddress + "/", "doc://");
                UrlBox.Text = address;
            }
        }
    }
}
