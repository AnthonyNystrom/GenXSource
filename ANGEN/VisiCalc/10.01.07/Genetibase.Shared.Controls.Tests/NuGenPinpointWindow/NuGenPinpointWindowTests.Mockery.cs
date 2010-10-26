/* -----------------------------------------------
 * NuGenPinpointWindowTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.PinpointInternals;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls.Tests
{
	partial class NuGenPinpointWindowTests
	{
		private sealed class LayoutManager : INuGenPinpointLayoutManager
		{
			public Rectangle GetSelectionFrameBounds(int desiredLeft, int desiredTop, int desiredWidth, int desiredHeight)
			{
				return new Rectangle(desiredLeft, desiredTop, desiredWidth, desiredHeight);
			}
		}

		private sealed class Renderer : INuGenPinpointRenderer
		{
			public void DrawFisheyeExpander(NuGenPaintParams paintParams)
			{
				Assert.IsNotNull(paintParams);
			}

			public void DrawText(NuGenTextPaintParams paintParams)
			{
				Assert.IsNotNull(paintParams);
			}

			public void DrawSelectionFrame(NuGenPaintParams paintParams)
			{
				Assert.IsNotNull(paintParams);
			}
		}

		private sealed class ServiceProvider : NuGenControlServiceProvider
		{
			private INuGenPinpointLayoutManager _layoutManager = new LayoutManager();
			private INuGenPinpointRenderer _renderer = new Renderer();

			protected override object GetService(Type serviceType)
			{
				Assert.IsNotNull(serviceType);
			
				if (serviceType == typeof(INuGenPinpointLayoutManager))
				{
					return _layoutManager;
				}
				else if (serviceType == typeof(INuGenPinpointRenderer))
				{
					return _renderer;
				}

				return base.GetService(serviceType);
			}

			public ServiceProvider()
			{
			}
		}
	}
}
