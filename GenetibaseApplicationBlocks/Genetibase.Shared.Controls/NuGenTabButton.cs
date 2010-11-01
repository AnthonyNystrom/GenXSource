/* -----------------------------------------------
 * NuGenTabButton.cs
 * Copyright © 2006-2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.TabControlInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	[ToolboxItem(false)]
	[System.ComponentModel.DesignerCategory("Code")]
	public sealed class NuGenTabButton : Button
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
		private void OnClose(EventArgs e)
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
		private void OnImageChanged(EventArgs e)
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
		private void OnSelectedChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_selectedChanged, e);

			if (this.Selected)
			{
				this.Font = new Font(this.Font, FontStyle.Bold);
				this.TabStateTracker.Select();
			}
			else
			{
				this.Font = new Font(this.Font, FontStyle.Regular);
				this.TabStateTracker.Deselect();
			}
		}

		/*
		 * ShowCloseButton
		 */

		private bool _showCloseButton;

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

		#region Properties.Public.Overridden

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

		#region Properties.Protected.Overridden

		/*
		 * DefaultSize
		 */

		private static readonly Size _defaultSize = new Size(250, 24);

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

		#region Properties.Private

		/*
		 * CloseButton
		 */

		private NuGenTabCloseButton _closeButton = null;

		/// <summary>
		/// Read-only.
		/// </summary>
		private NuGenTabCloseButton CloseButton
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

		#endregion

		#region Properties.Services

		/*
		 * Initiator
		 */

		private INuGenEventInitiatorService _initiator = null;

		/// <summary>
		/// </summary>
		private INuGenEventInitiatorService Initiator
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
		 * LayoutManager
		 */

		private INuGenTabLayoutManager _layoutManager;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		private INuGenTabLayoutManager LayoutManager
		{
			get
			{
				if (_layoutManager == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_layoutManager = this.ServiceProvider.GetService<INuGenTabLayoutManager>();

					if (_layoutManager == null)
					{
						throw new NuGenServiceNotFoundException<INuGenTabLayoutManager>();
					}
				}

				return _layoutManager;
			}
		}

		/*
		 * ServiceProvider
		 */

		private INuGenServiceProvider _serviceProvider = null;

		/// <summary>
		/// </summary>
		private INuGenServiceProvider ServiceProvider
		{
			get
			{
				return _serviceProvider;
			}
		}

		/*
		 * TabRenderer
		 */

		private INuGenTabRenderer _tabRenderer = null;

		/// <summary>
		/// </summary>
		private INuGenTabRenderer TabRenderer
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
		private INuGenTabStateTracker TabStateTracker
		{
			get
			{
				if (_tabStateTracker == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					INuGenTabStateService stateService = this.ServiceProvider.GetService<INuGenTabStateService>();

					if (stateService == null)
					{
						throw new NuGenServiceNotFoundException<INuGenTabStateService>();
					}

					_tabStateTracker = stateService.CreateStateTracker();
					Debug.Assert(_tabStateTracker != null, "_tabStateTracker != null");
				}

				return _tabStateTracker;
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		/*
		 * OnMouseEnter
		 */

		/// <summary>
		/// Raises the <see cref="M:System.Windows.Forms.Control.OnMouseEnter(System.EventArgs)"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnMouseEnter(EventArgs e)
		{
			this.TabStateTracker.MouseEnter();
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
			this.TabStateTracker.MouseLeave();
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

			Rectangle closeButtonBounds = this.CloseButton.Visible ? this.CloseButton.Bounds : Rectangle.Empty;
			Rectangle bounds = this.LayoutManager.GetContentRectangle(this.ClientRectangle, closeButtonBounds);
			Image image = this.Image;
			System.Drawing.ContentAlignment imageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			Rectangle imageBounds = this.LayoutManager.GetImageBounds(new NuGenImageBoundsParams(bounds, image, imageAlign));
			Rectangle textBounds = this.LayoutManager.GetTextBounds(new NuGenTextBoundsParams(bounds, imageBounds, imageAlign));

			NuGenTabButtonPaintParams tabItemParams = new NuGenTabButtonPaintParams(g, this.ClientRectangle);
			tabItemParams.Font = this.Font;
			tabItemParams.Image = this.Image;
			tabItemParams.ImageBounds = imageBounds;
			tabItemParams.IsRightToLeft = this.RightToLeft == RightToLeft.Yes;
			tabItemParams.Text = this.Text;

			if (this.Text != null)
			{
				tabItemParams.TextBounds = textBounds;
			}

			if (!this.Enabled)
			{
				tabItemParams.State = TabItemState.Disabled;
			}
			else
			{
				switch (this.TabStateTracker.GetControlState())
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
			this.CloseButton.Bounds = this.GetCloseButtonBounds();
		}

		#endregion

		#region Methods.Private

		/*
		 * GetCloseButtonBounds
		 */

		private Rectangle GetCloseButtonBounds()
		{
			return this.LayoutManager.GetCloseButtonBounds(this.ClientRectangle);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTabButton"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// <para>Requires:</para>
		/// <para><see cref="INuGenTabLayoutManager"/></para>
		/// <para><see cref="INuGenTabRenderer"/></para>
		/// <para><see cref="INuGenTabStateService"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="serviceProvider"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public NuGenTabButton(INuGenServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			_serviceProvider = serviceProvider;

			this.SetStyle(ControlStyles.ResizeRedraw, true);

			this.CloseButton.Bounds = this.GetCloseButtonBounds();
			this.CloseButton.Visible = false;
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
