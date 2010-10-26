/* -----------------------------------------------
 * NuGenBlendSelector.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.Drawing;
using Genetibase.Shared.Drawing;

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
	[ToolboxBitmap(typeof(NuGenBlendSelector), "Resources.NuGenIcon.png")]
	[DefaultEvent("ColorChanged")]
	[DefaultProperty("Color")]
	[Designer("Genetibase.Shared.Controls.Design.NuGenBlendSelectorDesigner")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenBlendSelector : Control
	{
		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		public event EventHandler ColorChanged;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		public event EventHandler ColorChanging;

		#region Properties.Public

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Color SelectedColor
		{
			get
			{
				return Blend(_vals[0]);
			}
		}

		/// <summary>
		/// </summary>
		[DefaultValue(0.5f)]
		[NuGenSRCategory("Category_Blend")]
		public float Value
		{
			get
			{
				return _vals[0];
			}
			set
			{
				if (float.IsInfinity(value) || float.IsNaN(value))
					value = 0f;
				_vals[0] = v = Math.Max(0f, Math.Min(value, 1f));
				this.Refresh();
			}
		}

		/// <summary>
		/// </summary>
		[DefaultValue(typeof(Color), "255,255,255,255")]
		[NuGenSRCategory("Category_Blend")]
		public Color UpperColor
		{
			get
			{
				return _cols[0];
			}
			set
			{
				_cols[0] = value;
				this.Refresh();
			}
		}

		/// <summary>
		/// </summary>
		[DefaultValue(typeof(Color), "0,255,255,255")]
		[NuGenSRCategory("Category_Blend")]
		public Color LowerColor
		{
			get
			{
				return _cols[1];
			}
			set
			{
				_cols[1] = value;
				this.Refresh();
			}
		}

		/// <summary>
		/// </summary>
		[DefaultValue(false)]
		[NuGenSRCategory("Category_Blend")]
		public bool Light
		{
			get
			{
				return _light;
			}
			set
			{
				_light = value;
				this.Refresh();
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
			base.OnMouseMove(e);

			if (!_tracking)
				_highlight = MouseAbove(e.Y);
			else
			{
				SetMousePos(false);
			}

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
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (e.Button == MouseButtons.Left)
			{
				_highlight = MouseAbove(e.Y);
				_tracking = true;
				if (_highlight != -1)
				{
					SetMousePos(false);
					this.Refresh();
				}
			}
		}
		
		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			if (e.Button == MouseButtons.Left)
			{
				SetMousePos(true);
				_tracking = false;
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Rectangle rct = this.GetBlendRectangle();
			if (rct.Height < 1)
				return;
			e.Graphics.FillRectangle(NuGenColorUtility.CheckerBrush, rct);
			_lbrs.Transform = new Matrix(0f, rct.Height, 1f, 0f, 0f, rct.Y - 1);
			_lbrs.LinearColors = _cols;
			e.Graphics.FillRectangle(_lbrs, rct);
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			e.Graphics.PixelOffsetMode = PixelOffsetMode.None;
			int y;
			Brush brs = _light ? Brushes.White : Brushes.Black;
			for (int i = 0; i < _vals.Length; i++)
			{
				y = (int)(_vals[i] * (float)rct.Height) + rct.Y;
				e.Graphics.FillPolygon(i == _highlight ? Brushes.Blue : brs,
					new Point[] { new Point(0, y - 3), new Point(5, y), new Point(0, y + 3) });
				e.Graphics.FillPolygon(i == _highlight ? Brushes.Blue : brs,
					new Point[] { new Point(rct.Right + 5, y - 3), new Point(rct.Right, y), new Point(rct.Right + 5, y + 3) });

			}
		}

		#endregion

		#region Methods.Private

		private Color Blend(float pos)
		{
			pos = Math.Max(0f, Math.Min(1f, pos));
			return Color.FromArgb(
				_cols[0].A + (int)(pos * (float)(_cols[1].A - _cols[0].A)),
				_cols[0].R + (int)(pos * (float)(_cols[1].R - _cols[0].R)),
				_cols[0].G + (int)(pos * (float)(_cols[1].G - _cols[0].G)),
				_cols[0].B + (int)(pos * (float)(_cols[1].B - _cols[0].B)));
		}

		private Rectangle GetBlendRectangle()
		{
			return Rectangle.Inflate(this.ClientRectangle, -5, -4);
		}

		private int MouseAbove(float y)
		{
			float p = y / Math.Max(1f, (float)this.Height);
			float max = 2f, max2 = 2f;
			int num = -1;
			for (int i = 0; i < _vals.Length; i++)
			{
				max = Math.Abs(p - _vals[i]);
				if (max < max2)
				{
					num = i;
					max2 = max;
				}
			}
			return num;
		}

		private void SetMousePos(bool final)
		{
			float y = this.PointToClient(Control.MousePosition).Y;
			if (_highlight == -1)
				return;
			_vals[_highlight] = ValueFromPosition(y);
			if (v != _vals[_highlight])
			{
				if (final && ColorChanged != null)
				{
					ColorChanged(this, new EventArgs());
					v = _vals[_highlight];
				}
				else if (ColorChanging != null)
					ColorChanging(this, new EventArgs());
			}
		}

		private float ValueFromPosition(float y)
		{
			return Math.Max(0f, Math.Min(1f, y / Math.Max(1f, (float)this.Height)));
		}

		#endregion

		private Color[] _cols = new Color[] { Color.Black, Color.White };
		private float[] _vals = new float[] { 0.5f };
		private float v;
		private int _highlight = -1;
		private bool _tracking = false, _light = false;

		private static LinearGradientBrush _lbrs = new LinearGradientBrush(
			new Point(0, 0), new Point(1, 0), Color.Black, Color.White);

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenBlendSelector"/> class.
		/// </summary>
		public NuGenBlendSelector()
		{
			this.SetStyle(ControlStyles.DoubleBuffer |
				ControlStyles.AllPaintingInWmPaint |
				ControlStyles.UserPaint |
				ControlStyles.ResizeRedraw, true);
		}
	}
}
