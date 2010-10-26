/* -----------------------------------------------
 * NuGenGenericBase.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Meters.ComponentModel;
using Genetibase.Meters.Design;
using Genetibase.Shared.Design;
using Genetibase.Shared;
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

namespace Genetibase.Meters
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

		private static readonly Object _valueChanged = new Object();

		/// <summary>
		/// Occurs when the value on the counter changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ValueChanged")]
		public event NuGenTargetEventHandler ValueChanged
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
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGenericBase.ValueChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="Genetibase.Shared.NuGenTargetEventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough]
		protected virtual void OnValueChanged(NuGenTargetEventArgs e)
		{
			NuGenTargetEventHandler handler = (NuGenTargetEventHandler)this.Events[_valueChanged];

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
		[NuGenSRCategory("Category_PerformanceCounter")]
		[NuGenSRDescription("Description_CategoryName")]
		[TypeConverter(typeof(NuGenCategoryNameConverter))]
		public virtual String CategoryName
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

		private static readonly Object _categoryNameChanged = new Object();

		/// <summary>
		/// Occurs when the value of <see cref="P:Genetibase.Meters.NuGenGenericBase.CategoryName"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_CategoryNameChanged")]
		public event EventHandler CategoryNameChanged
		{
			add
			{
				this.Events.AddHandler(_categoryNameChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_categoryNameChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGenericBase.CategoryNameChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCategoryNameChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_categoryNameChanged, e);
		}

		/*
		 * CounterName
		 */

		/// <summary>
		/// Gets or sets the name of the counter.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PerformanceCounter")]
		[NuGenSRDescription("Description_CounterName")]
		[TypeConverter(typeof(NuGenCounterNameConverter))]
		public virtual String CounterName
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

		private static readonly Object _counterNameChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenGenericBase.CounterName"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_CounterNameChanged")]
		public event EventHandler CounterNameChanged
		{
			add
			{
				this.Events.AddHandler(_counterNameChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_counterNameChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGenericBase.CounterNameChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCounterNameChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_counterNameChanged, e);
		}

		/*
		 * CounterHelp
		 */

		/// <summary>
		/// Gets the description for this counter.
		/// </summary>
		[Browsable(false)]
		public virtual String CounterHelp
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
		/// Gets or sets the instance name for this counter.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PerformanceCounter")]
		[NuGenSRDescription("Description_InstanceName")]
		[TypeConverter(typeof(NuGenInstanceNameConverter))]
		public virtual String InstanceName
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

		private static readonly Object _instanceNameChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="Genetibase.Meters.NuGenGenericBase.InstanceName"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_InstanceNameChanged")]
		public event EventHandler InstanceNameChanged
		{
			add
			{
				this.Events.AddHandler(_instanceNameChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_instanceNameChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGenericBase.InstanceNameChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnInstanceNameChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_instanceNameChanged, e);
		}

		/*
		 * MachineName
		 */

		/// <summary>
		/// Gets or sets the computer name for this counter.
		/// </summary>
		[Browsable(false)]
		[NuGenSRCategory("Category_PerformanceCounter")]
		[NuGenSRDescription("Description_MachineName")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public virtual String MachineName
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

		private static readonly Object _machineNameChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenGenericBase.MachineName"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_MachineNameChanged")]
		public event EventHandler MachineNameChanged
		{
			add
			{
				this.Events.AddHandler(_machineNameChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_machineNameChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGenericBase.MachineNameChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnMachineNameChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_machineNameChanged, e);
		}

		#endregion

		#region Events.Behavior

		/*
		 * TimerTick
		 */

		private static readonly Object _timerTick = new Object();

		/// <summary>
		/// Occurs when the <see cref="E:System.Windows.Forms.Timer.Tick"/> event is fired.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_TimerTick")]
		public event EventHandler TimerTick
		{
			add
			{
				this.Events.AddHandler(_timerTick, value);
			}
			remove
			{
				this.Events.RemoveHandler(_timerTick, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGenericBase.TimerTick"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnTimerTick(EventArgs e)
		{
			this.InvokePropertyChanged(_timerTick, e);
		}

		#endregion

		#region Properties.Appearance

		/// <summary>
		/// Gets or sets the background color for the meter.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_BackgroundColor")]
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

		private static readonly Object _backgroundColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenGenericBase.BackgroundColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_BackgroundColorChanged")]
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
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGenericBase.BackgroundColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBackgroundColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_backgroundColorChanged, e);
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
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_BackgroundStyle")]
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

		private static readonly Object _backgroundStyleChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenGenericBase.BackgroundStyle"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChanged")]
		[NuGenSRDescription("Description_BackgroundStyleChanged")]
		public event EventHandler BackgroundStyleChanged
		{
			add
			{
				this.Events.AddHandler(_backgroundStyleChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_backgroundStyleChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGenericBase.BackgroundStyleChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		public virtual void OnBackgroundStyleChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_backgroundStyleChanged, e);
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
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_BackgroundTransparency")]
		[TypeConverter(typeof(NuGenTransparencyConverter))]
		public virtual Int32 BackgroundTransparency
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

		private static readonly Object _backgroundTransparencyChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenGenericBase.BackgroundTransparency"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_BackgroundTransparencyChanged")]
		public event EventHandler BackgroundTransparencyChanged
		{
			add
			{
				this.Events.AddHandler(_backgroundTransparencyChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_backgroundTransparencyChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGenericBase.BackgroundTransparencyChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBackgroundTransparencyChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_backgroundTransparencyChanged, e);
		}

		/*
		 * BorderColor
		 */

		/// <summary>
		/// Gets or sets the border color for the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(typeof(Color), "Black")]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_BorderColor")]
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

		private static readonly Object _borderColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenGenericBase.BorderColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_BorderColorChanged")]
		public event EventHandler BorderColorChanged
		{
			add
			{
				this.Events.AddHandler(_borderColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_borderColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGenericBase.BorderColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBorderColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_borderColorChanged, e);
		}

		/*
		 * BorderStyle
		 */

		/// <summary>
		/// Gets or sets the border style for the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(NuGenBorderStyle.Flat)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_BorderStyle")]
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

		private static readonly Object _borderStyleChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenGenericBase.BorderStyle"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_BorderStyleChanged")]
		public event EventHandler BorderStyleChanged
		{
			add
			{
				this.Events.AddHandler(_borderStyleChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_borderStyleChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGenericBase.BorderStyleChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBorderStyleChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_borderStyleChanged, e);
		}

		/*
		 * ForegroundColor
		 */

		/// <summary>
		/// Gets or sets the foreground color for the meter.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(typeof(Color), "Blue")]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_ForegroundColor")]
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

		private static readonly Object _foregroundColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenBarBase.ForegroundColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ForegroundColorChanged")]
		public event EventHandler ForegroundColorChanged
		{
			add
			{
				this.Events.AddHandler(_foregroundColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_foregroundColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGenericBase.ForegroundColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnForegroundColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_foregroundColorChanged, e);
		}

		/*
		 * ForegroundStyle
		 */

		/// <summary>
		/// Gets or sets the foreground style for the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(NuGenBackgroundStyle.Gradient)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_ForegroundStyle")]
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

		private static readonly Object _foregroundStyleChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGeGenericBase.ForegroundStyle"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ForegroundStyleChanged")]
		public event EventHandler ForegroundStyleChanged
		{
			add
			{
				this.Events.AddHandler(_foregroundStyleChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_foregroundStyleChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGenericBase.ForegroundStyleChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnForegroundStyleChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_foregroundStyleChanged, e);
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
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_ForegroundTransparency")]
		[TypeConverter(typeof(NuGenTransparencyConverter))]
		public virtual Int32 ForegroundTransparency
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

		private static readonly Object _foregroundTransparencyChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="E:Genetibase.Meters.NuGenGenericBase.ForegroundTransparency"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ForegroundTransparencyChanged")]
		public event EventHandler ForegroundTransparencyChanged
		{
			add
			{
				this.Events.AddHandler(_foregroundTransparencyChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_foregroundTransparencyChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGenericBase.ForegroundTransparencyChanged"/>
		/// event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnForegroundTransparencyChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_foregroundTransparencyChanged, e);
		}

		/*
		 * StretchImage
		 */

		/// <summary>
		/// Gets or sets the value indicating whether to stretch the background image.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_StretchImage")]
		public virtual Boolean StretchImage
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

		private static readonly Object _stretchImageChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenGenericBase.StretchImage"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_StretchImageChanged")]
		public event EventHandler StretchImageChanged
		{
			add
			{
				this.Events.AddHandler(_stretchImageChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_stretchImageChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGenericBase.StretchImageChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnStretchImageChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_stretchImageChanged, e);
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
		[NuGenSRCategory("Category_BackgroundGradient")]
		[NuGenSRDescription("Description_BackGradientStartColor")]
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

		private static readonly Object _backGradientStartColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of <see cref="P:Genetibase.Meters.NuGenGenericBase.BackGradientStartColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_BackGradientStartColorChanged")]
		public event EventHandler BackGradientStartColorChanged
		{
			add
			{
				this.Events.AddHandler(_backGradientStartColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_backGradientStartColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGenericBase.BackGradientStartColorChanged"/>
		/// event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBackGradientStartColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_backGradientStartColorChanged, e);
		}

		/*
		 * BackGradientEndColor
		 */

		/// <summary>
		/// Gets or sets the end color of the background gradient.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_BackgroundGradient")]
		[NuGenSRDescription("Description_BackGradientEndColor")]
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

		private static readonly Object _backGradientEndColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenGenericBase.BackGradientEndColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_BackGradientEndColorChanged")]
		public event EventHandler BackGradientEndColorChanged
		{
			add
			{
				this.Events.AddHandler(_backGradientEndColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_backGradientEndColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGenericBase.BackGradientEndColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBackGradientEndColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_backGradientEndColorChanged, e);
		}

		/*
		 * BackTubeGradientStartColor
		 */

		/// <summary>
		/// Gets or sets the start color of the back tube gradient.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_BackgroundGradient")]
		[NuGenSRDescription("Description_BackTubeGradientStartColor")]
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

		private static readonly Object _backTubeStartColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of <see cref="P:Genetibase.Meters.NuGenGenericBase.BackTubeGradientStartColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_BackTubeStartColorChanged")]
		public event EventHandler BackTubeStartColorChanged
		{
			add
			{
				this.Events.AddHandler(_backTubeStartColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_backTubeStartColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGenericBase.BackTubeStartColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBackTubeStartColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_backTubeStartColorChanged, e);
		}

		/*
		 * BackTubeGradientEndColor
		 */

		/// <summary>
		/// Gets or sets the end color of the back tube gradient.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_BackgroundGradient")]
		[NuGenSRDescription("Description_BackTubeGradientEndColor")]
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

		private static readonly Object _backTubeEndColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenGenericBase.BackTubeGradientEndColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_BackTubeEndColorChanged")]
		public event EventHandler BackTubeEndColorChanged
		{
			add
			{
				this.Events.AddHandler(_backTubeEndColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_backTubeEndColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGenericBase.BackTubeEndColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBackTubeEndColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_backTubeEndColorChanged, e);
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
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_Latency")]
		[DefaultValue(500)]
		public virtual Int32 Latency
		{
			get
			{
				return _refreshTimer.Interval;
			}
			set
			{
				_refreshTimer.Interval = value;
				this.OnLatencyChanged(EventArgs.Empty);

			}
		}

		private static readonly Object _latencyChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenGenericBase.Latency"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_LatencyChanged")]
		public event EventHandler LatencyChanged
		{
			add
			{
				this.Events.AddHandler(_latencyChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_latencyChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGenericBase.LatencyChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnLatencyChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_latencyChanged, e);
		}

		/*
		 * SwitchedOn
		 */

		/// <summary>
		/// Gets or sets the value indicating whether the meter is switched on.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_SwitchedOn")]
		[DefaultValue(true)]
		public virtual Boolean SwitchedOn
		{
			get
			{
				return _refreshTimer.Enabled;
			}
			set
			{
				_refreshTimer.Enabled = value;
				this.OnSwitchedOnChanged(EventArgs.Empty);
			}
		}

		private static readonly Object _switchedOnChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenGenericBase.SwitchedOn"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_SwitchedOnChanged")]
		public event EventHandler SwitchedOnChanged
		{
			add
			{
				this.Events.AddHandler(_switchedOnChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_switchedOnChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGenericBase.SwitchedOnChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnSwitchedOnChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_switchedOnChanged, e);
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
		[NuGenSRCategory("Category_ForegroundGradient")]
		[NuGenSRDescription("Description_GradientStartColor")]
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

		private static readonly Object _gradientStartColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenGenericBase.GradientStartColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_GradientStartColorChanged")]
		public event EventHandler GradientStartColorChanged
		{
			add
			{
				this.Events.AddHandler(_gradientStartColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_gradientStartColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGenericBase.GradientStartColorChanged"/>
		/// event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnGradientStartColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_gradientStartColorChanged, e);
		}

		/*
		 * GradientEndColor
		 */

		/// <summary>
		/// Gets or sets the end color of the gradient.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_ForegroundGradient")]
		[NuGenSRDescription("Description_GradientEndColor")]
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

		private static readonly Object _gradientEndColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenGenericBase.GradientEndColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_GradientEndColorChanged")]
		public event EventHandler GradientEndColorChanged
		{
			add
			{
				this.Events.AddHandler(_gradientEndColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_gradientEndColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGenericBase.NuGenGradientEndColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnGradientEndColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_gradientEndColorChanged, e);
		}

		/*
		 * TubeGradientStartColor
		 */

		/// <summary>
		/// Gets or sets the start color the tube gradient.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_ForegroundGradient")]
		[NuGenSRDescription("Description_TubeGradientStartColor")]
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

		private static readonly Object _tubeStartColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenGenericBase.TubeStartColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_TubeStartColorChanged")]
		public event EventHandler TubeStartColorChanged
		{
			add
			{
				this.Events.AddHandler(_tubeStartColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_tubeStartColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGenericBase.TubeStartColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnTubeStartColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_tubeStartColorChanged, e);
		}

		/*
		 * TubeGradientEndColor
		 */

		/// <summary>
		/// Gets or sets the end color of the tube gradient.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_ForegroundGradient")]
		[NuGenSRDescription("Description_TubeGradientEndColor")]
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

		private static readonly Object _tubeEndColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of <see cref="P:Genetibase.Meters.NuGenGenericBase.TubeGradientEndColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_TubeEndColorChanged")]
		public event EventHandler TubeEndColorChanged
		{
			add
			{
				this.Events.AddHandler(_tubeEndColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_tubeEndColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGenericBase.TubeEndColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnTubeEndColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_tubeEndColorChanged, e);
		}

		#endregion

		#region Properties.PerformanceCounter

		/*
		 * CounterFormat
		 */

		/// <summary>
		/// Determines the format for the counter, i.e. "%".
		/// </summary>
		private String _counterFormat = "%";

		/// <summary>
		/// Gets or sets the format for the counter.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PerformanceCounter")]
		[NuGenSRDescription("Description_CounterFormat")]
		public virtual String CounterFormat
		{
			get
			{
				return _counterFormat;
			}
			set
			{
				if (_counterFormat != value)
				{
					_counterFormat = value;
					this.OnCounterFormatChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly Object _counterFormatChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenGenericBase.CounterFormat"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_CounterFormatChanged")]
		public event EventHandler CounterFormatChanged
		{
			add
			{
				this.Events.AddHandler(_counterFormatChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_counterFormatChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenGenericBase.CounterFormatChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnCounterFormatChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_counterFormatChanged, e);
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
		/// Gets the <see cref="T:Genetibase.Meters.NuGenBarBase"/> Object with the specified parameters set.
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
		/// Gets the <see cref="T:System.Diagnostics.PerformanceCounter"/> Object with the specified parameters set.
		/// </summary>
		protected virtual PerformanceCounter CounterType
		{
			get
			{
				return _counter;
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
			_refreshTimer.Enabled = this.Enabled;
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
		protected virtual void InvokePropertyChanged(Object key, EventArgs e)
		{
			EventHandler handler = (EventHandler)this.Events[key];

			if (handler != null)
			{
				handler(this, e);
			}
		}

		#endregion

		#region EventHandlers

		private void Bar_KeyDown(Object sender, KeyEventArgs e)
		{
			this.OnKeyDown(e);
		}

		private void Bar_KeyPress(Object sender, KeyPressEventArgs e)
		{
			this.OnKeyPress(e);
		}

		private void Bar_KeyUp(Object sender, KeyEventArgs e)
		{
			this.OnKeyUp(e);
		}

		private void refreshTimer_Tick(Object sender, EventArgs e)
		{
			this.OnTimerTick(EventArgs.Empty);
		}

		#endregion

		private Container _components;
		private Timer _refreshTimer;
		private PerformanceCounter _counter;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenGenericBase"/> class.
		/// </summary>
		public NuGenGenericBase()
		{
			_components = new Container();

			_counter = new PerformanceCounter();
			_counter.MachineName = Dns.GetHostName();
			_components.Add(_counter);

			_refreshTimer = new Timer();
			_refreshTimer.Enabled = true;
			_refreshTimer.Interval = 500;
			_refreshTimer.Tick += new EventHandler(refreshTimer_Tick);
			_components.Add(_refreshTimer);
			
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.UserMouse, false);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.BackColor = Color.Transparent;
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(Boolean disposing)
		{
			if (disposing)
			{
				if (_components != null && _components.Components != null)
				{
					foreach (IComponent component in _components.Components)
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
	}
}
