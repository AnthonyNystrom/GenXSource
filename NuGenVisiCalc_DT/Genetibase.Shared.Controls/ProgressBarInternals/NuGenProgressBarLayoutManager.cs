/* -----------------------------------------------
 * NuGenProgressBarLayoutManager.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls.ProgressBarInternals
{
	/// <summary>
	/// </summary>
	public class NuGenProgressBarLayoutManager : INuGenProgressBarLayoutManager
	{
		#region Declarations.Fields

		private int _blockSize;
		private int _blockOffsetSize;
		private int _marqueeBlockDivider;

		#endregion

		#region INuGenProgressBarLayoutManager Members

		/*
		 * GetBlocks
		 */

		/// <summary>
		/// </summary>
		/// <param name="continuousBounds"></param>
		/// <param name="orientation"></param>
		/// <returns></returns>
		public Rectangle[] GetBlocks(Rectangle continuousBounds, NuGenOrientationStyle orientation)
		{
			Rectangle[] blocks = new Rectangle[] { };

			if (orientation == NuGenOrientationStyle.Horizontal)
			{
				if (continuousBounds.Width > 0)
				{
					int blockCount = 1 + (continuousBounds.Width - _blockSize) / (_blockSize + _blockOffsetSize);
					blocks = new Rectangle[blockCount];
					int offset = continuousBounds.Left - (_blockSize + _blockOffsetSize);

					for (int i = 0; i < blockCount; i++)
					{
						offset += _blockSize;
						offset += _blockOffsetSize;

						blocks[i] = new Rectangle(offset, continuousBounds.Top, _blockSize, continuousBounds.Height);
					}
				}
			}
			else
			{
				if (continuousBounds.Height > 0)
				{
					int blockCount = 1 + (continuousBounds.Height - _blockSize) / (_blockSize + _blockOffsetSize);
					blocks = new Rectangle[blockCount];
					int offset = continuousBounds.Bottom + (_blockSize + _blockOffsetSize);

					for (int i = 0; i < blockCount; i++)
					{
						offset -= _blockSize;
						offset -= _blockOffsetSize;

						blocks[i] = new Rectangle(continuousBounds.Left, offset - _blockSize, continuousBounds.Width, _blockSize);
					}
				}
			}

			return blocks;
		}

		/*
		 * GetContinuousBounds
		 */

		/// <summary>
		/// </summary>
		public Rectangle GetContinuousBounds(Rectangle containerBounds, int min, int max, int value, NuGenOrientationStyle orientation)
		{
			Rectangle rect = Rectangle.Empty;

			if (max != min)
			{
				if (orientation == NuGenOrientationStyle.Horizontal)
				{
					rect = new Rectangle(
						containerBounds.Left,
						containerBounds.Top,
						(int)((float)containerBounds.Width / (float)(max - min) * (value - min)),
						containerBounds.Height
					);
				}
				else
				{
					int height = (int)((float)containerBounds.Height / (float)(max - min) * (value - min));

					rect = new Rectangle(
						containerBounds.Left,
						containerBounds.Bottom - height,
						containerBounds.Width,
						height
					);
				}
			}

			return rect;
		}

		/*
		 * GetMarqueeBlockBounds
		 */

		/// <summary>
		/// </summary>
		public Rectangle GetMarqueeBlockBounds(Rectangle containerBounds, int offset, NuGenOrientationStyle orientation)
		{
			if (containerBounds == Rectangle.Empty)
			{
				return Rectangle.Empty;
			}

			Point location;

			if (orientation == NuGenOrientationStyle.Horizontal)
			{
				location = new Point(offset + containerBounds.Left, containerBounds.Top);
			}
			else
			{
				location = new Point(containerBounds.Left, containerBounds.Top + offset);
			}

			return new Rectangle(
				location,
				this.GetMarqueeBlockSize(containerBounds, orientation)
			);
		}

		#endregion

		#region Properties.Protected.Virtual

		/*
		 * BlockSize
		 */

		/// <summary>
		/// </summary>
		protected virtual int BlockSize
		{
			get
			{
				return 12;
			}
		}

		/*
		 * BlockOffsetSize
		 */

		/// <summary>
		/// </summary>
		protected virtual int BlockOffsetSize
		{
			get
			{
				return 3;
			}
		}

		/*
		 * MarqueeBlockDivider
		 */

		/// <summary>
		/// </summary>
		protected virtual int MarqueeBlockDivider
		{
			get
			{
				return 4;
			}
		}

		#endregion

		#region Methods.Private

		/*
		 * GetMarqueeBlockSize
		 */

		private Size GetMarqueeBlockSize(Rectangle containerBounds, NuGenOrientationStyle orientation)
		{
			if (orientation == NuGenOrientationStyle.Horizontal)
			{
				return new Size(containerBounds.Width / _marqueeBlockDivider, containerBounds.Height);
			}

			return new Size(containerBounds.Width, containerBounds.Height / _marqueeBlockDivider);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenProgressBarLayoutManager"/> class.
		/// </summary>
		public NuGenProgressBarLayoutManager()
		{
			_blockSize = this.BlockSize;
			_blockOffsetSize = this.BlockOffsetSize;
			_marqueeBlockDivider = this.MarqueeBlockDivider;
		}

		#endregion
	}
}
