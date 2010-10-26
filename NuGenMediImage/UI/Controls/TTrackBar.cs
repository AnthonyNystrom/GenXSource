using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.VisualStyles;
using System.Drawing.Imaging;

namespace Attilan.Crystal.Controls
{
    /// <summary>
    /// Represents visual thumb states for CrystalTrackBar
    /// </summary>
    public enum CrystalThumbState
    {
        
        /// <summary>
        /// The mouse is outside the CrystalTrackBar control.
        /// </summary>
        Normal = 0,
        /// <summary>
        /// user has clicked on the thumb inside the CrystalTrackBar control.
        /// </summary>
        Pressed = 1,
        /// <summary>
        /// The thumb inside the CrystalTrackBar control is being erased.
        /// </summary>
        Erased = 2,
        /// <summary>
        /// The mouse has entered the CrystalTrackBar control.
        /// </summary>
        Hover = 3
    }

    /// <summary>
    /// CrystalTrackBar is a TrackBar replacement that can have a gradient or
    /// transparent background.
    /// </summary>
    [System.ComponentModel.DesignerCategory("code")]
    [
    ToolboxItem(true),
    ToolboxBitmap(typeof(resfinder), "CrystalToolkit.Resources.CrystalTrackBar.bmp")
        //ToolboxBitmap(typeof(System.Windows.Forms.TrackBar))
    ]
    public class CrystalTrackBar : CrystalGradientControl
    {
        #region Fields

        // rects for trackbar, tickbar, and thumb
        private Rectangle _trackRectangle = new Rectangle();
        private Rectangle _ticksRectangleBottomRight = new Rectangle();
        private Rectangle _ticksRectangleTopLeft = new Rectangle();
        private RectangleF _thumbRectangle = new Rectangle();

        // tick info
        private int _value = 0;
        private int _currentTickValue = 0;
        private int _numberTicks = 11;
        private int _minimum = 0;
        private int _maximum = 10;
        private int _tickFrequency = 1;
        private float _tickSpace = 0;
        private TickStyle _tickStyle = TickStyle.BottomRight;
        private EdgeStyle _tickEdgeStyle = EdgeStyle.Etched;

        private List<int> _tickValues = null;

        // states
        private CrystalThumbState _thumbState = CrystalThumbState.Normal;
        private Orientation _orientation = Orientation.Horizontal;

        // track bar
        private int _trackBarMargin = 10;
        private int _trackBarHeight = 4;

        // thumb images
        private Image _thumbImage = null;
        private Image _thumbHotImage = null;
        private Image _thumbWarmImage = null;

        // keyboard input
        private bool _keyboardControl = true;

        #endregion

        #region Events

        /// <summary>
        /// Any subscribers to the event handler will be called when CrystalTrackBar's value has changed.
        /// </summary>
        public event EventHandler ValueChanged;

        #endregion

        #region Public Properties

        /// <summary>
        /// Indicates where the ticks appear on the CrystalTrackBar.
        /// </summary>
        [
        Browsable(true),
        Category("Appearance"),
        DefaultValue(TickStyle.BottomRight),
        Description("Indicates where the ticks appear on the CrystalTrackBar.")
        ]
        public TickStyle TickStyle
        {
            get { return _tickStyle; }
            set
            {
                if (_tickStyle != value)
                {
                    _tickStyle = value;
                    RedrawControl();
                }
            }
        }

        /// <summary>
        /// The position of the thumb on the slider.
        /// </summary>
        [
        Bindable(true),
        Browsable(true),
        Category("Behavior"),
        DefaultValue(0),
        Description("The position of the thumb on the slider.")
        ]
        public int Value
        {
            get { return _value; }
            set
            {
                if (value < _minimum || value > _maximum)
                    throw new ArgumentException(
                        string.Format("'{0}' is not a valid value for 'Value'. 'Value' should be between '{1}' and '{2}'", value, _minimum, _maximum));

                // BUG FIX: Process part of this code even with value change,
                // to make the thumb display properly.

                // Cache the boolean that tells us if the value has changed.
                bool valChanged = (_value != value);

                // save the new value
                _value = value;

                // The given Value must be aligned with a value in the ticks list
                if (valChanged)
                    AdjustValue();

                // Update subscribers when all of the above is done.
                if ((ValueChanged != null) && (valChanged))
                    ValueChanged(this, new EventArgs());

                // Recalc the thumb position                 
                UpdateThumbRectangle();

                if (!IsThumbPressed())
                {
                    // BUG FIX: Can't ask parent to invalidate our rectangle here
                    // when TransparencyMode = true;
                    //InvalidateEx();
                    Rectangle rc = new Rectangle(this.Location, this.Size);
                    base.InvalidateEx(rc);
                }
            }
        }

        /// <summary>
        /// The maximum value for the thumb position of the slider of the CrystalTrackBar.
        /// </summary>
        [
        Browsable(true),
        Category("Behavior"),
        DefaultValue(10),
        Description("The maximum value for the thumb position of the slider of the CrystalTrackBar.")
        ]
        public int Maximum
        {
            get { return _maximum; }
            set
            {
                if (_maximum != value)
                {
                    _maximum = value;
                    CalcTicks();
                    RedrawControl();
                }
            }
        }

