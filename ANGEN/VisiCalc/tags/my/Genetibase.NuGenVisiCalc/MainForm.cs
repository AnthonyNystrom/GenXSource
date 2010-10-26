/* -----------------------------------------------
 * MainForm.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.NuGenVisiCalc.Properties;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Genetibase.NuGenVisiCalc
{
	/// <summary>
	/// </summary>
	public partial class MainForm : Form
	{
		#region Declarations.Fields

		private AboutForm _aboutForm = null;

		#endregion

		#region Properties.Protected

		/*
		 * MenuItemCheckedTracker
		 */

		private INuGenMenuItemCheckedTracker _menuItemCheckedTracker = null;

		/// <summary>
		/// </summary>
		protected INuGenMenuItemCheckedTracker MenuItemCheckedTracker
		{
			get
			{
				if (_menuItemCheckedTracker == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_menuItemCheckedTracker = this.ServiceProvider.GetService<INuGenMenuItemCheckedTracker>();

					if (_menuItemCheckedTracker == null)
					{
						throw new NuGenServiceNotFoundException<INuGenMenuItemCheckedTracker>();
					}
				}

				return _menuItemCheckedTracker;
			}
		}

		/*
		 * ToolStripAutoSizeService
		 */

		private INuGenToolStripAutoSizeService _toolStripAutoSizeService = null;

		/// <summary>
		/// </summary>
		protected INuGenToolStripAutoSizeService ToolStripAutoSizeService
		{
			get
			{
				if (_toolStripAutoSizeService == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_toolStripAutoSizeService = this.ServiceProvider.GetService<INuGenToolStripAutoSizeService>();

					if (_toolStripAutoSizeService == null)
					{
						throw new NuGenServiceNotFoundException<INuGenToolStripAutoSizeService>();
					}
				}

				return _toolStripAutoSizeService;
			}
		}

		/*
		 * WindowStateTracker
		 */

		private INuGenWindowStateTracker _windowStateTracker = null;

		/// <summary>
		/// </summary>
		protected INuGenWindowStateTracker WindowStateTracker
		{
			get
			{
				if (_windowStateTracker == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_windowStateTracker = this.ServiceProvider.GetService<INuGenWindowStateTracker>();

					if (_windowStateTracker == null)
					{
						throw new NuGenServiceNotFoundException<INuGenWindowStateTracker>();
					}
				}

				return _windowStateTracker;
			}
		}

		/*
		 * ZoomGroup
		 */

		private INuGenMenuItemGroup _zoomGroup = null;

		/// <summary>
		/// </summary>
		protected INuGenMenuItemGroup ZoomGroup
		{
			get
			{
				if (_zoomGroup == null)
				{
					_zoomGroup = this.MenuItemCheckedTracker.CreateGroup(
						new ToolStripMenuItem[] {
							_50zoomButton,
							_75zoomButton,
							_100zoomButton,
							_125zoomButton,
							_150zoomButton,
							_200zoomButton
						}
					);
				}

				return _zoomGroup;
			}
		}

		#endregion

		#region Properties.Protected.Virtual

		/*
		 * ExpressionComboMinWidth
		 */

		/// <summary>
		/// Read-only.
		/// </summary>
		protected virtual int ExpressionComboMinWidth
		{
			get
			{
				return 80;
			}
		}

		/*
		 * ServiceProvider
		 */

		private INuGenServiceProvider _serviceProvider = null;

		/// <summary>
		/// </summary>
		protected virtual INuGenServiceProvider ServiceProvider
		{
			get
			{
				return _serviceProvider;
			}
		}

		#endregion

		#region Methods.Protected

		/*
		 * SetExpressionComboSize
		 */

		/// <summary>
		/// </summary>
		protected void SetExpressionComboSize()
		{
			Debug.Assert(this.ToolStripAutoSizeService != null, "this.ToolStripAutoSizeService != null");
			Debug.Assert(_expressionCombo != null, "_expressionCombo != null");

			this.ToolStripAutoSizeService.SetNewWidth(_expressionCombo, this.ExpressionComboMinWidth);
		}

		#endregion

		#region Methods.Protected.Overriden

		/*
		 * OnLoad
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Form.Load"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			
			this.Location = Settings.Default.MainForm_Location;
			this.Size = Settings.Default.MainForm_Size;
			this.WindowState = Settings.Default.MainForm_State;

			_100zoomButton.Checked = true;
		}

		/*
		 * OnFormClosing
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Form.FormClosing"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.FormClosingEventArgs"></see> that contains the event data.</param>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);

			Settings.Default.MainForm_State = this.WindowStateTracker.GetWindowState(this);

			if (this.WindowState == FormWindowState.Normal)
			{
				Settings.Default.MainForm_Location = this.Location;
				Settings.Default.MainForm_Size = this.Size;
			}
			else
			{
				Settings.Default.MainForm_Location = this.RestoreBounds.Location;
				Settings.Default.MainForm_Size = this.RestoreBounds.Size;
			}

			// TODO: Turn on in final release.
			// Settings.Default.Save();
		}

		/*
		 * OnSizeChanged
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.SizeChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);
			this.SetExpressionComboSize();
			this.WindowStateTracker.SetWindowState(this);
		}

		#endregion

		#region EventHandlers

		private void _aboutButton_Click(object sender, EventArgs e)
		{
			if (
				_aboutForm == null
				|| _aboutForm.IsDisposed
				)
			{
				_aboutForm = new AboutForm();
				_aboutForm.FormClosed += delegate
				{
					_aboutForm.Dispose();
				};

				_aboutForm.ShowDialog(this);
			}
		}

		private void _newSchemaButton_Click(object sender, EventArgs e)
		{
			UserControl pageContent = new UserControl();
			pageContent.Dock = DockStyle.Fill;

			_tabbedMdi.AddTabPage(pageContent, "Expression", Resources.Blank);
		}

		private void _zoomPercent_CheckedChanged(object sender, EventArgs e)
		{
			Debug.Assert(sender is ToolStripMenuItem, "sender is ToolStripMenuItem");
			this.MenuItemCheckedTracker.CheckedChanged(this.ZoomGroup, (ToolStripMenuItem)sender);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="MainForm"/> class.
		/// </summary>
		public MainForm()
		{
			this.InitializeComponent();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MainForm"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="serviceProvider"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public MainForm(INuGenServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			_serviceProvider = serviceProvider;

			this.InitializeComponent();
		}

		#endregion
	}
}
