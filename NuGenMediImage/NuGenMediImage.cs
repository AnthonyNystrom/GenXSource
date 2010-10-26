using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Text;
using System.Windows.Forms; 
using Genetibase.NuGenMediImage.Handlers;
using Genetibase.NuGenMediImage.UI.Controls;
using Genetibase.NuGenMediImage.UI.Menus;
using System.Reflection;
using Genetibase.NuGenMediImage.Utility;
using Genetibase.NuGenMediImage.UI.Dialogs;
using System.IO;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel.Design;

namespace Genetibase.NuGenMediImage
{   
    public partial class NuGenMediImageCtrl : UserControl
    {
        #region Internal Designer Variables
        ThumbnailViewer thumbnailProxy;
        SliceViewer sliceProxy;
        TabBar tabBarProxy;
        PhotoMenu startMenu;
        #endregion

        #region Private Variables
        
        private FileType fileType;
        internal HistoryList opHistory = new HistoryList();
        private string fileName = string.Empty;
        private Color currentOverlayColor = Color.White;
        private MeasurementUnits measurementUnits = MeasurementUnits.Inches;
        private int framesPerSecond = 30;

        //private float currentBrightness = 0;
        //private float currentContrast = 1;
        //private double zoom = 1;
        private bool addToHistory = true;

        private bool _showZoomBox = false;
        private StartMenuCollection startMenuCollection;
        //private TabPageCollection tabPageCollection;

        private bool fileLoadPerformed = false;
        private bool _playFrames = false;        

        private bool _showMultiPane = false;
        private bool _loadMultiFrameInMultiPane = false;
        private bool _autoScrollViewer = true;   
        private bool progUpdate = false;
        //private ProgressBarStyle _progressBarStyle = ProgressBarStyle.Continuous;
        internal bool popup = false;

        private NuGenColors colorConfig = new NuGenColors();  
        Genetibase.NuGenAnnotation.DrawArea.DrawToolType lastTool = Genetibase.NuGenAnnotation.DrawArea.DrawToolType.Pointer;

        #region databound variables

        private DataSet _ds;
        private string _dataMemeber;
        private string _imageField;
        private string _headerField;
        private int _rowNumber;

        //private byte[] header = null;
        //private byte[] image = null;

        private const string _sep = " <nuGenSep>";

        private bool autoAnnotationActive = true;		

        #endregion

        #endregion              

        #region Public Events

        public delegate void FileLoadedEventHandler(object source, EventArgs e);
        public event FileLoadedEventHandler FileLoaded;

        public delegate void BrightnessChangedEventHandler(object sender, BrightnessChangedEventArgs e);
        //public event BrightnessChangedEventHandler BrightnessChanged;

        public delegate void ContrastChangedEventHandler(object sender, ContrastChangedEventArgs e);
        //public event ContrastChangedEventHandler ContrastChanged;
        
        #endregion

        #region Event Firing Methods
        
        private void FireFileLoaded()
        {
            if (FileLoaded != null)
                FileLoaded(this, new EventArgs());
        }

        #endregion


        public NuGenMediImageCtrl()
        {
            thumbnailProxy = new ThumbnailViewer(true,false,true,ThumbnailFileType.AllImages);
            sliceProxy = new SliceViewer(true,false);
            tabBarProxy = new TabBar(true,false,true);

            thumbnailProxy.Ctrl = this;
            sliceProxy.Ctrl = this;
            tabBarProxy.Ctrl = this;

            startMenu = new PhotoMenu(this);
            startMenuCollection = new StartMenuCollection();
            startMenuCollection.Init(startMenu);

            InitializeComponent();


            //tabPageCollection = new TabPageCollection();
            //tabPageCollection.Init(this.internalTabBar);

            this.ribbonGroup1.ngMediImage = this;
            this.ribbonGroup2.ngMediImage = this;
            this.ribbonGroup3.ngMediImage = this;
            this.framesRibbonGroup.ngMediImage = this;
            this.ribbonGroup5.ngMediImage = this;
            this.ribbonGroup6.ngMediImage = this;
            this.browseButtonGroup.ngMediImage = this;

            this.thumbnailViewer.NgMediImage = this;
            this.sliceViewer.NgMediImage = this;
            this.viewerPane.NgMediImage = this;

            this.bottomTabBar.NgMediImage = this;

            this.tbAnnotations.NgMediImage = this;
            this.tbContrast.NgMediImage = this;
            this.tbOperations.NgMediImage = this;
            this.tbStart.NgMediImage = this;
            this.tbZoom.NgMediImage = this;
            
            btnAnnotColor.NgMediImage = this;
            btnAnnotFillColor.NgMediImage = this;
            btnBrowse.NgMediImage = this;
            btnCircle.NgMediImage = this;
            btnContrastBrightness.NgMediImage = this;
            btnEmboss.NgMediImage = this;
            btnFlipHoriz.NgMediImage = this;
            btnLine.NgMediImage = this;
            btnLut.NgMediImage = this;
            btnMeasurementUnits.NgMediImage = this;
            btnNext.NgMediImage = this;
            btnOverLay.NgMediImage = this;
            btnPlay.NgMediImage = this;
            btnPrev.NgMediImage = this;
            btnRotateLeft.NgMediImage = this;
            btnRotateRight.NgMediImage = this;
            btnShowHeader.NgMediImage = this;
            btnShowZoomBox.NgMediImage = this;
            btnSmooth.NgMediImage = this;
            btnSuggestedContrast.NgMediImage = this;
            btnZoom100.NgMediImage = this;
            btnZoomBoxSize.NgMediImage = this;
            btnZoomBoxZoomLevel.NgMediImage = this;
            btnZoomFit.NgMediImage = this;
            ribbonButton1.NgMediImage = this;
            ribbonButton2.NgMediImage = this;
            ribbonButton3.NgMediImage = this;
            ribbonButton4.NgMediImage = this;



            this.FileLoaded += new FileLoadedEventHandler(NuGenMediImageCtrl_FileLoaded);
            this.trkZoom.Invalidate();
            this.trkFrame.Invalidate();

            this.picBoxMain.DrawArea.ActiveTool = Genetibase.NuGenAnnotation.DrawArea.DrawToolType.Ellipse;
        }
        
 
        #region Internal Properties
        internal Genetibase.NuGenMediImage.UI.Controls.ImageViewer internalThumbnailViewer
        {
            get
            {
                return this.thumbnailViewer;
            }
        }

        internal Genetibase.NuGenMediImage.UI.Controls.ImageViewerVertical internalSliceViewer
        {
            get
            {
                return this.sliceViewer;
            }
        }

        internal Genetibase.NuGenMediImage.UI.Controls.RibbonControl internalTabBar
        {
            get
            {
                return this.bottomTabBar;
            }
        }

        internal Genetibase.NuGenMediImage.UI.Controls.RibbonGroup internalBrowseButtonGroup
        {
            get
            {
                return this.browseButtonGroup;
            }
        }

