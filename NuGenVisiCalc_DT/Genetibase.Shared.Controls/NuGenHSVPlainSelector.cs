/* -----------------------------------------------
 * NuGenHSVPlainSelector.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
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
	[ToolboxBitmap(typeof(NuGenHSVPlainSelector), "Resources.NuGenIcon.png")]
	[DefaultEvent("SelectedColorChanged")]
	[DefaultProperty("SelectedColor")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenHSVPlainSelector : Control
	{
		#region Properties.Public

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		[DefaultValue(typeof(Color), "255;255;255")]
		public Color SelectedColor
		{
			get
			{
				return _sel;
			}
			set
			{
				_selpt = FindColor(value);
				_sel = sel = FindPoint2(_selpt);
				this.Refresh();
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public HSL SelectedHSL
		{
			get
			{
				return FindPoint(_selpt);
			}
			set
			{
				_selpt = FindColor(value);
				_sel = sel = FindPoint2(_selpt);
				this.Refresh();
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		public event EventHandler SelectedColorChanged;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		public event EventHandler SelectedColorChanging;

		#endregion

		#region Methods.Protected.Overridden

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.SizeChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			InitBrush();
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			UpdateColor(e.X, e.Y, false);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (e.Button != MouseButtons.None)
				UpdateColor(e.X, e.Y, false);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			UpdateColor(e.X, e.Y, true);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			e.Graphics.FillRectangle(_lbrsv, this.ClientRectangle);
			e.Graphics.FillRectangle(_lbrsh, this.ClientRectangle);

			PointF[] pt = new PointF[] { _selpt };
			_transfrm.TransformPoints(pt);

			DrawCross(e.Graphics, Point.Truncate(pt[0]), _selpt.X > 0.5f ? Pens.White : Pens.Black);
		}

		#endregion

		#region Methods.Private

		private void DrawCross(Graphics gr, Point pos, Pen pen)
		{
			gr.DrawLine(pen, pos.X - 3, pos.Y, pos.X - 1, pos.Y);
			gr.DrawLine(pen, pos.X + 3, pos.Y, pos.X + 1, pos.Y);
			gr.DrawLine(pen, pos.X, pos.Y - 3, pos.X, pos.Y - 1);
			gr.DrawLine(pen, pos.X, pos.Y + 3, pos.X, pos.Y + 1);
		}

		private void InitBrush()
		{
			if (_lbrsv == null)
			{
				_lbrsv = new LinearGradientBrush
					(new Point(0, 0), new Point(1, 0), Color.Black, Color.White);
				float dy = 0f, incy = 360f / (float)(num - 1);
				float[] pos = new float[num];
				Color[] col = new Color[num];
				HSL hue = new HSL(0, 50, 100);
				for (int i = 0; i < num; i++, dy += incy)
				{
					hue.h = (int)dy;
					pos[i] = dy / 360f;
					col[i] = HSL.Hue2RGB(hue);
				}
				ColorBlend ret = new ColorBlend();
				//pos[pos.Length-1]=1f;
				ret.Colors = col;
				ret.Positions = pos;
				_lbrsv.InterpolationColors = ret;
			}
			if (_lbrsh == null)
			{
				_lbrsh = new LinearGradientBrush
					(new Point(0, 0), new Point(1, 0), Color.Black, Color.White);
				ColorBlend ret = new ColorBlend();
				ret.Positions = new float[] { 0f, 0.5f, 1f };
				ret.Colors = new Color[] { Color.White, Color.FromArgb(0, 128, 128, 128), Color.Black };
				_lbrsh.InterpolationColors = ret;
			}
			float w = Math.Max(this.Width, 3),
				h = Math.Max(this.Height, 3);
			_lbrsv.Transform = new Matrix(0f, h, w, 0f, 0f, 0f);
			_lbrsh.Transform = new Matrix(w, 0f, 0f, h, 0f, 0f);
			_transfrm = new Matrix(w - 1f, 0f, 0f, h - 1f, 0f, 0f);
		}

		private void AddInvalidate(PointF value)
		{
			PointF[] pt = new PointF[] { value };
			_transfrm.TransformPoints(pt);
			this.Invalidate(new Rectangle(
				(int)(pt[0].X) - 4,
				(int)(pt[0].Y) - 4,
				8, 8));
		}

		private void UpdateColor(float x, float y, bool final)
		{
			float w = Math.Max(this.Width, 1), h = Math.Max(this.Height, 1);
			AddInvalidate(_selpt);
			_selpt = new PointF(
				Math.Max(0f, Math.Min(1f, x / w)),
				Math.Max(0f, Math.Min(1f, y / h)));
			_sel = FindPoint2(_selpt);
			if (sel != _sel)
			{
				if (final && SelectedColorChanged != null)
				{
					SelectedColorChanged(this, new EventArgs());
					sel = _sel;
				}
				else if (SelectedColorChanging != null)
					SelectedColorChanging(this, new EventArgs());
			}
			AddInvalidate(_selpt);
			this.Update();
		}

		private PointF FindColor(HSL rt)
		{
			return new PointF(1f - ((float)rt.l / 100f),
				(float)rt.h / 360f);
		}

		private PointF FindColor(Color val)
		{
			return FindColor(HSL.RGB2HSL(val));
		}

		private HSL FindPoint(PointF val)
		{
			return new HSL(
				Math.Min(360, Math.Max(0, (int)(val.Y * 360f))),
				Math.Min(100, Math.Max(0, 100 - (int)(val.X * 100f))),
				100);
		}

		private Color FindPoint2(PointF val)
		{
			return HSL.Hue2RGB(FindPoint(val));
		}

		#endregion

		private const int num = 13;
		private Matrix _transfrm = new Matrix();
		private Color _sel = Color.White, sel = Color.White;
		private PointF _selpt = new PointF(0f, 0f);
		private LinearGradientBrush _lbrsh, _lbrsv;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenHSVPlainSelector"/> class.
		/// </summary>
		public NuGenHSVPlainSelector()
		{
			this.SetStyle(ControlStyles.DoubleBuffer |
				ControlStyles.AllPaintingInWmPaint |
				ControlStyles.UserPaint |
				ControlStyles.ResizeRedraw, true);
			InitBrush();
		}
	}
}
