using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Genetibase.UI.Utils;
using Genetibase.UI.Drawing;

namespace Genetibase.UI
{

    [ToolboxBitmap(typeof(NuGenScreenCap))]
    public partial class NuGenScreenCap : UserControl
    {

        private Bitmap selectedImage = null;
        private bool autoSizeParentForm = false;

        // Moving ant border
        private bool drawMovingAntBorder = true;

        // Show coordinates with mouse pointer
        private bool showCoords = true;

        // Color of the coordinates
        private Color coordsColor = Color.White;
        private SelectedEffect selectedEffect;
        private enum SelectedEffect
        {
            Brightness, Contrast, Transparency
        }

        private int oldTransparency = 255;
        private Bitmap tempImage = null;
        private Bitmap selectedImageNoEffects = null;
        private System.Windows.Forms.PictureBox picBoxMain;

        // Set to true when some selection has been made
        private bool selectionMade = false;

        // The current location of mouse pointer
        private int mouseX = 0;
        private int mouseY = 0;
        private bool drawingRect = false;
        private bool drawingAnts = false;
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
        private System.Timers.Timer redrawTimer;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem bevelToolStripMenuItem;
        private ToolStripMenuItem roundEdgesToolStripMenuItem;

        public NuGenScreenCap()
        {
            InitializeComponent();
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
            //Get the initial screen shot
            ScreenCapture sc = new ScreenCapture();
            Bitmap img = sc.CaptureScreen();

            if (this.Bitmap != null)
                this.Bitmap.Dispose();
            this.Bitmap = img;

            if (autoSizeParentForm)
                SetupParentForm();
            this.saveFileDialog.Filter = "Bitmap Files|*.bmp|JPEG Files|*.jpeg|PNG Files|*.png|GIF Files|*.gif|TIFF Files|*.tiff|Icon Files|*.ico";
        }

        #region Properties

        /// <summary>
        /// Get or Set the color in whics coordinates are shown
        /// </summary>
        [Category("NuGenScreenCap")]
        public Color CoordsColor
        {
            get { return coordsColor; }
            set
            {
                coordsColor = value;

                if (coordsBrush != null)
                    coordsBrush.Dispose();
                coordsBrush = new SolidBrush(coordsColor);
            }
        }

        /// <summary>
        /// Get the image that was selected by the user after dragging mouse
        /// </summary>
        [Browsable(false)]
        [Category("NuGenScreenCap")]
        public Bitmap SelectedImage
        {
            get
            {
                return selectedImage;
            }
        }

        /// <summary>
        /// Bitmap thats displayed on the screen
        /// Normally the captured image
        /// </summary>
        /// 
        [Category("NuGenScreenCap")]
        internal Image Bitmap
        {
            get
            {
                return this.picBoxMain.Image;
            }
            set
            {
                this.picBoxMain.Image = value;
            }
        }

        /// <summary>        
        /// Autosizes the parent form of the control
        /// This includes making the form full screen,
        /// hiding the title bar, hiding the statusbar
        /// and making NuGenScreenCap DockStyle to Fill
        /// </summary>        
        [Category("NuGenScreenCap")]
        [DefaultValue(false)]
        public bool AutoSizeParentForm
        {
            get { return autoSizeParentForm; }
            set { autoSizeParentForm = value; }
        }

        /// <summary>
        /// Draw an animated border around selection
        /// </summary>
        [Category("NuGenScreenCap")]
        [DefaultValue(true)]
        public bool AnimatedSelectionBorder
        {
            get { return drawMovingAntBorder; }
            set { drawMovingAntBorder = value; }
        }

        /// <summary>
        /// Show mouse coordinates as the mouse moves
        /// </summary>
        [Category("NuGenScreenCap")]
        [DefaultValue(true)]
        public bool ShowMouseCoordinates
        {
            get { return showCoords; }
            set { showCoords = value; }
        }

        #endregion

        #region Mouse events

        private void picBoxMain_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;

            //if( showCoords )
            //	this.picBoxMain.Invalidate();
            if (!drawingRect)
                return;
            SetRectangle(e.X, e.Y);
            this.picBoxMain.Invalidate();
        }

