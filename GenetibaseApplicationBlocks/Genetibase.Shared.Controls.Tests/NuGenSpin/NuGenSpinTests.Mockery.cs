/* -----------------------------------------------
 * NuGenSpinTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.Tests
{
	partial class NuGenSmoothSpinTests
	{
		class StubSpin : NuGenSpin
		{
			public TextBox TextBox
			{
				[DebuggerStepThrough]
				get
				{
					return this.EditBox;
				}
			}

			public StubSpin()
				: base(new NuGenControlServiceProvider())
			{
			}
		}

		class EventSink : MockObject
		{
			#region Expectations

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

			public EventSink(StubSpin spin)
			{
				Assert.IsNotNull(spin);

				spin.ValueChanged += delegate
				{
					_valueChangedCount.Inc();
				};
			}

			#endregion
		}
	}
}
