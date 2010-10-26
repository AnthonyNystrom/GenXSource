using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using Genetibase.UI.NuGenImageWorks.Undo;
using System.IO;
using Microsoft.Win32;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Genetibase.UI.NuGenImageWorks
{
    public delegate void ShowProgressCallback(int max, int value, string text);
    public delegate void SimpleDelegate();


    //[LicenseProvider(typeof(Xheo.Licensing.ExtendedLicenseProvider))]
    public partial class MainForm : Form
    {
        //private License _license = null;


        Genetibase.UI.NuGenImageWorks.Undo.UndoHelper undoHelper = null;
        private string tempDirectory = null;
        Color oldColor = Color.Empty;

        public string TempDirectory
        {
            get { return tempDirectory; }
            set { tempDirectory = value; }
        }

        private ContentAlignment textAlign = ContentAlignment.BottomCenter;
        private ContentAlignment imageAlign = ContentAlignment.TopCenter;

        private Image waterMarkImage = null;
        private String waterMarkText = null;
        private Font waterMarkFont = SystemFonts.DefaultFont;

        private Rectangle cropData = Rectangle.Empty;
        private Color coordsColor = Color.White;

        private string fileName = string.Empty;
        // Set to true when some selection has been made
        private bool selectionMade = false;
        // The current location of mouse pointer
        private int mouseX = 0;
        private int mouseY = 0;

        private bool drawingRect = false;        
        private Rectangle rect = new Rectangle(0, 0, 0, 0);
        private int initX = 0;
        private int initY = 0;

        // Pens for drawing the ant border
        private Pen p = null;
        private Pen p2 = null;

        // offSet is used to setup a crawling ants border
        private int offSet = 0;

        // The brush with which coordinates are drawn
        private Brush coordsBrush = null;
        private Brush b = null;

        private bool cropping = false;

        public MainForm()
        {
            //_license = LicenseManager.Validate(typeof(MainForm), this);

            Program.frmMain = this;
            InitializeComponent();            
            undoHelper = new Genetibase.UI.NuGenImageWorks.Undo.UndoHelper( this );
            
            undoRedoAtm.UndoHelper = undoHelper;
            undoRedoEnahnce.UndoHelper = undoHelper;
            undoRedoFilm.UndoHelper = undoHelper;
            undoRedoGain.UndoHelper = undoHelper;
            undoRedoGamma.UndoHelper = undoHelper;
            undoRedoLens.UndoHelper = undoHelper;
            undoRedoOffset.UndoHelper = undoHelper;
            undoRedoOpCtrl.UndoHelper = undoHelper;
            undoRedoEnhance2.UndoHelper = undoHelper;

            undoRedoAtm.MainForm = this;
            undoRedoEnahnce.MainForm = this;
            undoRedoFilm.MainForm = this;
            undoRedoGain.MainForm = this;
            undoRedoGamma.MainForm = this;
            undoRedoLens.MainForm = this;
            undoRedoOffset.MainForm = this;
            imageViewer1.MainForm = this;
            undoRedoOpCtrl.MainForm = this;
            undoRedoEnhance2.MainForm = this;

            p = new Pen(Color.Black);
            p2 = new Pen(Color.White);

            p.DashCap = p2.DashCap = DashCap.Flat;
            p.DashStyle = p2.DashStyle = DashStyle.Custom;
            p.DashPattern = p2.DashPattern = new float[] { 4, 4 };

            Color c = Color.Blue;
            c = Color.FromArgb(20, c.R, c.G, c.B);
             b = new SolidBrush(c);

            coordsBrush = new SolidBrush(coordsColor);
            this.redrawTimer.Start();

            this.ribbonButtonOp5OK.Enabled = false;
            this.ribbonButtonOp5Cancel.Enabled = false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Size x = Splash.Instance.Size;
            Splash.Fadeout();
            oldColor = Color.FromArgb(230, 233, 240);
            splitContainer1.BackColor = oldColor;
            Program.Source = this.pictureBox1;
            Program.Destination = this.pictureBox2;
            Program.Title = this.label4;

            this.tabPage7.Close();

            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Genetibase\ImageWorks");
            string tDir = (string)key.GetValue("ThumbnailDir");
            string pDir = (string)key.GetValue("PresetDir");

            if( tDir != null )
                this.imageViewer1.SourceDirectory = tDir;

            if( pDir != null )
                this.LoadPresetImages(pDir);
            
        }

        public void Open()
        {
            this.tabPage7.Open();
            this.splitContainer1.Enabled = true;
            this.splitContainer1.SplitterDistance = (this.Width / 2) - 6;
            this.splitContainer1.Panel1Collapsed = false;
            this.splitContainer1.Panel2Collapsed = false;

            Filter ns = new Filter();
            //ns.Click += new EventHandler(ns_Click);
            //ns.Image = new Bitmap(Program.Photo, new Size(48, 32));
            //ns.IsPressed = true;

            //this.ribbonGallery1.Controls.Clear();
            //this.ribbonGallery1.Controls.Add(ns);

            Program.Filter = ns;
            Program.Effects = new Effects();

            Reset();
            undoHelper.Clear();

            // refresh the histogram if its visible
            picHistBox.Invalidate();
            picBoxDestHist.Invalidate();
        }

        private void Reset()
        {
            ClearEnhance();
            ClearGain();
            ClearGamma();
            ClearOffset();
            ClearAtm();
            ClearEnhance();
            ClearLens();
            ClearFilm();
            ClearOperations();
            ClearEnhance2();

            SetAtmosphereParams();
            SetEnhanceParams();
            SetGainParams();
            SetGammaParams();
            SetLensParams();
            SetOffsetParams();
            SetFilmParams();
        }

        void ns_Click(object sender, EventArgs e)
        {
            GetParams();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState != FormWindowState.Minimized)
                this.splitContainer1.SplitterDistance = (this.Width / 2) - 6;

            try
            {
                Program.Source.Size = Program.Destination.Size;
                Program.Optimize2();
                //Program.Optimize(Program.Source.Width, Program.Source.Height);
            }
            catch { }

            //pictureBox1.Size = pictureBox2.Size;
            pictureBox2_ImageChanged(null, null);
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            splitContainer1.BackColor = oldColor;
            pictureBox1.SuspendLayout();
            if (Program.Photo != null)
            {
                //if (Program.Source.Width < Program.Destination.Width)
                //    Program.Optimize(Program.Destination.Width, Program.Destination.Height);
                //else                    
                    Program.Optimize2();
                    //Program.Optimize(Program.Source.Width, Program.Source.Height);
            }
            pictureBox1.ResumeLayout();
            splitContainer1.BackColor = oldColor;
        }



        private void splitContainer1_SplitterMoving(object sender, SplitterCancelEventArgs e)
        {            
            splitContainer1.BackColor = Color.Black;
        }

        private void splitContainer1_DoubleClick(object sender, EventArgs e)
        {
            MainForm_Resize(sender, e);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.FromArgb(115, 115, 115)), 0, ((Panel)sender).Height - 1, this.Width, ((Panel)sender).Height - 1);
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            splitContainer1.BackColor = Color.Black;
            pictureBox1.Hide();            
            if (this.splitContainer1.Panel2Collapsed)
            {
                this.splitContainer1.Panel2Collapsed = false;
                //panel1.Dock = DockStyle.None;
            }
            else
            {
                this.splitContainer1.Panel2Collapsed = true;
                //panel1.Dock = DockStyle.Fill;
            }           

            Program.Optimize2();
            //this.LayoutPictureBoxes();
            //Program.Optimize(panel1.Width, panel1.Height);
            pictureBox1.Show();
            splitContainer1.BackColor = oldColor;
        }        

        private void pictureBox2_DoubleClick(object sender, EventArgs e)
        {
            splitContainer1.BackColor = Color.Black;
            if (this.splitContainer1.Panel1Collapsed)
                this.splitContainer1.Panel1Collapsed = false;
            else
                this.splitContainer1.Panel1Collapsed = true;

            Application.DoEvents();            
            splitContainer1.BackColor = oldColor;
        }

        private void ribbonControl1_OnPopup(object sender)
        {
            PhotoMenu menu = new PhotoMenu(this);
            menu.Location = ((RibbonControl)sender).PointToScreen(new Point(0, -menu.Height));
            menu.Show();
        }

        internal void SetEnhanceParams()
        {
            Program.Filter.SaturationLow = this.ribbonTrackEnhance1.Value;
            Program.Filter.SaturationMid = this.ribbonTrackEnhance3.Value;
            Program.Filter.SaturationHigh = this.ribbonTrackEnhance5.Value;

            Program.Filter.ContrastLow = this.ribbonTrackEnhance2.Value * 100.0;
            Program.Filter.ContrastMid = this.ribbonTrackEnhance4.Value * 100.0;
            Program.Filter.ContrastHigh = this.ribbonTrackEnhance6.Value * 100.0;
        }

        internal void SetEnhance2Params()
        {
            Effects2.brightness = (int)(this.ribbonTrackEnhanceN1.Value * 255);
            Effects2.contrast = (int)(this.ribbonTrackEnhanceN2.Value * 100);
            Effects2.transparency = (int)(this.ribbonTrackEnhanceN3.Value * 100);
            Effects2.smooth = (int)(this.ribbonTrackEnhanceN4.Value * 10);
            Effects2.sharpen = (int)(this.ribbonTrackEnhanceN5.Value * 10);
        }


        internal void SetOffsetParams()
        {
            double aux = 2.0;
            //double.TryParse(this.textBox3.Text, out aux);
            Program.Filter.OffsetRange = aux / 2.0;
            Program.Filter.OffsetLow = new RGB((this.ribbonTrackOffset1.Value * 255.0) * Program.Filter.OffsetRange, (this.ribbonTrackOffset2.Value * 255.0) * Program.Filter.OffsetRange, (this.ribbonTrackOffset3.Value * 255.0) * Program.Filter.OffsetRange);
            Program.Filter.OffsetMid = new RGB((this.ribbonTrackOffset4.Value * 255.0) * Program.Filter.OffsetRange, (this.ribbonTrackOffset5.Value * 255.0) * Program.Filter.OffsetRange, (this.ribbonTrackOffset6.Value * 255.0) * Program.Filter.OffsetRange);
            Program.Filter.OffsetHigh = new RGB((this.ribbonTrackOffset7.Value * 255.0) * Program.Filter.OffsetRange, (this.ribbonTrackOffset8.Value * 255.0) * Program.Filter.OffsetRange, (this.ribbonTrackOffset9.Value * 255.0) * Program.Filter.OffsetRange);
        }
        internal void SetGainParams()
        {
            double aux = 2.0;
            //double.TryParse(this.textBox1.Text, out aux);
            Program.Filter.GainRange = aux;
            Program.Filter.TemperatureLow = this.ribbonTrackGain1.Value;
            Program.Filter.TemperatureMid = this.ribbonTrackGain7.Value;
            Program.Filter.TemperatureHigh = this.ribbonTrackGain13.Value;

            Program.Filter.MagentaLow = this.ribbonTrackGain2.Value;
            Program.Filter.MagentaMid = this.ribbonTrackGain8.Value;
            Program.Filter.MagentaHigh = this.ribbonTrackGain14.Value;

            Program.Filter.OverallLow = this.ribbonTrackGain3.Value;
            Program.Filter.OverallMid = this.ribbonTrackGain9.Value;
            Program.Filter.OverallHigh = this.ribbonTrackGain15.Value;

            Program.Filter.GainLow = new RGB(this.ribbonTrackGain4.Value, this.ribbonTrackGain5.Value, this.ribbonTrackGain6.Value);
            Program.Filter.GainMid = new RGB(this.ribbonTrackGain10.Value, this.ribbonTrackGain11.Value, this.ribbonTrackGain12.Value);
            Program.Filter.GainHigh = new RGB(this.ribbonTrackGain16.Value, this.ribbonTrackGain17.Value, this.ribbonTrackGain18.Value);

            Program.Filter.CalcGain();
        }
        internal void SetGammaParams()
        {
            Program.Filter.InGammaLow = new RGB(1.0 + (this.ribbonTrackGamma1.Value) * 2.0, 1.0 + (this.ribbonTrackGamma2.Value) * 2.0, 1.0 + (this.ribbonTrackGamma3.Value) * 2.0);
            Program.Filter.OutGammaLow = new RGB(1.0 + (this.ribbonTrackGamma4.Value) * 2.0, 1.0 + (this.ribbonTrackGamma5.Value) * 2.0, 1.0 + (this.ribbonTrackGamma6.Value) * 2.0);
            Program.Filter.InGammaMid = new RGB(1.0 + (this.ribbonTrackGamma7.Value) * 2.0, 1.0 + (this.ribbonTrackGamma8.Value) * 2.0, 1.0 + (this.ribbonTrackGamma9.Value) * 2.0);
            Program.Filter.OutGammaMid = new RGB(1.0 + (this.ribbonTrackGamma10.Value) * 2.0, 1.0 + (this.ribbonTrackGamma11.Value) * 2.0, 1.0 + (this.ribbonTrackGamma12.Value) * 2.0);
            Program.Filter.InGammaHigh = new RGB(1.0 + (this.ribbonTrackGamma13.Value) * 2.0, 1.0 + (this.ribbonTrackGamma14.Value) * 2.0, 1.0 + (this.ribbonTrackGamma15.Value) * 2.0);
            Program.Filter.OutGammaHigh = new RGB(1.0 + (this.ribbonTrackGamma16.Value) * 2.0, 1.0 + (this.ribbonTrackGamma17.Value) * 2.0, 1.0 + (this.ribbonTrackGamma18.Value) * 2.0);

            Program.Filter.CalcGamma();
        }

        private void GetParams()
        {
            this.ribbonTrackEnhance1.Value = Program.Filter.SaturationLow;
            this.ribbonTrackEnhance3.Value = Program.Filter.SaturationMid;
            this.ribbonTrackEnhance5.Value = Program.Filter.SaturationHigh;

            this.ribbonTrackEnhance2.Value = Program.Filter.ContrastLow / 100.0;
            this.ribbonTrackEnhance4.Value = Program.Filter.ContrastMid / 100.0;
            this.ribbonTrackEnhance6.Value = Program.Filter.ContrastHigh / 100.0;

            //this.textBox3.Text = (Program.Filter.OffsetRange * 2.0).ToString();
            this.ribbonTrackOffset1.Value = Program.Filter.OffsetLow.Red / (255.0 * Program.Filter.OffsetRange);
            this.ribbonTrackOffset2.Value = Program.Filter.OffsetLow.Green / (255.0 * Program.Filter.OffsetRange);
            this.ribbonTrackOffset3.Value = Program.Filter.OffsetLow.Blue / (255.0 * Program.Filter.OffsetRange);

            this.ribbonTrackOffset4.Value = Program.Filter.OffsetMid.Red / (255.0 * Program.Filter.OffsetRange);
            this.ribbonTrackOffset5.Value = Program.Filter.OffsetMid.Green / (255.0 * Program.Filter.OffsetRange);
            this.ribbonTrackOffset6.Value = Program.Filter.OffsetMid.Blue / (255.0 * Program.Filter.OffsetRange);

            this.ribbonTrackOffset7.Value = Program.Filter.OffsetHigh.Red / (255.0 * Program.Filter.OffsetRange);
            this.ribbonTrackOffset8.Value = Program.Filter.OffsetHigh.Green / (255.0 * Program.Filter.OffsetRange);
            this.ribbonTrackOffset9.Value = Program.Filter.OffsetHigh.Blue / (255.0 * Program.Filter.OffsetRange);

            //this.textBox1.Text = Program.Filter.GainRange.ToString();
            this.ribbonTrackGain1.Value = Program.Filter.TemperatureLow;
            this.ribbonTrackGain7.Value = Program.Filter.TemperatureMid;
            this.ribbonTrackGain13.Value = Program.Filter.TemperatureHigh;

            this.ribbonTrackGain2.Value = Program.Filter.MagentaLow;
            this.ribbonTrackGain8.Value = Program.Filter.MagentaMid;
            this.ribbonTrackGain14.Value = Program.Filter.MagentaHigh;

            this.ribbonTrackGain3.Value = Program.Filter.OverallLow;
            this.ribbonTrackGain9.Value = Program.Filter.OverallMid;
            this.ribbonTrackGain15.Value = Program.Filter.OverallHigh;

            this.ribbonTrackGain4.Value = Program.Filter.GainLow.Red;
            this.ribbonTrackGain5.Value = Program.Filter.GainLow.Green;
            this.ribbonTrackGain6.Value = Program.Filter.GainLow.Blue;

            this.ribbonTrackGain10.Value = Program.Filter.GainMid.Red;
            this.ribbonTrackGain11.Value = Program.Filter.GainMid.Green;
            this.ribbonTrackGain12.Value = Program.Filter.GainMid.Blue;

            this.ribbonTrackGain16.Value = Program.Filter.GainHigh.Red;
            this.ribbonTrackGain17.Value = Program.Filter.GainHigh.Green;
            this.ribbonTrackGain18.Value = Program.Filter.GainHigh.Blue;

            this.ribbonTrackGamma1.Value = (Program.Filter.InGammaLow.Red - 1.0) / 2.0;
            this.ribbonTrackGamma2.Value = (Program.Filter.InGammaLow.Green - 1.0) / 2.0;
            this.ribbonTrackGamma3.Value = (Program.Filter.InGammaLow.Blue - 1.0) / 2.0;

            this.ribbonTrackGamma4.Value = (Program.Filter.OutGammaLow.Red - 1.0) / 2.0;
            this.ribbonTrackGamma5.Value = (Program.Filter.OutGammaLow.Green - 1.0) / 2.0;
            this.ribbonTrackGamma6.Value = (Program.Filter.OutGammaLow.Blue - 1.0) / 2.0;

            this.ribbonTrackGamma7.Value = (Program.Filter.InGammaMid.Red - 1.0) / 2.0;
            this.ribbonTrackGamma8.Value = (Program.Filter.InGammaMid.Green - 1.0) / 2.0;
            this.ribbonTrackGamma9.Value = (Program.Filter.InGammaMid.Blue - 1.0) / 2.0;

            this.ribbonTrackGamma10.Value = (Program.Filter.OutGammaMid.Red - 1.0) / 2.0;
            this.ribbonTrackGamma11.Value = (Program.Filter.OutGammaMid.Green - 1.0) / 2.0;
            this.ribbonTrackGamma12.Value = (Program.Filter.OutGammaMid.Blue - 1.0) / 2.0;

            this.ribbonTrackGamma13.Value = (Program.Filter.InGammaHigh.Red - 1.0) / 2.0;
            this.ribbonTrackGamma14.Value = (Program.Filter.InGammaHigh.Green - 1.0) / 2.0;
            this.ribbonTrackGamma15.Value = (Program.Filter.InGammaHigh.Blue - 1.0) / 2.0;

            this.ribbonTrackGamma16.Value = (Program.Filter.OutGammaHigh.Red - 1.0) / 2.0;
            this.ribbonTrackGamma17.Value = (Program.Filter.OutGammaHigh.Green - 1.0) / 2.0;
            this.ribbonTrackGamma18.Value = (Program.Filter.OutGammaHigh.Blue - 1.0) / 2.0;

            Program.Filter.CalcGain();
            Program.Filter.CalcGamma();

            //Program.Destination.Image = (Bitmap)Program.Source.Image.Clone();
            //Program.Filter.Do((Bitmap)Program.Destination.Image);
            ////GB
            //Program.Destination.Image = Program.Effects.Do((Bitmap)Program.Destination.Image);
            FilterEffects();
        }

        private void ribbonButton1_Click(object sender, EventArgs e)
        {
            Reset();
            FilterEffects();
        }

        public void ClearGain()
        {
            this.ribbonTrackGain1.Value = 0.0;
            this.ribbonTrackGain2.Value = 0.0;
            this.ribbonTrackGain3.Value = 0.0;
            this.ribbonTrackGain4.Value = 0.0;
            this.ribbonTrackGain5.Value = 0.0;
            this.ribbonTrackGain6.Value = 0.0;

            this.ribbonTrackGain7.Value = 0.0;
            this.ribbonTrackGain8.Value = 0.0;
            this.ribbonTrackGain9.Value = 0.0;
            this.ribbonTrackGain10.Value = 0.0;
            this.ribbonTrackGain11.Value = 0.0;
            this.ribbonTrackGain12.Value = 0.0;

            this.ribbonTrackGain13.Value = 0.0;
            this.ribbonTrackGain14.Value = 0.0;
            this.ribbonTrackGain15.Value = 0.0;
            this.ribbonTrackGain16.Value = 0.0;
            this.ribbonTrackGain17.Value = 0.0;
            this.ribbonTrackGain18.Value = 0.0;
        }

        private void ribbonButton2_Click(object sender, EventArgs e)
        {
            Reset();
            FilterEffects();
        }

        public void ClearGamma()
        {
            this.ribbonTrackGamma1.Value = 0.0;
            this.ribbonTrackGamma2.Value = 0.0;
            this.ribbonTrackGamma3.Value = 0.0;
            this.ribbonTrackGamma4.Value = 0.0;
            this.ribbonTrackGamma5.Value = 0.0;
            this.ribbonTrackGamma6.Value = 0.0;

            this.ribbonTrackGamma7.Value = 0.0;
            this.ribbonTrackGamma8.Value = 0.0;
            this.ribbonTrackGamma9.Value = 0.0;
            this.ribbonTrackGamma10.Value = 0.0;
            this.ribbonTrackGamma11.Value = 0.0;
            this.ribbonTrackGamma12.Value = 0.0;

            this.ribbonTrackGamma13.Value = 0.0;
            this.ribbonTrackGamma14.Value = 0.0;
            this.ribbonTrackGamma15.Value = 0.0;
            this.ribbonTrackGamma16.Value = 0.0;
            this.ribbonTrackGamma17.Value = 0.0;
            this.ribbonTrackGamma18.Value = 0.0;
        }

        private void ribbonButton3_Click(object sender, EventArgs e)
        {
            Reset();
            FilterEffects();
        }

        public void ClearOffset()
        {
            this.ribbonTrackOffset1.Value = 0.0;
            this.ribbonTrackOffset2.Value = 0.0;
            this.ribbonTrackOffset3.Value = 0.0;

            this.ribbonTrackOffset4.Value = 0.0;
            this.ribbonTrackOffset5.Value = 0.0;
            this.ribbonTrackOffset6.Value = 0.0;

            this.ribbonTrackOffset7.Value = 0.0;
            this.ribbonTrackOffset8.Value = 0.0;
            this.ribbonTrackOffset9.Value = 0.0;
        }

        private void ribbonButton4_Click(object sender, EventArgs e)
        {
            Reset();
            FilterEffects();
        }

        public void ClearEnhance()
        {
            this.ribbonTrackEnhance1.Value = 1.0;
            this.ribbonTrackEnhance3.Value = 1.0;
            this.ribbonTrackEnhance5.Value = 1.0;

            this.ribbonTrackEnhance2.Value = 0.0;
            this.ribbonTrackEnhance4.Value = 0.0;
            this.ribbonTrackEnhance6.Value = 0.0;
        }

        public void ClearEnhance2()
        {
            this.ribbonTrackEnhanceN1.Value = this.ribbonTrackEnhanceN1.DefaultValue;
            this.ribbonTrackEnhanceN2.Value = this.ribbonTrackEnhanceN2.DefaultValue;
            this.ribbonTrackEnhanceN3.Value = this.ribbonTrackEnhanceN3.DefaultValue;
            this.ribbonTrackEnhanceN4.Value = this.ribbonTrackEnhanceN4.DefaultValue;
            this.ribbonTrackEnhanceN5.Value = this.ribbonTrackEnhanceN5.DefaultValue;
        }

        public void ClearAtm()
        {
            this.ribbonTrackAtm1.Value = this.ribbonTrackAtm1.DefaultValue;
            this.ribbonTrackAtm2.Value = this.ribbonTrackAtm2.DefaultValue;
            this.ribbonTrackAtm3.Value = this.ribbonTrackAtm3.DefaultValue;
            this.ribbonTrackAtm4.Value = this.ribbonTrackAtm4.DefaultValue;
            this.ribbonTrackAtm5.Value = this.ribbonTrackAtm5.DefaultValue;
            this.ribbonTrackAtm6.Value = this.ribbonTrackAtm6.DefaultValue;
            this.ribbonTrackAtm7.Value = this.ribbonTrackAtm7.DefaultValue;
            this.ribbonTrackAtm8.Value = this.ribbonTrackAtm8.DefaultValue;
            this.ribbonTrackAtm9.Value = this.ribbonTrackAtm9.DefaultValue;
            this.ribbonTrackAtm10.Value = this.ribbonTrackAtm10.DefaultValue;
            this.ribbonTrackAtm11.Value = this.ribbonTrackAtm11.DefaultValue;
            this.ribbonTrackAtm12.Value = this.ribbonTrackAtm12.DefaultValue;
            this.ribbonTrackAtm13.Value = this.ribbonTrackAtm13.DefaultValue;
            this.ribbonTrackAtm14.Value = this.ribbonTrackAtm14.DefaultValue;
            this.ribbonTrackAtm15.Value = this.ribbonTrackAtm15.DefaultValue;
            this.ribbonTrackAtm16.Value = this.ribbonTrackAtm16.DefaultValue;
            this.ribbonTrackAtm17.Value = this.ribbonTrackAtm17.DefaultValue;
            this.ribbonTrackAtm18.Value = this.ribbonTrackAtm18.DefaultValue;
        }

        public void ClearLens()
        {
            this.ribbonTrackLens1.Value = this.ribbonTrackLens1.DefaultValue;
            this.ribbonTrackLens2.Value = this.ribbonTrackLens2.DefaultValue;
            this.ribbonTrackLens3.Value = this.ribbonTrackLens3.DefaultValue;
            this.ribbonTrackLens4.Value = this.ribbonTrackLens4.DefaultValue;
            this.ribbonTrackLens5.Value = this.ribbonTrackLens5.DefaultValue;
            this.ribbonTrackLens6.Value = this.ribbonTrackLens6.DefaultValue;
            this.ribbonTrackLens7.Value = this.ribbonTrackLens7.DefaultValue;
            this.ribbonTrackLens8.Value = this.ribbonTrackLens8.DefaultValue;
            this.ribbonTrackLens9.Value = this.ribbonTrackLens9.DefaultValue;
            this.ribbonTrackLens10.Value = this.ribbonTrackLens10.DefaultValue;
            this.ribbonTrackLens11.Value = this.ribbonTrackLens11.DefaultValue;
            this.ribbonTrackLens12.Value = this.ribbonTrackLens12.DefaultValue;
            this.ribbonTrackLens13.Value = this.ribbonTrackLens13.DefaultValue;
            this.ribbonTrackLens14.Value = this.ribbonTrackLens14.DefaultValue;
            this.ribbonTrackLens15.Value = this.ribbonTrackLens15.DefaultValue;
            this.ribbonTrackLens16.Value = this.ribbonTrackLens16.DefaultValue;
            this.ribbonTrackLens17.Value = this.ribbonTrackLens17.DefaultValue;
            this.ribbonTrackLens18.Value = this.ribbonTrackLens18.DefaultValue;
            this.ribbonTrackLens19.Value = this.ribbonTrackLens19.DefaultValue;
            this.ribbonTrackLens20.Value = this.ribbonTrackLens20.DefaultValue;
        }

        public void ClearFilm()
        {
            this.ribbonTrackFilm1.Value = this.ribbonTrackFilm1.DefaultValue;
            this.ribbonTrackFilm2.Value = this.ribbonTrackFilm2.DefaultValue;
            this.ribbonTrackFilm3.Value = this.ribbonTrackFilm3.DefaultValue;
            this.ribbonTrackFilm4.Value = this.ribbonTrackFilm4.DefaultValue;
        }

        public void ClearOperations()
        {
            this.ribbonTextOp1.Text = "";
            this.waterMarkFont = SystemFonts.DefaultFont;

            this.ribbonPicOp1.Image = null;

            if (this.waterMarkImage != null)
                this.waterMarkImage.Dispose();

            this.waterMarkImage = null;            

            this.TextAlign = ContentAlignment.BottomCenter;
            this.ImageAlign = ContentAlignment.TopCenter;
            this.cropData = Rectangle.Empty;

            Effects2.RotateForUndo = RotateFlipType.RotateNoneFlipNone;
            Effects2.FlipForUndo = RotateFlipType.RotateNoneFlipNone;
            Effects2.BoxFilter = new BoxFilterProp(30, 0,Color.DarkBlue,Color.LightBlue,false);
            Effects2.FloorReflectionFilter = new FloorReflectionFilterProp(150,55,DockStyle.None,50);

            ribbonTrackEnhanceN1.Value = Effects2.brightness = 0;
            ribbonTrackEnhanceN2.Value = Effects2.contrast = 0;
            
            ribbonTrackEnhanceN3.Value = 1;
            Effects2.transparency = 100;
            
            ribbonTrackEnhanceN4.Value = Effects2.smooth = 0;
            ribbonTrackEnhanceN5.Value = Effects2.sharpen = 0;

            this.chkDropShadow.Checked = false;
            this.chkGrayScale.Checked = false;
            this.chkRounded.Checked = false;

            Effects2.curvature = 0;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            SetGainParams();
            FilterEffects();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            SetOffsetParams();
            FilterEffects();
        }

        private void ribbonButton5_Click(object sender, EventArgs e)
        {
            SavePreset();

            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Genetibase\ImageWorks");
            string pDir = (string)key.GetValue("PresetDir");

            if (pDir != null)
                this.LoadPresetImages(pDir);
        }

        private void SavePreset()
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.AddExtension = true;
            saveDialog.DefaultExt = ".pre";
            saveDialog.Filter = "NuGenImageWorks Presets | *.pre";

            DialogResult res = saveDialog.ShowDialog();

            if (res != DialogResult.OK)
                return;

            Application.DoEvents();

            BinaryWriter wr = null;

            try
            {
                wr = new BinaryWriter(File.Open(saveDialog.FileName, FileMode.OpenOrCreate, FileAccess.Write));
            }
            catch { return; }

            wr.Write("Version." + Application.ProductVersion + Environment.NewLine);

            wr.Write(ribbonTrackAtm1.Value);
            wr.Write(ribbonTrackAtm2.Value);
            wr.Write(ribbonTrackAtm3.Value);
            wr.Write(ribbonTrackAtm4.Value);
            wr.Write(ribbonTrackAtm5.Value);
            wr.Write(ribbonTrackAtm6.Value);
            wr.Write(ribbonTrackAtm7.Value);
            wr.Write(ribbonTrackAtm8.Value);
            wr.Write(ribbonTrackAtm9.Value);
            wr.Write(ribbonTrackAtm10.Value);
            wr.Write(ribbonTrackAtm11.Value);
            wr.Write(ribbonTrackAtm12.Value);
            wr.Write(ribbonTrackAtm13.Value);
            wr.Write(ribbonTrackAtm14.Value);
            wr.Write(ribbonTrackAtm15.Value);
            wr.Write(ribbonTrackAtm16.Value);
            wr.Write(ribbonTrackAtm17.Value);
            wr.Write(ribbonTrackAtm18.Value);



            wr.Write(ribbonTrackEnhance1.Value);
            wr.Write(ribbonTrackEnhance2.Value);
            wr.Write(ribbonTrackEnhance3.Value);
            wr.Write(ribbonTrackEnhance4.Value);
            wr.Write(ribbonTrackEnhance5.Value);
            wr.Write(ribbonTrackEnhance6.Value);



            wr.Write(ribbonTrackFilm1.Value);
            wr.Write(ribbonTrackFilm2.Value);
            wr.Write(ribbonTrackFilm3.Value);
            wr.Write(ribbonTrackFilm4.Value);



            wr.Write(ribbonTrackGain1.Value);
            wr.Write(ribbonTrackGain2.Value);
            wr.Write(ribbonTrackGain3.Value);
            wr.Write(ribbonTrackGain4.Value);
            wr.Write(ribbonTrackGain5.Value);
            wr.Write(ribbonTrackGain6.Value);
            wr.Write(ribbonTrackGain7.Value);
            wr.Write(ribbonTrackGain8.Value);
            wr.Write(ribbonTrackGain9.Value);
            wr.Write(ribbonTrackGain10.Value);
            wr.Write(ribbonTrackGain11.Value);
            wr.Write(ribbonTrackGain12.Value);
            wr.Write(ribbonTrackGain13.Value);
            wr.Write(ribbonTrackGain14.Value);
            wr.Write(ribbonTrackGain15.Value);
            wr.Write(ribbonTrackGain16.Value);
            wr.Write(ribbonTrackGain17.Value);
            wr.Write(ribbonTrackGain18.Value);



            wr.Write(ribbonTrackGamma1.Value);
            wr.Write(ribbonTrackGamma2.Value);
            wr.Write(ribbonTrackGamma3.Value);
            wr.Write(ribbonTrackGamma4.Value);
            wr.Write(ribbonTrackGamma5.Value);
            wr.Write(ribbonTrackGamma6.Value);
            wr.Write(ribbonTrackGamma7.Value);
            wr.Write(ribbonTrackGamma8.Value);
            wr.Write(ribbonTrackGamma9.Value);
            wr.Write(ribbonTrackGamma10.Value);
            wr.Write(ribbonTrackGamma11.Value);
            wr.Write(ribbonTrackGamma12.Value);
            wr.Write(ribbonTrackGamma13.Value);
            wr.Write(ribbonTrackGamma14.Value);
            wr.Write(ribbonTrackGamma15.Value);
            wr.Write(ribbonTrackGamma16.Value);
            wr.Write(ribbonTrackGamma17.Value);
            wr.Write(ribbonTrackGamma18.Value);



            wr.Write(ribbonTrackLens1.Value);
            wr.Write(ribbonTrackLens2.Value);
            wr.Write(ribbonTrackLens3.Value);
            wr.Write(ribbonTrackLens4.Value);
            wr.Write(ribbonTrackLens5.Value);
            wr.Write(ribbonTrackLens6.Value);
            wr.Write(ribbonTrackLens7.Value);
            wr.Write(ribbonTrackLens8.Value);
            wr.Write(ribbonTrackLens9.Value);
            wr.Write(ribbonTrackLens10.Value);
            wr.Write(ribbonTrackLens11.Value);
            wr.Write(ribbonTrackLens12.Value);
            wr.Write(ribbonTrackLens13.Value);
            wr.Write(ribbonTrackLens14.Value);
            wr.Write(ribbonTrackLens15.Value);
            wr.Write(ribbonTrackLens16.Value);
            wr.Write(ribbonTrackLens17.Value);
            wr.Write(ribbonTrackLens18.Value);
            wr.Write(ribbonTrackLens19.Value);
            wr.Write(ribbonTrackLens20.Value);



            wr.Write(ribbonTrackOffset1.Value);
            wr.Write(ribbonTrackOffset2.Value);
            wr.Write(ribbonTrackOffset3.Value);
            wr.Write(ribbonTrackOffset4.Value);
            wr.Write(ribbonTrackOffset5.Value);
            wr.Write(ribbonTrackOffset6.Value);
            wr.Write(ribbonTrackOffset7.Value);
            wr.Write(ribbonTrackOffset8.Value);
            wr.Write(ribbonTrackOffset9.Value);

            // Save Enhance Simple Settings
            wr.Write(ribbonTrackEnhanceN1.Value);
            wr.Write(ribbonTrackEnhanceN2.Value);
            wr.Write(ribbonTrackEnhanceN3.Value);
            wr.Write(ribbonTrackEnhanceN4.Value);
            wr.Write(ribbonTrackEnhanceN5.Value);


            // Save Operations
            wr.Write((int)Effects2.RotateForUndo);
            wr.Write((int)Effects2.FlipForUndo);

            // Croping data
            wr.Write(cropData.X);
            wr.Write(cropData.Y);
            wr.Write(cropData.Width);
            wr.Write(cropData.Height);

            // Drop shadow and grayscale
            wr.Write(Effects2.dropshadow);
            wr.Write(Effects2.grayscale);

            // BoxFilter
            wr.Write(Effects2.BoxFilter.Angle);
            wr.Write(Effects2.BoxFilter.BoxDepth);
            wr.Write(Effects2.BoxFilter.BoxEndColor.ToArgb());
            wr.Write(Effects2.BoxFilter.BoxStartColor.ToArgb());
            wr.Write(Effects2.BoxFilter.DrawImageOnSides);

            // Rounded
            wr.Write(Effects2.rounded);

            // WaterMark
            if (waterMarkText != null && waterMarkText.Length != 0)
            {
                wr.Write(waterMarkText.Length);
                wr.Write(waterMarkText);
            }
            else
            {
                wr.Write(0);
            }

            wr.Write((int)textAlign);
            MemoryStream ms = null;

            wr.Write(waterMarkFont.FontFamily.Name.Length);
            wr.Write(waterMarkFont.FontFamily.Name);
            wr.Write(waterMarkFont.Size);
            wr.Write((int)waterMarkFont.Style);

            if (waterMarkImage != null)
            {
                ms = new MemoryStream();
                Bitmap t = (Bitmap)waterMarkImage;
                t.Save(ms, ImageFormat.Png);
                /// Put in the lengh of the image
                wr.Write(ms.Length);
                // put in the actual image
                wr.Write(ms.ToArray());
            }
            else
            {
                /// Put in the zero to indicate null image
                wr.Write((long)0);
            }

            wr.Write((int)imageAlign);

            // Version > 1.4
            // Fisheye
            wr.Write(Effects2.curvature);

            wr.Write(Effects2.FloorReflectionFilter.AlphaEnd);
            wr.Write(Effects2.FloorReflectionFilter.AlphaStart);
            wr.Write((int)Effects2.FloorReflectionFilter.DockPosition);
            wr.Write(Effects2.FloorReflectionFilter.Offset);
            // End Version > 1.4

            if (Program.Destination.Image != null)
            {
                try
                {
                    ms = new MemoryStream();
                    Bitmap t = (Bitmap)Program.Destination.Image.GetThumbnailImage(50, 50, null, IntPtr.Zero);
                    t.Save(ms, ImageFormat.Png);
                    wr.Write(ms.ToArray());
                }
                catch { }
            }

            wr.Flush();
            wr.Close();


            RibbonItem preset = new RibbonItem();
            preset.Click += new EventHandler(preset_Click);
            try
            {
                preset.Image = ReadPreset(saveDialog.FileName);
            }
            catch { }
            preset.IsPressed = false;
            preset.Tag = saveDialog.FileName;
            preset.Cursor = Cursors.Hand;
            toolTip1.SetToolTip(preset, Path.GetFileNameWithoutExtension(saveDialog.FileName));
            presetGallery.Controls.Add(preset);
        }

        private void ribbonButton6_Click(object sender, EventArgs e)
        {
            //string dir = @"C:\Documents and Settings\hq230002\My Documents\";
           //LoadPresetImages(dir);
        }

        void preset_Click(object sender, EventArgs e)
        {
            RibbonItem preset = (RibbonItem)sender;
            string file = (string)preset.Tag;

            try
            {
                ApplyPreset(file);
            }
            catch { }
        }

        private void LoadPresetImages( string dir)
        {
            try
            {
                for (int i = 0; i < presetGallery.Controls.Count; i++)
                {
                    Control x = presetGallery.Controls[i];
                    presetGallery.Controls.RemoveAt(i);
                    i--;
                    x.Dispose();
                }

                string[] files = Directory.GetFiles(dir, "*.pre");

                foreach (string file in files)
                {
                    RibbonItem preset = new RibbonItem();
                    preset.Click += new EventHandler(preset_Click);
                    try
                    {
                        preset.Image = ReadPreset(file);
                    }
                    catch { }
                    preset.IsPressed = false;
                    preset.Tag = file;
                    preset.Cursor = Cursors.Hand;
                    toolTip1.SetToolTip(preset, Path.GetFileNameWithoutExtension(file));
                    presetGallery.Controls.Add(preset);
                }
            }
            catch { }
        }

        private Image ReadPreset(string FileName)
        {
            Stream s = File.Open(FileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(s);
            
            int major = 0;
            int minor = 0;
            
            try{
            	String version = br.ReadString();
            	if(version.IndexOf("Version.") >=0){
            		string []ver = version.Split(new char[]{'.'});
            		major = int.Parse(ver[1]);
            		minor = int.Parse(ver[2]);
            	}
            	else{
            		br.BaseStream.Seek(0,SeekOrigin.Begin);
            	}
            }catch
            {
            	br.BaseStream.Seek(0,SeekOrigin.Begin);
            }

            #region ReadDouble
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();



            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();



             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();



             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();



            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();



             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();
            br.ReadDouble();



             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();


             // Save Enhance Simple Settings
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();
             br.ReadDouble();


             // Read Operations
             br.ReadInt32();
             br.ReadInt32();

             // Croping data
             br.ReadInt32();
             br.ReadInt32();
             br.ReadInt32();
             br.ReadInt32();

             // Drop shadow and grayscale
             br.ReadBoolean();
             br.ReadBoolean();


             // BoxFilter
             br.ReadInt32();
             br.ReadInt32();
             br.ReadInt32();
             br.ReadInt32();
             br.ReadBoolean();

             // Rounded
             br.ReadBoolean();

             //Watermark
             int length = br.ReadInt32();

             if (length != 0)
             {
                 //discard 1st character
                 br.ReadChars(1);
                 br.ReadChars(length);
             }

             br.ReadInt32();

             length = br.ReadInt32();

             //discard 1st character
             br.ReadChars(1);
             br.ReadChars(length);
             br.ReadSingle();
             br.ReadInt32();

             //Watermark Image
             length = (int)br.ReadInt64();

             if (length != 0)
             {
                 br.ReadBytes(length);
             }

             br.ReadInt32();

#endregion

			// For Versions Greater then 1.4
            if( major > 1 || ( major ==1 && minor > 4) )
            {
            	br.ReadInt32();
            
            	br.ReadInt32();
            	br.ReadInt32();
            	br.ReadInt32();
            	br.ReadInt32();
            }
            // End For Versions Greater then 1.4

            byte[] image = br.ReadBytes((int)(s.Length - s.Position));

            MemoryStream ms = new MemoryStream(image);
            Bitmap presetImage = new Bitmap(ms);
            ms.Close();

            br.Close();

            return presetImage;
        }

        private void ApplyPreset(string FileName)
        {
            Stream s = File.Open(FileName,FileMode.Open,FileAccess.Read);
            BinaryReader br = new BinaryReader(s);
            
            int major = 0;
            int minor = 0;
            
            try{
            	String version = br.ReadString();
            	if(version.IndexOf("Version.") >=0){
            		string []ver = version.Split(new char[]{'.'});
            		major = int.Parse(ver[1]);
            		minor = int.Parse(ver[2]);
            	}
            	else{
            		br.BaseStream.Seek(0,SeekOrigin.Begin);
            	}
            }catch
            {
            	br.BaseStream.Seek(0,SeekOrigin.Begin);
            }

            ribbonTrackAtm1.Value = br.ReadDouble();
            ribbonTrackAtm2.Value = br.ReadDouble();
            ribbonTrackAtm3.Value = br.ReadDouble();
            ribbonTrackAtm4.Value = br.ReadDouble();
            ribbonTrackAtm5.Value = br.ReadDouble();
            ribbonTrackAtm6.Value = br.ReadDouble();
            ribbonTrackAtm7.Value = br.ReadDouble();
            ribbonTrackAtm8.Value = br.ReadDouble();
            ribbonTrackAtm9.Value = br.ReadDouble();
            ribbonTrackAtm10.Value = br.ReadDouble();
            ribbonTrackAtm11.Value = br.ReadDouble();
            ribbonTrackAtm12.Value = br.ReadDouble();
            ribbonTrackAtm13.Value = br.ReadDouble();
            ribbonTrackAtm14.Value = br.ReadDouble();
            ribbonTrackAtm15.Value = br.ReadDouble();
            ribbonTrackAtm16.Value = br.ReadDouble();
            ribbonTrackAtm17.Value = br.ReadDouble();
            ribbonTrackAtm18.Value = br.ReadDouble();



            ribbonTrackEnhance1.Value = br.ReadDouble();
            ribbonTrackEnhance2.Value = br.ReadDouble();
            ribbonTrackEnhance3.Value = br.ReadDouble();
            ribbonTrackEnhance4.Value = br.ReadDouble();
            ribbonTrackEnhance5.Value = br.ReadDouble();
            ribbonTrackEnhance6.Value = br.ReadDouble();



            ribbonTrackFilm1.Value = br.ReadDouble();
            ribbonTrackFilm2.Value = br.ReadDouble();
            ribbonTrackFilm3.Value = br.ReadDouble();
            ribbonTrackFilm4.Value = br.ReadDouble();



            ribbonTrackGain1.Value = br.ReadDouble();
            ribbonTrackGain2.Value = br.ReadDouble();
            ribbonTrackGain3.Value = br.ReadDouble();
            ribbonTrackGain4.Value = br.ReadDouble();
            ribbonTrackGain5.Value = br.ReadDouble();
            ribbonTrackGain6.Value = br.ReadDouble();
            ribbonTrackGain7.Value = br.ReadDouble();
            ribbonTrackGain8.Value = br.ReadDouble();
            ribbonTrackGain9.Value = br.ReadDouble();
            ribbonTrackGain10.Value = br.ReadDouble();
            ribbonTrackGain11.Value = br.ReadDouble();
            ribbonTrackGain12.Value = br.ReadDouble();
            ribbonTrackGain13.Value = br.ReadDouble();
            ribbonTrackGain14.Value = br.ReadDouble();
            ribbonTrackGain15.Value = br.ReadDouble();
            ribbonTrackGain16.Value = br.ReadDouble();
            ribbonTrackGain17.Value = br.ReadDouble();
            ribbonTrackGain18.Value = br.ReadDouble();



            ribbonTrackGamma1.Value = br.ReadDouble();
            ribbonTrackGamma2.Value = br.ReadDouble();
            ribbonTrackGamma3.Value = br.ReadDouble();
            ribbonTrackGamma4.Value = br.ReadDouble();
            ribbonTrackGamma5.Value = br.ReadDouble();
            ribbonTrackGamma6.Value = br.ReadDouble();
            ribbonTrackGamma7.Value = br.ReadDouble();
            ribbonTrackGamma8.Value = br.ReadDouble();
            ribbonTrackGamma9.Value = br.ReadDouble();
            ribbonTrackGamma10.Value = br.ReadDouble();
            ribbonTrackGamma11.Value = br.ReadDouble();
            ribbonTrackGamma12.Value = br.ReadDouble();
            ribbonTrackGamma13.Value = br.ReadDouble();
            ribbonTrackGamma14.Value = br.ReadDouble();
            ribbonTrackGamma15.Value = br.ReadDouble();
            ribbonTrackGamma16.Value = br.ReadDouble();
            ribbonTrackGamma17.Value = br.ReadDouble();
            ribbonTrackGamma18.Value = br.ReadDouble();



            ribbonTrackLens1.Value = br.ReadDouble();
            ribbonTrackLens2.Value = br.ReadDouble();
            ribbonTrackLens3.Value = br.ReadDouble();
            ribbonTrackLens4.Value = br.ReadDouble();
            ribbonTrackLens5.Value = br.ReadDouble();
            ribbonTrackLens6.Value = br.ReadDouble();
            ribbonTrackLens7.Value = br.ReadDouble();
            ribbonTrackLens8.Value = br.ReadDouble();
            ribbonTrackLens9.Value = br.ReadDouble();
            ribbonTrackLens10.Value = br.ReadDouble();
            ribbonTrackLens11.Value = br.ReadDouble();
            ribbonTrackLens12.Value = br.ReadDouble();
            ribbonTrackLens13.Value = br.ReadDouble();
            ribbonTrackLens14.Value = br.ReadDouble();
            ribbonTrackLens15.Value = br.ReadDouble();
            ribbonTrackLens16.Value = br.ReadDouble();
            ribbonTrackLens17.Value = br.ReadDouble();
            ribbonTrackLens18.Value = br.ReadDouble();
            ribbonTrackLens19.Value = br.ReadDouble();
            ribbonTrackLens20.Value = br.ReadDouble();



            ribbonTrackOffset1.Value = br.ReadDouble();
            ribbonTrackOffset2.Value = br.ReadDouble();
            ribbonTrackOffset3.Value = br.ReadDouble();
            ribbonTrackOffset4.Value = br.ReadDouble();
            ribbonTrackOffset5.Value = br.ReadDouble();
            ribbonTrackOffset6.Value = br.ReadDouble();
            ribbonTrackOffset7.Value = br.ReadDouble();
            ribbonTrackOffset8.Value = br.ReadDouble();
            ribbonTrackOffset9.Value = br.ReadDouble();

            // START


            // Save Enhance Simple Settings
            ribbonTrackEnhanceN1.Value = br.ReadDouble();
            ribbonTrackEnhanceN2.Value = br.ReadDouble();
            ribbonTrackEnhanceN3.Value = br.ReadDouble();
            ribbonTrackEnhanceN4.Value = br.ReadDouble();
            ribbonTrackEnhanceN5.Value = br.ReadDouble();


            // Read Operations
            Effects2.RotateForUndo = (RotateFlipType)br.ReadInt32();
            Effects2.FlipForUndo = (RotateFlipType)br.ReadInt32();

            // Croping data
            cropData.X = br.ReadInt32();
            cropData.Y = br.ReadInt32();
            cropData.Width = br.ReadInt32();
            cropData.Height = br.ReadInt32();

            // Drop shadow and grayscale
            chkDropShadow.Checked = br.ReadBoolean();
            chkGrayScale.Checked = br.ReadBoolean();
            //wr.Write(Effects2.dropshadow);
            //wr.Write(Effects2.grayscale);

            // BoxFilter
            Effects2.BoxFilter.Angle = br.ReadInt32();
            Effects2.BoxFilter.BoxDepth = br.ReadInt32();
            Effects2.BoxFilter.BoxEndColor = Color.FromArgb(br.ReadInt32());
            Effects2.BoxFilter.BoxStartColor = Color.FromArgb(br.ReadInt32());
            Effects2.BoxFilter.DrawImageOnSides = br.ReadBoolean();

            // Rounded
            chkRounded.Checked = br.ReadBoolean();
            //wr.Write(Effects2.rounded);

            //Watermark
            int length = br.ReadInt32();

            if (length != 0)
            {
                //discard 1st character
                br.ReadChars(1);
                waterMarkText = new String(br.ReadChars(length));                
            }
            else
            {
                waterMarkText = null;
            }

            ribbonTextOp1.Text = waterMarkText;
            textAlign = (ContentAlignment)br.ReadInt32();

            MemoryStream ms = null;

            length = (int)br.ReadInt32();

            //Skip one
            br.ReadChar();
            string familyName = new String(br.ReadChars(length));
            float size = br.ReadSingle();
            FontStyle style = (FontStyle)br.ReadInt32();

            waterMarkFont.Dispose();
            waterMarkFont = null;
            waterMarkFont = new Font(familyName, size, style);

            //Watermark Image
            long len = br.ReadInt64();

            if (len != 0)
            {   
                byte[] img = br.ReadBytes((int)len);
                ms = new MemoryStream(img);

                Bitmap temp = new Bitmap(ms);

                //HACK
                Bitmap newB = new Bitmap(temp.Width,temp.Height);
                
                Graphics g = Graphics.FromImage(newB);
                g.DrawImage(temp, 0, 0);
                g.Dispose();
                temp.Dispose();
                //END HACK

                WaterMarkImage = newB;

                ms.Close();
                img = null;
            }
            else
            {
                WaterMarkImage = null;
            }

            //openFileDialog1.ShowDialog();

            //WaterMarkImage = new Bitmap(openFileDialog1.FileName);

            imageAlign = (ContentAlignment)br.ReadInt32();

            /// END

            // For Versions Greater then 1.4
            if( major > 1 || ( major ==1 && minor > 4) )
            {
            	Effects2.curvature = br.ReadInt32();
            
            	Effects2.FloorReflectionFilter.AlphaEnd =  br.ReadInt32();
            	Effects2.FloorReflectionFilter.AlphaStart = br.ReadInt32();
            	Effects2.FloorReflectionFilter.DockPosition = (DockStyle)br.ReadInt32();
            	Effects2.FloorReflectionFilter.Offset = br.ReadInt32();
            }
            // End For Versions Greater then 1.4

            byte[] image = br.ReadBytes((int)(s.Length - s.Position));
            
            ms = new MemoryStream(image);            
            Bitmap presetImage = new Bitmap(ms);            
            ms.Close();

            br.Close();

            this.SetLensParams();
            this.SetFilmParams();
            this.SetAtmosphereParams();
            this.SetGammaParams();
            this.SetGainParams();
            this.SetOffsetParams();
            this.SetEnhanceParams();
            this.SetEnhance2Params();

            FilterEffects();
        }

        private void ribbonButton7_Click(object sender, EventArgs e)
        {
            ClearGain();
            ClearGamma();
            ClearOffset();
            ClearEnhance();

            SetEnhanceParams();
            SetOffsetParams();
            SetGainParams();
            SetGammaParams();

            FilterEffects();
        }

        private void ribbonTrack47_MouseClick(object sender, MouseEventArgs e)
        {
            SetEnhanceParams();
            FilterEffects();
        }

        private void ribbonTrack47_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                SetEnhanceParams();
                FilterEffects();
            }
        }

        private void ribbonTrack39_MouseClick(object sender, MouseEventArgs e)
        {
                SetOffsetParams();
                FilterEffects();
        }

        private void ribbonTrack39_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left )
            {
                //this.nuGenProgressRollerGlass1.Busy = true;
                SetOffsetParams();

                FilterEffects();
            }
        }

        private void ribbonTrack1_MouseClick(object sender, MouseEventArgs e)
        {
            SetGainParams();
            FilterEffects();
        }

        private void ribbonTrack1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left )
            {
                SetGainParams();
                FilterEffects();
            }
        }

        private void ribbonTrack21_MouseClick(object sender, MouseEventArgs e)
        {
                SetGammaParams();
                FilterEffects();
        }

        private void ribbonTrack21_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //this.nuGenProgressRollerGlass1.Busy = true;
                SetGammaParams();

                FilterEffects();
            }
        }

        private void ribbon_Track1_MouseClick(object sender, MouseEventArgs e)
        {
            //this.nuGenProgressRollerGlass1.Busy = true;
            SetAtmosphereParams();
            FilterEffects();
        }

        private void ribbon_Track20_MouseClick(object sender, MouseEventArgs e)
        {
            //this.nuGenProgressRollerGlass1.Busy = true;
            SetLensParams();
            FilterEffects();
        }

        private void ribbon_Track41_MouseClick(object sender, MouseEventArgs e)
        {
            //this.nuGenProgressRollerGlass1.Busy = true;
            SetFilmParams();
            FilterEffects();
        }

        private void ribbon_Track1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //this.nuGenProgressRollerGlass1.Busy = true;
                SetAtmosphereParams();

                FilterEffects();
            }
        }

        private void ribbon_Track20_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //this.nuGenProgressRollerGlass1.Busy = true;
                SetLensParams();
                FilterEffects();
            }
        }

        private void ribbon_Track41_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //this.nuGenProgressRollerGlass1.Busy = true;
                SetFilmParams();
                FilterEffects();
            }
        }

        private void ribbonControl1_OnPopup1(object sender)
        {
            PhotoMenu menu = new PhotoMenu(this);
            menu.Location = ((RibbonControl)sender).PointToScreen(new Point(0, -menu.Height));
            menu.Show();
        }

        private void splitContainer1_1_DoubleClick(object sender, EventArgs e)
        {
            MainForm_Resize(sender, e);
        }

        private void splitContainer1_1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            if (Program.Photo != null)
            {
                //this.nuGenProgressRollerGlass1.Busy = true;
                if (Program.Source.Width < Program.Destination.Width)
                    Program.Optimize(Program.Destination.Width, Program.Destination.Height);
                else
                    Program.Optimize2();
                    //Program.Optimize(Program.Source.Width, Program.Source.Height);
            }
        }

        //private void pictureBox1_1_DoubleClick(object sender, EventArgs e)
        //{
        //    if (this.splitContainer1_1.Panel2Collapsed)
        //        this.splitContainer1_1.Panel2Collapsed = false;
        //    else
        //        this.splitContainer1_1.Panel2Collapsed = true;

        //    Program.Optimize(Program.Source.Width, Program.Source.Height);
        //}

        //private void pictureBox2_1_DoubleClick(object sender, EventArgs e)
        //{
        //    if (this.splitContainer1_1.Panel1Collapsed)
        //        this.splitContainer1_1.Panel1Collapsed = false;
        //    else
        //        this.splitContainer1_1.Panel1Collapsed = true;

        //    Program.Optimize(Program.Destination.Width, Program.Destination.Height);
        //}

        private void panel1_1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.FromArgb(115, 115, 115)), 0, ((Panel)sender).Height - 1, this.Width, ((Panel)sender).Height - 1);
        }

        //private void Form1_Load1(object sender, EventArgs e)
        //{
        //    Program.Source = this.pictureBox1_1;
        //    Program.Destination = this.pictureBox2_1;
        //    Program.Title = this.label4_1;

        //    this.ribbonControl1.Close();
        //}

        //private void MainForm_Resize1(object sender, EventArgs e)
        //{
        //    if (this.WindowState != FormWindowState.Minimized)
        //        this.splitContainer1_1.SplitterDistance = (this.Width / 2) - 6;
        //}

        internal void SetAtmosphereParams()
        {
            Program.Effects.FogTopDensity = Color.FromArgb((int)(255.0 * this.ribbonTrackAtm4.Value), (int)(255.0 * this.ribbonTrackAtm1.Value), (int)(255.0 * this.ribbonTrackAtm2.Value), (int)(255.0 * this.ribbonTrackAtm3.Value));
            Program.Effects.FogCenterDensity = Color.FromArgb((int)(255.0 * this.ribbonTrackAtm5.Value), (int)(255.0 * this.ribbonTrackAtm1.Value), (int)(255.0 * this.ribbonTrackAtm2.Value), (int)(255.0 * this.ribbonTrackAtm3.Value));
            Program.Effects.FogBottomDensity = Color.FromArgb((int)(255.0 * this.ribbonTrackAtm6.Value), (int)(255.0 * this.ribbonTrackAtm1.Value), (int)(255.0 * this.ribbonTrackAtm2.Value), (int)(255.0 * this.ribbonTrackAtm3.Value));
            Program.Effects.FogCenter = this.ribbonTrackAtm7.Value;
            Program.Effects.FogFreqX = this.ribbonTrackAtm8.Value;
            Program.Effects.FogFreqY = this.ribbonTrackAtm9.Value;

            Program.Effects.LightLight = Color.FromArgb((int)(255.0 * this.ribbonTrackAtm10.Value), (int)(255.0 * this.ribbonTrackAtm11.Value), (int)(255.0 * this.ribbonTrackAtm12.Value));

            Program.Effects.RainDensity = Color.FromArgb((int)(255.0 * this.ribbonTrackAtm13.Value), (int)(255.0 * this.ribbonTrackAtm14.Value), (int)(255.0 * this.ribbonTrackAtm15.Value));
            Program.Effects.RainNumber = this.ribbonTrackAtm16.Value;
            Program.Effects.RainAngle = this.ribbonTrackAtm17.Value;
            Program.Effects.RainVelocity = this.ribbonTrackAtm18.Value;
        }
        internal void SetLensParams()
        {
            Program.Effects.HLightSize = this.ribbonTrackLens1.Value;
            Program.Effects.HLightSizeVar = this.ribbonTrackLens4.Value;
            Program.Effects.HLightRadius = this.ribbonTrackLens2.Value;
            Program.Effects.HLightRadiusVar = this.ribbonTrackLens5.Value;
            Program.Effects.HLightBright = this.ribbonTrackLens3.Value;
            Program.Effects.HLightSteps = this.ribbonTrackLens6.Value;
            Program.Effects.MotionAngle = this.ribbonTrackLens7.Value;
            Program.Effects.MotionVelocity = this.ribbonTrackLens8.Value;
            Program.Effects.ZoomOffsetX = this.ribbonTrackLens9.Value;
            Program.Effects.ZoomOffsetY = this.ribbonTrackLens10.Value;
            Program.Effects.ZoomZoom = this.ribbonTrackLens11.Value;
            Program.Effects.DepthOffsetX = this.ribbonTrackLens12.Value;
            Program.Effects.DepthOffsetY = this.ribbonTrackLens13.Value;
            Program.Effects.DepthRadius = this.ribbonTrackLens14.Value;
            Program.Effects.FocusOffsetX = this.ribbonTrackLens15.Value;
            Program.Effects.FocusOffsetY = this.ribbonTrackLens16.Value;
            Program.Effects.FocusScale = this.ribbonTrackLens17.Value;
            Program.Effects.FilterFilter = Color.FromArgb((int)(255.0 * this.ribbonTrackLens18.Value), (int)(255.0 * this.ribbonTrackLens19.Value), (int)(255.0 * this.ribbonTrackLens20.Value));
        }
        internal void SetFilmParams()
        {
            Program.Effects.ExposureTiming = this.ribbonTrackFilm1.Value;
            Program.Effects.ExposureCorrection = this.ribbonTrackFilm2.Value;
            Program.Effects.EnhanceSaturation = this.ribbonTrackFilm3.Value;
            Program.Effects.EnhanceContrast = this.ribbonTrackFilm4.Value;
        }

        private void picHistBox_Paint(object sender, PaintEventArgs e)
        {            
            Bitmap b = Utility.GetHistogram( (Bitmap) Program.Source.Image ,picHistBox.Width,picHistBox.Height );
            e.Graphics.DrawImage(b, 0, 0);
            b.Dispose();
        }

        private void picBoxDestHist_Paint(object sender, PaintEventArgs e)
        {
            Bitmap b = Utility.GetHistogram((Bitmap)Program.Destination.Image, picHistBox.Width, picHistBox.Height);
            e.Graphics.DrawImage(b, 0, 0);
            b.Dispose();
        }

        #region Enhance Value Changed Event Handler
        private void ribbonTrackEnhance1_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackEnhance1 = e.OldValue;
        }

        private void ribbonTrackEnhance2_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackEnhance2 = e.OldValue;
        }

        private void ribbonTrackEnhance3_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackEnhance3 = e.OldValue;
        }

        private void ribbonTrackEnhance4_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackEnhance4 = e.OldValue;
        }

        private void ribbonTrackEnhance5_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackEnhance5 = e.OldValue;
        }

        private void ribbonTrackEnhance6_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackEnhance6 = e.OldValue;
        }
        #endregion

        #region Offset Value Changed Event Handlers
        private void ribbonTrackOffset1_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackOffset1 = e.OldValue;
        }

        private void ribbonTrackOffset2_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackOffset2 = e.OldValue;
        }

        private void ribbonTrackOffset3_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackOffset3 = e.OldValue;
        }

        private void ribbonTrackOffset4_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackOffset4 = e.OldValue;
        }

        private void ribbonTrackOffset5_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackOffset5 = e.OldValue;
        }

        private void ribbonTrackOffset6_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackOffset6 = e.OldValue;
        }

        private void ribbonTrackOffset7_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackOffset7 = e.OldValue;
        }

        private void ribbonTrackOffset8_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackOffset8 = e.OldValue;
        }

        private void ribbonTrackOffset9_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackOffset9 = e.OldValue;
        }

