using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Netron.Neon.OfficePickers
{
    /// <summary>
    /// Provides color picker control that could be used in a model or non-model form.
    /// </summary
    [DefaultEvent("SelectedWidthChanged"), DefaultProperty("Width"), ToolboxItem(true) ,
   ToolboxBitmap(typeof(OfficeLineWidthPicker), "LinePickers.OfficeLineWidthPicker"),
    Description("Provides a line width picker control that could be used in a model or non-model form.")]       
    public partial class OfficeLineWidthPicker : UserControl
    {
        #region Static Methods
        /// <summary>
        /// The preferred height to span the control to
        /// </summary>
        public static readonly int PreferredHeight = 138;
        /// <summary>
        /// The preferred width to span the control to
        /// </summary>
        public static readonly int PreferredWidth = 146;

        #endregion

        #region Events
        /// <summary>
        /// Occurs when the value of the LineWidth property changes. 
        /// </summary>
        [Category("Behavior"), Description("Occurs when the value of the LineWidth property changes.")]
        public event EventHandler SelectedWidthChanged;
        #endregion

        #region Properties
        private float mLineWeight = 1F;
        private Color mColor = Color.Black;
        /// <summary>
        /// Gets or sets the selected color from the OfficeLineWidthPicker
        /// </summary>
        [Category("Data"), Description("The color selected in the dialog"),
       DefaultValue(typeof(float), "1F")]
        public float LineWidth
        {
            get { return mLineWeight; }
            set 
            { 
                mLineWeight = value;
                // Set Selected LineWidth in the GUI
                SetLineWidth(value);
                // Fires the SelectedWidthChanged event.
                OnSelectedWeightChanged(EventArgs.Empty);                
            }
        }
      
        /// <summary>
        /// Gets the selected color name, or 'Custom' if it is not one
        /// of the Selectable mSelectableWidths.
        /// </summary>
        [Browsable(false)]
        public string LineWeightName
        {
            get
            {
                string mWidthName = "Custom weight (" + mLineWeight + " px)";
                if (mCurrentSelected > -1 && mCurrentSelected < CustomLineWeights.SelectableLineWeights.Length)
                {
                    mWidthName = CustomLineWeights.SelectableLineWeights[mCurrentSelected].ToString() +" px";
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
        /// Known mSelectableWidths list that user may select from 
        /// </summary>
        private SelectableLineWidths[] mSelectableWidths = new SelectableLineWidths[5];
        /// <summary>
        /// Buttons rectangle definitions.
        /// </summary>
        private Rectangle[] buttons = new Rectangle[6];
        /// <summary>
        /// Hot Track index to paint its button with HotTrack color
        /// </summary>
        private int mCurrentHotTrack = -1;
        /// <summary>
        /// Current selected index to paint its button with Selected color
        /// </summary>
        private int mCurrentSelected = -1;
        #endregion

        #region Ctor

        /// <summary>
        /// Initialized a new instance of the OfficeLineWidthPicker in order to provide 
        /// color picker control that could be used in a model or non-model form.
        /// </summary>
        public OfficeLineWidthPicker()
        {
            InitializeComponent();
            SetWidthObjects();
            // Set painting style for better performance.
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);
        }

        /// <summary>
        /// Initialized a new instance of the OfficeLineWidthPicker in order to provide 
        /// color picker control that could be used in a model or non-model form.   
        /// </summary>
        /// <param name="startingColor">Starting color to the OfficeLineWidthPicker control</param>
        public OfficeLineWidthPicker(float startingWidth)
            : this()
        {
            LineWidth = startingWidth;
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
            mContextForm.Height = OfficeLineWidthPicker.PreferredHeight;
            mContextForm.Show(this, startLocation, OfficeLineWidthPicker.PreferredWidth);
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
            frm.Height = OfficeLineWidthPicker.PreferredHeight;
            mContextForm = frm;
            frm.Show(parent, startLocation, OfficeLineWidthPicker.PreferredWidth);
        }
        /// <summary>
        /// Fires the OfficeLineWidthPicker.SelectedWidthChanged event
        /// </summary>
        /// <param name="e"></param>
        public void OnSelectedWeightChanged(EventArgs e)
        {
            Refresh();
            if (SelectedWidthChanged != null)
                SelectedWidthChanged(this, e);
        }

        #endregion

        #region Private and protected methods

        /// <summary>
        /// Creates the custom SelectableLineWeights buttons
        /// </summary>
        private void SetWidthObjects()
        {
            for (int widthIndex = 0; widthIndex < mSelectableWidths.Length; widthIndex++)
            {
                mSelectableWidths[widthIndex] = new SelectableLineWidths(CustomLineWeights.SelectableLineWeights[widthIndex]);
            }
        }
        /// <summary>
        /// Set width to the specified one
        /// </summary>
        /// <param name="color"></param>
        private void SetLineWidth(float width)
        {
            mCurrentHotTrack = -1;
            mCurrentSelected = -1;
            // Search the width on the known width list
            for (int widthIndex = 0; widthIndex < CustomLineWeights.SelectableLineWeights.Length; widthIndex++)
            {
                if (width== CustomLineWeights.SelectableLineWeights[widthIndex])
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
                    if(recIndex==5)
                        lineWidthTip.SetToolTip(this, "Choose as custom line weight...");
                    else
                         lineWidthTip.SetToolTip(this, CustomLineWeights.SelectableLineWeights[recIndex].ToString() + " px");
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
                    // More mSelectableWidths - open dialog
                    if (mCurrentSelected == 5)
                    {
                        LineWidth = OpenMoreLinesDialog();
                    }
                    else
                    {
                        LineWidth = CustomLineWeights.SelectableLineWeights[recIndex];
                        lineWidthTip.SetToolTip(this, CustomLineWeights.SelectableLineWeights[recIndex].ToString());
                    }
                    if (mContextForm != null)
                        mContextForm.Hide();
                    mContextForm = null;                   
                }
            }
            this.Refresh();
        }
        /// <summary>
        /// Open the 'More Weights' dialog, that is, a normal ColorDialog control.
        /// </summary>
        /// <returns></returns>
        private float OpenMoreLinesDialog()
        {
            mLineWidthDialog.LineWidth = LineWidth;
           
            Form parentForm = this.FindForm();
            ContextMenuForm contextFrm = parentForm as ContextMenuForm;
            if (contextFrm != null)
            {
                // Ignore lost focus events on owner context menu form
                contextFrm.Locked = true;
                mLineWidthDialog.ShowDialog(contextFrm);
                // Give the focus back to owner form
                if (mParentControl != null)
                {
                    mParentControl.FindForm().BringToFront();
                }               
                // Active lost focus events on owner context menu form
                contextFrm.Locked = false;
            }
            else
            {
                mLineWidthDialog.ShowDialog(this); // (parentForm);            
            }
            return mLineWidthDialog.LineWidth;
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
            using(Brush brush = new SolidBrush(ArtPalette.ColorPickerBackgroundDocked))
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
            int x = 5, y = 10;
            int recWidth = 100, recHeight = 18;
            // Go over all mSelectableWidths buttons, paint them
            for (int widthIndex = 0; widthIndex < mSelectableWidths.Length; widthIndex++)
            {
                // Check if current button is selected and/or hottrack
                bool hotTrack = widthIndex == mCurrentHotTrack;
                bool selected = widthIndex == mCurrentSelected;
                // Paints the color button itself, saving the rectangle of the
                // button.
                buttons[widthIndex] =  PaintLine(e.Graphics, mSelectableWidths[widthIndex].Width, hotTrack, selected, x, y);
                y += recHeight;
                
            }
            y += 4;
            PaintMoreLinesButton(e.Graphics, x, y);
        }
        /// <summary>
        /// Paints the more mSelectableWidths button
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        protected void PaintMoreLinesButton(Graphics graphics, int x, int y)
        {
            // The rectangle for the 'More Colors' button
            Rectangle buttonRec = new Rectangle(x, y, 127, 22);
            // The text will be displayed in the center of the rectangle
            // using format string.
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            Font buttonFont = new Font("Arial", 8);
            bool selected = mCurrentSelected == 5;
            bool hotTrack = mCurrentHotTrack == 5;
           
            // Paints the button with the selected, hot track settings if needed
            using (Brush hotTrackBrush = new SolidBrush(ArtPalette.ButtonHoverLight))
            using (Brush selectedBrush = new SolidBrush(ArtPalette.ButtonHoverDark))
            using (Pen selectedPen = new Pen(ArtPalette.SelectedBorder))
            using (Pen borderPen = new Pen(ArtPalette.ButtonBorder))
            {
                if (hotTrack)
                {
                    graphics.FillRectangle(hotTrackBrush, buttonRec);
                    graphics.DrawRectangle(selectedPen, buttonRec);
                }
                else if (selected)
                {
                    graphics.FillRectangle(selectedBrush, buttonRec);
                    graphics.DrawRectangle(selectedPen, buttonRec);
                }
            }
            //  Draw the string itself using the rectangle, font and format specified.
            graphics.DrawString("Custom weight...", buttonFont, Brushes.Black, buttonRec, format);
            format.Dispose();
            buttonFont.Dispose();
            buttons[5] = buttonRec;         
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
        private Rectangle PaintLine(Graphics graphics, float width, bool hotTrack, bool selected, int x, int y)
        {            
            // Button inside rectangle
            RectangleF mainRec = new RectangleF(x + 3, y + 3, 100, width);
            // Button border rectangle 
            Rectangle borderRec = new Rectangle(x, y, 102, 10);
            // Check if the button is selected and HotTrack ( no the same color)
            bool selectedAndHotTrack = selected && hotTrack;
            // Paints the button using the brushes needed
            
            using (Brush hotTrackBrush = new SolidBrush(ArtPalette.ButtonHoverLight))
            using (Brush selectedBrush = new SolidBrush(ArtPalette.ButtonHoverDark))
            using (Brush selectedHotTrackBrush = new SolidBrush(ArtPalette.SelectedAndHover))
            using (Pen selectedPen = new Pen(ArtPalette.SelectedBorder))
            using (Pen linePen = new Pen(Color.DimGray, width))
            using (Pen borderPen = new Pen(ArtPalette.ButtonBorder))
            {
                
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
                graphics.DrawLine(linePen, x+2, y+4, x+100, y+4);
                graphics.DrawString(width + " px", this.Font, Brushes.DimGray, x + 104, y - 1);
                //graphics.FillRectangle(Brushes.DimGray, mainRec);
                //graphics.DrawRectangle(borderPen, Rectangle.Round( mainRec));
            }
            return borderRec;
        }
        #endregion
    }
  }
