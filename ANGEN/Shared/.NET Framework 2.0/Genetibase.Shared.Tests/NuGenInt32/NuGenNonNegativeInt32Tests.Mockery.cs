/* -----------------------------------------------
 * NuGenNonNegativeInt32Tests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Tests
{
	partial class NuGenNonNegativeInt32Tests
	{
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

			public EventSink(NuGenNonNegativeInt32 eventBubbler)
			{
				Assert.IsNotNull(eventBubbler);

				eventBubbler.ValueChanged += delegate
				{
					_valueChangedCount.Inc();
				};
			}
		}
	}
}
