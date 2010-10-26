using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using NuGenSVisualLib.Settings;
using System.Reflection;

namespace ChemDevEnv
{
    public partial class Splash : Form
    {
        bool dev;

        public Splash(bool developer)
        {
            InitializeComponent();

            this.Opacity = 0;
            this.dev = developer;
        }

        public HashTableSettings globalSettings;

        private void Splash_Load(object sender, EventArgs e)
        {
            // load global settings
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ChemDevEnv.Resources.DefaultGlobalSettings.xml"))
            {
                globalSettings = HashTableSettings.LoadFromXml(new StreamReader(stream));
            }
            globalSettings["DeveloperMode"] = dev;
            // start fade out
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (dev)
                this.Opacity = 0;
            else if (this.Opacity > 0)
                this.Opacity -= 0.20;
            if (this.Opacity == 0)
            {
                timer1.Stop();
                this.Close();
            }
        }

        private void Splash_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Opacity != 0)
                e.Cancel = true;
        }
    }
}