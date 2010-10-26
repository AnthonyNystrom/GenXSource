/* -----------------------------------------------
 * NuGenPresenter.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks.ComponentModel;
using Genetibase.ApplicationBlocks.ImageExportInternals;
using Genetibase.ApplicationBlocks.PresenterInternals;
using Genetibase.ApplicationBlocks.Properties;
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

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks
{
	/// <summary>
	/// Magnifies the screen and allows to pan and zoom around the magnified screen.
	/// You can annotate the magnified image using the mouse or a Tablet PC pen. 
	/// </summary>
	[ToolboxItem(false)]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenPresenter : Component
	{
		#region Events.Public

		private static readonly object _exportSucceded = new object();

		/// <summary>
		/// Occurs when the export operation succeeded and/or the export dialog was normally closed.
		/// </summary>
		public event EventHandler ExportSucceeded
		{
			add
			{
				this.Events.AddHandler(_exportSucceded, value);
			}
			remove
			{
				this.Events.RemoveHandler(_exportSucceded, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.ApplicationBlocks.NuGenPresenter.ExportSucceeded"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnExportSucceeded(EventArgs e)
		{
			EventHandler handler = this.Events[_exportSucceded] as EventHandler;

			if (handler != null)
			{
				handler(this, e);
			}
		}

		#endregion

		#region Properties.Public

		/// <summary>
		/// </summary>
		public Color PenColor
		{
			get
			{
				return _presenterForm.PenColor;
			}
			set
			{
				_presenterForm.PenColor = value;
			}
		}

		/// <summary>
		/// </summary>
		public float PenWidth
		{
			get
			{
				return _presenterForm.PenWidth;
			}
			set
			{
				_presenterForm.PenWidth = value;
			}
		}

		private bool _exportDialogConstrainWidth;

		/// <summary>
		/// </summary>
		public bool ExportDialogConstrainWidth
		{
			get
			{
				return _exportDialogConstrainWidth;
			}
			set
			{
				_exportDialogConstrainWidth = value;
			}
		}

		private bool _exportDialogConstrainHeight;

		/// <summary>
		/// </summary>
		public bool ExportDialogConstrainHeigth
		{
			get
			{
				return _exportDialogConstrainHeight;
			}
			set
			{
				_exportDialogConstrainHeight = value;
			}
		}

		private StringCollection _exportPathCollection;

		/// <summary>
		/// </summary>
		public StringCollection ExportPathCollection
		{
			get
			{
				if (_exportPathCollection == null)
				{
					_exportPathCollection = new StringCollection();
				}

				return _exportPathCollection;
			}
			set
			{
				_exportPathCollection = value;
			}
		}

		private Icon _exportDialogIcon;

		/// <summary>
		/// </summary>
		public Icon ExportDialogIcon
		{
			get
			{
				return _exportDialogIcon;
			}
			set
			{
				_exportDialogIcon = value;
			}
		}

		private Point _exportDialogLocation;

		/// <summary>
		/// </summary>
		public Point ExportDialogLocation
		{
			get
			{
				return _exportDialogLocation;
			}
			set
			{
				_exportDialogLocation = value;
			}
		}

		private int _exportDialogMaximumHeight;

		/// <summary>
		/// </summary>
		public int ExportDialogMaximumHeight
		{
			get
			{
				return _exportDialogMaximumHeight;
			}
			set
			{
				_exportDialogMaximumHeight = value;
			}
		}

		private int _exportDialogMaximumWidth;

		/// <summary>
		/// </summary>
		public int ExportDialogMaximumWidth
		{
			get
			{
				return _exportDialogMaximumWidth;
			}
			set
			{
				_exportDialogMaximumWidth = value;
			}
		}

		private bool _exportDialogNumberWatermark;

		/// <summary>
		/// </summary>
		public bool ExportDialogNumberWatermark
		{
			get { return _exportDialogNumberWatermark; }
			set { _exportDialogNumberWatermark = value; }
		}

		private bool _exportDialogShowInTaskbar;

		/// <summary>
		/// </summary>
		public bool ExportDialogShowInTaskbar
		{
			get
			{
				return _exportDialogShowInTaskbar;
			}
			set
			{
				_exportDialogShowInTaskbar = value;
			}
		}

		private Size _exportDialogSize;

		/// <summary>
		/// </summary>
		public Size ExportDialogSize
		{
			get
			{
				return _exportDialogSize;
			}
			set
			{
				_exportDialogSize = value;
			}
		}

		private NuGenThumbnailMode _exportDialogThumbnailMode;

		/// <summary>
		/// </summary>
		public NuGenThumbnailMode ExportDialogThumbnailMode
		{
			get { return _exportDialogThumbnailMode; }
			set { _exportDialogThumbnailMode = value; }
		}

		private int _exportDialogThumbnailSize;

		/// <summary>
		/// </summary>
		public int ExportDialogThumbnailSize
		{
			get { return _exportDialogThumbnailSize; }
			set { _exportDialogThumbnailSize = value; }
		}

		private Font _exportDialogWatermarkFont;

		/// <summary>
		/// </summary>
		public Font ExportDialogWatermarkFont
		{
			get { return _exportDialogWatermarkFont; }
			set { _exportDialogWatermarkFont = value; }
		}

		private Color _exportDialogWatermarkColor;

		/// <summary>
		/// </summary>
		public Color ExportDialogWatermarkColor
		{
			get { return _exportDialogWatermarkColor; }
			set { _exportDialogWatermarkColor = value; }
		}

		private int _exportDialogWatermarkOpacity;

		/// <summary>
		/// </summary>
		public int ExportDialogWatermarkOpacity
		{
			get { return _exportDialogWatermarkOpacity; }
			set { _exportDialogWatermarkOpacity = value; }
		}

		private ContentAlignment _exportDialogWatermarkAlignment;

		/// <summary>
		/// </summary>
		public ContentAlignment ExportDialogWatermarkAlignment
		{
			get { return _exportDialogWatermarkAlignment; }
			set { _exportDialogWatermarkAlignment = value; }
		}

		#endregion

		#region Properties.HotKeys

		/// <summary>
		/// </summary>
		public Keys ClearHotKeys
		{
			get
			{
				return _presenterForm.ClearOperation.HotKeys;
			}
			set
			{
				_presenterForm.ClearOperation.HotKeys = value;
			}
		}

		/// <summary>
		/// </summary>
		public Keys EscapeHotKeys
		{
			get
			{
				return _presenterForm.EscapeOperation.HotKeys;
			}
			set
			{
				_presenterForm.EscapeOperation.HotKeys = value;
			}
		}

		/// <summary>
		/// </summary>
		public Keys SaveHotKeys
		{
			get
			{
				return _presenterForm.SaveOperation.HotKeys;
			}
			set
			{
				_presenterForm.SaveOperation.HotKeys = value;
			}
		}

		/// <summary>
		/// </summary>
		public Keys LockTranformHotKeys
		{
			get
			{
				return _presenterForm.LockTransformOperation.HotKeys;
			}
			set
			{
				_presenterForm.LockTransformOperation.HotKeys = value;
			}
		}

		/// <summary>
		/// </summary>
		public Keys ShowPointerHotKeys
		{
			get
			{
				return _presenterForm.ShowPointerOperation.HotKeys;
			}
			set
			{
				_presenterForm.ShowPointerOperation.HotKeys = value;
			}
		}

		/// <summary>
		/// </summary>
		public Keys ZoomInHotKeys
		{
			get
			{
				return _presenterForm.ZoomInOperation.HotKeys;
			}
			set
			{
				_presenterForm.ZoomInOperation.HotKeys = value;
			}
		}

		/// <summary>
		/// </summary>
		public Keys ZoomOutHotKeys
		{
			get
			{
				return _presenterForm.ZoomOutOperation.HotKeys;
			}
			set
			{
				_presenterForm.ZoomOutOperation.HotKeys = value;
			}
		}

		#endregion

		#region Properties.Services

		private INuGenServiceProvider _serviceProvider;

		/// <summary>
		/// </summary>
		protected INuGenServiceProvider ServiceProvider
		{
			get
			{
				return _serviceProvider;
			}
		}

		private INuGenTempImageService _tempImageService;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenTempImageService TempImageService
		{
			get
			{
				if (_tempImageService == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_tempImageService = this.ServiceProvider.GetService<INuGenTempImageService>();

					if (_tempImageService == null)
					{
						throw new NuGenServiceNotFoundException<INuGenTempImageService>();
					}
				}

				return _tempImageService;
			}
		}

		#endregion
	
		private IList<Image> _tempImageCollection;

		/// <summary>
		/// Gets the collection of the temporary images that are ready to be exported.
		/// Call <see cref="UpdateTempImageCollection"/> method to get the latest list of images.
		/// This property is a cache.
		/// </summary>
		public IList<Image> TempImageCollection
		{
			get
			{
				return _tempImageCollection;
			}
		}

		private NuGenPresenterMode _mode;

		/// <summary>
		/// </summary>
		public NuGenPresenterMode Mode
		{
			get
			{
				return _mode;
			}
			set
			{
				_mode = value;

				switch (_mode)
				{
					case NuGenPresenterMode.Draw:
					{
						_presenterForm.Show();
						_presenterForm.BeginNewZoomIn(false);
						break;
					}
					case NuGenPresenterMode.Zoom:
					{
						_presenterForm.Show();
						_presenterForm.BeginNewZoomIn(true);
						break;
					}
					default:
					{
						_presenterForm.Hide();
						break;
					}
				}
			}
		}

		/// <summary>
		/// Raises the export dialog.
		/// </summary>
		/// <param name="imagesToExport">Specify the collection of images to export.</param>
		/// <exception cref="ArgumentNullException"><paramref name="imagesToExport"/> is <see langword="null"/>.</exception>
		/// <exception cref="InvalidOperationException">There are no images to export.</exception>
		public void Export(IList<Image> imagesToExport)
		{
			if (imagesToExport == null)
			{
				throw new ArgumentNullException("imagesToExport");
			}

			using (NuGenImageExportBlock exportBlock = new NuGenImageExportBlock(this.ServiceProvider))
			{	
				if (imagesToExport.Count == 0) 			
				{
					throw new InvalidOperationException(Resources.InvalidOperation_NoImagesToExport);
				}

				exportBlock.ConstrainHeight = this.ExportDialogConstrainHeigth;
				exportBlock.ConstrainWidth = this.ExportDialogConstrainWidth;

				if (this.ExportPathCollection != null)
				{
					foreach (string path in this.ExportPathCollection)
					{
						exportBlock.ExportPathCollection.Add(path);
					}
				}

				exportBlock.Icon = this.ExportDialogIcon;
				exportBlock.Images.AddRange(imagesToExport);
				exportBlock.Location = this.ExportDialogLocation;
				exportBlock.MaximumHeight = this.ExportDialogMaximumHeight;
				exportBlock.MaximumWidth = this.ExportDialogMaximumWidth;
				exportBlock.NumberWatermark = this.ExportDialogNumberWatermark;
				exportBlock.ShowInTaskbar = this.ExportDialogShowInTaskbar;
				exportBlock.Size = this.ExportDialogSize;
				exportBlock.ThumbnailMode = this.ExportDialogThumbnailMode;
				exportBlock.ThumbnailSize = this.ExportDialogThumbnailSize;
				exportBlock.WatermarkAlignment = this.ExportDialogWatermarkAlignment;
				exportBlock.WatermarkColor = this.ExportDialogWatermarkColor;
				exportBlock.WatermarkColorOpacity = this.ExportDialogWatermarkOpacity;
				exportBlock.WatermarkFont = this.ExportDialogWatermarkFont;

				exportBlock.ShowDialog();

				this.ExportDialogConstrainHeigth = exportBlock.ConstrainHeight;
				this.ExportDialogConstrainWidth = exportBlock.ConstrainWidth;

				if (this.ExportPathCollection != null)
				{
					this.ExportPathCollection.Clear();
				}

				this.ExportPathCollection = exportBlock.ExportPathCollection;
				this.ExportDialogLocation = exportBlock.Location;
				this.ExportDialogMaximumHeight = exportBlock.MaximumHeight;
				this.ExportDialogMaximumWidth = exportBlock.MaximumWidth;
				this.ExportDialogNumberWatermark = exportBlock.NumberWatermark;
				this.ExportDialogSize = exportBlock.Size;
				this.ExportDialogThumbnailMode = exportBlock.ThumbnailMode;
				this.ExportDialogThumbnailSize = exportBlock.ThumbnailSize;
				this.ExportDialogWatermarkAlignment = exportBlock.WatermarkAlignment;
				this.ExportDialogWatermarkColor = exportBlock.WatermarkColor;
				this.ExportDialogWatermarkOpacity = exportBlock.WatermarkColorOpacity;
				this.ExportDialogWatermarkFont = exportBlock.WatermarkFont;

				this.OnExportSucceeded(EventArgs.Empty);
			}
		}

		/// <summary>
		/// Calls the Dispose method on each <see cref="Image"/> in the collection.
		/// </summary>
		public void ReleaseTempImageCollection()
		{
			if (_tempImageCollection != null)
			{
				foreach (Image image in _tempImageCollection)
				{
					image.Dispose();
				}

				_tempImageCollection.Clear();
			}
		}

		/// <summary>
		/// Refreshes the contents of the <see cref="TempImageCollection"/>.
		/// </summary>
		public void UpdateTempImageCollection()
		{
			this.ReleaseTempImageCollection();
			_tempImageCollection = this.TempImageService.GetTempImageCollection();
		}

		private NuGenPresenterForm _presenterForm;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPresenter"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenButtonStateService"/></para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		///		<para><see cref="INuGenButtonLayoutManager"/></para>
		///		<para><see cref="INuGenButtonRenderer"/></para>
		/// 	<para><see cref="INuGenCheckBoxLayoutManager"/></para>
		/// 	<para><see cref="INuGenCheckBoxRenderer"/></para>
		///		<para><see cref="INuGenColorsProvider"/></para>
		/// 	<para><see cref="INuGenComboBoxRenderer"/></para>
		///		<para><see cref="INuGenControlImageManager"/></para>
		///		<para><see cref="INuGenDropDownRenderer"/></para>
		/// 	<para><see cref="INuGenDirectorySelectorRenderer"/></para>
		/// 	<para><see cref="INuGenImageListService"/></para>
		///		<para><see cref="INuGenListBoxRenderer"/></para>
		///		<para><see cref="INuGenFontFamiliesProvider"/></para>
		/// 	<para><see cref="INuGenPanelRenderer"/></para>
		/// 	<para><see cref="INuGenProgressBarLayoutManager"/></para>
		/// 	<para><see cref="INuGenProgressBarRenderer"/></para>
		///		<para><see cref="INuGenRadioButtonLayoutManager"/></para>
		///		<para><see cref="INuGenRadioButtonRenderer"/></para>
		/// 	<para><see cref="INuGenScrollBarRenderer"/></para>
		///		<para><see cref="INuGenSpinRenderer"/></para>
		/// 	<para><see cref="INuGenSwitchButtonLayoutManager"/></para>
		/// 	<para><see cref="INuGenSwitchButtonRenderer"/></para>
		///		<para><see cref="INuGenTabStateService"/></para>
		///		<para><see cref="INuGenTabLayoutManager"/></para>
		///		<para><see cref="INuGenTabRenderer"/></para>
		///		<para><see cref="INuGenTempImageService"/></para>
		/// 	<para><see cref="INuGenTextBoxRenderer"/></para>
		/// 	<para><see cref="INuGenTrackBarRenderer"/></para>
		/// 	<para><see cref="INuGenThumbnailLayoutManager"/></para>
		/// 	<para><see cref="INuGenThumbnailRenderer"/></para>
		/// 	<para><see cref="INuGenToolStripRenderer"/></para>
		/// 	<para><see cref="INuGenValueTrackerService"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenPresenter(INuGenServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			_serviceProvider = serviceProvider;
			_presenterForm = new NuGenPresenterForm(serviceProvider);
		}

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.Component"></see> and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_presenterForm != null)
				{
					_presenterForm.Dispose();
					_presenterForm = null;
				}
			}

			base.Dispose(disposing);
		}
	}
}
