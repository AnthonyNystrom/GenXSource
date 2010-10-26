/* -----------------------------------------------
 * Mockery.cs
 * Copyright � 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Tests
{
	partial class NuGenObjectDescriptorTests
	{
		public class DummyClass
		{
			private int _myProperty;

			public int MyProperty
			{
				get
				{
					return _myProperty;
				}
				set
				{
					_myProperty = value;
				}
			}
		}
	}
}