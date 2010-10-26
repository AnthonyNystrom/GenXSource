/* -----------------------------------------------
 * NuGenCalculatorEngineTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.CalculatorInternals;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.Tests
{
	[TestFixture]
	public partial class NuGenCalculatorEngineTests
	{
		private Engine _engine;
		private EventSink _eventSink;

		[SetUp]
		public void SetUp()
		{
			_engine = new Engine();
			_eventSink = new EventSink(_engine);
		}

		[TearDown]
		public void TearDown()
		{
			_eventSink.Verify();
		}

		[Test]
		public void BackspaceTest()
		{
			_engine.ProcessAction(ActionManager.D8);
			_engine.ProcessAction(ActionManager.D8);
			_engine.ProcessAction(ActionManager.D8);
			Assert.AreEqual(888, _engine.Value);
			_engine.ProcessAction(ActionManager.Backspace);
			Assert.AreEqual(88, _engine.Value);
			_engine.ProcessAction(ActionManager.Backspace);
			Assert.AreEqual(8, _engine.Value);
			_engine.ProcessAction(ActionManager.Backspace);
			Assert.AreEqual(0, _engine.Value);
		}

		[Test]
		public void BackspaceDotTest()
		{
			_eventSink.ExpectedValueCount = 13;

			_engine.ProcessAction(ActionManager.D8);
			_engine.ProcessAction(ActionManager.Dot);
			_engine.ProcessAction(ActionManager.D2);
			Assert.AreEqual(8.2, _engine.Value);
			_engine.ProcessAction(ActionManager.Backspace);
			Assert.AreEqual(8, _engine.Value);
			_engine.ProcessAction(ActionManager.D2);
			Assert.AreEqual(8.2, _engine.Value);
			_engine.ProcessAction(ActionManager.D3);
			Assert.AreEqual(8.23, _engine.Value);
			_engine.ProcessAction(ActionManager.Backspace);
			_engine.ProcessAction(ActionManager.Backspace);
			_engine.ProcessAction(ActionManager.Backspace);
			Assert.AreEqual(8, _engine.Value);
			_engine.ProcessAction(ActionManager.Backspace);
			Assert.AreEqual(0, _engine.Value);
			_engine.ProcessAction(ActionManager.Backspace);
			Assert.AreEqual(0, _engine.Value);
		}

		[Test]
		public void BackspaceNegativeTest()
		{
			_engine.ProcessAction(ActionManager.D8);
			_engine.ProcessAction(ActionManager.Sign);
			_engine.ProcessAction(ActionManager.Backspace);
			Assert.AreEqual(0, _engine.Value);
		}

		[Test]
		public void BackspaceDotNegativeTest()
		{
			_engine.ProcessAction(ActionManager.D8);
			_engine.ProcessAction(ActionManager.Dot);
			_engine.ProcessAction(ActionManager.D2);
			_engine.ProcessAction(ActionManager.D3);
			_engine.ProcessAction(ActionManager.Sign);
			Assert.AreEqual(-8.23, _engine.Value);
			_engine.ProcessAction(ActionManager.Backspace);
			Assert.AreEqual(-8.2, _engine.Value);
			_engine.ProcessAction(ActionManager.Backspace);
			Assert.AreEqual(-8, _engine.Value);
			_engine.ProcessAction(ActionManager.Backspace);
			Assert.AreEqual(-8, _engine.Value);
			_engine.ProcessAction(ActionManager.Backspace);
			Assert.AreEqual(0, _engine.Value);
			_engine.ProcessAction(ActionManager.D8);
			Assert.AreEqual(8, _engine.Value);
		}

		[Test]
		public void CTest()
		{
			_engine.ProcessAction(ActionManager.D1);
			_engine.ProcessAction(ActionManager.Plus);
			_engine.ProcessAction(ActionManager.D3);
			_engine.ProcessAction(ActionManager.C);
			_engine.ProcessAction(ActionManager.D5);
			_engine.ProcessAction(ActionManager.Plus);
			_engine.ProcessAction(ActionManager.D2);
			_engine.ProcessAction(ActionManager.Evaluate);
			Assert.AreEqual(7, _engine.Value);
		}

		[Test]
		public void CETest()
		{
			_engine.ProcessAction(ActionManager.D5);
			_engine.ProcessAction(ActionManager.Multiply);
			_engine.ProcessAction(ActionManager.D1);
			_engine.ProcessAction(ActionManager.CE);
			Assert.AreEqual(0, _engine.Value);
			_engine.ProcessAction(ActionManager.D2);
			_engine.ProcessAction(ActionManager.Evaluate);
			Assert.AreEqual(10, _engine.Value);
		}

		[Test]
		public void D0Test()
		{
			_engine.ProcessAction(ActionManager.D0);
			Assert.AreEqual(0, _engine.Value);
			_engine.ProcessAction(ActionManager.D0);
			Assert.AreEqual(0, _engine.Value);
		}

		[Test]
		public void D0D1ComboTest()
		{
			_engine.ProcessAction(ActionManager.D1);
			_engine.ProcessAction(ActionManager.D0);
			Assert.AreEqual(10, _engine.Value);
			_engine.ProcessAction(ActionManager.D0);
			Assert.AreEqual(100, _engine.Value);
		}

		[Test]
		public void D1Test()
		{
			_engine.ProcessAction(ActionManager.D1);
			Assert.AreEqual(1, _engine.Value);
			_engine.ProcessAction(ActionManager.D1);
			Assert.AreEqual(11, _engine.Value);
		}

		[Test]
		public void D2Test()
		{
			_engine.ProcessAction(ActionManager.D2);
			Assert.AreEqual(2, _engine.Value);
			_engine.ProcessAction(ActionManager.D2);
			Assert.AreEqual(22, _engine.Value);
		}

		[Test]
		public void D3Test()
		{
			_engine.ProcessAction(ActionManager.D3);
			Assert.AreEqual(3, _engine.Value);
			_engine.ProcessAction(ActionManager.D3);
			Assert.AreEqual(33, _engine.Value);
		}

		[Test]
		public void D4Test()
		{
			_engine.ProcessAction(ActionManager.D4);
			Assert.AreEqual(4, _engine.Value);
			_engine.ProcessAction(ActionManager.D4);
			Assert.AreEqual(44, _engine.Value);
		}

		[Test]
		public void D5Test()
		{
			_engine.ProcessAction(ActionManager.D5);
			Assert.AreEqual(5, _engine.Value);
			_engine.ProcessAction(ActionManager.D5);
			Assert.AreEqual(55, _engine.Value);
		}

		[Test]
		public void D6Test()
		{
			_engine.ProcessAction(ActionManager.D6);
			Assert.AreEqual(6, _engine.Value);
			_engine.ProcessAction(ActionManager.D6);
			Assert.AreEqual(66, _engine.Value);
		}

		[Test]
		public void D7Test()
		{
			_engine.ProcessAction(ActionManager.D7);
			Assert.AreEqual(7, _engine.Value);
			_engine.ProcessAction(ActionManager.D7);
			Assert.AreEqual(77, _engine.Value);
		}

		[Test]
		public void D8Test()
		{
			_engine.ProcessAction(ActionManager.D8);
			Assert.AreEqual(8, _engine.Value);
			_engine.ProcessAction(ActionManager.D8);
			Assert.AreEqual(88, _engine.Value);
		}

		[Test]
		public void D9Test()
		{
			_engine.ProcessAction(ActionManager.D9);
			Assert.AreEqual(9, _engine.Value);
			_engine.ProcessAction(ActionManager.D9);
			Assert.AreEqual(99, _engine.Value);
		}

		[Test]
		public void DivXTest()
		{
			_engine.ProcessAction(ActionManager.D2);
			_engine.ProcessAction(ActionManager.DivX);
			Assert.AreEqual(0.5, _engine.Value);
		}

		[Test]
		public void DivXPlusEvaluateTest()
		{
			_engine.ProcessAction(ActionManager.Dot);
			_engine.ProcessAction(ActionManager.D5);
			Assert.AreEqual(0.5, _engine.Value);
			_engine.ProcessAction(ActionManager.Plus);
			_engine.ProcessAction(ActionManager.D2);
			_engine.ProcessAction(ActionManager.DivX);
			Assert.AreEqual(0.5, _engine.Value);
			_engine.ProcessAction(ActionManager.Evaluate);
			Assert.AreEqual(1, _engine.Value);
		}

		[Test]
		public void DotMultiplyTest()
		{
			_engine.ProcessAction(ActionManager.D1);
			_engine.ProcessAction(ActionManager.Dot);
			_engine.ProcessAction(ActionManager.D5);
			Assert.AreEqual(1.5, _engine.Value);
			_engine.ProcessAction(ActionManager.Multiply);
			_engine.ProcessAction(ActionManager.D1);
			_engine.ProcessAction(ActionManager.Dot);
			_engine.ProcessAction(ActionManager.D5);
			Assert.AreEqual(1.5, _engine.Value);
			_engine.ProcessAction(ActionManager.Evaluate);
			Assert.AreEqual(2.25, _engine.Value);
		}

		[Test]
		public void DivideEvaluateTest()
		{
			_engine.ProcessAction(ActionManager.D2);
			_engine.ProcessAction(ActionManager.D4);
			_engine.ProcessAction(ActionManager.Divide);
			_engine.ProcessAction(ActionManager.D2);
			_engine.ProcessAction(ActionManager.Evaluate);
			Assert.AreEqual(12, _engine.Value);
		}

		[Test]
		[ExpectedException(typeof(DivideByZeroException))]
		public void DivideByZeroExceptionTest()
		{
			_engine.ProcessAction(ActionManager.D1);
			_engine.ProcessAction(ActionManager.Divide);
			_engine.ProcessAction(ActionManager.D0);
			_engine.ProcessAction(ActionManager.Evaluate);
		}

		[Test]
		public void EvaluateMultiplySequentialTest()
		{
			_engine.ProcessAction(ActionManager.D3);
			_engine.ProcessAction(ActionManager.Multiply);
			_engine.ProcessAction(ActionManager.D4);
			_engine.ProcessAction(ActionManager.Evaluate);
			Assert.AreEqual(12, _engine.Value);
			_engine.ProcessAction(ActionManager.Evaluate);
			Assert.AreEqual(48, _engine.Value);
		}

		[Test]
		public void EvaluatePlusSequentialTest()
		{
			_engine.ProcessAction(ActionManager.D1);
			_engine.ProcessAction(ActionManager.Plus);
			_engine.ProcessAction(ActionManager.D3);
			_engine.ProcessAction(ActionManager.Evaluate);
			Assert.AreEqual(4, _engine.Value);
			_engine.ProcessAction(ActionManager.Evaluate);
			Assert.AreEqual(7, _engine.Value);
			_engine.ProcessAction(ActionManager.Evaluate);
			Assert.AreEqual(10, _engine.Value);
		}

		[Test]
		public void EvaluateSqrtSequentialTest()
		{
			_engine.ProcessAction(ActionManager.D1);
			_engine.ProcessAction(ActionManager.Plus);
			_engine.ProcessAction(ActionManager.D2);
			_engine.ProcessAction(ActionManager.Evaluate);
			Assert.AreEqual(3, _engine.Value);
			_engine.ProcessAction(ActionManager.D5);
			_engine.ProcessAction(ActionManager.Multiply);
			_engine.ProcessAction(ActionManager.D5);
			_engine.ProcessAction(ActionManager.Evaluate);
			Assert.AreEqual(25, _engine.Value);
			_engine.ProcessAction(ActionManager.Sqrt);
			Assert.AreEqual(5, _engine.Value);
		}

		[Test]
		public void MinusEvaluateTest()
		{
			_engine.ProcessAction(ActionManager.D1);
			_engine.ProcessAction(ActionManager.Minus);
			_engine.ProcessAction(ActionManager.D2);
			_engine.ProcessAction(ActionManager.Evaluate);
			Assert.AreEqual(-1, _engine.Value);
		}

		[Test]
		public void MultiplyEvaluateTest()
		{
			_engine.ProcessAction(ActionManager.D2);
			_engine.ProcessAction(ActionManager.D5);
			_engine.ProcessAction(ActionManager.Multiply);
			_engine.ProcessAction(ActionManager.D2);
			_engine.ProcessAction(ActionManager.D5);
			_engine.ProcessAction(ActionManager.Evaluate);
			Assert.AreEqual(625, _engine.Value);
		}

		[Test]
		public void PercentTest()
		{
			_engine.ProcessAction(ActionManager.D2);
			_engine.ProcessAction(ActionManager.D5);
			_engine.ProcessAction(ActionManager.Multiply);
			_engine.ProcessAction(ActionManager.D2);
			_engine.ProcessAction(ActionManager.D0);
			_engine.ProcessAction(ActionManager.D0);
			_engine.ProcessAction(ActionManager.Percent);
			Assert.AreEqual(50, _engine.Value);
		}

		[Test]
		public void PlusEvaluateTest()
		{
			_engine.ProcessAction(ActionManager.D1);
			Assert.AreEqual(1, _engine.Value);
			_engine.ProcessAction(ActionManager.Plus);
			Assert.AreEqual(1, _engine.Value);
			_engine.ProcessAction(ActionManager.D2);
			Assert.AreEqual(2, _engine.Value);
			_engine.ProcessAction(ActionManager.Evaluate);
			Assert.AreEqual(3, _engine.Value);
		}

		[Test]
		public void PlusSequentialTest()
		{
			_engine.ProcessAction(ActionManager.D1);
			_engine.ProcessAction(ActionManager.Plus);
			_engine.ProcessAction(ActionManager.D2);
			_engine.ProcessAction(ActionManager.Plus);
			Assert.AreEqual(3, _engine.Value);
			_engine.ProcessAction(ActionManager.D3);
			_engine.ProcessAction(ActionManager.Plus);
			Assert.AreEqual(6, _engine.Value);
		}

		[Test]
		public void SignTest()
		{
			_engine.ProcessAction(ActionManager.D1);
			_engine.ProcessAction(ActionManager.Sign);
			Assert.AreEqual(-1, _engine.Value);
		}

		[Test]
		public void SignPlusEvaluateTest()
		{
			_engine.ProcessAction(ActionManager.D1);
			_engine.ProcessAction(ActionManager.Dot);
			_engine.ProcessAction(ActionManager.D7);
			_engine.ProcessAction(ActionManager.D6);
			Assert.AreEqual(1.76, _engine.Value);
			_engine.ProcessAction(ActionManager.Plus);
			_engine.ProcessAction(ActionManager.D2);
			_engine.ProcessAction(ActionManager.D5);
			_engine.ProcessAction(ActionManager.Dot);
			Assert.AreEqual(25, _engine.Value);
			_engine.ProcessAction(ActionManager.Sign);
			Assert.AreEqual(-25, _engine.Value);
			_engine.ProcessAction(ActionManager.D7);
			_engine.ProcessAction(ActionManager.D6);
			Assert.AreEqual(-25.76, _engine.Value);
			_engine.ProcessAction(ActionManager.Evaluate);
			Assert.AreEqual(-24, _engine.Value);
		}

		[Test]
		public void SqrtTest()
		{
			_engine.ProcessAction(ActionManager.D2);
			_engine.ProcessAction(ActionManager.D5);
			_engine.ProcessAction(ActionManager.Sqrt);
			Assert.AreEqual(5, _engine.Value);
		}

		[Test]
		public void SqrtDigitTest()
		{
			_engine.ProcessAction(ActionManager.D2);
			_engine.ProcessAction(ActionManager.D5);
			_engine.ProcessAction(ActionManager.Sqrt);
			Assert.AreEqual(5, _engine.Value);
			_engine.ProcessAction(ActionManager.D3);
			_engine.ProcessAction(ActionManager.D6);
			_engine.ProcessAction(ActionManager.Sqrt);
			Assert.AreEqual(6, _engine.Value);
		}

		[Test]
		public void SqrtPlusEvaluateTest()
		{
			_engine.ProcessAction(ActionManager.D2);
			_engine.ProcessAction(ActionManager.Dot);
			_engine.ProcessAction(ActionManager.D2);
			_engine.ProcessAction(ActionManager.D5);
			_engine.ProcessAction(ActionManager.Sqrt);
			_engine.ProcessAction(ActionManager.Plus);
			_engine.ProcessAction(ActionManager.D1);
			_engine.ProcessAction(ActionManager.Dot);
			_engine.ProcessAction(ActionManager.D5);
			_engine.ProcessAction(ActionManager.Evaluate);
			Assert.AreEqual(3, _engine.Value);
		}

		[Test]
		public void ValueTest()
		{
			_engine.Value = 25;
			_engine.ProcessAction(ActionManager.Plus);
			_engine.ProcessAction(ActionManager.D2);
			_engine.ProcessAction(ActionManager.D5);
			_engine.ProcessAction(ActionManager.Evaluate);
			Assert.AreEqual(50, _engine.Value);
		}
	}
}
