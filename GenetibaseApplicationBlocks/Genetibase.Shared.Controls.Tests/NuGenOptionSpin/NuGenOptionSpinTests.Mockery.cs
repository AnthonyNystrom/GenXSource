/* -----------------------------------------------
 * NuGenOptionSpinTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.Tests
{
	partial class NuGenSmoothOptionSpinTests
	{
		class StubSpin : NuGenOptionSpin
		{
			public new TextBox EditBox
			{
				get
				{
					return base.EditBox;
				}
			}

			public StubSpin()
				: base(new NuGenControlServiceProvider())
			{
			}
		}
	}
}
