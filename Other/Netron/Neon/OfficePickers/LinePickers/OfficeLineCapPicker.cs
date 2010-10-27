using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Netron.Neon.OfficePickers
{
    /// <summary>
    /// Provides color picker control that could be used in a model or non-model form.
    /// </summary
    [DefaultEvent("SelectedCapChanged"), DefaultProperty("LineCap"), ToolboxItem(true),
   ToolboxBitmap(typeof(OfficeLineCapPicker), "LinePickers.OfficeLineCapPicker"),
    Description("Provides a line cap picker control that could be used in a model or non-model form.")]
    public partial class OfficeLineCapPicker : UserControl
    {
        #region Static Methods
        /// <summary>
        /// The preferred height to span the control to
        /// </summary>
        public static readonly int PreferredHeight = 270;
        /// <summary>
        /// The preferred width to span the control to
        /// </summary>
        public static readonly int PreferredWidth = 200;

        #endregion

        #region Events
        /// <summary>
        /// Occurs when the value of the LineCap property changes. 
        /// </summary>
        [Category("Behavior"), Description("Occurs when the value of the LineCap property changes.")]
        public event EventHandler SelectedCapChanged;
        #endregion

        #region Properties
        private ConnectionCap mLineCap = ConnectionCap.NoCap;
        private Color mColor = Color.Black;
        /// <summary>
        /// Gets or sets the selected color from the OfficeLineCapPicker
        /// </summary>
        [Category("Data"), Description("The color selected in the dialog"),
       DefaultValue(typeof(float), "1F")]
        public ConnectionCap LineCap
        {
            get { return mLineCap; }
            set
            {
                mLineCap = value;
                // Set Selected LineCap in the GUI
                SetLineCap(value);
                // Fires the SelectedCapChanged event.
                OnSelectedCapChanged(EventArgs.Empty);
            }
        }
        public CustomLineCap CustomLineCap
        {
            get { return mCustomLineCap; }
        }

        /// <summary>
        /// Gets the selected color name, or 'Custom' if it is not one
        /// of the Selectable mSelectableStyles.
        /// </summary>
        [Browsable(false)]
        public string LineCapName
        {
            get
            {
                string mWidthName = "None";
                if (mCurrentSelected > -1 && mCurrentSelected < CustomCaps.SelectableLineCaps.Length)
                {
                    mWidthName = CustomCaps.SelectableLineCaps[mCurrentSelected].ToString();
                }
                return mWidthName;

            }
        }

        #endregion

        #region Private Members

        /// <summary>
        /// Parent form when this control is inside a context menu form
        /// </summary>
        private ContextMenuForm mContextForm;
        /// <summary>
        /// Parent control, when on of the Show(Control parent ...) is called.
        /// </summary>
        private Control mParentControl;
        /// <summary>
        /// Known mSelectableStyles list that user may select from 
        /// </summary>
        private SelectableLineCaps[] mSelectableStyles = new SelectableLineCaps[14];
        /// <summary>
        /// Buttons rectangle definitions.
        /// </summary>
        private Rectangle[] buttons = new Rectangle[14];
        /// <summary>
        /// Hot Track index to paint its button with HotTrack color
        /// </summary>
        private int mCurrentHotTrack = -1;
        /// <summary>
        /// Current selected index to paint its button with Selected color
        /// </summary>
        private int mCurrentSelected = -1;
        private CustomLineCap generalizationCap;
        private CustomLineCap mCustomLineCap = null;
        #endregion

        #region Ctor

        /// <summary>
        /// Initialized a new instance of the OfficeLineCapPicker in order to provide 
        /// color picker control that could be used in a model or non-model form.
        /// </summary>
        public OfficeLineCapPicker()
        {
            InitializeComponent();
            SetCapObjects();
            // Set painting style for better performance.
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);

            Point[] ps = new Point[3] { new Point(-2, 0), new Point(0, 4), new Point(2, 0) };
            GraphicsPath gpath = new GraphicsPath();
            gpath.AddPolygon(ps);
            gpath.CloseAllFigures();
            generalizationCap = new CustomLineCap(null, gpath);

        }

        /// <summary>
        /// Initialized a new instance of the OfficeLineCapPicker in order to provide 
        /// color picker control that could be used in a model or non-model form.   
        /// </summary>
        /// <param name="startingColor">Starting color to the OfficeLineCapPicker control</param>
        public OfficeLineCapPicker(ConnectionCap startStyle)
            : this()
        {
            LineCap = startStyle;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Opens the control inside a context menu in the specified location
        /// relative to the specified control.
        /// </summary>
        /// <param name="left">Parent control coordinates left location of the control</param>
        /// <param name="top">Parent control coordinates top location of the control</param>
        /// <param name="parent">Parent control to place the control at</param>
        public void Show(Control parent, int left, int top)
        {
            Show(parent, new Point(left, top));
        }
        /// <summary>
        /// Opens the control inside a context menu in the specified location
        /// </summary>
        /// <param name="left">Screen coordinates left location of the control</param>
        /// <param name="top">Screen coordinates top location of the control</param>
        public void Show(int left, int top)
        {
            Show(new Point(left, top));
        }
        /// <summary>
        /// Opens the control inside a context menu in the specified location
        /// </summary>
        /// <param name="startLocation">Screen coordinates location of the control</param>
        public void Show(Point startLocation)
        {
            // Creates new contextmenu form, adds the control to it, display it.
            mContextForm = new ContextMenuForm();
            mContextForm.SetContainingControl(this);
            mContextForm.Height = OfficeLineCapPicker.PreferredHeight;
            mContextForm.Show(this, startLocation, OfficeLineCapPicker.PreferredWidth);
        }
        /// <summary>
        /// Opens the control inside a context menu in the specified location
        /// </summary>
        /// <param name="startLocation">Screen coordinates location of the control</param>
        /// <param name="parent">Parent control to place the control at</param> 
        public void Show(Control parent, Point startLocation)
        {
            mParentControl = parent;
            // Creates new contextmenu form, adds the control to it, display it.      
            ContextMenuForm frm = new ContextMenuForm();
            frm.SetContainingControl(this);
            frm.Height = OfficeLineCapPicker.PreferredHeight;
            mContextForm = frm;
            frm.Show(parent, startLocation, OfficeLineCapPicker.PreferredWidth);
        }
        /// <summary>
        /// Fires the OfficeLineCapPicker.SelectedCapChanged event
        /// </summary>
        /// <param name="e"></param>
        public void OnSelectedCapChanged(EventArgs e)
        {
            Refresh();
            if (SelectedCapChanged != null)
                SelectedCapChanged(this, e);
        }

        #endregion

        #region Private and protected methods

        /// <summary>
        /// Creates the custom SelectableLineCaps buttons
        /// </summary>
        private void SetCapObjects()
        {
            for (int widthIndex = 0; widthIndex < mSelectableStyles.Length; widthIndex++)
            {
                mSelectableStyles[widthIndex] = new SelectableLineCaps(CustomCaps.SelectableLineCaps[widthIndex]);
            }
        }
        /// <summary>
        /// Set width to the specified one
        /// </summary>
        /// <param name="color"></param>
        private void SetLineCap(ConnectionCap style)
        {
            mCurrentHotTrack = -1;
            mCurrentSelected = -1;
            // Search the width on the known width list
            for (int widthIndex = 0; widthIndex < CustomCaps.SelectableLineCaps.Length; widthIndex++)
            {
                if (style == CustomCaps.SelectableLineCaps[widthIndex])
                {
                    mCurrentSelected = widthIndex;
                    mCurrentHotTrack = -1;
                }
            }
            this.Refresh();
        }
        /// <summary>
        /// Overrides, when mouse move - allow the hot-track look-and-feel
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            // Check cursor, if on one of the buttons - hot track it
            for (int recIndex = 0; recIndex < buttons.Length; recIndex++)
            {
                // Check that current mouse position is in one of the rectangle
                // to have HotTrack effect.
                if (buttons[recIndex].Contains(e.Location))
                {
                    mCurrentHotTrack = recIndex;
                    if (recIndex == 14)
                        lineWidthTip.SetToolTip(this, "Choose as custom line cap...");
                    else
                        lineWidthTip.SetToolTip(this, CustomCaps.SelectableLineCaps[recIndex].ToString());
                }
            }
            this.Refresh();
        }
        /// <summary>
        /// Overrides, when click on, handles color selection.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            // Check cursor, if on one of the buttons - hot track it
            for (int recIndex = 0; recIndex < buttons.Length; recIndex++)
            {
                if (buttons[recIndex].Contains(e.Location))
                {
                    mCurrentSelected = recIndex;


                    //if a custom cap is choosen we need to set the CustomLineCap
                    if (CustomCaps.SelectableLineCaps[recIndex] == ConnectionCap.Generalization)
                        mCustomLineCap = generalizationCap;
                    else
                        mCustomLineCap = null;

                    LineCap = CustomCaps.SelectableLineCaps[recIndex];
                    lineWidthTip.SetToolTip(this, CustomCaps.SelectableLineCaps[recIndex].ToString());

                    if (mContextForm != null)
                        mContextForm.Hide();
                    mContextForm = null;
                }
            }
            this.Refresh();
        }


        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            mCurrentHotTrack = -1;
            Refresh();
        }
        /// <summary>
        /// Override, paint background to white
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            using (Brush brush = new SolidBrush(ArtPalette.ColorPickerBackgroundDocked))
            {
                pevent.Graphics.FillRectangle(brush, pevent.ClipRectangle);
            }
        }

        /// <summary>
        /// Overrides, paint all buttons
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            int x = 10, y = 10;
            int recWidth = 100, recHeight = 18;
            // Go over all mSelectableStyles buttons, paint them
            for (int widthIndex = 0; widthIndex < mSelectableStyles.Length; widthIndex++)
            {
                // Check if current button is selected and/or hottrack
                bool hotTrack = widthIndex == mCurrentHotTrack;
                bool selected = widthIndex == mCurrentSelected;
                // Paints the color button itself, saving the rectangle of the
                // button.
                buttons[widthIndex] = PaintLine(e.Graphics, mSelectableStyles[widthIndex].ConnectionCap, hotTrack, selected, x, y);
                y += recHeight;

            }
            y += 4;

        }

        /// <summary>
        /// Paints one color button
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="color"></param>
        /// <param name="hotTrack"></param>
        /// <param name="selected"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Rectangle PaintLine(Graphics graphics, ConnectionCap style, bool hotTrack, bool selected, int x, int y)
        {
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            // Button inside rectangle
            RectangleF mainRec = new RectangleF(x + 3, y + 3, 100, 1.5F);
            // Button border rectangle 
            Rectangle borderRec = new Rectangle(x - 7, y - 3, 114, 15);
            // Check if the button is selected and HotTrack ( no the same color)
            bool selectedAndHotTrack = selected && hotTrack;
            // Paints the button using the brushes needed
            using (Brush hotTrackBrush = new SolidBrush(ArtPalette.ButtonHoverLight))
            using (Brush selectedBrush = new SolidBrush(ArtPalette.ButtonHoverDark))
            using (Brush selectedHotTrackBrush = new SolidBrush(ArtPalette.SelectedAndHover))
            using (Pen selectedPen = new Pen(ArtPalette.SelectedBorder))
            using (Pen linePen = new Pen(Color.DimGray, 1.7F))
            using (Pen anchorPen = new Pen(Color.DimGray, 5F))
            using (Pen borderPen = new Pen(ArtPalette.ButtonBorder))
            {


                //linePen.CustomEndCap = 
                // Paints the rectangle with the Track/Selected color
                // if this color is selected/hottrack
                if (selectedAndHotTrack)
                {
                    graphics.FillRectangle(selectedHotTrackBrush, borderRec);
                    graphics.DrawRectangle(selectedPen, borderRec);
                }
                else if (hotTrack)
                {
                    graphics.FillRectangle(hotTrackBrush, borderRec);
                    graphics.DrawRectangle(selectedPen, borderRec);
                }
                else if (selected)
                {
                    graphics.FillRectangle(selectedBrush, borderRec);
                    graphics.DrawRectangle(selectedPen, borderRec);
                }
                // Fills the rectangle with the current color, paints
                // the background.    

                graphics.DrawString(style.ToString(), this.Font, Brushes.DimGray, x + 109, y - 1);
                switch (style)
                {
                    case ConnectionCap.ArrowBoth:
                        anchorPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                        graphics.DrawLine(anchorPen, x + 104, y + 4, x + 105, y + 4);
                        anchorPen.StartCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                        anchorPen.EndCap = System.Drawing.Drawing2D.LineCap.NoAnchor;
                        graphics.DrawLine(linePen, x + 2, y + 4, x + 100, y + 4);
                        graphics.DrawLine(anchorPen, x - 5, y + 4, x - 4, y + 4);
                        break;
                    case ConnectionCap.ArrowRight:
                        anchorPen.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                        graphics.DrawLine(linePen, x + 2, y + 4, x + 100, y + 4);
                        graphics.DrawLine(anchorPen, x + 104, y + 4, x + 105, y + 4);
                        break;
                    case ConnectionCap.ArrowLeft:
                        anchorPen.StartCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                        graphics.DrawLine(linePen, x + 2, y + 4, x + 100, y + 4);
                        graphics.DrawLine(anchorPen, x - 5, y + 4, x - 4, y + 4);
                        break;
                    case ConnectionCap.DiamondBoth:
                        graphics.DrawLine(linePen, x + 2, y + 4, x + 100, y + 4);
                        anchorPen.EndCap = System.Drawing.Drawing2D.LineCap.DiamondAnchor;
                        graphics.DrawLine(anchorPen, x + 99, y + 4, x + 100, y + 4);
                        anchorPen.StartCap = System.Drawing.Drawing2D.LineCap.DiamondAnchor;
                        graphics.DrawLine(anchorPen, x, y + 4, x + 1, y + 4);
                        break;
                    case ConnectionCap.DiamondRight:
                        graphics.DrawLine(linePen, x + 2, y + 4, x + 100, y + 4);
                        anchorPen.EndCap = System.Drawing.Drawing2D.LineCap.DiamondAnchor;
                        graphics.DrawLine(anchorPen, x + 99, y + 4, x + 100, y + 4);
                        break;
                    case ConnectionCap.DiamondLeft:
                        graphics.DrawLine(linePen, x + 2, y + 4, x + 100, y + 4);
                        anchorPen.StartCap = System.Drawing.Drawing2D.LineCap.DiamondAnchor;
                        graphics.DrawLine(anchorPen, x - 1, y + 4, x + 1, y + 4);
                        break;
                    case ConnectionCap.RoundBoth:
                        graphics.DrawLine(linePen, x + 2, y + 4, x + 100, y + 4);
                        anchorPen.EndCap = System.Drawing.Drawing2D.LineCap.RoundAnchor;
                        graphics.DrawLine(anchorPen, x + 99, y + 4, x + 100, y + 4);
                        anchorPen.StartCap = System.Drawing.Drawing2D.LineCap.RoundAnchor;
                        graphics.DrawLine(anchorPen, x - 1, y + 4, x + 1, y + 4);
                        break;
                    case ConnectionCap.RoundRight:
                        graphics.DrawLine(linePen, x + 2, y + 4, x + 100, y + 4);
                        anchorPen.EndCap = System.Drawing.Drawing2D.LineCap.RoundAnchor;
                        graphics.DrawLine(anchorPen, x + 99, y + 4, x + 100, y + 4);
                        break;
                    case ConnectionCap.RoundLeft:
                        graphics.DrawLine(linePen, x + 2, y + 4, x + 100, y + 4);
                        anchorPen.StartCap = System.Drawing.Drawing2D.LineCap.RoundAnchor;
                        graphics.DrawLine(anchorPen, x - 1, y + 4, x + 1, y + 4);
                        break;
                    case ConnectionCap.SquareBoth:
                        graphics.DrawLine(linePen, x + 2, y + 4, x + 100, y + 4);
                        anchorPen.EndCap = System.Drawing.Drawing2D.LineCap.SquareAnchor;
                        graphics.DrawLine(anchorPen, x + 99, y + 4, x + 100, y + 4);
                        anchorPen.StartCap = System.Drawing.Drawing2D.LineCap.SquareAnchor;
                        graphics.DrawLine(anchorPen, x - 1, y + 4, x + 1, y + 4);
                        break;
                    case ConnectionCap.SquareRight:
                        graphics.DrawLine(linePen, x + 2, y + 4, x + 100, y + 4);
                        anchorPen.EndCap = System.Drawing.Drawing2D.LineCap.SquareAnchor;
                        graphics.DrawLine(anchorPen, x + 99, y + 4, x + 100, y + 4);
                        break;
                    case ConnectionCap.SquareLeft:
                        graphics.DrawLine(linePen, x + 2, y + 4, x + 100, y + 4);
                        anchorPen.StartCap = System.Drawing.Drawing2D.LineCap.SquareAnchor;
                        graphics.DrawLine(anchorPen, x - 1, y + 4, x + 1, y + 4);
                        break;
                    case ConnectionCap.Generalization:
                        anchorPen.CustomEndCap = generalizationCap;

                        //anchorPen.EndCap = System.Drawing.Drawing2D.LineCap.Custom;
                        anchorPen.StartCap = System.Drawing.Drawing2D.LineCap.NoAnchor;
                        //graphics.DrawLine(anchorPen, x - 1, y + 4, x + 1, y + 4);
                        anchorPen.Width = 2F;
                        graphics.DrawLine(anchorPen, x - 2, y + 4, x + 96, y + 4);
                        break;

                    case ConnectionCap.NoCap:
                        return borderRec;

                }



                //graphics.FillRectangle(Brushes.DimGray, mainRec);
                //graphics.DrawRectangle(borderPen, Rectangle.Round( mainRec));
            }
            return borderRec;
        }
        #endregion
    }
}