        internal Genetibase.NuGenMediImage.UI.Controls.Viewer ImageViewer
        {
            get 
            {
                if (viewerPane.Visible 
                    && viewerPane.Selected != null
                    && !(this.LoadMultiFrameInMultiPane && fileLoadPerformed)
                    )
                    return viewerPane.Selected.Viewer;
                else
                    return picBoxMain.Viewer; 
            }
        }

        internal Genetibase.NuGenMediImage.UI.Controls.ViewerAnnot ViewerAnnotCtrl
        {
            get
            {
                if (viewerPane.Visible && viewerPane.Selected != null)
                    return viewerPane.Selected;
                else
                    return picBoxMain;
            }
        }

        internal Genetibase.NuGenAnnotation.DrawArea DrawArea
        {
            get
            {
                //if (viewerPane.Visible && viewerPane.Selected != null)
                //    return viewerPane.Selected;
                //else
                return picBoxMain.DrawArea;
            }
        }

        #endregion

        #region Public Properties

        #region Public Designer Properties

        [Category("NuGenMediImage"), Editor(typeof(CollectionEditor), typeof(UITypeEditor)),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]        
        public StartMenuCollection StartMenuItems
        {
            get { return startMenuCollection; }
        }


        [Category("NuGenMediImage")]
        public override Color BackColor
        {
            get
            {
                return this.pnlCenter.BackColor;
            }
            set
            {
                base.BackColor = value;
                this.ImageViewer.BackColor = value;
                this.pnlBottom.BackColor = value;
                this.pnlCenter.BackColor = value;
                this.viewerPane.BackColor = value;
            }
        }

        [Category("NuGenMediImage")]
        public Color RibbonGroupBottomColor1
        {
            get 
            {
                return this.GetColorConfig().RibbonGroupBottomColor1;
            }
            set 
            {
                this.GetColorConfig().RibbonGroupBottomColor1 = value;
                Invalidate(true);
            }
        }

        [Category("NuGenMediImage")]
        public Color RibbonGroupBottomColor2
        {
            get
            {
                return this.GetColorConfig().RibbonGroupBottomColor2;
            }
            set
            {
                this.GetColorConfig().RibbonGroupBottomColor2 = value;
                Invalidate(true);
            }
        }

        [Category("NuGenMediImage")]
        public Color RibbonGroupBackColor
        {
            get { return this.GetColorConfig().RibbonGroupBackColor; }
            set 
            {
                this.GetColorConfig().RibbonGroupBackColor = value;
                Invalidate(true);
            }
        }

        [Category("NuGenMediImage")]
        public Color TabBarBackColor
        {
            get { return this.GetColorConfig().TabBarBackColor; }
            set 
            {
                this.GetColorConfig().TabBarBackColor = value;
                Invalidate(true);
            }
        }

        [Category("NuGenMediImage")]
        public Color MultiPaneBackColor
        {
            get { return this.GetColorConfig().MultiPaneBackColor; }
            set
            {
                this.GetColorConfig().MultiPaneBackColor = value;
                this.viewerPane.BackColor = this.GetColorConfig().MultiPaneBackColor;
                Invalidate(true);
            }
        }

        [Category("NuGenMediImage")]
        public Color TabPageColor1
        {
            get { return this.GetColorConfig().TabPageColor1; }
            set 
            {
                this.GetColorConfig().TabPageColor1 = value;
                Invalidate(true);
            }
        }

        [Category("NuGenMediImage")]
        public Color TabPageColor2
        {
            get { return this.GetColorConfig().TabPageColor2; }
            set
            {
                this.GetColorConfig().TabPageColor2 = value;
                Invalidate(true);
            }
        }

        [Category("NuGenMediImage")]
        public Color TabPageColor3
        {
            get { return this.GetColorConfig().TabPageColor3; }
            set
            {
                this.GetColorConfig().TabPageColor3 = value;
                Invalidate(true);
            }
        }

        [Category("NuGenMediImage")]
        public Color TabPageColor4
        {
            get { return this.GetColorConfig().TabPageColor4; }
            set
            {
                this.GetColorConfig().TabPageColor4 = value;
                Invalidate(true);
            }
        }

        [Category("NuGenMediImage")]
        public Color ProgressBarColor
        {
            get { return this.GetColorConfig().ProgressBarColor; }
            set
            {
                this.GetColorConfig().ProgressBarColor = value;
                Invalidate(true);
            }
        }

        //[Category("NuGenMediImage")]       
        //public ProgressBarStyle ProgressBarStyle
        //{
        //    get { return _progressBarStyle; }
        //    set 
        //    { 
        //        _progressBarStyle = value;
        //        this.sliceViewer.ProgressBarStyle = value;
        //        this.thumbnailViewer.ProgressBarStyle = value;
        //    }
        //}       

        [Category("NuGenMediImage")]        
        public ThumbnailViewer ThumbnailViewer
        {
            get
            {
                return thumbnailProxy;
            }
            set
            {
                thumbnailProxy = value;
                thumbnailProxy.Ctrl = this;
                thumbnailProxy.Visible = thumbnailProxy.Visible;
                thumbnailProxy.Collapsed = thumbnailProxy.Collapsed;
                thumbnailProxy.ShowBrowseButton = thumbnailProxy.ShowBrowseButton;
                //thumbnailProxy.Ctrl.thumbnailViewer.Visible = thumbnailProxy.Visible;
                //thumbnailProxy.Ctrl.thumbnailViewer.Collapsed = thumbnailProxy.Collapsed;
                //thumbnailProxy.Ctrl.internalBrowseButtonGroup.Visible = thumbnailProxy.ShowBrowseButton;
            }
        }

        [Category("NuGenMediImage")]
        public SliceViewer SliceViewer
        {
            get
            {
                return sliceProxy;
            }
            set
            {
                sliceProxy = value;
                sliceProxy.Ctrl = this;
                sliceProxy.Ctrl.thumbnailViewer.Visible = thumbnailProxy.Visible;
                sliceProxy.Ctrl.thumbnailViewer.Collapsed = thumbnailProxy.Collapsed;
            }
        }

        [Category("NuGenMediImage")]
        public TabBar TabBar
        {
            get
            {
                return tabBarProxy;
            }
            set
            {
                tabBarProxy = value;
                tabBarProxy.Ctrl = this;
                tabBarProxy.Ctrl.bottomTabBar.Visible = tabBarProxy.Visible;
                tabBarProxy.Ctrl.bottomTabBar.Collapsed = tabBarProxy.Collapsed;
                tabBarProxy.Ctrl.bottomTabBar.ShowStartTab = tabBarProxy.ShowStartTab;                
            }
        }

