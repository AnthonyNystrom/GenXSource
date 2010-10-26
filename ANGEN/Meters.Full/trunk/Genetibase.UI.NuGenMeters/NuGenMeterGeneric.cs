/* -----------------------------------------------
 * NuGenMeterGeneric.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using en  = Genetibase.PerformanceCounters.NuGenProcessorCounter;
using pc  = Genetibase.PerformanceCounters.Processor;
using win = Genetibase.WinApi.WinUser;

using Genetibase.Shared.Design;
using Genetibase.Shared;
using Genetibase.UI.NuGenMeters.ComponentModel;
using Genetibase.WinApi;

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.UI.NuGenMeters
{
	/// <summary>
	/// Defines the base functionality for the meter controls.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[ToolboxItem(true)]
	public class NuGenMeterGeneric : NuGenGenericBase
	{
		#region Declarations

		private System.ComponentModel.IContainer components;
		private NuGenMeterBar meter;
		private NuGenCTLabel captionText;
		private NuGenCTLabel progressText;

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
					this.orientation = value;
					this.OnOrientationChanged(EventArgs.Empty);
					this.SetLayout(value);
					this.Refresh();
				}
			}
		}

		private static readonly object EventOrientationChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.Orientation"/>
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
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.OrientationChanged"/> event.
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
		/// Gets or sets the value indicating whether to show the tick line.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("AppearanceCategory")]
		[NuGenSRDescription("TickLineDescription")]
		[DefaultValue(true)]
		public virtual bool TickLine
		{
			get { return this.meter.TickLine; }
			set 
			{ 
				this.meter.TickLine = value;
				this.OnTickLineChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventTickLineChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.TickLine"/>
		/// property changes.
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
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.TickLineChanged"/> event.
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
		/// Indicates whether to show the tick line.
		/// </summary>
		private bool releaseTickLine = true;

		/// <summary>
		/// Gets or sets the value indicating whether to release the tick line.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("BehaviorCategory")]
		[NuGenSRDescription("ReleaseTickLineDescription")]
		[DefaultValue(true)]
		public virtual bool ReleaseTickLine
		{
			get { return this.releaseTickLine; }
			set 
			{ 
				this.releaseTickLine = value;
				this.meter.ReleaseTickLine = this.releaseTickLine;
				this.OnReleaseTickLineChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventReleaseTickLineChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.ReleaseTickLine"/>
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
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.ReleaseTickLineChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnReleaseTickLineChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventReleaseTickLineChanged, e);
		}

		/*
		 * ReleaseTickLineTimeout
		 */

		/// <summary>
		/// The time in milliseconds between the tick line releases.
		/// </summary>
		private double releaseTickLineTimeout = 5000d;

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
				this.meter.ReleaseTickLineTimeout = this.releaseTickLineTimeout;
				this.OnReleaseTickLineTimeoutChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventReleaseTickLineTimeoutChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.ReleaseTickLineTimeout"/>
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
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.ReleaseTickLineTimeout"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnReleaseTickLineTimeoutChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventReleaseTickLineTimeoutChanged, e);
		}

		#endregion

		#region Properties.CaptionText
		
		/*
		 * CaptionText
		 */

		/// <summary>
		/// Gets or sets the caption text for the meter.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("CaptionTextCategory")]
		[NuGenSRDescription("CaptionTextDescription")]
		public virtual string CaptionText
		{
			get { return this.captionText.Text; }
			set 
			{
				if (this.captionText.Text != value) 
				{
					this.captionText.Text = value;
					this.OnCaptionTextChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object EventCaptionTextChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.CaptionText"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("CaptionTextChangedDescription")]
		public event EventHandler CaptionTextChanged
		{
			add
			{
				this.Events.AddHandler(EventCaptionTextChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventCaptionTextChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterBase.CaptionTextChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCaptionTextChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventCaptionTextChanged, e);
		}

		/*
		 * CaptionTextAlignment
		 */

		/// <summary>
		/// Gets or sets the alignment of the caption text.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("CaptionTextCategory")]
		[DefaultValue(ContentAlignment.MiddleCenter)]
		[NuGenSRDescription("CaptionTextAlignmentDescription")]
		public ContentAlignment CaptionTextAlignment
		{
			get
			{
				return this.captionText.TextAlign;
			}
			set
			{
				this.captionText.TextAlign = value;
				this.OnCaptionTextAlignmentChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventCaptionTextAlignmentChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.CaptionTextAlignment"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("CaptionTextAlignmentChangedDescription")]
		public event EventHandler CaptionTextAlignmentChanged
		{
			add
			{
				this.Events.AddHandler(EventCaptionTextAlignmentChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventCaptionTextAlignmentChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterBase.CaptionTextAlignmentChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCaptionTextAlignmentChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventCaptionTextAlignmentChanged, e);
		}

		/*
		 * CaptionTextBackColor
		 */

		/// <summary>
		/// Gets or sets the background color for the label.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("CaptionTextCategory")]
		[NuGenSRDescription("CaptionTextBackColorDescription")]
		public virtual Color CaptionTextBackColor
		{
			get { return this.captionText.BackgroundColor; }
			set 
			{ 
				this.captionText.BackgroundColor = value; 
				this.OnCaptionTextBackColorChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventCaptionTextBackColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.CaptionTextBackColor"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("CaptionTextBackColorChangedDescription")]
		public event EventHandler CaptionTextBackColorChanged
		{
			add
			{
				this.Events.AddHandler(EventCaptionTextBackColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventCaptionTextBackColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.CaptionTextBackColorChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCaptionTextBackColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventCaptionTextBackColorChanged, e);
		}

		/*
		 * CaptionTextBackgroundTransparency
		 */

		/// <summary>
		/// Gets or sets the background transparency level for the label.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("CaptionTextCategory")]
		[DefaultValue(0)]
		[NuGenSRDescription("CaptionTextBackgroundTransparencyDescription")]
		[Editor(typeof(NuGenTransparencyEditor), typeof(UITypeEditor))]
		[TypeConverter(typeof(NuGenTransparencyConverter))]
		public virtual int CaptionTextBackgroundTransparency
		{
			get { return this.captionText.BackgroundTransparency; }
			set 
			{
				this.captionText.BackgroundTransparency = value;
				this.OnCaptionTextBackgroundTransparencyChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventCaptionTextBackgroundTransparencyChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.CaptionTextBackgroundTransparency"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("CaptionTextBackgroundTransparencyChangedDescription")]
		public event EventHandler CaptionTextBackgroundTransparencyChanged
		{
			add
			{
				this.Events.AddHandler(EventCaptionTextBackgroundTransparencyChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventCaptionTextBackgroundTransparencyChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.CaptionTextBackgroundTransparencyChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCaptionTextBackgroundTransparencyChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventCaptionTextBackgroundTransparencyChanged, e);
		}

		/*
		 * CaptionTextBorderColor
		 */

		/// <summary>
		/// Gets or sets the border color for the label.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("CaptionTextCategory")]
		[DefaultValue(typeof(Color), "Black")]
		[NuGenSRDescription("CaptionTextBorderColorDescription")]
		public virtual Color CaptionTextBorderColor
		{
			get { return this.captionText.BorderColor; }
			set 
			{
				this.captionText.BorderColor = value;
				this.OnCaptionTextBorderColorChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventCaptionTextBorderColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.CaptionTextBorderColorChanged"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("CaptionTextBorderColorChangedDescription")]
		public event EventHandler CaptionTextBorderColorChanged
		{
			add
			{
				this.Events.AddHandler(EventCaptionTextBorderColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventCaptionTextBorderColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.CaptionTextBorderColorChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCaptionTextBorderColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventCaptionTextBorderColorChanged, e);
		}

		/*
		 * CaptionTextBorderStyle
		 */

		/// <summary>
		/// Gets or sets the border style of the CaptionText.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("CaptionTextCategory")]
		[DefaultValue(NuGenBorderStyle.None)]
		[NuGenSRDescription("CaptionTextBorderStyleDescription")]
		public virtual NuGenBorderStyle CaptionTextBorderStyle
		{
			get { return this.captionText.BorderStyle; }
			set 
			{ 
				this.captionText.BorderStyle = value;
				this.OnCaptionTextBorderStyleChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventCaptionTextBorderStyleChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.CaptionTextBorderStyle"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("CaptionTextBorderStyleChangedDescription")]
		public event EventHandler CaptionTextBorderStyleChanged
		{
			add
			{
				this.Events.AddHandler(EventCaptionTextBorderStyleChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventCaptionTextBorderStyleChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterBase.CaptionTextBorderStyleChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCaptionTextBorderStyleChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventCaptionTextBorderStyleChanged, e);
		}

		/*
		 * CaptionTextEatLine
		 */

		/// <summary>
		/// Gets or sets the value indicating whether to eat the tail symbols which cannot be displayed
		/// due to the label width.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("CaptionTextCategory")]
		[DefaultValue(true)]
		[NuGenSRDescription("CaptionTextEatLineDescription")]
		public virtual bool CaptionTextEatLine
		{
			get { return this.captionText.EatLine; }
			set 
			{
				this.captionText.EatLine = value;
				this.OnCaptionTextEatLineChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventCaptionTextEatLineChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.CaptionTextEatLine"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("CaptionTextEatLineChangedDescription")]
		public event EventHandler CaptionTextEatLineChanged
		{
			add
			{
				this.Events.AddHandler(EventCaptionTextEatLineChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventCaptionTextEatLineChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterBase.CaptionTextEatLineChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCaptionTextEatLineChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventCaptionTextEatLineChanged, e);
		}

		/*
		 * CaptionTextFont
		 */

		/// <summary>
		/// Gets or sets the font for the caption text.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("CaptionTextCategory")]
		[NuGenSRDescription("CaptionTextFontDescription")]
		public virtual Font CaptionTextFont
		{
			get { return this.captionText.Font; }
			set 
			{ 
				this.captionText.Font = value;
				this.OnCaptionTextFontChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventCaptionTextFontChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.CaptionTextFont"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("CaptionTextFontChangedDescription")]
		public event EventHandler CaptionTextFontChanged
		{
			add
			{
				this.Events.AddHandler(EventCaptionTextFontChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventCaptionTextFontChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterBase.CaptionTextFontChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCaptionTextFontChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventCaptionTextFontChanged, e);
		}

		/*
		 * CaptionTextForeColor
		 */

		/// <summary>
		/// Gets or sets the foreground color of the control.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("CaptionTextCategory")]
		[NuGenSRDescription("CaptionTextForeColorDescription")]
		public virtual Color CaptionTextForeColor
		{
			get { return this.captionText.ForegroundColor; }
			set 
			{
				this.captionText.ForegroundColor = value;
				this.OnCaptionTextForeColorChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventCaptionForeColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.CaptionTextForeColor"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("CaptionTextForeColorChangedDescription")]
		public event EventHandler CaptionTextForeColorChanged
		{
			add
			{
				this.Events.AddHandler(EventCaptionForeColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventCaptionForeColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterBase.CaptionTextForeColorChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCaptionTextForeColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventCaptionForeColorChanged, e);
		}

		/*
		 * CaptionTextForegroundTransparency
		 */

		/// <summary>
		/// Gets or sets the foreground transparency level for the label.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("CaptionTextCategory")]
		[DefaultValue(0)]
		[NuGenSRDescription("CaptionTextForegroundTransparencyDescription")]
		[Editor(typeof(NuGenTransparencyEditor), typeof(UITypeEditor))]
		[TypeConverter(typeof(NuGenTransparencyConverter))]
		public virtual int CaptionTextForegroundTransparency
		{
			get
			{
				return this.captionText.ForegroundTransparency;
			}
			set
			{
				this.captionText.ForegroundTransparency = value;
				this.OnForegroundTransparencyChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventCaptionTextForegroundTransparencyChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.CaptionTextForegroundTransparency"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("CaptionTextForegroundTransparencyChangedDescription")]
		public event EventHandler CaptionTextForegroundTransparencyChanged
		{
			add
			{
				this.Events.AddHandler(EventCaptionTextForegroundTransparencyChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventCaptionTextForegroundTransparencyChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.CaptionTextForegroundTransparencyChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCaptionTextForegroundTransparencyChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventCaptionTextForegroundTransparencyChanged, e);
		}

		/*
		 * CaptionTextEdgeIndent
		 */

		/// <summary>
		/// Determines the indent of the caption text from the nearest edge of the meter.
		/// </summary>
		private int captionTextEdgeIndent = 32;

		/// <summary>
		/// Gets or sets the indent of the caption text from the nearest edge of the meter.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("CaptionTextCategory")]
		[DefaultValue(32)]
		[NuGenSRDescription("CaptionTextEdgeIndentDescription")]
		public int CaptionTextEdgeIndent
		{
			get { return this.captionTextEdgeIndent; }
			set 
			{
				this.captionTextEdgeIndent = value;
				this.OnCaptionTextEdgeIndentChanged(EventArgs.Empty);
				this.captionText.Height = value;
			}
		}

		private static readonly object EventCaptionTextEdgeIndentChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.CaptionTextEdgeIndent"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("CaptionTextEdgeIndentChangedDescription")]
		public event EventHandler CaptionTextEdgeIndentChanged
		{
			add
			{
				this.Events.AddHandler(EventCaptionTextEdgeIndentChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventCaptionTextEdgeIndentChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterBase.CaptionTextEdgeIndentChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCaptionTextEdgeIndentChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventCaptionTextEdgeIndentChanged, e);
		}
	
		/*
		 * CaptionTextOrientation
		 */

		/// <summary>
		/// Gets or sets the direction of the caption text.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("CaptionTextCategory")]
		[DefaultValue(NuGenOrientationStyle.Horizontal)]
		[NuGenSRDescription("CaptionTextOrientationDescription")]
		public virtual NuGenOrientationStyle CaptionTextOrientation
		{
			get { return this.captionText.TextOrientation; }
			set 
			{
				this.captionText.TextOrientation = value;
				this.OnCaptionTextOrientationChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventCaptionTextOrientationChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.CaptionTextOrientation"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("CaptionTextOrientationChangedDescription")]
		public event EventHandler CaptionTextOrientationChanged
		{
			add
			{
				this.Events.AddHandler(EventCaptionTextOrientationChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventCaptionTextOrientationChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterBase.CaptionTextOrientationChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCaptionTextOrientationChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventCaptionTextOrientationChanged, e);
		}

		/*
		 * CaptionTextPosition
		 */
		
		/// <summary>
		/// Determines the positioning of the caption text.
		/// </summary>
		private NuGenCaptionTextPositionStyle captionTextPosition = NuGenCaptionTextPositionStyle.Header;

		/// <summary>
		/// Gets or sets the positioning of the caption text.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("CaptionTextCategory")]
		[DefaultValue(NuGenCaptionTextPositionStyle.Header)]
		[NuGenSRDescription("CaptionTextPositionDescription")]
		public virtual NuGenCaptionTextPositionStyle CaptionTextPosition
		{
			get { return this.captionTextPosition; }
			set 
			{
				if (this.captionTextPosition != value)
				{
					this.captionTextPosition = value;
					this.OnCaptionTextPositionChanged(EventArgs.Empty);
					this.SetLayout(this.Orientation);
				}
			}
		}

		private static readonly object EventCaptionTextPositionChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.CaptionTextPosition"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("CaptionTextPositionChangedDescription")]
		public event EventHandler CaptionTextPositionChanged
		{
			add
			{
				this.Events.AddHandler(EventCaptionTextPositionChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventCaptionTextPositionChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterBase.CaptionTextPositionChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCaptionTextPositionChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventCaptionTextPositionChanged, e);
		}

		/*
		 * CaptionTextVisible
		 */

		/// <summary>
		/// Gets or sets the value indicating whether the caption text is visible.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("CaptionTextCategory")]
		[DefaultValue(true)]
		[NuGenSRDescription("CaptionTextVisibleDescription")]
		public virtual bool CaptionTextVisible
		{
			get { return this.captionText.Visible; }
			set 
			{ 
				this.captionText.Visible = value;
				this.OnCaptionTextVisibleChanged(EventArgs.Empty);
				this.SetLayout(this.Orientation);
			}
		}

		private static readonly object EventCaptionTextVisibleChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.CaptionTextVisible"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("CaptionTextVisibleChangedDescription")]
		public event EventHandler CaptionTextVisibleChanged
		{
			add
			{
				this.Events.AddHandler(EventCaptionTextVisibleChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventCaptionTextVisibleChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterBase.CaptionTextVisibleChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCaptionTextVisibleChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventCaptionTextVisibleChanged, e);
		}

		/*
		 * CaptionTetWordWrap
		 */

		/// <summary>
		/// Gets or sets the value indicating whether the lines are automatically word-wrapped.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("CaptionTextCategory")]
		[DefaultValue(false)]
		[NuGenSRDescription("CaptionTextWordWrapDescription")]
		public virtual bool CaptionTextWordWrap
		{
			get { return this.captionText.WordWrap; }
			set 
			{ 
				this.captionText.WordWrap = value;
				this.OnCaptionTextWordWrapChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventCaptionTextWordWrapChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.CaptionTextWordWrap"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("CaptionTextWordWrapChangedDescription")]
		public event EventHandler CaptionTextWordWrapChanged
		{
			add
			{
				this.Events.AddHandler(EventCaptionTextWordWrapChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventCaptionTextWordWrapChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterBase.CaptionTextWordWrapChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCaptionTextWordWrapChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventCaptionTextWordWrapChanged, e);
		}

		#endregion

		#region Properties.ProgressText

		/*
		 * ProgressTextAlignment
		 */

		/// <summary>
		/// Gets or sets the aligment of the progress text.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("ProgressTextCategory")]
		[DefaultValue(ContentAlignment.MiddleCenter)]
		[NuGenSRDescription("ProgressTextAlignmentDescription")]
		public virtual ContentAlignment ProgressTextAlignment
		{
			get
			{
				return this.progressText.TextAlign;
			}
			set
			{
				this.progressText.TextAlign = value;
				this.OnProgressTextAlignmentChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventProgressTextAlignmentChanged = new object();
		
		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.ProgressTextAlignment"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("ProgressTextAlignmentChangedDescription")]
		public event EventHandler ProgressTextAlignmentChanged
		{
			add
			{
				this.Events.AddHandler(EventProgressTextAlignmentChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventProgressTextAlignmentChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterBase.ProgressTextAlignmentChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnProgressTextAlignmentChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventProgressTextAlignmentChanged, e);
		}

		/*
		 * ProgressTextBackColor
		 */

		/// <summary>
		/// Gets or sets the background color for the label.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("ProgressTextCategory")]
		[DefaultValue(typeof(Color), "Black")]
		[NuGenSRDescription("ProgressTextBackColorDescription")]
		public virtual Color ProgressTextBackColor
		{
			get { return this.progressText.BackgroundColor; }
			set 
			{
				if (this.progressText.BackgroundColor != value) 
				{
					this.progressText.BackgroundColor = value; 
					this.OnProgressTextBackColorChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object EventProgressTextBackColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.ProgressTextBackColor"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("ProgressTextBackColorChangedDescription")]
		public event EventHandler ProgressTextBackColorChanged
		{
			add
			{
				this.Events.AddHandler(EventProgressTextBackColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventProgressTextBackColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.ProgressTextBackColorChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnProgressTextBackColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventProgressTextBackColorChanged, e);
		}

		/*
		 * ProgressTextBackgroundTransparency
		 */
		
		/// <summary>
		/// Gets or sets the background transparency level for the label.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("ProgressTextCategory")]
		[DefaultValue(0)]
		[NuGenSRDescription("ProgressTextBackgroundTransparencyDescription")]
		[Editor(typeof(NuGenTransparencyEditor), typeof(UITypeEditor))]
		[TypeConverter(typeof(NuGenTransparencyConverter))]
		public virtual int ProgressTextBackgroundTransparency
		{
			get { return this.progressText.BackgroundTransparency; }
			set 
			{
				this.progressText.BackgroundTransparency = value;
				this.OnProgressTextBackgroundTransparencyChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventProgressTextBackgroundTransparencyChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.ProgressTextBackgroundTransparency"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("ProgressTextBackgroundTransparencyChangedDescription")]
		public event EventHandler ProgressTextBackgroundTransparencyChanged
		{
			add
			{
				this.Events.AddHandler(EventProgressTextBackgroundTransparencyChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventProgressTextBackgroundTransparencyChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.ProgressTextBackgroundTransparencyChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnProgressTextBackgroundTransparencyChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventProgressTextBackgroundTransparencyChanged, e);
		}

		/*
		 * ProgressTextBorderColor
		 */

		/// <summary>
		/// Gets or sets the border color for the label.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("ProgressTextCategory")]
		[NuGenSRDescription("ProgressTextBorderColorDescription")]
		public virtual Color ProgressTextBorderColor
		{
			get
			{
				return this.progressText.BorderColor;
			}
			set
			{
				this.progressText.BorderColor = value;
				this.OnProgressTextBorderColorChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventProgressTextBorderColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.ProgressTextBorderColor"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("ProgressTextBorderColorChangedDescription")]
		public event EventHandler ProgressTextBorderColorChanged
		{
			add
			{
				this.Events.AddHandler(EventProgressTextBorderColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventProgressTextBorderColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.ProgressTextBorderColorChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnProgressTextBorderColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventProgressTextBorderColorChanged, e);
		}

		/*
		 * ProgressTextBorderStyle
		 */

		/// <summary>
		/// Gets or sets the border style of the ProgressText.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("ProgressTextCategory")]
		[DefaultValue(NuGenBorderStyle.None)]
		[NuGenSRDescription("ProgressTextBorderStyleDescription")]
		public virtual NuGenBorderStyle ProgressTextBorderStyle
		{
			get
			{
				return this.progressText.BorderStyle;
			}
			set
			{
				this.progressText.BorderStyle = value;
				this.OnProgressTextBorderStyleChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventProgressTextBorderStyleChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.ProgressTextBorderStyle"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("ProgressTextBorderStyleChangedDescription")]
		public event EventHandler ProgressTextBorderStyleChanged
		{
			add
			{
				this.Events.AddHandler(EventProgressTextBorderStyleChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventProgressTextBorderStyleChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterBase.ProgressTextBorderStyleChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnProgressTextBorderStyleChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventProgressTextBorderStyleChanged, e);
		}

		/*
		 * ProgressTextEatLine
		 */

		/// <summary>
		/// Gets or sets the value indicating whether to eat the tail symbols which cannot be displayed
		/// due to the label width.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("ProgressTextCategory")]
		[DefaultValue(true)]
		[NuGenSRDescription("ProgressTextEatLineDescription")]
		public virtual bool ProgressTextEatLine
		{
			get { return this.progressText.EatLine; }
			set 
			{
				this.progressText.EatLine = value;
				this.OnProgressTextEatLineChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventProgressTextEatLineChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.ProgressTextEatLine"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("ProgressTextEatLineChangedDescription")]
		public event EventHandler ProgressTextEatLineChanged
		{
			add
			{
				this.Events.AddHandler(EventProgressTextEatLineChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventProgressTextEatLineChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterBase.ProgressTextEatLineChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnProgressTextEatLineChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventProgressTextEatLineChanged, e);
		}

		/*
		 * ProgressTextEdgeIndent
		 */

		/// <summary>
		/// Determines the indent of the progress text from the nearest edge of the meter.
		/// </summary>
		private int progressTextEdgeIndent = 32;

		/// <summary>
		/// Gets or sets the indent of the progress text from the nearest edge of the meter.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("ProgressTextCategory")]
		[DefaultValue(32)]
		[NuGenSRDescription("ProgressTextEdgeIndentDescription")]
		public int ProgressTextEdgeIndent
		{
			get
			{
				return this.progressTextEdgeIndent;
			}
			set
			{
				this.progressTextEdgeIndent = value;
				this.OnProgressTextEdgeIndentChanged(EventArgs.Empty);

				if (this.Orientation == NuGenOrientationStyle.Horizontal)
				{
					this.progressText.Width = value;
				}
				else
				{
					this.progressText.Height = value;
				}
			}
		}

		private static readonly object EventProgressTextEdgeIndentChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.ProgressTextEdgeIndent"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("ProgressTextEdgeIndentChangedDescription")]
		public event EventHandler ProgressTextEdgeIndentChanged
		{
			add
			{
				this.Events.AddHandler(EventProgressTextEdgeIndentChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventProgressTextEdgeIndentChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterBase.ProgressTextEdgeIndentChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnProgressTextEdgeIndentChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventProgressTextEdgeIndentChanged, e);
		}

		/*
		 * ProgressTextFont
		 */

		/// <summary>
		/// Gets or sets the font for the progress text.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("ProgressTextCategory")]
		[NuGenSRDescription("ProgressTextFontDescription")]
		public virtual Font ProgressTextFont
		{
			get { return this.progressText.Font; }
			set 
			{
				this.progressText.Font = value;
				this.OnProgressTextFontChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventProgressTextFontChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.ProgressTextFont"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("ProgressTextFontChangedDescription")]
		public event EventHandler ProgressTextFontChanged
		{
			add
			{
				this.Events.AddHandler(EventProgressTextFontChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventProgressTextFontChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterBase.ProgressTextFontChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnProgressTextFontChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventProgressTextFontChanged, e);
		}

		/*
		 * ProgressTextForeColor
		 */

		/// <summary>
		/// Gets or sets the foreground color of the control.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("ProgressTextCategory")]
		[NuGenSRDescription("ProgressTextForeColorDescription")]
		public virtual Color ProgressTextForeColor
		{
			get
			{
				return this.progressText.ForegroundColor;
			}
			set
			{
				this.progressText.ForegroundColor = value;
				this.OnProgressTextForeColorChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventProgressTextForeColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.ProgressTextForeColor"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("ProgressTextForeColorChangedDescription")]
		public event EventHandler ProgressTextForeColorChanged
		{
			add
			{
				this.Events.AddHandler(EventProgressTextForeColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventProgressTextForeColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterBase.ProgressTextForeColorChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnProgressTextForeColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventProgressTextForeColorChanged, e);
		}

		/*
		 * ProgressTextForegroundTransparency
		 */

		/// <summary>
		/// Gets or sets the foreground transparency level for the label.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("ProgressTextCategory")]
		[DefaultValue(0)]
		[NuGenSRDescription("ProgressTextForegroundTransparencyDescription")]
		[Editor(typeof(NuGenTransparencyEditor), typeof(UITypeEditor))]
		[TypeConverter(typeof(NuGenTransparencyConverter))]
		public virtual int ProgressTextForegroundTransparency
		{
			get
			{
				return this.progressText.ForegroundTransparency;
			}
			set
			{
				this.progressText.ForegroundTransparency = value;
				this.OnProgressTextForegroundTransparencyChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventProgressTextForegroundTransparencyChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.ProgressTextForegroundTransparency"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("ProgressTextForegroundTransparencyChangedDescription")]
		public event EventHandler ProgressTextForegroundTransparencyChanged
		{
			add
			{
				this.Events.AddHandler(EventProgressTextForegroundTransparencyChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventProgressTextForegroundTransparencyChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.ProgressTextForegroundTransparencyChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnProgressTextForegroundTransparencyChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventProgressTextForegroundTransparencyChanged, e);
		}

		/*
		 * ProgressTextOrientation
		 */

		/// <summary>
		/// Gets or sets the direction of the progress text.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("ProgressTextCategory")]
		[DefaultValue(NuGenOrientationStyle.Horizontal)]
		[NuGenSRDescription("ProgressTextOrientationDescription")]
		public virtual NuGenOrientationStyle ProgressTextOrientation
		{
			get { return this.progressText.TextOrientation; }
			set 
			{
				this.progressText.TextOrientation = value;
				this.OnProgressTextOrientationChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventProgressTextOrientationChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.ProgressTextOrientation"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("ProgressTextOrientationChangedDescription")]
		public event EventHandler ProgressTextOrientationChanged
		{
			add
			{
				this.Events.AddHandler(EventProgressTextOrientationChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventProgressTextOrientationChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterBase.ProgressTextOrientationChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnProgressTextOrientationChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventProgressTextOrientationChanged, e);
		}

		/*
		 * ProgressTextPosition
		 */

		/// <summary>
		/// Determines the positioning of the progress text.
		/// </summary>
		private NuGenProgressTextPositionStyle progressTextPosition = NuGenProgressTextPositionStyle.Head;

		/// <summary>
		/// Gets or sets the positioning of the progress text.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("ProgressTextCategory")]
		[DefaultValue(NuGenProgressTextPositionStyle.Head)]
		[NuGenSRDescription("ProgressTextPositionDescription")]
		public virtual NuGenProgressTextPositionStyle ProgressTextPosition
		{
			get { return this.progressTextPosition; }
			set 
			{
				if (this.progressTextPosition != value)
				{
					this.progressTextPosition = value;
					this.OnProgressTextPositionChanged(EventArgs.Empty);
					this.SetLayout(this.Orientation);
				}
			}
		}

		private static readonly object EventProgressTextPositionChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.ProgressTextPosition"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("ProgressTextPositionChangedDescription")]
		public event EventHandler ProgressTextPositionChanged
		{
			add
			{
				this.Events.AddHandler(EventProgressTextPositionChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventProgressTextPositionChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterBase.ProgressTextPositionChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnProgressTextPositionChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventProgressTextPositionChanged, e);
		}

		/*
		 * ProgressTextVisible
		 */

		/// <summary>
		/// Gets or sets the value indicating whether the progress text is visible.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("ProgressTextCategory")]
		[DefaultValue(true)]
		[NuGenSRDescription("ProgressTextVisibleDescription")]
		public virtual bool ProgressTextVisible
		{
			get { return this.progressText.Visible; }
			set 
			{ 
				this.progressText.Visible = value;
				this.OnProgressTextVisibleChanged(EventArgs.Empty);
				this.SetLayout(this.Orientation);
			}
		}

		private static readonly object EventProgressTextVisibleChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterGeneric.ProgressTextVisible"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("ProgressTextVisibleChangedDescription")]
		public event EventHandler ProgressTextVisibleChanged
		{
			add
			{
				this.Events.AddHandler(EventProgressTextVisibleChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventProgressTextVisibleChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterBase.ProgressTextVisibleChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnProgressTextVisibleChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventProgressTextVisibleChanged, e);
		}

		#endregion

		#region Properties.NonBrowsable

		/// <summary>
		/// Gets or sets the current value of the meter.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual float Value
		{
			get
			{
				return this.meter.Value;
			}
			set
			{
				this.meter.Value = value;
				this.OnValueChanged(new NuGenTargetEventArgs(this, value));
			}
		}

		#endregion

		#region Properties.Public.Overriden

		/// <summary>
		/// Gets or sets the background color for the meter.
		/// </summary>
		/// <value></value>
		[DefaultValue(typeof(Color), "Yellow")]
		public override Color BackgroundColor
		{
			get
			{
				return base.BackgroundColor;
			}
			set
			{
				base.BackgroundColor = value;
			}
		}

		/// <summary>
		/// Gets or sets the background style for the control.
		/// </summary>
		[DefaultValue(NuGenBackgroundStyle.Gradient)]
		public override NuGenBackgroundStyle BackgroundStyle
		{
			get
			{
				return base.BackgroundStyle;
			}
			set
			{
				base.BackgroundStyle = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of the counter.
		/// </summary>
		/// <value></value>
		[RefreshProperties(RefreshProperties.Repaint)]
		public override string CounterName
		{
			get
			{
				return base.CounterName;
			}
			set
			{
				base.CounterName = value;
				this.CaptionText = value;
			}
		}

		#endregion

		#region Properties.Protected

		/// <summary>
		/// Determines the divider for the counter value.
		/// </summary>
		private const float DIVIDER = 1.0f;

		/// <summary>
		/// Gets or sets a divider for the counter value.
		/// </summary>
		protected virtual float Divider
		{
			get { return DIVIDER; }
			set { return; }
		}

		/// <summary>
		/// Gets or sets the counter format prefix.
		/// </summary>
		protected virtual string Prefix
		{
			get { return ""; }
			set { return; }
		}

		#endregion

		#region Properties.Protected.Overriden

		/// <summary>
		/// Gets the <c>Genetibase.UI.NuGenBarBase</c> object with the specified parameters set.
		/// </summary>
		/// <value></value>
		protected override NuGenBarBase Bar
		{
			get { return this.meter; }
		}

		/// <summary>
		/// Defines the counter.
		/// </summary>
		private PerformanceCounter internalCounterType  = pc.GetCounter(en.PercentProcessorTime);

		/// <summary>
		/// Gets the <c>System.Diagnostics.PerformanceCounter</c> object with the specified parameters set.
		/// </summary>
		/// <value></value>
		protected override PerformanceCounter CounterType
		{
			get { return this.internalCounterType; }
		}

		#endregion

		#region Methods.Protected.Overriden

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGenericBase.TimerTick"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		protected override void OnTimerTick(EventArgs e)
		{
			if (this.CounterType == null)
			{
				Trace.WriteLine("this.CounterType cannot be null.", "Error");
				return;
			}

			try
			{
				this.Value = this.CounterType.NextValue();
			}
			catch
			{
				return;
			}

			this.progressText.Text = Convert.ToString(Math.Round((this.Value / this.Divider), 2)) + " " + this.Prefix + this.CounterFormat;
			this.Refresh();

			base.OnTimerTick(e);
		}

		#endregion

		#region Methods.Private
		
		/// <summary>
		/// Sets the layout according to the orientation specified.
		/// </summary>
		/// <param name="orientation">Orientation of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterBase"/> control.</param>
		private void SetLayout(NuGenOrientationStyle orientation)
		{			
			this.SuspendLayout();
			this.Controls.Clear();

			/*
			 * meter
			 */

			int meterWidth = this.meter.Width;
			int meterHeight = this.meter.Height;

			if (this.meter.Orientation == orientation)
			{
				this.Width = meterWidth;
				this.Height = meterHeight;
			}
			else
			{
				this.Height = meterWidth;
				this.Width = meterHeight;
			}

			this.meter.Orientation = orientation;
			this.meter.Top = 0;
			this.meter.Left = 0;
			
			this.Controls.Add(this.meter);

			/*
			 * progressText 
			 */

			if (this.progressText.Visible) 
			{
				if (orientation == NuGenOrientationStyle.Vertical) 
				{
					this.progressText.Height = this.ProgressTextEdgeIndent;
					this.Height += this.ProgressTextEdgeIndent;

					switch (this.ProgressTextPosition)
					{
						case NuGenProgressTextPositionStyle.Head:
							this.progressText.Dock = DockStyle.Bottom;
							break;
						case NuGenProgressTextPositionStyle.Tail:
							this.meter.Top += this.ProgressTextEdgeIndent;
							this.progressText.Dock = DockStyle.Top;
							break;
					}
				}
				else if (orientation == NuGenOrientationStyle.Horizontal)
				{
					this.progressText.Width = this.ProgressTextEdgeIndent;

					switch (this.ProgressTextPosition)
					{
						case NuGenProgressTextPositionStyle.Head:
							this.progressText.Width = this.ProgressTextEdgeIndent;
							this.Width += this.ProgressTextEdgeIndent;
							this.meter.Left += this.ProgressTextEdgeIndent;
							this.progressText.Dock = DockStyle.Left;
							break;
						case NuGenProgressTextPositionStyle.Tail:
							this.progressText.Width = this.ProgressTextEdgeIndent;
							this.Width += this.ProgressTextEdgeIndent;
							this.progressText.Dock = DockStyle.Right;
							break;
					}
				}

				this.Controls.Add(this.progressText);
			}
			
			/*
			 * captionText
			 */

			if (this.captionText.Visible)
			{
				this.captionText.Height = this.CaptionTextEdgeIndent;
				this.Height += this.CaptionTextEdgeIndent;
				
				switch (this.CaptionTextPosition)
				{
					case NuGenCaptionTextPositionStyle.Header:
						this.meter.Top += this.CaptionTextEdgeIndent;
						this.captionText.Dock = DockStyle.Top;
						break;
					case NuGenCaptionTextPositionStyle.Footer:
						this.captionText.Dock = DockStyle.Bottom;
						break;
				}

				this.Controls.Add(this.captionText);
			}

			this.ResumeLayout();
		}

		#endregion

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterBase"/> class.
		/// </summary>
		public NuGenMeterGeneric()
		{
			InitializeComponent();
			//
			// meter
			//
			this.meter.ReleaseTickLineTimeout = this.ReleaseTickLineTimeout;
			//
			// captionText
			//
			this.captionText = new NuGenCTLabel();
			//
			// progressText
			//
			this.progressText = new NuGenCTLabel();
			this.progressText.Text = "0" + this.CounterFormat;
			//
			// NuGenMeterBase
			//
			this.CaptionText = this.CounterType.CounterName;
			this.SetLayout(NuGenOrientationStyle.Vertical);
			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.BackColor = Color.Transparent;
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
		[DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.meter = new NuGenMeterBar();
			this.SuspendLayout();
			// 
			// meter
			// 
			this.meter.BackGradientEndColor = System.Drawing.Color.Coral;
			this.meter.BackGradientStartColor = System.Drawing.Color.Yellow;
			this.meter.BackTubeGradientEndColor = System.Drawing.Color.Coral;
			this.meter.BackTubeGradientStartColor = System.Drawing.Color.Yellow;
			this.meter.BorderColor = System.Drawing.Color.Black;
			this.meter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.meter.GradientEndColor = System.Drawing.Color.Red;
			this.meter.GradientStartColor = System.Drawing.Color.Blue;
			this.meter.Location = new System.Drawing.Point(0, 0);
			this.meter.Name = "meter";
			this.meter.Size = new System.Drawing.Size(40, 128);
			this.meter.TabIndex = 4;
			this.meter.TubeGradientEndColor = System.Drawing.Color.Red;
			this.meter.TubeGradientStartColor = System.Drawing.Color.Blue;
			// 
			// NuGenMeterBase
			// 
			this.Controls.Add(this.meter);
			this.Name = "NuGenMeterBase";
			this.Size = new System.Drawing.Size(40, 128);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
