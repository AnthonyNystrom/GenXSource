/* -----------------------------------------------
 * NuGenDateItem.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.CalendarInternals;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	[TypeConverter(typeof(NuGenDateItemConverter))]
	[DesignTimeVisible(false)]
	[ToolboxItem(false)]
	public class NuGenDateItem : IComponent
	{
		#region Properties.Public

		/// <summary>
		/// </summary>
		[Browsable(false)]
		public object Tag
		{
			set
			{
				if (value != m_tag)
					m_tag = value;
			}
			get
			{
				return m_tag;
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(false)]
		public virtual ISite Site
		{
			get
			{
				return m_site;
			}
			set
			{
				m_site = value;
			}
		}

		/// <summary>
		/// </summary>
		[Description("Indicates the range of the recurrence.")]
		[Category("Recurrence")]
		public DateTime Range
		{
			get
			{
				return m_rangeDate;
			}
			set
			{
				if (m_rangeDate != value)
				{
					m_rangeDate = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		[Description("Indicates the recurrence of the info.")]
		[Category("Recurrence")]
		public NuGenRecurrence Pattern
		{
			get
			{
				return m_pattern;
			}
			set
			{
				if (m_pattern != value)
				{
					m_pattern = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		[Description("The day for which the formatting applies.")]
		[Category("Ocurrence")]
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
					m_rangeDate = m_date;
				}
			}
		}

		/// <summary>
		/// </summary>
		[Category("Color")]
		[Description("Background color assigned to this day.")]
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
				}
			}
		}

		/// <summary>
		/// </summary>
		[Category("Color")]
		[Description("Second background color when using a gradient.")]
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
				}
			}
		}

		/// <summary>
		/// </summary>
		[Category("Color")]
		[Description("Type of gradient used.")]
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
				}
			}
		}

		/// <summary>
		/// </summary>
		[Category("Behavior")]
		[Description("Indicates wether the date should be treated as a weekend.")]
		public bool Weekend
		{
			get
			{
				return m_weekend;
			}
			set
			{
				if (m_weekend != value)
				{
					m_weekend = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		[Category("Behavior")]
		[Description("Indicates wether the date is enabled i.e. selectable.")]
		public bool Enabled
		{
			get
			{
				return m_enabled;
			}
			set
			{
				if (m_enabled != value)
				{
					m_enabled = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		[Category("Appearance")]
		[Description("Indicates wether bold font should be used for the date.")]
		public bool BoldedDate
		{
			get
			{
				return m_bolded;
			}
			set
			{
				if (m_bolded != value)
				{
					m_bolded = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		[Category("Color")]
		[Description("Color used for date.")]
		public Color DateColor
		{
			get
			{
				return m_dateColor;
			}
			set
			{
				if (m_dateColor != value)
				{
					m_dateColor = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		[Category("Color")]
		[Description("Color used for text.")]
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
				}
			}
		}

		/// <summary>
		/// </summary>
		[Category("Appearance")]
		[Description("Text to be displayed for day.")]
		public string Text
		{
			get
			{
				return m_text;
			}
			set
			{
				if (m_text != value)
				{
					m_text = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		[Category("Appearance")]
		[Description("Index for the image assigned to this date.")]
		[DefaultValue(-1)]
		[Editor("Genetibase.Shared.Design.NuGenImageIndexEditor", typeof(UITypeEditor))]
		[RefreshProperties(RefreshProperties.Repaint)]
		[RelatedImageList("Calendar.ImageList")]
		[TypeConverter(typeof(TreeViewImageIndexConverter))]
		public int ImageListIndex
		{
			get
			{
				return m_imageIndex;
			}
			set
			{
				if (m_imageIndex != value)
				{
					m_image = null;
					m_imageIndex = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		[Category("Appearance")]
		[Description("Image used as background.")]
		public Image BackgroundImage
		{
			get
			{
				return m_bgImage;
			}
			set
			{
				if (m_bgImage != value)
				{
					m_bgImage = value;
				}
			}
		}

		/// <summary>
		/// </summary>
		[Category("Appearance")]
		[Description("Image assigned to this date.")]
		[Browsable(true)]
		public Image Image
		{
			get
			{
				if (m_image != null) return m_image;

				if ((GetImageList() != null) && (m_imageIndex != -1))
				{
					try
					{
						return GetImageList().Images[m_imageIndex];
					}
					catch (Exception)
					{
						return null;
					}
				}
				else return null;
			}
			set
			{
				m_image = value;
			}

		}

		#endregion

		#region Properties.NonBrowsable

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public NuGenCalendar Calendar
		{
			set
			{
				m_calendar = value;
			}
			get
			{
				return m_calendar;
			}
		}

		#endregion

		#region Properties.Internal

		internal ImageList GetImageList()
		{
			if (m_calendar != null)
				return m_calendar.ImageList;
			else
				return null;
		}

		internal int Index
		{
			set
			{
				m_index = value;
			}
			get
			{
				return m_index;
			}
		}

		#endregion

		private DateTime m_date;
		private DateTime m_rangeDate;

		private bool disposed;
		private Color m_backColor1;
		private Color m_backColor2;
		private NuGenGradientMode m_gradientMode;
		private Color m_dateColor;
		private Color m_textColor;
		private string m_text;
		private int m_imageIndex;
		private Image m_image;
		private bool m_weekend;
		private bool m_enabled;
		private bool m_bolded;
		private ISite m_site;
		private NuGenCalendar m_calendar;
		private NuGenRecurrence m_pattern;
		private int m_index;
		private Image m_bgImage;
		private object m_tag;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDateItem"/> class.
		/// </summary>
		public NuGenDateItem()
		{
			m_imageIndex = -1;
			m_backColor1 = Color.Empty;
			m_backColor2 = Color.White;
			m_dateColor = Color.Empty;
			m_textColor = Color.Empty;
			m_gradientMode = NuGenGradientMode.None;
			m_text = "";
			m_enabled = true;
			m_bgImage = null;
			m_pattern = NuGenRecurrence.None;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Occurs when this <see cref="NuGenDateItem"/> was disposed.
		/// </summary>
		public event EventHandler Disposed;

		/// <summary>
		/// </summary>
		/// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{

					if (m_image != null)
						m_image.Dispose();
					if (m_bgImage != null)
						m_bgImage.Dispose();

					if (Disposed != null)
						Disposed(this, EventArgs.Empty);
				}

				disposed = true;
			}
		}
	}
}
