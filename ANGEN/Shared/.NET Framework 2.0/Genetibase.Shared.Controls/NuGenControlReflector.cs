/* -----------------------------------------------
 * NuGenControlReflector.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.ControlReflectorInternals;
using Genetibase.Shared.Controls.Properties;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Reprresents a panel that acts like a mirror for the target control.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenControlReflector), "Resources.NuGenIcon.png")]
	[DefaultEvent("Paint")]
	[DefaultProperty("ControlToReflect")]
	[Designer("Genetibase.Shared.Controls.Design.NuGenControlReflectorDesigner")]
	[NuGenSRDescription("Description_ControlReflector")]
	[System.ComponentModel.DesignerCategory("Code")]
	public partial class NuGenControlReflector : UserControl
	{
		#region Properties.Appearance

		/*
		 * ReflectStyle
		 */

		private NuGenReflectStyle _reflectStyle;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(NuGenReflectStyle.Bottom)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_ControlReflector_ReflectStyle")]
		public NuGenReflectStyle ReflectStyle
		{
			get
			{
				return _reflectStyle;
			}
			[SecurityPermission(SecurityAction.LinkDemand)]
			set
			{
				if (_reflectStyle != value)
				{
					_reflectStyle = value;
					this.BuildReflectedImage(this.ControlToReflect);
					this.OnReflectStyleChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _reflectStyleChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="ReflectStyle"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ControlReflector_ReflectStyleChanged")]
		public event EventHandler ReflectStyleChanged
		{
			add
			{
				this.Events.AddHandler(_reflectStyleChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_reflectStyleChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="ReflectStyleChanged"/> event.
		/// </summary>
		protected virtual void OnReflectStyleChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_reflectStyleChanged, e);
		}

		/*
		 * Transparency
		 */

		private int _transparency = 40;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(40)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_ControlReflector_Transparency")]
		[Editor("Genetibase.Shared.Design.NuGenTransparencyEditor", typeof(UITypeEditor))]
		[TypeConverter("Genetibase.Shared.Design.NuGenTransparencyConverter")]
		public int Transparency
		{
			get
			{
				return _transparency;
			}
			[SecurityPermission(SecurityAction.LinkDemand)]
			set
			{
				if (_transparency != value)
				{
					_transparency = value;

					Control ctrl = _msgFilter.TargetControl;
					this.BuildReflectedImage(ctrl);

					this.OnTransparencyChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _transparencyChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Transparency"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ControlReflector_TransparencyChanged")]
		public event EventHandler TransparencyChanged
		{
			add
			{
				this.Events.AddHandler(_transparencyChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_transparencyChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="Genetibase.Shared.Controls.NuGenControlReflector.TransparencyChanged"/> event.
		/// </summary>
		protected virtual void OnTransparencyChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_transparencyChanged, e);
		}

		#endregion

		#region Properties.Behavior

		/*
		 * ControlToReflect
		 */

		/// <summary>
		/// Gets or sets the <see cref="Control"/> to be mirrored.
		/// </summary>
		/// <exception cref="ArgumentException">
		/// <para>
		///		The control cannot reflect itself.
		/// </para>
		/// -or-
		/// <para>
		///		<see cref="System.Windows.Forms.Form"/> and the inheritors cannot be mirrored.
		/// </para>
		/// -or-
		/// <para>
		///		Specified control's parent is <see langword="null"/>.
		/// </para>
		/// </exception>
		[Browsable(true)]
		[DefaultValue(null)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_ControlReflector_ControlToReflect")]
		public Control ControlToReflect
		{
			[SecurityPermission(SecurityAction.LinkDemand)]
			get
			{
				return _msgFilter.TargetControl;
			}
			[SecurityPermission(SecurityAction.LinkDemand)]
			set
			{
				if (object.ReferenceEquals(value, this))
				{
					throw new ArgumentException(Resources.Argument_SameControl);
				}

				if (value is Form)
				{
					throw new ArgumentException(Resources.Argument_OnlyControlsAcceptable);
				}

				Control ctrlToReflect = _msgFilter.TargetControl;

				if (ctrlToReflect != value)
				{
					if (ctrlToReflect != null)
					{
						ctrlToReflect.ParentChanged -= _msgFilter_TargetControlParentChanged;
					}

					_msgFilter.TargetControl = value;

					if (value != null)
					{
						value.ParentChanged += _msgFilter_TargetControlParentChanged;
						this.BuildReflectedImage(value);
					}
					else
					{
						_reflectedImage = null;
						this.Invalidate();
					}
				}
			}
		}

		private static readonly object _controlToReflectChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="ControlToReflect"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ControlReflector_ControlToReflectChanged")]
		public event EventHandler ControlToReflectChanged
		{
			add
			{
				this.Events.AddHandler(_controlToReflectChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_controlToReflectChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="ControlToReflectChanged"/> event.
		/// </summary>
		protected virtual void OnControlToReflectChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_controlToReflectChanged, e);
		}

		#endregion

		#region Properties.Hidden

		/*
		 * AutoScroll
		 */

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool AutoScroll
		{
			get
			{
				return base.AutoScroll;
			}
			set
			{
				base.AutoScroll = value;
			}
		}

		/*
		 * AutoScrollMargin
		 */

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Size AutoScrollMargin
		{
			get
			{
				return base.AutoScrollMargin;
			}
			set
			{
				base.AutoScrollMargin = value;
			}
		}

		/*
		 * AutoScrollMinSize
		 */

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Size AutoScrollMinSize
		{
			get
			{
				return base.AutoScrollMinSize;
			}
			set
			{
				base.AutoScrollMinSize = value;
			}
		}

		/*
		 * AutoScrollOffset
		 */

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Point AutoScrollOffset
		{
			get
			{
				return base.AutoScrollOffset;
			}
			set
			{
				base.AutoScrollOffset = value;
			}
		}

		/*
		 * AutoSize
		 */

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
			}
		}

		/*
		 * AutoSizeMode
		 */

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new AutoSizeMode AutoSizeMode
		{
			get
			{
				return base.AutoSizeMode;
			}
			set
			{
				base.AutoSizeMode = value;
			}
		}

		/*
		 * AutoValidate
		 */

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override AutoValidate AutoValidate
		{
			get
			{
				return base.AutoValidate;
			}
			set
			{
				base.AutoValidate = value;
			}
		}

		/*
		 * BackgroundImage
		 */

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}

		/*
		 * BackgroundImageLayout
		 */

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				return base.BackgroundImageLayout;
			}
			set
			{
				base.BackgroundImageLayout = value;
			}
		}

		/*
		 * CausesValidation
		 */

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool CausesValidation
		{
			get
			{
				return base.CausesValidation;
			}
			set
			{
				base.CausesValidation = value;
			}
		}

		/*
		 * Font
		 */

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
			}
		}

		/*
		 * ForeColor
		 */

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
			}
		}

		/*
		 * RightToLeft
		 */

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override RightToLeft RightToLeft
		{
			get
			{
				return base.RightToLeft;
			}
			set
			{
				base.RightToLeft = value;
			}
		}

		/*
		 * TabIndex
		 */

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new int TabIndex
		{
			get
			{
				return base.TabIndex;
			}
			set
			{
				base.TabIndex = value;
			}
		}

		/*
		 * TabStop
		 */

		/// <summary>
		/// Do not use this property.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new bool TabStop
		{
			get
			{
				return base.TabStop;
			}
			set
			{
				base.TabStop = value;
			}
		}

		#endregion

		#region Properties.Public.Overridden

		/*
		 * BackColor
		 */

		/// <summary>
		/// Gets or sets the background color for the control.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that represents the background color of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor"></see> property.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[DefaultValue(typeof(Color), "Transparent")]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		#endregion

		#region Properties.Private

		/*
		 * Opacity
		 */

		private int Opacity
		{
			get
			{
				return 255 - NuGenControlPaint.GetAlphaChannel(_transparency);
			}
		}

		#endregion

		#region Properties.Services

		/*
		 * ImageGenerator
		 */

		private INuGenReflectedImageGenerator _imageGenerator;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenReflectedImageGenerator ImageGenerator
		{
			get
			{
				if (_imageGenerator == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_imageGenerator = this.ServiceProvider.GetService<INuGenReflectedImageGenerator>();

					if (_imageGenerator == null)
					{
						throw new NuGenServiceNotFoundException<INuGenReflectedImageGenerator>();
					}
				}

				return _imageGenerator;
			}
		}

		/*
		 * Initiator
		 */

		private INuGenEventInitiatorService _initiator;

		/// <summary>
		/// </summary>
		protected virtual INuGenEventInitiatorService Initiator
		{
			get
			{
				if (_initiator == null)
				{
					_initiator = new NuGenEventInitiatorService(this, this.Events);
				}

				return _initiator;
			}
		}
		
		/*
		 * ServiceProvider
		 */

		private INuGenServiceProvider _serviceProvider;

		/// <summary>
		/// </summary>
		protected INuGenServiceProvider ServiceProvider
		{
			get
			{
				return _serviceProvider;
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		/*
		 * OnPaint
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			if (_reflectedImage != null)
			{
				e.Graphics.DrawImageUnscaled(_reflectedImage, new Point(0, 0));
			}
		}

		#endregion

		#region Methods.Private

		/*
		 * BuildReflectedImage
		 */

		private void BuildReflectedImage(Control ctrlToReflect)
		{
			if (ctrlToReflect != null)
			{
				Bitmap bmp = this.ImageGenerator.GetControlImage(ctrlToReflect);
				this.ImageGenerator.BuildReflectedImage(bmp, this.ReflectStyle, this.Opacity);
				_reflectedImage = bmp;
				this.Invalidate();
			}
		}

		#endregion

		#region EventHandlers

		private void _msgFilter_TargetControlParentChanged(object sender, EventArgs e)
		{
			if (_msgFilter.TargetControl.Parent == null)
			{
				this.ControlToReflect = null;
			}
		}

		private void _msgFilter_TargetControlPaint(object sender, EventArgs e)
		{
			this.BuildReflectedImage(this.ControlToReflect);
		}

		#endregion

		private MessageFilter _msgFilter;
		private Image _reflectedImage;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenControlReflector"/> class.
		/// </summary>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public NuGenControlReflector()
			: this(NuGenServiceManager.ControlReflectorServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenControlReflector"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// <para>Requires:</para>
		/// <para><see cref="INuGenReflectedImageGenerator"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="serviceProvider"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public NuGenControlReflector(INuGenServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			_serviceProvider = serviceProvider;

			_msgFilter = new MessageFilter();
			_msgFilter.TargetControlPaint += _msgFilter_TargetControlPaint;

			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.UserPaint, true);

			this.SetStyle(ControlStyles.Selectable, false);

			this.BackColor = Color.Transparent;
			this.TabStop = false;
		}
	}
}