        private void picBoxMain_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                selectionMade = false;
                drawingRect = true;
                drawingAnts = true;
                initX = e.X;
                initY = e.Y;
                rect.X = e.X;
                rect.Y = e.Y;
                rect.Width = 0;
                rect.Height = 0;
            }
        }

        private void picBoxMain_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {

            if (!selectionMade)
            {
                // this.redrawTimer.Stop();
                drawingRect = false;

                if (rect.Width > 0 && rect.Height > 0)
                {
                    selectionMade = true;

                    if (selectedImage != null)
                        selectedImage.Dispose();
                    selectedImage = new Bitmap(rect.Width, rect.Height);
                    Graphics g = Graphics.FromImage(selectedImage);
                    Rectangle destRect = new Rectangle(0, 0, rect.Width, rect.Height);
                    g.DrawImage(Bitmap, destRect, rect, GraphicsUnit.Pixel);
                    g.Dispose();

                    if (selectedImageNoEffects != null)
                        selectedImageNoEffects.Dispose();
                    selectedImageNoEffects = (Bitmap)selectedImage.Clone();
                    this.picBoxMain.Invalidate();
                    // Gargabge Collection
                    System.GC.Collect();
                }
            }
        }

        #endregion

        public void CaptureScreen()
        {
            ScreenCapture sc = new ScreenCapture();
            Bitmap img = sc.CaptureScreen();

            if (this.Bitmap != null)
                this.Bitmap.Dispose();
            this.Bitmap = img;
        }

        private void SetupParentForm()
        {
            this.ParentForm.ControlBox = false;
            this.ParentForm.FormBorderStyle = FormBorderStyle.None;
            this.ParentForm.WindowState = FormWindowState.Maximized;
            this.ParentForm.Size = Screen.PrimaryScreen.Bounds.Size;
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

        private void picBoxMain_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

            if ((drawingAnts || drawingRect) && (rect.Width > 1 && rect.Height > 1))
            {
                offSet = (offSet - 1) % 8;
                p.DashOffset = offSet;
                p2.DashOffset = offSet + 4;

                if (!selectionMade)
                    e.Graphics.FillRectangle(b, rect);
                Rectangle newRect = new Rectangle(rect.X - 1, rect.Y - 1, rect.Width + 1, rect.Height + 1);
                e.Graphics.DrawRectangle(p, newRect);
                e.Graphics.DrawRectangle(p2, newRect);
            }

            if (selectionMade && selectedImage != null)
            {
                e.Graphics.DrawImage(selectedImage, rect.Location);
                e.Graphics.FillRectangle(b, rect);
            }

            if (showCoords)
            {
                string str = "(" + mouseX.ToString() + "," + mouseY.ToString() + ")";

                if (drawingRect)
                    str = "(" + rect.Width.ToString() + "," + rect.Height.ToString() + ")";
                Size coordinatesSize = e.Graphics.MeasureString(str, Font).ToSize();
                e.Graphics.DrawString(str, this.Font, coordsBrush, mouseX + 5 - coordinatesSize.Width / 2, mouseY + 25);
            }
        }

        // Timer repaints the selection
        // Helpfull in drawing the dotted selection boundry
        private void redrawTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.picBoxMain.Invalidate();
        }

        private void NuGenScreenCap_Load(object sender, EventArgs e)
        {
        }

        private enum SelectedFormat { Bitmap = 1, Jpeg, Png, Gif, Tiff, Ico };

        /// <summary>
        /// Copy to clipboard menu item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (this.selectedImage == null)
                return;
            Clipboard.SetDataObject(this.selectedImage, true);
            // Gargabge Collection
            System.GC.Collect();
            closeToolStripMenuItem_Click(null, null);
        }

        /// <summary>
        /// Called on refresh menu item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Effects.Rounded = false;

            if (this.selectedImage == null)
                return;
            this.selectedImage.Dispose();
            this.selectedImage = (Bitmap)selectedImageNoEffects.Clone();
            this.picBoxMain.Refresh();
            // Gargabge Collection
            System.GC.Collect();
        }

        /// <summary>
        /// Called on Bevel menu item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bevelToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (this.selectedImage == null)
                return;
            Bitmap oldImg = this.selectedImage;
            this.selectedImage = Effects.Bevel(this.selectedImage);
            oldImg.Dispose();
            oldImg = null;
            this.picBoxMain.Invalidate();
            // Gargabge Collection
            System.GC.Collect();
        }

        /// <summary>
        /// Called on Round Edges menu item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void roundEdgesToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (this.selectedImage == null)
                return;
            Effects.RoundImage(this.SelectedImage, 50);
            this.picBoxMain.Invalidate();
            // Gargabge Collection
            System.GC.Collect();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            b.Dispose();
            p.Dispose();
            p2.Dispose();
            coordsBrush.Dispose();
            b = null;
            p = null;
            p2 = null;
            coordsBrush = null;
            Clipboard.Clear();
            // Gargabge Collection
            System.GC.Collect();
            this.ParentForm.Close();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

            if (this.selectedImage == null)
            {
                bevelToolStripMenuItem.Enabled = false;
                roundEdgesToolStripMenuItem.Enabled = false;
                copyToolStripMenuItem.Enabled = false;
                blurToolStripMenuItem.Enabled = false;
                grayScaleMenuItem.Enabled = false;
                brightnessMenuItem.Enabled = false;
                contrastMenuItem.Enabled = false;
                sharpenMenuItem.Enabled = false;
                mnuDropShadow.Enabled = false;
                mnuTransparency.Enabled = false;
                mnuFeather.Enabled = false;
                mnuWash.Enabled = false;
                mnuSaveAs.Enabled = false;
            }

            else
            {
                bevelToolStripMenuItem.Enabled = true;
                roundEdgesToolStripMenuItem.Enabled = true;
                copyToolStripMenuItem.Enabled = true;
                blurToolStripMenuItem.Enabled = true;
                grayScaleMenuItem.Enabled = true;
                brightnessMenuItem.Enabled = true;
                contrastMenuItem.Enabled = true;
                sharpenMenuItem.Enabled = true;
                mnuDropShadow.Enabled = true;
                mnuTransparency.Enabled = true;
                mnuFeather.Enabled = true;
                mnuWash.Enabled = true;
                mnuSaveAs.Enabled = true;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

            if (this.selectedImage == null)
                return;
            Bitmap oldImg = this.SelectedImage;
            this.selectedImage = Effects.GrayScale(this.SelectedImage);
            oldImg.Dispose();
            oldImg = null;
            this.picBoxMain.Invalidate();
            // Gargabge Collection
            System.GC.Collect();
        }

        private void blurToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (this.selectedImage == null)
                return;
            Effects.GaussianBlur(this.selectedImage, 4);
            this.picBoxMain.Invalidate();
            // Gargabge Collection
            System.GC.Collect();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

            if (this.selectedImage == null)
                return;
            Bitmap oldImg = this.SelectedImage;
            this.selectedImage = Effects.Sharpen(this.selectedImage, 11);
            oldImg.Dispose();
            oldImg = null;
            this.picBoxMain.Invalidate();
            // Gargabge Collection
            System.GC.Collect();
        }

        private void brightnessMenuItem_Click(object sender, EventArgs e)
        {

            if (tempImage != null)
                tempImage.Dispose();
            // keep a copy
            tempImage = (Bitmap)selectedImage.Clone();
            trkEffects.Maximum = 255;
            trkEffects.Minimum = -255;
            trkEffects.Value = 0;
            selectedEffect = SelectedEffect.Brightness;
            pnlEffects.Location = new Point(mouseX, mouseY);
            pnlEffects.Visible = true;
        }

        private void trkEffects_Scroll(object sender, EventArgs e)
        {

            if (this.selectedImage == null || this.tempImage == null)
                return;
            Bitmap imgCopy = tempImage;
            Bitmap oldImg = this.selectedImage;

            if (selectedEffect == SelectedEffect.Brightness)
            {
                this.selectedImage = Effects.Brightness(imgCopy, trkEffects.Value);
            }

            else if (selectedEffect == SelectedEffect.Contrast)
            {
                this.selectedImage = Effects.Contrast(imgCopy, trkEffects.Value);
            }

            else if (selectedEffect == SelectedEffect.Transparency)
            {
                oldTransparency = trkEffects.Value;
                this.selectedImage = Effects.Transparency(imgCopy, oldTransparency);
            }
            oldImg.Dispose();
            oldImg = null;
            this.picBoxMain.Invalidate();
            // Gargabge Collection
            System.GC.Collect();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.pnlEffects.Hide();
        }

        private void contrastMenuItem_Click(object sender, EventArgs e)
        {

            if (tempImage != null)
                tempImage.Dispose();
            // keep a copy
            tempImage = (Bitmap)selectedImage.Clone();
            trkEffects.Maximum = 100;
            trkEffects.Minimum = -100;
            trkEffects.Value = 0;
            selectedEffect = SelectedEffect.Contrast;
            pnlEffects.Location = new Point(mouseX, mouseY);
            pnlEffects.Visible = true;
        }

        private void mnuDropShadow_Click(object sender, EventArgs e)
        {

            if (this.selectedImage == null)
                return;
            Bitmap oldImg = this.SelectedImage;
            this.selectedImage = Effects.DropShadow(this.selectedImage, 10, 20);
            oldImg = this.selectedImage;
            this.picBoxMain.Invalidate();
            // Gargabge Collection
            System.GC.Collect();
        }

        private void mnuTransparency_Click(object sender, EventArgs e)
        {

            if (tempImage != null)
                tempImage.Dispose();
            // keep a copy
            tempImage = (Bitmap)selectedImage.Clone();
            trkEffects.Maximum = 255;
            trkEffects.Minimum = 0;
            trkEffects.Value = oldTransparency;
            selectedEffect = SelectedEffect.Transparency;
            pnlEffects.Location = new Point(mouseX, mouseY);
            pnlEffects.Visible = true;
        }

        private void mnuWash_Click(object sender, EventArgs e)
        {

            if (this.selectedImage == null)
                return;
            ColorDialog colorDialog = new ColorDialog();
            DialogResult result = colorDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                Effects.Wash(this.SelectedImage, colorDialog.Color);
                this.picBoxMain.Invalidate();
                colorDialog.Dispose();
                // Gargabge Collection
                System.GC.Collect();
            }
        }

        private void mnuFeather_Click(object sender, EventArgs e)
        {

            if (this.selectedImage == null)
                return;
            ColorDialog colorDialog = new ColorDialog();
            DialogResult result = colorDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                Effects.FeatherImage(this.SelectedImage, colorDialog.Color);
                this.picBoxMain.Invalidate();
                colorDialog.Dispose();
                // Gargabge Collection
                System.GC.Collect();
            }
        }

        private void mnuSaveAs_Click(object sender, EventArgs e)
        {
            //close the menu strip before showing the dialog
            contextMenuStrip1.Close();
            saveFileDialog.FileName = "";
            DialogResult result = saveFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                string fileName = saveFileDialog.FileName;

                if (saveFileDialog.FilterIndex == (int)SelectedFormat.Bitmap)
                    selectedImage.Save(fileName, System.Drawing.Imaging.ImageFormat.Bmp);

                else if (saveFileDialog.FilterIndex == (int)SelectedFormat.Jpeg)
                    selectedImage.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);

                else if (saveFileDialog.FilterIndex == (int)SelectedFormat.Png)
                    selectedImage.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);

                else if (saveFileDialog.FilterIndex == (int)SelectedFormat.Ico)
                    selectedImage.Save(fileName, System.Drawing.Imaging.ImageFormat.Icon);

                else if (saveFileDialog.FilterIndex == (int)SelectedFormat.Gif)
                    selectedImage.Save(fileName, System.Drawing.Imaging.ImageFormat.Gif);

                else if (saveFileDialog.FilterIndex == (int)SelectedFormat.Tiff)
                    selectedImage.Save(fileName, System.Drawing.Imaging.ImageFormat.Tiff);
            }
        }
    }
}