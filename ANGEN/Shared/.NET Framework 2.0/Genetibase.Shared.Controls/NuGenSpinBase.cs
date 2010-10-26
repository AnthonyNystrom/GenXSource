/* -----------------------------------------------
 * NuGenSpinBase.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.SpinInternals;
using Genetibase.Shared.Properties;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// <seealso cref="NumericUpDown"/>
	/// </summary>
	[Designer("Genetibase.Shared.Controls.Design.NuGenSpinDesigner")]
	[System.ComponentModel.DesignerCategory("Code")]
	public abstract partial class NuGenSpinBase : NuGenControl
	{
		#region Properties.Behavior

		/*
		 * InterceptArrowKeys
		 */

		private bool _interceptArrowKeys = true;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_Spin_InterceptArrowKeys")]
		public bool InterceptArrowKeys
		{
			get
			{
				return _interceptArrowKeys;
			}
			set
			{
				if (_interceptArrowKeys != value)
				{
					_interceptArrowKeys = value;
					this.OnInterceptArrowKeysChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _interceptArrowKeysChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="InterceptArrowKeys"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Spin_InterceptArrowKeysChanged")]
		public event EventHandler InterceptArrowKeysChanged
		{
			add
			{
				this.Events.AddHandler(_interceptArrowKeysChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_interceptArrowKeysChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="InterceptArrowKeysChanged"/> event.
		/// </summary>
		protected virtual void OnInterceptArrowKeysChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_interceptArrowKeysChanged, e);
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
		[DefaultValue(typeof(Color), "Window")]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
				this.EditBox.BackColor = value;
			}
		}

		#endregion

		#region Properties.Hidden

		/*
		 * Padding
		 */

		/// <summary>
		/// Do not use this property. Any value will affect the appearance.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new Padding Padding
		{
			get
			{
				return base.Padding;
			}
			set
			{
				base.Padding = value;
			}
		}

		#endregion

		#region Properties.Components

		/*
		 * ButtonDown
		 */

		private SpinButton _buttonDown;

		/// <summary>
		/// </summary>
		protected SpinButton ButtonDown
		{
			[DebuggerStepThrough]
			get
			{
				if (_buttonDown == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_buttonDown = new SpinButton(this.ServiceProvider);
					_buttonDown.Dock = DockStyle.Top;
					_buttonDown.Style = NuGenSpinButtonStyle.Down;

					_buttonDown.Click += delegate
					{
						this.OnDownButtonClick();
						this.StopDecrement();
					};

					_buttonDown.MouseDown += delegate
					{
						this.StartDecrement();
					};

					_buttonDown.MouseUp += delegate
					{
						this.StopDecrement();
					};
				}

				return _buttonDown;
			}
		}

		/*
		 * ButtonUp
		 */

		private SpinButton _buttonUp;

		/// <summary>
		/// </summary>
		protected SpinButton ButtonUp
		{
			[DebuggerStepThrough]
			get
			{
				if (_buttonUp == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_buttonUp = new SpinButton(this.ServiceProvider);
					_buttonUp.Dock = DockStyle.Top;
					_buttonUp.Style = NuGenSpinButtonStyle.Up;

					_buttonUp.Click += delegate
					{
						this.OnUpButtonClick();
						this.StopIncrement();
					};

					_buttonUp.MouseDown += delegate
					{
						this.StartIncrement();
					};

					_buttonUp.MouseUp += delegate
					{
						this.StopIncrement();
					};
				}

				return _buttonUp;
			}
		}

		/*
		 * EditBox
		 */

		private SpinEdit _editBox;

		/// <summary>
		/// </summary>
		protected SpinEdit EditBox
		{
			[DebuggerStepThrough]
			get
			{
				if (_editBox == null)
				{
					_editBox = new SpinEdit();

					_editBox.Width = this.ClientRectangle.Width - this.ClientRectangle.Left - SystemInformation.VerticalScrollBarWidth - 2;
					_editBox.Top = this.ClientRectangle.Top + this.ClientRectangle.Height / 2 - _editBox.Height / 2;
					_editBox.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;

					_editBox.KeyDown += delegate(object sender, KeyEventArgs e)
					{
						this.OnKeyDown(e);
					};

					_editBox.KeyUp += delegate(object sender, KeyEventArgs e)
					{
						this.OnKeyUp(e);
					};

					_editBox.TextChanged += delegate(object sender, EventArgs e)
					{
						this.OnEditBoxTextChanged();
					};
				}

				return _editBox;
			}
		}

		/*
		 * FastSwitchTimer
		 */

		private Timer _fastSwitchTimer;

		/// <summary>
		/// </summary>
		protected Timer FastSwitchTimer
		{
			[DebuggerStepThrough]
			get
			{
				if (_fastSwitchTimer == null)
				{
					_fastSwitchTimer = new Timer();
					_fastSwitchTimer.Interval = _timerInterval;
					_fastSwitchTimer.Tick += delegate
					{
						this.DecrementTimer.Interval = Math.Max(this.DecrementTimer.Interval / 2, 10);
						this.IncrementTimer.Interval = Math.Max(this.IncrementTimer.Interval / 2, 10);

						_fastSwitchTimer.Interval = Math.Max(_fastSwitchTimer.Interval / 2, 10);
					};
				}

				return _fastSwitchTimer;
			}
		}

		/*
		 * DecrementTimer
		 */

		private Timer _decrementTimer;

		/// <summary>
		/// </summary>
		protected Timer DecrementTimer
		{
			[DebuggerStepThrough]
			get
			{
				if (_decrementTimer == null)
				{
					_decrementTimer = new Timer();
					_decrementTimer.Tick += delegate
					{
						this.OnDownButtonClick();
					};
				}

				return _decrementTimer;
			}
		}

		/*
		 * IncrementTimer
		 */

		private Timer _incrementTimer;

		/// <summary>
		/// </summary>
		protected Timer IncrementTimer
		{
			[DebuggerStepThrough]
			get
			{
				if (_incrementTimer == null)
				{
					_incrementTimer = new Timer();
					_incrementTimer.Tick += delegate
					{
						this.OnUpButtonClick();
					};
				}

				return _incrementTimer;
			}
		}

		#endregion

		#region Properties.Services

		/*
		 * Renderer
		 */

		private INuGenSpinRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenSpinRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					_renderer = this.ServiceProvider.GetService<INuGenSpinRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenSpinRenderer>();
					}
				}

				return _renderer;
			}
		}

		#endregion

		#region Properties.Protected.Overridden

		private static readonly Size _defaultSize = new Size(100, 20);

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

		#region Methods.Public.Abstract

		/*
		 * OnDownButtonClick
		 */

		/// <summary>
		/// </summary>
		public abstract void OnDownButtonClick();

		/*
		 * OnUpButtonClick
		 */

		/// <summary>
		/// </summary>
		public abstract void OnUpButtonClick();

		/*
		 * OnEditBoxTextChanged
		 */

		/// <summary>
		/// </summary>
		public abstract void OnEditBoxTextChanged();

		#endregion

		#region Methods.Protected.Overridden

		/*
		 * OnEnabledChanged
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.EnabledChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnEnabledChanged(EventArgs e)
		{
			Debug.Assert(this.StateTracker != null, "this.StateTracker != null");
			this.StateTracker.Enabled(this.Enabled);
			base.OnEnabledChanged(e);
		}

		/*
		 * OnGotFocus
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.GotFocus"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnGotFocus(EventArgs e)
		{
			Debug.Assert(this.StateTracker != null, "this.StateTracker != null");
			this.StateTracker.GotFocus();
			base.OnGotFocus(e);
		}

		/*
		 * OnHandleCreated
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			Debug.Assert(_spinButtonsPanel != null, "_spinButtonsPanel != null");
			Debug.Assert(this.EditBox != null, "this.EditBox != null");

			if (this.RightToLeft == RightToLeft.No)
			{
				_spinButtonsPanel.Dock = DockStyle.Right;
				this.EditBox.Left = this.ClientRectangle.Left + 2;

			}
			else
			{
				_spinButtonsPanel.Dock = DockStyle.Left;
				this.EditBox.Left = this.ClientRectangle.Right - this.EditBox.Width - 2;
			}
		}

		/*
		 * OnKeyDown
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.KeyDown"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.KeyEventArgs"></see> that contains the event data.</param>
		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (this.InterceptArrowKeys)
			{
				if (e.KeyData == Keys.Up)
				{
					this.OnUpButtonClick();
				}
				else if (e.KeyData == Keys.Down)
				{
					this.OnDownButtonClick();
				}
			}
		}

		/*
		 * OnLostFocus
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.LostFocus"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnLostFocus(EventArgs e)
		{
			Debug.Assert(this.StateTracker != null, "this.StateTracker != null");
			this.StateTracker.LostFocus();
			base.OnLostFocus(e);
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
			Debug.Assert(this.Renderer != null, "this.Renderer != null");
			Debug.Assert(this.StateTracker != null, "this.StateTracker != null");

			NuGenPaintParams paintParams = new NuGenPaintParams(e.Graphics);
			paintParams.Bounds = this.ClientRectangle;
			paintParams.State = this.StateTracker.GetControlState();

			this.Renderer.DrawBorder(paintParams);
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
			height = this.Height;
			base.SetBoundsCore(x, y, width, height, specified);
		}

		#endregion

		#region Methods.Private

		/*
		 * StartDecrement
		 */

		private void StartDecrement()
		{
			this.FastSwitchTimer.Interval = _timerInterval;
			this.DecrementTimer.Interval = _timerInterval;
			this.DecrementTimer.Start();
			this.FastSwitchTimer.Start();
		}

		/*
		 * StopDecrement
		 */

		private void StopDecrement()
		{
			this.FastSwitchTimer.Stop();
			this.DecrementTimer.Stop();
		}

		/*
		 * StartIncrement
		 */

		private void StartIncrement()
		{
			this.FastSwitchTimer.Interval = _timerInterval;
			this.IncrementTimer.Interval = _timerInterval;
			this.IncrementTimer.Start();
			this.FastSwitchTimer.Start();
		}

		/*
		 * StopIncrement
		 */

		private void StopIncrement()
		{
			this.FastSwitchTimer.Stop();
			this.IncrementTimer.Stop();
		}

		#endregion

		private const int _timerInterval = 500;
		private Panel _spinButtonsPanel = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSpin"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requries:<para/>
		/// <see cref="INuGenSpinRenderer"/><para/>
		/// <see cref="INuGenControlStateService"/><para/>
		/// </param>
		protected NuGenSpinBase(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.BackColor = SystemColors.Window;

			_spinButtonsPanel = new Panel();

			_spinButtonsPanel.Width = SystemInformation.VerticalScrollBarWidth;
			_spinButtonsPanel.Parent = this;

			_spinButtonsPanel.Controls.Add(this.ButtonDown);
			_spinButtonsPanel.Controls.Add(this.ButtonUp);

			this.Controls.Add(this.EditBox);
		}
	}
}
