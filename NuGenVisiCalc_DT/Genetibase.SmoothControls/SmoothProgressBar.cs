/* -----------------------------------------------
 * SmoothProgressBar.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ProgressBarInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// Represents a <see cref="NuGenSmoothProgressBar"/> that <see cref="ToolStrip"/> and the inheritors can host.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	public class SmoothProgressBar : NuGenToolStripProgressBar
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SmoothProgressBar"/> class.
		/// </summary>
		public SmoothProgressBar()
			: this(NuGenSmoothServiceManager.ProgressBarServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SmoothProgressBar"/> class.
		/// </summary>
		/// <param name="serviceProvider">Requires:<para/>
		/// <see cref="INuGenProgressBarRenderer"/><para/>
		/// <see cref="INuGenControlStateTracker"/><para/>
		/// </param>
		public SmoothProgressBar(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
