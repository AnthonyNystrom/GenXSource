/* -----------------------------------------------
 * NuGenTabbedMdiTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Genetibase.Controls.Tests
{
	[TestFixture]
	public class NuGenTabbedMdiTests
	{
		private NuGenTabbedMdi _tabbedMdi = null;
		private Control _ctrl = null;

		[SetUp]
		public void SetUp()
		{
			_ctrl = new Button();
			_tabbedMdi = new NuGenTabbedMdi();
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddTabPageArgumentNullExceptionOnContentTest()
		{
			_tabbedMdi.AddTabPage(null, "", null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddTabPageArgumentNullExceptionOnTextTest()
		{
			_tabbedMdi.AddTabPage(_ctrl, null, null);
		}

		[Test]
		public void TabPageContentTest()
		{
			NuGenTabPage tabPage = _tabbedMdi.AddTabPage(_ctrl, "", null);
			Assert.AreEqual(1, _tabbedMdi.TabPages.Count);
		}
	}
}
