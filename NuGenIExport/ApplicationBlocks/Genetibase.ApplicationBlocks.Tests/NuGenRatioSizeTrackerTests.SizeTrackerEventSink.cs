/* -----------------------------------------------
 * NuGenRateSizeTrackerTests.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using DotNetMock;

using Genetibase.ApplicationBlocks.ImageExport;

using System;

namespace Genetibase.ApplicationBlocks.Tests
{
	partial class NuGenRatioSizeTrackerTests
	{
		class SizeTrackerEventSink : MockObject
		{
			#region Declarations

			private NuGenRatioSizeTracker sizeTracker = null;

			#endregion

			#region Properties.Public

			/* 
			 * ExpectedHeightChangedCallsCount
			 */

			private ExpectationCounter expectedHeightChangedCallsCount = new ExpectationCounter("expectedHeightChangedCallsCount");

			public int ExpectedHeightChangedCallsCount
			{
				set
				{
					this.expectedHeightChangedCallsCount.Expected = value;
				}
			}

			/*
			 * ExpetedHeight
			 */

			private ExpectationValue expectedHeight = new ExpectationValue("expectedHeight");

			public int ExpectedHeight
			{
				set
				{
					this.expectedHeight.Expected = value;
				}
			}

			/*
			 * ExpectedWidth
			 */

			private ExpectationValue expectedWidth = new ExpectationValue("expectedWidth");

			public int ExpectedWidth
			{
				set
				{
					this.expectedWidth.Expected = value;
				}
			}

			/*
			 * ExpectedWidthChangedCallsCount
			 */

			private ExpectationCounter expectedWidthChangedCallsCount = new ExpectationCounter("expectedWidthChangedCallsCount");

			public int ExpectedWidthChangedCallsCount
			{
				set
				{
					this.expectedWidthChangedCallsCount.Expected = value;
				}
			}

			#endregion

			#region Constructors

			public SizeTrackerEventSink(NuGenRatioSizeTracker sizeTracker)
			{
				if (sizeTracker == null)
				{
					throw new ArgumentNullException("sizeTracker");
				}

				this.sizeTracker = sizeTracker;

				this.sizeTracker.HeightChanged += delegate
				{
					this.expectedHeight.Actual = this.sizeTracker.Height;
					this.expectedWidth.Actual = this.sizeTracker.Width;
					this.expectedHeightChangedCallsCount.Inc();
				};

				this.sizeTracker.WidthChanged += delegate
				{
					this.expectedWidth.Actual = this.sizeTracker.Width;
					this.expectedHeight.Actual = this.sizeTracker.Width;
					this.expectedWidthChangedCallsCount.Inc();
				};
			}

			#endregion
		}
	}
}
