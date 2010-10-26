/* -----------------------------------------------
 * NuGenThumbnailContainer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks.ComponentModel;
using Genetibase.ApplicationBlocks.ImageExportInternals;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.ScrollBarInternals;
using Genetibase.Shared.Controls.SwitcherInternals;
using Genetibase.Shared.Controls.TrackBarInternals;
using Genetibase.Shared.Controls.ToolStripInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;
using Genetibase.WinApi;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks
{
	/// <summary>
	/// </summary>
	[ToolboxItem(false)]
	[System.ComponentModel.DesignerCategory("Code")]
	public partial class NuGenThumbnailContainer : NuGenControl
	{
		#region Properties.Layout

		/*
		 * Mode
		 */

		private NuGenThumbnailMode _mode;

		/// <summary>
		/// Gets or sets the value indicating how the thumbnails are layouted.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Layout")]
		[NuGenSRDescription("Description_Thumbnail_Mode")]
		public NuGenThumbnailMode Mode
		{
			get
			{
				return _mode;
			}
			set
			{
				if (_mode != value)
				{
					_mode = value;
					_toolBar.Mode = _mode;
					this.RebuildLayout();
				}
			}
		}

		private static readonly object _modeChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Mode"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Thumbnail_ModeChanged")]
		public event EventHandler ModeChanged
		{
			add
			{
				this.Events.AddHandler(_modeChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_modeChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.ApplicationBlocks.NuGenThumbnailContainer.ModeChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnModeChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_modeChanged, e);
		}

		/*
		 * ThumbnailSize
		 */

		/// <summary>
		/// Gets or sets the width and height for thumbnails.
		/// </summary>
		/// <exception cref="ArgumentException"><paramref name="value"/> should be positive.</exception>
		[Browsable(true)]
		[NuGenSRCategory("Category_Layout")]
		[NuGenSRDescription("Description_Thumbnail_ThumbnailSize")]
		public int ThumbnailSize
		{
			get
			{
				return _gridPanel.ThumbnailSize;
			}
			set
			{
				_gridPanel.ThumbnailSize = value;
				_toolBar.ThumbnailSize = value;
			}
		}

		private void ResetThumbnailSize()
		{
			this.ThumbnailSize = _gridPanel.DefaultThumbnailSize;
		}

		private bool ShouldSerializeThumbnailSize()
		{
			return this.ThumbnailSize != _gridPanel.DefaultThumbnailSize;
		}

		private static readonly object _thumbnailSizeChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="ThumbnailSize"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Thumbnail_ThumbnailSizeChanged")]
		public event EventHandler ThumbnailSizeChanged
		{
			add
			{
				this.Events.AddHandler(_thumbnailSizeChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_thumbnailSizeChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="ThumbnailSizeChanged"/> event.
		/// </summary>
		protected virtual void OnThumbnailSizeChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_thumbnailSizeChanged, e);
		}

		#endregion

		#region Properties.NonBrowsable

		/// <summary>
		/// </summary>
		[Browsable(false)]
		public ImageCollection Images
		{
			get
			{
				Debug.Assert(_imageTracker != null, "_imageTracker != null");
				return _imageTracker.Images;
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(false)]
		public IList<Image> SelectedImages
		{
			get
			{
				Debug.Assert(_gridPanel != null, "_gridPanel != null");
				return _gridPanel.SelectedImages;
			}
		}

		#endregion

		#region Properties.Protected.Overridden

		private static readonly Size _defaultSize = new Size(300, 200);

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

		#region Methods.Protected.Overridden

		/// <summary>
		/// </summary>
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if (keyData == Keys.Escape)
			{
				this.Mode = NuGenThumbnailMode.GridView;
			}

			return base.ProcessDialogKey(keyData);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			NuGenBorderPaintParams paintParams = new NuGenBorderPaintParams(e.Graphics);
			paintParams.Bounds = this.ClientRectangle;
			paintParams.DrawBorder = true;
			paintParams.State = this.StateTracker.GetControlState();

			this.Renderer.DrawBackground(paintParams);
			this.Renderer.DrawBorder(paintParams);
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.ApplicationBlocks.NuGenThumbnailContainer.Resize"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			this.RebuildLayout();
		}

		/// <summary>
		/// </summary>
		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
				case WinUser.WM_THEMECHANGED:
				{
					this.RebuildLayout();
					m.Result = IntPtr.Zero;
					break;
				}
			}

			base.WndProc(ref m);
		}

		#endregion

		#region Methods.Layout

		private void RebuildLayout()
		{
			this.SuspendLayout();

			int preferredHeight = this.ClientRectangle.Height - _toolBar.Height;

			if (_mode == NuGenThumbnailMode.GridView)
			{
				_loupePanel.Visible = false;

				int scrollBarWidth = SystemInformation.VerticalScrollBarWidth;
				Rectangle preferredBounds;

				if (this.RightToLeft == RightToLeft.Yes)
				{
					preferredBounds = Rectangle.FromLTRB(
						this.ClientRectangle.Left + scrollBarWidth
						, this.ClientRectangle.Top
						, this.ClientRectangle.Right
						, this.ClientRectangle.Top + preferredHeight
					);
				}
				else
				{
					preferredBounds = Rectangle.FromLTRB(
						this.ClientRectangle.Left
						, this.ClientRectangle.Top
						, this.ClientRectangle.Right - scrollBarWidth
						, this.ClientRectangle.Top + preferredHeight
					);
				}

				_scrollBar.Dock = this.RightToLeft == RightToLeft.Yes ? DockStyle.Left : DockStyle.Right;
				_scrollBar.Visible = true;
				_scrollBar.Width = SystemInformation.VerticalScrollBarWidth;

				_gridPanel.Bounds = this.LayoutManager.GetGridPanelBounds(preferredBounds, this.RightToLeft);
				_gridPanel.RebuildLayout();
				_gridPanel.Visible = true;

				_scrollBar.Maximum = (int)Math.Ceiling(
					(_gridPanel.DisplayRectangle.Height - _gridPanel.Height) / _autoScrollValueDivider
				);
			}
			else
			{
				_gridPanel.Visible = false;
				_scrollBar.Visible = false;

				if (_gridPanel.SelectedImages.Count > 0)
				{
					_loupePanel.SelectedImage = _gridPanel.SelectedImages[0];
				}

				Rectangle preferredBounds = new Rectangle(
					this.ClientRectangle.Left
					, this.ClientRectangle.Top
					, this.ClientRectangle.Width
					, preferredHeight
				);

				_loupePanel.Bounds = this.LayoutManager.GetLoupePanelBounds(preferredBounds, this.RightToLeft);
				_loupePanel.Visible = true;
			}

			this.ResumeLayout(true);
		}

		#endregion

		#region EventHandlers.LayoutPanels

		private void _layoutPanel_ModeChanged(object sender, ModeEventArgs e)
		{
			this.Mode = e.Mode;
		}

		private void _gridPanel_Layout(object sender, EventArgs e)
		{
			this.RebuildLayout();
		}

		private void _gridPanel_RightToLeftChanged(object sender, EventArgs e)
		{
			this.RebuildLayout();
		}

		private void _gridPanel_ThumbnailSizeChanged(object sender, EventArgs e)
		{
			this.OnThumbnailSizeChanged(e);
		}

		#endregion

		#region EventHandlers.ScrollBar

		private void _scrollBar_ValueChanged(object sender, EventArgs e)
		{
			_gridPanel.AutoScrollPosition = new Point(0, _scrollBar.Value * (int)_autoScrollValueDivider);
		}

		#endregion

		#region EventHandlers.ToolBar

		private void _toolBar_Rotate90CWButtonClick(object sender, EventArgs e)
		{
			_loupePanel.RotateSelectedImage90CW();
		}

		private void _toolBar_Rotate90CCWButtonClick(object sender, EventArgs e)
		{
			_loupePanel.RotateSelectedImage90CCW();
		}

		private void _toolBar_ZoomInButtonClick(object sender, EventArgs e)
		{
			_loupePanel.ZoomInSelectedImage();
		}

		private void _toolBar_ZoomOutButtonClick(object sender, EventArgs e)
		{
			_loupePanel.ZoomOutSelectedImage();
		}

		private void _toolBar_ThumbnailSizeChanged(object sender, EventArgs e)
		{
			this.ThumbnailSize = _toolBar.ThumbnailSize;
		}

		#endregion

		private GridPanel _gridPanel;
		private LoupePanel _loupePanel;
		private ImageTracker _imageTracker;
		private ToolBar _toolBar;
		private NuGenScrollBar _scrollBar;
		private const double _autoScrollValueDivider = 10.0;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenThumbnailContainer"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// <para><see cref="INuGenButtonStateService"/></para>
		/// <para><see cref="INuGenControlStateService"/></para>
		/// <para><see cref="INuGenPanelRenderer"/></para>
		/// <para><see cref="INuGenScrollBarRenderer"/></para>
		/// <para><see cref="INuGenSwitchButtonLayoutManager"/></para>
		/// <para><see cref="INuGenSwitchButtonRenderer"/></para>
		/// <para><see cref="INuGenTrackBarRenderer"/></para>
		/// <para><see cref="INuGenThumbnailLayoutManager"/></para>
		/// <para><see cref="INuGenThumbnailRenderer"/></para>
		/// <para><see cref="INuGenToolStripRenderer"/></para>
		/// <para><see cref="INuGenValueTrackerService"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenThumbnailContainer(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.Selectable, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, false);
			this.SetStyle(ControlStyles.UserPaint, true);

			_scrollBar = new NuGenScrollBar(serviceProvider);
			_scrollBar.Orientation = NuGenOrientationStyle.Vertical;
			_scrollBar.Dock = DockStyle.Right;
			_scrollBar.Maximum = 0;
			_scrollBar.Value = 0;
			_scrollBar.Visible = true;
			_scrollBar.ValueChanged += _scrollBar_ValueChanged;
			_scrollBar.Parent = this;

			_toolBar = new ToolBar(serviceProvider);
			_toolBar.Height = this.LayoutManager.GetToolbarHeight();
			_toolBar.ModeChanged += _layoutPanel_ModeChanged;
			_toolBar.Rotate90CWButtonClick += _toolBar_Rotate90CWButtonClick;
			_toolBar.Rotate90CCWButtonClick += _toolBar_Rotate90CCWButtonClick;
			_toolBar.ZoomInButtonClick += _toolBar_ZoomInButtonClick;
			_toolBar.ZoomOutButtonClick += _toolBar_ZoomOutButtonClick;
			_toolBar.ThumbnailSizeChanged += _toolBar_ThumbnailSizeChanged;
			_toolBar.Parent = this;

			_imageTracker = new ImageTracker();

			_gridPanel = new GridPanel(serviceProvider, _imageTracker);
			_gridPanel.Layout += _gridPanel_Layout;
			_gridPanel.ModeChanged += _layoutPanel_ModeChanged;
			_gridPanel.RightToLeftChanged += _gridPanel_RightToLeftChanged;
			_gridPanel.ThumbnailSizeChanged += _gridPanel_ThumbnailSizeChanged;
			_gridPanel.Visible = false;
			_gridPanel.Parent = this;

			_loupePanel = new LoupePanel(serviceProvider, _imageTracker);
			_loupePanel.ModeChanged += _layoutPanel_ModeChanged;
			_loupePanel.Visible = false;
			_loupePanel.Parent = this;

			this.RebuildLayout();
		}

		/// <summary>
		/// </summary>
		/// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_gridPanel != null)
				{
					_gridPanel.Layout -= _gridPanel_Layout;
					_gridPanel.ModeChanged -= _layoutPanel_ModeChanged;
					_gridPanel.RightToLeftChanged -= _gridPanel_RightToLeftChanged;
					_gridPanel.ThumbnailSizeChanged -= _gridPanel_ThumbnailSizeChanged;
				}

				if (_loupePanel != null)
				{
					_loupePanel.ModeChanged -= _layoutPanel_ModeChanged;
				}

				if (_scrollBar != null)
				{
					_scrollBar.ValueChanged -= _scrollBar_ValueChanged;
				}

				if (_toolBar != null)
				{
					_toolBar.ModeChanged -= _layoutPanel_ModeChanged;
					_toolBar.Rotate90CWButtonClick -= _toolBar_Rotate90CWButtonClick;
					_toolBar.Rotate90CCWButtonClick -= _toolBar_Rotate90CCWButtonClick;
					_toolBar.ZoomInButtonClick -= _toolBar_ZoomInButtonClick;
					_toolBar.ZoomOutButtonClick -= _toolBar_ZoomOutButtonClick;
					_toolBar.ThumbnailSizeChanged -= _toolBar_ThumbnailSizeChanged;
				}
			}

			base.Dispose(disposing);
		}
	}
}
