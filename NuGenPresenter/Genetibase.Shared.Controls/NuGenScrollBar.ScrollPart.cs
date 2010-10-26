/* -----------------------------------------------
 * NuGenScrollBar.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ScrollBarInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	partial class NuGenScrollBar
	{
		/// <summary>
		/// </summary>
		private abstract class ScrollPart : NuGenButtonBase
		{
			/*
			 * Orientation
			 */

			private NuGenOrientationStyle _orientation;

			/// <summary>
			/// </summary>
			public NuGenOrientationStyle Orientation
			{
				get
				{
					return _orientation;
				}
				set
				{
					if (_orientation != value)
					{
						_orientation = value;

						int bufferWidth = this.Width;
						this.Width = this.Height;
						this.Height = bufferWidth;

						this.Invalidate();
					}
				}
			}

			/*
			 * DefaultSize
			 */

			private static readonly Size _defaultSize = new Size(100, SystemInformation.HorizontalScrollBarHeight);

			/// <summary>
			/// </summary>
			protected override Size DefaultSize
			{
				get
				{
					return _defaultSize;
				}
			}

			/*
			 * OnPaint
			 */

			/// <summary>
			/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
			/// </summary>
			/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
			protected override void OnPaint(PaintEventArgs e)
			{
				if (this.Orientation == NuGenOrientationStyle.Vertical)
				{
					NuGenControlPaint.Make90CCWGraphics(e.Graphics, this.ClientRectangle);
				}
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="ScrollPart"/> class.
			/// </summary>
			/// <param name="serviceProvider">
			/// Requires:<para/>
			/// <see cref="INuGenButtonStateService"/><para/>
			/// <see cref="INuGenScrollBarRenderer"/><para/>
			/// </param>
			public ScrollPart(INuGenServiceProvider serviceProvider)
				: base(serviceProvider)
			{
			}
		}
	}
}
