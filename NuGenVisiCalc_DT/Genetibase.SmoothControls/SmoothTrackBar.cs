/* -----------------------------------------------
 * SmoothTrackBar.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.TrackBarInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// Represents a <see cref="NuGenSmoothTrackBar"/> that <see cref="ToolStrip"/> and the inheritors can host.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	public class SmoothTrackBar : NuGenToolStripTrackBar
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SmoothTrackBar"/> class.
		/// </summary>
		public SmoothTrackBar()
			: base(NuGenSmoothServiceManager.TrackBarServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SmoothTrackBar"/> class.
		/// </summary>
		/// <param name="serviceProvider">Requires:<para/>
		/// 	<see cref="INuGenButtonStateTracker"/><para/>
		/// 	<see cref="INuGenControlStateTracker"/><para/>
		/// 	<see cref="INuGenTrackBarRenderer"/><para/></param>
		public SmoothTrackBar(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
