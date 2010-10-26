using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices; 
using System.Threading;

namespace Genetibase.NuGenMediImage.UI.Controls
{ 
   
    //[TypeConverter(typeof(ImageViewerConverter))]
    //[Designer(typeof(ImageViewerDesigner))]
    internal partial class ImageViewer : UserControl
    {        
        [DllImport("uxtheme.dll")]
        public static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        string[] standardExts = { "*.jpg", "*.jpeg", "*.gif", "*.bmp", "*.tif", "*.tiff", "*.png", "*.wmf", "*.emf", "*.exif" };
        string[] dicomExts = { "*.DIC","*.DCM","*.DICOM" };
        string[] allExts = null;

        private RibbonItem lastSelected = null;

        private string sourceDirectory = null;
        
        private ThumbnailList thumbNails = null;

        private ThumbnailFileType thumbnailFileType = ThumbnailFileType.CommonImages;

        private NuGenMediImageCtrl ngMediImage = null;

        internal NuGenMediImageCtrl NgMediImage
        {
            get { return ngMediImage; }
            set 
            { 
                ngMediImage = value;
                this.mainRibbonGroup.ngMediImage = ngMediImage;
                this.hiddenRibbenGroup.ngMediImage = ngMediImage;
                this.mainRibbonGroup.BackgroundImage = GenerateImage();
                this.btnLeft.NgMediImage = ngMediImage;
                this.btnRight.NgMediImage = ngMediImage;
                this.buttonDOWN.NgMediImage = ngMediImage;
                this.buttonUP.NgMediImage = ngMediImage;                
                this.progressBar1.ForeColor = ngMediImage.GetColorConfig().ProgressBarColor;
            }
        }
        
        ArrayList files = new ArrayList();
        ArrayList toolTips = new ArrayList();

        string fileName = "";

        int step = 0 ;
        int space = 5;
        
        
        bool mouseDown = false;
        Thread upT = null;
        Thread downT = null;

        
        private int selectedIndex = -1;

        private bool _collapsed = false;
        private bool _showToolTip = true;        

        public ThumbnailFileType ThumbnailFileType
        {
            get { return thumbnailFileType; }
            set { thumbnailFileType = value; }
        }

        internal bool ShowToolTip
        {
            get { return _showToolTip; }
            set 
            { 
                _showToolTip = value;
                toolTip1.Active = _showToolTip;
            }
        }

