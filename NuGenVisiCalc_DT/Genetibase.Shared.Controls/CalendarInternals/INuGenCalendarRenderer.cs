/* -----------------------------------------------
 * INuGenCalendarRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls.CalendarInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenCalendarRenderer
	{
		/*
		 * Draw
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException"><para><paramref name="paintParams"/> is <see langword="null"/>.</para></exception>
		void DrawBackground(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException"><para><paramref name="paintParams"/> is <see langword="null"/>.</para></exception>
		void DrawBorder(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException"><para><paramref name="paintParams"/> is <see langword="null"/>.</para></exception>
		void DrawImage(NuGenImagePaintParams paintParams);

		/*
		 * Border
		 */

		/// <summary>
		/// </summary>
		Color GetBorderColor(NuGenControlState state);

		/*
		 * Footer
		 */

		/// <summary>
		/// </summary>
		NuGenGradientMode GetFooterGradientMode(NuGenControlState state);

		/// <summary>
		/// </summary>
		Color GetFooterBackColorBegin(NuGenControlState state);

		/// <summary>
		/// </summary>
		Color GetFooterBackColorEnd(NuGenControlState state);

		/// <summary>
		/// </summary>
		Color GetFooterTextColor(NuGenControlState state);

		/*
		 * Header
		 */

		/// <summary>
		/// </summary>
		NuGenGradientMode GetHeaderGradientMode(NuGenControlState state);

		/// <summary>
		/// </summary>
		Color GetHeaderBackColorBegin(NuGenControlState state);

		/// <summary>
		/// </summary>
		Color GetHeaderBackColorEnd(NuGenControlState state);

		/// <summary>
		/// </summary>
		Color GetHeaderTextColor(NuGenControlState state);

		/*
		 * Images
		 */

		/// <summary>
		/// </summary>
		Image GetPreviousYearImage();

		/// <summary>
		/// </summary>
		Image GetPreviousMonthImage();

		/// <summary>
		/// </summary>
		Image GetNextYearImage();

		/// <summary>
		/// </summary>
		Image GetNextMonthImage();

		/*
		 * Month
		 */

		/// <summary>
		/// </summary>
		Color GetMonthSelectedBackColor();

		/// <summary>
		/// </summary>
		Color GetMonthSelectedBorderColor();

		/// <summary>
		/// </summary>
		Color GetMonthFocusedBackColor();

		/// <summary>
		/// </summary>
		Color GetMonthFocusedBorderColor();

		/*
		 * Weekday
		 */

		/// <summary>
		/// </summary>
		NuGenGradientMode GetWeekdayGradientMode(NuGenControlState state);

		/// <summary>
		/// </summary>
		Color GetWeekdayBackColorBegin(NuGenControlState state);

		/// <summary>
		/// </summary>
		Color GetWeekdayBackColorEnd(NuGenControlState state);

		/// <summary>
		/// </summary>
		Color GetWeekdayTextColor(NuGenControlState state);

		/*
		 * Weeknumber
		 */

		/// <summary>
		/// </summary>
		NuGenGradientMode GetWeeknumberGradientMode(NuGenControlState state);

		/// <summary>
		/// </summary>
		Color GetWeeknumberBackColorBegin(NuGenControlState state);

		/// <summary>
		/// </summary>
		Color GetWeeknumberBackColorEnd(NuGenControlState state);

		/// <summary>
		/// </summary>
		Color GetWeeknumberTextColor(NuGenControlState state);
	}
}
