/* -----------------------------------------------
 * NuGenProgressBar.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.ProgressBarInternals;
using Genetibase.Shared.Controls.Properties;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// <seealso cref="ProgressBar"/>
	/// </summary>
	[ToolboxItem(false)]
	[DefaultEvent("Click")]
	[DefaultProperty("Value")]
	[Designer("Genetibase.Shared.Controls.Design.NuGenProgressBarDesigner")]
	[NuGenSRDescription("Description_ProgressBar")]
	[System.ComponentModel.DesignerCategory("Code")]
	public partial class NuGenProgressBar : NuGenOrientationControlBase
	{
		#region Events

		/*
		 * MarqueeTimerTick
		 */

		private static readonly object _marqueeTimerTick = new object();

		/// <summary>
		/// Occurs while marquee blocks are being rendered by the progress bar.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		[NuGenSRDescription("Description_ProgressBar_MarqueeTimerTick")]
		public event EventHandler MarqueeTimerTick
		{
			add
			{
				this.Events.AddHandler(_marqueeTimerTick, value);
			}
			remove
			{
				this.Events.RemoveHandler(_marqueeTimerTick, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="MarqueeTimerTick"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnMarqueeTimerTick(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokeEventHandler(_marqueeTimerTick, e);

			Rectangle marqueeBounds = this.LayoutManager.GetMarqueeBlockBounds(
				this.ClientRectangle,
				_marqueeOffset,
				this.Orientation
			);

			if (this.Orientation == NuGenOrientationStyle.Horizontal)
			{
				_marqueeOffset += 10;

				if (_marqueeOffset > this.ClientRectangle.Right)
				{
					this.ResetOffset();
				}
			}
			else
			{
				_marqueeOffset -= 10;

				if (_marqueeOffset < this.ClientRectangle.Top - marqueeBounds.Height)
				{
					this.ResetOffset();
				}
			}

			this.Invalidate();
		}

		#endregion

		#region Properties.Behavior

		/*
		 * MarqueeAnimationSpeed
		 */

		private int _marqueeAnimationSpeed = 100;

		/// <summary>
		/// Gets or sets the speed of the marquee animation in milliseconds.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(100)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_ProgressBar_MarqueeAnimationSpeed")]
		public int MarqueeAnimationSpeed
		{
			get
			{
				return _marqueeAnimationSpeed;
			}
			set
			{
				if (_marqueeAnimationSpeed != value)
				{
					_marqueeAnimationSpeed = value;
					this.OnMarqueeAnimationSpeedChanged(EventArgs.Empty);

					Debug.Assert(this.MarqueeTimer != null, "this.MarqueeTimer != null");
					this.MarqueeTimer.Interval = value;
				}
			}
		}

		private static readonly object _marqueeAnimationSpeedChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="MarqueeAnimationSpeed"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ProgressBar_MarqueeAnimationSpeedChanged")]
		public event EventHandler MarqueeAnimationSpeedChanged
		{
			add
			{
				this.Events.AddHandler(_marqueeAnimationSpeedChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_marqueeAnimationSpeedChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="MarqueeAnimationSpeedChanged"/> event.
		/// </summary>
		protected virtual void OnMarqueeAnimationSpeedChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_marqueeAnimationSpeedChanged, e);
		}

		/*
		 * Maximum
		 */

		private NuGenNonNegativeInt32 _maximum;

		/// <summary>
		/// </summary>
		protected NuGenNonNegativeInt32 MaximumInternal
		{
			get
			{
				if (_maximum == null)
				{
					_maximum = new NuGenNonNegativeInt32();
					_maximum.Value = 100;
				}

				return _maximum;
			}
		}

		/// <summary>
		/// Gets or sets the maximum value this progress bar accepts.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_ProgressBar_Maximum")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public int Maximum
		{
			get
			{
				return this.MaximumInternal.Value;
			}
			set
			{
				Debug.Assert(this.MaximumInternal != null, "this.MaximumInternal != null");
				Debug.Assert(this.ValueInternal != null, "this.ValueInternal != null");

				if (this.MaximumInternal.Value != value)
				{
					this.MaximumInternal.Value = value;
					this.OnMaximumChanged(EventArgs.Empty);
					this.ValueInternal.Maximum = this.MaximumInternal.Value;
					this.Invalidate();
				}
			}
		}

		private void ResetMaximum()
		{
			Debug.Assert(this.MaximumInternal != null, "this.MaximumInternal != null");
			this.MaximumInternal.Value = 100;
		}

		private bool ShouldSerializeMaximum()
		{
			Debug.Assert(this.MaximumInternal != null, "this.MaximumInternal != null");
			return this.MaximumInternal.Value != 100;
		}

		private static readonly object _maximumChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Maximum"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ProgressBar_MaximumChanged")]
		public event EventHandler MaximumChanged
		{
			add
			{
				this.Events.AddHandler(_maximumChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_maximumChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="MaximumChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnMaximumChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_maximumChanged, e);
		}

		/*
		 * Minimum
		 */

		private NuGenNonNegativeInt32 _minimum;

		/// <summary>
		/// </summary>
		protected NuGenNonNegativeInt32 MinimumInternal
		{
			get
			{
				if (_minimum == null)
				{
					_minimum = new NuGenNonNegativeInt32();
					_minimum.Value = 0;
				}

				return _minimum;
			}
		}

		/// <summary>
		/// Gets or sets the minimum value this progress bar accepts.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_ProgressBar_Minimum")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public int Minimum
		{
			get
			{
				Debug.Assert(this.MinimumInternal != null, "this.MinimumInternal != null");
				return this.MinimumInternal.Value;
			}
			set
			{
				Debug.Assert(this.MinimumInternal != null, "this.MinimumInternal != null");
				Debug.Assert(this.ValueInternal != null, "this.ValueInternal != null");

				if (this.MinimumInternal.Value != value)
				{
					this.MinimumInternal.Value = value;
					this.OnMinimumChanged(EventArgs.Empty);
					this.ValueInternal.Minimum = this.MinimumInternal.Value;
					this.Invalidate();
				}
			}
		}

		private void ResetMinimum()
		{
			Debug.Assert(this.MinimumInternal != null, "this.MinimumInternal != null");
			this.MinimumInternal.Value = 0;
		}

		private bool ShouldSerializeMinimum()
		{
			Debug.Assert(this.MinimumInternal != null, "this.MinimumInternal != null");
			return this.MinimumInternal.Value != 0;
		}

		private static readonly object _minimumChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Minimum"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ProgressBar_MinimumChanged")]
		public event EventHandler MinimumChanged
		{
			add
			{
				this.Events.AddHandler(_minimumChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_minimumChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="MinimumChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnMinimumChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_minimumChanged, e);
		}

		/*
		 * Step
		 */

		private int _step = 10;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(10)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_ProgressBar_Step")]
		public int Step
		{
			get
			{
				return _step;
			}
			set
			{
				if (_step != value)
				{
					_step = value;
					this.OnStepChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _stepChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Step"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ProgressBar_StepChanged")]
		public event EventHandler StepChanged
		{
			add
			{
				this.Events.AddHandler(_stepChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_stepChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="StepChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnStepChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_stepChanged, e);
		}

		/*
		 * Style
		 */

		private NuGenProgressBarStyle _style = NuGenProgressBarStyle.Continuous;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(NuGenProgressBarStyle.Continuous)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_ProgressBar_Style")]
		public NuGenProgressBarStyle Style
		{
			get
			{
				return _style;
			}
			set
			{
				if (_style != value)
				{
					_style = value;
					this.OnStyleChanged(EventArgs.Empty);

					Debug.Assert(this.MarqueeTimer != null, "this.MarqueeTimer != null");

					if (_style == NuGenProgressBarStyle.Marquee)
					{
						this.ResetOffset();
						this.MarqueeTimer.Start();
					}
					else
					{
						this.MarqueeTimer.Stop();
					}

					this.Invalidate();
				}
			}
		}

		private static readonly object _styleChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Style"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ProgressBar_StyleChanged")]
		public new event EventHandler StyleChanged
		{
			add
			{
				this.Events.AddHandler(_styleChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_styleChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="StyleChanged"/> event.
		/// </summary>
		protected new virtual void OnStyleChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_styleChanged, e);
		}

		/*
		 * Value
		 */

		private NuGenInt32 _valueInternal;

		/// <summary>
		/// </summary>
		protected NuGenInt32 ValueInternal
		{
			get
			{
				if (_valueInternal == null)
				{
					_valueInternal = new NuGenInt32(this.Minimum, this.Maximum);
					_valueInternal.Value = this.Minimum;

					_valueInternal.MaximumChanged += delegate
					{
						if (_valueInternal.Maximum != this.Maximum)
						{
							this.MaximumInternal.Value = _valueInternal.Maximum;
							this.OnMaximumChanged(EventArgs.Empty);
						}
					};

					_valueInternal.MinimumChanged += delegate
					{
						if (_valueInternal.Minimum != this.Minimum)
						{
							this.MinimumInternal.Value = _valueInternal.Minimum;
							this.OnMinimumChanged(EventArgs.Empty);
						}
					};
				}

				return _valueInternal;
			}
		}

		/// <summary>
		/// Gets or sets the current value for the progress bar.
		/// </summary>
		/// <exception cref="ArgumentException">
		/// <para>
		///		The value specified is greater than the value of the <see cref="Maximum"/> property.
		/// </para>
		/// -or-
		/// <para>
		///		The value specified is less than the value of the <see cref="Minimum"/> property.
		/// </para>
		/// </exception>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_ProgressBar_Value")]
		public int Value
		{
			get
			{
				Debug.Assert(this.ValueInternal != null, "this.ValueInternal != null");
				return this.ValueInternal.Value;
			}
			set
			{
				Debug.Assert(this.ValueInternal != null, "this.ValueInternal != null");

				if (this.ValueInternal.Value != value)
				{
					this.ValueInternal.Value = value;
					this.OnValueChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private void ResetValue()
		{
			Debug.Assert(this.ValueInternal != null, "this.ValueInternal != null");
			this.ValueInternal.Value = this.Minimum;
		}

		private bool ShouldSerializeValue()
		{
			Debug.Assert(this.ValueInternal != null, "this.ValueInternal != null");
			return this.ValueInternal.Value != this.Minimum;
		}

		private static readonly object _valueChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Value"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ProgressBar_ValueChanged")]
		public event EventHandler ValueChanged
		{
			add
			{
				this.Events.AddHandler(_valueChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_valueChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="ValueChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnValueChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_valueChanged, e);
		}

		#endregion

		#region Properties.Protected

		/*
		 * MarqueeTimer
		 */

		private Timer _marqueeTimer;

		/// <summary>
		/// </summary>
		protected Timer MarqueeTimer
		{
			get
			{
				if (_marqueeTimer == null)
				{
					_marqueeTimer = new Timer();
					_marqueeTimer.Interval = this.MarqueeAnimationSpeed;

					_marqueeTimer.Tick += delegate
					{
						this.OnMarqueeTimerTick(EventArgs.Empty);
					};
				}

				return _marqueeTimer;
			}
		}

		#endregion

		#region Properties.Protected.Overridden

		/*
		 * DefaultSize
		 */

		private static readonly Size _defaultSize = new Size(180, 25);

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

		#region Properties.Services

		/*
		 * LayoutManager
		 */

		private INuGenProgressBarLayoutManager _layoutManager;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenProgressBarLayoutManager LayoutManager
		{
			get
			{
				if (_layoutManager == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_layoutManager = this.ServiceProvider.GetService<INuGenProgressBarLayoutManager>();

					if (_layoutManager == null)
					{
						throw new NuGenServiceNotFoundException<INuGenProgressBarLayoutManager>();
					}
				}

				return _layoutManager;
			}
		}

		/*
		 * Renderer
		 */

		private INuGenProgressBarRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenProgressBarRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_renderer = this.ServiceProvider.GetService<INuGenProgressBarRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenProgressBarRenderer>();
					}
				}

				return _renderer;
			}
		}

		#endregion

		#region Methods.Public

		/*
		 * Increment
		 */

		/// <summary>
		/// Increases the progress bar <see cref="Value"/> by the specified <paramref name="step"/>.
		/// </summary>
		/// <param name="step"></param>
		/// <exception cref="InvalidOperationException">
		/// <para>
		///		<see cref="Style"/> is set to <see cref="NuGenProgressBarStyle.Marquee"/>.
		/// </para>
		/// </exception>
		public void Increment(int step)
		{
			if (this.Style == NuGenProgressBarStyle.Marquee)
			{
				throw new InvalidOperationException(Resources.InvalidOperation_ProgressBarStyle);
			}

			if (this.Value + step < this.Minimum)
			{
				this.Value = this.Minimum;
				return;
			}

			if (this.Value + step > this.Maximum)
			{
				this.Value = this.Maximum;
				return;
			}

			this.Value += step;
		}

		/*
		 * PerformStep
		 */

		/// <summary>
		/// Increases the progress bar <see cref="Value"/> by the <see cref="Step"/>.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// <para>
		///		<see cref="Style"/> is set to <see cref="NuGenProgressBarStyle.Marquee"/>.
		/// </para>
		/// </exception>
		public void PerformStep()
		{
			this.Increment(this.Step);
		}

		#endregion

		#region Methods.Protected.Overridden

		/*
		 * OnPaintBackground
		 */

		/// <summary>
		/// Raises the paint background event.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			base.OnPaintBackground(e);

			Debug.Assert(this.Renderer != null, "this.Renderer != null");
			Debug.Assert(this.StateTracker != null, "this.StateTracker != null");

			NuGenPaintParams paintParams = new NuGenPaintParams(e.Graphics);
			paintParams.Bounds = this.ClientRectangle;
			paintParams.State = this.StateTracker.GetControlState();
			
			this.Renderer.DrawBackground(paintParams);
		}

		/*
		 * OnPaint
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Debug.Assert(this.Renderer != null, "this.Renderer != null");
			Debug.Assert(this.StateTracker != null, "this.StateTracker != null");

			NuGenControlState state = this.StateTracker.GetControlState();
			Rectangle bounds = this.ClientRectangle;
			Rectangle foreBounds = Rectangle.Empty;

			switch (this.Style)
			{
				case NuGenProgressBarStyle.Marquee:
				{
					if (this.Enabled && !this.DesignMode)
					{
						foreBounds = this.LayoutManager.GetMarqueeBlockBounds(
							bounds,
							_marqueeOffset,
							this.Orientation
						);
					}
					break;
				}
				default:
				{
					foreBounds = this.LayoutManager.GetContinuousBounds(
						bounds,
						this.Minimum,
						this.Maximum,
						this.Value,
						this.Orientation
					);
					break;
				}
			}

			NuGenPaintParams paintParams = new NuGenPaintParams(e.Graphics);
			paintParams.State = state;

			if (this.Style == NuGenProgressBarStyle.Blocks)
			{
				Rectangle[] blocks = this.LayoutManager.GetBlocks(foreBounds, this.Orientation);

				for (int i = 0; i < blocks.Length; i++)
				{
					paintParams.Bounds = blocks[i];
					this.Renderer.DrawForeground(paintParams);
				}
			}
			else
			{
				paintParams.Bounds = foreBounds;
				this.Renderer.DrawForeground(paintParams);
			}

			paintParams.Bounds = bounds;
			this.Renderer.DrawBorder(paintParams);
		}

		#endregion

		#region Methods.Private

		/*
		 * ResetOffset
		 */

		private void ResetOffset()
		{
			Rectangle marqueeBounds = this.LayoutManager.GetMarqueeBlockBounds(
				this.ClientRectangle,
				_marqueeOffset,
				this.Orientation
			);

			if (this.Orientation == NuGenOrientationStyle.Horizontal)
			{
				_marqueeOffset = this.ClientRectangle.Left - marqueeBounds.Width;
			}
			else
			{
				_marqueeOffset = this.ClientRectangle.Bottom + marqueeBounds.Height;
			}
		}

		#endregion

		private int _marqueeOffset;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenProgressBar"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// <para>Requires:</para>
		/// <para><see cref="INuGenProgressBarRenderer"/></para>
		/// <para><see cref="INuGenProgressBarLayoutManager"/></para>
		/// <para><see cref="INuGenControlStateService"/></para>
		/// </param>
		public NuGenProgressBar(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.Selectable, false);
		}
	}
}
