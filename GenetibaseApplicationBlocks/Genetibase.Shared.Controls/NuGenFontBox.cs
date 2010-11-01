/* -----------------------------------------------
 * NuGenFontBox.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComboBoxInternals;
using Genetibase.Shared.Controls.FontBoxInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Represents a combo box which displays available fonts to the user.
	/// </summary>
	[ToolboxItem(false)]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenFontBox : NuGenComboBox
	{
		#region Properties.Hidden

		/*
		 * DataSource
		 */

		/// <summary>
		/// Gets or sets the data source for this <see cref="T:System.Windows.Forms.ComboBox"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>An object that implements the <see cref="T:System.Collections.IList"></see> interface, such as a <see cref="T:System.Data.DataSet"></see> or an <see cref="T:System.Array"></see>. The default is null.</returns>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new object DataSource
		{
			get
			{
				return base.DataSource;
			}
			set
			{
				base.DataSource = value;
			}
		}

		/*
		 * DisplayMember
		 */

		/// <summary>
		/// Gets or sets the property to display for this <see cref="T:System.Windows.Forms.ListControl"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.String"></see> specifying the name of an object property that is contained in the collection specified by the <see cref="P:System.Windows.Forms.ListControl.DataSource"></see> property. The default is an empty string (""). </returns>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new string DisplayMember
		{
			get
			{
				return base.DisplayMember;
			}
			set
			{
				base.DisplayMember = value;
			}
		}

		/*
		 * DropDownStyle
		 */

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

		/*
		 * ImageList
		 */

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
			}
		}

		/*
		 * Items
		 */

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

		/*
		 * Text
		 */

		/// <summary>
		/// Gets or sets the text associated with this control.
		/// </summary>
		/// <value></value>
		/// <returns>The text associated with this control.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		/*
		 * ValueMember
		 */

		/// <summary>
		/// Gets or sets the property to use as the actual value for the items in the <see cref="T:System.Windows.Forms.ListControl"></see>.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.String"></see> representing the name of an object property that is contained in the collection specified by the <see cref="P:System.Windows.Forms.ListControl.DataSource"></see> property. The default is an empty string ("").</returns>
		/// <exception cref="T:System.ArgumentException">The specified property cannot be found on the object specified by the <see cref="P:System.Windows.Forms.ListControl.DataSource"></see> property. </exception>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new string ValueMember
		{
			get
			{
				return base.ValueMember;
			}
			set
			{
				base.ValueMember = value;
			}
		}

		#endregion

		#region Properties.Services

		/*
		 * FontFamiliesProvider
		 */

		private INuGenFontFamiliesProvider _fontFamiliesProvider;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenFontFamiliesProvider FontFamiliesProvider
		{
			get
			{
				if (_fontFamiliesProvider == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_fontFamiliesProvider = this.ServiceProvider.GetService<INuGenFontFamiliesProvider>();

					if (_fontFamiliesProvider == null)
					{
						throw new NuGenServiceNotFoundException<INuGenFontFamiliesProvider>();
					}
				}

				return _fontFamiliesProvider;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenFontBox"/> class.
		/// </summary>
		/// <param name="serviceProvider">Requires:<para/>
		/// <see cref="INuGenComboBoxRenderer"/><para/>
		/// <see cref="INuGenButtonStateTracker"/><para/>
		/// <see cref="INuGenImageListService"/><para/>
		/// <see cref="INuGenFontFamiliesProvider"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenFontBox(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			List<string> fontNames = null;
			this.FontFamiliesProvider.FillWithFontNames(out fontNames);
			Debug.Assert(fontNames != null, "fontNames != null");

			System.Windows.Forms.ImageList imageList = null;
			this.FontFamiliesProvider.FillWithFontSamples(fontNames, out imageList);
			Debug.Assert(imageList != null, "imageList != null");

			base.ImageList = imageList;

			int selectedIndex = 0;

			for (int i = 0; i < fontNames.Count; i++)
			{
				string fontName = fontNames[i];

				if (fontName == "Arial")
				{
					selectedIndex = i;
				}

				this.Items.Add(fontName);
			}

			this.DropDownStyle = ComboBoxStyle.DropDownList;
			this.SelectedIndex = selectedIndex;
		}

		#endregion
	}
}