        [Category("NuGenMediImage"), Editor(typeof(CollectionEditor), typeof(UITypeEditor)),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TabControl.TabPageCollection TabBarPages
        {
            get
            {             
                return internalTabBar.TabPages;
            }
        }

        //[Category("NuGenMediImage"), Editor(typeof(CollectionEditor), typeof(UITypeEditor)),
        //DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        //public TabPageCollection TabPages3
        //{
        //    get
        //    {
        //        return this.tabPageCollection;
        //    }
        //}

        #endregion

        [Category("NuGenMediImage")]
        public bool AnnotationMode
        {
            get
            {
                return this.ImageViewer.AnnotationMode;
            }

            set
            {
                this.ImageViewer.AnnotationMode = value;

                if (!value)
                {
                    this.DrawArea.ActiveTool = Genetibase.NuGenAnnotation.DrawArea.DrawToolType.Pointer;
                }
                else
                {
                    this.DrawArea.ActiveTool = lastTool;
                }
            }
        }

        [Category("NuGenMediImage")]
        public bool AutoAnnotationActive
        {
            get
            {
                return this.autoAnnotationActive;
            }

            set
            {
                this.autoAnnotationActive = value;
            }
        }

        [Category("NuGenMediImage")]        
        public bool ShowThumbnailToolTip
        {
            get { return this.internalThumbnailViewer.ShowToolTip; }
            set { this.internalThumbnailViewer.ShowToolTip = value; }
        }


        [Category("NuGenMediImage")]
        public MeasurementUnits MeasurementUnits
        {
            get { return measurementUnits; }
            set 
            { 
                measurementUnits = value;
                this.ImageViewer.MeasurementUnit = value;
            }
        }

        //[Category("NuGenMediImage")]
        //public System.Windows.Forms.Control.ControlCollection StartMenuItems
        //{
        //    get
        //    {
        //        return this.startMenu.MenuItemControls;
        //    }
        //}

        /*[Category("NuGenMediImage")]
        public Genetibase.NuGenMediImage.UI.Controls.RibbonControl.TabPageCollection TabPages
        {
            get
            {
                return this.bottomTabBar.TabPages;
            }
        }*/

        #region Data Binding Properties

        [Bindable(true), Category("NuGenMediImage")]
		public DataSet DataSource
		{
			get
			{
				return _ds;
			}
        

			set
			{
				_ds = value;
			}
		}

        [Bindable(true), Category("NuGenMediImage")]
		public string DataMemeber
		{
			get
			{
				return _dataMemeber;
			}
        

			set
			{
				_dataMemeber = value;
			}
		}

        [Bindable(true), Category("NuGenMediImage")]
		public string ImageField
		{
			get
			{
				return _imageField;
			}
        

			set
			{
				_imageField = value;
			}
		}

        [Bindable(true), Category("NuGenMediImage")]
		public string HeaderField
		{
			get
			{
				return _headerField;
			}
        

			set
			{
				_headerField = value;
			}
		}

        [Bindable(true), Category("NuGenMediImage")]
		public int RowNumber
		{
			get
			{
				return _rowNumber;
			}
			set
			{
				_rowNumber = value;
			}
        }

        #endregion


        /// <summary>
        /// Get the file name of the loaded file
        /// </summary>  
        [Category("NuGenMediImage")]
        [Browsable(true)]
        public bool AutoScrollViewer
        {
            get
            {
                return _autoScrollViewer;
            }
            set
            {
                _autoScrollViewer = value;
                this.picBoxMain.AutoScroll = value;
                this.viewerPane.MultiPaneAutoScroll = value;
            }
        }

        /// <summary>
        /// Get the file name of the loaded file
        /// </summary>
        [Category("NuGenMediImage")]
        [Browsable(false)]
        public string FileName
        {
            get
            {
                return fileName;
            }
        }

        /// <summary>
        /// Get the file type of the loaded file
        /// </summary>
        [Category("NuGenMediImage")]
        [Browsable(false)]
        public FileType LoadedFileType
        {
            get
            {
                return fileType;
            }
        }

        /// <summary>
        /// The size of the zoom box
        /// </summary>
        [Category("NuGenMediImage")]
        public Size ZoomBoxSize
        {
            get
            {
                return ImageViewer.ZoomBoxSize;
            }
            set
            {
                ImageViewer.ZoomBoxSize = value;
            }
        }

        /// <summary>
        /// The level of zoom that is showed in the Zoom Box
        /// </summary>
        [Category("NuGenMediImage")]
        public int ZoomBoxZoom
        {
            get
            {
                return ImageViewer.ZoomBoxZoom;
            }
            set
            {
                ImageViewer.ZoomBoxZoom = value;
            }
        }

        [Browsable(false)]
        public Image Image
        {
            get
            {
                return ImageViewer.Image;
            }
        }

        [Browsable(false)]
        public ImageArray Images
        {
            get
            {
                return ImageViewer.Images;
            }
        }


        [Browsable(false)]        
        public int FramesPerSecond
        {
            get { return framesPerSecond; }
            set 
            {
                if (value <= 0 || value > 30)
                    throw new ArgumentException();

                framesPerSecond = value; 
            }
        }

        [Category("NuGenMediImage")]
        public bool ZoomFit
        {
            get
            {
                return ImageViewer.ZoomFit;
            }
            set
            {
                ImageViewer.ZoomFit = value;
            }
        }

        [Category("NuGenMediImage")]        
        public double Zoom
        {
            get
            {
                return this.ImageViewer.Zoom;
            }
            set
            {
                if (!progUpdate)
                {
                    this.ImageViewer.Zoom = value;
                    ApplyAnnotationZoom();
                }
            }
        }

        [Category("NuGenMediImage")]
        public bool HeaderVisible
        {
            get
            {
                return ImageViewer.HeaderVisible;
            }
            set
            {
                ImageViewer.HeaderVisible = value;

                if (value)
                    this.picBoxMain.Dock = DockStyle.Fill;
                else
                    this.picBoxMain.Dock = DockStyle.None;
            }
        }

        [Category("NuGenMediImage")]
        public float Brightness
        {
            get
            {
                return ImageViewer.Brightness;
            }
            set
            {
                this.ImageViewer.Brightness = value;
            }
        }

        [Category("NuGenMediImage")]        
        public int Emboss
        {
            get
            {
                return ImageViewer.Emboss;
            }
            set
            {
                this.ImageViewer.Emboss = value;
            }
        }


        [Category("NuGenMediImage")]        
        public float Contrast
        {
            get
            {
                return ImageViewer.Contrast;
            }
            set
            {
                this.ImageViewer.Contrast = value;
            }
        }

        [Category("NuGenMediImage")]
        public Color OverLayColor
        {
            get
            {                
                return currentOverlayColor;
            }
            set
            {
                currentOverlayColor = value;
                //if (currentOverlayColor == Color.Empty || currentOverlayColor == null)
                //    this.ImageViewer.ClearOverlay();
                //else
                //    this.ImageViewer.OverLayHeader(currentOverlayColor, fileName, currentBrightness, currentContrast);
            }            
        }

        [Category("NuGenMediImage")]        
        public bool ShowZoomBox
        {
            get
            {
                return _showZoomBox;
            }
            set
            {
                _showZoomBox = value;
                ImageViewer.MShift = value;

            }
        }

        [Category("NuGenMediImage")]
        public bool MouseAdjust
        {
            get
            {
                return ImageViewer.MouseCBMode;
            }
            set
            {
                if( ImageViewer.MouseCBMode != value )
                    ImageViewer.MouseCBMode = value;

                this.btnCBMouseAdjust.IsPressed = value;
            }
        }


        [Category("NuGenMediImage")]
        public bool LoadMultiFrameInMultiPane
        {
            get { return _loadMultiFrameInMultiPane; }
            set { _loadMultiFrameInMultiPane = value; }
        }

        [Category("NuGenMediImage")]
        public bool MultiPaneVisible
        {
            get
            {
                return _showMultiPane;
            }
            set
            {
                _showMultiPane = value;
                if (_showMultiPane)
                {
                    this.viewerPane.Visible = true;
                    this.viewerPane.PanesVisible = true;
                    this.viewerPane.BringToFront();
                }
                else
                {
                    this.viewerPane.Visible = false;
                    this.viewerPane.PanesVisible = false;
                    this.viewerPane.SendToBack();
                }

                //UpdateUI();
            }
        }

        [Category("NuGenMediImage")]
        public int MultiPaneRows
        {
            get
            {
                return viewerPane.Rows;
            }

            set
            {
                viewerPane.Rows = value;
            }
        }

        [Category("NuGenMediImage")]
        public int MultiPaneColumns
        {
            get
            {
                return viewerPane.Cols;
            }

            set
            {
                viewerPane.Cols = value;
            }
        }

        [Browsable(false)]
        public string Header
        {
            get
            {
                return ImageViewer.Header;
            }
        }
    
        #endregion

        #region Public Methods Related to UI manipulation
        
        public void HideThumbnailViewer()
        {
            pnlTop.Visible = false;
        }

        public void ShowThumbnailViewer()
        {
            pnlTop.Visible = true;
        }

        public void TabBarReset()
        {
            this.internalTabBar.SuspendLayout();
            this.internalTabBar.TabPages.Clear();

            this.internalTabBar.TabPages.Add(tbStart);
            this.internalTabBar.TabPages.Add(tbContrast);
            this.internalTabBar.TabPages.Add(tbZoom);
            this.internalTabBar.TabPages.Add(tbEffects);
            this.internalTabBar.TabPages.Add(tbOperations);
            this.internalTabBar.TabPages.Add(tbAnnotations);

            this.internalTabBar.ResumeLayout();
        }

        #endregion

        #region Public methods

        #region DataBound Methods
        private DataTable ValidateDataSource()
        {
            if (this._ds != null)
            {
                DataTableCollection tables = this._ds.Tables;
                DataTable dt = null;
                if (_dataMemeber.Trim() == string.Empty)
                {
                    if (tables.Count > 0)
                        dt = tables[0];
                }
                else
                {
                    foreach (DataTable tempdt in tables)
                    {
                        if (tempdt.TableName.ToUpper().Equals(_dataMemeber.ToUpper()))
                        {
                            dt = tempdt;
                            return dt;
                        }
                    }
                }

                if (dt == null)
                    throw new DataException("No valid Data Table found to bind with");

                if (!dt.Columns.Contains(_headerField) || dt.Columns.Contains(_imageField))
                    throw new DataException("Specified columns " + _headerField + " or " + _imageField + " not found");

                return dt;
            }
            return null;
        }

        public void DataBind()
        {
            DataTable dt = ValidateDataSource();
            DataRow dr = dt.Rows[_rowNumber];

            byte[] header = (byte[])dr[_headerField];
            byte[] image = (byte[])dr[_imageField];

            MemoryStream m = new MemoryStream(header);
            BinaryReader reader = new BinaryReader(m);

            string info = reader.ReadString();

            FileType fileType;
            string headerName = null;
            string fileName = null;
            string temp = null;
            int idx, idx2;

            idx = info.IndexOf(_sep, 0);
            temp = info.Substring(0, idx).Trim();
            fileType = (FileType)int.Parse(temp);

            try
            {
                idx2 = info.IndexOf(_sep, idx + 1);
                temp = info.Substring(idx + _sep.Length, (idx2) - (idx + _sep.Length)).Trim();
                idx = idx2;
                headerName = temp;

                idx2 = info.IndexOf("\n", idx + 1);
                temp = info.Substring(idx + _sep.Length, (idx2) - (idx + _sep.Length)).Trim();
                fileName = temp;
            }
            catch { }

            if (fileName.Trim() == string.Empty)
                fileName = Guid.NewGuid().ToString();

            BinaryWriter wr = null;

            if (headerName != string.Empty && header != null)
            {
                wr = new BinaryWriter(File.Open(Path.GetTempPath() + "\\" + headerName, FileMode.Create, FileAccess.Write));
                wr.Write(header, info.Length + 1, header.Length - (info.Length + 1));
                wr.Close();
            }

            wr = new BinaryWriter(File.Open(Path.GetTempPath() + "\\" + fileName, FileMode.Create, FileAccess.Write));
            wr.Write(image);
            wr.Close();

            string headerF = Path.GetTempPath() + "\\" + headerName;
            string imageF = Path.GetTempPath() + "\\" + fileName;

            switch (fileType)
            {
                case FileType.Analyze:
                    LoadAnalyzeImage(headerF);
                    break;

                case FileType.DICOM:
                    LoadDICOMImage(imageF);
                    break;

                case FileType.Inter:
                    LoadInterImage(headerF);
                    break;

                case FileType.Normal:
                    LoadImage(imageF);
                    break;

                case FileType.PPM:
                    LoadPPMImage(imageF);
                    break;
            }
        }

        public void SaveData(DataRow dr)
        {
            if (dr.Table.DataSet != _ds)
            {
                throw new ArgumentException("dr must belong to the dataset used as DataSource");
            }

            if (!dr.Table.TableName.ToUpper().Equals(_dataMemeber.ToUpper()))
            {
                throw new ArgumentException("dr must belong to the datatable used in DataSource");
            }

            DataTable dt = ValidateDataSource();

            int columnCount = dt.Columns.Count;

            for (int i = 0; i < columnCount; i++)
            {
                if (dt.Columns[i].ColumnName.ToUpper().Equals(_headerField.ToUpper()))
                {
                    MemoryStream m = new MemoryStream();
                    BinaryWriter wr = new BinaryWriter(m);

                    wr.Write((int)fileType + _sep + this.ImageViewer.HeaderName + _sep + this.ImageViewer.ImageName + "\n");

                    byte[] temp = new byte[m.Length];
                    m.Seek(0, SeekOrigin.Begin);
                    m.Read(temp, 0, temp.Length);

                    wr.Close();
                    m.Close();


                    byte[] newHeaderData = null;

                    if (ImageViewer.HeaderData != null)
                        newHeaderData = new byte[temp.Length + this.ImageViewer.HeaderData.Length];
                    else
                        newHeaderData = new byte[temp.Length];

                    m = new MemoryStream(newHeaderData);
                    wr = new BinaryWriter(m);

                    wr.Write(temp);

                    if (ImageViewer.HeaderData != null)
                        wr.Write(this.ImageViewer.HeaderData);

                    wr.Close();
                    m.Close();

                    dr[i] = newHeaderData;
                }

                else if (dt.Columns[i].ColumnName.ToUpper().Equals(_imageField.ToUpper()))
                {
                    dr[i] = this.ImageViewer.ImageData;
                }
            }
        }

        #endregion

        public void Print()
        {
            DialogResult res = printDialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                printDocument.Print();
            }
        }

