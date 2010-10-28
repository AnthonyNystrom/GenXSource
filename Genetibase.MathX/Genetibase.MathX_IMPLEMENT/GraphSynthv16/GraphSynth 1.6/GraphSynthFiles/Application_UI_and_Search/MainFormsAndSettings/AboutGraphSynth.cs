using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;

namespace GraphSynth.Forms
{
    partial class AboutGraphSynth : Form
    {
        helpWindow help = new helpWindow();

        public AboutGraphSynth()
        {
            InitializeComponent();

            help = new helpWindow();
            /* Initialize the AboutBox to display the product information from the assembly information.
             *  Change assembly information settings for your application through either:
             *  - Project->Properties->Application->Assembly Information
             *  - AssemblyInfo.cs */
            this.Text = String.Format("About {0}", AssemblyTitle);
            this.labelProductName.Text = AssemblyProduct;
            this.labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
            this.labelCopyright.Text = AssemblyCopyright;
            this.labelCompanyName.Text = AssemblyCompany;
            this.labelDescription.Text = AssemblyDescription;

        }

        public void ShowAsDialog()
        {
            ShowDialog();
        }
        #region Assembly Attribute Accessors

        public string AssemblyTitle
        {
            get
            {
                /* Get all Title attributes on this assembly */
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                /* If there is at least one Title attribute */
                if (attributes.Length > 0)
                {
                    /* Select the first one */
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    /* If it is not an empty string, return it */
                    if (titleAttribute.Title != "")
                        return titleAttribute.Title;
                }
                /* If there was no Title attribute, or if the Title attribute 
                 * was the empty string, return the .exe name */
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AssemblyDescription
        {
            get
            {
                /* Get all Description attributes on this assembly */
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                /* If there aren't any Description attributes, return an empty string */
                if (attributes.Length == 0)
                    return "";
                /* If there is a Description attribute, return its value */
                return ((AssemblyDescriptionAttribute)attributes[0]).Description;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                /* Get all Product attributes on this assembly */
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                /* If there aren't any Product attributes, return an empty string */
                if (attributes.Length == 0)
                    return "";
                /* If there is a Product attribute, return its value */
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyCopyright
        {
            get
            {
                /* Get all Copyright attributes on this assembly */
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                /* If there aren't any Copyright attributes, return an empty string */
                if (attributes.Length == 0)
                    return "";
                /* If there is a Copyright attribute, return its value */
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AssemblyCompany
        {
            get
            {
                /* Get all Company attributes on this assembly */
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
                /* If there aren't any Company attributes, return an empty string */
                if (attributes.Length == 0)
                    return "";
                /* If there is a Company attribute, return its value */
                return ((AssemblyCompanyAttribute)attributes[0]).Company;
            }
        }
        #endregion


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        { help.BrowseUrlWithShellWebBrowser(Program.settings.onlineHelpURL+"index.htm"); }

        private void labelVersion_Click(object sender, EventArgs e)
        { help.BrowseUrlWithShellWebBrowser(Program.settings.onlineHelpURL + "download.htm"); }

        private void labelCompanyName_Click(object sender, EventArgs e)
        { help.BrowseUrlWithShellWebBrowser("http://www.utexas.edu"); }

        private void labelProductName_Click(object sender, EventArgs e)
        { help.getFullHelpFilePathThenOpen("index"); }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void labelCopyright_Click(object sender, EventArgs e)
            { help.BrowseUrlWithShellWebBrowser("http://www.me.utexas.edu/~campbell"); }
    }
}
