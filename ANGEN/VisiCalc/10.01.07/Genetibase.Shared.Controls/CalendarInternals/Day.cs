/* -----------------------------------------------
 * Day.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Drawing;

using System;
using System.Drawing; 
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.CalendarInternals
{	
	internal class Day : IDisposable
	{
		#region Properties.Public

		public int SelectionArea
		{
			get
			{
				return m_selection;
			}
			set
			{
				if (value != m_selection)
				{
					m_selection = value;
				}
			}
		}

		public bool UserDrawn
		{
			get
			{
				return m_userDrawn;
			}
			set
			{
				if (value != m_userDrawn)
				{
					m_userDrawn = value;
				}
			}
		}

		public int Week
		{
			get
			{
				return m_calendar.WeeknumberCallBack(m_date);
			}
		}

		public DayOfWeek Weekday
		{
			get
			{
				return m_date.DayOfWeek;
			}
		}

		public NuGenDayState State
		{
			get
			{
				return m_state;
			}
			set
			{
				if (value != m_state)
				{
					m_state = value;
				}
			}
		}

		public Rectangle Rectangle
		{
			get
			{
				return m_rect;
			}
			set
			{
				if (value != m_rect)
				{
					m_rect = value;
					m_region = new Region(m_rect);
				}
			}
		}

		public DateTime Date
		{
			get
			{
				return m_date;
			}
			set
			{
				if (m_date != value)
				{
					m_date = value;
				}
			}
		}

		#endregion

		#region Properties.Internal

		internal NuGenCalendar Calendar
		{
			get
			{
				return m_calendar;
			}
			set
			{
				m_calendar = value;
			}
		}

		internal NuGenMonth Month
		{
			get
			{
				return m_month;
			}
			set
			{
				m_month = value;
			}
		}

		internal Region[] DateRegion
		{
			get
			{
				return m_dateRgn;
			}
		}

		internal Region[] TextRegion
		{
			get
			{
				return m_textRgn;
			}
		}

		#endregion

		#region Methods.Internal

		internal void Draw(Graphics e, NuGenDateItem queryInfo)
		{
			StringFormat dateAlign = new StringFormat();
			StringFormat textAlign = new StringFormat();
			Font boldFont = new Font(m_month.DateFont.Name, m_month.DateFont.Size, m_month.DateFont.Style | FontStyle.Bold);
			Color bgColor1 = m_month.Colors.Days.BackColor1;
			Color bgColor2 = m_month.Colors.Days.BackColor2;
			NuGenGradientMode gradientMode = m_month.Colors.Days.GradientMode;
			Color textColor = m_month.Colors.Days.Text;
			Color dateColor = m_month.Colors.Days.Date;
			Brush dateBrush = new SolidBrush(dateColor);
			Brush textBrush = new SolidBrush(textColor);
			Brush bgBrush = new SolidBrush(bgColor1);

			string dateString;
			m_imageRect = new Rectangle();
			string text = "";
			bool drawDay = false;
			bool enabled = true;
			Image bgImage = null;

			int i = -1;

			bool boldedDate = false;

			NuGenDateItem[] info;
			m_dayImage = null;

			dateAlign = NuGenControlPaint.ContentAlignmentToStringFormat(m_month.DateAlign);
			textAlign = NuGenControlPaint.ContentAlignmentToStringFormat(m_month.TextAlign);

			if ((m_month.SelectedMonth.Month == m_date.Month) || (m_month.Calendar.ShowTrailingDates))
				drawDay = true;

			if (((m_date.DayOfWeek == DayOfWeek.Saturday) && (m_month.Colors.Weekend.Saturday)) ||
				 ((m_date.DayOfWeek == DayOfWeek.Sunday) && (m_month.Colors.Weekend.Sunday)))
			{
				bgColor1 = m_month.Colors.Weekend.BackColor1;
				bgColor2 = m_month.Colors.Weekend.BackColor2;
				dateColor = m_month.Colors.Weekend.Date;
				textColor = m_month.Colors.Weekend.Text;
				gradientMode = m_month.Colors.Weekend.GradientMode;
			}

			if (m_month.SelectedMonth.Month != m_date.Month)
			{
				bgColor1 = m_month.Colors.Trailing.BackColor1;
				bgColor2 = m_month.Colors.Trailing.BackColor2;
				gradientMode = m_month.Colors.Trailing.GradientMode;
				dateColor = m_month.Colors.Trailing.Date;
				textColor = m_month.Colors.Trailing.Text;
			}

			// Check if formatting should be applied
			if ((m_month.FormatTrailing) || (m_month.SelectedMonth.Month == m_date.Month))
			{
				// check of there is formatting for this day
				if (queryInfo != null)
				{
					info = new NuGenDateItem[1];
					info[0] = queryInfo;
				}
				else
					info = m_calendar.GetDateInfo(this.Date);
				if (info.Length > 0)
					i = 0;
				// go through the available dateitems
				while ((i < info.Length) && (drawDay))
				{
					if (info.Length > 0)
					{
						NuGenDateItem dateInfo = info[i];

						if (dateInfo.BackColor1 != Color.Empty)
							bgColor1 = dateInfo.BackColor1;
						if (dateInfo.BackColor2 != Color.Empty)
							bgColor2 = dateInfo.BackColor2;
						gradientMode = dateInfo.GradientMode;
						if (dateInfo.DateColor != Color.Empty)
							dateColor = dateInfo.DateColor;
						if (dateInfo.TextColor != Color.Empty)
							textColor = dateInfo.TextColor;
						text = dateInfo.Text;

						if (dateInfo.Weekend)
						{
							bgColor1 = m_month.Colors.Weekend.BackColor1;
							bgColor2 = m_month.Colors.Weekend.BackColor2;
							gradientMode = m_month.Colors.Weekend.GradientMode;
							dateColor = m_month.Colors.Weekend.Date;
							textColor = m_month.Colors.Weekend.Text;
						}
						boldedDate = dateInfo.BoldedDate;
						enabled = dateInfo.Enabled;
						if (!dateInfo.Enabled)
						{
							bgColor1 = m_month.Colors.Disabled.BackColor1;
							bgColor2 = m_month.Colors.Disabled.BackColor2;
							gradientMode = m_month.Colors.Disabled.GradientMode;
							dateColor = m_month.Colors.Disabled.Date;
							textColor = m_month.Colors.Disabled.Text;
						}

						m_dayImage = dateInfo.Image;

						if (m_dayImage != null)
							m_imageRect = ImageRect(m_month.ImageAlign);

						bgImage = dateInfo.BackgroundImage;
					}

					if (m_state == NuGenDayState.Selected)
					{
						dateColor = m_month.Colors.Selected.Date;
						textColor = m_month.Colors.Selected.Text;
					}
					if ((m_state == NuGenDayState.Focus) && (m_month.Calendar.ShowFocus))
					{
						dateColor = m_month.Colors.Focus.Date;
						textColor = m_month.Colors.Focus.Text;
					}


					if (bgImage != null)
						e.DrawImage(bgImage, m_rect);
					else
					{
						if (gradientMode == NuGenGradientMode.None)
						{
							if (bgColor1 != Color.Transparent)
							{
								bgBrush = new SolidBrush(Color.FromArgb(m_month.Transparency.Background, bgColor1));
								e.FillRectangle(bgBrush, m_rect);
							}
						}
						else
							m_calendar.DrawGradient(e, Rectangle, bgColor1, bgColor2, gradientMode);
					}


					ControlPaint.DrawBorder(e, m_rect, m_month.Colors.Days.Border, m_month.BorderStyles.Normal);
					if (m_dayImage != null)
					{
						if (enabled)
							e.DrawImageUnscaled(m_dayImage, m_imageRect);
						else
							ControlPaint.DrawImageDisabled(e, m_dayImage, m_imageRect.X, m_imageRect.Y, m_month.Colors.Disabled.BackColor1);
					}

					// Check if we should append month name to date
					if ((m_month.ShowMonthInDay) &&
						((m_date.AddDays(-1).Month != m_date.Month) ||
						(m_date.AddDays(1).Month != m_date.Month)))
						dateString = m_date.Day.ToString() + " " + m_calendar._dateTimeFormat.GetMonthName(m_date.Month);
					else
						dateString = m_date.Day.ToString();

					if (dateColor != Color.Transparent)
					{
						dateBrush = new SolidBrush(Color.FromArgb(m_month.Transparency.Text, dateColor));
						CharacterRange[] characterRanges = { new CharacterRange(0, dateString.Length) };
						dateAlign.SetMeasurableCharacterRanges(characterRanges);
						m_dateRgn = new Region[1];
						// Should date be bolded ?
						if (!boldedDate)
						{
							e.DrawString(dateString, m_month.DateFont, dateBrush, m_rect, dateAlign);
							m_dateRgn = e.MeasureCharacterRanges(dateString, m_month.DateFont, m_rect, dateAlign);
						}
						else
						{
							e.DrawString(dateString, boldFont, dateBrush, m_rect, dateAlign);
							m_dateRgn = e.MeasureCharacterRanges(dateString, boldFont, m_rect, dateAlign);
						}

					}
					if ((text.Length > 0) && (textColor != Color.Transparent))
					{
						textBrush = new SolidBrush(Color.FromArgb(m_month.Transparency.Text, textColor));
						CharacterRange[] characterRanges = { new CharacterRange(0, text.Length) };
						textAlign.SetMeasurableCharacterRanges(characterRanges);
						m_textRgn = new Region[1];
						e.DrawString(text, m_month.TextFont, textBrush, m_rect, textAlign);
						m_textRgn = e.MeasureCharacterRanges(text, m_month.TextFont, m_rect, textAlign);
					}
					i++;
				}
			}

			dateBrush.Dispose();
			bgBrush.Dispose();
			textBrush.Dispose();
			boldFont.Dispose();
			dateAlign.Dispose();
			textAlign.Dispose();
		}

		internal bool ImageHitTest(Point p)
		{

			bool status = false;
			if ((!m_userDrawn) && (m_dayImage != null) && (Month.EnableImageClick))
			{
				Region r = new Region(m_imageRect);
				if (r.IsVisible(p))
					status = true;
				else
					status = false;
				r.Dispose();
			}

			return status;

		}

		internal bool TextHitTest(Point p)
		{

			bool status = false;
			if ((!m_userDrawn) && (m_textRgn != null))
			{
				if (m_textRgn[0].IsVisible(p))
					status = true;
				else
					status = false;
			}

			return status;

		}

		internal bool DateHitTest(Point p)
		{
			bool status = false;
			if ((!m_userDrawn) && (m_textRgn != null))
			{
				if (m_dateRgn[0].IsVisible(p))
					status = true;
				else
					status = false;
			}

			return status;
		}

		internal bool HitTest(Point p)
		{

			if (ImageHitTest(p))
				m_calendar.Cursor = Cursors.Hand;
			else
				m_calendar.Cursor = Cursors.Arrow;



			if (m_region.IsVisible(p))
				return true;
			else
				return false;
		}


		#endregion

		#region Methods.Private

		private Image GetImage(int index)
		{
			// Check that an ImageList exists and that index is valid
			if (m_month.Calendar.ImageList != null)
			{
				if ((index >= 0) && (index < m_month.Calendar.ImageList.Images.Count))
				{
					return m_month.Calendar.ImageList.Images[index];
				}
				else return null;
			}

			else return null;
		}

		private Rectangle ImageRect(ContentAlignment align)
		{
			Rectangle imageRect = new Rectangle(0, 0, m_rect.Width, m_rect.Height);

			switch (align)
			{

				case ContentAlignment.MiddleLeft:
				{
					imageRect.X = m_rect.X + 2;
					imageRect.Y = m_rect.Top + ((m_rect.Height / 2) - (m_dayImage.Height / 2));
					break;
				}
				case ContentAlignment.MiddleRight:
				{
					imageRect.X = m_rect.Right - 2 - m_dayImage.Width;
					imageRect.Y = m_rect.Top + ((m_rect.Height / 2) - (m_dayImage.Height / 2));
					break;
				}
				case ContentAlignment.TopCenter:
				{
					imageRect.X = m_rect.X + ((m_rect.Width / 2) - (m_dayImage.Width / 2));
					imageRect.Y = m_rect.Y + 2;
					break;
				}
				case ContentAlignment.BottomCenter:
				{
					imageRect.X = m_rect.X + ((m_rect.Width / 2) - (m_dayImage.Width / 2));
					imageRect.Y = m_rect.Bottom - 2 - m_dayImage.Height;
					break;
				}

				case ContentAlignment.TopLeft:
				{
					imageRect.X = m_rect.X + 2;
					imageRect.Y = m_rect.Y + 2;
					break;
				}
				case ContentAlignment.TopRight:
				{
					imageRect.X = m_rect.Right - 2 - m_dayImage.Width;
					imageRect.Y = m_rect.Y + 2;
					break;
				}
				case ContentAlignment.MiddleCenter:
				{
					imageRect.X = m_rect.X + ((m_rect.Width / 2) - (m_dayImage.Width / 2));
					imageRect.Y = m_rect.Top + ((m_rect.Height / 2) - (m_dayImage.Height / 2));
					break;
				}
				case ContentAlignment.BottomLeft:
				{
					imageRect.X = m_rect.X + 2;
					imageRect.Y = m_rect.Bottom - 2 - m_dayImage.Height;
					break;
				}
				case ContentAlignment.BottomRight:
				{
					imageRect.X = m_rect.Right - 2 - m_dayImage.Width;
					imageRect.Y = m_rect.Bottom - 2 - m_dayImage.Height;
					break;
				}
			}

			imageRect.Height = m_dayImage.Height;
			imageRect.Width = m_dayImage.Width;
			return imageRect;
		}

		#endregion

		private bool disposed;
		private Rectangle m_rect;
		private Region m_region;
		private DateTime m_date;
		private NuGenCalendar m_calendar;
		private NuGenMonth m_month;
		private Image m_dayImage;
		private int m_selection;
		private bool m_userDrawn;
		private Rectangle m_imageRect;
        private Region[] m_dateRgn;
        private Region[] m_textRgn;
    
		private NuGenDayState m_state;
		         
        public Day()
		{
			m_state = NuGenDayState.Normal; 
			m_selection = -1;
			m_userDrawn = false;
    	}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		
		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					m_region.Dispose();
					m_dayImage.Dispose();
				
				}
				// shared cleanup logic
				disposed = true;
			}
		}
	}
}
