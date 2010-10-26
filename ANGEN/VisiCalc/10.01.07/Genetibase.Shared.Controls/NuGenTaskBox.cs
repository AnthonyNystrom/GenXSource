/* -----------------------------------------------
 * NuGenTaskBox.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;
using Genetibase.WinApi;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using Genetibase.Shared.Controls.TaskBoxInternals;
using System.Runtime.InteropServices;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	[ToolboxItem(false)]
	[DefaultEvent("CollapsedChanged")]
	[DefaultProperty("Text")]
	[Designer("Genetibase.Shared.Controls.Design.NuGenTaskBoxDesigner")]
	[NuGenSRDescription("Description_TaskBox")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenTaskBox : NuGenControl
	{
		#region Properties.Appearance

		/*
		 * Image
		 */

		/// <summary>
		/// Gets or sets the image this <see cref="NuGenTaskBox"/> displays.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(null)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_TaskBox_Image")]
		public Image Image
		{
			get
			{
				return this.ImageDescriptor.Image;
			}
			set
			{
				this.ImageDescriptor.Image = value;
			}
		}

		/*
		 * ImageIndex
		 */

		/// <summary>
		/// Gets or sets the index of the image in the ImageList to display on the task box.
		/// </summary>
		/// <exception cref="ArgumentException">
		/// <para>Index should be greater or equal to -1.</para>
		/// </exception>
		[DefaultValue(-1)]
		[Editor("Genetibase.Shared.Controls.Design.NuGenImageIndexEditor", typeof(UITypeEditor))]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_TaskBox_ImageIndex")]
		[RefreshProperties(RefreshProperties.Repaint)]
		[TypeConverter(typeof(ImageIndexConverter))]
		public int ImageIndex
		{
			get
			{
				return this.ImageDescriptor.ImageIndex;
			}
			set
			{
				this.ImageDescriptor.ImageIndex = value;
			}
		}

		/*
		 * ImageList
		 */

		/// <summary>
		/// Gets or sets the ImageList to get the image to display on the task box.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(null)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_TaskBox_ImageList")]
		public ImageList ImageList
		{
			get
			{
				return this.ImageDescriptor.ImageList;
			}
			set
			{
				this.ImageDescriptor.ImageList = value;
			}
		}

		#endregion

		#region Properties.Behavior

		/*
		 * Collapsed
		 */

		private bool _collapsed;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_TaskBox_Collapsed")]
		public bool Collapsed
		{
			get
			{
				return _collapsed;
			}
			set
			{
				if (_collapsed != value)
				{
					_collapsed = value;
					this.OnCollapsedChanged(EventArgs.Empty);
					this.AnimationStart();
				}
			}
		}

		private static readonly object _collapsedChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Collapsed"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_TaskBox_CollapsedChanged")]
		public event EventHandler CollapsedChanged
		{
			add
			{
				this.Events.AddHandler(_collapsedChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_collapsedChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="CollapsedChanged"/> event.
		/// </summary>
		protected virtual void OnCollapsedChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_collapsedChanged, e);
		}

		/*
		 * SmoothAnimation
		 */

		private bool _smoothAnimation;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_TaskBox_SmoothAnimation")]
		public bool SmoothAnimation
		{
			get
			{
				return _smoothAnimation;
			}
			set
			{
				if (_smoothAnimation != value)
				{
					_smoothAnimation = value;
					this.OnSmoothAnimationChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _smoothAnimationChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="SmoothAnimation"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_TaskBox_SmoothAnimationChanged")]
		public event EventHandler SmoothAnimationChanged
		{
			add
			{
				this.Events.AddHandler(_smoothAnimationChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_smoothAnimationChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="SmoothAnimationChanged"/> event.
		/// </summary>
		protected virtual void OnSmoothAnimationChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_smoothAnimationChanged, e);
		}

		#endregion

		#region Properties.Public

		/// <summary>
		/// Gets or sets the height of the control.
		/// </summary>
		/// <value></value>
		/// <returns>The height of the control in pixels.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		public new int Height
		{
			get
			{
				return base.Height;
			}
			set
			{
				if (_collapsed)
				{
					value = this.LayoutManager.GetHeaderHeight();
				}

				base.Height = value;
			}
		}

		/// <summary>
		/// Gets or sets the height and width of the control.
		/// </summary>
		/// <value></value>
		/// <returns>The <see cref="T:System.Drawing.Size"></see> that represents the height and width of the control in pixels.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		public new Size Size
		{
			get
			{
				return base.Size;
			}
			set
			{
				if (_collapsed)
				{
					value = new Size(value.Width, this.LayoutManager.GetHeaderHeight());
				}

				base.Size = value;
			}
		}

		#endregion

		#region Properties.Public.Overridden

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

		#endregion

		#region Properties.Protected.Overridden

		private static readonly Size _defaultSize = new Size(155, 100);

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
		 * HeaderStateTracker
		 */

		private INuGenButtonStateTracker _headerStateTracker;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenButtonStateTracker HeaderStateTracker
		{
			get
			{
				if (_headerStateTracker == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					INuGenButtonStateService stateService = this.ServiceProvider.GetService<INuGenButtonStateService>();

					if (stateService == null)
					{
						throw new NuGenServiceNotFoundException<INuGenButtonStateService>();
					}

					_headerStateTracker = stateService.CreateStateTracker();
					Debug.Assert(_headerStateTracker != null, "_headerStateTracker != null");
				}

				return _headerStateTracker;
			}
		}

		/*
		 * ImageDescriptor
		 */

		private INuGenControlImageDescriptor _imageDescriptor;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenControlImageDescriptor ImageDescriptor
		{
			get
			{
				if (_imageDescriptor == null)
				{
					INuGenControlImageManager imageManager = this.ServiceProvider.GetService<INuGenControlImageManager>();

					if (imageManager == null)
					{
						throw new NuGenServiceNotFoundException<INuGenControlImageManager>();
					}

					_imageDescriptor = imageManager.CreateImageDescriptor();
					_imageDescriptor.ImageChanged += _imageDescriptor_Updated;
					_imageDescriptor.ImageIndexChanged += _imageDescriptor_Updated;
					_imageDescriptor.ImageListChanged += _imageDescriptor_Updated;
				}

				return _imageDescriptor;
			}
		}

		/*
		 * LayoutManager
		 */

		private INuGenTaskBoxLayoutManager _layoutManager;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenTaskBoxLayoutManager LayoutManager
		{
			get
			{
				if (_layoutManager == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_layoutManager = this.ServiceProvider.GetService<INuGenTaskBoxLayoutManager>();

					if (_layoutManager == null)
					{
						throw new NuGenServiceNotFoundException<INuGenTaskBoxLayoutManager>();
					}
				}

				return _layoutManager;
			}
		}

		/*
		 * Renderer
		 */

		private INuGenTaskBoxRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenTaskBoxRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_renderer = this.ServiceProvider.GetService<INuGenTaskBoxRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenTaskBoxRenderer>();
					}
				}

				return _renderer;
			}
		}

		#endregion

		#region Methods.Public

		/// <summary>
		/// Invalidates the entire surface of the control and causes the control to be redrawn.
		/// </summary>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		public new void Invalidate()
		{
			User32.RedrawWindow(this.Handle, IntPtr.Zero, IntPtr.Zero, WinUser.RDW_FRAME | 0x85);
			this.Update();
		}

		#endregion

		#region Methods.Protected

		/// <summary>
		/// </summary>
		protected void InvalidateHeader()
		{
			int headerHeight = this.LayoutManager.GetHeaderHeight();
			RECT rect = new RECT(
				this.ClientRectangle.Left
				, -headerHeight
				, this.ClientRectangle.Right
				, headerHeight
			);
			User32.RedrawWindow(this.Handle, ref rect, IntPtr.Zero, WinUser.RDW_FRAME | 0x85);
		}

		#endregion

		#region Methods.Protected.Overridden

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.EnabledChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnEnabledChanged(EventArgs e)
		{
			base.OnEnabledChanged(e);
			this.HeaderStateTracker.Enabled(this.Enabled);
			this.Invalidate();
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			if (this.HitTestHeader(e.Location))
			{
				this.HeaderStateTracker.MouseDown();
				this.Collapsed = !this.Collapsed;
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (this.HitTestHeader(e.Location))
			{
				this.HeaderStateTracker.MouseEnter();
				this.SetHeaderCursor(true);
			}
			else
			{
				this.HeaderStateTracker.MouseLeave();
				this.SetHeaderCursor(false);
			}

			this.InvalidateHeader();
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			this.HeaderStateTracker.MouseLeave();
			this.SetHeaderCursor(false);
			this.InvalidateHeader();
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			this.HeaderStateTracker.MouseUp();
			this.SetHeaderCursor(false);
			this.InvalidateHeader();
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Rectangle clientRect = this.ClientRectangle;
			NuGenPaintParams paintParams = new NuGenPaintParams(e.Graphics);
			paintParams.Bounds = clientRect;
			paintParams.State = this.StateTracker.GetControlState();
			NuGenPaintParams borderPaintParams = new NuGenPaintParams(paintParams);
			borderPaintParams.Bounds = new Rectangle(
				clientRect.Left,
				clientRect.Top - 1,
				clientRect.Width - 1,
				clientRect.Height
			);
			this.Renderer.DrawBackground(paintParams);
			this.Renderer.DrawBorder(borderPaintParams);
			base.OnPaint(e);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.TextChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			this.InvalidateHeader();
		}

		/// <summary>
		/// </summary>
		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
				case WinUser.WM_NCCALCSIZE:
				{
					if (m.WParam == IntPtr.Zero)
					{
						RECT rect = (RECT)m.GetLParam(typeof(RECT));
						this.CalculateClientBounds(ref rect);
						Marshal.StructureToPtr(rect, m.LParam, true);
					}
					else
					{
						NCCALCSIZE_PARAMS calc = (NCCALCSIZE_PARAMS)m.GetLParam(typeof(NCCALCSIZE_PARAMS));
						this.CalculateClientBounds(ref calc.rectProposed);
						Marshal.StructureToPtr(calc, m.LParam, true);
					}

					m.Result = new IntPtr(WinUser.WVR_REDRAW);
					break;
				}
				case WinUser.WM_NCHITTEST:
				{
					/* Map all mouse messages to client area. */
					m.Result = (IntPtr)WinUser.HTCLIENT;
					return;
				}
				case WinUser.WM_NCPAINT:
				{
					base.DefWndProc(ref m);

					IntPtr hDC = User32.GetWindowDC(m.HWnd);

					if (hDC == IntPtr.Zero)
					{
						return;
					}

					RECT bounds = new RECT();
					User32.GetWindowRect(m.HWnd, ref bounds);

					if (bounds.Width < 1 || bounds.Height < 1)
					{
						return;
					}

					using (NuGenNativeGrfx grfx = new NuGenNativeGrfx(hDC, bounds.Size))
					{
						this.DrawHeader(grfx.Graphics, this.RectangleToClient(bounds));
						grfx.DrawToTargetGraphics();
					}

					User32.ReleaseDC(m.HWnd, hDC);
					m.Result = IntPtr.Zero;

					return;
				}
			}

			base.WndProc(ref m);
		}

		#endregion

		#region Methods.Private

		private bool _resizeFinished = true;
		private int _expandedHeight;
		private int _step;
		private int _preHeight;
		private int _endHeight;

		private void AnimationStart()
		{
			this.AnimationEnd();

			if (_resizeFinished && _collapsed)
			{
				_expandedHeight = base.Height;
				_resizeFinished = false;
			}

			if (this.SmoothAnimation && !this.DesignMode)
			{
				_preHeight = this.Height;

				if (_collapsed)
				{
					_endHeight = this.LayoutManager.GetHeaderHeight();
				}
				else
				{
					_endHeight = _expandedHeight;
				}

				_step = 0;
				_smoothAnimationTimer.Start();
			}
			else
			{
				if (_collapsed)
				{
					base.Height = this.LayoutManager.GetHeaderHeight();
				}
				else
				{
					base.Height = _expandedHeight;
				}

				_resizeFinished = true;
			}
		}

		private void AnimationEnd()
		{
			_smoothAnimationTimer.Stop();
		}

		private void CalculateClientBounds(ref RECT rect)
		{
			rect.top += this.LayoutManager.GetHeaderHeight();
		}

		private void DrawHeader(Graphics g, Rectangle bounds)
		{
			Debug.Assert(g != null, "g != null");

			int headerHeight = this.LayoutManager.GetHeaderHeight();
			int collapseButtonWidth = this.LayoutManager.GetCollapseButtonWidth();

			Rectangle headerBounds = new Rectangle(
				bounds.Left
				, bounds.Top + headerHeight
				, bounds.Width
				, headerHeight
			);

			Rectangle headerBodyBounds = headerBounds;
			headerBodyBounds.Width -= collapseButtonWidth;

			if (this.RightToLeft == RightToLeft.Yes)
			{
				headerBodyBounds.X += collapseButtonWidth;
			}

			NuGenControlState headerState = this.HeaderStateTracker.GetControlState();
			NuGenItemPaintParams headerPaintParams = new NuGenItemPaintParams(g);
			headerPaintParams.Bounds = NuGenControlPaint.BorderRectangle(headerBounds);
			headerPaintParams.ContentAlign =
				this.RightToLeft == RightToLeft.Yes
					? ContentAlignment.MiddleRight
					: ContentAlignment.MiddleLeft
					;
			headerPaintParams.Font = this.Font;
			headerPaintParams.ForeColor = this.ForeColor;
			headerPaintParams.Image = this.Image;
			headerPaintParams.State = headerState;
			headerPaintParams.Text = this.Text;

			this.Renderer.DrawBackground(headerPaintParams);
			this.Renderer.DrawBorder(headerPaintParams);

			headerPaintParams.Bounds = headerBodyBounds;
			this.Renderer.DrawHeader(headerPaintParams);

			Rectangle collapseButtonBounds = new Rectangle(headerBounds.Left, headerBounds.Top, collapseButtonWidth, headerBounds.Height);

			if (this.RightToLeft == RightToLeft.Yes)
			{
				if (!_collapsed)
				{
					NuGenControlPaint.Make180CCWGraphics(headerPaintParams.Graphics, collapseButtonBounds);
				}
			}
			else
			{
				if (!_collapsed)
				{
					headerPaintParams.Graphics.RotateTransform(180);
					headerPaintParams.Graphics.TranslateTransform(
						-(headerBounds.Width + headerBodyBounds.Width) + 1
						, -headerHeight + 1
					);
				}

				collapseButtonBounds.X += headerBodyBounds.Right;
			}

			headerPaintParams.Bounds = collapseButtonBounds;
			this.Renderer.DrawCollapseButton(headerPaintParams);
		}

		private bool HitTestHeader(Point location)
		{
			return location.Y < this.ClientRectangle.Top;
		}

		private bool _isHeaderHighlighted;

		private void SetHeaderCursor(bool isHeaderHighlighted)
		{
			if (_isHeaderHighlighted != isHeaderHighlighted)
			{
				_isHeaderHighlighted = isHeaderHighlighted;

				if (isHeaderHighlighted)
				{
					this.Cursor = Cursors.Hand;
				}
				else
				{
					this.Cursor = Cursors.Default;
				}
			}
		}

		#endregion

		#region EventHandlers.ImageDescriptor

		private void _imageDescriptor_Updated(object sender, EventArgs e)
		{
			if (this.IsHandleCreated)
			{
				this.Invalidate();
			}
		}

		#endregion

		#region EventHandlers.Timer

		private void _smoothAnimationTimer_Tick(object sender, EventArgs e)
		{
			base.Height = _preHeight
				+ (int)(Math.Sin(
					Math.PI * (double)(_step) / 40.0)
					* (double)(_endHeight - _preHeight)
				);

			if (++_step > 20)
			{
				this.AnimationEnd();
			}
		}

		#endregion

		private Timer _smoothAnimationTimer;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTaskBox"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		///		<para><see cref="INuGenButtonStateService"/></para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		///		<para><see cref="INuGenTaskBoxLayoutManager"/></para>	
		///		<para><see cref="INuGenTaskBoxRenderer"/></para>
		///		<para><see cref="INuGenControlImageManager"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenTaskBox(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.Selectable, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, false);
			this.SetStyle(ControlStyles.UserPaint, true);

			_smoothAnimationTimer = new Timer();
			_smoothAnimationTimer.Interval = 10;
			_smoothAnimationTimer.Tick += _smoothAnimationTimer_Tick;
		}

		/// <summary>
		/// </summary>
		/// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_imageDescriptor != null)
				{
					_imageDescriptor.ImageChanged -= _imageDescriptor_Updated;
					_imageDescriptor.ImageIndexChanged -= _imageDescriptor_Updated;
					_imageDescriptor.ImageListChanged -= _imageDescriptor_Updated;

					_imageDescriptor = null;
				}

				if (_smoothAnimationTimer != null)
				{
					_smoothAnimationTimer.Tick -= _smoothAnimationTimer_Tick;
					_smoothAnimationTimer.Dispose();
					_smoothAnimationTimer = null;
				}
			}

			base.Dispose(disposing);
		}
	}
}
