using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace GraphSynth.Forms
{
    public partial class helpWindow : Form
    {
        public void showHelpWindow()
        {
            InitializeComponent();
            this.MdiParent = Program.main;
        }


        public bool BrowseUrlWithShellWebBrowser(string URLstring)
        {
            Process process = new Process();
            process.StartInfo.FileName = URLstring;
            process.StartInfo.Verb = "open";
            process.StartInfo.UseShellExecute = true;
            bool bRet = false;
            try
            {
                bRet = process.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "URL-Browser Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return bRet;
        }
        public void getFullHelpFilePathThenOpen(string htmFile)
        {
            string fullpath = "";
            if (htmFile == null || htmFile.Length == 0)
                htmFile = "index";
            if (Program.settings.getHelpFromOnline == true)
                fullpath = Program.settings.onlineHelpURL;
            else fullpath = Program.settings.helpDirectory;
            fullpath += htmFile;
            fullpath += ".htm";
            BrowseUrlWithShellWebBrowser(fullpath);
        }


        private void startPageLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        { getFullHelpFilePathThenOpen("index"); }

        private void introMethodLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        { getFullHelpFilePathThenOpen("introMethod"); }

        private void SearchProcessLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        { getFullHelpFilePathThenOpen("searchProcess"); }

        private void graphsLlink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        { getFullHelpFilePathThenOpen("graphs"); }

        private void grammarsLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        { getFullHelpFilePathThenOpen("grammars"); }

        private void rulesetsLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        { getFullHelpFilePathThenOpen("ruleSets"); }

        private void recognitionLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        { getFullHelpFilePathThenOpen("rule_recognition"); }

        private void applicationLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        { getFullHelpFilePathThenOpen("rule_application"); }

        private void generationLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        { getFullHelpFilePathThenOpen("generation"); }

        private void examplesLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        { getFullHelpFilePathThenOpen("examples"); }

        private void downloadLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        { getFullHelpFilePathThenOpen("download"); }

        private void navigatingLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        { getFullHelpFilePathThenOpen("navigating"); }
    }
}