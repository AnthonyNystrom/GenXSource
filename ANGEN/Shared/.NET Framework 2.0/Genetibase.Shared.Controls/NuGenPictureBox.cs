/* -----------------------------------------------
 * NuGenPictureBox.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
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
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenPictureBox), "Resources.NuGenIcon.png")]
	[DefaultEvent("Paint")]
	[DefaultProperty("Image")]
	[Designer("Genetibase.Shared.Controls.Design.NuGenPictureBoxDesigner")]
	[NuGenSRDescription("Description_PictureBox")]
	[System.ComponentModel.DesignerCategory("Code")]
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
					this.AdjustAutoScrollMinSize();
					this.Invalidate();
					this.OnDisplayModeChanged(EventArgs.Empty);
					
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

		private Image _image;

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
					this.AdjustAutoScrollMinSize();
					this.Invalidate();
					this.OnImageChanged(EventArgs.Empty);
					
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

		/*
		 * ImageAlign
		 */

		private ContentAlignment _imageAlign = ContentAlignment.MiddleCenter;

		/// <summary>
		/// Gets or sets the way the image is aligned within the control. Affects only when the <see cref="DisplayMode"/> is set to
		/// ActualSize and the image is fully visible (there are no scroll bars).
		/// </summary>
		[Browsable(true)]
		[DefaultValue(ContentAlignment.MiddleCenter)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_PictureBox_ImageAlign")]
		public ContentAlignment ImageAlign
		{
			get
			{
				return _imageAlign;
			}
			set
			{
				if (_imageAlign != value)
				{
					_imageAlign = value;
					this.OnImageAlignChanged(EventArgs.Empty);
					this.Refresh();
				}
			}
		}

		private static readonly object _imageAlignChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="ImageAlign"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_PictureBox_ImageAlignChanged")]
		public event EventHandler ImageAlignChanged
		{
			add
			{
				this.Events.AddHandler(_imageAlignChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_imageAlignChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="ImageAlignChanged"/> event.
		/// </summary>
		protected virtual void OnImageAlignChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_imageAlignChanged, e);
		}

		/*
		 * ZoomFactor
		 */

		private double _zoomFactor = 1.0;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(1.0)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_PictureBox_ZoomFactor")]
		public double ZoomFactor
		{
			get
			{
				return _zoomFactor;
			}
			set
			{
				if (_zoomFactor != value)
				{
					_zoomFactor = value;
					this.OnZoomFactorChanged(EventArgs.Empty);

					if (this.DisplayMode == NuGenDisplayMode.Zoom)
					{
						this.AdjustAutoScrollMinSize();
					}

					this.Invalidate();
				}
			}
		}

		private static readonly object _zoomFactorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="ZoomFactor"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_PictureBox_ZoomFactorChanged")]
		public event EventHandler ZoomFactorChanged
		{
			add
			{
				this.Events.AddHandler(_zoomFactorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_zoomFactorChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenPictureBox.ZoomFactorChanged"/> event.
		/// </summary>
		protected virtual void OnZoomFactorChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_zoomFactorChanged, e);
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

		private INuGenControlStateTracker _stateTracker;

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
			Graphics g = e.Graphics;

			if (this.Image != null)
			{
				Rectangle imageBounds = this.ClientRectangle;

				switch (this.DisplayMode)
				{
					case NuGenDisplayMode.ActualSize:
					{
						if (this.AutoScrollPosition.X == 0
							&& this.AutoScrollPosition.Y == 0
							&& this.AutoScrollMinSize.Width <= this.ClientRectangle.Width
							&& this.AutoScrollMinSize.Height <= this.ClientRectangle.Height
							)
						{
							imageBounds = NuGenControlPaint.ImageBoundsFromContentAlignment(
								this.Image.Size
								, this.ClientRectangle
								, this.ImageAlign
							);
						}
						else
						{
							imageBounds = new Rectangle(
								this.AutoScrollPosition.X
								, this.AutoScrollPosition.Y
								, this.Image.Width
								, this.Image.Height
							);
						}

						break;
					}
					case NuGenDisplayMode.Zoom:
					{
						imageBounds = new Rectangle(
							(int)(this.AutoScrollPosition.X / _zoomFactor)
							, (int)(this.AutoScrollPosition.Y / _zoomFactor)
							, (int)(this.Image.Width * _zoomFactor)
							, (int)(this.Image.Height * _zoomFactor)
						);
						float zoomFactor = (float)_zoomFactor;
						g.ScaleTransform(zoomFactor, zoomFactor);
						break;
					}
					case NuGenDisplayMode.ScaleToFit:
					{
						imageBounds = NuGenControlPaint.ScaleToFit(this.ClientRectangle, this.Image.Size);
						break;
					}
				}

				NuGenImagePaintParams imagePaintParams = new NuGenImagePaintParams(g);
				imagePaintParams.Bounds = imageBounds;
				imagePaintParams.Image = this.Image;
				imagePaintParams.State = this.StateTracker.GetControlState();

				this.Renderer.DrawImage(imagePaintParams);
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
			this.AdjustAutoScrollMinSize();
			base.OnResize(e);
		}

		#endregion

		#region Methods.Private

		/*
		 * AdjustAutoScrollMinSize
		 */

		private void AdjustAutoScrollMinSize()
		{
			if (_image != null && _displayMode == NuGenDisplayMode.ActualSize)
			{
				this.AutoScrollMinSize = new Size(_image.Size.Width, _image.Size.Height);
			}
			else if (_image != null && _displayMode == NuGenDisplayMode.Zoom)
			{
				this.AutoScrollMinSize = new Size(
					(int)(_image.Size.Width * _zoomFactor * _zoomFactor)
					, (int)(_image.Size.Height * _zoomFactor * _zoomFactor)
				);
			}
			else
			{
				this.AutoScrollMinSize = this.ClientRectangle.Size;
			}
		}

		#endregion

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
	}
}
