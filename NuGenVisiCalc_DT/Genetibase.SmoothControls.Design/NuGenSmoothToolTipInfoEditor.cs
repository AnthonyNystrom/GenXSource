/* -----------------------------------------------
 * NuGenSmoothToolTipInfoEditor.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.Design;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.Design
{
	internal sealed class NuGenSmoothToolTipInfoEditor : NuGenToolTipInfoEditor
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothToolTipInfoEditor"/> class.
		/// </summary>
		public NuGenSmoothToolTipInfoEditor()
			: base(NuGenSmoothServiceManager.ToolTipServiceProvider)
		{
		}
	}
}
