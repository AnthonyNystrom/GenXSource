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
		private StubSpin _spin;
		private EventSink _eventSink;
		private string _item, _item2, _item3, _item4, _item5;

		[SetUp]
		public void SetUp()
		{
			_spin = new StubSpin();
			_eventSink = new EventSink(_spin);

			_item = "item";
			_item2 = "item2";
			_item3 = "item3";
			_item4 = "item4";
			_item5 = "item5";

			_spin.Items.AddRange(new string[] { _item, _item2, _item3, _item4, _item5 });
		}

		[TearDown]
		public void TearDown()
		{
			_eventSink.Verify();
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
			string currentText = _spin.Text;
			_spin.OnUpButtonClick();
			Assert.AreEqual(currentText, _spin.Text);

			_spin.OnDownButtonClick();
			Assert.AreEqual(_item, _spin.Text);

			_spin.OnDownButtonClick();
			Assert.AreEqual(_item2, _spin.Text);

			_spin.OnDownButtonClick();
			Assert.AreEqual(_item3, _spin.Text);

			_spin.OnUpButtonClick();
			Assert.AreEqual(_item2, _spin.Text);

			_spin.OnUpButtonClick();
			Assert.AreEqual(_item, _spin.Text);

			for (int i = 0; i < 10; i++)
			{
				_spin.OnDownButtonClick();
			}

			Assert.AreEqual(_item5, _spin.Text);

			for (int i = 0; i < 10; i++)
			{
				_spin.OnUpButtonClick();
			}

			Assert.AreEqual(_item, _spin.Text);
		}

		[Test]
		public void SelectedIndexSetTest()
		{
			_eventSink.ExpectedSelectedItemChangedCount = 1;

			_spin.SelectedIndex = 1;
			Assert.AreEqual("item2", _spin.SelectedItem);
		}

		[Test]
		public void SelectedIndexGetTest()
		{
			_eventSink.ExpectedSelectedItemChangedCount = 2;

			_spin.SelectedItem = "item3";
			Assert.AreEqual(2, _spin.SelectedIndex);

			_spin.Text = "some_item";
			Assert.AreEqual(-1, _spin.SelectedIndex);
			Assert.IsNull(_spin.SelectedItem);
		}

		[Test]
		public void SelectedIndexArgumentOutOfRangeExceptionTest()
		{
			_eventSink.ExpectedSelectedItemChangedCount = 0;

			try
			{
				_spin.SelectedIndex = -2;
				Assert.Fail();
			}
			catch (ArgumentOutOfRangeException)
			{
			}

			try
			{
				_spin.SelectedIndex = 100;
				Assert.Fail();
			}
			catch (ArgumentOutOfRangeException)
			{
			}
		}

		[Test]
		public void SelectedItemGetTest()
		{
			_eventSink.ExpectedSelectedItemChangedCount = 2;

			_spin.Text = "item";
			Assert.AreEqual("item", _spin.SelectedItem);

			_spin.Text = "some_item";
			Assert.IsNull(_spin.SelectedItem);
		}

		[Test]
		public void SelectedItemSetTest()
		{
			_eventSink.ExpectedSelectedItemChangedCount = 1;

			_spin.SelectedItem = "item2";
			Assert.AreEqual("item2", _spin.Text);

			_spin.SelectedItem = "some_item";
			Assert.AreEqual("item2", _spin.Text);
		}
	}
}
