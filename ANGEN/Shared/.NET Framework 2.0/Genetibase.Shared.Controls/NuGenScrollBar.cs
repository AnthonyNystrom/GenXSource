/* -----------------------------------------------
 * NuGenScrollBar.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.ScrollBarInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;
using Genetibase.WinApi;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	[ToolboxItem(false)]
	[DefaultProperty("Value")]
	[DefaultEvent("ValueChanged")]
	[Designer("Genetibase.Shared.Controls.Design.NuGenScrollBarDesigner")]
	[NuGenSRDescription("Description_ScrollBar")]
	[System.ComponentModel.DesignerCategory("Code")]
	public partial class NuGenScrollBar : NuGenOrientationControlBase
	{
		#region Properties.Behavior

		/*
		 * LargeChange
		 */

		/// <summary>
		/// Gets or sets the amount by which the scroll box position changes when the user clicks in the scroll bar or presses the PAGE UP or PAGE DOWN keys.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_ScrollBar_LargeChange")]
		public int LargeChange
		{
			get
			{
				return this.ValueTracker.LargeChange;
			}
			set
			{
				if (this.ValueTracker.LargeChange != value)
				{
					this.ValueTracker.LargeChange = value;
					this.Invalidate();

					this.OnLargeChangeChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// </summary>
		protected virtual int DefaultLargeChange
		{
			get
			{
				return 5;
			}
		}

		private void ResetLargeChange()
		{
			this.LargeChange = this.DefaultLargeChange;
		}

		private bool ShouldSerializeLargeChange()
		{
			return this.LargeChange != this.DefaultLargeChange;
		}

		private static readonly object _largeChangeChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="LargeChange"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChange")]
		[NuGenSRDescription("Description_ScrollBar_LargeChangeChanged")]
		public event EventHandler LargeChangeChanged
		{
			add
			{
				this.Events.AddHandler(_largeChangeChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_largeChangeChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="Genetibase.Shared.Controls.NuGenScrollBar.LargeChangeChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnLargeChangeChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_largeChangeChanged, e);
		}

		/*
		 * Maximum
		 */

		/// <summary>
		/// Gets or sets the upper limit value of the scrollable range.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_ScrollBar_Maximum")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public int Maximum
		{
			get
			{
				return this.ValueTracker.Maximum;
			}
			set
			{
				if (this.ValueTracker.Maximum != value)
				{
					this.ValueTracker.Maximum = value;
					this.Invalidate();

					this.OnMaximumChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// </summary>
		protected virtual int DefaultMaximum
		{
			get
			{
				return 10;
			}
		}

		private void ResetMaximum()
		{
			this.Maximum = this.DefaultMaximum;
		}

		private bool ShouldSerializeMaximum()
		{
			return this.Maximum != this.DefaultMaximum;
		}

		private static readonly object _maximumChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Maximum"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChange")]
		[NuGenSRDescription("Description_TrackBar_MaximumChanged")]
		public event EventHandler MaximumChanged
		{
			add
			{
				this.Events.AddHandler(_maximumChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_maximumChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="Genetibase.Shared.Controls.NuGenScrollBar.MaximumChanged"/> event.
		/// </summary>
		protected virtual void OnMaximumChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_maximumChanged, e);
		}

		/*
		 * Minimum
		 */

		/// <summary>
		/// Gets or sets the lower limit value of the scrollable range.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_ScrollBar_Minimum")]
		[RefreshProperties(RefreshProperties.Repaint)]
		public int Minimum
		{
			get
			{
				return this.ValueTracker.Minimum;
			}
			set
			{
				if (this.ValueTracker.Minimum != value)
				{
					this.ValueTracker.Minimum = value;
					this.Invalidate();

					this.OnMinimumChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// </summary>
		protected virtual int DefaultMinimum
		{
			get
			{
				return 0;
			}
		}

		private void ResetMinimum()
		{
			this.Minimum = this.DefaultMinimum;
		}

		private bool ShouldSerializeMinimum()
		{
			return this.Minimum != this.DefaultMinimum;
		}

		private static readonly object _minimumChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Minimum"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChange")]
		[NuGenSRDescription("Description_ScrollBar_MinimumChanged")]
		public event EventHandler MinimumChanged
		{
			add
			{
				this.Events.AddHandler(_minimumChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_minimumChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="Genetibase.Shared.Controls.NuGenScrollBar.MinimumChanged"/> event.
		/// </summary>
		protected virtual void OnMinimumChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_minimumChanged, e);
		}

		/*
		 * SmallChange
		 */

		/// <summary>
		/// Gets or sets the amount by which the scroll box position changes when the user clicks a scroll arrow or presses an arrow key.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_SmallBar_SmallChange")]
		public int SmallChange
		{
			get
			{
				return this.ValueTracker.SmallChange;
			}
			set
			{
				if (this.ValueTracker.SmallChange != value)
				{
					this.ValueTracker.SmallChange = value;
					this.Invalidate();

					this.OnSmallChangeChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// </summary>
		protected virtual int DefaultSmallChange
		{
			get
			{
				return 1;
			}
		}

		private void ResetSmallChange()
		{
			this.SmallChange = this.DefaultSmallChange;
		}

		private bool ShouldSerializeSmallChange()
		{
			return this.SmallChange != this.DefaultSmallChange;
		}

		private static readonly object _smallChangeChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="SmallChange"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChange")]
		[NuGenSRDescription("Description_ScrollBar_SmallChangeChanged")]
		public event EventHandler SmallChangeChanged
		{
			add
			{
				this.Events.AddHandler(_smallChangeChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_smallChangeChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="Genetibase.Shared.Controls.NuGenScrollBar.SmallChangeChanged"/> event.
		/// </summary>
		protected virtual void OnSmallChangeChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_smallChangeChanged, e);
		}

		/*
		 * Value
		 */

		/// <summary>
		/// Gets or sets the value that the scroll box position represents.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_ScrollBar_Value")]
		public int Value
		{
			get
			{
				return this.ValueTracker.Value;
			}
			set
			{
				if (this.ValueTracker.Value != value)
				{
					this.ValueTracker.Value = value;
					this.Invalidate();

					this.OnValueChanged(EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// </summary>
		protected virtual int DefaultValue
		{
			get
			{
				return 0;
			}
		}

		private void ResetValue()
		{
			this.Value = this.DefaultValue;
		}

		private bool ShouldSerializeValue()
		{
			return this.Value != this.DefaultValue;
		}

		private static readonly object _valueChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Value"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChange")]
		[NuGenSRDescription("Description_ScrollBar_ValueChanged")]
		public event EventHandler ValueChanged
		{
			add
			{
				this.Events.AddHandler(_valueChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_valueChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="Genetibase.Shared.Controls.NuGenScrollBar.ValueChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnValueChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_valueChanged, e);
		}

		#endregion

		#region Properties.Public.Overridden

		/*
		 * BackColor
		 */

		/// <summary>
		/// Gets or sets the background color for the control.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that represents the background color of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultBackColor"></see> property.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[DefaultValue(typeof(Color), "Transparent")]
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

		#endregion

		#region Properties.Protected.Overridden

		/*
		 * DefaultSize
		 */

		private static readonly Size _defaultSize = new Size(100, SystemInformation.HorizontalScrollBarHeight);

		/// <summary>
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				return _defaultSize;
			}
		}

		#endregion

		#region Properties.Services

		/*
		 * Renderer
		 */

		private INuGenScrollBarRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenScrollBarRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_renderer = this.ServiceProvider.GetService<INuGenScrollBarRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenScrollBarRenderer>();
					}
				}

				return _renderer;
			}
		}

		/*
		 * ValueDescriptor
		 */

		private INuGenValueTracker _valueTracker;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenValueTracker ValueTracker
		{
			get
			{
				if (_valueTracker == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					INuGenValueTrackerService valueTrackerService = this.ServiceProvider.GetService<INuGenValueTrackerService>();

					if (valueTrackerService == null)
					{
						throw new NuGenServiceNotFoundException<INuGenValueTrackerService>();
					}

					_valueTracker = valueTrackerService.CreateValueTracker();
					Debug.Assert(_valueTracker != null, "_valueTracker != null");
				}

				return _valueTracker;
			}
		}

		#endregion

		#region Methods.Public.New

		/*
		 * Invalidate
		 */

		/// <summary>
		/// Invalidates the entire surface of the control and causes the control to be redrawn.
		/// </summary>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		public new void Invalidate()
		{
			base.Invalidate();
			this.BuildLayout();
		}

		#endregion

		#region Methods.Protected.Overridden

		/*
		 * OnLoad
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.UserControl.Load"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			this.BuildLayout();
		}

		/*
		 * OnResize
		 */

		/// <summary>
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			this.BuildLayout();
		}

		#endregion

		#region Methods.Components

		/*
		 * InitializeLeftTopButton
		 */

		private void InitializeLeftTopButton(NuGenScrollButton leftTopButton)
		{
			Debug.Assert(leftTopButton != null, "leftTopButton != null");

			leftTopButton.MouseDown += _leftTopButton_MouseDown;
			leftTopButton.MouseUp += _leftTopButton_MouseUp;
		}

		/*
		 * InitializeLeftTopTrack
		 */

		private void InitializeLeftTopTrack(ScrollTrack leftTopTrack)
		{
			Debug.Assert(leftTopTrack != null, "leftTopTrack != null");

			leftTopTrack.MouseDown += _leftTopTrack_MouseDown;
			leftTopTrack.MouseUp += _leftTopTrack_MouseUp;
		}

		/*
		 * InitializeRightBottomTrack
		 */

		private void InitializeRightBottomTrack(ScrollTrack rightBottomTrack)
		{
			Debug.Assert(rightBottomTrack != null, "rightBottomTrack != null");

			rightBottomTrack.MouseDown += _rightBottomTrack_MouseDown;
			rightBottomTrack.MouseUp += _rightBottomTrack_MouseUp;
		}

		/*
		 * InitializeRightBottomButton
		 */

		private void InitializeRightBottomButton(NuGenScrollButton rightBottomButton)
		{
			Debug.Assert(rightBottomButton != null, "rightBottomButton != null");

			rightBottomButton.MouseDown += _rightBottomButton_MouseDown;
			rightBottomButton.MouseUp += _rightBottomButton_MouseUp;
		}

		/*
		 * InitializeLargeChangeDownTimer
		 */

		private void InitializeLargeChangeDownTimer(Timer largeChangeDownTimer)
		{
			Debug.Assert(largeChangeDownTimer != null, "largeChangeDownTimer != null");

			largeChangeDownTimer.Tick += _largeChangeDownTimer_Tick;
			largeChangeDownTimer.Interval = _timerDefaultInterval;
		}

		/*
		 * InitializeLargeChangeUpTimer
		 */

		private void InitializeLargeChangeUpTimer(Timer largeChangeUpTimer)
		{
			Debug.Assert(largeChangeUpTimer != null, "largeChangeUpTimer != null");

			largeChangeUpTimer.Tick += _largeChangeUpTimer_Tick;
			largeChangeUpTimer.Interval = _timerDefaultInterval;
		}

		/*
		 * InitializeSmallChangeDownTimer
		 */

		private void InitializeSmallChangeDownTimer(Timer smallChangeDownTimer)
		{
			Debug.Assert(smallChangeDownTimer != null, "smallChangeDownTimer != null");

			smallChangeDownTimer.Tick += _smallChangeDownTimer_Tick;
			smallChangeDownTimer.Interval = _timerDefaultInterval;
		}

		/*
		 * InitializeSmallChangeUpTimer
		 */

		private void InitializeSmallChangeUpTimer(Timer smallChangeUpTimer)
		{
			Debug.Assert(smallChangeUpTimer != null, "smallChangeUpTimer != null");

			smallChangeUpTimer.Tick += _smallChangeUpTimer_Tick;
			smallChangeUpTimer.Interval = _timerDefaultInterval;
		}

		/*
		 * InitializeSizeBox
		 */

		private void InitializeSizeBox(SizeBox sizeBox)
		{
			Debug.Assert(sizeBox != null, "sizeBox != null");

			sizeBox.MouseDown += _sizeBox_MouseDown;
			sizeBox.MouseMove += _sizeBox_MouseMove;
			sizeBox.MouseUp += _sizeBox_MouseUp;
		}

		#endregion

		#region Methods.Logic

		/*
		 * DoLargeChangeDown
		 */

		private void DoLargeChangeDown()
		{
			this.ValueTracker.LargeChangeDown();
			this.Invalidate();
			this.OnValueChanged(EventArgs.Empty);
		}

		/*
		 * DoLargeChangeUp
		 */

		private void DoLargeChangeUp()
		{
			this.ValueTracker.LargeChangeUp();
			this.Invalidate();
			this.OnValueChanged(EventArgs.Empty);
		}

		/*
		 * DoSmallChangeDown
		 */

		private void DoSmallChangeDown()
		{
			this.ValueTracker.SmallChangeDown();
			this.Invalidate();
			this.OnValueChanged(EventArgs.Empty);
		}

		/*
		 * DoSmallChangeUp
		 */

		private void DoSmallChangeUp()
		{
			this.ValueTracker.SmallChangeUp();
			this.Invalidate();
			this.OnValueChanged(EventArgs.Empty);
		}

		#endregion

		#region Methods.Layout

		/*
		 * BuildLayout
		 */

		private void BuildLayout()
		{
			this.SuspendLayout();

			_leftTopTrack.Orientation
				= _rightBottomTrack.Orientation
				= _sizeBox.Orientation
				= this.Orientation
				;

			int scrollAreaDimension = this.GetScrollAreaDimension();
			int sizeBoxDimension = this.GetSizeBoxDimension(scrollAreaDimension);
			int scrollPosition = this.GetPositionFromValue(scrollAreaDimension, sizeBoxDimension);

			if (this.Orientation == NuGenOrientationStyle.Horizontal)
			{
				_leftTopButton.Dock = DockStyle.Left;
				_leftTopButton.Style = NuGenScrollButtonStyle.Left;

				_rightBottomButton.Dock = DockStyle.Right;
				_rightBottomButton.Style = NuGenScrollButtonStyle.Right;

				_sizeBox.Left = _leftTopButton.Right + scrollPosition;
				_sizeBox.Top = this.ClientRectangle.Top;
				_sizeBox.Width = sizeBoxDimension;
				_sizeBox.Height = this.ClientRectangle.Height;

				_leftTopTrack.Left = _leftTopButton.Right;
				_leftTopTrack.Top = this.ClientRectangle.Top;
				_leftTopTrack.Width = _sizeBox.Left - _leftTopButton.Right;
				_leftTopTrack.Height = this.ClientRectangle.Height;

				_rightBottomTrack.Left = _sizeBox.Right;
				_rightBottomTrack.Top = this.ClientRectangle.Top;
				_rightBottomTrack.Width = _rightBottomButton.Left - _sizeBox.Right;
				_rightBottomTrack.Height = this.ClientRectangle.Height;
			}
			else
			{
				_leftTopButton.Dock = DockStyle.Top;
				_leftTopButton.Style = NuGenScrollButtonStyle.Up;

				_rightBottomButton.Dock = DockStyle.Bottom;
				_rightBottomButton.Style = NuGenScrollButtonStyle.Down;

				_sizeBox.Left = this.ClientRectangle.Left;
				_sizeBox.Top = _leftTopButton.Bottom + scrollPosition;
				_sizeBox.Width = this.ClientRectangle.Width;
				_sizeBox.Height = sizeBoxDimension;

				_leftTopTrack.Left = this.ClientRectangle.Left;
				_leftTopTrack.Top = _leftTopButton.Bottom;
				_leftTopTrack.Width = this.ClientRectangle.Width;
				_leftTopTrack.Height = _sizeBox.Top - _leftTopButton.Bottom + 1;

				_rightBottomTrack.Left = this.ClientRectangle.Left;
				_rightBottomTrack.Top = _sizeBox.Bottom;
				_rightBottomTrack.Width = this.ClientRectangle.Width;
				_rightBottomTrack.Height = _rightBottomButton.Top - _sizeBox.Bottom + 1;
				_rightBottomButton.BringToFront();
			}

			this.ResumeLayout(true);
		}

		/*
		 * GetSizeBoxDimension
		 */

		private int GetSizeBoxDimension(int scrollAreaDimension)
		{
			if (scrollAreaDimension < _minimumSizeBoxDimension)
			{
				return 0;
			}

			int sizeBoxDimension = 0;

			if (this.Maximum == this.Minimum)
			{
				sizeBoxDimension = scrollAreaDimension;
			}
			else
			{
				sizeBoxDimension = (int)Math.Min(
					scrollAreaDimension * 9 / (this.Maximum - this.Minimum)
					, scrollAreaDimension * 0.9f
				);
			}

			return Math.Max(sizeBoxDimension, _minimumSizeBoxDimension);
		}

		/*
		 * GetScrollAreaDimension
		 */

		private int GetScrollAreaDimension()
		{
			if (this.Orientation == NuGenOrientationStyle.Horizontal)
			{
				return _rightBottomButton.Left - _leftTopButton.Right;
			}

			return _rightBottomButton.Top - _leftTopButton.Bottom;
		}

		/*
		 * GetPositionFromValue
		 */

		private int GetPositionFromValue(int scrollAreaDimension, int sizeBoxDimension)
		{
			if (this.Maximum == this.Minimum)
			{
				return 0;
			}

			return (int)(this.GetStep(scrollAreaDimension, sizeBoxDimension) * (float)this.Value);
		}

		/*
		 * GetStep
		 */

		private float GetStep(int scrollAreaDimension, int sizeBoxDimension)
		{
			return (float)(scrollAreaDimension - sizeBoxDimension) / (float)(this.Maximum - this.Minimum);
		}

		/*
		 * GetValueFromPosition
		 */

		private int GetValueFromPosition(Point cursorPosition, int scrollAreaDimension, int sizeBoxDimension)
		{
			int value = 0;
			float step = this.GetStep(scrollAreaDimension, sizeBoxDimension);

			if (this.Orientation == NuGenOrientationStyle.Horizontal)
			{
				value = (int)((float)(cursorPosition.X - _leftTopButton.Right) / step);
			}
			else
			{
				value = (int)((float)(cursorPosition.Y - _leftTopButton.Bottom) / step);
			}

			return Math.Min(this.Maximum, Math.Max(value, this.Minimum));
		}

		#endregion

		#region EventHandlers.Buttons

		private void _leftTopButton_MouseDown(object sender, MouseEventArgs e)
		{
			this.DoSmallChangeDown();
			_smallChangeDownTimer.Start();
		}

		private void _leftTopButton_MouseUp(object sender, MouseEventArgs e)
		{
			_smallChangeDownTimer.Stop();
			_smallChangeDownTimer.Interval = _timerDefaultInterval;
		}

		private void _rightBottomButton_MouseDown(object sender, MouseEventArgs e)
		{
			this.DoSmallChangeUp();
			_smallChangeUpTimer.Start();
		}

		private void _rightBottomButton_MouseUp(object sender, MouseEventArgs e)
		{
			_smallChangeUpTimer.Stop();
			_smallChangeUpTimer.Interval = _timerDefaultInterval;
		}

		#endregion

		#region EventHandlers.SizeBox

		private bool _isDragging;

		private void _sizeBox_MouseDown(object sender, MouseEventArgs e)
		{
			_isDragging = true;
		}

		private void _sizeBox_MouseMove(object sender, MouseEventArgs e)
		{
			if (_isDragging)
			{
				Point cursorPosition = NuGenControlPaint.TranslatePoint(e.Location, (Control)sender, this);
				int scrollAreaDimension = this.GetScrollAreaDimension();

				this.Value = this.GetValueFromPosition(cursorPosition, scrollAreaDimension, this.GetSizeBoxDimension(scrollAreaDimension));
			}
		}

		private void _sizeBox_MouseUp(object sender, MouseEventArgs e)
		{
			_isDragging = false;
		}

		#endregion

		#region EventHandlers.Timers

		private void _largeChangeDownTimer_Tick(object sender, EventArgs e)
		{
			this.DoLargeChangeDown();
			_largeChangeDownTimer.Interval = _timerInterval;
		}

		private void _largeChangeUpTimer_Tick(object sender, EventArgs e)
		{
			this.DoLargeChangeUp();
			_largeChangeUpTimer.Interval = _timerInterval;
		}

		private void _smallChangeDownTimer_Tick(object sender, EventArgs e)
		{
			this.DoSmallChangeDown();
			_smallChangeDownTimer.Interval = _timerInterval;
		}

		private void _smallChangeUpTimer_Tick(object sender, EventArgs e)
		{
			this.DoSmallChangeUp();
			_smallChangeUpTimer.Interval = _timerInterval;
		}

		#endregion

		#region EventHandlers.Tracks

		private void _leftTopTrack_MouseDown(object sender, MouseEventArgs e)
		{
			this.DoLargeChangeDown();
			_largeChangeDownTimer.Start();
		}

		private void _leftTopTrack_MouseUp(object sender, MouseEventArgs e)
		{
			_largeChangeDownTimer.Stop();
			_largeChangeDownTimer.Interval = _timerDefaultInterval;
		}

		private void _rightBottomTrack_MouseDown(object sender, MouseEventArgs e)
		{
			this.DoLargeChangeUp();
			_largeChangeUpTimer.Start();
		}

		private void _rightBottomTrack_MouseUp(object sender, MouseEventArgs e)
		{
			_largeChangeUpTimer.Stop();
			_largeChangeUpTimer.Interval = _timerDefaultInterval;
		}

		#endregion

		private const int _minimumSizeBoxDimension = 6;
		private const int _timerDefaultInterval = 500;
		private const int _timerInterval = 50;

		private IContainer _components;

		private Timer _smallChangeDownTimer;
		private Timer _smallChangeUpTimer;
		private Timer _largeChangeDownTimer;
		private Timer _largeChangeUpTimer;

		private NuGenScrollButton _leftTopButton;
		private NuGenScrollButton _rightBottomButton;
		private ScrollTrack _leftTopTrack;
		private ScrollTrack _rightBottomTrack;
		private SizeBox _sizeBox;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenScrollBar"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenButtonStateService"/><para/>
		/// <see cref="INuGenControlStateService"/><para/>
		/// <see cref="INuGenValueTrackerService"/><para/>
		/// <see cref="INuGenScrollBarRenderer"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenScrollBar(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			_components = new Container();

			_smallChangeDownTimer = new Timer(_components);
			_smallChangeUpTimer = new Timer(_components);
			_largeChangeDownTimer = new Timer(_components);
			_largeChangeUpTimer = new Timer(_components);

			_leftTopButton = new NuGenScrollButton(serviceProvider);
			_rightBottomButton = new NuGenScrollButton(serviceProvider);
			_leftTopTrack = new ScrollTrack(serviceProvider);
			_rightBottomTrack = new ScrollTrack(serviceProvider);
			_sizeBox = new SizeBox(serviceProvider);

			this.InitializeSizeBox(_sizeBox);
			this.InitializeLeftTopButton(_leftTopButton);
			this.InitializeRightBottomButton(_rightBottomButton);
			this.InitializeLeftTopTrack(_leftTopTrack);
			this.InitializeRightBottomTrack(_rightBottomTrack);

			this.InitializeLargeChangeDownTimer(_largeChangeDownTimer);
			this.InitializeLargeChangeUpTimer(_largeChangeUpTimer);
			this.InitializeSmallChangeDownTimer(_smallChangeDownTimer);
			this.InitializeSmallChangeUpTimer(_smallChangeUpTimer);

			this.Controls.AddRange(
				new Control[] {
					_sizeBox
					, _leftTopTrack
					, _rightBottomTrack
					, _leftTopButton
					, _rightBottomButton
				}
			);

			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.UserPaint, true);
			this.BackColor = Color.Transparent;

			this.ValueTracker.LargeChange = this.DefaultLargeChange;
			this.ValueTracker.Maximum = this.DefaultMaximum;
			this.ValueTracker.Minimum = this.DefaultMinimum;
			this.ValueTracker.SmallChange = this.DefaultSmallChange;
			this.ValueTracker.Value = this.DefaultValue;

			this.BuildLayout();
		}

		/// <summary>
		/// </summary>
		/// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_components != null)
				{
					_components.Dispose();
				}
			}

			base.Dispose(disposing);
		}
	}
}
