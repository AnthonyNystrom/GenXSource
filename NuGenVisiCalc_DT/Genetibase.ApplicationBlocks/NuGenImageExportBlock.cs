/* -----------------------------------------------
 * NuGenImageExportBlock.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks.ComponentModel;
using Genetibase.ApplicationBlocks.ImageExportInternals;
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
	/// Provides user interface to export multiple images.
	/// </summary>
	[ToolboxItem(false)]
	[Designer("Genetibase.ApplicationBlocks.Design.NuGenImageExportBlockDesigner")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenImageExportBlock : NuGenComponent
	{
		#region Properties.Appearance

		/*
		 * Icon
		 */

		/// <summary>
		/// Gets or sets the icon that is displayed in the task bar for the export dialog.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_ImageExportBlock_Icon")]
		public Icon Icon
		{
			get
			{
				return _exportDialog.Icon;
			}
			set
			{
				_exportDialog.Icon = value;
			}
		}

		/*
		 * ShowInTaskbar
		 */

		/// <summary>
		/// Gets or sets the value indicating whether to show a button in the Windows task bar for the
		/// export dialog.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_ImageExportBlock_ShowInTaskbar")]
		public bool ShowInTaskbar
		{
			get
			{
				return _exportDialog.ShowInTaskbar;
			}
			set
			{
				_exportDialog.ShowInTaskbar = value;
			}
		}

		/*
		 * Text
		 */

		/// <summary>
		/// Gets or sets the text for the export dialog.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_ImageExportBlock_Text")]
		public string Text
		{
			get
			{
				return _exportDialog.Text;
			}
			set
			{
				_exportDialog.Text = value;
			}
		}

		private static readonly object _textChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Text"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ImageExportBlock_TextChanged")]
		public event EventHandler TextChanged
		{
			add
			{
				this.Events.AddHandler(_textChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_textChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.ApplicationBlocks.NuGenImageExportBlock.TextChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnTextChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_textChanged, e);	
		}

		#endregion

		#region Properties.Layout

		/// <summary>
		/// Gets or sets the export dialog location.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Layout")]
		[NuGenSRDescription("Description_ImageExportBlock_Location")]
		public Point Location
		{
			get
			{
				return _exportDialog.Location;
			}
			set
			{
				_exportDialog.Location = value;
			}
		}

		/// <summary>
		/// Gets or sets the export dialog size.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Layout")]
		[NuGenSRDescription("Description_ImageExportBlock_Size")]
		public Size Size
		{
			get
			{
				return _exportDialog.Size;
			}
			set
			{
				_exportDialog.Size = value;
			}
		}

		#endregion

		#region Properties.NonBrowsable

		/// <summary>
		/// </summary>
		[Browsable(false)]
		public StringCollection ExportPathCollection
		{
			get
			{
				return _exportDialog.PathCollection;
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(false)]
		public NuGenThumbnailContainer.ImageCollection Images
		{
			get
			{
				Debug.Assert(_exportDialog != null, "_exportDialog != null");
				return _exportDialog.Images;
			}
		}

		/// <summary>
		/// Gets or sets the value indicating whether to constrain the width of the exported images.
		/// </summary>
		[Browsable(false)]
		public bool ConstrainWidth
		{
			get
			{
				return _exportDialog.ConstrainWidth;
			}
			set
			{
				_exportDialog.ConstrainWidth = value;
			}
		}

		/// <summary>
		/// Gets or sets the maximum width for the exportd images.
		/// </summary>
		[Browsable(false)]
		public int MaximumWidth
		{
			get
			{
				return _exportDialog.MaximumWidth;
			}
			set
			{
				_exportDialog.MaximumWidth = value;
			}
		}

		/// <summary>
		/// Gets or sets the value indicating whether to constrain the height of the exported images.
		/// </summary>
		[Browsable(false)]
		public bool ConstrainHeight
		{
			get
			{
				return _exportDialog.ConstrainHeight;
			}
			set
			{
				_exportDialog.ConstrainHeight = value;
			}
		}

		/// <summary>
		/// Gets or sets the maximum height for the exported images.
		/// </summary>
		[Browsable(false)]
		public int MaximumHeight
		{
			get
			{
				return _exportDialog.MaximumHeight;
			}
			set
			{
				_exportDialog.MaximumHeight = value;
			}
		}

		/// <summary>
		/// Gets or sets the value indicating whether to draw a number watermark on the exported images.
		/// </summary>
		[Browsable(false)]
		public bool NumberWatermark
		{
			get
			{
				return _exportDialog.NumberWatermark;
			}
			set
			{
				_exportDialog.NumberWatermark = value;
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(false)]
		public NuGenThumbnailMode ThumbnailMode
		{
			get
			{
				return _exportDialog.ThumbnailMode;
			}
			set
			{
				_exportDialog.ThumbnailMode = value;
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(false)]
		public int ThumbnailSize
		{
			get
			{
				return _exportDialog.ThumbnailSize;
			}
			set
			{
				_exportDialog.ThumbnailSize = value;
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(false)]
		public Font WatermarkFont
		{
			get
			{
				return _exportDialog.WatermarkFont;
			}
			set
			{
				_exportDialog.WatermarkFont = value;
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(false)]
		public Color WatermarkColor
		{
			get
			{
				return _exportDialog.WatermarkColor;
			}
			set
			{
				_exportDialog.WatermarkColor = value;
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(false)]
		public int WatermarkColorOpacity
		{
			get
			{
				return _exportDialog.WatermarkColorOpacity;
			}
			set
			{
				_exportDialog.WatermarkColorOpacity = value;
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(false)]
		public ContentAlignment WatermarkAlignment
		{
			get
			{
				return _exportDialog.WatermarkAlignment;
			}
			set
			{
				_exportDialog.WatermarkAlignment = value;
			}
		}

		#endregion

		#region Methods.Public

		/// <summary>
		/// Shows the export form to the user. 
		/// </summary>
		public void Show()
		{
			_exportDialog.Show();
		}

		/// <summary>
		/// Shows the export form with the specified owner to the user.
		/// </summary>
		/// <param name="owner"></param>
		/// <exception cref="ArgumentException">
		/// The form specified in the <paramref name="owner"/> parameter is the same as the form being shown.
		/// </exception>
		public void Show(IWin32Window owner)
		{
			_exportDialog.Show(owner);
		}

		/// <summary>
		/// Shows the export form as a modal dialog box with the currently active window set as its owner.
		/// </summary>
		/// <exception cref="ArgumentException">
		/// The form specified in the <paramref name="owner"/> parameter is the same as the form being shown.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// <para>The form being shown is already visible.</para>
		/// -or- 
		/// <para>The form being shown is disabled.</para>
		/// -or- 
		/// <para>The form being shown is not a top-level window.</para>
		/// -or- 
		/// <para>The form being shown as a dialog box is already a modal form.</para>
		/// -or-
		/// <para>The current process is not running in user interactive mode.</para>
		/// </exception>
		public DialogResult ShowDialog()
		{
			return _exportDialog.ShowDialog();
		}

		/// <summary>
		/// Shows the export form as a modal dialog box with the specified owner.
		/// </summary>
		/// <param name="owner"></param>
		public DialogResult ShowDialog(IWin32Window owner)
		{
			return _exportDialog.ShowDialog(owner);
		}

		#endregion

		#region EventHandlers.ExportDialog

		private void _exportDialog_TextChanged(object sender, EventArgs e)
		{
			this.OnTextChanged(e);
		}

		#endregion

		private ImageExportDialog _exportDialog;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenImageExportBlock"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// <para><see cref="INuGenButtonStateService"/></para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		///		<para><see cref="INuGenButtonLayoutManager"/></para>
		///		<para><see cref="INuGenButtonRenderer"/></para>
		/// 	<para><see cref="INuGenCheckBoxLayoutManager"/></para>
		/// 	<para><see cref="INuGenCheckBoxRenderer"/></para>
		/// 	<para><see cref="INuGenComboBoxRenderer"/></para>
		///		<para><see cref="INuGenColorsProvider"/></para>
		///		<para><see cref="INuGenControlImageManager"/></para>
		///		<para><see cref="INuGenDirectorySelectorRenderer"/></para>
		///		<para><see cref="INuGenDropDownRenderer"/></para>
		/// 	<para><see cref="INuGenImageListService"/></para>
		///		<para><see cref="INuGenFontFamiliesProvider"/></para>
		///		<para><see cref="INuGenListBoxRenderer"/></para>
		/// 	<para><see cref="INuGenPanelRenderer"/></para>
		/// 	<para><see cref="INuGenProgressBarLayoutManager"/></para>
		/// 	<para><see cref="INuGenProgressBarRenderer"/></para>
		///		<para><see cref="INuGenRadioButtonLayoutManager"/></para>
		///		<para><see cref="INuGenRadioButtonRenderer"/></para>
		/// 	<para><see cref="INuGenScrollBarRenderer"/></para>
		///		<para><see cref="INuGenSpinRenderer"/></para>
		/// 	<para><see cref="INuGenSwitchButtonLayoutManager"/></para>
		/// 	<para><see cref="INuGenSwitchButtonRenderer"/></para>
		///		<para><see cref="INuGenTabStateTracker"/></para>
		///		<para><see cref="INuGenTabLayoutManager"/></para>
		///		<para><see cref="INuGenTabRenderer"/></para>
		/// 	<para><see cref="INuGenTextBoxRenderer"/></para>
		/// 	<para><see cref="INuGenTrackBarRenderer"/></para>
		/// 	<para><see cref="INuGenThumbnailLayoutManager"/></para>
		/// 	<para><see cref="INuGenThumbnailRenderer"/></para>
		/// 	<para><see cref="INuGenToolStripRenderer"/></para>
		///		<para><see cref="INuGenValueTrackerService"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenImageExportBlock(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			_exportDialog = new ImageExportDialog(serviceProvider);
			_exportDialog.TextChanged += _exportDialog_TextChanged;
		}

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.Component"></see> and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_exportDialog != null)
				{
					_exportDialog.TextChanged -= _exportDialog_TextChanged;
					_exportDialog.Dispose();
					_exportDialog = null;
				}
			}

			base.Dispose(disposing);
		}
	}
}
