/* -----------------------------------------------
 * MainForm.cs
 * Copyright © 2006-2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Genetibase.ApplicationBlocks;
using Genetibase.NuGenVisiCalc.Expression;
using Genetibase.NuGenVisiCalc.Properties;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Windows;
using Genetibase.WinApi;
using Genetibase.Shared.Drawing;
using Genetibase.SmoothApplicationBlocks;

namespace Genetibase.NuGenVisiCalc
{
	[System.ComponentModel.DesignerCategory("Form")]
	internal sealed partial class MainForm : VisiCalcFormBase
	{
		#region Properties.Private

		/*
		 * Expression
		 */

		private String Expression
		{
			get
			{
				return _expressionCombo.Text;
			}
			set
			{
				_expressionCombo.Text = value;
			}
		}

		/*
		 * ExpressionComboMinWidth
		 */

		private Int32 ExpressionComboMinWidth
		{
			get
			{
				return 80;
			}
		}

		/*
		 * ServiceProvider
		 */

		private INuGenServiceProvider _serviceProvider;

		private INuGenServiceProvider ServiceProvider
		{
			get
			{
				return _serviceProvider;
			}
		}

		/*
		 * ZoomGroup
		 */

		private INuGenMenuItemGroup _zoomGroup;

		private INuGenMenuItemGroup ZoomGroup
		{
			get
			{
				if (_zoomGroup == null)
				{
					_zoomGroup = MenuItemCheckedTracker.CreateGroup(
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

		#region Properties.Services

		/*
		 * MenuItemCheckedTracker
		 */

		private INuGenMenuItemCheckedTracker _menuItemCheckedTracker;

		private INuGenMenuItemCheckedTracker MenuItemCheckedTracker
		{
			get
			{
				if (_menuItemCheckedTracker == null)
				{
					Debug.Assert(ServiceProvider != null, "ServiceProvider != null");
					_menuItemCheckedTracker = ServiceProvider.GetService<INuGenMenuItemCheckedTracker>();

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

		private INuGenToolStripAutoSizeService _toolStripAutoSizeService;

		private INuGenToolStripAutoSizeService ToolStripAutoSizeService
		{
			get
			{
				if (_toolStripAutoSizeService == null)
				{
					Debug.Assert(ServiceProvider != null, "ServiceProvider != null");
					_toolStripAutoSizeService = ServiceProvider.GetService<INuGenToolStripAutoSizeService>();

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

		private INuGenWindowStateTracker _windowStateTracker;

		private INuGenWindowStateTracker WindowStateTracker
		{
			get
			{
				if (_windowStateTracker == null)
				{
					Debug.Assert(ServiceProvider != null, "ServiceProvider != null");
					_windowStateTracker = ServiceProvider.GetService<INuGenWindowStateTracker>();

					if (_windowStateTracker == null)
					{
						throw new NuGenServiceNotFoundException<INuGenWindowStateTracker>();
					}
				}

				return _windowStateTracker;
			}
		}

		#endregion

		#region Methods.Private

		private void SetExpressionComboSize()
		{
			Debug.Assert(ToolStripAutoSizeService != null, "ToolStripAutoSizeService != null");
			Debug.Assert(_expressionCombo != null, "_expressionCombo != null");

			ToolStripAutoSizeService.SetNewWidth(_expressionCombo, ExpressionComboMinWidth);
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

			_canvasSizeToolStrip.CanvasSize = Settings.Default.MainForm_CanvasSize;
			_canvasSizeToolStrip.MaintainAspectRatio = Settings.Default.MainForm_MaintainCanvasSizeAspectRatio;

			Location = Settings.Default.MainForm_Location;
			Size = Settings.Default.MainForm_Size;
			WindowState = Settings.Default.MainForm_State;

			MenuItemCheckedTracker.ChangeChecked(ZoomGroup, _100zoomButton);

			StringCollection expressionHistory = Settings.Default.MainForm_ExpressionHistory;

			if (expressionHistory != null)
			{
				foreach (String expression in expressionHistory)
				{
					AddExpressionToHistory(expression);
				}
			}

			SetPropertiesFormVisible(Settings.Default.PropertiesForm_Visible);
			SetToolboxFormVisible(Settings.Default.ToolboxForm_Visible);
			SetOutputFormVisible(Settings.Default.OutputForm_Visible);
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

			Settings.Default.MainForm_State = WindowStateTracker.GetWindowState(this);

			if (WindowState == FormWindowState.Normal)
			{
				Settings.Default.MainForm_Location = Location;
				Settings.Default.MainForm_Size = Size;
			}
			else
			{
				Settings.Default.MainForm_Location = RestoreBounds.Location;
				Settings.Default.MainForm_Size = RestoreBounds.Size;
			}

			Boolean propertiesFormIsVisible = _propertiesButton.Checked;
			Boolean toolboxFormIsVisible = _toolboxButton.Checked;
			Boolean outputFormIsVisible = _outputButton.Checked;

			Settings.Default.PropertiesForm_Visible = propertiesFormIsVisible;

			if (propertiesFormIsVisible)
			{
				Settings.Default.PropertiesForm_Location = _propertiesForm.Location;
				Settings.Default.PropertiesForm_Size = _propertiesForm.Size;
			}

			Settings.Default.ToolboxForm_Visible = toolboxFormIsVisible;

			if (toolboxFormIsVisible)
			{
				Settings.Default.ToolboxForm_Location = _toolboxForm.Location;
				Settings.Default.ToolboxForm_Size = _toolboxForm.Size;
			}

			Settings.Default.OutputForm_Visible = outputFormIsVisible;

			if (outputFormIsVisible)
			{
				Settings.Default.OutputForm_Location = _outputForm.Location;
				Settings.Default.OutputForm_Size = _outputForm.Size;
			}

			Settings.Default.MainForm_ExpressionHistory = new StringCollection();

			foreach (Object expression in _expressionCombo.Items)
			{
				Settings.Default.MainForm_ExpressionHistory.Add(expression.ToString());
			}

			Settings.Default.Save();
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
			SetExpressionComboSize();
			WindowStateTracker.SetWindowState(this);
		}

		#endregion

		#region Methods.Private

		private NuGenTabPage AddCanvas(Canvas canvasToAdd)
		{
			Debug.Assert(canvasToAdd != null, "canvasToAdd != null");

			canvasToAdd.MouseDown += _canvas_MouseDown;
			canvasToAdd.Dock = DockStyle.Fill;

			return _tabbedMdi.AddTabPage(canvasToAdd, Resources.Text_Expression, Resources.Blank);
		}

		private void AddExpressionToHistory(String expression)
		{
			if (_expressionCombo.Items.Count <= 100)
			{
				foreach (Object item in _expressionCombo.Items)
				{
					if (String.Compare(item.ToString(), expression.Replace(" ", ""), StringComparison.OrdinalIgnoreCase) == 0)
					{
						return;
					}
				}

				_expressionCombo.Items.Add(expression);
			}
		}

		private void InitializePropertiesForm()
		{
			_propertiesForm = new PropertiesForm(ServiceProvider);
			_propertiesForm.FormClosed += _propertiesForm_FormClosed;
			_propertiesForm.RelinquishFocus += _relinquishFocus;
			_propertiesForm.Owner = this;
		}

		private void InitializeToolboxForm()
		{
			_toolboxForm = new ToolboxForm(ServiceProvider);
			_toolboxForm.FormClosed += _toolboxForm_FormClosed;
			_toolboxForm.RelinquishFocus += _relinquishFocus;
			_toolboxForm.Owner = this;
		}

		private void InitializeOutputForm()
		{
			_outputForm = new OutputForm();
			_outputForm.FormClosed += _outputForm_FormClosed;
			_outputForm.RelinquishFocus += _relinquishFocus;
			_outputForm.Owner = this;
		}

		private void Log(String message)
		{
			if (_outputForm != null && !_outputForm.IsDisposed)
			{
				_outputForm.Log(message);
			}
		}

		private void SetCanvasDependableButtonsVisibility(Boolean visible)
		{
			_canvasDropDownButton.Enabled = visible;
			_expressionFromDiagramButton.Enabled = visible;
			_exportToImageButton.Enabled = visible;
			_insertFromFileButton.Enabled = visible;
			_printSplitButton.Enabled = visible;
			_saveSchemaButton.Enabled = visible;
			_zoomButton.Enabled = visible;
		}

		private void SetPropertiesFormVisible(Boolean isVisible)
		{
			_propertiesButton.Checked = isVisible;

			if (isVisible)
			{
				if (_propertiesForm == null || _propertiesForm.IsDisposed)
				{
					InitializePropertiesForm();
				}

				_propertiesForm.Visible = true;
			}
		}

		private void SetToolboxFormVisible(Boolean isVisible)
		{
			_toolboxButton.Checked = isVisible;

			if (isVisible)
			{
				if (_toolboxForm == null || _toolboxForm.IsDisposed)
				{
					InitializeToolboxForm();
				}

				_toolboxForm.Visible = true;
			}
		}

		private void SetOutputFormVisible(Boolean isVisible)
		{
			_outputButton.Checked = isVisible;

			if (isVisible)
			{
				if (_outputForm == null || _outputForm.IsDisposed)
				{
					InitializeOutputForm();
				}

				_outputForm.Visible = true;
			}
		}

		#endregion

		#region EventHandlers.Canvas

		private void _canvas_MouseDown(Object sender, MouseEventArgs e)
		{
			Canvas canvas = sender as Canvas;

			if (canvas != null)
			{
				if (canvas.SelectedNode != null)
				{
					_propertiesForm.SelectObject(canvas.SelectedNode);
				}
				else
				{
					_propertiesForm.SelectObject(canvas);
				}
			}
		}

		#endregion

		#region EventHandlers.MenuItems

		private void _aboutButton_Click(Object sender, EventArgs e)
		{
			if (_aboutForm == null || _aboutForm.IsDisposed)
			{
				_aboutForm = new AboutForm();
				_aboutForm.ShowDialog(this);
			}
		}

		private void _canvasDropDownButton_DropDownClosed(Object sender, EventArgs e)
		{
			_canvasSizeToolStrip.InvokeCancelOperation();
		}

		private void _canvasDropDownButton_DropDownOpening(Object sender, EventArgs e)
		{
			_canvasSizeToolStrip.CanvasSize = _tabbedMdi.ActiveCanvas.SchemaSize;
		}

		private void _diagramFromExpressionButton_Click(Object sender, EventArgs e)
		{
			Canvas activeCanvas = _tabbedMdi.ActiveCanvas;

			if (activeCanvas == null)
			{
				activeCanvas = new Canvas();
				AddCanvas(activeCanvas);
			}

			try
			{
				ExpressionSchemaBuilder.BuildSchema(ServiceProvider, activeCanvas, Expression);
				AddExpressionToHistory(Expression);
			}
			catch (ArgumentException argEx)
			{
				Log(argEx.Message);
			}
			catch (ExpressionSyntaxException expressionEx)
			{
				Log(expressionEx.Message);
			}
		}



		private void _exportToImageButton_Click(Object sender, EventArgs e)
		{
			Canvas activeCanvas = _tabbedMdi.ActiveCanvas;

			if (activeCanvas != null)
			{
				Control schema = activeCanvas.Schema;

				using (Bitmap bmp = new Bitmap(schema.Width, schema.Height))
				{
					NuGenControlPaint.DrawToBitmap(schema, bmp);
					Application.DoEvents();
					
					using (NuGenSmoothImageExportBlock imageExport = new NuGenSmoothImageExportBlock())
					{
						Application.DoEvents();
						imageExport.ThumbnailMode = NuGenThumbnailMode.LoupeView;
						imageExport.Images.Add(bmp);

						StringCollection exportPathCollection = Settings.Default.MainForm_ExportPathCollection;

						if (exportPathCollection != null)
						{
							foreach (String path in exportPathCollection)
							{
								imageExport.ExportPathCollection.Add(path);
							}
						}

						imageExport.ShowDialog(this);
						Settings.Default.MainForm_ExportPathCollection = imageExport.ExportPathCollection;
					}
				}
			}
		}

		private void _expressionFromDiagramButton_Click(Object sender, EventArgs e)
		{
			Canvas activeCanvas = _tabbedMdi.ActiveCanvas;

			if (activeCanvas != null)
			{
				try
				{
					Expression = ExpressionSchemaBuilder.BuildExpressionFromSchema(ServiceProvider, activeCanvas);
				}
				catch (ArgumentException argEx)
				{
					Log(argEx.Message);
				}
				catch (ExpressionSyntaxException expressionEx)
				{
					Log(expressionEx.Message);
				}
			}
		}

		private void _insertFromFileButton_Click(Object sender, EventArgs e)
		{
			Canvas activeCanvas = _tabbedMdi.ActiveCanvas;

			if (activeCanvas != null)
			{
				if (_insertFileDialog.ShowDialog() == DialogResult.OK)
				{
					try
					{
						activeCanvas.InsertSchema(_insertFileDialog.FileName);
					}
					catch (Exception ex)
					{
						Log(ex.Message);
					}
				}
			}
		}

		private void _newSchemaButton_Click(Object sender, EventArgs e)
		{
			AddCanvas(new Canvas());
		}

		private void _openSchemaButton_Click(Object sender, EventArgs e)
		{
			if (_openFileDialog.ShowDialog() == DialogResult.OK)
			{
				Canvas newCanvas = new Canvas();

				try
				{
					newCanvas.OpenSchema(_openFileDialog.FileName);
					AddCanvas(newCanvas);
				}
				catch (Exception ex)
				{
					Log(ex.Message);
				}
			}
		}

		private void _propertiesButton_Click(Object sender, EventArgs e)
		{
			SetPropertiesFormVisible(true);
		}

		private void _toolboxButton_Click(Object sender, EventArgs e)
		{
			SetToolboxFormVisible(true);
		}

		private void _outputButton_Click(Object sender, EventArgs e)
		{
			SetOutputFormVisible(true);
		}

		private void _saveSchemaButton_Click(Object sender, EventArgs e)
		{
			Canvas activeCanvas = _tabbedMdi.ActiveCanvas;

			if (activeCanvas != null)
			{
				if (_saveFileDialog.ShowDialog() == DialogResult.OK)
				{
					try
					{
						activeCanvas.SaveSchema(_saveFileDialog.FileName);
					}
					catch (Exception ex)
					{
						Log(ex.Message);
					}
				}
			}
		}

		private void _zoomPercent_Click(Object sender, EventArgs e)
		{
			Debug.Assert(sender is ToolStripMenuItem, "sender is ToolStripMenuItem");
			ToolStripMenuItem menuItem = sender as ToolStripMenuItem;

			if (menuItem != null)
			{
				MenuItemCheckedTracker.ChangeChecked(ZoomGroup, menuItem);
				Canvas activeCanvas = _tabbedMdi.ActiveCanvas;

				if (activeCanvas != null)
				{
					Single scaleFactor;

					if (menuItem.Tag == null || !Single.TryParse(menuItem.Tag.ToString(), out scaleFactor))
					{
						scaleFactor = 1;
					}

					activeCanvas.Scale = scaleFactor;
				}
			}
		}

		#endregion

		#region EventHandlers.TabbedMdi

		private void _tabbedMdi_StateChanged(Object sender, MdiStateEventArgs e)
		{
			SetCanvasDependableButtonsVisibility(!e.IsEmpty);
		}

		#endregion

		#region EventHandlers.ToolWindows

		private void _propertiesForm_FormClosed(Object sender, FormClosedEventArgs e)
		{
			SetPropertiesFormVisible(false);
		}

		private void _toolboxForm_FormClosed(Object sender, FormClosedEventArgs e)
		{
			SetToolboxFormVisible(false);
		}

		private void _outputForm_FormClosed(Object sender, FormClosedEventArgs e)
		{
			SetOutputFormVisible(false);
		}

		private void _relinquishFocus(Object sender, EventArgs e)
		{
			Focus();
		}

		#endregion

		#region EventHandlers.CanvasSizeToolStrip

		private void _canvasSizeToolStrip_ApplySettings(Object sender, EventArgs e)
		{
			_canvasDropDownButton.HideDropDown();
			_tabbedMdi.ActiveCanvas.SchemaSize = _canvasSizeToolStrip.CanvasSize;
		}

		private void _canvasSizeToolStrip_CancelSettings(Object sender, EventArgs e)
		{
			_canvasDropDownButton.HideDropDown();
		}

		#endregion

		private AboutForm _aboutForm;
		private PropertiesForm _propertiesForm;
		private ToolboxForm _toolboxForm;
		private OutputForm _outputForm;

		/// <summary>
		/// Initializes a new instance of the <see cref="MainForm"/> class.
		/// </summary>
		/// <remarks>
		/// Needed for VS IDE Designer support. MainForm(INuGenServiceProvider) constructor is actually invoked.
		/// </remarks>
		public MainForm()
		{
			InitializeComponent();
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
			InitializeComponent();
			ForceActiveTitleBar = true;
			SetStyle(ControlStyles.Opaque, true);

			_canvasSizeToolStrip.ApplySettings += _canvasSizeToolStrip_ApplySettings;
			_canvasSizeToolStrip.CancelSettings += _canvasSizeToolStrip_CancelSettings;

			_insertFileDialog.Filter = Resources.Text_InsertFileDialog_Filter;
			_insertFileDialog.FilterIndex = 3;
			_insertFileDialog.Title = Resources.Text_InsertFileDialog;

			_openFileDialog.Filter = Resources.Text_OpenFileDialog_Filter;
			_openFileDialog.Title = Resources.Text_OpenFileDialog;

			_saveFileDialog.Filter = Resources.Text_SaveFileDialog_Filter;
			_saveFileDialog.Title = Resources.Text_SaveFileDialog;

			SetCanvasDependableButtonsVisibility(false);
		}
	}
}
