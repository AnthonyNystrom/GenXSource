/* -----------------------------------------------
 * NuGenEventInitiatorServiceTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.ComponentModel;
using System.Windows.Controls;
using Genetibase.NuGenMock;
using Genetibase.Shared.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetibase.Shared.Tests
{
	partial class NuGenEventInitiatorServiceTests
	{
		public delegate void CustomEventHandler(Object param);
		public delegate void InvalidParamTypeEventHandler(EventArgs e, Object sender);

		class DummyControl : Control
		{
			public void Start()
			{
				this.OnStarted(EventArgs.Empty);
			}

			private INuGenEventHandlerListProvider _handlerListProvider;

			private INuGenEventHandlerListProvider HandlerListProvider
			{
				get
				{
					if (_handlerListProvider == null)
					{
						_handlerListProvider = new NuGenEventHandlerListProvider();
					}

					return _handlerListProvider;
				}
			}

			private EventHandlerList Events
			{
				get
				{
					return this.HandlerListProvider.Events;
				}
			}

			private static readonly Object _started = new Object();

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

			private static readonly Object _startedCustomHandler = new Object();

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

			private static readonly Object _startedInvalidHandler = new Object();

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

			private static readonly Object _startedInvalidParamCountHandler = new Object();

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

			private NuGenEventInitiatorService _initiator;

			public DummyControl()
			{
				_initiator = new NuGenEventInitiatorService(this, this.Events);
			}
		}

		class DummyControlEventSink : MockObject
		{
			private ExpectationCounter _startedCallsCount = new ExpectationCounter("startedCallsCount");

			public Int32 ExpectedStartedCallsCount
			{
				set
				{
					_startedCallsCount.Expected = value;
				}
			}

			private ExpectationValue<Object> _sender = new ExpectationValue<Object>("sender");

			public Object ExpectedSender
			{
				set
				{
					_sender.Expected = value;
				}
			}

			public DummyControlEventSink(DummyControl dummyControl)
			{
				Assert.IsNotNull(dummyControl);

				dummyControl.Started += delegate(Object sender, EventArgs e)
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
