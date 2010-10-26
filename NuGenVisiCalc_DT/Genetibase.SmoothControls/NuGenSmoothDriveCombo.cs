/* -----------------------------------------------
 * NuGenSmoothDriveCombo.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ComboBoxInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// <seealso cref="NuGenDriveCombo"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothDriveCombo), "Resources.NuGenIcon.png")]
	[System.ComponentModel.DesignerCategory("Code")]
	public partial class NuGenSmoothDriveCombo : NuGenDriveCombo
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothDriveCombo"/> class.
		/// </summary>
		public NuGenSmoothDriveCombo()
			: this(NuGenSmoothServiceManager.ComboBoxServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothDriveCombo"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenComboBoxRenderer"/></para>
		/// 	<para><see cref="INuGenButtonStateService"/></para>
		/// 	<para><see cref="INuGenImageListService"/></para></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothDriveCombo(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
