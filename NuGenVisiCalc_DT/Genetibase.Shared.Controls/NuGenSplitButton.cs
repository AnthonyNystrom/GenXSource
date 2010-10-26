/* -----------------------------------------------
 * NuGenSplitButton.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.SplitButtonInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	[ToolboxItem(false)]
	[DefaultEvent("DropDownClosed")]
	[DefaultProperty("Text")]
	[NuGenSRDescription("Description_SplitButton")]
	public class NuGenSplitButton : NuGenSplitButtonBase
	{
		/*
		 * DropDown
		 */

		private static readonly object _dropDown = new object();

		/// <summary>
		/// Occurs when the popup window is about to drop down.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		[NuGenSRDescription("Description_SplitButton_DropDown")]
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
		/// Will bubble the <see cref="Genetibase.Shared.Controls.NuGenSplitButton.DropDown"/> event.
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
		[NuGenSRDescription("Description_SplitButton_DropDownClosed")]
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
		/// Will bubble the <see cref="Genetibase.Shared.Controls.NuGenSplitButton.DropDownClosed"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnDropDownClosed(EventArgs e)
		{
			this.Initiator.InvokeEventHandler(_dropDownClosed, e);
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

		/// <summary>
		/// </summary>
		public void CloseDropDown()
		{
			Debug.Assert(_popupContainer != null, "_popupContainer != null");
			_popupContainer.ClosePopup();
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Shared.Controls.NuGenSplitButton.MouseDown"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
		[SecurityPermission(SecurityAction.Demand)]
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			this.OnDropDown(EventArgs.Empty);
		}

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

		private IContainer _components;
		private NuGenPopupContainer _popupContainer;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSplitButton"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenButtonStateTracker"/></para>
		/// 	<para><see cref="INuGenSplitButtonLayoutManager"/></para>
		///		<para><see cref="INuGenSplitButtonRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSplitButton(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			_components = new Container();

			_popupContainer = new NuGenPopupContainer(serviceProvider);
			_components.Add(_popupContainer);
			_popupContainer.PopupBorderStyleChanged += _popupContainer_PopupBorderStyleChanged;
			_popupContainer.PopupClosing += _popupContainer_PopupClosing;
			_popupContainer.PopupSizeChanged += _popupContainer_PopupSizeChanged;
			_popupContainer.HostControl = this;
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
