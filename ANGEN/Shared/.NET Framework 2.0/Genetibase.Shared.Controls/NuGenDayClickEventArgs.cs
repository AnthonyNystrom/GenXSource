/* -----------------------------------------------
 * NuGenDayClickEventArgs.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	public class NuGenDayClickEventArgs : EventArgs
	{
		/// <summary>
		/// </summary>
		public string Date
		{
			get
			{
				return this.m_date;
			}
		}

		/// <summary>
		/// </summary>
		public Rectangle DayRectangle
		{
			get
			{
				return m_dayRect;
			}
		}

		/// <summary>
		/// </summary>
		public MouseButtons Button
		{
			get
			{
				return this.m_button;
			}
		}

		/// <summary>
		/// </summary>
		public int DayX
		{
			get
			{
				return m_dayX;
			}
		}

		/// <summary>
		/// </summary>
		public int DayY
		{
			get
			{
				return m_dayY;
			}
		}

		/// <summary>
		/// </summary>
		public int X
		{
			get
			{
				return m_x;
			}
		}

		/// <summary>
		/// </summary>
		public int Y
		{
			get
			{
				return m_y;
			}
		}

		private string m_date;
		private MouseButtons m_button;
		private int m_dayX;
		private int m_dayY;
		private int m_x;
		private int m_y;
		private Rectangle m_dayRect;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDayClickEventArgs"/> class.
		/// </summary>
		public NuGenDayClickEventArgs()
		{
			m_date = DateTime.Today.ToShortDateString();
			m_button = MouseButtons.Left;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDayClickEventArgs"/> class.
		/// </summary>
		public NuGenDayClickEventArgs(string date, MouseButtons button, int dayX, int dayY, int x, int y, Rectangle rect)
		{
			m_date = date;
			m_button = button;
			m_dayX = dayX;
			m_dayY = dayY;
			m_x = x;
			m_y = y;
			m_dayRect = rect;
		}
	}
}
