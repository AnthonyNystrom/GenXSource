/* -----------------------------------------------
 * NuGenWndLessControl.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// Represents a window-less control with event support.
	/// </summary>
	public class NuGenWndLessControl : NuGenEventInitiator
	{
		#region Events

		/*
		 * Click
		 */

		private static readonly object _click = new object();

		/// <summary>
		/// Occurs when this control is clicked.
		/// </summary>
		public event EventHandler Click
		{
			add
			{
				this.Events.AddHandler(_click, value);
			}
			remove
			{
				this.Events.RemoveHandler(_click, value);
			}
		}

		/// <summary>
		/// Add custom logic before the <see cref="E:Genetibase.Shared.Windows.NuGenWndLessControl.Click"/> event will be raised.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnClick(EventArgs e)
		{
		}

		/*
		 * GotFocus
		 */

		private static readonly object _gotFocus = new object();

		/// <summary>
		/// Occurs when this control receives focus.
		/// </summary>
		public event EventHandler GotFocus
		{
			add
			{
				this.Events.AddHandler(_gotFocus, value);	
			}
			remove
			{
				this.Events.RemoveHandler(_gotFocus, value);
			}
		}

		/// <summary>
		/// Add custom logic before the <see cref="E:Genetibase.Shared.Windows.NuGenWndLessControl.GotFocus"/> event will be raised.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnGotFocus(EventArgs e)
		{
		}

		/*
		 * LostFocus
		 */

		private static readonly object _lostFocus = new object();

		/// <summary>
		/// Occurs when this control loses focus.
		/// </summary>
		public event EventHandler LostFocus
		{
			add
			{
				this.Events.AddHandler(_lostFocus, value);
			}
			remove
			{
				this.Events.RemoveHandler(_lostFocus, value);
			}
		}

		/// <summary>
		/// Add custom logic before the <see cref="E:Genetibase.Shared.Windows.NuGenWndLessControl.LostFocus"/> event will be raised.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnLostFocus(EventArgs e)
		{
		}

		/*
		 * MouseDown
		 */

		private static readonly object _mouseDown = new object();

		/// <summary>
		/// Occurs when a mouse button is pressed over this <see cref="NuGenWndLessControl"/> surface.
		/// </summary>
		public event MouseEventHandler MouseDown
		{
			add
			{
				this.Events.AddHandler(_mouseDown, value);
			}
			remove
			{
				this.Events.RemoveHandler(_mouseDown, value);
			}
		}

		/// <summary>
		/// Add custom logic before the <see cref="E:Genetibase.Shared.Windows.NuGenWndLessControl.MouseDown"/> event will be raised.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnMouseDown(MouseEventArgs e)
		{
			this.Invalidate();
		}

		/*
		 * MouseEnter
		 */

		private static readonly object _mouseEnter = new object();

		/// <summary>
		/// Occurs when the mouse pointer enters this <see cref="NuGenWndLessControl"/> surface.
		/// </summary>
		public event EventHandler MouseEnter
		{
			add
			{
				this.Events.AddHandler(_mouseEnter, value);
			}
			remove
			{
				this.Events.RemoveHandler(_mouseEnter, value);
			}
		}

		/// <summary>
		/// Add custom logic before the <see cref="E:Genetibase.Shared.Windows.NuGenWndLessControl.MouseEnter"/> event will be raised.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnMouseEnter(EventArgs e)
		{
			this.Invalidate();
		}

		/*
		 * MouseLeave
		 */

		private static readonly object _mouseLeave = new object();

		/// <summary>
		/// Occurs when the mouse pointer leaves this <see cref="NuGenWndLessControl"/> surface.
		/// </summary>
		public event EventHandler MouseLeave
		{
			add
			{
				this.Events.AddHandler(_mouseLeave, value);
			}
			remove
			{
				this.Events.RemoveHandler(_mouseLeave, value);
			}
		}

		/// <summary>
		/// Add custom logic before the <see cref="E:Genetibase.Shared.Windows.NuGenWndLessControl.MouseLeave"/> event will be raised.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnMouseLeave(EventArgs e)
		{
			this.Invalidate();
		}

		/*
		 * MouseMove
		 */

		private static readonly object _mouseMove = new object();

		/// <summary>
		/// Occurs when the mouse pointer is moving over this <see cref="NuGenWndLessControl"/> surface.
		/// </summary>
		public event MouseEventHandler MouseMove
		{
			add
			{
				this.Events.AddHandler(_mouseMove, value);
			}
			remove
			{
				this.Events.RemoveHandler(_mouseMove, value);
			}
		}

		/// <summary>
		/// Add custom logic before the <see cref="E:Genetibase.Shared.Windows.NuGenWndLessControl.MouseMove"/> event will be raised.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnMouseMove(MouseEventArgs e)
		{
		}

		/*
		 * MouseUp
		 */

		private static readonly object _mouseUp = new object();

		/// <summary>
		/// Occurs when a mouse button is released.
		/// </summary>
		public event MouseEventHandler MouseUp
		{
			add
			{
				this.Events.AddHandler(_mouseUp, value);
			}
			remove
			{
				this.Events.RemoveHandler(_mouseUp, value);
			}
		}

		/// <summary>
		/// Add custom logic before the <see cref="E:Genetibase.Shared.Windows.NuGenWndLessControl.MouseUp"/> event will be raised.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnMouseUp(MouseEventArgs e)
		{
			this.Invalidate();
		}

		/*
		 * Paint
		 */

		private static readonly object _paint = new object();

		/// <summary>
		/// Occurs when this <see cref="NuGenWndLessControl"/> is redrawn.
		/// </summary>
		public event PaintEventHandler Paint
		{
			add
			{
				this.Events.AddHandler(_paint, value);
			}
			remove
			{
				this.Events.RemoveHandler(_paint, value);
			}
		}

		/// <summary>
		/// Add custom logic before the <see cref="E:Genetibase.Shared.Windows.NuGenWndLessControl.Paint"/> event will be raised.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnPaint(PaintEventArgs e)
		{
		}

		#endregion

		#region Properties.Public

		/*
		 * Bounds
		 */

		private Rectangle _bounds = Rectangle.Empty;
		private Point _defaultLocation = new Point(0, 0);

		/// <summary>
		/// Gets or sets this <see cref="NuGenWndLessControl"/> bounds.
		/// </summary>
		public Rectangle Bounds
		{
			get
			{
				if (_bounds == Rectangle.Empty)
				{
					return new Rectangle(_defaultLocation, this.DefaultSize);
				}

				return _bounds;
			}
			set
			{
				_bounds = value;
			}
		}

		private static Size _defaultSize = new Size(100, 100);

		/// <summary>
		/// </summary>
		protected virtual Size DefaultSize
		{
			get
			{
				return _defaultSize;
			}
		}

		/*
		 * Enabled
		 */

		private bool _enabled = true;

		/// <summary>
		/// </summary>
		public bool Enabled
		{
			get
			{
				return _enabled;
			}
			set
			{
				if (_enabled != value)
				{
					_enabled = value;
					this.OnEnabledChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _enabledChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Shared.Windows.NuGenWndLessControl.Enabled"/> property changes.
		/// </summary>
		public event EventHandler EnabledChanged
		{
			add
			{
				this.Events.AddHandler(_enabledChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_enabledChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Windows.NuGenWndLessControl.EnabledChanged"/> event.
		/// </summary>
		protected virtual void OnEnabledChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_enabledChanged, e);
		}

		/*
		 * Parent
		 */

		private Control _parent;

		/// <summary>
		/// </summary>
		public Control Parent
		{
			get
			{
				return _parent;
			}
			set
			{
				if (_parent != value)
				{
					this.UnsubscribeParentEvents(_parent);
					_parent = value;
					this.SubscribeParentEvents(_parent);
					this.OnParentChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _parentChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Parent"/> property changes.
		/// </summary>
		public event EventHandler ParentChanged
		{
			add
			{
				this.Events.AddHandler(_parentChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_parentChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="ParentChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnParentChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_parentChanged, e);
		}

		/*
		 * Visible
		 */

		private bool _visible = true;

		/// <summary>
		/// </summary>
		public bool Visible
		{
			get
			{
				return _visible;
			}
			set
			{
				if (_visible != value)
				{
					_visible = value;
					this.OnVisibleChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private static readonly object _visibleChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Visible"/> property changes.
		/// </summary>
		public event EventHandler VisibleChanged
		{
			add
			{
				this.Events.AddHandler(_visibleChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_visibleChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="VisibleChanged"/> event.
		/// </summary>
		protected virtual void OnVisibleChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_visibleChanged, e);
		}

		#endregion

		#region Methods.Public

		/*
		 * Invalidate
		 */

		/// <summary>
		/// Repaint the parent control area this <see cref="NuGenWndLessControl"/> occupies.
		/// </summary>
		public void Invalidate()
		{
			if (this.Parent != null)
			{
				this.Parent.Invalidate(this.Bounds);
			}
		}

		#endregion

		#region Methods.Private

		/*
		 * SubscribeParentEvents
		 */

		private void SubscribeParentEvents(Control parent)
		{
			if (parent != null)
			{
				parent.EnabledChanged += this.Parent_EnabledChanged;
				parent.GotFocus += this.Parent_GotFocus;
				parent.LostFocus += this.Parent_LostFocus;
				parent.MouseClick += this.Parent_MouseClick;
				parent.MouseDown += this.Parent_MouseDown;
				parent.MouseEnter += this.Parent_MouseEnter;
				parent.MouseLeave += this.Parent_MouseLeave;
				parent.MouseMove += this.Parent_MouseMove;
				parent.MouseUp += this.Parent_MouseUp;
				parent.Paint += this.Parent_Paint;
			}
		}

		/*
		 * UnsubscribeParentEvents
		 */

		private void UnsubscribeParentEvents(Control parent)
		{
			if (parent != null)
			{
				parent.EnabledChanged -= this.Parent_EnabledChanged;
				parent.GotFocus -= this.Parent_GotFocus;
				parent.LostFocus -= this.Parent_LostFocus;
				parent.MouseClick -= this.Parent_MouseClick;
				parent.MouseDown -= this.Parent_MouseDown;
				parent.MouseEnter -= this.Parent_MouseEnter;
				parent.MouseLeave -= this.Parent_MouseLeave;
				parent.MouseMove -= this.Parent_MouseMove;
				parent.MouseUp -= this.Parent_MouseUp;
				parent.Paint -= this.Parent_Paint;
			}
		}

		#endregion

		#region EventHandlers.Parent

		private void Parent_EnabledChanged(object sender, EventArgs e)
		{
			this.Enabled = this.Parent.Enabled;
		}

		private void Parent_GotFocus(object sender, EventArgs e)
		{
			this.OnGotFocus(e);
			this.Initiator.InvokeEventHandler(this, e);
		}

		private void Parent_LostFocus(object sender, EventArgs e)
		{
			this.OnLostFocus(e);
			this.Initiator.InvokeEventHandler(this, e);
		}

		private void Parent_MouseClick(object sender, MouseEventArgs e)
		{
			if (this.Enabled && this.Bounds.Contains(e.Location))
			{
				this.OnClick(e);
				this.Initiator.InvokeEventHandler(_click, e);
			}
		}

		private bool _mouseButtonPressedInside;

		private void Parent_MouseDown(object sender, MouseEventArgs e)
		{
			if (this.Enabled && this.Bounds.Contains(e.Location))
			{
				_mouseButtonPressedInside = true;

				this.OnMouseDown(e);
				this.Initiator.InvokeMouseEventHandler(_mouseDown, e);
			}
		}

		private void Parent_MouseEnter(object sender, EventArgs e)
		{
			Debug.Assert(this.Parent != null, "this.Parent != null");
			Point cursorPosition = this.Parent.PointToClient(Cursor.Position);

			if (this.Enabled && this.Bounds.Contains(cursorPosition))
			{
				this.OnMouseEnter(e);
				this.Initiator.InvokeEventHandler(_mouseEnter, e);
			}
		}

		private void Parent_MouseLeave(object sender, EventArgs e)
		{
			Debug.Assert(this.Parent != null, "this.Parent != null");

			if (_mouseInside)
			{
				this.OnMouseLeave(e);
				this.Initiator.InvokeEventHandler(_mouseLeave, e);
			}
		}

		private void Parent_MouseMove(object sender, MouseEventArgs e)
		{
			if (this.Enabled)
			{
				if (this.Bounds.Contains(e.Location))
				{
					if (!_mouseInside)
					{
						this.OnMouseEnter(EventArgs.Empty);
						this.Initiator.InvokeEventHandler(_mouseEnter, EventArgs.Empty);

						_mouseInside = true;
					}

					this.OnMouseMove(e);
					this.Initiator.InvokeMouseEventHandler(_mouseMove, e);
				}
				else if (!this.Parent.Capture)
				{
					if (_mouseInside)
					{
						this.OnMouseLeave(EventArgs.Empty);
						this.Initiator.InvokeEventHandler(_mouseLeave, EventArgs.Empty);

						_mouseInside = false;
					}
				}
			}
		}

		private void Parent_MouseUp(object sender, MouseEventArgs e)
		{
			if (_mouseButtonPressedInside)
			{
				_mouseButtonPressedInside = false;

				this.OnMouseUp(e);
				this.Initiator.InvokeMouseEventHandler(_mouseUp, e);
			}
		}

		private void Parent_Paint(object sender, PaintEventArgs e)
		{
			if (_visible)
			{
				this.OnPaint(e);

				PaintEventHandler handler = this.Events[_paint] as PaintEventHandler;

				if (handler != null)
				{
					handler(this, e);
				}
			}
		}

		#endregion

		private bool _mouseInside;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenWndLessControl"/> class.
		/// </summary>
		public NuGenWndLessControl()
		{
		}
	}
}
