/* -----------------------------------------------
 * NuGenFontSizeBox.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComboBoxInternals;
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
	/// Represents a combo box which allows a user to select the font size.
	/// </summary>
	[ToolboxItem(false)]
	[Designer("Genetibase.Shared.Controls.Design.NuGenFontSizeBoxDesigner")]
	public class NuGenFontSizeBox : NuGenInt32Combo
	{
		#region Properties.Hidden

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
		public new int Minimum
		{
			get
			{
				return base.Minimum;
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new int Maximum
		{
			get
			{
				return base.Maximum;
			}
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenFontSizeBox"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenComboBoxRenderer"/></para>
		/// 	<para><see cref="INuGenButtonStateService"/></para>
		/// 	<para><see cref="INuGenImageListService"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenFontSizeBox(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			base.Items.AddRange(
				new object[]	
				{
					8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28
					, 36, 48, 72
				}
			);

			base.Value = 10;
			base.Minimum = 1;
			base.Maximum = 1638;

			this.MaxDropDownItems = 12;
		}
	}
}
