/* -----------------------------------------------
 * NuGenAlignSelectorTests.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using Genetibase.Shared.Controls.RadioButtonInternals;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls.Tests
{
	partial class NuGenAlignSelectorTests
	{
		private sealed class EventSink : MockObject
		{
			private ExpectationCounter _alignmentChangedCounter = new ExpectationCounter("alignmentChangedCounter");

			public int ExpectedAlignmentChangedCount
			{
				set
				{
					_alignmentChangedCounter.Expected = value;
				}
			}

			public EventSink(NuGenAlignSelector eventInitiator)
			{
				Assert.IsNotNull(eventInitiator);

				eventInitiator.AlignmentChanged += delegate
				{
					_alignmentChangedCounter.Inc();
				};
			}
		}

		private sealed class ServiceProvider : NuGenControlServiceProvider
		{
			private INuGenRadioButtonLayoutManager _radioButtonLayoutManager = new RadioButtonLayoutManager();
			private INuGenRadioButtonRenderer _radioButtonRenderer = new RadioButtonRenderer();

			protected override object GetService(Type serviceType)
			{
				Assert.IsNotNull(serviceType);

				if (serviceType == typeof(INuGenRadioButtonLayoutManager))
				{
					return _radioButtonLayoutManager;
				}
				else if (serviceType == typeof(INuGenRadioButtonRenderer))
				{
					return _radioButtonRenderer;
				}

				return base.GetService(serviceType);
			}
		}

		private sealed class RadioButtonLayoutManager : INuGenRadioButtonLayoutManager
		{
			public Size GetRadioSize()
			{
				return Size.Empty;
			}

			public Rectangle GetRadioButtonBounds(NuGenBoundsParams radioBoundsParams)
			{
				Assert.IsNotNull(radioBoundsParams);
				return Rectangle.Empty;
			}

			public Rectangle GetTextBounds(NuGenBoundsParams textBoundsParams)
			{
				Assert.IsNotNull(textBoundsParams);
				return Rectangle.Empty;
			}
		}

		private sealed class RadioButtonRenderer : INuGenRadioButtonRenderer
		{
			public void DrawRadioButton(NuGenRadioButtonPaintParams paintParams)
			{
				Assert.IsNotNull(paintParams);
			}

			public void DrawText(NuGenTextPaintParams paintParams)
			{
				Assert.IsNotNull(paintParams);
			}
		}
	}
}
