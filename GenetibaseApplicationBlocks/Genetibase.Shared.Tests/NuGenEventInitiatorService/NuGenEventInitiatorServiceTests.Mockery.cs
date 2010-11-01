/* -----------------------------------------------
 * NuGenEventInitiatorServiceTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
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

		class DummyControl : Control
		{
			#region Declarations.Fields

			private NuGenEventInitiatorService _initiator = null;

			#endregion

			#region Methods.Public.Start

			/*
			 * Start
			 */

			public void Start()
			{
				this.OnStarted(EventArgs.Empty);
			}

			private static readonly object eventStarted = new object();

			public event EventHandler Started
			{
				add
				{
					this.Events.AddHandler(eventStarted, value);
				}
				remove
				{
					this.Events.RemoveHandler(eventStarted, value);
				}
			}

			protected virtual void OnStarted(EventArgs e)
			{
				_initiator.InvokeAction(eventStarted, e);
			}

			/*
			 * StartCustomHandler
			 */

			public void StartCustomHandler()
			{
				this.OnStartedCustomHandler(EventArgs.Empty);
			}

			private static readonly object eventStartedCustomHandler = new object();

			public event CustomEventHandler StartedCustomHandler
			{
				add
				{
					this.Events.AddHandler(eventStartedCustomHandler, value);
				}
				remove
				{
					this.Events.RemoveHandler(eventStartedCustomHandler, value);
				}
			}

			protected virtual void OnStartedCustomHandler(EventArgs e)
			{
				_initiator.InvokeAction(eventStartedCustomHandler, e);
			}

			#endregion

			#region Methods.Public.StartGeneric

			/*
			 * StartGeneric
			 */

			public void StartGeneric()
			{
				this.OnStartedGeneric(EventArgs.Empty);
			}

			private static readonly object eventStartedGeneric = new object();

			public event EventHandler<EventArgs> StartedGeneric
			{
				add
				{
					this.Events.AddHandler(eventStartedGeneric, value);
				}
				remove
				{
					this.Events.RemoveHandler(eventStartedGeneric, value);
				}
			}

			protected virtual void OnStartedGeneric(EventArgs e)
			{
				_initiator.InvokeActionT<EventArgs>(eventStartedGeneric, e);
			}

			/*
			 * StartGenericCustomHandler
			 */

			public void StartGenericCustomHandler()
			{
				this.OnStartedGenericCustomHandler(EventArgs.Empty);
			}

			private static readonly object eventStartedGenericCustomHandler = new object();

			public event CustomEventHandler StartedGenericCustomHandler
			{
				add
				{
					this.Events.AddHandler(eventStartedGenericCustomHandler, value);
				}
				remove
				{
					this.Events.RemoveHandler(eventStartedGenericCustomHandler, value);
				}
			}

			protected virtual void OnStartedGenericCustomHandler(EventArgs e)
			{
				_initiator.InvokeActionT<EventArgs>(eventStartedGenericCustomHandler, e);
			}

			#endregion

			#region Methods.Public.StartUnsubscribed

			public void StartUnsubscribed()
			{
				this.OnUnsubscribedStarted(EventArgs.Empty);
			}

			private static readonly object eventUnsubscribedStarted = new object();

			public event EventHandler UnsubscribedStarted
			{
				add
				{
					this.Events.AddHandler(eventUnsubscribedStarted, value);
				}
				remove
				{
					this.Events.RemoveHandler(eventUnsubscribedStarted, value);
				}
			}

			protected virtual void OnUnsubscribedStarted(EventArgs e)
			{
				_initiator.InvokeAction(eventUnsubscribedStarted, e);
			}

			#endregion

			#region Methods.Public.MouseAction

			/*
			 * StartMouseAction
			 */

			public void StartMouseAction(MouseEventArgs mouseEventArgs)
			{
				this.OnMouseAction(mouseEventArgs);
			}

			private static readonly object eventMouseAction = new object();

			public event MouseEventHandler MouseAction
			{
				add
				{
					this.Events.AddHandler(eventMouseAction, value);
				}
				remove
				{
					this.Events.RemoveHandler(eventMouseAction, value);
				}
			}

			protected virtual void OnMouseAction(MouseEventArgs e)
			{
				_initiator.InvokeMouseAction(eventMouseAction, e);
			}

			/*
			 * StartMouseActionCustomHandler
			 */

			public void StartMouseActionCustomHandler()
			{
				this.OnMouseActionCustomHandler(new MouseEventArgs(MouseButtons.None, 0, 0, 0, 0));
			}

			private static readonly object eventMouseActionCustomHandler = new object();

			public event CustomEventHandler MouseActionCustomHandler
			{
				add
				{
					this.Events.AddHandler(eventMouseActionCustomHandler, value);
				}
				remove
				{
					this.Events.RemoveHandler(eventMouseActionCustomHandler, value);
				}
			}

			protected virtual void OnMouseActionCustomHandler(MouseEventArgs e)
			{
				_initiator.InvokeMouseAction(eventMouseActionCustomHandler, e);
			}

			#endregion

			#region Methods.Public.PropertyChanged

			/*
			 * PropertyChanged
			 */

			public void StartPropertyChanged()
			{
				this.OnPropertyChanged();
			}

			private static readonly object eventPropertyChanged = new object();

			public event EventHandler PropertyChanged
			{
				add
				{
					this.Events.AddHandler(eventPropertyChanged, value);
				}
				remove
				{
					this.Events.RemoveHandler(eventPropertyChanged, value);
				}
			}

			protected virtual void OnPropertyChanged()
			{
				_initiator.InvokePropertyChanged(eventPropertyChanged, EventArgs.Empty);
			}

			/*
			 * PropertyChangedCustomHandler
			 */

			public void StartPropertyChangedCustomHandler()
			{
				this.OnPropertyChangedCustomHandler(EventArgs.Empty);
			}

			private static readonly object eventPropertyChangedCustomHandler = new object();

			public event CustomEventHandler PropertyChangedCustomHandler
			{
				add
				{
					this.Events.AddHandler(eventPropertyChangedCustomHandler, value);
				}
				remove
				{
					this.Events.RemoveHandler(eventPropertyChangedCustomHandler, value);
				}
			}

			protected virtual void OnPropertyChangedCustomHandler(EventArgs e)
			{
				_initiator.InvokePropertyChanged(eventPropertyChangedCustomHandler, e);
			}

			#endregion

			#region Constructors

			public DummyControl()
			{
				_initiator = new NuGenEventInitiatorService(this, this.Events);
			}

			#endregion
		}

		class DummyControlEventSink : MockObject
		{
			#region Properties.Expectations.Start

			/*
			 * StartedCallsCount
			 */

			private ExpectationCounter startedCallsCount = new ExpectationCounter("startedCallsCount");

			public int ExpectedStartedCallsCount
			{
				set
				{
					this.startedCallsCount.Expected = value;
				}
			}

			/*
			 * Sender
			 */

			private ExpectationValue sender = new ExpectationValue("sender");

			public object ExpectedSender
			{
				set
				{
					this.sender.Expected = value;
				}
			}

			#endregion

			#region Properties.Expectations.StartGeneric

			/*
			 * StartedGenericCallsCount
			 */

			private ExpectationCounter startedGenericCallsCount = new ExpectationCounter("startedGenericCallsCount");

			public int ExpectedStartedGenericCallsCount
			{
				set
				{
					this.startedGenericCallsCount.Expected = value;
				}
			}

			/*
			 * SenderGeneric
			 */

			private ExpectationValue senderGeneric = new ExpectationValue("senderGeneric");

			public object ExpectedSenderGeneric
			{
				set
				{
					this.senderGeneric.Expected = value;
				}
			}

			#endregion

			#region Properties.Expectations.StartMouseAction

			/*
			 * StartedMouseCallsCount
			 */

			private ExpectationCounter startedMouseCallsCount = new ExpectationCounter("startedMouseCallsCount");

			public int ExpectedStartedMouseCallsCount
			{
				set
				{
					this.startedMouseCallsCount.Expected = value;
				}
			}

			/*
			 * StartedMouseSender
			 */

			private ExpectationValue startedMouseSender = new ExpectationValue("startedMouseSender");

			public object ExpectedStartedMouseSender
			{
				set
				{
					this.startedMouseSender.Expected = value;
				}
			}

			/*
			 * StartedMouseEventArgs
			 */

			private ExpectationValue startedMouseEventArgs = new ExpectationValue("statedMouseEventArgs");

			public MouseEventArgs ExpectedStartedMouseEventArgs
			{
				set
				{
					this.startedMouseEventArgs.Expected = value;
				}
			}

			#endregion

			#region Properties.Expectatioins.StartPropertyChanged

			private ExpectationCounter propertyChangedCallsCount = new ExpectationCounter("propertyChangedCallsCount");

			public int ExpectedPropertyChangedCallsCount
			{
				set
				{
					this.propertyChangedCallsCount.Expected = value;
				}
			}

			#endregion

			#region Constructors

			public DummyControlEventSink(DummyControl dummyControl)
			{
				if (dummyControl == null)
				{
					Assert.Fail("dummyControl cannot be null.");
				}

				dummyControl.StartedGeneric += delegate(object sender, EventArgs e)
				{
					this.startedGenericCallsCount.Inc();
					this.senderGeneric.Actual = sender;
				};

				dummyControl.Started += delegate(object sender, EventArgs e)
				{
					this.startedCallsCount.Inc();
					this.sender.Actual = sender;
				};

				dummyControl.MouseAction += delegate(object sender, MouseEventArgs e)
				{
					this.startedMouseCallsCount.Inc();
					this.startedMouseSender.Actual = sender;
					this.startedMouseEventArgs.Actual = e;
				};

				dummyControl.StartedCustomHandler += delegate
				{
				};

				dummyControl.StartedGenericCustomHandler += delegate
				{
				};

				dummyControl.MouseActionCustomHandler += delegate
				{
				};

				dummyControl.PropertyChanged += delegate
				{
					this.propertyChangedCallsCount.Inc();
				};

				dummyControl.PropertyChangedCustomHandler += delegate
				{
				};
			}

			#endregion
		}
	}
}
