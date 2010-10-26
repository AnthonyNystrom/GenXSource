/* -----------------------------------------------
 * NuGenSmoothDialogBlockDesigner.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks.Design;
using Genetibase.SmoothControls;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothApplicationBlocks.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenSmoothDialogBlock"/>.
	/// </summary>
	internal sealed class NuGenSmoothDialogBlockDesigner : NuGenDialogBlockDesigner
	{
		/// <summary>
		/// </summary>
		public override void InitializeNewComponent(IDictionary defaultValues)
		{
			base.InitializeNewComponent(defaultValues);
			this.CreateButtons<NuGenSmoothButton>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothDialogBlockDesigner"/> class.
		/// </summary>
		public NuGenSmoothDialogBlockDesigner()
		{
		}
	}
}
