/* -----------------------------------------------
 * NuGenMeterBar.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using gdi = Genetibase.WinApi.Gdi32;
using win = Genetibase.WinApi.WinUser;

using Genetibase.Shared;
using Genetibase.Shared.Drawing;
using Genetibase.UI.NuGenMeters.ComponentModel;
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

namespace Genetibase.UI.NuGenMeters
{
	/// <summary>
	/// Flexible meter bar.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[ToolboxItem(false)]
	public class NuGenMeterBar : NuGenBarBase
	{
		#region Declarations

		private Container components = null;
		private System.Timers.Timer releaseTickLine;

		#endregion

		#region Properties.Appearance
		
		/*
		 * Orientation
		 */

		/// <summary>
		/// Determines the orientation of the control.
		/// </summary>
		private NuGenOrientationStyle orientation = NuGenOrientationStyle.Vertical;

		/// <summary>
		/// Gets or sets the orientation of the control.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Appearance")]
		[NuGenSRDescription("OrientationDescription")]
		[DefaultValue(NuGenOrientationStyle.Vertical)]
		public virtual NuGenOrientationStyle Orientation
		{
			get { return this.orientation; }
			set 
			{
				if (this.orientation != value)
				{
					int w = this.Width;
					this.Width = this.Height;
					this.Height = w;

					this.orientation = value;
					this.OnOrientationChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly object EventOrientationChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenMeterBar.Orientation"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("OrientationChangedDescription")]
		public event EventHandler OrientationChanged
		{
			add
			{
				this.Events.AddHandler(EventOrientationChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventOrientationChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenMeterBar.OrientationChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnOrientationChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventOrientationChanged, e);
		}

		/*
		 * TickLine
		 */

		/// <summary>
		/// Indicates whether to show the tick line.
		/// </summary>
		private bool tickLine = true;

		/// <summary>
		/// Gets or sets the value indicating whether to show the tick line.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("AppearanceCategory")]
		[NuGenSRDescription("TickLineDescription")]
		[DefaultValue(true)]
		public virtual bool TickLine
		{
			get { return this.tickLine; }
			set 
			{
				if (this.tickLine != value)
				{
					this.tickLine = value;
					this.OnTickLineChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly object EventTickLineChanged = new object();

		/// <summary>
		/// Occurs when the value of TickLine property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("TickLineChangedDescription")]
		public event EventHandler TickLineChanged
		{
			add
			{
				this.Events.AddHandler(EventTickLineChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventTickLineChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenMeterBar.TickLineChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnTickLineChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventTickLineChanged, e);
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
		[NuGenSRCategory("BehaviorCategory")]
		[NuGenSRDescription("ReleaseTickLineDescription")]
		[DefaultValue(true)]
		public virtual bool ReleaseTickLine
		{
			get { return this.releaseTickLine.Enabled; }
			set 
			{ 
				if (this.releaseTickLine.Enabled != value) 
				{
					this.releaseTickLine.Enabled = value;
					this.OnReleaseTickLineChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object EventReleaseTickLineChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenMeterBar.ReleaseTickLine"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("ReleaseTickLineChangedDescription")]
		public event EventHandler ReleaseTickLineChanged
		{
			add
			{
				this.Events.AddHandler(EventReleaseTickLineChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventReleaseTickLineChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenMeterBar.ReleaseTickLineChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnReleaseTickLineChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventReleaseTickLineChanged, e);
		}

		/*
		 * ReleseTickLineTimeout
		 */

		/// <summary>
		/// Determines the time in milliseconds between the tick line releases.
		/// </summary>
		private double releaseTickLineTimeout = 5000;

		/// <summary>
		/// Gets or sets the time in milliseconds between the tick line releases.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("BehaviorCategory")]
		[NuGenSRDescription("ReleaseTickLineTimeoutDescription")]
		[DefaultValue(5000D)]
		public virtual double ReleaseTickLineTimeout
		{
			get { return this.releaseTickLineTimeout; }
			set 
			{ 
				this.releaseTickLineTimeout = value;
				this.releaseTickLine.Interval = value;
				this.OnReleaseTickLineTimeoutChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventReleaseTickLineTimeoutChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenMeterBar.ReleaseTickLineTimeout"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("ReleaseTickLineTimeoutChangedDescription")]
		public event EventHandler ReleaseTickLineTimeoutChanged
		{
			add
			{
				this.Events.AddHandler(EventReleaseTickLineTimeoutChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventReleaseTickLineTimeoutChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenMeterBar.ReleaseTickLineTimeout"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnReleaseTickLineTimeoutChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventReleaseTickLineTimeoutChanged, e);
		}

		/*
		 * Value
		 */

		/// <summary>
		/// The current value of the meter.
		/// </summary>
		private float value = 0;

		/// <summary>
		/// Gets or sets the current value of the meter.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("BehaviorCategory")]
		[NuGenSRDescription("ValueDescription")]
		[DefaultValue(0.0f)]
		public virtual float Value
		{
			get { return this.value; }
			set 
			{
				if (this.Enabled)
				{
					if (this.value != value)
					{
						if (this.Maximum < value)
							this.Maximum = value;

						this.value = value;
						this.OnValueChanged(EventArgs.Empty);

						/* 
						 * TickLine functionality.
						 */

						if (this.value > this.PreviousMaximum) 
						{
							this.PreviousMaximum = this.value;
						}
						else if (this.PreviousMaximum == this.Maximum)
						{
							this.PreviousMaximum = this.value;
						}

						this.Refresh();
					}
				}
			}
		}

		private static readonly object EventValueChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenMeterBar.Value"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("ValueChangedDescription")]
		public event EventHandler ValueChanged
		{
			add
			{
				this.Events.AddHandler(EventValueChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventValueChanged, value);
			}
		}

		/// <summary>
		/// Raises the<see cref="E:Genetibase.UI.NuGenMeters.NuGenMeterBar.ValueChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnValueChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventValueChanged, e);
		}

		#endregion

		#region Properties.Protected
		
		/// <summary>
		/// The recently maximum value for the meter.
		/// </summary>
		private float internalPreviousMaximum = 0.0f;

		/// <summary>
		/// Gets or sets the recently maximum value for the meter.
		/// </summary>
		protected virtual float PreviousMaximum
		{
			get { return this.internalPreviousMaximum; }
			set 
			{
				if (this.internalPreviousMaximum != value)
				{
					this.internalPreviousMaximum = value;
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
			this.releaseTickLine.Enabled = !this.releaseTickLine.Enabled;
			base.OnEnabledChanged(e);
		}

		/// <summary>
		/// Specifies the pen width to draw the border.
		/// </summary>
		private const int PEN_WIDTH = 1;
		
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

			if (this.Orientation == NuGenOrientationStyle.Vertical) {
				g.TranslateTransform(0, tweakedRectangle.Height);
				g.RotateTransform(-90);

				int w = 0;

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
			            colorBlend.Positions = new float[] {0.0f, 0.5f, 1.0f};

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
						(int)((float)this.ClientRectangle.Width / this.Maximum * this.PreviousMaximum),
						this.ClientRectangle.Top + PEN_WIDTH,
						(int)((float)this.ClientRectangle.Width / this.Maximum * this.PreviousMaximum),
						this.ClientRectangle.Bottom - PEN_WIDTH * 2
						);
				}
				else
				{
					NuGenControlPaint.DrawReversibleLine(
						g,
						this.ClientRectangle.Left + PEN_WIDTH * 2,
						(int)((float)this.ClientRectangle.Height - (float)this.ClientRectangle.Height / this.Maximum * this.PreviousMaximum),
						this.ClientRectangle.Right - PEN_WIDTH * 2,
						(int)((float)this.ClientRectangle.Height - (float)this.ClientRectangle.Height / this.Maximum * this.PreviousMaximum)
						);

				}
			}

			/*
			 * Border.
			 */

			switch (this.BorderStyle) {
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
		
		private void releaseTickLine_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			this.PreviousMaximum = this.Value;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <c>Genetibase.UI.NuGenMeterBar</c> class.
		/// </summary>
		public NuGenMeterBar() : base()
		{
			InitializeComponent();
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
			this.releaseTickLine = new System.Timers.Timer();
			((System.ComponentModel.ISupportInitialize)(this.releaseTickLine)).BeginInit();
			// 
			// releaseTickLine
			// 
			this.releaseTickLine.Enabled = true;
			this.releaseTickLine.SynchronizingObject = this;
			this.releaseTickLine.Elapsed += new System.Timers.ElapsedEventHandler(this.releaseTickLine_Elapsed);
			// 
			// NuGenMeterBar
			// 
			this.Name = "NuGenMeterBar";
			this.Size = new System.Drawing.Size(40, 168);
			((System.ComponentModel.ISupportInitialize)(this.releaseTickLine)).EndInit();

		}
		#endregion
	}
}
