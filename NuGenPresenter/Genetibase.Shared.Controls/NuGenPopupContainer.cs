/* -----------------------------------------------
 * NuGenPopupContainer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Environment;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.Properties;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Allows to show a custom pop up window on a specified <see cref="Control"/>.
	/// </summary>
	[ToolboxItem(false)]
	[DefaultEvent("PopupClosing")]
	[DefaultProperty("HostControl")]
	[NuGenSRDescription("Description_PopupContainer")]
	[System.ComponentModel.DesignerCategory("Code")]
	public partial class NuGenPopupContainer : Component
	{
		#region Events

		/*
		 * PopupClosing
		 */

		private static readonly object _popupClosing = new object();

		/// <summary>
		/// Occurs when the popup window is closing.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		[NuGenSRDescription("Description_PopupContainer_PopupClosing")]
		public event EventHandler PopupClosing
		{
			add
			{
				this.Events.AddHandler(_popupClosing, value);
			}
			remove
			{
				this.Events.RemoveHandler(_popupClosing, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="Genetibase.Shared.Controls.NuGenPopupContainer.PopupClosing"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnPopupClosing(EventArgs e)
		{
			this.Initiator.InvokeEventHandler(_popupClosing, e);
		}

		#endregion

		#region Properties.Appearance

		/*
		 * PopupBorderStyle
		 */

		private FormBorderStyle _popupBorderStyle;

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
				return _popupBorderStyle;
			}
			set
			{
				if (_popupBorderStyle != value)
				{
					_popupBorderStyle = value;
					this.OnPopupBorderStyleChanged(EventArgs.Empty);
				}
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

		/*
		 * PopupSize
		 */

		private Size _popupSize;

		/// <summary>
		/// Gets or sets the popup window size.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Layout")]
		[NuGenSRDescription("Description_PopupContainer_PopupSize")]
		public Size PopupSize
		{
			get
			{
				if (_popupSize == Size.Empty)
				{
					return _defaultPopupSize;
				}

				return _popupSize;
			}
			set
			{
				if (_popupSize != value)
				{
					_popupSize = value;
					this.OnPopupSizeChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly Size _defaultPopupSize = new Size(100, 100);

		private void ResetPopupSize()
		{
			this.PopupSize = _defaultPopupSize;
		}

		private bool ShouldSerializePopupSize()
		{
			return this.PopupSize != _defaultPopupSize;
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

		#region Properties.Behavior

		/*
		 * HostControl
		 */

		private Control _hostControl;

		/// <summary>
		/// Gets or sets the popup window owner.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(null)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_PopupContainer_HostControl")]
		public Control HostControl
		{
			get
			{
				return _hostControl;
			}
			set
			{
				if (_hostControl != value)
				{
					_hostControl = value;
					this.OnHostControlChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _hostControlChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="HostControl"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_PopupContainer_HostControlChanged")]
		public event EventHandler HostControlChanged
		{
			add
			{
				this.Events.AddHandler(_hostControlChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_hostControlChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="HostControlChanged"/> event.
		/// </summary>
		protected virtual void OnHostControlChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_hostControlChanged, e);
		}

		#endregion

		#region Properties.Services

		/*
		 * Initiator
		 */

		private INuGenEventInitiatorService _initiator;

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

		private INuGenServiceProvider _serviceProvider;

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

		#region Methods.Public

		/*
		 * ClosePopup
		 */

		/// <summary>
		/// Returns <see langword="true"/> if the previously opened window was closed; otherwise, <see langword="false"/>.
		/// </summary>
		/// <returns></returns>
		public bool ClosePopup()
		{
			if (_popupForm != null && !_popupForm.IsDisposed && !_popupForm.Disposing)
			{
				_popupForm.Close();
				return true;
			}

			return false;
		}

		/*
		 * ShowPopup
		 */

		private PopupForm _popupForm;

		/// <summary>
		/// Returns <see langword="true"/> if the previously closed popup window was opened; otherwise, <see langword="false"/>.
		/// </summary>
		/// <param name="popupControl">
		/// Specifies the control to pop up. Can be <see langword="null"/>. An empty panel is displayed then.
		/// Make sure that the specified <paramref name="popupControl"/> is not hosted by any control.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// <para>
		///		<see cref="HostControl"/> and <paramref name="popupControl"/> cannot be represented by the same instance.
		/// </para>
		/// </exception>
		[SecurityPermission(SecurityAction.LinkDemand)]
		public bool ShowPopup(Control popupControl)
		{
			if (_popupForm == null || _popupForm.IsDisposed || _popupForm.Disposing)
			{
				if (_hostControl != null)
				{
					if (object.ReferenceEquals(_hostControl, popupControl))
					{
						throw new InvalidOperationException(Resources.InvalidOperation_PopupSameControl);
					}

					FormMessageFilter msgFilter = new FormMessageFilter(_hostControl);
					msgFilter.HostClosed += delegate
					{
						this.ClosePopup();
					};

					_popupForm = new PopupForm(this.ServiceProvider);
					_popupForm.FormBorderStyle = this.PopupBorderStyle;
					_popupForm.FormClosing += delegate
					{
						this.PopupSize = _popupForm.Size;
						this.OnPopupClosing(EventArgs.Empty);
					};
					_popupForm.Disposed += delegate
					{
						msgFilter.ReleaseHandle();
						msgFilter = null;
					};

					Control parentControl = _hostControl.Parent;
					Rectangle hostBounds = new Rectangle(new Point(0, 0), this.PopupSize);

					if (parentControl != null)
					{
						Rectangle hostControlBounds = parentControl.RectangleToScreen(_hostControl.Bounds);
						hostBounds.X = hostControlBounds.Left;
						hostBounds.Y = hostControlBounds.Bottom;
					}

					if (popupControl != null)
					{
						popupControl.Parent = _popupForm;
						popupControl.Dock = DockStyle.Fill;
					}

					_popupForm.Bounds = hostBounds;
					_popupForm.Show();

					return true;
				}
			}

			return false;
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPopupContainer"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// <para>Requires:</para>
		/// <para><see cref="INuGenPanelRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenPopupContainer(INuGenServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			_serviceProvider = serviceProvider;
		}
	}
}
