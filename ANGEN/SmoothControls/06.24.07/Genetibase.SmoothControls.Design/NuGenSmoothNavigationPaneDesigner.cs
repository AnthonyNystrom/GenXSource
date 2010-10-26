/* -----------------------------------------------
 * NuGenSmoothNavigationPaneDesigner.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.Design;

using System;
using System.Collections;
using System.Diagnostics;

namespace Genetibase.SmoothControls.Design
{
	internal sealed class NuGenSmoothNavigationPaneDesigner : NuGenNavigationPaneDesigner
	{
		protected override void PostFilterProperties(IDictionary properties)
		{			
			properties.Remove("BackColor");
			properties.Remove("BackgroundImage");
			properties.Remove("BackgroundImageLayout");
			properties.Remove("BorderStyle");
			base.PostFilterProperties(properties);
		}
	}
}
