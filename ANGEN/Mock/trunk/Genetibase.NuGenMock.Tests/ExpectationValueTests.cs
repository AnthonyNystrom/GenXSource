/* -----------------------------------------------
 * ExpectationValueTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetibase.NuGenMock.Tests
{
	[TestClass]
	public class ExpectationValueTests
	{
		private ExpectationValue<Type> _expectationValue;

		[TestInitialize]
		public void SetUp()
		{
			_expectationValue = new ExpectationValue<Type>("expectationValue");
		}

		[TestMethod]
		public void ClearActualTest()
		{
			Assert.IsFalse(_expectationValue.IsVerified);
			_expectationValue.ExpectNothing();
			_expectationValue.Actual = typeof(Object);
			_expectationValue.ClearActual();
			_expectationValue.Verify();
			Assert.IsTrue(_expectationValue.IsVerified);
			_expectationValue.ClearActual();
			Assert.IsFalse(_expectationValue.IsVerified);
		}

		[TestMethod]
		public void ClearExpectedTest()
		{
			_expectationValue.Actual = typeof(Object);
			_expectationValue.Expected = typeof(Object);
			Assert.IsTrue(_expectationValue.HasExpectations);
			_expectationValue.Verify();
			Assert.IsTrue(_expectationValue.IsVerified);
			_expectationValue.ClearExpected();
			Assert.IsFalse(_expectationValue.HasExpectations);
			Assert.IsFalse(_expectationValue.IsVerified);
		}

		[TestMethod]
		public void ActualAffectsIsVerifiedTest()
		{
			_expectationValue.Expected = typeof(Object);
			_expectationValue.Actual = typeof(Object);
			_expectationValue.Verify();
			Assert.IsTrue(_expectationValue.IsVerified);
			_expectationValue.Actual = typeof(String);
			Assert.IsFalse(_expectationValue.IsVerified);
		}

		[TestMethod]
		public void ExpectedAffectsIsVerifiedTest()
		{
			_expectationValue.Expected = typeof(Object);
			_expectationValue.Actual = typeof(Object);
			_expectationValue.Verify();
			Assert.IsTrue(_expectationValue.IsVerified);
			_expectationValue.Expected = typeof(String);
			Assert.IsFalse(_expectationValue.IsVerified);
		}

		[TestMethod]
		public void ExpectNothingTest()
		{
			_expectationValue.Actual = typeof(Object);
			_expectationValue.Expected = typeof(Object);
			_expectationValue.Verify();
			Assert.IsTrue(_expectationValue.IsVerified);
			_expectationValue.ExpectNothing();
			Assert.IsTrue(_expectationValue.HasExpectations);
			Assert.IsFalse(_expectationValue.IsVerified);
		}

		[TestMethod]
		public void ImmediateVerification()
		{
			_expectationValue.VerifyImmediate = true;
			_expectationValue.Expected = typeof(Object);
			_expectationValue.Actual = typeof(Object);
			Assert.IsTrue(_expectationValue.IsVerified);
		}

		[TestMethod]
		public void VerifyWillSucceedTest()
		{
			_expectationValue.Expected = typeof(Object);
			_expectationValue.Actual = typeof(Object);
			_expectationValue.Verify();
		}

		[TestMethod]
		[Ignore]
		public void VerifyWillFailTest()
		{
			_expectationValue.Expected = typeof(Object);
			_expectationValue.Actual = typeof(String);
			_expectationValue.Verify();
		}
	}
}