        public void MoveToImage(int index)
        {
            if( !_playFrames )
                this.sliceViewer.SelectedIndex = index;
            this.ImageViewer.MoveToImage(index);
        }

        public int NextImage()
        {
            return this.ImageViewer.NextImage();
        }

        public int FirstImage()
        {
            return this.ImageViewer.FirstImage();
        }

        public int LastImage()
        {
            return this.ImageViewer.LastImage();
        }

        public int PrevImage()
        {
            return this.ImageViewer.PrevImage();
        }

        public void CloseFile()
        {
            this.internalSliceViewer.Images = null;
            this.ImageViewer.Close();                        

            try
            {
                this.DrawArea.TheLayers.Clear();
                this.DrawArea.ActiveTool = Genetibase.NuGenAnnotation.DrawArea.DrawToolType.Pointer;
            }
            catch { }

            System.GC.Collect();
        }


        public void LoadImage(string Path)
        {
            try
            {
                this.CloseFile();
                fileLoadPerformed = true;
                LoadAssociatedAnnotations(Path);
                this.ImageViewer.LoadImage(Path);
                fileName = Util.GetFileName(Path);

                fileType = FileType.Normal;
                FireFileLoaded();
            }
            catch { }
            finally
            {
                fileLoadPerformed = false;
            }

        }

