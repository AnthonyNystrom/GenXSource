/* -----------------------------------------------
 * NuGenFormStateStoreTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.Drawing;

using NUnit.Framework;

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.UI.NuGenInterface.Tests
{
	[TestFixture]
	public partial class NuGenFormStateStoreTests
	{
		private NuGenFormStateStore _Store = null;
		private Form _Form = null;

		[SetUp]
		public void SetUp()
		{
			_Form = new Form();
			_Store = new NuGenFormStateStore();
		}

		[Test]
		public void StoreRestoreTest()
		{
			_Store.StoreFormState(_Form);

			bool originalDoubleBuffer = NuGenControlPaint.GetStyle(_Form, ControlStyles.OptimizedDoubleBuffer);
			bool originalOpaque = NuGenControlPaint.GetStyle(_Form, ControlStyles.Opaque);
			bool originalResizeRedraw = NuGenControlPaint.GetStyle(_Form, ControlStyles.ResizeRedraw);
			bool originalSelectable = NuGenControlPaint.GetStyle(_Form, ControlStyles.Selectable);
			Color originalBackColor = _Form.BackColor;
			Padding originalPadding = _Form.Padding;

			bool doubleBuffer = true;
			bool opaque = true;
			bool resizeRedraw = true;
			bool selectable = true;
			Color backColor = Color.Blue;
			Padding padding = new Padding(2, 3, 4, 5);

			Assert.AreNotEqual(originalBackColor, backColor);
			Assert.AreNotEqual(originalBackColor, padding);

			NuGenControlPaint.SetStyle(_Form, ControlStyles.OptimizedDoubleBuffer, doubleBuffer);
			NuGenControlPaint.SetStyle(_Form, ControlStyles.Opaque, opaque);
			NuGenControlPaint.SetStyle(_Form, ControlStyles.ResizeRedraw, resizeRedraw);
			NuGenControlPaint.SetStyle(_Form, ControlStyles.Selectable, selectable);
			_Form.BackColor = backColor;
			_Form.Padding = padding;

			_Store.RestoreFormState(_Form);

			bool restoredDoubleBuffer = NuGenControlPaint.GetStyle(_Form, ControlStyles.OptimizedDoubleBuffer);
			bool restoredOpaque = NuGenControlPaint.GetStyle(_Form, ControlStyles.Opaque);
			bool restoredResizeRedraw = NuGenControlPaint.GetStyle(_Form, ControlStyles.ResizeRedraw);
			bool restoredSelectable = NuGenControlPaint.GetStyle(_Form, ControlStyles.Selectable);
			Color restoredBackColor = _Form.BackColor;
			Padding restoredPadding = _Form.Padding;

			Assert.AreEqual(originalDoubleBuffer, restoredDoubleBuffer);
			Assert.AreEqual(originalOpaque, restoredOpaque);
			Assert.AreEqual(originalResizeRedraw, restoredResizeRedraw);
			Assert.AreEqual(originalSelectable, restoredSelectable);
			Assert.AreEqual(originalBackColor, restoredBackColor);
			Assert.AreEqual(originalPadding, restoredPadding);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RestoreFormStateArgumentNullExceptionTest()
		{
			_Store.RestoreFormState(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void StoreFormStateArgumentNullExceptionTest()
		{
			_Store.StoreFormState(null);
		}
	}
}
