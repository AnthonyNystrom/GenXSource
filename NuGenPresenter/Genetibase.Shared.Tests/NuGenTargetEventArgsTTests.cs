/* -----------------------------------------------
 * NuGenTargetEventArgsTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;

using NUnit.Framework;

using System;
using System.Windows.Forms;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public partial class NuGenTargetEventArgsTTests
	{
		[Test]
		public void ConstructorTest()
		{
			Control ctrl = new Control();
			string ctrlString = "ctrl";

			NuGenTargetEventArgsT<Control, string> eventArgs = new NuGenTargetEventArgsT<Control, string>(ctrl, ctrlString);

			Assert.AreEqual(ctrl, eventArgs.Target);
			Assert.AreEqual(ctrlString, eventArgs.TargetData);
		}
	}
}