        internal void LoadAssociatedAnnotations(string fileName)
        {
            string annoFileName = Path.GetDirectoryName(fileName) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(fileName) + ".anno";

            if (File.Exists(annoFileName))
            {
                this.LoadAnnotation(annoFileName);
            }
        }

        public void LoadAnnotation(string Path)
        {
            try
            {
                using (Stream stream = new FileStream(
                           Path, FileMode.Open, FileAccess.Read))
                {
                    // Deserialize object from text format
                    IFormatter formatter = new BinaryFormatter();
                    
                    this.DrawArea.TheLayers = (NuGenAnnotation.Layers)formatter.Deserialize(stream);
                }
            }
            catch { }
        }

        public void SaveAnnotation(string Path)
        {
            try
            {
                using (Stream stream = new FileStream(
                           Path, FileMode.Create, FileAccess.Write))
                {
                    // Serialize object to text format
                    IFormatter formatter = new BinaryFormatter();

                    formatter.Serialize(stream, this.DrawArea.TheLayers);
                }
            }
            catch { }
        }
         

        public void LoadDICOMImage(string Path)
        {
            try
            {
                this.CloseFile();
                fileLoadPerformed = true;
                LoadAssociatedAnnotations(Path);
                LoadDICOMImageThread(Path);
            }
            catch { }
            finally
            {
                fileLoadPerformed = false;
            }
            //ParameterizedThreadStart ts = new ParameterizedThreadStart(LoadDICOMImageThread);            
            //Thread t = new Thread(ts);            
            //t.Start(Path);
        }

        
        internal void LoadDICOMImageThread(Object path)
        {
            string Path = (string)path;
            this.ImageViewer.DicomSliceLoad += new DicomSliceLoadEventHandler(ImageViewer_DicomSliceLoad);
            this.ImageViewer.DicomLoad += new DicomLoadEventHandler(ImageViewer_DicomLoad);

            try
            {                
                this.ImageViewer.LoadDICOMImage(Path);
            }
            catch { }
            finally
            {
                this.sliceViewer.HideProgress();
            }
            fileName = Util.GetFileName(Path);

            fileType = FileType.DICOM;
            FireFileLoaded();
        }

        void ImageViewer_DicomLoad(object sender, DicomLoadEventArgs e)
        {
            if( e.Idx <= e.Total )
                this.sliceViewer.ShowProgress(e.Total, e.Idx, "Reading     DICOM");
        }

        void ImageViewer_DicomSliceLoad(object sender, DicomSliceLoadEventArgs e)
        {           
            this.sliceViewer.Invoke(new AddSliceDelegate(this.sliceViewer.AddSlice),new object[]{e.Thumbnail,e.Idx,e.Total});
        }

        public void LoadRawImage(string Path, int BitsPerPixel, int Offset, int Width, int Height, int NumberOfImages, RAWImage.Format format)
        {
            try
            {
                this.CloseFile();
                fileLoadPerformed = true;
                LoadAssociatedAnnotations(Path);
                this.ImageViewer.LoadRawImage(Path, BitsPerPixel, Offset, Width, Height, NumberOfImages, format);
                fileName = Util.GetFileName(Path);


                fileType = FileType.RAW;
                FireFileLoaded();
            }
            catch { }
            finally
            {
                fileLoadPerformed = false;
            }
        }

        public void LoadAnalyzeImage(string Path)
        {
            try
            {
                this.CloseFile();
                fileLoadPerformed = true;
                LoadAssociatedAnnotations(Path);
                this.ImageViewer.LoadAnalyzeImage(Path);
                fileName = Util.GetFileName(Path);


                fileType = FileType.Analyze;
                FireFileLoaded();
            }
            catch { }
            finally
            {
                fileLoadPerformed = false;
            }
        }

        public void LoadInterImage(string Path)
        {
            try
            {
                this.CloseFile();
                fileLoadPerformed = true;
                this.ImageViewer.LoadInterImage(Path);
                fileName = Util.GetFileName(Path);


                fileType = FileType.Inter;
                FireFileLoaded();
            }
            catch { }
            finally
            {
                fileLoadPerformed = false;
            }

        }       

        public void LoadPPMImage(string Path)
        {
            try
            {

                this.CloseFile();
                this.fileLoadPerformed = true;
                LoadAssociatedAnnotations(Path);
                this.ImageViewer.LoadPPMImage(Path);
                fileName = Util.GetFileName(Path);


                fileType = FileType.PPM;
                FireFileLoaded();
            }
            catch { }
            finally
            {
                fileLoadPerformed = false;
            }
        }

        public void Save(string fileName, ImageFormat format)
        {
            this.ImageViewer.Image.Save(fileName, format);
        }

        public void Zoom100()
        {
            this.ImageViewer.Zoom100();

            //if (addToHistory)
            //{
            //    History h = new History("Zoom100");
            //    opHistory.Add(h);
            //}
        }

        public void CopyToClipBoard()
        {
            this.ImageViewer.CopyToClipBoard();
        }

        public void Smooth()
        {
            this.ImageViewer.Smooth();

            //if (addToHistory)
            //{
            //    History h = new History("Smooth");
            //    opHistory.Add(h);
            //}
        }

        /*public void BrightnessAndContrast(float nBrightness, float nContrast)
        {
            this.ImageViewer.BrightnessAndContrast(nBrightness, nContrast, true);

            currentBrightness = nBrightness;
            currentContrast = nContrast;

            BrightnessChangedEventArgs e = new BrightnessChangedEventArgs();
            e.brightness = nBrightness;
            if (BrightnessChanged != null)
                BrightnessChanged(this, e);


            ContrastChangedEventArgs e2 = new ContrastChangedEventArgs();
            e2.contrast = nContrast;

            if (ContrastChanged != null)
                ContrastChanged(this, e2);

            if (addToHistory)
            {
                History h = new History("BrightnessAndContrast");
                h.FunctionParameters.Add(nBrightness);
                h.FunctionParameters.Add(nContrast);
                h.FunctionParameters.Add(false);

                if (opHistory.Count > 0)
                {
                    History checkOld = opHistory[opHistory.Count - 1];
                    if (checkOld.FunctionName == "BrightnessAndContrast")
                    {
                        opHistory.Remove(checkOld);
                    }
                }

                opHistory.Add(h);
            }
        }*/

