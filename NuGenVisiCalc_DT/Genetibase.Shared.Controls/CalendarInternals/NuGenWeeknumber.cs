/* -----------------------------------------------
 * Weeknumber.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.CalendarInternals
{
	/// <summary>
	/// </summary>
	[TypeConverter(typeof(NuGenWeeknumberConverter))]
	public class NuGenWeeknumber : IDisposable
	{
		/// <summary>
		/// </summary>
		internal event EventHandler<NuGenWeeknumberClickEventArgs> Click;

		/// <summary>
		/// </summary>
		internal event EventHandler<NuGenWeeknumberClickEventArgs> DoubleClick;

		/// <summary>
		/// </summary>
		internal event EventHandler<NuGenWeeknumberPropertyEventArgs> PropertyChanged;
		
		private bool disposed;
		private NuGenCalendar m_calendar;
		private Color m_backColor1;
        private Color m_backColor2;
        private NuGenGradientMode m_gradientMode;
		private Color m_textColor;
		private Color m_borderColor;
		private Font m_font;
		private Rectangle m_rect;
		private Region m_region;
        private NuGenWeeknumberAlign m_align; 

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenWeeknumber"/> class.
		/// </summary>
		public NuGenWeeknumber(NuGenCalendar calendar)
		{
			m_calendar = calendar;
			m_backColor1 = Color.White;
            m_backColor2 = Color.White;
            m_gradientMode = NuGenGradientMode.None;
            m_textColor = Color.FromArgb(0,84,227); 
			m_borderColor = Color.Black;
			m_font = new Font("Microsoft Sans Serif",(float)8.25);
            m_align = NuGenWeeknumberAlign.Top; 	
		}

		#endregion
		
		#region Dispose
		
		/// <summary>
		/// </summary>
		/// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
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

		#endregion

		#region Properties

		/// <summary>
		/// </summary>
		[Description("Color used border.")]
		[DefaultValue(typeof(Color),"Black")]
		public Color BorderColor
		{
			get
			{
				return m_borderColor;
			}
			set
			{
				if (m_borderColor!=value)
				{
					m_borderColor = value;
					if (PropertyChanged!=null)
						PropertyChanged(this,new NuGenWeeknumberPropertyEventArgs(NuGenWeeknumberProperty.BorderColor)); 
					m_calendar.Invalidate();
				}
			}
		}

		/// <summary>
		/// </summary>
        [Description("Determines the position for the text.")]
        [DefaultValue(typeof(NuGenWeeknumberAlign), "Top")]
        public NuGenWeeknumberAlign Align
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
                        PropertyChanged(this, new NuGenWeeknumberPropertyEventArgs(NuGenWeeknumberProperty.Align));
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
		[Description("Color used for background.")]
		[DefaultValue(typeof(Color),"White")]
		public Color BackColor1
		{
			get
			{
				return m_backColor1;
			}
			set
			{
				if (m_backColor1!=value)
				{
					m_backColor1 = value;
					if (PropertyChanged!=null)
						PropertyChanged(this,new NuGenWeeknumberPropertyEventArgs(NuGenWeeknumberProperty.BackColor1)); 
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
                        PropertyChanged(this, new NuGenWeeknumberPropertyEventArgs(NuGenWeeknumberProperty.BackColor2));
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
                        PropertyChanged(this, new NuGenWeeknumberPropertyEventArgs(NuGenWeeknumberProperty.GradientMode));
                    m_calendar.Invalidate();
                }
            }
        }

		/// <summary>
		/// </summary>
		[Description("Font used for week numbers.")]
		[DefaultValue(typeof(Font),"Microsoft Sans Serif; 8,25pt")]
		public Font Font
		{
			get
			{
				return m_font;
			}
			set
			{
				if (m_font!=value)
				{
					m_font = value;
					m_calendar.DoLayout();
					if (PropertyChanged!=null)
						PropertyChanged(this,new NuGenWeeknumberPropertyEventArgs(NuGenWeeknumberProperty.Font)); 
					m_calendar.Invalidate();
				}
			}
		}

		/// <summary>
		/// </summary>
		[Description("Color used for text.")]
		[DefaultValue(typeof(Color),"0,84,227")]
		public Color TextColor
		{
			get
			{
				return m_textColor;
			}
			set
			{
				if (m_textColor!=value)
				{
					m_textColor = value;
					if (PropertyChanged!=null)
						PropertyChanged(this,new NuGenWeeknumberPropertyEventArgs(NuGenWeeknumberProperty.TextColor)); 
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
				m_calendar.ActiveRegion = NuGenCalendarRegion.Weeknumbers;  
			}
		}

		internal void MouseClick(Point mouseLocation,MouseButtons button, NuGenClickMode mode)
		{
			GregorianCalendar gCalendar = new GregorianCalendar();
	
			if (m_region.IsVisible(mouseLocation))
			{
				int week = 0;
				
				int i = ((mouseLocation.Y-m_rect.Top) / (int)m_calendar.Month.DayHeight);				
				week = gCalendar.GetWeekOfYear(m_calendar.Month.m_days[i*7].Date,m_calendar._dateTimeFormat.CalendarWeekRule,m_calendar._dateTimeFormat.FirstDayOfWeek);
				if (mode == NuGenClickMode.Single)
				{
					if (this.Click!=null)
						this.Click(this,new NuGenWeeknumberClickEventArgs(week,button));
				}
				else
				{
					if (this.DoubleClick!=null)
						this.DoubleClick(this,new NuGenWeeknumberClickEventArgs(week,button));
				}
		
			}
		}

		internal bool IsVisible(Rectangle clip)
		{
			return m_region.IsVisible(clip); 	
		}

		internal void Draw(Graphics e)
		{
			StringFormat textFormat = new StringFormat();
			Pen linePen = new Pen(m_borderColor,1);
			Rectangle weekRect = new Rectangle();
			 
			int weeknr=0;	
			Brush weekBrush = new SolidBrush(this.BackColor1);
			Brush weekTextBrush = new SolidBrush(this.TextColor); 
			int dayHeight;
			
			// Draw header
            textFormat.Alignment = StringAlignment.Center;
		    switch (m_align)
            {
                case NuGenWeeknumberAlign.Top:
                {

                    textFormat.LineAlignment = StringAlignment.Near;
                    break;
                }
                case NuGenWeeknumberAlign.Center:
                {

                    textFormat.LineAlignment = StringAlignment.Center;
                    break;
                }
                case NuGenWeeknumberAlign.Bottom:
                {

                    textFormat.LineAlignment = StringAlignment.Far;
                    break;
                }



            }

            if (m_gradientMode == NuGenGradientMode.None)
                e.FillRectangle(weekBrush, m_rect);
            else
                m_calendar.DrawGradient(e, m_rect, m_backColor1, m_backColor2, m_gradientMode);  
			
			dayHeight = (int)m_calendar.Month.DayHeight; 			
			for (int i = 0;i<6;i++)
			{
				weekRect.Y = m_rect.Y + dayHeight*i;
                weekRect.Y += (i+1)* m_calendar.Month.Padding.Vertical;    
                weekRect.Width = m_rect.Width; 
				weekRect.X =0;
                if (i == 5)
                    weekRect.Height = m_rect.Height - (m_calendar.Month.Padding.Vertical*7) - (int)(dayHeight*5)-1;
                else
                    weekRect.Height = dayHeight;
				
				weeknr = GetWeek(m_calendar.Month.m_days[i*7].Date);
				
				e.DrawString(weeknr.ToString(),this.Font,weekTextBrush,weekRect,textFormat);
					  
			}
			e.DrawLine(linePen,m_rect.Right-1,m_rect.Top,m_rect.Right-1,m_rect.Bottom); 
			// tidy up
			weekBrush.Dispose(); 
			weekTextBrush.Dispose();
			linePen.Dispose(); 
			
		}

		internal int GetWeek(DateTime dt)
		{
			int weeknr;
					
			try
			{
				// retrieve week by calling weeknumber callback
				weeknr = m_calendar.WeeknumberCallBack(dt);	
			}
			catch(Exception)
			{
				//if callback fails , call CalcWeek 
				weeknr = CalcWeek(dt);		
			}
			return weeknr;
		}

		internal int CalcWeek(DateTime dt)
		{
			int weeknr = 0;
			GregorianCalendar gCalendar = new GregorianCalendar();

			if ((m_calendar._dateTimeFormat.CalendarWeekRule == CalendarWeekRule.FirstFourDayWeek) &&
				(m_calendar._dateTimeFormat.FirstDayOfWeek == DayOfWeek.Monday))
				// Get ISO week
				weeknr = GetISO8601Weeknumber(dt); 	
			else
				// else get Microsoft week
				weeknr = gCalendar.GetWeekOfYear(dt,m_calendar._dateTimeFormat.CalendarWeekRule, m_calendar._dateTimeFormat.FirstDayOfWeek);
			
			return weeknr;
		}

        private int GetISO8601Weeknumber(DateTime dt)
        {
            DateTime week1;
            int IsoYear = dt.Year;
            int IsoWeek;
            if (dt >= new DateTime(IsoYear, 12, 29))
            {
                week1 = GetIsoWeekOne(IsoYear + 1);
                if (dt < week1)
                {
                    week1 = GetIsoWeekOne(IsoYear);
                }
                else
                {
                    IsoYear++;
                }
            }
            else
            {
                week1 = GetIsoWeekOne(IsoYear);
                if (dt < week1)
                {
                    week1 = GetIsoWeekOne(--IsoYear);
                }
            }

            IsoWeek = (IsoYear * 100) + ((dt - week1).Days / 7 + 1);
            return IsoWeek % 100;
        }

        private DateTime GetIsoWeekOne(int Year)
        {
            // get the date for the 4-Jan for this year
            DateTime dt = new DateTime(Year, 1, 4);

            // get the ISO day number for this date 1==Monday, 7==Sunday
            int dayNumber = (int)dt.DayOfWeek; // 0==Sunday, 6==Saturday
            if (dayNumber == 0)
            {
                dayNumber = 7;
            }

            // return the date of the Monday that is less than or equal
            // to this date
            return dt.AddDays(1 - dayNumber);
        }

		#endregion
	}
}
