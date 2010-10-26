/* -----------------------------------------------
 * NuGenSmoothGroupBox.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.GroupBoxInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;
using Genetibase.SmoothControls.ComponentModel;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// <see cref="GroupBox"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothGroupBox), "Resources.NuGenIcon.png")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenSmoothGroupBox : NuGenGroupBox
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothGroupBox"/> class.
		/// </summary>
		public NuGenSmoothGroupBox()
			: this(NuGenSmoothServiceManager.GroupBoxServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothGroupBox"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenGroupBoxRenderer"/>
		/// <see cref="INuGenControlStateTracker"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenSmoothGroupBox(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
