/* -----------------------------------------------
 * ExpectationListTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetibase.NuGenMock.Tests
{
	[TestClass]
	public class ExpectationListTests
	{
		private ExpectationList<Int32> _expectationList;

		[TestInitialize]
		public void SetUp()
		{
			_expectationList = new ExpectationList<Int32>("expectationList");
		}

		[TestMethod]
		public void ClearActualTest()
		{
			Assert.IsFalse(_expectationList.IsVerified);
			_expectationList.ExpectNothing();
			_expectationList.ActualList.Add(1);
			_expectationList.ActualList.Add(2);
			_expectationList.ClearActual();
			_expectationList.Verify();
			Assert.IsTrue(_expectationList.IsVerified);
			_expectationList.ClearActual();
			Assert.IsFalse(_expectationList.IsVerified);
		}

		[TestMethod]
		public void ClearExpectedTest()
		{
			_expectationList.AddActual(1);
			_expectationList.AddActual(2);

			_expectationList.AddExpected(1);
			_expectationList.AddExpected(2);

			_expectationList.HasExpectations = true;
			Assert.IsTrue(_expectationList.HasExpectations);

			_expectationList.Verify();
			Assert.IsTrue(_expectationList.IsVerified);

			_expectationList.ClearExpected();

			Assert.IsFalse(_expectationList.HasExpectations);
			Assert.IsFalse(_expectationList.IsVerified);
		}

		[TestMethod]
		public void AddActualAffectsIsVerifiedTest()
		{
			_expectationList.AddExpected(1);
			_expectationList.AddExpected(2);

			_expectationList.AddActual(1);
			_expectationList.AddActual(2);

			_expectationList.Verify();
			Assert.IsTrue(_expectationList.IsVerified);

			_expectationList.AddActual(3);
			Assert.IsFalse(_expectationList.IsVerified);
		}

		[TestMethod]
		public void AddExpectedAffectsIsVerifiedTest()
		{
			_expectationList.AddExpected(1);
			_expectationList.AddExpected(2);

			_expectationList.AddActual(1);
			_expectationList.AddActual(2);

			_expectationList.Verify();
			Assert.IsTrue(_expectationList.IsVerified);
			_expectationList.AddExpected(3);
			Assert.IsFalse(_expectationList.IsVerified);
		}

		[TestMethod]
		public void ExpectNothingTest()
		{
			_expectationList.AddExpected(1);
			_expectationList.AddActual(1);
			_expectationList.Verify();
			Assert.IsTrue(_expectationList.IsVerified);
			_expectationList.ExpectNothing();
			Assert.IsFalse(_expectationList.IsVerified);
		}

		[TestMethod]
		public void ImmediateVerificationWillSucceed()
		{
			_expectationList.VerifyImmediate = true;
			_expectationList.AddExpected(1);
			_expectationList.AddActual(1);
			Assert.IsTrue(_expectationList.IsVerified);
		}

		[TestMethod]
		[Ignore]
		public void ImmediateVerificationWillFail()
		{
			_expectationList.VerifyImmediate = true;
			_expectationList.AddExpected(2);
			_expectationList.AddActual(1);
		}

		[TestMethod]
		public void VerifyWillSucceedTest()
		{
			_expectationList.AddExpected(1);
			_expectationList.AddExpected(2);
			_expectationList.AddExpected(3);

			_expectationList.AddActual(1);
			_expectationList.AddActual(2);
			_expectationList.AddActual(3);

			_expectationList.Verify();
			Assert.IsTrue(_expectationList.IsVerified);
		}

		[TestMethod]
		[Ignore]
		public void VerifyWillFailTest()
		{
			_expectationList.AddExpected(1);
			_expectationList.AddExpected(2);
			_expectationList.AddExpected(3);

			_expectationList.AddActual(1);
			_expectationList.AddActual(2);
			_expectationList.AddActual(5);
			
			_expectationList.Verify();
		}
	}
}