#endregion

        #region Gain Value Changed Event Handlers

        private void ribbonTrackGain1_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGain1 = e.OldValue;
        }

        private void ribbonTrackGain2_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGain2 = e.OldValue;
        }

        private void ribbonTrackGain3_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGain3 = e.OldValue;
        }

        private void ribbonTrackGain4_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGain4 = e.OldValue;
        }

        private void ribbonTrackGain5_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGain5 = e.OldValue;
        }

        private void ribbonTrackGain6_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGain6 = e.OldValue;
        }

        private void ribbonTrackGain7_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGain7 = e.OldValue;
        }

        private void ribbonTrackGain8_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGain8 = e.OldValue;
        }

        private void ribbonTrackGain9_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGain9 = e.OldValue;
        }

        private void ribbonTrackGain10_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGain10 = e.OldValue;
        }

        private void ribbonTrackGain11_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGain11 = e.OldValue;
        }

        private void ribbonTrackGain12_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGain12 = e.OldValue;
        }

        private void ribbonTrackGain13_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGain13 = e.OldValue;
        }

        private void ribbonTrackGain14_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGain14 = e.OldValue;
        }

        private void ribbonTrackGain15_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGain15 = e.OldValue;
        }

        private void ribbonTrackGain16_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGain16 = e.OldValue;
        }

        private void ribbonTrackGain17_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGain17 = e.OldValue;
        }

        private void ribbonTrackGain18_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGain18 = e.OldValue;
        }


        #endregion

        #region Gamma Value Changed Event Handlers


        private void ribbonTrackGamma1_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGamma1 = e.OldValue;
        }

        private void ribbonTrackGamma2_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGamma2 = e.OldValue;
        }

        private void ribbonTrackGamma3_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGamma3 = e.OldValue;
        }

        private void ribbonTrackGamma4_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGamma4 = e.OldValue;
        }

        private void ribbonTrackGamma5_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGamma5 = e.OldValue;
        }

        private void ribbonTrackGamma6_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGamma6 = e.OldValue;
        }

        private void ribbonTrackGamma7_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGamma7 = e.OldValue;
        }

        private void ribbonTrackGamma8_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGamma8 = e.OldValue;
        }

        private void ribbonTrackGamma9_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGamma9 = e.OldValue;
        }

        private void ribbonTrackGamma10_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGamma10 = e.OldValue;
        }

        private void ribbonTrackGamma11_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGamma11 = e.OldValue;
        }

        private void ribbonTrackGamma12_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGamma12 = e.OldValue;
        }

        private void ribbonTrackGamma13_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGamma13 = e.OldValue;
        }

        private void ribbonTrackGamma14_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGamma14 = e.OldValue;
        }

        private void ribbonTrackGamma15_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGamma15 = e.OldValue;
        }

        private void ribbonTrackGamma16_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGamma16 = e.OldValue;
        }

        private void ribbonTrackGamma17_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGamma17 = e.OldValue;
        }

        private void ribbonTrackGamma18_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackGamma18 = e.OldValue;
        }


        #endregion

        #region Atmosphere Value Changed Event Handlers
        private void ribbonTrackAtm1_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackAtm1 = e.OldValue;
        }

        private void ribbonTrackAtm2_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackAtm2 = e.OldValue;
        }

        private void ribbonTrackAtm3_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackAtm3 = e.OldValue;
        }

        private void ribbonTrackAtm4_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackAtm4 = e.OldValue;
        }

        private void ribbonTrackAtm5_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackAtm5 = e.OldValue;
        }

        private void ribbonTrackAtm6_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackAtm6 = e.OldValue;
        }

        private void ribbonTrackAtm7_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackAtm7 = e.OldValue;
        }

        private void ribbonTrackAtm8_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackAtm8 = e.OldValue;
        }

        private void ribbonTrackAtm9_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackAtm9 = e.OldValue;
        }

        private void ribbonTrackAtm10_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackAtm10 = e.OldValue;
        }

        private void ribbonTrackAtm11_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackAtm11 = e.OldValue;
        }

        private void ribbonTrackAtm12_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackAtm12 = e.OldValue;
        }

        private void ribbonTrackAtm13_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackAtm13 = e.OldValue;
        }

        private void ribbonTrackAtm14_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackAtm14 = e.OldValue;
        }

        private void ribbonTrackAtm15_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackAtm15 = e.OldValue;
        }

        private void ribbonTrackAtm16_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackAtm16 = e.OldValue;
        }

        private void ribbonTrackAtm17_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackAtm17 = e.OldValue;
        }

        private void ribbonTrackAtm18_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackAtm18 = e.OldValue;
        }

