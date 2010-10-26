/* -----------------------------------------------
 * NuGenSmoothPropertyGrid.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.PropertyGridInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Reflection;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// <seealso cref="System.Windows.Forms.PropertyGrid"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothPropertyGrid), "Resources.NuGenIcon.png")]
	[System.ComponentModel.DesignerCategory("Code")]
	public partial class NuGenSmoothPropertyGrid : NuGenPropertyGrid
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothPropertyGrid"/> class.
		/// </summary>
		public NuGenSmoothPropertyGrid()
			: this(NuGenSmoothServiceManager.PropertyGridServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothPropertyGrid"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenPropertyGridRenderer"/><para/>
		/// <see cref="INuGenControlStateTracker"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenSmoothPropertyGrid(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.ToolStripRenderer = new NuGenSmoothToolStripRenderer();
		}
	}
}
