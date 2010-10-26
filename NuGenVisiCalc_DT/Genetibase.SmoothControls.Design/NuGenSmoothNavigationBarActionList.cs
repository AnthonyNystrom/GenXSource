/* -----------------------------------------------
 * NuGenSmoothNavigationBarActionList.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.Design;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.Design
{
	internal sealed class NuGenSmoothNavigationBarActionList : NuGenNavigationBarActionList
	{
		public override void AddNavigationPane()
		{
			this.CreateNavigationPane<NuGenSmoothNavigationPane>();
		}

		public NuGenSmoothNavigationBarActionList(NuGenNavigationBar navigationBar)
			: base(navigationBar)
		{
		}
	}
}