        /// <summary>
        /// The minimum value for the thumb position of the slider of the CrystalTrackBar.
        /// </summary>
        [
        Browsable(true),
        Category("Behavior"),
        DefaultValue(0),
        Description("The minimum value for the thumb position of the slider of the CrystalTrackBar.")
        ]
        public int Minimum
        {
            get { return _minimum; }
            set
            {
                if (_minimum != value)
                {
                    _minimum = value;
                    CalcTicks();
                    RedrawControl();
                }
            }
        }

        /// <summary>
        /// Indicates the interval between ticks on the CrystalTrackBar.
        /// </summary>
        [
        Browsable(true),
        Category("Appearance"),
        DefaultValue(1),
        Description("Indicates the interval between ticks on the CrystalTrackBar.")
        ]
        public int TickFrequency
        {
            get { return _tickFrequency; }
            set
            {
                if (_tickFrequency != value)
                {
                    _tickFrequency = value;
                    CalcTicks();
                    RedrawControl();
                }
            }
        }

        /// <summary>
        /// A list of integer values for each tick item on trackbar.
        /// </summary>
        [
        Browsable(false),
        Description("A list (List<int>) of integer values for each tick item on trackbar.")
        ]
        public List<int> TickValues
        {
            get { return _tickValues; }
        }

        /// <summary>
        /// Allows the user to adjust trackbar with the keys: left arrow, right arrow, up, down, home, end.
        /// </summary>
        [
        Browsable(true),
        Category("Behavior"),
        DefaultValue(true),
        Description("Allows the user to adjust trackbar with the keys: left arrow, right arrow, up, down, home, end.")
        ]
        public bool KeyboardControl
        {
            get { return _keyboardControl; }
            set { _keyboardControl = value; }
        }

        /// <summary>
        /// Allows the CrystalTrackBar to be displayed horizontally or vertically.
        /// </summary>
        [
        Browsable(true),
        Category("Appearance"),
        Description("Allows the CrystalTrackBar to be displayed horizontally or vertically.")
        ]
        public Orientation Orientation
        {
            get { return _orientation; }
            set
            {
                if (_orientation != value)
                {
                    _orientation = value;

                    // flip the size
                    this.Size = new Size(Size.Height, Size.Width);
                }
            }
        }

        /// <summary>
        /// Determines the appearance of the ticks on the CrystalTrackBar.
        /// </summary>
        [
        Browsable(true),
        Category("Appearance"),
        Description("Determines the appearance of the ticks on the CrystalTrackBar.")
        ]
        public EdgeStyle TickEdgeStyle
        {
            get { return _tickEdgeStyle; }
            set
            {
                if (_tickEdgeStyle != value)
                {
                    _tickEdgeStyle = value;
                    RedrawControl();
                }
            }
        }

        #endregion

        #region Constructors and Initializers

        /// <summary>
        /// Default constructor for CrystalTrackBar.
        /// </summary>
        public CrystalTrackBar()
        {
            // this call says "I'll draw it myself"
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
            ControlStyles.ResizeRedraw |
            ControlStyles.UserPaint, true);

            SetDefaultSize();
        }

        /// <summary>
        /// Sets the default size for the control, used for display in the Forms designer.
        /// </summary>
        protected virtual void SetDefaultSize()
        {
            // Set default size so that user will see the control in the designer
            if (IsHorizontal())
                this.Size = new Size(105, 45);
            else
                this.Size = new Size(45, 105);
        }

        #endregion

        #region Tick Methods

        /// <summary>
        /// Calculates the tick intervals based on the Minimum, Maximum, and TickFrequency values.
        /// </summary>
        protected virtual void CalcTicks()
        {
            _numberTicks = 0;
            _tickValues = new List<int>();
            for (int tickCount = _minimum; tickCount <= _maximum; tickCount += _tickFrequency)
            {
                _tickValues.Add(tickCount);
                _numberTicks++;
            }

            // Make sure that maximum value is the last value.
            _tickValues.RemoveAt(_numberTicks - 1);
            _tickValues.Add(_maximum);

            AdjustValue();
        }

        /// <summary>
        /// Adjusts the current Value to match the closest tick value.
        /// </summary>
        protected void AdjustValue()
        {
            if (_tickValues == null)
                return;

            _currentTickValue = 0;
            foreach (int tickValue in _tickValues)
            {
                if (tickValue < _value)
                {
                    _currentTickValue++;
                }
                else
                {
                    _value = tickValue;
                    break;
                }
            }
        }

