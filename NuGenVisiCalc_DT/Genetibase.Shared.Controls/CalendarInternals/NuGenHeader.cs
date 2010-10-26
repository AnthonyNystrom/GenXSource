/* -----------------------------------------------
 * Header.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ScrollBarInternals;
using Genetibase.Shared.Drawing;

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Windows.Forms.VisualStyles;

namespace Genetibase.Shared.Controls.CalendarInternals
{
	/// <summary>
	/// </summary>
	[TypeConverter(typeof(NuGenHeaderConverter))]
	public sealed class NuGenHeader : IDisposable
	{
		internal event EventHandler<NuGenClickEventArgs> Click;
		internal event EventHandler<NuGenClickEventArgs> DoubleClick;
		internal event EventHandler PrevMonthButtonClick;
		internal event EventHandler NextMonthButtonClick;
		internal event EventHandler PrevYearButtonClick;
		internal event EventHandler NextYearButtonClick;
		internal event EventHandler<NuGenHeaderPropertyEventArgs> PropertyChanged;

		private bool disposed;
		private NuGenCalendar _calendar;
		private Color _backColor1;
		private Color _backColor2;
		private NuGenGradientMode _gradientMode;
		private Color _textColor;
		private Font _font;
		private bool _monthSelector;
		private bool _yearSelector;
		private bool _contextMenu;

		private ContextMenu _monthMenu = new ContextMenu();
		private Rectangle _rect;
		private Region _region;
		private Rectangle _nextBtnRect;
		private Rectangle _prevBtnRect;
		private Rectangle _nextYearBtnRect;
		private Rectangle _prevYearBtnRect;
		private Rectangle _textRect;

		private HeaderButtonState _prevBtnState;
		private HeaderButtonState _nextBtnState;
		private HeaderButtonState _prevYearBtnState;
		private HeaderButtonState _nextYearBtnState;

		private string _text;
		private StringAlignment _align;

		private bool _showMonth;
		private INuGenServiceProvider _serviceProvider;

		#region EventHandlers

		private void MonthContextMenu_Click(object sender, EventArgs e)
		{
			MenuItem item = (MenuItem)sender;
			_calendar.ActiveMonth.Month = item.Index + 1;
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenHeader"/> class.
		/// </summary>
		/// <param name="calendar"></param>
		/// <param name="serviceProvider">
		/// <para>Requires:</para>
		/// <para><see cref="INuGenCalendarRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="calendar"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="serviceProvider"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public NuGenHeader(NuGenCalendar calendar, INuGenServiceProvider serviceProvider)
		{
			if (calendar == null)
			{
				throw new ArgumentNullException("calendar");
			}

			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			_calendar = calendar;
			_serviceProvider = serviceProvider;
			_backColor1 = Color.FromArgb(0, 84, 227);
			_backColor2 = Color.White;
			_gradientMode = NuGenGradientMode.None;
			_textColor = Color.White;
			_font = new Font("Microsoft Sans Serif", (float)8.25, FontStyle.Bold);
			_showMonth = true;
			_monthSelector = true;
			_text = "";
			_contextMenu = true;
			_align = StringAlignment.Center;
			_prevBtnState = HeaderButtonState.Normal;
			_nextBtnState = HeaderButtonState.Normal;
			_prevYearBtnState = HeaderButtonState.Normal;
			_nextYearBtnState = HeaderButtonState.Normal;

			// create monthContext menu and setup event handlers
			for (int k = 0; k < 12; k++)
			{
				_monthMenu.MenuItems.Add(_monthMenu.MenuItems.Count,
					new MenuItem(""));
				_monthMenu.MenuItems[_monthMenu.MenuItems.Count - 1].Click += new EventHandler(MonthContextMenu_Click);

			}

			Setup();
		}

		/// <summary>
		/// </summary>
		/// <param name="disposing"><see langword="true"/> to dispose both managed and unmanaged resources; <see langword="false"/> to dispose only unmanaged resources.</param>
		private void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					// Remove event handlers
					for (int k = 0; k < _monthMenu.MenuItems.Count; k++)
					{
						_monthMenu.MenuItems[k].Click -= new EventHandler(MonthContextMenu_Click);
					}

					_font.Dispose();
					_region.Dispose();
					_monthMenu.Dispose();

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

		internal Rectangle Rect
		{
			get
			{
				return _rect;
			}
			set
			{
				_rect = value;
				_region = new Region(_rect);
				Setup();
			}
		}

		/// <summary>
		/// </summary>
		[Description("Determines if the month selection menu should be displayed when right clicking the header.")]
		[DefaultValue(true)]
		public bool MonthContextMenu
		{
			get
			{
				return _contextMenu;
			}
			set
			{
				if (_contextMenu != value)
				{
					_contextMenu = value;
					if (PropertyChanged != null)
						PropertyChanged(this, new NuGenHeaderPropertyEventArgs(NuGenHeaderProperty.MonthContextMenu));
					_calendar.Invalidate();
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
				return _align;
			}
			set
			{
				if (_align != value)
				{
					_align = value;
					if (PropertyChanged != null)
						PropertyChanged(this, new NuGenHeaderPropertyEventArgs(NuGenHeaderProperty.Align));
					_calendar.Invalidate();
				}
			}
		}

		/// <summary>
		/// </summary>
		[Description("Determines wether the month selector buttons should be displayed.")]
		[DefaultValue(true)]
		public bool MonthSelectors
		{
			get
			{
				return _monthSelector;
			}
			set
			{
				if (_monthSelector != value)
				{
					_monthSelector = value;

					Setup();

					if (PropertyChanged != null)
						PropertyChanged(this, new NuGenHeaderPropertyEventArgs(NuGenHeaderProperty.MonthSelectors));
					_calendar.Invalidate();
				}
			}
		}

		/// <summary>
		/// </summary>
		[Description("Determines wether the year selector buttons should be displayed.")]
		[DefaultValue(false)]
		public bool YearSelectors
		{
			get
			{
				return _yearSelector;
			}
			set
			{
				if (_yearSelector != value)
				{
					_yearSelector = value;

					Setup();

					if (PropertyChanged != null)
						PropertyChanged(this, new NuGenHeaderPropertyEventArgs(NuGenHeaderProperty.YearSelectors));
					_calendar.Invalidate();
				}
			}
		}


		/// <summary>
		/// </summary>
		[Description("Determines wether the current month should be displayed.")]
		[DefaultValue(true)]
		public bool ShowMonth
		{
			get
			{
				return _showMonth;
			}
			set
			{
				if (_showMonth != value)
				{
					_showMonth = value;
					if (PropertyChanged != null)
						PropertyChanged(this, new NuGenHeaderPropertyEventArgs(NuGenHeaderProperty.ShowMonth));
					_calendar.Invalidate();
				}
			}
		}

		/// <summary>
		/// </summary>
		[Description("Text to be displayed in the header.")]
		[DefaultValue("")]
		public string Text
		{
			get
			{
				return _text;
			}
			set
			{
				if (_text != value)
				{
					_text = value;
					if (PropertyChanged != null)
						PropertyChanged(this, new NuGenHeaderPropertyEventArgs(NuGenHeaderProperty.Text));
					_calendar.Invalidate();
				}
			}
		}

		/// <summary>
		/// </summary>
		[Description("Color used for background.")]
		[DefaultValue(typeof(Color), "0,84,227")]
		public Color BackColor1
		{
			get
			{
				return _backColor1;
			}
			set
			{
				if (_backColor1 != value)
				{
					_backColor1 = value;
					if (PropertyChanged != null)
						PropertyChanged(this, new NuGenHeaderPropertyEventArgs(NuGenHeaderProperty.BackColor1));
					_calendar.Invalidate();
				}
			}
		}

		/// <summary>
		/// </summary>
		[Description("Second color used for background when using a gradient.")]
		[DefaultValue(typeof(Color), "White")]
		public Color BackColor2
		{
			get
			{
				return _backColor2;
			}
			set
			{
				if (_backColor2 != value)
				{
					_backColor2 = value;
					if (PropertyChanged != null)
						PropertyChanged(this, new NuGenHeaderPropertyEventArgs(NuGenHeaderProperty.BackColor2));
					_calendar.Invalidate();
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
				return _gradientMode;
			}
			set
			{
				if (_gradientMode != value)
				{
					_gradientMode = value;
					if (PropertyChanged != null)
						PropertyChanged(this, new NuGenHeaderPropertyEventArgs(NuGenHeaderProperty.GradientMode));
					_calendar.Invalidate();
				}
			}
		}

		/// <summary>
		/// </summary>
		[Description("Font used for header.")]
		[DefaultValue(typeof(Font), "Microsoft Sans Serif; 8,25pt")]
		public Font Font
		{
			get
			{
				return _font;
			}
			set
			{
				if (_font != value)
				{
					_font = value;
					_calendar.DoLayout();
					if (PropertyChanged != null)
						PropertyChanged(this, new NuGenHeaderPropertyEventArgs(NuGenHeaderProperty.Font));
					_calendar.Invalidate();
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
				return _textColor;
			}
			set
			{
				if (_textColor != value)
				{
					_textColor = value;
					if (PropertyChanged != null)
						PropertyChanged(this, new NuGenHeaderPropertyEventArgs(NuGenHeaderProperty.TextColor));
					_calendar.Invalidate();
				}
			}
		}

		#endregion

		#region Properties.Services

		private INuGenCalendarRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		private INuGenCalendarRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					Debug.Assert(_serviceProvider != null, "_serviceProvider != null");
					_renderer = _serviceProvider.GetService<INuGenCalendarRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenCalendarRenderer>();
					}
				}

				return _renderer;
			}
		}

		#endregion

		#region Methods

		private void Setup()
		{

			int x = 10;

			if (_yearSelector)
			{

				_prevYearBtnRect = new Rectangle(x, 5, 20, 20);
				_nextYearBtnRect = new Rectangle(_rect.Width - x - 20, 5, 20, 20);
				x += 20;
			}
			else
			{
				_prevYearBtnRect = new Rectangle(0, 0, 0, 0);
				_nextYearBtnRect = new Rectangle(0, 0, 0, 0);
			}
			if (_monthSelector)
			{

				_prevBtnRect = new Rectangle(x, 5, 20, 20);
				_nextBtnRect = new Rectangle(_rect.Width - x - 20, 5, 20, 20);
				x += 20;
			}
			else
			{
				_prevBtnRect = new Rectangle(0, 0, 0, 0);
				_nextBtnRect = new Rectangle(0, 0, 0, 0);
			}
			_textRect = new Rectangle(x + 2, 0, _rect.Width - (2 * x) - 8, _rect.Height);

		}

		private void DisplayMonthContextMenu(Point mouseLocation)
		{

			// Setup context menu
			string[] months = _calendar.AllowedMonths();
			for (int k = 0; k < months.Length; k++)
			{
				_monthMenu.MenuItems[k].Text = months[k];

				if (k == _calendar.ActiveMonth.Month - 1)
					_monthMenu.MenuItems[k].Checked = true;
				else
					_monthMenu.MenuItems[k].Checked = false;

			}
			//show context menu
			_monthMenu.Show(_calendar, new Point(mouseLocation.X, mouseLocation.Y));

		}

		internal void MouseClick(Point mouseLocation, MouseButtons button, NuGenClickMode mode)
		{
			Region leftBtnRgn = new Region(_prevBtnRect);
			Region rightBtnRgn = new Region(_nextBtnRect);
			Region leftYearBtnRgn = new Region(_prevYearBtnRect);
			Region rightYearBtnRgn = new Region(_nextYearBtnRect);
			MouseButtons selectButton;

			if (SystemInformation.MouseButtonsSwapped)
				selectButton = MouseButtons.Right;
			else
				selectButton = MouseButtons.Left;

			bool btnClick = false;

			if (_region.IsVisible(mouseLocation))
			{
				if (button == selectButton)
				{
					if (_monthSelector)
					{
						if ((leftBtnRgn.IsVisible(mouseLocation)) && (_prevBtnState != HeaderButtonState.Inactive) &&
							(_prevBtnState != HeaderButtonState.Pushed))
						{
							_prevBtnState = HeaderButtonState.Pushed;
							if (this.PrevMonthButtonClick != null)
								this.PrevMonthButtonClick(this, new EventArgs());
							btnClick = true;
						}
						if ((rightBtnRgn.IsVisible(mouseLocation)) && (_nextBtnState != HeaderButtonState.Inactive) &&
							(_nextBtnState != HeaderButtonState.Pushed))
						{
							_nextBtnState = HeaderButtonState.Pushed;
							if (this.NextMonthButtonClick != null)
								this.NextMonthButtonClick(this, new EventArgs());
							btnClick = true;
						}
					}
					if (_yearSelector)
					{
						if ((leftYearBtnRgn.IsVisible(mouseLocation)) && (_prevYearBtnState != HeaderButtonState.Inactive) &&
							(_prevYearBtnState != HeaderButtonState.Pushed))
						{
							_prevYearBtnState = HeaderButtonState.Pushed;
							if (this.PrevYearButtonClick != null)
								this.PrevYearButtonClick(this, new EventArgs());
							btnClick = true;
						}
						if ((rightYearBtnRgn.IsVisible(mouseLocation)) && (_nextYearBtnState != HeaderButtonState.Inactive) &&
							(_nextYearBtnState != HeaderButtonState.Pushed))
						{
							_nextYearBtnState = HeaderButtonState.Pushed;
							if (this.NextYearButtonClick != null)
								this.NextYearButtonClick(this, new EventArgs());
							btnClick = true;
						}
					}
				}
				else
				{
					if (_contextMenu)
					{
						DisplayMonthContextMenu(mouseLocation);
					}
				}

				if (mode == NuGenClickMode.Single)
				{
					if ((this.Click != null) && (!btnClick))
						this.Click(this, new NuGenClickEventArgs(button));
				}
				else
				{
					if ((this.DoubleClick != null) && (!btnClick))
						this.DoubleClick(this, new NuGenClickEventArgs(button));
				}
			}

			leftBtnRgn.Dispose();
			rightBtnRgn.Dispose();
			leftYearBtnRgn.Dispose();
			rightYearBtnRgn.Dispose();
		}



		internal void MouseUp()
		{
			// if mouse button is released no button should be pushed
			if (_prevBtnState != HeaderButtonState.Inactive)
				_prevBtnState = HeaderButtonState.Normal;
			if (_nextBtnState != HeaderButtonState.Inactive)
				_nextBtnState = HeaderButtonState.Normal;
			if (_prevYearBtnState != HeaderButtonState.Inactive)
				_prevYearBtnState = HeaderButtonState.Normal;
			if (_nextYearBtnState != HeaderButtonState.Inactive)
				_nextYearBtnState = HeaderButtonState.Normal;

			_calendar.Invalidate();
		}

		internal void MouseMove(Point mouseLocation)
		{
			Region prevBtnRgn = new Region(_prevBtnRect);
			Region nextBtnRgn = new Region(_nextBtnRect);
			Region prevYearBtnRgn = new Region(_prevYearBtnRect);
			Region nextYearBtnRgn = new Region(_nextYearBtnRect);
			HeaderButtonState oldPrevMonthState = _prevBtnState;
			HeaderButtonState oldNextMonthState = _nextBtnState;
			HeaderButtonState oldPrevYearState = _prevYearBtnState;
			HeaderButtonState oldNextYearState = _nextYearBtnState;


			if (_monthSelector)
			{
				// If not within left scroll button, make sure its not pushed
				if (!prevBtnRgn.IsVisible(mouseLocation))
				{
					if (_prevBtnState != HeaderButtonState.Inactive)
						_prevBtnState = HeaderButtonState.Normal;
				}
				else if (_prevBtnState != HeaderButtonState.Inactive)
					_prevBtnState = HeaderButtonState.Hot;

				if (oldPrevMonthState != _prevBtnState)
					DrawButton(_calendar.CreateGraphics(), _prevBtnState, HeaderButtons.PreviousMonth, _prevBtnRect);
				// If not within right scroll button, make sure its not pushed
				if (!nextBtnRgn.IsVisible(mouseLocation))
				{
					if (_nextBtnState != HeaderButtonState.Inactive)
						_nextBtnState = HeaderButtonState.Normal;
				}
				else if (_nextBtnState != HeaderButtonState.Inactive)
					_nextBtnState = HeaderButtonState.Hot;

				if (oldNextMonthState != _nextBtnState)
					DrawButton(_calendar.CreateGraphics(), _nextBtnState, HeaderButtons.NextMonth, _nextBtnRect);
			}
			if (_yearSelector)
			{
				// If not within left scroll button, make sure its not pushed
				if (!prevYearBtnRgn.IsVisible(mouseLocation))
				{
					if (_prevYearBtnState != HeaderButtonState.Inactive)
						_prevYearBtnState = HeaderButtonState.Normal;
				}
				else if (_prevYearBtnState != HeaderButtonState.Inactive)
					_prevYearBtnState = HeaderButtonState.Hot;

				if (oldPrevYearState != _prevYearBtnState)
					DrawButton(_calendar.CreateGraphics(), _prevYearBtnState, HeaderButtons.PreviousYear, _prevYearBtnRect);


				// If not within right scroll button, make sure its not pushed
				if (!nextYearBtnRgn.IsVisible(mouseLocation))
				{
					if (_nextYearBtnState != HeaderButtonState.Inactive)
						_nextYearBtnState = HeaderButtonState.Normal;
				}
				else if (_nextYearBtnState != HeaderButtonState.Inactive)
					_nextYearBtnState = HeaderButtonState.Hot;

				if (oldNextYearState != _nextYearBtnState)
					DrawButton(_calendar.CreateGraphics(), _nextYearBtnState, HeaderButtons.NextYear, _nextYearBtnRect);

			}

			if (_region.IsVisible(mouseLocation))
				_calendar.ActiveRegion = NuGenCalendarRegion.Header;


			prevBtnRgn.Dispose();
			nextBtnRgn.Dispose();
			prevYearBtnRgn.Dispose();
			nextYearBtnRgn.Dispose();

		}

		internal bool IsVisible(Rectangle clip)
		{
			return _region.IsVisible(clip);
		}

		internal void Draw(Graphics e)
		{
			StringFormat textFormat = new StringFormat();
			Brush textBrush = new SolidBrush(TextColor);
			Brush bgBrush = new SolidBrush(BackColor1);

			string minMonth;
			string maxMonth;
			string currentMonth;


			string month;
			textFormat.LineAlignment = StringAlignment.Center;
			textFormat.Alignment = _align;

			if (_gradientMode != NuGenGradientMode.None)
				_calendar.DrawGradient(e, _rect, BackColor1, BackColor2, _gradientMode);
			else
				e.FillRectangle(bgBrush, _rect);


			if (_monthSelector)
			{
				currentMonth = _calendar.Month.SelectedMonth.Year.ToString() + "-" + _calendar.Month.SelectedMonth.Month.ToString();

				minMonth = _calendar.MinDate.Year.ToString() + "-" + _calendar.MinDate.Month.ToString();
				maxMonth = _calendar.MaxDate.Year.ToString() + "-" + _calendar.MaxDate.Month.ToString();

				if ((minMonth == currentMonth) && (_prevBtnState != HeaderButtonState.Pushed))
					_prevBtnState = HeaderButtonState.Inactive;
				else if (_prevBtnState != HeaderButtonState.Pushed)
					_prevBtnState = HeaderButtonState.Normal;

				if ((maxMonth == currentMonth) && (_nextBtnState != HeaderButtonState.Pushed))
					_nextBtnState = HeaderButtonState.Inactive;
				else if (_nextBtnState != HeaderButtonState.Pushed)
					_nextBtnState = HeaderButtonState.Normal;

			}
			if (_yearSelector)
			{
				currentMonth = _calendar.Month.SelectedMonth.Year.ToString() + "-" + _calendar.Month.SelectedMonth.Month.ToString() + "-01";

				DateTime currentDate = DateTime.Parse(currentMonth);
				int days = DateTime.DaysInMonth(_calendar.MinDate.Year, _calendar.MinDate.Month);
				DateTime minDate = DateTime.Parse(_calendar.MinDate.Year.ToString() + "-" + _calendar.MinDate.Month.ToString() + "-" + days.ToString());
				days = DateTime.DaysInMonth(_calendar.MaxDate.Year, _calendar.MaxDate.Month);
				DateTime maxDate = DateTime.Parse(_calendar.MaxDate.Year.ToString() + "-" + _calendar.MaxDate.Month.ToString() + "-" + days.ToString());

				if ((DateTime.Compare(currentDate.AddYears(-1), minDate) < 0) && (_prevYearBtnState != HeaderButtonState.Pushed))
					_prevYearBtnState = HeaderButtonState.Inactive;
				else if (_prevYearBtnState != HeaderButtonState.Pushed)
					_prevYearBtnState = HeaderButtonState.Normal;

				if ((DateTime.Compare(currentDate.AddYears(1), maxDate) > 0) && (_nextYearBtnState != HeaderButtonState.Pushed))
					_nextYearBtnState = HeaderButtonState.Inactive;
				else if (_nextYearBtnState != HeaderButtonState.Pushed)
					_nextYearBtnState = HeaderButtonState.Normal;
			}


			if (_monthSelector)
			{

				DrawButton(e, _prevBtnState, HeaderButtons.PreviousMonth, _prevBtnRect);
				DrawButton(e, _nextBtnState, HeaderButtons.NextMonth, _nextBtnRect);
			}
			if (_yearSelector)
			{

				DrawButton(e, _prevYearBtnState, HeaderButtons.PreviousYear, _prevYearBtnRect);
				DrawButton(e, _nextYearBtnState, HeaderButtons.NextYear, _nextYearBtnRect);

			}

			month = _calendar._dateTimeFormat.GetMonthName(_calendar.Month.SelectedMonth.Month) + " " + _calendar.Month.SelectedMonth.Year.ToString();
			if (ShowMonth)
				e.DrawString(month, Font, textBrush, _textRect, textFormat);
			else
				e.DrawString(_text, Font, textBrush, _textRect, textFormat);

			textBrush.Dispose();
			bgBrush.Dispose();
		}

		#endregion

		#region Methods.Private

		private void DrawButton(Graphics g, HeaderButtonState state, HeaderButtons button, Rectangle bounds)
		{
			NuGenControlState currentState = NuGenControlState.Normal;

			if (_calendar.Enabled)
			{
				if (state == HeaderButtonState.Hot)
					currentState = NuGenControlState.Hot;
				else if (state == HeaderButtonState.Inactive)
					currentState = NuGenControlState.Disabled;
				else if (state == HeaderButtonState.Pushed)
					currentState = NuGenControlState.Pressed;
			}
			else
				currentState = NuGenControlState.Disabled;

			NuGenPaintParams paintParams = new NuGenPaintParams(g);
			paintParams.Bounds = bounds;
			paintParams.State = currentState;

			this.Renderer.DrawBackground(paintParams);
			this.Renderer.DrawBorder(paintParams);

			Image buttonImage;

			switch (button)
			{
				case HeaderButtons.NextYear:
				{
					buttonImage = this.Renderer.GetNextYearImage();
					break;
				}
				case HeaderButtons.NextMonth:
				{
					buttonImage = this.Renderer.GetNextMonthImage();
					break;
				}
				case HeaderButtons.PreviousYear:
				{
					buttonImage = this.Renderer.GetPreviousYearImage();
					break;
				}
				default:
				{
					buttonImage = this.Renderer.GetPreviousMonthImage();
					break;
				}
			}

			Debug.Assert(buttonImage != null, "buttonImage != null");

			NuGenImagePaintParams imagePaintParams = new NuGenImagePaintParams(paintParams);
			imagePaintParams.Image = buttonImage;
			Size imageSize = buttonImage.Size;
			imagePaintParams.Bounds = new Rectangle(
				bounds.Left + (bounds.Width - imageSize.Width) / 2
				, bounds.Top + (bounds.Height - imageSize.Height) / 2
				, imageSize.Width
				, imageSize.Height
			);

			this.Renderer.DrawImage(imagePaintParams);	
		}

		#endregion
	}
}
