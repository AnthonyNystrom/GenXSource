using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Windows.Forms;
using System.IO;
using Genetibase.NuGenMediImage.UI.Controls;
using Genetibase.NuGenMediImage.Utility;
using Genetibase.NuGenMediImage.Handlers;
using System.Threading;
using Genetibase.NuGenAnnotation;

namespace Genetibase.NuGenMediImage.UI.Controls
{
	/// <summary>
	/// Summary description for Viewer.
	/// </summary>
	public class Viewer : System.Windows.Forms.UserControl
	{
        internal event DicomSliceLoadEventHandler DicomSliceLoad;
        internal event DicomLoadEventHandler DicomLoad;

		//private System.ComponentModel.IContainer components;	

		private bool ZoomDrag = false;
        private bool annotationMode = true;
        public string filePath = string.Empty;        

		private bool MouseIsDown = false;
        private Cursor oldCursor =  null;
		
		//private bool _lutApplied = false;

		//private bool _overLay = false;
		private bool _overLayPaint = false;
		//private float currentBrightness = 0;
		//private float currentContrast = 1;
		private string currentfileName = string.Empty;
		private Color currentOverlayColor = Color.White;

		private const int DragBoxWidth = 100;
		private Genetibase.NuGenMediImage.UI.Controls.RibbonItem picBoxZoom;
		private const int DragBoxHeight = 100;

		//private Bitmap originalBitmap = null;
		//private Bitmap tempOriginalBitmap = null;
		private Bitmap zoomBoxBitmap = null;	

        //private Bitmap currentBitmap = null;

		//private ImageArray images = null;

        private bool headerVisible = false;

		private bool MLeft = false;
		private bool MRight = false;
		public bool MShift = false;

        private bool _mouseCBMode = false;
        private double _zoomBeforeCBMode = 1.0;
        private float _CBBrightness = 0.0F;
        private float _CBContrast = 1.0F;

		private int mouseDownStartX = 0;
		private int mouseDownStartY = 0;
		private Panel OuterPanel;
		public CustomPictureBox picBoxMain;
		private System.Windows.Forms.RichTextBox txtBoxMain;

		private double zoomBoxlastZoom = 1.0;
		private int zoomLevel = 2;

		private MeasurementUnits mUNits = MeasurementUnits.Inches;

		private bool _zoomFit = false;

        internal DrawArea _drawArea = null;        

        public bool AnnotationMode
        {
            get { return annotationMode; }
            set { annotationMode = value; }
        }

        public bool MouseCBMode
        {
            get { return _mouseCBMode; }
            set 
            { 
                _mouseCBMode = value;

                if (_mouseCBMode)
                {
                    if (!this.ZoomFit)
                        _zoomBeforeCBMode = this.Zoom;

                    _CBBrightness = this.Brightness;
                    _CBContrast = this.Contrast;

                    this.PerformZoomFit();
                }
                else
                {
                    if (!this.ZoomFit)
                        this.Zoom = _zoomBeforeCBMode;
                }

                SetupCBWindow();
            }
        }

        private bool _ZoomFit
        {
            get 
            {
                return _zoomFit;
            }
            set
            {
                _zoomFit = value;

                //if (_zoomFit)
                //    CenterImage();
                //else
                //    UnCenterImage();
            }
        }

		private double _zoomValue = 1;
		private float _brightness = 0;
        private int _emboss = 0;

        private float _contrast = 1;

		protected byte []_headerData = null;
		protected byte []_imageData = null;

        public event Genetibase.NuGenMediImage.NuGenMediImageCtrl.FileLoadedEventHandler FileLoaded;

        internal OpHelper opHelper;
        internal ImageArray originalBitmapArray = null;

        protected string _headerName = string.Empty;
        private ThemedPanel textBoxMainPanel;
        //private BackgroundWorker opWorker;
		protected string _imageName = string.Empty;
        private Panel ribbonProgress;
        private Label lblProgress;
        private ProgressBar progressBar1;
        public CustomPictureBox picBoxAdjustments;
        private RibbonButton btnDone;
        
		
		private double customDPI = -1;		


        public int SelectedIndex
        {
            get 
            {
                if (opHelper != null)
                    return opHelper.SelectedIndex;
                else
                    return 0;
            }
            set 
            {
                if( opHelper != null )
                    opHelper.SelectedIndex = value;
            }
        }

        public new bool AutoScroll
        {
            get
            {
                return this.OuterPanel.AutoScroll;
            }
            set
            {
                this.OuterPanel.AutoScroll = value;
            }
        }
		
		public string HeaderName
		{
			get
			{
				return _headerName;
			}
		}

        public bool HeaderVisible
        {
            get 
            { 
                return headerVisible; 
            }
            set 
            { 
                headerVisible = value;

                if (headerVisible)
                    ShowHeader();
                else
                    HideHeader();
            }
        }


		public string ImageName
		{
			get
			{
				return _imageName;
			}
		}


		public byte[] HeaderData
		{
			get
			{
				return _headerData;
			}
		}

		public byte[] ImageData
		{
			get
			{
				return _imageData;
			}
		}

		public float Brightness
		{
			get
			{
				return _brightness;
			}
            set
            {
                _brightness = value;
                DoBrightness(_brightness);
            }
		}


        public int Emboss
        {
            get { return _emboss; }
            set 
            { 
                _emboss = value;
                DoEmboss(_emboss);
            }
        }

		public float Contrast
		{
			get
			{
				return _contrast;
			}
            set
            {
                _contrast = value;
                DoContrast(_contrast);
            }
		}

		public double Zoom
		{
			get
			{
				return _zoomValue;
			}
            set
            {
                _zoomValue = value;
                _ZoomFit = false;
                DoZoom(value);                
            }
		}

		public bool ZoomFit
		{
			get
			{
				return _ZoomFit;
			}
			set
			{
				_ZoomFit = value;

                if (_ZoomFit)
                {
                    PerformZoomFit();
                }
                else
                {
                    Zoom100();
                }
			}
		}

		public ImageArray Images
		{
			get
			{
                if (originalBitmapArray != null)
                    return originalBitmapArray;

                return null;
                //if (opHelper != null)
                //    return opHelper.OriginalBitmap;

                //else return null;
			}
		}

		public MeasurementUnits MeasurementUnit
		{
			get
			{
				return mUNits;
			}

			set
			{
				mUNits = value;
			}
		}

		public string Header
		{
			get
			{
				return txtBoxMain.Text;
			}
		}

		public Size ZoomBoxSize
		{
			get
			{
				return picBoxZoom.Size;
			}
			set
			{
				picBoxZoom.Size = value;
			}
		}

		public int ZoomBoxZoom
		{
			get
			{
				return zoomLevel;
			}
			set
			{
				zoomLevel = value;
			}
		}
	

		public Viewer()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

            this.FileLoaded += new NuGenMediImageCtrl.FileLoadedEventHandler(Viewer_FileLoaded);
		}

        void Viewer_FileLoaded(object source, EventArgs e)
        {
            this.BackColor = Color.Black;            
        }

        private void FireFileLoaded()
        {
            if (FileLoaded != null)
                FileLoaded(this, new EventArgs());
        }

        private void FireDicomLoad(DicomLoadEventArgs e)
        {
            if (DicomLoad != null)
                DicomLoad(this, e);
        }

