/* -----------------------------------------------
 * INuGenToolTipLayoutManager.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.ToolTipInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenToolTipLayoutManager
	{
		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="tooltipInfo"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="headerFont"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="textFont"/> is <see langword="null"/>.</para>
		/// </exception>
		NuGenToolTipLayoutDescriptor BuildLayoutDescriptor(
			Graphics g,
			NuGenToolTipInfo tooltipInfo,
			Font headerFont,
			Font textFont
		);

		/// <summary>
		/// </summary>
		Size GetMinimumTooltipSize();

		/// <summary>
		/// </summary>
		void SetMinimumTooltipSize(Size value);

		/// <summary>
		/// </summary>
		Size GetShadowSize();

		/// <summary>
		/// </summary>
		/// <param name="targetControl">
		/// If <see langword="null"/> tooltip location is calculated according to <paramref name="cursorPosition"/> and <paramref name="cursorSize"/> only.
		/// </param>
		/// <param name="cursorPosition">Screen coordinates expected.</param>
		/// <param name="cursorSize"></param>
		/// <param name="placeBelowControl"></param>
		/// <returns></returns>
		Point GetToolTipLocation(Control targetControl, Point cursorPosition, Size cursorSize, bool placeBelowControl);
	}
}