        public new bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;
                try
                {                    
                    this.Parent.Visible = value;                    
                }
                catch { }
            }
        }

        //public ProgressBarStyle ProgressBarStyle
        //{
        //    get { return this.progressBar1.Style; }
        //    set
        //    {
        //        this.progressBar1.Style = value;
        //    }
        //}

        public bool Collapsed
        {
            get { return _collapsed; }
            set 
            {
                _collapsed = value;
                CollapseHelper();
            }
        }

        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }            
        }

        public string FileName
        {
            get { return fileName; }
        }
		
        private int showingHeight = 0;
        private int hiddenHeight = 18;

        public event CollapsedEventHandler CollapsedChanged;
        public event EventHandler SelectedIndexChanged;        
        
        public string SourceDirectory
        {
            get { return sourceDirectory; }
            set
            {
                try
                {
                    if (sourceDirectory == value)
                        return;

                    sourceDirectory = value;

                    toolTips.Clear();

                    if (sourceDirectory == null)
                    {
                        RemoveThumbnails();
                        return;
                    }

                    fileSystemWatcher1.Path = sourceDirectory;

                    if (sourceDirectory != null && System.IO.Directory.Exists(sourceDirectory))
                    {
                        bool thumbsDBOld = true;
                        bool fileExists = File.Exists(sourceDirectory + Path.DirectorySeparatorChar + "Thumbs.iw");
                        float fileSize = 0;
                        long projectedFileSize = 0;

                        if (fileExists)
                        {
                            try
                            {
                                thumbsDBOld = IsThumbsDBOld();
                            }
                            catch { }

                            try
                            {
                                System.IO.FileInfo f = new System.IO.FileInfo(sourceDirectory + Path.DirectorySeparatorChar + "Thumbs.iw");
                                fileSize = f.Length;
                                projectedFileSize = (files.Count * 10054) + (files.Count * 1024) + (files.Count * 1024);
                            }
                            catch { }
                        }

                        if (thumbsDBOld
                            ||
                            !fileExists
                            ||
                            fileSize != projectedFileSize)
                        {
                            CreateThumbsDB();                            
                        }

                        try
                        {
                            this.SuspendLayout();

                            RemoveThumbnails();
                            LoadFromThumbsDB();
                            LoadThumbnails();                            
                        }
                        catch (Exception ex3) { MessageBox.Show(ex3.ToString()); }
                        finally
                        {
                            this.ResumeLayout();
                        }

                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }

                /*try
                {
                    Directory.Delete(mainForm.TempDirectory, true);
                }
                catch { }*/
            }
        }   
        
        public void DoSelect(string Path)
        {
        	foreach(RibbonItem item in this.innerPanel.Controls)
        	{
        		int fileNum = (int)item.Tag;
                fileName = (string)files[fileNum];
                
        		if( fileName == Path )
        		{
	        		if (lastSelected != null)
	                {
	                    lastSelected.IsPressed = false;
	                }
	
	                lastSelected = item;
	                lastSelected.IsPressed = true;
        		}
        	}
        }

        private void LoadFromThumbsDB()
        {            
            FileStream f = File.Open(this.SourceDirectory + Path.DirectorySeparatorChar + "Thumbs.iw", FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(f);

            // clear old thumbnail images
            if (thumbNails != null)
            {
                foreach (Bitmap b in thumbNails)
                {
                    if (b != null)
                        b.Dispose();
                }

                thumbNails.Clear();
            }

            thumbNails = new ThumbnailList();

            // clear old thumbnail paths
            if (files != null)
                files.Clear();

            while (br.BaseStream.Position != br.BaseStream.Length)
            {
                byte[] image = br.ReadBytes(10054);
                char[] pathC = br.ReadChars(1024);
                char[] toolTipC = br.ReadChars(1024);
                string path = new String(pathC).Trim();
                string toolTip = new String(toolTipC).Trim();

                MemoryStream ms = new MemoryStream(image);
                Bitmap thumb = new Bitmap(ms);
                thumbNails.Add(thumb);
                files.Add(path);
                toolTips.Add(toolTip);

                image = null;
                pathC = null;
                path = null;
            }

            br.Close();
        }

        private void CreateThumbsDB()
        {
            try
            {
                RemoveThumbnails();
                Application.DoEvents();
                this.Invalidate();
                File.Delete(this.sourceDirectory + Path.DirectorySeparatorChar + "Thumbs.iw");
                FileStream f = File.Open(this.sourceDirectory + Path.DirectorySeparatorChar + "Thumbs.iw", FileMode.OpenOrCreate, FileAccess.Write);

                string[] tfiles = null;


                files.Clear();

                string[] exts = null;

                if (thumbnailFileType == ThumbnailFileType.CommonImages)
                {
                    exts = standardExts;
                }
                else if (thumbnailFileType == ThumbnailFileType.DICOM)
                {
                    exts = dicomExts;
                }
                else if (thumbnailFileType == ThumbnailFileType.AllImages)
                {
                    exts = allExts;                    
                }

                foreach (string ext in exts)
                {
                    tfiles = Directory.GetFiles(this.sourceDirectory, ext);

                    foreach (string file in tfiles)
                    {
                        files.Add(file);
                    }
                }

                files.Sort();

                this.Invoke(new ShowProgressCallback(this.ShowProgress),
                    new object[] { files.Count, 0, "Creating/Updating thumbnails" });

                int count = 0;

                Genetibase.NuGenMediImage.Handlers.DicomReader dcm = null;

                foreach (string file in files)
                {
                    Bitmap b = null;                    

                    try
                    {
                        string ext = Path.GetExtension(file).ToUpper();

                        if (ext == ".DIC" || ext == ".DCM" || ext == ".DICOM")
                        {
                            dcm = new Genetibase.NuGenMediImage.Handlers.DicomReader(file,true);
                            b = (Bitmap)dcm.Images[0];

                            toolTips.Add(dcm.CompactHeader);
                        }
                        else
                        {
                            b = new Bitmap(file);
                            toolTips.Add(file);
                        }
                    }
                    catch (Exception)
                    {
                        b = (Bitmap)picBoxClone.ErrorImage.Clone();
                        toolTips.Add(file);
                    }

                    Image thumb = b.GetThumbnailImage(50, 50, null, IntPtr.Zero);
                    MemoryStream ms = new MemoryStream();
                    thumb.Save(ms, ImageFormat.Bmp);

                    byte[] image = ms.ToArray();
                    f.Write(image, 0, image.Length);

                    string file1024 = file + new String(' ', 1024 - file.Length);
                    byte[] file1024b = System.Text.ASCIIEncoding.ASCII.GetBytes(file1024);
                    f.Write(file1024b, 0, file1024b.Length);

                    string toolTip1024 = (string)toolTips[count] + new String(' ', 1024 - ((string)toolTips[count]).Length);
                    byte[] toolTip1024b = System.Text.ASCIIEncoding.ASCII.GetBytes(toolTip1024);
                    f.Write(toolTip1024b, 0, toolTip1024b.Length);


                    ms.Dispose();
                    image = null;
                    file1024b = null;
                    thumb.Dispose();
                    b.Dispose();

                    count++;
                    this.Invoke(new ShowProgressCallback(this.ShowProgress),new object[] { files.Count, count, "Creating/Updating thumbnails" });                    
                    Application.DoEvents();
                }

                f.Close();
                toolTips.Clear();

            }
            catch (Exception) { }
            finally
            {
                this.Invoke(new SimpleDelegate(this.HideProgress));                
            }
        }

        private bool IsThumbsDBOld()
        {
            string[] tfiles = null;
            string thumbsDB = this.sourceDirectory + Path.DirectorySeparatorChar + "thumbsDB";

            files.Clear();

            string[] exts = null;

            if (thumbnailFileType == ThumbnailFileType.CommonImages)
            {
                exts = standardExts;
            }
            else if (thumbnailFileType == ThumbnailFileType.DICOM)
            {
                exts = dicomExts;
            }
            else if (thumbnailFileType == ThumbnailFileType.AllImages)
            {
                exts = allExts;

            }

            foreach (string ext in exts)
            {
                tfiles = Directory.GetFiles(this.sourceDirectory, ext);

                foreach (string file in tfiles)
                {
                    files.Add(file);
                }

                tfiles = null;
            }

            thumbNails = new ThumbnailList(files.Count);

            DateTime lastestModTime = DateTime.MinValue;

            for (int j = 0; j < files.Count; j++)
            {
                string file = (string)files[j];
                if (lastestModTime < File.GetCreationTime(file))
                    lastestModTime = File.GetCreationTime(file);

                if (lastestModTime < File.GetLastWriteTime(file))
                    lastestModTime = File.GetLastWriteTime(file);
            }

            DateTime thumbsDBTime = File.GetLastWriteTime(this.SourceDirectory + Path.DirectorySeparatorChar + "Thumbs.iw");

            if (lastestModTime > thumbsDBTime)
                return true;
            else
                return false;

        }


        //public ArrayList Images
        //{
        //    set
        //    {
        //        try
        //        {
        //            this.SuspendLayout();
        //            RemoveThumbnails();
        //            if( thumbNails != null )
        //                this.thumbNails.Clear();

        //            thumbNails = new ThumbnailList();

        //            for (int i = 0; i < value.Count; i++)
        //            {
        //                this.thumbNails.Add((Image)value[i]);
        //            }

        //            LoadThumbnails();
        //            this.ResumeLayout();
        //        }
        //        catch (Exception) { }                
        //    }
        //}

        public ImageViewer()
        {
            PopulateAllExts();
            InitializeComponent();

            SetWindowTheme(progressBar1.Handle,"","");

            try
            {
                this.innerPanel.Controls.Remove(picBoxClone);
            }
            catch { }

            this.mainRibbonGroup.BackgroundImage = GenerateImage();

            showingHeight = Height;
            step = picBoxClone.Width;
            
            downT = new Thread(new ThreadStart(this.ScrollRightThread));
            upT =  new Thread(new ThreadStart(this.ScrollLeftThread));
        }

        private void PopulateAllExts()
        {
            int total = dicomExts.Length + standardExts.Length;
            allExts = new string[total];
            int idx = 0;

            for (int i = 0; i < standardExts.Length; i++)
            {
                allExts[idx] = standardExts[i];
                idx++;
            }

            for (int i = 0; i < dicomExts.Length; i++)
            {
                allExts[idx] = dicomExts[i];
                idx++;
            }
        }

        private void RemoveThumbnails()
        {
            for (int j = 0; j < innerPanel.Controls.Count; j++)
            {
                Control x = innerPanel.Controls[j];                

                if (x.GetType() == typeof(RibbonItem))
                {
                    innerPanel.Controls.Remove(x);

                    if (((RibbonItem)x).Image != null)
                    {
                        ((RibbonItem)x).Image.Dispose();
                    }

                    ((RibbonItem)x).Dispose();
                    
                    j--;
                }
            }
            System.GC.Collect();
        }

        private void ResizeInnerPanel()
        {
            int width = outerOuterPanel.Width;

            int remainder = outerOuterPanel.Width % (step + space);

            // the last space at the bottom
            if (remainder != space)
            {
                this.outerPanel.Width = outerOuterPanel.Width - (remainder - space);
            }
        }

        private void LoadThumbnails()
        {
            RemoveThumbnails();

            innerPanel.Width = thumbNails.Count * (step + space) + 20;

            int i = 0;
            int count = 0;

            //for( int k = firstIndex; k < thumbNails.Count; k++)
            for (int k = 0; k < thumbNails.Count; k++)
            {
                Image t = thumbNails[k];

                RibbonItem newPicBox = new RibbonItem();
                newPicBox.Width = picBoxClone.Width;
                newPicBox.Height = picBoxClone.Height;
                newPicBox.BackColor = picBoxClone.BackColor;
                newPicBox.Image = t;

                this.innerPanel.Controls.Add(newPicBox);
                newPicBox.Location = new Point(5 + i, 4);

                string ext = Path.GetExtension((string)files[k]).ToUpper();

                this.toolTip1.SetToolTip(newPicBox, (string)toolTips[k]);
                
                newPicBox.Click += new EventHandler(newPicBox_Click);
                
                newPicBox.Cursor = Cursors.Hand;
                newPicBox.Tag = k;
                selectedIndex = k;

                i += (step + space);
                count++;

                //int temp = (count + 1) * ( step + space ) + 60;
                //if (temp > this.mainRibbonGroup.Height)
                //{
                //    lastIndex = count + firstIndex;
                //    break;                     
                //}                
            }

            ResizeInnerPanel();
        }

        void newPicBox_Click(object sender, EventArgs e)
        {
            try
            {                
                RibbonItem source = (RibbonItem)sender;
                int fileNum = (int)source.Tag;
                fileName = (string)files[fileNum];

                if (SelectedIndexChanged != null && source != null)
                    SelectedIndexChanged(this, EventArgs.Empty);

                if (lastSelected != null)
                {
                    lastSelected.IsPressed = false;
                }

                lastSelected = source;
                lastSelected.IsPressed = true;

            }
            catch (Exception) {}
        }

        private void btnUpDown_Click(object sender, EventArgs e)
        {
        	_collapsed = !_collapsed;
            CollapseHelper();            
        }

        public void ShowProgress(int max, int value, string text)
        {
            if (!transPanel1.Visible)
            {
                transPanel1.Show();
            }

            progressBar1.Maximum = max;
            progressBar1.Value = value;
            lblProgress.Text = text;
        }

        public void HideProgress()
        {
            transPanel1.Hide();
        }

        private void CollapseHelper()
        {
            if (_collapsed)
            {
                
                try
                {
                    this.Parent.Height = hiddenHeight;
                }
                catch { }
                
                mainRibbonGroup.Hide();
                //hiddenRibbenGroup.Dock = DockStyle.Fill;
                hiddenRibbenGroup.Show();
                //this.Height = hiddenHeight;
            }
            else
            {                   
                //this.Height = showingHeight;                

                try
                {
                    this.Parent.Height = showingHeight;
                }
                catch { }
                
                hiddenRibbenGroup.Hide();
                //hiddenRibbenGroup.Dock = DockStyle.None;
                mainRibbonGroup.Show();                
            }

            if (CollapsedChanged != null)
            {                
                CollapsedChanged(this, EventArgs.Empty);
            }
        }

        private void hiddenRibbenGroup_SizeChanged(object sender, EventArgs e)
        {
            int newX = 0;
            
            newX = hiddenRibbenGroup.Width / 2 - buttonUP.Width / 2;
            buttonUP.Location = new Point(newX, buttonUP.Location.Y) ;
        }

        private void mainRibbonGroup_SizeChanged(object sender, EventArgs e)
        {
            int newX = 0;

            newX = mainRibbonGroup.Width / 2 - buttonDOWN.Width / 2;
            buttonDOWN.Location = new Point(newX, buttonDOWN.Location.Y);

            //if (thumbNails != null)
            //{
            //    try
            //    {
            //        LoadThumbnails();
            //    }
            //    catch { }
            //}

            transPanel1.Left = this.Width / 2 - transPanel1.Width / 2;            
        }
        
        private Bitmap GenerateImage()
        {
            if (ngMediImage == null)
                return null;

            Bitmap b = new Bitmap(50, 88);            
            Graphics g = Graphics.FromImage(b);

            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.CompositingQuality = CompositingQuality.HighQuality;            

            Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

            Color[] col = new Color[] { ngMediImage.GetColorConfig().TabPageColor1, ngMediImage.GetColorConfig().TabPageColor2, ngMediImage.GetColorConfig().TabPageColor3, ngMediImage.GetColorConfig().TabPageColor4 };
            float[] pos = new float[] { 0.0f, 0.2f, 0.2f, 1.0f };

            ColorBlend blend = new ColorBlend();
            blend.Colors = col;
            blend.Positions = pos;
            LinearGradientBrush brush = new LinearGradientBrush(rect, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);
            brush.InterpolationColors = blend;

            RibbonControl.FillRoundRectangle(g, brush, rect, 3f);

            RibbonControl.DrawRoundRectangle(g, new Pen(ngMediImage.GetColorConfig().Color6), rect, 3f);
            rect.Offset(1, 1);
            rect.Width -= 2;
            rect.Height -= 2;
            RibbonControl.DrawRoundRectangle(g, new Pen(new LinearGradientBrush(rect, ngMediImage.GetColorConfig().TabPageBorderColor, Color.Transparent, LinearGradientMode.ForwardDiagonal)), rect, 3f);

            g.Dispose();

            Bitmap b2 = new Bitmap(12, 88);

            Graphics g2 = Graphics.FromImage(b2);
            g2.DrawImage(b, 0, 0, new Rectangle(10, 0, 12, 88), GraphicsUnit.Pixel);
            g2.Dispose();
            b.Dispose();

            return b2;
        }

        //private void mainRibbonGroup_Paint(object sender, PaintEventArgs e)
        //{
        //    e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        //    e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
        //    e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
        //    e.Graphics.CompositingQuality = CompositingQuality.HighQuality;

        //    e.Graphics.Clear(ngMediImage.GetColorConfig().TabBarBackColor);

        //    Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

        //    Color[] col = new Color[] { ngMediImage.GetColorConfig().TabPageColor1, ngMediImage.GetColorConfig().TabPageColor2, ngMediImage.GetColorConfig().TabPageColor3, ngMediImage.GetColorConfig().TabPageColor4 };
        //    float[] pos = new float[] { 0.0f, 0.2f, 0.2f, 1.0f };

        //    ColorBlend blend = new ColorBlend();
        //    blend.Colors = col;
        //    blend.Positions = pos;
        //    LinearGradientBrush brush = new LinearGradientBrush(rect, Color.Transparent, Color.Transparent, LinearGradientMode.Vertical);
        //    brush.InterpolationColors = blend;

        //    RibbonControl.FillRoundRectangle(e.Graphics, brush, rect, 3f);

        //    RibbonControl.DrawRoundRectangle(e.Graphics, new Pen(ngMediImage.GetColorConfig().Color6), rect, 3f);
        //    rect.Offset(1, 1);
        //    rect.Width -= 2;
        //    rect.Height -= 2;
        //    RibbonControl.DrawRoundRectangle(e.Graphics, new Pen(new LinearGradientBrush(rect, ngMediImage.GetColorConfig().TabPageBorderColor, Color.Transparent, LinearGradientMode.ForwardDiagonal)), rect, 3f);
        
        //}

        private void ScrollLeft()
        {
            for (int i = 0; i < outerPanel.Width - space; i += 1)
            {
                if (innerPanel.Left == 0)
                    break;

                innerPanel.Left = innerPanel.Left + 1;
                Application.DoEvents();
            }
        }

        private void ScrollRight()
        {
            if ((innerPanel.Left + innerPanel.Width) < outerOuterPanel.Width)
                return;

            for (int i = 0; i < outerPanel.Width - space; i += 1)
            {

                innerPanel.Left = innerPanel.Left - 1;
                Application.DoEvents();
            }
        }

        private void outerPanel_Resize(object sender, EventArgs e)
        {        	
            ResizeInnerPanel();
        }
        
        void BtnLeftMouseDown(object sender, MouseEventArgs e)
        {
        	mouseDown = true;
        	
            if( upT.ThreadState == ThreadState.Running )
            {            	
            	return;
            }
            
            // if already fully scrolled up
            if (innerPanel.Left >= 0)
            {            	
                return;
            }
            
            upT = new Thread(new ThreadStart(this.ScrollLeftThread));            
            upT.Start();
        }
        
        void BtnLeftMouseUp(object sender, MouseEventArgs e)
        {        	            
        	mouseDown = false;                    	
        }
        
        private void btnRight_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            
            if( downT.ThreadState == ThreadState.Running )
            	return;
            
            if ((innerPanel.Left + innerPanel.Width - space * 3*2 ) <= outerPanel.Width)
                	return;
            
            downT = new Thread(new ThreadStart(this.ScrollRightThread));
            downT.Start();
        }

        private void btnRight_MouseUp(object sender, MouseEventArgs e)
        {
        	mouseDown = false;        	
        }
        
        private void ScrollRightSingle()
        {
        	if( this.thumbNails == null )
        		return;
        	
            for (int i = 0; i < picBoxClone.Width + space; i += 1)
            {            	
            	if ((innerPanel.Left + innerPanel.Width - space * 3 ) <= outerPanel.Width)
                	return;

                innerPanel.Left = innerPanel.Left - 1;
                Application.DoEvents();
            }
        }
        
        
        private void ScrollLeftSingle()
        {
        	if( this.thumbNails == null )
        		return;        	
        
            for (int i = 0; i < picBoxClone.Width + space; i += 1)
            {            	
            	if (innerPanel.Left >= 0)
                	return;

                innerPanel.Left = innerPanel.Left + 1;
                Application.DoEvents();
            }            
        }
        	
        private void ScrollRightThread()
        {
    		while( mouseDown )
        	{
        		if ((innerPanel.Left + innerPanel.Width - space * 3 ) <= outerPanel.Width)
                	return;
        	
        		this.Invoke(new SimpleDelegate( this.ScrollRightSingle ));
        		//System.Threading.Thread.Sleep( 50 );
        	}
        }
        
        private void ScrollLeftThread()
        {
        	while( mouseDown )
        	{
        		if (innerPanel.Left >= 0)
            	{
            		return;
           		}
        		
        		this.Invoke(new SimpleDelegate( this.ScrollLeftSingle ));
        		//System.Threading.Thread.Sleep( 50 );
        	}
        }

        private void ImageViewer_Load(object sender, EventArgs e)
        {
            if (ngMediImage != null)
                this.progressBar1.ForeColor = ngMediImage.GetColorConfig().ProgressBarColor;
            else
                this.progressBar1.ForeColor = NuGenColorsStatic.ProgressBarColor;
        }
 

    }

    internal class ThumbnailList : ArrayList
    {

        public ThumbnailList(int capacity):base(capacity)
        {            
        }

        public ThumbnailList(): base()
        {
        }

        public new Image this[int idx]
        {
            get
            {
                return (Image)base[idx];
            }

            set
            {
                base[idx] = value;
            }            
        }
    }
}
