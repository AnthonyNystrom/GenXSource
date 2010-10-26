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
	partial class NuGenSpinTests
	{
		private sealed class StubSpin : NuGenSpin
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
				: base(new ServiceProvider())
			{
			}
		}

		private sealed class ServiceProvider : NuGenControlServiceProvider
		{
			private INuGenInt32ValueConverter _int32ValueConverter;

			protected override object GetService(Type serviceType)
			{
				Assert.IsNotNull(serviceType);

				if (serviceType == typeof(INuGenInt32ValueConverter))
				{
					return _int32ValueConverter;
				}

				return base.GetService(serviceType);
			}

			public ServiceProvider()
			{
				_int32ValueConverter = new NuGenInt32ValueConverter();
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

			public EventSink(StubSpin spin)
			{
				Assert.IsNotNull(spin);

				spin.ValueChanged += delegate
				{
					_valueChangedCount.Inc();
				};
			}
		}
	}
}
