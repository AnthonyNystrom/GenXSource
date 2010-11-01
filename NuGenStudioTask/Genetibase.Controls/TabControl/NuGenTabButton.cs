/* -----------------------------------------------
 * NuGenTabButton.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Genetibase.Controls
{
	/// <summary>
	/// </summary>
	[ToolboxItem(false)]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenTabButton : Button
	{
		#region Events

		/*
		 * Close
		 */

		private static readonly object _close = new object();

		/// <summary>
		/// Occurs when the close button was clicked.
		/// </summary>
		public event EventHandler Close
		{
			add
			{
				this.Events.AddHandler(_close, value);
			}
			remove
			{
				this.Events.RemoveHandler(_close, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="Close"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnClose(EventArgs e)
		{
			this.Initiator.InvokeAction(_close, e);
		}

		#endregion
		
		#region Properties.Public

		/*
		 * Image
		 */

		private Image _image = null;

		/// <summary>
		/// Gets or sets the image this <see cref="NuGenTabButton"/> displays.
		/// </summary>
		public new Image Image
		{
			get
			{
				return _image;
			}
			set
			{
				if (_image != value)
				{
					_image = value;
					this.OnImageChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private static readonly object _imageChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Image"/> property changes.
		/// </summary>
		public event EventHandler ImageChanged
		{
			add
			{
				this.Events.AddHandler(_imageChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_imageChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="ImageChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnImageChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_imageChanged, e);
		}

		/*
		 * Selected
		 */

		private bool _selected = false;

		/// <summary>
		/// </summary>
		public bool Selected
		{
			get
			{
				return _selected;
			}
			set
			{
				if (_selected != value)
				{
					_selected = value;
					this.OnSelectedChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private static readonly object _selectedChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Selected"/> property changes.
		/// </summary>
		public event EventHandler SelectedChanged
		{
			add
			{
				this.Events.AddHandler(_selectedChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_selectedChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="SelectedChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnSelectedChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_selectedChanged, e);

			if (this.Selected)
			{
				this.Font = new Font(this.Font, FontStyle.Bold);
				this.TabStateTracker.Select(this);
			}
			else
			{
				this.Font = new Font(this.Font, FontStyle.Regular);
				this.TabStateTracker.Deselect(this);
			}
		}
		
		/*
		 * ShowCloseButton
		 */

		private bool _showCloseButton = false;

		/// <summary>
		/// Gets or sets the value indicating whether to show the close button for this <see cref="NuGenTabButton"/>.
		/// </summary>
		public bool ShowCloseButton
		{
			get
			{
				return _showCloseButton;
			}
			set
			{
				if (_showCloseButton != value)
				{
					_showCloseButton = value;
					this.CloseButton.Visible = _showCloseButton;
				}
			}
		}

		#endregion

		#region Properties.Public.Overriden

		/*
		 * Text
		 */

		/// <summary>
		/// </summary>
		/// <value></value>
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		[EditorBrowsable(EditorBrowsableState.Always)]
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
		/// Occurs when the tab button text changes.
		/// </summary>
		[Browsable(true)]
		[EditorBrowsable(EditorBrowsableState.Always)]
		public new event EventHandler TextChanged
		{
			add
			{
				base.TextChanged += value;
			}
			remove
			{
				base.TextChanged -= value;
			}
		}

		#endregion

		#region Properties.Protected

		/*
		 * CloseButton
		 */

		private NuGenTabCloseButton _closeButton = null;

		/// <summary>
		/// Read-only.
		/// </summary>
		protected NuGenTabCloseButton CloseButton
		{
			get
			{
				if (_closeButton == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_closeButton = new NuGenTabCloseButton(this.ServiceProvider);
				}

				return _closeButton;
			}
		}

		/*
		 * TabRenderer
		 */

		private INuGenTabRenderer _tabRenderer = null;

		/// <summary>
		/// </summary>
		protected INuGenTabRenderer TabRenderer
		{
			get
			{
				if (_tabRenderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_tabRenderer = this.ServiceProvider.GetService<INuGenTabRenderer>();

					if (_tabRenderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenTabRenderer>();
					}
				}

				return _tabRenderer;
			}
		}

		/*
		 * TabStateTracker
		 */

		private INuGenTabStateTracker _tabStateTracker = null;

		/// <summary>
		/// </summary>
		protected INuGenTabStateTracker TabStateTracker
		{
			get
			{
				if (_tabStateTracker == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_tabStateTracker = this.ServiceProvider.GetService<INuGenTabStateTracker>();

					if (_tabStateTracker == null)
					{
						throw new NuGenServiceNotFoundException<INuGenTabStateTracker>();
					}
				}

				return _tabStateTracker;
			}
		}

		#endregion

		#region Properties.Protected.Overriden

		/*
		 * DefaultSize
		 */

		private Size _defaultSize = Size.Empty;

		/// <summary>
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				if (_defaultSize == Size.Empty)
				{
					_defaultSize = new Size(250, 24);
				}

				return _defaultSize;
			}
		}

		#endregion

		#region Properties.Protected.Virtual

		/*
		 * CloseButtonBounds
		 */

		/// <summary>
		/// </summary>
		protected virtual Rectangle CloseButtonBounds
		{
			get
			{
				Debug.Assert(this.CloseButton != null, "this.CloseButton != null");
				return new Rectangle(
					this.ClientRectangle.Right - 22,
					this.ClientRectangle.Top + this.ClientRectangle.Height / 2 - 8,
					16,
					16
				);
			}
		}

		/*
		 * ImageBounds
		 */

		/// <summary>
		/// </summary>
		protected virtual Rectangle ImageBounds
		{
			get
			{
				return new Rectangle(
					this.ClientRectangle.Left + 8,
					this.ClientRectangle.Top + this.ClientRectangle.Height / 2 - 8,
					16,
					16
				);
			}
		}

		/*
		 * Initiator
		 */

		private INuGenEventInitiatorService _initiator = null;

		/// <summary>
		/// </summary>
		protected virtual INuGenEventInitiatorService Initiator
		{
			get
			{
				if (_initiator == null)
				{
					_initiator = new NuGenEventInitiatorService(this, this.Events);
				}

				return _initiator;
			}
		}

		/*
		 * ServiceProvider
		 */

		private INuGenServiceProvider _serviceProvider = null;

		/// <summary>
		/// </summary>
		protected virtual INuGenServiceProvider ServiceProvider
		{
			get
			{
				return _serviceProvider;
			}
		}

		#endregion

		#region Methods.Protected.Overriden

		/*
		 * OnMouseEnter
		 */

		/// <summary>
		/// Raises the <see cref="M:System.Windows.Forms.Control.OnMouseEnter(System.EventArgs)"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnMouseEnter(EventArgs e)
		{
			this.TabStateTracker.MouseEnter(this);
			base.OnMouseEnter(e);
		}

		/*
		 * OnMouseLeave
		 */

		/// <summary>
		/// Raises the <see cref="M:System.Windows.Forms.Control.OnMouseLeave(System.EventArgs)"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnMouseLeave(EventArgs e)
		{
			this.TabStateTracker.MouseLeave(this);
			base.OnMouseLeave(e);
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

			using (SolidBrush sb = new SolidBrush(this.BackColor))
			{
				g.FillRectangle(sb, this.ClientRectangle);
			}

			NuGenTabButtonPaintParams tabItemParams = new NuGenTabButtonPaintParams(g, this.ClientRectangle);
			
			tabItemParams.Font = this.Font;
			tabItemParams.Image = this.Image;
			tabItemParams.ImageBounds = this.ImageBounds;
			tabItemParams.IsRightToLeft = this.RightToLeft == RightToLeft.Yes;
			tabItemParams.Text = this.Text;

			float textHeight = g.MeasureString(this.Text, this.Font).Height;
			
			tabItemParams.TextBounds = new Rectangle(
				this.ClientRectangle.Left + this.ImageBounds.Right + 5,
				this.ClientRectangle.Top + this.ClientRectangle.Height / 2 - (int)(textHeight / 2.0f),
				this.ClientRectangle.Width - this.ImageBounds.Width - this.CloseButton.Width - 20,
				(int)textHeight
			);

			if (!this.Enabled)
			{
				tabItemParams.State = TabItemState.Disabled;
			}
			else
			{
				switch (this.TabStateTracker.GetControlState(this))
				{
					case NuGenControlState.Hot:
					{
						tabItemParams.State = TabItemState.Hot;
						break;
					}
					case NuGenControlState.Pressed:
					{
						tabItemParams.State = TabItemState.Selected;
						break;
					}
				}
			}

			Debug.Assert(this.TabRenderer != null, "this.TabRenderer != null");
			this.TabRenderer.DrawTabButton(tabItemParams);
		}

		/*
		 * OnSizeChanged
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.SizeChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnSizeChanged(EventArgs e)
		{
			base.OnSizeChanged(e);

			Debug.Assert(this.CloseButton != null, "this.CloseButton != null");
			this.CloseButton.Bounds = this.CloseButtonBounds;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTabButton"/> class.
		/// </summary>
		public NuGenTabButton()
			: this(new NuGenTabControlServiceProvider())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTabButton"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenButtonStateTracker"/><para/>
		/// <see cref="INuGenTabRenderer"/><para/>
		/// <see cref="INuGenTabStateTracker"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="serviceProvider"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public NuGenTabButton(INuGenServiceProvider serviceProvider)
			: base()
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			_serviceProvider = serviceProvider;

			this.SetStyle(ControlStyles.ResizeRedraw, true);

			this.CloseButton.Bounds = this.CloseButtonBounds;
			this.CloseButton.Parent = this;
			this.CloseButton.Click += delegate
			{
				this.OnClose(EventArgs.Empty);
			};
			this.CloseButton.MouseEnter += delegate
			{
				this.OnMouseEnter(EventArgs.Empty);
			};
		}

		#endregion
	}
}
