using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NuGenSVisualLib.Chem;
using Org.OpenScience.CDK.IO.Formats;
using System.IO;

namespace NuGenSVisualLib
{
    public partial class SupportedFormatsInfoDlg : Form
    {
        public SupportedFormatsInfoDlg()
        {
            InitializeComponent();
        }

        private void SupportedFormatsInfoDlg_Load(object sender, EventArgs e)
        {
            int temp;
            if (MoleculeLoader.supportedFormats == null)
                MoleculeLoader.CreateOpenFilter(out temp, null);

            //FileStream fs = new FileStream("formats.txt", FileMode.Create);
            //StreamWriter file = new StreamWriter(fs, Encoding.UTF8);

            foreach (IChemFormatMatcher format in MoleculeLoader.supportedFormats)
            {
                StringBuilder exts = new StringBuilder();

                string io = "";
                if (format.ReaderClassName != null && format.ReaderClassName.Length > 0)
                    io += "Read";
                if (format.WriterClassName != null && format.WriterClassName.Length > 0)
                {
                    if (io.Length > 0)
                        io += "/Write";
                    else
                        io += "Write";
                }

                int idx = 0;
                foreach (string ext in format.NameExtensions)
                {
                    if (idx++ > 0)
                        exts.Append(",");

                    exts.Append("*.");
                    exts.Append(ext);
                }

                ListViewItem item = new ListViewItem(new string[] { format.FormatName, exts.ToString(), format.MIMEType,
                                                                    format.XMLBased.ToString(), io});

                listView1.Items.Add(item);

                //file.WriteLine(format.FormatName + " | " + exts.ToString());
            }

            //file.Close();
            //fs.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}