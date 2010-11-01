/* -----------------------------------------------
 * NuGenTrackBarTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Controls.Tests
{
	partial class NuGenTrackBarTests
	{
		class TrackBarServiceProvider : NuGenControlServiceProvider
		{
			private INuGenValueTracker _tracker = new NuGenValueTracker();

			protected override object GetService(Type serviceType)
			{
				if (serviceType == typeof(INuGenValueTracker))
				{
					return _tracker;
				}

				return base.GetService(serviceType);
			}
		}
	}
}
