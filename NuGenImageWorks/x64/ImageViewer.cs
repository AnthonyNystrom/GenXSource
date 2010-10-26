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

namespace Genetibase.UI.NuGenImageWorks
{
    public delegate void SlideChangedEventHandler(object sender, SlideChangedEventArgs e);

    public partial class ImageViewer : UserControl
    {
        [DllImport("uxtheme.dll")]
        public static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        string[] exts = { "*.jpg", "*.jpeg", "*.gif", "*.bmp", "*.tif", "*.tiff", "*.png", "*.wmf", "*.emf", "*.exif" };


        private MainForm mainForm = null;
        private RibbonItem lastSelected = null;

        public MainForm MainForm
        {            
            get { return mainForm; }
            set { mainForm = value; }
        }
        
        private string sourceDirectory = null;
        private bool showing = true;
        private ThumbnailList thumbNails = null;
        
        private int firstIndex = 0;
        private int lastIndex = 0;
        ArrayList files = new ArrayList();

        public event SlideChangedEventHandler SlideChanged;        
        

        public string SourceDirectory
        {
            get { return sourceDirectory; }
            set 
            {
                try
                {
                    firstIndex = 0;
                    sourceDirectory = value;
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
                                projectedFileSize = (files.Count * 10054) + (files.Count * 1024);
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

                        this.SuspendLayout();

                        RemoveThumbnails();
                        LoadFromThumbsDB();
                        LoadThumbnails();

                        this.ResumeLayout();

                    }
                }
                catch { }

                /*try
                {
                    Directory.Delete(mainForm.TempDirectory, true);
                }
                catch { }*/
            }
        }

        public ImageViewer()
        {
            InitializeComponent();
            SetWindowTheme(progressBar1.Handle,"","");

            try
            {
                this.mainRibbonGroup.Controls.Remove(picBoxClone);
            }
            catch { }           
        }

        private bool IsThumbsDBOld()
        {            
            string []tfiles = null;            
            string thumbsDB = this.sourceDirectory + Path.DirectorySeparatorChar + "thumbsDB";

            files.Clear();
            
            foreach(string ext in exts )
            {
                tfiles = Directory.GetFiles(this.sourceDirectory, ext);

                foreach (string file in tfiles)
                {
                    files.Add(file);
                }
            }

            thumbNails = new ThumbnailList(files.Count);

            DateTime lastestModTime = DateTime.MinValue ;

            for (int j = 0; j < files.Count; j++)
            {
                string file = (string)files[j];
                if( lastestModTime < File.GetCreationTime(file) )
                    lastestModTime = File.GetCreationTime(file);

                if( lastestModTime < File.GetLastWriteTime(file) )
                    lastestModTime = File.GetLastWriteTime(file);
            }

            DateTime thumbsDBTime = File.GetLastWriteTime(this.SourceDirectory + Path.DirectorySeparatorChar + "Thumbs.iw");

            if (lastestModTime > thumbsDBTime)
                return true;
            else
                return false;
            
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
            if( files != null )
                files.Clear();

            while (br.BaseStream.Position != br.BaseStream.Length)
            {
                byte[] image = br.ReadBytes(10054);
                char[] pathC = br.ReadChars(1024);
                string path = new String(pathC).Trim();

                MemoryStream ms = new MemoryStream(image);
                Bitmap thumb = new Bitmap(ms);
                thumbNails.Add(thumb);
                files.Add(path);
                
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
                this.mainForm.Cursor = Program.aniCursor.Cursor;
                RemoveThumbnails();
                File.Delete(this.sourceDirectory + Path.DirectorySeparatorChar + "Thumbs.iw");
                FileStream f = File.Open(this.sourceDirectory + Path.DirectorySeparatorChar + "Thumbs.iw", FileMode.OpenOrCreate, FileAccess.Write);

                string[] tfiles = null;               


                files.Clear();

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

                foreach (string file in files)
                {
                    Bitmap b = null;

                    try
                    {
                        b = new Bitmap(file);
                    }
                    catch (Exception)
                    {
                        b = (Bitmap)picBoxClone.ErrorImage.Clone();
                    }

                    Image thumb = b.GetThumbnailImage(50, 50, null, IntPtr.Zero);
                    MemoryStream ms = new MemoryStream();
                    thumb.Save(ms, ImageFormat.Bmp);

                    byte[] image = ms.ToArray();
                    f.Write(image, 0, image.Length);

                    string file1024 = file + new String(' ', 1024 - file.Length);
                    byte[] file1024b = System.Text.ASCIIEncoding.ASCII.GetBytes(file1024);
                    f.Write(file1024b, 0, file1024b.Length);

                    ms.Dispose();
                    image = null;
                    file1024b = null;
                    thumb.Dispose();
                    b.Dispose();

                    count++;
                    this.Invoke(new ShowProgressCallback(this.ShowProgress),
                    new object[] { files.Count, count, "Creating/Updating thumbnails" });
                    Application.DoEvents();
                }

                f.Close();
                firstIndex = 0;
                
            }
            catch { }
            finally
            {
                this.Invoke(new SimpleDelegate(this.HideProgress));
                this.mainForm.Cursor = Cursors.Arrow;
            }
        }

        private void RemoveThumbnails()
        {
            for (int j = 0; j < mainRibbonGroup.Controls.Count; j++)
            {
                Control x = mainRibbonGroup.Controls[j];

                if (x.GetType() == typeof(RibbonItem))
                {
                    mainRibbonGroup.Controls.Remove(x);
                    x.Dispose();
                    j--;
                }
            }
        }


