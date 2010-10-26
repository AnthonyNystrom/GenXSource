/* -----------------------------------------------
 * NuGenInt32ValueConverterTests.cs
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
	partial class NuGenInt32ValueConverterTests
	{
		private sealed class EventSink : MockObject
		{
			private ExpectationCounter _valueChangedCounter = new ExpectationCounter("valueChangedCounter");

			public int ExpectedValueChangedCount
			{
				set
				{
					_valueChangedCounter.Expected = value;
				}
			}

			public EventSink(NuGenInt32ValueConverter eventInitiator)
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
