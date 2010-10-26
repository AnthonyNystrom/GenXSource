/* -----------------------------------------------
 * NMCUSTOMDRAW.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Contains information specific to an NM_CUSTOMDRAW notification message.
	/// </summary>
	public struct NMCUSTOMDRAW
	{
		/// <summary>
		/// NMHDR structure that contains information about this notification message.
		/// </summary>
		public NMHDR hdr;

		/// <summary>
		/// Current drawing stage.
		/// </summary>
		public Int32 dwDrawStage;

		/// <summary>
		/// Handle to the control's device context. Use this HDC to perform any GDI functions.
		/// </summary>
		public IntPtr hdc;

		/// <summary>
		/// RECT structure that describes the bounding rectangle of the area being drawn. This member is
		/// initialized by the CDDS_ITEMPREPAINT and CDDS_PREPAINT notifications. 
		/// </summary>
		public RECT rc;

		/// <summary>
		/// Item number. What is contained in this member will depend on the type of control that is
		/// sending the notification. See the NM_CUSTOMDRAW notification reference for the specific control
		/// to determine what, if anything, is contained in this member.
		/// </summary>
		public Int32 dwItemSpec;

		/// <summary>
		/// Current item state.
		/// </summary>
		public Int32 uItemState;

		/// <summary>
		/// Application-defined item data.
		/// </summary>
		public Int32 lItemlParam;
	}
}
