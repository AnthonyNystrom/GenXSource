/* -----------------------------------------------
 * NuGenWndLessControlTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using Genetibase.Shared.Windows;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Tests
{
	partial class NuGenWndLessControlTests
	{
		class EventSink : MockObject
		{
			#region Expectations

			/*
			 * Click
			 */

			private ExpectationCounter _clickCount = new ExpectationCounter("clickCount");

			public int ExpectedClickCount
			{
				set
				{
					_clickCount.Expected = value;
				}
			}

			/*
			 * MouseDown
			 */

			private ExpectationCounter _mouseDownCount = new ExpectationCounter("mouseDownCount");

			public int ExpectedMouseDownCount
			{
				set
				{
					_mouseDownCount.Expected = value;
				}
			}

			/*
			 * MouseEnter
			 */

			private ExpectationCounter _mouseEnterCount = new ExpectationCounter("mouseEnterCount");

			public int ExpectedMouseEnterCount
			{
				set
				{
					_mouseEnterCount.Expected = value;
				}
			}

			/*
			 * MouseLeave
			 */

			private ExpectationCounter _mouseLeaveCount = new ExpectationCounter("mouseLeaveCount");

			public int ExpectedMouseLeaveCount
			{
				set
				{
					_mouseLeaveCount.Expected = value;
				}
			}

			/*
			 * MouseMove
			 */

			private ExpectationCounter _mouseMoveCount = new ExpectationCounter("mouseMoveCount");

			public int ExpectedMouseMoveCount
			{
				set
				{
					_mouseMoveCount.Expected = value;
				}
			}

			/*
			 * MouseUp
			 */

			private ExpectationCounter _mouseUpCount = new ExpectationCounter("mouseUpCount");

			public int ExpectedMouseUpCount
			{
				set
				{
					_mouseUpCount.Expected = value;
				}
			}

			/*
			 * Paint
			 */

			private ExpectationCounter _paintCount = new ExpectationCounter("paintCount");

			public int ExpectedPaintCount
			{
				set
				{
					_paintCount.Expected = value;
				}
			}

			/*
			 * ParentChanged
			 */

			private ExpectationCounter _parentChangedCount = new ExpectationCounter("parentChangedCount");

			public int ExpectedParentChangedCount
			{
				set
				{
					_parentChangedCount.Expected = value;
				}
			}

			#endregion

			#region Constructors

			public EventSink(NuGenWndLessControl eventBubbler)
			{
				Assert.IsNotNull(eventBubbler);

				eventBubbler.Click += delegate
				{
					_clickCount.Inc();
				};

				eventBubbler.MouseDown += delegate
				{
					_mouseDownCount.Inc();
				};

				eventBubbler.MouseEnter += delegate
				{
					_mouseEnterCount.Inc();
				};

				eventBubbler.MouseLeave += delegate
				{
					_mouseLeaveCount.Inc();
				};

				eventBubbler.MouseMove += delegate
				{
					_mouseMoveCount.Inc();
				};

				eventBubbler.MouseUp += delegate
				{
					_mouseUpCount.Inc();
				};

				eventBubbler.Paint += delegate
				{
					_paintCount.Inc();
				};

				eventBubbler.ParentChanged += delegate
				{
					_parentChangedCount.Inc();
				};
			}

			#endregion
		}

		class StubControl : Control
		{
			#region Methods.Public

			public void InvokeClick(MouseEventArgs e)
			{
				this.SetCursorPosition(e.Location);
				this.OnClick(EventArgs.Empty);
			}

			public void InvokeMouseDown(MouseEventArgs e)
			{
				this.OnMouseDown(e);
			}

			public void InvokeMouseEnter(MouseEventArgs e)
			{
				this.SetCursorPosition(e.Location);
				this.OnMouseEnter(EventArgs.Empty);
			}

			public void InvokeMouseLeave(MouseEventArgs e)
			{
				this.SetCursorPosition(e.Location);
				this.OnMouseLeave(e);
			}

			public void InvokeMouseMove(MouseEventArgs e)
			{
				this.OnMouseMove(e);
			}

			public void InvokeMouseUp(MouseEventArgs e)
			{
				this.OnMouseUp(e);
			}

			#endregion

			#region Methods.Private

			private void SetCursorPosition(Point cp)
			{
				Point sp = this.PointToScreen(cp);
				Cursor.Position = sp;
			}

			#endregion

			#region Constructors

			public StubControl()
			{
			}

			#endregion
		}
	}
}