        public void ReadAndApplyLUT(string fileName)
        {
            this.ImageViewer.ApplyLUT(fileName);

            //if (addToHistory)
            //{
            //    History h = new History("ReadAndApplyLUT");
            //    h.FunctionParameters.Add(fileName);
            //    opHistory.Add(h);
            //}
        }

        public void ClearLUT()
        {
            this.ImageViewer.ClearLUT();

            for (int i = 0; i < opHistory.Count; i++)
            {
                History h = opHistory[i];

                if (h.FunctionName == "ReadAndApplyLUT")
                {
                    opHistory.Remove(h);
                    i--;
                }
            }

            //TODO Remove all LUTs
        }        

        public static string[] GetLUTsName()
        {
            return new string[]{
								   "BLACKBDY",
									"bone",
									"CARDIAC",
									"FLOW",
									"GE_color",
									"Gold",
									"HOTIRON",
									"NIH",
									"NIH_fire",
									"NIH_ice",
									"Rainramp",
									"SPECTRUM",
									"Rainramp",
									"X_hot",
									"X_rain",
							   };
        }
        
        public void Rotate(RotateFlipType rotate)
        {
            this.ImageViewer.Rotate(rotate);

            if (addToHistory)
            {
                History h = new History("Rotate");
                h.FunctionParameters.Add(rotate);
                opHistory.Add(h);
            }
        }

        private void PlayFrames()
        {
            if (this.Images == null)
                return;

            int start = this.ImageViewer.SelectedIndex;
            if (start >= (this.Images.Count - 1))
                start = 0;

            int i = 0;

            for (i = start; i < this.Images.Count; i++)
            {
                if (!_playFrames)
                {
                    if (i == this.Images.Count)
                        this.sliceViewer.SelectedIndexFast = i - 1;
                    else
                        this.sliceViewer.SelectedIndexFast = i;
                    return;
                }

                trkFrame.Value = i;                
                this.lblFrame.Text = (trkFrame.Value + 1).ToString() + " of " + this.Images.Count.ToString();                

                this.MoveToImage(i);
                Application.DoEvents();

                if (!_playFrames)
                {
                    if (i == this.Images.Count)
                        this.sliceViewer.SelectedIndexFast = i - 1;
                    else
                        this.sliceViewer.SelectedIndexFast = i;

                    return;
                }
                System.Threading.Thread.Sleep(1000 / framesPerSecond);
            }

            if( i == this.Images.Count )
                this.sliceViewer.SelectedIndexFast = i - 1;
            else
                this.sliceViewer.SelectedIndexFast = i;

            _playFrames = false;            
            this.btnPlay.Text = "Play";
        }

        public void Flip(RotateFlipType flip)
        {
            this.ImageViewer.Flip(flip);

            if (addToHistory)
            {
                History h = new History("Flip");
                h.FunctionParameters.Add(flip);
                opHistory.Add(h);
            }
        }

        #endregion

        public void RedoFromHistory()
        {
            addToHistory = false;

            Type t = typeof(Viewer);
            MemberInfo[] arr = t.FindMembers(MemberTypes.Method, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance, null, null);

            for (int i = 0; i < opHistory.Count; i++)
            {
                History h = opHistory[i];
                t.InvokeMember(h.FunctionName, BindingFlags.DeclaredOnly | BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance, null, this.ImageViewer, h.paramArray);
            }

            addToHistory = true;
        }


        #region Private & Internal Methods       

        internal NuGenColors GetColorConfig()
        {
            return colorConfig;            
        }

        internal void ApplyAnnotationZoom()
        {           
            if (!progUpdate)
            {
                this.picBoxMain.DrawArea.Zoom = (float)Zoom;
            }        
        }
        
        private void UpdateUI()
        {
            //progUpdate = true;
            //this.trkZoom.Value = (int)(this.ImageViewer.Zoom * 100);
            //this.trkFrame.Value = this.ImageViewer.SelectedIndex;

            //this.numericUpDownBrightness.Value = (decimal)(this.ImageViewer.Brightness * 100);
            //this.numericUpDownContrast.Value = (decimal)(this.ImageViewer.Contrast * 100);
            //this.btnZoomFit.IsPressed = this.ImageViewer.ZoomFit;
            //this.btnShowZoomBox.IsPressed = this.ImageViewer.MShift;
            //this.btnShowHeader.IsPressed = this.ImageViewer.HeaderVisible;
            //Application.DoEvents();
            //progUpdate = false;

            progUpdate = true;

            if (bottomTabBar.SelectedTab == tbContrast)
            {
                this.numericUpDownBrightness.Value = (decimal)(this.ImageViewer.Brightness * 100);
                this.numericUpDownContrast.Value = (decimal)(this.ImageViewer.Contrast * 100);
            }
            else if (bottomTabBar.SelectedTab == tbZoom)
            {
                this.trkZoom.Value = (int)(this.ImageViewer.Zoom * 100);
                this.trkZoom.Refresh();
                this.btnZoomFit.IsPressed = this.ImageViewer.ZoomFit;
            }
            else if (bottomTabBar.SelectedTab == tbFrames)
            {
                this.trkFrame.Value = this.ImageViewer.SelectedIndex;
            }
            else if (bottomTabBar.SelectedTab == tbOperations)
            {
                this.btnShowZoomBox.IsPressed = this.ImageViewer.MShift;
                this.btnShowHeader.IsPressed = this.ImageViewer.HeaderVisible;
            }

            Application.DoEvents();
            progUpdate = false;
        }

        #endregion

        #region Event Handlers

        private void bottomTabBar_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.bottomTabBar.Collapsed = !this.bottomTabBar.Collapsed;
        }
        
        void NuGenMediImageCtrl_FileLoaded(object source, EventArgs e)
        {
            if( !this.HeaderVisible )
                this.ZoomFit = true;

            this.sliceViewer.Images = this.Images;            
            HandleMultiFrameImage();
            this.trkFrame.Value = 0;
            this.sliceViewer.SelectedIndex = 0;
            
            if( this.Images != null && this.Images.Count > 1 )
                this.trkFrame.Maximum = this.Images.Count - 1;
        }

