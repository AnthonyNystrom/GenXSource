/* -----------------------------------------------
 * NuGenInt32Tests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using NUnit.Framework;

using System;
using System.Collections.Generic;

namespace Genetibase.Shared.Tests
{
	partial class NuGenInt32Tests
	{
		private sealed class EventSink : MockObject
		{
			/*
			 * MaximumChanged
			 */

			private ExpectationCounter _maximumChangedCount = new ExpectationCounter("maximumChangedCount");

			public int ExpectedMaximumChangedCount
			{
				set
				{
					_maximumChangedCount.Expected = value;
				}
			}

			/*
			 * MinimumChanged
			 */

			private ExpectationCounter _minimumChangedCount = new ExpectationCounter("minimumChangedCount");

			public int ExpectedMinimumChangedCount
			{
				set
				{
					_minimumChangedCount.Expected = value;
				}
			}

			/*
			 * ValueChanged
			 */

			private ExpectationCounter _valueChangedCount = new ExpectationCounter("valueChangedCount");

			public int ExpectedValueChangedCount
			{
				set
				{
					_valueChangedCount.Expected = value;
				}
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="EventSink"/> class.
			/// </summary>
			public EventSink(NuGenInt32 eventBubbler)
			{
				Assert.IsNotNull(eventBubbler);

				eventBubbler.MaximumChanged += delegate
				{
					_maximumChangedCount.Inc();
				};

				eventBubbler.MinimumChanged += delegate
				{
					_minimumChangedCount.Inc();
				};

				eventBubbler.ValueChanged += delegate
				{
					_valueChangedCount.Inc();
				};
			}
		}
	}
}
