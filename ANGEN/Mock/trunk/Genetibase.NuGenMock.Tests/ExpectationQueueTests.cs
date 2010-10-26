/* -----------------------------------------------
 * ExpectationQueueTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetibase.NuGenMock.Tests
{
	[TestClass]
	public class ExpectationQueueTests
	{
		private ExpectationQueue<Int32> _expectationQueue;

		[TestInitialize]
		public void SetUp()
		{
			_expectationQueue = new ExpectationQueue<Int32>("expectationQueue");
		}

		[TestMethod]
		public void ClearActualTest()
		{
			Assert.IsFalse(_expectationQueue.IsVerified);
			_expectationQueue.ExpectNothing();
			_expectationQueue.Actual = 1;
			_expectationQueue.ClearActual();
			_expectationQueue.Verify();
			Assert.IsTrue(_expectationQueue.IsVerified);
			_expectationQueue.ClearActual();
			Assert.IsFalse(_expectationQueue.IsVerified);
		}

		[TestMethod]
		public void ClearExpectedTest()
		{
			_expectationQueue.Actual = 1;
			_expectationQueue.Expected = 1;
			Assert.IsTrue(_expectationQueue.HasExpectations);
			_expectationQueue.Verify();
			Assert.IsTrue(_expectationQueue.IsVerified);
			_expectationQueue.ClearExpected();
			Assert.IsFalse(_expectationQueue.HasExpectations);
			Assert.IsFalse(_expectationQueue.IsVerified);
		}

		[TestMethod]
		public void ActualAffectsIsVerifiedTest()
		{
			_expectationQueue.Expected = 1;
			_expectationQueue.Actual = 1;
			_expectationQueue.Verify();
			Assert.IsTrue(_expectationQueue.IsVerified);
			_expectationQueue.Actual = 2;
			Assert.IsFalse(_expectationQueue.IsVerified);
		}

		[TestMethod]
		public void ExpectedAffectsIsVerifiedTest()
		{
			_expectationQueue.Expected = 1;
			_expectationQueue.Actual = 1;
			_expectationQueue.Verify();
			Assert.IsTrue(_expectationQueue.IsVerified);
			_expectationQueue.Expected = 2;
			Assert.IsFalse(_expectationQueue.IsVerified);
		}

		[TestMethod]
		public void ExpectNothingTest()
		{
			_expectationQueue.Actual = 1;
			_expectationQueue.Expected = 1;
			_expectationQueue.Verify();
			Assert.IsTrue(_expectationQueue.IsVerified);
			_expectationQueue.ExpectNothing();
			Assert.IsTrue(_expectationQueue.HasExpectations);
			Assert.IsFalse(_expectationQueue.IsVerified);
		}

		[TestMethod]
		public void ImmediateVerificationWillSucceedTest()
		{
			_expectationQueue.VerifyImmediate = true;
			_expectationQueue.Expected = 1;
			_expectationQueue.Actual = 1;
			Assert.IsTrue(_expectationQueue.IsVerified);
		}

		[TestMethod]
		public void ImmediateVerificationWillSucceed2Test()
		{
			_expectationQueue.VerifyImmediate = true;
			_expectationQueue.Expected = 1;
			_expectationQueue.Expected = 2;
			_expectationQueue.Expected = 3;
			_expectationQueue.Actual = 1;
			_expectationQueue.Actual = 2;
		}

		[TestMethod]
		[Ignore]
		public void ImmediateVerificationWillFailTest()
		{
			_expectationQueue.VerifyImmediate = true;
			_expectationQueue.Expected = 1;
			_expectationQueue.Expected = 2;
			_expectationQueue.Actual = 1;
			_expectationQueue.Actual = 2;
			_expectationQueue.Actual = 3;
		}

		[TestMethod]
		[Ignore]
		public void ImmediateVerificationWillFailTest2()
		{
			_expectationQueue.VerifyImmediate = true;
			_expectationQueue.Expected = 1;
			_expectationQueue.Actual = 1;
			_expectationQueue.Expected = 2;
			_expectationQueue.Actual = 2;
			_expectationQueue.Actual = 3;
		}

		[TestMethod]
		[Ignore]
		public void ImmediateVerificationWillFailTest3()
		{
			_expectationQueue.VerifyImmediate = true;
			_expectationQueue.Expected = 1;
			_expectationQueue.Actual = 2;
		}

		[TestMethod]
		public void VerifyWillSucceedTest()
		{
			_expectationQueue.Expected = 1;
			_expectationQueue.Actual = 1;
			_expectationQueue.Verify();
		}

		[TestMethod]
		public void VerifyWillSucceedTest2()
		{
			_expectationQueue.Expected = 1;
			_expectationQueue.Expected = 2;
			_expectationQueue.Expected = 3;

			_expectationQueue.Actual = 1;
			_expectationQueue.Actual = 2;
			_expectationQueue.Actual = 3;

			_expectationQueue.Verify();
		}

		[TestMethod]
		[Ignore]
		public void VerifyWillFailTest()
		{
			_expectationQueue.Expected = 1;
			_expectationQueue.Expected = 3;

			_expectationQueue.Actual = 1;
			_expectationQueue.Actual = 4;

			_expectationQueue.Verify();
		}
	}
}
