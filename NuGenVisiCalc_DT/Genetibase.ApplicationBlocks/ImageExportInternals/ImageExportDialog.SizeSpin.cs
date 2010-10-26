/* -----------------------------------------------
 * ImageExportDialog.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.SpinInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.ApplicationBlocks.ImageExportInternals
{
	partial class ImageExportDialog
	{
		private sealed class SizeSpin : NuGenSpin
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="SizeSpin"/> class.
			/// </summary>
			/// <param name="serviceProvider">Requires:<para/>
			/// 	<see cref="INuGenSpinRenderer"/><para/>
			/// 	<see cref="INuGenButtonStateTracker"/><para/>
			/// 	<see cref="INuGenControlStateTracker"/><para/></param>
			/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
			public SizeSpin(INuGenServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				this.Maximum = 9600;
				this.Minimum = 1;
				this.Value = 640;
				this.Width = 100;
			}

			protected override void OnMaximumChanged(EventArgs e)
			{
				base.OnMaximumChanged(e);
			}
		}
	}
}
