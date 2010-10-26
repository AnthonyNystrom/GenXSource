/* -----------------------------------------------
 * NuGenColorHistory.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.Drawing;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenColorHistory), "Resources.NuGenIcon.png")]
	[DefaultEvent("SelectedColorChanged")]
	[DefaultProperty("SelectedColor")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenColorHistory : Control
	{
		#region Properties.Public

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Color SelectedColor
		{
			get
			{
				return _lock ? _selectedcol : _buf[0];
			}
			set
			{
				SetSelectedColor(value);
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		public event EventHandler SelectedColorChanged;

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Color[] Table
		{
			get
			{
				return _buf;
			}
			set
			{
				if (value == null || value.Length != _buf.Length)
					return;
				_buf = value;
				this.Refresh();
				_selectedcol = _buf[0];
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		[NuGenSRCategory("Category_Behavior")]
		public bool Lock
		{
			get
			{
				return _lock;
			}
			set
			{
				_lock = value;
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(5)]
		[NuGenSRCategory("Category_Appearance")]
		public int Columns
		{
			get
			{
				return _columns;
			}
			set
			{
				_columns = Math.Max(1, Math.Min(50, value));
				UpdateTable();
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(2)]
		[NuGenSRCategory("Category_Appearance")]
		public int Rows
		{
			get
			{
				return _rows;
			}
			set
			{
				_rows = Math.Max(1, Math.Min(50, value));
				UpdateTable();
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			int h = _highlight;
			_highlight = MouseAbove(e.X, e.Y);
			if (h != _highlight)
				this.Refresh();
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			_highlight = -1;
			this.Refresh();
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			_highlight = MouseAbove(e.X, e.Y);
			if (e.Button == MouseButtons.Left && _highlight != -1)
			{
				Color v = _selectedcol;
				SetSelectedColor(_buf[_highlight]);
				if (v != _selectedcol && SelectedColorChanged != null)
					SelectedColorChanged(this, new EventArgs());
				this.Refresh();
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.FillRectangle(NuGenColorUtility.CheckerBrush, this.ClientRectangle);
			float dx, dy = 0,
				incx = this.Width / (float)_columns,
				incy = this.Height / (float)_rows;
			int i;

			for (float y = 0; y < _rows; y++, dy += incy)
			{
				dx = 0;
				for (float x = 0; x < _columns; x++, dx += incx)
				{
					i = (int)(y * _columns + x);
					_brs.Color = _buf[i];
					e.Graphics.FillRectangle(_brs,
						dx, dy, incx, incy);
					if ((_highlight == i || i == 0) && !_lock)
						e.Graphics.DrawRectangle(i == 0 ?
							Pens.Red : SystemPens.Highlight,
							dx, dy, incx - 1, incy - 1);
				}
			}
		}

		#endregion

		#region Methods.Private

		private void UpdateTable()
		{
			Color[] buf = new Color[_rows * _columns];
			Array.Copy(_buf, 0, buf, 0, Math.Min(_buf.Length, buf.Length));
			_buf = buf;
			this.Refresh();
		}

		private void SetSelectedColor(Color val)
		{
			_selectedcol = val;
			if (_lock)
				return;
			int index = Array.IndexOf(_buf, val);
			if (index == -1)
				index = _buf.Length - 1;

			for (int i = index; i > 0; i--)
				_buf[i] = _buf[i - 1];
			_buf[0] = val;
			this.Refresh();
		}

		private int MouseAbove(int x, int y)
		{
			if (this.Width < 1 || this.Height < 1)
				return -1;

			int vx = (int)Math.Floor((double)(x * _columns / this.Width)),
				vy = (int)Math.Floor((double)(y * _rows / this.Height));
			if (vx < 0 || vx >= _columns || vy < 0 || vy >= _rows)
				return -1;
			return vx + _columns * vy;
		}

		#endregion

		private int _rows = 2, _columns = 5, _highlight = -1;
		private Color[] _buf = new Color[10];
		private Color _selectedcol = Color.Transparent;
		private bool _lock = false;
		private SolidBrush _brs = new SolidBrush(Color.White);

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenColorHistory"/> class.
		/// </summary>
		public NuGenColorHistory()
		{
			this.SetStyle(ControlStyles.DoubleBuffer |
				ControlStyles.AllPaintingInWmPaint |
				ControlStyles.UserPaint |
				ControlStyles.ResizeRedraw, true);
		}
	}
}