        /// <summary>
        /// Draws the ticks using the TrackBarRenderer.
        /// </summary>
        /// <param name="gfx">Graphics object.</param>
        private void DrawRenderTicks(Graphics gfx)
        {
            if (TickStyle != TickStyle.None)
            {
                if (IsHorizontal())
                {
                    if ((_tickStyle == TickStyle.Both) || (_tickStyle == TickStyle.BottomRight))
                        TrackBarRenderer.DrawHorizontalTicks(gfx, _ticksRectangleBottomRight, _numberTicks, _tickEdgeStyle);

                    if ((_tickStyle == TickStyle.Both) || (_tickStyle == TickStyle.TopLeft))
                        TrackBarRenderer.DrawHorizontalTicks(gfx, _ticksRectangleTopLeft, _numberTicks, _tickEdgeStyle);
                }
                else
                {
                    if ((_tickStyle == TickStyle.Both) || (_tickStyle == TickStyle.BottomRight))
                        TrackBarRenderer.DrawVerticalTicks(gfx, _ticksRectangleBottomRight, _numberTicks, _tickEdgeStyle);

                    if ((_tickStyle == TickStyle.Both) || (_tickStyle == TickStyle.TopLeft))
                        TrackBarRenderer.DrawVerticalTicks(gfx, _ticksRectangleTopLeft, _numberTicks, _tickEdgeStyle);
                }
            }
        }

        /// <summary>
        /// Draws the Ticks using a line, horizontally.
        /// </summary>
        /// <param name="gfx">Graphics object.</param>
        /// <param name="ticksRect">Ticks rectangle.</param>
        private void DrawHorizLineTicks(Graphics gfx, Rectangle ticksRect)
        {
            for (int index = 0; index < _numberTicks; index++)
            {
                // Calculate where the current tick value X coordinate is.
                float xTick = ticksRect.X;
                xTick += (_tickSpace * (index));

                using (Pen linePen = new Pen(Color.White, 2))
                {
                    linePen.EndCap = LineCap.Round;
                    gfx.DrawLine(linePen, xTick,
                                    ticksRect.Y,
                                    xTick,
                                    ticksRect.Y + ticksRect.Height);
                }
            }
        }

        /// <summary>
        /// Draws the Ticks using a line, vertically.
        /// </summary>
        /// <param name="gfx">Graphics object.</param>
        /// <param name="ticksRect">Ticks rectangle.</param>
        private void DrawVertLineTicks(Graphics gfx, Rectangle ticksRect)
        {
            for (int index = 0; index < _numberTicks; index++)
            {
                // Calculate where the current tick value X coordinate is.
                float yTick = ticksRect.Y;
                yTick += (_tickSpace * (index));

                float xTick = ticksRect.X;
                //float xTick = ticksRect.X - 2;

                using (Pen linePen = new Pen(Color.White, 2))
                {
                    linePen.EndCap = LineCap.Round;
                    gfx.DrawLine(linePen, xTick,
                                    yTick,
                                    xTick + ticksRect.Width,
                                    yTick);
                }
            }
        }

        /// <summary>
        /// Draws the Ticks using a dash line.
        /// </summary>
        /// <param name="gfx">Graphics object.</param>
        private void DrawLineTicks(Graphics gfx)
        {
            if (TickStyle != TickStyle.None)
            {
                if (IsHorizontal())
                {
                    if ((_tickStyle == TickStyle.Both) || (_tickStyle == TickStyle.BottomRight))
                        DrawHorizLineTicks(gfx, _ticksRectangleBottomRight);

                    if ((_tickStyle == TickStyle.Both) || (_tickStyle == TickStyle.TopLeft))
                        DrawHorizLineTicks(gfx, _ticksRectangleBottomRight);
                }
                else
                {
                    if ((_tickStyle == TickStyle.Both) || (_tickStyle == TickStyle.BottomRight))
                        DrawVertLineTicks(gfx, _ticksRectangleBottomRight);

                    if ((_tickStyle == TickStyle.Both) || (_tickStyle == TickStyle.TopLeft))
                        DrawVertLineTicks(gfx, _ticksRectangleTopLeft);
                }
            }
        }

        /// <summary>
        /// Draws the ticks above, below, left, or right of the trackbar.
        /// </summary>
        /// <param name="gfx">The Graphics object to draw on.</param>
        protected virtual void DrawTicks(Graphics gfx)
        {
            if (TrackBarRenderer.IsSupported)
                DrawRenderTicks(gfx);
            else
                DrawLineTicks(gfx);

        }

        /// <summary>
        /// Calculates the current X tick coordinate for a horizontal CrystalTrackBar.
        /// </summary>
        /// <returns></returns>
        protected float CurrentTickXCoordinate()
        {
            // Calculate where the current tick value X coordinate is.
            float xTick = (_tickSpace * (_currentTickValue));
            xTick += _trackBarMargin;

            // Take half the width of the thumb...
            float halfThumb = _thumbRectangle.Width / 2;

            // ...subtract this from the current X tick and return
            return xTick - halfThumb;
        }

        /// <summary>
        /// Calculates the current Y tick coordinate for a horizontal CrystalTrackBar.
        /// </summary>
        /// <returns></returns>
        protected float CurrentTickYCoordinate()
        {
            // Calculate where the current tick value X coordinate is.
            float yTick = (_tickSpace * (_currentTickValue));
            yTick += _trackBarMargin;

            // Take half the height of the thumb...
            float halfThumb = _thumbRectangle.Height / 2;

            // ...subtract this from the current Y tick and return
            return yTick - halfThumb;
        }

