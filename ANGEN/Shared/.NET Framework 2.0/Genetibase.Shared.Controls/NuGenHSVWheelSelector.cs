/* -----------------------------------------------
 * NuGenHSVWheelSelector.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.Drawing;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Genetibase.Shared.Controls.ComponentModel;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenHSVWheelSelector), "Resources.NuGenIcon.png")]
	[DefaultEvent("SelectedColorChanged")]
	[DefaultProperty("SelectedColor")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenHSVWheelSelector : Control
	{
		#region Properties.Public

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(typeof(Color), "255;255;255")]
		[NuGenSRCategory("Category_Appearance")]
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
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			Rectangle rct = this.ClientRectangle;
			rct.Inflate(-1, -1);
			e.Graphics.FillEllipse(_pthbrs, rct);
			PointF[] pt = new PointF[] { _selpt };
			_transfrm.TransformPoints(pt);
			e.Graphics.FillEllipse(Brushes.White, pt[0].X - 1, pt[0].Y - 1, 3, 3);
			e.Graphics.DrawEllipse(Pens.Black, pt[0].X - 1, pt[0].Y - 1, 3, 3);
		}

		#endregion

		#region Methods.Private

		private void InitBrush()
		{
			if (_pthbrs == null)
			{
				double num2 = (double)num;
				PointF[] pts = new PointF[num];
				Color[] cols = new Color[num];
				for (int i = 0; i < num; i++)
				{
					cols[i] = HSL.Hue2RGB(new HSL((int)((double)i * (360 / num2)), 50, 100));
					pts[i] = new PointF((float)Math.Sin((double)i * (2.0 / num2) * Math.PI),
							(float)Math.Cos((double)i * (2.0 / num2) * Math.PI));
				}
				_pthbrs = new PathGradientBrush(pts);
				_pthbrs.SurroundColors = cols;
				_pthbrs.FocusScales = new PointF(0f, 0f);
				_pthbrs.CenterColor = Color.White;
			}
			float w = Math.Max(this.Width / 2, 3), h = Math.Max(this.Height / 2, 3);
			_pthbrs.Transform = new Matrix(w, 0f, 0f, h, w, h);
			_transfrm = new Matrix(w - 2f, 0f, 0f, h - 2f, w, h);
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
			float w = Math.Max(this.Width / 2, 1), h = Math.Max(this.Height / 2, 1);
			AddInvalidate(_selpt);
			_sel = FindPoint2(new PointF((x - w) / w, (y - h) / h));
			_selpt = FindColor(_sel);
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
			float dist = (float)(100 - Math.Max(50, rt.l)) / 50f;
			return new PointF(dist * (float)Math.Sin(Math.PI * (double)rt.h / 180.0),
				dist * (float)Math.Cos(Math.PI * (double)rt.h / 180.0));
		}

		private PointF FindColor(Color val)
		{
			return FindColor(HSL.RGB2HSL(val));
		}

		private HSL FindPoint(PointF val)
		{
			double angle = (Math.Atan2(val.X, val.Y) + PI2) % PI2,
				dist = Math.Sqrt(val.Y * val.Y + val.X * val.X);
			return new HSL((int)(180.0 * angle / Math.PI),
				100 - (int)(Math.Min(1.0, dist) * 50.0), dist == 0.0 ? 0 : 100);
		}

		private Color FindPoint2(PointF val)
		{
			return HSL.Hue2RGB(FindPoint(val));
		}

		#endregion

		private const double PI2 = Math.PI * 2.0;
		private const int num = 0x40;
		private Matrix _transfrm = new Matrix();
		private Color _sel = Color.White, sel = Color.White;
		private PointF _selpt = new PointF(0f, 0f);
		private PathGradientBrush _pthbrs;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenHSVWheelSelector"/> class.
		/// </summary>
		public NuGenHSVWheelSelector()
		{
			this.SetStyle(ControlStyles.DoubleBuffer |
				ControlStyles.AllPaintingInWmPaint |
				ControlStyles.UserPaint |
				ControlStyles.ResizeRedraw, true);
			InitBrush();
		}
	}
}
