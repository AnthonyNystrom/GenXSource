/* -----------------------------------------------
 * NuGenWeekday.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.CalendarInternals
{
	/// <summary>
	/// </summary>
	[TypeConverter(typeof(NuGenWeekdayConverter))]
	public class NuGenWeekday : IDisposable
	{
		/// <summary>
		/// </summary>
		internal event EventHandler<NuGenWeekdayClickEventArgs> Click;

		/// <summary>
		/// </summary>
		internal event EventHandler<NuGenWeekdayClickEventArgs> DoubleClick;

		/// <summary>
		/// </summary>
		internal event EventHandler<NuGenWeekdayPropertyEventArgs> PropertyChanged;

		private bool disposed;
		private NuGenCalendar m_calendar;
		private Color m_backColor1;
		private Color m_backColor2;
		private NuGenGradientMode m_gradientMode;
		private Color m_textColor;
		private Color m_borderColor;
		private Font m_font;
		private NuGenDayFormat m_dayFormat;
		private StringAlignment m_align;
		private Rectangle m_rect;
		private Region m_region;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenWeekday"/> class.
		/// </summary>
		public NuGenWeekday(NuGenCalendar calendar)
		{
			m_calendar = calendar;
			m_backColor1 = Color.White;
			m_backColor2 = Color.White;
			m_gradientMode = NuGenGradientMode.None;
			m_textColor = Color.FromArgb(0, 84, 227);
			m_font = new Font("Microsoft Sans Serif", (float)8.25);
			m_dayFormat = NuGenDayFormat.Short;
			m_align = StringAlignment.Center;
			m_borderColor = Color.Black;
		}

		/// <summary>
		/// </summary>
		/// <param name="disposing"><para><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</para></param>
		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					m_font.Dispose();
					m_region.Dispose();
				}
				// shared cleanup logic
				disposed = true;
			}
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#region Properties

		/// <summary>
		/// </summary>
		[Description("Color used border.")]
		[DefaultValue(typeof(Color), "Black")]
		public Color BorderColor
		{
			get
			{
				return m_borderColor;
			}
			set
			{
				if (m_borderColor != value)
				{
					m_borderColor = value;
					if (PropertyChanged != null)
						PropertyChanged(this, new NuGenWeekdayPropertyEventArgs(NuGenWeekdayProperty.BorderColor));
					m_calendar.Invalidate();
				}
			}
		}

		/// <summary>
		/// </summary>
		[Description("Color used for background.")]
		[DefaultValue(typeof(Color), "White")]
		public Color BackColor1
		{
			get
			{
				return m_backColor1;
			}
			set
			{
				if (m_backColor1 != value)
				{
					m_backColor1 = value;
					if (PropertyChanged != null)
						PropertyChanged(this, new NuGenWeekdayPropertyEventArgs(NuGenWeekdayProperty.BackColor1));
					m_calendar.Invalidate();
				}
			}
		}

		/// <summary>
		/// </summary>
		[Description("Second background color when using gradient.")]
		[DefaultValue(typeof(Color), "White")]
		public Color BackColor2
		{
			get
			{
				return m_backColor2;
			}
			set
			{
				if (m_backColor2 != value)
				{
					m_backColor2 = value;
					if (PropertyChanged != null)
						PropertyChanged(this, new NuGenWeekdayPropertyEventArgs(NuGenWeekdayProperty.BackColor2));
					m_calendar.Invalidate();
				}
			}
		}

		/// <summary>
		/// </summary>
		[Description("Type of gradient used.")]
		[DefaultValue(typeof(NuGenGradientMode), "None")]
		public NuGenGradientMode GradientMode
		{
			get
			{
				return m_gradientMode;
			}
			set
			{
				if (m_gradientMode != value)
				{
					m_gradientMode = value;
					if (PropertyChanged != null)
						PropertyChanged(this, new NuGenWeekdayPropertyEventArgs(NuGenWeekdayProperty.GradientMode));
					m_calendar.Invalidate();
				}
			}
		}

		internal Rectangle Rect
		{
			get
			{
				return m_rect;
			}
			set
			{
				m_rect = value;
				m_region = new Region(m_rect);
			}
		}

		/// <summary>
		/// </summary>
		[Description("Indicates wether the name of the day should be displayed using a long or short format.")]
		[DefaultValue(typeof(NuGenDayFormat), "Short")]
		public NuGenDayFormat Format
		{
			get
			{
				return m_dayFormat;
			}
			set
			{
				if (m_dayFormat != value)
				{
					m_dayFormat = value;
					if (PropertyChanged != null)
						PropertyChanged(this, new NuGenWeekdayPropertyEventArgs(NuGenWeekdayProperty.Format));
					m_calendar.Invalidate();
				}
			}
		}

		/// <summary>
		/// </summary>
		[Description("Determines the position for the text.")]
		[DefaultValue(StringAlignment.Center)]
		public StringAlignment Align
		{
			get
			{
				return m_align;
			}
			set
			{
				if (m_align != value)
				{
					m_align = value;
					if (PropertyChanged != null)
						PropertyChanged(this, new NuGenWeekdayPropertyEventArgs(NuGenWeekdayProperty.Align));
					m_calendar.Invalidate();
				}
			}
		}

		/// <summary>
		/// </summary>
		[Description("The font used for weekdays.")]
		[DefaultValue(typeof(Font), "Microsoft Sans Serif; 8,25pt")]
		public Font Font
		{
			get
			{
				return m_font;
			}
			set
			{
				if (m_font != value)
				{
					m_font = value;
					m_calendar.DoLayout();
					if (PropertyChanged != null)
						PropertyChanged(this, new NuGenWeekdayPropertyEventArgs(NuGenWeekdayProperty.Font));
					m_calendar.Invalidate();
				}
			}
		}

		/// <summary>
		/// </summary>
		[Description("Color used for text.")]
		[DefaultValue(typeof(Color), "0,84,227")]
		public Color TextColor
		{
			get
			{
				return m_textColor;
			}
			set
			{
				if (m_textColor != value)
				{
					m_textColor = value;
					if (PropertyChanged != null)
						PropertyChanged(this, new NuGenWeekdayPropertyEventArgs(NuGenWeekdayProperty.TextColor));
					m_calendar.Invalidate();
				}
			}
		}

		#endregion

		#region Methods

		internal void MouseMove(Point mouseLocation)
		{
			if (m_region.IsVisible(mouseLocation))
			{
				m_calendar.ActiveRegion = NuGenCalendarRegion.Weekdays;
			}
		}


		internal void MouseClick(Point mouseLocation, MouseButtons button, NuGenClickMode mode)
		{
			if (m_region.IsVisible(mouseLocation))
			{
				int day;
				day = (mouseLocation.X / (int)m_calendar.Month.DayWidth);
				if (mode == NuGenClickMode.Single)
				{
					if (this.Click != null)
						this.Click(this, new NuGenWeekdayClickEventArgs(day, button));
				}
				else
				{
					if (this.DoubleClick != null)
						this.DoubleClick(this, new NuGenWeekdayClickEventArgs(day, button));
				}

			}

		}

		internal string[] GetWeekDays()
		{
			int index = 0;
			string[] sysNames;
			string[] weekdays = new string[7];
			int FirstDayOfWeek = (int)m_calendar._dateTimeFormat.FirstDayOfWeek;

			// Get system names for weekdays
			if (Format == NuGenDayFormat.Short)
				sysNames = m_calendar._dateTimeFormat.AbbreviatedDayNames;
			else
				sysNames = m_calendar._dateTimeFormat.DayNames;

			weekdays.Initialize();

			// Arrange weekdays starting with first day of week
			for (int i = FirstDayOfWeek; i < weekdays.Length; i++)
			{
				weekdays[index] = sysNames[i];
				index++;
			}
			for (int i = 0; i < FirstDayOfWeek; i++)
			{
				weekdays[index] = sysNames[i];
				index++;
			}

			return weekdays;
		}

		internal bool IsVisible(Rectangle clip)
		{
			return m_region.IsVisible(clip);
		}

		internal void Draw(Graphics e)
		{
			Pen linePen = new Pen(m_borderColor, 1);
			StringFormat textFormat = new StringFormat();
			Rectangle dayRect = new Rectangle();
			int dayWidth;
			string[] weekdays;

			weekdays = GetWeekDays();

			Brush headerBrush = new SolidBrush(this.BackColor1);
			Brush headerTextBrush = new SolidBrush(this.TextColor);

			textFormat.LineAlignment = StringAlignment.Center;
			textFormat.Alignment = Align;

			if (m_gradientMode == NuGenGradientMode.None)
				e.FillRectangle(headerBrush, 0, m_rect.Top, m_calendar.Width, m_rect.Height);
			else
				m_calendar.DrawGradient(e, m_rect, m_backColor1, m_backColor2, m_gradientMode);

			dayWidth = (int)m_calendar.Month.DayWidth;

			for (int i = 0; i < 7; i++)
			{
				dayRect.Y = m_rect.Y;
				dayRect.Width = dayWidth;
				dayRect.Height = m_rect.Height;
				dayRect.X = (dayWidth * i) + m_rect.X;
				dayRect.X += (i + 1) * m_calendar.Month.Padding.Horizontal;
				if (i == 6)
					dayRect.Width = m_rect.Width - (int)(m_calendar.Month.Padding.Horizontal * 8) - (int)(dayWidth * 6) - 1;

				e.DrawString(weekdays[i], this.Font, headerTextBrush, dayRect, textFormat);
			}
			e.DrawLine(linePen, m_rect.X, m_rect.Bottom - 1, m_rect.Right, m_rect.Bottom - 1);

			// tidy up
			headerBrush.Dispose();
			headerTextBrush.Dispose();
			linePen.Dispose();
		}

		#endregion
	}
}