        public void UpdateThumbnail(string filePath)
        {
            Bitmap b = new Bitmap(filePath);
            Image thumb = b.GetThumbnailImage(50, 50, null, IntPtr.Zero);

            Image old = lastSelected.Image;
            lastSelected.Image = thumb;

            if (old != null)
                old = null;

            b.Dispose();
        }

        private void LoadThumbnails()
        {
            RemoveThumbnails();

            int step = picBoxClone.Width;
            int space = 5;
            int i = 0;
            int count = 0;

            for( int k = firstIndex; k < thumbNails.Count; k++)
            {
                Image t = thumbNails[k];

                RibbonItem newPicBox = new RibbonItem();
                newPicBox.Width = picBoxClone.Width;
                newPicBox.Height = picBoxClone.Height;
                newPicBox.BackColor = picBoxClone.BackColor;
                newPicBox.Image = t;

                this.mainRibbonGroup.Controls.Add(newPicBox);
                newPicBox.Location = new Point(30 + i, 4);
                newPicBox.MouseEnter += new EventHandler(newPicBox_MouseEnter);
                newPicBox.MouseLeave += new EventHandler(newPicBox_MouseLeave);
                this.toolTip1.SetToolTip(newPicBox, (string)files[k]);
                newPicBox.Click += new EventHandler(newPicBox_Click);
                
                newPicBox.Cursor = Cursors.Hand;
                newPicBox.Tag = k;

                i += (step + space);
                count++;

                int temp = (count + 1) * ( step + space ) + 60;
                if (temp > this.mainRibbonGroup.Width)
                {
                    lastIndex = count + firstIndex;
                    break;                     
                }
            }
        }

        void newPicBox_Click(object sender, EventArgs e)
        {
            try
            {

                RibbonItem source = (RibbonItem)sender;
                int fileNum = (int)source.Tag;
                string fileName = (string)files[fileNum];

                string fileSize = Utility.GetFileSize(fileName);

                FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read);
                Program.Photo = new Bitmap(fs);
                fs.Close();

                string[] fn = fileName.Split('\\');
                Program.Title.Text = "File: " + fn[fn.Length - 1] + " Resolution: " + Program.Photo.Width.ToString() + " x " + Program.Photo.Height.ToString() + " Size: " + fileSize;
                Program.FileName = fileName;

                ((MainForm)this.Parent.Parent).Open();

                Program.Source.Size = Program.Destination.Size;

                Program.Optimize2();

                Program.Destination.Image = (Bitmap)Program.Source.Image.Clone();

                if (lastSelected != null)
                {
                    lastSelected.IsPressed = false;
                }

                lastSelected = source;
                lastSelected.IsPressed = true;
            }
            catch (Exception) {}
        }

        void newPicBox_MouseLeave(object sender, EventArgs e)
        {
            //PictureBox source = (PictureBox)sender;
            //source.BorderStyle = BorderStyle.None;
        }

        void newPicBox_MouseEnter(object sender, EventArgs e)
        {
            //PictureBox source = (PictureBox)sender;
            //source.BorderStyle = BorderStyle.Fixed3D;
        }

        private void btnUpDown_Click(object sender, EventArgs e)
        {
            if (showing)
            {
                mainRibbonGroup.Hide();
                //hiddenRibbenGroup.Dock = DockStyle.Fill;
                hiddenRibbenGroup.Show();
            }
            else
            {
                hiddenRibbenGroup.Hide();
                //hiddenRibbenGroup.Dock = DockStyle.None;
                mainRibbonGroup.Show();
            }

            showing = !showing;

            if (SlideChanged != null)
            {
                SlideChanged(this, new SlideChangedEventArgs(showing));
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

            if (thumbNails != null)
            {
                try
                {
                    LoadThumbnails();
                }
                catch { }
            }

            transPanel1.Left = this.Width / 2 - transPanel1.Width / 2;            
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            if (thumbNails != null && firstIndex > 0)
            {
                int step = picBoxClone.Width;
                int space = 5;
                int possibleCount = this.mainRibbonGroup.Width / (step + space + 60);
                possibleCount++;

                int newIndex = firstIndex - possibleCount;
                if (newIndex < 0)
                    newIndex = 0;

                firstIndex = newIndex;

                try
                {
                    LoadThumbnails();
                }
                catch { }
            }
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            if (thumbNails != null && lastIndex < thumbNails.Count)
            {
                firstIndex = lastIndex;
                LoadThumbnails();
            }
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

        private void buttonUP_SizeChanged(object sender, EventArgs e)
        {

        }

        private void ImageViewer_KeyPress(object sender, KeyPressEventArgs e)
        {            
        }

        private void fileSystemWatcher1_Changed(object sender, FileSystemEventArgs e)
        {
            ReloadSourceDir(e.FullPath);
        }

        private void fileSystemWatcher1_Created(object sender, FileSystemEventArgs e)
        {
            ReloadSourceDir(e.FullPath);
        }

        private void fileSystemWatcher1_Deleted(object sender, FileSystemEventArgs e)
        {
            ReloadSourceDir(e.FullPath);
        }

        private void fileSystemWatcher1_Renamed(object sender, RenamedEventArgs e)
        {
            ReloadSourceDir(e.FullPath);
        }

        private void ReloadSourceDir(string FileName)
        {
            string  newExt = Path.GetExtension(FileName);
            bool matched = false;

            foreach (string ext in exts)
            {
                if (ext.Replace("*", "") == newExt)
                {
                    matched = true;
                    break;
                }
            }

            if( matched)
                this.SourceDirectory = this.SourceDirectory;
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
