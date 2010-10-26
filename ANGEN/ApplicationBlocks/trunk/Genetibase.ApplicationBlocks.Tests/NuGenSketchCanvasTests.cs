/* -----------------------------------------------
 * NuGenSketchCanvasTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks.Tests
{
	[TestFixture]
	public class NuGenSketchCanvasTests
	{
		private NuGenSketchCanvas _sketchCanvas;

		[Test]
		public void ConstructorTest()
		{
			INuGenServiceProvider serviceProvider = new NuGenControlServiceProvider();
			try
			{
				_sketchCanvas = new NuGenSketchCanvas((IntPtr)(-1), serviceProvider);
				Assert.Fail();
			}
			catch (ArgumentException)
			{
			}

			Button button = new Button();
			button.CreateControl();
			Assert.IsTrue(button.IsHandleCreated);

			try
			{
				_sketchCanvas = new NuGenSketchCanvas(button.Handle, serviceProvider);
			}
			catch
			{
				Assert.Fail();
			}
		}
	}
}
