/* -----------------------------------------------
 * NuGenEventInitiatorTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using Genetibase.NuGenMock;
using Genetibase.Shared.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetibase.Shared.Tests
{
	partial class NuGenEventInitiatorTests
	{
		class DummyEventArgs : EventArgs
		{
		}

		class StubEventInitiator : NuGenEventInitiator
		{
			public void Run()
			{
				this.OnDummyEvent(new DummyEventArgs());
			}

			private static readonly Object _DummyEvent = new Object();

			public event EventHandler<DummyEventArgs> DummyEvent
			{
				add
				{
					this.Events.AddHandler(_DummyEvent, value);
				}
				remove
				{
					this.Events.RemoveHandler(_DummyEvent, value);
				}
			}

			protected virtual void OnDummyEvent(DummyEventArgs e)
			{
				this.Initiator.InvokeEventHandlerT<DummyEventArgs>(_DummyEvent, e);
			}
		}

		class MockStubEventInitiatorSink : MockObject
		{
			ExpectationCounter _DummyEventCallsCount = new ExpectationCounter("_DummyEventCallsCount");

			public void SetExpectedDummyEventCalls(Int32 value)
			{
				_DummyEventCallsCount.Expected = value;
			}

			public MockStubEventInitiatorSink(StubEventInitiator stubEventInitiator)
			{
				Assert.IsNotNull(stubEventInitiator);

				stubEventInitiator.DummyEvent += delegate
				{
					_DummyEventCallsCount.Inc();
				};
			}
		}
	}
}
