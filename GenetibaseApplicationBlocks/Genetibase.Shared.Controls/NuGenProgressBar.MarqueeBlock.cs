/* -----------------------------------------------
 * NuGenProgressBar.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Drawing;
using System.Text;

namespace Genetibase.Shared.Controls
{
	partial class NuGenProgressBar
	{
		/// <summary>
		/// Encapsulates marquee block associated data.
		/// </summary>
		protected class MarqueeBlock
		{
			#region Properties.Public

			/*
			 * Offset
			 */

			private int _offset = 0;

			/// <summary>
			/// </summary>
			public int Offset
			{
				get
				{
					return _offset;
				}
				set
				{
					_offset = value;
				}
			}

			#endregion

			#region Methods.Public

			/*
			 * GetBounds
			 */

			/// <summary>
			/// </summary>
			/// <param name="containerBounds"></param>
			/// <param name="orientation"></param>
			/// <returns></returns>
			public Rectangle GetBounds(Rectangle containerBounds, NuGenOrientationStyle orientation)
			{
				if (containerBounds == Rectangle.Empty)
				{
					return Rectangle.Empty;
				}

				if (orientation == NuGenOrientationStyle.Horizontal)
				{
					return new Rectangle(
						new Point(this.Offset, containerBounds.Top),
						this.GetSize(containerBounds, orientation)
					);
				}

				return new Rectangle(
					new Point(containerBounds.Left, containerBounds.Top + this.Offset),
					this.GetSize(containerBounds, orientation)
				);
			}

			#endregion

			#region Methods.Protected.Virtual

			/*
			 * GetSize
			 */

			/// <summary>
			/// </summary>
			/// <param name="containerBounds"></param>
			/// <param name="orientation"></param>
			/// <returns></returns>
			protected virtual Size GetSize(Rectangle containerBounds, NuGenOrientationStyle orientation)
			{
				if (orientation == NuGenOrientationStyle.Horizontal)
				{
					return new Size(containerBounds.Width / 4, containerBounds.Height);
				}

				return new Size(containerBounds.Width, containerBounds.Height / 4);
			}

			#endregion

			#region Constructors

			/// <summary>
			/// Initializes a new instance of the <see cref="MarqueeBlock"/> class.
			/// </summary>
			public MarqueeBlock()
			{

			}

			#endregion
		}
	}
}
