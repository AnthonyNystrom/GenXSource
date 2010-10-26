/* -----------------------------------------------
 * NuGenSmoothToolTipInfoEditor.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.Design;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.Design
{
	/// <summary>
	/// </summary>
	internal sealed class NuGenSmoothToolTipInfoEditor : NuGenToolTipInfoEditor
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothToolTipInfoEditor"/> class.
		/// </summary>
		public NuGenSmoothToolTipInfoEditor()
			: base(NuGenSmoothServiceManager.ToolTipServiceProvider)
		{
		}

		#endregion
	}
}
