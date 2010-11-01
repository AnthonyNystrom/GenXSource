/* -----------------------------------------------
 * NuGenTabControlTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;

namespace Genetibase.Controls.Tests
{
	partial class NuGenTabControlTests
	{
		class TabControlStub : NuGenTabControl
		{
			public new List<NuGenTabButton> TabButtons
			{
				get
				{
					return base.TabButtons;
				}
			}
		}
	}
}