        private void FireSliceLoaded(DicomSliceLoadEventArgs e)
        {
            if (DicomSliceLoad != null)
                DicomSliceLoad(this, e);
        }

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
                //if(components != null)
                //{
                //    components.Dispose();
                //}

                //if( originalBitmap != null )
                //{
                //    originalBitmap.Dispose();
                //    originalBitmap = null;
                //}

                //if( tempOriginalBitmap != null )
                //{
                //    tempOriginalBitmap.Dispose();
                //    tempOriginalBitmap = null;
                //}

                if( zoomBoxBitmap != null )
                {
                    zoomBoxBitmap.Dispose();
                    zoomBoxBitmap = null;
                }

                //if( currentBitmap != null )
                //{
                //    currentBitmap.Dispose();
                //    currentBitmap = null;
                //}
			}
			base.Dispose( disposing );
		}
		
		#region Properties
		public Image Image
		{
			get{return this.picBoxMain.Image;}
			set
			{
				this.picBoxMain.Image = value;
				this.SetLayout();
				//this.ChangeSize();
			}
		}

        //internal Image OriginalImage
        //{
        //    get { return this.opHelper.OriginalBitmap[this.opHelper.SelectedIndex]; }            
        //}

		public bool ZoomWithDrag
		{
			get
			{
				return ZoomDrag;
			}
			set
			{
				ZoomDrag = value;
			}
		}

		#endregion
		
		#region Srollable Helper Function

		private void Scrollable()
		{
            //this.picBoxMain.SizeMode = PictureBoxSizeMode.AutoSize;
            this.picBoxMain.Width = this.picBoxMain.Image.Width;
            this.picBoxMain.Height = this.picBoxMain.Image.Height;
            //this.CenterImage();
		}

		private void SetPicBoxSize()
		{
            //this.AutoScroll = false;
            //this.picBoxMain.SizeMode = PictureBoxSizeMode.AutoSize;
            picBoxMain.Width = this.picBoxMain.Image.Width;
            picBoxMain.Height = this.picBoxMain.Image.Height;
            //this.AutoScroll = true;
		}		

		private void SetLayout()
		{
			if ( this.picBoxMain.Image == null )
				return;			
			else
			{
				//this.AutoScroll = false;
				this.Scrollable();	
				//this.AutoScroll = true;
			
			}
		}

		private void CenterImage()
		{
            int top = (int)((this.Height - this.picBoxMain.Height)/2.0);
			int left = (int)((this.Width - this.picBoxMain.Width)/2.0);
			if ( top < 0 )
				top = 0;
			if ( left < 0 )
				left = 0;
			this.picBoxMain.Top = top;
			this.picBoxMain.Left = left;

            try
            {
                this._drawArea.Left = this.picBoxMain.Left;
                this._drawArea.Top = this.picBoxMain.Top;
                this._drawArea.Width = picBoxMain.Width;
                this._drawArea.Height = picBoxMain.Height;
            }
            catch { }
            
		}

        private void UnCenterImage()
        {
            this.picBoxMain.Top = 0;
            this.picBoxMain.Left = 0;

            try
            {
                this._drawArea.Left = this.picBoxMain.Left;
                this._drawArea.Top = this.picBoxMain.Top;
                this._drawArea.Width = picBoxMain.Width;
                this._drawArea.Height = picBoxMain.Height;
            }
            catch { }
        }

		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.OuterPanel = new System.Windows.Forms.Panel();
            this.ribbonProgress = new System.Windows.Forms.Panel();
            this.lblProgress = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.picBoxZoom = new Genetibase.NuGenMediImage.UI.Controls.RibbonItem();
            this.picBoxMain = new Genetibase.NuGenMediImage.UI.Controls.CustomPictureBox();
            this.picBoxAdjustments = new Genetibase.NuGenMediImage.UI.Controls.CustomPictureBox();
            this.btnDone = new Genetibase.NuGenMediImage.UI.Controls.RibbonButton();
            this.textBoxMainPanel = new Genetibase.NuGenMediImage.UI.Controls.ThemedPanel();
            this.txtBoxMain = new System.Windows.Forms.RichTextBox();
            this.OuterPanel.SuspendLayout();
            this.ribbonProgress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxAdjustments)).BeginInit();
            this.textBoxMainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // OuterPanel
            // 
            this.OuterPanel.Controls.Add(this.ribbonProgress);
            this.OuterPanel.Controls.Add(this.picBoxMain);
            this.OuterPanel.Controls.Add(this.picBoxAdjustments);
            this.OuterPanel.Controls.Add(this.btnDone);
            this.OuterPanel.Controls.Add(this.textBoxMainPanel);
            this.OuterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OuterPanel.Location = new System.Drawing.Point(0, 0);
            this.OuterPanel.Name = "OuterPanel";
            this.OuterPanel.Size = new System.Drawing.Size(416, 328);
            this.OuterPanel.TabIndex = 5;
            this.OuterPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtBoxMain_MouseDown);
            this.OuterPanel.SizeChanged += new System.EventHandler(this.OuterPanel_SizeChanged);
            // 
            // ribbonProgress
            // 
            this.ribbonProgress.BackColor = System.Drawing.Color.Gray;
            this.ribbonProgress.Controls.Add(this.lblProgress);
            this.ribbonProgress.Controls.Add(this.progressBar1);
            this.ribbonProgress.Location = new System.Drawing.Point(6, 6);
            this.ribbonProgress.Name = "ribbonProgress";
            this.ribbonProgress.Size = new System.Drawing.Size(259, 42);
            this.ribbonProgress.TabIndex = 11;
            this.ribbonProgress.Visible = false;
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.BackColor = System.Drawing.Color.Transparent;
            this.lblProgress.ForeColor = System.Drawing.Color.Black;
            this.lblProgress.Location = new System.Drawing.Point(50, 2);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(158, 13);
            this.lblProgress.TabIndex = 2;
            this.lblProgress.Text = "Applying Operations to All Slices";
            // 
            // progressBar1
            // 
            this.progressBar1.ForeColor = System.Drawing.Color.DarkOrange;
            this.progressBar1.Location = new System.Drawing.Point(3, 16);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(237, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 3;
            this.progressBar1.Value = 50;
            // 
            // picBoxZoom
            // 
            this.picBoxZoom.IsPressed = false;
            this.picBoxZoom.Location = new System.Drawing.Point(72, 136);
            this.picBoxZoom.Margin = new System.Windows.Forms.Padding(0);
            this.picBoxZoom.Name = "picBoxZoom";
            this.picBoxZoom.Size = new System.Drawing.Size(100, 100);
            this.picBoxZoom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBoxZoom.TabIndex = 1;
            this.picBoxZoom.TabStop = false;
            this.picBoxZoom.Visible = false;
            // 
            // picBoxMain
            // 
            this.picBoxMain.Image = null;
            this.picBoxMain.Location = new System.Drawing.Point(0, 0);
            this.picBoxMain.Name = "picBoxMain";
            this.picBoxMain.Size = new System.Drawing.Size(150, 140);
            this.picBoxMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBoxMain.TabIndex = 3;
            this.picBoxMain.TabStop = false;
            this.picBoxMain.MouseLeave += new System.EventHandler(this.txtBoxMain_MouseLeave);
            this.picBoxMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picBoxMain_MouseDown);
            this.picBoxMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picBoxMain_MouseMove);
            this.picBoxMain.Paint += new System.Windows.Forms.PaintEventHandler(this.picBoxMain_Paint);
            this.picBoxMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picBoxMain_MouseUp);
            this.picBoxMain.MouseEnter += new System.EventHandler(this.txtBoxMain_MouseEnter);
            // 
            // picBoxAdjustments
            // 
            this.picBoxAdjustments.BackColor = System.Drawing.Color.Transparent;
            this.picBoxAdjustments.Image = null;
            this.picBoxAdjustments.Location = new System.Drawing.Point(0, 0);
            this.picBoxAdjustments.Name = "picBoxAdjustments";
            this.picBoxAdjustments.Size = new System.Drawing.Size(150, 140);
            this.picBoxAdjustments.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBoxAdjustments.TabIndex = 12;
            this.picBoxAdjustments.TabStop = false;
            this.picBoxAdjustments.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picBoxAdjustments_MouseDown);
            this.picBoxAdjustments.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picBoxAdjustments_MouseMove);
            this.picBoxAdjustments.Paint += new System.Windows.Forms.PaintEventHandler(this.picBoxAdjustments_Paint);
            this.picBoxAdjustments.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picBoxAdjustments_MouseUp);
            // 
            // btnDone
            // 
            this.btnDone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDone.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDone.ImageIndex = 8;
            this.btnDone.IsFlat = false;
            this.btnDone.IsPressed = false;
            this.btnDone.Location = new System.Drawing.Point(338, 305);
            this.btnDone.Margin = new System.Windows.Forms.Padding(1);
            this.btnDone.Name = "btnDone";
            this.btnDone.NgMediImage = null;
            this.btnDone.Padding = new System.Windows.Forms.Padding(2);
            this.btnDone.Size = new System.Drawing.Size(78, 22);
            this.btnDone.TabIndex = 45;
            this.btnDone.Text = "OK";
            this.btnDone.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnDone.Visible = false;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // textBoxMainPanel
            // 
            this.textBoxMainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(233)))), ((int)(((byte)(240)))));
            this.textBoxMainPanel.Controls.Add(this.txtBoxMain);
            this.textBoxMainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMainPanel.Location = new System.Drawing.Point(0, 0);
            this.textBoxMainPanel.Name = "textBoxMainPanel";
            this.textBoxMainPanel.Padding = new System.Windows.Forms.Padding(5);
            this.textBoxMainPanel.Size = new System.Drawing.Size(416, 328);
            this.textBoxMainPanel.TabIndex = 7;
            this.textBoxMainPanel.Visible = false;
            // 
            // txtBoxMain
            // 
            this.txtBoxMain.BackColor = System.Drawing.SystemColors.Window;
            this.txtBoxMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxMain.Location = new System.Drawing.Point(5, 5);
            this.txtBoxMain.Name = "txtBoxMain";
            this.txtBoxMain.ReadOnly = true;
            this.txtBoxMain.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtBoxMain.Size = new System.Drawing.Size(406, 318);
            this.txtBoxMain.TabIndex = 4;
            this.txtBoxMain.Text = "";
            this.txtBoxMain.MouseEnter += new System.EventHandler(this.txtBoxMain_MouseEnter);
            this.txtBoxMain.MouseLeave += new System.EventHandler(this.txtBoxMain_MouseLeave);
            this.txtBoxMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtBoxMain_MouseDown);
            // 
            // Viewer
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.picBoxZoom);
            this.Controls.Add(this.OuterPanel);
            this.Name = "Viewer";
            this.Size = new System.Drawing.Size(416, 328);
            this.Load += new System.EventHandler(this.Viewer_Load);
            this.Resize += new System.EventHandler(this.Viewer_Resize);
            this.OuterPanel.ResumeLayout(false);
            this.ribbonProgress.ResumeLayout(false);
            this.ribbonProgress.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxAdjustments)).EndInit();
            this.textBoxMainPanel.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void Viewer_Load(object sender, System.EventArgs e)
		{
			this.picBoxMain.Width = 0;
			this.picBoxMain.Height = 0;
			this.SetLayout();
		}

		private void Viewer_Resize(object sender, System.EventArgs e)
		{
            this.picBoxMain.Size = this.Size;

            if( ZoomFit )
                PerformZoomFit();
		}

        //private void DisposePicBoxImage()
        //{
        //    if( picBoxMain.Image != null )
        //    {
        //        picBoxMain.Image.Dispose();
        //        picBoxMain.Image = null;
        //    }
        //}

		private void CheckOverlay()
		{
			//if( _overLay )			
			//Console.WriteLine(" HELLO " + fileName + currentBrightness + currentContrast);
			//SetOverLayHeaderParam( currentOverlayColor,currentfileName, currentBrightness, currentContrast );				
			
		}

		#region Public Functions

        //private void SetOverLayHeaderParam( Color color, string fileName, float brightness, float contrast )
        //{
        //    this.picBoxMain.SetOverLayHeaderParam( color, fileName, brightness,contrast );
        //}

        //public void OverLayHeader( Color color, string fileName, float brightness, float contrast )
        //{	
        //    _overLay = true;
        //    currentOverlayColor = color;
        //    currentfileName = fileName;
        //    currentBrightness = brightness;
        //    currentContrast = contrast;
        //    this.picBoxMain.OverLayHeader( color, fileName, brightness, contrast );			
        //}

        //public void ClearOverlay()
        //{
        //    _overLay = false;
        //}


		internal void DoZoom( double ZoomFactor )
		{
            if (picBoxMain.Image == null)
                return;
            
            _zoomValue = ZoomFactor;

            //if (opHelper != null)
            //{
            //    opHelper.AddZoom(ZoomFactor);
            //    opWorker.RunWorkerAsync();
            //}
            //CheckOverlay();

            int newWidth = 0;
            int newHeight = 0;

            if (opHelper == null || opHelper.GetOperatedBitmap(opHelper.SelectedIndex) == null)
            {
                newWidth = Convert.ToInt32 (picBoxMain.Image.Width * ZoomFactor);
                newHeight = Convert.ToInt32 (picBoxMain.Image.Height * ZoomFactor);
            }
            else
            {
                newWidth = Convert.ToInt32(opHelper.GetOperatedBitmap(opHelper.SelectedIndex).Width * ZoomFactor);
                newHeight = Convert.ToInt32(opHelper.GetOperatedBitmap(opHelper.SelectedIndex).Height * ZoomFactor);
            }
            ////OuterPanel.AutoScroll = true;            
            //picBoxMain.Width = Convert.ToInt32 ( originalBitmap.Width * ZoomFactor );
            //picBoxMain.Height = Convert.ToInt32 ( originalBitmap.Height * ZoomFactor );
            //picBoxMain.Width = Convert.ToInt32(opHelper.GetOperatedBitmap(_selectedIndex).Width * ZoomFactor);
            //picBoxMain.Height = Convert.ToInt32(opHelper.GetOperatedBitmap(_selectedIndex).Height * ZoomFactor);
            //picBoxMain.Width = newWidth;
            //picBoxMain.Height = newHeight;



            //picBoxMain.SizeMode = PictureBoxSizeMode.StretchImage;

            //this._drawArea.Left = this.picBoxMain.Left;
            //this._drawArea.Top = this.picBoxMain.Top;
            //this._drawArea.Width = picBoxMain.Width;
            //this._drawArea.Height = picBoxMain.Height;

            GetParent().ViewerAnnotCtrl.Width = newWidth;
            GetParent().ViewerAnnotCtrl.Height = newHeight;

            _zoomValue = ZoomFactor;
            CheckOverlay();

            //GetParent().DrawArea.Parent.Width = picBoxMain.Width;
            //GetParent().DrawArea.Parent.Height = picBoxMain.Height;
            GetParent().ApplyAnnotationZoom();
		}

        private void SetupCBWindow()
        {
            if (_mouseCBMode)
            {
                this.opHelper.SetupOnlineBmp();
                
                ResizePicBoxAdjustments();

                Bitmap b = (Bitmap)this.opHelper.OnlineBitmap.Clone();
                this.opHelper.PerformCBOps(b, _CBContrast, _CBBrightness);
                picBoxAdjustments.Image = b;

                picBoxAdjustments.BringToFront();
                btnDone.Visible = true;
                btnDone.BackColor = GetParent().GetColorConfig().RibbonGroupBackColor;
                btnDone.BringToFront();
                picBoxAdjustments.Show();
                picBoxMain.Hide();
            }
            else
            {
                picBoxAdjustments.Hide();
                btnDone.Visible = false;
                picBoxMain.Show();
            }
        }

        private void ResizePicBoxAdjustments()
        {
            int w = GetParent().pnlCenter.Width;
            int h = GetParent().pnlCenter.Height;
            Bitmap onlineBitmap = this.opHelper.OnlineBitmap;

            if (w > h)
            {
                double factor = (double)onlineBitmap.Width / (double)onlineBitmap.Height;
                w = (int)(h * factor);
            }
            else
            {
                double factor = (double)onlineBitmap.Height / (double)onlineBitmap.Width;
                h = (int)(w * factor);
            }

            picBoxAdjustments.Left = 0;
            picBoxAdjustments.Top = 0;

            picBoxAdjustments.Width = w;
            picBoxAdjustments.Height = h;
            picBoxAdjustments.SizeMode = PictureBoxSizeMode.StretchImage;
        }

		public void Zoom100()
		{
            if (picBoxMain.Image == null)
                return;

            this.Zoom = 1;
		}

		public void Close()
		{
			this.txtBoxMain.Text = "";
		
			this._headerData = null;
			this._headerName = "";

			this._imageData = null;
			this._imageName = "";

			if( this.Image != null )
			{				
				this.Image.Dispose();
				this.Image = null;				
			}

            if (zoomBoxBitmap != null)
            {
                zoomBoxBitmap.Dispose();
                zoomBoxBitmap = null;
            }

            //if (this.Images != null)
            //{
            //    foreach (Image x in this.Images)
            //    {
            //        x.Dispose();
            //    }
            //    this.Images.Clear();
            //}
			
			if( this.opHelper!= null )
				this.opHelper.Dispose();

            GetParent().ViewerAnnotCtrl.Width = 1;
            GetParent().ViewerAnnotCtrl.Height = 1;
		}

		public void LoadPPMImage( string Path )
		{	
			

			//DisposePicBoxImage();
			// create new bitmap from path
			PPMImage ppm = new PPMImage();
			ppm.LoadImage( Path );
			
			Bitmap loadThis = (Bitmap)ppm.Images[0];
			SetImageData( ppm );		
			 
			LoadImageHelper( ppm.Images );
			
			this.filePath = Path;

			this.txtBoxMain.AppendText( "Portable Pixmap (PPM) Format" + System.Environment.NewLine );
			this.txtBoxMain.AppendText( "Width: " + loadThis.Width + " Height: " + loadThis.Height  + System.Environment.NewLine   );
			this.txtBoxMain.AppendText( "Resolution (DPI): " + loadThis.VerticalResolution +  System.Environment.NewLine   );
            FireFileLoaded();
		}


		public void LoadDICOMImage( string Path )
		{	
			//DisposePicBoxImage();
			// create new bitmap from path
			DicomReader dcm = new DicomReader();
            dcm.DicomSliceLoad += new DicomSliceLoadEventHandler(dcm_DicomSliceLoad);
            dcm.DicomLoad += new DicomLoadEventHandler(dcm_DicomLoad);

            dcm.ReadDicom(Path);            
			SetImageData( dcm );

            //if (this.GetParent().MultiPaneVisible && this.GetParent().LoadMultiFrameInMultiPane)
            //{
            //    ;//Dont do any thing
            //}
            //else
            //{
                this.LoadImageHelper(dcm.Images);
            //}
			this.filePath = Path;
			customDPI = dcm.getDicomHeaderReader().ImagerPixelSpacing;

			this.txtBoxMain.AppendText( dcm.Header );
            //FireFileLoaded();            
		}

        void dcm_DicomLoad(object sender, DicomLoadEventArgs e)
        {
            this.FireDicomLoad(e);
        }

        void dcm_DicomSliceLoad(object sender, DicomSliceLoadEventArgs e)
        {
            this.FireSliceLoaded(e);
            System.Windows.Forms.Application.DoEvents();

            //if (e.Idx == 0)
            //{
            //    LoadSliceHelper((Bitmap)e.Thumbnail.Clone());
            //}
        }

		public void LoadAnalyzeImage( string Path )
		{	
			
			//DisposePicBoxImage();
			// create new bitmap from path
			//RAWImage.ColorOrder.
			//RAWImage.Format.Interleaved
			AnalyzeImage analyze = new AnalyzeImage();
			analyze.LoadImage( Path );

			SetImageData( analyze );
			 
			LoadImageHelper( analyze.Images );
			this.filePath = Path;

			this.txtBoxMain.AppendText( analyze.Header );

            FireFileLoaded();
		}

		private void SetImageData( ImageBase obj )
		{
			this._headerData = obj.HeaderData;
			this._headerName = obj.HeaderName;

			this._imageData = obj.ImageData;
			this._imageName = obj.ImageName;
		}

		public void LoadInterImage( string Path )
		{	
			
			//DisposePicBoxImage();
			// create new bitmap from path
			//RAWImage.ColorOrder.
			//RAWImage.Format.Interleaved
			InterImage inter = new InterImage();
			inter.LoadImage( Path );

			SetImageData( inter );
			 
			LoadImageHelper( inter.Images );
			this.filePath = Path;

			this.txtBoxMain.AppendText( inter.Header );
            FireFileLoaded();
		}

        //public void LoadECATImage( string Path )
        //{	
			
        //    //DisposePicBoxImage();
        //    // create new bitmap from path
        //    //RAWImage.ColorOrder.
        //    //RAWImage.Format.Interleaved
        //    //EcatImageReader ecat = new EcatImageReader();
        //    //Bitmap img =(Bitmap)ecat.read( Path, 0 );
			 
        //    //LoadImageHelper( img );
        //}

		public void LoadRawImage( string Path,int BitsPerPixel, int Offset, int Width, int Height, int NumberOfImages , RAWImage.Format format)
		{	
			
			//DisposePicBoxImage();
			// create new bitmap from path			
			RAWImage raw = new RAWImage();
			raw.LoadImage( Path, BitsPerPixel, Offset,Width,Height,NumberOfImages,format );
			 
			LoadImageHelper( raw.Images );
			this.filePath = Path;
            FireFileLoaded();
		}

		public void LoadImage( string Path )
		{	
			
			//DisposePicBoxImage();

			// create new bitmap from path	
			BinaryReader reader = new BinaryReader( File.Open( Path, FileMode.Open, FileAccess.Read,FileShare.Read) );
			this._imageData = reader.ReadBytes( (int)reader.BaseStream.Length );
			reader.Close();			
			
			Bitmap loadThis = new Bitmap( Path );			
						 
			LoadImageHelper( loadThis );
			this.filePath = Path;
			
			this.txtBoxMain.AppendText( GetBitmapFormat (loadThis) + " Format" + System.Environment.NewLine );
			this.txtBoxMain.AppendText( "Width: " + loadThis.Width + " Height: " + loadThis.Height  + System.Environment.NewLine   );
			this.txtBoxMain.AppendText( "Resolution (DPI): " + loadThis.VerticalResolution +  System.Environment.NewLine   );
            FireFileLoaded();
		}

		private string GetBitmapFormat( Bitmap b )
		{
			ImageFormat f = b.RawFormat;

			if( f.Equals(ImageFormat.Bmp) )
				return "Bitmap";
			else if( f.Equals(ImageFormat.Gif ) )
				return "Graphics Interchange Format (GIF)";
			else if( f.Equals(ImageFormat.Jpeg ) )
				return "JPEG";
			else if( f.Equals(ImageFormat.Png ) )
				return "Portable Network Graphics (PNG)";
			else if( f.Equals(ImageFormat.Tiff ) )
				return "Tag Image File Format (TIFF)";

			return "";
		}

		internal void LoadImageHelper( Image loadThis )
		{
            ImageArray newLoadThis = new ImageArray();
            newLoadThis.Add(loadThis);
            LoadImageHelper(newLoadThis);            
		}

        internal void LoadSliceHelper(Bitmap loadThis)
        {
            //if (images != null)
            //{
            //    foreach (Image img in images)
            //    {
            //        img.Dispose();
            //    }

            //    images.Clear();
            //    images = null;
            //}

            //images = new ImageArray();
            //images.Add(loadThis);

            //if( originalBitmap != null )
            //{
            //    originalBitmap.Dispose();
            //}

            //originalBitmap = (Bitmap) loadThis.Clone();

            // Load new Image
            picBoxMain.Image = loadThis;

            // Make the size of the picturebox equal to that of the image
            //SetPicBoxSize();
            //this.ZoomFit = true;
        }        


		internal void LoadImageHelper( ImageArray loadThis )
		{
            //if( images != null )
            //{
            //    foreach( Image img in images )
            //    {
            //        img.Dispose();
            //    }
				
            //    images.Clear();
            //    images = null;
            //}

            //images = loadThis;

            if (opHelper != null)
                opHelper.Dispose();

            opHelper = new OpHelper(loadThis, this);
            originalBitmapArray = loadThis;
            

            if (this.InvokeRequired)
            {
                // Load new Image                
                //picBoxMain.Image = images[0];
                picBoxMain.Image = opHelper.GetOperatedBitmap(opHelper.SelectedIndex);
                //originalBitmap = (Bitmap)picBoxMain.Image;
                //currentBitmap = (Bitmap)picBoxMain.Image.Clone();

                // Make the size of the picturebox equal to that of the image
                this.Invoke(new SimpleDelegate(SetPicBoxSize));
            }
            else
            {
                // Load new Image
                //picBoxMain.Image = images[0];
                picBoxMain.Image = opHelper.GetOperatedBitmap(opHelper.SelectedIndex);
                //originalBitmap = (Bitmap)picBoxMain.Image;
                //currentBitmap = (Bitmap)picBoxMain.Image.Clone();

                // Make the size of the picturebox equal to that of the image
                SetPicBoxSize();
            }

            opHelper.SliceProccesed+=new SliceProcessedEventHandler(opHelper_SliceProccesed);
            
		}

        void opHelper_SliceProccesed(object sender, SliceEventArgs e)
        {            
            if( this.progressBar1.Value < progressBar1.Maximum )
                this.progressBar1.Value++;

            ribbonProgress.Refresh();
            Application.DoEvents();
        }

        /*public void MultiPaneLoadHelper(Image loadThis)
        {
            //Load new Image
            picBoxMain.Image = loadThis;
            originalBitmap = (Bitmap)picBoxMain.Image;
            currentBitmap = (Bitmap)picBoxMain.Image.Clone();

            // Make the size of the picturebox equal to that of the image
            SetPicBoxSize();
        }*/

		internal void MoveToImage(int index)
		{            
            if (this.Images == null)
                return;

            if( !(GetParent().LoadMultiFrameInMultiPane && GetParent().MultiPaneVisible ) && this.Images.Count > 1)
                ProcessAllFrames();

            opHelper.SelectedIndex = index;			
		}

		public int NextImage()
		{
            if (this.Images == null)
                return 0;

            if( opHelper.SelectedIndex + 1 >= this.Images.Count )
                return opHelper.SelectedIndex;

            opHelper.SelectedIndex = opHelper.SelectedIndex + 1;
            return opHelper.SelectedIndex;
		}

        public int FirstImage()
        {
            if (this.Images == null)
                return 0;

            opHelper.SelectedIndex = 0;
            return opHelper.SelectedIndex;
        }

        public int LastImage()
        {
            if (this.Images == null)
                return 0;

            opHelper.SelectedIndex = this.Images.Count - 1;
            return opHelper.SelectedIndex;
        }

		public int PrevImage()
		{
            if (this.Images == null)
                return 0;


            if (opHelper.SelectedIndex - 1 < 0)
                return opHelper.SelectedIndex;

            opHelper.SelectedIndex = opHelper.SelectedIndex - 1;

            return opHelper.SelectedIndex;
		}

        //private void MoveImageHelper( ImageArray loadThis, int index )
        //{
        //    //// Load new Image
        //    //picBoxMain.Image = loadThis[index];
        //    //originalBitmap =(Bitmap)picBoxMain.Image;

        //    //// Make the size of the picturebox equal to that of the image
        //    //SetPicBoxSize();
        //    //currentImageIndex = index;
        //    ////opHelper.SelectedIndex = currentImageIndex;

        //    if (this.Images == null)
        //        return;

        //    opHelper.SelectedIndex = this._selectedIndex;            
        //}

		public void CopyToClipBoard()
		{
            try
            {
                if (opHelper != null && opHelper.GetOperatedBitmap(opHelper.SelectedIndex) != null)
                    Clipboard.SetDataObject(opHelper.GetOperatedBitmap(opHelper.SelectedIndex), true);
            }catch{}
		}

		public void Smooth2()
		{
            if (picBoxMain.Image == null)
                return;			

            Bitmap newBitmap = null; Filters.Smooth((Bitmap)picBoxMain.Image, 1);
			
			if( newBitmap != picBoxMain.Image)
			{
				//Dispose original Image
				picBoxMain.Image.Dispose();
			}

			picBoxMain.Image = newBitmap;
			CheckOverlay();
		}

        private void Process()
        {
            if (opHelper != null)
            {
                opHelper.PerformOperations(true, OpHelper.MultiFrameThreadDirection.None);
            }            
        }

        private void ProcessTemporary()
        {
            if (opHelper != null)
            {
                //opHelper.PerformCBOps(true, OpHelper.MultiFrameThreadDirection.None);
            }
        }

        private void ProcessForward()
        {
            if (opHelper != null)
            {
                opHelper.PerformOperations(false,OpHelper.MultiFrameThreadDirection.Forward);
            }
        }

        private void ProcessBackword()
        {
            if (opHelper != null)
            {
                opHelper.PerformOperations(false, OpHelper.MultiFrameThreadDirection.Backward);
            }
        }

        internal void ProcessAllFrames()
        {
            if (opHelper.DoAllSliceOP == false)
                return;

            try
            {
                this.progressBar1.ForeColor = GetParent().GetColorConfig().ProgressBarColor;
                this.ribbonProgress.Visible = true;

                this.progressBar1.Value = 0;
                this.progressBar1.Maximum = this.Images.Count;

                try
                {
                    ProcessForward();
                }
                catch { }
                try
                {
                    ProcessBackword();
                }
                catch { }
            }
            catch { }
            finally
            {
                this.ribbonProgress.Visible = false;
            }
            //return RunMulitFrameProcessingThread();
        }

        private void RunProcessingThread()
        {
            Process();
            //while (opHelper.Working)
            //{
            //    opHelper.Stop = true;
            //    Thread.Sleep(100);
            //}

            //if (t != null)
            //    t.Abort();

            //ThreadStart ts = new ThreadStart(Process);
            //t = new Thread(ts);
            //t.Start();            
        }

        private Thread[] RunMulitFrameProcessingThread()
        {
            Thread []threads = new Thread[2];

            ThreadStart ts = new ThreadStart(ProcessForward);
            Thread t1 = new Thread(ts);
            t1.Priority = ThreadPriority.AboveNormal;
            t1.Start();            


            ThreadStart ts2 = new ThreadStart(ProcessBackword);
            Thread t2 = new Thread(ts2);
            t2.Priority = ThreadPriority.BelowNormal;
            t2.Start();

            threads[0] = t1;
            threads[1] = t2;

            return threads;
        }

        public void Smooth()
        {   
            if (opHelper != null)
            {
                opHelper.AddSmooth(1);
                RunProcessingThread();
            }

            CheckOverlay();
        }

		public void DoBrightness(float brightness)
		{
            if (opHelper != null)
            {
                opHelper.AddBrightness(brightness);
                if (opHelper.OnlineMode)
                    ProcessTemporary();
                else
                    Process();
            }
            CheckOverlay();            
		}		

		public void DoContrast( float nContrast )
		{
            if (opHelper != null)
            {
                opHelper.AddContrast(nContrast);
                if (opHelper.OnlineMode)
                    ProcessTemporary();
                else
                    Process();
            }
            CheckOverlay();
		}


        public void ApplyLUT(string lutName)
        {
            if (picBoxMain.Image == null)
                return;

            if (opHelper != null)
            {
                opHelper.AddLUT(lutName);
                //opWorker.RunWorkerAsync();
                RunProcessingThread();
            }
            CheckOverlay();
        }

        //public void ReadAndApplyLUT( string fileName )
        //{
        //    if (picBoxMain.Image == null)
        //        return;			
			
        //    if( fileName.ToUpper() == "NONE")
        //    {
        //        ClearLUT();
        //        return;
        //    }
        //    //DisposePicBoxImage();

        //    string resourceName = "Genetibase.NuGenMediImage" + ".LUTs." + fileName + ".LUT";
        //    Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
        //    //Stream stream = File.Open( fileName, FileMode.Open, FileAccess.Read );

        //    Bitmap newBitmap = Filters.ReadAndApplyLUT( stream, originalBitmap );
        //    picBoxMain.Image = newBitmap;

        //    if( tempOriginalBitmap == null )
        //        tempOriginalBitmap = originalBitmap;

        //    originalBitmap = newBitmap;
			
        //    _lutApplied = true;
        //    CheckOverlay();
        //}



		public void ClearLUT()
		{
            if (picBoxMain.Image == null)
                return;

            if (opHelper != null)
            {
                opHelper.ClearLUT();
                //opWorker.RunWorkerAsync();
                RunProcessingThread();
            }
            CheckOverlay();
		}

		#endregion

        public void ZoomFitForMultiPane()
        {
            this.ZoomFit = true;
            //return;
            //if (picBoxMain.Image == null)
            //    return;

            //this.picBoxMain.Dock = DockStyle.Fill;            
            //this.picBoxMain.SizeMode = PictureBoxSizeMode.Zoom;            
        }
	
		internal void PerformZoomFit()
        {
            if (picBoxMain.Image == null)
                return;

            int oldWidth = 0;
            int oldHeight = 0;
			// Get the old dimensions
            if (opHelper == null || opHelper.GetOperatedBitmap(opHelper.SelectedIndex) == null)
            {
                oldWidth = picBoxMain.Image.Width;
                oldHeight = picBoxMain.Image.Height;
            }
            else
            {
                oldWidth = opHelper.GetOperatedBitmap(opHelper.SelectedIndex).Width;
                oldHeight = opHelper.GetOperatedBitmap(opHelper.SelectedIndex).Height;
            }
			

			double factor = 1;			

			// If the ratio of target height to target width is greater the original image's ratio

            Control x = this.Parent.Parent.Parent;

			if ( (double)x.Height / (double)x.Width >  (double)oldHeight / (double)oldWidth )
			{
				factor = (double)x.Width / (double)oldWidth;
			}
			else
			{
				factor = (double)x.Height / (double)oldHeight;
			}

            DoZoom(factor);
            //this.CenterImage();           
            
            

			// Get the new dimensions by multiplying by the ZoomFactor
			/*int newWidth = (int)(oldWidth * factor);
			int newHeight = (int)(oldHeight * factor);
		
			this.picBoxMain.Width = newWidth;
			this.picBoxMain.Height = newHeight;

			this.picBoxMain.SizeMode = PictureBoxSizeMode.StretchImage;*/
		}

		public void ShowMeasurement(int x, int y)
        {            
			picBoxMain.Refresh();
			Graphics g  = picBoxMain.CreateGraphics();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality ;
			
			int cX = (x - mouseDownStartX) / 2  + mouseDownStartX - 15;
			int cY = (y - mouseDownStartY) /2 + mouseDownStartY - 10;
			
			g.DrawLine(Pens.White, mouseDownStartX, mouseDownStartY, x, y );

			double dist = (double) Math.Sqrt( ( mouseDownStartX - x ) * ( mouseDownStartX - x ) + ( mouseDownStartY - y ) * ( mouseDownStartY - y ) );            

			string Unit = " \"";
			
			
			if( customDPI > 0 ){//&& this.nuGenMediImage.FileType == FileType.DICOM ){
				// this is in MM
				dist = dist * customDPI;
				// Convert to Inches
				dist = dist / 2.54 / 10;
			}
			else{
				// Convert to Inches
				dist = dist / g.DpiX;
			}

			if( mUNits == MeasurementUnits.CM )
			{
				dist *= 2.54;
				Unit = " cm";
			}
			else if( mUNits == MeasurementUnits.MM )
			{
				dist *= 2.54 * 10;
				Unit = " mm";
			}
			else if( mUNits == MeasurementUnits.NM )
			{
				dist *= 2.54 * 1000;
				Unit = " nm";
			}

            dist /= Zoom;
			dist = Math.Round( dist, 2 );

			string write  = dist.ToString() + Unit;
			Font f = new Font(FontFamily.GenericSansSerif,10);
			SizeF size = g.MeasureString( write, f );
			
												
			g.FillRectangle( Brushes.Black, cX, cY , size.Width , 15 );
			g.DrawString( write , f , Brushes.White, cX , cY );			
			g.Dispose();
			g = null;			
			CheckOverlay();
		}

		
		/*public void ShowZoomBoxUnderMouse1( int x, int y )
		{		
			
			Image oldImage = picBoxZoom.Image;

			Point p = this.PointToScreen( new Point( x, y ) );

			Console.WriteLine("Trans " + p.X + " " + p.Y );
				
			Bitmap rectUnderMouse = GetBoxUnderMouse.CaptureScreen(p.X, p.Y, DragBoxWidth, DragBoxHeight);
			
			Console.WriteLine("Called");

			picBoxZoom.Width = rectUnderMouse.Width * 2;
			picBoxZoom.Height = rectUnderMouse.Height * 2;

			picBoxZoom.Location = new Point(x,y);
			picBoxZoom.Image = rectUnderMouse;
			picBoxZoom.Visible = true;
			picBoxZoom.Refresh();
			
			
			// Dispose the old Image
			//rectUnderMouse.Dispose();
			//rectUnderMouse = null;		

			if( oldImage != null )
				oldImage.Dispose();

			oldImage = null;
			CheckOverlay();
		}
*/
		private void picBoxMain_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
            if (annotationMode)
            {
                this.OnMouseDown(e);
                return;
            }

			MouseIsDown = true;

			MLeft = false;
			MRight = false;
			
			if ( e.Button == MouseButtons.Right )
			{
				MLeft = true;				
			}
			else if( e.Button == MouseButtons.Left )
				MRight = true;
			

			mouseDownStartX = e.X;
			mouseDownStartY = e.Y;            

            if (MouseIsDown && MLeft)
            {
                oldCursor = this.Cursor;
                this.Cursor = Cursors.Cross;
            }

            this.OnMouseClick(e);
            

		}

		private void picBoxMain_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
            if (annotationMode)
            {
                this.OnMouseMove(e);
                return;
            }

            
            if( MouseIsDown && MLeft )
			{
				ShowMeasurement( e.X, e.Y );
			}

			else if( MouseIsDown && MRight && MShift )
			{				
				int newX,newY;

                int diffX = (e.X + picBoxZoom.Size.Width / 2 /*+ vScrollBar1.Width*/) - this.picBoxMain.Width;
                int diffY = (e.Y + picBoxZoom.Size.Height / 2 /*+ hScrollBar1.Height*/) - this.picBoxMain.Height;

				if( diffX > 0 && diffY > 0 )
				{
					newX = e.X - diffX;
					newY = e.Y - diffY;
					ShowZoomBox( newX, newY);	
				}
				else if( diffX > 0 )
				{
					newX = e.X - diffX;
					ShowZoomBox( newX, e.Y);	
				}
				else if( diffY > 0 )
				{
					newY = e.Y - diffY;
					ShowZoomBox( e.X, newY);	
				}
				else
				{
					newX = e.X;
					newY = e.Y;

					diffX = e.X - (int)picBoxZoom.Size.Width / 2 ;
					diffY = e.Y - (int)picBoxZoom.Size.Height / 2 ;					

					if( diffX < 0 )
					{
						newX = (int)picBoxZoom.Size.Width / 2;
					}
					if( diffY < 0 )
					{
						newY = (int)picBoxZoom.Size.Height / 2;
					}

					ShowZoomBox( newX, newY);			
				}
			}
			
			else if( MouseIsDown && MRight )
			{
				//old brightness contrast code
                //float contrast = 1 + (float)(e.X - mouseDownStartX) / 200;
                //float brightness = (float)(mouseDownStartY - e.Y) / 200;

                //if (brightness < -1)
                //    brightness = -1;
                //else if (brightness > 1)
                //    brightness = 1;

                //if (contrast < 0)
                //    contrast = 0;
                //else if (contrast > 2)
                //    contrast = 2;

                //NuGenMediImageCtrl parent = null;

                //try
                //{
                //    parent = GetParent();

                //    if (opHelper != null && opHelper.Processing)
                //        return;

                //    parent.Brightness = brightness;
                //    parent.Contrast = contrast;
                //}
                //catch { }
			}

			
		}

        private NuGenMediImageCtrl GetParent()
        {
            NuGenMediImageCtrl parent = null;
            bool notFound = true;
            Control baseCtrl = this;

            do
            {
                if (baseCtrl.Parent == null)
                {
                    notFound = false;
                    break;
                }

                if (baseCtrl.Parent.GetType() == typeof(NuGenMediImageCtrl))
                {
                    parent = (NuGenMediImageCtrl)baseCtrl.Parent;
                    notFound = false;
                    break;
                }

                baseCtrl = baseCtrl.Parent;
            }
            while (notFound);
            return parent;
        }

		private void picBoxMain_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
            if (annotationMode)
            {
                this.OnMouseUp(e);
                return;
            }

			MouseIsDown = false;
			MLeft = false;
			MRight = false;

			picBoxZoom.Visible = false;
			if( picBoxZoom.Image != null )
				picBoxZoom.Image.Dispose();
			picBoxZoom.Image = null;

            if (zoomBoxBitmap != null)
            {
                zoomBoxBitmap.Dispose();
                zoomBoxBitmap = null;
            }

			picBoxMain.Invalidate();

			mouseDownStartX = 0;
			mouseDownStartY = 0;

            if (MouseIsDown && MLeft)
            {
                this.Cursor = oldCursor;
            }
		}

        private void HideHeader()
		{
            textBoxMainPanel.Visible = false;		
			picBoxMain.Visible = true;
			CheckOverlay();
		}

		private void ShowHeader()
		{
			//ZoomFit = false;

            textBoxMainPanel.Visible = true;			
			picBoxMain.Visible = false;
		}

		/// <summary>
		/// Special settings for the picturebox ctrl
		/// </summary>
		private void InitCtrl ()
		{
			//picBoxMain.SizeMode = PictureBoxSizeMode.StretchImage;
			picBoxMain.Location = new Point ( 0, 0 );
			OuterPanel.Dock = DockStyle.Fill;
			OuterPanel.Cursor = System.Windows.Forms.Cursors.NoMove2D;
			//OuterPanel.AutoScroll = true;
			//OuterPanel.MouseEnter += new EventHandler(PicBox_MouseEnter);
			//picBoxMain.MouseEnter += new EventHandler(PicBox_MouseEnter);
			//OuterPanel.MouseWheel += new MouseEventHandler(PicBox_MouseWheel);

			txtBoxMain.Location = new Point ( 0, 0 );			
		}

		private void picBoxMain_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if( _overLayPaint )
			{
				return;
			}

            //if( _overLay )
            //{
            //    OverLayHeader( currentOverlayColor, currentfileName, currentBrightness, currentContrast );
            //}

            //if (annotationMode)
            //{                
                this.OnPaint(e);
            //}
		}

		private void ShowZoomBox( int px, int py )
		{            
            if (picBoxMain.Image == null)
                return;

            if (zoomBoxlastZoom != _zoomValue || zoomBoxBitmap == null)
            {
                zoomBoxlastZoom = _zoomValue;
                // Get the old dimensions
                int oldWidth = picBoxMain.Image.Width;
                int oldHeight = picBoxMain.Image.Height;

                // Get the new dimensions
                int newWidth = (int)(oldWidth * _zoomValue);
                int newHeight = (int)(oldHeight * _zoomValue);

                // Create a new Image based on the new dimensions
                zoomBoxBitmap = new Bitmap(picBoxMain.Image, newWidth, newHeight);

            }
            //else if (zoomBoxBitmap == null)
            //{
            //    zoomBoxBitmap = new Bitmap(picBoxMain.Image);
            //}

            //zoomBoxBitmap = (Bitmap)picBoxMain.Image;

			if( (px + (int)(picBoxZoom.Width / (zoomLevel * 2))) > zoomBoxBitmap.Width )
				return;

			int x = px - (int)(picBoxZoom.Width / (zoomLevel * 2) );
			int y = py - (int)(picBoxZoom.Height / (zoomLevel * 2) );

			//zoomBoxBitmap = (Bitmap)pictureBox1.Image;
			Bitmap newBitmap = new Bitmap( picBoxZoom.Width, picBoxZoom.Height);

			Graphics g = Graphics.FromImage( newBitmap );			
			g.DrawImage( zoomBoxBitmap, new Rectangle( 0,0, picBoxZoom.Width,picBoxZoom.Height) , x, y,(int)picBoxZoom.Width / zoomLevel, (int)picBoxZoom.Height / zoomLevel,GraphicsUnit.Pixel);			

			picBoxZoom.Location = new Point( px - (int)picBoxZoom.Width / 2 + picBoxMain.Left , py - (int)picBoxZoom.Height / 2 + picBoxMain.Top );

			if( picBoxZoom.Image != null )
				picBoxZoom.Image.Dispose();

			picBoxZoom.Image = newBitmap;
			
			picBoxZoom.Show();
			picBoxZoom.Refresh();
			picBoxMain.Refresh();			

			g.Dispose();
			picBoxZoom.Hide();
		}

		public void Rotate(RotateFlipType rotate)
		{
            if (picBoxMain.Image == null)
                return;

            if (opHelper != null)
            {
                opHelper.AddRotate(rotate);
                //opWorker.RunWorkerAsync();
                RunProcessingThread();
            }
            CheckOverlay();

            //if (picBoxMain.Image == null)
            //    return;			
			
            //Bitmap newBitmap = Filters.Rotate( (Bitmap) picBoxMain.Image, rotate );

            //if( tempOriginalBitmap == null )
            //    tempOriginalBitmap = originalBitmap;

            //originalBitmap = newBitmap;
				
            //if( newBitmap != picBoxMain.Image)
            //{
            //    //Dispose original Image
            //    picBoxMain.Image.Dispose();
            //}

            //picBoxMain.Image = newBitmap;
            //CheckOverlay();
		}

		public void DoEmboss(int OffSet)
		{
            if (picBoxMain.Image == null)
                return;

            if (opHelper != null)
            {
                opHelper.AddEmboss(OffSet);
                //opWorker.RunWorkerAsync();
                RunProcessingThread();
            }

            //Bitmap newBitmap = (Bitmap) originalBitmap.Clone();
            //newBitmap = Filters.Emboss( newBitmap, OffSet );
						
            //if( newBitmap != picBoxMain.Image && picBoxMain.Image != originalBitmap)
            //{
            //    //Dispose original Image
            //    picBoxMain.Image.Dispose();
            //}

            //picBoxMain.Image = newBitmap;
            //CheckOverlay();
		}

		public void Flip(RotateFlipType flip)
        {
            if (picBoxMain.Image == null)
                return;

            if (opHelper != null)
            {
                opHelper.AddFlip(flip);
                //opWorker.RunWorkerAsync();
                RunProcessingThread();
            }
            CheckOverlay();


            //if (picBoxMain.Image == null)
            //    return;			
			

            //Bitmap newBitmap = Filters.Flip( (Bitmap)  picBoxMain.Image, flip );
			
            //if( tempOriginalBitmap == null )
            //    tempOriginalBitmap = originalBitmap;

            //originalBitmap = newBitmap;
				
            //if( newBitmap != picBoxMain.Image)
            //{
            //    //Dispose original Image
            //    picBoxMain.Image.Dispose();
            //}

            //picBoxMain.Image = newBitmap;
            //CheckOverlay();

		}

		private void txtBoxMain_MouseDown(object sender, MouseEventArgs e)
        {            
            OnMouseClick(e);
        }

        private void txtBoxMain_MouseEnter(object sender, EventArgs e)
        {
            OnMouseEnter(e);
        }

        private void txtBoxMain_MouseLeave(object sender, EventArgs e)
        {
            OnMouseLeave(e);
        }

        private void OuterPanel_SizeChanged(object sender, EventArgs e)
        {
            int newX = 0;
            int newY = 0;

            newX = OuterPanel.Width / 2 - ribbonProgress.Width / 2;
            newY = OuterPanel.Height / 2 - ribbonProgress.Height / 2;

            this.ribbonProgress.Location = new Point(newX, newY);

            if( GetParent()!=null && this.opHelper!= null && this.opHelper.OnlineBitmap != null )
                ResizePicBoxAdjustments();
        }

        private void picBoxAdjustments_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDownStartX = e.X;
            mouseDownStartY = e.Y;        
            MouseIsDown = true;
        }

        private void picBoxAdjustments_MouseMove(object sender, MouseEventArgs e)
        {
            if (!MouseIsDown)
                return;

            float origC = _CBContrast;
            float origB = _CBBrightness;


            Console.WriteLine("Contrast " + (e.X - mouseDownStartX).ToString());
            Console.WriteLine("Brightness " + (mouseDownStartY - e.Y).ToString());

            _CBContrast = origC + (float)(e.X - mouseDownStartX) / 2000;
            _CBBrightness = origB + (float)(mouseDownStartY - e.Y) / 1000;

            if (_CBBrightness < -1)
                _CBBrightness = -1;
            else if (_CBBrightness > 1)
                _CBBrightness = 1;

            if (_CBContrast < 0)
                _CBContrast = 0;
            else if (_CBContrast > 2)
                _CBContrast = 2;

            Console.WriteLine("Calculated Brightness = " + _CBBrightness);
            Console.WriteLine("Calculated Contrast = " + _CBContrast);
            
            Bitmap b = (Bitmap)this.opHelper.OnlineBitmap.Clone();
            this.opHelper.PerformCBOps(b, _CBContrast, _CBBrightness);
            picBoxAdjustments.Image = b;
            picBoxAdjustments.Refresh();
        }

        private void picBoxAdjustments_MouseUp(object sender, MouseEventArgs e)
        {
            MouseIsDown = false;
        }

        private void picBoxAdjustments_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            //g.DrawString("B:" + (_CBBrightness * 100).ToString(), new Font(FontFamily.GenericSerif, 12), Brushes.Red, 0, 0);
            //g.DrawString("C:" + (_CBContrast  * 100).ToString(), new Font(FontFamily.GenericSerif, 12), Brushes.Red, 0, 20);

        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            GetParent().Brightness = _CBBrightness;
            GetParent().Contrast = _CBContrast;
            GetParent().MouseAdjust = false;

            this.MouseCBMode = false;
        }

        //private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    this.opHelper.Do(true);
        //}
	}
}