        /// <summary>
        /// Returns the current tick coordinate based on the orientation of the CrystalTrackBar.
        /// </summary>
        /// <returns>Returns the current tick coordinate.</returns>
        protected virtual float CurrentThumbTick()
        {
            if (IsHorizontal())
                return CurrentTickXCoordinate();
            else
                return CurrentTickYCoordinate();
        }

        #endregion

        #region TrackBar Methods

        /// <summary>
        /// Draws the TrackBar using TrackBarRenderer
        /// </summary>
        /// <param name="gfx">The Graphics object to draw on.</param>
        private void DrawRendererTrackBar(Graphics gfx)
        {
            if (IsHorizontal())
                TrackBarRenderer.DrawHorizontalTrack(gfx, _trackRectangle);
            else
                TrackBarRenderer.DrawVerticalTrack(gfx, _trackRectangle);
        }

        /// <summary>
        /// Draws the TrackBar using a line.
        /// </summary>
        /// <param name="gfx">The Graphics object to draw on.</param>
        private void DrawLineTrackBar(Graphics gfx)
        {
            if (IsHorizontal())
            {
                using (Pen linePen = new Pen(Color.White, 4))
                {
                    linePen.EndCap = LineCap.Round;
                    gfx.DrawLine(linePen, _trackRectangle.X, _trackRectangle.Y + 2,
                                    _trackRectangle.X + _trackRectangle.Width,
                                    _trackRectangle.Y + 2);
                }
                using (Pen linePen = new Pen(Color.Gray, 2))
                {
                    linePen.EndCap = LineCap.Round;
                    gfx.DrawLine(linePen, _trackRectangle.X, _trackRectangle.Y,
                                    _trackRectangle.X + _trackRectangle.Width,
                                    _trackRectangle.Y);
                }
            }
            else
            {
                using (Pen linePen = new Pen(Color.White, 4))
                {
                    linePen.EndCap = LineCap.Round;
                    gfx.DrawLine(linePen, _trackRectangle.X + 2, _trackRectangle.Y,
                                    _trackRectangle.X + 2,
                                    _trackRectangle.Y + _trackRectangle.Height);
                }
                using (Pen linePen = new Pen(Color.Gray, 2))
                {
                    linePen.EndCap = LineCap.Round;
                    gfx.DrawLine(linePen, _trackRectangle.X, _trackRectangle.Y,
                                    _trackRectangle.X,
                                    _trackRectangle.Y + _trackRectangle.Height);
                }
            }
        }

        /// <summary>
        /// Draws the solid track bar line.
        /// </summary>
        /// <param name="gfx">The Graphics object to draw on.</param>
        protected virtual void DrawTrackBar(Graphics gfx)
        {
            if (!TrackBarRenderer.IsSupported)
                DrawLineTrackBar(gfx);
            else
                DrawRendererTrackBar(gfx);
        }

        /// <summary>
        /// Calculates the sizes of the bar, thumb, and ticks rectangle for a horizontal track bar.
        /// </summary>
        private void SetupHorizontalTrackBar()
        {
            using (Graphics g = this.CreateGraphics())
            {
                _trackRectangle.X = ClientRectangle.X + _trackBarMargin;
                _trackRectangle.Y = ClientRectangle.Height / 2;
                _trackRectangle.Width = ClientRectangle.Width - (_trackBarMargin * 2);
                _trackRectangle.Height = _trackBarHeight;

                // Calculate the size of the rectangle in which to 
                // draw the ticks.
                _ticksRectangleBottomRight.X = _trackRectangle.X;
                _ticksRectangleBottomRight.Y = _trackRectangle.Y + 8; // ticks on the bottom
                _ticksRectangleBottomRight.Width = _trackRectangle.Width;
                _ticksRectangleBottomRight.Height = 4;

                _ticksRectangleTopLeft = _ticksRectangleBottomRight;
                _ticksRectangleTopLeft.Y = _trackRectangle.Y - 8;  // ticks on the Top

                _tickSpace = ((float)_ticksRectangleBottomRight.Width - 1) /
                    ((float)_numberTicks - 1);

                // Calculate the size of the thumb.
                // w = 11.0, h = 21.0
                if (TrackBarRenderer.IsSupported)
                {
                    _thumbRectangle.Size =
                        TrackBarRenderer.GetBottomPointingThumbSize(g,
                        TrackBarThumbState.Normal);
                }
                else
                {
                    _thumbRectangle.Size = new SizeF(11.0f, 21.0f);
                }

                UpdateThumbRectangle();
                _thumbRectangle.Y = _trackRectangle.Y - 8;
            }
        }

