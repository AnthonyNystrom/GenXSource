/* -----------------------------------------------
 * NuGenThumbnailContainer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks.ImageExportInternals;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.SwitcherInternals;
using Genetibase.Shared.Controls.TrackBarInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks
{
	partial class NuGenThumbnailContainer
	{
		private sealed class ToolBar : NuGenPanel
		{
			public event EventHandler ZoomInButtonClick;

			private void OnZoomInButtonClick(EventArgs e)
			{
				if (this.ZoomInButtonClick != null)
				{
					this.ZoomInButtonClick(this, e);
				}
			}

			public event EventHandler ZoomOutButtonClick;

			private void OnZoomOutButtonClick(EventArgs e)
			{
				if (this.ZoomOutButtonClick != null)
				{
					this.ZoomOutButtonClick(this, e);
				}
			}

			public event EventHandler Rotate90CWButtonClick;

			private void OnRotate90CWButtonClick(EventArgs e)
			{
				if (this.Rotate90CWButtonClick != null)
				{
					this.Rotate90CWButtonClick(this, e);
				}
			}

			public event EventHandler Rotate90CCWButtonClick;

			private void OnRotate90CCWButtonClick(EventArgs e)
			{
				if (this.Rotate90CCWButtonClick != null)
				{
					this.Rotate90CCWButtonClick(this, e);
				}
			}

			private NuGenThumbnailMode _mode;

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

						if (_mode == NuGenThumbnailMode.GridView)
						{
							_loupeModeButton.Checked = false;
							_gridModeButton.Checked = true;
							_rotateCCWButton.Visible = _rotateCWButton.Visible = false;
							_trackBar.Visible = true;
							_separators[0].Visible = false;
							_zoomInButton.Visible = _zoomOutButton.Visible = false;
						}
						else
						{
							_gridModeButton.Checked = false;
							_loupeModeButton.Checked = true;
							_rotateCCWButton.Visible = _rotateCWButton.Visible = true;
							_trackBar.Visible = false;
							_separators[0].Visible = true;
							_zoomInButton.Visible = _zoomOutButton.Visible = true;
						}

						this.OnModeChanged(new ModeEventArgs(_mode));
					}
				}
			}

			public event EventHandler<ModeEventArgs> ModeChanged;

			private void OnModeChanged(ModeEventArgs e)
			{
				if (this.ModeChanged != null)
				{
					this.ModeChanged(this, e);
				}
			}

			private int _thumbnailSize;

			public int ThumbnailSize
			{
				get
				{
					return _thumbnailSize;
				}
				set
				{
					if (_thumbnailSize != value)
					{
						_thumbnailSize = value;
						this.OnThumbnailSizeChanged(EventArgs.Empty);
						_trackBar.Value = value;
					}
				}
			}

			public event EventHandler ThumbnailSizeChanged;

			private void OnThumbnailSizeChanged(EventArgs e)
			{
				if (this.ThumbnailSizeChanged != null)
				{
					this.ThumbnailSizeChanged(this, e);
				}
			}

			private INuGenThumbnailLayoutManager _thumbnailLayoutManager;

			private INuGenThumbnailLayoutManager ThumbnailLayoutManager
			{
				get
				{
					if (_thumbnailLayoutManager == null)
					{
						Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
						_thumbnailLayoutManager = this.ServiceProvider.GetService<INuGenThumbnailLayoutManager>();

						if (_thumbnailLayoutManager == null)
						{
							throw new NuGenServiceNotFoundException<INuGenThumbnailLayoutManager>();
						}
					}

					return _thumbnailLayoutManager;
				}
			}

			private INuGenThumbnailRenderer _thumbnailRenderer;

			private INuGenThumbnailRenderer ThumbnailRenderer
			{
				get
				{
					if (_thumbnailRenderer == null)
					{
						Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
						_thumbnailRenderer = this.ServiceProvider.GetService<INuGenThumbnailRenderer>();

						if (_thumbnailRenderer == null)
						{
							throw new NuGenServiceNotFoundException<INuGenThumbnailRenderer>();
						}
					}

					return _thumbnailRenderer;
				}
			}

			private void _gridModeButton_Click(object sender, EventArgs e)
			{
				this.Mode = NuGenThumbnailMode.GridView;
			}

			private void _loupeModeButton_Click(object sender, EventArgs e)
			{
				this.Mode = NuGenThumbnailMode.LoupeView;
			}

			private void _rotateCWButton_Click(object sender, EventArgs e)
			{
				this.OnRotate90CWButtonClick(e);
			}

			private void _rotateCCWButton_Click(object sender, EventArgs e)
			{
				this.OnRotate90CCWButtonClick(e);
			}

			private void _trackBar_ValueChanged(object sender, EventArgs e)
			{
				this.ThumbnailSize = _trackBar.Value;
			}

			private void _zoomInButton_Click(object sender, EventArgs e)
			{
				this.OnZoomInButtonClick(e);
			}

			private void _zoomOutButton_Click(object sender, EventArgs e)
			{
				this.OnZoomOutButtonClick(e);
			}

			private ToolBarButton
				_gridModeButton
				, _loupeModeButton
				, _rotateCWButton
				, _rotateCCWButton
				, _zoomInButton
				, _zoomOutButton 
				;
			private ToolBarSeparatorButton[] _separators;
			private NuGenTrackBar _trackBar;

			/// <summary>
			/// Initializes a new instance of the <see cref="ToolBar"/> class.
			/// </summary>
			/// <param name="serviceProvider"><para>Requires:</para>
			///		<para><see cref="INuGenButtonStateService"/></para>
			///		<para><see cref="INuGenControlStateService"/></para>
			/// 	<para><see cref="INuGenPanelRenderer"/></para>
			///		<para><see cref="INuGenSwitchButtonLayoutManager"/></para>
			///		<para><see cref="INuGenSwitchButtonRenderer"/></para>
			///		<para><see cref="INuGenTrackBarRenderer"/></para>
			///		<para><see cref="INuGenValueTrackerService"/></para>
			///		<para><see cref="INuGenThumbnailLayoutManager"/></para>
			///		<para><see cref="INuGenThumbnailRenderer"/></para>
			/// </param>
			/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
			/// <exception cref="NuGenServiceNotFoundException"/>
			public ToolBar(INuGenServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				this.Dock = DockStyle.Bottom;
				this.Padding = new Padding(1, 2, 1, 2);

				_trackBar = new NuGenTrackBar(serviceProvider);
				_trackBar.Dock = DockStyle.Left;
				_trackBar.Minimum = 100;
				_trackBar.Maximum = 500;
				_trackBar.LargeChange = 100;
				_trackBar.SmallChange = 25;
				_trackBar.TickStyle = TickStyle.TopLeft;
				_trackBar.Width = 100;
				_trackBar.ValueChanged += _trackBar_ValueChanged;

				_rotateCWButton = new ToolBarButton(serviceProvider);
				_rotateCWButton.Click += _rotateCWButton_Click;
				_rotateCWButton.Image = this.ThumbnailRenderer.GetRotateCWImage();
				_rotateCWButton.Visible = false;

				_rotateCCWButton = new ToolBarButton(serviceProvider);
				_rotateCCWButton.Click += _rotateCCWButton_Click;
				_rotateCCWButton.Image = this.ThumbnailRenderer.GetRotateCCWImage();
				_rotateCCWButton.Visible = false;

				_separators = new ToolBarSeparatorButton[2];

				for (int i = 0; i < _separators.Length; i++)
				{
					_separators[i] = new ToolBarSeparatorButton(serviceProvider);
				}

				_separators[0].Visible = false;

				_loupeModeButton = new ToolBarButton(serviceProvider);
				_loupeModeButton.Click += _loupeModeButton_Click;
				_loupeModeButton.Image = this.ThumbnailRenderer.GetLoupeModeImage();

				_gridModeButton = new ToolBarButton(serviceProvider);
				_gridModeButton.Checked = true;
				_gridModeButton.Click += _gridModeButton_Click;
				_gridModeButton.Image = this.ThumbnailRenderer.GetGridModeImage();

				_zoomInButton = new ToolBarButton(serviceProvider);
				_zoomInButton.Click += _zoomInButton_Click;
				_zoomInButton.Image = this.ThumbnailRenderer.GetZoomInImage();
				_zoomInButton.Visible = false;

				_zoomOutButton = new ToolBarButton(serviceProvider);
				_zoomOutButton.Click += _zoomOutButton_Click;
				_zoomOutButton.Image = this.ThumbnailRenderer.GetZoomOutImage();
				_zoomOutButton.Visible = false;

				this.Controls.AddRange(
					new Control[]
					{
						_trackBar
						, _zoomInButton
						, _zoomOutButton
						, _separators[0]
						, _rotateCWButton
						, _rotateCCWButton
						, _separators[1]
						, _loupeModeButton
						, _gridModeButton
					}
				);
			}

			protected override void Dispose(bool disposing)
			{
				if (disposing)
				{
					if (_gridModeButton != null)
					{
						_gridModeButton.Click -= _gridModeButton_Click;
					}

					if (_loupeModeButton != null)
					{
						_loupeModeButton.Click -= _loupeModeButton_Click;
					}

					if (_trackBar != null)
					{
						_trackBar.ValueChanged -= _trackBar_ValueChanged;
					}

					if (_rotateCWButton != null)
					{
						_rotateCWButton.Click -= _rotateCWButton_Click;
					}

					if (_rotateCCWButton != null)
					{
						_rotateCCWButton.Click -= _rotateCCWButton_Click;
					}

					if (_zoomInButton != null)
					{
						_zoomInButton.Click -= _zoomInButton_Click;
					}

					if (_zoomOutButton != null)
					{
						_zoomOutButton.Click -= _zoomOutButton_Click;
					}
				}

				base.Dispose(disposing);
			}
		}
	}
}
