/* -----------------------------------------------
 * NuGenTitle.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.TitleInternals;
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
	/// </summary>
	[ToolboxItem(false)]
	[DefaultEvent("Paint")]
	[DefaultProperty("Text")]
	[Designer("Genetibase.Shared.Controls.Design.NuGenTitleDesigner")]
	[NuGenSRDescription("Description_Title")]
	public class NuGenTitle : NuGenControl
	{
		#region Properties.Appearance

		/*
		 * Image
		 */

		/// <summary>
		/// Gets or sets the image this <see cref="NuGenTitle"/> displays.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(null)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Title_Image")]
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
		/// Gets or sets the index of the image in the ImageList to display on the title.
		/// </summary>
		/// <exception cref="ArgumentException">
		/// <para>Index should be greater or equal to -1.</para>
		/// </exception>
		[DefaultValue(-1)]
		[Editor("Genetibase.Shared.Controls.Design.NuGenImageIndexEditor", typeof(UITypeEditor))]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Title_ImageIndex")]
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
		/// Gets or sets the ImageList to get the image to display on the title.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(null)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Title_ImageList")]
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

		#endregion

		#region Properties.Public.Overridden

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
				this.Invalidate();
			}
		}

		#endregion

		#region Properties.Protected.Overridden

		private static readonly Size _defaultSize = new Size(150, 24);

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

		#region Properties.Services

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

		private INuGenTitleRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenTitleRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_renderer = this.ServiceProvider.GetService<INuGenTitleRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenTitleRenderer>();
					}
				}

				return _renderer;
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			NuGenItemPaintParams paintParams = new NuGenItemPaintParams(e.Graphics);
			paintParams.Bounds = this.ClientRectangle;

			paintParams.ContentAlign = this.RightToLeft == RightToLeft.Yes
				? ContentAlignment.MiddleRight
				: ContentAlignment.MiddleLeft
				;
			paintParams.Font = this.Font;
			paintParams.ForeColor = this.ForeColor;
			paintParams.Image = this.Image;
			paintParams.State = this.StateTracker.GetControlState();
			paintParams.Text = this.Text;

			this.Renderer.DrawBackground(paintParams);
			this.Renderer.DrawBody(paintParams);
			this.Renderer.DrawBorder(paintParams);

			base.OnPaint(e);
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

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTitle"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// <para>Requires:</para>
		/// <para><see cref="INuGenControlImageManager"/></para>
		/// <para><see cref="INuGenControlStateService"/></para>
		/// <para><see cref="INuGenTitleRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenTitle(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.Selectable, false);
			this.SetStyle(ControlStyles.UserPaint, true);

			this.TabStop = false;
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
