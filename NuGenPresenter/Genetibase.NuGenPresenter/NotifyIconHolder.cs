/* -----------------------------------------------
 * NotifyIconHolder.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using appBlocks = Genetibase.ApplicationBlocks;

using Genetibase.ApplicationBlocks;
using Genetibase.NuGenPresenter.Properties;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Windows;
using Genetibase.SmoothControls;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Genetibase.NuGenPresenter
{
	internal sealed class NotifyIconHolder : IDisposable
	{
		private void BeginDraw()
		{
			_drawInvokeTimer.Start();
		}

		private void BeginZoom()
		{
			_zoomInvokeTimer.Start();
		}

		private ToolStripMenuItem CreateMenuItem(string text, EventHandler onClick)
		{
			return new ToolStripMenuItem(text, null, onClick);
		}

		private ContextMenuStrip CreateContextMenuStrip()
		{
			ContextMenuStrip contextMenuStrip = new NuGenSmoothContextMenuStrip();

			_exitMenuItem = this.CreateMenuItem(Resources.Text_ContextMenu_Exit, _exitMenuItem_Click);
			_aboutMenuItem = this.CreateMenuItem(Resources.Text_ContextMenu_About, _aboutMenuItem_Click);
			_helpMenuItem = this.CreateMenuItem(Resources.Text_ContextMenu_Help, _helpMenuItem_Click);
			_optionsMenuItem = this.CreateMenuItem(Resources.Text_ContextMenu_Options, _optionsMenuItem_Click);
			_exportMenuItem = this.CreateMenuItem(Resources.Text_ContextMenu_Export, _exportMenuItem_Click);
			_breakTimerMenuItem = this.CreateMenuItem(Resources.Text_ContextMenu_BreakTimer, _breakTimerMenuItem_Click);
			_drawMenuItem = this.CreateMenuItem(Resources.Text_ContextMenu_Draw, _drawMenuItem_Click);
			_zoomMenuItem = this.CreateMenuItem(Resources.Text_ContextMenu_Zoom, _zoomMenuItem_Click);

			contextMenuStrip.Items.AddRange(
				new ToolStripItem[]
				{
					_exitMenuItem
					, new ToolStripSeparator()
					, _aboutMenuItem
					, _helpMenuItem
					, new ToolStripSeparator()					
					, _optionsMenuItem
					, _exportMenuItem
					, new ToolStripSeparator()
					, _breakTimerMenuItem
					, _drawMenuItem
					, _zoomMenuItem
				}
			);

			return contextMenuStrip;
		}

		private void ReadSettings()
		{
			_presenter.ClearHotKeys = Settings.Default.ClearHotKeys;
			_presenter.EscapeHotKeys = Settings.Default.EscapeHotKeys;
			_presenter.SaveHotKeys = Settings.Default.SaveHotKeys;
			_presenter.LockTranformHotKeys = Settings.Default.LockTransformHotKeys;
			_presenter.ShowPointerHotKeys = Settings.Default.ShowPointerHotKeys;
			_presenter.ZoomInHotKeys = Settings.Default.ZoomInHotKeys;
			_presenter.ZoomOutHotKeys = Settings.Default.ZoomOutHotKeys;
			_presenter.PenColor = Settings.Default.PenColor;
			_presenter.PenWidth = Math.Max(1, Settings.Default.PenWidth);

			_presenter.ExportPathCollection.Clear();
			StringCollection pathCollection = Settings.Default.ExportPathCollection;

			if (pathCollection != null)
			{
				foreach (string path in pathCollection)
				{
					_presenter.ExportPathCollection.Add(path);
				}
			}
			
			_presenter.ExportDialogConstrainHeigth = Settings.Default.ConstrainHeight;
			_presenter.ExportDialogConstrainWidth = Settings.Default.ConstrainWidth;
			_presenter.ExportDialogLocation = Settings.Default.ExportDialogLocation;
			_presenter.ExportDialogSize = Settings.Default.ExportDialogSize;
			_presenter.ExportDialogMaximumWidth = Math.Min(9600, Math.Max(Settings.Default.MaximumWidth, 1));
			_presenter.ExportDialogMaximumHeight = Math.Min(9600, Math.Max(Settings.Default.MaximumHeight, 1));			
			_presenter.ExportDialogNumberWatermark = Settings.Default.NumberWatermark;
			
			int thumbnailMode = Settings.Default.ThumbnailMode;

			if (Enum.IsDefined(typeof(NuGenThumbnailMode), thumbnailMode))
			{
				_presenter.ExportDialogThumbnailMode = (NuGenThumbnailMode)thumbnailMode;
			}
			else
			{
				_presenter.ExportDialogThumbnailMode = NuGenThumbnailMode.LoupeView;
			}
			
			_presenter.ExportDialogThumbnailSize = Math.Max(1, Settings.Default.ThumbnailSize);
			_presenter.ExportDialogWatermarkAlignment = Settings.Default.WatermarkAlignment;
			_presenter.ExportDialogWatermarkColor = Settings.Default.WatermarkColor;
			_presenter.ExportDialogWatermarkOpacity = Settings.Default.WatermarkColorOpacity;
			_presenter.ExportDialogWatermarkFont = Settings.Default.WatermarkFont;

			_keyInterceptor.Operations.Clear();
			_keyInterceptor.Operations.Add(
				new NuGenHotKeyOperation(
					"Draw"
					, delegate { this.BeginDraw(); }
					, Settings.Default.DrawHotKeys
				)
			);
			_keyInterceptor.Operations.Add(
				new NuGenHotKeyOperation(
					"Zoom"
					, delegate { this.BeginZoom(); }
					, Settings.Default.ZoomHotKeys
				)
			);
		}

		#region EventHandlers.ContextMenu

		private void _aboutMenuItem_Click(object sender, EventArgs e)
		{
			using (AboutForm aboutForm = new AboutForm())
			{
				aboutForm.ShowDialog();
			}
		}

		private void _breakTimerMenuItem_Click(object sender, EventArgs e)
		{
			if (Settings.Default.SlideShow)
			{
				using (SlideShowForm form = new SlideShowForm())
				{
					form.ShowDialog();
				}
			}
			else
			{
				using (BreakTimerForm form = new BreakTimerForm())
				{
					form.ShowDialog();
				}
			}
		}

		private void _drawMenuItem_Click(object sender, EventArgs e)
		{
			this.BeginDraw();
		}

		private void _exitMenuItem_Click(object sender, EventArgs e)
		{
			_presenter.UpdateTempImageCollection();
			IList<Image> images = _presenter.TempImageCollection;

			if (images.Count > 0)
			{
				DialogResult userInput = MessageBox.Show(
					Resources.Message_ExportReady
					, Resources.Message_Alert
					, MessageBoxButtons.YesNoCancel
					, MessageBoxIcon.Exclamation
				);

				if (userInput == DialogResult.Cancel)
				{
					return;
				}
				else if (userInput == DialogResult.Yes)
				{
					_presenter.Export(images);
				}
			}

			_presenter.Mode = NuGenPresenterMode.Hidden;
			_presenter.ReleaseTempImageCollection();
			_notifyIcon.Visible = false;
			_serviceProvider.Dispose();
			Application.Exit();
		}

		private void _exportMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				_presenter.UpdateTempImageCollection();
				_presenter.Export(_presenter.TempImageCollection);
			}
			catch (InvalidOperationException)
			{
				MessageBox.Show(Resources.Message_NoImagesToExport, Resources.Message_Alert, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void _helpMenuItem_Click(object sender, EventArgs e)
		{
		}

		private void _optionsMenuItem_Click(object sender, EventArgs e)
		{
			using (OptionsForm optionsForm = new OptionsForm())
			{
				if (optionsForm.ShowDialog() == DialogResult.OK)
				{
					this.ReadSettings();
				}
			}
		}

		private void _zoomMenuItem_Click(object sender, EventArgs e)
		{
			this.BeginZoom();
		}

		#endregion

		#region EventHandlers.Presenter

		private void _presenter_ExportSucceeded(object sender, EventArgs e)
		{
			Settings.Default.ConstrainHeight = _presenter.ExportDialogConstrainHeigth;
			Settings.Default.ConstrainWidth = _presenter.ExportDialogConstrainWidth;
			Settings.Default.ExportDialogLocation = _presenter.ExportDialogLocation;
			Settings.Default.ExportDialogSize = _presenter.ExportDialogSize;
			Settings.Default.ExportPathCollection = _presenter.ExportPathCollection;
			Settings.Default.MaximumHeight = _presenter.ExportDialogMaximumHeight;
			Settings.Default.MaximumWidth = _presenter.ExportDialogMaximumWidth;
			Settings.Default.NumberWatermark = _presenter.ExportDialogNumberWatermark;
			Settings.Default.ThumbnailMode = (int)_presenter.ExportDialogThumbnailMode;
			Settings.Default.ThumbnailSize = _presenter.ExportDialogThumbnailSize;
			Settings.Default.WatermarkAlignment = _presenter.ExportDialogWatermarkAlignment;
			Settings.Default.WatermarkColor = _presenter.ExportDialogWatermarkColor;
			Settings.Default.WatermarkColorOpacity = _presenter.ExportDialogWatermarkOpacity;
			Settings.Default.WatermarkFont = _presenter.ExportDialogWatermarkFont;

			Settings.Default.Save();
		}

		#endregion

		#region EventHandlers.Timer

		private void _drawInvokeTimer_Tick(object sender, EventArgs e)
		{
			_presenter.Mode = NuGenPresenterMode.Draw;
			_drawInvokeTimer.Stop();
		}

		private void _zoomInvokeTimer_Tick(object sender, EventArgs e)
		{
			_presenter.Mode = NuGenPresenterMode.Zoom;
			_zoomInvokeTimer.Stop();
		}

		#endregion

		private NuGenKeyInterceptor _keyInterceptor;
		private NotifyIcon _notifyIcon;
		private appBlocks.NuGenPresenter _presenter;
		private Timer _zoomInvokeTimer;
		private Timer _drawInvokeTimer;
		private ToolStripMenuItem
			_exitMenuItem
			, _aboutMenuItem
			, _helpMenuItem
			, _optionsMenuItem
			, _exportMenuItem
			, _breakTimerMenuItem
			, _drawMenuItem
			, _zoomMenuItem
			;
		private PresenterServiceProvider _serviceProvider;

		public NotifyIconHolder(PresenterServiceProvider serviceProvider)
		{
			Debug.Assert(serviceProvider != null, "serviceProvider != null");
			_serviceProvider = serviceProvider;
			_presenter = new appBlocks.NuGenPresenter(serviceProvider);
			_presenter.ExportDialogIcon = Resources.Bullet;
			_presenter.ExportDialogShowInTaskbar = true;
			_presenter.ExportSucceeded += _presenter_ExportSucceeded;

			_keyInterceptor = new NuGenKeyInterceptor();

			_notifyIcon = new NotifyIcon();
			_notifyIcon.Icon = Resources.Bullet;
			_notifyIcon.Text = Resources.Text_NotifyIcon_NuGenPresenter;

			_zoomInvokeTimer = new Timer();
			_zoomInvokeTimer.Tick += _zoomInvokeTimer_Tick;

			_drawInvokeTimer = new Timer();
			_drawInvokeTimer.Tick += _drawInvokeTimer_Tick;

			_notifyIcon.ContextMenuStrip = this.CreateContextMenuStrip();
			_notifyIcon.Visible = true;

			this.ReadSettings();
		}

		public void Dispose()
		{
			if (_presenter != null)
			{
				_presenter.ExportSucceeded -= _presenter_ExportSucceeded;
				_presenter.Dispose();
				_presenter = null;
			}

			if (_keyInterceptor != null)
			{
				_keyInterceptor.Operations.Clear();
				_keyInterceptor.Dispose();
				_keyInterceptor = null;
			}

			if (_drawInvokeTimer != null)
			{
				_drawInvokeTimer.Tick -= _drawInvokeTimer_Tick;
				_drawInvokeTimer.Dispose();
				_drawInvokeTimer = null;
			}

			if (_zoomInvokeTimer != null)
			{
				_zoomInvokeTimer.Tick -= _zoomInvokeTimer_Tick;
				_zoomInvokeTimer.Dispose();
				_zoomInvokeTimer = null;
			}

			if (_aboutMenuItem != null)
			{
				_aboutMenuItem.Click -= _aboutMenuItem_Click;
				_aboutMenuItem.Dispose();
				_aboutMenuItem = null;
			}

			if (_exitMenuItem != null)
			{
				_exitMenuItem.Click -= _exitMenuItem_Click;
				_exitMenuItem.Dispose();
				_exitMenuItem = null;
			}

			if (_exportMenuItem != null)
			{
				_exportMenuItem.Click -= _exportMenuItem_Click;
				_exportMenuItem.Dispose();
				_exportMenuItem = null;
			}

			if (_helpMenuItem != null)
			{
				_helpMenuItem.Click -= _helpMenuItem_Click;
				_helpMenuItem.Dispose();
				_helpMenuItem = null;
			}

			if (_optionsMenuItem != null)
			{
				_optionsMenuItem.Click -= _optionsMenuItem_Click;
				_optionsMenuItem.Dispose();
				_optionsMenuItem = null;
			}

			if (_breakTimerMenuItem != null)
			{
				_breakTimerMenuItem.Click -= _breakTimerMenuItem_Click;
				_breakTimerMenuItem.Dispose();
				_breakTimerMenuItem = null;
			}

			if (_drawMenuItem != null)
			{
				_drawMenuItem.Click -= _drawMenuItem_Click;
				_drawMenuItem.Dispose();
				_drawMenuItem = null;
			}

			if (_zoomMenuItem != null)
			{
				_zoomMenuItem.Click -= _zoomMenuItem_Click;
				_zoomMenuItem.Dispose();
				_zoomMenuItem = null;
			}

			if (_notifyIcon != null)
			{
				_notifyIcon.Dispose();
				_notifyIcon = null;
			}
		}
	}
}
