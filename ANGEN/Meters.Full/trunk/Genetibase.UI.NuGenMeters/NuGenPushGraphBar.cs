/* -----------------------------------------------
 * NuGenPushGraphBar.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using Genetibase.Shared.Design;
using Genetibase.Shared;
using Genetibase.Shared.Drawing;
using Genetibase.UI.NuGenMeters.ComponentModel;
using Genetibase.WinApi;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.UI.NuGenMeters
{
	/// <summary>
	/// Flexible graph bar.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[ToolboxItem(false)]
	public class NuGenPushGraphBar : NuGenBarBase
	{
		#region Declarations.Consts

		/// <summary>
		/// Initial min value for the graph.
		/// </summary>
		private const int INITIAL_MIN = 0;

		/// <summary>
		/// Initial max value for the graph.
		/// </summary>
		private const int INITIAL_MAX = 100;

		/// <summary>
		/// Used for tweaking.
		/// </summary>
		private int PEN_WIDTH = 1;

		#endregion

		#region Declarations.Fields

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		private int maxCoords = -1;
		private int peekOffset = 0;
		private int pushOffset = 0;

		/// <summary>
		/// Contains all the graph lines to draw.
		/// </summary>
		private List<NuGenGraphLine> lines = new List<NuGenGraphLine>();

		IntPtr dcBack = IntPtr.Zero;
		IntPtr bmBack = IntPtr.Zero;

		#endregion

		#region Properties.Appearance

		/*
		 * ExtraPerformance
		 */

		private bool extraPerformance = false;

		/// <summary>
		/// Gets or sets the value indicating whether to use native GDI for rendering.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		[NuGenSRCategory("AppearanceCategory")]
		[NuGenSRDescription("ExtraPerformanceDescription")]
		public bool ExtraPerformance
		{
			get
			{
				return this.extraPerformance;
			}
			set
			{
				if (this.extraPerformance != value)
				{
					this.extraPerformance = value;
					this.OnExtraPerformanceChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly object EventExtraPerformanceChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:NuGenPushGraphBar.ExtraPerformance"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("ExtraPerformanceChangedDescription")]
		public event EventHandler ExtraPerformanceChanged
		{
			add
			{
				this.Events.AddHandler(EventExtraPerformanceChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventExtraPerformanceChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:NuGenPushGraphBar.ExtraPerformanceChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnExtraPerformanceChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventExtraPerformanceChanged, e);
		}

		/*
		 * ShowGrid
		 */

		/// <summary>
		/// Indicates whether to show the grid.
		/// </summary>
		private bool showGrid = true;

		/// <summary>
		/// Gets or sets the value indicating whether to show the grid.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("AppearanceCategory")]
		[DefaultValue(true)]
		[NuGenSRDescription("ShowGridDescription")]
		public virtual bool ShowGrid
		{
			get
			{
				return this.showGrid;
			}
			set
			{
				if (this.showGrid != value)
				{
					this.showGrid = value;
					this.OnShowGridChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly object EventShowGridChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenPushGraphBar.ShowGrid"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("ShowGridChangedDescription")]
		public event EventHandler ShowGridChanged
		{
			add
			{
				this.Events.AddHandler(EventShowGridChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventShowGridChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenPushGraphBar.ShowGridChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnShowGridChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventShowGridChanged, e);
		}

		#endregion

		#region Properties.Behavior

		/*
		 * Step
		 */

		/// <summary>
		/// Determines the line step for the graph.
		/// </summary>
		private int step = 5;

		/// <summary>
		/// Gets or sets the line step for the graph.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("BehaviorCategory")]
		[DefaultValue(5)]
		[NuGenSRDescription("StepDescription")]
		public virtual int Step
		{
			get
			{
				return this.step;
			}
			set
			{
				if (this.step != value)
				{
					value = Math.Max(value, 1);
					value = Math.Min(value, int.MaxValue);

					this.step = value;

					this.OnStepChanged(EventArgs.Empty);

					this.maxCoords = -1;
				}
			}
		}

		private static readonly object EventStepChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenPushGraphBar.Step"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("StepChangedDescription")]
		public event EventHandler StepChanged
		{
			add
			{
				this.Events.AddHandler(EventStepChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventStepChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenPushGraphBar.StepChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnStepChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventStepChanged, e);
		}

		#endregion

		#region Properties.Grid

		/*
		 * GridColor
		 */

		/// <summary>
		/// Determines the color of the grid.
		/// </summary>
		private Color gridColor = Color.Green;

		/// <summary>
		/// Gets or sets the color of the grid.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("GridCategory")]
		[DefaultValue(typeof(Color), "Green")]
		[NuGenSRDescription("GridColorDescription")]
		public virtual Color GridColor
		{
			get
			{
				return this.gridColor;
			}
			set
			{
				if (this.gridColor != value)
				{
					this.gridColor = value;
					this.OnGridColorChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly object EventGridColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenPushGraphBar.GridColor"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("GridColorChangedDescription")]
		public event EventHandler GridColorChanged
		{
			add
			{
				this.Events.AddHandler(EventGridColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventGridColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenPushGraphBar.GridColorChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnGridColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventGridColorChanged, e);
		}

		/*
		 * GridStep
		 */

		/// <summary>
		/// Determines the step of the grid.
		/// </summary>
		private int gridStep = 15;

		/// <summary>
		/// Gets or sets the step for the grid.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("GridCategory")]
		[DefaultValue(15)]
		[NuGenSRDescription("GridStepDescription")]
		public virtual int GridStep
		{
			get
			{
				return this.gridStep;
			}
			set
			{
				if (this.gridStep != value)
				{
					value = Math.Max(value, 1);
					value = Math.Min(value, int.MaxValue);

					this.gridStep = value;

					this.OnGridStepChanged(EventArgs.Empty);

					this.Refresh();
				}
			}
		}

		private static readonly object EventGridStepChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenPushGraphBar.GridStep"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("GridStepChangedDescription")]
		public event EventHandler GridStepChanged
		{
			add
			{
				this.Events.AddHandler(EventGridStepChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventGridStepChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenPushGraphBar.GridStepChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnGridStepChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventGridStepChanged, e);
		}

		/*
		 * GridTransparency
		 */

		/// <summary>
		/// Determines the grid transparency level.
		/// </summary>
		private int gridTransparency = 0;

		/// <summary>
		/// Gets or sets the grid transparency level.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("GridCategory")]
		[DefaultValue(0)]
		[NuGenSRDescription("GridTransparencyDescription")]
		[Editor(typeof(NuGenTransparencyEditor), typeof(UITypeEditor))]
		[TypeConverter(typeof(NuGenTransparencyConverter))]
		public virtual int GridTransparency
		{
			get
			{
				return this.gridTransparency;
			}
			set
			{
				if (this.gridTransparency != value)
				{
					value = Math.Max(value, 0);
					value = Math.Min(value, 100);

					this.gridTransparency = value;

					this.OnGridTransparencyChanged(EventArgs.Empty);

					this.Refresh();
				}
			}
		}

		private static readonly object EventGridTransparencyChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenPushGraphBar.GridTransparency"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("GridTransparencyChangedDescription")]
		public event EventHandler GridTransparencyChanged
		{
			add
			{
				this.Events.AddHandler(EventGridTransparencyChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventGridTransparencyChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenPushGraphBar.GridTransparencyChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnGridTransparencyChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventGridTransparencyChanged, e);
		}

		#endregion

		#region Properties.NonBrowsable

		/// <summary>
		/// Gets or sets the collection of lines for the graph.
		/// </summary>
		[Browsable(false)]
		public List<NuGenGraphLine> Lines
		{
			get
			{
				return this.lines;
			}
			set
			{
				this.lines = value;
			}
		}

		#endregion

		#region Properties.Protected

		/// <summary>
		/// Gets or sets the maximum value for this meter.
		/// </summary>
		protected override float Maximum
		{
			get
			{
				return base.Maximum;
			}
			set
			{
				value = Math.Max(value, 1);
				value = Math.Min(value, float.MaxValue);

				base.Maximum = value;
			}
		}

		#endregion

		#region Methods.Public

		/// <summary>
		/// Adds a new graph line to the collection.
		/// </summary>
		/// <param name="lineColor">A <c>System.Drawing.Color</c> that represents the color of the graph line.</param>
		/// <param name="id">The Id of the graph line in the collection.</param>
		public void AddLine(int id, Color lineColor)
		{
			NuGenGraphLine pushGraphLine = new NuGenGraphLine(id);
			pushGraphLine.LineColor = lineColor;

			this.lines.Add(pushGraphLine);
		}

		/// <summary>
		/// Gets a value indicating whether to show the graph as a set of bars.
		/// </summary>
		/// <param name="index">The index of the graph line in the collection.</param>
		/// <returns><c>true</c> if the graph line is shown as a set of bars; otherwise, <c>false</c>.</returns>
		public bool GetIsBar(int index)
		{
			NuGenGraphLine pushGraphLine = this.GetLineFromIndex(index);

			if (pushGraphLine != null)
				return pushGraphLine.IsBar;
			else
				return false;
		}

		/// <summary>
		/// Gets the color of a graph line at the specified index in the collection.
		/// </summary>
		/// <param name="index">The index of the graph line to get the color for.</param>
		/// <returns>A <c>System.Drawing.Color</c> that represents the color of the graph line.</returns>
		public Color GetLineColor(int index)
		{
			NuGenGraphLine pushGraphLine = this.GetLineFromIndex(index);
			return (pushGraphLine != null) ? pushGraphLine.LineColor : Color.Black;
		}

		/// <summary>
		/// Adds a new value to the specified graph line.
		/// </summary>
		/// <param name="index">The index of the graph line to add the value to.</param>
		/// <param name="value">The value to add.</param>
		public void Push(int index, float value)
		{
			NuGenGraphLine pushGraphLine = this.GetLineFromIndex(index);

			value = Math.Max(value, this.Minimum);
			value = Math.Min(value, int.MaxValue);

			if (value > this.Maximum)
			{
				this.Maximum = value;
			}

			value -= this.Minimum;
			value += this.peekOffset;

			pushGraphLine.Values.Add(value);
		}

		/// <summary>
		/// Removes a graph line at the specified index in the collection.
		/// </summary>
		/// <param name="index">The index of the graph line to remove.</param>
		public void RemoveLine(int index)
		{
			for (int i = 0; i < this.lines.Count; i++)
			{
				if (this.lines[i].Index == index)
				{
					this.lines.RemoveAt(i);
					return;
				}
			}
		}

		/// <summary>
		/// Sets the value indicating whether to show the graph as a set of bars.
		/// </summary>
		/// <param name="index">The index of the graph line in the collection.</param>
		/// <param name="isBar"><c>true</c> to show the graph line as a set of bars; otherwise, <c>false</c>.</param>
		public void SetIsBar(int index, bool isBar)
		{
			NuGenGraphLine pushGraphLine = this.GetLineFromIndex(index);

			if (pushGraphLine != null)
			{
				pushGraphLine.IsBar = isBar;
			}
		}

		/// <summary>
		/// Sets the color of a graph line at the specified index in the collection.
		/// </summary>
		/// <param name="index">The index of the line to set the color for.</param>
		/// <param name="lineColor">The color to set for the line.</param>
		public void SetLineColor(int index, Color lineColor)
		{
			NuGenGraphLine pushGraphLine = this.GetLineFromIndex(index);
			pushGraphLine.LineColor = lineColor;
		}

		/// <summary>
		/// Reinitializes the control.
		/// </summary>
		public virtual new void Update()
		{
			int maxPoints = 0;

			for (int n = 0; n < this.lines.Count; n++)
			{
				if (maxPoints < this.lines[n].Values.Count)
				{
					maxPoints = this.lines[n].Values.Count;
				}
			}

			if (maxPoints >= this.maxCoords)
			{
				this.pushOffset = (this.pushOffset - (((maxPoints - this.maxCoords) + 1) * this.Step)) % this.GridStep;
			}

			this.Refresh();
		}

		#endregion

		#region Methods.Protected.Overriden

		/*
		 * OnPaint
		 */

		/// <summary>
		/// Draws the control.
		/// </summary>
		/// <param name="e">Provides data for the <c>System.Windows.Forms.Control.Paint</c> event.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			if (this.ExtraPerformance)
			{
				this.RenderUsingGdi(g);
			}
			else
			{
				this.RenderUsingGdiPlus(g);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.SizeChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		protected override void OnSizeChanged(EventArgs e)
		{
			this.maxCoords = -1;
			base.OnSizeChanged(e);
		}

		#endregion

		#region Methods.Drawing.GDI

		/*
		 * DrawBarGdi
		 */

		/// <summary>
		/// Draws a bar using native GDI.
		/// </summary>
		/// <param name="dc">Specifies drawing context for this <see cref="T:Genetibase.UI.NuGenPushGraphBar"/>.</param>
		/// <param name="rect">Specifies the bar bounds.</param>
		/// <param name="graphLine">Specifies graph line parameters.</param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="graphLine"/> is <see langword="null"/>.</para>
		/// </exception>
		protected virtual void DrawBarGdi(IntPtr dc, Rectangle rect, NuGenGraphLine graphLine)
		{
			if (graphLine == null)
			{
				throw new ArgumentNullException("graphLine");
			}

			RECT bufferRect = (RECT)rect;
			User32.FillSolidRect(dc, ref bufferRect, graphLine.LineColor);
		}

		/*
		 * DrawGraphGdi
		 */

		/// <summary>
		/// Draws the graph using native GDI.
		/// </summary>
		/// <param name="dc">Specifies drawing context for this <see cref="T:Genetibase.UI.NuGenPushGraphBar"/>.</param>
		/// <param name="rect">Specifies the <see cref="T:System.Drawing.Rectangle"/> to fit the graph in.</param>
		protected virtual void DrawGraphGdi(IntPtr dc, Rectangle rect)
		{
			int maxPoints = 0;

			if (this.maxCoords == -1)
			{
				/* Maximum push points not yet calculated. */

				this.maxCoords = (rect.Width / this.Step) + 2
					+ ((rect.Width % this.Step > 0) ? 1 : 0);

				if (this.maxCoords <= 0)
				{
					this.maxCoords = 1;
				}
			}

			for (int lineIndex = 0; lineIndex < this.lines.Count; lineIndex++)
			{
				if (maxPoints < this.lines[lineIndex].Values.Count)
				{
					maxPoints = this.lines[lineIndex].Values.Count;
				}
			}

			if (maxPoints == 0 /* No lines to draw. */)
			{
				return;
			}

			for (int lineIndex = 0; lineIndex < this.lines.Count; lineIndex++)
			{
				/* 
				 * If the line has less push points than the line with the greatest 
				 * number of push points, new push points are appended with 
				 * the same value as the previous push point. If no push points
				 * exist for the line, one is added with the least value possible.
				 */

				NuGenGraphLine pushGraphLine = this.lines[lineIndex];

				if (pushGraphLine.Values.Count == 0)
				{
					pushGraphLine.Values.Add(this.Minimum);
				}

				while (pushGraphLine.Values.Count < maxPoints)
				{
					pushGraphLine.Values.Add(pushGraphLine.Values[pushGraphLine.Values.Count - 1]);
				}

				while (this.lines[lineIndex].Values.Count >= this.maxCoords)
				{
					this.lines[lineIndex].Values.RemoveAt(0);
				}

				if (this.lines[lineIndex].Values.Count == 0 /* No push points to draw. */)
				{
					return;
				}

				/* Now prepare to draw the line or bar. */

				IntPtr penLine = Gdi32.CreatePen(
					WinGdi.PS_SOLID,
					PEN_WIDTH,
					ColorTranslator.ToWin32(this.lines[lineIndex].LineColor)
				);

				IntPtr hOldPen = Gdi32.SelectObject(dc, penLine);

				if (pushGraphLine.IsBar)
				{
					Gdi32.MoveTo(dc, rect.Left, rect.Height);
				}
				else
				{
					float initialValue = pushGraphLine.Values[0];
					float percent = (float)(rect.Height - PEN_WIDTH) / (this.Maximum - this.Minimum);
					float relValue = (float)(rect.Height - PEN_WIDTH) - initialValue * percent;

					Gdi32.MoveTo(dc, (int)rect.Left, (int)(maxPoints == 1 ? rect.Height : relValue));
				}

				for (int valueIndex = 0; valueIndex < pushGraphLine.Values.Count; valueIndex++)
				{
					int xOffset = rect.Left + (valueIndex * this.Step);
					float initialValue = pushGraphLine.Values[valueIndex];
					float percent = (float)rect.Height / (float)(this.Maximum - this.Minimum);
					int relValue = Math.Max(PEN_WIDTH * 2, (int)(rect.Height - initialValue * percent));

					if (pushGraphLine.IsBar)
					{
						/* Draw a bar. */

						Rectangle rectBar = new Rectangle(
							xOffset,
							relValue,
							this.Step,
							rect.Height
							);

						this.DrawBarGdi(dc, rectBar, pushGraphLine);
					}
					else
					{
						/* Draw a line. */
						Gdi32.LineTo(dc, xOffset, relValue);
					}
				}

				Gdi32.SelectObject(dc, hOldPen);
				Gdi32.DeleteObject(penLine);
			}
		}

		/*
		 * DrawGridGdi
		 */

		/// <summary>
		/// Draw the grid overlay using native GDI.
		/// </summary>
		/// <param name="dc">Specifies drawing context for this <see cref="T:Genetibase.UI.NuGenPushGraphBar"/>.</param>
		/// <param name="rect">Specifies the <see cref="T:System.Drawing.Rectangle"/> to draw within.</param>
		/// <param name="gridColor">Specifies the grid color.</param>
		/// <param name="gridStep">Specifies the grid step.</param>
		protected virtual void DrawGridGdi(IntPtr dc, Rectangle rect, Color gridColor, int gridStep)
		{
			using (Pen pen = new Pen(gridColor))
			{
				IntPtr hPen = Gdi32.CreatePen(pen);
				IntPtr hOldPen = Gdi32.SelectObject(dc, hPen);

				for (int row = rect.Height; row >= 0; row -= gridStep)
				{
					Gdi32.MoveTo(dc, rect.Left, row);
					Gdi32.LineTo(dc, rect.Right, row);
				}

				for (int col = rect.Left + this.pushOffset; col < rect.Right; col += gridStep)
				{
					if (col < rect.Left)
					{
						continue;
					}

					Gdi32.MoveTo(dc, col, 0);
					Gdi32.LineTo(dc, col, rect.Height);
				}

				Gdi32.SelectObject(dc, hOldPen);
				Gdi32.DeleteObject(hPen);
			}
		}

		/*
		 * FreeBackBuffer
		 */

		/// <summary>
		/// Creates the offscreen DC and associated bitmap. 
		/// </summary>
		/// <param name="dc">Specifies drawing context for this <see cref="T:Genetibase.UI.NuGenPushGraphBar"/>.</param>
		protected virtual void FreeBackBuffer(IntPtr dc)
		{
			Rectangle clientRect = this.ClientRectangle;

			Gdi32.BitBlt(
				dc,
				clientRect.Left,
				clientRect.Top,
				clientRect.Width,
				clientRect.Height,
				this.dcBack,
				0,
				0,
				WinGdi.SRCCOPY
			);

			Gdi32.DeleteObject(this.bmBack);
			Gdi32.DeleteDC(this.dcBack);
		}

		/*
		 * InitBackBuffer
		 */

		/// <summary>
		/// Creates the offscreen DC and associated bitmap. 
		/// </summary>
		/// <param name="dc">Specifies drawing context for this <see cref="T:Genetibase.UI.NuGenPushGraphBar"/>.</param>
		/// <returns></returns>
		protected virtual IntPtr InitBackBuffer(IntPtr dc)
		{
			this.dcBack = Gdi32.CreateCompatibleDC(dc);
			Rectangle clientRectangle = this.ClientRectangle;

			this.bmBack = Gdi32.CreateCompatibleBitmap(
				dc,
				this.ClientRectangle.Width,
				this.ClientRectangle.Height
			);

			Gdi32.SelectObject(this.dcBack, this.bmBack);
			return dcBack;
		}

		/*
		 * RenderUsingGdi
		 */

		/// <summary>
		/// Renders this <see cref="T:Genetibase.UI.NuGenMeters.NuGenPushGraphBar"/> using native GDI algorythms.
		/// </summary>
		/// <param name="g">Specifies a GDI+ drawing surface.</param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// </exception>
		protected virtual void RenderUsingGdi(Graphics g)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			IntPtr paintDC = g.GetHdc();

			/* First we crate the back buffer. */
			IntPtr dc = this.InitBackBuffer(paintDC);

			/* Ensure we don't draw out of our client rectangle. */
			Gdi32.IntersectClipRect(dc, this.ClientRectangle);

			/* Fill the background. */

			RECT rect = (RECT)this.ClientRectangle;
			User32.FillSolidRect(dc, ref rect, this.BackgroundColor);

			Gdi32.SetBkMode(dc, Gdi32.TRANSPARENT);

			/*
			 * DrawGrid
			 */

			if (this.ShowGrid)
			{
				this.DrawGridGdi(dc, this.ClientRectangle, this.GridColor, this.GridStep);
			}

			/*
			 * DrawGraph
			 */

			this.DrawGraphGdi(dc, this.ClientRectangle);
			this.FreeBackBuffer(paintDC);

			g.ReleaseHdc(paintDC);

			NuGenControlPaint.DrawBorder(g, this.ClientRectangle, this.BorderColor, this.BorderStyle);
		}

		#endregion

		#region Methods.Drawing.GDIPlus

		/*
		 * DrawBarGdiPlus
		 */

		/// <summary>
		/// Draws a bar within the specified rectangle with the specified color.
		/// </summary>
		/// <param name="g">A <c>System.Drawing.Graphics</c> to draw on.</param>
		/// <param name="rect">A <c>System.Drawing.RectangleF</c> to fit the bar in.</param>
		/// <param name="line">Incapsulates a graph line.</param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="line"/> is <see langword="null"/>.</para>
		/// </exception>
		protected virtual void DrawBarGdiPlus(Graphics g, RectangleF rect, NuGenGraphLine line)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			if (line == null)
			{
				throw new ArgumentNullException("line");
			}

			using (SolidBrush sb = new SolidBrush(NuGenControlPaint.ColorFromArgb(this.ForegroundTransparency, line.LineColor)))
			{
				g.FillRectangle(sb, rect);
			}
		}

		/*
		 * DrawGridGdiPlus
		 */

		/// <summary>
		/// Draws the grid within the specified rectangle with the specified color and step.
		/// </summary>
		/// <param name="g">A <c>System.Drawing.Graphics</c> to draw on.</param>
		/// <param name="rect">A <c>System.Drawing.Rectangle</c> to fit the grid in.</param>
		/// <param name="gridColor">Color of the grid.</param>
		/// <param name="gridStep">Step of the grid.</param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// </exception>
		protected virtual void DrawGridGdiPlus(Graphics g, Rectangle rect, Color gridColor, int gridStep)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			using (Pen p = new Pen(gridColor))
			{
				for (int row = rect.Height; row >= 0; row -= gridStep)
				{
					Point startPoint = new Point(rect.Left, row);
					Point endPoint = new Point(rect.Right, row);

					g.DrawLine(p, startPoint, endPoint);
				}

				for (int col = rect.Left + this.pushOffset; col < rect.Right; col += gridStep)
				{
					if (col < rect.Left)
					{
						continue;
					}

					Point startPoint = new Point(col, 0);
					Point endPoint = new Point(col, rect.Height);

					g.DrawLine(p, startPoint, endPoint);
				}
			}
		}

		/*
		 * DrawGraphGdiPlus
		 */

		/// <summary>
		/// Draws the graph.
		/// </summary>
		/// <param name="g">A <c>System.Drawing.Graphics</c> to draw on.</param>
		/// <param name="rect">A <c>System.Drawing.Rectangle</c> to fit the graph in.</param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// </exception>
		protected virtual void DrawGraphGdiPlus(Graphics g, Rectangle rect)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			int maxPoints = 0;

			if (this.maxCoords == -1)
			{
				/* Maximum push points not yet calculated. */

				this.maxCoords = (rect.Width / this.Step) + 2
					+ ((rect.Width % this.Step > 0) ? 1 : 0);

				if (this.maxCoords <= 0)
				{
					this.maxCoords = 1;
				}
			}

			for (int lineIndex = 0; lineIndex < this.lines.Count; lineIndex++)
			{
				if (maxPoints < this.lines[lineIndex].Values.Count)
				{
					maxPoints = this.lines[lineIndex].Values.Count;
				}
			}

			if (maxPoints == 0 /* No lines to draw. */)
			{
				return;
			}

			for (int lineIndex = 0; lineIndex < this.lines.Count; lineIndex++)
			{
				/* 
				 * If the line has less push points than the line with the greatest 
				 * number of push points, new push points are appended with 
				 * the same value as the previous push point. If no push points
				 * exist for the line, one is added with the least value possible.
				 */

				NuGenGraphLine pushGraphLine = this.lines[lineIndex];

				if (pushGraphLine.Values.Count == 0)
				{
					pushGraphLine.Values.Add(this.Minimum);
				}

				while (pushGraphLine.Values.Count < maxPoints)
				{
					pushGraphLine.Values.Add(pushGraphLine.Values[pushGraphLine.Values.Count - 1]);
				}

				while (this.lines[lineIndex].Values.Count >= this.maxCoords)
				{
					this.lines[lineIndex].Values.RemoveAt(0);
				}

				if (this.lines[lineIndex].Values.Count == 0 /* No push points to draw. */)
				{
					return;
				}

				using (Pen p = new Pen(NuGenControlPaint.ColorFromArgb(this.ForegroundTransparency, (this.lines[lineIndex] as NuGenGraphLine).LineColor)))
				{
					PointF startPoint = PointF.Empty;
					PointF endPoint = PointF.Empty;

					if (pushGraphLine.IsBar)
					{
						startPoint = new PointF(rect.Left, rect.Height);
					}
					else
					{
						float initialValue = pushGraphLine.Values[0];
						float percent = (float)(rect.Height - PEN_WIDTH) / (this.Maximum - this.Minimum);
						float relValue = (float)(rect.Height - PEN_WIDTH) - initialValue * percent;

						startPoint = new PointF(rect.Left, maxPoints == 1 ? rect.Height : relValue);
					}

					for (int valueIndex = 0; valueIndex < pushGraphLine.Values.Count; valueIndex++)
					{
						float xOffset = rect.Left + (valueIndex * this.Step);
						float initialValue = pushGraphLine.Values[valueIndex];
						float percent = (float)rect.Height / (float)(this.Maximum - this.Minimum);
						float relValue = Math.Max(PEN_WIDTH * 2, (float)rect.Height - initialValue * percent);

						if (pushGraphLine.IsBar)
						{
							/* Draw a bar. */

							RectangleF rectBar = new RectangleF(
								xOffset,
								relValue,
								this.Step,
								rect.Height
								);

							this.DrawBarGdiPlus(g, rectBar, pushGraphLine);
						}
						else
						{
							/* Draw a line. */

							endPoint = new PointF(xOffset, relValue);
							g.DrawLine(p, startPoint, endPoint);
							startPoint = endPoint;
						}
					}
				}
			}
		}

		/*
		 * RenderUsingGdiPlus
		 */

		/// <summary>
		/// Renders this <see cref="T:Genetibase.UI.NuGenMeters.NuGenPushGraphBar"/> using GDI+ algorythms.
		/// </summary>
		/// <param name="g">Specifies a GDI+ drawing surface.</param>
		protected virtual void RenderUsingGdiPlus(Graphics g)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			// High quality drawing.
			g.SmoothingMode = SmoothingMode.AntiAlias;

			Rectangle tweakedRectangle = new Rectangle(
				this.ClientRectangle.X,
				this.ClientRectangle.Y,
				this.ClientRectangle.Width - PEN_WIDTH,
				this.ClientRectangle.Height - PEN_WIDTH * 3
				);

			/*
			 * Background.
			 */

			if (this.BackgroundImage == null)
			{
				switch (this.BackgroundStyle)
				{
					case NuGenBackgroundStyle.Gradient:
					using (LinearGradientBrush lgb = new LinearGradientBrush(
							   tweakedRectangle,
							   NuGenControlPaint.ColorFromArgb(this.BackgroundTransparency, this.BackGradientStartColor),
							   NuGenControlPaint.ColorFromArgb(this.BackgroundTransparency, this.BackGradientEndColor),
							   90
							   ))
					{
						g.FillRectangle(lgb, this.ClientRectangle);
					}
					break;

					case NuGenBackgroundStyle.Tube:
					using (LinearGradientBrush lgb = new LinearGradientBrush(
							   tweakedRectangle,
							   NuGenControlPaint.ColorFromArgb(this.BackgroundTransparency, this.BackTubeGradientStartColor),
							   NuGenControlPaint.ColorFromArgb(this.BackgroundTransparency, this.BackTubeGradientEndColor),
							   90
							   ))
					{
						ColorBlend colorBlend = new ColorBlend(3);

						colorBlend.Colors = new Color[] {
																NuGenControlPaint.ColorFromArgb(this.BackgroundTransparency, this.BackTubeGradientEndColor),
																NuGenControlPaint.ColorFromArgb(this.BackgroundTransparency, this.BackTubeGradientStartColor),
																NuGenControlPaint.ColorFromArgb(this.BackgroundTransparency, this.BackTubeGradientEndColor)
															};

						colorBlend.Positions = new float[] { 0.0f, 0.5f, 1.0f };

						lgb.InterpolationColors = colorBlend;

						g.FillRectangle(lgb, this.ClientRectangle);
					}
					break;

					case NuGenBackgroundStyle.VerticalGradient:
					using (LinearGradientBrush lgb = new LinearGradientBrush(
							   tweakedRectangle,
							   NuGenControlPaint.ColorFromArgb(this.BackgroundTransparency, this.BackGradientStartColor),
							   NuGenControlPaint.ColorFromArgb(this.BackgroundTransparency, this.BackGradientEndColor),
							   360
							   ))
					{
						g.FillRectangle(lgb, this.ClientRectangle);
					}
					break;
				}
			}
			else
			{
				if (this.StretchImage)
				{
					g.DrawImage(
						this.BackgroundImage,
						tweakedRectangle,
						0,
						0,
						this.BackgroundImage.Width,
						this.BackgroundImage.Height,
						GraphicsUnit.Pixel,
						NuGenControlPaint.GetTransparentImageAttributes(this.BackgroundTransparency, false)
						);
				}
				else
				{
					g.DrawImage(
						this.BackgroundImage,
						tweakedRectangle,
						tweakedRectangle.X,
						tweakedRectangle.Y,
						tweakedRectangle.Width,
						tweakedRectangle.Height,
						GraphicsUnit.Pixel,
						NuGenControlPaint.GetTransparentImageAttributes(this.BackgroundTransparency, true)
						);
				}
			}

			/*
			 * Grid.
			 */

			if (this.ShowGrid)
			{
				this.DrawGridGdiPlus(g, tweakedRectangle, NuGenControlPaint.ColorFromArgb(this.GridTransparency, this.GridColor), this.GridStep);
			}

			/*
			 * Graph.
			 */

			this.DrawGraphGdiPlus(g, tweakedRectangle);

			/*
			 * Border.
			 */

			NuGenControlPaint.DrawBorder(g, this.ClientRectangle, NuGenControlPaint.ColorFromArgb(this.ForegroundTransparency, this.BorderColor), this.BorderStyle);

			/*
			 * Grayscale.
			 */

			if (this.Enabled == false)
			{
				Image img = NuGenControlPaint.CreateBitmapFromGraphics(g, this.ClientRectangle);
				ControlPaint.DrawImageDisabled(g, img, 0, 0, this.BackColor);
			}
		}

		#endregion

		#region Methods.Private

		/*
		 * GetLineFromIndex
		 */

		/// <summary>
		/// Gets a <c>Genetibase.UI.PushGraphLine</c> object from the collection at the specified index.
		/// </summary>
		/// <param name="index">The Id of the graph line in the collection.</param>
		/// <returns>A <c>Genetibase.UI.PushGraphLine</c> object located in the collection at the specified index.</returns>
		private NuGenGraphLine GetLineFromIndex(int index)
		{
			for (int i = 0; i < this.lines.Count; i++)
			{
				if (this.lines[i].Index == index)
				{
					return this.lines[i];
				}
			}

			return null;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenPushGraphBar"/> class.
		/// </summary>
		public NuGenPushGraphBar()
			: base()
		{
			InitializeComponent();
			//
			// NuGenPushGraphBar
			//
			this.BackgroundColor = Color.Black;
			this.BackgroundStyle = NuGenBackgroundStyle.Constant;
		}

		#endregion

		#region Dispose

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// NuGenPushGraphBar
			// 
			this.Name = "NuGenPushGraphBar";
			this.Size = new System.Drawing.Size(250, 100);

		}
		#endregion
	}
}