        private void HandleMultiFrameImage()
        {
            if (this.Images == null || _loadMultiFrameInMultiPane == false ) // || this.Images.Count < 2)
                return;

            int i = 0;
            int j = 0;
            int k = 0;

            for (i = 0; i < this.viewerPane.Rows; i++)
            {
                for (j = 0; j < this.viewerPane.Cols; j++)
                {
                    
                    Viewer v = this.viewerPane.GetViewer(i, j).Viewer;

                    if (k >= this.Images.Count)
                    {
                        v.Close();
                    }
                    else
                    {
                        v.LoadImageHelper(this.Images);

                        v.MoveToImage(k);
                        v.ZoomFit = true;
                    }

                    k = k + 1;

                    //if (k >= this.Images.Count)
                    //{
                    //    break;
                    //}
                }
            }
        }


        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            if (this.Image != null)
            {
                Image img = this.Image;
                e.Graphics.DrawImage(img, 0, 0, img.Width, img.Height);
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
            internalThumbnailViewer.ThumbnailFileType = this.thumbnailProxy.ThumbnailFileType;
            internalThumbnailViewer.SourceDirectory = dir;

            f.Dispose();
            f = null;
        }        

        private void bottomTabBar_OnPopup(object sender)
        {
            popup = true;
            startMenu.Location = ((RibbonControl)sender).PointToScreen(new Point(0, -startMenu.Height));
            startMenu.Show();
        }

        private void thumbnailViewer_CollapsedChanged(object sender, EventArgs e)
        {
            if (this.internalThumbnailViewer.Collapsed)
            {
                this.internalBrowseButtonGroup.Visible = false;
            }
            else
            {
                if (this.ThumbnailViewer.ShowBrowseButton)
                {
                    this.internalBrowseButtonGroup.Visible = true;
                }
            }
        }

        private void btnContrastBrightness_Click(object sender, EventArgs e)
        {

            Application.DoEvents();
            this.Brightness = 0;
            this.Contrast = 0.85f;
            this.numericUpDownContrast.Value = 85;
            this.numericUpDownBrightness.Value = 0;
            Application.DoEvents();
        }

        private void btnSuggestedContrast_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            this.Brightness = 0;
            this.Contrast = 1;

