/* -----------------------------------------------
 * NuGenPresenterForm.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.ApplicationBlocks.PresenterInternals
{
	partial class NuGenPresenterForm
	{
		private sealed class Transform : NuGenPair<float>
		{
			public float X
			{
				get
				{
					return this.GetValueOne();
				}
				set
				{
					this.SetValueOne(value);
				}
			}

			public float Y
			{
				get
				{
					return this.GetValueTwo();
				}
				set
				{
					this.SetValueTwo(value);
				}
			}

			public Transform(float x, float y)
				: base(x, y)
			{
			}
		}
	}
}