#endregion

        #region Lens Value Changed Event Handlers

        private void ribbonTrackLens1_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackLens1 = e.OldValue;
        }

        private void ribbonTrackLens2_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackLens2 = e.OldValue;
        }

        private void ribbonTrackLens3_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackLens3 = e.OldValue;
        }

        private void ribbonTrackLens4_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackLens4 = e.OldValue;
        }

        private void ribbonTrackLens5_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackLens5 = e.OldValue;
        }

        private void ribbonTrackLens6_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackLens6 = e.OldValue;
        }

        private void ribbonTrackLens7_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackLens7 = e.OldValue;
        }

        private void ribbonTrackLens8_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackLens8 = e.OldValue;
        }

        private void ribbonTrackLens9_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackLens9 = e.OldValue;
        }

        private void ribbonTrackLens10_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackLens10 = e.OldValue;
        }

        private void ribbonTrackLens11_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackLens11 = e.OldValue;
        }

        private void ribbonTrackLens12_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackLens12 = e.OldValue;
        }

        private void ribbonTrackLens13_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackLens13 = e.OldValue;
        }

        private void ribbonTrackLens14_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackLens14 = e.OldValue;
        }

        private void ribbonTrackLens15_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackLens15 = e.OldValue;
        }

        private void ribbonTrackLens16_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackLens16 = e.OldValue;
        }

        private void ribbonTrackLens17_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackLens17 = e.OldValue;
        }

        private void ribbonTrackLens18_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackLens18 = e.OldValue;
        }

        private void ribbonTrackLens20_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackLens20 = e.OldValue;
        }

        private void ribbonTrackLens19_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackLens19 = e.OldValue;
        }


        #endregion

        #region Film Value Changed Event Handlers
        private void ribbonTrackFilm4_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackFilm4 = e.OldValue;
        }

        private void ribbonTrackFilm3_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackFilm3 = e.OldValue;
        }

        private void ribbonTrackFilm1_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackFilm1 = e.OldValue;
        }

        private void ribbonTrackFilm2_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackFilm2 = e.OldValue;
        }
        #endregion

        private void imageViewer1_SlideChanged(object sender, SlideChangedEventArgs e)
        {
            if (e.Visible)
            {
                pnlTop.Size = new Size(pnlTop.Width, 81);
                browseRibbonGroup.Show();
            }
            else
            {
                pnlTop.Size = new Size(pnlTop.Width, 16);
                browseRibbonGroup.Hide();                
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            f.CheckFileExists = false;
            f.FileName = "[Folder Selection..]";
            f.Filter = "Folders|no.files";

            DialogResult res = f.ShowDialog();

            if (res != DialogResult.OK)
                return;

            Application.DoEvents();

            string dir = Path.GetDirectoryName(f.FileName);
            if (dir == null || dir.Length < 1)
                return;

            Application.DoEvents();
            
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Genetibase\ImageWorks");
            key.SetValue("ThumbnailDir", dir);

            //this.nuGenProgressRollerGlass1.Busy = true;
            imageViewer1.SourceDirectory = dir;
            //this.nuGenProgressRollerGlass1.Busy = false;
        }

        private void btnBrowsePreset_Click(object sender, EventArgs e)
        {
            OpenFileDialog f = new OpenFileDialog();
            f.CheckFileExists = false;
            f.FileName = "[Folder Selection..]";
            f.Filter = "Folders|no.files";

            DialogResult res = f.ShowDialog();

            if (res != DialogResult.OK)
                return;

            Application.DoEvents();

            string dir = Path.GetDirectoryName(f.FileName);
            if (dir == null || dir.Length < 1)
                return;

            Application.DoEvents();

            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Genetibase\ImageWorks");
            key.SetValue("PresetDir", dir);
            LoadPresetImages(dir);
        }

        private void ribbonButton6_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "NuGenImageWorks Presets | *.pre";
            DialogResult res = openDlg.ShowDialog();

            if (res != DialogResult.OK)
                return;

            Application.DoEvents();
            try
            {
                ApplyPreset(openDlg.FileName);
            }
            catch { }
        }

        public void HelpMode(bool mode)
        {
            this.helpToolTip.Active = mode;

            if (mode)
            {
                this.Cursor = Cursors.Help;
            }
            else
            {
                this.Cursor = Cursors.Default;
            }
        }

        public Bitmap FilterEffects()
        {
            return FilterEffects(true);
        }

        public Bitmap FilterEffects(bool updateDisplay)
        {
            if (Program.Photo != null)
            {
                try
                {                   
                    Bitmap b = (Bitmap)Program.Source.Image.Clone();

                    if (cropData != Rectangle.Empty)
                    {
                        Bitmap oldB = b;
                        b = Operations.Crop(b, (int)(cropData.X * Program.percentage), (int)(cropData.Y * Program.percentage), (int)(cropData.Width * Program.percentage), (int)(cropData.Height * Program.percentage));
                        oldB.Dispose();
                        oldB = null;
                    }                    

                    if (Effects2.Rotate != RotateFlipType.RotateNoneFlipNone)
                    {
                        b.RotateFlip(Effects2.Rotate);
                    }

                    if (Effects2.Flip != RotateFlipType.RotateNoneFlipNone)
                    {
                        b.RotateFlip(Effects2.Flip);
                    }                   

                    Program.Filter.Do(b);
                    //GB
                    b = Program.Effects.Do(b);

                    Effects2.Do(b);

                    WaterMark wm = new WaterMark(this.WaterMarkText, this.TextAlign, this.WaterMarkFont, this.WaterMarkImage, this.ImageAlign);
                    b = (Bitmap)wm.MarkImage(b);


                    if (Effects2.rounded)
                    {
                        Effects2.RoundImageTransparent(b, 50);
                    }

                    if (Effects2.dropshadow)
                    {
                        Genetibase.BasicFilters.DropShadowFilter f3 = new Genetibase.BasicFilters.DropShadowFilter();
                        b = (Bitmap)f3.ExecuteFilter(b);
                    }

                    if (Effects2.curvature != 0)
                    {
                        Genetibase.BasicFilters.FisheyeFilter f = new Genetibase.BasicFilters.FisheyeFilter();
                        f.Curvature = (float)Effects2.curvature / 100;
                        b = (Bitmap)f.ExecuteFilter(b);
                    }

                    if (Effects2.FloorReflectionFilter.DockPosition != DockStyle.None)
                    {
                        Genetibase.BasicFilters.FloorReflectionFilter f = new Genetibase.BasicFilters.FloorReflectionFilter();
                        f.AlphaEnd = Effects2.FloorReflectionFilter.AlphaEnd;
                        f.AlphaStart = Effects2.FloorReflectionFilter.AlphaStart;
                        f.DockPosition = Effects2.FloorReflectionFilter.DockPosition;
                        f.Offset = (int)(Effects2.FloorReflectionFilter.Offset * Program.percentage);

                        b = (Bitmap)f.ExecuteFilter(b);
                    }

                    if (Effects2.BoxFilter.Angle != 0)
                    {
                        Genetibase.BasicFilters.BoxFilter f = new Genetibase.BasicFilters.BoxFilter();
                        f.DrawImageOnSides = Effects2.BoxFilter.DrawImageOnSides;
                        f.BoxStartColor = Effects2.BoxFilter.BoxStartColor;
                        f.BoxEndColor = Effects2.BoxFilter.BoxEndColor;
                        f.BoxDepth = (int)(Effects2.BoxFilter.BoxDepth * Program.percentage);
                        f.Angle = Effects2.BoxFilter.Angle;

                        if (Extra.Enable)
                        {
                            f.BackGroundColor = Extra.BackgroundColor;
                        }

                        b = (Bitmap)f.ExecuteFilter(b);
                    }

                    if (updateDisplay)
                    {
                        Bitmap temp = (Bitmap)Program.Destination.Image;
                        Program.Destination.Image = b;

                        if (temp != null)
                            temp.Dispose();
                    }

                    if (b != null)
                        return (Bitmap)b.Clone();
                    else
                        return null;
                }
                catch (Exception ex)
                {
                    Program.Destination.Image = (Bitmap)Program.Source.Image.Clone();
                }
                finally {  }
            }

            return null;
        }

        public Bitmap FilterEffectsForSave()
        {
            if (Program.Photo != null)
            {
                try
                {
                    Bitmap b = new Bitmap(Program.Photo);                    

                    if (Effects2.Rotate != RotateFlipType.RotateNoneFlipNone)
                    {
                        b.RotateFlip(Effects2.Rotate);
                    }

                    if (Effects2.Flip != RotateFlipType.RotateNoneFlipNone)
                    {
                        b.RotateFlip(Effects2.Flip);
                    }


                    if (cropData != Rectangle.Empty)
                    {
                        Bitmap oldB = b;
                        b = Operations.Crop(b, cropData.X, cropData.Y, cropData.Width, cropData.Height);
                        oldB.Dispose();
                        oldB = null;
                    }

                    Program.Filter.Do(b);
                    //GB
                    b = Program.Effects.Do(b);

                    Effects2.Do(b);

                    WaterMark wm = new WaterMark(this.WaterMarkText, this.TextAlign, this.waterMarkFont, this.waterMarkImage, this.ImageAlign);
                    b = (Bitmap)wm.MarkImage(b);

                    if (Effects2.rounded)
                    {
                        Effects2.RoundImageTransparent(b, 50);
                    }

                    if (Effects2.dropshadow)
                    {
                        Genetibase.BasicFilters.DropShadowFilter f3 = new Genetibase.BasicFilters.DropShadowFilter();
                        b = (Bitmap)f3.ExecuteFilter(b);
                    }


                    if (Effects2.curvature != 0)
                    {
                        Genetibase.BasicFilters.FisheyeFilter f = new Genetibase.BasicFilters.FisheyeFilter();
                        f.Curvature = (float)Effects2.curvature / 100;
                        b = (Bitmap)f.ExecuteFilter(b);
                    }

                    if (Effects2.FloorReflectionFilter.DockPosition != DockStyle.None)
                    {
                        Genetibase.BasicFilters.FloorReflectionFilter f = new Genetibase.BasicFilters.FloorReflectionFilter();
                        f.AlphaEnd = Effects2.FloorReflectionFilter.AlphaEnd;
                        f.AlphaStart = Effects2.FloorReflectionFilter.AlphaStart;
                        f.DockPosition = Effects2.FloorReflectionFilter.DockPosition;
                        f.Offset = Effects2.FloorReflectionFilter.Offset;

                        b = (Bitmap)f.ExecuteFilter(b);
                    }

                    if (Effects2.BoxFilter.Angle != 0)
                    {
                        Genetibase.BasicFilters.BoxFilter f = new Genetibase.BasicFilters.BoxFilter();
                        f.DrawImageOnSides = Effects2.BoxFilter.DrawImageOnSides;
                        f.BoxStartColor = Effects2.BoxFilter.BoxStartColor;
                        f.BoxEndColor = Effects2.BoxFilter.BoxEndColor;
                        f.BoxDepth = Effects2.BoxFilter.BoxDepth;
                        f.Angle = Effects2.BoxFilter.Angle;

                        if (Extra.Enable)
                        {
                            f.BackGroundColor = Extra.BackgroundColor;
                        }

                        b = (Bitmap)f.ExecuteFilter(b);//
                    }


                    return b;
                }
                catch(Exception)
                {                    
                }                                
            }

            return null;
        }

        private void progressPanel_Resize(object sender, EventArgs e)
        {

        }

        private void ribbonButtonOp2_Click(object sender, EventArgs e)
        {
            if (Program.Photo != null)
            {
                try
                {
                    undoHelper.Rotate = Effects2.Rotate;
                    Effects2.Rotate = RotateFlipType.Rotate90FlipNone;                    
                    /*undoHelper.Img = (Image)Program.Photo.Clone();
                    Program.Photo = (Bitmap)Program.Source.Image.Clone();
                    Program.Source.Refresh();*/
                    FilterEffects();                                     
                }
                catch { }

                
            }
        }

        private void ribbonButtonOp1_Click(object sender, EventArgs e)
        {
            if (Program.Photo != null)
            {
                try
                {
                    undoHelper.Rotate = Effects2.Rotate;
                    Effects2.Rotate = RotateFlipType.Rotate270FlipNone;                   
                    /*Program.Source.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    undoHelper.Img = (Image)Program.Photo.Clone();
                    Program.Photo = (Bitmap)Program.Source.Image.Clone();
                    Program.Source.Refresh();*/
                    FilterEffects();                              
                }
                catch { }
            }
        }

        private void ribbonButtonOp3_Click(object sender, EventArgs e)
        {
            if (Program.Photo != null)
            {
                try
                {
                    undoHelper.Flip = Effects2.Flip;
                    Effects2.Flip = RotateFlipType.RotateNoneFlipX;                   

                    /*Program.Source.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    undoHelper.Img = (Image)Program.Photo.Clone();
                    Program.Photo = (Bitmap)Program.Source.Image.Clone();
                    Program.Source.Refresh();*/
                    FilterEffects();                    

                }
                catch { }
            }
        }

        private void ribbonButtonOp4_Click(object sender, EventArgs e)
        {
            if (Program.Photo != null)
            {
                try
                {
                    undoHelper.Flip = Effects2.Flip;
                    Effects2.Flip = RotateFlipType.RotateNoneFlipY;

                    /*Program.Source.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    undoHelper.Img = (Image)Program.Photo.Clone();
                    Program.Photo = (Bitmap)Program.Source.Image.Clone();
                    Program.Source.Refresh();*/
                    FilterEffects();                    
                }
                catch { }
            }
        }

        private void ribbonButtonOp5_Click(object sender, EventArgs e)
        {
            ribbonButtonOp5.IsPressed = !ribbonButtonOp5.IsPressed;
            //drawingRect = ribbonButtonOp5.IsPressed;

            ribbonButtonOp5OK.Enabled = ribbonButtonOp5.IsPressed;
            ribbonButtonOp5Cancel.Enabled = ribbonButtonOp5.IsPressed;

            cropping = ribbonButtonOp5.IsPressed;

            if (cropping)
            {
                pictureBox1.Cursor = Cursors.Cross;
            }
            else
            {
                pictureBox1.Cursor = Cursors.Arrow;

            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (cropping && (selectionMade || drawingRect) && (rect.Width > 1 && rect.Height > 1))
            {
                offSet = (offSet - 1) % 8;

                p.DashOffset = offSet;
                p2.DashOffset = offSet + 4;

                e.Graphics.FillRectangle(b, rect);

                Rectangle newRect = new Rectangle(rect.X - 1, rect.Y - 1, rect.Width + 1, rect.Height + 1);

                e.Graphics.DrawRectangle(p, newRect);
                e.Graphics.DrawRectangle(p2, newRect);

                if (!selectionMade)
                {
                    string str = "(" + mouseX.ToString() + "," + mouseY.ToString() + ")";

                    if (drawingRect)
                        str = "(" + rect.Width.ToString() + "," + rect.Height.ToString() + ")";

                    Size coordinatesSize = e.Graphics.MeasureString(str, Font).ToSize();
                    e.Graphics.DrawString(str, this.Font, coordsBrush, mouseX + 5 - coordinatesSize.Width / 2, mouseY + 25);
                }
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                selectionMade = false;

                drawingRect = true;
                //drawingAnts = true;
                initX = e.X;
                initY = e.Y;

                rect.X = e.X;
                rect.Y = e.Y;

                rect.Width = 0;
                rect.Height = 0;
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;

            if (drawingRect && cropping)
            {
                SetRectangle(e.X, e.Y);
                this.pictureBox1.Invalidate();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (!selectionMade)
            {
                // this.redrawTimer.Stop();
                drawingRect = false;

                if (rect.Width > 0 && rect.Height > 0)
                {
                    selectionMade = true;
                }
            }
        }

        private void SetRectangle(int x, int y)
        {
            if (x >= rect.X)    // moving right
                rect.Width = x - initX;
            else                          // moving left
            {
                rect.X = x;
                rect.Width = initX - x;
            }

            if (y >= rect.Y)    // moving down
                rect.Height = y - initY;
            else                          // moving up
            {
                rect.Y = y;
                rect.Height = initY - y;
            }
        }

        private void redrawTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.pictureBox1.Invalidate();
        }

        private void ribbonButtonOp5OK_Click(object sender, EventArgs e)
        {
            try
            {   
                undoHelper.CropData = cropData;
                Bitmap orig = (Bitmap)Program.Photo;
                
                int newX = (int)(initX  / Program.percentage);
                int newY = (int)(initY  / Program.percentage);
                int newW = (int)(rect.Width  / Program.percentage);
                int newH = (int)(rect.Height / Program.percentage);

                cropData = new Rectangle(newX, newY, newW, newH);                
                //Program.Source.Image = Operations.Crop(orig, newX, newY, newW, newH);
                //Program.Photo = (Bitmap)Program.Source.Image.Clone();
                //orig.Dispose();

                //Program.Source.Refresh();

                FilterEffects();
            }
            catch { }
            finally
            {
                ribbonButtonOp5.IsPressed = false;
                pictureBox1.Cursor = Cursors.Arrow;
                ribbonButtonOp5.IsPressed = false;
                ribbonButtonOp5OK.Enabled = false;
                ribbonButtonOp5Cancel.Enabled = false;
                cropping = false;
                this.initX = 0;
                this.initY = 0;
                this.rect = Rectangle.Empty;
            }
        }

        private void pictureBox2_ImageChanged(object sender, EventArgs e)
        {
            if (Program.Source != null && Program.Source.Image != null)
            {
                LayoutPictureBoxes();
            }
        }

        internal void LayoutPictureBoxes()
        {
            Image b = Program.Source.Image;
            PictureBoxEx px = Program.Source;
            PictureBox px2 = Program.Destination;

            px.Size = b.Size;

            // width is more then height
            if (b.Width > b.Height)
            {                
                //float ratio = (float)b.Height / b.Width;
                //px.Height = (int)(px2.Width * ratio);
                //int newY = (int)(px2.Height / 2 - px.Height / 2);
                int newY, newX;

                //if (px.Width == panel1.Width)
                //{
                    newY = (int)((double)panel1.Height / 2 - (double)px.Height / 2);
                    newX = (int)((double)panel1.Width / 2 - (double)px.Width / 2);
                //}

                px.Location = new Point(newX, newY);               
                
            }
            else
            {
                //float ratio = (float)b.Width / b.Height;
                //px.Width = (int)(px2.Height * ratio);                
                //int newX = (int)(px2.Width / 2 - px.Width / 2);
                int newY, newX;

                //if (px.Height == panel1.Height)
                //{
                newY = (int)((double)panel1.Height / 2 - (double)px.Height / 2);
                newX = (int)((double)panel1.Width / 2 - (double)px.Width / 2);
                //}

                px.Location = new Point(newX, newY);
            }

            //CenterPictureBox();
        }

        private void CenterPictureBox()
        {
            Panel pnl = this.panel1;
            PictureBoxEx pix = this.pictureBox1;

            pix.Height = Program.Source.Image.Height;
            pix.Width = Program.Source.Image.Width;
        }

        private void ribbonButtonOp5Cancel_Click(object sender, EventArgs e)
        {
            ribbonButtonOp5.IsPressed = false;
            pictureBox1.Cursor = Cursors.Arrow;
            this.ribbonButtonOp5.Enabled = true;
            this.ribbonButtonOp5OK.Enabled = false;
            this.ribbonButtonOp5Cancel.Enabled = false;
            this.ribbonButtonOp5.IsPressed = false;

            this.initX = 0;
            this.initY = 0;
            this.rect = Rectangle.Empty;

            cropping = false;
        }


        public Image Img
        {
            get
            {
                return Program.Photo;
            }
            set
            {                
                Program.Source.Image = value;
                Program.Source.Refresh();
                FilterEffects();
            }
        }

        private void ribbonTextOp2_DoubleClick(object sender, EventArgs e)
        {
            FontDialog dlg = new FontDialog();
            dlg.Font = this.waterMarkFont;

            DialogResult res = dlg.ShowDialog();            

            if (res != DialogResult.OK)
                return;

            Application.DoEvents();

            undoHelper.WaterMarkFont = this.WaterMarkFont;

            this.waterMarkFont = dlg.Font;
            FilterEffects();
        }

        private void ribbonButton7_Click_1(object sender, EventArgs e)
        {
            try
            {
                undoHelper.WaterMarkImage = this.waterMarkImage;
                DialogResult res = this.openFileDialog1.ShowDialog();

                if (res != DialogResult.OK )
                    return;


                Application.DoEvents();                
                this.WaterMarkImage = new Bitmap(this.openFileDialog1.FileName);
            }
            catch{}

            Application.DoEvents();

            FilterEffects();
        }

        private void ribbonTextOp3_DoubleClick(object sender, EventArgs e)
        {
            frmCA objCA = new frmCA();
            objCA.ContentAlignment = this.textAlign;

            DialogResult res = objCA.ShowDialog();

            if (res != DialogResult.OK)
                return;

            Application.DoEvents();

            undoHelper.TextAlign = this.TextAlign;

            this.textAlign = objCA.ContentAlignment;

            Application.DoEvents();
            FilterEffects();
        }

        private void ribbonTextOp4_DoubleClick(object sender, EventArgs e)
        {
            frmCA objCA = new frmCA();
            objCA.ContentAlignment = this.imageAlign;

            DialogResult res = objCA.ShowDialog();

            if (res != DialogResult.OK)
                return;

            Application.DoEvents();

            undoHelper.ImageAlign = this.ImageAlign;

            this.imageAlign = objCA.ContentAlignment;

            Application.DoEvents();
            FilterEffects();
        }

        private void ribbonButton8_Click(object sender, EventArgs e)
        {
            undoHelper.WaterMarkImage = this.waterMarkImage;

            if (this.waterMarkImage != null)
                this.waterMarkImage.Dispose();

            this.WaterMarkImage = null;

            Application.DoEvents();
            FilterEffects();
        }

        private void ribbonTextOp1_TextChanged(object sender, EventArgs e)
        {
            undoHelper.WaterMarkText = this.WaterMarkText;
            waterMarkText = ribbonTextOp1.Text;

            Application.DoEvents();
            FilterEffects();
        }

        public Image WaterMarkImage
        {
            get 
            {
                if (waterMarkImage == null)
                    return null;

                int newW = (int)(waterMarkImage.Width * Program.percentage);
                int newH = (int)(waterMarkImage.Height * Program.percentage);

                if( newH == 0 )
                    newH = 1;
                if(newW == 0)
                    newW = 1;

                Image temp = waterMarkImage.GetThumbnailImage(newW, newH,null,IntPtr.Zero);
                return temp;
            }
            set
            {
                waterMarkImage = value;
                if (waterMarkImage != null)
                {
                    Image thumb = waterMarkImage.GetThumbnailImage(50, 50, null, IntPtr.Zero);
                    this.ribbonPicOp1.Image = thumb;
                }
                else
                {
                    this.ribbonPicOp1.Image = null;
                }                
            }
        }


        public Rectangle CropData
        {
            get { return cropData; }
            set { cropData = value; }
        }

        public String WaterMarkText
        {
            get
            {
                return waterMarkText;
            }
            set
            {
                waterMarkText = value;
                this.ribbonTextOp1.Text = value;
            }
        }

        public Font WaterMarkFont
        {
            get
            {   
                float newSize = (float)(waterMarkFont.Size * Program.percentage);                
                return new Font( waterMarkFont.FontFamily, newSize , waterMarkFont.Style);
                //return waterMarkFont;
            }
            set
            {
                waterMarkFont = value;
            }
        }

        public ContentAlignment ImageAlign
        {
            get
            {
                return imageAlign;
            }
            set
            {
                imageAlign = value;
            }
        }

        public ContentAlignment TextAlign
        {
            get
            {
                return textAlign;
            }
            set
            {
                textAlign = value;
            }
        }

        private void ribbonTrackEnhanceN1_MouseClick(object sender, MouseEventArgs e)
        {
            SetEnhance2Params();
            FilterEffects();
        }

        private void ribbonTrackEnhanceN1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                SetEnhance2Params();
                FilterEffects();
            }
        }

        private void ribbonTrackEnhanceN2_MouseClick(object sender, MouseEventArgs e)
        {
            SetEnhance2Params();
            FilterEffects();
        }

        private void ribbonTrackEnhanceN2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                SetEnhance2Params();
                FilterEffects();
            }
        }

        private void ribbonTrackEnhanceN3_MouseClick(object sender, MouseEventArgs e)
        {
            SetEnhance2Params();
            FilterEffects();
        }

        private void ribbonTrackEnhanceN3_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                SetEnhance2Params();
                FilterEffects();
            }
        }

        private void ribbonTrackEnhanceN1_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackEnhanceN1 = e.OldValue;
        }

        private void ribbonTrackEnhanceN2_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackEnhanceN2 = e.OldValue;
        }

        private void ribbonTrackEnhanceN3_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackEnhanceN3 = e.OldValue;
        }

        private void pictureBox2_Resize(object sender, EventArgs e)
        {
            try
            {
                LayoutPictureBoxes();
            }
            catch { }
        }

        private void ribbonTrackEnhanceN4_MouseClick(object sender, MouseEventArgs e)
        {
            SetEnhance2Params();
            FilterEffects();
        }

        private void ribbonTrackEnhanceN4_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                SetEnhance2Params();
                FilterEffects();
            }
        }

        private void ribbonTrackEnhanceN5_MouseClick(object sender, MouseEventArgs e)
        {
            SetEnhance2Params();
            FilterEffects();
        }

        private void ribbonTrackEnhanceN5_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                SetEnhance2Params();
                FilterEffects();
            }
        }

        private void ribbonTrackEnhanceN4_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackEnhanceN4 = e.OldValue;
        }

        private void ribbonTrackEnhanceN5_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            undoHelper.RibbonTrackEnhanceN5 = e.OldValue;
        }

        private void helpToolTip_Popup(object sender, PopupEventArgs e)
        {
            stopHelpTimer.Start();
        }

        private void stopHelpTimer_Tick(object sender, EventArgs e)
        {
            stopHelpTimer.Stop();
            this.HelpMode(false);            
        }

        private void btnBoxFilter_Click(object sender, EventArgs e)
        {
            frmBF bf = new frmBF();
            bf.BoxFilterProp = Effects2.BoxFilter;

            DialogResult res = bf.ShowDialog();
            Application.DoEvents();

            if (res != DialogResult.OK)
                return;

            Application.DoEvents();

            if (Program.Photo != null)
            {
                try
                {
                    undoHelper.BoxFilter = Effects2.BoxFilter;
                    Effects2.BoxFilter = bf.BoxFilterProp;

                    FilterEffects();

                }
                catch { }
            }
        }

        private void chkGrayScale_CheckedChanged(object sender, EventArgs e)
        {
            if (Program.Photo != null)
            {
                try
                {
                    undoHelper.GrayScale = Effects2.grayscale;
                    Effects2.grayscale = chkGrayScale.Checked;

                    FilterEffects();

                }
                catch { }
            }
        }

        private void chkDropShadow_CheckedChanged(object sender, EventArgs e)
        {
            if (Program.Photo != null)
            {
                try
                {
                    undoHelper.DropShadow = Effects2.dropshadow;
                    Effects2.dropshadow = chkDropShadow.Checked;

                    FilterEffects();

                }
                catch { }
            }
        }

        private void chkRounded_CheckedChanged(object sender, EventArgs e)
        {
            if (Program.Photo != null)
            {
                try
                {
                    undoHelper.Rounded = Effects2.rounded;
                    Effects2.rounded = chkRounded.Checked;

                    FilterEffects();

                }
                catch { }
            }
        }

        private void btnFishEye_Click(object sender, EventArgs e)
        {
            frmFE bf = new frmFE();          

            DialogResult res = bf.ShowDialog();
            Application.DoEvents();

            if (res != DialogResult.OK)
                return;

            Application.DoEvents();

            if (Program.Photo != null)
            {
                try
                {
                    undoHelper.FishEyeCurvature = Effects2.curvature;
                    Effects2.curvature = bf.Curvature;

                    FilterEffects();

                }
                catch { }
            }
        }

        private void btnFloorReflection_Click(object sender, EventArgs e)
        {
            frmFR bf = new frmFR();

            DialogResult res = bf.ShowDialog();
            Application.DoEvents();

            if (res != DialogResult.OK)
                return;

            Application.DoEvents();

            if (Program.Photo != null)
            {
                try
                {
                    undoHelper.FloorReflection = Effects2.FloorReflectionFilter;
                    Effects2.FloorReflectionFilter = bf.FloorReflection;

                    FilterEffects();

                }
                catch { }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Application.DoEvents();
            Program.Optimize(Program.Destination.Width, Program.Destination.Height);
            Program.Optimize2();
            Application.DoEvents();
        }


    }
}
