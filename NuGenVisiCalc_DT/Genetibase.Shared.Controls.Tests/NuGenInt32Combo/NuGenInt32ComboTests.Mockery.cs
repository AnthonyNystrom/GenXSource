/* -----------------------------------------------
 * NuGenInt32ComboTests.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using Genetibase.Shared.Controls.ComboBoxInternals;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.Tests
{
	partial class NuGenInt32ComboTests
	{
		private sealed class StubInt32Combo : NuGenInt32Combo
		{
			public new void OnKeyDown(KeyEventArgs e)
			{
				base.OnKeyDown(e);
			}

			public StubInt32Combo()
				: base(new ServiceProvider())
			{
			}
		}

		private sealed class EventSink : MockObject
		{
			private ExpectationCounter _valueChangedCount = new ExpectationCounter("valueChangedCount");

			public int ExpectedValueChangedCount
			{
				set
				{
					_valueChangedCount.Expected = value;
				}
			}

			public EventSink(NuGenInt32Combo eventInitiator)
			{
				Assert.IsNotNull(eventInitiator);

				eventInitiator.ValueChanged += delegate
				{
					_valueChangedCount.Inc();
				};
			}
		}

		private sealed class ServiceProvider : NuGenImageControlServiceProvider
		{
			private INuGenComboBoxRenderer _comboBoxRenderer;
			private INuGenInt32ValueConverter _int32ValueConverter;

			protected override object GetService(Type serviceType)
			{
				Assert.IsNotNull(serviceType);

				if (serviceType == typeof(INuGenComboBoxRenderer))
				{
					return _comboBoxRenderer;
				}
				else if (serviceType == typeof(INuGenInt32ValueConverter))
				{
					return _int32ValueConverter;
				}

				return base.GetService(serviceType);
			}

			public ServiceProvider()
			{
				_comboBoxRenderer = new ComboBoxRenderer();
				_int32ValueConverter = new NuGenInt32ValueConverter();
			}
		}

		private sealed class ComboBoxRenderer : INuGenComboBoxRenderer
		{
			public void DrawBorder(NuGenPaintParams paintParams)
			{
				Assert.IsNotNull(paintParams);
			}

			public void DrawComboBoxButton(NuGenPaintParams paintParams)
			{
				Assert.IsNotNull(paintParams);
			}

			public void DrawItem(NuGenItemPaintParams paintParams)
			{
				Assert.IsNotNull(paintParams);
			}

			public ComboBoxRenderer()
			{
			}
		}
	}
}
