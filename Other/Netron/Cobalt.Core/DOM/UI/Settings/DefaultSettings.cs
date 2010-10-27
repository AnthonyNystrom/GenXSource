using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Netron.Cobalt
{
    public partial class DefaultSettings : UserControl, ISettings
    {
        MainForm mainForm;

        public DefaultSettings()
        {
            InitializeComponent();
        }



        public new void Load()
        {
            //nothing to do
        }

        public void Save()
        {
            //nothing to do
        }
        public MainForm MainForm
        {
            get
            {
                return mainForm;
            }
            set
            {
                mainForm = value;
            }
        }
    }
}
