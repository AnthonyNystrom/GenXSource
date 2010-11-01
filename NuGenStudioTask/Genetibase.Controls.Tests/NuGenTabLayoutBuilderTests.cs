/* -----------------------------------------------
 * NuGenTabLayoutBuilderTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Genetibase.Controls.Tests
{
	[TestFixture]
	public class NuGenTabLayoutBuilderTests
	{
		private List<NuGenTabButton> _tabButtons = null;
		private NuGenTabLayoutBuilder _layoutBuilder = null;
		private Rectangle _tabStripBounds = Rectangle.Empty;

		[SetUp]
		public void SetUp()
		{
			_tabButtons = new List<NuGenTabButton>();

			_tabButtons.Add(new NuGenTabButton());
			_tabButtons.Add(new NuGenTabButton());
			_tabButtons.Add(new NuGenTabButton());

			_tabButtons[1].Selected = true;

			foreach (NuGenTabButton tabButton in _tabButtons)
			{
				tabButton.Width = 50;
			}

			_layoutBuilder = new NuGenTabLayoutBuilder(_tabButtons, new Size(100, 25));
			_tabStripBounds = new Rectangle(0, 0, 100, 29);
		}

		[Test]
		public void SetTabButtonBoundsTest()
		{
			_layoutBuilder.RebuildLayout(_tabStripBounds);

			Assert.AreEqual(_tabButtons[2].Right, _tabStripBounds.Right, 5);
			Assert.AreEqual(_tabButtons[0].Top, _tabStripBounds.Top);
			Assert.AreEqual(_tabButtons[0].Left, _tabStripBounds.Left);
		}
	}
}
