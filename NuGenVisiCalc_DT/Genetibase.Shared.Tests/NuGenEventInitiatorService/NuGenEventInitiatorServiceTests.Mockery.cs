/* -----------------------------------------------
 * NuGenEventInitiatorServiceTests.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using Genetibase.Shared.ComponentModel;

using NUnit.Framework;

using System;
using System.Windows.Forms;

namespace Genetibase.Shared.Tests
{
	partial class NuGenEventInitiatorServiceTests
	{
		public delegate void CustomEventHandler(object param);
		public delegate void InvalidParamTypeEventHandler(EventArgs e, object sender);

		class DummyControl : Control
		{
			public void Start()
			{
				this.OnStarted(EventArgs.Empty);
			}

			private static readonly object _started = new object();

			public event EventHandler<EventArgs> Started
			{
				add
				{
					this.Events.AddHandler(_started, value);
				}
				remove
				{
					this.Events.RemoveHandler(_started, value);
				}
			}

			protected virtual void OnStarted(EventArgs e)
			{
				_initiator.InvokeHandler<EventHandler<EventArgs>, EventArgs>(_started, e);
			}

			public void StartCustomHandler()
			{
				this.OnStartedCustomHandler(EventArgs.Empty);
			}

			private static readonly object _startedCustomHandler = new object();

			public event CustomEventHandler StartedCustomHandler
			{
				add
				{
					this.Events.AddHandler(_startedCustomHandler, value);
				}
				remove
				{
					this.Events.RemoveHandler(_startedCustomHandler, value);
				}
			}

			protected virtual void OnStartedCustomHandler(EventArgs e)
			{
				_initiator.InvokeHandler<EventHandler, EventArgs>(_startedCustomHandler, e);
			}

			public void StartInvalidHandler()
			{
				this.OnStartedInvalidHandler(EventArgs.Empty);
			}

			private static readonly object _startedInvalidHandler = new object();

			public event CustomEventHandler StartedInvalidHandler
			{
				add
				{
					this.Events.AddHandler(_startedInvalidHandler, value);
				}
				remove
				{
					this.Events.RemoveHandler(_startedInvalidHandler, value);
				}
			}

			protected virtual void OnStartedInvalidHandler(EventArgs e)
			{
				_initiator.InvokeHandler<CustomEventHandler, EventArgs>(_startedInvalidHandler, e);
			}

			public void StartInvalidParamCountHandler()
			{
				this.OnStartedInvalidParamCountHandler(EventArgs.Empty);
			}

			private static readonly object _startedInvalidParamCountHandler = new object();

			public event InvalidParamTypeEventHandler StartedInvalidParamCountHandler
			{
				add
				{
					this.Events.AddHandler(_startedInvalidParamCountHandler, value);
				}
				remove
				{
					this.Events.RemoveHandler(_startedInvalidParamCountHandler, value);
				}
			}

			protected virtual void OnStartedInvalidParamCountHandler(EventArgs e)
			{
				_initiator.InvokeHandler<InvalidParamTypeEventHandler, EventArgs>(_startedInvalidParamCountHandler, e);
			}

			private NuGenEventInitiatorService _initiator = null;

			public DummyControl()
			{
				_initiator = new NuGenEventInitiatorService(this, this.Events);
			}
		}

		class DummyControlEventSink : MockObject
		{
			private ExpectationCounter _startedCallsCount = new ExpectationCounter("startedCallsCount");

			public int ExpectedStartedCallsCount
			{
				set
				{
					_startedCallsCount.Expected = value;
				}
			}

			private ExpectationValue _sender = new ExpectationValue("sender");

			public object ExpectedSender
			{
				set
				{
					_sender.Expected = value;
				}
			}

			public DummyControlEventSink(DummyControl dummyControl)
			{
				if (dummyControl == null)
				{
					Assert.Fail("dummyControl cannot be null.");
				}

				dummyControl.Started += delegate(object sender, EventArgs e)
				{
					_startedCallsCount.Inc();
					_sender.Actual = sender;
				};

				dummyControl.StartedCustomHandler += delegate
				{
				};

				dummyControl.StartedInvalidHandler += delegate
				{
				};

				dummyControl.StartedInvalidParamCountHandler += delegate
				{
				};
			}
		}
	}
}
