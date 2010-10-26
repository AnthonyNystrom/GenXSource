/* -----------------------------------------------
 * NuGenSwitchButton.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.SwitcherInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Represents a button for the <see cref="NuGenSwitchPanel"/>.
	/// </summary>
	[ToolboxItem(false)]
	[Designer("Genetibase.Shared.Controls.Design.NuGenSwitchButtonDesigner")]
	public class NuGenSwitchButton : NuGenCheckedControl
	{
		#region Properties.Appearance

		private Image _image;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(null)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_SwitchButton_Image")]
		public Image Image
		{
			get
			{
				return _image;
			}
			set
			{
				if (_image != value)
				{
					_image = value;
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
		[NuGenSRDescription("Description_SwitchButton_ImageChanged")]
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
		/// Will bubble the <see cref="ImageChanged"/> event.
		/// </summary>
		protected virtual void OnImageChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_imageChanged, e);
		}

		/*
		 * ImageAlign
		 */

		private ContentAlignment _imageAlign = ContentAlignment.TopCenter;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(ContentAlignment.TopCenter)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_SwitchButton_ImageAlign")]
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
					this.Invalidate();
				}
			}
		}

		private static readonly object _imageAlignChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="ImageAlign"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_SwitchButton_ImageAlignChanged")]
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
		 * TextAlign
		 */

		private ContentAlignment _textAlign = ContentAlignment.BottomCenter;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(ContentAlignment.BottomCenter)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_SwitchButton_TextAlign")]
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
		[NuGenSRDescription("Description_SwitchButton_TextAlignChanged")]
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

		#region Properties.Layout

		/*
		 * Orientation
		 */

		private NuGenOrientationStyle _orientation;

		/// <summary>
		/// </summary>
		public NuGenOrientationStyle Orientation
		{
			get
			{
				return _orientation;
			}
			set
			{
				if (_orientation != value)
				{
					_orientation = value;
					this.OnOrientationChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private static readonly object _orientationChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Orientation"/> property changes.
		/// </summary>
		public event EventHandler OrientationChanged
		{
			add
			{
				this.Events.AddHandler(_orientationChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_orientationChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="OrientationChanged"/> event.
		/// </summary>
		protected virtual void OnOrientationChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_orientationChanged, e);
		}

		#endregion

		#region Properties.Public.Overridden

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

		#region Properties.Hidden

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

		#endregion

		#region Properties.Services

		private INuGenSwitchButtonLayoutManager _layoutManager;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenSwitchButtonLayoutManager LayoutManager
		{
			get
			{
				if (_layoutManager == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_layoutManager = this.ServiceProvider.GetService<INuGenSwitchButtonLayoutManager>();

					if (_layoutManager == null)
					{
						throw new NuGenServiceNotFoundException<INuGenSwitchButtonLayoutManager>();
					}
				}

				return _layoutManager;
			}
		}

		private INuGenSwitchButtonRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenSwitchButtonRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_renderer = this.ServiceProvider.GetService<INuGenSwitchButtonRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenSwitchButtonRenderer>();
					}
				}

				return _renderer;
			}
		}

		#endregion

		#region Properties.Protected.Overridden

		private static readonly Size _defaultSize = new Size(88, 54);

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

		#region Methods.Protected.Overridden

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			Rectangle bounds = this.ClientRectangle;

			if (this.Orientation == NuGenOrientationStyle.Horizontal)
			{
				bounds.X++;
				bounds.Width -= 2;
			}
			else
			{
				bounds.Y++;
				bounds.Height -= 2;
			}

			Rectangle contentBounds = this.LayoutManager.GetContentRectangle(bounds);
			NuGenControlState currentState = this.ButtonStateTracker.GetControlState();

			NuGenPaintParams paintParams = new NuGenPaintParams(e.Graphics);
			paintParams.Bounds = bounds;
			paintParams.State = currentState;

			if (currentState != NuGenControlState.Normal)
			{
				this.Renderer.DrawBackground(paintParams);
				
				NuGenPaintParams borderPaintParams = new NuGenPaintParams(paintParams);
				borderPaintParams.Bounds = NuGenControlPaint.BorderRectangle(borderPaintParams.Bounds);
				this.Renderer.DrawBorder(borderPaintParams);
			}

			Image image = this.Image;
			Rectangle imageBounds = Rectangle.Empty;
			ContentAlignment imageAlign = this.ImageAlign;

			if (image != null)
			{
				NuGenImagePaintParams imagePaintParams = new NuGenImagePaintParams(g);
				imagePaintParams.Bounds = imageBounds = this.LayoutManager.GetImageBounds(
					new NuGenBoundsParams(contentBounds
						, imageAlign
						, new Rectangle(Point.Empty, image.Size)
						, this.RightToLeft
					)
				);
				imagePaintParams.Image = image;
				imagePaintParams.State = currentState;

				this.Renderer.DrawImage(imagePaintParams);
			}

			if (imageBounds != Rectangle.Empty)
			{
				imageBounds.Inflate(3, 3);
			}

			NuGenTextPaintParams textPaintParams = new NuGenTextPaintParams(g);

			textPaintParams.Bounds = this.LayoutManager.GetTextBounds(
				new NuGenBoundsParams(contentBounds, imageAlign, imageBounds, this.RightToLeft)
			);
			textPaintParams.Font = this.Font;
			textPaintParams.ForeColor = this.ForeColor;
			textPaintParams.Text = this.Text;
			textPaintParams.TextAlign = NuGenControlPaint.RTLContentAlignment(this.TextAlign, this.RightToLeft);
			textPaintParams.State = currentState;

			this.Renderer.DrawText(textPaintParams);
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSwitchButton"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		///		<para><see cref="INuGenButtonStateService"/></para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		///		<para><see cref="INuGenSwitchButtonLayoutManager"/></para>
		///		<para><see cref="INuGenSwitchButtonRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSwitchButton(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.Opaque, false);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.Selectable, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.UserPaint, true);

			this.BackColor = Color.Transparent;
		}
	}
}
