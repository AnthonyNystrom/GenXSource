/* -----------------------------------------------
 * NuGenProgressBarTests.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using DotNetMock;
using DotNetMock.Dynamic;

using Genetibase.Shared.Controls.ProgressBarInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls.Tests
{
	partial class NuGenProgressBarTests
	{
		private sealed class DummyLayoutManager : INuGenProgressBarLayoutManager
		{
			public Rectangle[] GetBlocks(Rectangle continuousBounds, NuGenOrientationStyle orientation)
			{
				return null;
			}

			public Rectangle GetContinuousBounds(Rectangle containerBounds, int min, int max, int value, NuGenOrientationStyle orientation)
			{
				return Rectangle.Empty;
			}

			public Rectangle GetMarqueeBlockBounds(Rectangle containerBounds, int offset, NuGenOrientationStyle orientation)
			{
				return Rectangle.Empty;
			}
		}

		private sealed class ProgressBarServiceProvider : NuGenControlServiceProvider
		{
			private INuGenProgressBarLayoutManager _layoutManager;

			private INuGenProgressBarLayoutManager LayoutManager
			{
				get
				{
					if (_layoutManager == null)
					{
						_layoutManager = new DummyLayoutManager();
					}

					return _layoutManager;
				}
			}

			private DynamicMock _renderer;

			private INuGenProgressBarRenderer Renderer
			{
				get
				{
					if (_renderer == null)
					{
						_renderer = new DynamicMock(typeof(INuGenProgressBarRenderer));
					}

					return (INuGenProgressBarRenderer)_renderer.Object;
				}
			}

			protected override object GetService(Type serviceType)
			{
				if (serviceType == typeof(INuGenProgressBarLayoutManager))
				{
					return this.LayoutManager;
				}
				else if (serviceType == typeof(INuGenProgressBarRenderer))
				{
					return this.Renderer;
				}

				return base.GetService(serviceType);
			}

			public ProgressBarServiceProvider()
			{
			}
		}
	}
}
