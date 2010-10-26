using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using NuGenSVisualLib.Rendering.Effects;

namespace NuGenSVisualLib
{
    public partial class LODcontrol : UserControl
    {
        public event EventHandler OnLODValueChanged;

        public LODcontrol()
        {
            InitializeComponent();
        }

        public ushort CurrentLODValue
        {
            get { return (ushort)trackBar2.Value; }
        }

        public void SetupValues(LevelOfDetailRange range, ushort value)
        {
            if (range == null)
            {
                this.Enabled = false;
                this.trackBar2.Minimum = value;
                this.trackBar2.Maximum = value;
                this.trackBar2.Value = value;
                return;
            }

            this.Enabled = true;
            this.trackBar2.Minimum = range.Min;
            this.trackBar2.Maximum = range.Max;
            this.trackBar2.Value = value;
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            if (OnLODValueChanged != null)
                OnLODValueChanged(this, null);
        }
    }
}
