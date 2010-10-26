/* -----------------------------------------------
 * ExpectationArrayTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetibase.NuGenMock.Tests
{
	[TestClass]
	public class ExpectationArrayTests
	{
		private ExpectationArray<Int32> _expectationArray;

		[TestInitialize]
		public void SetUp()
		{
			_expectationArray = new ExpectationArray<Int32>("expectationArray");
		}

		[TestMethod]
		public void ClearActualTest()
		{
			Assert.IsFalse(_expectationArray.IsVerified);
			_expectationArray.ExpectNothing();
			_expectationArray.Actual = new Int32[] { 1, 2, 3 };
			_expectationArray.ClearActual();
			_expectationArray.Verify();
			Assert.IsTrue(_expectationArray.IsVerified);
			_expectationArray.ClearActual();
			Assert.IsFalse(_expectationArray.IsVerified);
		}

		[TestMethod]
		public void ClearExpectedTest()
		{
			_expectationArray.Actual = new Int32[] { 1, 2, 3 };
			_expectationArray.Expected = new Int32[] { 1, 2, 3 };
			Assert.IsTrue(_expectationArray.HasExpectations);
			_expectationArray.Verify();
			Assert.IsTrue(_expectationArray.IsVerified);
			_expectationArray.ClearExpected();
			Assert.IsFalse(_expectationArray.HasExpectations);
			Assert.IsFalse(_expectationArray.IsVerified);
		}

		[TestMethod]
		public void ActualAffectsIsVerifiedTest()
		{
			_expectationArray.Expected = new Int32[] { 1, 2, 3 };
			_expectationArray.Actual = new Int32[] { 1, 2, 3 };
			_expectationArray.Verify();
			Assert.IsTrue(_expectationArray.IsVerified);
			_expectationArray.Actual = new Int32[] { 3, 4, 5 };
			Assert.IsFalse(_expectationArray.IsVerified);
		}

		[TestMethod]
		public void ExpectedAffectsIsVerifiedTest()
		{
			_expectationArray.Expected = new Int32[] { 1, 2, 3 };
			_expectationArray.Actual = new Int32[] { 1, 2, 3 };
			_expectationArray.Verify();
			Assert.IsTrue(_expectationArray.IsVerified);
			_expectationArray.Expected = new Int32[] { 3, 4, 5 };
			Assert.IsFalse(_expectationArray.IsVerified);
		}

		[TestMethod]
		public void ExpectNothingTest()
		{
			_expectationArray.Actual = new Int32[] { 1, 2, 3 };
			_expectationArray.Expected = new Int32[] { 1, 2, 3 };
			_expectationArray.Verify();
			Assert.IsTrue(_expectationArray.IsVerified);
			_expectationArray.ExpectNothing();
			Assert.IsFalse(_expectationArray.IsVerified);
		}

		[TestMethod]
		public void ImmediateVerification()
		{
			_expectationArray.VerifyImmediate = true;
			_expectationArray.Expected = new Int32[] { 1, 2, 3 };
			_expectationArray.Actual = new Int32[] { 1, 2, 3 };
			Assert.IsTrue(_expectationArray.IsVerified);
		}

		[TestMethod]
		public void VerifyWillSucceedTest()
		{
			_expectationArray.Actual = new Int32[] { 1, 2, 3 };
			_expectationArray.Expected = new Int32[] { 1, 2, 3 };
			_expectationArray.Verify();
			Assert.IsTrue(_expectationArray.IsVerified);
		}

		[TestMethod]
		[Ignore]
		public void VerifyWillFailTest()
		{
			_expectationArray.Actual = new Int32[] { 1, 2, 3 };
			_expectationArray.Expected = new Int32[] { 3, 4, 5 };
			_expectationArray.Verify();
		}

		[TestMethod]
		[ExpectedException(typeof(AssertionException))]
		public void VerifyAssertionExceptionTest()
		{
			_expectationArray.Actual = null;
			_expectationArray.Verify();
		}

		[TestMethod]
		[ExpectedException(typeof(AssertionException))]
		public void VerifyAssertionExceptionTest2()
		{
			_expectationArray.Expected = null;
			_expectationArray.Verify();
		}

		[TestMethod]
		public void VerifyNullArraysTest()
		{
			_expectationArray.Actual = null;
			_expectationArray.Expected = null;
			_expectationArray.Verify();
			Assert.IsTrue(_expectationArray.IsVerified);
		}
	}
}
