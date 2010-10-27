using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
namespace Netron.Neon.OfficePickers
{
   

    /// <summary>
    /// Represents a ToolStripButtonItem that contains Color Picker control.
    /// </summary>
    [DefaultEvent("SelectedCapChanged"), DefaultProperty("LineCap"), 
    Description("ToolStripItem that allows selecting a style from a style picker control."),
    ToolboxItem(false),
    ToolboxBitmap(typeof(ToolStripLineCapPicker), "LinePickers.ToolStripLineCapPicker")]
    public class ToolStripLineCapPicker : ToolStripDropDownButton
    {

        #region Fields
        private ToolStripPickerDisplayType mButtonDisplayStyle = ToolStripPickerDisplayType.UnderLineAndImage;
        private ConnectionCap mLineCap = ConnectionCap.NoCap;
        private bool mAddWidthNameToToolTip = true;
        private Color mLineColor = Color.Black;
        #endregion

        #region Events
        /// <summary>
        /// Occurs when the value of the Color property changes. 
        /// </summary>
        [Category("Behavior"), Description("Occurs when the value of the Color property changes.")]
        public event EventHandler SelectedCapChanged;
        #endregion

        #region Properties
  
        
        /// <summary>
        /// Gets or sets the ToolStripPickerDisplayType in order to
        /// specified the display style of the button - image, text, underline etc.
        /// </summary>
        [Category("Appearance"), Description("Specifies whether to display the image, text and underline on the button.")
        ,DefaultValue(typeof(ToolStripPickerDisplayType), "ToolStripPickerDisplayType.UnderLineAndImage")]                     
        public ToolStripPickerDisplayType ButtonDisplayStyle
        {
            get { return mButtonDisplayStyle; }
            set
            {
                mButtonDisplayStyle = value;
                UpdateDisplayStyle();
            }
        }

        /// <summary>
        /// Overrides, Gets or sets the ToolStripItem.DisplayStyle property, use
        /// the ButtonDisplayStyle instead.
        /// </summary>
        [Browsable(false)]        
        public override ToolStripItemDisplayStyle DisplayStyle
        {
            get { return base.DisplayStyle; }
            set { base.DisplayStyle = value; }
        }
       
        /// <summary>
        /// Gets or sets the line width assign to the color picker control.
        /// </summary>
        [Category("Data"), 
        Description("Gets or sets the 'line width' assign to the color picker control."),
        DefaultValue(typeof(float),"1F" )]
        public ConnectionCap LineCap
        {
            get { return mLineCapPicker.LineCap; }
            set 
            { 
                mLineCapPicker.LineCap = value;
                Refresh();
                OnSelectedStyleChanged(EventArgs.Empty);
            }
        }

        public CustomLineCap CustomLineCap
        {
            get { return mLineCapPicker.CustomLineCap; }
        }

        /// <summary>
        /// Gets or sets the line width assign to the color picker control.
        /// </summary>
        [Category("Data"),
        Description("Gets or sets the color of the underline."),
        DefaultValue(typeof(Color), "Color.Black")]
        public Color LineColor
        {
            get { return mLineColor; }
            set
            {
                mLineColor = value;
                Refresh();
            }
        }

        
        /// <summary>
        /// Gets or sets value indicating whether to render the color name to the tool tip text.
        /// </summary>
        [DefaultValue(true), 
        Category("Behavior"), Description("Value indicating whether to render the color name to the tool tip text.")]
        public bool AddColorNameToToolTip
        {
            get { return mAddWidthNameToToolTip; }
            set { mAddWidthNameToToolTip = value; }
        }

        private string _originalToolTipText = "";
        /// <summary>
        /// Gets or sets the text that appears as a tooltip in the button.
        /// the color name will be rendered to the tooltip if the AddColorNameToolTip property set to true.
        /// </summary>
        [Category("Behavior"), Description("The text that appears as a tooltip (the color name will be render  automatically if defined to do so.")]
        new public string ToolTipText
        {
            get { return _originalToolTipText; }
            set 
            {
                _originalToolTipText = value;
                if (mAddWidthNameToToolTip)
                {
                    base.ToolTipText =  mLineCapPicker.LineCap.ToString()  ;
                }
                else
                {
                    base.ToolTipText = value;
                }
            }
        }        

