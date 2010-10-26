/* -----------------------------------------------
 * Mockery.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Tests
{
	partial class NuGenInvokerTests
	{
		public class StaticClass
		{
			static int Method(int param)
			{
				return param * 2;
			}
		}

		public class ParentClass : ChildClass
		{
		}

		public class ChildClass
		{
			private int privateField;

			public int PrivateField
			{
				get
				{
					return privateField;
				}
			}

			public ChildClass()
			{
				privateField = 0;
			}
		}

		public class DummyClass
		{
			class WithEventClass
			{
				public event EventHandler SomeEvent;

				private void InvokeSomeEvent()
				{
					if (SomeEvent != null)
					{
						SomeEvent(this, EventArgs.Empty);
					}
				}

				private int _property;

				public int Property
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

			private int _privateField;

			public int PrivateField
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

			protected void InvokeWithParameters(int param)
			{
				_privateField = param;
			}

			protected int InvokeWithReturnValue(int param)
			{
				return param * 2;
			}

			private WithEventClass _eventsClass = new WithEventClass();
		}
	}
}
