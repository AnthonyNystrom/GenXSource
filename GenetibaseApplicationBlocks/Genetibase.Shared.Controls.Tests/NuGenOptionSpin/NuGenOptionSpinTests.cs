/* -----------------------------------------------
 * NuGenOptionSpinTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Controls.Tests
{
	[TestFixture]
	public partial class NuGenSmoothOptionSpinTests
	{
		private StubSpin _spin = null;

		[SetUp]
		public void SetUp()
		{
			_spin = new StubSpin();
		}

		[Test]
		public void EditBoxTextTest()
		{
			_spin.Text = "Text";
			Assert.AreEqual("Text", _spin.EditBox.Text);
		}

		[Test]
		public void ItemsTest()
		{
			string item = "item";
			string item2 = "item2";
			string item3 = "item3";
			string item4 = "item4";
			string item5 = "item5";

			_spin.Items.AddRange(new string[] { item, item2, item3, item4, item5 });

			string currentText = _spin.Text;
			_spin.OnUpButtonClick();
			Assert.AreEqual(currentText, _spin.Text);

			_spin.OnDownButtonClick();
			Assert.AreEqual(item, _spin.Text);

			_spin.OnDownButtonClick();
			Assert.AreEqual(item2, _spin.Text);

			_spin.OnDownButtonClick();
			Assert.AreEqual(item3, _spin.Text);

			_spin.OnUpButtonClick();
			Assert.AreEqual(item2, _spin.Text);

			_spin.OnUpButtonClick();
			Assert.AreEqual(item, _spin.Text);

			for (int i = 0; i < 10; i++)
			{
				_spin.OnDownButtonClick();
			}

			Assert.AreEqual(item5, _spin.Text);

			for (int i = 0; i < 10; i++)
			{
				_spin.OnUpButtonClick();
			}

			Assert.AreEqual(item, _spin.Text);
		}
	}
}