        #endregion

        #region Private Members
        /// <summary>
        /// The color picker control that opens when clicking on the button
        /// </summary>
        private OfficeLineCapPicker mLineCapPicker = new OfficeLineCapPicker();
        /// <summary>
        /// Default color rectangle (under line)
        /// </summary>
        private Rectangle _colorRectangle = new Rectangle(2, 17, 14, 4);
        /// <summary>
        /// The underline picture rectangle - stretch to 14X14
        /// </summary>
        private Rectangle _pictureRectangle = new Rectangle(2, 2, 14, 14);
        // Settings for the button painting
        private bool _showColorUnderLine = true;
        private bool _showUnderLineImage = true;
        private bool _showUnderLineText = false;

        private System.ComponentModel.IContainer components = null;


        #endregion

        #region Ctor
        /// <summary>
        /// Initializes a new instance of the ToolStripLineCapPicker that holds
        /// OfficeColorPicker control inside a ToolStripItem to add to ToolStrip containers.
        /// </summary>
        public ToolStripLineCapPicker() : base()
        {
            InitControl();
        }
       
        /// <summary>
        /// Initializes a new instance of the ToolStripLineCapPicker that holds
        /// OfficeColorPicker control inside a ToolStripItem to add to ToolStrip containers.
        /// </summary>
        /// <param name="startingColor">The color to assign to the color picker control</param>
        public ToolStripLineCapPicker(ConnectionCap startingStyle)
            : base()
        {
            LineCap = startingStyle;
            InitControl();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
                if (!mLineCapPicker.IsDisposed)
                    mLineCapPicker.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Inits
        /// <summary>
        /// Set starting properties for the control and register the needed events.
        /// </summary>
        private void InitControl()
        {
            mLineCapPicker.SelectedCapChanged += new EventHandler(HandleSelectedColorChanged);
            this.AutoSize = false;
            this.LineCap = ConnectionCap.NoCap;
        }
        /// <summary>
        /// Set the painting properties by the mButtonDisplayStyle property.
        /// </summary>
        private void UpdateDisplayStyle()
        {
            switch (mButtonDisplayStyle)
            {
                case ToolStripPickerDisplayType.NormalImage:
                    DisplayStyle = ToolStripItemDisplayStyle.Image;
                    _showColorUnderLine = false;
                    _showUnderLineImage = false;
                    _showUnderLineText = false;
                    break;
                case ToolStripPickerDisplayType.NormalImageAndText:
                    DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                    _showColorUnderLine = false;
                    _showUnderLineImage = false;
                    _showUnderLineText = false;
                    break;
                case ToolStripPickerDisplayType.UnderLineAndImage:
                    DisplayStyle = ToolStripItemDisplayStyle.None;
                    _showColorUnderLine = true;
                    _showUnderLineImage = true;
                    _showUnderLineText = false;
                    break;
                case ToolStripPickerDisplayType.UnderLineAndText:
                    DisplayStyle = ToolStripItemDisplayStyle.None;
                    _showColorUnderLine = true;
                    _showUnderLineImage = false;
                    _showUnderLineText = true;
                    break;
                case ToolStripPickerDisplayType.UnderLineTextAndImage:
                    DisplayStyle = ToolStripItemDisplayStyle.None;
                    _showColorUnderLine = true;
                    _showUnderLineImage = true;
                    _showUnderLineText = true;
                    break;
                case ToolStripPickerDisplayType.UnderLineOnly:
                    DisplayStyle = ToolStripItemDisplayStyle.None;
                    _showColorUnderLine = true;
                    _showUnderLineImage = false;
                    _showUnderLineText = false;
                    break;
                case ToolStripPickerDisplayType.None:
                    DisplayStyle = ToolStripItemDisplayStyle.None;
                    _showColorUnderLine = false;
                    _showUnderLineImage = false;
                    _showUnderLineText = false;
                    break;
                case ToolStripPickerDisplayType.Text:
                    DisplayStyle = ToolStripItemDisplayStyle.Text;
                    _showColorUnderLine = false;
                    _showUnderLineImage = false;
                    _showUnderLineText = false;
                    break;
                default:
                    break;
            }
            Refresh();
        }
        
        #endregion

        #region Overrides (event handlers)
        /// <summary>
        /// When clicking on the button - opens the Color Picker
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e)
        {
            Point startPoint = GetOpenPoint();
            mLineCapPicker.Show(startPoint);
            base.OnClick(e);
        }

     

