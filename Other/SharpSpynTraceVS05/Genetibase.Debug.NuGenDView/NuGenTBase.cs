using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.Debug
{
    internal sealed class NuGenTBase : UserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private Container components = null;

        private const int DefaultCapacity = 1024 * 64;

        private int _topRow;
        private int _firstColumn;
        private StringCycleBuffer _buffer;
        private int _countVisibleRows;
        private float _rowHeight;
        private float _charWidth;
        private int _countVisibleColumns;
        private NuGenTSelector _NuGenTSelector;
        private Color _NuGenTSelectorForeColor;
        private Color _NuGenTSelectorBackColor;

        public NuGenTBase ()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent ();

            _buffer = new StringCycleBuffer (DefaultCapacity);
            _topRow = 0;
            _firstColumn = 0;
            _NuGenTSelector = new NuGenTSelector ();
            _NuGenTSelector.NuGenTSelectorUpdated += new EventHandler (NuGenTSelectorUpdated);
            _NuGenTSelectorForeColor = Color.White;
            _NuGenTSelectorBackColor = Color.Blue;

            SetStyle (ControlStyles.AllPaintingInWmPaint, true);
            SetStyle (ControlStyles.DoubleBuffer, true);
            SetStyle (ControlStyles.UserPaint, true);
            SetStyle (ControlStyles.ResizeRedraw, true);
        }

        [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        [Browsable (false)]
        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                CalculateMeasurements ();
                Redraw ();
            }
        }

        [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        [Browsable (false)]
        public int FirstColumn
        {
            get { return _firstColumn; }

            set
            {
                _firstColumn = value;
                Redraw ();
            }
        }

        [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        [Browsable (false)]
        public int TopRow
        {
            get { return _topRow; }

            set
            {
                if (_topRow >= _buffer.Length)
                {
                    throw new ArgumentOutOfRangeException ("value");
                }

                _topRow = value;
                Redraw ();
            }
        }

        [Browsable (false)]
        public int NumberItems
        {
            get { return _buffer.Length; }
        }

        [Browsable (false)]
        public int CountVisibleRows
        {
            get { return _countVisibleRows; }
        }

        [Browsable (false)]
        public int CountVisibleColumns
        {
            get { return _countVisibleColumns; }
        }

        [Browsable (false)]
        public NuGenTSelector NuGenTSelector
        {
            get { return _NuGenTSelector; }
        }

        [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        [Browsable (false)]
        public Color NuGenTSelectorForeColor
        {
            get { return _NuGenTSelectorForeColor; }

            set
            {
                _NuGenTSelectorForeColor = value;
                Redraw ();
            }
        }

        [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        [Browsable (false)]
        public Color NuGenTSelectorBackColor
        {
            get { return _NuGenTSelectorBackColor; }

            set
            {
                _NuGenTSelectorBackColor = value;
                Redraw ();
            }
        }

    	public void Add (string value, bool h)
    	{
			string[] substrs = value.Split (new char[] {'\n'});
			foreach (string s in substrs)
			{
				_buffer.Add (s, h);
			}

			if (IsRowVisible (NumberItems))
			{
				Redraw ();
			}
    	}
    	
        public void Add (string value)
        {
			Add(value, false);
        }

        public int HitTest (int y)
        {
            int row = _topRow + (int) Math.Floor ((double) y / (double) _rowHeight);
            if (row >= NumberItems)
            {
                return -1;
            }

            return row;
        }

        public bool CopyNuGenTSelectorToClipboard ()
        {
            if (_NuGenTSelector.Length == 0)
            {
                return false;
            }
            else
            {
                StringBuilder sb = new StringBuilder ();
                for (int i = 0; i < _NuGenTSelector.Length; i++)
                {
                    sb.AppendFormat ("{0}\r\n", _buffer[_NuGenTSelector.Start + i]);
                }

                Clipboard.SetDataObject (sb.ToString ());
                return true;
            }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose (bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose ();
                }
            }
            base.Dispose (disposing);
        }

        protected override void OnSizeChanged (EventArgs e)
        {
            base.OnSizeChanged (e);
            CalculateMeasurements ();
        }

        protected override void OnPaint (PaintEventArgs e)
        {
            base.OnPaint (e);
            float y = 0;
            float x = _charWidth * (float) _firstColumn * -1;

            int lastRow = _topRow + _countVisibleRows;
            if (lastRow > _buffer.Length)
            {
                lastRow = _buffer.Length;
            }

            using (Brush unselectedTextForegroundBrush = new SolidBrush (ForeColor),
                         selectedTextBackgroundBrush = new SolidBrush (NuGenTSelectorBackColor),
                         selectedTextForegroundBrush = new SolidBrush (NuGenTSelectorForeColor),
						 highlightBrush = new SolidBrush(Color.Blue))
            {
                for (int row = _topRow; row < lastRow; row++)
                {
                	bool h = _buffer.IsHighlighted(row);
                    if (_NuGenTSelector.IsLineSelected (row))
                    {
                        e.Graphics.FillRectangle (selectedTextBackgroundBrush, 0, y, (float) Width, _rowHeight);
                        e.Graphics.DrawString (_buffer[row], Font, 
							h ? highlightBrush : selectedTextForegroundBrush, x, y);
                    }
                    else
                    {
                        e.Graphics.DrawString (_buffer[row], Font, 
							h ? highlightBrush : unselectedTextForegroundBrush, x, y);
                    }

                    y += _rowHeight;
                }
            }
        }

        private bool IsRowVisible (int index)
        {
            return index < (_topRow + _countVisibleRows);
        }

        private void Redraw ()
        {
            Invalidate ();
        }

        private void CalculateMeasurements ()
        {
            using (Graphics g = CreateGraphics ())
            {
                SizeF fontSize = g.MeasureString ("X", Font);
                _rowHeight = fontSize.Height;
                _countVisibleRows = (int) Math.Ceiling ((double) Height / (double) fontSize.Height);
                _charWidth = fontSize.Width;
                _countVisibleColumns = (int) Math.Ceiling ((double) Width / (double) fontSize.Width);
            }
        }

        private void NuGenTSelectorUpdated (object sender, EventArgs e)
        {
            Redraw ();
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            components = new System.ComponentModel.Container ();
        }

        #endregion
    }
}