        /// <summary>
        /// Calculates the sizes of the bar, thumb, and ticks rectangle for a vertical track bar.
        /// </summary>
        private void SetupVerticalTrackBar()
        {
            using (Graphics g = this.CreateGraphics())
            {
                _trackRectangle.X = ClientRectangle.Width / 2;
                _trackRectangle.Y = ClientRectangle.Y + _trackBarMargin;
                _trackRectangle.Width = _trackBarHeight;
                _trackRectangle.Height = ClientRectangle.Height - (_trackBarMargin * 2);

                // Calculate the size of the rectangle in which to 
                // draw the ticks.
                _ticksRectangleBottomRight.Y = _trackRectangle.Y;
                _ticksRectangleBottomRight.X = _trackRectangle.X + 8; // ticks on the right
                _ticksRectangleBottomRight.Width = 4;
                _ticksRectangleBottomRight.Height = _trackRectangle.Height;

                _ticksRectangleTopLeft = _ticksRectangleBottomRight;
                _ticksRectangleTopLeft.X = _trackRectangle.X - 8; // ticks on the left

                _tickSpace = ((float)_ticksRectangleBottomRight.Height - 1) /
                    ((float)_numberTicks - 1);

                // Calculate the size of the thumb.
                // w = 21.0, h = 11.0
                if (TrackBarRenderer.IsSupported)
                {
                    _thumbRectangle.Size =
                        TrackBarRenderer.GetRightPointingThumbSize(g,
                        TrackBarThumbState.Normal);
                }
                else
                {
                    _thumbRectangle.Size = new SizeF(21.0f, 11.0f);
                }

                UpdateThumbRectangle();
                _thumbRectangle.X = _trackRectangle.X - 8;
            }
        }

        /// <summary>
        /// Calculates the sizes of the bar, thumb, and ticks rectangle.
        /// </summary>
        protected virtual void SetupTrackBar()
        {
            CalcTicks();

            if (IsHorizontal())
                SetupHorizontalTrackBar();
            else
                SetupVerticalTrackBar();
        }

        /// <summary>
        /// Tells the CrystalTrackBar that it needs to repaint the entire client area.
        /// </summary>
        public virtual void RedrawTrackBar()
        {
            Rectangle rc = new Rectangle(this.Location, this.Size);
            base.InvalidateEx(rc);
        }


        #endregion

        #region Thumb Methods

        /// <summary>
        /// Gets the string containing the resource name of the thumb image matching the "Normal" state.
        /// </summary>
        /// <returns>The string containing the resource name of the image.</returns>
        protected Bitmap GetNormalThumbImageName()
        {            
            Bitmap b = new Bitmap(18, 18);
            Graphics g = Graphics.FromImage(b);

            g.Clear(Color.Transparent);
            
            Rectangle rect= new Rectangle(0,0,b.Width,b.Height);

            LinearGradientBrush lgb = new LinearGradientBrush(rect, this.Color1, this.Color2, LinearGradientMode.Horizontal);

            g.FillEllipse(lgb,rect);            

            lgb.Dispose();
            g.Dispose();

            return b;
        }

        /// <summary>
        /// Gets the string containing the resource name of the thumb image matching the "Hover" state.
        /// </summary>
        /// <returns>The string containing the resource name of the image.</returns>
        protected Bitmap GetWarmThumbImageName()
        {
            return GetNormalThumbImageName();
        }

        /// <summary>
        /// Gets the string containing the resource name of the thumb image matching the "Pressed" state.
        /// </summary>
        /// <returns>The string containing the resource name of the image.</returns>
        protected Bitmap GetHotThumbImageName()
        {
            return GetNormalThumbImageName();
        }

        /// <summary>
        /// Gets the thumb image name that matches the given state.
        /// </summary>
        /// <param name="thumbState">The state of the desired thumb image.</param>
        /// <returns></returns>
        protected Bitmap GetThumbImageName(CrystalThumbState thumbState)
        {
            switch (thumbState)
            {
                case CrystalThumbState.Pressed:
                    return GetHotThumbImageName();
                case CrystalThumbState.Hover:
                    return GetWarmThumbImageName();
                default:
                    return GetNormalThumbImageName();
            }
        }

        /// <summary>
        /// Gets the image field matching a given thumb state.
        /// </summary>
        /// <param name="thumbState">The desired thumb state.</param>
        /// <returns>The Image matching the thumb state.</returns>
        protected Image GetCachedThumbImage(CrystalThumbState thumbState)
        {
            switch (thumbState)
            {
                case CrystalThumbState.Pressed:
                    return _thumbHotImage;
                case CrystalThumbState.Hover:
                    return _thumbWarmImage;
                default:
                    return _thumbImage;
            }
        }

        /// <summary>
        /// Sets the image field matching a given thumb state, allowing us to cache the image.
        /// </summary>
        /// <param name="thumbState">The desired thumb state.</param>
        /// <param name="thumbImage">The image to cache.</param>
        protected void SetCachedThumbImage(CrystalThumbState thumbState, Image thumbImage)
        {
            switch (thumbState)
            {
                case CrystalThumbState.Pressed:
                    _thumbHotImage = thumbImage;
                    break;
                case CrystalThumbState.Hover:
                    _thumbWarmImage = thumbImage;
                    break;
                default:
                    _thumbImage = thumbImage;
                    break;
            }
        }

        /// <summary>
        /// Gets the thumb image matching the current thumb state.
        /// </summary>
        /// <returns>The image matching the current thumb state.</returns>
        protected Image GetThumbImage()
        {
            Image thumbImage = GetCachedThumbImage(_thumbState);

            if (thumbImage == null)
            {
                thumbImage = GetThumbImageName(_thumbState);

                ((Bitmap)thumbImage).MakeTransparent(Color.Fuchsia);

                SetCachedThumbImage(_thumbState, thumbImage);
            }

            return thumbImage;
        }

