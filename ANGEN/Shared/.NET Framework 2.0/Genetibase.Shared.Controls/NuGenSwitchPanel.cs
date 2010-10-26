/* -----------------------------------------------
 * NuGenSwitchPanel.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.SwitcherInternals;
using Genetibase.Shared.Drawing;
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
	/// Represents a panel that supports horizontal and vertical layout
	/// for a collection of <see cref="NuGenSwitchButton"/> instances.
	/// </summary>
	[ToolboxItem(false)]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenSwitchPanel : NuGenOrientationControlBase
	{
		#region Properties.Public

		/*
		 * CheckOnAdd
		 */

		private bool _checkOnAdd = true;

		/// <summary>
		/// Gets or sets the value indicating whether the newly added button appears checked.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_SwitchPanel_CheckOnAdd")]
		public bool CheckOnAdd
		{
			get
			{
				return _checkOnAdd;
			}
			set
			{
				if (_checkOnAdd != value)
				{
					_checkOnAdd = value;
					this.OnCheckOnAddChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _checkOnAddChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="CheckOnAdd"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_SwitchPanel_CheckOnAddChanged")]
		public event EventHandler CheckOnAddChanged
		{
			add
			{
				this.Events.AddHandler(_checkOnAddChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_checkOnAddChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="CheckOnAddChanged"/> event.
		/// </summary>
		protected virtual void OnCheckOnAddChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_checkOnAddChanged, e);
		}

		/*
		 * CheckOnRemove
		 */

		private bool _checkOnRemove = true;

		/// <summary>
		/// Gets or sets the value indicating whether a new button is checked if the removed
		/// button was checked.
		/// </summary>
		public bool CheckOnRemove
		{
			get
			{
				return _checkOnRemove;
			}
			set
			{
				if (_checkOnRemove != value)
				{
					_checkOnRemove = value;
					this.OnCheckOnRemoveChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _checkOnRemoveChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="CheckOnRemove"/> property changes.
		/// </summary>
		public event EventHandler CheckOnRemoveChanged
		{
			add
			{
				this.Events.AddHandler(_checkOnRemoveChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_checkOnRemoveChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="CheckOnRemoveChanged"/> event.
		/// </summary>
		protected virtual void OnCheckOnRemoveChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_checkOnRemoveChanged, e);
		}

		/*
		 * SelectedSwitchButton
		 */

		private NuGenSwitchButton _selectedSwitchButton;

		/// <summary>
		/// </summary>
		public NuGenSwitchButton SelectedSwitchButton
		{
			get
			{
				return _selectedSwitchButton;
			}
			set
			{
				if (_selectedSwitchButton != value)
				{
					foreach (NuGenSwitchButton switchButton in _switchButtons)
					{
						switchButton.Checked = false;
					}

					_selectedSwitchButton = value;

					if (_selectedSwitchButton != null)
					{
						_selectedSwitchButton.Checked = true;
					}

					this.OnSelectedSwitchButtonChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _selectedSwitchButtonChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="SelectedSwitchButton"/> property changes.
		/// </summary>
		public event EventHandler SelectedSwitchButtonChanged
		{
			add
			{
				this.Events.AddHandler(_selectedSwitchButtonChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_selectedSwitchButtonChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenSwitchPanel.SelectedSwitchButtonChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnSelectedSwitchButtonChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_selectedSwitchButtonChanged, e);
		}

		/*
		 * SwitchButtonSize
		 */

		private Size _switchButtonSize;

		/// <summary>
		/// X-coordinates determines the width of the buttons in horizontal orientation;
		/// Y-coordinates determines the height of the buttons in vertical orientation.
		/// </summary>
		public Size SwitchButtonSize
		{
			get
			{
				if (_switchButtonSize == Size.Empty)
				{
					return NuGenSwitchPanel.DefaultSwitchButtonSize;
				}

				return _switchButtonSize;
			}
			set
			{
				if (_switchButtonSize != value)
				{
					_switchButtonSize = value;
					this.OnSwitchButtonSizeChanged(EventArgs.Empty);

					Debug.Assert(_switchButtons != null, "_switchButtons != null");
					foreach (NuGenSwitchButton button in _switchButtons)
					{
						button.Size = _switchButtonSize;
					}
				}
			}
		}

		private static readonly Size _defaultSwitchButtonSize = new Size(54, 54);

		internal static Size DefaultSwitchButtonSize
		{
			get
			{
				return _defaultSwitchButtonSize;
			}
		}

		private static readonly object _switchButtonSizeChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="SwitchButtonSize"/> property changes.
		/// </summary>
		public event EventHandler SwitchButtonSizeChanged
		{
			add
			{
				this.Events.AddHandler(_switchButtonSizeChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_switchButtonSizeChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="SwitchButtonSizeChanged"/> event.
		/// </summary>
		protected virtual void OnSwitchButtonSizeChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_switchButtonSizeChanged, e);
		}

		/*
		 * SwitchButtonImageAlign
		 */

		private ContentAlignment _switchButtonImageAlign = NuGenSwitchPanel.DefaultSwitchButtonImageAlign;

		/// <summary>
		/// Gets or sets the alignment of the image on the switch buttons.
		/// </summary>
		public ContentAlignment SwitchButtonImageAlign
		{
			get
			{
				return _switchButtonImageAlign;
			}
			set
			{
				if (_switchButtonImageAlign != value)
				{
					_switchButtonImageAlign = value;
					this.OnSwitchButtonImageAlignChanged(EventArgs.Empty);

					Debug.Assert(_switchButtons != null, "_switchButtons != null");
					foreach (NuGenSwitchButton button in _switchButtons)
					{
						button.ImageAlign = this.SwitchButtonImageAlign;
					}
				}
			}
		}

		internal static ContentAlignment DefaultSwitchButtonImageAlign
		{
			get
			{
				return ContentAlignment.TopCenter;
			}
		}

		private static readonly object _switchButtonImageAlignChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="SwitchButtonImageAlign"/> property changes.
		/// </summary>
		public event EventHandler SwitchButtonImageAlignChanged
		{
			add
			{
				this.Events.AddHandler(_switchButtonImageAlignChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_switchButtonImageAlignChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="SwitchButtonImageAlignChanged"/> event.
		/// </summary>
		protected virtual void OnSwitchButtonImageAlignChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_switchButtonImageAlignChanged, e);
		}

		/*
		 * SwitchButtonTextAlign
		 */

		private ContentAlignment _switchButtonTextAlign = NuGenSwitchPanel.DefaultSwitchButtonTextAlign;

		/// <summary>
		/// Gets or sets the alignment of the text of the switch buttons.
		/// </summary>
		public ContentAlignment SwitchButtonTextAlign
		{
			get
			{
				return _switchButtonTextAlign;
			}
			set
			{
				if (_switchButtonTextAlign != value)
				{
					_switchButtonTextAlign = value;
					this.OnSwitchButtonTextAlignChanged(EventArgs.Empty);

					Debug.Assert(_switchButtons != null, "_switchButtons != null");
					foreach (NuGenSwitchButton button in _switchButtons)
					{
						button.TextAlign = this.SwitchButtonTextAlign;
					}
				}
			}
		}

		internal static ContentAlignment DefaultSwitchButtonTextAlign
		{
			get
			{
				return ContentAlignment.BottomCenter;
			}
		}

		private static readonly object _switchButtonTextAlignChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="SwitchButtonTextAlign"/> property changes.
		/// </summary>
		public event EventHandler SwitchButtonTextAlignChanged
		{
			add
			{
				this.Events.AddHandler(_switchButtonTextAlignChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_switchButtonTextAlignChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="SwitchButtonTextAlignChanged"/> event.
		/// </summary>
		protected virtual void OnSwitchButtonTextAlignChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_switchButtonTextAlignChanged, e);
		}

		#endregion

		#region Properties.Protected.Overridden

		private static readonly Size _defaultSize = new Size(260, 54);

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

		private INuGenPanelRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenPanelRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_renderer = this.ServiceProvider.GetService<INuGenPanelRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenPanelRenderer>();
					}
				}

				return _renderer;
			}
		}

		#endregion

		#region Methods.Public

		/// <summary>
		/// </summary>
		/// <param name="pointToTest">Screen coordinates expected.</param>
		public NuGenSwitcherHitResult HitTest(Point pointToTest)
		{
			Point cp = this.PointToClient(pointToTest);

			if (this.ClientRectangle.Contains(cp))
			{
				foreach (Control ctrl in this.Controls)
				{
					NuGenSwitchButton switchButton = ctrl as NuGenSwitchButton;

					if (switchButton != null && switchButton.Bounds.Contains(cp))
					{
						return NuGenSwitcherHitResult.SwitchButtons;
					}
				}

				return NuGenSwitcherHitResult.SwitchPanel;
			}

			return NuGenSwitcherHitResult.Nowhere;
		}

		#endregion

		#region Methods.Protected.Overridden

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.ControlAdded"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ControlEventArgs"></see> that contains the event data.</param>
		protected override void OnControlAdded(ControlEventArgs e)
		{
			base.OnControlAdded(e);
			NuGenSwitchButton switchButton = e.Control as NuGenSwitchButton;

			if (switchButton != null)
			{
				_switchButtons.Add(switchButton);
				this.InitializeSwitchButton(switchButton);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.ControlRemoved"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ControlEventArgs"></see> that contains the event data.</param>
		protected override void OnControlRemoved(ControlEventArgs e)
		{
			base.OnControlRemoved(e);
			NuGenSwitchButton switchButton = e.Control as NuGenSwitchButton;

			if (switchButton != null)
			{
				_switchButtons.Remove(switchButton);
				this.ResetSwitchButton(switchButton);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenOrientationControlBase.OrientationChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnOrientationChanged(EventArgs e)
		{
			base.OnOrientationChanged(e);

			foreach (NuGenSwitchButton switchButton in _switchButtons)
			{
				this.SetSwitchButtonDockStyle(switchButton, this.Orientation);
			}

			this.Padding = this.GetPadding();
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			NuGenBorderPaintParams paintParams = new NuGenBorderPaintParams(e.Graphics);
			paintParams.Bounds = this.ClientRectangle;
			paintParams.DrawBorder = true;
			paintParams.State = this.StateTracker.GetControlState();

			this.Renderer.DrawBackground(paintParams);
			this.Renderer.DrawBorder(paintParams);

			base.OnPaint(e);
		}

		#endregion

		#region Methods.Private

		private Padding GetPadding()
		{
			if (this.Orientation == NuGenOrientationStyle.Horizontal)
			{
				return NuGenSwitchPanel.GetHorizontalPadding();
			}

			return NuGenSwitchPanel.GetVerticalPadding();
		}

		private static readonly Padding _horizontalPadding = new Padding(1, 0, 1, 0);

		private static Padding GetHorizontalPadding()
		{
			return _horizontalPadding;
		}

		private static readonly Padding _verticalPadding = new Padding(0, 1, 0, 1);

		private static Padding GetVerticalPadding()
		{
			return _verticalPadding;
		}

		private void InitializeSwitchButton(NuGenSwitchButton switchButton)
		{
			Debug.Assert(switchButton != null, "switchButton != null");
			this.SetSwitchButtonDockStyle(switchButton, this.Orientation);
			switchButton.Click += _switchButton_Click;
			switchButton.ImageAlign = this.SwitchButtonImageAlign;
			switchButton.Orientation = this.Orientation;
			switchButton.TextAlign = this.SwitchButtonTextAlign;
			switchButton.Size = this.SwitchButtonSize;
			this.SelectedSwitchButton = switchButton;
			switchButton.BringToFront();
		}

		private void ResetSwitchButton(NuGenSwitchButton switchButton)
		{
			Debug.Assert(switchButton != null, "switchButton != null");
			switchButton.Click -= _switchButton_Click;

			if (_switchButtons.Count > 0)
			{
				this.SelectedSwitchButton = _switchButtons[_switchButtons.Count - 1];
			}
		}

		private void SetSwitchButtonDockStyle(NuGenSwitchButton switchButton, NuGenOrientationStyle orientation)
		{
			Debug.Assert(switchButton != null, "switchButton != null");
			switchButton.Orientation = orientation;

			if (orientation == NuGenOrientationStyle.Horizontal)
			{
				switchButton.Dock = DockStyle.Left;
			}
			else
			{
				switchButton.Dock = DockStyle.Top;
			}
		}

		#endregion

		#region EventHandlers.SwitchButton

		private void _switchButton_Click(object sender, EventArgs e)
		{
			this.SelectedSwitchButton = (NuGenSwitchButton)sender;
		}

		#endregion

		private IList<NuGenSwitchButton> _switchButtons;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSwitchPanel"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		///		<para><see cref="INuGenButtonStateService"/></para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		///		<para><see cref="INuGenPanelRenderer"/></para>
		///		<para><see cref="INuGenSwitchButtonRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSwitchPanel(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			_switchButtons = new List<NuGenSwitchButton>();

			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.Opaque, false);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.Selectable, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.UserPaint, true);

			this.Padding = this.GetPadding();
		}
	}
}
