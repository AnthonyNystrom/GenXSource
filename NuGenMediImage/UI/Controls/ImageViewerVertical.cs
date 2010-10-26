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
using System.Runtime.InteropServices; 
using System.Threading;

namespace Genetibase.NuGenMediImage.UI.Controls
{
    public partial class ImageViewerVertical : UserControl
    {        
        [DllImport("uxtheme.dll")]
        public static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        string[] exts = { "*.jpg", "*.jpeg", "*.gif", "*.bmp", "*.tif", "*.tiff", "*.png", "*.wmf", "*.emf", "*.exif" };

        private RibbonItem lastSelected = null;
        private bool mouseDown = false;
        
        private ThumbnailList thumbNails = null;
        internal NuGenMediImageCtrl ngMediImage = null;

        public int firstShowingIdx = 0;
        public int lastShowingIdx = 0;

        internal NuGenMediImageCtrl NgMediImage
        {
            get { return ngMediImage; }
            set 
            { 
                ngMediImage = value;
                this.mainRibbonGroup.ngMediImage = ngMediImage;
                this.hiddenRibbenGroup.ngMediImage = ngMediImage;
                this.btnLeft.NgMediImage = ngMediImage;
                this.btnRight.NgMediImage = ngMediImage;
                this.buttonDOWN.NgMediImage = ngMediImage;
                this.buttonUP.NgMediImage = ngMediImage;
                //this.mainRibbonGroup.BackgroundImage = GenerateImage();
                this.progressBar1.ForeColor = ngMediImage.GetColorConfig().ProgressBarColor;
                this.progressBar1.Color = ngMediImage.GetColorConfig().ProgressBarColor;
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
        
        
        Thread upT = null;
        Thread downT = null;
        
        int step = 0 ;
        int space = 5;

        ArrayList files = new ArrayList();

        private int selectedIndex = -1;

        private bool _collapsed = false;


        public int TotalThumbs
        {
            get
            {
                return (lastShowingIdx - firstShowingIdx);
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

        public bool Collapsed
        {
            get { return _collapsed; }
            set
            {
                _collapsed = value;
                CollapseHelper();
            }
        }

        [Browsable(false)]
        internal int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                if (value < 0 || value >= this.innerPanel.Controls.Count)
                    return;

                if ((int)(Math.Abs(value - selectedIndex)) > this.TotalThumbs)
                {
                    SelectedIndexFast = value;
                    return;
                }

                selectedIndex = value;
                DoSelect();            
            }
        }

        [Browsable(false)]
        internal int SelectedIndexFast
        {
            set
            {
                if (value < 0 || value >= this.innerPanel.Controls.Count)
                    return;

                selectedIndex = value;
                DoSelectFast();
            }
        }

        private void DoSelect()
        {
            foreach (RibbonItem picBoxCtrl in this.innerPanel.Controls)
            {
                if ((int)picBoxCtrl.Tag == selectedIndex)
                {
                    if (lastSelected != null)
                    {
                        lastSelected.IsPressed = false;
                    }

                    lastSelected = picBoxCtrl;
                    lastSelected.IsPressed = true;                    
                }
            }

            if (selectedIndex < firstShowingIdx)
            {
                for (int i = 0; i < (lastShowingIdx - firstShowingIdx + 1); i++)
                {
                    ScrollUpSingle();
                }
            }

            if (selectedIndex > lastShowingIdx)
            {
                for (int i = 0; i < (lastShowingIdx - firstShowingIdx + 1); i++)
                {
                    ScrollDownSingle();
                }
            }           
        }

        private void DoSelectFast()
        {
            foreach (RibbonItem picBoxCtrl in this.innerPanel.Controls)
            {
                if ((int)picBoxCtrl.Tag == selectedIndex)
                {
                    if (lastSelected != null)
                    {
                        lastSelected.IsPressed = false;
                    }

                    lastSelected = picBoxCtrl;
                    lastSelected.IsPressed = true;
                }
            }

            if (selectedIndex < firstShowingIdx)
            {
                while (selectedIndex < firstShowingIdx)
                {
                    ScrollUpLength();
                }
            }
            

            if (selectedIndex > lastShowingIdx)
            {
                while(selectedIndex > lastShowingIdx)
                {
                    ScrollDownLength();
                }
            }
            
        }
		
        private int showingHeight = 78;
        private int hiddenHeight = 18;

        public event CollapsedEventHandler CollapsedChanged;
        public event EventHandler SelectedIndexChanged;

        public ImageArray Images
        {
            set
            {
                try
                {                    
                    this.SuspendLayout();
                    RemoveThumbnails();
                    if (thumbNails != null)
                        this.thumbNails.Clear();

                    if (value != null && value.Count >= 1)
                    {
                        thumbNails = new ThumbnailList();
                        for (int i = 0; i < value.Count; i++)
                        {
                            //this.thumbNails.Add((Image)((Image)value[i]).Clone());
                            this.thumbNails.Add((Image)value[i]);
                        }

                        if (this.InvokeRequired)
                        {
                            this.Invoke(new SimpleDelegate(LoadThumbnails));
                        }
                        else
                        {
                            LoadThumbnails();
                        }
                    }

                    
                }
                catch (Exception) { }
                finally
                {                    
                    this.ResumeLayout();
                    this.Invoke(new SimpleDelegate(this.HideProgress));
                }
            }
        }

        internal void ShowProgress(int max, int value, string text)
        {
            if (!transPanel1.Visible)
            {
                transPanel1.Show();
            }
                       
            progressBar1.Maximum = max;
            progressBar1.Value = value;
            lblProgress.Text = text;

            progressBar1.Refresh();

            Application.DoEvents();
        }

        public void HideProgress()
        {
            transPanel1.Hide();
        }

        public ImageViewerVertical()
        {
            InitializeComponent();           

            try
            {
                this.innerPanel.Controls.Remove(picBoxClone);
            }
            catch { }
            showingHeight = Width;
            step = picBoxClone.Height;
            
            upT = new Thread(new ThreadStart(this.ScrollUpThread));
            downT = new Thread(new ThreadStart(this.ScrollDownThread));
        }

        private void RemoveThumbnails()
        {
            for (int j = 0; j < innerPanel.Controls.Count; j++)
            {
                Control x = innerPanel.Controls[j];

                if (x.GetType() == typeof(RibbonItem))
                {
                    innerPanel.Controls.Remove(x);
                    x.Dispose();
                    j--;
                }
            }
        }

        //private void ScrollUp()
        //{
        //    for (int i = 0; i < outerPanel.Height - space; i += 1)
        //    {
        //        if (innerPanel.Top == 0)
        //            break;

        //        innerPanel.Top = innerPanel.Top + 1;
        //        Application.DoEvents();
        //    }
        //}

        //private void ScrollDown()
        //{
        //    if ((innerPanel.Top + innerPanel.Height + space) < outerOuterPanel.Height)
        //        return;

        //    for (int i = 0; i < outerPanel.Height - space; i += 1)
        //    {
                
        //        innerPanel.Top = innerPanel.Top - 1;
        //        Application.DoEvents();   
        //    }
        //}

        private void ScrollDownSingle()
        {
        	if( this.thumbNails == null )
        		return;
        	
            for (int i = 0; i < picBoxClone.Height + space; i += 1)
            {            	
            	if ((innerPanel.Top + innerPanel.Height - space * 2 ) <= outerPanel.Height)
                	return;

                innerPanel.Top = innerPanel.Top - 1;
                Application.DoEvents();
            }


            this.firstShowingIdx++;
            this.lastShowingIdx = this.firstShowingIdx + outerOuterPanel.Height / (step + space) - 1;
        }

        private void ScrollDownLength()
        {
            if (this.thumbNails == null)
                return;

            if ((innerPanel.Top + innerPanel.Height - space * 2) <= outerPanel.Height)
                return;

            innerPanel.Top = innerPanel.Top - (picBoxClone.Height + space) * (lastShowingIdx - firstShowingIdx);
            Application.DoEvents();

            this.firstShowingIdx += (lastShowingIdx - firstShowingIdx);
            this.lastShowingIdx = this.firstShowingIdx + outerOuterPanel.Height / (step + space) - 1;
        }
        
        
        private void ScrollUpSingle()
        {
        	if( this.thumbNails == null )
        		return;        	
        
            for (int i = 0; i < picBoxClone.Height + space; i += 1)
            {            	
            	if (innerPanel.Top >= 0)
                	return;

                innerPanel.Top = innerPanel.Top + 1;
                Application.DoEvents();
            }            

            this.firstShowingIdx--;
            this.lastShowingIdx = this.firstShowingIdx + outerOuterPanel.Height / (step + space) - 1;
        
        }

        private void ScrollUpLength()
        {
            if (this.thumbNails == null)
                return;

            if (innerPanel.Top >= 0)
                return;

            innerPanel.Top = innerPanel.Top + (picBoxClone.Height + space) * (lastShowingIdx - firstShowingIdx);
            if (innerPanel.Top > 0)
                innerPanel.Top = 0;

            Application.DoEvents();

            this.firstShowingIdx -= (lastShowingIdx - firstShowingIdx);
            this.lastShowingIdx = this.firstShowingIdx + outerOuterPanel.Height / (step + space) - 1;
        }

        private void ScrollThumbnails()
        {
            //int i = 0;
            //int section = outerPanel.Height / 3;

            //while (true)
            //{
            //    if( i < section )
            //    {
            //        innerPanel.Top = innerPanel.Top - 2;
            //        i+=2;                    
            //    }
            //    else if (i < (section * 2))
            //    {
            //        innerPanel.Top = innerPanel.Top - 2;
            //        i+=2;                    
            //    }
            //    else
            //    {
            //        innerPanel.Top = innerPanel.Top - 1;
            //        i ++;                    
            //    }

            //    //System.Threading.Thread.Sleep(50);
            //    Application.DoEvents();

            //    if (i > outerPanel.Height)
            //        break;
            //}

            for (int i = 0; i < outerPanel.Height; i += 1)
            {
                innerPanel.Top = innerPanel.Top - 1;
                //System.Threading.Thread.Sleep(50);
                Application.DoEvents();
                //innerPanel.Invalidate(true);
            }
        }


        private void ResizeInnerPanel()
        {            
            int height = outerOuterPanel.Height;
            
            int remainder = outerOuterPanel.Height % (step + space);

            // the last space at the bottom
            if (remainder != space)
            {
                this.outerPanel.Height = outerOuterPanel.Height - (remainder - space);
            }                

            this.lastShowingIdx = this.firstShowingIdx + outerOuterPanel.Height / (step + space) - 1;
        }

        internal void AddSlice(Bitmap b,int idx,int total)
        {            
            //this.Invoke(new ShowProgressCallback(this.ShowProgress),
            //       new object[] { total, idx, "Loading       Slices" });
            this.ShowProgress(total, idx, "Loading       Slices");

            //innerPanel.Height = idx * (step + space) + 20;

            //RibbonItem newPicBox = new RibbonItem();
            //newPicBox.Width = picBoxClone.Width;
            //newPicBox.Height = picBoxClone.Height;
            //newPicBox.BackColor = picBoxClone.BackColor;
            //newPicBox.Image = b;

            //this.innerPanel.Controls.Add(newPicBox);
            //newPicBox.Location = new Point(4, 5 + (step + space) * idx);
            //this.innerPanel.Refresh();
        }

        private void LoadThumbnails()
        {
            RemoveThumbnails();
            innerPanel.Height = thumbNails.Count * (step + space) + 20;
            
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
                newPicBox.Location = new Point(4, 5 + i);
                
                
                this.toolTip1.SetToolTip(newPicBox, "Slice " + (k + 1) + " of " + thumbNails.Count);
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

                selectedIndex = (int)source.Tag;

                if (SelectedIndexChanged != null && source != null)
                    SelectedIndexChanged(this, EventArgs.Empty);

                
                if (lastSelected != null)
                {
                    lastSelected.IsPressed = false;
                }

                lastSelected = source;
                lastSelected.IsPressed = true;
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnUpDown_Click(object sender, EventArgs e)
        {
            _collapsed = !_collapsed;
            CollapseHelper();
        }

        private void CollapseHelper()
        {
            if (_collapsed)
            {
                mainRibbonGroup.Hide();
                //hiddenRibbenGroup.Dock = DockStyle.Fill;
                hiddenRibbenGroup.Show();
                this.Width = hiddenHeight;
                try
                {
                    this.Parent.Width = hiddenHeight;
                }
                catch { }
            }
            else
            {
                hiddenRibbenGroup.Hide();
                //hiddenRibbenGroup.Dock = DockStyle.None;
                mainRibbonGroup.Show();
                this.Width = showingHeight;

                try
                {
                    this.Parent.Width = showingHeight;
                }
                catch { }
            }

            if (CollapsedChanged != null)
            {
                CollapsedChanged(this, EventArgs.Empty);
            }
        }

        private void hiddenRibbenGroup_SizeChanged(object sender, EventArgs e)
        {
            int newY = 0;
            
            newY = hiddenRibbenGroup.Height / 2 - buttonUP.Height / 2;
            buttonUP.Location = new Point(buttonUP.Location.X,newY) ;
        }

        private void mainRibbonGroup_SizeChanged(object sender, EventArgs e)
        {
            int newY = 0;

            newY = mainRibbonGroup.Height / 2 - buttonDOWN.Height / 2;
            buttonDOWN.Location = new Point(buttonDOWN.Location.X, newY);

            transPanel1.Top = this.Height / 2 - transPanel1.Height / 2; 

            /*if (thumbNails != null)
            {
                try
                {
                    LoadThumbnails();
                }
                catch { }
            }*/
        }

        private void outerPanel_Resize(object sender, EventArgs e)
        {
            ResizeInnerPanel();
        }      
        
        

        private void btnRight_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;            
            
            if( downT.ThreadState == ThreadState.Running )
            	return;
            
            if ((innerPanel.Top + innerPanel.Height - space*2 ) <= outerPanel.Height)
                	return;
            
            downT = new Thread(new ThreadStart(this.ScrollDownThread));
            downT.Start();
        }

        private void btnRight_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }
        
        private void ScrollDownThread()
        {
        	while( mouseDown )
        	{
        		if ((innerPanel.Top + innerPanel.Height - space * 2 ) <= outerPanel.Height)
                	return;
        		
        		this.Invoke(new SimpleDelegate( this.ScrollDownSingle ));
        		//System.Threading.Thread.Sleep( 50 );
        	}
        }
        
        private void ScrollUpThread()
        {
        	while( mouseDown )
        	{
        		if (innerPanel.Top >= 0)
            	{
            		return;
           		}
        		
        		this.Invoke(new SimpleDelegate( this.ScrollUpSingle ));
        		//System.Threading.Thread.Sleep( 50 );
        	}
        }
        
        
        void BtnLeftMouseDown(object sender, MouseEventArgs e)
        {
        	mouseDown = true;
        	
            if( upT.ThreadState == ThreadState.Running )
            {            	
            	return;
            }
            
            // if already fully scrolled up
            if (innerPanel.Top >= 0)
            {            	
                return;
            }
            
            upT = new Thread(new ThreadStart(this.ScrollUpThread));            
            upT.Start();
        }
        
        void BtnLeftMouseUp(object sender, MouseEventArgs e)
        {
        	mouseDown = false;            
        }

        private void ImageViewerVertical_Load(object sender, EventArgs e)
        {
            if (ngMediImage != null)
            {
                this.progressBar1.ForeColor = ngMediImage.GetColorConfig().ProgressBarColor;
                this.progressBar1.Color = ngMediImage.GetColorConfig().ProgressBarColor;
            }
            else
            {
                this.progressBar1.ForeColor = NuGenColorsStatic.ProgressBarColor;
                this.progressBar1.Color = NuGenColorsStatic.ProgressBarColor;
            }
        }
    }   
}
