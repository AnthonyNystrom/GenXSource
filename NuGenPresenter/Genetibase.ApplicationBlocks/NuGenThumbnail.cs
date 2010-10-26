/* -----------------------------------------------
 * NuGenThumbnail.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks.ComponentModel;
using Genetibase.ApplicationBlocks.ImageExportInternals;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks
{
	[System.ComponentModel.DesignerCategory("Code")]
	internal partial class NuGenThumbnail : NuGenCheckedControl
	{
		#region Events

		public event EventHandler Rotate90CWButtonClick;
		public event EventHandler Rotate90CCWButtonClick;

		#endregion

		#region Properties.Appearance

		/*
		 * Image
		 */

		/// <summary>
		/// Gets or sets the image this <see cref="NuGenThumbnail"/> displays.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(null)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Thumbnail_Image")]
		public Image Image
		{
			get
			{
				return this.ImageDescriptor.Image;
			}
			set
			{
				this.ImageDescriptor.Image = value;
			}
		}

		/*
		 * ImageIndex
		 */

		/// <summary>
		/// Gets or sets the index of the image in the ImageList to display on the control.
		/// </summary>
		/// <exception cref="ArgumentException">
		/// <para>Index should be greater or equal to -1.</para>
		/// </exception>
		[DefaultValue(-1)]
		[Editor("Genetibase.Shared.Design.NuGenImageIndexEditor", typeof(UITypeEditor))]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Thumbnail_ImageIndex")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[TypeConverter(typeof(ImageIndexConverter))]
		public int ImageIndex
		{
			get
			{
				return this.ImageDescriptor.ImageIndex;
			}
			set
			{
				this.ImageDescriptor.ImageIndex = value;
			}
		}

		/*
		 * ImageList
		 */

		/// <summary>
		/// Gets or sets the ImageList to get the image to display on the label.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(null)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Thumbnail_ImageList")]
		public ImageList ImageList
		{
			get
			{
				return this.ImageDescriptor.ImageList;
			}
			set
			{
				this.ImageDescriptor.ImageList = value;
			}
		}

		/*
		 * TextAlign
		 */

		private ContentAlignment _textAlign = ContentAlignment.TopLeft;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(ContentAlignment.TopLeft)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Thumbnail_TextAlign")]
		public ContentAlignment TextAlign
		{
			get
			{
				return _textAlign;
			}
			set
			{
				if (_textAlign != value)
				{
					_textAlign = value;
					this.OnTextAlignChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private static readonly object _textAlignChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="TextAlign"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Thumbnail_TextAlignChanged")]
		public event EventHandler TextAlignChanged
		{
			add
			{
				this.Events.AddHandler(_textAlignChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_textAlignChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.ApplicationBlocks.NuGenThumbnail.TextAlignChanged"/> event.
		/// </summary>
		protected virtual void OnTextAlignChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_textAlignChanged, e);
		}

		#endregion

		#region Properties.Hidden

		/// <summary>
		/// Gets or sets the background color for the control.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that represents the background color of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor"></see> property.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		/// <summary>
		/// Gets or sets the background image displayed in the control.
		/// </summary>
		/// <value></value>
		/// <returns>An <see cref="T:System.Drawing.Image"></see> that represents the image to display in the background of the control.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
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

		/// <summary>
		/// Gets or sets the background image layout as defined in the <see cref="T:System.Windows.Forms.ImageLayout"></see> enumeration.
		/// </summary>
		/// <value></value>
		/// <returns>One of the values of <see cref="T:System.Windows.Forms.ImageLayout"></see> (<see cref="F:System.Windows.Forms.ImageLayout.Center"></see> , <see cref="F:System.Windows.Forms.ImageLayout.None"></see>, <see cref="F:System.Windows.Forms.ImageLayout.Stretch"></see>, <see cref="F:System.Windows.Forms.ImageLayout.Tile"></see>, or <see cref="F:System.Windows.Forms.ImageLayout.Zoom"></see>). <see cref="F:System.Windows.Forms.ImageLayout.Tile"></see> is the default value.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified enumeration value does not exist. </exception>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
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
		/// Gets or sets the font of the text displayed by the control.
		/// </summary>
		/// <value></value>
		/// <returns>The <see cref="T:System.Drawing.Font"></see> to apply to the text displayed by the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultFont"></see> property.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
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

		/// <summary>
		/// Gets or sets the foreground color of the control.
		/// </summary>
		/// <value></value>
		/// <returns>The foreground <see cref="T:System.Drawing.Color"></see> of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultForeColor"></see> property.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
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

		#endregion

		#region Properties.Services

		private INuGenButtonStateTracker _cwRotateButtonStateTracker;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenButtonStateTracker CWRotateButtonStateTracker
		{
			get
			{
				if (_cwRotateButtonStateTracker == null)
				{
					INuGenButtonStateService service = this.ServiceProvider.GetService<INuGenButtonStateService>();

					if (service == null)
					{
						throw new NuGenServiceNotFoundException<INuGenButtonStateService>();
					}

					_cwRotateButtonStateTracker = service.CreateStateTracker();
					Debug.Assert(_cwRotateButtonStateTracker != null, "_cwRotateButtonStateTracker != null");
				}

				return _cwRotateButtonStateTracker;
			}
		}

		private INuGenButtonStateTracker _ccwRotateButtonStateTracker;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenButtonStateTracker CCWRotateButtonStateTracker
		{
			get
			{
				if (_ccwRotateButtonStateTracker == null)
				{
					INuGenButtonStateService service = this.ServiceProvider.GetService<INuGenButtonStateService>();

					if (service == null)
					{
						throw new NuGenServiceNotFoundException<INuGenButtonStateService>();
					}

					_ccwRotateButtonStateTracker = service.CreateStateTracker();
					Debug.Assert(_ccwRotateButtonStateTracker != null, "_ccwRotateButtonStateTracker != null");
				}

				return _ccwRotateButtonStateTracker;
			}
		}

		private INuGenControlImageDescriptor _imageDescriptor;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenControlImageDescriptor ImageDescriptor
		{
			get
			{
				if (_imageDescriptor == null)
				{
					INuGenControlImageManager imageManager = this.ServiceProvider.GetService<INuGenControlImageManager>();

					if (imageManager == null)
					{
						throw new NuGenServiceNotFoundException<INuGenControlImageManager>();
					}

					_imageDescriptor = imageManager.CreateImageDescriptor();
					_imageDescriptor.ImageChanged += _imageDescriptor_Updated;
					_imageDescriptor.ImageIndexChanged += _imageDescriptor_Updated;
					_imageDescriptor.ImageListChanged += _imageDescriptor_Updated;
				}

				return _imageDescriptor;
			}
		}

		private INuGenThumbnailLayoutManager _layoutManager;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenThumbnailLayoutManager LayoutManager
		{
			get
			{
				if (_layoutManager == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_layoutManager = this.ServiceProvider.GetService<INuGenThumbnailLayoutManager>();

					if (_layoutManager == null)
					{
						throw new NuGenServiceNotFoundException<INuGenThumbnailLayoutManager>();
					}
				}

				return _layoutManager;
			}
		}

		private INuGenThumbnailRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenThumbnailRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_renderer = this.ServiceProvider.GetService<INuGenThumbnailRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenThumbnailRenderer>();
					}
				}

				return _renderer;
			}
		}

		#endregion

		#region Methods.Public

		/*
		 * RotateImage90CW
		 */

		/// <summary>
		/// </summary>
		public void RotateImage90CW()
		{
			ImageRotator.RotateImage(this.Image, ImageRotationStyle.CW);
			this.Invalidate();
		}

		/*
		 * RotateImage90CCW
		 */

		/// <summary>
		/// </summary>
		public void RotateImage90CCW()
		{
			ImageRotator.RotateImage(this.Image, ImageRotationStyle.CCW);
			this.Invalidate();
		}

		#endregion

		#region Methods.Protected.Overridden

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.DoubleClick"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnDoubleClick(EventArgs e)
		{
			if (!this.IsInsideRotateButtons(this.PointToClient(Control.MousePosition)))
			{
				base.OnDoubleClick(e);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (!this.IsInsideRotateButtons(e.Location))
			{
				base.OnMouseDown(e);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			NuGenPaintParams paintParams = new NuGenPaintParams(e.Graphics);
			paintParams.Bounds = this.ClientRectangle;
			paintParams.State = this.ButtonStateTracker.GetControlState();

			switch (paintParams.State)
			{
				case NuGenControlState.Pressed:
				case NuGenControlState.Hot:
				{
					_cwRotateButton.Visible = _ccwRotateButton.Visible = true;
					break;
				}
				default:
				{
					_cwRotateButton.Visible = _ccwRotateButton.Visible = false;
					break;
				}
			}

			this.Renderer.DrawBackground(paintParams);
			this.Renderer.DrawBorder(paintParams);

			NuGenTextPaintParams textPaintParams = new NuGenTextPaintParams(paintParams);

			using (Font font = this.Renderer.GetFont(textPaintParams.Bounds))
			{
				textPaintParams.Bounds = this.LayoutManager.GetTextBounds(textPaintParams.Bounds, font.Size);
				textPaintParams.Font = font;
				textPaintParams.ForeColor = this.Renderer.GetForeColor(textPaintParams.State);
				textPaintParams.Text = this.Text;
				textPaintParams.TextAlign = this.TextAlign;

				this.Renderer.DrawText(textPaintParams);
			}

			Image image = this.Image;

			if (image != null)
			{
				NuGenImagePaintParams imagePaintParams = new NuGenImagePaintParams(paintParams);
				imagePaintParams.Bounds = this.LayoutManager.GetImageBounds(imagePaintParams.Bounds, image.Size);
				imagePaintParams.Image = image;

				this.Renderer.DrawImage(imagePaintParams);
				this.Renderer.DrawBorder(imagePaintParams);
			}

			base.OnPaint(e);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Resize"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			_cwRotateButton.Bounds = this.GetCWRotateButtonBounds();
			_ccwRotateButton.Bounds = this.GetCCWRotateButtonBounds();
		}

		#endregion

		#region Methods.Private

		private Rectangle GetCWRotateButtonBounds()
		{
			return new Rectangle(
				this.LayoutManager.GetCWRotateButtonLocation(this.ClientRectangle, _cwRotateButtonSize)
				, _cwRotateButtonSize
			);
		}

		private Rectangle GetCCWRotateButtonBounds()
		{
			return new Rectangle(
				this.LayoutManager.GetCCWRotateButtonLocation(this.ClientRectangle, _ccwRotateButtonSize)
				, _ccwRotateButtonSize
			);
		}

		private bool IsInsideRotateButtons(Point pointToCheck)
		{
			if (
				_cwRotateButton.Bounds.Contains(pointToCheck)
				|| _ccwRotateButton.Bounds.Contains(pointToCheck)
				)
			{
				return true;
			}

			return false;
		}

		#endregion

		#region EventHandlers.ImageDescriptor

		private void _imageDescriptor_Updated(object sender, EventArgs e)
		{
			if (this.IsHandleCreated)
			{
				this.Invalidate();
			}
		}

		#endregion

		#region EventHandlers.RotateButtons

		private void _cwRotateButton_Click(object sender, EventArgs e)
		{
			if (this.Rotate90CWButtonClick != null)
			{
				this.Rotate90CWButtonClick(this, e);
			}
		}

		private void _ccwRotateButton_Click(object sender, EventArgs e)
		{
			if (this.Rotate90CCWButtonClick != null)
			{
				this.Rotate90CCWButtonClick(this, e);
			}
		}

		#endregion

		private Size _cwRotateButtonSize;
		private Size _ccwRotateButtonSize;

		private CWRotateButton _cwRotateButton;
		private CCWRotateButton _ccwRotateButton;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenThumbnail"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenButtonStateService"/></para>
		///		<para><see cref="INuGenControlImageManager"/></para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		///		<para><see cref="INuGenThumbnailLayoutManager"/></para>
		///		<para><see cref="INuGenThumbnailRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenThumbnail(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.Selectable, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, false);
			this.SetStyle(ControlStyles.UserPaint, true);

			_cwRotateButtonSize = this.LayoutManager.GetCWRotateButtonSize();
			_ccwRotateButtonSize = this.LayoutManager.GetCCWRotateButtonSize();

			_cwRotateButton = new CWRotateButton(serviceProvider);
			_cwRotateButton.Bounds = this.GetCWRotateButtonBounds();
			_cwRotateButton.Click += _cwRotateButton_Click;
			_cwRotateButton.Parent = this;

			_ccwRotateButton = new CCWRotateButton(serviceProvider);
			_ccwRotateButton.Bounds = this.GetCCWRotateButtonBounds();
			_ccwRotateButton.Click += _ccwRotateButton_Click;
			_ccwRotateButton.Parent = this;
		}

		/// <summary>
		/// </summary>
		/// <param name="disposing">
		/// <see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.
		/// </param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_imageDescriptor != null)
				{
					_imageDescriptor.ImageChanged -= _imageDescriptor_Updated;
					_imageDescriptor.ImageIndexChanged -= _imageDescriptor_Updated;
					_imageDescriptor.ImageListChanged -= _imageDescriptor_Updated;

					_imageDescriptor = null;
				}
			}

			base.Dispose(disposing);
		}
	}
}
