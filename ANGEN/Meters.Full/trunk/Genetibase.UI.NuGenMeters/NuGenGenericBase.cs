/* -----------------------------------------------
 * NuGenGenericBase.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using pc = Genetibase.PerformanceCounters.Processor;
using en = Genetibase.PerformanceCounters.NuGenProcessorCounter;
using win = Genetibase.WinApi.WinUser;

using Genetibase.Shared.Design;
using Genetibase.Shared;
using Genetibase.UI.NuGenMeters.ComponentModel;
using Genetibase.UI.NuGenMeters.Design;
using Genetibase.WinApi;

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.UI.NuGenMeters
{
	/// <summary>
	/// A generic base for meter and graph controls.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[DefaultEvent("Click")]
	[DefaultProperty("BackgroundStyle")]
	[Designer(typeof(NuGenGenericBaseDesigner))]
	[ToolboxBitmap(typeof(NuGenGenericBase), "Toolbox.NuGen.bmp")]
	[ToolboxItem(false)]
	public class NuGenGenericBase : UserControl, INuGenCounter
	{
		#region INuGenCounter Members

		/*
		 * ValueChanged
		 */

		private static readonly object EventValueChanged = new object();

		/// <summary>
		/// Occurs when the value on the counter changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("ValueChangedDescription")]
		public event NuGenTargetEventHandler ValueChanged
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
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGenericBase.ValueChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="Genetibase.Shared.NuGenTargetEventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough]
		protected virtual void OnValueChanged(NuGenTargetEventArgs e)
		{
			NuGenTargetEventHandler handler = (NuGenTargetEventHandler)this.Events[EventValueChanged];

			if (handler != null)
			{
				handler(this, e);
			}
		}
		
		/*
		 * NextValue
		 */

		/// <summary>
		/// Obtains a counter sample and returns the calculated value for it.
		/// </summary>
		/// <returns>
		/// The next calculated value that the system obtains for this counter.
		/// </returns>
		public virtual float NextValue()
		{
			return this.CounterType.NextValue();
		}

		/*
		 * CategoryName
		 */

		/// <summary>
		/// Gets or sets the name of the counter category for this counter.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PerformanceCounterCategory")]
		[NuGenSRDescription("CategoryNameDescription")]
		[TypeConverter(typeof(NuGenCategoryNameConverter))]
		public virtual string CategoryName
		{
			get
			{
				return this.CounterType.CategoryName;
			}
			set
			{
				this.CounterType.CategoryName = value;
				this.OnCategoryNameChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventCategoryNameChanged = new object();

		/// <summary>
		/// Occurs when the value of CategoryName property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("CategoryNameChangedDescription")]
		public event EventHandler CategoryNameChanged
		{
			add
			{
				this.Events.AddHandler(EventCategoryNameChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventCategoryNameChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGenericBase.CategoryNameChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCategoryNameChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventCategoryNameChanged, e);
		}

		/*
		 * CounterName
		 */

		/// <summary>
		/// Gets or sets the name of the counter.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PerformanceCounterCategory")]
		[NuGenSRDescription("CounterNameDescription")]
		[TypeConverter(typeof(NuGenCounterNameConverter))]
		public virtual string CounterName
		{
			get
			{
				return this.CounterType.CounterName;
			}
			set
			{
				try
				{
					this.CounterType.CounterName = value;
				}
				catch
				{
					throw new ArgumentException(Properties.Resources.CounterExistException);
				}

				this.OnCounterNameChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventCounterNameChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenGenericBase.CounterName"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("CounterNameChangedDescription")]
		public event EventHandler CounterNameChanged
		{
			add
			{
				this.Events.AddHandler(EventCounterNameChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventCounterNameChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGenericBase.CounterNameChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCounterNameChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventCounterNameChanged, e);
		}

		/*
		 * CounterHelp
		 */

		/// <summary>
		/// Gets the description for this counter.
		/// </summary>
		[Browsable(false)]
		public virtual string CounterHelp
		{
			get
			{
				return this.CounterType.CounterHelp;
			}
		}

		/*
		 * InstanceName
		 */

		/// <summary>
		/// Gets or sets an instance name for this counter.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PerformanceCounterCategory")]
		[NuGenSRDescription("InstanceNameDescription")]
		[TypeConverter(typeof(NuGenInstanceNameConverter))]
		public virtual string InstanceName
		{
			get
			{
				return this.CounterType.InstanceName;
			}
			set
			{
				this.CounterType.InstanceName = value;
				this.OnInstanceNameChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventInstanceNameChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Genetibase.UI.NuGenMeters.NuGenGenericBase.InstanceName"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("InstanceNameChangedDescription")]
		public event EventHandler InstanceNameChanged
		{
			add
			{
				this.Events.AddHandler(EventInstanceNameChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventInstanceNameChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGenericBase.InstanceNameChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnInstanceNameChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventInstanceNameChanged, e);
		}

		/*
		 * MachineName
		 */

		/// <summary>
		/// Gets or sets the computer name for this counter.
		/// </summary>
		[Browsable(false)]
		[NuGenSRCategory("PerformanceCounterCategory")]
		[NuGenSRDescription("MachineNameDescription")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual string MachineName
		{
			get
			{
				return this.CounterType.MachineName;
			}
			set
			{
				this.CounterType.MachineName = value;
				this.OnMachineNameChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventMachineNameChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenGenericBase.MachineName"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("MachineNameChangedDescription")]
		public event EventHandler MachineNameChanged
		{
			add
			{
				this.Events.AddHandler(EventMachineNameChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventMachineNameChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGenericBase.MachineNameChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnMachineNameChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventMachineNameChanged, e);
		}

		#endregion

		#region Declarations

		private Container components = null;
		private Timer refreshTimer = null;
		private PerformanceCounter counter = null;

		#endregion

		#region Events.Behavior

		/*
		 * TimerTick
		 */

		private static readonly object EventTimerTick = new object();

		/// <summary>
		/// Occurs when the <see cref="E:System.Windows.Forms.Timer.Tick"/> event is fired.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("BehaviorCategory")]
		[NuGenSRDescription("TimerTickDescription")]
		public event EventHandler TimerTick
		{
			add
			{
				this.Events.AddHandler(EventTimerTick, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventTimerTick, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGenericBase.TimerTick"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnTimerTick(EventArgs e)
		{
			this.InvokePropertyChanged(EventTimerTick, e);
		}

		#endregion

		#region Properties.Appearance

		/// <summary>
		/// Gets or sets the background color for the meter.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("AppearanceCategory")]
		[NuGenSRDescription("BackgroundColorDescription")]
		public virtual Color BackgroundColor
		{
			get
			{
				return this.Bar.BackgroundColor;
			}
			set
			{
				this.Bar.BackgroundColor = value;
				this.OnBackgroundColorChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventBackgroundColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenGenericBase.BackgroundColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("BackgroundColorChangedDescription")]
		public event EventHandler BackgroundColorChanged
		{
			add
			{
				this.Events.AddHandler(this, value);
			}
			remove
			{
				this.Events.RemoveHandler(this, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGenericBase.BackgroundColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBackgroundColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventBackgroundColorChanged, e);
		}

		/*
		 * BackgroundImage
		 */

		/// <summary>
		/// Gets or sets the background image displayed in the control.
		/// </summary>
		/// <value></value>
		[Browsable(true)]
		public virtual new Image BackgroundImage
		{
			get
			{
				return this.Bar.BackgroundImage;
			}
			set
			{
				this.Bar.BackgroundImage = value;
				this.OnBackgroundImageChanged(EventArgs.Empty);
			}
		}

		/*
		 * BackgroundStyle
		 */

		/// <summary>
		/// Gets or sets the background style for the control.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("AppearanceCategory")]
		[NuGenSRDescription("BackgroundStyleDescription")]
		public virtual NuGenBackgroundStyle BackgroundStyle
		{
			get
			{
				return this.Bar.BackgroundStyle;
			}
			set
			{
				this.Bar.BackgroundStyle = value;
				this.OnBackgroundStyleChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventBackgroundStyleChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenGenericBase.BackgroundStyle"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChanged")]
		[NuGenSRDescription("BackgroundStyleChangedDescription")]
		public event EventHandler BackgroundStyleChanged
		{
			add
			{
				this.Events.AddHandler(EventBackgroundStyleChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventBackgroundStyleChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGenericBase.BackgroundStyleChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		public virtual void OnBackgroundStyleChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventBackgroundStyleChanged, e);
		}

		/*
		 * BackgroundTransparency
		 */

		/// <summary>
		/// Gets or sets the background transparency level for the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(0)]
		[Editor(typeof(NuGenTransparencyEditor), typeof(UITypeEditor))]
		[NuGenSRCategory("AppearanceCategory")]
		[NuGenSRDescription("BackgroundTransparencyDescription")]
		[TypeConverter(typeof(NuGenTransparencyConverter))]
		public virtual int BackgroundTransparency
		{
			get
			{
				return this.Bar.BackgroundTransparency;
			}
			set
			{
				this.Bar.BackgroundTransparency = value;
				this.OnBackgroundTransparencyChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventBackgroundTransparencyChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenGenericBase.BackgroundTransparency"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("BackgroundTransparencyChangedDescription")]
		public event EventHandler BackgroundTransparencyChanged
		{
			add
			{
				this.Events.AddHandler(EventBackgroundTransparencyChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventBackgroundTransparencyChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGenericBase.BackgroundTransparencyChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBackgroundTransparencyChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventBackgroundTransparencyChanged, e);
		}

		/*
		 * BorderColor
		 */

		/// <summary>
		/// Gets or sets the border color for the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(typeof(Color), "Black")]
		[NuGenSRCategory("AppearanceCategory")]
		[NuGenSRDescription("BorderColorDescription")]
		public virtual Color BorderColor
		{
			get
			{
				return this.Bar.BorderColor;
			}
			set
			{
				this.Bar.BorderColor = value;
				this.OnBorderColorChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventBorderColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenGenericBase.BorderColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedDescription")]
		[NuGenSRDescription("BorderColorChangedDescription")]
		public event EventHandler BorderColorChanged
		{
			add
			{
				this.Events.AddHandler(EventBorderColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventBorderColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGenericBase.BorderColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBorderColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventBorderColorChanged, e);
		}

		/*
		 * BorderStyle
		 */

		/// <summary>
		/// Gets or sets the border style for the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(NuGenBorderStyle.Flat)]
		[NuGenSRCategory("AppearanceCategory")]
		[NuGenSRDescription("BorderStyleDescription")]
		public new virtual NuGenBorderStyle BorderStyle
		{
			get
			{
				return this.Bar.BorderStyle;
			}
			set
			{
				this.Bar.BorderStyle = value;
				this.OnBorderStyleChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventBorderStyleChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenGenericBase.BorderStyle"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("BorderStyleChangedDescription")]
		public event EventHandler BorderStyleChanged
		{
			add
			{
				this.Events.AddHandler(EventBorderStyleChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventBorderStyleChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGenericBase.BorderStyleChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBorderStyleChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventBorderStyleChanged, e);
		}

		/*
		 * ForegroundColor
		 */

		/// <summary>
		/// Gets or sets the foreground color for the meter.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(typeof(Color), "Blue")]
		[NuGenSRCategory("AppearanceCategory")]
		[NuGenSRDescription("ForegroundColorDescription")]
		public virtual Color ForegroundColor
		{
			get
			{
				return this.Bar.ForegroundColor;
			}
			set
			{
				this.Bar.ForegroundColor = value;
				this.OnForegroundColorChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventForegroundColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenBarBase.ForegroundColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("ForegroundColorChangedDescription")]
		public event EventHandler ForegroundColorChanged
		{
			add
			{
				this.Events.AddHandler(EventForegroundColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventForegroundColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGenericBase.ForegroundColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnForegroundColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventForegroundColorChanged, e);
		}

		/*
		 * ForegroundStyle
		 */

		/// <summary>
		/// Gets or sets the foreground style for the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(NuGenBackgroundStyle.Gradient)]
		[NuGenSRCategory("AppearanceCategory")]
		[NuGenSRDescription("ForegroundStyleDescription")]
		public virtual NuGenBackgroundStyle ForegroundStyle
		{
			get
			{
				return this.Bar.ForegroundStyle;
			}
			set
			{
				this.Bar.ForegroundStyle = value;
				this.OnForegroundStyleChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventForegroundStyleChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGeGenericBase.ForegroundStyle"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("ForegroundStyleChangedDescription")]
		public event EventHandler ForegroundStyleChanged
		{
			add
			{
				this.Events.AddHandler(EventForegroundStyleChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventForegroundStyleChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGenericBase.ForegroundStyleChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnForegroundStyleChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventForegroundStyleChanged, e);
		}

		/*
		 * ForegroundTransparency
		 */

		/// <summary>
		/// Gets or sets the foreground transparency level for the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(0)]
		[Editor(typeof(NuGenTransparencyEditor), typeof(UITypeEditor))]
		[NuGenSRCategory("AppearanceCategory")]
		[NuGenSRDescription("ForegroundTransparencyDescription")]
		[TypeConverter(typeof(NuGenTransparencyConverter))]
		public virtual int ForegroundTransparency
		{
			get
			{
				return this.Bar.ForegroundTransparency;
			}
			set
			{
				this.Bar.ForegroundTransparency = value;
				this.OnForegroundTransparencyChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventForegroundTransparencyChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGenericBase.ForegroundTransparency"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("ForegroundTransparencyChangedDescription")]
		public event EventHandler ForegroundTransparencyChanged
		{
			add
			{
				this.Events.AddHandler(EventForegroundTransparencyChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventForegroundTransparencyChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGenericBase.ForegroundTransparencyChanged"/>
		/// event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnForegroundTransparencyChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventForegroundTransparencyChanged, e);
		}

		/*
		 * StretchImage
		 */

		/// <summary>
		/// Gets or sets the value indicating whether to stretch the background image.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		[NuGenSRCategory("AppearanceCategory")]
		[NuGenSRDescription("StretchImageDescription")]
		public virtual bool StretchImage
		{
			get
			{
				return this.Bar.StretchImage;
			}
			set
			{
				this.Bar.StretchImage = value;
				this.OnStretchImageChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventStretchImageChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenGenericBase.StretchImage"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("StretchImageChangedDescription")]
		public event EventHandler StretchImageChanged
		{
			add
			{
				this.Events.AddHandler(EventStretchImageChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventStretchImageChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGenericBase.StretchImageChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnStretchImageChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventStretchImageChanged, e);
		}

		#endregion

		#region Properties.BackgroundGradient

		/*
		 * BackGradientStartColor
		 */

		/// <summary>
		/// Gets or sets the start color of the background gradient.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("BackgroundGradientCategory")]
		[NuGenSRDescription("BackGradientStartColorDescription")]
		[DefaultValue(typeof(Color), "Yellow")]
		public virtual Color BackGradientStartColor
		{
			get
			{
				return this.Bar.BackGradientStartColor;
			}
			set
			{
				this.Bar.BackGradientStartColor = value;
				this.OnBackGradientStartColorChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventBackGradientStartColorChanged = new object();

		/// <summary>
		/// Occurs when the value of <see cref="P:Genetibase.UI.NuGenMeters.NuGenGenericBase.BackGradientStartColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("BackGradientStartColorChangedDescription")]
		public event EventHandler BackGradientStartColorChanged
		{
			add
			{
				this.Events.AddHandler(EventBackGradientStartColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventBackGradientStartColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGenericBase.BackGradientStartColorChanged"/>
		/// event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBackGradientStartColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventBackGradientStartColorChanged, e);
		}

		/*
		 * BackGradientEndColor
		 */

		/// <summary>
		/// Gets or sets the end color of the background gradient.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("BackgroundGradientCategory")]
		[NuGenSRDescription("BackGradientEndColorDescription")]
		[DefaultValue(typeof(Color), "Coral")]
		public virtual Color BackGradientEndColor
		{
			get
			{
				return this.Bar.BackGradientEndColor;
			}
			set
			{
				this.Bar.BackGradientEndColor = value;
				this.OnBackGradientEndColorChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventBackGradientEndColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenGenericBase.BackGradientEndColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("BackGradientEndColorChangedDescription")]
		public event EventHandler BackGradientEndColorChanged
		{
			add
			{
				this.Events.AddHandler(EventBackGradientEndColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventBackGradientEndColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGenericBase.BackGradientEndColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBackGradientEndColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventBackGradientEndColorChanged, e);
		}

		/*
		 * BackTubeGradientStartColor
		 */

		/// <summary>
		/// Gets or sets the start color of the back tube gradient.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("BackgroundGradientCategory")]
		[NuGenSRDescription("BackTubeGradientStartColorDescription")]
		[DefaultValue(typeof(Color), "Yellow")]
		public virtual Color BackTubeGradientStartColor
		{
			get
			{
				return this.Bar.BackTubeGradientStartColor;
			}
			set
			{
				this.Bar.BackTubeGradientStartColor = value;
				this.OnBackTubeStartColorChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventBackTubeStartColorChanged = new object();

		/// <summary>
		/// Occurs when the value of <see cref="P:Genetibase.UI.NuGenMeters.NuGenGenericBase.BackTubeGradientStartColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("BackTubeStartColorChangedDescription")]
		public event EventHandler BackTubeStartColorChanged
		{
			add
			{
				this.Events.AddHandler(EventBackTubeStartColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventBackTubeStartColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGenericBase.BackTubeStartColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBackTubeStartColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventBackTubeStartColorChanged, e);
		}

		/*
		 * BackTubeGradientEndColor
		 */

		/// <summary>
		/// Gets or sets the end color of the back tube gradient.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("BackgroundGradientCategory")]
		[NuGenSRDescription("BackTubeGradientEndColorDescription")]
		[DefaultValue(typeof(Color), "Coral")]
		public virtual Color BackTubeGradientEndColor
		{
			get
			{
				return this.Bar.BackTubeGradientEndColor;
			}
			set
			{
				this.Bar.BackTubeGradientEndColor = value;
				this.OnBackTubeEndColorChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventBackTubeEndColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenGenericBase.BackTubeGradientEndColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("BackTubeEndColorChangedDescription")]
		public event EventHandler BackTubeEndColorChanged
		{
			add
			{
				this.Events.AddHandler(EventBackTubeEndColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventBackTubeEndColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGenericBase.BackTubeEndColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBackTubeEndColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventBackTubeEndColorChanged, e);
		}

		#endregion

		#region Properties.Behavior

		/*
		 * Latency
		 */

		/// <summary>
		/// Gets or sets the latency for the counter.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("BehaviorCategory")]
		[NuGenSRDescription("LatencyDescription")]
		[DefaultValue(500)]
		public virtual int Latency
		{
			get
			{
				return this.refreshTimer.Interval;
			}
			set
			{
				this.refreshTimer.Interval = value;
				this.OnLatencyChanged(EventArgs.Empty);

			}
		}

		private static readonly object EventLatencyChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenGenericBase.Latency"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("LatencyChangedDescription")]
		public event EventHandler LatencyChanged
		{
			add
			{
				this.Events.AddHandler(EventLatencyChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventLatencyChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGenericBase.LatencyChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnLatencyChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventLatencyChanged, e);
		}

		/*
		 * SwitchedOn
		 */

		/// <summary>
		/// Gets or sets the value indicating whether the meter is switched on.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("BehaviorCategory")]
		[NuGenSRDescription("SwitchedOnDescription")]
		[DefaultValue(true)]
		public virtual bool SwitchedOn
		{
			get
			{
				return this.refreshTimer.Enabled;
			}
			set
			{
				this.refreshTimer.Enabled = value;
				this.OnSwitchedOnChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventSwitchedOnChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenGenericBase.SwitchedOn"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("SwitchedOnChangedDescription")]
		public event EventHandler SwitchedOnChanged
		{
			add
			{
				this.Events.AddHandler(EventSwitchedOnChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventSwitchedOnChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGenericBase.SwitchedOnChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnSwitchedOnChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventSwitchedOnChanged, e);
		}

		#endregion

		#region Properties.ForegroundGradient

		/*
		 * GradientStartColor
		 */

		/// <summary>
		/// Gets or sets the start color of the gradient.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("ForegroundCategoryCategory")]
		[NuGenSRDescription("GradientStartColorDescription")]
		[DefaultValue(typeof(Color), "Blue")]
		public virtual Color GradientStartColor
		{
			get
			{
				return this.Bar.GradientStartColor;
			}
			set
			{
				this.Bar.GradientStartColor = value;
				this.OnGradientStartColorChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventGradientStartColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenGenericBase.GradientStartColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("GradientStartColorChangedDescription")]
		public event EventHandler GradientStartColorChanged
		{
			add
			{
				this.Events.AddHandler(EventGradientStartColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventGradientStartColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGenericBase.GradientStartColorChanged"/>
		/// event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnGradientStartColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventGradientStartColorChanged, e);
		}

		/*
		 * GradientEndColor
		 */

		/// <summary>
		/// Gets or sets the end color of the gradient.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("ForegroundGradientCategory")]
		[NuGenSRDescription("GradientEndColorDescription")]
		[DefaultValue(typeof(Color), "Red")]
		public virtual Color GradientEndColor
		{
			get
			{
				return this.Bar.GradientEndColor;
			}
			set
			{
				this.Bar.GradientEndColor = value;
				this.OnGradientEndColorChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventGradientEndColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenGenericBase.GradientEndColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("GradientEndColorChangedDescription")]
		public event EventHandler GradientEndColorChanged
		{
			add
			{
				this.Events.AddHandler(EventGradientEndColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventGradientEndColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGenericBase.NuGenGradientEndColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnGradientEndColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventGradientEndColorChanged, e);
		}

		/*
		 * TubeGradientStartColor
		 */

		/// <summary>
		/// Gets or sets the start color the tube gradient.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("ForegroundGradientCategory")]
		[NuGenSRDescription("TubeGradientStartColorDescription")]
		[DefaultValue(typeof(Color), "Blue")]
		public virtual Color TubeGradientStartColor
		{
			get
			{
				return this.Bar.TubeGradientStartColor;
			}
			set
			{
				this.Bar.TubeGradientStartColor = value;
				this.OnTubeStartColorChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventTubeStartColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenGenericBase.TubeStartColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("TubeStartColorChangedDescription")]
		public event EventHandler TubeStartColorChanged
		{
			add
			{
				this.Events.AddHandler(EventTubeStartColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventTubeStartColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGenericBase.TubeStartColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnTubeStartColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventTubeStartColorChanged, e);
		}

		/*
		 * TubeGradientEndColor
		 */

		/// <summary>
		/// Gets or sets the end color of the tube gradient.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("ForegroundGradientCategory")]
		[NuGenSRDescription("TubeGradientEndColorDescription")]
		[DefaultValue(typeof(Color), "Red")]
		public virtual Color TubeGradientEndColor
		{
			get
			{
				return this.Bar.TubeGradientEndColor;
			}
			set
			{
				this.Bar.TubeGradientEndColor = value;
				this.OnTubeEndColorChanged(EventArgs.Empty);
			}
		}

		private static readonly object EventTubeEndColorChanged = new object();

		/// <summary>
		/// Occurs when the value of <see cref="P:Genetibase.UI.NuGenMeters.NuGenGenericBase.TubeGradientEndColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("TubeEndColorChangedDescription")]
		public event EventHandler TubeEndColorChanged
		{
			add
			{
				this.Events.AddHandler(EventTubeEndColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventTubeEndColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGenericBase.TubeEndColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnTubeEndColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventTubeEndColorChanged, e);
		}

		#endregion

		#region Properties.PerformanceCounter

		/*
		 * CounterFormat
		 */

		/// <summary>
		/// Determines the format for the counter, i.e. "%".
		/// </summary>
		private string counterFormat = "%";

		/// <summary>
		/// Gets or sets the format for the counter.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PerformanceCounterCategory")]
		[NuGenSRDescription("CounterFormatDescription")]
		public virtual string CounterFormat
		{
			get
			{
				return this.counterFormat;
			}
			set
			{
				if (this.counterFormat != value)
				{
					this.counterFormat = value;
					this.OnCounterFormatChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object EventCounterFormatChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenGenericBase.CounterFormat"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("CounterFormatChangedDescription")]
		public event EventHandler CounterFormatChanged
		{
			add
			{
				this.Events.AddHandler(EventCounterFormatChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventCounterFormatChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenGenericBase.CounterFormatChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCounterFormatChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventCounterFormatChanged, e);
		}

		#endregion

		#region Properties.Hidden

		/// <summary>
		/// Gets the collection of controls contained within the control.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Windows.Forms.Control.ControlCollection"></see> representing the collection of controls contained within the control.</returns>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ControlCollection Controls
		{
			get
			{
				return base.Controls;
			}
		}

		#endregion

		#region Properties.Protected

		/*
		 * Bar
		 */

		/// <summary>
		/// Gets the <see cref="T:Genetibase.UI.NuGenMeters.NuGenBarBase"/> object with the specified parameters set.
		/// </summary>
		protected virtual NuGenBarBase Bar
		{
			get
			{
				return new NuGenBarBase();
			}
		}

		/*
		 * CounterType
		 */

		/// <summary>
		/// Gets the <see cref="T:System.Diagnostics.PerformanceCounter"/> object with the specified parameters set.
		/// </summary>
		protected virtual PerformanceCounter CounterType
		{
			get
			{
				return this.counter;
			}
		}

		#endregion

		#region Methods.Public.Virtual

		/*
		 * Reset
		 */

		/// <summary>
		/// Resets the range of the meter to its default value.
		/// </summary>
		public virtual void Reset()
		{
			this.Bar.ResetRange();
		}

		#endregion

		#region Methods.Protected.Overriden

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.EnabledChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			this.refreshTimer.Enabled = this.Enabled;
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.UserControl.Load"/> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
		protected override void OnLoad(EventArgs e)
		{
			this.Bar.KeyDown += new KeyEventHandler(Bar_KeyDown);
			this.Bar.KeyPress += new KeyPressEventHandler(Bar_KeyPress);
			this.Bar.KeyUp += new KeyEventHandler(Bar_KeyUp);
		}

		#endregion

		#region Methods.Protected.Virtual

		/// <summary>
		/// Invokes event handlers specified by the <paramref name="key"/>.
		/// <param name="key">Specifies the event handlers to invoke.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		/// </summary>
		protected virtual void InvokePropertyChanged(object key, EventArgs e)
		{
			EventHandler handler = (EventHandler)this.Events[key];

			if (handler != null)
			{
				handler(this, e);
			}
		}

		#endregion

		#region EventHandlers

		private void Bar_KeyDown(object sender, KeyEventArgs e)
		{
			this.OnKeyDown(e);
		}

		private void Bar_KeyPress(object sender, KeyPressEventArgs e)
		{
			this.OnKeyPress(e);
		}

		private void Bar_KeyUp(object sender, KeyEventArgs e)
		{
			this.OnKeyUp(e);
		}

		private void refreshTimer_Tick(object sender, EventArgs e)
		{
			this.OnTimerTick(EventArgs.Empty);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenGenericBase"/> class.
		/// </summary>
		public NuGenGenericBase()
		{
			this.components = new Container();
			//
			// counter
			//
			this.counter = pc.GetCounter(en.PercentProcessorTime);
			this.counter.MachineName = Dns.GetHostName();
			this.components.Add(this.counter);
			//
			// refreshTimer
			//
			this.refreshTimer = new Timer();
			this.refreshTimer.Enabled = true;
			this.refreshTimer.Interval = 500;
			this.refreshTimer.Tick += new EventHandler(refreshTimer_Tick);
			this.components.Add(this.refreshTimer);
			//
			// nuGenGenericBase
			//
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.UserMouse, false);
			this.SetStyle(ControlStyles.UserPaint, true);
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
				if (this.components != null && this.components.Components != null)
				{
					foreach (IComponent component in this.components.Components)
					{
						if (component != null)
						{
							component.Dispose();
						}
					}
				}
			}

			base.Dispose(disposing);
		}

		#endregion
	}
}
