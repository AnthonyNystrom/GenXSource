/* -----------------------------------------------
 * NuGenCalendar.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.CalendarInternals;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.Properties;
using Genetibase.Shared.Controls.ScrollBarInternals;
using Genetibase.Shared.Windows;
using Genetibase.WinApi;

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Runtime.Serialization;
using System.Globalization;
using System.Threading;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms.VisualStyles;
using System.Resources;
using System.Text;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	[ToolboxItem(false)]
	[DefaultEvent("MonthChanged")]
	[DefaultProperty("Name")]
	[Designer("Genetibase.Shared.Controls.Design.NuGenCalendarDesigner")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenCalendar : NuGenControl
	{
		#region Events

		/*
		 * CalendarColorChanged
		 */

		private static readonly object _calendarColorChanged = new object();

		/// <summary>
		/// Occurs when a calendar color changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Calendar_CalendarColorChanged")]
		public event EventHandler<NuGenCalendarColorEventArgs> CalendarColorChanged
		{
			add
			{
				this.Events.AddHandler(_calendarColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_calendarColorChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="CalendarColorChanged"/> event.
		/// </summary>
		protected virtual void OnCalendarColorChanged(NuGenCalendarColorEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenCalendarColorEventArgs>(_calendarColorChanged, e);
		}

		/*
		 * DayRender
		 */

		private static readonly object _dayRender = new object();

		/// <summary>
		/// Occurs when a day is drawn.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_DayRender")]
		public event EventHandler<NuGenDayRenderEventArgs> DayRender
		{
			add
			{
				this.Events.AddHandler(_dayRender, value);
			}
			remove
			{
				this.Events.RemoveHandler(_dayRender, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="DayRender"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnDayRender(NuGenDayRenderEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenDayRenderEventArgs>(_dayRender, e);
		}

		/*
		 * DayQueryInfo
		 */

		private static readonly object _dayQueryInfo = new object();

		/// <summary>
		/// Occurs when a day is about to be drawn.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_DayQueryInfo")]
		public event EventHandler<NuGenDayQueryInfoEventArgs> DayQueryInfo
		{
			add
			{
				this.Events.AddHandler(_dayQueryInfo, value);
			}
			remove
			{
				this.Events.RemoveHandler(_dayQueryInfo, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="DayQueryInfo"/> event. 
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnDayQueryInfo(NuGenDayQueryInfoEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenDayQueryInfoEventArgs>(_dayQueryInfo, e);
		}

		/*
		 * DayDragDrop
		 */

		private static readonly object _dayDragDrop = new object();

		/// <summary>
		/// Occurs when a drag-n-drop operation is completed.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_DayDragDrop")]
		public event EventHandler<NuGenDayDragDropEventArgs> DayDragDrop
		{
			add
			{
				this.Events.AddHandler(_dayDragDrop, value);
			}
			remove
			{
				this.Events.RemoveHandler(_dayDragDrop, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="DayDragDrop"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnDayDragDrop(NuGenDayDragDropEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenDayDragDropEventArgs>(_dayDragDrop, e);
		}

		/*
		 * MonthChanged
		 */

		private static readonly object _monthChanged = new object();

		/// <summary>
		/// Occurs when a month is changed.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_MonthChanged")]
		public event EventHandler<NuGenMonthChangedEventArgs> MonthChanged
		{
			add
			{
				this.Events.AddHandler(_monthChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_monthChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="MonthChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnMonthChanged(NuGenMonthChangedEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenMonthChangedEventArgs>(_monthChanged, e);
		}

		/*
		 * BeforeMonthChanged
		 */

		private static readonly object _beforeMonthChanged = new object();

		/// <summary>
		/// Occurs when a month is about to change.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_BeforeMonthChanged")]
		public event EventHandler<NuGenBeforeMonthChangedEventArgs> BeforeMonthChanged
		{
			add
			{
				this.Events.AddHandler(_beforeMonthChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_beforeMonthChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="BeforeMonthChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnBeforeMonthChanged(NuGenBeforeMonthChangedEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenBeforeMonthChangedEventArgs>(_beforeMonthChanged, e);
		}

		/*
		 * ImageClick
		 */

		private static readonly object _imageClick = new object();

		/// <summary>
		/// Occurs when an image is clicked.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_ImageClick")]
		public event EventHandler<NuGenDayClickEventArgs> ImageClick
		{
			add
			{
				this.Events.AddHandler(_imageClick, value);
			}
			remove
			{
				this.Events.RemoveHandler(_imageClick, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="ImageClick"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnImageClick(NuGenDayClickEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenDayClickEventArgs>(_imageClick, e);
		}

		/*
		 * DayMouseMove
		 */

		private static readonly object _dayMouseMove = new object();

		/// <summary>
		/// Occurs when the mouse pointer is moved over the day.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_DayMouseMove")]
		public event EventHandler<NuGenDayMouseMoveEventArgs> DayMouseMove
		{
			add
			{
				this.Events.AddHandler(_dayMouseMove, value);
			}
			remove
			{
				this.Events.RemoveHandler(_dayMouseMove, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="DayMouseMove"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnDayMouseMove(NuGenDayMouseMoveEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenDayMouseMoveEventArgs>(_dayMouseMove, e);
		}

		/*
		 * DayClick
		 */

		private static readonly object _dayClick = new object();

		/// <summary>
		/// Occurs when a day is clicked.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_DayClick")]
		public event EventHandler<NuGenDayClickEventArgs> DayClick
		{
			add
			{
				this.Events.AddHandler(_dayClick, value);
			}
			remove
			{
				this.Events.RemoveHandler(_dayClick, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="DayClick"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnDayClick(NuGenDayClickEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenDayClickEventArgs>(_dayClick, e);
		}

		/*
		 * DayDoubleClick
		 */

		private static readonly object _dayDoubleClick = new object();

		/// <summary>
		/// Occurs when a day is double-clicked.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_DayDoubleClick")]
		public event EventHandler<NuGenDayClickEventArgs> DayDoubleClick
		{
			add
			{
				this.Events.AddHandler(_dayDoubleClick, value);
			}
			remove
			{
				this.Events.RemoveHandler(_dayDoubleClick, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="DayDoubleClick"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnDayDoubleClick(NuGenDayClickEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenDayClickEventArgs>(_dayDoubleClick, e);
		}

		/*
		 * HeaderClick
		 */

		private static readonly object _headerClick = new object();

		/// <summary>
		/// Occurs when the header is clicked.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_HeaderClick")]
		public event EventHandler<NuGenClickEventArgs> HeaderClick
		{
			add
			{
				this.Events.AddHandler(_headerClick, value);
			}
			remove
			{
				this.Events.RemoveHandler(_headerClick, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="HeaderClick"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnHeaderClick(NuGenClickEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenClickEventArgs>(_headerClick, e);
		}

		/*
		 * HeaderDoubleClick
		 */

		private static readonly object _headerDoubleClick = new object();

		/// <summary>
		/// Occurs when the header is double-clicked.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_HeaderDoubleClick")]
		public event EventHandler<NuGenClickEventArgs> HeaderDoubleClick
		{
			add
			{
				this.Events.AddHandler(_headerDoubleClick, value);
			}
			remove
			{
				this.Events.RemoveHandler(_headerDoubleClick, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="HeaderDoubleClick"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnHeaderDoubleClick(NuGenClickEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenClickEventArgs>(_headerDoubleClick, e);
		}

		/*
		 * FooterClick
		 */

		private static readonly object _footerClick = new object();

		/// <summary>
		/// Occurs when the footer is clicked.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_FooterClick")]
		public event EventHandler<NuGenClickEventArgs> FooterClick
		{
			add
			{
				this.Events.AddHandler(_footerClick, value);
			}
			remove
			{
				this.Events.RemoveHandler(_footerClick, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="FooterClick"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnFooterClick(NuGenClickEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenClickEventArgs>(_footerClick, e);
		}

		/*
		 * FooterDoubleClick
		 */

		private static readonly object _footerDoubleClick = new object();

		/// <summary>
		/// Occurs when the footer is double-clicked.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_FooterDoubleClick")]
		public event EventHandler<NuGenClickEventArgs> FooterDoubleClick
		{
			add
			{
				this.Events.AddHandler(_footerDoubleClick, value);
			}
			remove
			{
				this.Events.RemoveHandler(_footerDoubleClick, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="FooterDoubleClick"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnFooterDoubleClick(NuGenClickEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenClickEventArgs>(_footerDoubleClick, e);
		}

		/*
		 * DaySelected
		 */

		private static readonly object _daySelected = new object();

		/// <summary>
		/// Occurs when one or more days are selected.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_DaySelected")]
		public event EventHandler<NuGenDaySelectedEventArgs> DaySelected
		{
			add
			{
				this.Events.AddHandler(_daySelected, value);
			}
			remove
			{
				this.Events.RemoveHandler(_daySelected, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="DaySelected"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnDaySelected(NuGenDaySelectedEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenDaySelectedEventArgs>(_daySelected, e);
		}

		/*
		 * DayDeselected
		 */

		private static readonly object _dayDeselected = new object();

		/// <summary>
		/// Occurs when one or more days are deselected.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_DayDeselected")]
		public event EventHandler<NuGenDaySelectedEventArgs> DayDeselected
		{
			add
			{
				this.Events.AddHandler(_dayDeselected, value);
			}
			remove
			{
				this.Events.RemoveHandler(_dayDeselected, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="DayDeselected"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnDayDeselected(NuGenDaySelectedEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenDaySelectedEventArgs>(_dayDeselected, e);
		}

		/*
		 * BeforeDaySelected
		 */

		private static readonly object _beforeDaySelected = new object();

		/// <summary>
		/// Occurs before a day becomes selected.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_BeforeDaySelected")]
		public event EventHandler<NuGenDayStateChangedEventArgs> BeforeDaySelected
		{
			add
			{
				this.Events.AddHandler(_beforeDaySelected, value);
			}
			remove
			{
				this.Events.RemoveHandler(_beforeDaySelected, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="BeforeDaySelected"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnBeforeDaySelected(NuGenDayStateChangedEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenDayStateChangedEventArgs>(_beforeDaySelected, e);
		}

		/*
		 * BeforeDayDeselected
		 */

		private static readonly object _beforeDayDeselected = new object();

		/// <summary>
		/// Occurs before a day becomes deselected.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_BeforeDayDeselected")]
		public event EventHandler<NuGenDayStateChangedEventArgs> BeforeDayDeselected
		{
			add
			{
				this.Events.AddHandler(_beforeDayDeselected, value);
			}
			remove
			{
				this.Events.RemoveHandler(_beforeDayDeselected, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="BeforeDayDeselected"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnBeforeDayDeselected(NuGenDayStateChangedEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenDayStateChangedEventArgs>(_beforeDayDeselected, e);
		}

		/*
		 * DayLostFocus
		 */

		private static readonly object _dayLostFocus = new object();

		/// <summary>
		/// Occurs when a day loses focus.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_DayLostFocus")]
		public event EventHandler<NuGenDayEventArgs> DayLostFocus
		{
			add
			{
				this.Events.AddHandler(_dayLostFocus, value);
			}
			remove
			{
				this.Events.RemoveHandler(_dayLostFocus, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="DayLostFocus"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnDayLostFocus(NuGenDayEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenDayEventArgs>(_dayLostFocus, e);
		}

		/*
		 * DayGotFocus
		 */

		private static readonly object _dayGotFocus = new object();

		/// <summary>
		/// Occurs when a day receives focus.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_DayGotFocus")]
		public event EventHandler<NuGenDayEventArgs> DayGotFocus
		{
			add
			{
				this.Events.AddHandler(_dayGotFocus, value);
			}
			remove
			{
				this.Events.RemoveHandler(_dayGotFocus, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="DayGotFocus"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnDayGotFocus(NuGenDayEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenDayEventArgs>(_dayGotFocus, e);
		}

		/*
		 * WeeknumberClick
		 */

		private static readonly object _weeknumberClick = new object();

		/// <summary>
		/// Occurs when a week number is clicked.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_WeeknumberClick")]
		public event EventHandler<NuGenWeeknumberClickEventArgs> WeeknumberClick
		{
			add
			{
				this.Events.AddHandler(_weeknumberClick, value);
			}
			remove
			{
				this.Events.RemoveHandler(_weeknumberClick, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="WeeknumberClick"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnWeeknumberClick(NuGenWeeknumberClickEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenWeeknumberClickEventArgs>(_weeknumberClick, e);
		}

		/*
		 * WeeknumberDoubleClick
		 */

		private static readonly object _weeknumberDoubleClick = new object();

		/// <summary>
		/// Occurs when a week number is double-clicked.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_WeeknumberDoubleClick")]
		public event EventHandler<NuGenWeeknumberClickEventArgs> WeeknumberDoubleClick
		{
			add
			{
				this.Events.AddHandler(_weeknumberDoubleClick, value);
			}
			remove
			{
				this.Events.RemoveHandler(_weeknumberDoubleClick, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="WeeknumberDoubleClick"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnWeeknumberDoubleClick(NuGenWeeknumberClickEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenWeeknumberClickEventArgs>(_weeknumberDoubleClick, e);
		}

		/*
		 * WeekdayClick
		 */

		private static readonly object _weekdayClick = new object();

		/// <summary>
		/// Occurs when a weekday is clicked.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_WeekdayClick")]
		public event EventHandler<NuGenWeekdayClickEventArgs> WeekdayClick
		{
			add
			{
				this.Events.AddHandler(_weekdayClick, value);
			}
			remove
			{
				this.Events.RemoveHandler(_weekdayClick, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="WeekdayClick"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnWeekdayClick(NuGenWeekdayClickEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT(_weekdayClick, e);
		}

		/*
		 * WeekdayDoubleClick
		 */

		private static readonly object _weekdayDoubleClick = new object();

		/// <summary>
		/// Occurs when a weekday is double-clicked.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_WeekdayDoubleClick")]
		public event EventHandler<NuGenWeekdayClickEventArgs> WeekdayDoubleClick
		{
			add
			{
				this.Events.AddHandler(_weekdayDoubleClick, value);
			}
			remove
			{
				this.Events.RemoveHandler(_weekdayDoubleClick, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="WeekdayDoubleClick"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnWeekdayDoubleClick(NuGenWeekdayClickEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenWeekdayClickEventArgs>(_weekdayDoubleClick, e);
		}

		/*
		 * FooterMouseEnter
		 */

		private static readonly object _footerMouseEnter = new object();

		/// <summary>
		/// Occurs when the mouse pointer enters the footer.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_FooterMouseEnter")]
		public event EventHandler FooterMouseEnter
		{
			add
			{
				this.Events.AddHandler(_footerMouseEnter, value);
			}
			remove
			{
				this.Events.RemoveHandler(_footerMouseEnter, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="FooterMouseEnter"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnFooterMouseEnter(EventArgs e)
		{
			this.Initiator.InvokeEventHandler(_footerMouseEnter, e);
		}

		/*
		 * FooterMouseLeave
		 */

		private static readonly object _footerMouseLeave = new object();

		/// <summary>
		/// Occurs when the mouse pointer leaves the footer.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_FooterMouseLeave")]
		public event EventHandler FooterMouseLeave
		{
			add
			{
				this.Events.AddHandler(_footerMouseLeave, value);
			}
			remove
			{
				this.Events.RemoveHandler(_footerMouseLeave, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="FooterMouseLeave"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnFooterMouseLeave(EventArgs e)
		{
			this.Initiator.InvokeEventHandler(_footerMouseLeave, e);
		}

		/*
		 * HeaderMouseEnter
		 */

		private static readonly object _headerMouseEnter = new object();

		/// <summary>
		/// Occurs when the mouse pointer enters the header.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_HeaderMouseEnter")]
		public event EventHandler HeaderMouseEnter
		{
			add
			{
				this.Events.AddHandler(_headerMouseEnter, value);
			}
			remove
			{
				this.Events.RemoveHandler(_headerMouseEnter, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="HeaderMouseEnter"/>.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnHeaderMouseEnter(EventArgs e)
		{
			this.Initiator.InvokeEventHandler(_headerMouseEnter, e);
		}

		/*
		 * HeaderMouseLeave
		 */

		private static readonly object _headerMouseLeave = new object();

		/// <summary>
		/// Occurs when the mouse pointer leaves the header.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_HeaderMouseLeave")]
		public event EventHandler HeaderMouseLeave
		{
			add
			{
				this.Events.AddHandler(_headerMouseLeave, value);
			}
			remove
			{
				this.Events.RemoveHandler(_headerMouseLeave, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="HeaderMouseLeave"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnHeaderMouseLeave(EventArgs e)
		{
			this.Initiator.InvokeEventHandler(_headerMouseLeave, e);
		}

		/*
		 * WeekdaysMouseEnter
		 */

		private static readonly object _weekdaysMouseEnter = new object();

		/// <summary>
		/// Occurs when the mouse pointer enters weekdays.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_WeekdaysMouseEnter")]
		public event EventHandler WeekdaysMouseEnter
		{
			add
			{
				this.Events.AddHandler(_weekdaysMouseEnter, value);
			}
			remove
			{
				this.Events.RemoveHandler(_weekdaysMouseEnter, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="WeekdaysMouseEnter"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnWeekdaysMouseEnter(EventArgs e)
		{
			this.Initiator.InvokeEventHandler(_weekdaysMouseEnter, e);
		}

		/*
		 * WeekdaysMouseLeave
		 */

		private static readonly object _weekdaysMouseLeave = new object();

		/// <summary>
		/// Occurs when the mouse pointer leaves weekdays.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_WeekdaysMouseLeave")]
		public event EventHandler WeekdaysMouseLeave
		{
			add
			{
				this.Events.AddHandler(_weekdaysMouseLeave, value);
			}
			remove
			{
				this.Events.RemoveHandler(_weekdaysMouseLeave, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="WeekdaysMouseLeave"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnWeekdaysMouseLeave(EventArgs e)
		{
			this.Initiator.InvokeEventHandler(_weekdaysMouseLeave, e);
		}

		/*
		 * WeeknumbersMouseEnter
		 */

		private static readonly object _weeknumbersMouseEnter = new object();

		/// <summary>
		/// Occurs when the mouse pointer enters week numbers.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_WeeknumbersMouseEnter")]
		public event EventHandler WeeknumbersMouseEnter
		{
			add
			{
				this.Events.AddHandler(_weeknumbersMouseEnter, value);
			}
			remove
			{
				this.Events.RemoveHandler(_weeknumbersMouseEnter, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="WeeknumbersMouseEnter"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnWeeknumbersMouseEnter(EventArgs e)
		{
			this.Initiator.InvokeEventHandler(_weeknumbersMouseEnter, e);
		}

		/*
		 * WeeknumbersMouseLeave
		 */

		private static readonly object _weeknumbersMouseLeave = new object();

		/// <summary>
		/// Occurs when the mouse pointer leaves week numbers.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Calendar")]
		[NuGenSRDescription("Description_Calendar_WeeknumbersMouseLeave")]
		public event EventHandler WeeknumbersMouseLeave
		{
			add
			{
				this.Events.AddHandler(_weeknumbersMouseLeave, value);
			}
			remove
			{
				this.Events.RemoveHandler(_weeknumbersMouseLeave, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="WeeknumbersMouseLeave"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnWeeknumbersMouseLeave(EventArgs e)
		{
			this.Initiator.InvokeEventHandler(_weeknumbersMouseLeave, e);
		}

		#endregion

		#region Properties.Appearance

		/*
		 * BorderColor
		 */

		/// <summary>
		/// Gets or sets the color used for the border.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(typeof(Color), "Black")]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Calendar_BorderColor")]
		public Color BorderColor
		{
			get
			{
				return _borderColor;
			}
			set
			{
				if (value != _borderColor)
				{
					_borderColor = value;
					this.OnCalendarColorChanged(new NuGenCalendarColorEventArgs(NuGenCalendarColor.Border));
					Invalidate();
				}
			}
		}

		/*
		 * BorderStyle
		 */

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(ButtonBorderStyle.Solid)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Calendar_BorderStyle")]
		public new ButtonBorderStyle BorderStyle
		{
			get
			{
				return _borderStyle;
			}
			set
			{
				if (value != _borderStyle)
				{
					_borderStyle = value;
					this.OnBorderStyleChanged(EventArgs.Empty);
					Invalidate();
				}

			}
		}

		private static readonly object _borderStyleChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="BorderStyle"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Calendar_BorderStyleChanged")]
		public event EventHandler BorderStyleChanged
		{
			add
			{
				this.Events.AddHandler(_borderStyleChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_borderStyleChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="BorderStyleChanged"/> event.
		/// </summary>
		protected virtual void OnBorderStyleChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_borderStyleChanged, e);
		}

		/*
		 * Footer
		 */

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Calendar_Footer")]
		public NuGenFooter Footer
		{
			get
			{
				return _footer;
			}
		}

		private static readonly object _footerPropertyChanged = new object();

		/// <summary>
		/// Occurs when a footer property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Calendar_FooterPropertyChanged")]
		public event EventHandler<NuGenFooterPropertyEventArgs> FooterPropertyChanged
		{
			add
			{
				this.Events.AddHandler(_footerPropertyChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_footerPropertyChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="FooterPropertyChanged"/> event.
		/// </summary>
		protected virtual void OnFooterPropertyChanged(NuGenFooterPropertyEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenFooterPropertyEventArgs>(_footerPropertyChanged, e);
		}

		/*
		 * Header
		 */

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Calendar_Header")]
		public NuGenHeader Header
		{
			get
			{
				return _header;
			}
		}

		private static readonly object _headerPropertyChanged = new object();

		/// <summary>
		/// Occurs when a header property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Calendar_HeaderPropertyChanged")]
		public event EventHandler<NuGenHeaderPropertyEventArgs> HeaderPropertyChanged
		{
			add
			{
				this.Events.AddHandler(_headerPropertyChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_headerPropertyChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="HeaderPropertyChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnHeaderPropertyChanged(NuGenHeaderPropertyEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenHeaderPropertyEventArgs>(_headerPropertyChanged, e);
		}

		/*
		 * Month
		 */

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Calendar_Month")]
		public NuGenMonth Month
		{
			get
			{
				return _month;
			}
		}

		private static readonly object _monthPropertyChanged = new object();

		/// <summary>
		/// Occurs when a month property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Calendar_MonthPropertyChanged")]
		public event EventHandler<NuGenMonthPropertyEventArgs> MonthPropertyChanged
		{
			add
			{
				this.Events.AddHandler(_monthPropertyChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_monthPropertyChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="MonthPropertyChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnMonthPropertyChanged(NuGenMonthPropertyEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenMonthPropertyEventArgs>(_monthPropertyChanged, e);
		}

		private static readonly object _monthColorChanged = new object();

		/// <summary>
		/// Occurs when a month color changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Calendar_MonthColorChanged")]
		public event EventHandler<NuGenMonthColorEventArgs> MonthColorChanged
		{
			add
			{
				this.Events.AddHandler(_monthColorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_monthColorChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="MonthColorChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnMonthColorChanged(NuGenMonthColorEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenMonthColorEventArgs>(_monthColorChanged, e);
		}

		private static readonly object _monthBorderStyleChanged = new object();

		/// <summary>
		/// Occurs when a month border style changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Calendar_MonthBorderStyleChanged")]
		public event EventHandler<NuGenMonthBorderStyleEventArgs> MonthBorderStyleChanged
		{
			add
			{
				this.Events.AddHandler(_monthBorderStyleChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_monthBorderStyleChanged, value);
			}
		}

		/// <summary>
		/// Will bubble thte <see cref="MonthBorderStyleChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnMonthBorderStyleChanged(NuGenMonthBorderStyleEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenMonthBorderStyleEventArgs>(_monthBorderStyleChanged, e);
		}

		/*
		 * ShowFooter
		 */

		/// <summary>
		/// Gets or sets the value indicating whether the calendar should display the footer.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Calendar_ShowFooter")]
		public bool ShowFooter
		{
			get
			{
				return _showFooter;
			}
			set
			{
				if (value != _showFooter)
				{
					_showFooter = value;
					DoLayout();
					this.OnShowFooterChanged(EventArgs.Empty);
					Invalidate();
				}
			}
		}

		private static readonly object _showFooterChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="ShowFooter"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Calendar_ShowFooterChanged")]
		public event EventHandler ShowFooterChanged
		{
			add
			{
				this.Events.AddHandler(_showFooterChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_showFooterChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="ShowFooterChanged"/> event.
		/// </summary>
		protected virtual void OnShowFooterChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_showFooterChanged, e);
		}

		/*
		 * ShowHeader
		 */

		/// <summary>
		/// Gets or sets the value indicating whether the calendar should display the header.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Calendar_ShowHeader")]
		public bool ShowHeader
		{
			get
			{
				return _showHeader;
			}
			set
			{
				if (value != _showHeader)
				{
					_showHeader = value;
					DoLayout();
					this.OnShowHeaderChanged(EventArgs.Empty);
					Invalidate();
				}
			}
		}

		private static readonly object _showHeaderChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="ShowHeader"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Calendar_ShowHeaderChanged")]
		public event EventHandler ShowHeaderChanged
		{
			add
			{
				this.Events.AddHandler(_showHeaderChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_showHeaderChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="ShowHeaderChanged"/> event.
		/// </summary>
		protected virtual void OnShowHeaderChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_showHeaderChanged, e);
		}

		/*
		 * ShowWeekdays
		 */

		/// <summary>
		/// Gets or sets the value indicating whether the calendar should display weekdays.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Calendar_ShowWeekdays")]
		public bool ShowWeekdays
		{
			get
			{
				return _showWeekday;
			}
			set
			{
				if (value != _showWeekday)
				{
					_showWeekday = value;
					DoLayout();
					this.OnShowWeekdaysChanged(EventArgs.Empty);
					Invalidate();
				}
			}
		}

		private static readonly object _showWeekdaysChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="ShowWeekdays"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Calendar_ShowWeekdaysChanged")]
		public event EventHandler ShowWeekdaysChanged
		{
			add
			{
				this.Events.AddHandler(_showWeekdaysChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_showWeekdaysChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="ShowWeekdaysChanged"/> event.
		/// </summary>
		protected virtual void OnShowWeekdaysChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_showWeekdaysChanged, e);
		}

		/*
		 * TodayColor
		 */

		/// <summary>
		/// Gets or sets the color used to mark today.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(typeof(Color), "Red")]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Calendar_TodayColor")]
		public Color TodayColor
		{
			get
			{
				return _todayColor;
			}
			set
			{
				if (value != _todayColor)
				{
					_todayColor = value;
					this.OnCalendarColorChanged(new NuGenCalendarColorEventArgs(NuGenCalendarColor.Today));
					Invalidate();
				}
			}
		}

		/*
		 * Weekdays
		 */

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Calendar_Weekdays")]
		public NuGenWeekday Weekdays
		{
			get
			{
				return _weekday;
			}
		}

		private static readonly object _weekdayPropertyChanged = new object();

		/// <summary>
		/// Occurs when a weekday property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Calendar_WeekdayPropertyChanged")]
		public event EventHandler<NuGenWeekdayPropertyEventArgs> WeekdayPropertyChanged
		{
			add
			{
				this.Events.AddHandler(_weekdayPropertyChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_weekdayPropertyChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="WeekdayPropertyChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnWeekdayPropertyChanged(NuGenWeekdayPropertyEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenWeekdayPropertyEventArgs>(_weekdayPropertyChanged, e);
		}

		/*
		 * Weeknumbers
		 */

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Calendar_Weeknumbers")]
		public NuGenWeeknumber Weeknumbers
		{
			get
			{
				return _weeknumber;
			}
		}

		private static readonly object _weeknumberPropertyChanged = new object();

		/// <summary>
		/// Occurs when a weeknumber property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Calendar_WeeknumberPropertyChanged")]
		public event EventHandler<NuGenWeeknumberPropertyEventArgs> WeeknumberPropertyChanged
		{
			add
			{
				this.Events.AddHandler(_weeknumberPropertyChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_weeknumberPropertyChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="WeeknumberPropertyChanged"/> event.
		/// </summary>
		protected virtual void OnWeeknumberPropertyChanged(NuGenWeeknumberPropertyEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenWeeknumberPropertyEventArgs>(_weeknumberPropertyChanged, e);
		}

		#endregion

		#region Properties.Behavior

		/*
		 * ActiveMonth
		 */

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Description("")]
		public NuGenActiveMonth ActiveMonth
		{
			get
			{
				return _activeMonth;
			}
		}

		/*
		 * Calendar
		 */

		/// <summary>
		/// </summary>
		[NuGenSRCategory("Category_Behavior")]
		[RefreshProperties(RefreshProperties.All)]
		[Description("Culture to use for calendar.")]
		public CultureInfo Culture
		{
			get
			{
				return _culture;
			}
			set
			{
				try
				{
					Thread.CurrentThread.CurrentCulture = value;
					_dateTimeFormat = DateTimeFormatInfo.CurrentInfo;
				}
				catch (Exception)
				{
				}
				finally
				{
					_culture = value;
					_defaultFirstDayOfWeek = _dateTimeFormat.FirstDayOfWeek;

					if (_firstDayOfWeek != 0)
						_dateTimeFormat.FirstDayOfWeek = IntToDayOfWeek(_firstDayOfWeek - 1);
					else
						_dateTimeFormat.FirstDayOfWeek = _defaultFirstDayOfWeek;

					this.OnCultureChanged(EventArgs.Empty);
				}
				_month.RemoveSelection(true);
				Setup();
				Invalidate();
			}
		}

		private static readonly object _cultureChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Culture"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Calendar_CultureChanged")]
		public event EventHandler CultureChanged
		{
			add
			{
				this.Events.AddHandler(_cultureChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_cultureChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="CultureChanged"/> event.
		/// </summary>
		protected virtual void OnCultureChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_cultureChanged, e);
		}

		/*
		 * Dates
		 */

		/// <summary>
		/// </summary>
		[NuGenSRCategory("Category_Behavior")]
		[DefaultValue(null)]
		[Description("Collection with formatted dates.")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor("Genetibase.Shared.Controls.Design.NuGenDateItemCollectionEditor", typeof(UITypeEditor))]
		public NuGenDateItemCollection Dates
		{
			get
			{
				return this._dateItemCollection;
			}
		}

		/*
		 * ExtendedSelectionKey
		 */

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[DefaultValue(typeof(NuGenExtendedSelectionKey), "Ctrl")]
		[Description("Key used for Extended selection mode.")]
		public NuGenExtendedSelectionKey ExtendedSelectionKey
		{
			get
			{
				return _extendedKey;
			}
			set
			{
				if (value != _extendedKey)
				{
					_extendedKey = value;
					this.OnExtendedSelectionKeyChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _extendedSelectionKeyChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="ExtendedSelectionKey"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Calendar_ExtendedSelectionKeyChanged")]
		public event EventHandler ExtendedSelectionKeyChanged
		{
			add
			{
				this.Events.AddHandler(_extendedSelectionKeyChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_extendedSelectionKeyChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="ExtendedSelectionKeyChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnExtendedSelectionKeyChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_extendedSelectionKeyChanged, e);
		}

		/*
		 * FirstDayOfWeek
		 */

		/// <summary>
		/// </summary>
		[Description("First day of week.")]
		[RefreshProperties(RefreshProperties.All)]
		[NuGenSRCategory("Category_Behavior")]
		[DefaultValue(0)]
		[TypeConverter(typeof(FirstDayOfWeekConverter))]
		public int FirstDayOfWeek
		{
			get
			{
				return _firstDayOfWeek;
			}
			set
			{
				if (_firstDayOfWeek != value)
				{
					_firstDayOfWeek = value;
					if (_firstDayOfWeek != 0)
						_dateTimeFormat.FirstDayOfWeek = IntToDayOfWeek(_firstDayOfWeek - 1);
					else
						_dateTimeFormat.FirstDayOfWeek = _defaultFirstDayOfWeek;

					this.OnFirstDayOfWeekChanged(EventArgs.Empty);

					Setup();
					Invalidate();
				}
			}
		}

		private static readonly object _firstDayOfWeekChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="FirstDayOfWeek"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Calendar_FirstDayOfWeekChanged")]
		public event EventHandler FirstDayOfWeekChanged
		{
			add
			{
				this.Events.AddHandler(_firstDayOfWeekChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_firstDayOfWeekChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="FirstDayOfWeekChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnFirstDayOfWeekChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_firstDayOfWeekChanged, e);
		}

		/*
		 * ImageList
		 */

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[Description("ImageList thats contains the images used in the calendar.")]
		public ImageList ImageList
		{
			get
			{
				return _imageList;
			}
			set
			{
				if (value != _imageList)
				{
					_imageList = value;
					this.OnImageListChanged(EventArgs.Empty);
					Invalidate();
				}
			}
		}

		private static readonly object _imageListChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="ImageList"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Calendar_ImageListChanged")]
		public event EventHandler ImageListChanged
		{
			add
			{
				this.Events.AddHandler(_imageListChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_imageListChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="ImageListChanged"/> event.
		/// </summary>
		protected virtual void OnImageListChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_imageListChanged, e);
		}

		/*
		 * Keyboard
		 */

		/// <summary>
		/// </summary>
		[NuGenSRCategory("Category_Behavior")]
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public KeyboardConfig Keyboard
		{
			get
			{
				return _keyboard;
			}
		}

		private static readonly object _keyboardChanged = new object();

		/// <summary>
		/// Occurs when the keyboard configuration changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Calendar_KeyboardChanged")]
		public event EventHandler<NuGenKeyboardChangedEventArgs> KeyboardChanged
		{
			add
			{
				this.Events.AddHandler(_keyboardChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_keyboardChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="KeyboardChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnKeyboardChanged(NuGenKeyboardChangedEventArgs e)
		{
			this.Initiator.InvokeEventHandlerT<NuGenKeyboardChangedEventArgs>(_keyboardChanged, e);
		}

		/*
		 * KeyboardEnabled
		 */

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[Description("Indicates wether keyboard support is enabled.")]
		[NuGenSRCategory("Category_Behavior")]
		[DefaultValue(typeof(bool), "True")]
		public bool KeyboardEnabled
		{
			get
			{
				return _keyboardEnabled;
			}
			set
			{
				if (_keyboardEnabled != value)
				{
					_keyboardEnabled = value;
					this.OnKeyboardEnabledChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _keyboardEnabledChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="KeyboardEnabled"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Calendar_KeyboardEnabledChanged")]
		public event EventHandler KeyboardEnabledChanged
		{
			add
			{
				this.Events.AddHandler(_keyboardEnabledChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_keyboardEnabledChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="KeyboardEnabledChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnKeyboardEnabledChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_keyboardEnabledChanged, e);
		}

		/*
		 * MinDate
		 */

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[Description("The minimum date that can be selected.")]
		[TypeConverter(typeof(DateTimeTypeConverter))]
		public DateTime MinDate
		{
			get
			{
				return _minDate;
			}
			set
			{
				if (value != _minDate)
				{
					if (value <= _maxDate)
					{
						this.OnMinDateChanged(EventArgs.Empty);
						_minDate = value;
						Invalidate();
					}
				}
			}
		}

		private static readonly object _minDateChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="MinDate"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Calendar_MinDateChanged")]
		public event EventHandler MinDateChanged
		{
			add
			{
				this.Events.AddHandler(_minDateChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_minDateChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="MinDateChanged"/> event.
		/// </summary>
		protected virtual void OnMinDateChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_minDateChanged, e);
		}


		/*
		 * MaxDate
		 */

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[Description("The maximum date that can be selected.")]
		[TypeConverter(typeof(DateTimeTypeConverter))]
		public DateTime MaxDate
		{
			get
			{
				return _maxDate;
			}
			set
			{
				if (value != _maxDate)
				{
					if (value >= _minDate)
					{
						_maxDate = value;
						this.OnMaxDateChanged(EventArgs.Empty);
						Invalidate();
					}
				}
			}
		}

		private static readonly object _maxDateChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="MaxDate"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Calendar_MaxDateChanged")]
		public event EventHandler MaxDateChanged
		{
			add
			{
				this.Events.AddHandler(_maxDateChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_maxDateChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="MaxDateChanged"/> event.
		/// </summary>
		protected virtual void OnMaxDateChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_maxDateChanged, e);
		}

		/*
		 * SelectButton
		 */

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[Description("The mouse button used for selections.")]
		[DefaultValue(typeof(MouseButtons), "Left")]
		public MouseButtons SelectButton
		{
			get
			{
				return _selectButton;
			}
			set
			{
				if (value != _selectButton)
				{
					this.OnSelectButtonChanged(EventArgs.Empty);
					_selectButton = value;
				}
			}
		}

		private static readonly object _selectButtonChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="SelectButton"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Calendar_SelectButtonChanged")]
		public event EventHandler SelectButtonChanged
		{
			add
			{
				this.Events.AddHandler(_selectButtonChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_selectButtonChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="SelectButtonChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnSelectButtonChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_selectButtonChanged, e);
		}

		/*
		 * SelectionMode
		 */

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[Description("Indicates the selection mode used.")]
		[DefaultValue(typeof(NuGenSelectionMode), "MultiSimple")]
		public NuGenSelectionMode SelectionMode
		{
			get
			{
				return _selectionMode;
			}
			set
			{
				if (value != _selectionMode)
				{
					_selectionMode = value;

					// if new selectionMode is more limited than the "old" , clear existing selections
					if (value < _selectionMode)
						ClearSelection();
					this.OnSelectionModeChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _selectionModeChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="SelectionMode"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Calendar_SelectionModeChanged")]
		public event EventHandler SelectionModeChanged
		{
			add
			{
				this.Events.AddHandler(_selectionModeChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_selectionModeChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="SelectionModeChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnSelectionModeChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_selectionModeChanged, e);
		}

		/*
		 * SelectTrailingDates
		 */

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[Description("Indicates wether its possible to select trailing dates.")]
		[DefaultValue(true)]
		public bool SelectTrailingDates
		{
			get
			{
				return _selectTrailing;
			}
			set
			{
				if (value != _selectTrailing)
				{
					_selectTrailing = value;
					this.OnSelectTrailingChanged(EventArgs.Empty);
					Invalidate();
				}
			}
		}

		private static readonly object _selectTrailingChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="SelectTrailingDates"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Calendar_SelectTrailingChanged")]
		public event EventHandler SelectTrailingChanged
		{
			add
			{
				this.Events.AddHandler(_selectTrailingChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_selectTrailingChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="SelectTrailingChanged"/> event.
		/// </summary>
		protected virtual void OnSelectTrailingChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_selectTrailingChanged, e);
		}

		/*
		 * ShowFocus
		 */

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[Description("Indicates wether the focus should be displayed.")]
		[DefaultValue(true)]
		public bool ShowFocus
		{
			get
			{
				return _showFocus;
			}
			set
			{
				if (value != _showFocus)
				{
					_showFocus = value;
					this.OnShowFocusChanged(EventArgs.Empty);
					Invalidate();
				}
			}
		}

		private static readonly object _showFocusChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="ShowFocus"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Calendar_ShowFocusChanged")]
		public event EventHandler ShowFocusChanged
		{
			add
			{
				this.Events.AddHandler(_showFocusChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_showFocusChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="ShowFocusChanged"/> event.
		/// </summary>
		protected virtual void OnShowFocusChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_showFocusChanged, e);
		}

		/*
		 * ShowToday
		 */

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[NuGenSRCategory("Category_Behavior")]
		[Description("Indicates wether todays date should be marked.")]
		public bool ShowToday
		{
			get
			{
				return _showToday;
			}
			set
			{
				if (value != _showToday)
				{
					_showToday = value;
					this.OnShowTodayChanged(EventArgs.Empty);
					Invalidate();
				}
			}
		}

		private static readonly object _showTodayChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="ShowToday"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Calendar_ShowTodayChanged")]
		public event EventHandler ShowTodayChanged
		{
			add
			{
				this.Events.AddHandler(_showTodayChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_showTodayChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="ShowTodayChanged"/> event.
		/// </summary>
		protected virtual void OnShowTodayChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_showTodayChanged, e);
		}

		/*
		 * ShowTrailingDates
		 */

		/// <summary>
		/// </summary>
		[Description("Indicates wether the trailing dates should be drawn.")]
		[NuGenSRCategory("Category_Behavior")]
		[DefaultValue(true)]
		public bool ShowTrailingDates
		{
			get
			{
				return _showTrailing;
			}
			set
			{
				if (_showTrailing != value)
				{
					_showTrailing = value;
					if (value == false)
						SelectTrailingDates = false;
					this.OnShowTrailingChanged(EventArgs.Empty);
					Invalidate();
				}
			}
		}

		private static readonly object _showTrailingChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="ShowTrailingDates"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Calendar_ShowTrailingChanged")]
		public event EventHandler ShowTrailingChanged
		{
			add
			{
				this.Events.AddHandler(_showTrailingChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_showTrailingChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="ShowTrailingChanged"/> event.
		/// </summary>
		protected virtual void OnShowTrailingChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_showTrailingChanged, e);
		}

		/*
		 * ShowWeeknumbers
		 */

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		[Description("Indicates wether the calendar should display week numbers.")]
		[DefaultValue(false)]
		public bool ShowWeeknumbers
		{
			get
			{
				return _showWeeknumber;
			}
			set
			{
				if (value != _showWeeknumber)
				{
					_showWeeknumber = value;
					DoLayout();
					this.OnShowWeeknumbersChanged(EventArgs.Empty);
					Invalidate();
				}
			}
		}

		private static readonly object _showWeeknumbersChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="ShowWeeknumbers"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Calendar_ShowWeeknumbersChanged")]
		public event EventHandler ShowWeeknumbersChanged
		{
			add
			{
				this.Events.AddHandler(_showWeeknumbersChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_showWeeknumbersChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="ShowWeeknumbersChanged"/> event.
		/// </summary>
		protected virtual void OnShowWeeknumbersChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_showWeeknumbersChanged, e);
		}

		/*
		 * UseTheme
		 */

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[DefaultValue(true)]
		public bool UseTheme
		{
			get
			{
				return m_useTheme;
			}
			set
			{
				if (value != m_useTheme)
				{
					m_useTheme = value;
					this.OnUseThemeChanged(EventArgs.Empty);
					if (m_useTheme)
						GetThemeColors();
					else
						Invalidate();
				}
			}
		}

		private static readonly object _useThemeChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="UseTheme"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Calendar_UseThemeChanged")]
		public event EventHandler UseThemeChanged
		{
			add
			{
				this.Events.AddHandler(_useThemeChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_useThemeChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="UseThemeChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnUseThemeChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_useThemeChanged, e);
		}

		#endregion

		#region Properties.NonBrowsable

		/// <summary>
		/// </summary>
		[Browsable(false)]
		public NuGenSelectedDatesCollection SelectedDates
		{
			get
			{
				return UpdateSelectedCollection();
			}
		}

		#endregion

		#region Properties.Hidden

		/// <summary>
		/// Gets or sets the background image displayed in the control.
		/// </summary>
		/// <value></value>
		/// <returns>An <see cref="T:System.Drawing.Image"></see> that represents the image to display in the background of the control.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether control's elements are aligned to support locales using right-to-left fonts.
		/// </summary>
		/// <value></value>
		/// <returns>One of the <see cref="T:System.Windows.Forms.RightToLeft"></see> values. The default is <see cref="F:System.Windows.Forms.RightToLeft.Inherit"></see>.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:System.Windows.Forms.RightToLeft"></see> values. </exception>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override RightToLeft RightToLeft
		{
			get
			{
				return base.RightToLeft;
			}
			set
			{
				base.RightToLeft = value;
			}
		}

		/// <summary>
		/// Gets or sets the font of the text displayed by the control.
		/// </summary>
		/// <value></value>
		/// <returns>The <see cref="T:System.Drawing.Font"></see> to apply to the text displayed by the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultFont"></see> property.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Font Font
		{
			get
			{
				return base.Font;
			}
			set
			{
				base.Font = value;
			}
		}

		/// <summary>
		/// Gets or sets the background color for the control.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that represents the background color of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor"></see> property.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
			}
		}

		/// <summary>
		/// </summary>
		/// <value></value>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
			}
		}

		/// <summary>
		/// Gets or sets the foreground color of the control.
		/// </summary>
		/// <value></value>
		/// <returns>The foreground <see cref="T:System.Drawing.Color"></see> of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultForeColor"></see> property.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
			}
		}

		#endregion

		#region Properties.Internal

		internal bool SelectKeyDown
		{
			get
			{
				if (!_keyboardEnabled)
					return false;
				else
					return _selectKeyDown;
			}
		}

		internal bool ExtendedKey
		{
			get
			{
				if (_extendedKey == NuGenExtendedSelectionKey.None)
					return true;
				else
					return _ctrlKey;
			}
			set
			{
				_ctrlKey = value;
			}
		}

		internal NuGenCalendarRegion ActiveRegion
		{
			get
			{
				return _activeRegion;
			}
			set
			{
				if (value != _activeRegion)
				{
					// raise OnLeave event...
					switch (_activeRegion)
					{
						case NuGenCalendarRegion.None:
						case NuGenCalendarRegion.Month:
						case NuGenCalendarRegion.Day:
						{
							break;
						}
						case NuGenCalendarRegion.Header:
						{
							this.OnHeaderMouseLeave(EventArgs.Empty);
							break;
						}
						case NuGenCalendarRegion.Weekdays:
						{
							this.OnWeekdaysMouseLeave(EventArgs.Empty);
							break;
						}
						case NuGenCalendarRegion.Weeknumbers:
						{
							this.OnWeeknumbersMouseLeave(EventArgs.Empty);
							break;
						}
						case NuGenCalendarRegion.Footer:
						{
							this.OnFooterMouseLeave(EventArgs.Empty);
							break;
						}
					}
					_activeRegion = value;
					// Raise onEnter event...
					switch (_activeRegion)
					{
						case NuGenCalendarRegion.None:
						case NuGenCalendarRegion.Month:
						case NuGenCalendarRegion.Day:
						{
							break;
						}
						case NuGenCalendarRegion.Header:
						{
							this.OnHeaderMouseLeave(EventArgs.Empty);
							break;
						}
						case NuGenCalendarRegion.Weekdays:
						{
							this.OnWeekdaysMouseEnter(EventArgs.Empty);
							break;
						}
						case NuGenCalendarRegion.Weeknumbers:
						{
							this.OnWeeknumbersMouseEnter(EventArgs.Empty);
							break;
						}
						case NuGenCalendarRegion.Footer:
						{
							this.OnFooterMouseEnter(EventArgs.Empty);
							break;
						}
					}
				}
			}
		}

		#endregion

		#region Properties.Services

		private INuGenCalendarRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenCalendarRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_renderer = this.ServiceProvider.GetService<INuGenCalendarRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenCalendarRenderer>();
					}
				}

				return _renderer;
			}
		}

		#endregion

		#region Methods.Public

		/// <summary>
		/// </summary>
		public bool IsSelected(DateTime dt)
		{
			bool sel = false;
			for (int i = 0; i < 42; i++)
			{
				if (_month.m_days[i].Date.ToShortDateString() == dt.ToShortDateString())
				{
					if (_month.m_days[i].State == NuGenDayState.Selected)
						sel = true;
					break;
				}
			}
			return sel;
		}

		/// <summary>
		/// </summary>
		public void ClearSelection()
		{
			_month.RemoveSelection(true);
			Invalidate();
		}

		/// <summary>
		/// </summary>
		public Bitmap Snapshot()
		{

			Graphics e = this.CreateGraphics();
			// Create a new bitmap
			Bitmap bmp = new Bitmap(this.Width, this.Height, e);
			// Create a graphics context connected to the bitmap
			e = Graphics.FromImage(bmp);
			// Draw the calendar on the bitmap
			Draw(e, this.DisplayRectangle);

			e.Dispose();
			return bmp;
		}

		/// <summary>
		/// </summary>
		public void Print()
		{
			_printDoc.Print();
		}

		/// <summary>
		/// </summary>
		public void SaveAsImage(string filename, ImageFormat format)
		{
			Bitmap bmp = Snapshot();
			bmp.Save(filename, format);
		}

		/// <summary>
		/// </summary>
		public void Copy()
		{
			try
			{
				Bitmap bmp = Snapshot();
				System.Windows.Forms.Clipboard.SetDataObject(bmp);
			}
			catch (Exception)
			{

			}

		}

		/// <summary>
		/// </summary>
		public void AddDateInfo(NuGenDateItem[] info)
		{
			for (int i = 0; i < info.Length; i++)
			{
				if (info[i] != null)
					Dates.Add(info[i]);
			}
			Invalidate();
		}

		/// <summary>
		/// </summary>
		public void RemoveDateInfo(NuGenDateItem info)
		{
			Dates.Remove(info);
		}

		/// <summary>
		/// </summary>
		public void RemoveDateInfo(DateTime info)
		{
			for (int i = 0; i < Dates.Count; i++)
			{
				if (Dates[i].Date.ToShortDateString() == info.ToShortDateString())
				{
					Dates.RemoveAt(i);
				}
			}
			Invalidate();
		}

		/// <summary>
		/// </summary>
		public void AddDateInfo(NuGenDateItem info)
		{
			Dates.Add(info);
			Invalidate();
		}

		/// <summary>
		/// </summary>
		public void ResetDateInfo()
		{
			Dates.Clear();
			Invalidate();
		}

		/// <summary>
		/// </summary>
		public NuGenDateItem[] GetDateInfo()
		{
			NuGenDateItem[] ret = new NuGenDateItem[0];
			ret.Initialize();
			for (int i = 0; i < Dates.Count; i++)
			{
				ret = Dates.AddInfo(Dates[i], ret);
			}
			return ret;
		}

		/// <summary>
		/// </summary>
		public NuGenDateItem[] GetDateInfo(DateTime dt)
		{
			return Dates.DateInfo(dt);
		}

		/// <summary>
		/// </summary>
		public void SelectDate(DateTime date)
		{
			SelectRange(date, date);
		}

		/// <summary>
		/// </summary>
		public void DeselectRange(DateTime From, DateTime To)
		{
			int from = -1;
			int to = -1;
			if ((From >= _minDate) && (From <= _maxDate) &&
				(To >= _minDate) && (To <= _maxDate) &&
				(SelectionMode == NuGenSelectionMode.MultiExtended))
			{
				for (int i = 0; i < 42; i++)
				{
					if (_month.m_days[i].Date.ToShortDateString() == From.ToShortDateString())
						from = i;
					if (_month.m_days[i].Date.ToShortDateString() == To.ToShortDateString())
						to = i;
					if ((from != -1) && (to != -1))
						break;
				}
				if ((from != -1) && (to != -1))
				{
					_month.DeselectRange(from, to);
					this.Invalidate();
				}
			}
		}

		/// <summary>
		/// </summary>
		public void SelectArea(DateTime topLeft, DateTime bottomRight)
		{
			int topleft = -1;
			int bottomright = -1;
			if ((topLeft >= _minDate) && (topLeft <= _maxDate) &&
				(bottomRight >= _minDate) && (bottomRight <= _maxDate) &&
				(SelectionMode >= NuGenSelectionMode.MultiSimple))
			{

				if ((topLeft.Year.ToString() + topLeft.Month.ToString() == bottomRight.Year.ToString() + bottomRight.Month.ToString()) &&
					 (ActiveMonth.Year.ToString() + ActiveMonth.Month.ToString() != topLeft.Year.ToString() + topLeft.Month.ToString()))
				{
					// Change month
					if (ActiveMonth.Year != topLeft.Year)
						ActiveMonth.Year = topLeft.Year;
					if (ActiveMonth.Month != topLeft.Month)
						ActiveMonth.Month = topLeft.Month;

				}

				for (int i = 0; i < 42; i++)
				{
					if (_month.m_days[i].Date.ToShortDateString() == topLeft.ToShortDateString())
						topleft = i;
					if (_month.m_days[i].Date.ToShortDateString() == bottomRight.ToShortDateString())
						bottomright = i;
					if ((topleft != -1) && (bottomright != -1))
						break;
				}
				if ((topleft != -1) && (bottomright != -1))
				{
					_month.SelectArea(topleft, bottomright);
					this.Invalidate();
				}
			}
		}

		/// <summary>
		/// </summary>
		public void DeselectArea(DateTime topLeft, DateTime bottomRight)
		{
			int topleft = -1;
			int bottomright = -1;
			if ((topLeft >= _minDate) && (topLeft <= _maxDate) &&
				(bottomRight >= _minDate) && (bottomRight <= _maxDate) &&
				(SelectionMode == NuGenSelectionMode.MultiExtended))
			{
				for (int i = 0; i < 42; i++)
				{
					if (_month.m_days[i].Date.ToShortDateString() == topLeft.ToShortDateString())
						topleft = i;
					if (_month.m_days[i].Date.ToShortDateString() == bottomRight.ToShortDateString())
						bottomright = i;
					if ((topleft != -1) && (bottomright != -1))
						break;
				}
				if ((topleft != -1) && (bottomright != -1))
				{
					_month.DeselectArea(topleft, bottomright);
					this.Invalidate();
				}
			}
		}

		/// <summary>
		/// </summary>
		public void SelectRange(DateTime fromDate, DateTime toDate)
		{
			int to = -1;
			int from = -1;

			if (((fromDate >= _minDate) && (toDate <= _maxDate) &&
				(toDate >= _minDate) && (toDate <= _maxDate)) &&
				((SelectionMode >= NuGenSelectionMode.MultiSimple) ||
				((fromDate == toDate) &&
				(SelectionMode == NuGenSelectionMode.One))))
			{

				if ((fromDate.Year.ToString() + fromDate.Month.ToString() == toDate.Year.ToString() + toDate.Month.ToString()) &&
					 (ActiveMonth.Year.ToString() + ActiveMonth.Month.ToString() != fromDate.Year.ToString() + fromDate.Month.ToString()))
				{
					// Change month
					if (ActiveMonth.Year != fromDate.Year)
						ActiveMonth.Year = fromDate.Year;
					if (ActiveMonth.Month != fromDate.Month)
						ActiveMonth.Month = fromDate.Month;

				}


				for (int i = 0; i < 42; i++)
				{
					if (_month.m_days[i].Date.ToShortDateString() == fromDate.ToShortDateString())
						from = i;
					if (_month.m_days[i].Date.ToShortDateString() == toDate.ToShortDateString())
						to = i;
					if ((to != -1) && (from != -1))
						break;
				}
				if ((from != -1) && (to != -1))
				{
					_month.SelectRange(from, to);
					this.Invalidate();
				}

			}

		}

		/// <summary>
		/// </summary>
		public void SelectWeekday(DayOfWeek day)
		{
			if (_selectionMode >= NuGenSelectionMode.MultiSimple)
			{
				for (int i = 0; i <= 6; i++)
				{
					if ((int)_month.m_days[i].Weekday == (int)day)
					{
						_month.SelectArea(i, i + 35);
						this.Invalidate();
						break;
					}
				}
			}
		}

		/// <summary>
		/// </summary>
		public void DeselectWeekday(DayOfWeek day)
		{
			if (_selectionMode == NuGenSelectionMode.MultiExtended)
			{
				for (int i = 0; i <= 6; i++)
				{
					if ((int)_month.m_days[i].Weekday == (int)day)
					{
						_month.DeselectArea(i, i + 35);
						this.Invalidate();
						break;
					}
				}
			}
		}

		/// <summary>
		/// </summary>
		public void SelectWeek(int week)
		{
			if (_selectionMode >= NuGenSelectionMode.MultiSimple)
			{
				for (int i = 0; i < 6; i++)
				{
					if (_month.m_days[i * 7].Week == week)
					{
						_month.SelectRange(i * 7, (i * 7) + 6);
						this.Invalidate();
						break;
					}
				}
			}
		}

		/// <summary>
		/// </summary>
		public void DeselectWeek(int week)
		{
			if (_selectionMode == NuGenSelectionMode.MultiExtended)
			{
				for (int i = 0; i < 6; i++)
				{
					if (_month.m_days[i * 7].Week == week)
					{
						_month.DeselectArea(i * 7, (i * 7) + 6);
						this.Invalidate();
						break;
					}

				}
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		/// <summary>
		/// Preprocesses keyboard or input messages within the message loop before they are dispatched.
		/// </summary>
		/// <param name="msg">A <see cref="T:System.Windows.Forms.Message"></see>, passed by reference, that represents the message to process. The possible values are WM_KEYDOWN, WM_SYSKEYDOWN, WM_CHAR, and WM_SYSCHAR.</param>
		/// <returns>
		/// true if the message was processed by the control; otherwise, false.
		/// </returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		public override bool PreProcessMessage(ref Message msg)
		{

			// Check if message is KEY_DOWN
			if (msg.Msg == WinUser.WM_KEYDOWN)
			{
				Keys keyData = ((Keys)(int)msg.WParam) | ModifierKeys;
				Keys keyCode = ((Keys)(int)msg.WParam);
				// Make sure we handle certain keys
				switch (keyCode)
				{

					default:
					{
						if ((keyCode == _keyboard.Up) ||
							 (keyCode == _keyboard.Down) ||
							 (keyCode == _keyboard.Left) ||
							 (keyCode == _keyboard.Right) ||
							 (keyCode == _keyboard.Select) ||
							 (keyCode == _keyboard.NextMonth) ||
							 (keyCode == _keyboard.NextYear) ||
							 (keyCode == _keyboard.PreviousMonth) ||
							 (keyCode == _keyboard.PreviousYear))
						{
							if (_keyHandled)
								return false;

						}


						break;
					}

				}
			}

			return base.PreProcessMessage(ref msg);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.LostFocus"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnLostFocus(EventArgs e)
		{
			if ((_month.DayInFocus != -1) && (_activeRegion != NuGenCalendarRegion.Month))
			{

				_month.m_days[_month.DayInFocus].State = NuGenDayState.Normal;
				_month.DayInFocus = -1;
				Invalidate();
			}

			base.OnLostFocus(e);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.GotFocus"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnGotFocus(EventArgs e)
		{
			base.OnGotFocus(e);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Enter"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnEnter(EventArgs e)
		{
			this.Focus();
			if ((_month.DayInFocus == -1) && (_activeRegion != NuGenCalendarRegion.Month))
			{
				for (int i = 0; i < 42; i++)
				{
					if (_month.m_days[i].State == NuGenDayState.Normal)
					{
						_month.DayInFocus = i;
						_month.m_days[_month.DayInFocus].State = NuGenDayState.Focus;
						Invalidate();
						break;
					}
				}
			}

			base.OnEnter(e);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Leave"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnLeave(EventArgs e)
		{
			base.OnEnter(e);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.DragDrop"></see> event.
		/// </summary>
		/// <param name="drgevent">A <see cref="T:System.Windows.Forms.DragEventArgs"></see> that contains the event data.</param>
		protected override void OnDragDrop(DragEventArgs drgevent)
		{
			base.OnDragDrop(drgevent);

			int day = _month.GetDay(_mouseLocation);
			if (day != -1)
			{
				this.OnDayDragDrop(new NuGenDayDragDropEventArgs(drgevent.Data, drgevent.KeyState, _month.m_days[day].Date.ToShortDateString()));
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.DragEnter"></see> event.
		/// </summary>
		/// <param name="drgevent">A <see cref="T:System.Windows.Forms.DragEventArgs"></see> that contains the event data.</param>
		protected override void OnDragEnter(DragEventArgs drgevent)
		{
			base.OnDragEnter(drgevent);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.DragOver"></see> event.
		/// </summary>
		/// <param name="drgevent">A <see cref="T:System.Windows.Forms.DragEventArgs"></see> that contains the event data.</param>
		protected override void OnDragOver(DragEventArgs drgevent)
		{
			base.OnDragOver(drgevent);
			if ((drgevent.AllowedEffect & DragDropEffects.Move) == DragDropEffects.Move)
			{
				// By default, the drop action should be move, if allowed.
				drgevent.Effect = DragDropEffects.Move;
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.DragLeave"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnDragLeave(EventArgs e)
		{
			base.OnDragLeave(e);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.SystemColorsChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnSystemColorsChanged(EventArgs e)
		{
			base.OnSystemColorsChanged(e);
			if (UseTheme)
				GetThemeColors();
		}

		/// <summary>
		/// </summary>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{

			base.WndProc(ref m);
			switch (m.Msg)
			{
				case WinUser.WM_THEMECHANGED:
				{
					// Theme has changed , get new colors if Theme = true
					if (UseTheme)
						GetThemeColors();
					break;
				}
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			// set location and button
			_mouseLocation = new Point(e.X, e.Y);
			_mouseButton = e.Button;

			_month.Click(_mouseLocation, _mouseButton);

			if (ShowHeader)
				_header.MouseClick(_mouseLocation, _mouseButton, NuGenClickMode.Single);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			if (ShowHeader)
				_header.MouseUp();
			_month.MouseUp();
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			// set location and button
			_mouseLocation = new Point(e.X, e.Y);
			_mouseButton = e.Button;

			if (ShowHeader)
				_header.MouseMove(_mouseLocation);
			_month.MouseMove(_mouseLocation);
			if (ShowFooter)
				_footer.MouseMove(_mouseLocation);
			if (ShowWeekdays)
				_weekday.MouseMove(_mouseLocation);
			if (ShowWeeknumbers)
				_weeknumber.MouseMove(_mouseLocation);

		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnMouseLeave(EventArgs e)
		{
			ActiveRegion = NuGenCalendarRegion.None;
			_month.RemoveFocus();
			base.OnMouseLeave(e);
			Invalidate();
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Click"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnClick(EventArgs e)
		{
			base.OnClick(e);
			this.Focus();
			if (ShowWeekdays)
				_weekday.MouseClick(_mouseLocation, _mouseButton, NuGenClickMode.Single);
			if (ShowWeeknumbers)
				_weeknumber.MouseClick(_mouseLocation, _mouseButton, NuGenClickMode.Single);
			if (ShowFooter)
				_footer.MouseClick(_mouseLocation, _mouseButton, NuGenClickMode.Single);

		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.DoubleClick"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnDoubleClick(EventArgs e)
		{
			base.OnDoubleClick(e);

			_month.DoubleClick(_mouseLocation, _mouseButton);

			if (ShowWeekdays)
				_weekday.MouseClick(_mouseLocation, _mouseButton, NuGenClickMode.Double);
			if (ShowWeeknumbers)
				_weeknumber.MouseClick(_mouseLocation, _mouseButton, NuGenClickMode.Double);
			if (ShowHeader)
				_header.MouseClick(_mouseLocation, _mouseButton, NuGenClickMode.Double);
			if (ShowFooter)
				_footer.MouseClick(_mouseLocation, _mouseButton, NuGenClickMode.Double);

		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Draw(e.Graphics, e.ClipRectangle);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Resize"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			DoLayout();
		}

		#endregion

		#region Methods.Private

		private DayOfWeek IntToDayOfWeek(int d)
		{

			switch (d)
			{
				case 0:
				return DayOfWeek.Sunday;
				case 1:
				return DayOfWeek.Monday;
				case 2:
				return DayOfWeek.Tuesday;
				case 3:
				return DayOfWeek.Wednesday;
				case 4:
				return DayOfWeek.Thursday;
				case 5:
				return DayOfWeek.Friday;
				case 6:
				return DayOfWeek.Saturday;
				default:
				return DayOfWeek.Friday; // should never be used.  	 
			}
		}

		private void Draw(Graphics e, Rectangle clip)
		{

			if ((ShowHeader) && (_header.IsVisible(clip)))
				_header.Draw(e);
			if ((ShowWeekdays) && (_weekday.IsVisible(clip)))
				_weekday.Draw(e);
			if ((ShowWeeknumbers) && (_weeknumber.IsVisible(clip)))
				_weeknumber.Draw(e);
			if ((ShowFooter) && (_footer.IsVisible(clip)))
				_footer.Draw(e);

			if (_month.IsVisible(clip))
				_month.Draw(e);

			// Draw border
			ControlPaint.DrawBorder(e, this.ClientRectangle, _borderColor, _borderStyle);

		}

		private void GetThemeColors()
		{
			NuGenControlState currentState = this.StateTracker.GetControlState();

			_month.Colors.Selected.Border = this.Renderer.GetMonthSelectedBorderColor();
			this.BorderColor = this.Renderer.GetBorderColor(currentState);

			_month.Colors.Selected.BackColor = this.Renderer.GetMonthSelectedBackColor();
			_month.Colors.Focus.BackColor = this.Renderer.GetMonthFocusedBackColor();
			_month.Colors.Focus.Border = this.Renderer.GetMonthFocusedBorderColor();

			_header.GradientMode = this.Renderer.GetHeaderGradientMode(currentState);
			_header.BackColor1 = this.Renderer.GetHeaderBackColorBegin(currentState);
			_header.BackColor2 = this.Renderer.GetHeaderBackColorEnd(currentState);
			_header.TextColor = this.Renderer.GetHeaderTextColor(currentState);

			_footer.GradientMode = this.Renderer.GetFooterGradientMode(currentState);
			_footer.BackColor1 = this.Renderer.GetFooterBackColorBegin(currentState);
			_footer.BackColor2 = this.Renderer.GetFooterBackColorEnd(currentState);
			_footer.TextColor = this.Renderer.GetFooterTextColor(currentState);

			_weekday.GradientMode = this.Renderer.GetWeekdayGradientMode(currentState);
			_weekday.BackColor1 = this.Renderer.GetWeekdayBackColorBegin(currentState);
			_weekday.BackColor2 = this.Renderer.GetWeekdayBackColorEnd(currentState);
			_weekday.TextColor = this.Renderer.GetWeekdayTextColor(currentState);

			_weeknumber.GradientMode = this.Renderer.GetWeeknumberGradientMode(currentState);
			_weeknumber.BackColor1 = this.Renderer.GetWeeknumberBackColorBegin(currentState);
			_weeknumber.BackColor2 = this.Renderer.GetWeeknumberBackColorEnd(currentState);
			_weeknumber.TextColor = this.Renderer.GetWeeknumberTextColor(currentState);

			Invalidate();
		}

		internal void Setup()
		{
			_month.Setup();
		}

		internal string[] AllowedMonths()
		{
			string[] monthList = new string[12];
			string[] months = _dateTimeFormat.MonthNames;
			monthList.Initialize();

			for (int i = 0; i < 12; i++)
				monthList[i] = months[i];

			return monthList;

		}

		internal void DrawGradient(Graphics e, Rectangle rect, Color color1, Color color2, NuGenGradientMode mode)
		{
			LinearGradientMode gradient = LinearGradientMode.BackwardDiagonal;
			if (mode == NuGenGradientMode.Vertical)
				gradient = LinearGradientMode.Vertical;
			else if (mode == NuGenGradientMode.Horizontal)
				gradient = LinearGradientMode.Horizontal;
			else if (mode == NuGenGradientMode.BackwardDiagonal)
				gradient = LinearGradientMode.BackwardDiagonal;
			else if (mode == NuGenGradientMode.ForwardDiagonal)
				gradient = LinearGradientMode.ForwardDiagonal;

			LinearGradientBrush gb = new LinearGradientBrush(rect, color1, color2, gradient);
			e.FillRectangle(gb, rect);
			gb.Dispose();
		}

		internal string[] DayNames()
		{
			string[] dayList = new string[8];
			string[] days = _dateTimeFormat.DayNames;
			dayList.Initialize();

			dayList[0] = "Default";
			for (int i = 1; i <= 7; i++)
				dayList[i] = days[i - 1];

			return dayList;

		}

		internal bool IsYearValid(string y)
		{
			string[] years = AllowedYears();
			bool ret = false;
			for (int i = 0; i < years.Length; i++)
			{
				if (y == years[i])
					ret = true;
			}
			return ret;
		}

		internal int MonthNumber(string m)
		{
			int ret = -1;
			string[] months;
			months = AllowedMonths();

			for (int i = 0; i < months.Length; i++)
			{
				if (m.CompareTo(months[i]) == 0)
					return i + 1;
			}
			if ((Convert.ToInt32(m) >= 1) && (Convert.ToInt32(m) <= 12))
			{
				ret = Convert.ToInt32(m);
			}
			return ret;
		}

		internal int DayNumber(string m)
		{
			int ret = 0;
			string[] days;
			days = DayNames();

			for (int i = 0; i < days.Length; i++)
			{
				if (m.CompareTo(days[i]) == 0)
					return i;
			}
			if ((Convert.ToInt32(m) >= 0) && (Convert.ToInt32(m) < 8))
			{
				ret = Convert.ToInt32(m);
			}
			return ret;
		}

		internal string MonthName(int m)
		{
			string[] validNames;
			string name = "";
			validNames = AllowedMonths();
			if ((m >= 1) && (m <= 12))
			{
				name = validNames[m - 1];
			}
			return name;
		}

		internal string[] AllowedYears()
		{

			string[] yearList = new string[(_maxDate.Year - _minDate.Year) + 1];

			yearList.Initialize();

			int year;

			year = 0;
			for (int i = _minDate.Year; i <= _maxDate.Year; i++)
			{
				yearList[year] = i.ToString();
				year++;
			}

			return yearList;
		}

		internal void DoLayout()
		{
			int y = 0;
			int x = 0;

			Graphics g;
			SizeF weekSize = new SizeF();

			g = this.CreateGraphics();
			weekSize = g.MeasureString("99", _weeknumber.Font);

			if (ShowHeader)
			{
				if (_header.Font.Height > 31)
					y = 2 + this.Font.Height + 2;
				else
					y = 31;

				_headerRect = new Rectangle(0, 0, this.Width, y);
			}
			else
			{
				_headerRect = new Rectangle(0, 0, 0, 0);
			}

			if (ShowWeeknumbers)
				x = 2 + (int)weekSize.Width + 2;

			_weekdaysRect.Height = 2 + _weekday.Font.Height + 2;
			if (ShowWeekdays)
			{
				_weekdaysRect.Y = y;
				_weekdaysRect.Width = this.Width - x;
				_weekdaysRect.X = x;
				y = y + _weekdaysRect.Height;
			}
			else
				_weekdaysRect = new Rectangle(0, 0, 0, 0);

			if (ShowWeeknumbers)
				_weeknumbersRect = new Rectangle(0, y - 1, x, this.Height - y + 1);


			_monthRect.Y = y;
			_monthRect.X = x;
			_monthRect.Width = this.Width - x;

			if (ShowFooter)
			{
				_footerRect.Height = 2 + _footer.Font.Height + 2;
				_footerRect.Y = this.Height - _footerRect.Height;
				_footerRect.X = 0;
				_footerRect.Width = this.Width;
				_monthRect.Height = this.Height - _footerRect.Height - y;
				_weeknumbersRect.Height -= _footerRect.Height;
			}
			else
			{
				_footerRect = new Rectangle(0, 0, 0, 0);
				_monthRect.Height = this.Height - y;
			}

			_month.Rect = _monthRect;
			_month.SetupDays();

			_footer.Rect = _footerRect;
			_header.Rect = _headerRect;
			_weeknumber.Rect = _weeknumbersRect;
			_weekday.Rect = _weekdaysRect;

			g.Dispose();
		}

		#endregion

		#region EventHandlers

		private void m_hook_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.LControlKey:
				case Keys.RControlKey:
				{
					if (_extendedKey == NuGenExtendedSelectionKey.Ctrl)
						_ctrlKey = true;
					break;
				}
				case Keys.LShiftKey:
				case Keys.RShiftKey:
				{
					if (_extendedKey == NuGenExtendedSelectionKey.Shift)
						_ctrlKey = true;
					break;
				}
				case Keys.LMenu:
				{
					if (_extendedKey == NuGenExtendedSelectionKey.Alt)
						_ctrlKey = true;
					break;
				}

				default:
				{
					if (_keyboardEnabled)
					{
						if (Focused)
						{
							_keyHandled = false;
							if (e.KeyCode == _keyboard.Left)
							{
								if (!_selectKeyDown)
									MoveFocus(-1);
								else if ((_month.DayInFocus - 1 >= 0) && (_month.m_days[_month.DayInFocus - 1].Rectangle.Y == _month.m_days[_month.DayInFocus].Rectangle.Y))
									_month.FocusMoved(_month.DayInFocus - 1);
								_keyHandled = true;
							}
							if (e.KeyCode == _keyboard.Right)
							{
								if (!_selectKeyDown)
									MoveFocus(1);
								else if ((_month.DayInFocus + 1 <= 41) && (_month.m_days[_month.DayInFocus + 1].Rectangle.Y == _month.m_days[_month.DayInFocus].Rectangle.Y))
									_month.FocusMoved(_month.DayInFocus + 1);
								_keyHandled = true;
							}
							if (e.KeyCode == _keyboard.Up)
							{
								if (!_selectKeyDown)
									MoveFocus(-7);
								else if (_month.DayInFocus - 7 >= 0)
									_month.FocusMoved(_month.DayInFocus - 7);
								_keyHandled = true;
							}
							if (e.KeyCode == _keyboard.Down)
							{
								if (!_selectKeyDown)
									MoveFocus(7);
								else if (_month.DayInFocus + 7 <= 41)
									_month.FocusMoved(_month.DayInFocus + 7);
								_keyHandled = true;
							}
							if (e.KeyCode == _keyboard.NextMonth)
							{
								_keyHandled = true;
								ChangeMonth(1);
							}
							if (e.KeyCode == _keyboard.PreviousMonth)
							{
								_keyHandled = true;
								ChangeMonth(-1);
							}
							if (e.KeyCode == _keyboard.NextYear)
							{
								_keyHandled = true;
								ChangeMonth(12);
							}
							if (e.KeyCode == _keyboard.PreviousYear)
							{
								_keyHandled = true;
								ChangeMonth(-12);
							}
							if ((e.KeyCode == _keyboard.Select) && (_month.DayInFocus != -1))
							{
								_keyHandled = true;
								if (!_selectKeyDown)
								{
									_selectKeyDown = true;


									_month.DaySelect(_month.DayInFocus, _selectButton,
													  new Point(_month.m_days[_month.DayInFocus].Rectangle.X + 1,
																_month.m_days[_month.DayInFocus].Rectangle.Y + 1));
								}

							}

						}
					}

					break;
				}
			}
		}

		private void m_hook_KeyUp(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.LControlKey:
				case Keys.RControlKey:
				{
					if (_extendedKey == NuGenExtendedSelectionKey.Ctrl)
						_ctrlKey = false;
					break;
				}
				case Keys.LShiftKey:
				case Keys.RShiftKey:
				{
					if (_extendedKey == NuGenExtendedSelectionKey.Shift)
						_ctrlKey = false;
					break;
				}
				case Keys.LMenu:
				{
					if (_extendedKey == NuGenExtendedSelectionKey.Alt)
						_ctrlKey = false;
					break;
				}

				default:
				{
					if (_keyboardEnabled)
					{
						if (Focused)
						{
							if (e.KeyCode == _keyboard.Select)
							{
								_selectKeyDown = false;
								_month.MouseUp();
							}
						}
					}
					break;
				}

			}
		}

		private void MoveFocus(int step)
		{
			int focus = _month.DayInFocus;
			if ((focus + step >= 0) && (focus + step <= 41))
			{
				_keyHandled = true;
				_month.DayInFocus = focus + step;
				if (_month.m_days[focus + step].State == NuGenDayState.Normal)
					_month.m_days[focus + step].State = NuGenDayState.Focus;
				if ((focus <= 41) && (focus >= 0) && (_month.m_days[focus].State == NuGenDayState.Focus))
					_month.m_days[focus].State = NuGenDayState.Normal;

				Invalidate();
			}
			else
				_keyHandled = false;

		}


		private NuGenSelectedDatesCollection UpdateSelectedCollection()
		{
			_selectedDates.Clear();

			for (int i = 0; i < 42; i++)
			{
				if (_month.m_days[i].State == NuGenDayState.Selected)
				{
					_selectedDates.Add(_month.m_days[i].Date);
				}
			}

			return _selectedDates;
		}

		private void m_printDoc_BeginPrint(object sender, PrintEventArgs e)
		{

		}

		private void m_printDoc_PrintPage(object sender, PrintPageEventArgs e)
		{
			Bitmap bmp;
			bmp = Snapshot();
			e.Graphics.DrawImage(bmp, 1, 1, bmp.Width, bmp.Height);
			e.HasMorePages = false;
			bmp.Dispose();
		}

		private void m_printDoc_QueryPageSettings(object sender, QueryPageSettingsEventArgs e)
		{

		}

		private void m_activeMonth_MonthChanged(object sender, NuGenMonthChangedEventArgs e)
		{
			_month.RemoveSelection(true);
			this.OnMonthChanged(e);
		}

		void m_activeMonth_BeforeMonthChanged(object sender, NuGenBeforeMonthChangedEventArgs e)
		{
			this.OnBeforeMonthChanged(e);
		}


		private void m_month_DayRender(object sender, NuGenDayRenderEventArgs e)
		{
			this.OnDayRender(e);
		}

		private void m_month_DayQueryInfo(object sender, NuGenDayQueryInfoEventArgs e)
		{
			this.OnDayQueryInfo(e);
		}

		private void m_month_DaySelected(object sender, NuGenDaySelectedEventArgs e)
		{
			this.OnDaySelected(e);
		}

		private void m_month_DayDeselected(object sender, NuGenDaySelectedEventArgs e)
		{
			this.OnDayDeselected(e);
		}

		void m_month_BeforeDayDeselected(object sender, NuGenDayStateChangedEventArgs e)
		{
			this.OnBeforeDayDeselected(e);
		}

		void m_month_BeforeDaySelected(object sender, NuGenDayStateChangedEventArgs e)
		{
			this.OnBeforeDaySelected(e);
		}

		private void m_month_DayLostFocus(object sender, NuGenDayEventArgs e)
		{
			this.OnDayLostFocus(e);
		}

		private void m_month_DayGotFocus(object sender, NuGenDayEventArgs e)
		{
			this.OnDayGotFocus(e);
		}

		private void m_month_ImageClick(object sender, NuGenDayClickEventArgs e)
		{
			this.OnImageClick(e);
		}

		private void m_month_DayClick(object sender, NuGenDayClickEventArgs e)
		{
			this.OnDayClick(e);
		}

		private void m_month_DayMouseMove(object sender, NuGenDayMouseMoveEventArgs e)
		{
			this.OnDayMouseMove(e);
		}

		private void m_month_DayDoubleClick(object sender, NuGenDayClickEventArgs e)
		{
			this.OnDayDoubleClick(e);
		}

		private void m_footer_Click(object sender, NuGenClickEventArgs e)
		{
			this.OnFooterClick(e);
		}

		private void m_footer_DoubleClick(object sender, NuGenClickEventArgs e)
		{
			this.OnFooterDoubleClick(e);
		}

		private void m_header_Click(object sender, NuGenClickEventArgs e)
		{
			this.OnHeaderClick(e);
		}

		private void m_header_DoubleClick(object sender, NuGenClickEventArgs e)
		{
			this.OnHeaderDoubleClick(e);
		}

		private void m_header_PrevMonthButtonClick(object sender, EventArgs e)
		{
			ChangeMonth(-1);
		}

		private void m_header_NextMonthButtonClick(object sender, EventArgs e)
		{
			ChangeMonth(1);
		}

		private void ChangeMonth(int step)
		{
			int oldMonth = _month.SelectedMonth.Month;
			int oldYear = _month.SelectedMonth.Year;
			DateTime m = _month.SelectedMonth.AddMonths(step);
			int month = m.Month;
			int year = m.Year;
			_activeMonth.RaiseEvent = false;
			_activeMonth.Year = year;
			// check if user changed year
			if (oldMonth == month)
				// if so , make sure the BeforeMonthChanged event will trigger for month
				if (month + 1 < 12)
					_activeMonth.Month = month + 1;
				else
					_activeMonth.Month = month - 1;
			_activeMonth.RaiseEvent = true;
			_activeMonth.Month = month;
			if ((_activeMonth.Month == month) && (_activeMonth.Year == year))
				_month.SelectedMonth = m;
			else
			{
				_activeMonth.RaiseEvent = false;
				_activeMonth.Month = oldMonth;
				_activeMonth.Year = oldYear;
				_activeMonth.RaiseEvent = true;
			}

		}

		private void m_header_PrevYearButtonClick(object sender, EventArgs e)
		{
			ChangeMonth(-12);

		}

		private void m_header_NextYearButtonClick(object sender, EventArgs e)
		{
			ChangeMonth(12);
		}

		private void m_weekday_Click(object sender, NuGenWeekdayClickEventArgs e)
		{
			this.OnWeekdayClick(e);
		}

		private void m_weekday_DoubleClick(object sender, NuGenWeekdayClickEventArgs e)
		{
			this.OnWeekdayDoubleClick(e);
		}

		private void m_weeknumber_DoubleClick(object sender, NuGenWeeknumberClickEventArgs e)
		{
			this.OnWeeknumberDoubleClick(e);
		}

		private void m_weeknumber_Click(object sender, NuGenWeeknumberClickEventArgs e)
		{
			this.OnWeeknumberClick(e);
		}

		private void m_footer_PropertyChanged(object sender, NuGenFooterPropertyEventArgs e)
		{
			this.OnFooterPropertyChanged(e);
		}

		private void m_weeknumber_PropertyChanged(object sender, NuGenWeeknumberPropertyEventArgs e)
		{
			this.OnWeeknumberPropertyChanged(e);
		}

		private void m_weekday_PropertyChanged(object sender, NuGenWeekdayPropertyEventArgs e)
		{
			this.OnWeekdayPropertyChanged(e);
		}

		private void m_header_PropertyChanged(object sender, NuGenHeaderPropertyEventArgs e)
		{
			this.OnHeaderPropertyChanged(e);
		}

		private void m_month_PropertyChanged(object sender, NuGenMonthPropertyEventArgs e)
		{
			this.OnMonthPropertyChanged(e);
		}

		private void m_month_ColorChanged(object sender, NuGenMonthColorEventArgs e)
		{
			this.OnMonthColorChanged(e);
		}

		private void m_month_BorderStyleChanged(object sender, NuGenMonthBorderStyleEventArgs e)
		{
			this.OnMonthBorderStyleChanged(e);
		}

		private void m_dateItemCollection_DateItemModified(object sender, EventArgs e)
		{
			Invalidate();
		}


		#endregion

		#region KeyboardConfig

		/// <summary>
		/// </summary>
		[TypeConverter(typeof(NuGenKeyboardTypeConverter))]
		public class KeyboardConfig
		{
			private NuGenCalendar m_parent;
			private Keys m_up;
			private Keys m_down;
			private Keys m_left;
			private Keys m_right;
			private Keys m_select;
			private Keys m_nextMonth;
			private Keys m_prevMonth;
			private Keys m_nextYear;
			private Keys m_prevYear;

			/// <summary>
			/// Initializes a new instance of the <see cref="KeyboardConfig"/> class.
			/// </summary>
			public KeyboardConfig(NuGenCalendar calendar)
			{
				m_up = Keys.Up;
				m_parent = calendar;
				m_down = Keys.Down;
				m_left = Keys.Left;
				m_right = Keys.Right;
				m_select = Keys.Space;
				m_nextMonth = Keys.PageUp;
				m_prevMonth = Keys.PageDown;
				m_nextYear = Keys.Home;
				m_prevYear = Keys.End;

			}

			/// <summary>
			/// </summary>
			[Description("Key used to move up.")]
			[DefaultValue(typeof(Keys), "Up")]
			public Keys Up
			{
				get
				{
					return m_up;
				}
				set
				{
					if (m_up != value)
					{
						NuGenKeyboardChangedEventArgs args = new NuGenKeyboardChangedEventArgs(NuGenKeyboard.Up, m_up, value);
						m_up = value;
						m_parent.OnKeyboardChanged(args);
					}
				}
			}

			/// <summary>
			/// </summary>
			[Description("Key used to move down.")]
			[DefaultValue(typeof(Keys), "Down")]
			public Keys Down
			{
				get
				{
					return m_down;
				}
				set
				{
					NuGenKeyboardChangedEventArgs args = new NuGenKeyboardChangedEventArgs(NuGenKeyboard.Down, m_down, value);
					if (m_down != value)
					{
						m_down = value;
						m_parent.OnKeyboardChanged(args);
					}
				}
			}

			/// <summary>
			/// </summary>
			[Description("Key used to move left.")]
			[DefaultValue(typeof(Keys), "Left")]
			public Keys Left
			{
				get
				{
					return m_left;
				}
				set
				{
					NuGenKeyboardChangedEventArgs args = new NuGenKeyboardChangedEventArgs(NuGenKeyboard.Left, m_left, value);
					if (m_left != value)
					{
						m_left = value;
						m_parent.OnKeyboardChanged(args);
					}
				}
			}

			/// <summary>
			/// </summary>
			[Description("Key used to move right.")]
			[DefaultValue(typeof(Keys), "Right")]
			public Keys Right
			{
				get
				{
					return m_right;
				}
				set
				{
					NuGenKeyboardChangedEventArgs args = new NuGenKeyboardChangedEventArgs(NuGenKeyboard.Right, m_right, value);
					if (m_right != value)
					{
						m_right = value;
						m_parent.OnKeyboardChanged(args);
					}
				}
			}

			/// <summary>
			/// </summary>
			[Description("Key used to select.")]
			[DefaultValue(typeof(Keys), "Space")]
			public Keys Select
			{
				get
				{
					return m_select;
				}
				set
				{
					NuGenKeyboardChangedEventArgs args = new NuGenKeyboardChangedEventArgs(NuGenKeyboard.Select, m_select, value);
					if (m_select != value)
					{
						m_select = value;
						m_parent.OnKeyboardChanged(args);
					}
				}
			}

			/// <summary>
			/// </summary>
			[Description("Key used for next month.")]
			[DefaultValue(typeof(Keys), "PageUp")]
			public Keys NextMonth
			{
				get
				{
					return m_nextMonth;
				}
				set
				{
					NuGenKeyboardChangedEventArgs args = new NuGenKeyboardChangedEventArgs(NuGenKeyboard.NextMonth, m_nextMonth, value);
					if (m_nextMonth != value)
					{
						m_nextMonth = value;
						m_parent.OnKeyboardChanged(args);
					}
				}
			}

			/// <summary>
			/// </summary>
			[Description("Key used for previous month.")]
			[DefaultValue(typeof(Keys), "PageDown")]
			public Keys PreviousMonth
			{
				get
				{
					return m_prevMonth;
				}
				set
				{
					NuGenKeyboardChangedEventArgs args = new NuGenKeyboardChangedEventArgs(NuGenKeyboard.PreviousMonth, m_prevMonth, value);
					if (m_prevMonth != value)
					{
						m_prevMonth = value;
						m_parent.OnKeyboardChanged(args);
					}
				}
			}

			/// <summary>
			/// </summary>
			[Description("Key used for next year.")]
			[DefaultValue(typeof(Keys), "Home")]
			public Keys NextYear
			{
				get
				{
					return m_nextYear;
				}
				set
				{
					NuGenKeyboardChangedEventArgs args = new NuGenKeyboardChangedEventArgs(NuGenKeyboard.NextYear, m_nextYear, value);
					if (m_nextYear != value)
					{
						m_nextYear = value;
						m_parent.OnKeyboardChanged(args);
					}
				}
			}


			/// <summary>
			/// </summary>
			[Description("Key used for previous year.")]
			[DefaultValue(typeof(Keys), "End")]
			public Keys PreviousYear
			{
				get
				{
					return m_prevYear;
				}
				set
				{
					NuGenKeyboardChangedEventArgs args = new NuGenKeyboardChangedEventArgs(NuGenKeyboard.PreviousYear, m_prevYear, value);
					if (m_prevYear != value)
					{
						m_prevYear = value;
						m_parent.OnKeyboardChanged(args);
					}
				}
			}
		}

		#endregion

		private Color _borderColor;
		private bool _ctrlKey;
		private GlobalHook _hook = new GlobalHook();
		private bool _keyHandled;
		private bool _selectKeyDown;
		private bool _keyboardEnabled;
		private bool m_useTheme;

		private NuGenExtendedSelectionKey _extendedKey;

		private NuGenWeekday _weekday;
		private NuGenMonth _month;
		private NuGenFooter _footer;
		private NuGenWeeknumber _weeknumber;
		private int _firstDayOfWeek;
		private DayOfWeek _defaultFirstDayOfWeek;

		private NuGenCalendarRegion _activeRegion;
		private NuGenHeader _header;
		private KeyboardConfig _keyboard;
		private bool _showToday;
		private bool _showTrailing;
		private IntPtr _theme;
		private bool _selectTrailing;
		private NuGenSelectionMode _selectionMode;
		private CultureInfo[] _installedCultures;
		private CultureInfo _culture;

		private ImageList _imageList;
		private PrintDocument _printDoc = new PrintDocument();

		internal DateTimeFormatInfo _dateTimeFormat = new DateTimeFormatInfo();

		private Rectangle _weekdaysRect = new Rectangle();
		private Rectangle _monthRect = new Rectangle();
		private Rectangle _footerRect = new Rectangle();
		private Rectangle _headerRect = new Rectangle();
		private Rectangle _weeknumbersRect = new Rectangle();
		private NuGenDateItemCollection _dateItemCollection;
		private NuGenSelectedDatesCollection _selectedDates;

		private ButtonBorderStyle _borderStyle;
		private bool _showFocus;
		private MouseButtons _selectButton;

		private DateTime _minDate;
		private DateTime _maxDate;

		private Color _todayColor;

		private bool _showFooter;
		private bool _showWeekday;
		private bool _showHeader;
		private bool _showWeeknumber;

		private NuGenActiveMonth _activeMonth;

		/// <summary>
		/// </summary>
		[field: NonSerialized]
		public NuGenWeekCallback WeeknumberCallBack;

		private Point _mouseLocation;
		private MouseButtons _mouseButton;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCalendar"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// <para>Requires:</para>
		/// <para><see cref="INuGenControlStateService"/></para>
		/// <para><see cref="INuGenCalendarRenderer"/></para>
		/// </param>
		public NuGenCalendar(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.SetStyle(ControlStyles.DoubleBuffer, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

			_borderColor = Color.Black;
			_selectButton = MouseButtons.Left;
			_extendedKey = NuGenExtendedSelectionKey.Ctrl;

			_activeRegion = NuGenCalendarRegion.None;
			_selectionMode = NuGenSelectionMode.MultiSimple;
			_dateTimeFormat = DateTimeFormatInfo.CurrentInfo;
			_theme = IntPtr.Zero;

			_installedCultures = CultureInfo.GetCultures(CultureTypes.InstalledWin32Cultures);
			_culture = CultureInfo.CurrentCulture;

			_showToday = true;
			_showTrailing = true;
			_showFocus = true;
			_todayColor = Color.Red;
			//initiate regions	
			_weekday = new NuGenWeekday(this);
			_month = new NuGenMonth(this);
			_footer = new NuGenFooter(this);
			_weeknumber = new NuGenWeeknumber(this);
			_header = new NuGenHeader(this, serviceProvider);

			_keyboard = new KeyboardConfig(this);
			_keyboardEnabled = true;
			_activeMonth = new NuGenActiveMonth(this);
			_dateItemCollection = new NuGenDateItemCollection(this);
			_selectedDates = new NuGenSelectedDatesCollection(this);

			// setup callback for weeknumbers
			WeeknumberCallBack = new NuGenWeekCallback(_weeknumber.CalcWeek);

			// setup internal events
			_hook.KeyDown += new KeyEventHandler(m_hook_KeyDown);
			_hook.KeyUp += new KeyEventHandler(m_hook_KeyUp);

			_dateItemCollection.DateItemModified += m_dateItemCollection_DateItemModified;

			_month.DayRender += m_month_DayRender;
			_month.DayQueryInfo += m_month_DayQueryInfo;
			_month.DayLostFocus += m_month_DayLostFocus;
			_month.DayGotFocus += m_month_DayGotFocus;
			_month.ImageClick += m_month_ImageClick;
			_month.DayMouseMove += m_month_DayMouseMove;
			_month.DayClick += m_month_DayClick;
			_month.DayDoubleClick += m_month_DayDoubleClick;
			_month.DaySelected += m_month_DaySelected;
			_month.DayDeselected += m_month_DayDeselected;
			_month.ColorChanged += m_month_ColorChanged;
			_month.BorderStyleChanged += m_month_BorderStyleChanged;
			_month.PropertyChanged += m_month_PropertyChanged;
			_month.BeforeDaySelected += m_month_BeforeDaySelected;
			_month.BeforeDayDeselected += m_month_BeforeDayDeselected;

			_footer.Click += m_footer_Click;
			_footer.DoubleClick += m_footer_DoubleClick;
			_footer.PropertyChanged += m_footer_PropertyChanged;

			_weeknumber.PropertyChanged += m_weeknumber_PropertyChanged;
			_weeknumber.Click += m_weeknumber_Click;
			_weeknumber.DoubleClick += m_weeknumber_DoubleClick;

			_weekday.PropertyChanged += m_weekday_PropertyChanged;
			_weekday.Click += m_weekday_Click;
			_weekday.DoubleClick += m_weekday_DoubleClick;

			_header.PropertyChanged += m_header_PropertyChanged;
			_header.Click += m_header_Click;
			_header.DoubleClick += m_header_DoubleClick;
			_header.PrevMonthButtonClick += m_header_PrevMonthButtonClick;
			_header.NextMonthButtonClick += m_header_NextMonthButtonClick;
			_header.PrevYearButtonClick += m_header_PrevYearButtonClick;
			_header.NextYearButtonClick += m_header_NextYearButtonClick;


			_activeMonth.MonthChanged += m_activeMonth_MonthChanged;
			_activeMonth.BeforeMonthChanged += m_activeMonth_BeforeMonthChanged;

			_printDoc.BeginPrint += m_printDoc_BeginPrint;
			_printDoc.PrintPage += m_printDoc_PrintPage;
			_printDoc.QueryPageSettings += m_printDoc_QueryPageSettings;

			_borderStyle = ButtonBorderStyle.Solid;

			_printDoc.DocumentName = "MonthCalendar";

			_showFooter = true;
			_showHeader = true;
			_showWeekday = true;

			_selectTrailing = true;
			_selectKeyDown = false;

			_activeMonth.Month = DateTime.Today.Month;
			_activeMonth.Year = DateTime.Today.Year;

			_minDate = DateTime.Now.AddYears(-10);
			_maxDate = DateTime.Now.AddYears(10);

			_month.SelectedMonth = DateTime.Parse(_activeMonth.Year + "-" + _activeMonth.Month + "-01");

			_hook.InstallKeyboardHook();
			_keyHandled = false;
			this.Size = new Size(250, 250);
			Setup();
			this.UseTheme = true;
		}

		/// <summary>
		/// </summary>
		/// <param name="disposing">
		/// <see langword="true"/> to dispose both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.
		/// </param>
		protected override void Dispose(bool disposing)
		{
			_hook.RemoveKeyboardHook();

			if (disposing)
			{
				_hook.KeyDown -= new KeyEventHandler(m_hook_KeyDown);
				_hook.KeyUp -= new KeyEventHandler(m_hook_KeyUp);

				_activeMonth.MonthChanged -= m_activeMonth_MonthChanged;
				_activeMonth.BeforeMonthChanged -= m_activeMonth_BeforeMonthChanged;

				_dateItemCollection.DateItemModified -= m_dateItemCollection_DateItemModified;

				_month.DayRender -= m_month_DayRender;
				_month.DayQueryInfo -= m_month_DayQueryInfo;
				_month.DayLostFocus -= m_month_DayLostFocus;
				_month.DayGotFocus -= m_month_DayGotFocus;
				_month.ImageClick -= m_month_ImageClick;
				_month.DayMouseMove -= m_month_DayMouseMove;
				_month.DayClick -= m_month_DayClick;
				_month.DayDoubleClick -= m_month_DayDoubleClick;
				_month.DaySelected -= m_month_DaySelected;
				_month.DayDeselected -= m_month_DayDeselected;
				_month.ColorChanged -= m_month_ColorChanged;
				_month.BorderStyleChanged -= m_month_BorderStyleChanged;
				_month.PropertyChanged -= m_month_PropertyChanged;
				_month.BeforeDaySelected -= m_month_BeforeDaySelected;
				_month.BeforeDayDeselected -= m_month_BeforeDayDeselected;


				_footer.Click -= m_footer_Click;
				_footer.DoubleClick -= m_footer_DoubleClick;
				_footer.PropertyChanged -= m_footer_PropertyChanged;

				_weeknumber.PropertyChanged -= m_weeknumber_PropertyChanged;
				_weeknumber.Click -= m_weeknumber_Click;
				_weeknumber.DoubleClick -= m_weeknumber_DoubleClick;

				_weekday.PropertyChanged -= m_weekday_PropertyChanged;
				_weekday.Click -= m_weekday_Click;
				_weekday.DoubleClick -= m_weekday_DoubleClick;

				_header.PropertyChanged -= m_header_PropertyChanged;
				_header.Click -= m_header_Click;
				_header.DoubleClick -= m_header_DoubleClick;
				_header.PrevMonthButtonClick -= m_header_PrevMonthButtonClick;
				_header.NextMonthButtonClick -= m_header_NextMonthButtonClick;
				_header.PrevYearButtonClick -= m_header_PrevYearButtonClick;
				_header.NextYearButtonClick -= m_header_NextYearButtonClick;


				_printDoc.BeginPrint -= m_printDoc_BeginPrint;
				_printDoc.PrintPage -= m_printDoc_PrintPage;
				_printDoc.QueryPageSettings -= m_printDoc_QueryPageSettings;

				_printDoc.Dispose();
				_header.Dispose();
				_weeknumber.Dispose();
				_weekday.Dispose();
				_month.Dispose();
				_footer.Dispose();

				_hook.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}
