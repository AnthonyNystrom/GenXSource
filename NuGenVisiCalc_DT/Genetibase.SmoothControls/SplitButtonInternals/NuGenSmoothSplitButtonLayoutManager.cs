/* -----------------------------------------------
 * NuGenSmoothSplitButtonLayoutManager.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.SplitButtonInternals;
using Genetibase.Shared.Drawing;
using Genetibase.SmoothControls.ButtonInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.SmoothControls.SplitButtonInternals
{
	/// <summary>
	/// </summary>
	public sealed class NuGenSmoothSplitButtonLayoutManager : INuGenSplitButtonLayoutManager
	{
		/// <summary>
		/// </summary>
		public Rectangle GetArrowRectangle(Rectangle clientRectangle, RightToLeft rightToLeft)
		{
			Point arrowLocation = new Point(clientRectangle.Left, clientRectangle.Top);
			Size arrowSize = new Size(28, clientRectangle.Height);

			if (rightToLeft == RightToLeft.No)
			{
				arrowLocation.X = clientRectangle.Right - arrowSize.Width;
			}

			return Rectangle.Inflate(new Rectangle(arrowLocation, arrowSize), -3, -6);
		}

		/// <summary>
		/// </summary>
		public Rectangle GetContentRectangle(
			Rectangle clientRectangle
			, Rectangle arrowRectangle
			, RightToLeft rightToLeft
			)
		{
			Rectangle contentRectangle;

			if (rightToLeft == RightToLeft.Yes)
			{
				contentRectangle = Rectangle.FromLTRB(
					arrowRectangle.Right
					, clientRectangle.Top
					, clientRectangle.Right
					, clientRectangle.Bottom
				);
			}
			else
			{
				contentRectangle = Rectangle.FromLTRB(
					clientRectangle.Left
					, clientRectangle.Top
					, arrowRectangle.Left
					, clientRectangle.Bottom
				);
			}

			return Rectangle.Inflate(contentRectangle, -6, -6);
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="imageBoundsParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public Rectangle GetImageBounds(NuGenBoundsParams imageBoundsParams)
		{
			if (imageBoundsParams == null)
			{
				throw new ArgumentNullException("imageBoundsParams");
			}

			return _buttonLayoutManager.GetImageBounds(imageBoundsParams);
		}

		/// <summary>
		/// </summary>
		public Rectangle GetSplitLineRectangle(Rectangle clientRectangle, Rectangle arrowRectangle, RightToLeft rightToLeft)
		{
			if (rightToLeft == RightToLeft.Yes)
			{
				return new Rectangle(
					arrowRectangle.Right
					, arrowRectangle.Top
					, arrowRectangle.Width
					, arrowRectangle.Height
				);
			}

			return new Rectangle(
				arrowRectangle.Left
				, arrowRectangle.Top
				, arrowRectangle.Width
				, arrowRectangle.Height
			);
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="textBoundsParams"/> is <see langword="null"/>.</para>
		/// </exception>
		public Rectangle GetTextBounds(NuGenBoundsParams textBoundsParams)
		{
			if (textBoundsParams == null)
			{
				throw new ArgumentNullException("textBoundsParams");
			}

			return _buttonLayoutManager.GetTextBounds(textBoundsParams);
		}

		private NuGenSmoothButtonLayoutManager _buttonLayoutManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothSplitButtonLayoutManager"/> class.
		/// </summary>
		public NuGenSmoothSplitButtonLayoutManager()
		{
			_buttonLayoutManager = new NuGenSmoothButtonLayoutManager();
		}
	}
}
