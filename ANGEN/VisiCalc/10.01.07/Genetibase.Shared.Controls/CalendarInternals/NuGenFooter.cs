/* -----------------------------------------------
 * NuGenFooter.cs
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
	[TypeConverter(typeof(NuGenFooterTypeConverter))]
	public class NuGenFooter : IDisposable
	{
		#region private class members

		private bool disposed;
		private NuGenCalendar m_calendar;
		private Color m_backColor1;
		private Color m_backColor2;
		private NuGenGradientMode m_gradientMode;
		private Color m_textColor;
		private Font m_font;
		private NuGenTodayFormat m_format;
		private string m_text;
		private bool m_showToday;
		private Rectangle m_rect;
		private Region m_region;
		private StringAlignment m_align;
		#endregion

		#region EventHandlers

		internal event EventHandler<NuGenClickEventArgs> Click;
		internal event EventHandler<NuGenClickEventArgs> DoubleClick;
		internal event EventHandler<NuGenFooterPropertyEventArgs> PropertyChanged;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenFooter"/> class.
		/// </summary>
		public NuGenFooter(NuGenCalendar calendar)
		{
			m_calendar = calendar;
			m_backColor1 = Color.White;
			m_backColor2 = Color.White;
			m_gradientMode = NuGenGradientMode.None;
			m_textColor = Color.Black;
			m_font = new Font("Microsoft Sans Serif", (float)8.25, FontStyle.Bold);
			m_format = NuGenTodayFormat.Short;
			m_text = "";
			m_showToday = true;
			m_align = StringAlignment.Near;
		}

		#endregion

		#region Methods

		internal void MouseMove(Point mouseLocation)
		{
			if (m_region.IsVisible(mouseLocation))
			{
				m_calendar.ActiveRegion = NuGenCalendarRegion.Footer;
			}
		}

		internal void MouseClick(Point mouseLocation, MouseButtons button, NuGenClickMode mode)
		{
			if (m_region.IsVisible(mouseLocation))
			{
				if (mode == NuGenClickMode.Single)
				{
					if (this.Click != null)
						this.Click(this, new NuGenClickEventArgs(button));
				}
				else
				{
					if (this.DoubleClick != null)
						this.DoubleClick(this, new NuGenClickEventArgs(button));
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
			Brush textBrush = new SolidBrush(TextColor);
			Brush bgBrush = new SolidBrush(BackColor1);
			textFormat.Alignment = StringAlignment.Near;
			textFormat.LineAlignment = StringAlignment.Center;
			Rectangle txtRect;

			if (m_gradientMode == NuGenGradientMode.None)
				e.FillRectangle(bgBrush, m_rect);
			else
				m_calendar.DrawGradient(e, m_rect, m_backColor1, m_backColor2, m_gradientMode);

			textFormat.LineAlignment = StringAlignment.Center;
			textFormat.Alignment = Align;

			txtRect = new Rectangle(m_rect.Left + 2, m_rect.Top, m_rect.Width - (2 * 2), m_rect.Height);

			if (m_showToday)
			{
				if (m_format == NuGenTodayFormat.Short)
					e.DrawString(DateTime.Now.ToShortDateString(), Font, textBrush, txtRect, textFormat);
				else
					e.DrawString(DateTime.Now.ToLongDateString(), Font, textBrush, txtRect, textFormat);

			}
			else
				e.DrawString(m_text, Font, textBrush, txtRect, textFormat);

			// Clean up
			textBrush.Dispose();
			bgBrush.Dispose();
		}

		#endregion

		#region Dispose

		/// <summary>
		/// </summary>
		/// <param name="disposing"><see langword="true"/> to dispose both managed and unmanaged resources; <see langword="false"/> to release onlyl unmanaged resources.</param>
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
		[Description("Determines the position for the text.")]
		[DefaultValue(StringAlignment.Near)]
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
						PropertyChanged(this, new NuGenFooterPropertyEventArgs(NuGenFooterProperty.Align));
					m_calendar.Invalidate();
				}
			}
		}

		/// <summary>
		/// </summary>
		[Description("Text to be displayed in the footer.")]
		[DefaultValue("")]
		public string Text
		{
			get
			{
				return m_text;
			}
			set
			{
				if (value != m_text)
				{
					m_text = value;
					if (PropertyChanged != null)
						PropertyChanged(this, new NuGenFooterPropertyEventArgs(NuGenFooterProperty.Text));
					m_calendar.Invalidate();
				}
			}

		}

		/// <summary>
		/// </summary>
		[Description("Determines wether todays date should be displayed.")]
		[DefaultValue(true)]
		public bool ShowToday
		{
			get
			{
				return m_showToday;
			}
			set
			{
				if (value != m_showToday)
				{
					m_showToday = value;
					if (PropertyChanged != null)
						PropertyChanged(this, new NuGenFooterPropertyEventArgs(NuGenFooterProperty.ShowToday));
					m_calendar.Invalidate();
				}
			}

		}

		/// <summary>
		/// </summary>
		[Description("Determines the format used to display todays date.")]
		[DefaultValue(NuGenTodayFormat.Short)]
		public NuGenTodayFormat Format
		{
			get
			{
				return m_format;
			}
			set
			{
				if (value != m_format)
				{
					m_format = value;
					if (PropertyChanged != null)
						PropertyChanged(this, new NuGenFooterPropertyEventArgs(NuGenFooterProperty.Format));
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
						PropertyChanged(this, new NuGenFooterPropertyEventArgs(NuGenFooterProperty.BackColor1));
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
						PropertyChanged(this, new NuGenFooterPropertyEventArgs(NuGenFooterProperty.BackColor2));
					m_calendar.Invalidate();
				}
			}
		}

		/// <summary>
		/// </summary>
		[Description("Type of gradient used.")]
		[DefaultValue(NuGenGradientMode.None)]
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
						PropertyChanged(this, new NuGenFooterPropertyEventArgs(NuGenFooterProperty.GradientMode));
					m_calendar.Invalidate();
				}
			}
		}

		/// <summary>
		/// </summary>
		[Description("Font used for footer.")]
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
						PropertyChanged(this, new NuGenFooterPropertyEventArgs(NuGenFooterProperty.Font));
					m_calendar.Invalidate();
				}
			}
		}

		/// <summary>
		/// </summary>
		[Description("Color used for text.")]
		[DefaultValue(typeof(Color), "Black")]
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
						PropertyChanged(this, new NuGenFooterPropertyEventArgs(NuGenFooterProperty.Text));
					m_calendar.Invalidate();
				}
			}
		}

		#endregion
	}
}
