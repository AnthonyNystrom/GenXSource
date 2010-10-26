/* -----------------------------------------------
 * NuGenOptionSpinTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.Tests
{
	partial class NuGenSmoothOptionSpinTests
	{
		private sealed class StubSpin : NuGenOptionSpin
		{
			public new TextBox EditBox
			{
				get
				{
					return base.EditBox;
				}
			}

			public StubSpin()
				: base(new NuGenControlServiceProvider())
			{
			}
		}

		private sealed class EventSink : MockObject
		{
			private ExpectationCounter _selectedItemChangedCount = new ExpectationCounter("selectedItemChangedCount");

			public int ExpectedSelectedItemChangedCount
			{
				set
				{
					_selectedItemChangedCount.Expected = value;
				}
			}

			public EventSink(NuGenOptionSpin eventInitiator)
			{
				Assert.IsNotNull(eventInitiator);

				eventInitiator.SelectedItemChanged += delegate
				{
					_selectedItemChangedCount.Inc();
				};
			}
		}
	}
}
