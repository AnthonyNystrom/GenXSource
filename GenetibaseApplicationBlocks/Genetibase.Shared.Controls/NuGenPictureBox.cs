/* -----------------------------------------------
 * NuGenPictureBox.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.Design;
using Genetibase.Shared.Controls.PictureBoxInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// A picture box with ScaleToFit and AutoScroll features.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[Designer(typeof(NuGenPictureBoxDesigner))]
	[ToolboxItem(true)]
	public class NuGenPictureBox : Panel
	{
		#region Properties.Appearance

		/*
		 * DisplayMode
		 */

		private NuGenDisplayMode _displayMode = NuGenDisplayMode.ScaleToFit;

		/// <summary>
		/// Gets or sets the display mode which determines how the image is resized within this <see cref="NuGenPictureBox"/>.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(NuGenDisplayMode.ScaleToFit)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_PictureBox_DisplayMode")]
		public NuGenDisplayMode DisplayMode
		{
			[DebuggerStepThrough]
			get
			{
				return _displayMode;
			}
			set
			{
				if (_displayMode != value)
				{
					_displayMode = value;

					switch (_displayMode)
					{
						case NuGenDisplayMode.ActualSize:
						{
							this.AutoScrollMinSize = (this.Image != null) ? this.Image.Size : Size.Empty;
							break;
						}
						case NuGenDisplayMode.ScaleToFit:
						case NuGenDisplayMode.StretchToFit:
						{
							this.AutoScrollMinSize = Size.Empty;
							break;
						}
					}

					this.OnDisplayModeChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private static readonly object _displayModeChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="DisplayMode"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_PictureBox_DisplayModeChanged")]
		public event EventHandler DisplayModeChanged
		{
			add
			{
				this.Events.AddHandler(_displayModeChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_displayModeChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="DisplayModeChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		protected virtual void OnDisplayModeChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_displayModeChanged, e);
		}

		/*
		 * Image
		 */

		private Image _image = null;

		/// <summary>
		/// Gets or sets the image to display within this <see cref="NuGenPictureBox"/>.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(null)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_PictureBox_Image")]
		public Image Image
		{
			[DebuggerStepThrough]
			get
			{
				return _image;
			}
			set
			{
				if (_image != value)
				{
					_image = value;

					if (_image != null && this.DisplayMode == NuGenDisplayMode.ActualSize)
					{
						this.AutoScrollMinSize = this.Image.Size;
					}
					else
					{
						this.AutoScrollMinSize = this.ClientRectangle.Size;
					}

					this.OnImageChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private static readonly object _imageChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Image"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_PictureBox_ImageChanged")]
		public event EventHandler ImageChanged
		{
			add
			{
				this.Events.AddHandler(_imageChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_imageChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="ImageChanged"/> event.
		/// </summary>
		/// <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
		protected virtual void OnImageChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_imageChanged, e);
		}

		#endregion

		#region Properties.Hidden

		/*
		 * BackgroundImage
		 */

		/// <summary>
		/// Do not use this property. Any value will not affect the appearance.
		/// </summary>
		/// <value></value>
		[Browsable(false)]
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

		/// <summary>
		/// Occurs when the value of the <see cref="P:NuGenPictureBox.BackgroundImage"/> property changes.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackgroundImageChanged
		{
			add
			{
				base.BackgroundImageChanged += value;
			}
			remove
			{
				base.BackgroundImageChanged -= value;
			}
		}

		/*
		 * BackgroundImageLayout
		 */

		/// <summary>
		/// Do not use this property. Any value will not affect the appearance.
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

		/// <summary>
		/// Occurs when the value of the <see cref="BackgroundImageLayout"/> property changes.
		/// </summary>
		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackgroundImageLayoutChanged
		{
			add
			{
				base.BackgroundImageLayoutChanged += value;
			}
			remove
			{
				base.BackgroundImageLayoutChanged -= value;
			}
		}

		#endregion

		#region Properties.Public.Overridden

		/*
		 * AutoScroll
		 */

		/// <summary>
		/// Gets or sets a value indicating whether the container will allow the user to scroll to any controls placed outside of its visible boundaries.
		/// </summary>
		/// <value></value>
		[DefaultValue(true)]
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

		#region Properties.Services

		/*
		 * Initiator
		 */

		private INuGenEventInitiatorService _initiator = null;

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
		 * Renderer
		 */

		private INuGenPictureBoxRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenPictureBoxRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_renderer = this.ServiceProvider.GetService<INuGenPictureBoxRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenPictureBoxRenderer>();
					}
				}

				return _renderer;
			}
		}

		/*
		 * ServiceProvider
		 */

		private INuGenServiceProvider _serviceProvider = null;

		/// <summary>
		/// </summary>
		protected INuGenServiceProvider ServiceProvider
		{
			get
			{
				return _serviceProvider;
			}
		}

		/*
		 * StateTracker
		 */

		private INuGenControlStateTracker _stateTracker = null;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenControlStateTracker StateTracker
		{
			get
			{
				if (_stateTracker == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					INuGenControlStateService stateService = this.ServiceProvider.GetService<INuGenControlStateService>();

					if (stateService == null)
					{
						throw new NuGenServiceNotFoundException<INuGenControlStateService>();
					}

					_stateTracker = stateService.CreateStateTracker();
					Debug.Assert(_stateTracker != null, "_stateTracker != null");
				}

				return _stateTracker;
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		/*
		 * OnEnabledChanged
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.EnabledChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnEnabledChanged(EventArgs e)
		{
			Debug.Assert(this.StateTracker != null, "this.StateTracker != null");
			this.StateTracker.Enabled(this.Enabled);
			base.OnEnabledChanged(e);
		}

		/*
		 * OnGotFocus
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.GotFocus"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnGotFocus(EventArgs e)
		{
			Debug.Assert(this.StateTracker != null, "this.StateTracker != null");
			this.StateTracker.GotFocus();
			base.OnGotFocus(e);
		}

		/*
		 * OnLostFocus
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.LostFocus"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnLostFocus(EventArgs e)
		{
			Debug.Assert(this.StateTracker != null, "this.StateTracker != null");
			this.StateTracker.LostFocus();
			base.OnLostFocus(e);
		}

		/*
		 * OnPaint
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (this.Image != null)
			{
				Rectangle imageBounds = this.ClientRectangle;

				switch (this.DisplayMode)
				{
					case NuGenDisplayMode.ActualSize:
					{
						imageBounds = new Rectangle(
							this.AutoScrollPosition.X,
							this.AutoScrollPosition.Y,
							this.Image.Width,
							this.Image.Height
						);
						break;
					}
					case NuGenDisplayMode.ScaleToFit:
					{
						imageBounds = NuGenControlPaint.ScaleToFit(this.ClientRectangle, this.Image);
						break;
					}
				}

				this.Renderer.DrawImage(
					new NuGenImagePaintParams(
						this,
						e.Graphics,
						imageBounds,
						this.StateTracker.GetControlState(),
						this.Image
					)
				);
			}
		}

		/*
		 * OnResize
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Resize"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnResize(EventArgs e)
		{
			if (this.Image == null)
			{
				this.AutoScrollMinSize = this.ClientRectangle.Size;
			}

			base.OnResize(e);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPictureBox"/> class.
		/// </summary>
		public NuGenPictureBox()
			: this(NuGenServiceManager.PictureBoxServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPictureBox"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenPictureBoxRenderer"/><para/>
		/// <see cref="INuGenControlStateService"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenPictureBox(INuGenServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			_serviceProvider = serviceProvider;

			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.UserPaint, true);

			this.SetStyle(ControlStyles.Selectable, false);

			this.AutoScroll = true;
			this.BackColor = Color.Transparent;
		}

		#endregion
	}
}
