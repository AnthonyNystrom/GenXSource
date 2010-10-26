/* -----------------------------------------------
 * NuGenSmoothPictureBox.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.PictureBoxInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;
using Genetibase.SmoothControls.ComponentModel;
using Genetibase.SmoothControls.PictureBoxInternals;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// <seealso cref="NuGenPictureBox"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothPictureBox), "Resources.NuGenIcon.png")]
	[Designer("Genetibase.SmoothControls.Design.NuGenSmoothPictureBoxDesigner")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenSmoothPictureBox : NuGenControl
	{
		#region Properties.Appearance

		/*
		 * DisplayMode
		 */

		/// <summary>
		/// Gets or sets the display mode which determines how the image is resized within this <see cref="NuGenSmoothPictureBox"/>.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(NuGenDisplayMode.ScaleToFit)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_PictureBox_DisplayMode")]
		public NuGenDisplayMode DisplayMode
		{
			get
			{
				return _pictureBox.DisplayMode;
			}
			set
			{
				_pictureBox.DisplayMode = value;
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
		 * DrawBorder
		 */

		private bool _drawBorder = true;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_SmoothPictureBox_DrawBorder")]
		public bool DrawBorder
		{
			get
			{
				return _drawBorder;
			}
			set
			{
				if (_drawBorder != value)
				{
					_drawBorder = value;
					this.OnDrawBorderChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private static readonly object _drawBorderChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="DrawBorder"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_SmoothPictureBox_DrawBorderChanged")]
		public event EventHandler DrawBorderChanged
		{
			add
			{
				this.Events.AddHandler(_drawBorderChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_drawBorderChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="DrawBorderChanged"/> event.
		/// </summary>
		protected virtual void OnDrawBorderChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_drawBorderChanged, e);
		}

		/*
		 * Image
		 */

		/// <summary>
		/// Gets or sets the image to display within this <see cref="NuGenPictureBox"/>.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(null)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_PictureBox_Image")]
		public Image Image
		{
			get
			{
				return _pictureBox.Image;
			}
			set
			{
				_pictureBox.Image = value;
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
		 * ZoomFactor
		 */

		/// <summary>
		/// </summary>
		public double ZoomFactor
		{
			get
			{
				return _pictureBox.ZoomFactor;
			}
			set
			{
				_pictureBox.ZoomFactor = value;
			}
		}

		private static readonly object _zoomFactorChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="ZoomFactor"/> property changes.
		/// </summary>
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
		/// Will bubble the <see cref="ZoomFactorChanged"/> event.
		/// </summary>
		protected virtual void OnZoomFactorChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_zoomFactorChanged, e);
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
		/// Do not use this property. Any value will not affect the appearance.
		/// </summary>
		/// <value></value>
		/// <returns>One of the <see cref="T:System.Windows.Forms.BorderStyle"></see> values. The default is BorderStyle.None.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified value when setting this property is not a valid <see cref="T:System.Windows.Forms.BorderStyle"></see> value.</exception>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new BorderStyle BorderStyle
		{
			get
			{
				return base.BorderStyle;
			}
			set
			{
				base.BorderStyle = value;
			}
		}

		#endregion

		#region Properties.Services

		private INuGenSmoothPictureBoxLayoutManager _layoutManager;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenSmoothPictureBoxLayoutManager LayoutManager
		{
			get
			{
				if (_layoutManager == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_layoutManager = this.ServiceProvider.GetService<INuGenSmoothPictureBoxLayoutManager>();

					if (_layoutManager == null)
					{
						throw new NuGenServiceNotFoundException<INuGenSmoothPictureBoxLayoutManager>();
					}
				}

				return _layoutManager;
			}
		}

		private INuGenPanelRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenPanelRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_renderer = this.ServiceProvider.GetService<INuGenPanelRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenPanelRenderer>();
					}
				}

				return _renderer;
			}
		}

		#endregion

		/*
		 * OnPaint
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			NuGenBorderPaintParams paintParams = new NuGenBorderPaintParams(e.Graphics);
			paintParams.Bounds = this.ClientRectangle;
			paintParams.DrawBorder = this.DrawBorder;
			paintParams.State = this.StateTracker.GetControlState();

			this.Renderer.DrawBorder(paintParams);
		}

		private void _pictureBox_DisplayModeChanged(object sender, EventArgs e)
		{
			this.OnDisplayModeChanged(e);
		}

		private void _pictureBox_ImageChanged(object sender, EventArgs e)
		{
			this.OnImageChanged(e);
		}

		private void _pictureBox_ZoomFactorChanged(object sender, EventArgs e)
		{
			this.OnZoomFactorChanged(e);
		}

		private NuGenPictureBox _pictureBox;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothPictureBox"/> class.
		/// </summary>
		public NuGenSmoothPictureBox()
			: this(NuGenSmoothServiceManager.PictureBoxServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothPictureBox"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// <para>Requires:</para>
		/// <para><see cref="INuGenControlStateTracker"/></para>
		/// <para><see cref="INuGenPanelRenderer"/></para>
		/// <para><see cref="INuGenPictureBoxRenderer"/></para>
		/// <para><see cref="INuGenSmoothPictureBoxLayoutManager"/></para>
		/// </param>
		public NuGenSmoothPictureBox(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.Selectable, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.UserPaint, true);

			this.BackColor = Color.Transparent;
			this.Padding = this.LayoutManager.GetInternalPictureBoxPadding();

			_pictureBox = new NuGenPictureBox(serviceProvider);
			_pictureBox.BackColor = Color.Transparent;
			_pictureBox.Dock = DockStyle.Fill;
			_pictureBox.DisplayModeChanged += _pictureBox_DisplayModeChanged;
			_pictureBox.ImageChanged += _pictureBox_ImageChanged;
			_pictureBox.ZoomFactorChanged += _pictureBox_ZoomFactorChanged;
			_pictureBox.Parent = this;
		}

		/// <summary>
		/// </summary>
		/// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_pictureBox != null)
				{
					_pictureBox.DisplayModeChanged -= _pictureBox_DisplayModeChanged;
					_pictureBox.ImageChanged -= _pictureBox_ImageChanged;
					_pictureBox.ZoomFactorChanged -= _pictureBox_ZoomFactorChanged;
					_pictureBox.Dispose();
					_pictureBox = null;
				}
			}

			base.Dispose(disposing);
		}
	}
}
