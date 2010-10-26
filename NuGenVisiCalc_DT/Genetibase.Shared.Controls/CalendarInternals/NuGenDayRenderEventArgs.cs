/* -----------------------------------------------
 * NuGenDayRenderEventArgs.cs
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
	public class NuGenDayRenderEventArgs : EventArgs
	{
		#region Class Data

		private Graphics m_graphics;
		private bool m_ownerDraw;
		private Rectangle m_rect;
		private DateTime m_date;
		private NuGenDayState m_state;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the DayRenderEventArgs class with default settings
		/// </summary>
		public NuGenDayRenderEventArgs()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDayRenderEventArgs"/> class.
		/// </summary>
		public NuGenDayRenderEventArgs(Graphics graphics, Rectangle rect, DateTime date, NuGenDayState state)
		{
			this.m_graphics = graphics;
			this.m_rect = rect;
			this.m_date = date;
			this.m_state = state;
		}

		#endregion

		#region Properties

		/// <summary>
		/// </summary>
		public Graphics Graphics
		{
			get
			{
				return m_graphics;
			}
		}

		/// <summary>
		/// </summary>
		public DateTime Date
		{
			get
			{
				return m_date;
			}
		}

		/// <summary>
		/// </summary>
		public int Width
		{
			get
			{
				return m_rect.Width;
			}
		}

		/// <summary>
		/// </summary>
		public NuGenDayState State
		{
			get
			{
				return m_state;
			}
		}

		/// <summary>
		/// </summary>
		public int Height
		{
			get
			{
				return m_rect.Height;
			}
		}

		/// <summary>
		/// </summary>
		public bool OwnerDraw
		{
			set
			{
				m_ownerDraw = value;
			}
			get
			{
				return m_ownerDraw;
			}
		}

		#endregion
	}

}
