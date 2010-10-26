/* -----------------------------------------------
 * NuGenEventInitiatorTests.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using Genetibase.Shared.ComponentModel;

using NUnit.Framework;

using System;

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

			private static readonly object _DummyEvent = new object();

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

			public void SetExpectedDummyEventCalls(int value)
			{
				_DummyEventCallsCount.Expected = value;
			}

			public MockStubEventInitiatorSink(StubEventInitiator stubEventInitiator)
			{
				if (stubEventInitiator == null)
				{
					Assert.Fail("stubEventInitiator cannot be null.");
				}

				stubEventInitiator.DummyEvent += delegate
				{
					_DummyEventCallsCount.Inc();
				};
			}
		}
	}
}
