/* -----------------------------------------------
 * NuGenRateSizeTrackerTests.cs
 * Copyright © 2006-2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using Genetibase.ApplicationBlocks.ImageExportInternals;

using System;

namespace Genetibase.ApplicationBlocks.Tests
{
	partial class NuGenRatioSizeTrackerTests
	{
		class SizeTrackerEventSink : MockObject
		{
			private ExpectationCounter _expectedHeightChangedCallsCount = new ExpectationCounter("expectedHeightChangedCallsCount");

			public int ExpectedHeightChangedCallsCount
			{
				set
				{
					_expectedHeightChangedCallsCount.Expected = value;
				}
			}

			private ExpectationValue _expectedHeight = new ExpectationValue("expectedHeight");

			public int ExpectedHeight
			{
				set
				{
					_expectedHeight.Expected = value;
				}
			}

			private ExpectationValue _expectedWidth = new ExpectationValue("expectedWidth");

			public int ExpectedWidth
			{
				set
				{
					_expectedWidth.Expected = value;
				}
			}

			private ExpectationCounter _expectedWidthChangedCallsCount = new ExpectationCounter("expectedWidthChangedCallsCount");

			public int ExpectedWidthChangedCallsCount
			{
				set
				{
					_expectedWidthChangedCallsCount.Expected = value;
				}
			}

			private NuGenRatioSizeTracker _sizeTracker;

			public SizeTrackerEventSink(NuGenRatioSizeTracker sizeTracker)
			{
				if (sizeTracker == null)
				{
					throw new ArgumentNullException("sizeTracker");
				}

				_sizeTracker = sizeTracker;

				_sizeTracker.HeightChanged += delegate
				{
					_expectedHeight.Actual = _sizeTracker.Height;
					_expectedWidth.Actual = _sizeTracker.Width;
					_expectedHeightChangedCallsCount.Inc();
				};

				_sizeTracker.WidthChanged += delegate
				{
					_expectedWidth.Actual = _sizeTracker.Width;
					_expectedHeight.Actual = _sizeTracker.Width;
					_expectedWidthChangedCallsCount.Inc();
				};
			}
		}
	}
}
