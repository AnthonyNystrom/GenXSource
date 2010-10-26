/* -----------------------------------------------
 * NuGenInt32Tests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using Genetibase.NuGenMock;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

			public Int32 ExpectedMaximumChangedCount
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

			public Int32 ExpectedMinimumChangedCount
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

			public Int32 ExpectedValueChangedCount
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
