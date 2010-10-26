/* -----------------------------------------------
 * NuGenSmoothCalendarRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using res = Genetibase.SmoothControls.Properties.Resources;

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.CalendarInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.SmoothControls.CalendarInternals
{
	/// <summary>
	/// </summary>
	public class NuGenSmoothCalendarRenderer : NuGenSmoothRenderer, INuGenCalendarRenderer
	{
		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		/// <exception cref="ArgumentNullException"><paramref name="paintParams"/> is <see langword="null"/>.</exception>
		public void DrawBorder(NuGenPaintParams paintParams)
		{
			if (paintParams == null)
			{
				throw new ArgumentNullException("paintParams");
			}

			base.DrawBorder(paintParams.Graphics, paintParams.Bounds, paintParams.State);
		}

		/*
		 * Border
		 */

		/// <summary>
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public Color GetBorderColor(NuGenControlState state)
		{
			return this.ColorManager.GetBorderColor(state);
		}

		/*
		 * Footer
		 */

		/// <summary>
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public NuGenGradientMode GetFooterGradientMode(NuGenControlState state)
		{
			return NuGenGradientMode.None;
		}

		/// <summary>
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public Color GetFooterBackColorBegin(NuGenControlState state)
		{
			return Color.White;
		}

		/// <summary>
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public Color GetFooterBackColorEnd(NuGenControlState state)
		{
			return Color.White;
		}

		/// <summary>
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public Color GetFooterTextColor(NuGenControlState state)
		{
			return Color.Black;
		}

		/*
		 * Header
		 */

		/// <summary>
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public NuGenGradientMode GetHeaderGradientMode(NuGenControlState state)
		{
			return NuGenGradientMode.Vertical;
		}

		/// <summary>
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public Color GetHeaderBackColorBegin(NuGenControlState state)
		{
			return this.ColorManager.GetBackgroundGradientBegin(state);
		}

		/// <summary>
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public Color GetHeaderBackColorEnd(NuGenControlState state)
		{
			return this.ColorManager.GetBackgroundGradientEnd(state);
		}

		/// <summary>
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public Color GetHeaderTextColor(NuGenControlState state)
		{
			return Color.Black;
		}

		/*
		 * Image
		 */

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public Image GetPreviousYearImage()
		{
			return res.FastBack;
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public Image GetPreviousMonthImage()
		{
			return res.Back;
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public Image GetNextYearImage()
		{
			return res.FastForward;
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public Image GetNextMonthImage()
		{
			return res.Forward;
		}

		/*
		 * Month
		 */

		/// <summary>
		/// </summary>
		public Color GetMonthSelectedBackColor()
		{
			return this.ColorManager.GetBorderColor(NuGenControlState.Hot);
		}

		/// <summary>
		/// </summary>
		public Color GetMonthSelectedBorderColor()
		{
			return this.ColorManager.GetBorderColor(NuGenControlState.Hot);	
		}

		/// <summary>
		/// </summary>
		public Color GetMonthFocusedBackColor()
		{
			return this.ColorManager.GetBackgroundGradientBegin(NuGenControlState.Focused);
		}

		/// <summary>
		/// </summary>
		public Color GetMonthFocusedBorderColor()
		{
			return this.ColorManager.GetBorderColor(NuGenControlState.Focused);
		}

		/*
		 * Weekday
		 */

		/// <summary>
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public NuGenGradientMode GetWeekdayGradientMode(NuGenControlState state)
		{
			return NuGenGradientMode.None;
		}

		/// <summary>
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public Color GetWeekdayBackColorBegin(NuGenControlState state)
		{
			return Color.White;
		}

		/// <summary>
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public Color GetWeekdayBackColorEnd(NuGenControlState state)
		{
			return Color.White;
		}

		/// <summary>
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public Color GetWeekdayTextColor(NuGenControlState state)
		{
			return Color.Black;
		}

		/*
		 * Weeknumber
		 */

		/// <summary>
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public NuGenGradientMode GetWeeknumberGradientMode(NuGenControlState state)
		{
			return NuGenGradientMode.None;
		}

		/// <summary>
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public Color GetWeeknumberBackColorBegin(NuGenControlState state)
		{
			return Color.White;
		}

		/// <summary>
		/// </summary>
		public Color GetWeeknumberBackColorEnd(NuGenControlState state)
		{
			return Color.White;
		}

		/// <summary>
		/// </summary>
		/// <param name="state"></param>
		/// <returns></returns>
		public Color GetWeeknumberTextColor(NuGenControlState state)
		{
			return Color.Black;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothCalendarRenderer"/> class.
		/// </summary>
		/// <param name="serviceProvider">Requires:<para/>
		/// 	<see cref="INuGenSmoothColorManager"/><para/></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothCalendarRenderer(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