        /// <summary>
        /// Draws the image of the thumb on the CrystalTrackBar.
        /// </summary>
        /// <param name="gfx">The Graphics object to draw on.</param>
        /// <param name="thumbRect">The rectangle area where the thumb should be drawn.</param>
        protected void DrawThumbImage(Graphics gfx, RectangleF thumbRect)
        {
            Image thumbImage = GetThumbImage();

            if (thumbImage != null)
            {
                if (IsHorizontal())
                {
                    // The thumb images in our assembly are smaller than the
                    // actual thumb rect supplied by TrackBarRenderer.
                    // Drawing smaller than the system rect allows us to erase
                    // the thumb easier.
                    thumbRect.Height = thumbImage.Height;
                    thumbRect.Width = thumbImage.Width;
                }
                else
                {
                    // If the track bar is vertical, we're going to take
                    // the horizontal thumb image and rotate it to match
                    // the tickstyle.
                    Bitmap flipImage = new Bitmap(thumbImage);

                    if (_tickStyle == TickStyle.TopLeft)
                        flipImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    else
                        flipImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    thumbImage = flipImage;

                    // The thumb images in our assembly are smaller than the
                    // actual thumb rect supplied by TrackBarRenderer.
                    // Drawing smaller than the system rect allows us to erase
                    // the thumb easier.
                    thumbRect.Height = thumbImage.Height;
                    thumbRect.Width = thumbImage.Width;
                }

                gfx.DrawImage(thumbImage, thumbRect);
            }
        }

        /// <summary>
        /// Draws the thumb on the CrystalTrackBar.
        /// </summary>
        /// <param name="gfx">The Graphics object to draw on.</param>
        protected virtual void DrawThumb(Graphics gfx)
        {
            // if the thumb is getting erased, we just want to draw
            // the track bar and ticks.  A second repaint will have us
            // draw the thumb in an updated position later.
            if (IsThumbErasing())
                return;

            DrawThumbImage(gfx, _thumbRectangle);
        }

        /// <summary>
        /// Erases the thumb from the CrystalTrackBar.
        /// </summary>
        protected virtual void EraseThumb()
        {
            CrystalThumbState oldThumbState = _thumbState;
            _thumbState = CrystalThumbState.Erased;

            // This will force a repaint in the base class...
            InvalidateEx(_thumbRectangle);

            //...by the time we reach here, we know that the
            // trackbar and ticks have been repainted without the thumb.
            _thumbState = oldThumbState;
        }

        /// <summary>
        /// Updates the thumb using the current tick value of the CrystalTrackBar.
        /// </summary>
        private void UpdateThumbRectangle()
        {
            if (IsHorizontal())
                _thumbRectangle.X = CurrentTickXCoordinate();
            else
                _thumbRectangle.Y = CurrentTickYCoordinate();
        }

        #endregion

        #region Helper Methods: States, Tick Adjustment, etc.

        /// <summary>
        /// Returns the current X or Y location depending on the orientation.
        /// </summary>
        /// <param name="location">A point within the CrystalTrackBar.</param>
        /// <returns>Returns the current X or Y location depending on the orientation.</returns>
        protected float CurrentLocation(Point location)
        {
            if (IsHorizontal())
                return location.X;
            else
                return location.Y;
        }

