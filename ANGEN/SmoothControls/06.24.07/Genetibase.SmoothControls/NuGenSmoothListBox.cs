/* -----------------------------------------------
 * NuGenSmoothListBox.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ListBoxInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// <seealso cref="ListBox"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothListBox), "Resources.NuGenIcon.png")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenSmoothListBox : NuGenListBox
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothListBox"/> class.
		/// </summary>
		public NuGenSmoothListBox()
			: this(NuGenSmoothServiceManager.ListBoxServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothListBox"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenControlStateTracker"/></para>
		/// 	<para><see cref="INuGenListBoxRenderer"/></para>
		/// 	<para><see cref="INuGenImageListService"/></para></param>
		public NuGenSmoothListBox(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
