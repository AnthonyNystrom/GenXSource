/* -----------------------------------------------
 * ImageExportDialog.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using res = Genetibase.ApplicationBlocks.Properties.Resources;

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ButtonInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks.ImageExportInternals
{
	partial class ImageExportDialog
	{
		private sealed class ControlPanel : UserControl
		{
			public event EventHandler Back;

			private void OnBack(EventArgs e)
			{
				if (this.Back != null)
				{
					this.Back(this, e);
				}
			}

			public event EventHandler Cancel;

			private void OnCancel(EventArgs e)
			{
				if (this.Cancel != null)
				{
					this.Cancel(this, e);
				}
			}

			public event EventHandler CancelExport;

			private void OnCancelExport(EventArgs e)
			{
				if (this.CancelExport != null)
				{
					this.CancelExport(this, e);
				}
			}

			public event EventHandler Close;

			private void OnClose(EventArgs e)
			{
				if (this.Close != null)
				{
					this.Close(this, e);
				}
			}

			public event EventHandler Export;

			private void OnExport(EventArgs e)
			{
				if (this.Export != null)
				{
					this.Export(this, e);
				}
			}

			public event EventHandler Next;

			private void OnNext(EventArgs e)
			{
				if (this.Next != null)
				{
					this.Next(this, e);
				}
			}

			public bool BackVisible
			{
				get
				{
					return _backButton.Visible;
				}
				set
				{
					_backButton.Visible = value;
				}
			}

			public bool CancelVisible
			{
				get
				{
					return _cancelButton.Visible;
				}
				set
				{
					_cancelButton.Visible = value;
				}
			}

			public bool CancelExportVisible
			{
				get
				{
					return _cancelExportButton.Visible;
				}
				set
				{
					_cancelExportButton.Visible = value;
				}
			}

			public bool CloseVisible
			{
				get
				{
					return _closeButton.Visible;
				}
				set
				{
					_closeButton.Visible = value;
				}
			}

			public bool ExportVisible
			{
				get
				{
					return _exportButton.Visible;
				}
				set
				{
					_exportButton.Visible = value;
				}
			}

			public bool NextVisible
			{
				get
				{
					return _nextButton.Visible;
				}
				set
				{
					_nextButton.Visible = value;
				}
			}

			private static readonly Size _defaultSize = new Size(150, 35);

			protected override Size DefaultSize
			{
				get
				{
					return _defaultSize;
				}
			}

			private void _button_Click(object sender, EventArgs e)
			{
				ControlButton button = (ControlButton)sender;

				switch (button.Action)
				{
					case ControlAction.Back:
					{
						this.OnBack(EventArgs.Empty);
						break;
					}
					case ControlAction.Cancel:
					{
						this.OnCancel(EventArgs.Empty);
						break;
					}
					case ControlAction.CancelExport:
					{
						this.OnCancelExport(EventArgs.Empty);
						break;
					}
					case ControlAction.Close:
					{
						this.OnClose(EventArgs.Empty);
						break;
					}
					case ControlAction.Export:
					{
						this.OnExport(EventArgs.Empty);
						break;
					}
					case ControlAction.Next:
					{
						this.OnNext(EventArgs.Empty);
						break;
					}
				}
			}

			private ControlButton 
				_backButton
				, _cancelButton
				, _cancelExportButton
				, _closeButton
				, _exportButton
				, _nextButton
				;

			/// <summary>
			/// Initializes a new instance of the <see cref="ControlPanel"/> class.
			/// </summary>
			/// <param name="serviceProvider">
			/// <para>Requires:</para>
			/// <para><see cref="INuGenButtonLayoutManager"/></para>
			/// <para><see cref="INuGenButtonRenderer"/></para>
			/// </param>
			/// <exception cref="ArgumentNullException">
			/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
			/// </exception>
			public ControlPanel(INuGenServiceProvider serviceProvider)
			{
				if (serviceProvider == null)
				{
					throw new ArgumentNullException("serviceProvider");
				}

				this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
				
				this.BackColor = Color.Transparent;
				this.Dock = DockStyle.Bottom;
				this.Padding = new Padding(0, 5, 0, 0);
				this.SuspendLayout();

				this.Controls.AddRange(
					new Control[]
					{
						_backButton = new ControlButton(serviceProvider, ControlAction.Back)
						, _nextButton = new ControlButton(serviceProvider, ControlAction.Next)
						, _exportButton = new ControlButton(serviceProvider, ControlAction.Export)
						, _cancelButton = new ControlButton(serviceProvider, ControlAction.Cancel)
						, _cancelExportButton = new ControlButton(serviceProvider, ControlAction.CancelExport)
						, _closeButton = new ControlButton(serviceProvider, ControlAction.Close)
					}
				);

				_backButton.Text = res.Text_ImageExportDialog_backButton;
				_backButton.Visible = false;

				_nextButton.Text = res.Text_ImageExportDialog_nextButton;

				_exportButton.Text = res.Text_ImageExportDialog_exportButton;
				_exportButton.Visible = false;

				_cancelButton.Text = res.Text_ImageExportDialog_cancelButton;

				_cancelExportButton.Text = res.Text_ImageExportDialog_cancelExportButton;
				_cancelExportButton.Visible = false;

				_closeButton.Text = res.Text_ImageExportDialog_closeButton;
				_closeButton.Visible = false;

				foreach (Control ctrl in this.Controls)
				{
					NuGenButton button = ctrl as NuGenButton;

					if (button != null)
					{
						button.Click += _button_Click;
						button.Dock = DockStyle.Right;
					}
				}

				this.ResumeLayout(true);
			}

			private bool _isDisposed;

			protected override void Dispose(bool disposing)
			{
				if (disposing)
				{
					if (!_isDisposed)
					{
						_isDisposed = true;

						foreach (Control ctrl in this.Controls)
						{
							NuGenButton button = ctrl as NuGenButton;

							if (button != null)
							{
								button.Click -= _button_Click;
							}
						}
					}
				}

				base.Dispose(disposing);
			}
		}
	}
}
