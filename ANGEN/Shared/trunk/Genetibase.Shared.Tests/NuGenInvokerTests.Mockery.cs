/* -----------------------------------------------
 * Mockery.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Tests
{
	partial class NuGenInvokerTests
	{
		internal sealed class StaticClass
		{
			static Int32 Method(Int32 param)
			{
				return param * 2;
			}
		}

		internal sealed class ParentClass : ChildClass
		{
		}

		internal class ChildClass
		{
			private Int32 privateField;

			public Int32 PrivateField
			{
				get
				{
					return privateField;
				}
			}

			private Object PrivateProperty
			{
				get
				{
					return new Object();
				}
			}

			private void PrivateMethod()
			{
			}

			private event EventHandler PrivateEvent;

			public ChildClass()
			{
				privateField = 0;
				this.PrivateEvent += null;
			}
		}

		internal class DummyClass
		{
			private class WithEventClass
			{
				public event EventHandler SomeEvent;

				private void InvokeSomeEvent()
				{
					if (SomeEvent != null)
					{
						SomeEvent(this, EventArgs.Empty);
					}
				}

				private Int32 _property;

				public Int32 Property
				{
					get
					{
						return _property;
					}
					set
					{
						_property = value;
					}
				}
			}

			private Int32 _privateField;

			public Int32 PrivateField
			{
				get
				{
					return _privateField;
				}
				set
				{
					_privateField = value;
				}
			}

			protected void InvokeWithParameters(Int32 param)
			{
				_privateField = param;
			}

			protected Int32 InvokeWithReturnValue(Int32 param)
			{
				return param * 2;
			}

			private WithEventClass _eventsClass = new WithEventClass();
		}
	}
}
