/* -----------------------------------------------
 * NuGenSmoothSplitContainer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.SplitContainerInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;
using Genetibase.SmoothControls.ComponentModel;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// <seealso cref="SplitContainer"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothSplitContainer), "Resources.NuGenIcon.png")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenSmoothSplitContainer : NuGenSplitContainer
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothSplitContainer"/> class.
		/// </summary>
		public NuGenSmoothSplitContainer()
			: this(NuGenSmoothServiceManager.SplitContainerServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothSplitContainer"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenSplitContainerRenderer"/><para/>
		/// <see cref="INuGenControlStateTracker"/><para/>
		/// </param>
		public NuGenSmoothSplitContainer(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
