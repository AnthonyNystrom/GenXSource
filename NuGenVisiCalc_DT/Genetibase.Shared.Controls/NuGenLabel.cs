/* -----------------------------------------------
 * NuGenLabel.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.LabelInternals;
using Genetibase.Shared.Controls.Properties;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// <seealso cref="Label"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenLabel), "Resources.NuGenIcon.png")]
	[DefaultEvent("Click")]
	[DefaultProperty("Text")]
	[Designer("Genetibase.Shared.Controls.Design.NuGenLabelDesigner")]
	[NuGenSRDescription("Description_Label")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenLabel : NuGenUIControl
	{
		#region Properties.Appearance

		/*
		 * Image
		 */

		/// <summary>
		/// Gets or sets the image this <see cref="NuGenLabel"/> displays.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(null)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Label_Image")]
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
		 * ImageAlign
		 */

		private ContentAlignment _imageAlign = ContentAlignment.MiddleLeft;

		/// <summary>
		/// Gets or sets the alignment of the image that will be displayed on the label.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(ContentAlignment.MiddleLeft)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Label_ImageAlign")]
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
					this.Invalidate();
				}
			}
		}

		/*
		 * ImageIndex
		 */

		/// <summary>
		/// Gets or sets the index of the image in the ImageList to display on the label.
		/// </summary>
		/// <exception cref="ArgumentException">
		/// <para>Index should be greater or equal to -1.</para>
		/// </exception>
		[DefaultValue(-1)]
		[Editor("Genetibase.Shared.Design.NuGenImageIndexEditor", typeof(UITypeEditor))]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Label_ImageIndex")]
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
		[NuGenSRDescription("Description_Label_ImageList")]
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

		private ContentAlignment _textAlign = ContentAlignment.MiddleLeft;

		/// <summary>
		/// Gets or sets the alignment of the text that will be displayed on the label.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(ContentAlignment.MiddleLeft)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Label_TextAlign")]
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
					this.Invalidate();

					this.OnTextAlignChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _textAlignChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="TextAlign"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Label_TextAlignChanged")]
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
		/// Will bubble the <see cref="TextAlignChanged"/> event.
		/// </summary>
		protected virtual void OnTextAlignChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_textAlignChanged, e);
		}

		#endregion

		#region Properties.Public.Overridden

		/*
		 * AutoSize
		 */

		/// <summary>
		/// </summary>
		[DefaultValue(true)]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
				this.AdjustSize();
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

		/*
		 * Text
		 */

		/// <summary>
		/// </summary>
		/// <value></value>
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		#endregion

		#region Properties.Protected.Overridden

		private static readonly Size _defaultSize = new Size(155, 16);

		/// <summary>
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				return _defaultSize;
			}
		}

		#endregion

		#region Properties.Protected.Virtual

		/*
		 * PreferredHeight
		 */

		/// <summary>
		/// </summary>
		protected virtual int PreferredHeight
		{
			get
			{
				int textHeight = 0;
				int imageHeight = 0;

				textHeight = imageHeight = this.Padding.Top + this.Padding.Bottom;

				if (this.Text != null)
				{
					using (Graphics g = Graphics.FromHwnd(this.Handle))
					{
						textHeight += g.MeasureString(this.Text, this.Font).ToSize().Height;
					}
				}

				if (this.Image != null)
				{
					imageHeight += this.Image.Height;
				}

				return Math.Max(textHeight, imageHeight);
			}
		}

		/*
		 * PreferredWidth
		 */

		/// <summary>
		/// </summary>
		protected virtual int PreferredWidth
		{
			get
			{
				int preferredWidth = this.Padding.Left + this.Padding.Right + 5;

				if (this.Text != null)
				{
					using (Graphics g = Graphics.FromHwnd(this.Handle))
					{
						preferredWidth += g.MeasureString(this.Text, this.Font).ToSize().Width;
					}
				}

				if (this.Image != null)
				{
					preferredWidth += this.Image.Width + 5;
				}

				return preferredWidth;
			}
		}

		#endregion

		#region Properties.Services

		/*
		 * ImageDescriptor
		 */

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

		/*
		 * LayoutManager
		 */

		private INuGenLabelLayoutManager _layoutManager;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenLabelLayoutManager LayoutManager
		{
			get
			{
				if (_layoutManager == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_layoutManager = this.ServiceProvider.GetService<INuGenLabelLayoutManager>();

					if (_layoutManager == null)
					{
						throw new NuGenServiceNotFoundException<INuGenLabelLayoutManager>();
					}
				}

				return _layoutManager;
			}
		}

		/*
		 * Renderer
		 */

		private INuGenLabelRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenLabelRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_renderer = this.ServiceProvider.GetService<INuGenLabelRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenLabelRenderer>();
					}
				}

				return _renderer;
			}
		}

		#endregion

		#region Methods.Protected

		/// <summary>
		/// </summary>
		protected void AdjustSize()
		{
			if (this.AutoSize)
			{
				int height = _requestedHeight;
				int width = _requestedWidth;

				try
				{
					Size size = this.AutoSize ? this.PreferredSize : new Size(width, height);
					base.Size = size;
				}
				finally
				{
					_requestedHeight = height;
					_requestedWidth = width;
				}
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		/*
		 * OnFontChanged
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.FontChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.AdjustSize();
			this.Invalidate();
		}

		/*
		 * OnPaint
		 */

		/// <summary>
		/// Raises the paint event.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			Rectangle bounds = this.ClientRectangle;
			NuGenControlState currentState = this.ButtonStateTracker.GetControlState();
			Image image = this.Image;
			ContentAlignment imageAlign = this.ImageAlign;
			Rectangle imageBounds = Rectangle.Empty;

			if (image != null)
			{
				NuGenImagePaintParams imagePaintParams = new NuGenImagePaintParams(g);
				imagePaintParams.Bounds = imageBounds = this.LayoutManager.GetImageBounds(
					new NuGenBoundsParams(
						bounds
						, imageAlign
						, new Rectangle(Point.Empty, image.Size)
						, this.RightToLeft
					)
				);
				imagePaintParams.Image = image;
				imagePaintParams.State = currentState;

				this.Renderer.DrawImage(imagePaintParams);
			}

			NuGenTextPaintParams textPaintParams = new NuGenTextPaintParams(g);

			textPaintParams.Bounds = this.LayoutManager.GetTextBounds(
				new NuGenBoundsParams(
					bounds
					, imageAlign
					, imageBounds
					, this.RightToLeft
				)
			);
			textPaintParams.Font = this.Font;
			textPaintParams.ForeColor = this.ForeColor;
			textPaintParams.Text = this.Text;
			textPaintParams.TextAlign = NuGenControlPaint.RTLContentAlignment(this.TextAlign, this.RightToLeft);
			textPaintParams.State = currentState;

			this.Renderer.DrawText(textPaintParams);
		}

		/*
		 * OnTextChanged
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.TextChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			this.AdjustSize();
			this.Invalidate();
		}

		/*
		 * SetBoundsCore
		 */

		/// <summary>
		/// Performs the work of setting the specified bounds of this control.
		/// </summary>
		/// <param name="x">The new <see cref="P:System.Windows.Forms.Control.Left"></see> property value of the control.</param>
		/// <param name="y">The new <see cref="P:System.Windows.Forms.Control.Top"></see> property value of the control.</param>
		/// <param name="width">The new <see cref="P:System.Windows.Forms.Control.Width"></see> property value of the control.</param>
		/// <param name="height">The new <see cref="P:System.Windows.Forms.Control.Height"></see> property value of the control.</param>
		/// <param name="specified">A bitwise combination of the <see cref="T:System.Windows.Forms.BoundsSpecified"></see> values.</param>
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if ((specified & BoundsSpecified.Height) != BoundsSpecified.None)
			{
				_requestedHeight = height;
			}

			if ((specified & BoundsSpecified.Width) != BoundsSpecified.None)
			{
				_requestedWidth = width;
			}

			if (this.AutoSize)
			{
				height = this.PreferredHeight;
				width = this.PreferredWidth;
			}

			base.SetBoundsCore(x, y, width, height, specified);
		}

		#endregion

		#region EventHandlers.ImageDescriptor

		private void _imageDescriptor_Updated(object sender, EventArgs e)
		{
			if (this.IsHandleCreated)
			{
				this.AdjustSize();
				this.Invalidate();
			}
		}

		#endregion

		private int _requestedWidth;
		private int _requestedHeight;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenLabel"/> class.
		/// </summary>
		public NuGenLabel()
			: this(NuGenServiceManager.LabelServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenLabel"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// <para>Requires:</para>
		/// <para><see cref="INuGenButtonStateService"/></para>
		/// <para><see cref="INuGenControlImageManager"/></para>
		/// <para><see cref="INuGenControlStateService"/></para>
		/// <para><see cref="INuGenLabelLayoutManager"/></para>
		/// <para><see cref="INuGenLabelRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenLabel(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.Selectable, false);

			this.AutoSize = true;
			this.BackColor = Color.Transparent;

			_requestedHeight = base.Height;
			_requestedWidth = base.Width;
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
