/* -----------------------------------------------
 * NuGenPositiveInt32Tests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using NUnit.Framework;

using System;
using System.Collections.Generic;

namespace Genetibase.Shared.Tests
{
	partial class NuGenPositiveInt32Tests
	{
		class EventSink : MockObject
		{
			#region Expectations

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

			#endregion

			#region Constructors

			public EventSink(NuGenPositiveInt32 eventBubbler)
			{
				Assert.IsNotNull(eventBubbler);

				eventBubbler.ValueChanged += delegate
				{
					_valueChangedCount.Inc();
				};
			}

			#endregion
		}
	}
}
