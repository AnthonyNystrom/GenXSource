/* -----------------------------------------------
 * NuGenSmoothProgressBarTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using DotNetMock.Dynamic;

using Genetibase.Shared;
using Genetibase.Shared.Controls;

using NUnit.Framework;

using System;
using System.Collections.Generic;

namespace Genetibase.Shared.Controls.Tests
{
	[TestFixture]
	public partial class NuGenProgressBarTests
	{
		private NuGenProgressBar _progressBar = null;

		[SetUp]
		public void SetUp()
		{
			_progressBar = new NuGenProgressBar(new ProgressBarServiceProvider());
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void MinimumArgumentExceptionTest()
		{
			_progressBar.Minimum = -10;
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void MaximumArgumentExceptionTest()
		{
			_progressBar.Maximum = -10;
		}

		[Test]
		public void IncrementTest()
		{
			_progressBar.Value = _progressBar.Minimum;
			_progressBar.Increment(20);

			Assert.AreEqual(_progressBar.Minimum + 20, _progressBar.Value);
		}

		[Test]
		public void IncrementMaximumTest()
		{
			_progressBar.Minimum = 0;
			_progressBar.Maximum = 100;

			for (int i = 0; i < 10; i++)
			{
				_progressBar.Increment(20);
			}

			Assert.AreEqual(_progressBar.Maximum, _progressBar.Value);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void IncrementInvalidOperationTest()
		{
			_progressBar.Style = NuGenProgressBarStyle.Marquee;
			_progressBar.Increment(20);
		}

		[Test]
		public void PerformStepTest()
		{
			_progressBar.Value = _progressBar.Minimum;
			_progressBar.PerformStep();
			Assert.AreEqual(_progressBar.Minimum + _progressBar.Step, _progressBar.Value);
		}

		[Test]
		public void PerformStepMaximumTest()
		{
			_progressBar.Minimum = 0;
			_progressBar.Maximum = 100;
			_progressBar.Step = 10;

			for (int i = 0; i < 20; i++)
			{
				_progressBar.PerformStep();
			}

			Assert.AreEqual(_progressBar.Maximum, _progressBar.Value);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void PerformStepInvalidOperationExceptionTest()
		{
			_progressBar.Style = NuGenProgressBarStyle.Marquee;
			_progressBar.PerformStep();
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void ValueArgumentExceptionTest()
		{
			_progressBar.Maximum = 100;
			_progressBar.Value = 110;
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void ValueArgumentException2Test()
		{
			_progressBar.Minimum = 10;
			_progressBar.Value = 5;
		}

		[Test]
		public void ValueTest()
		{
			_progressBar.Value = 50;
			Assert.AreEqual(50, _progressBar.Value);
		}

		[Test]
		public void Value2Test()
		{
			_progressBar.Value = 30;
			_progressBar.Minimum = 40;

			Assert.AreEqual(40, _progressBar.Value);
			Assert.AreEqual(40, _progressBar.Minimum);

			_progressBar.Minimum = 20;
			_progressBar.Value = 20;

			Assert.AreEqual(20, _progressBar.Value);
			Assert.AreEqual(20, _progressBar.Minimum);

			_progressBar.Maximum = 10;

			Assert.AreEqual(10, _progressBar.Minimum);
			Assert.AreEqual(10, _progressBar.Maximum);
			Assert.AreEqual(10, _progressBar.Value);
		}
	}
}
