/* -----------------------------------------------
 * NuGenComboBox.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComboBoxInternals;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.Properties;
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
	/// <seealso cref="ComboBox"/>
	/// </summary>
	[ToolboxItem(false)]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenComboBox : ComboBox
	{
		#region Properties.Appearance

		/*
		 * ImageList
		 */

		private ImageList _imageList;

		/// <summary>
		/// Gets or sets the <see cref="ImageList"/> to get the images to display on the combo box items.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(null)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_ComboBox_ImageList")]
		public ImageList ImageList
		{
			get
			{
				return _imageList;
			}
			set
			{
				if (_imageList != value)
				{
					_imageList = value;
					this.OnImageListChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _imageListChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="ImageList"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ComboBox_ImageListChanged")]
		public event EventHandler ImageListChanged
		{
			add
			{
				this.Events.AddHandler(_imageListChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_imageListChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="ImageListChanged"/> event.
		/// </summary>
		protected virtual void OnImageListChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_imageListChanged, e);
		}

		#endregion

		#region Properties.Behavior

		/*
		 * AutoDropDownWidth
		 */

		private bool _autoDropDownWidth = true;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_ComboBox_AutoDropDownWidth")]
		public bool AutoDropDownWidth
		{
			get
			{
				return _autoDropDownWidth;
			}
			set
			{
				if (_autoDropDownWidth != value)
				{
					_autoDropDownWidth = value;
					this.OnAutoDropDownWidthChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _autoDropDownWidthChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="AutoDropDownWidth"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_ComboBox_AutoDropDownWidthChanged")]
		public event EventHandler AutoDropDownWidthChanged
		{
			add
			{
				this.Events.AddHandler(_autoDropDownWidthChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_autoDropDownWidthChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="AutoDropDownWidthChanged"/> event.
		/// </summary>
		protected virtual void OnAutoDropDownWidthChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_autoDropDownWidthChanged, e);
		}

		#endregion

		#region Properties.Hidden

		/*
		 * DrawMode
		 */

		/// <summary>
		/// Do not use this property. Any value will not affect the appearance.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new DrawMode DrawMode
		{
			get
			{
				return base.DrawMode;
			}
			set
			{
				base.DrawMode = DrawMode.OwnerDrawVariable;
			}
		}

		/*
		 * FlatStyle
		 */

		/// <summary>
		/// Do not use this property. Any value will not affect the appearance.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new FlatStyle FlatStyle
		{
			get
			{
				return base.FlatStyle;
			}
			set
			{
				base.FlatStyle = FlatStyle.Flat;
			}
		}

		#endregion

		#region Properties.Services

		/*
		 * ButtonStateTracker
		 */

		private INuGenButtonStateTracker _buttonStateTracker = null;

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
		 * ImageListService
		 */

		private INuGenImageListService _imageListService;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenImageListService ImageListService
		{
			get
			{
				if (_imageListService == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_imageListService = this.ServiceProvider.GetService<INuGenImageListService>();

					if (_imageListService == null)
					{
						throw new NuGenServiceNotFoundException<INuGenImageListService>();
					}
				}

				return _imageListService;
			}
		}

		/*
		 * Renderer
		 */

		private INuGenComboBoxRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenComboBoxRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_renderer = this.ServiceProvider.GetService<INuGenComboBoxRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenComboBoxRenderer>();
					}
				}

				return _renderer;
			}
		}

		/*
		 * ServiceProvider
		 */

		private INuGenServiceProvider _serviceProvider = null;

		/// <summary>
		/// </summary>
		protected INuGenServiceProvider ServiceProvider
		{
			get
			{
				return _serviceProvider;
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		/*
		 * OnDrawItem
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.ComboBox.DrawItem"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DrawItemEventArgs"></see> that contains the event data.</param>
		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			if (e.Index < 0)
			{
				return;
			}

			NuGenItemParams itemParams = new NuGenItemParams(this, e.Graphics, e.Bounds, e.State);

			itemParams.Font = this.Font;
			itemParams.ForeColor = e.ForeColor;

			if (this.ImageList != null)
			{
				itemParams.Image = this.ImageListService.FindImageAtIndex(this.ImageList, e.Index);
			}

			itemParams.Text = this.GetItemText(this.Items[e.Index]);
			
			this.Renderer.DrawItem(NuGenItemService.BuildItemPaintParams(itemParams));
		}

		/*
		 * OnDropDown
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.ComboBox.DropDown"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnDropDown(EventArgs e)
		{
			base.OnDropDown(e);

			using (Graphics g = Graphics.FromHwnd(this.Handle))
			{
				int width = 0;

				for (int i = 0; i < this.Items.Count; i++)
				{
					MeasureItemEventArgs eventArgs = new MeasureItemEventArgs(g, i, this.ItemHeight);
					this.OnMeasureItem(eventArgs);
					width = Math.Max(width, eventArgs.ItemWidth);
				}

				this.DropDownWidth = Math.Max(width, this.Width);
			}
		}

		/*
		 * OnEnabledChanged
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.EnabledChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnEnabledChanged(EventArgs e)
		{
			Debug.Assert(this.ButtonStateTracker != null, "this.ButtonStateTracker != null");
			this.ButtonStateTracker.Enabled(this.Enabled);
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
			Debug.Assert(this.ButtonStateTracker != null, "this.ButtonStateTracker != null");
			this.ButtonStateTracker.GotFocus();
			base.OnGotFocus(e);
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
			Debug.Assert(this.ButtonStateTracker != null, "this.ButtonStateTracker != null");
			this.ButtonStateTracker.LostFocus();
			base.OnLostFocus(e);
		}

		/*
		 * OnMeasureItem
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.ComboBox.MeasureItem"></see> event.
		/// </summary>
		/// <param name="e">The <see cref="T:System.Windows.Forms.MeasureItemEventArgs"></see> that was raised.</param>
		protected override void OnMeasureItem(MeasureItemEventArgs e)
		{
			Size itemSize = NuGenItemService.GetItemSize(
				e.Graphics,
				this.ImageList,
				this.GetItemText(this.Items[e.Index]),
				this.Font
			);

			e.ItemWidth = itemSize.Width;
			e.ItemHeight = itemSize.Height;
		}

		/*
		 * OnMouseDown
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			this.Invalidate();
		}

		/*
		 * OnMouseEnter
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseEnter"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnMouseEnter(EventArgs e)
		{
			Debug.Assert(this.ButtonStateTracker != null, "this.ButtonStateTracker != null");
			this.ButtonStateTracker.MouseEnter();
			base.OnMouseEnter(e);
			this.Invalidate();
		}

		/*
		 * OnMouseLeave
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnMouseLeave(EventArgs e)
		{
			Debug.Assert(this.ButtonStateTracker != null, "this.ButtonStateTracker != null");
			this.ButtonStateTracker.MouseLeave();
			base.OnMouseLeave(e);
			this.Invalidate();
		}

		/*
		 * OnMouseUp
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			this.Invalidate();
		}

		/*
		 * WndProc
		 */

		/// <summary>
		/// Processes Windows messages.
		/// </summary>
		/// <param name="m"></param>
		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
				case WinUser.WM_PAINT:
				case WinUser.WM_PRINT:
				{
					base.WndProc(ref m);

					if (!(this.DropDownStyle == ComboBoxStyle.Simple))
					{
						using (Graphics g = m.WParam != IntPtr.Zero ? Graphics.FromHdc(m.WParam) : Graphics.FromHwnd(this.Handle))
						{
							this.DrawComboBox(g, this.ClientRectangle);
						}
					}

					break;
				}
				default:
				{
					base.WndProc(ref m);
					break;
				}
			}
		}

		#endregion

		#region Methods.Private

		/*
		 * DrawComboBox
		 */

		/// <summary>
		/// </summary>
		/// <param name="g"></param>
		/// <param name="bounds"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		private void DrawComboBox(Graphics g, Rectangle bounds)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			Debug.Assert(this.ButtonStateTracker != null, "this.ButtonStateTracker != null");
			Debug.Assert(this.Renderer != null, "this.Renderer != null");

			NuGenControlState currentState = this.ButtonStateTracker.GetControlState();

			this.Renderer.DrawComboBoxButton(new NuGenPaintParams(
				this,
				g,
				NuGenControlPaint.DropDownButtonBounds(this.ClientRectangle, this.RightToLeft),
				currentState)
			);
			this.Renderer.DrawBorder(new NuGenPaintParams(this, g, this.ClientRectangle, currentState));
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenComboBox"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenComboBoxRenderer"/><para/>
		/// <see cref="INuGenButtonStateService"/><para/>
		/// <see cref="INuGenImageListService"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="serviceProvider"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public NuGenComboBox(INuGenServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			_serviceProvider = serviceProvider;

			this.DrawMode = DrawMode.OwnerDrawVariable;
		}

		#endregion
	}
}
