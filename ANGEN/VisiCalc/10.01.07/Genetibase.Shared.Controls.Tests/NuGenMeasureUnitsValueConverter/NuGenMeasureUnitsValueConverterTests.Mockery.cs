/* -----------------------------------------------
 * NuGenMeasureUnitsValueConverterTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.Tests
{
	partial class NuGenMeasureUnitsValueConverterTests
	{
		private sealed class EventSink : MockObject
		{
			private ExpectationCounter _valueChangedCounter = new ExpectationCounter("valueChangedCounter");

			public Int32 ExpectedValueChangedCount
			{
				set
				{
					_valueChangedCounter.Expected = value;
				}
			}

			public EventSink(NuGenMeasureUnitsValueConverter eventInitiator)
			{
				Assert.IsNotNull(eventInitiator);

				eventInitiator.ValueChanged += delegate
				{
					_valueChangedCounter.Inc();
				};
			}
		}
	}
}
