using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NuGenSVisualLib.Recording;

namespace NuGenSVisualLib.Recording
{
    partial class ViewRecordingDlg : Form
    {
        RecordingSettings settings;
        ViewRecording recording;

        public ViewRecordingDlg(RecordingSettings settings, ViewRecording recording)
        {
            InitializeComponent();

            this.settings = settings;
            this.recording = recording;

            SetupControlValues();
        }

        private void SetupControlValues()
        {
            label2.Text = recording.Duration.ToString();

            label11.Text = settings.Width.ToString() + "x" + settings.Height.ToString();
            label12.Text = settings.Codec.Name;
            label6.Text = ((double)settings.FramesPerSecond * recording.Duration.TotalSeconds).ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;


        }
    }
}