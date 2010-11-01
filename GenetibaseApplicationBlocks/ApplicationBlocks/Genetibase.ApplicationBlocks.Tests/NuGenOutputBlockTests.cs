/* -----------------------------------------------
 * NuGenOutputBlock.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using DotNetMock;

using Genetibase.ApplicationBlocks.ImageExport;

using NUnit.Framework;

using System;
using System.Diagnostics;
using System.IO;

namespace Genetibase.ApplicationBlocks.Tests
{
	[TestFixture]
	public class NuGenOutputBlockTests
	{
		private NuGenOutputBlock outputBlock = null;

		[SetUp]
		public void SetUp()
		{
			this.outputBlock = new NuGenOutputBlock();
		}

		[Test]
		public void OutputBlockFilenameTest()
		{
			string filename = "Filename";
			this.outputBlock.Filename = filename;
			Assert.AreEqual(filename, this.outputBlock.Filename);
		}

		[Test]
		public void OutputBlockDirectoryNameTest()
		{
			string directoryName = @"D:\My Documents";
			this.outputBlock.DirectoryName = directoryName;
			Assert.AreEqual(directoryName, this.outputBlock.DirectoryName);
		}
	}
}
