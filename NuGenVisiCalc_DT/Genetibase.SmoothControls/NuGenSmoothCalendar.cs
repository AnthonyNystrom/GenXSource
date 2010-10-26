/* -----------------------------------------------
 * NuGenSmoothCalendar.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.CalendarInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// <seealso cref="NuGenCalendar"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothCalendar), "Resources.NuGenIcon.png")]
	public class NuGenSmoothCalendar : NuGenCalendar
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothCalendar"/> class.
		/// </summary>
		public NuGenSmoothCalendar() 
			: this(NuGenSmoothServiceManager.CalendarServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothCalendar"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		/// 	<para><see cref="INuGenCalendarRenderer"/></para>
		/// </param>
		public NuGenSmoothCalendar(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
