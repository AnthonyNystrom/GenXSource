/* -----------------------------------------------
 * NuGenContentAlignment.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Drawing
{
	/// <summary>
	/// Represents methods for content alignment processing.
	/// </summary>
	public sealed class NuGenContentAlignment
	{
		#region Methods.Public.Static

		/// <summary>
		/// Gets the <see cref="StringAlignment"/> from the specified <see cref="ContentAlignment"/>.
		/// </summary>
		/// <param name="contentAlignment">The <see cref="ContentAlignment"/>.</param>
		/// <returns></returns>
		public static StringAlignment GetAlignment(ContentAlignment contentAlignment)
		{
			if (
				(contentAlignment & ContentAlignment.BottomCenter) != 0
				|| (contentAlignment & ContentAlignment.MiddleCenter) != 0
				|| (contentAlignment & ContentAlignment.TopCenter) != 0
				)
			{
				return StringAlignment.Center;
			}
			else if (
				(contentAlignment & ContentAlignment.BottomRight) != 0
				|| (contentAlignment & ContentAlignment.MiddleRight) != 0
				|| (contentAlignment & ContentAlignment.TopRight) != 0
				)
			{
				return StringAlignment.Far;
			}
			else
			{
				return StringAlignment.Near;
			}
		}

		/// <summary>
		/// Gets the line <see cref="StringAlignment"/> from the specified <see cref="ContentAlignment"/>.
		/// </summary>
		/// <param name="contentAlignment">The <see cref="ContentAlignment"/>.</param>
		/// <returns></returns>
		public static StringAlignment GetLineAlignment(ContentAlignment contentAlignment)
		{
			if (
				(contentAlignment & ContentAlignment.MiddleCenter) != 0
				|| (contentAlignment & ContentAlignment.MiddleLeft) != 0
				|| (contentAlignment & ContentAlignment.MiddleRight) != 0
				)
			{
				return StringAlignment.Center;
			}
			else if (
				(contentAlignment & ContentAlignment.BottomCenter) != 0
				|| (contentAlignment & ContentAlignment.BottomLeft) != 0
				|| (contentAlignment & ContentAlignment.BottomRight) != 0
				)
			{
				return StringAlignment.Far;
			}
			else
			{
				return StringAlignment.Near;
			}
		}

		#endregion

		#region Constructors
	
		/// <summary>
		/// This class is immutable.
		/// </summary>
		private NuGenContentAlignment()
		{
		}
		
		#endregion
	}
}
