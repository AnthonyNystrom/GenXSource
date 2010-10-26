/* -----------------------------------------------
 * ImageExportDialog.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using res = Genetibase.ApplicationBlocks.Properties.Resources;

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ButtonInternals;
using Genetibase.Shared.Controls.CheckBoxInternals;
using Genetibase.Shared.Controls.ColorBoxInternals;
using Genetibase.Shared.Controls.ComboBoxInternals;
using Genetibase.Shared.Controls.DirectorySelectorInternals;
using Genetibase.Shared.Controls.DropDownInternals;
using Genetibase.Shared.Controls.FontBoxInternals;
using Genetibase.Shared.Controls.ListBoxInternals;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.ProgressBarInternals;
using Genetibase.Shared.Controls.RadioButtonInternals;
using Genetibase.Shared.Controls.ScrollBarInternals;
using Genetibase.Shared.Controls.SpinInternals;
using Genetibase.Shared.Controls.SwitcherInternals;
using Genetibase.Shared.Controls.TabControlInternals;
using Genetibase.Shared.Controls.TextBoxInternals;
using Genetibase.Shared.Controls.ToolStripInternals;
using Genetibase.Shared.Controls.TrackBarInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;
using Genetibase.WinApi;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks.ImageExportInternals
{
	internal sealed partial class ImageExportDialog : NuGenForm
	{
		#region Properties.Public

		private Step _currentStep;

		public Step CurrentStep
		{
			get
			{
				return _currentStep;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				_currentStep = value;
				_currentStep.Visible = true;
				_currentStep.BringToFront();
				_title.Caption = _currentStep.Caption;
				_title.Description = _currentStep.Description;
			}
		}

		public NuGenThumbnailContainer.ImageCollection Images
		{
			get
			{
				return _thumbnailContainer.Images;
			}
		}

		public StringCollection PathCollection
		{
			get
			{
				return _pathSelector.PathCollection;
			}
		}

		public bool ConstrainWidth
		{
			get
			{
				return _maxWidthCheckBox.Checked;
			}
			set
			{
				_maxWidthCheckBox.Checked = value;
			}
		}

		public int MaximumWidth
		{
			get
			{
				return _maxWidthSpin.Value;
			}
			set
			{
				_maxWidthSpin.Value = value;
			}
		}

		public bool ConstrainHeight
		{
			get
			{
				return _maxHeightCheckBox.Checked;
			}
			set
			{
				_maxHeightCheckBox.Checked = value;
			}
		}

		public int MaximumHeight
		{
			get
			{
				return _maxHeightSpin.Value;
			}
			set
			{
				_maxHeightSpin.Value = value;
			}
		}

		public bool NumberWatermark
		{
			get
			{
				return _numWatermarkCheckBox.Checked;
			}
			set
			{
				_numWatermarkCheckBox.Checked = value;
			}
		}

		public NuGenThumbnailMode ThumbnailMode
		{
			get
			{
				return _thumbnailContainer.Mode;
			}
			set
			{
				_thumbnailContainer.Mode = value;
			}
		}

		public int ThumbnailSize
		{
			get
			{
				return _thumbnailContainer.ThumbnailSize;
			}
			set
			{
				_thumbnailContainer.ThumbnailSize = value;
			}
		}

		public Font WatermarkFont
		{
			get
			{
				return _watermarkFontBlock.SelectedFont;
			}
			set
			{
				_watermarkFontBlock.SelectedFont = value;
			}
		}

		public Color WatermarkColor
		{
			get
			{
				return _watermarkColorBox.SelectedColor;
			}
			set
			{
				_watermarkColorBox.SelectedColor = value;
			}
		}

		public int WatermarkColorOpacity
		{
			get
			{
				return _watermarkOpacitySpin.Value;
			}
			set
			{
				_watermarkOpacitySpin.Value = value;
			}
		}

		public ContentAlignment WatermarkAlignment
		{
			get
			{
				return _watermarkAlignDropDown.SelectedAlignment;
			}
			set
			{
				_watermarkAlignDropDown.SelectedAlignment = value;
			}
		}

		#endregion

		#region Properties.Protected.Overridden

		private static readonly Size _defaultSize = new Size(640, 480);

		protected override Size DefaultSize
		{
			get
			{
				return _defaultSize;
			}
		}

		#endregion

		#region Properties.Services

		private INuGenPanelRenderer _renderer;

		private INuGenPanelRenderer Renderer
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

		protected override void OnPaint(PaintEventArgs e)
		{
			NuGenPaintParams paintParams = new NuGenPaintParams(e.Graphics);
			paintParams.Bounds = this.ClientRectangle;
			paintParams.State = NuGenControlState.Normal;

			this.Renderer.DrawExtendedBackground(paintParams);
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WinUser.WM_CLOSE)
			{
				this.Visible = false;
				this.SetPreviewStep();
				return;
			}

			base.WndProc(ref m);
		}

		#endregion

		#region Methods.Private

		private void ExportImage(Image image)
		{
			Debug.Assert(image != null, "image != null");

			_exportService.Export(
				image
				, _exportImageType
				, _exportFileFormat
				, this.GetImageSize(image.Size)
				, _exportPath
				, _exportTemplate
			);
		}

		private void ExportImageWithWatermark(Image image, int watermarkCount)
		{
			Debug.Assert(image != null, "image != null");

			_exportService.ExportWithWatermark(
				image
				, _exportImageType
				, _exportFileFormat
				, this.GetImageSize(image.Size)
				, watermarkCount
				, _exportWatermarkFont
				, _exportWatermarkColor
				, _exportWatermarkAlign
				, _exportPath
				, _exportTemplate
			);
		}

		private void ExportImages()
		{
			for (int i = 0; i < _exportImages.Length; i++)
			{
				if (_exportNumWatermark)
				{
					this.ExportImageWithWatermark(_exportImages[i], i + 1);
				}
				else
				{
					this.ExportImage(_exportImages[i]);
				}

				if (_shouldCancel)
				{
					break;
				}
			}
		}

		private Size GetImageSize(Size originalImageSize)
		{
			NuGenRatioSizeTracker tracker = new NuGenRatioSizeTracker(originalImageSize);

			int maxWidth = _maxWidthCheckBox.Checked ? _maxWidthSpin.Value : originalImageSize.Width;
			int maxHeight = _maxHeightCheckBox.Checked ? _maxHeightSpin.Value : originalImageSize.Height;

			tracker.MaintainAspectRatio = true;
			tracker.Width = maxWidth;

			if (tracker.Height > maxHeight)
			{
				tracker.Height = maxHeight;
			}

			return tracker.Size;
		}

		private void SetExportParams(
			NuGenProgressBar progressBar
			, Image[] images
			, NuGenImageType imageType
			, NuGenImageFileFormat fileFormat
			, bool numWatermark
			, Font watermarkFont
			, Color watermarkColor
			, ContentAlignment watermarkAlign
			, string path
			, string template
			)
		{
			Debug.Assert(progressBar != null, "progressBar != null");
			Debug.Assert(images != null, "images != null");

			_exportProgressBar.Minimum = 0;
			_exportProgressBar.Maximum =
				images.Length
				* NuGenEnum.FlagsSetOn(imageType)
				* NuGenEnum.FlagsSetOn(fileFormat)
				;
			_exportProgressBar.Style = NuGenProgressBarStyle.Marquee;
			_exportProgressBar.Value = 0;

			_exportImages = images;
			_exportImageType = imageType;
			_exportFileFormat = fileFormat;
			_exportNumWatermark = numWatermark;
			_exportWatermarkFont = watermarkFont;
			_exportWatermarkColor = watermarkColor;
			_exportWatermarkAlign = watermarkAlign;
			_exportPath = path;
			_exportTemplate = template;
		}

		#endregion

		#region Methods.Steps

		private void SetExportStep()
		{
			_exportProgressBar.Visible = true;

			_controlPanel.BackVisible = false;
			_controlPanel.CancelVisible = false;
			_controlPanel.CancelExportVisible = true;
			_controlPanel.CloseVisible = false;
			_controlPanel.ExportVisible = false;
			_controlPanel.NextVisible = false;
		}

		private void SetFinishStep()
		{
			_controlPanel.BackVisible = true;
			_controlPanel.CancelVisible = false;
			_controlPanel.CancelExportVisible = false;
			_controlPanel.CloseVisible = true;
			_controlPanel.ExportVisible = true;
			_controlPanel.NextVisible = false;

			_exportProgressBar.Visible = false;
			this.CurrentStep = _settingsStep;
		}

		private void SetPreviewStep()
		{
			_controlPanel.BackVisible = false;
			_controlPanel.CancelVisible = true;
			_controlPanel.CloseVisible = false;
			_controlPanel.ExportVisible = false;
			_controlPanel.NextVisible = true;

			_exportProgressBar.Visible = false;
			this.CurrentStep = _previewStep;
		}

		private void SetSettingsStep()
		{
			_shouldCancel = false;

			_controlPanel.BackVisible = true;
			_controlPanel.CancelVisible = true;
			_controlPanel.CloseVisible = false;
			_controlPanel.ExportVisible = true;
			_controlPanel.NextVisible = false;

			_exportProgressBar.Visible = false;
			this.CurrentStep = _settingsStep;
		}

		#endregion

		#region EventHandlers.Controls

		private void _chooseButton_Click(object sender, EventArgs e)
		{
			_pathSelector.ChooseDirectory();
		}

		private void _maxHeightCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			_maxHeightSpin.Enabled = _maxHeightCheckBox.Checked;
		}

		private void _maxWidthCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			_maxWidthSpin.Enabled = _maxWidthCheckBox.Checked;
		}

		private void _numWatermarkCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			_colorPanel.Enabled = _watermarkAlignDropDown.Enabled = _watermarkFontBlock.Enabled = _numWatermarkCheckBox.Checked;
		}

		#endregion

		#region EventHandlers.ControlPanel

		private void _controlPanel_Back(object sender, EventArgs e)
		{
			this.SetPreviewStep();
		}

		private void _controlPanel_Cancel(object sender, EventArgs e)
		{
			this.Hide();
			this.SetPreviewStep();
		}

		private void _controlPanel_CancelExport(object sender, EventArgs e)
		{
			_shouldCancel = true;
		}

		private void _controlPanel_Close(object sender, EventArgs e)
		{
			this.Hide();
			this.SetPreviewStep();
		}

		private void _controlPanel_Export(object sender, EventArgs e)
		{
			if (!NuGenArgument.IsValidDirectoryName(_pathSelector.SelectedPath))
			{
				MessageBox.Show(
					string.Format(res.Argument_InvalidDirectory, new string(Path.GetInvalidPathChars()))
					, res.Message_Alert
					, MessageBoxButtons.OK
					, MessageBoxIcon.Exclamation
				);
				return;
			}

			if (!NuGenArgument.IsValidFileName(_templateTextBox.Text))
			{
				MessageBox.Show(
					string.Format(res.Argument_InvalidFilename, new string(Path.GetInvalidFileNameChars()))
					, res.Message_Alert
					, MessageBoxButtons.OK
					, MessageBoxIcon.Exclamation
				);

				_templateTextBox.SelectAll();
				_templateTextBox.Focus();
				return;
			}

			this.SetExportStep();
			Image[] images;

			if (_thumbnailContainer.SelectedImages.Count > 0)
			{
				images = new Image[_thumbnailContainer.SelectedImages.Count];
				_thumbnailContainer.SelectedImages.CopyTo(images, 0);
			}
			else
			{
				images = new Image[_thumbnailContainer.Images.Count];
				_thumbnailContainer.Images.CopyTo(images, 0);
			}

			this.SetExportParams(
				_exportProgressBar
				, images
				, _typeCombo.ImageType
				, _formatCombo.FileFormat
				, _numWatermarkCheckBox.Checked
				, _watermarkFontBlock.SelectedFont
				, NuGenControlPaint.ColorFromArgb(100 - _watermarkOpacitySpin.Value, _watermarkColorBox.SelectedColor)				
				, _watermarkAlignDropDown.SelectedAlignment
				, _pathSelector.SelectedPath
				, _templateTextBox.Text
			);

			MethodInvoker methodInvoker = new MethodInvoker(this.ExportImages);
			methodInvoker.BeginInvoke(
				new AsyncCallback(
					delegate
					{
						this.BeginInvoke(
							new MethodInvoker(
								delegate
								{
									this.SetFinishStep();
								}
							)
						);
					}
				)
				, null
			);
		}

		private void _controlPanel_Next(object sender, EventArgs e)
		{
			this.SetSettingsStep();
		}

		#endregion

		#region EventHandlers.ExportService

		private void _exportProgress(object sender, CancelEventArgs e)
		{
			e.Cancel = _shouldCancel;

			if (_exportProgressBar.IsHandleCreated)
			{
				this.BeginInvoke(new MethodInvoker(delegate
				{
					_exportProgressBar.Style = NuGenProgressBarStyle.Continuous;
					_exportProgressBar.Value++;
				}));
			}
		}

		#endregion

		private NuGenThumbnailContainer _thumbnailContainer;
		private ControlPanel _controlPanel;
		private Step _previewStep;
		private Step _settingsStep;
		private Title _title;
		private TableLayoutPanel _settingsLayoutPanel;
		private NuGenLabel
			_destFolderLabel
			, _templateLabel
			, _formatLabel
			, _typeLabel
			, _percentLabel
			;
		private NuGenCheckBox
			_maxWidthCheckBox
			, _maxHeightCheckBox
			, _numWatermarkCheckBox
			;
		private SizeSpin
			_maxWidthSpin
			, _maxHeightSpin
			;
		private PathSelector _pathSelector;
		private NuGenButton _chooseButton;
		private LayoutPanel
			_pathPanel
			, _colorPanel
			;
		private FormatCombo _formatCombo;
		private TypeCombo _typeCombo;
		private NuGenTextBox _templateTextBox;
		private NuGenProgressBar _exportProgressBar;
		private NuGenFontBlock _watermarkFontBlock;
		private NuGenColorBox _watermarkColorBox;
		private OpacitySpin _watermarkOpacitySpin;
		private NuGenAlignDropDown _watermarkAlignDropDown;

		private ImageExportService _exportService;
		private Image[] _exportImages;
		private NuGenImageType _exportImageType;
		private NuGenImageFileFormat _exportFileFormat;
		private bool _exportNumWatermark;
		private string _exportPath;
		private string _exportTemplate;
		private Color _exportWatermarkColor;
		private Font _exportWatermarkFont;
		private ContentAlignment _exportWatermarkAlign;
		private bool _shouldCancel;

		private delegate void ExportDelegate(
			Image image,
			NuGenImageType imageType,
			NuGenImageFileFormat fileFormat,
			Size resolution,
			string path,
			string filename
		);

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageExportDialog"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		///		<para>Requires:</para>
		/// 	<para><see cref="INuGenButtonStateService"/></para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		///		<para><see cref="INuGenButtonLayoutManager"/></para>
		///		<para><see cref="INuGenButtonRenderer"/></para>
		///		<para><see cref="INuGenCheckBoxLayoutManager"/></para>
		///		<para><see cref="INuGenCheckBoxRenderer"/></para>
		///		<para><see cref="INuGenColorsProvider"/></para>
		///		<para><see cref="INuGenComboBoxRenderer"/></para>
		///		<para><see cref="INuGenControlImageManager"/></para>
		///		<para><see cref="INuGenDirectorySelectorRenderer"/></para>
		///		<para><see cref="INuGenDropDownRenderer"/></para>
		///		<para><see cref="INuGenFontFamiliesProvider"/></para>
		///		<para><see cref="INuGenImageListService"/></para>
		/// 	<para><see cref="INuGenListBoxRenderer"/></para>
		///		<para><see cref="INuGenPanelRenderer"/></para>
		///		<para><see cref="INuGenProgressBarLayoutManager"/></para>
		///		<para><see cref="INuGenProgressBarRenderer"/></para>
		///		<para><see cref="INuGenRadioButtonLayoutManager"/></para>
		///		<para><see cref="INuGenRadioButtonRenderer"/></para>
		/// 	<para><see cref="INuGenScrollBarRenderer"/></para>
		///		<para><see cref="INuGenSpinRenderer"/></para>
		/// 	<para><see cref="INuGenSwitchButtonLayoutManager"/></para>
		/// 	<para><see cref="INuGenSwitchButtonRenderer"/></para>
		///		<para><see cref="INuGenTabStateTracker"/></para>
		///		<para><see cref="INuGenTabLayoutManager"/></para>
		///		<para><see cref="INuGenTabRenderer"/></para>
		///		<para><see cref="INuGenTextBoxRenderer"/></para>
		/// 	<para><see cref="INuGenTrackBarRenderer"/></para>
		/// 	<para><see cref="INuGenThumbnailLayoutManager"/></para>
		/// 	<para><see cref="INuGenThumbnailRenderer"/></para>
		/// 	<para><see cref="INuGenToolStripRenderer"/></para>
		/// 	<para><see cref="INuGenValueTrackerService"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public ImageExportDialog(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.UserPaint, true);

			this.FormBorderStyle = FormBorderStyle.Sizable;
			this.MinimizeBox = false;
			this.MaximizeBox = false;
			this.Padding = new Padding(5);
			this.ShowInTaskbar = false;
			this.StartPosition = FormStartPosition.Manual;
			this.Text = res.Text_ImageExportDialog_Text;

			_exportService = new ImageExportService();
			_exportService.Progress += _exportProgress;

			_controlPanel = new ControlPanel(serviceProvider);
			_controlPanel.Back += _controlPanel_Back;
			_controlPanel.Cancel += _controlPanel_Cancel;
			_controlPanel.CancelExport += _controlPanel_CancelExport;
			_controlPanel.Close += _controlPanel_Close;
			_controlPanel.Export += _controlPanel_Export;
			_controlPanel.Next += _controlPanel_Next;

			_previewStep = new Step(
				res.Text_ImageExportDialog_StepOneCaption
				, res.Text_ImageExportDialog_StepOneDescription
			);
			_settingsStep = new Step(
				res.Text_ImageExportDialog_StepTwoCaption
				, res.Text_ImageExportDialog_StepTwoDescription
			);
			_title = new Title();

			this.Controls.AddRange(
				new Control[] 
				{
					_controlPanel
					, _previewStep
					, _settingsStep
					, _title
				}
			);

			_thumbnailContainer = new NuGenThumbnailContainer(serviceProvider);
			_thumbnailContainer.Dock = DockStyle.Fill;
			_thumbnailContainer.Parent = _previewStep;

			_exportProgressBar = new NuGenProgressBar(serviceProvider);
			_exportProgressBar.Dock = DockStyle.Bottom;
			_exportProgressBar.Parent = _settingsStep;

			_settingsLayoutPanel = new TableLayoutPanel();
			_settingsLayoutPanel.BackColor = Color.Transparent;
			_settingsLayoutPanel.Dock = DockStyle.Fill;
			_settingsLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
			_settingsLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));

			for (int i = 0; i < 9; i++)
			{
				_settingsLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
			}

			_settingsLayoutPanel.Parent = _settingsStep;
			NuGenControlPaint.SetStyle(
				_settingsLayoutPanel
				, ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint
				, true
			);

			_destFolderLabel = new NuGenLabel();
			_destFolderLabel.Text = res.Text_ImageExportDialog_destFolderLabel;

			_templateLabel = new NuGenLabel();
			_templateLabel.Text = res.Text_ImageExportDialog_templateLabel;

			_formatLabel = new NuGenLabel();
			_formatLabel.Text = res.Text_ImageExportDialog_formatLabel;

			_typeLabel = new NuGenLabel();
			_typeLabel.Text = res.Text_ImageExportDialog_typeLabel;

			_chooseButton = new NuGenButton(serviceProvider);
			_chooseButton.Click += _chooseButton_Click;
			_chooseButton.Dock = DockStyle.Right;
			_chooseButton.Text = res.Text_ImageExportDialog_chooseButton;

			_pathSelector = new PathSelector(serviceProvider);
			_pathSelector.Dock = DockStyle.Fill;

			_maxHeightCheckBox = new NuGenCheckBox(serviceProvider);
			_maxHeightCheckBox.Text = res.Text_ImageExportDialog_maxHeightCheckBox;
			_maxHeightCheckBox.CheckedChanged += _maxHeightCheckBox_CheckedChanged;

			_maxWidthCheckBox = new NuGenCheckBox(serviceProvider);
			_maxWidthCheckBox.Text = res.Text_ImageExportDialog_maxWidthCheckBox;
			_maxWidthCheckBox.CheckedChanged += _maxWidthCheckBox_CheckedChanged;

			_numWatermarkCheckBox = new NuGenCheckBox(serviceProvider);
			_numWatermarkCheckBox.CheckedChanged += _numWatermarkCheckBox_CheckedChanged;
			_numWatermarkCheckBox.Text = res.Text_ImageExportDialog_numWatermarkCheckBox;

			_templateTextBox = new NuGenTextBox(serviceProvider);
			_formatCombo = new FormatCombo(serviceProvider);
			_typeCombo = new TypeCombo(serviceProvider);

			_settingsLayoutPanel.Controls.Add(_destFolderLabel, 0, 0);
			_settingsLayoutPanel.Controls.Add(_templateLabel, 0, 1);
			_settingsLayoutPanel.Controls.Add(_formatLabel, 0, 2);
			_settingsLayoutPanel.Controls.Add(_typeLabel, 0, 3);
			_settingsLayoutPanel.Controls.Add(_maxWidthCheckBox, 0, 4);
			_settingsLayoutPanel.Controls.Add(_maxHeightCheckBox, 0, 5);
			_settingsLayoutPanel.Controls.Add(_numWatermarkCheckBox, 0, 6);

			_maxHeightSpin = new SizeSpin(serviceProvider);
			_maxHeightSpin.Enabled = _maxHeightCheckBox.Checked;

			_maxWidthSpin = new SizeSpin(serviceProvider);
			_maxWidthSpin.Enabled = _maxWidthCheckBox.Checked;

			_pathPanel = new LayoutPanel();
			_pathPanel.Dock = DockStyle.Fill;
			_pathPanel.Controls.AddRange(new Control[] { _pathSelector, _chooseButton });

			_watermarkFontBlock = new NuGenFontBlock(serviceProvider);
			_watermarkFontBlock.Enabled = false;
			_watermarkFontBlock.Width = 300;

			NuGenSpacer[] colorSpacers = new NuGenSpacer[2];

			for (int i = 0; i < colorSpacers.Length; i++)
			{
				colorSpacers[i] = new NuGenSpacer();
				colorSpacers[i].Dock = DockStyle.Right;
				colorSpacers[i].Width = 3;
			}

			_watermarkColorBox = new NuGenColorBox(serviceProvider);
			_watermarkColorBox.AutoSize = false;
			_watermarkColorBox.Dock = DockStyle.Fill;
			_watermarkColorBox.SelectedColor = Color.Gray;

			_watermarkOpacitySpin = new OpacitySpin(serviceProvider);
			_watermarkOpacitySpin.Dock = DockStyle.Right;
			_watermarkOpacitySpin.Width = 50;

			_percentLabel = new NuGenLabel();
			_percentLabel.AutoSize = false;
			_percentLabel.Dock = DockStyle.Right;
			_percentLabel.Text = "%";
			_percentLabel.Width = 10;

			_colorPanel = new LayoutPanel();
			_colorPanel.Controls.AddRange(
				new Control[]
				{
					_watermarkColorBox
					, colorSpacers[0]
					, _watermarkOpacitySpin
					, colorSpacers[1]
					, _percentLabel
				}
			);
			_colorPanel.Enabled = false;
			_colorPanel.Size = new Size(241, 20);

			_watermarkAlignDropDown = new NuGenAlignDropDown(serviceProvider);
			_watermarkAlignDropDown.Enabled = false;
			_watermarkAlignDropDown.SelectedAlignment = ContentAlignment.TopLeft;
			_watermarkAlignDropDown.Width = 175;

			_settingsLayoutPanel.Controls.Add(_pathPanel, 1, 0);
			_settingsLayoutPanel.Controls.Add(_templateTextBox, 1, 1);
			_settingsLayoutPanel.Controls.Add(_formatCombo, 1, 2);
			_settingsLayoutPanel.Controls.Add(_typeCombo, 1, 3);
			_settingsLayoutPanel.Controls.Add(_maxWidthSpin, 1, 4);
			_settingsLayoutPanel.Controls.Add(_maxHeightSpin, 1, 5);
			_settingsLayoutPanel.Controls.Add(_watermarkFontBlock, 1, 6);
			_settingsLayoutPanel.Controls.Add(_colorPanel, 1, 7);
			_settingsLayoutPanel.Controls.Add(_watermarkAlignDropDown, 1, 8);

			this.CurrentStep = _previewStep;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_exportService != null)
				{
					_exportService.Progress -= _exportProgress;
					_exportService.Dispose();
				}

				if (_controlPanel != null)
				{
					_controlPanel.Back -= _controlPanel_Back;
					_controlPanel.Cancel -= _controlPanel_Cancel;
					_controlPanel.CancelExport -= _controlPanel_CancelExport;
					_controlPanel.Close -= _controlPanel_Close;
					_controlPanel.Export -= _controlPanel_Export;
					_controlPanel.Next -= _controlPanel_Next;
					_controlPanel.Dispose();
					_controlPanel = null;
				}

				if (_chooseButton != null)
				{
					_chooseButton.Click -= _chooseButton_Click;
					_chooseButton.Dispose();
					_chooseButton = null;
				}

				if (_maxWidthCheckBox != null)
				{
					_maxWidthCheckBox.CheckedChanged -= _maxWidthCheckBox_CheckedChanged;
					_maxWidthCheckBox.Dispose();
					_maxWidthCheckBox = null;
				}

				if (_maxHeightCheckBox != null)
				{
					_maxHeightCheckBox.CheckedChanged -= _maxHeightCheckBox_CheckedChanged;
					_maxHeightCheckBox.Dispose();
					_maxHeightCheckBox = null;
				}

				if (_numWatermarkCheckBox != null)
				{
					_numWatermarkCheckBox.CheckedChanged -= _numWatermarkCheckBox_CheckedChanged;
					_numWatermarkCheckBox.Dispose();
					_numWatermarkCheckBox = null;
				}
			}

			base.Dispose(disposing);
		}
	}
}
