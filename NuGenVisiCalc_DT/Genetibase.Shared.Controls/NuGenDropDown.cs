/* -----------------------------------------------
 * NuGenDropDown.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.DropDownInternals;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.Properties;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Represents a combo box like control which can display a custom drop down control.
	/// </summary>
	[ToolboxItem(false)]
	[DefaultEvent("DropDownClosed")]
	[DefaultProperty("PopupSize")]
	[Designer("Genetibase.Shared.Controls.Design.NuGenDropDownDesigner")]
	[NuGenSRDescription("Description_DropDown")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenDropDown : NuGenUIControl
	{
		#region Events

		/*
		 * DropDown
		 */

		private static readonly object _dropDown = new object();

		/// <summary>
		/// Occurs when the popup window is about to drop down.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		[NuGenSRDescription("Description_DropDown_DropDown")]
		public event EventHandler DropDown
		{
			add
			{
				this.Events.AddHandler(_dropDown, value);
			}
			remove
			{
				this.Events.RemoveHandler(_dropDown, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="Genetibase.Shared.Controls.NuGenDropDown.DropDown"/> event.
		/// </summary>
		/// <param name="e"></param>
		[SecurityPermission(SecurityAction.LinkDemand)]
		protected virtual void OnDropDown(EventArgs e)
		{
			if (this.PopupControl != null && this.Parent != null)
			{
				Debug.Assert(_popupContainer != null, "_popupContainer != null");
				_popupContainer.ShowPopup(this.PopupControl);
			}

			this.Initiator.InvokeEventHandler(_dropDown, e);
		}

		/*
		 * DropDownClosed
		 */

		private static readonly object _dropDownClosed = new object();

		/// <summary>
		/// Occurs when the popup window is closed.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		[NuGenSRDescription("Description_DropDown_DropDownClosed")]
		public event EventHandler DropDownClosed
		{
			add
			{
				this.Events.AddHandler(_dropDownClosed, value);
			}
			remove
			{
				this.Events.RemoveHandler(_dropDownClosed, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="Genetibase.Shared.Controls.NuGenDropDown.DropDownClosed"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnDropDownClosed(EventArgs e)
		{
			this.Initiator.InvokeEventHandler(_dropDownClosed, e);
		}

		#endregion

		#region Properties.Appearance

		/*
		 * Image
		 */

		/// <summary>
		/// Gets or sets the image this <see cref="NuGenDropDown"/> displays.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(null)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_DropDown_Image")]
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
		/// Gets or sets the index of the image in the ImageList to display on the drop down body.
		/// </summary>
		/// <exception cref="ArgumentException">
		/// <para>Index should be greater or equal to -1.</para>
		/// </exception>
		[DefaultValue(-1)]
		[Editor("Genetibase.Shared.Controls.Design.NuGenImageIndexEditor", typeof(UITypeEditor))]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_DropDown_ImageIndex")]
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
		/// Gets or sets the ImageList to get the image to display on the link label.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(null)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_DropDown_ImageList")]
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

		/*
		 * PopupBorderStyle
		 */

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(FormBorderStyle.None)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_PopupContainer_PopupBorderStyle")]
		public FormBorderStyle PopupBorderStyle
		{
			get
			{
				return _popupContainer.PopupBorderStyle;
			}
			set
			{
				_popupContainer.PopupBorderStyle = value;
			}
		}

		private static readonly object _popupBorderStyleChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="PopupBorderStyle"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_PopupContainer_PopupBorderStyleChanged")]
		public event EventHandler PopupBorderStyleChanged
		{
			add
			{
				this.Events.AddHandler(_popupBorderStyleChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_popupBorderStyleChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="PopupBorderStyleChanged"/> event.
		/// </summary>
		protected virtual void OnPopupBorderStyleChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_popupBorderStyleChanged, e);
		}

		#endregion

		#region Properties.Layout

		/*
		 * PopupSize
		 */

		/// <summary>
		/// Gets or sets the size of the popup window.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Layout")]
		[NuGenSRDescription("Description_PopupContainer_PopupSize")]
		public Size PopupSize
		{
			get
			{
				Debug.Assert(_popupContainer != null, "_popupContainer != null");
				return _popupContainer.PopupSize;
			}
			set
			{
				Debug.Assert(_popupContainer != null, "_popupContainer != null");
				_popupContainer.PopupSize = value;
			}
		}

		private static readonly object _popupSizeChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="PopupSize"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_PopupContainer_PopupSizeChanged")]
		public event EventHandler PopupSizeChanged
		{
			add
			{
				this.Events.AddHandler(_popupSizeChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_popupSizeChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="PopupSizeChanged"/> event.
		/// </summary>
		protected virtual void OnPopupSizeChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_popupSizeChanged, e);
		}

		#endregion

		#region Properties.Public

		/*
		 * PopupControl
		 */

		private Control _popupControl;

		/// <summary>
		/// Gets or sets the <see cref="Control"/> to popup when the drop-down button clicked.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Control PopupControl
		{
			get
			{
				return _popupControl;
			}
			set
			{
				_popupControl = value;
			}
		}

		/*
		 * PreferredSize
		 */

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Size PreferredSize
		{
			get
			{
				return new Size(this.PreferredWidth, this.PreferredHeight);
			}
		}

		#endregion

		#region Properties.Hidden

		/*
		 * BorderStyle
		 */

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new BorderStyle BorderStyle
		{
			get
			{
				return base.BorderStyle;
			}
			set
			{
				base.BorderStyle = value;
			}
		}

		#endregion

		#region Properties.Public.Overridden

		/*
		 * AutoSize
		 */

		/// <summary>
		/// Gets or sets a value indicating whether [auto size].
		/// </summary>
		/// <value><c>true</c> if [auto size]; otherwise, <c>false</c>.</value>
		[DefaultValue(true)]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
				this.AdjustSize();
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
			}
		}

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
				this.Invalidate();
			}
		}

		#endregion

		#region Properties.Protected.Overridden

		/*
		 * DefaultSize
		 */

		private static readonly Size _defaultSize = new Size(155, 21);

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
		 * PreferredHeight
		 */

		/// <summary>
		/// </summary>
		protected virtual int PreferredHeight
		{
			get
			{
				if (this.Text != null)
				{
					using (Graphics g = Graphics.FromHwnd(this.Handle))
					{
						return Math.Max(
							this.DefaultSize.Height,
							g.MeasureString(this.Text, this.Font).ToSize().Height
						);
					}
				}

				return base.Height;
			}
		}

		/*
		 * PreferredWidth
		 */

		/// <summary>
		/// </summary>
		protected virtual int PreferredWidth
		{
			get
			{
				return _requestedWidth;
			}
		}

		#endregion

		#region Properties.Services

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
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
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
		 * Renderer
		 */

		private INuGenDropDownRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenDropDownRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_renderer = this.ServiceProvider.GetService<INuGenDropDownRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenDropDownRenderer>();
					}
				}

				return _renderer;
			}
		}

		#endregion

		#region Methods.Public

		/*
		 * CloseDropDown
		 */

		/// <summary>
		/// </summary>
		public void CloseDropDown()
		{
			Debug.Assert(_popupContainer != null, "_popupContainer != null");
			_popupContainer.ClosePopup();
		}

		#endregion

		#region Methods.Protected

		/*
		 * AdjustSize
		 */

		/// <summary>
		/// </summary>
		protected void AdjustSize()
		{
			if (this.AutoSize)
			{
				int height = _requestedHeight;
				int width = _requestedWidth;

				try
				{
					Size size = this.AutoSize ? this.PreferredSize : new Size(width, height);
					base.Size = size;
				}
				finally
				{
					_requestedHeight = height;
					_requestedWidth = width;
				}
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.FontChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.AdjustSize();
			this.Invalidate();
		}

		/// <summary>
		/// Raises the mouse down event.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
		[SecurityPermission(SecurityAction.Demand)]
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (this.GetDropDownButtonBounds().Contains(e.Location))
			{
				this.ButtonStateTracker.MouseDown();
			}

			this.Invalidate();
			this.OnDropDown(EventArgs.Empty);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (this.GetDropDownButtonBounds().Contains(e.Location))
			{
				this.ButtonStateTracker.MouseUp();
			}

			this.Invalidate();
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			Rectangle bounds = this.ClientRectangle;
			Rectangle dropDownBounds = this.GetDropDownButtonBounds();
			Rectangle bodyBounds = this.GetBodyBounds(dropDownBounds);
			NuGenControlState bodyState = this.StateTracker.GetControlState();
			NuGenControlState buttonState = this.ButtonStateTracker.GetControlState();

			NuGenItemPaintParams paintParams = new NuGenItemPaintParams(g);
			paintParams.Bounds = bodyBounds;
			paintParams.ContentAlign = this.RightToLeft == RightToLeft.No
					? ContentAlignment.MiddleLeft
					: ContentAlignment.MiddleRight
					;
			paintParams.Font = this.Font;
			paintParams.ForeColor = this.ForeColor;
			paintParams.Image = this.Image;
			paintParams.Text = this.Text;
			paintParams.State = bodyState;

			this.Renderer.DrawDropDownBody(paintParams);

			paintParams.Bounds = dropDownBounds;
			paintParams.State = buttonState;
			this.Renderer.DrawDropDownButton(paintParams);

			paintParams.Bounds = bounds;
			this.Renderer.DrawBorder(paintParams);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.TextChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			this.AdjustSize();
			this.Invalidate();
		}

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
			if ((specified & BoundsSpecified.Height) != BoundsSpecified.None)
			{
				_requestedHeight = height;
			}

			if ((specified & BoundsSpecified.Width) != BoundsSpecified.None)
			{
				_requestedWidth = width;
			}

			if (this.AutoSize)
			{
				height = this.PreferredHeight;
				width = this.PreferredWidth;
			}

			base.SetBoundsCore(x, y, width, height, specified);
		}

		#endregion

		#region Methods.Private

		private Rectangle GetBodyBounds(Rectangle dropDownButtonBounds)
		{
			if (this.RightToLeft == RightToLeft.No)
			{
				return new Rectangle(
					this.ClientRectangle.Left,
					this.ClientRectangle.Top,
					this.ClientRectangle.Width - (this.ClientRectangle.Right - dropDownButtonBounds.Left) + 1,
					this.ClientRectangle.Height
				);
			}

			return new Rectangle(
				dropDownButtonBounds.Right - 1,
				this.ClientRectangle.Top,
				this.ClientRectangle.Width - dropDownButtonBounds.Right + 1,
				this.ClientRectangle.Height
			);
		}

		private Rectangle GetDropDownButtonBounds()
		{
			return NuGenControlPaint.DropDownButtonBounds(this.ClientRectangle, this.RightToLeft);
		}

		#endregion

		#region EventHandlers.ImageDescriptor

		private void _imageDescriptor_Updated(object sender, EventArgs e)
		{
			if (this.IsHandleCreated)
			{
				this.AdjustSize();
				this.Invalidate();
			}
		}

		#endregion

		#region EventHandlers.PopupContainer

		private void _popupContainer_PopupBorderStyleChanged(object sender, EventArgs e)
		{
			this.OnPopupBorderStyleChanged(e);
		}

		private void _popupContainer_PopupClosing(object sender, EventArgs e)
		{
			this.OnDropDownClosed(e);
		}

		private void _popupContainer_PopupSizeChanged(object sender, EventArgs e)
		{
			this.OnPopupSizeChanged(e);
		}

		#endregion

		private int _requestedWidth;
		private int _requestedHeight;

		private IContainer _components;
		private NuGenPopupContainer _popupContainer;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDropDown"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenButtonStateTracker"/></para>
		/// 	<para><see cref="INuGenControlStateTracker"/></para>
		///		<para><see cref="INuGenControlImageManager"/></para>
		///		<para><see cref="INuGenDropDownRenderer"/></para>
		///		<para><see cref="INuGenPanelRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenDropDown(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			_components = new Container();

			_popupContainer = new NuGenPopupContainer(serviceProvider);
			_components.Add(_popupContainer);
			_popupContainer.PopupBorderStyleChanged += _popupContainer_PopupBorderStyleChanged;
			_popupContainer.PopupClosing += _popupContainer_PopupClosing;
			_popupContainer.PopupSizeChanged += _popupContainer_PopupSizeChanged;
			_popupContainer.HostControl = this;

			this.AutoSize = true;
			this.BackColor = SystemColors.Window;
			this.Width = this.DefaultSize.Width;

			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.UserPaint, true);
		}

		/// <summary>
		/// </summary>
		/// <param name="disposing"></param>
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
