/* -----------------------------------------------
 * NuGenTabControl.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Collections;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.Design;
using Genetibase.Shared.Controls.Properties;
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
	[DefaultEvent("TabIndexChanged")]
	[DefaultProperty("Dock")]
	[Designer(typeof(NuGenTabControlDesigner))]
	[ToolboxItem(true)]
	[System.ComponentModel.DesignerCategory("Code")]
	public partial class NuGenTabControl : UserControl
	{
		#region Declarations.Fields

		private Dictionary<NuGenTabButton, NuGenTabPage> _buttonPageDictionary;
		private Dictionary<NuGenTabPage, NuGenTabButton> _pageButtonDictionary;
		private List<NuGenTabButton> _tabButtons;

		#endregion

		#region Events

		/*
		 * TabButtonClick
		 */

		private static readonly object _tabButtonClick = new object();

		/// <summary>
		/// <seealso cref="E:System.Windows.Forms.Button.Click"/>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		public event EventHandler TabButtonClick
		{
			add
			{
				this.Events.AddHandler(_tabButtonClick, value);
			}
			remove
			{
				this.Events.RemoveHandler(_tabButtonClick, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="TabButtonClick"/> event.
		/// </summary>
		protected virtual void OnTabButtonClick(object sender, EventArgs e)
		{
			EventHandler handler = this.Events[_tabButtonClick] as EventHandler;

			if (handler != null)
			{
				handler(sender, e);
			}
		}

		/*
		 * TabButtonDoubleClick
		 */

		private static readonly object _tabButtonDoubleClick = new object();

		/// <summary>
		/// <seealso cref="Button.DoubleClick"/>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		public event EventHandler TabButtonDoubleClick
		{
			add
			{
				this.Events.AddHandler(_tabButtonDoubleClick, value);
			}
			remove
			{
				this.Events.RemoveHandler(_tabButtonDoubleClick, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="TabButtonDoubleClick"/> event.
		/// </summary>
		protected virtual void OnTabButtonDoubleClick(object sender, EventArgs e)
		{
			EventHandler handler = this.Events[_tabButtonDoubleClick] as EventHandler;

			if (handler != null)
			{
				handler(sender, e);
			}
		}

		/*
		 * TabButtonDragDrop
		 */

		private static readonly object _tabButtonDragDrop = new object();

		/// <summary>
		/// <seealso cref="E:System.Windows.Forms.Button.DragDrop"/>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		public event DragEventHandler TabButtonDragDrop
		{
			add
			{
				this.Events.AddHandler(_tabButtonDragDrop, value);
			}
			remove
			{
				this.Events.RemoveHandler(_tabButtonDragDrop, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="TabButtonDragDrop"/> event.
		/// </summary>
		protected virtual void OnTabButtonDragDrop(object sender, DragEventArgs e)
		{
			DragEventHandler handler = this.Events[_tabButtonDragDrop] as DragEventHandler;

			if (handler != null)
			{
				handler(sender, e);
			}
		}

		/*
		 * TabButtonDragEnter
		 */

		private static readonly object _tabButtonDragEnter = new object();

		/// <summary>
		/// <seealso cref="E:System.Windows.Forms.Button.DragEnter"/>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		public event DragEventHandler TabButtonDragEnter
		{
			add
			{
				this.Events.AddHandler(_tabButtonDragEnter, value);
			}
			remove
			{
				this.Events.RemoveHandler(_tabButtonDragEnter, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="TabButtonDragEnter"/> event.
		/// </summary>
		protected virtual void OnTabButtonDragEnter(object sender, DragEventArgs e)
		{
			DragEventHandler handler = this.Events[_tabButtonDragEnter] as DragEventHandler;

			if (handler != null)
			{
				handler(sender, e);
			}
		}

		/*
		 * TabButtonDragLeave
		 */

		private static readonly object _tabButtonDragLeave = new object();

		/// <summary>
		/// <seealso cref="E:System.Windows.Forms.Button.DragLeave"/>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		public event EventHandler TabButtonDragLeave
		{
			add
			{
				this.Events.AddHandler(_tabButtonDragLeave, value);
			}
			remove
			{
				this.Events.RemoveHandler(_tabButtonDragLeave, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="TabButtonDragLeave"/> event.
		/// </summary>
		protected virtual void OnTabButtonDragLeave(object sender, EventArgs e)
		{
			EventHandler handler = this.Events[_tabButtonDragLeave] as EventHandler;

			if (handler != null)
			{
				handler(sender, e);
			}
		}

		/*
		 * TabButtonMouseDown
		 */

		private static readonly object _tabButtonMouseDown = new object();

		/// <summary>
		/// <seealso cref="E:System.Windows.Forms.Button.MouseDown"/>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		public event MouseEventHandler TabButtonMouseDown
		{
			add
			{
				this.Events.AddHandler(_tabButtonMouseDown, value);
			}
			remove
			{
				this.Events.RemoveHandler(_tabButtonMouseDown, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="TabButtonMouseDown"/> event.
		/// </summary>
		protected virtual void OnTabButtonMouseDown(object sender, MouseEventArgs e)
		{
			MouseEventHandler handler = this.Events[_tabButtonMouseDown] as MouseEventHandler;

			if (handler != null)
			{
				handler(sender, e);
			}
		}

		/*
		 * TabButtonMouseEnter
		 */

		private static readonly object _tabButtonMouseEnter = new object();

		/// <summary>
		/// <seealso cref="E:System.Windows.Forms.Button.MouseEnter"/>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		public event EventHandler TabButtonMouseEnter
		{
			add
			{
				this.Events.AddHandler(_tabButtonMouseEnter, value);
			}
			remove
			{
				this.Events.RemoveHandler(_tabButtonMouseEnter, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="TabButtonMouseEnter"/> event.
		/// </summary>
		protected virtual void OnTabButtonMouseEnter(object sender, EventArgs e)
		{
			EventHandler handler = this.Events[_tabButtonMouseEnter] as EventHandler;

			if (handler != null)
			{
				handler(sender, e);
			}
		}

		/*
		 * TabButtonMouseHover
		 */

		private static readonly object _tabButtonMouseHover = new object();

		/// <summary>
		/// <seealso cref="E:System.Windows.Forms.Button.MouseHover"/>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		public event EventHandler TabButtonMouseHover
		{
			add
			{
				this.Events.AddHandler(_tabButtonMouseHover, value);
			}
			remove
			{
				this.Events.RemoveHandler(_tabButtonMouseHover, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="TabButtonMouseHover"/> event.
		/// </summary>
		protected virtual void OnTabButtonMouseHover(object sender, EventArgs e)
		{
			EventHandler handler = this.Events[_tabButtonMouseHover] as EventHandler;

			if (handler != null)
			{
				handler(sender, e);
			}
		}

		/*
		 * TabButtonMouseLeave
		 */

		private static readonly object _tabButtonMouseLeave = new object();

		/// <summary>
		/// <seealso cref="E:System.Windows.Forms.Button.MouseLeave"/>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		public event EventHandler TabButtonMouseLeave
		{
			add
			{
				this.Events.AddHandler(_tabButtonMouseLeave, value);
			}
			remove
			{
				this.Events.RemoveHandler(_tabButtonMouseLeave, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="TabButtonMouseLeave"/> event.
		/// </summary>
		protected virtual void OnTabButtonMouseLeave(object sender, EventArgs e)
		{
			EventHandler handler = this.Events[_tabButtonMouseLeave] as EventHandler;

			if (handler != null)
			{
				handler(sender, e);
			}
		}

		/*
		 * TabButtonMouseMove
		 */

		private static readonly object _tabButtonMouseMove = new object();

		/// <summary>
		/// <seealso cref="E:System.Windows.Forms.Button.MouseMove"/>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		public event MouseEventHandler TabButtonMouseMove
		{
			add
			{
				this.Events.AddHandler(_tabButtonMouseMove, value);
			}
			remove
			{
				this.Events.RemoveHandler(_tabButtonMouseMove, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="TabButtonMouseMove"/> event.
		/// </summary>
		protected virtual void OnTabButtonMouseMove(object sender, MouseEventArgs e)
		{
			MouseEventHandler handler = this.Events[_tabButtonMouseMove] as MouseEventHandler;

			if (handler != null)
			{
				handler(sender, e);
			}
		}

		/*
		 * TabButtonMouseUp
		 */

		private static readonly object _tabButtonMouseUp = new object();

		/// <summary>
		/// <seealso cref="E:System.Windows.Forms.Button.MouseUp"/>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		public event MouseEventHandler TabButtonMouseUp
		{
			add
			{
				this.Events.AddHandler(_tabButtonMouseUp, value);
			}
			remove
			{
				this.Events.RemoveHandler(_tabButtonMouseUp, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="TabButtonMouseUp"/> event.
		/// </summary>
		protected virtual void OnTabButtonMouseUp(object sender, MouseEventArgs e)
		{
			MouseEventHandler handler = this.Events[_tabButtonMouseUp] as MouseEventHandler;

			if (handler != null)
			{
				handler(sender, e);
			}
		}

		/*
		 * TabCloseButtonClick
		 */

		private static readonly object _tabCloseButtonClick = new object();

		/// <summary>
		/// </summary>
		public event NuGenTabCancelEventHandler TabCloseButtonClick
		{
			add
			{
				this.Events.AddHandler(_tabCloseButtonClick, value);
			}
			remove
			{
				this.Events.RemoveHandler(_tabCloseButtonClick, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="TabCloseButtonClick"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnTabCloseButtonClick(NuGenTabCancelEventArgs e)
		{
			NuGenTabCancelEventHandler handler = this.Events[_tabCloseButtonClick] as NuGenTabCancelEventHandler;

			if (handler != null)
			{
				handler(this, e);
			}
		}

		/*
		 * TabPageAdded
		 */

		private static readonly object _tabPageAdded = new object();

		/// <summary>
		/// Occurs when a new tab page is added.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		[NuGenSRDescription("Description_TabControl_TabPageAdded")]
		public event EventHandler<NuGenCollectionEventArgs<NuGenTabPage>> TabPageAdded
		{
			add
			{
				this.Events.AddHandler(_tabPageAdded, value);
			}
			remove
			{
				this.Events.RemoveHandler(_tabPageAdded, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="TabPageAdded"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnTabPageAdded(NuGenCollectionEventArgs<NuGenTabPage> e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokeActionT<NuGenCollectionEventArgs<NuGenTabPage>>(_tabPageAdded, e);
		}

		/*
		 * TabPageRemoved
		 */

		private static readonly object _tabPageRemoved = new object();

		/// <summary>
		/// Occurs when a tab page is removed.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		[NuGenSRDescription("Description_TabControl_TabPageRemoved")]
		public event EventHandler<NuGenCollectionEventArgs<NuGenTabPage>> TabPageRemoved
		{
			add
			{
				this.Events.AddHandler(_tabPageRemoved, value);
			}
			remove
			{
				this.Events.RemoveHandler(_tabPageRemoved, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="TabPageRemoved"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnTabPageRemoved(NuGenCollectionEventArgs<NuGenTabPage> e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokeActionT<NuGenCollectionEventArgs<NuGenTabPage>>(_tabPageRemoved, e);
		}

		#endregion

		#region Properties.Appearance

		/*
		 * CloseButtonOnTab
		 */

		private bool _closeButtonOnTab = true;

		/// <summary>
		/// Gets or sets the value indicating whether to show a close button for every tab.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_TabControl_CloseButtonOnTab")]
		public bool CloseButtonOnTab
		{
			get
			{
				return _closeButtonOnTab;
			}
			set
			{
				if (_closeButtonOnTab != value)
				{
					_closeButtonOnTab = value;

					foreach (NuGenTabButton tabButton in _tabButtons)
					{
						tabButton.ShowCloseButton = _closeButtonOnTab;
					}
				}
			}
		}

		/*
		 * FlatStyle
		 */

		private FlatStyle _flatStyle = FlatStyle.System;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(FlatStyle.System)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_TabControl_FlatStyle")]
		public FlatStyle FlatStyle
		{
			get
			{
				return _flatStyle;
			}
			set
			{
				if (_flatStyle != value)
				{
					_flatStyle = value;
					this.OnFlatStyleChanged(EventArgs.Empty);
					this.Invalidate(this.TabPageBounds);
				}
			}
		}

		private static readonly object _flatStyleChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="FlatStyle"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_TabControl_FlatStyleChanged")]
		public event EventHandler FlatStyleChanged
		{
			add
			{
				this.Events.AddHandler(_flatStyleChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_flatStyleChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="FlatStyleChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnFlatStyleChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_flatStyleChanged, e);
		}

		#endregion

		#region Properties.Hidden

		/*
		 * BackgroundImage
		 */

		/// <summary>
		/// Do not use this property. Any value will not affect the appearance.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}

		/*
		 * BackgroundImageLayout
		 */

		/// <summary>
		/// Do not use this property. Any value will not affect the appearance.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override ImageLayout BackgroundImageLayout
		{
			get
			{
				return base.BackgroundImageLayout;
			}
			set
			{
				base.BackgroundImageLayout = value;
			}
		}

		/*
		 * Controls
		 */

		/// <summary>
		/// Gets the collection of controls contained within the control.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Windows.Forms.Control.ControlCollection"></see> representing the collection of controls contained within the control.</returns>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ControlCollection Controls
		{
			get
			{
				return base.Controls;
			}
		}

		#endregion

		#region Properties.NonBrowsable

		/*
		 * Padding
		 */

		/// <summary>
		/// Gets or sets padding within the control.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding"></see> representing the control's internal spacing characteristics.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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

		/*
		 * SelectedIndex
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="value"/> must be within the bounds of the <see cref="TabPages"/> collection.
		/// </exception>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int SelectedIndex
		{
			get
			{
				if (this.SelectedTabButton != null)
				{
					Debug.Assert(_buttonPageDictionary != null, "_buttonPageDictionary != null");
					return this.TabPages.IndexOf(_buttonPageDictionary[this.SelectedTabButton]);
				}

				return -1;
			}
			set
			{
				if (
					this.SelectedTabButton == null
					|| this.SelectedIndex != value
					)
				{
					NuGenTabButton tabButtonToSelect = null;

					try
					{
						tabButtonToSelect = _tabButtons[value];
					}
					catch (ArgumentOutOfRangeException e)
					{
						throw e;
					}

					Debug.Assert(_buttonPageDictionary != null, "_buttonPageDictionary != null");
					this.SelectTab(_buttonPageDictionary[tabButtonToSelect]);
				}
			}
		}

		private static readonly object _selectedIndexChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="SelectedIndex"/> property changes.
		/// </summary>
		public event EventHandler SelectedIndexChanged
		{
			add
			{
				this.Events.AddHandler(_selectedIndexChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_selectedIndexChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="SelectedIndexChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnSelectedIndexChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokeAction(_selectedIndexChanged, e);
		}

		/*
		 * SelectedTab
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="value"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public NuGenTabPage SelectedTab
		{
			get
			{
				Debug.Assert(_buttonPageDictionary != null, "_buttonPageDictionary != null");

				if (
					this.SelectedTabButton != null
					&& _buttonPageDictionary.ContainsKey(this.SelectedTabButton)
					)
				{
					return _buttonPageDictionary[this.SelectedTabButton];
				}

				return null;
			}
			set
			{
				if (value != null)
				{
					Debug.Assert(_pageButtonDictionary != null, "_pageButtonDictionary != null");

					if (!_pageButtonDictionary.ContainsKey(value))
					{
						throw new ArgumentException(
							string.Format(
								Resources.Argument_InvalidTabPage,
								typeof(NuGenTabPage).Name,
								typeof(NuGenTabControl).Name)
						);
					}

					this.SelectedTabButton = _pageButtonDictionary[value];
				}
				else
				{
					this.SelectedTabButton = null;
				}
			}
		}

		/*
		 * TabCount
		 */

		/// <summary>
		/// Read-only.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int TabCount
		{
			get
			{
				Debug.Assert(this.TabPages != null, "this.TabPages != null");
				return this.TabPages.Count;
			}
		}

		/*
		 * TabPages
		 */

		private TabPageCollection _tabPages = null;

		/// <summary>
		/// Read-only.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public TabPageCollection TabPages
		{
			get
			{
				if (_tabPages == null)
				{
					_tabPages = new TabPageCollection(this);
				}

				return _tabPages;
			}
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
		[DefaultValue(typeof(Color), "Transparent")]
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

		#endregion

		#region Properties.Protected

		/*
		 * LayoutBuilder
		 */

		private NuGenTabLayoutBuilder _layoutBuilder = null;

		/// <summary>
		/// <seealso cref="NuGenTabLayoutBuilder"/>
		/// </summary>
		protected NuGenTabLayoutBuilder LayoutBuilder
		{
			get
			{
				if (_layoutBuilder == null)
				{
					Debug.Assert(this.LayoutManager != null, "this.LayoutManager != null");
					_layoutBuilder = this.LayoutManager.RegisterLayoutBuilder(_tabButtons);
					Debug.Assert(_layoutBuilder != null, "_layoutBuilder != null");
				}

				return _layoutBuilder;
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
		 * TabPageBounds
		 */

		/// <summary>
		/// </summary>
		protected Rectangle TabPageBounds
		{
			get
			{
				Debug.Assert(this.LayoutManager != null, "this.LayoutManager != null");
				return this.LayoutManager.GetTabPageBounds(this.ClientRectangle, this.TabStripBounds);
			}
		}

		/*
		 * TabStripBounds
		 */

		/// <summary>
		/// </summary>
		protected Rectangle TabStripBounds
		{
			get
			{
				Debug.Assert(this.LayoutManager != null, "this.LayoutManager != null");
				return this.LayoutManager.GetTabStripBounds(this.ClientRectangle);
			}
		}

		#endregion

		#region Properties.Private

		/*
		 * SelectedTabButton
		 */

		private NuGenTabButton _selectedTabButton;

		/// <summary>
		/// </summary>
		private NuGenTabButton SelectedTabButton
		{
			get
			{
				return _selectedTabButton;
			}
			set
			{
				if (_selectedTabButton != value)
				{
					if (_selectedTabButton != null)
					{
						_selectedTabButton.Selected = false;
					}

					_selectedTabButton = value;

					if (_selectedTabButton != null)
					{
						_selectedTabButton.Selected = true;
					}

					this.LayoutBuilder.RebuildLayout(this.TabStripBounds);
					this.OnSelectedIndexChanged(EventArgs.Empty);
				}
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
		 * LayoutManager
		 */

		private INuGenTabLayoutManager _layoutManager;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenTabLayoutManager LayoutManager
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
		 * HitTest
		 */

		/// <summary>
		/// </summary>
		/// <param name="pointToTest">Screen coordinates are expected.</param>
		/// <returns></returns>
		public NuGenTabControlHitResults HitTest(Point pointToTest)
		{
			Point cp = this.PointToClient(pointToTest);

			if (this.ClientRectangle.Contains(cp))
			{
				foreach (NuGenTabButton tabButton in _tabButtons)
				{
					if (tabButton.Bounds.Contains(cp))
					{
						return NuGenTabControlHitResults.TabButtons;
					}
				}

				return NuGenTabControlHitResults.Body;
			}

			return NuGenTabControlHitResults.Nowhere;
		}

		/*
		 * SelectTab
		 */

		/// <summary>
		/// </summary>
		/// <param name="tabPageToSelect"></param>
		public void SelectTab(NuGenTabPage tabPageToSelect)
		{
			this.SelectedTab = tabPageToSelect;
		}

		#endregion

		#region Methods.Protected.Overridden

		/*
		 * OnControlRemoved
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.ControlRemoved"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ControlEventArgs"></see> that contains the event data.</param>
		protected override void OnControlRemoved(ControlEventArgs e)
		{
			base.OnControlRemoved(e);

			if (e.Control is NuGenTabPage)
			{
				NuGenTabPage tabPage = (NuGenTabPage)e.Control;
				int index = this.TabPages.IndexOf(tabPage);

				this.RemoveTabPage(tabPage);
				if (this.TabPages.ListInternal.Remove(tabPage))
				{
					this.OnTabPageRemoved(new NuGenCollectionEventArgs<NuGenTabPage>(index, tabPage));
				}
			}
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
			Debug.Assert(this.TabRenderer != null, "this.TabRenderer != null");
			NuGenTabBodyPaintParams tabPageParams = new NuGenTabBodyPaintParams(e.Graphics, this.TabPageBounds);
			tabPageParams.FlatStyle = this.FlatStyle;
			this.TabRenderer.DrawTabBody(tabPageParams);
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
			this.LayoutBuilder.RebuildLayout(this.TabStripBounds);
		}

		#endregion

		#region Methods.Private.Layout

		/*
		 * SetPadding
		 */

		private void SetPadding(FlatStyle flatStyle)
		{
			Padding currentPadding = this.TabRenderer.GetPadding(flatStyle);
			currentPadding.Top += this.TabStripBounds.Bottom;
			this.Padding = currentPadding;
		}

		#endregion

		#region Methods.Private.TabButton

		/*
		 * InitializeTabButton
		 */

		private void InitializeTabButton(NuGenTabButton tabButtonToInitialize)
		{
			Debug.Assert(tabButtonToInitialize != null, "tabButtonToInitialize != null");

			tabButtonToInitialize.ShowCloseButton = this.CloseButtonOnTab;

			tabButtonToInitialize.Click += this.tabButton_Click;
			tabButtonToInitialize.Close += this.tabButton_Close;
			tabButtonToInitialize.DoubleClick += this.tabButton_DoubleClick;
			tabButtonToInitialize.DragDrop += this.tabButton_DragDrop;
			tabButtonToInitialize.DragEnter += this.tabButton_DragEnter;
			tabButtonToInitialize.DragLeave += this.tabButton_DragLeave;
			tabButtonToInitialize.MouseDown += this.tabButton_MouseDown;
			tabButtonToInitialize.MouseEnter += this.tabButton_MouseEnter;
			tabButtonToInitialize.MouseHover += this.tabButton_MouseHover;
			tabButtonToInitialize.MouseLeave += this.tabButton_MouseLeave;
			tabButtonToInitialize.MouseMove += this.tabButton_MouseMove;
			tabButtonToInitialize.MouseUp += this.tabButton_MouseUp;
			tabButtonToInitialize.SelectedChanged += this.tabButton_SelectedChanged;

			if (this.Enabled)
			{
				this.SelectedTabButton = tabButtonToInitialize;
			}
			else
			{
				this.SelectedTabButton = null;
			}

			this.LayoutBuilder.RebuildLayout(this.TabStripBounds);
		}

		/*
		 * AddTabButton
		 */

		private void AddTabButton(NuGenTabButton tabButtonToAdd)
		{
			Debug.Assert(tabButtonToAdd != null, "tabButtonToAdd != null");
			Debug.Assert(_tabButtons != null, "_tabButtons != null");

			_tabButtons.Add(tabButtonToAdd);
			this.Controls.Add(tabButtonToAdd);

			this.InitializeTabButton(tabButtonToAdd);
		}

		/*
		 * InsertTabButton
		 */

		private void InsertTabButton(int index, NuGenTabButton tabButtonToInsert)
		{
			Debug.Assert(tabButtonToInsert != null, "tabButtonToInsert != null");
			Debug.Assert(_tabButtons != null, "_tabButtons != null");

			_tabButtons.Insert(index, tabButtonToInsert);
			this.Controls.Add(tabButtonToInsert);

			this.InitializeTabButton(tabButtonToInsert);
		}

		/*
		 * RemoveTabButton
		 */

		private void RemoveTabButton(NuGenTabButton tabButtonToRemove)
		{
			Debug.Assert(tabButtonToRemove != null, "tabButtonToRemove != null");

			tabButtonToRemove.Click -= this.tabButton_Click;
			tabButtonToRemove.Close -= this.tabButton_Close;
			tabButtonToRemove.DoubleClick -= this.tabButton_DoubleClick;
			tabButtonToRemove.DragDrop -= this.tabButton_DragDrop;
			tabButtonToRemove.DragEnter -= this.tabButton_DragEnter;
			tabButtonToRemove.DragLeave -= this.tabButton_DragLeave;
			tabButtonToRemove.MouseDown -= this.tabButton_MouseDown;
			tabButtonToRemove.MouseEnter -= this.tabButton_MouseEnter;
			tabButtonToRemove.MouseHover -= this.tabButton_MouseHover;
			tabButtonToRemove.MouseLeave -= this.tabButton_MouseLeave;
			tabButtonToRemove.MouseMove -= this.tabButton_MouseMove;
			tabButtonToRemove.MouseUp -= this.tabButton_MouseUp;
			tabButtonToRemove.SelectedChanged -= this.tabButton_SelectedChanged;

			Debug.Assert(_tabButtons != null, "_tabButtons != null");
			int selectedIndex = _tabButtons.IndexOf(tabButtonToRemove);
			_tabButtons.Remove(tabButtonToRemove);
			this.Controls.Remove(tabButtonToRemove);

			if (_tabButtons.Count < 1)
			{
				this.SelectedTabButton = null;
				return;
			}

			if (selectedIndex >= _tabButtons.Count)
			{
				selectedIndex = _tabButtons.Count - 1;
			}

			this.SelectedTabButton = _tabButtons[selectedIndex];
			this.LayoutBuilder.RebuildLayout(this.TabStripBounds);
		}

		#endregion

		#region Methods.Private.TabPage

		/*
		 * InitializeTabPage
		 */

		private NuGenTabButton InitializeTabPage(NuGenTabPage tabPageToInitialize)
		{
			Debug.Assert(tabPageToInitialize != null, "tabPageToInitialize != null");
			Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");

			NuGenTabButton tabButtonToAssociate = new NuGenTabButton(this.ServiceProvider);
			tabButtonToAssociate.Image = tabPageToInitialize.TabButtonImage;
			tabButtonToAssociate.Text = tabPageToInitialize.Text;

			Debug.Assert(_buttonPageDictionary != null, "_buttonPageDictionary != null");
			Debug.Assert(_pageButtonDictionary != null, "_pageButtonDictionary != null");

			_buttonPageDictionary.Add(tabButtonToAssociate, tabPageToInitialize);
			_pageButtonDictionary.Add(tabPageToInitialize, tabButtonToAssociate);

			tabPageToInitialize.EnabledChanged += this.tabPage_EnabledChanged;
			tabPageToInitialize.TabButtonImageChanged += this.tabPage_TabButtonImageChanged;
			tabPageToInitialize.TextChanged += this.tabPage_TextChanged;

			this.Controls.Add(tabPageToInitialize);
			tabPageToInitialize.BringToFront();

			return tabButtonToAssociate;
		}

		/*
		 * AddTabPage
		 */

		private void AddTabPage(NuGenTabPage tabPageToAdd)
		{
			Debug.Assert(tabPageToAdd != null, "tabPageToAdd != null");
			this.AddTabButton(this.InitializeTabPage(tabPageToAdd));
			this.OnTabPageAdded(
				new NuGenCollectionEventArgs<NuGenTabPage>(this.TabPages.Count - 1, tabPageToAdd)
			);
		}

		/*
		 * InsertTabPage
		 */

		private void InsertTabPage(int index, NuGenTabPage tabPageToInsert)
		{
			Debug.Assert(tabPageToInsert != null, "tabPageToInsert != null");
			this.InsertTabButton(index, this.InitializeTabPage(tabPageToInsert));
			this.OnTabPageAdded(
				new NuGenCollectionEventArgs<NuGenTabPage>(index, tabPageToInsert)
			);
		}

		/*
		 * RemoveTabPage
		 */

		private void RemoveTabPage(NuGenTabPage tabPageToRemove)
		{
			Debug.Assert(tabPageToRemove != null, "tabPageToRemove != null");
			Debug.Assert(_buttonPageDictionary != null, "_buttonPageDictionary != null");
			Debug.Assert(_pageButtonDictionary != null, "_pageButtonDictionary != null");

			NuGenTabButton associatedTabButton = _pageButtonDictionary[tabPageToRemove];
			Debug.Assert(associatedTabButton != null, "associatedTabButton != null");

			tabPageToRemove.EnabledChanged -= this.tabPage_EnabledChanged;
			tabPageToRemove.TabButtonImageChanged -= this.tabPage_TabButtonImageChanged;
			tabPageToRemove.TextChanged -= this.tabPage_TextChanged;

			_buttonPageDictionary.Remove(associatedTabButton);
			_pageButtonDictionary.Remove(tabPageToRemove);

			this.RemoveTabButton(associatedTabButton);
		}

		#endregion

		#region EventHandlers.TabButton

		private void tabButton_Click(object sender, EventArgs e)
		{
			Debug.Assert(sender is NuGenTabButton, "sender is NuGenTabButton");
			this.SelectedTabButton = (NuGenTabButton)sender;
			this.OnTabButtonClick(sender, e);
		}

		private void tabButton_Close(object sender, EventArgs e)
		{
			Debug.Assert(sender is NuGenTabButton, "sender is NuGenTabButton");
			Debug.Assert(_buttonPageDictionary != null, "_buttonPageDictionary != null");
			Debug.Assert(_buttonPageDictionary.ContainsKey((NuGenTabButton)sender), "_buttonPageDictionary.ContainsKey((NuGenTabButton)sender)");

			NuGenTabPage tabPage = _buttonPageDictionary[(NuGenTabButton)sender];
			NuGenTabCancelEventArgs eventArgs = new NuGenTabCancelEventArgs(tabPage);
			this.OnTabCloseButtonClick(eventArgs);

			if (!eventArgs.Cancel)
			{
				this.TabPages.Remove(_buttonPageDictionary[(NuGenTabButton)sender]);
			}
		}

		private void tabButton_DoubleClick(object sender, EventArgs e)
		{
			this.OnTabButtonDoubleClick(sender, e);
		}

		private void tabButton_DragDrop(object sender, DragEventArgs e)
		{
			this.OnTabButtonDragDrop(sender, e);
		}

		private void tabButton_DragEnter(object sender, DragEventArgs e)
		{
			this.OnTabButtonDragEnter(sender, e);
		}

		private void tabButton_DragLeave(object sender, EventArgs e)
		{
			this.OnTabButtonDragLeave(sender, e);
		}

		private void tabButton_MouseDown(object sender, MouseEventArgs e)
		{
			this.OnTabButtonMouseDown(sender, e);
		}

		private void tabButton_MouseEnter(object sender, EventArgs e)
		{
			this.OnTabButtonMouseEnter(sender, e);
		}

		private void tabButton_MouseHover(object sender, EventArgs e)
		{
			this.OnTabButtonMouseHover(sender, e);
		}

		private void tabButton_MouseLeave(object sender, EventArgs e)
		{
			this.OnTabButtonMouseLeave(sender, e);
		}

		private void tabButton_MouseMove(object sender, MouseEventArgs e)
		{
			this.OnTabButtonMouseMove(sender, e);
		}

		private void tabButton_MouseUp(object sender, MouseEventArgs e)
		{
			this.OnTabButtonMouseUp(sender, e);
		}

		private void tabButton_SelectedChanged(object sender, EventArgs e)
		{
			Debug.Assert(sender is NuGenTabButton, "sender is NuGenTabButton");
			NuGenTabButton tabButton = (NuGenTabButton)sender;

			if (tabButton.Selected)
			{
				Debug.Assert(_buttonPageDictionary != null, "_buttonPageDictionary != null");
				Debug.Assert(_buttonPageDictionary.ContainsKey(tabButton), "_buttonPageDictionary.ContainsKey(tabButton)");
				NuGenTabPage activeTabPage = _buttonPageDictionary[tabButton];
				Debug.Assert(activeTabPage != null, "activeTabPage != null");
				activeTabPage.BringToFront();
			}
		}

		#endregion

		#region EventHandlers.TabPage

		private void tabPage_EnabledChanged(object sender, EventArgs e)
		{
			Debug.Assert(sender is NuGenTabPage, "sender is NuGenTabPage");
			NuGenTabPage tabPage = (NuGenTabPage)sender;

			Debug.Assert(_pageButtonDictionary != null, "_pageButtonDictionary != null");
			Debug.Assert(_pageButtonDictionary.ContainsKey(tabPage), "_pageButtonDictionary.ContainsKey(tabPage)");
			NuGenTabButton tabButton = _pageButtonDictionary[tabPage];

			tabButton.Enabled = tabPage.Enabled;
			tabButton.Invalidate(true);

			if (
				!tabPage.Enabled
				&& tabPage == this.SelectedTab
				)
			{
				NuGenTabPage newSelectedTabPage = null;

				for (int i = this.SelectedIndex - 1; i > -1; i--)
				{
					if (this.TabPages[i].Enabled)
					{
						newSelectedTabPage = this.TabPages[i];
					}
				}

				if (newSelectedTabPage == null)
				{
					for (int i = this.SelectedIndex + 1; i < this.TabPages.Count; i++)
					{
						if (this.TabPages[i].Enabled)
						{
							newSelectedTabPage = this.TabPages[i];
						}
					}
				}

				this.SelectedTab = newSelectedTabPage;
			}
		}

		private void tabPage_TabButtonImageChanged(object sender, EventArgs e)
		{
			Debug.Assert(sender is NuGenTabPage, "sender is NuGenTabPage");
			NuGenTabPage tabPage = (NuGenTabPage)sender;
			
			Debug.Assert(_pageButtonDictionary != null, "_pageButtonDictionary != null");
			Debug.Assert(_pageButtonDictionary.ContainsKey(tabPage), "_pageButtonDictionary.ContainsKey(tabPage)");
			NuGenTabButton tabButton = _pageButtonDictionary[tabPage];

			tabButton.Image = tabPage.TabButtonImage;
		}

		private void tabPage_TextChanged(object sender, EventArgs e)
		{
			Debug.Assert(sender is NuGenTabPage, "sender is NuGenTabPage");

			NuGenTabPage tabPage = (NuGenTabPage)sender;

			Debug.Assert(_pageButtonDictionary != null, "_pageButtonDictionary != null");
			Debug.Assert(_pageButtonDictionary.ContainsKey(tabPage), "_pageButtonDictionary.ContainsKey(tabPage)");
			_pageButtonDictionary[tabPage].Text = tabPage.Text;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTabControl"/> class.
		/// </summary>
		public NuGenTabControl()
			: this(NuGenServiceManager.TabControlServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTabControl"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenButtonStateTracker"/><para/>
		/// <see cref="INuGenTabRenderer"/><para/>
		/// <see cref="INuGenTabStateTracker"/><para/>
		/// <see cref="INuGenTabLayoutManager"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="serviceProvider"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public NuGenTabControl(INuGenServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			_serviceProvider = serviceProvider;

			_buttonPageDictionary = new Dictionary<NuGenTabButton, NuGenTabPage>();
			_pageButtonDictionary = new Dictionary<NuGenTabPage, NuGenTabButton>();
			_tabButtons = new List<NuGenTabButton>();

			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetPadding(this.FlatStyle);
			this.FlatStyleChanged += delegate
			{
				this.SetPadding(this.FlatStyle);
			};
			
			this.BackColor = Color.Transparent;
		}

		#endregion
	}
}
