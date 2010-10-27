using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Leadtools.Forms.Ocr;
using Leadtools.Forms.DocumentWriters;
using System.Diagnostics;
using Leadtools.Codecs;
using Leadtools;

namespace DocumentParser
{
    public partial class MainForm : Form
    {
        // The OCR engine instance used in this demo
        private IOcrEngine _ocrEngine;
        // The current OCR document
        private IOcrDocument _ocrDocument;

        public MainForm()
        {
            InitializeComponent();

            Leadtools.Demos.Support.Unlock();

            _ocrEngine = OcrEngineManager.CreateEngine(OcrEngineType.Plus, false);
            _ocrEngine.Startup(null, null, null, null);            

            _ocrDocument = _ocrEngine.DocumentManager.CreateDocument();            
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Tiff Files|*.tif;*.tiff;";
            dlg.InitialDirectory = Path.Combine(Directory.GetCurrentDirectory(), "TestImages");

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (dlg.FileNames.Length == 1)
                {
                    try
                    {
                        sourceImageBox.Image = Image.FromFile(dlg.FileName);

                        _ocrDocument.Pages.Clear();
                        _ocrDocument.Pages.AddPage(dlg.FileName, null);

                        _ocrDocument.Pages.AutoZone(null);
                        _ocrDocument.Pages.UpdateFillMethod();
                    }
                    catch (FileNotFoundException)
                    {
                        MessageBox.Show("Error loading file");
                    }
                }
            }            
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            // Recognize the document
            _ocrDocument.Pages.Recognize(0, -1, null);

            // Pull out the text
            String text = _ocrDocument.Pages[0].RecognizeText(null);
            richTextBox1.Text = text;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _ocrEngine.Shutdown();
        }        
    }
}
