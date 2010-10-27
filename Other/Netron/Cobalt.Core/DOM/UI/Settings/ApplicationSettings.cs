using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Netron.Cobalt
{
    public partial class ApplicationSettings : Form
    {
        private Category currentCategory = Category.None;
        private ISettings currentSettings;
        private MainForm mainForm;
        enum Category
        {
            HttpServer,
            Diagram,
            Default,
            None
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (currentSettings != null)
                currentSettings.Save();
        }
        public ApplicationSettings(MainForm mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            LoadSettings(Category.Default);
        }

        private void webbrowserLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoadSettings(Category.HttpServer);
        }

        private void LoadSettings(Category category)
        {
            if (currentCategory == category)
                return;
            else
            {
                if (currentSettings != null)
                {

                    currentSettings.Save();
                    if(mainPanel.Controls.Contains(currentSettings as Control))
                        mainPanel.Controls.Remove(currentSettings as Control);
                    currentSettings.Dispose();
                }
            }
            Control control = null;
            switch (category)
            {
                case Category.HttpServer:

                    //control = new ServerSettings();

                    break;
                case Category.Diagram:
                    break;
                case Category.Default:
                    control = new DefaultSettings();
                    break;
                default:
                    break;
            }

            control.Dock = DockStyle.Fill;
            (control as ISettings).MainForm = mainForm;
            (control as ISettings).Load();
            mainPanel.Controls.Add(control);
            currentSettings = control as ISettings;
            currentCategory = category;
           
        }

      
    }
}