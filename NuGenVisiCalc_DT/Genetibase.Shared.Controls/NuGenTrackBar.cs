/* -----------------------------------------------
 * NuGenTrackBar.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.TrackBarInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// <seealso cref="TrackBar"/>
	/// </summary>
	[ToolboxItem(false)]
	[DefaultEvent("ValueChanged")]
	[DefaultProperty("Value")]
	[Designer("Genetibase.Shared.Controls.Design.NuGenTrackBarDesigner")]
	[NuGenSRDescription("Description_TrackBar")]
	[System.ComponentModel.DesignerCategory("Code")]
	public partial class NuGenTrackBar : NuGenOrientationControlBase
	{
		#region Declarations.Fields

		private TrackButton _trackButton;

		#endregion

		#region Properties.Appearance

		/*
		 * TickStyle
		 */

		private TickStyle _tickStyle = TickStyle.BottomRight;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(TickStyle.BottomRight)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_TrackBar_TickStyle")]
		public TickStyle TickStyle
		{
			get
			{
				return _tickStyle;
			}
			set
			{
				if (_tickStyle != value)
				{
					_tickStyle = value;
					this.OnTickStyleChanged(EventArgs.Empty);
					_trackButton.TickStyle = value;
					this.Invalidate();
				}
			}
		}

		private static readonly object _tickStyleChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="TickStyle"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_TrackBar_TickStyleChanged")]
		public event EventHandler TickStyleChanged
		{
			add
			{
				this.Events.AddHandler(_tickStyleChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_tickStyleChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="TickStyleChanged"/> event.
		/// </summary>
		protected virtual void OnTickStyleChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_tickStyleChanged, e);
		}

		#endregion

		#region Properties.Behavior

		/*
		 * LargeChange
		 */

		/// <summary>
		/// Gets or sets the number of positions the slider moves in response to mouse clicks or the
		/// PageUp and PageDown keys.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_TrackBar_LargeChange")]
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
					this.AdjustTrackButtonBounds();
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
		[NuGenSRDescription("Description_TrackBar_LargeChangeChanged")]
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
		/// Will bubble the <see cref="Genetibase.Shared.Controls.NuGenTrackBar.LargeChangeChanged"/> event.
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
		/// Gets or sets the maximum value for the position of the slider.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_TrackBar_Maximum")]
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
					this.AdjustTrackButtonBounds();
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
		/// Will bubble the <see cref="Genetibase.Shared.Controls.NuGenTrackBar.MaximumChanged"/> event.
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
		/// Gets or sets the minimum value for the position of the slider.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_TrackBar_Minimum")]
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
					this.AdjustTrackButtonBounds();
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
		[NuGenSRDescription("Description_TrackBar_MinimumChanged")]
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
		/// Will bubble the <see cref="Genetibase.Shared.Controls.NuGenTrackBar.MinimumChanged"/> event.
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
		/// Gets or sets the number of positions the slider moves in response to keyboard input (arrow keys).
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_TrackBar_SmallChange")]
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
					this.AdjustTrackButtonBounds();
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
		[NuGenSRDescription("Description_TrackBar_SmallChangeChanged")]
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
		/// Will bubble the <see cref="Genetibase.Shared.Controls.NuGenTrackBar.SmallChangeChanged"/> event.
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
		/// Gets or sets the position of the slider.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_TrackBar_Value")]
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
					this.AdjustTrackButtonBounds();
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
		[NuGenSRDescription("Description_TrackBar_ValueChanged")]
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
		/// Will bubble the <see cref="Genetibase.Shared.Controls.NuGenTrackBar.ValueChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnValueChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_valueChanged, e);
		}

		#endregion

		#region Properties.Public.Overriden

		/*
		 * AutoSize
		 */

		/// <summary>
		/// </summary>
		[DefaultValue(false)]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;

				if (this.Orientation == NuGenOrientationStyle.Horizontal)
				{
					this.SetStyle(ControlStyles.FixedHeight, value);
					this.SetStyle(ControlStyles.FixedWidth, false);

					if (value)
					{
						this.Height = this.PreferredDimension;
					}
				}
				else
				{
					this.SetStyle(ControlStyles.FixedHeight, false);
					this.SetStyle(ControlStyles.FixedWidth, value);

					if (value)
					{
						this.Width = this.PreferredDimension;
					}
				}

				this.AdjustTrackButtonBounds();
			}
		}

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

		/*
		 * Focused
		 */

		private bool _focused;

		/// <summary>
		/// Gets a value indicating whether the control has input focus.
		/// </summary>
		/// <value></value>
		/// <returns>true if the control has focus; otherwise, false.</returns>
		public override bool Focused
		{
			get
			{
				return _focused;
			}
		}

		#endregion

		#region Properties.Protected.Overridden

		/*
		 * DefaultSize
		 */

		private static readonly Size _defaultSize = new Size(150, 45);

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

		#region Properties.Protected.Virtual

		/*
		 * AgnosticTrackBounds
		 */

		/// <summary>
		/// Gets orientation agnostic track button bounds.
		/// </summary>
		protected virtual Rectangle AgnosticTrackBounds
		{
			get
			{
				Rectangle agnosticClientRectangle = NuGenControlPaint.OrientationAgnosticRectangle(this.ClientRectangle, this.Orientation);

				int height = agnosticClientRectangle.Height / 10;

				return new Rectangle(
					agnosticClientRectangle.Left + _trackButton.Width,
					agnosticClientRectangle.Top + agnosticClientRectangle.Height / 2 - height / 2,
					agnosticClientRectangle.Width - _trackButton.Width * 2,
					height
				);
			}
		}

		/*
		 * DimensionLimit
		 */

		private static readonly NuGenLimit _dimensionLimit = new NuGenLimit(20, 50);

		/// <summary>
		/// <para>Horizontal:</para>
		/// Gets the minimum and maximum height.<para/>
		/// <para>Vertical:</para>
		/// Gets the minimum and maximum width.<para/>
		/// </summary>
		protected virtual NuGenLimit DimensionLimit
		{
			get
			{
				return _dimensionLimit;
			}
		}

		/*
		 * PreferredDimension
		 */

		/// <summary>
		/// </summary>
		protected virtual int PreferredDimension
		{
			get
			{
				return 45;
			}
		}

		#endregion

		#region Properties.Private

		/*
		 * TrackBounds
		 */

		private Rectangle TrackBounds
		{
			get
			{
				if (this.Orientation == NuGenOrientationStyle.Horizontal)
				{
					return this.AgnosticTrackBounds;
				}

				return new Rectangle(
					this.AgnosticTrackBounds.Top,
					this.AgnosticTrackBounds.Left,
					this.AgnosticTrackBounds.Height,
					this.AgnosticTrackBounds.Width
				);
			}
		}

		#endregion

		#region Properties.Services

		/*
		 * ButtonStateTracker
		 */

		private INuGenButtonStateTracker _buttonStateTracker;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenButtonStateTracker ButtonStateTracker
		{
			get
			{
				if (_buttonStateTracker == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					INuGenButtonStateService stateService = this.ServiceProvider.GetService<INuGenButtonStateService>();

					if (stateService == null)
					{
						throw new NuGenServiceNotFoundException<INuGenButtonStateService>();
					}

					_buttonStateTracker = stateService.CreateStateTracker();
					Debug.Assert(_buttonStateTracker != null, "_buttonStateTracker != null");
				}

				return _buttonStateTracker;
			}
		}

		/*
		 * Renderer
		 */

		private INuGenTrackBarRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenTrackBarRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_renderer = this.ServiceProvider.GetService<INuGenTrackBarRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenTrackBarRenderer>();
					}
				}

				return _renderer;
			}
		}

		/*
		 * ValueTracker
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

		#region Methods.Protected.Overridden

		/*
		 * OnLayout
		 */

		/// <summary>
		/// Raises the layout event.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.Forms.LayoutEventArgs"/> instance containing the event data.</param>
		protected override void OnLayout(LayoutEventArgs e)
		{
			base.OnLayout(e);
			this.AdjustTrackButtonBounds();
		}

		/*
		 * OnMouseDown
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			int valueFromPos = this.GetValueFromPosition(e.Location);
			int currentValue = this.Value;
			bool shouldUpdate = false;
			
			if (valueFromPos > currentValue)
			{
				this.ValueTracker.LargeChangeUp();
				shouldUpdate = true;
			}
			else if (valueFromPos < currentValue)
			{
				this.ValueTracker.LargeChangeDown();
				shouldUpdate = true;
			}

			if (shouldUpdate)
			{
				this.Invalidate(this.TrackBounds);
				this.AdjustTrackButtonBounds();
				this.OnValueChanged(EventArgs.Empty);
			}
		}

		/*
		 * OnOrientationChanged
		 */

		/// <summary>
		/// Will bubble the <see cref="Genetibase.Shared.Controls.NuGenOrientationControlBase.OrientationChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnOrientationChanged(EventArgs e)
		{
			_trackButton.Orientation = this.Orientation;
			base.OnOrientationChanged(e);
		}

		/*
		 * OnPaint
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;

			if (this.Orientation == NuGenOrientationStyle.Vertical)
			{
				NuGenControlPaint.Make90CCWGraphics(g, this.ClientRectangle);
			}

			NuGenTrackBarPaintParams paintParams = new NuGenTrackBarPaintParams(g, this.ValueTracker);

			paintParams.Bounds = this.AgnosticTrackBounds;
			paintParams.TickStyle = this.TickStyle;
			paintParams.State = this.StateTracker.GetControlState();

			this.Renderer.DrawTrack(paintParams);

			if (_focused && this.ShowFocusCues)
			{
				ControlPaint.DrawFocusRectangle(e.Graphics, this.ClientRectangle);
			}
		}

		/*
		 * ProcessDialogKey
		 */

		/// <summary>
		/// </summary>
		/// <param name="keyData"></param>
		/// <returns></returns>
		[SecurityPermission(SecurityAction.LinkDemand)]
		protected override bool ProcessDialogKey(Keys keyData)
		{
			bool shouldUpdate = false;

			switch (keyData)
			{
				case Keys.Up:
				case Keys.Right:
				{
					this.ValueTracker.SmallChangeUp();
					shouldUpdate = true;
					break;
				}
				case Keys.Down:
				case Keys.Left:
				{
					this.ValueTracker.SmallChangeDown();
					shouldUpdate = true;
					break;
				}
			}

			if (shouldUpdate)
			{
				this.Invalidate(this.TrackBounds);
				this.AdjustTrackButtonBounds();
				this.OnValueChanged(EventArgs.Empty);
			}

			return base.ProcessDialogKey(keyData);
		}

		/*
		 * SetBoundsCore
		 */

		/// <summary>
		/// Performs the work of setting the specified bounds of this control.
		/// </summary>
		/// <param name="x">The new <see cref="P:System.Windows.Forms.Control.Left"></see> property value of the control.</param>
		/// <param name="y">The new <see cref="P:System.Windows.Forms.Control.Top"></see> property value of the control.</param>
		/// <param name="width">The new <see cref="P:System.Windows.Forms.Control.Width"></see> property value of the control.</param>
		/// <param name="height">The new <see cref="P:System.Windows.Forms.Control.Height"></see> property value of the control.</param>
		/// <param name="specified">A bitwise combination of the <see cref="T:System.Windows.Forms.BoundsSpecified"></see> values.</param>
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if (this.Orientation == NuGenOrientationStyle.Vertical)
			{
				width = this.DimensionLimit.GetLimitedValue(width);
			}
			else
			{
				height = this.DimensionLimit.GetLimitedValue(height);
			}

			base.SetBoundsCore(x, y, width, height, specified);
		}

		#endregion

		#region Methods.Protected.Virtual

		/*
		 * AdjustTrackButtonBounds
		 */

		/// <summary>
		/// </summary>
		protected virtual void AdjustTrackButtonBounds()
		{
			int offset = this.GetOffsetFromValue(this.Value);

			if (this.Orientation == NuGenOrientationStyle.Horizontal)
			{
				_trackButton.Height = this.Height / 2;
				_trackButton.Width = this.Height / 4;
				_trackButton.Left = offset - _trackButton.Width / 2;
				_trackButton.Top = this.ClientRectangle.Top + this.ClientRectangle.Height / 2 - _trackButton.Height / 2;
			}
			else
			{
				_trackButton.Height = this.Width / 4;
				_trackButton.Width = this.Width / 2;
				_trackButton.Top = offset - _trackButton.Height / 2;
				_trackButton.Left = this.ClientRectangle.Left + this.ClientRectangle.Width / 2 - _trackButton.Width / 2;
			}
		}

		#endregion

		#region Methods.Private

		/*
		 * GetStep
		 */

		private float GetStep()
		{
			float interval = this.ValueTracker.Maximum - this.ValueTracker.Minimum;
			return (float)this.AgnosticTrackBounds.Width / interval;
		}

		/*
		 * GetOffsetFromValue
		 */

		private int GetOffsetFromValue(int value)
		{
			if (this.Orientation == NuGenOrientationStyle.Horizontal)
			{
				return this.AgnosticTrackBounds.Left + (int)(this.GetStep() * (value - this.Minimum));
			}

			return this.AgnosticTrackBounds.Right - (int)(this.GetStep() * (value - this.Minimum));
		}

		/*
		 * GetValueFromPosition
		 */

		/// <summary>
		/// </summary>
		/// <param name="cursorPosition">Client coordinates expected.</param>
		/// <returns></returns>
		private int GetValueFromPosition(Point cursorPosition)
		{
			int value = 0;

			if (this.Orientation == NuGenOrientationStyle.Horizontal)
			{
				value = (int)((cursorPosition.X - this.AgnosticTrackBounds.Left) / this.GetStep());
			}
			else
			{
				value = -(int)((cursorPosition.Y - this.AgnosticTrackBounds.Right) / this.GetStep());
			}

			return Math.Min(this.Maximum, Math.Max(this.Minimum, value + this.Minimum));
		}

		#endregion

		#region EventHandlers.TrackButton

		private void TrackButton_GotFocus(object sender, EventArgs e)
		{
			_focused = true;
			this.OnGotFocus(EventArgs.Empty);
		}

		private void TrackButton_LostFocus(object sender, EventArgs e)
		{
			_focused = false;
			this.OnLostFocus(EventArgs.Empty);
		}

		private bool _isDragging;

		private void TrackButton_MouseDown(object sender, MouseEventArgs e)
		{
			_isDragging = true;
		}

		private void TrackButton_MouseMove(object sender, MouseEventArgs e)
		{
			if (_isDragging)
			{
				Point cursorPos = NuGenControlPaint.TranslatePoint(e.Location, _trackButton, this);
				this.Value = this.GetValueFromPosition(cursorPos);
			}
		}

		private void TrackButton_MouseUp(object sender, MouseEventArgs e)
		{
			_isDragging = false;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTrackBar"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenTrackBarRenderer"/><para/>
		/// <see cref="INuGenButtonStateService"/><para/>
		/// <see cref="INuGenControlStateService"/><para/>
		/// <see cref="INuGenValueTrackerService"/><para/>
		/// </param>
		public NuGenTrackBar(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			_trackButton = new TrackButton(serviceProvider);

			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.Selectable, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

			this.BackColor = Color.Transparent;
			this.TabStop = true;

			_trackButton.Parent = this;
			_trackButton.TickStyle = this.TickStyle;

			_trackButton.MouseDown += this.TrackButton_MouseDown;
			_trackButton.MouseMove += this.TrackButton_MouseMove;
			_trackButton.MouseUp += this.TrackButton_MouseUp;
			_trackButton.GotFocus += this.TrackButton_GotFocus;
			_trackButton.LostFocus += this.TrackButton_LostFocus;

			this.ValueTracker.LargeChange = this.DefaultLargeChange;
			this.ValueTracker.Maximum = this.DefaultMaximum;
			this.ValueTracker.Minimum = this.DefaultMinimum;
			this.ValueTracker.SmallChange = this.DefaultSmallChange;
			this.ValueTracker.Value = this.DefaultValue;

			this.AdjustTrackButtonBounds();
		}

		#endregion
	}
}
