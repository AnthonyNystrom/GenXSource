/* -----------------------------------------------
 * NuGenBarBase.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using win = Genetibase.WinApi.WinUser;

using Genetibase.Shared.Design;
using Genetibase.Shared;
using Genetibase.Shared.Drawing;
using Genetibase.UI.NuGenMeters.ComponentModel;
using Genetibase.UI.NuGenMeters.Design;

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
	/// Implements base meter and graph bar functionality.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[Designer(typeof(NuGenBarBaseDesigner))]
	[ToolboxItem(false)]
	public class NuGenBarBase : UserControl
	{
		#region Declarations.Consts

		private const float INITIAL_MIN = 0.0f;
		private const float INITIAL_MAX = 100.0f;

		#endregion

		#region Properties.Appearance

		/*
		 * BackgroundColor
		 */

		/// <summary>
		/// Determines the background color for the control.
		/// </summary>
		private Color backgroundColor = Color.Yellow;

		/// <summary>
		/// Gets or sets the background color for the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(typeof(Color), "Yellow")]
		[NuGenSRCategory("AppearanceCategory")]
		[NuGenSRDescription("BackgroundColorDescription")]
		public virtual Color BackgroundColor
		{
			get
			{
				return this.backgroundColor;
			}
			set
			{
				if (this.backgroundColor != value)
				{
					this.backgroundColor = value;
					this.OnBackgroundColorChanged(EventArgs.Empty);
					this.BackColor = Color.FromArgb(NuGenControlPaint.GetAlphaChannel(this.BackgroundTransparency), value);
					this.Refresh();
				}
			}
		}

		private static readonly object EventBackgroundColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenBarBase.BackgroundColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("BackgroundColorChangedDescription")]
		public event EventHandler BackgroundColorChanged
		{
			add
			{
				this.Events.AddHandler(EventBackgroundColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventBackgroundColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.BackgroundColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough]
		protected virtual void OnBackgroundColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventBackgroundColorChanged, e);
		}

		/*
		 * BackgroundStyle
		 */

		/// <summary>
		/// Determines the style of the background.
		/// </summary>
		private NuGenBackgroundStyle backgroundStyle = NuGenBackgroundStyle.Gradient;

		/// <summary>
		/// Gets or sets the background style for the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(NuGenBackgroundStyle.Gradient)]
		[NuGenSRCategory("AppearanceCategory")]
		[NuGenSRDescription("BackgroundStyleDescription")]
		public virtual NuGenBackgroundStyle BackgroundStyle
		{
			get
			{
				return this.backgroundStyle;
			}
			set
			{
				if (this.backgroundStyle != value)
				{
					this.backgroundStyle = value;
					this.OnBackgroundStyleChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly object EventBackgroundStyleChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenBarBase.BackgroundStyle"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
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
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenBarBase.BackgroundStyleChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough]
		protected virtual void OnBackgroundStyleChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventBackgroundStyleChanged, e);
		}

		/*
		 * BackgroundTransparency
		 */

		/// <summary>
		/// Determines the background transparency level for the control.
		/// </summary>
		private int backgroundTransparency = 0;

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
				return this.backgroundTransparency;
			}
			set
			{
				if (this.backgroundTransparency != value)
				{
					this.backgroundTransparency = value;
					this.OnBackgroundTransparencyChanged(EventArgs.Empty);
					this.BackColor = Color.FromArgb(NuGenControlPaint.GetAlphaChannel(value), this.BackgroundColor);
					this.Refresh();
				}
			}
		}

		private static readonly object EventBackgroundTransparencyChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenBarBase.BackgroundTransparency"/>
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
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenBarBase.BackgroundTransparencyChanged"/> event.
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
		/// Determines the border color for the control.
		/// </summary>
		private Color borderColor = Color.Black;

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
				return this.borderColor;
			}
			set
			{
				if (this.borderColor != value)
				{
					this.borderColor = value;
					this.OnBorderColorChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly object EventBorderColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenBarBase.BorderColor"/>
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
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenBarBase.BorderColorChanged"/> event.
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
		/// Determines the border style for the control.
		/// </summary>
		private NuGenBorderStyle borderStyle = NuGenBorderStyle.Flat;

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
				return this.borderStyle;
			}
			set
			{
				if (this.borderStyle != value)
				{
					this.borderStyle = value;
					this.OnBorderStyleChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly object EventBorderStyleChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenBarBase.BorderStyle"/>
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
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenBarBase.BorderStyleChanged"/> event.
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
		/// Determines the foreground color for the control.
		/// </summary>
		private Color foregroundColor = Color.Blue;

		/// <summary>
		/// Gets or sets the foreground color for the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(typeof(Color), "Blue")]
		[NuGenSRCategory("AppearanceCategory")]
		[NuGenSRDescription("ForegroundColorDescription")]
		public virtual Color ForegroundColor
		{
			get
			{
				return this.foregroundColor;
			}
			set
			{
				if (this.foregroundColor != value)
				{
					this.foregroundColor = value;
					this.OnForegroundColorChanged(EventArgs.Empty);
					this.ForeColor = Color.FromArgb(NuGenControlPaint.GetAlphaChannel(this.ForegroundTransparency), value);
					this.Refresh();
				}
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
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenBarBase.ForegroundColorChanged"/> event.
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
		/// Determines the foregroundstyle for the control.
		/// </summary>
		private NuGenBackgroundStyle foregroundStyle = NuGenBackgroundStyle.Gradient;

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
				return this.foregroundStyle;
			}
			set
			{
				if (this.foregroundStyle != value)
				{
					this.foregroundStyle = value;
					this.OnForegroundStyleChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly object EventForegroundStyleChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenBarBase.ForegroundStyle"/>
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
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenBarBase.ForegroundStyleChanged"/> event.
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
		/// Determines the foreground transparency level for the control.
		/// </summary>
		private int foregroundTransparency = 0;

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
				return this.foregroundTransparency;
			}
			set
			{
				if (this.foregroundTransparency != value)
				{
					this.foregroundTransparency = value;
					this.OnForegroundTransparencyChanged(EventArgs.Empty);
					this.ForeColor = Color.FromArgb(NuGenControlPaint.GetAlphaChannel(value), this.ForegroundColor);
					this.Refresh();
				}
			}
		}

		private static readonly object EventForegroundTransparencyChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="E:Genetibase.UI.NuGenMeters.NuGenBarBase.ForegroundTransparency"/>
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
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenBarBase.ForegroundTransparencyChanged"/>
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
		/// Indicates whether to stretch the background image.
		/// </summary>
		private bool stretchImage = false;

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
				return this.stretchImage;
			}
			set
			{
				if (this.stretchImage != value)
				{
					this.stretchImage = value;
					this.OnStretchImageChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly object EventStretchImageChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenBarBase.StretchImage"/>
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
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenBarBase.StretchImageChanged"/> event.
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
		/// Determines the start color of the back gradient.
		/// </summary>
		private Color backGradientStartColor = Color.Yellow;

		/// <summary>
		/// Gets or sets the start color of the background gradient.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("BackgroundGradientCategory")]
		[NuGenSRDescription("BackGradientStartColorDescription")]
		public virtual Color BackGradientStartColor
		{
			get
			{
				return this.backGradientStartColor;
			}
			set
			{
				if (this.backGradientStartColor != value)
				{
					this.backGradientStartColor = value;
					this.OnBackGradientStartColorChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly object EventBackGradientStartColorChanged = new object();

		/// <summary>
		/// Occurs when the value of <see cref="P:Genetibase.UI.NuGenMeters.NuGenBarBase.BackGradientStartColor"/>
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
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenBarBase.BackGradientStartColorChanged"/>
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
		/// Determines the end color of the back gradient.
		/// </summary>
		private Color backGradientEndColor = Color.Coral;

		/// <summary>
		/// Gets or sets the end color of the background gradient.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("BackgroundGradientCategory")]
		[NuGenSRDescription("BackGradientEndColorDescription")]
		public virtual Color BackGradientEndColor
		{
			get
			{
				return this.backGradientEndColor;
			}
			set
			{
				if (this.backGradientEndColor != value)
				{
					this.backGradientEndColor = value;
					this.OnBackGradientEndColorChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly object EventBackGradientEndColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenBarBase.BackGradientEndColor"/>
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
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenBarBase.BackGradientEndColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBackGradientEndColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventBackGradientEndColorChanged, e);
		}
		
		/*
		 * BackTubeStartColor
		 */

		/// <summary>
		/// Determines the start color of the back tube gradient.
		/// </summary>
		private Color backTubeStartColor = Color.Yellow;

		/// <summary>
		/// Gets or sets the start color of the back tube gradient.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("BackgroundGradientCategory")]
		[NuGenSRDescription("BackTubeGradientStartColorDescription")]
		public virtual Color BackTubeGradientStartColor
		{
			get
			{
				return this.backTubeStartColor;
			}
			set
			{
				if (this.backTubeStartColor != value)
				{
					this.backTubeStartColor = value;
					this.OnBackTubeStartColorChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly object EventBackTubeStartColorChanged = new object();

		/// <summary>
		/// Occurs when the value of <see cref="P:Genetibase.UI.NuGenMeters.NuGenBarBase.BackTubeGradientStartColor"/>
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
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenBarBase.BackTubeStartColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBackTubeStartColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventBackTubeStartColorChanged, e);
		}

		/*
		 * BackTubeEndColor
		 */

		/// <summary>
		/// Determines the end color of the back tube gradient.
		/// </summary>
		private Color backTubeEndColor = Color.Coral;

		/// <summary>
		/// Gets or sets the end color of the back tube gradient.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("BackgroundGradientCategory")]
		[NuGenSRDescription("BackTubeGradientEndColorDescription")]
		public virtual Color BackTubeGradientEndColor
		{
			get
			{
				return this.backTubeEndColor;
			}
			set
			{
				if (this.backTubeEndColor != value)
				{
					this.backTubeEndColor = value;
					this.OnBackTubeEndColorChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly object EventBackTubeEndColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenBarBase.BackTubeGradientEndColor"/>
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
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenBarBase.BackTubeEndColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBackTubeEndColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventBackTubeEndColorChanged, e);
		}

		#endregion

		#region Properties.ForegroundGradient

		/*
		 * GradientStartColor
		 */

		/// <summary>
		/// Determines the start color of the gradient.
		/// </summary>
		private Color gradientStartColor = Color.Blue;

		/// <summary>
		/// Gets or sets the start color of the gradient.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("ForegroundCategoryCategory")]
		[NuGenSRDescription("GradientStartColorDescription")]
		public virtual Color GradientStartColor
		{
			get
			{
				return this.gradientStartColor;
			}
			set
			{
				if (this.gradientStartColor != value)
				{
					this.gradientStartColor = value;
					this.OnGradientStartColorChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly object EventGradientStartColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenBarBase.GradientStartColor"/>
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
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenBarBase.GradientStartColorChanged"/> event.
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
		/// Determines the end color of the gradient.
		/// </summary>
		private Color gradientEndColor = Color.Red;

		/// <summary>
		/// Gets or sets the end color of the gradient.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("ForegroundGradientCategory")]
		[NuGenSRDescription("GradientEndColorDescription")]
		public virtual Color GradientEndColor
		{
			get
			{
				return this.gradientEndColor;
			}
			set
			{
				if (this.gradientEndColor != value)
				{
					this.gradientEndColor = value;
					this.OnGradientEndColorChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly object EventGradientEndColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenBarBase.GradientEndColor"/>
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
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenBarBase.NuGenGradientEndColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnGradientEndColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventGradientEndColorChanged, e);
		}

		/*
		 * TubeStartColor
		 */

		/// <summary>
		/// Determines the start color of the tube gradient.
		/// </summary>
		private Color tubeStartColor = Color.Blue;

		/// <summary>
		/// Gets or sets the start color the tube gradient.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("ForegroundGradientCategory")]
		[NuGenSRDescription("TubeGradientStartColorDescription")]
		public virtual Color TubeGradientStartColor
		{
			get
			{
				return this.tubeStartColor;
			}
			set
			{
				if (this.tubeStartColor != value)
				{
					this.tubeStartColor = value;
					this.OnTubeStartColorChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly object EventTubeStartColorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenBarBase.TubeStartColor"/>
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
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenBarBase.TubeStartColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnTubeStartColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventTubeStartColorChanged, e);
		}

		/*
		 * TubeEndColor
		 */

		/// <summary>
		/// Determines the end color of the tube gradient.
		/// </summary>
		private Color tubeEndColor = Color.Red;

		/// <summary>
		/// Gets or sets the end color of the tube gradient.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("ForegroundGradientCategory")]
		[NuGenSRDescription("TubeGradientEndColorDescription")]
		public virtual Color TubeGradientEndColor
		{
			get
			{
				return this.tubeEndColor;
			}
			set
			{
				if (this.tubeEndColor != value)
				{
					this.tubeEndColor = value;
					this.OnTubeEndColorChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly object EventTubeEndColorChanged = new object();

		/// <summary>
		/// Occurs when the value of <see cref="P:Genetibase.UI.NuGenMeters.NuGenBarBase.TubeGradientEndColor"/>
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
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenBarBase.TubeEndColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnTubeEndColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventTubeEndColorChanged, e);
		}

		#endregion

		#region Properties.Protected

		/*
		 * Maximum
		 */

		/// <summary>
		/// Determines the maximum value for this meter.
		/// </summary>
		private float maximum = 100;

		/// <summary>
		/// Gets or sets the maximum value for this meter.
		/// </summary>
		protected virtual float Maximum
		{
			get
			{
				return this.maximum;
			}
			set
			{
				if (this.maximum != value)
				{
					this.maximum = value;
					this.OnMaximumChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object EventMaximumChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenBarBase.Maximum"/>
		/// property changes.
		/// </summary>
		[Browsable(false)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("MaximumChangedDescription")]
		public event EventHandler MaximumChanged
		{
			add
			{
				this.Events.AddHandler(EventMaximumChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventMaximumChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenBarBase.MaximumChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnMaximumChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventMaximumChanged, e);
		}

		/*
		 * Minimum
		 */

		/// <summary>
		/// Determines the minimum value for this meter.
		/// </summary>
		private float minimum = 0;

		/// <summary>
		/// Gets or sets the minimum value for this meter.
		/// </summary>
		protected virtual float Minimum
		{
			get
			{
				return this.minimum;
			}
			set
			{
				if (this.minimum != value)
				{
					this.minimum = value;
					this.OnMinimumChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object EventMinimumChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenBarBase.Minimum"/>
		/// property changes.
		/// </summary>
		[Browsable(false)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("MinimumChangedDescription")]
		public event EventHandler MinimumChanged
		{
			add
			{
				this.Events.AddHandler(EventMinimumChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventMinimumChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenBarBase.MinimumChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnMinimumChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventMinimumChanged, e);
		}

		#endregion

		#region Properties.Public.New

		/// <summary>
		/// The background image used for the control.
		/// </summary>
		private Image backgroundImage = null;

		/// <summary>
		/// Gets or sets the background image used for the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(null)]
		[NuGenSRCategory("AppearanceCategory")]
		[NuGenSRDescription("BackgroundImageDescription")]
		public virtual new Image BackgroundImage
		{
			get
			{
				return this.backgroundImage;
			}
			set
			{
				if (this.backgroundImage != value)
				{
					this.backgroundImage = value;
					this.OnBackgroundImageChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		#endregion

		#region Methods.Public

		/*
		 * ResetRange
		 */

		/// <summary>
		/// Sets the range of the meter to its default value.
		/// </summary>
		public virtual void ResetRange()
		{
			this.Minimum = INITIAL_MIN;
			this.Maximum = INITIAL_MAX;
		}

		#endregion

		#region Methods.Protected.Overriden

		/*
		 * WndProc
		 */

		/// <summary>
		/// Makes the control transparent for mouse events.
		/// </summary>
		/// <param name="m">Windows message.</param>
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == win.WM_NCHITTEST)
			{
				m.Result = (IntPtr)win.HTTRANSPARENT;
			}
			else
			{
				base.WndProc(ref m);
			}
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

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenBarBase"/> class.
		/// </summary>
		public NuGenBarBase()
		{
			this.BackColor = this.BackgroundColor;
			this.ForeColor = this.ForegroundColor;
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.UserMouse, false);
			this.SetStyle(ControlStyles.UserPaint, true);
		}

		#endregion
	}
}
