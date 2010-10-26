/* -----------------------------------------------
 * NuGenCalculatorEngineTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.CalculatorInternals;

using DotNetMock;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.Tests
{
	partial class NuGenCalculatorEngineTests
	{
		private sealed class EventSink : MockObject
		{
			private ExpectationCounter _valueCounter = new ExpectationCounter("valueCounter");

			public int ExpectedValueCount
			{
				set
				{
					_valueCounter.Expected = value;
				}
			}

			public EventSink(Engine engine)
			{
				Assert.IsNotNull(engine);

				engine.ValueChanged += delegate
				{
					_valueCounter.Inc();
				};
			}
		}
	}
}
