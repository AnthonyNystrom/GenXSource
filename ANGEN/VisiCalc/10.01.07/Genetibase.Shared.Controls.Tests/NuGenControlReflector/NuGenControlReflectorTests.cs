/* -----------------------------------------------
 * NuGenControlReflectorTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.Tests
{
	[TestFixture]
	public class NuGenControlReflectorTests
	{
		private NuGenControlReflector _reflector;
		private Control _dummyControl;
		private Form _dummyParent;

		[SetUp]
		public void SetUp()
		{
			_dummyControl = new Control();
			_dummyParent = new Form();
			_reflector = new NuGenControlReflector();
		}

		[Test]
		public void ControlToReflectArgumentException()
		{
			try
			{
				_reflector.ControlToReflect = _dummyControl;
				Assert.Fail();
			}
			catch (ArgumentException)
			{
			}

			try
			{
				_reflector.ControlToReflect = _reflector;
				Assert.Fail();
			}
			catch (ArgumentException)
			{
			}

			try
			{
				_reflector.ControlToReflect = _dummyParent;
			}
			catch (ArgumentException)
			{
			}
		}
	}
}
