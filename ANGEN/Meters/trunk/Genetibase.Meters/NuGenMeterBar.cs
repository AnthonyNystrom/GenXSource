/* -----------------------------------------------
 * NuGenMeterBar.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.Drawing;
using Genetibase.Meters.ComponentModel;
using Genetibase.WinApi;

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Meters
{
	/// <summary>
	/// Flexible meter bar.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[ToolboxItem(false)]
	public class NuGenMeterBar : NuGenBarBase
	{
		#region Properties.Appearance

		/*
		 * Orientation
		 */

		/// <summary>
		/// Determines the orientation of the control.
		/// </summary>
		private NuGenOrientationStyle _orientation = NuGenOrientationStyle.Vertical;

		/// <summary>
		/// Gets or sets the orientation of the control.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Appearance")]
		[NuGenSRDescription("Description_Orientation")]
		[DefaultValue(NuGenOrientationStyle.Vertical)]
		public virtual NuGenOrientationStyle Orientation
		{
			get
			{
				return _orientation;
			}
			set
			{
				if (_orientation != value)
				{
					Int32 w = this.Width;
					this.Width = this.Height;
					this.Height = w;

					_orientation = value;
					this.OnOrientationChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly Object _orientationChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenMeterBar.Orientation"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_OrientationChanged")]
		public event EventHandler OrientationChanged
		{
			add
			{
				this.Events.AddHandler(_orientationChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_orientationChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenMeterBar.OrientationChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnOrientationChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_orientationChanged, e);
		}

		/*
		 * TickLine
		 */

		/// <summary>
		/// Indicates whether to show the tick line.
		/// </summary>
		private Boolean _tickLine = true;

		/// <summary>
		/// Gets or sets the value indicating whether to show the tick line.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_TickLine")]
		[DefaultValue(true)]
		public virtual Boolean TickLine
		{
			get
			{
				return _tickLine;
			}
			set
			{
				if (_tickLine != value)
				{
					_tickLine = value;
					this.OnTickLineChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly Object _tickLineChanged = new Object();

		/// <summary>
		/// Occurs when the value of TickLine property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_TickLineChanged")]
		public event EventHandler TickLineChanged
		{
			add
			{
				this.Events.AddHandler(_tickLineChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_tickLineChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenMeterBar.TickLineChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnTickLineChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_tickLineChanged, e);
		}

		#endregion

		#region Properties.Behavior

		/*
		 * ReleaseTickLine
		 */

		/// <summary>
		/// Gets or sets the value indicating whether to release the tick line.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_ReleaseTickLine")]
		[DefaultValue(true)]
		public virtual Boolean ReleaseTickLine
		{
			get
			{
				return _releaseTickLineTimer.Enabled;
			}
			set
			{
				if (_releaseTickLineTimer.Enabled != value)
				{
					_releaseTickLineTimer.Enabled = value;
					this.OnReleaseTickLineChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly Object _releaseTickLineChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenMeterBar.ReleaseTickLine"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ReleaseTickLineChanged")]
		public event EventHandler ReleaseTickLineChanged
		{
			add
			{
				this.Events.AddHandler(_releaseTickLineChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_releaseTickLineChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenMeterBar.ReleaseTickLineChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnReleaseTickLineChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_releaseTickLineChanged, e);
		}

		/*
		 * ReleseTickLineTimeout
		 */

		/// <summary>
		/// Determines the time in milliseconds between the tick line releases.
		/// </summary>
		private Double _releaseTickLineTimeout = 5000;

		/// <summary>
		/// Gets or sets the time in milliseconds between the tick line releases.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_ReleaseTickLineTimeout")]
		[DefaultValue(5000D)]
		public virtual Double ReleaseTickLineTimeout
		{
			get
			{
				return _releaseTickLineTimeout;
			}
			set
			{
				_releaseTickLineTimeout = value;
				_releaseTickLineTimer.Interval = value;
				this.OnReleaseTickLineTimeoutChanged(EventArgs.Empty);
			}
		}

		private static readonly Object _releaseTickLineTimeoutChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenMeterBar.ReleaseTickLineTimeout"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ReleaseTickLineTimeoutChanged")]
		public event EventHandler ReleaseTickLineTimeoutChanged
		{
			add
			{
				this.Events.AddHandler(_releaseTickLineTimeoutChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_releaseTickLineTimeoutChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenMeterBar.ReleaseTickLineTimeout"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnReleaseTickLineTimeoutChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_releaseTickLineTimeoutChanged, e);
		}

		/*
		 * Value
		 */

		/// <summary>
		/// The current value of the meter.
		/// </summary>
		private float _value;

		/// <summary>
		/// Gets or sets the current value of the meter.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_Value")]
		[DefaultValue(0.0f)]
		public virtual float Value
		{
			get
			{
				return _value;
			}
			set
			{
				if (this.Enabled)
				{
					if (_value != value)
					{
						if (this.Maximum < value)
							this.Maximum = value;

						_value = value;
						this.OnValueChanged(EventArgs.Empty);

						/* 
						 * TickLine functionality.
						 */

						if (_value > this.PreviousMaximum)
						{
							this.PreviousMaximum = _value;
						}
						else if (this.PreviousMaximum == this.Maximum)
						{
							this.PreviousMaximum = _value;
						}

						this.Refresh();
					}
				}
			}
		}

		private static readonly Object _valueChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenMeterBar.Value"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ValueChanged")]
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
		/// Raises the<see cref="E:Genetibase.Meters.NuGenMeterBar.ValueChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnValueChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_valueChanged, e);
		}

		#endregion

		#region Properties.Protected

		/// <summary>
		/// The recently maximum value for the meter.
		/// </summary>
		private float _previousMaximum;

		/// <summary>
		/// Gets or sets the recently maximum value for the meter.
		/// </summary>
		protected virtual float PreviousMaximum
		{
			get
			{
				return _previousMaximum;
			}
			set
			{
				if (_previousMaximum != value)
				{
					_previousMaximum = value;
					this.Refresh();
				}
			}
		}

		#endregion

		#region Methods.Protected.Overriden

		/// <summary>
		/// Raises the <c>System.Windows.Forms.Control.EnabledChanged</c> event.
		/// </summary>
		/// <param name="e">A <c>System.EventArgs</c> that contain the event data.</param>
		protected override void OnEnabledChanged(EventArgs e)
		{
			_releaseTickLineTimer.Enabled = !_releaseTickLineTimer.Enabled;
			base.OnEnabledChanged(e);
		}

		/// <summary>
		/// Specifies the pen width to draw the border.
		/// </summary>
		private const Int32 PEN_WIDTH = 1;

		/// <summary>
		/// Draws the control.
		/// </summary>
		/// <param name="e">Provides data for the <c>System.Windows.Forms.Control.Paint</c> event.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			// High quality drawing.
			g.SmoothingMode = SmoothingMode.AntiAlias;

			// Define a tweaked rectangle for that the border was visible.
			Rectangle tweakedRectangle = new Rectangle(
				this.ClientRectangle.X,
				this.ClientRectangle.Y,
				this.ClientRectangle.Width - PEN_WIDTH,
				this.ClientRectangle.Height - PEN_WIDTH
				);

			if (this.Orientation == NuGenOrientationStyle.Vertical)
			{
				g.TranslateTransform(0, tweakedRectangle.Height);
				g.RotateTransform(-90);

				Int32 w = 0;

				w = tweakedRectangle.Width;
				tweakedRectangle.Width = tweakedRectangle.Height;
				tweakedRectangle.Height = w;
			}

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
							   Color.FromArgb(NuGenControlPaint.GetAlphaChannel(this.BackgroundTransparency), this.BackGradientStartColor),
							   Color.FromArgb(NuGenControlPaint.GetAlphaChannel(this.BackgroundTransparency), this.BackGradientEndColor),
							   90
							   ))
					{
						g.FillRectangle(lgb, tweakedRectangle);
					}

					break;

					case NuGenBackgroundStyle.VerticalGradient:
					using (LinearGradientBrush lgb = new LinearGradientBrush(
							   tweakedRectangle,
							   Color.FromArgb(NuGenControlPaint.GetAlphaChannel(this.BackgroundTransparency), this.BackGradientStartColor),
							   Color.FromArgb(NuGenControlPaint.GetAlphaChannel(this.BackgroundTransparency), this.BackGradientEndColor),
							   360
							   ))
					{
						g.FillRectangle(lgb, tweakedRectangle);
					}
					break;

					case NuGenBackgroundStyle.Tube:
					using (LinearGradientBrush lgb = new LinearGradientBrush(
							   tweakedRectangle,
							   Color.FromArgb(NuGenControlPaint.GetAlphaChannel(this.BackgroundTransparency), this.BackTubeGradientStartColor),
							   Color.FromArgb(NuGenControlPaint.GetAlphaChannel(this.BackgroundTransparency), this.BackTubeGradientEndColor),
							   90
							   ))
					{
						ColorBlend colorBlend = new ColorBlend(3);

						colorBlend.Colors = new Color[] {
			                                                    Color.FromArgb(NuGenControlPaint.GetAlphaChannel(this.BackgroundTransparency), this.BackTubeGradientEndColor),
			                                                    Color.FromArgb(NuGenControlPaint.GetAlphaChannel(this.BackgroundTransparency), this.BackTubeGradientStartColor),
			                                                    Color.FromArgb(NuGenControlPaint.GetAlphaChannel(this.BackgroundTransparency), this.BackTubeGradientEndColor)
			                                                };
						colorBlend.Positions = new float[] { 0.0f, 0.5f, 1.0f };

						lgb.InterpolationColors = colorBlend;

						g.FillRectangle(lgb, tweakedRectangle);
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
			 * Foreground.
			 */

			switch (this.ForegroundStyle)
			{
				case NuGenBackgroundStyle.Constant:
				using (SolidBrush sb = new SolidBrush(this.ForeColor))
				{
					g.FillRectangle(sb, new RectangleF(
						tweakedRectangle.X,
						tweakedRectangle.Y,
						tweakedRectangle.Width / this.Maximum * this.Value,
						tweakedRectangle.Height)
						);
				}
				break;

				case NuGenBackgroundStyle.Gradient:
				using (LinearGradientBrush lgb = new LinearGradientBrush(
						   tweakedRectangle,
						   Color.FromArgb(NuGenControlPaint.GetAlphaChannel(this.ForegroundTransparency), this.GradientStartColor),
						   Color.FromArgb(NuGenControlPaint.GetAlphaChannel(this.ForegroundTransparency), this.GradientEndColor),
						   90))
				{
					g.FillRectangle(lgb, new RectangleF(
						tweakedRectangle.X,
						tweakedRectangle.Y,
						tweakedRectangle.Width / (float)this.Maximum * this.Value,
						tweakedRectangle.Height)
						);
				}
				break;

				case NuGenBackgroundStyle.VerticalGradient:
				using (LinearGradientBrush lgb = new LinearGradientBrush(
						   tweakedRectangle,
						   Color.FromArgb(NuGenControlPaint.GetAlphaChannel(this.ForegroundTransparency), this.GradientStartColor),
						   Color.FromArgb(NuGenControlPaint.GetAlphaChannel(this.ForegroundTransparency), this.GradientEndColor),
						   360
						   ))
				{
					g.FillRectangle(lgb, new RectangleF(
						tweakedRectangle.X,
						tweakedRectangle.Y,
						tweakedRectangle.Width / this.Maximum * this.Value,
						tweakedRectangle.Height)
						);
				}
				break;

				case NuGenBackgroundStyle.Tube:
				using (LinearGradientBrush lgb = new LinearGradientBrush(
						   tweakedRectangle,
						   Color.FromArgb(NuGenControlPaint.GetAlphaChannel(this.ForegroundTransparency), this.TubeGradientStartColor),
						   Color.FromArgb(NuGenControlPaint.GetAlphaChannel(this.ForegroundTransparency), this.TubeGradientEndColor),
						   90
						   ))
				{
					ColorBlend colorBlend = new ColorBlend(3);

					colorBlend.Colors = new Color[] {
			                                                Color.FromArgb(NuGenControlPaint.GetAlphaChannel(this.ForegroundTransparency), this.TubeGradientEndColor),
			                                                Color.FromArgb(NuGenControlPaint.GetAlphaChannel(this.ForegroundTransparency), this.TubeGradientStartColor),
			                                                Color.FromArgb(NuGenControlPaint.GetAlphaChannel(this.ForegroundTransparency), this.TubeGradientEndColor)
			                                            };
					colorBlend.Positions = new float[] { 0.0f, 0.5f, 1.0f };

					lgb.InterpolationColors = colorBlend;

					g.FillRectangle(lgb, new RectangleF(
						tweakedRectangle.X,
						tweakedRectangle.Y,
						tweakedRectangle.Width / this.Maximum * this.Value,
						tweakedRectangle.Height)
						);
				}
				break;
			}

			/*
			 * TickLine.
			 */

			if (this.TickLine && this.PreviousMaximum != 0.0f && this.PreviousMaximum != this.Maximum && this.PreviousMaximum != this.Value)
			{
				if (this.Orientation == NuGenOrientationStyle.Horizontal)
				{
					NuGenControlPaint.DrawReversibleLine(
						g,
						(Int32)((float)this.ClientRectangle.Width / this.Maximum * this.PreviousMaximum),
						this.ClientRectangle.Top + PEN_WIDTH,
						(Int32)((float)this.ClientRectangle.Width / this.Maximum * this.PreviousMaximum),
						this.ClientRectangle.Bottom - PEN_WIDTH * 2
						);
				}
				else
				{
					NuGenControlPaint.DrawReversibleLine(
						g,
						this.ClientRectangle.Left + PEN_WIDTH * 2,
						(Int32)((float)this.ClientRectangle.Height - (float)this.ClientRectangle.Height / this.Maximum * this.PreviousMaximum),
						this.ClientRectangle.Right - PEN_WIDTH * 2,
						(Int32)((float)this.ClientRectangle.Height - (float)this.ClientRectangle.Height / this.Maximum * this.PreviousMaximum)
						);

				}
			}

			/*
			 * Border.
			 */

			switch (this.BorderStyle)
			{
				case NuGenBorderStyle.Dashed:
				case NuGenBorderStyle.Dotted:
				case NuGenBorderStyle.Solid:
				Rectangle borderRectangle = new Rectangle(
					tweakedRectangle.Left,
					tweakedRectangle.Top,
					tweakedRectangle.Right + PEN_WIDTH,
					tweakedRectangle.Bottom + PEN_WIDTH
					);

				NuGenControlPaint.DrawBorder(g, borderRectangle, NuGenControlPaint.ColorFromArgb(this.ForegroundTransparency, this.BorderColor), this.BorderStyle);
				break;
				default:
				g.ResetTransform();
				NuGenControlPaint.DrawBorder(g, this.ClientRectangle, NuGenControlPaint.ColorFromArgb(this.ForegroundTransparency, this.BorderColor), this.BorderStyle);
				break;
			}

			/*
			 * Grayscale.
			 */

			if (this.Enabled == false)
			{
				Image img = NuGenControlPaint.CreateBitmapFromGraphics(g, this.ClientRectangle);

				if (this.Orientation == NuGenOrientationStyle.Vertical)
				{
					g.ResetTransform();
				}

				ControlPaint.DrawImageDisabled(g, img, 0, 0, this.BackColor);
			}
		}

		#endregion

		#region EventHandlers

		private void releaseTickLine_Elapsed(Object sender, System.Timers.ElapsedEventArgs e)
		{
			this.PreviousMaximum = this.Value;
		}

		#endregion

		private Container _components;
		private System.Timers.Timer _releaseTickLineTimer;

		/// <summary>
		/// Initializes a new instance of the <c>Genetibase.UI.NuGenMeterBar</c> class.
		/// </summary>
		public NuGenMeterBar()
			: base()
		{
			InitializeComponent();
		}

		#region Dispose

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(Boolean disposing)
		{
			if (disposing)
			{
				if (_components != null)
				{
					_components.Dispose();
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
			_releaseTickLineTimer = new System.Timers.Timer();
			((System.ComponentModel.ISupportInitialize)(_releaseTickLineTimer)).BeginInit();
			// 
			// releaseTickLine
			// 
			_releaseTickLineTimer.Enabled = true;
			_releaseTickLineTimer.SynchronizingObject = this;
			_releaseTickLineTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.releaseTickLine_Elapsed);
			// 
			// NuGenMeterBar
			// 
			this.Name = "NuGenMeterBar";
			this.Size = new System.Drawing.Size(40, 168);
			((System.ComponentModel.ISupportInitialize)(_releaseTickLineTimer)).EndInit();

		}
		#endregion
	}
}
