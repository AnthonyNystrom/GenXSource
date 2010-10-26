/* -----------------------------------------------
 * NuGenVisiPlot2D.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.MathX.ComponentModel;
using Genetibase.MathX.FormulaInterpreter;
using Genetibase.MathX.Properties;

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace Genetibase.MathX
{
	/// <summary>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenVisiPlot2D), "Resources.NuGenIcon.png")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenVisiPlot2D : Control
	{
		#region Events

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		public event NuGenPlotSuccessEventHandler Success;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		public event NuGenPlotPositionEventHandler PositionChanged;

		#endregion

		#region Properties.Appearance

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[NuGenSRCategory("Category_AntiAlias")]
		public bool AntiAlias
		{
			get
			{
				return _flags[(int)Flags.AntiAlias];
			}
			set
			{
				_flags[(int)Flags.AntiAlias] = value;
				this.Reload();
			}
		}

		/// <summary>
		/// Gets or sets the background color for the control.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that represents the background color of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor"></see> property.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		public override Color BackColor
		{
			get
			{
				return _colors[(int)Colors.BackColor];
			}
			set
			{
				_colors[(int)Colors.BackColor] = Color.FromArgb(255, value);
				_colors[(int)Colors.Ruler] = Color.FromArgb(100,
					value.GetBrightness() > 0.5f ? Color.Black : Color.White);
				this.Reload();
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		public Color DescriptionColor
		{
			get
			{
				return _colors[(int)Colors.DescColor];
			}
			set
			{
				_colors[(int)Colors.DescColor] = value;
				this.Reload();
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		[NuGenSRCategory("Category_Appearance")]
		public bool DisplayAsymptotes
		{
			get
			{
				return _flags[(int)Flags.Asymptotes];
			}
			set
			{
				_flags[(int)Flags.Asymptotes] = value;
				this.CalculatePaths(true);
				this.Reload();
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[NuGenSRCategory("Category_Appearance")]
		public bool DisplayAxis
		{
			get
			{
				return _flags[(int)Flags.Axis];
			}
			set
			{
				_flags[(int)Flags.Axis] = value;
				this.Reload();
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[NuGenSRCategory("Category_Appearance")]
		public bool DisplayDescriptionX
		{
			get
			{
				return _flags[(int)Flags.DescriptionX];
			}
			set
			{
				_flags[(int)Flags.DescriptionX] = value;
				this.Reload();
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[NuGenSRCategory("Category_Appearance")]
		public bool DisplayDescriptionY
		{
			get
			{
				return _flags[(int)Flags.DescriptionY];
			}
			set
			{
				_flags[(int)Flags.DescriptionY] = value;
				this.Reload();
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[NuGenSRCategory("Category_Appearance")]
		public bool DisplayGrid
		{
			get
			{
				return _flags[(int)Flags.Grid];
			}
			set
			{
				_flags[(int)Flags.Grid] = value;
				this.Reload();
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		public Font DescriptionFont
		{
			get
			{
				return _font;
			}
			set
			{
				_font = value;
				this.Reload();
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		[NuGenSRCategory("Category_Appearance")]
		public bool DisplayRulers
		{
			get
			{
				return _flags[(int)Flags.Rulers];
			}
			set
			{
				_flags[(int)Flags.Rulers] = value;
				this.Reload();
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		public Color GridColor
		{
			get
			{
				return _colors[(int)Colors.GridColor];
			}
			set
			{
				_colors[(int)Colors.GridColor] = value;
				this.Reload();
			}
		}

		#endregion

		#region Properties.Behavior

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		[NuGenSRCategory("Category_Behavior")]
		public bool AutoSelect
		{
			get
			{
				return _flags[(int)Flags.AutoScroll];
			}
			set
			{
				_flags[(int)Flags.AutoScroll] = value;
				_timer.Enabled = _border = false;
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		[NuGenSRCategory("Category_Behavior")]
		public bool CenterAxis
		{
			get
			{
				return _flags[(int)Flags.CenterAxis];
			}
			set
			{
				_flags[(int)Flags.CenterAxis] = value;
				this.Reload();
			}

		}

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				_description = value;
				this.Reload();
			}
		}

		#endregion

		#region Properties.Layout

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Layout")]
		public Rectangle GridRectangle
		{
			get
			{
				return _gitter;
			}
			set
			{
				Size sz = _gitter.Size;
				_gitter = value;
				if (_gitter.Width < 1)
					_gitter.Width = 1;
				if (_gitter.Height < 1)
					_gitter.Height = 1;
				this.RecalcGitter();
				CalculatePaths(sz != _gitter.Size);
				this.Reload();
			}
		}

		#endregion

		#region Properties.NonBrowsable

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public NuGenFormulaCollection Formulas
		{
			get
			{
				return _formulas;
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public double PaintBuffer
		{
			get
			{
				return _paintBuffer;
			}
			set
			{
				_paintBuffer = Math.Min(Math.Max(0.0, value), 9.0);
				Reload();
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public NuGenPlotPaintIntervalF Selection
		{
			get
			{
				return _selection;
			}
			set
			{
				NuGenPlotPaintInterval iv = ScaleTranslateMathToClient(
					NuGenPlotPaintIntervalF.Union(RightOrderInterval(_selection), value));


				_selection = value;

				Reload(new Rectangle(iv.Start - 3, 0, Math.Abs(iv.Width) + 6, this.Height));
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public NuGenPlotPaintInterval SelectionScreen
		{
			get
			{
				return this.ScaleTranslateMathToClient(Selection);
			}
			set
			{
				this.Selection = this.ScaleTranslateClientToMath(value, false);
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public NuGenPlotToolBase Tool
		{
			get
			{
				return _tool;
			}
			set
			{
				_tool = value;
				this.Reload();
			}
		}

		#endregion

		#region Properties.Internal

		internal Rectangle InnerMargin
		{
			get
			{
				return new Rectangle(20, 20, this.Width - 40, this.Height - 40);
			}
		}

		#endregion

		#region Properties.Protected.Overridden

		private static readonly Size _defaultSize = new Size(200, 200);

		/// <summary>
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				return _defaultSize;
			}
		}

		#endregion

		#region Methods.Public

		/// <summary>
		/// </summary>
		public void BeginUpdate()
		{
			_updating = true;
		}

		/// <summary>
		/// </summary>
		public void EndUpdate()
		{
			_updating = false;
			this.Reload();
		}

		/// <summary>
		/// </summary>
		public void Reload()
		{
			if (_updating)
			{
				return;
			}

			this.Refresh();
		}

		/// <summary>
		/// </summary>
		public void Reload(Rectangle rect)
		{
			if (_updating)
			{
				return;
			}

			this.Invalidate(rect);
			this.Update();
		}

		#endregion

		#region Methods.Protected.Overridden

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if (_tool != null)
				_tool.MouseDown(this, e);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (e.Button != MouseButtons.None)
				TestTimer();
			if (_timer.Enabled)
				return;
			if (_tool != null)
				_tool.MouseMove(this, e);
			if (PositionChanged != null)
				PositionChanged((double)(e.X - _gitter.X) / (double)_gitter.Width,
					(double)(_gitter.Y - e.Y) / (double)_gitter.Height);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (_tool != null)
				_tool.MouseUp(this, e);
			_timer.Enabled = _border = false;
			Reload();
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseWheel"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			base.OnMouseWheel(e);
			if (_tool != null)
				_tool.MouseWheel(this, e);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			this.DrawSurface(e.Graphics);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.SizeChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnSizeChanged(EventArgs e)
		{
			this.Reload();
		}

		#endregion

		#region Methods.Asymptotes

		private double w, begin, end, d;
		private float ppy, py, y, h;
		private int pmax, pcurr, i;

		private float GetValIS(NuGenFormulaElement fml, double val)
		{
			NuGenInterpreter.SetVariableValue(_varx, val);
			float res = (float)fml.Value;
			if (float.IsNaN(res))
				return 0f;
			else if (float.IsPositiveInfinity(res))
				return 2e30f;
			else if (float.IsNegativeInfinity(res))
				return -2e30f;
			else
				return res;
		}
		private float GetVal(NuGenFormulaElement fml, double val)
		{
			return Math.Min(1e4f, Math.Max(GetValIS(fml, val), -1e4f));
		}
		private bool SAsymp(float pastY, float presentY, float futureY)
		{
			return SwapGreater(futureY - presentY, presentY - pastY);
		}
		private bool SwapGreater(float greater, float less)
		{
			return Math.Abs(less) > 1e1f && Math.Sign(greater) != Math.Sign(less) &&
				Math.Abs(greater) > Math.Abs(less);
		}
		private void SaveLine(GraphicsPath pth, float x1, float y1, float x2, float y2)
		{
			float m = (y2 - y1) / (x2 - x1), t = y1 + m * -x1;

			if (Math.Abs(m * x1 + t) > 2e5f)
			{
				y1 = (float)Math.Sign(y1) * 2e5f;
				x1 = (y1 - t) / m;
			}
			else
			{
				x1 = -2e5f;
				y1 = m * x1 + t;
			}

			if (Math.Abs(m * x2 + t) > 2e5f)
			{
				y2 = (float)Math.Sign(y2) * 2e5f;
				x2 = (y2 - t) / m;
			}
			else
			{
				x2 = 2e5f;
				y2 = m * x2 + t;
			}

			pth.AddLine(x1, y1, x2, y2);
		}

		#endregion

		#region Methods.Drawing

		private void RecalcGitter()
		{
			if (_flags[(int)Flags.CenterAxis])
			{
				_gitter.X = this.Width / 2;
				_gitter.Y = this.Height / 2;
			}
		}

		/// <summary>
		/// </summary>
		public void DrawSurface(Graphics g)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			CalculatePaths(false);
			RecalcGitter();
			g.Clear(_colors[(int)Colors.BackColor]);
			// zeichnen beginnt
			if (_flags[(int)Flags.Grid])
				DrawGrid(g);
			if (_flags[(int)Flags.Axis])
				DrawAxis(g);
			if (_flags[(int)Flags.DescriptionX])
				DrawDescriptionX(g);
			if (_flags[(int)Flags.DescriptionY])
				DrawDescriptionY(g);
			if (_border)
			{
				ControlPaint.DrawSelectionFrame(g, true,
					this.ClientRectangle, InnerMargin, Color.White);
			}
			if (_flags[(int)Flags.AntiAlias])
				g.SmoothingMode = SmoothingMode.AntiAlias;
			//translate
			g.TranslateTransform((float)_gitter.X, (float)_gitter.Y);
			g.RenderingOrigin = _gitter.Location;
			for (int i = 0; i < _formulas.Count; i++)
			{
				DrawPath(g, _formulas[i]);
			}
			DrawSelection(g);
			g.SmoothingMode = SmoothingMode.None;
			g.TranslateTransform(-(float)_gitter.X, -(float)_gitter.Y);
			if (_flags[(int)Flags.Rulers])
				DrawRulers(g);
		}

		private void DrawPath(Graphics gr, NuGenFormula fml)
		{
			if (fml.HasChildren)
			{
				for (int i = 0; i < fml.Children.Count; i++)
					DrawPath(gr, fml.Children[i]);
				return;
			}
			if (fml.draw)
			{
				_pen.Color = fml.frb;
				gr.DrawPath(_pen, fml.pth);
				if (_flags[(int)Flags.Asymptotes])
					gr.DrawPath(_hatchPen, fml.pthasympt);
			}
		}

		private void DrawRulers(Graphics gr)
		{
			_brush.Color = _colors[(int)Colors.Ruler];
			gr.FillRectangle(_brush, 0, 0, 16, this.Height);
			gr.FillRectangle(_brush, 0, 0, this.Width, 16);
			_sf.LineAlignment = _sf.Alignment = StringAlignment.Center;
			_sf.FormatFlags = StringFormatFlags.NoClip | StringFormatFlags.DirectionVertical;
			int maxval = Math.Max(Math.Abs(_gitter.X) + this.Width, Math.Abs(_gitter.Y) + this.Height) / _gitter.Width;
			int d, i =
				(int)Math.Ceiling(gr.MeasureString(maxval.ToString() + "xx",
				SystemInformation.MenuFont, 100, _sf).Height / (double)_gitter.Width);
			//int i=r_Gitter.Height<20?2:1;// i*=r_Gitter.Height;
			for (int y = _gitter.Y % _gitter.Height; y <= this.Height; y += _gitter.Height)//horizontal
			{
				d = (y - _gitter.Y) / -_gitter.Height;
				if (d % i == 0)
				{
					gr.DrawString(d.ToString() + _description, _font, Brushes.White, new RectangleF(
						0f, (float)y, 16f, 0f), _sf);
				}
			}
			_sf.FormatFlags = StringFormatFlags.NoClip;
			for (int x = _gitter.X % _gitter.Width; x <= this.Width; x += _gitter.Width)//vertikal
			{
				d = (x - _gitter.X) / _gitter.Width;
				if (d % i == 0)
				{
					gr.DrawString(d.ToString() + _description, _font, Brushes.White, new RectangleF(
						(float)x, 0f, 0f, 16f), _sf);
				}
			}
		}

		private void DrawDescriptionY(Graphics gr)
		{
			_brush.Color = _pen.Color = _colors[(int)Colors.DescColor];
			string bes;
			int i = _gitter.Height < 20 ? 2 : 1;
			i *= _gitter.Height;
			_sf.FormatFlags = StringFormatFlags.NoClip;
			_sf.Alignment = StringAlignment.Near;
			_sf.LineAlignment = StringAlignment.Center;
			for (int y = _gitter.Y % _gitter.Height; y <= this.Height; y += _gitter.Height)//horizontal
			{
				if ((y - _gitter.Y) % i == 0)
				{
					bes = ((y - _gitter.Y) / -_gitter.Height).ToString() + _description;
					gr.DrawString(bes, _font, _brush, new RectangleF(
						(float)_gitter.X + 3f, (float)y, 0f, 0f), _sf);
					gr.DrawLine(_pen, _gitter.X - 2, y, _gitter.X + 2, y);
				}
			}
		}

		private void DrawDescriptionX(Graphics gr)
		{
			_brush.Color = _pen.Color = _colors[(int)Colors.DescColor];
			string bes;
			int i = _gitter.Width < 20 ? 2 : 1;
			i *= _gitter.Width;
			_sf.FormatFlags = StringFormatFlags.NoClip | StringFormatFlags.DirectionVertical;
			_sf.Alignment = StringAlignment.Near;
			_sf.LineAlignment = StringAlignment.Center;
			for (int x = _gitter.X % _gitter.Width; x <= this.Width; x += _gitter.Width)//vertikal
			{
				if ((x - _gitter.X) % i == 0)
				{
					bes = ((x - _gitter.X) / _gitter.Width).ToString() + _description;
					gr.DrawString(bes, _font, _brush, new RectangleF(
						(float)x, (float)_gitter.Y + 3f, 0f, 0f), _sf);
					gr.DrawLine(_pen, x, _gitter.Y - 2, x, _gitter.Y + 2);
				}
			}
		}

		private void DrawAxis(Graphics gr)
		{
			_pen.Color = _colors[(int)Colors.DescColor];
			gr.DrawLine(_pen, 0, _gitter.Y, this.Width, _gitter.Y);//horizontal
			gr.DrawLine(_pen, _gitter.X, 0, _gitter.X, this.Height);//vertikal
		}

		private void DrawGrid(Graphics gr)
		{
			_pen.Color = _colors[(int)Colors.GridColor];
			for (int y = _gitter.Y % _gitter.Height; y <= this.Height; y += _gitter.Height)//horizontal
			{
				gr.DrawLine(_pen, 0, y, this.Width, y);
			}
			for (int x = _gitter.X % _gitter.Width; x <= this.Width; x += _gitter.Width)//vertikal
			{
				gr.DrawLine(_pen, x, 0, x, this.Height);
			}
		}

		private void DrawSelection(Graphics gr)
		{
			if (_selection.End - _selection.Start == 0f)
				return;
			if (!(_tool is NuGenSelectionPlotTool))
				return;

			NuGenPlotPaintInterval iv = ScaleMathToClient(RightOrderInterval(_selection));
			_brush.Color = _colors[(int)Colors.Selection];

			Rectangle rct = Rectangle.FromLTRB(iv.Start, -_gitter.Y, iv.End, 0);
			rct.Height = this.Height;
			//Use this for range selection
			gr.FillRectangle(_brush, rct);
			gr.DrawLine(Pens.White, rct.X, rct.Y, rct.X, rct.Bottom);
			gr.DrawLine(Pens.White, rct.Right, rct.Y, rct.Right, rct.Bottom);
		}

		#endregion

		#region Methods.Selection

		/// <summary>
		/// </summary>
		public NuGenPlotPaintInterval ScaleMathToClient(NuGenPlotPaintIntervalF interval)
		{
			interval.Start *= (float)_gitter.Width;
			interval.End *= (float)_gitter.Width;
			return NuGenPlotPaintInterval.Round(interval);
		}

		/// <summary>
		/// </summary>
		public NuGenPlotPaintInterval ScaleTranslateMathToClient(NuGenPlotPaintIntervalF interval)
		{
			NuGenPlotPaintInterval ret = ScaleMathToClient(interval);
			ret.Start += _gitter.X;
			ret.End += _gitter.X;
			return ret;
		}

		/// <summary>
		/// </summary>
		public NuGenPlotPaintIntervalF ScaleTranslateClientToMath(NuGenPlotPaintInterval interval, bool round)
		{
			interval.Start -= _gitter.X;
			interval.End -= _gitter.X;
			NuGenPlotPaintIntervalF ret = new NuGenPlotPaintIntervalF(
				(float)(interval.Start) / (float)(_gitter.Width),
				(float)(interval.End) / (float)(_gitter.Width));
			if (round)
				return new NuGenPlotPaintIntervalF(
					(float)Math.Round((double)ret.Start, 0),
					(float)Math.Round((double)ret.End, 0));
			else
				return ret;
		}

		/// <summary>
		/// </summary>
		public void SetSelectionScreenRound(NuGenPlotPaintInterval selscr)
		{
			Selection = ScaleTranslateClientToMath(selscr, true);
		}

		#endregion

		#region Methods.Internal

		internal static NuGenPlotPaintIntervalF RightOrderInterval(NuGenPlotPaintIntervalF iv)
		{
			return new NuGenPlotPaintIntervalF(
				Math.Min(iv.Start, iv.End),
				Math.Max(iv.Start, iv.End)
			);
		}

		#endregion

		#region Methods.Private

		private void CalculatePaths(bool force)
		{
			w = (double)_gitter.Width;
			begin = (double)(-_gitter.X) - _paintBuffer * (double)(this.Width);
			end = (double)(this.Width) * (1.0 + _paintBuffer) - (double)(_gitter.X);
			h = (float)_gitter.Height;
			pmax = (int)Math.Ceiling(end - begin);
			pcurr = i = 0;
			if (_flags[(int)Flags.Asymptotes])
				foreach (NuGenFormula fml in _formulas)
				{
					i++;
					CalculatePathInternalAsy(fml, force);
				}
			else
				foreach (NuGenFormula fml in _formulas)
				{
					i++;
					CalculatePathInternal(fml, force);
				}
			if (Success != null)
				Success(0, 100, "");
		}

		private void CalculatePathInternal(NuGenFormula fml, bool force)
		{
			if (fml.draw && (force || _forceFormula == i - 1
				|| fml.PaintValidate.Start > (double)(-_gitter.X) / w
				|| fml.PaintValidate.End < (double)(this.Width - _gitter.X) / w))
			{
				if (fml.HasChildren)
				{
					for (int _i = 0; _i < fml.Children.Count; _i++)
					{
						CalculatePathInternal(fml.Children[_i], force);
					}
					return;
				}
				NuGenInterpreter.SetVariableValue(_vara, fml.Param);
				fml.pth.Reset();
				fml.pthasympt.Reset();
				py = GetVal(fml._ve, Math.Max(begin / w, fml.Interval.Start)) * -h;/////
				pcurr = 0;
				for (d = Math.Max(begin, fml.Interval.Start * w) + 1.0;
					d <= Math.Min(end, fml.Interval.End * w); d++, pcurr++)
				{
					//intptr.WertändernUS(d/w);
					y = GetVal(fml._ve, d / w) * -h;///////
					fml.pth.AddLine((float)(d - 1.0), py, (float)d, y);
					py = y;
					if (Success != null)
					{
						Success(
							pcurr,
							pmax,
							string.Format(
								Resources.Text_VisiPlot2D_SuccessDescription,
								i.ToString(CultureInfo.CurrentCulture),
								_formulas.Count.ToString(CultureInfo.CurrentCulture)
							)
						);
					}
				}
				fml.PaintValidate = new NuGenPlotInterval(begin / w, end / w);
				_forceFormula = -1;
			}
		}
		private void CalculatePathInternalAsy(NuGenFormula fml, bool force)
		{
			if (fml.draw && (force || _forceFormula == i - 1
				|| fml.PaintValidate.Start > (double)(-_gitter.X) / w
				|| fml.PaintValidate.End < (double)(this.Width - _gitter.X) / w))
			{
				if (fml.HasChildren)
				{
					for (int _i = 0; _i < fml.Children.Count; _i++)
					{
						CalculatePathInternalAsy(fml.Children[_i], force);
					}
					return;
				}
				NuGenInterpreter.SetVariableValue(_vara, fml.Param);
				fml.pth.Reset();
				fml.pthasympt.Reset();
				//intptr.WertändernUS(Math.Max(begin/w,fml.intervall.start));
				ppy = py = GetVal(fml._ve, Math.Max(begin / w, fml.Interval.Start)) * -h;/////
				SaveLine(fml.pthasympt, -2e5f * h, GetValIS(fml._ve, -2e5f) * -h, 2e5f * h, GetValIS(fml._ve, 2e5f) * -h);
				pcurr = 0;
				for (d = Math.Max(begin, fml.Interval.Start * w) + 1.0;
					d <= Math.Min(end, fml.Interval.End * w); d++, pcurr++)
				{
					//intptr.WertändernUS(d/w);
					y = GetVal(fml._ve, d / w) * -h;///////
					if (SAsymp(ppy, py, y))
					{
						fml.pthasympt.StartFigure();
						fml.pthasympt.AddLine((float)d, 2e5f, (float)d, -2e5f);
						fml.pth.StartFigure();
					}
					else
						fml.pth.AddLine((float)(d - 1.0), py, (float)d, y);
					ppy = py;
					py = y;
					if (Success != null)
					{
						Success(
							pcurr,
							pmax,
							string.Format(
								Resources.Text_VisiPlot2D_SuccessDescription,
								i.ToString(CultureInfo.CurrentCulture),
								_formulas.Count.ToString(CultureInfo.CurrentCulture)
							)
						);
					}
				}
				fml.PaintValidate = new NuGenPlotInterval(begin / w, end / w);
				_forceFormula = -1;
			}
		}

		private void TestTimer()
		{
			if (_timer.Enabled !=
			(_timer.Enabled = _border = (_flags[(int)Flags.AutoScroll] &&
				!_flags[(int)Flags.CenterAxis] &&
				!InnerMargin.Contains(this.PointToClient(Control.MousePosition)))))
				Reload();
		}

		#endregion

		#region EventHandlers

		private void _timer_Tick(object sender, EventArgs e)
		{
			Point pt = this.PointToClient(Control.MousePosition);
			TestTimer();
			if (_tool != null)
				_tool.TimerScroll(this, new MouseEventArgs(
					Control.MouseButtons, 0, pt.X, pt.Y, 0));
		}

		#endregion

		internal enum Colors
		{
			BackColor,
			GridColor,
			DescColor,
			Selection,
			Ruler
		}

		internal enum Flags
		{
			AntiAlias,
			DescriptionX,
			DescriptionY,
			Axis,
			Grid,
			CenterAxis,
			Asymptotes,
			AutoScroll,
			Rulers
		}

		private readonly static int _varx = NuGenInterpreter.GetVariableIndex('x');
		private readonly static int	_vara = NuGenInterpreter.GetVariableIndex('a');
		private NuGenFormulaCollection _formulas;
		private Timer _timer = new Timer();

		private Color[] _colors = new Color[] { Color.Black, Color.Green, Color.Lime, Color.FromArgb(30, 255, 255, 255), Color.FromArgb(80, 0, 0, 0), Color.FromArgb(100, 255, 255, 255) };
		private bool[] _flags = new bool[] { true, true, true, true, true, false, false, false, false };

		private Rectangle _gitter = new Rectangle(50, 50, 20, 20);
		private string _description;
		private double _paintBuffer = 0.0;
		private Font _font = new Font("Microsoft Sans Serif", 8.25f);
		private Pen _pen = new Pen(Color.White);
		private SolidBrush _brush = new SolidBrush(Color.White);
		private Pen _hatchPen = new Pen(new HatchBrush(HatchStyle.LargeCheckerBoard, Color.FromArgb(128, 200, 200, 200), Color.Transparent), 1f);
		private NuGenPlotPaintIntervalF _selection = NuGenPlotPaintIntervalF.Empty;
		private NuGenPlotToolBase _tool;
		private bool _border;
		private bool _updating;
		private StringFormat _sf;
		private int _forceFormula = -1;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenVisiPlot2D"/> class.
		/// </summary>
		public NuGenVisiPlot2D()
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.Selectable, true);
			this.SetStyle(ControlStyles.UserPaint, true);

			_formulas = new NuGenFormulaCollection(this);
			_timer.Interval = 50;
			_timer.Tick += new EventHandler(_timer_Tick);
			this.Reload();
			_sf = new StringFormat(StringFormatFlags.DirectionVertical | StringFormatFlags.NoClip);
			_sf.LineAlignment = StringAlignment.Center;
			_sf.Alignment = StringAlignment.Center;
		}

		/// <summary>
		/// </summary>
		/// <param name="disposing">
		/// <see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to realse only unmanaged resources.
		/// </param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_timer != null)
				{
					_timer.Tick -= _timer_Tick;
					_timer.Dispose();
					_timer = null;
				}
			}

			base.Dispose(disposing);
		}
	}
}
