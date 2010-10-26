/* -----------------------------------------------
 * NuGenStringProcessorTests.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Text;

using NUnit.Framework;

using System;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public class NuGenStringProcessorTests
	{
		private NuGenStringProcessor _stringProcessor = null;

		[SetUp]
		public void SetUp()
		{
			_stringProcessor = new NuGenStringProcessor();
		}

		[Test]
		public void GetContentUntilCRLFTest()
		{
			string firstLine = "first line";
			string text = string.Concat(firstLine, System.Environment.NewLine, "second line");
			Assert.AreEqual(firstLine, _stringProcessor.GetContentUntilCRLF(text));

			text = "first line\r\nsecond line";
			Assert.AreEqual(firstLine, _stringProcessor.GetContentUntilCRLF(text));
			Assert.AreEqual(firstLine, _stringProcessor.GetContentUntilCRLF(firstLine));

			text = "first line\rsecond line";
			Assert.AreEqual(text, _stringProcessor.GetContentUntilCRLF(text));

			text = "first line\nsecond line";
			Assert.AreEqual(text, _stringProcessor.GetContentUntilCRLF(text));

			text = "";
			Assert.AreEqual(text, _stringProcessor.GetContentUntilCRLF(text));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetContentUntilCRLFArgumentNullExceptionTest()
		{
			_stringProcessor.GetContentUntilCRLF(null);
		}
	}
}