        /// <summary>
        /// Gets the button position by the parent ToolStrip
        /// </summary>
        /// <returns></returns>
        private Point GetOpenPoint()
        {
            if (this.Owner == null) return new Point(5, 5);
            int x = 0;
            foreach (ToolStripItem item in this.Parent.Items)
            {
                if (item == this) break;
                x += item.Width;
            }
            return this.Owner.PointToScreen(new Point(x, -4));
        }
      
        /// <summary>
        /// Fires the SelectedCapChanged event.
        /// </summary>
        /// <param name="e"></param>
        public void OnSelectedStyleChanged(EventArgs e)
        {
            if (SelectedCapChanged != null)
                SelectedCapChanged(this, e);
        }
        /// <summary>
        /// Repaint the button with the new color 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleSelectedColorChanged(object sender, EventArgs e)
        {
            Refresh();
            OnSelectedStyleChanged(EventArgs.Empty);
        }
        /// <summary>
        /// Repaint the parent tool strip and the button tool tip
        /// </summary>
        private void Refresh()
        {
            // Call the tool tip set property to add color name to tool tip
            ToolTipText = _originalToolTipText;
            // Refresh the toolstrip that holds this button.
            if (this.Owner != null)
                this.Owner.Refresh();
        }
        #endregion

        #region Painting the Button
        /// <summary>
        /// Paints the underline rectangle.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="bounds"></param>
        private void PaintUnderLine(Graphics g)
        {
            // Paint the line down on the button
            using (Pen pen = new Pen(Color.Red,2F))
            {
                //pen.DashStyle = mLineCap;
                _colorRectangle =new Rectangle(2, this.Height - 6, this.Width - 16, 4);
                //g.FillRectangle(brush, _colorRectangle);
                g.DrawLine(pen, 2, this.Height - 6, this.Width - 16, this.Height - 6);
            }
        }
        /// <summary>
        /// Paints the under line image
        /// </summary>
        /// <param name="g"></param>
        /// <param name="bounds"></param>
        /// <returns></returns>
        private Size PaintUnderLineImage(Graphics g)
        {
            Bitmap bmp = this.Image as Bitmap;
            // Paints the image, if exists
            if (bmp != null)
            {
                bmp.MakeTransparent(this.ImageTransparentColor);
                g.DrawImage(bmp, _pictureRectangle);
                return bmp.Size;
            }
            else
            {
                return new Size(0, 0);
            }
        }
        /// <summary>
        /// Paints the underline text
        /// </summary>
        /// <param name="g"></param>
        /// <param name="imageSize"></param>
        /// <param name="bounds"></param>
        private void PaintUnderLineText(Graphics g, Size imageSize)
        {
            using (Brush brush = new SolidBrush(ForeColor))
            {
                int x = imageSize.Width + 2;
                int y = 2;
                Rectangle stringRec = new Rectangle(x, y, this.Width - x, this.Height - y);
                g.DrawString(Text, this.Font, brush, stringRec);
            }
        }

        /// <summary>
        /// Overrides, Paint the image in the specified scale and the color line if defined.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (_showColorUnderLine)
            {                
                base.OnPaint(e);
                Size imageSize = new Size(0, 0);
                 PaintUnderLine(e.Graphics);               
                if (this.Image != null && _showUnderLineImage)
                {
                    imageSize = PaintUnderLineImage(e.Graphics);
                }
                if (_showUnderLineText)
                {
                    PaintUnderLineText(e.Graphics, imageSize);
                }
            }
            else
            {
                base.OnPaint(e);
            }
        }       
        #endregion

       
    }
}