            this.numericUpDownContrast.Value = 100;
            this.numericUpDownBrightness.Value = 0;
            Application.DoEvents();
        }

        private void numericUpDownContrast_ValueChanged(object sender, EventArgs e)
        {
            Application.DoEvents();
            if( !progUpdate)
                this.Contrast = (float)numericUpDownContrast.Value / 100;
        }


        private void numericUpDownFPS_ValueChanged(object sender, EventArgs e)
        {
            Application.DoEvents();
            if (!progUpdate)
                this.FramesPerSecond = (int)numericUpDownFPS.Value;
        }

        private void numericUpDownBrightness_ValueChanged(object sender, EventArgs e)
        {
            Application.DoEvents();
            if (!progUpdate)            
                this.Brightness = (float)numericUpDownBrightness.Value / 100;
        }

        private void btnZoomFit_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            btnZoomFit.IsPressed = !btnZoomFit.IsPressed;
            this.ZoomFit = btnZoomFit.IsPressed;
        }

        private void btnZoom100_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            btnZoomFit.IsPressed = false;
            this.Zoom100();
        }

        private void trkZoom_Scroll(object sender, EventArgs e)
        {
            Application.DoEvents();
            btnZoomFit.IsPressed = false;

            if (!progUpdate)
            {
                float ZoomFactor = (float)trkZoom.Value / 100.0F;
                if (ZoomFactor == 1.00F)
                    ZoomFactor = 1.01F;

                this.Zoom = ZoomFactor;
            }
        }

        private void btnEmboss_Click(object sender, EventArgs e)
        {
            if (this.ImageViewer.Image == null)
                return;

            Application.DoEvents();
            DlgEmboss dlgEmboss = new DlgEmboss(this);

            int nWidth = this.ImageViewer.Image.Width;
            int nHeight = this.ImageViewer.Image.Height;

            float ratio = nWidth / 128F;

            nWidth = (int)(nWidth / ratio);
            nHeight = (int)(nHeight / ratio);

            Image b = null;

            if (this.ImageViewer.opHelper != null)
            {
                b = this.ImageViewer.opHelper.OriginalBitmap[this.ImageViewer.SelectedIndex];
            }
            else
            {
                b = this.ImageViewer.Image.GetThumbnailImage(nWidth, nHeight, null, IntPtr.Zero);
            }

            dlgEmboss.OriginalImage = b;
            dlgEmboss.EmbossValue = this.ImageViewer.Emboss;

            DialogResult res = dlgEmboss.ShowDialog();

            Application.DoEvents();

            if (res == DialogResult.OK)
            {
                this.ImageViewer.Emboss = dlgEmboss.EmbossValue ; 
            }

            //EmbossSlider frmEmboss = new EmbossSlider();
            //frmEmboss.nuGenMediImage = this;
            //frmEmboss.ShowDialog();
        }

        private void btnSmooth_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            this.Smooth();
        }

        private void btnLut_Click(object sender, EventArgs e)
        {
            LutMenu menu = new LutMenu(this);
            menu.Location = ((RibbonButton)sender).PointToScreen(new Point(0, -menu.Height));
            menu.Show();
        }

        private void btnRotateLeft_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            this.Rotate(RotateFlipType.Rotate270FlipNone);
            this.DrawArea.Rotation = 270;
            this.DrawArea.Invalidate();
            
        }

        private void btnRotateRight_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            this.Rotate(RotateFlipType.Rotate90FlipNone);
            this.DrawArea.Rotation = 90;
            this.DrawArea.Invalidate();
        }

        private void btnFlipHoriz_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            this.Flip(RotateFlipType.RotateNoneFlipX);
        }

        private void btnFlipVer_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            this.Flip(RotateFlipType.RotateNoneFlipY);
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            int index = this.PrevImage();
            this.trkFrame.Value = index;
            this.lblFrame.Text = (trkFrame.Value + 1).ToString() + " of " + this.Images.Count.ToString();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            int index = this.NextImage();
            this.trkFrame.Value = index;
            this.lblFrame.Text = (trkFrame.Value + 1).ToString() + " of " + this.Images.Count.ToString();
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            int index = this.FirstImage();
            this.trkFrame.Value = index;
            this.lblFrame.Text = (trkFrame.Value + 1).ToString() + " of " + this.Images.Count.ToString();
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            int index = this.LastImage();
            this.trkFrame.Value = index;
            this.lblFrame.Text = (trkFrame.Value + 1).ToString() + " of " + this.Images.Count.ToString();
        }  

        private void btnPlay_Click(object sender, EventArgs e)
        {
            _playFrames = !_playFrames;

            Application.DoEvents();

            if (_playFrames)
            {
                btnPlay.Text = "Stop";
                this.PlayFrames();
            }
            else
            {
                btnPlay.Text = "Play";
            }
        }


        private void btnShowHeader_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            btnShowHeader.IsPressed = !btnShowHeader.IsPressed;
            this.HeaderVisible = btnShowHeader.IsPressed;
        }

        private void btnOverLay_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            OverLayMenu menu = new OverLayMenu(this);
            menu.Location = ((RibbonButton)sender).PointToScreen(new Point(0, -menu.Height));
            menu.Show();
        }

        private void btnZoomBoxZoomLevel_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            ZoomBoxMenu menu = new ZoomBoxMenu(this);
            menu.Location = ((RibbonButton)sender).PointToScreen(new Point(0, -menu.Height));
            menu.Show();
        }

        private void btnZoomBoxSize_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            ZoomSizeMenu menu = new ZoomSizeMenu(this);
            menu.Location = ((RibbonButton)sender).PointToScreen(new Point(0, -menu.Height));
            menu.Show();
        }

        private void btnShowZoomBox_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            btnShowZoomBox.IsPressed = !btnShowZoomBox.IsPressed;
            this.ShowZoomBox = btnShowZoomBox.IsPressed;
        }

        private void thumbnailViewer_SelectedIndexChanged(object sender, EventArgs e)
        {
             Application.DoEvents();
            string file = thumbnailViewer.FileName;

            string annoFileName = Path.GetDirectoryName(file) + Path.DirectorySeparatorChar + Path.GetFileNameWithoutExtension(file) + ".anno";

            if (File.Exists(annoFileName))
            {
                this.LoadAnnotation(annoFileName);
            }


            string ext = Path.GetExtension(file).ToUpper();

            if (ext == ".DIC" || ext == ".DCM" || ext == ".DICOM")
            {
            	this.CloseFile();
                this.LoadDICOMImage(file);
            }
            else
            {
            	this.CloseFile();
                this.LoadImage(file);
            }
            Application.DoEvents();
        }

        private void viewerPane_SelectedIndexChanged(object sender, EventArgs e)
        {
            Application.DoEvents();
            UpdateUI();
            this.sliceViewer.Images = viewerPane.Selected.Viewer.Images;
            
            //this.thumbnailViewer.DoSelect( this.ImageViewer.filePath );
            
            Application.DoEvents();
            
        }

        private void sliceViewer_SelectedIndexChanged(object sender, EventArgs e)
        {
            Application.DoEvents();
            int index = sliceViewer.SelectedIndex;
            this.ImageViewer.MoveToImage(index);
            trkFrame.Value = index;
            this.lblFrame.Text = (trkFrame.Value + 1).ToString() + " of " + this.Images.Count.ToString();
            Application.DoEvents();
        }


        private void trkFrame_ValueChanged(object sender, EventArgs e)
        {
            Application.DoEvents();
            try
            {
                this.MoveToImage(trkFrame.Value);
                this.lblFrame.Text = (trkFrame.Value + 1).ToString() + " of " + this.Images.Count.ToString();
            }
            catch { }
            Application.DoEvents();
        }

        private void btnMeasurementUnits_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            MeasurementUnitsMenu menu = new MeasurementUnitsMenu(this);
            menu.Location = ((RibbonButton)sender).PointToScreen(new Point(0, -menu.Height));
            menu.Show();
            Application.DoEvents();
        }

        private void bottomTabBar_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUI();

            if (popup)
            {
                popup = false;
                return;
            }

            if (bottomTabBar.SelectedTab == tbFrames)
            {
                if (this.Images == null)
                    return;

                try
                {
                    this.lblFrame.Text = (trkFrame.Value + 1).ToString() + " of " + this.Images.Count.ToString();
                }
                catch { }

                if (!( this.MultiPaneVisible && this.LoadMultiFrameInMultiPane ) && this.Images.Count > 1)
                    this.ImageViewer.ProcessAllFrames();

                if (this.LoadMultiFrameInMultiPane && this.MultiPaneVisible)
                    this.framesRibbonGroup.Enabled = false;
                else
                    this.framesRibbonGroup.Enabled = true;

            }
            else if (bottomTabBar.SelectedTab == tbAnnotations && this.autoAnnotationActive)
            {
                this.AnnotationMode = true;
            }
            else if( this.autoAnnotationActive )
            {
                this.AnnotationMode = false;
            }
        }

        private void UnSelectAnnotationModes()
        {
            this.btnLine.IsPressed = false;
            this.btnCircle.IsPressed = false;
            this.btnAnnoText.IsPressed = false;
            this.btnAnnotArrow.IsPressed = false;
            this.btnPencil.IsPressed = false;
            this.btnRectangle.IsPressed = false;
            this.btnPolyLine.IsPressed = false;
        }

        private void btnLine_Click(object sender, EventArgs e)
        {
            UnSelectAnnotationModes();
            this.btnLine.IsPressed = true;
            this.DrawArea.ActiveTool = Genetibase.NuGenAnnotation.DrawArea.DrawToolType.Line;
        }

        private void btnCircle_Click(object sender, EventArgs e)
        {
            UnSelectAnnotationModes();
            this.btnCircle.IsPressed = true;
            this.DrawArea.ActiveTool = Genetibase.NuGenAnnotation.DrawArea.DrawToolType.Ellipse;
        }

        private void btnAnnotColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                this.DrawArea.LineColor = colorDialog.Color;                
            }

            colorDialog.Dispose();
            colorDialog = null;
        }

        private void btnAnnotArrow_Click(object sender, EventArgs e)
        {
            UnSelectAnnotationModes();
            this.btnAnnotArrow.IsPressed = true;
            this.DrawArea.ActiveTool = Genetibase.NuGenAnnotation.DrawArea.DrawToolType.Pointer;
        }

        private void btnAnnotFillColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                this.DrawArea.FillColor = colorDialog.Color;
            }

            colorDialog.Dispose();
            colorDialog = null;
        }

        private void btnFill_Click(object sender, EventArgs e)
        {
            btnFill.IsPressed = !btnFill.IsPressed;
            this.DrawArea.DrawFilled = btnFill.IsPressed;
        }

        private void btnPencil_Click(object sender, EventArgs e)
        {
            UnSelectAnnotationModes();
            this.btnPencil.IsPressed = true;
            this.DrawArea.ActiveTool = Genetibase.NuGenAnnotation.DrawArea.DrawToolType.Polygon;
        }

        private void btnRectangle_Click(object sender, EventArgs e)
        {
            UnSelectAnnotationModes();
            this.btnRectangle.IsPressed = true;
            this.DrawArea.ActiveTool = Genetibase.NuGenAnnotation.DrawArea.DrawToolType.Rectangle;
        }

        private void btnPolyLine_Click(object sender, EventArgs e)
        {
            UnSelectAnnotationModes();
            this.btnPolyLine.IsPressed = true;
            this.DrawArea.ActiveTool = Genetibase.NuGenAnnotation.DrawArea.DrawToolType.PolyLine;
        }

        private void btnAnnoText_Click(object sender, EventArgs e)
        {
            UnSelectAnnotationModes();
            this.btnAnnoText.IsPressed = true;
            this.DrawArea.ActiveTool = Genetibase.NuGenAnnotation.DrawArea.DrawToolType.Text;
        }

        private void pnlCenter_SizeChanged(object sender, EventArgs e)
        {
            if (this.ZoomFit)
                this.ImageViewer.PerformZoomFit();
        }

        private void btnCBMouseAdjust_Click(object sender, EventArgs e)
        {
            if (this.ImageViewer.Image == null)
                return;

            btnCBMouseAdjust.IsPressed = !btnCBMouseAdjust.IsPressed;
            this.ImageViewer.MouseCBMode = btnCBMouseAdjust.IsPressed;
        }

        #endregion        

       
    }
}
