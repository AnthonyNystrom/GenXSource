/* -----------------------------------------------
 * NuGenMeter.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Design;
using Genetibase.Shared;
using Genetibase.Meters.ComponentModel;
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

namespace Genetibase.Meters
{
	/// <summary>
	/// Defines the base functionality for the meter controls.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[ToolboxItem(true)]
	public class NuGenMeter : NuGenGenericBase
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
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Orientation")]
		[DefaultValue(NuGenOrientationStyle.Vertical)]
		public virtual NuGenOrientationStyle Orientation
		{
			get
			{
				return this._orientation;
			}
			set
			{
				if (this._orientation != value)
				{
					this._orientation = value;
					this.OnOrientationChanged(EventArgs.Empty);
					this.SetLayout(value);
					this.Refresh();
				}
			}
		}

		private static readonly Object _orientationChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenMeterGeneric.Orientation"/>
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
		/// Raises the <see cref="E:Genetibase.Meters.NuGenMeterGeneric.OrientationChanged"/> event.
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
				return _meter.TickLine;
			}
			set
			{
				_meter.TickLine = value;
				this.OnTickLineChanged(EventArgs.Empty);
			}
		}

		private static readonly Object _tickLineChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenMeterGeneric.TickLine"/>
		/// property changes.
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
		/// Raises the <see cref="E:Genetibase.Meters.NuGenMeterGeneric.TickLineChanged"/> event.
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
		/// Indicates whether to show the tick line.
		/// </summary>
		private Boolean _releaseTickLine = true;

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
				return this._releaseTickLine;
			}
			set
			{
				this._releaseTickLine = value;
				_meter.ReleaseTickLine = this._releaseTickLine;
				this.OnReleaseTickLineChanged(EventArgs.Empty);
			}
		}

		private static readonly Object _releaseTickLineChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenMeterGeneric.ReleaseTickLine"/>
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
		/// Raises the <see cref="E:Genetibase.Meters.NuGenMeterGeneric.ReleaseTickLineChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnReleaseTickLineChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_releaseTickLineChanged, e);
		}

		/*
		 * ReleaseTickLineTimeout
		 */

		/// <summary>
		/// The time in milliseconds between the tick line releases.
		/// </summary>
		private Double _releaseTickLineTimeout = 5000d;

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
				return this._releaseTickLineTimeout;
			}
			set
			{
				this._releaseTickLineTimeout = value;
				_meter.ReleaseTickLineTimeout = this._releaseTickLineTimeout;
				this.OnReleaseTickLineTimeoutChanged(EventArgs.Empty);
			}
		}

		private static readonly Object _releaseTickLineTimeoutChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenMeterGeneric.ReleaseTickLineTimeout"/>
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
		/// Raises the <see cref="E:Genetibase.Meters.NuGenMeterGeneric.ReleaseTickLineTimeout"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnReleaseTickLineTimeoutChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_releaseTickLineTimeoutChanged, e);
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
		[NuGenSRCategory("Category_CaptionText")]
		[NuGenSRDescription("Description_CaptionText")]
		public virtual String CaptionText
		{
			get
			{
				return _captionText.Text;
			}
			set
			{
				if (_captionText.Text != value)
				{
					_captionText.Text = value;
					this.OnCaptionTextChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly Object _captionTextChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.CaptionText"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_CaptionTextChanged")]
		public event EventHandler CaptionTextChanged
		{
			add
			{
				this.Events.AddHandler(_captionTextChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_captionTextChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.Meters.NuGenMeterBase.CaptionTextChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCaptionTextChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_captionTextChanged, e);
		}

		/*
		 * CaptionTextAlignment
		 */

		/// <summary>
		/// Gets or sets the alignment of the caption text.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_CaptionText")]
		[DefaultValue(ContentAlignment.MiddleCenter)]
		[NuGenSRDescription("Description_CaptionTextAlignment")]
		public ContentAlignment CaptionTextAlignment
		{
			get
			{
				return _captionText.TextAlign;
			}
			set
			{
				_captionText.TextAlign = value;
				this.OnCaptionTextAlignmentChanged(EventArgs.Empty);
			}
		}

		private static readonly Object _captionTextAlignmentChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.CaptionTextAlignment"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_CaptionTextAlignmentChanged")]
		public event EventHandler CaptionTextAlignmentChanged
		{
			add
			{
				this.Events.AddHandler(_captionTextAlignmentChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_captionTextAlignmentChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.Meters.NuGenMeterBase.CaptionTextAlignmentChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCaptionTextAlignmentChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_captionTextAlignmentChanged, e);
		}

		/*
		 * CaptionTextBackColor
		 */

		/// <summary>
		/// Gets or sets the background color for the label.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_CaptionText")]
		[NuGenSRDescription("Description_CaptionTextBackColor")]
		public virtual Color CaptionTextBackColor
		{
			get
			{
				return _captionText.BackgroundColor;
			}
			set
			{
				_captionText.BackgroundColor = value;
				this.OnCaptionTextBackColorChanged(EventArgs.Empty);
			}
		}

		private static readonly Object _captionTextBackColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.CaptionTextBackColor"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_CaptionTextBackColorChanged")]
		public event EventHandler CaptionTextBackColorChanged
		{
			add
			{
				this.Events.AddHandler(_captionTextBackColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_captionTextBackColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.CaptionTextBackColorChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCaptionTextBackColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_captionTextBackColorChanged, e);
		}

		/*
		 * CaptionTextBackgroundTransparency
		 */

		/// <summary>
		/// Gets or sets the background transparency level for the label.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_CaptionText")]
		[DefaultValue(0)]
		[NuGenSRDescription("Description_CaptionTextBackgroundTransparency")]
		[Editor(typeof(NuGenTransparencyEditor), typeof(UITypeEditor))]
		[TypeConverter(typeof(NuGenTransparencyConverter))]
		public virtual Int32 CaptionTextBackgroundTransparency
		{
			get
			{
				return _captionText.BackgroundTransparency;
			}
			set
			{
				_captionText.BackgroundTransparency = value;
				this.OnCaptionTextBackgroundTransparencyChanged(EventArgs.Empty);
			}
		}

		private static readonly Object _captionTextBackgroundTransparencyChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.CaptionTextBackgroundTransparency"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_CaptionTextBackgroundTransparencyChanged")]
		public event EventHandler CaptionTextBackgroundTransparencyChanged
		{
			add
			{
				this.Events.AddHandler(_captionTextBackgroundTransparencyChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_captionTextBackgroundTransparencyChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.CaptionTextBackgroundTransparencyChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCaptionTextBackgroundTransparencyChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_captionTextBackgroundTransparencyChanged, e);
		}

		/*
		 * CaptionTextBorderColor
		 */

		/// <summary>
		/// Gets or sets the border color for the label.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_CaptionText")]
		[DefaultValue(typeof(Color), "Black")]
		[NuGenSRDescription("Description_CaptionTextBorderColor")]
		public virtual Color CaptionTextBorderColor
		{
			get
			{
				return _captionText.BorderColor;
			}
			set
			{
				_captionText.BorderColor = value;
				this.OnCaptionTextBorderColorChanged(EventArgs.Empty);
			}
		}

		private static readonly Object _captionTextBorderColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.CaptionTextBorderColorChanged"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_CaptionTextBorderColorChanged")]
		public event EventHandler CaptionTextBorderColorChanged
		{
			add
			{
				this.Events.AddHandler(_captionTextBorderColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_captionTextBorderColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.CaptionTextBorderColorChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCaptionTextBorderColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_captionTextBorderColorChanged, e);
		}

		/*
		 * CaptionTextBorderStyle
		 */

		/// <summary>
		/// Gets or sets the border style of the CaptionText.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_CaptionText")]
		[DefaultValue(NuGenBorderStyle.None)]
		[NuGenSRDescription("Description_CaptionTextBorderStyle")]
		public virtual NuGenBorderStyle CaptionTextBorderStyle
		{
			get
			{
				return _captionText.BorderStyle;
			}
			set
			{
				_captionText.BorderStyle = value;
				this.OnCaptionTextBorderStyleChanged(EventArgs.Empty);
			}
		}

		private static readonly Object _captionTextBorderStyleChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.CaptionTextBorderStyle"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_CaptionTextBorderStyleChanged")]
		public event EventHandler CaptionTextBorderStyleChanged
		{
			add
			{
				this.Events.AddHandler(_captionTextBorderStyleChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_captionTextBorderStyleChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.Meters.NuGenMeterBase.CaptionTextBorderStyleChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCaptionTextBorderStyleChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_captionTextBorderStyleChanged, e);
		}

		/*
		 * CaptionTextEatLine
		 */

		/// <summary>
		/// Gets or sets the value indicating whether to eat the tail symbols which cannot be displayed
		/// due to the label width.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_CaptionText")]
		[DefaultValue(true)]
		[NuGenSRDescription("Description_CaptionTextEatLine")]
		public virtual Boolean CaptionTextEatLine
		{
			get
			{
				return _captionText.EatLine;
			}
			set
			{
				_captionText.EatLine = value;
				this.OnCaptionTextEatLineChanged(EventArgs.Empty);
			}
		}

		private static readonly Object _captionTextEatLineChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.CaptionTextEatLine"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_CaptionTextEatLineChanged")]
		public event EventHandler CaptionTextEatLineChanged
		{
			add
			{
				this.Events.AddHandler(_captionTextEatLineChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_captionTextEatLineChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.Meters.NuGenMeterBase.CaptionTextEatLineChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCaptionTextEatLineChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_captionTextEatLineChanged, e);
		}

		/*
		 * CaptionTextFont
		 */

		/// <summary>
		/// Gets or sets the font for the caption text.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_CaptionText")]
		[NuGenSRDescription("Description_CaptionTextFont")]
		public virtual Font CaptionTextFont
		{
			get
			{
				return _captionText.Font;
			}
			set
			{
				_captionText.Font = value;
				this.OnCaptionTextFontChanged(EventArgs.Empty);
			}
		}

		private static readonly Object _captionTextFontChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.CaptionTextFont"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_CaptionTextFontChanged")]
		public event EventHandler CaptionTextFontChanged
		{
			add
			{
				this.Events.AddHandler(_captionTextFontChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_captionTextFontChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.Meters.NuGenMeterBase.CaptionTextFontChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCaptionTextFontChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_captionTextFontChanged, e);
		}

		/*
		 * CaptionTextForeColor
		 */

		/// <summary>
		/// Gets or sets the foreground color of the control.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_CaptionText")]
		[NuGenSRDescription("Description_CaptionTextForeColor")]
		public virtual Color CaptionTextForeColor
		{
			get
			{
				return _captionText.ForegroundColor;
			}
			set
			{
				_captionText.ForegroundColor = value;
				this.OnCaptionTextForeColorChanged(EventArgs.Empty);
			}
		}

		private static readonly Object _captionForeColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.CaptionTextForeColor"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_CaptionTextForeColorChanged")]
		public event EventHandler CaptionTextForeColorChanged
		{
			add
			{
				this.Events.AddHandler(_captionForeColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_captionForeColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.Meters.NuGenMeterBase.CaptionTextForeColorChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCaptionTextForeColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_captionForeColorChanged, e);
		}

		/*
		 * CaptionTextForegroundTransparency
		 */

		/// <summary>
		/// Gets or sets the foreground transparency level for the label.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_CaptionText")]
		[DefaultValue(0)]
		[NuGenSRDescription("Description_CaptionTextForegroundTransparency")]
		[Editor(typeof(NuGenTransparencyEditor), typeof(UITypeEditor))]
		[TypeConverter(typeof(NuGenTransparencyConverter))]
		public virtual Int32 CaptionTextForegroundTransparency
		{
			get
			{
				return _captionText.ForegroundTransparency;
			}
			set
			{
				_captionText.ForegroundTransparency = value;
				this.OnForegroundTransparencyChanged(EventArgs.Empty);
			}
		}

		private static readonly Object _captionTextForegroundTransparencyChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.CaptionTextForegroundTransparency"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_CaptionTextForegroundTransparencyChanged")]
		public event EventHandler CaptionTextForegroundTransparencyChanged
		{
			add
			{
				this.Events.AddHandler(_captionTextForegroundTransparencyChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_captionTextForegroundTransparencyChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.CaptionTextForegroundTransparencyChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCaptionTextForegroundTransparencyChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_captionTextForegroundTransparencyChanged, e);
		}

		/*
		 * CaptionTextEdgeIndent
		 */

		/// <summary>
		/// Determines the indent of the caption text from the nearest edge of the meter.
		/// </summary>
		private Int32 _captionTextEdgeIndent = 32;

		/// <summary>
		/// Gets or sets the indent of the caption text from the nearest edge of the meter.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_CaptionText")]
		[DefaultValue(32)]
		[NuGenSRDescription("Description_CaptionTextEdgeIndent")]
		public Int32 CaptionTextEdgeIndent
		{
			get
			{
				return this._captionTextEdgeIndent;
			}
			set
			{
				this._captionTextEdgeIndent = value;
				this.OnCaptionTextEdgeIndentChanged(EventArgs.Empty);
				_captionText.Height = value;
			}
		}

		private static readonly Object _captionTextEdgeIndentChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.CaptionTextEdgeIndent"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_CaptionTextEdgeIndentChanged")]
		public event EventHandler CaptionTextEdgeIndentChanged
		{
			add
			{
				this.Events.AddHandler(_captionTextEdgeIndentChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_captionTextEdgeIndentChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.Meters.NuGenMeterBase.CaptionTextEdgeIndentChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCaptionTextEdgeIndentChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_captionTextEdgeIndentChanged, e);
		}

		/*
		 * CaptionTextOrientation
		 */

		/// <summary>
		/// Gets or sets the direction of the caption text.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_CaptionText")]
		[DefaultValue(NuGenOrientationStyle.Horizontal)]
		[NuGenSRDescription("Description_CaptionTextOrientation")]
		public virtual NuGenOrientationStyle CaptionTextOrientation
		{
			get
			{
				return _captionText.TextOrientation;
			}
			set
			{
				_captionText.TextOrientation = value;
				this.OnCaptionTextOrientationChanged(EventArgs.Empty);
			}
		}

		private static readonly Object _captionTextOrientationChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.CaptionTextOrientation"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_CaptionTextOrientationChanged")]
		public event EventHandler CaptionTextOrientationChanged
		{
			add
			{
				this.Events.AddHandler(_captionTextOrientationChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_captionTextOrientationChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.Meters.NuGenMeterBase.CaptionTextOrientationChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCaptionTextOrientationChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_captionTextOrientationChanged, e);
		}

		/*
		 * CaptionTextPosition
		 */

		/// <summary>
		/// Determines the positioning of the caption text.
		/// </summary>
		private NuGenCaptionTextPositionStyle _captionTextPosition = NuGenCaptionTextPositionStyle.Header;

		/// <summary>
		/// Gets or sets the positioning of the caption text.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_CaptionText")]
		[DefaultValue(NuGenCaptionTextPositionStyle.Header)]
		[NuGenSRDescription("Description_CaptionTextPosition")]
		public virtual NuGenCaptionTextPositionStyle CaptionTextPosition
		{
			get
			{
				return this._captionTextPosition;
			}
			set
			{
				if (this._captionTextPosition != value)
				{
					this._captionTextPosition = value;
					this.OnCaptionTextPositionChanged(EventArgs.Empty);
					this.SetLayout(this.Orientation);
				}
			}
		}

		private static readonly Object _captionTextPositionChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.CaptionTextPosition"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_CaptionTextPositionChanged")]
		public event EventHandler CaptionTextPositionChanged
		{
			add
			{
				this.Events.AddHandler(_captionTextPositionChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_captionTextPositionChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.Meters.NuGenMeterBase.CaptionTextPositionChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCaptionTextPositionChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_captionTextPositionChanged, e);
		}

		/*
		 * CaptionTextVisible
		 */

		/// <summary>
		/// Gets or sets the value indicating whether the caption text is visible.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_CaptionText")]
		[DefaultValue(true)]
		[NuGenSRDescription("Description_CaptionTextVisible")]
		public virtual Boolean CaptionTextVisible
		{
			get
			{
				return _captionText.Visible;
			}
			set
			{
				_captionText.Visible = value;
				this.OnCaptionTextVisibleChanged(EventArgs.Empty);
				this.SetLayout(this.Orientation);
			}
		}

		private static readonly Object _captionTextVisibleChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.CaptionTextVisible"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_CaptionTextVisibleChanged")]
		public event EventHandler CaptionTextVisibleChanged
		{
			add
			{
				this.Events.AddHandler(_captionTextVisibleChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_captionTextVisibleChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.Meters.NuGenMeterBase.CaptionTextVisibleChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCaptionTextVisibleChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_captionTextVisibleChanged, e);
		}

		/*
		 * CaptionTetWordWrap
		 */

		/// <summary>
		/// Gets or sets the value indicating whether the lines are automatically word-wrapped.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_CaptionText")]
		[DefaultValue(false)]
		[NuGenSRDescription("Description_CaptionTextWordWrap")]
		public virtual Boolean CaptionTextWordWrap
		{
			get
			{
				return _captionText.WordWrap;
			}
			set
			{
				_captionText.WordWrap = value;
				this.OnCaptionTextWordWrapChanged(EventArgs.Empty);
			}
		}

		private static readonly Object _captionTextWordWrapChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.CaptionTextWordWrap"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_CaptionTextWordWrapChanged")]
		public event EventHandler CaptionTextWordWrapChanged
		{
			add
			{
				this.Events.AddHandler(_captionTextWordWrapChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_captionTextWordWrapChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.Meters.NuGenMeterBase.CaptionTextWordWrapChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCaptionTextWordWrapChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_captionTextWordWrapChanged, e);
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
		[NuGenSRCategory("Category_ProgressText")]
		[DefaultValue(ContentAlignment.MiddleCenter)]
		[NuGenSRDescription("Description_ProgressTextAlignment")]
		public virtual ContentAlignment ProgressTextAlignment
		{
			get
			{
				return _progressText.TextAlign;
			}
			set
			{
				_progressText.TextAlign = value;
				this.OnProgressTextAlignmentChanged(EventArgs.Empty);
			}
		}

		private static readonly Object _progressTextAlignmentChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.ProgressTextAlignment"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ProgressTextAlignmentChanged")]
		public event EventHandler ProgressTextAlignmentChanged
		{
			add
			{
				this.Events.AddHandler(_progressTextAlignmentChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_progressTextAlignmentChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.Meters.NuGenMeterBase.ProgressTextAlignmentChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnProgressTextAlignmentChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_progressTextAlignmentChanged, e);
		}

		/*
		 * ProgressTextBackColor
		 */

		/// <summary>
		/// Gets or sets the background color for the label.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_ProgressText")]
		[DefaultValue(typeof(Color), "Black")]
		[NuGenSRDescription("Description_ProgressTextBackColor")]
		public virtual Color ProgressTextBackColor
		{
			get
			{
				return _progressText.BackgroundColor;
			}
			set
			{
				if (_progressText.BackgroundColor != value)
				{
					_progressText.BackgroundColor = value;
					this.OnProgressTextBackColorChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly Object _progressTextBackColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.ProgressTextBackColor"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ProgressTextBackColorChanged")]
		public event EventHandler ProgressTextBackColorChanged
		{
			add
			{
				this.Events.AddHandler(_progressTextBackColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_progressTextBackColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.ProgressTextBackColorChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnProgressTextBackColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_progressTextBackColorChanged, e);
		}

		/*
		 * ProgressTextBackgroundTransparency
		 */

		/// <summary>
		/// Gets or sets the background transparency level for the label.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_ProgressText")]
		[DefaultValue(0)]
		[NuGenSRDescription("Description_ProgressTextBackgroundTransparency")]
		[Editor(typeof(NuGenTransparencyEditor), typeof(UITypeEditor))]
		[TypeConverter(typeof(NuGenTransparencyConverter))]
		public virtual Int32 ProgressTextBackgroundTransparency
		{
			get
			{
				return _progressText.BackgroundTransparency;
			}
			set
			{
				_progressText.BackgroundTransparency = value;
				this.OnProgressTextBackgroundTransparencyChanged(EventArgs.Empty);
			}
		}

		private static readonly Object _progressTextBackgroundTransparencyChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.ProgressTextBackgroundTransparency"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ProgressTextBackgroundTransparencyChanged")]
		public event EventHandler ProgressTextBackgroundTransparencyChanged
		{
			add
			{
				this.Events.AddHandler(_progressTextBackgroundTransparencyChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_progressTextBackgroundTransparencyChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.ProgressTextBackgroundTransparencyChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnProgressTextBackgroundTransparencyChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_progressTextBackgroundTransparencyChanged, e);
		}

		/*
		 * ProgressTextBorderColor
		 */

		/// <summary>
		/// Gets or sets the border color for the label.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_ProgressText")]
		[NuGenSRDescription("Description_ProgressTextBorderColor")]
		public virtual Color ProgressTextBorderColor
		{
			get
			{
				return _progressText.BorderColor;
			}
			set
			{
				_progressText.BorderColor = value;
				this.OnProgressTextBorderColorChanged(EventArgs.Empty);
			}
		}

		private static readonly Object _progressTextBorderColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.ProgressTextBorderColor"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ProgressTextBorderColorChanged")]
		public event EventHandler ProgressTextBorderColorChanged
		{
			add
			{
				this.Events.AddHandler(_progressTextBorderColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_progressTextBorderColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.ProgressTextBorderColorChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnProgressTextBorderColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_progressTextBorderColorChanged, e);
		}

		/*
		 * ProgressTextBorderStyle
		 */

		/// <summary>
		/// Gets or sets the border style of the ProgressText.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_ProgressText")]
		[DefaultValue(NuGenBorderStyle.None)]
		[NuGenSRDescription("Description_ProgressTextBorderStyle")]
		public virtual NuGenBorderStyle ProgressTextBorderStyle
		{
			get
			{
				return _progressText.BorderStyle;
			}
			set
			{
				_progressText.BorderStyle = value;
				this.OnProgressTextBorderStyleChanged(EventArgs.Empty);
			}
		}

		private static readonly Object _progressTextBorderStyleChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.ProgressTextBorderStyle"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ProgressTextBorderStyleChanged")]
		public event EventHandler ProgressTextBorderStyleChanged
		{
			add
			{
				this.Events.AddHandler(_progressTextBorderStyleChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_progressTextBorderStyleChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.Meters.NuGenMeterBase.ProgressTextBorderStyleChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnProgressTextBorderStyleChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_progressTextBorderStyleChanged, e);
		}

		/*
		 * ProgressTextEatLine
		 */

		/// <summary>
		/// Gets or sets the value indicating whether to eat the tail symbols which cannot be displayed
		/// due to the label width.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_ProgressText")]
		[DefaultValue(true)]
		[NuGenSRDescription("Description_ProgressTextEatLine")]
		public virtual Boolean ProgressTextEatLine
		{
			get
			{
				return _progressText.EatLine;
			}
			set
			{
				_progressText.EatLine = value;
				this.OnProgressTextEatLineChanged(EventArgs.Empty);
			}
		}

		private static readonly Object _progressTextEatLineChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.ProgressTextEatLine"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ProgressTextEatLineChanged")]
		public event EventHandler ProgressTextEatLineChanged
		{
			add
			{
				this.Events.AddHandler(_progressTextEatLineChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_progressTextEatLineChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.Meters.NuGenMeterBase.ProgressTextEatLineChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnProgressTextEatLineChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_progressTextEatLineChanged, e);
		}

		/*
		 * ProgressTextEdgeIndent
		 */

		/// <summary>
		/// Determines the indent of the progress text from the nearest edge of the meter.
		/// </summary>
		private Int32 _progressTextEdgeIndent = 32;

		/// <summary>
		/// Gets or sets the indent of the progress text from the nearest edge of the meter.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_ProgressText")]
		[DefaultValue(32)]
		[NuGenSRDescription("Description_ProgressTextEdgeIndent")]
		public Int32 ProgressTextEdgeIndent
		{
			get
			{
				return this._progressTextEdgeIndent;
			}
			set
			{
				this._progressTextEdgeIndent = value;
				this.OnProgressTextEdgeIndentChanged(EventArgs.Empty);

				if (this.Orientation == NuGenOrientationStyle.Horizontal)
				{
					_progressText.Width = value;
				}
				else
				{
					_progressText.Height = value;
				}
			}
		}

		private static readonly Object _progressTextEdgeIndentChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.ProgressTextEdgeIndent"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ProgressTextEdgeIndentChanged")]
		public event EventHandler ProgressTextEdgeIndentChanged
		{
			add
			{
				this.Events.AddHandler(_progressTextEdgeIndentChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_progressTextEdgeIndentChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.Meters.NuGenMeterBase.ProgressTextEdgeIndentChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnProgressTextEdgeIndentChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_progressTextEdgeIndentChanged, e);
		}

		/*
		 * ProgressTextFont
		 */

		/// <summary>
		/// Gets or sets the font for the progress text.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_ProgressText")]
		[NuGenSRDescription("Description_ProgressTextFont")]
		public virtual Font ProgressTextFont
		{
			get
			{
				return _progressText.Font;
			}
			set
			{
				_progressText.Font = value;
				this.OnProgressTextFontChanged(EventArgs.Empty);
			}
		}

		private static readonly Object _progressTextFontChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.ProgressTextFont"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ProgressTextFontChanged")]
		public event EventHandler ProgressTextFontChanged
		{
			add
			{
				this.Events.AddHandler(_progressTextFontChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_progressTextFontChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.Meters.NuGenMeterBase.ProgressTextFontChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnProgressTextFontChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_progressTextFontChanged, e);
		}

		/*
		 * ProgressTextForeColor
		 */

		/// <summary>
		/// Gets or sets the foreground color of the control.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_ProgressText")]
		[NuGenSRDescription("Description_ProgressTextForeColor")]
		public virtual Color ProgressTextForeColor
		{
			get
			{
				return _progressText.ForegroundColor;
			}
			set
			{
				_progressText.ForegroundColor = value;
				this.OnProgressTextForeColorChanged(EventArgs.Empty);
			}
		}

		private static readonly Object _progressTextForeColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.ProgressTextForeColor"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ProgressTextForeColorChanged")]
		public event EventHandler ProgressTextForeColorChanged
		{
			add
			{
				this.Events.AddHandler(_progressTextForeColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_progressTextForeColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.Meters.NuGenMeterBase.ProgressTextForeColorChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnProgressTextForeColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_progressTextForeColorChanged, e);
		}

		/*
		 * ProgressTextForegroundTransparency
		 */

		/// <summary>
		/// Gets or sets the foreground transparency level for the label.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_ProgressText")]
		[DefaultValue(0)]
		[NuGenSRDescription("Description_ProgressTextForegroundTransparency")]
		[Editor(typeof(NuGenTransparencyEditor), typeof(UITypeEditor))]
		[TypeConverter(typeof(NuGenTransparencyConverter))]
		public virtual Int32 ProgressTextForegroundTransparency
		{
			get
			{
				return _progressText.ForegroundTransparency;
			}
			set
			{
				_progressText.ForegroundTransparency = value;
				this.OnProgressTextForegroundTransparencyChanged(EventArgs.Empty);
			}
		}

		private static readonly Object _progressTextForegroundTransparencyChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.ProgressTextForegroundTransparency"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ProgressTextForegroundTransparencyChanged")]
		public event EventHandler ProgressTextForegroundTransparencyChanged
		{
			add
			{
				this.Events.AddHandler(_progressTextForegroundTransparencyChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_progressTextForegroundTransparencyChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.ProgressTextForegroundTransparencyChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnProgressTextForegroundTransparencyChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_progressTextForegroundTransparencyChanged, e);
		}

		/*
		 * ProgressTextOrientation
		 */

		/// <summary>
		/// Gets or sets the direction of the progress text.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_ProgressText")]
		[DefaultValue(NuGenOrientationStyle.Horizontal)]
		[NuGenSRDescription("Description_ProgressTextOrientation")]
		public virtual NuGenOrientationStyle ProgressTextOrientation
		{
			get
			{
				return _progressText.TextOrientation;
			}
			set
			{
				_progressText.TextOrientation = value;
				this.OnProgressTextOrientationChanged(EventArgs.Empty);
			}
		}

		private static readonly Object _progressTextOrientationChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.ProgressTextOrientation"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ProgressTextOrientationChanged")]
		public event EventHandler ProgressTextOrientationChanged
		{
			add
			{
				this.Events.AddHandler(_progressTextOrientationChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_progressTextOrientationChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.Meters.NuGenMeterBase.ProgressTextOrientationChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnProgressTextOrientationChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_progressTextOrientationChanged, e);
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
		[NuGenSRCategory("Category_ProgressText")]
		[DefaultValue(NuGenProgressTextPositionStyle.Head)]
		[NuGenSRDescription("Description_ProgressTextPosition")]
		public virtual NuGenProgressTextPositionStyle ProgressTextPosition
		{
			get
			{
				return this.progressTextPosition;
			}
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

		private static readonly Object _progressTextPositionChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.ProgressTextPosition"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ProgressTextPositionChanged")]
		public event EventHandler ProgressTextPositionChanged
		{
			add
			{
				this.Events.AddHandler(_progressTextPositionChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_progressTextPositionChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.Meters.NuGenMeterBase.ProgressTextPositionChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnProgressTextPositionChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_progressTextPositionChanged, e);
		}

		/*
		 * ProgressTextVisible
		 */

		/// <summary>
		/// Gets or sets the value indicating whether the progress text is visible.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_ProgressText")]
		[DefaultValue(true)]
		[NuGenSRDescription("Description_ProgressTextVisible")]
		public virtual Boolean ProgressTextVisible
		{
			get
			{
				return _progressText.Visible;
			}
			set
			{
				_progressText.Visible = value;
				this.OnProgressTextVisibleChanged(EventArgs.Empty);
				this.SetLayout(this.Orientation);
			}
		}

		private static readonly Object _progressTextVisibleChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="T:Genetibase.Meters.NuGenMeterGeneric.ProgressTextVisible"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ProgressTextVisibleChanged")]
		public event EventHandler ProgressTextVisibleChanged
		{
			add
			{
				this.Events.AddHandler(_progressTextVisibleChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_progressTextVisibleChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="T:Genetibase.Meters.NuGenMeterBase.ProgressTextVisibleChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnProgressTextVisibleChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_progressTextVisibleChanged, e);
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
				return _meter.Value;
			}
			set
			{
				_meter.Value = value;
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
		public override String CounterName
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
			get
			{
				return DIVIDER;
			}
			set
			{
				return;
			}
		}

		/// <summary>
		/// Gets or sets the counter format prefix.
		/// </summary>
		protected virtual String Prefix
		{
			get
			{
				return "";
			}
			set
			{
				return;
			}
		}

		#endregion

		#region Properties.Protected.Overriden

		/// <summary>
		/// Gets the <see cref="NuGenBarBase"/> object with the specified parameters set.
		/// </summary>
		/// <value></value>
		protected override NuGenBarBase Bar
		{
			get
			{
				return _meter;
			}
		}

		/// <summary>
		/// Defines the counter.
		/// </summary>
		private PerformanceCounter _counterType;

		/// <summary>
		/// Gets the <see cref="System.Diagnostics.PerformanceCounter"/> Object with the specified parameters set.
		/// </summary>
		/// <value></value>
		protected override PerformanceCounter CounterType
		{
			get
			{
				if (_counterType == null)
				{
					_counterType = new PerformanceCounter();
				}

				return _counterType;
			}
		}

		#endregion

		#region Methods.Protected.Overriden

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGenericBase.TimerTick"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		protected override void OnTimerTick(EventArgs e)
		{
			try
			{
				this.Value = this.CounterType.NextValue();
			}
			catch
			{
				return;
			}

			_progressText.Text = Convert.ToString(Math.Round((this.Value / this.Divider), 2)) + " " + this.Prefix + this.CounterFormat;
			this.Refresh();

			base.OnTimerTick(e);
		}

		#endregion

		#region Methods.Private

		/// <summary>
		/// Sets the layout according to the orientation specified.
		/// </summary>
		/// <param name="orientation">Orientation of the <see cref="T:Genetibase.Meters.NuGenMeterBase"/> control.</param>
		private void SetLayout(NuGenOrientationStyle orientation)
		{
			this.SuspendLayout();
			this.Controls.Clear();

			/*
			 * meter
			 */

			Int32 meterWidth = _meter.Width;
			Int32 meterHeight = _meter.Height;

			if (_meter.Orientation == orientation)
			{
				this.Width = meterWidth;
				this.Height = meterHeight;
			}
			else
			{
				this.Height = meterWidth;
				this.Width = meterHeight;
			}

			_meter.Orientation = orientation;
			_meter.Top = 0;
			_meter.Left = 0;

			this.Controls.Add(_meter);

			/*
			 * progressText 
			 */

			if (_progressText.Visible)
			{
				if (orientation == NuGenOrientationStyle.Vertical)
				{
					_progressText.Height = this.ProgressTextEdgeIndent;
					this.Height += this.ProgressTextEdgeIndent;

					switch (this.ProgressTextPosition)
					{
						case NuGenProgressTextPositionStyle.Head:
						_progressText.Dock = DockStyle.Bottom;
						break;
						case NuGenProgressTextPositionStyle.Tail:
						_meter.Top += this.ProgressTextEdgeIndent;
						_progressText.Dock = DockStyle.Top;
						break;
					}
				}
				else if (orientation == NuGenOrientationStyle.Horizontal)
				{
					_progressText.Width = this.ProgressTextEdgeIndent;

					switch (this.ProgressTextPosition)
					{
						case NuGenProgressTextPositionStyle.Head:
						_progressText.Width = this.ProgressTextEdgeIndent;
						this.Width += this.ProgressTextEdgeIndent;
						_meter.Left += this.ProgressTextEdgeIndent;
						_progressText.Dock = DockStyle.Left;
						break;
						case NuGenProgressTextPositionStyle.Tail:
						_progressText.Width = this.ProgressTextEdgeIndent;
						this.Width += this.ProgressTextEdgeIndent;
						_progressText.Dock = DockStyle.Right;
						break;
					}
				}

				this.Controls.Add(_progressText);
			}

			/*
			 * captionText
			 */

			if (_captionText.Visible)
			{
				_captionText.Height = this.CaptionTextEdgeIndent;
				this.Height += this.CaptionTextEdgeIndent;

				switch (this.CaptionTextPosition)
				{
					case NuGenCaptionTextPositionStyle.Header:
					_meter.Top += this.CaptionTextEdgeIndent;
					_captionText.Dock = DockStyle.Top;
					break;
					case NuGenCaptionTextPositionStyle.Footer:
					_captionText.Dock = DockStyle.Bottom;
					break;
				}

				this.Controls.Add(_captionText);
			}

			this.ResumeLayout();
		}

		#endregion

		private System.ComponentModel.IContainer _components;
		private NuGenMeterBar _meter;
		private NuGenCTLabel _captionText;
		private NuGenCTLabel _progressText;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Genetibase.Meters.NuGenMeterBase"/> class.
		/// </summary>
		public NuGenMeter()
		{
			InitializeComponent();
			
			_meter.ReleaseTickLineTimeout = this.ReleaseTickLineTimeout;
			_captionText = new NuGenCTLabel();
			_progressText = new NuGenCTLabel();
			_progressText.Text = "0" + this.CounterFormat;
			
			this.CaptionText = this.CounterType.CounterName;
			this.SetLayout(NuGenOrientationStyle.Vertical);
			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.BackColor = Color.Transparent;
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
		[DebuggerStepThrough()]
		private void InitializeComponent()
		{
			_meter = new NuGenMeterBar();
			this.SuspendLayout();
			// 
			// meter
			// 
			_meter.BackGradientEndColor = System.Drawing.Color.Coral;
			_meter.BackGradientStartColor = System.Drawing.Color.Yellow;
			_meter.BackTubeGradientEndColor = System.Drawing.Color.Coral;
			_meter.BackTubeGradientStartColor = System.Drawing.Color.Yellow;
			_meter.BorderColor = System.Drawing.Color.Black;
			_meter.Dock = System.Windows.Forms.DockStyle.Fill;
			_meter.GradientEndColor = System.Drawing.Color.Red;
			_meter.GradientStartColor = System.Drawing.Color.Blue;
			_meter.Location = new System.Drawing.Point(0, 0);
			_meter.Name = "meter";
			_meter.Size = new System.Drawing.Size(40, 128);
			_meter.TabIndex = 4;
			_meter.TubeGradientEndColor = System.Drawing.Color.Red;
			_meter.TubeGradientStartColor = System.Drawing.Color.Blue;
			// 
			// NuGenMeterBase
			// 
			this.Controls.Add(_meter);
			this.Name = "NuGenMeterBase";
			this.Size = new System.Drawing.Size(40, 128);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
