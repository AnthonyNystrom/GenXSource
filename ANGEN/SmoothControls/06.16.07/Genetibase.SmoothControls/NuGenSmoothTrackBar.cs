/* -----------------------------------------------
 * NuGenSmoothTrackBar.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.TrackBarInternals;
using Genetibase.Shared.Windows;
using Genetibase.SmoothControls.ComponentModel;
using Genetibase.SmoothControls.Properties;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// <seealso cref="TrackBar"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothTrackBar), "Resources.NuGenIcon.png")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenSmoothTrackBar : NuGenTrackBar
	{
		/*
		 * ShowFocusCues
		 */

		/// <summary>
		/// Gets a value indicating whether the control should display focus rectangles.
		/// </summary>
		/// <value></value>
		/// <returns>true if the control should display focus rectangles; otherwise, false.</returns>
		protected override bool ShowFocusCues
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothTrackBar"/> class.
		/// </summary>
		public NuGenSmoothTrackBar()
			: this(NuGenSmoothServiceManager.TrackBarServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothTrackBar"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenTrackBarRenderer"/>
		/// <see cref="INuGenControlStateTracker"/>
		/// <see cref="INuGenButtonStateTracker"/>
		/// <see cref="INuGenValueTracker"/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenSmoothTrackBar(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
