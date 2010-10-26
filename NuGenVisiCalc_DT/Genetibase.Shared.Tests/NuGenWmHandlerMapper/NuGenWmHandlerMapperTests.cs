/* -----------------------------------------------
 * NuGenWmHandlerMapperTests.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.Windows;

using NUnit.Framework;

using System;
using System.Windows.Forms;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public partial class NuGenWmHandlerMapperTests
	{
		private const int WM_LBUTTONDOWN = 0x0201;
		private const int WM_LBUTTONUP = 0x0202;
		private const int WM_PAINT = 0x000F;
		
		private NuGenWmHandlerMapper _handlerMapper = null;
		private MockMessageProcessor _messageProcessor = null;
		private BackMockMessageProcessor _badMessageProcessor = null;

		[SetUp]
		public void SetUp()
		{
			_handlerMapper = new NuGenWmHandlerMapper();
			_messageProcessor = new MockMessageProcessor();
			_badMessageProcessor = new BackMockMessageProcessor();
		}
	}
}