        /// <summary>
        /// Determines if the current thumb position on the track bar has changed.
        /// </summary>
        /// <param name="location">The current point location of the mouse.</param>
        /// <returns>Boolean indicating that the thumb position has changed.</returns>
        private bool HasTrackValueChanged(Point location)
        {
            if (_currentTickValue < (_numberTicks - 1) &&
                CurrentLocation(location) > CurrentThumbTick() +
                (int)(_tickSpace))
            {
                return true;
            }

            // Track movements to the next tick to the left, if 
            // cursor has moved halfway to the next tick.
            else if (_currentTickValue > 0 &&
                CurrentLocation(location) < CurrentThumbTick() -
                (int)(_tickSpace / 2))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Increments the value of the CrystalTrackBar.
        /// </summary>
        protected void IncrementValue()
        {
            if (_currentTickValue < (_numberTicks - 1))
            {
                EraseThumb();

                _currentTickValue++;
                Value = _tickValues[_currentTickValue];

                RedrawControl();
            }
        }

        /// <summary>
        /// Decrements the value of the CrystalTrackBar.
        /// </summary>
        protected void DecrementValue()
        {
            if (_currentTickValue > 0)
            {
                EraseThumb();

                _currentTickValue--;
                Value = _tickValues[_currentTickValue];

                RedrawControl();
            }
        }

        /// <summary>
        /// Sets the value of the CrystalTrackBar to the first tick item.
        /// </summary>
        protected void FirstValue()
        {
            if (_currentTickValue != 0)
            {
                EraseThumb();

                _currentTickValue = 0;
                Value = _tickValues[_currentTickValue];

                RedrawControl();
            }
        }

        /// <summary>
        /// Sets the value of the CrystalTrackBar to the last tick item.
        /// </summary>
        protected void LastValue()
        {
            if (_currentTickValue != (_numberTicks - 1))
            {
                EraseThumb();

                _currentTickValue = (_numberTicks - 1);
                Value = _tickValues[_currentTickValue];

                RedrawControl();
            }
        }

        /// <summary>
        /// Determines if the thumb on the CrystalTrackBar has been clicked.
        /// </summary>
        /// <returns>Boolean value indicating that the thumb was pressed.</returns>
        protected bool IsThumbPressed()
        {
            return (_thumbState == CrystalThumbState.Pressed);
        }

        /// <summary>
        /// Determines if the thumb on the CrystalTrackBar is being erased.
        /// </summary>
        /// <returns>Boolean value indicating that the thumb is being erased.</returns>
        protected bool IsThumbErasing()
        {
            return (_thumbState == CrystalThumbState.Erased);
        }

        /// <summary>
        /// Determines if the CrystalTrackBar is horizontal.
        /// </summary>
        /// <returns>Boolean value indicating that the CrystalTrackBar is horizontal.</returns>
        private bool IsHorizontal()
        {
            return (_orientation == Orientation.Horizontal);
        }

        #endregion

        #region Overriden Methods

        /// <summary>
        /// Invalidates a rectangle on the CrystalTrackBar control.
        /// </summary>
        /// <param name="rc">The rectangle to be invalidated.</param>
        protected override void InvalidateEx(Rectangle rc)
        {
            if ((TransparentMode) && (Parent != null))
            {
                // If we have a totally transparent background, we have to
                // ask the parent to invalidate our rectangle.
                // We'll map our rect from our location to screen to parent location.
                Point screenPoint = PointToScreen(rc.Location);
                Point parentPoint = Parent.PointToClient(screenPoint);
                Rectangle parentRect = new Rectangle(parentPoint, rc.Size);
                base.InvalidateEx(parentRect);
            }
            else
            {
                // If we are not in TransparentMode, we can just invalidate
                // our own rectangle directly.
                base.InvalidateEx(rc);
            }
        }

        /// <summary>
        /// Provides the initial layout for the CrystalTrackBar.
        /// </summary>
        protected override void InitLayout()
        {
            SetupTrackBar();
            base.InitLayout();
        }

        /// <summary>
        /// Adjusts the CrystalTrackBar when the size has been changed.
        /// </summary>
        /// <param name="e">The event argument.</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            SetupTrackBar();
            base.OnSizeChanged(e);
        }

