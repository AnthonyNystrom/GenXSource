/* -----------------------------------------------
 * NuGenDirectorySelector.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.DirectorySelectorInternals;
using Genetibase.Shared.Controls.Properties;
using Genetibase.Shared.Controls.ToolStripInternals;
using Genetibase.Shared.Controls.ToolTipInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Genetibase.Shared.Drawing;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Represents a drop-down control that persists the previously selected directories.
	/// </summary>
	[ToolboxItem(false)]
	[Designer("Genetibase.Shared.Controls.Design.NuGenDirectorySelectorDesigner")]
	[System.ComponentModel.DesignerCategory("Code")]
	public partial class NuGenDirectorySelector : NuGenControl
	{
		#region Properties.Appearance

		private bool _canChooseDirectory;

		/// <summary>
		/// Gets or sets the value indicating whether the drop-down menu contains an item that shows a <see cref="FolderBrowserDialog"/>.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_DirectorySelector_CanChooseDirectory")]
		public bool CanChooseDirectory
		{
			get
			{
				return _canChooseDirectory;
			}
			set
			{
				if (_canChooseDirectory != value)
				{
					_canChooseDirectory = value;
					this.OnCanChooseDirectoryChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _canChooseDirectoryChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="CanChooseDirectory"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_DirectorySelector_CanChooseDirectoryChanged")]
		public event EventHandler CanChooseDirectoryChanged
		{
			add
			{
				this.Events.AddHandler(_canChooseDirectoryChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_canChooseDirectoryChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="CanChooseDirectoryChanged"/> event.
		/// </summary>
		protected virtual void OnCanChooseDirectoryChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_canChooseDirectoryChanged, e);
		}

		#endregion

		#region Properties.Behavior

		private int _maxPathEntries = 6;

		/// <summary>
		/// Gets or sets the value indicating the maximum amount of entries the drop-down menu can contain.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(6)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_DirectorySelector_MaxPathEntries")]
		public int MaxPathEntries
		{
			get
			{
				return _maxPathEntries;
			}
			set
			{
				_maxPathEntries = value;
			}
		}

		#endregion

		#region Properties.NonBrowsable

		private StringCollection _pathCollection;

		/// <summary>
		/// Gets a collection of previously selected directories.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public StringCollection PathCollection
		{
			get
			{
				if (_pathCollection == null)
				{
					_pathCollection = new StringCollection();
				}

				return _pathCollection;
			}
		}

		private string _selectedPath;

		/// <summary>
		/// Gets or sets the currently selected directory. If this directory is not contained
		/// within <see cref="PathCollection"/> it is added to the collection.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string SelectedPath
		{
			get
			{
				return _selectedPath;
			}
			set
			{
				if (_selectedPath != value)
				{
					_selectedPath = value;

					if (value.Length > 0)
					{
						if (!this.PathCollection.Contains(_selectedPath))
						{
							this.PathCollection.Add(_selectedPath);
						}
						else
						{
							this.PathCollection.Remove(_selectedPath);
							this.PathCollection.Add(_selectedPath);
						}
					}

					this.Invalidate();
				}
			}
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

		#region Properties.Protected.Overridden

		private static readonly Size _defaultSize = new Size(155, 21);

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

		private INuGenButtonStateTracker _buttonStateTracker;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenButtonStateTracker ButtonStateTracker
		{
			get
			{
				if (_buttonStateTracker == null)
				{
					INuGenButtonStateService stateService = this.ServiceProvider.GetService<INuGenButtonStateService>();

					if (stateService == null)
					{
						throw new NuGenServiceNotFoundException<INuGenButtonStateService>();
					}

					_buttonStateTracker = stateService.CreateStateTracker();
				}

				return _buttonStateTracker;
			}
		}

		private INuGenDirectorySelectorRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenDirectorySelectorRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_renderer = this.ServiceProvider.GetService<INuGenDirectorySelectorRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenDirectorySelectorRenderer>();
					}
				}

				return _renderer;
			}
		}

		#endregion

		#region Methods.Public

		/// <summary>
		/// Invokes a <see cref="FolderBrowserDialog"/>.
		/// </summary>
		public void ChooseDirectory()
		{
			using (FolderBrowserDialog browserDialog = new FolderBrowserDialog())
			{
				if (browserDialog.ShowDialog() == DialogResult.OK)
				{
					this.SelectedPath = browserDialog.SelectedPath;
				}
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
			string text = this.SelectedPath;

			if (text != null)
			{
				Graphics g = e.Graphics;
				Font font = this.Font;
				Rectangle bounds = this.GetTextBounds();

				NuGenTextPaintParams pathPaintParams = new NuGenTextPaintParams(g);
				pathPaintParams.Bounds = bounds;
				pathPaintParams.Font = font;
				pathPaintParams.ForeColor = this.ForeColor;
				pathPaintParams.State = this.StateTracker.GetControlState();
				pathPaintParams.Text = text;
				pathPaintParams.TextAlign = this.RightToLeft == RightToLeft.Yes
					? ContentAlignment.MiddleRight
					: ContentAlignment.MiddleLeft
					;
				this.Renderer.DrawText(pathPaintParams);

				_toolTip.SetToolTip(
					this
					, g.MeasureString(text, font).Width > bounds.Width
						? new NuGenToolTipInfo("", null, text)
						: null
				);
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
			_dropDownButton.Bounds = this.GetDropDownButtonBounds();
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.RightToLeftChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected override void OnRightToLeftChanged(EventArgs e)
		{
			base.OnRightToLeftChanged(e);
			_dropDownButton.Bounds = this.GetDropDownButtonBounds();
		}

		#endregion

		#region Methods.Private

		private Rectangle GetDropDownButtonBounds()
		{
			return NuGenControlPaint.DropDownButtonBounds(this.ClientRectangle, this.RightToLeft);
		}

		private Rectangle GetTextBounds()
		{
			Rectangle dropDownButtonBounds = this.GetDropDownButtonBounds();

			if (this.RightToLeft == RightToLeft.Yes)
			{
				return Rectangle.FromLTRB(
					dropDownButtonBounds.Right
					, this.ClientRectangle.Top
					, this.ClientRectangle.Right
					, this.ClientRectangle.Bottom
				);
			}

			return Rectangle.FromLTRB(
				this.ClientRectangle.Left
				, this.ClientRectangle.Top
				, dropDownButtonBounds.Left
				, this.ClientRectangle.Bottom
			);
		}

		#endregion

		#region EventHandlers.ContextMenu

		private void _chooseItem_Click(object sender, EventArgs e)
		{
			this.ChooseDirectory();
		}

		private void _pathItem_Click(object sender, EventArgs e)
		{
			this.SelectedPath = ((ToolStripItem)sender).Text;
		}

		#endregion

		#region EventHandlers.DropDownButton

		private void _dropDownButton_MouseDown(object sender, MouseEventArgs e)
		{
			_contextMenu.Items.Clear();

			int pathCollectionCount = this.PathCollection.Count;
			int limit = Math.Min(_maxPathEntries, pathCollectionCount);

			for (int i = pathCollectionCount - 1; i >= pathCollectionCount - limit; i--)
			{
				_contextMenu.Items.Add(this.PathCollection[i], null, _pathItem_Click);
			}

			if (this.CanChooseDirectory)
			{
				if (_contextMenu.Items.Count > 0)
				{
					_contextMenu.Items.Add(new ToolStripSeparator());
				}

				_contextMenu.Items.Add(Resources.Text_DirectorySelector_Choose, null, _chooseItem_Click);
			}

			_contextMenu.Show(this, e.X, e.Y);
		}

		#endregion

		private DropDownButton _dropDownButton;
		private NuGenContextMenuStrip _contextMenu;
		private NuGenToolTip _toolTip;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDirectorySelector"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		///		<para><see cref="INuGenButtonStateService"/></para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		///		<para><see cref="INuGenDirectorySelectorRenderer"/></para>
		///		<para><see cref="INuGenToolStripRenderer"/></para>
		///		<para><see cref="INuGenToolTipLayoutManager"/></para>
		///		<para><see cref="INuGenToolTipRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenDirectorySelector(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.Selectable, true);
			this.SetStyle(ControlStyles.UserPaint, true);

			this.BackColor = Color.Transparent;

			_toolTip = new NuGenToolTip(serviceProvider);

			_dropDownButton = new DropDownButton(serviceProvider);
			_dropDownButton.Bounds = this.GetDropDownButtonBounds();
			_dropDownButton.MouseDown += _dropDownButton_MouseDown;
			_dropDownButton.Parent = this;

			_contextMenu = new NuGenContextMenuStrip(serviceProvider);
		}

		/// <summary>
		/// </summary>
		/// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_dropDownButton != null)
				{
					_dropDownButton.MouseDown -= _dropDownButton_MouseDown;
					_dropDownButton.Dispose();
					_dropDownButton = null;
				}
			}

			base.Dispose(disposing);
		}
	}
}
