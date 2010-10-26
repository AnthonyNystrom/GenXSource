/* -----------------------------------------------
 * EventModelTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetibase.NuGenMock.Tests
{
	[TestClass]
	public class ExpectationCounterTests
	{
		private ExpectationCounter _expectationCounter;

		[TestInitialize]
		public void SetUp()
		{
			_expectationCounter = new ExpectationCounter("counter");	
		}

		[TestMethod]
		public void ClearActualTest()
		{
			Assert.IsFalse(_expectationCounter.IsVerified);
			_expectationCounter.ExpectNothing();
			_expectationCounter.Inc();
			_expectationCounter.ClearActual();
			_expectationCounter.Verify();
			Assert.IsTrue(_expectationCounter.IsVerified);
			_expectationCounter.ClearActual();
			Assert.IsFalse(_expectationCounter.IsVerified);
		}

		[TestMethod]
		public void ClearExpectedTest()
		{
			_expectationCounter.Expected = 0;
			Assert.IsTrue(_expectationCounter.HasExpectations);
			_expectationCounter.Verify();
			Assert.IsTrue(_expectationCounter.IsVerified);
			_expectationCounter.ClearExpected();
			Assert.IsFalse(_expectationCounter.HasExpectations);
			Assert.IsFalse(_expectationCounter.IsVerified);
		}

		[TestMethod]
		public void ExpectedAffectsIsVerifiedTest()
		{
			_expectationCounter.Expected = 1;
			_expectationCounter.Inc();
			_expectationCounter.Verify();
			Assert.IsTrue(_expectationCounter.IsVerified);
			_expectationCounter.Expected = 2;
			Assert.IsFalse(_expectationCounter.IsVerified);
		}

		[TestMethod]
		public void ExpectNothingTest()
		{
			_expectationCounter.Expected = 0;
			_expectationCounter.Verify();
			Assert.IsTrue(_expectationCounter.IsVerified);
			_expectationCounter.ExpectNothing();
			Assert.IsFalse(_expectationCounter.IsVerified);
		}

		[TestMethod]
		public void ImmediateVerification()
		{
			_expectationCounter.Expected = 2;
			_expectationCounter.VerifyImmediate = true;
			_expectationCounter.Inc();
			Assert.IsTrue(_expectationCounter.IsVerified);
		}

		[TestMethod]
		public void IsVerifiedTest()
		{
			Assert.IsFalse(_expectationCounter.IsVerified);
			_expectationCounter.Expected = 0;
			_expectationCounter.Verify();
			Assert.IsTrue(_expectationCounter.IsVerified);
		}

		[TestMethod]
		[Ignore]
		public void IncVerifyWillFailTest()
		{
			_expectationCounter.Expected = 0;
			_expectationCounter.VerifyImmediate = true;
			_expectationCounter.Inc();
		}

		[TestMethod]
		public void VerifyWillSucceedTest()
		{
			_expectationCounter.Expected = 2;
			_expectationCounter.Inc();
			_expectationCounter.Inc();
			_expectationCounter.Verify();
		}

		[TestMethod]
		[Ignore]
		public void VerifyWillFailTest()
		{
			_expectationCounter.Expected = 2;
			_expectationCounter.Verify();
		}
	}
}