        /// <summary>
        /// Paints the CrystalTrackBar.
        /// </summary>
        /// <param name="e">The paint event argument.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            DrawTrackBar(e.Graphics);
            DrawTicks(e.Graphics);
            DrawThumb(e.Graphics);
        }

        /// <summary>
        /// Determine whether the user has clicked the mouse button on the track bar thumb.
        /// </summary>
        /// <param name="e">The mouse event argument.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (this._thumbRectangle.Contains(e.Location))
            {
                _thumbState = CrystalThumbState.Pressed;
            }
        }

        // Redraw the track bar thumb if the user has moved it.
        /// <summary>
        /// Determine whether the user has released the mouse button.
        /// </summary>
        /// <param name="e">The mouse event argument.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            _thumbState = CrystalThumbState.Hover;
            RedrawControl();
        }

        /// <summary>
        /// Tracks the mouse movement within the CrystalTrackBar.
        /// </summary>
        /// <param name="e">The mouse event argument.</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            // The user is moving the thumb.
            if (IsThumbPressed())
            {
                // if the value hasn't changed (the user is just holding at one position),
                // then it may not necessary to erase/redraw the thumb,
                // as it may cause too much flicker.
                if (!HasTrackValueChanged(e.Location))
                {
                    // BUG FIX: Without this call to redraw the control,
                    // the background gets erased when the user holds the mouse
                    // down, but does not move the track value.
                    if (TransparentMode)
                        RedrawControl();

                    return;
                }

                // Track movements to the next tick to the right, if 
                // the cursor has moved halfway to the next tick.
                if (_currentTickValue < (_numberTicks - 1) &&
                    CurrentLocation(e.Location) > CurrentThumbTick() +
                    (int)(_tickSpace))
                {
                    IncrementValue();
                }
                // Track movements to the next tick to the left, if 
                // cursor has moved halfway to the next tick.
                else if (_currentTickValue > 0 &&
                    CurrentLocation(e.Location) < CurrentThumbTick() -
                    (int)(_tickSpace / 2))
                {
                    DecrementValue();
                }
            }
            else
            {
                // ThumbState switches to hover because user is still
                // in trackbar area.
                _thumbState = CrystalThumbState.Hover;

                // Don't call redraw here, it will cause more flicker than necessary.
                //RedrawControl();
            }
        }

        /// <summary>
        /// Determines whether the mouse cursor has entered the CrystalTrackBar.
        /// </summary>
        /// <param name="e">The mouse event argument.</param>
        protected override void OnMouseEnter(EventArgs e)
        {
            _thumbState = CrystalThumbState.Hover;
            RedrawControl();
            base.OnMouseEnter(e);
        }

        /// <summary>
        /// Determines whether the mouse cursor has left the CrystalTrackBar.
        /// </summary>
        /// <param name="e">The mouse event argument.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            _thumbState = CrystalThumbState.Normal;
            RedrawControl();
            base.OnMouseLeave(e);
        }

        /// <summary>
        /// Processes the keystroke for the trackbar.
        /// </summary>
        /// <param name="keyData">The keystroke data.</param>
        /// <returns>True if the CrystalTrackBar processes a keystroke, false if it does not process the keystroke.</returns>
        public virtual bool ProcessKeystroke(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Left:
                    if (IsHorizontal())
                        DecrementValue();
                    return true;

                case Keys.Right:
                    if (IsHorizontal())
                        IncrementValue();
                    return true;

                case Keys.Up:
                    if (!IsHorizontal())
                        DecrementValue();
                    return true;

                case Keys.Down:
                    if (!IsHorizontal())
                        IncrementValue();
                    return true;

                case Keys.Home:
                    FirstValue();
                    return true;

                case Keys.End:
                    LastValue();
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Processes the up, down, left, right, pgup, pgdn keystrokes.
        /// Note this only takes effect when the property KeyboardControl is true.
        /// </summary>
        /// <param name="msg">The windows message.</param>
        /// <param name="keyData">The keystroke data.</param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (KeyboardControl)
            {
                if (msg.Msg == 0x100B)
                {
                    if (ProcessKeystroke(keyData))
                    {
                        return true;
                    }
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion
    }

    /// <summary>
    /// Allows a CrystalTrackBar to be hosted as a ToolStripItem
    /// </summary>
    [System.ComponentModel.DesignerCategory("code")]
    [
        ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip),
        ToolboxBitmap(typeof(resfinder), "CrystalToolkit.Resources.CrystalTrackBar.bmp")
    ]
    public partial class CrystalToolStripTrackBar : ToolStripControlHost
    {
        private const int _defaultWidth = 100;
        private const int _defaultHeight = 16;

        /// <summary>
        /// Default constructor for CrystalToolStripTrackBar.
        /// </summary>
        public CrystalToolStripTrackBar()
            : base(CreateControlInstance())
        {
        }

        /// <summary>
        /// Create a strongly typed property called CrystalTrackBar - handy to prevent casting everywhere.
        /// </summary>
        public CrystalTrackBar CrystalTrackBar
        {
            get
            {
                return Control as CrystalTrackBar;
            }
        }

        /// <summary>
        /// Create the actual control, note this is static so it can be called from the
        /// constructor.
        /// 
        /// </summary>
        /// <returns></returns>
        private static Control CreateControlInstance()
        {
            CrystalTrackBar t = new CrystalTrackBar();
            t.AutoSize = false;
            t.Size = new Size(_defaultWidth, _defaultHeight);
            t.TickStyle = TickStyle.None;
            t.TransparentMode = true;
            // Add other initialization code here.
            return t;
        }

        /// <summary>
        /// The value of the CrystalTrackBar inside the toolstrip host.
        /// </summary>
        [DefaultValue(0)]
        public int Value
        {
            get { return CrystalTrackBar.Value; }
            set
            {
                CrystalTrackBar.Value = value;
            }
        }

        /// <summary>
        /// Attach to events we want to re-wrap
        /// </summary>
        /// <param name="control">The control, which should be a CrystalTrackBar object.</param>
        protected override void OnSubscribeControlEvents(Control control)
        {
            base.OnSubscribeControlEvents(control);
            CrystalTrackBar trackBar = control as CrystalTrackBar;
            trackBar.ValueChanged += new EventHandler(trackBar_ValueChanged);
        }

        /// <summary>
        /// Detach from events.
        /// </summary>
        /// <param name="control">The control, which should be a CrystalTrackBar object.</param>
        protected override void OnUnsubscribeControlEvents(Control control)
        {
            base.OnUnsubscribeControlEvents(control);
            CrystalTrackBar trackBar = control as CrystalTrackBar;
            trackBar.ValueChanged -= new EventHandler(trackBar_ValueChanged);
        }

        /// <summary>
        /// Routing for event
        /// CrystalTrackBar.ValueChanged -> CrystalToolStripTrackBar.ValueChanged
        /// </summary>
        /// <param name="sender">The event sender, in this case, CrystalTrackBar.</param>
        /// <param name="e">Event arguments.</param>
        void trackBar_ValueChanged(object sender, EventArgs e)
        {
            // when the trackbar value changes, fire an event.
            if (this.ValueChanged != null)
            {
                ValueChanged(sender, e);
            }
        }

        /// <summary>
        /// Event handler to hook into the value change event of the CrystalTrackBar.
        /// </summary>
        public event EventHandler ValueChanged;

        /// <summary>
        /// Default size for this control.
        /// </summary>
        protected override Size DefaultSize
        {
            get
            {
                return new Size(_defaultWidth, _defaultHeight);
            }
        }
    }
}