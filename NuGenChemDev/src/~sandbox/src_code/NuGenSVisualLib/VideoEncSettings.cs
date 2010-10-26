using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NuGenVideoEnc;

namespace NuGenSVisualLib.Recording
{
    public partial class VideoEncSettings : Form
    {
        RecordingSettings settings;

        int[] resFourThree = new int[] { 240, 180,
                                         320, 240,
                                         512, 384,
                                         640, 480,
                                         800, 600,
                                         1024, 768 };

        int[] resSixteenNine = new int[] { 320, 200,
                                           640, 400,
                                           720, 450,
                                           960, 600,
                                           1280, 800,
                                           1440, 900 };

        public VideoEncSettings(RecordingSettings settings)
        {
            InitializeComponent();

            this.settings = settings;

            LoadCodecs();
            LoadControlValues();
        }

        private void LoadControlValues()
        {
            // codec
            int idx = 0;
            listView1.SelectedIndices.Clear();
            foreach (ICodec codec in VideoEncodingInterface.AvailableCodecs)
            {
                if (codec == settings.Codec)
                    listView1.SelectedIndices.Add(idx);
                idx++;
            }

            // match res
            int[] res = null;
            ListBox resBox = null;
            if ((settings.Width / 4f) * 3f == settings.Height)
            {
                // 4:3
                radioButton1.Checked = true;
                res = resFourThree;
                resBox = listBox1;
            }
            else if ((settings.Width / 16f) * 9f == settings.Height)
            {
                // 16:9
                radioButton2.Checked = true;
                res = resSixteenNine;
                resBox = listBox2;
            }

            if (res != null)
            {
                // match into list
                bool selected = false;
                for (int i = 0; i < res.Length; i += 2)
                {
                    if (res[i] == settings.Width)
                    {
                        resBox.SelectedIndex = (int)((float)i * 0.5f);
                        selected = true;
                        break;
                    }
                }
                if (!selected)
                {
                    radioButton4.Checked = true;
                    numericUpDown1.Value = settings.Height;
                    numericUpDown2.Value = settings.Width;
                }
            }
            else
            {
                radioButton4.Checked = true;
                numericUpDown1.Value = settings.Height;
                numericUpDown2.Value = settings.Width;
            }
        }

        private void LoadCodecs()
        {
            foreach (ICodec codec in VideoEncodingInterface.AvailableCodecs)
            {
                ListViewItem item = new ListViewItem(new string[] { codec.Name, codec.CompressionType,
                                                                    codec.FileExtension, codec.MIMEType,
                                                                    codec.Provider});
                listView1.Items.Add(item);
            }
        }

        public RecordingSettings RecordingSettings
        {
            get { return settings; }
        }

        private void PackageControlValues()
        {
            settings = new RecordingSettings();
            
            // codec
            settings.Codec = VideoEncodingInterface.AvailableCodecs[listView1.SelectedIndices[0]];

            // res
            if (radioButton1.Checked)
            {
                settings.Width = resFourThree[listBox1.SelectedIndex * 2];
                settings.Height = resFourThree[(listBox1.SelectedIndex * 2) + 1];
            }
            else if (radioButton2.Checked)
            {
                settings.Width = resSixteenNine[listBox2.SelectedIndex * 2];
                settings.Height = resSixteenNine[(listBox2.SelectedIndex * 2) + 1];
            }
            else if (radioButton3.Checked)
            {
            }
            else
            {
                settings.Width = (int)numericUpDown2.Value;
                settings.Height = (int)numericUpDown1.Value;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PackageControlValues();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}