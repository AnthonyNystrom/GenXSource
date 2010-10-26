/* -----------------------------------------------
 * NuGenCollectionEditorInitializerTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Design.Tests
{
	[TestFixture]
	public class NuGenCollectionEditorInitializerTests
	{
		[Test]
		public void InitializeEditorFormTest()
		{
			Form form = new Form();
			NuGenCollectionEditorInitializer.InitializeEditorForm(form);

			Assert.IsFalse(form.MaximizeBox);
			Assert.IsFalse(form.MinimizeBox);
			Assert.IsFalse(form.ShowIcon);
			Assert.AreEqual(AutoScaleMode.Font, form.AutoScaleMode);
			Assert.AreEqual(FormStartPosition.CenterParent, form.StartPosition);
			Assert.AreEqual(new Padding(10), form.Padding);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void InitializeEditorFormArgumentNullExceptionTest()
		{
			NuGenCollectionEditorInitializer.InitializeEditorForm(null);
		}
	}
}
