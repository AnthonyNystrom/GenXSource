/* -----------------------------------------------
 * NuGenHotKeyOperationTests.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Windows;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public class NuGenHotKeyOperationTests
	{
		private NuGenHotKeyOperation _operation;
		private string _name;
		private Keys _hotKeys;
		private NuGenHotKeyOperationHandler _handler;

		[SetUp]
		public void SetUp()
		{
			_name = "Erase";
			_handler = this.EraseOperation;
			_hotKeys = Keys.E;

			_operation = new NuGenHotKeyOperation(_name, _handler, _hotKeys);
		}

		[Test]
		public void ConstructorTest()
		{
			Assert.AreEqual(_name, _operation.Name);
			Assert.AreEqual(_handler, _operation.Handler);
			Assert.AreEqual(_hotKeys, _operation.HotKeys);
		}

		[Test]
		public void ConstructorArgumentNullExceptionTest()
		{
			try
			{
				_operation = new NuGenHotKeyOperation("", _handler, _hotKeys);
				Assert.Fail();
			}
			catch (ArgumentNullException)
			{
			}

			try
			{
				_operation = new NuGenHotKeyOperation(null, _handler, _hotKeys);
				Assert.Fail();
			}
			catch (ArgumentNullException)
			{
			}

			try
			{
				_operation = new NuGenHotKeyOperation(_name, null, _hotKeys);
				Assert.Fail();
			}
			catch (ArgumentNullException)
			{
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void HandlerArgumentNullExceptionTest()
		{
			_operation.Handler = null;
		}

		[Test]
		public void NameArgumentNullExceptionTest()
		{
			try
			{
				_operation.Name = "";
				Assert.Fail();
			}
			catch (ArgumentNullException)
			{
			}

			try
			{
				_operation.Name = null;
				Assert.Fail();
			}
			catch (ArgumentNullException)
			{
			}
		}

		private void EraseOperation()
		{
		}
	}
}
