/* -----------------------------------------------
 * NuGenSmoothCheckBoxDesigner.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.Design;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms.Design;

namespace Genetibase.SmoothControls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenSmoothCheckBox"/>.
	/// </summary>
	internal sealed class NuGenSmoothCheckBoxDesigner : NuGenCheckBoxDesigner
	{
		protected override void PreFilterProperties(IDictionary properties)
		{
			properties.Remove("UseCompatibleTextRendering");
			base.PreFilterProperties(properties);
		}
	}
}
