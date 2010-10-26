/* -----------------------------------------------
 * NuGenSmoothThumbnailLayoutManager.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks.ImageExportInternals;
using Genetibase.Shared.Drawing;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.SmoothApplicationBlocks.ImageExportInternals
{
	/// <summary>
	/// </summary>
	public sealed class NuGenSmoothThumbnailLayoutManager : INuGenThumbnailLayoutManager
	{
		/// <summary>
		/// </summary>
		/// <param name="clientRectangle"></param>
		/// <param name="cwRotateButtonSize"></param>
		/// <returns></returns>
		public Point GetCWRotateButtonLocation(Rectangle clientRectangle, Size cwRotateButtonSize)
		{
			return new Point(
				clientRectangle.Right - cwRotateButtonSize.Width - _rotateButtonOffset
				, clientRectangle.Bottom - cwRotateButtonSize.Height - _rotateButtonOffset
			);
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public Size GetCWRotateButtonSize()
		{
			return _rotateButtonSize;
		}

		/// <summary>
		/// </summary>
		/// <param name="clientRectangle"></param>
		/// <param name="ccwRotateButtonSize"></param>
		/// <returns></returns>
		public Point GetCCWRotateButtonLocation(Rectangle clientRectangle, Size ccwRotateButtonSize)
		{
			return new Point(
				clientRectangle.Left + _rotateButtonOffset
				, clientRectangle.Bottom - ccwRotateButtonSize.Height - _rotateButtonOffset
			);	
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public Size GetCCWRotateButtonSize()
		{
			return _rotateButtonSize;
		}

		/// <summary>
		/// </summary>
		/// <param name="clientRectangle"></param>
		/// <param name="imageSize"></param>
		/// <returns></returns>
		public Rectangle GetImageBounds(Rectangle clientRectangle, Size imageSize)
		{
			return NuGenControlPaint.ScaleToFit(
				Rectangle.Inflate(clientRectangle, -15, -15)
				, imageSize
			);
		}

		/// <summary>
		/// </summary>
		public Rectangle GetGridPanelBounds(Rectangle clientRectangle, RightToLeft rightToLeft)
		{
			if (rightToLeft == RightToLeft.Yes)
			{
				return new Rectangle(
					clientRectangle.Left
					, clientRectangle.Top + 1
					, clientRectangle.Width - 1
					, clientRectangle.Height - 1
				);
			}

			return new Rectangle(
				clientRectangle.Left + 1
				, clientRectangle.Top + 1
				, clientRectangle.Width - 1
				, clientRectangle.Height - 1
			);
		}

		/// <summary>
		/// </summary>
		public Rectangle GetLoupePanelBounds(Rectangle clientRectangle, RightToLeft rightToLeft)
		{
			return new Rectangle(
				clientRectangle.Left
				, clientRectangle.Top
				, clientRectangle.Width
				, clientRectangle.Height + 1
			);
		}

		/// <summary>
		/// </summary>
		/// <param name="clientRectangle"></param>
		/// <param name="fontSize"></param>
		/// <returns></returns>
		public Rectangle GetTextBounds(Rectangle clientRectangle, float fontSize)
		{
			return Rectangle.Inflate(clientRectangle, 0, (int)(fontSize * 0.30f));	
		}

		/// <summary>
		/// </summary>
		public int GetToolbarHeight()
		{
			return 32;
		}

		/// <summary>
		/// </summary>
		public Size GetToolbarButtonSize()
		{
			return new Size(30, 30);	
		}

		private static readonly Size _rotateButtonSize = new Size(16, 16);
		private const int _rotateButtonOffset = 5;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothThumbnailLayoutManager"/> class.
		/// </summary>
		public NuGenSmoothThumbnailLayoutManager()
		{
		}
	}
}
