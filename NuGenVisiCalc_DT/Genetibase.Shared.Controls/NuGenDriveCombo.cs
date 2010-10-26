/* -----------------------------------------------
 * NuGenDriveCombo.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComboBoxInternals;
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

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Displays a list of available drives.
	/// </summary>
	[Designer("Genetibase.Shared.Controls.Design.NuGenDriveComboDesigner")]
	public partial class NuGenDriveCombo : NuGenComboBox
	{
		/// <summary>
		/// Gets or sets a value specifying the style of the combo box.
		/// </summary>
		/// <value></value>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ComboBoxStyle"></see> values. The default is DropDown.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:System.Windows.Forms.ComboBoxStyle"></see> values. </exception>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new ComboBoxStyle DropDownStyle
		{
			get
			{
				return base.DropDownStyle;
			}
			set
			{
				base.DropDownStyle = value;
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="ImageList"/> to get the images to display on the combo box items.
		/// </summary>
		/// <value></value>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new ImageList ImageList
		{
			get
			{
				return base.ImageList;
			}
			set
			{
				base.ImageList = value;
			}
		}

		/// <summary>
		/// Gets an object representing the collection of the items contained in this <see cref="T:System.Windows.Forms.ComboBox"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Windows.Forms.ComboBox.ObjectCollection"></see> representing the items in the <see cref="T:System.Windows.Forms.ComboBox"></see>.</returns>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new ObjectCollection Items
		{
			get
			{
				return base.Items;
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string SelectedDrive
		{
			get
			{
				if (base.SelectedIndex != -1)
				{
					NuGenDriveItem item = base.SelectedItem as NuGenDriveItem;

					if (item != null)
					{
						return item.Drive;
					}
				}

				return null;
			}
		}

		private void RefreshDrives()
		{
			_imageList = new ImageList();
			this.ImageList = _imageList;
			this.Items.Clear();

			_imageList.ColorDepth = ColorDepth.Depth32Bit;

			Shell32.SHFILEINFO shInfo = new Shell32.SHFILEINFO();
			Shell32.SHGFI dwAttribs = 0
				| Shell32.SHGFI.SHGFI_ICON
				| Shell32.SHGFI.SHGFI_SMALLICON
				| Shell32.SHGFI.SHGFI_SYSICONINDEX
				| Shell32.SHGFI.SHGFI_DISPLAYNAME
				;

			foreach (string drive in Directory.GetLogicalDrives())
			{
				IntPtr handle = Shell32.SHGetFileInfo(drive, Shell32.FILE_ATTRIBUTE_NORMAL, ref shInfo, (uint)System.Runtime.InteropServices.Marshal.SizeOf(shInfo), dwAttribs);

				if (!handle.Equals(IntPtr.Zero))
				{
					_imageList.Images.Add(Icon.FromHandle(shInfo.hIcon).Clone() as Icon);
					User32.DestroyIcon(shInfo.hIcon);
					this.Items.Add(new NuGenDriveItem(drive, shInfo.szDisplayName));
				}
			}

			if (base.Items.Count != 0)
				base.SelectedIndex = 0;
		}

		private void _deviceChangeFilter_DeviceChanged(object sender, EventArgs e)
		{
			this.RefreshDrives();
		}

		private DeviceChangeFilter _deviceChangeFilter;
		private ImageList _imageList;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDriveCombo"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenComboBoxRenderer"/></para>
		/// 	<para><see cref="INuGenButtonStateService"/></para>
		/// 	<para><see cref="INuGenImageListService"/></para></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenDriveCombo(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			_deviceChangeFilter = new DeviceChangeFilter();
			_deviceChangeFilter.DeviceChanged += _deviceChangeFilter_DeviceChanged;
			Application.AddMessageFilter(_deviceChangeFilter);

			this.DropDownStyle = ComboBoxStyle.DropDownList;
			this.RefreshDrives();
		}

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.ComboBox"></see> and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_deviceChangeFilter != null)
				{
					Application.RemoveMessageFilter(_deviceChangeFilter);
					_deviceChangeFilter.DeviceChanged -= _deviceChangeFilter_DeviceChanged;
					_deviceChangeFilter = null;
				}

				if (_imageList != null)
				{
					_imageList.Dispose();
					_imageList = null;
				}
			}

			base.Dispose(disposing);
		}
	}
}
