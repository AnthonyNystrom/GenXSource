/* -----------------------------------------------
 * NuGenBarBase.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Design;
using Genetibase.Shared;
using Genetibase.Shared.Drawing;
using Genetibase.Meters.ComponentModel;
using Genetibase.Meters.Design;
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
	/// Implements base meter and graph bar functionality.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[Designer(typeof(NuGenBarBaseDesigner))]
	[ToolboxItem(false)]
	public class NuGenBarBase : UserControl
	{
		#region Properties.Appearance

		/*
		 * BackgroundColor
		 */

		private Color _backgroundColor;

		/// <summary>
		/// Gets or sets the background color for the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(typeof(Color), "Yellow")]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_BackgroundColor")]
		public virtual Color BackgroundColor
		{
			get
			{
				if (_backgroundColor == Color.Empty)
				{
					return Color.Yellow;
				}

				return _backgroundColor;
			}
			set
			{
				if (_backgroundColor != value)
				{
					_backgroundColor = value;
					this.OnBackgroundColorChanged(EventArgs.Empty);
					this.BackColor = Color.FromArgb(NuGenControlPaint.GetAlphaChannel(this.BackgroundTransparency), value);
					this.Refresh();
				}
			}
		}

		private static readonly Object _backgroundColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenBarBase.BackgroundColor"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_BackgroundColorChanged")]
		public event EventHandler BackgroundColorChanged
		{
			add
			{
				this.Events.AddHandler(_backgroundColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_backgroundColorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.BackgroundColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough]
		protected virtual void OnBackgroundColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_backgroundColorChanged, e);
		}

		/*
		 * BackgroundStyle
		 */

		private NuGenBackgroundStyle _backgroundStyle = NuGenBackgroundStyle.Gradient;

		/// <summary>
		/// Gets or sets the background style for the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(NuGenBackgroundStyle.Gradient)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_BackgroundStyle")]
		public virtual NuGenBackgroundStyle BackgroundStyle
		{
			get
			{
				return _backgroundStyle;
			}
			set
			{
				if (_backgroundStyle != value)
				{
					_backgroundStyle = value;
					this.OnBackgroundStyleChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly Object EventBackgroundStyleChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenBarBase.BackgroundStyle"/>
		/// property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_BackgroundStyleChanged")]
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
		/// Raises the <see cref="E:Genetibase.Meters.NuGenBarBase.BackgroundStyleChanged"/> event.
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

		private Int32 _backgroundTransparency;

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
				return _backgroundTransparency;
			}
			set
			{
				if (_backgroundTransparency != value)
				{
					_backgroundTransparency = value;
					this.OnBackgroundTransparencyChanged(EventArgs.Empty);
					this.BackColor = Color.FromArgb(NuGenControlPaint.GetAlphaChannel(value), this.BackgroundColor);
					this.Refresh();
				}
			}
		}

		private static readonly Object _backgroundTransparencyChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenBarBase.BackgroundTransparency"/>
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
		/// Raises the <see cref="E:Genetibase.Meters.NuGenBarBase.BackgroundTransparencyChanged"/> event.
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

		private Color _borderColor;

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
				if (_borderColor == Color.Empty)
				{
					return Color.Black;
				}

				return _borderColor;
			}
			set
			{
				if (_borderColor != value)
				{
					_borderColor = value;
					this.OnBorderColorChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly Object _borderColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenBarBase.BorderColor"/>
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
		/// Raises the <see cref="E:Genetibase.Meters.NuGenBarBase.BorderColorChanged"/> event.
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
		/// Determines the border style for the control.
		/// </summary>
		private NuGenBorderStyle _borderStyle = NuGenBorderStyle.Flat;

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
				return _borderStyle;
			}
			set
			{
				if (_borderStyle != value)
				{
					_borderStyle = value;
					this.OnBorderStyleChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly Object _borderStyleChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenBarBase.BorderStyle"/>
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
		/// Raises the <see cref="E:Genetibase.Meters.NuGenBarBase.BorderStyleChanged"/> event.
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

		private Color _foregroundColor;

		/// <summary>
		/// Gets or sets the foreground color for the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(typeof(Color), "Blue")]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_ForegroundColor")]
		public virtual Color ForegroundColor
		{
			get
			{
				if (_foregroundColor == Color.Empty)
				{
					return Color.Blue;
				}

				return _foregroundColor;
			}
			set
			{
				if (_foregroundColor != value)
				{
					_foregroundColor = value;
					this.OnForegroundColorChanged(EventArgs.Empty);
					this.ForeColor = Color.FromArgb(NuGenControlPaint.GetAlphaChannel(this.ForegroundTransparency), value);
					this.Refresh();
				}
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
		/// Raises the <see cref="E:Genetibase.Meters.NuGenBarBase.ForegroundColorChanged"/> event.
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

		private NuGenBackgroundStyle _foregroundStyle = NuGenBackgroundStyle.Gradient;

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
				return _foregroundStyle;
			}
			set
			{
				if (_foregroundStyle != value)
				{
					_foregroundStyle = value;
					this.OnForegroundStyleChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly Object _foregroundStyleChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenBarBase.ForegroundStyle"/>
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
		/// Raises the <see cref="E:Genetibase.Meters.NuGenBarBase.ForegroundStyleChanged"/> event.
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
		/// Determines the foreground transparency level for the control.
		/// </summary>
		private Int32 _foregroundTransparency;

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
				return _foregroundTransparency;
			}
			set
			{
				if (_foregroundTransparency != value)
				{
					_foregroundTransparency = value;
					this.OnForegroundTransparencyChanged(EventArgs.Empty);
					this.ForeColor = Color.FromArgb(NuGenControlPaint.GetAlphaChannel(value), this.ForegroundColor);
					this.Refresh();
				}
			}
		}

		private static readonly Object _foregroundTransparencyChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="E:Genetibase.Meters.NuGenBarBase.ForegroundTransparency"/>
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
		/// Raises the <see cref="E:Genetibase.Meters.NuGenBarBase.ForegroundTransparencyChanged"/>
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

		private Boolean _stretchImage;

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
				return _stretchImage;
			}
			set
			{
				if (_stretchImage != value)
				{
					_stretchImage = value;
					this.OnStretchImageChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly Object _stretchImageChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenBarBase.StretchImage"/>
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
		/// Raises the <see cref="E:Genetibase.Meters.NuGenBarBase.StretchImageChanged"/> event.
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
		/// Determines the start color of the back gradient.
		/// </summary>
		private Color _backGradientStartColor;

		/// <summary>
		/// Gets or sets the start color of the background gradient.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_BackgroundGradient")]
		[NuGenSRDescription("Description_BackGradientStartColor")]
		public virtual Color BackGradientStartColor
		{
			get
			{
				if (_backGradientStartColor == Color.Empty)
				{
					return Color.Yellow;
				}

				return _backGradientStartColor;
			}
			set
			{
				if (_backGradientStartColor != value)
				{
					_backGradientStartColor = value;
					this.OnBackGradientStartColorChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly Object _backGradientStartColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of <see cref="P:Genetibase.Meters.NuGenBarBase.BackGradientStartColor"/>
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
		/// Raises the <see cref="E:Genetibase.Meters.NuGenBarBase.BackGradientStartColorChanged"/> event.
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

		private Color _backGradientEndColor;

		/// <summary>
		/// Gets or sets the end color of the background gradient.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_BackgroundGradient")]
		[NuGenSRDescription("Description_BackGradientEndColor")]
		public virtual Color BackGradientEndColor
		{
			get
			{
				if (_backGradientEndColor == Color.Empty)
				{
					return Color.Coral;
				}

				return _backGradientEndColor;
			}
			set
			{
				if (_backGradientEndColor != value)
				{
					_backGradientEndColor = value;
					this.OnBackGradientEndColorChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly Object _backGradientEndColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenBarBase.BackGradientEndColor"/>
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
		/// Raises the <see cref="E:Genetibase.Meters.NuGenBarBase.BackGradientEndColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBackGradientEndColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_backGradientEndColorChanged, e);
		}
		
		/*
		 * BackTubeStartColor
		 */

		/// <summary>
		/// Determines the start color of the back tube gradient.
		/// </summary>
		private Color _backTubeStartColor;

		/// <summary>
		/// Gets or sets the start color of the back tube gradient.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_BackgroundGradient")]
		[NuGenSRDescription("Description_BackTubeGradientStartColor")]
		public virtual Color BackTubeGradientStartColor
		{
			get
			{
				if (_backTubeStartColor == Color.Empty)
				{
					return Color.Yellow;
				}

				return _backTubeStartColor;
			}
			set
			{
				if (_backTubeStartColor != value)
				{
					_backTubeStartColor = value;
					this.OnBackTubeStartColorChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly Object _backTubeStartColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of <see cref="P:Genetibase.Meters.NuGenBarBase.BackTubeGradientStartColor"/> property changes.
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
		/// Raises the <see cref="E:Genetibase.Meters.NuGenBarBase.BackTubeStartColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBackTubeStartColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_backTubeStartColorChanged, e);
		}

		/*
		 * BackTubeEndColor
		 */

		private Color _backTubeEndColor;

		/// <summary>
		/// Gets or sets the end color of the back tube gradient.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_BackgroundGradient")]
		[NuGenSRDescription("Description_BackTubeGradientEndColor")]
		public virtual Color BackTubeGradientEndColor
		{
			get
			{
				if (_backTubeEndColor == Color.Empty)
				{
					return Color.Coral;
				}

				return _backTubeEndColor;
			}
			set
			{
				if (_backTubeEndColor != value)
				{
					_backTubeEndColor = value;
					this.OnBackTubeEndColorChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly Object _backTubeEndColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenBarBase.BackTubeGradientEndColor"/> property changes.
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
		/// Raises the <see cref="E:Genetibase.Meters.NuGenBarBase.BackTubeEndColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnBackTubeEndColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_backTubeEndColorChanged, e);
		}

		#endregion

		#region Properties.ForegroundGradient

		/*
		 * GradientStartColor
		 */

		/// <summary>
		/// Determines the start color of the gradient.
		/// </summary>
		private Color _gradientStartColor;

		/// <summary>
		/// Gets or sets the start color of the gradient.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_ForegroundGradient")]
		[NuGenSRDescription("Description_GradientStartColor")]
		public virtual Color GradientStartColor
		{
			get
			{
				if (_gradientStartColor == Color.Empty)
				{
					return Color.Blue;
				}

				return _gradientStartColor;
			}
			set
			{
				if (_gradientStartColor != value)
				{
					_gradientStartColor = value;
					this.OnGradientStartColorChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly Object _gradientStartColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenBarBase.GradientStartColor"/> property changes.
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
		/// Raises the <see cref="E:Genetibase.Meters.NuGenBarBase.GradientStartColorChanged"/> event.
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

		private Color _gradientEndColor;

		/// <summary>
		/// Gets or sets the end color of the gradient.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_ForegroundGradient")]
		[NuGenSRDescription("Description_GradientEndColor")]
		public virtual Color GradientEndColor
		{
			get
			{
				if (_gradientEndColor == Color.Empty)
				{
					return Color.Red;
				}

				return _gradientEndColor;
			}
			set
			{
				if (_gradientEndColor != value)
				{
					_gradientEndColor = value;
					this.OnGradientEndColorChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly Object _gradientEndColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenBarBase.GradientEndColor"/> property changes.
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
		/// Raises the <see cref="E:Genetibase.Meters.NuGenBarBase.NuGenGradientEndColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnGradientEndColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_gradientEndColorChanged, e);
		}

		/*
		 * TubeStartColor
		 */

		private Color _tubeStartColor;

		/// <summary>
		/// Gets or sets the start color the tube gradient.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(typeof(Color), "Blue")]
		[NuGenSRCategory("Category_ForegroundGradient")]
		[NuGenSRDescription("Description_TubeGradientStartColor")]
		public virtual Color TubeGradientStartColor
		{
			get
			{
				if (_tubeStartColor == Color.Empty)
				{
					return Color.Blue;
				}

				return _tubeStartColor;
			}
			set
			{
				if (_tubeStartColor != value)
				{
					_tubeStartColor = value;
					this.OnTubeStartColorChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly Object _tubeStartColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenBarBase.TubeStartColor"/> property changes.
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
		/// Raises the <see cref="E:Genetibase.Meters.NuGenBarBase.TubeStartColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnTubeStartColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_tubeStartColorChanged, e);
		}

		/*
		 * TubeEndColor
		 */

		private Color _tubeEndColor;

		/// <summary>
		/// Gets or sets the end color of the tube gradient.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_ForegroundGradient")]
		[NuGenSRDescription("Description_TubeGradientEndColor")]
		public virtual Color TubeGradientEndColor
		{
			get
			{
				if (_tubeEndColor == Color.Empty)
				{
					return Color.Red;
				}

				return _tubeEndColor;
			}
			set
			{
				if (_tubeEndColor != value)
				{
					_tubeEndColor = value;
					this.OnTubeEndColorChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly Object _tubeEndColorChanged = new Object();

		/// <summary>
		/// Occurs when the value of <see cref="P:Genetibase.Meters.NuGenBarBase.TubeGradientEndColor"/> property changes.
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
		/// Raises the <see cref="E:Genetibase.Meters.NuGenBarBase.TubeEndColorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnTubeEndColorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_tubeEndColorChanged, e);
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

		private static readonly Object EventMaximumChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenBarBase.Maximum"/>
		/// property changes.
		/// </summary>
		[Browsable(false)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_MaximumChanged")]
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
		/// Raises the <see cref="E:Genetibase.Meters.NuGenBarBase.MaximumChanged"/> event.
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

		private static readonly Object EventMinimumChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenBarBase.Minimum"/>
		/// property changes.
		/// </summary>
		[Browsable(false)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_MinimumChanged")]
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
		/// Raises the <see cref="E:Genetibase.Meters.NuGenBarBase.MinimumChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnMinimumChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventMinimumChanged, e);
		}

		#endregion

		#region Properties.Public.New

		private Image _backgroundImage;

		/// <summary>
		/// Gets or sets the background image used for the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(null)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_BackgroundImage")]
		public virtual new Image BackgroundImage
		{
			get
			{
				return _backgroundImage;
			}
			set
			{
				if (_backgroundImage != value)
				{
					_backgroundImage = value;
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
			this.Minimum = _initialMin;
			this.Maximum = _initialMax;
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
			if (m.Msg == WinUser.WM_NCHITTEST)
			{
				m.Result = (IntPtr)WinUser.HTTRANSPARENT;
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
		protected virtual void InvokePropertyChanged(Object key, EventArgs e)
		{
			EventHandler handler = (EventHandler)this.Events[key];

			if (handler != null)
			{
				handler(this, e);
			}
		}

		#endregion

		private const Single _initialMin = 0.0f;
		private const Single _initialMax = 100.0f;

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
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.UserMouse, false);
		}
	}
}
