/* -----------------------------------------------
 * NuGenSmoothDialogBlockLayoutManager.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks.DialogInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.SmoothApplicationBlocks.DialogInternals
{
	/// <summary>
	/// </summary>
	public class NuGenSmoothDialogBlockLayoutManager : INuGenDialogBlockLayoutManager
	{
		private static readonly Size _defaultSize = new Size(150, 38);

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public Size GetDefaultSize()
		{
			return _defaultSize;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothDialogBlockLayoutManager"/> class.
		/// </summary>
		public NuGenSmoothDialogBlockLayoutManager()
		{
		}
	}
}
