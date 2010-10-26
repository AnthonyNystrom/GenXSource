/* -----------------------------------------------
 * NuGenSwitcher.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Collections;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.SwitcherInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Genetibase.Shared.Controls.Properties;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	[ToolboxItem(false)]
	[DefaultEvent("SwitchButtonClick")]
	[DefaultProperty("Alignment")]
	[Designer("Genetibase.Shared.Controls.Design.NuGenSwitcherDesigner")]
	[NuGenSRDescription("Description_Switcher")]
	[System.ComponentModel.DesignerCategory("Code")]
	public partial class NuGenSwitcher : NuGenControl
	{
		#region Events

		/*
		 * SwitchButtonClick
		 */

		private static readonly object _switchButtonClick = new object();

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		public event EventHandler SwitchButtonClick
		{
			add
			{
				this.Events.AddHandler(_switchButtonClick, value);
			}
			remove
			{
				this.Events.RemoveHandler(_switchButtonClick, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenSwitcher.SwitchButtonClick"/> event.
		/// </summary>
		protected virtual void OnSwitchButtonClick(object sender, EventArgs e)
		{
			EventHandler handler = this.Events[_switchButtonClick] as EventHandler;

			if (handler != null)
			{
				handler(sender, e);
			}
		}

		/*
		 * SwitchButtonDoubleClick
		 */

		private static readonly object _switchButtonDoubleClick = new object();

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		public event EventHandler SwitchButtonDoubleClick
		{
			add
			{
				this.Events.AddHandler(_switchButtonDoubleClick, value);
			}
			remove
			{
				this.Events.RemoveHandler(_switchButtonDoubleClick, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenSwitcher.SwitchButtonDoubleClick"/> event.
		/// </summary>
		protected virtual void OnSwitchButtonDoubleClick(object sender, EventArgs e)
		{
			EventHandler handler = this.Events[_switchButtonDoubleClick] as EventHandler;

			if (handler != null)
			{
				handler(sender, e);
			}
		}

		/*
		 * SwitchButtonDragDrop
		 */

		private static readonly object _switchButtonDragDrop = new object();

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		public event DragEventHandler SwitchButtonDragDrop
		{
			add
			{
				this.Events.AddHandler(_switchButtonDragDrop, value);
			}
			remove
			{
				this.Events.RemoveHandler(_switchButtonDragDrop, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenSwitcher.SwitchButtonDragDrop"/> event.
		/// </summary>
		protected virtual void OnSwitchButtonDragDrop(object sender, DragEventArgs e)
		{
			DragEventHandler handler = this.Events[_switchButtonDragDrop] as DragEventHandler;

			if (handler != null)
			{
				handler(sender, e);
			}
		}

		/*
		 * SwitchButtonDragEnter
		 */

		private static readonly object _switchButtonDragEnter = new object();

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		public event DragEventHandler SwitchButtonDragEnter
		{
			add
			{
				this.Events.AddHandler(_switchButtonDragEnter, value);
			}
			remove
			{
				this.Events.RemoveHandler(_switchButtonDragEnter, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenSwitcher.SwitchButtonDragEnter"/> event.
		/// </summary>
		protected virtual void OnSwitchButtonDragEnter(object sender, DragEventArgs e)
		{
			DragEventHandler handler = this.Events[_switchButtonDragEnter] as DragEventHandler;

			if (handler != null)
			{
				handler(sender, e);
			}
		}

		/*
		 * SwitchButtonDragLeave
		 */

		private static readonly object _switchButtonDragLeave = new object();

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		public event EventHandler SwitchButtonDragLeave
		{
			add
			{
				this.Events.AddHandler(_switchButtonDragLeave, value);
			}
			remove
			{
				this.Events.RemoveHandler(_switchButtonDragLeave, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenSwitcher.SwitchButtonDragLeave"/> event.
		/// </summary>
		protected virtual void OnSwitchButtonDragLeave(object sender, EventArgs e)
		{
			EventHandler handler = this.Events[_switchButtonDragLeave] as EventHandler;

			if (handler != null)
			{
				handler(sender, e);
			}
		}

		/*
		 * SwitchButtonMouseDown
		 */

		private static readonly object _switchButtonMouseDown = new object();

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		public event MouseEventHandler SwitchButtonMouseDown
		{
			add
			{
				this.Events.AddHandler(_switchButtonMouseDown, value);
			}
			remove
			{
				this.Events.RemoveHandler(_switchButtonMouseDown, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenSwitcher.SwitchButtonMouseDown"/> event.
		/// </summary>
		protected virtual void OnSwitchButtonMouseDown(object sender, MouseEventArgs e)
		{
			MouseEventHandler handler = this.Events[_switchButtonMouseDown] as MouseEventHandler;

			if (handler != null)
			{
				handler(sender, e);
			}
		}

		/*
		 * SwitchButtonMouseEnter
		 */

		private static readonly object _switchButtonMouseEnter = new object();

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		public event EventHandler SwitchButtonMouseEnter
		{
			add
			{
				this.Events.AddHandler(_switchButtonMouseEnter, value);
			}
			remove
			{
				this.Events.RemoveHandler(_switchButtonMouseEnter, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenSwitcher.SwitchButtonMouseEnter"/> event.
		/// </summary>
		protected virtual void OnSwitchButtonMouseEnter(object sender, EventArgs e)
		{
			EventHandler handler = this.Events[_switchButtonMouseEnter] as EventHandler;

			if (handler != null)
			{
				handler(sender, e);
			}
		}

		/*
		 * SwitchButtonMouseHover
		 */

		private static readonly object _switchButtonMouseHover = new object();

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		public event EventHandler SwitchButtonMouseHover
		{
			add
			{
				this.Events.AddHandler(_switchButtonMouseHover, value);
			}
			remove
			{
				this.Events.RemoveHandler(_switchButtonMouseHover, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenSwitcher.SwitchButtonMouseHover"/> event.
		/// </summary>
		protected virtual void OnSwitchButtonMouseHover(object sender, EventArgs e)
		{
			EventHandler handler = this.Events[_switchButtonMouseHover] as EventHandler;

			if (handler != null)
			{
				handler(sender, e);
			}
		}

		/*
		 * SwitchButtonMouseLeave
		 */

		private static readonly object _switchButtonMouseLeave = new object();

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		public event EventHandler SwitchButtonMouseLeave
		{
			add
			{
				this.Events.AddHandler(_switchButtonMouseLeave, value);
			}
			remove
			{
				this.Events.RemoveHandler(_switchButtonMouseLeave, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenSwitcher.SwitchButtonMouseLeave"/> event.
		/// </summary>
		protected virtual void OnSwitchButtonMouseLeave(object sender, EventArgs e)
		{
			EventHandler handler = this.Events[_switchButtonMouseLeave] as EventHandler;

			if (handler != null)
			{
				handler(sender, e);
			}
		}

		/*
		 * SwitchButtonMouseUp
		 */

		private static readonly object _switchButtonMouseUp = new object();

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		public event MouseEventHandler SwitchButtonMouseUp
		{
			add
			{
				this.Events.AddHandler(_switchButtonMouseUp, value);
			}
			remove
			{
				this.Events.RemoveHandler(_switchButtonMouseUp, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenSwitcher.SwitchButtonMouseUp"/> event.
		/// </summary>
		protected virtual void OnSwitchButtonMouseUp(object sender, MouseEventArgs e)
		{
			MouseEventHandler handler = this.Events[_switchButtonMouseUp] as MouseEventHandler;

			if (handler != null)
			{
				handler(sender, e);
			}
		}

		/*
		 * SwitchPageAdded
		 */

		private static readonly object _switchPageAdded = new object();

		/// <summary>
		/// Occurs when a new <see cref="NuGenSwitchPage"/> is added.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		[NuGenSRDescription("Description_Switcher_SwitchPageAdded")]
		public event EventHandler<NuGenCollectionEventArgs<NuGenSwitchPage>> SwitchPageAdded
		{
			add
			{
				this.Events.AddHandler(_switchPageAdded, value);
			}
			remove
			{
				this.Events.RemoveHandler(_switchPageAdded, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenSwitcher.SwitchPageAdded"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnSwitchPageAdded(NuGenCollectionEventArgs<NuGenSwitchPage> e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokeEventHandlerT<NuGenCollectionEventArgs<NuGenSwitchPage>>(_switchPageAdded, e);
		}

		/*
		 * SwitchPageRemoved
		 */

		private static readonly object _switchPageRemoved = new object();

		/// <summary>
		/// Occurs when a <see cref="NuGenSwitchPage"/> is removed.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		[NuGenSRDescription("Description_Switcher_SwitchPageRemoved")]
		public event EventHandler<NuGenCollectionEventArgs<NuGenSwitchPage>> SwitchPageRemoved
		{
			add
			{
				this.Events.AddHandler(_switchPageRemoved, value);
			}
			remove
			{
				this.Events.RemoveHandler(_switchPageRemoved, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenSwitcher.SwitchPageRemoved"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnSwitchPageRemoved(NuGenCollectionEventArgs<NuGenSwitchPage> e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokeEventHandlerT<NuGenCollectionEventArgs<NuGenSwitchPage>>(_switchPageRemoved, e);
		}

		#endregion

		#region Properties.Appearance

		/*
		 * Alignment
		 */

		private NuGenSwitcherAlignment _alignment;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(NuGenSwitcherAlignment.Top)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Switcher_Alignment")]
		public NuGenSwitcherAlignment Alignment
		{
			get
			{
				return _alignment;
			}
			set
			{
				if (_alignment != value)
				{
					_alignment = value;
					this.OnAlignmentChanged(EventArgs.Empty);
					this.RebuildLayout();
				}
			}
		}

		private static readonly object _alignmentChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Alignment"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Switcher_AlignmentChanged")]
		public event EventHandler AlignmentChanged
		{
			add
			{
				this.Events.AddHandler(_alignmentChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_alignmentChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="AlignmentChanged"/> event.
		/// </summary>
		protected virtual void OnAlignmentChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_alignmentChanged, e);
		}

		/*
		 * SwitchButtonImageAlign
		 */

		/// <summary>
		/// Gets or sets the alignment of the image on switch buttons.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Switcher_SwitchButtonImageAlign")]
		public ContentAlignment SwitchButtonImageAlign
		{
			get
			{
				return _switchPanel.SwitchButtonImageAlign;
			}
			set
			{
				_switchPanel.SwitchButtonImageAlign = value;
			}
		}

		private bool ShouldSerializeSwitchButtonImageAlign()
		{
			return this.SwitchButtonImageAlign != NuGenSwitchPanel.DefaultSwitchButtonImageAlign;
		}

		private void ResetSwitchButtonImageAlign()
		{
			this.SwitchButtonImageAlign = NuGenSwitchPanel.DefaultSwitchButtonImageAlign;
		}

		private static readonly object _switchButtonImageAlignChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="SwitchButtonImageAlign"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Switcher_SwitchButtonImageAlignChanged")]
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

		/// <summary>
		/// Gets or sets the alignment of the text of switch buttons.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Switcher_SwitchButtonTextAlign")]
		public ContentAlignment SwitchButtonTextAlign
		{
			get
			{
				return _switchPanel.SwitchButtonTextAlign;
			}
			set
			{
				_switchPanel.SwitchButtonTextAlign = value;
			}
		}

		private bool ShouldSerializeSwitchButtonTextAlign()
		{
			return this.SwitchButtonTextAlign != NuGenSwitchPanel.DefaultSwitchButtonTextAlign;
		}

		private void ResetSwitchButtonTextAlign()
		{
			this.SwitchButtonTextAlign = NuGenSwitchPanel.DefaultSwitchButtonTextAlign;
		}

		private static readonly object _switchButtonTextAlignChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="SwitchButtonTextAlign"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Switcher_SwitchButtonTextAlignChanged")]
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

		#region Properties.Layout

		/*
		 * SwitchButtonSize
		 */

		/// <summary>
		/// X-coordinate determines the width of the buttons in horizontal orientation;
		/// Y-coordinate determines the height of the buttons in vertical orientation.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Layout")]
		[NuGenSRDescription("Description_Switcher_SwitchButtonSize")]
		public Size SwitchButtonSize
		{
			get
			{
				return _switchPanel.SwitchButtonSize;
			}
			set
			{
				_switchPanel.SwitchButtonSize = value;
			}
		}

		private static readonly object _switchButtonSizeChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="SwitchButtonSize"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Switcher_SwitchButtonSizeChanged")]
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
		 * SwitchPanelSize
		 */

		private Size _switchPanelSize;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Layout")]
		[NuGenSRDescription("Description_Switcher_SwitchPanelSize")]
		public Size SwitchPanelSize
		{
			get
			{
				if (_switchPanelSize == Size.Empty)
				{
					return this.DefaultSwitchPanelSize;
				}

				return _switchPanelSize;
			}
			set
			{
				if (_switchPanelSize != value)
				{
					_switchPanelSize = value;
					this.OnSwitchPanelSizeChanged(EventArgs.Empty);
					this.RebuildLayout();
				}
			}
		}

		private static readonly Size _defaultSwitchPanelSize = new Size(54, 54);

		private Size DefaultSwitchPanelSize
		{
			get
			{
				return _defaultSwitchPanelSize;
			}
		}

		private void ResetSwitchPanelSize()
		{
			this.SwitchPanelSize = this.DefaultSwitchPanelSize;
		}

		private bool ShouldSerializeSwitchPanelSize()
		{
			return this.SwitchPanelSize != this.DefaultSwitchPanelSize;
		}

		private static readonly object _switchPanelSizeChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="SwitchPanelSize"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Switcher_SwitchPanelSizeChanged")]
		public event EventHandler SwitchPanelSizeChanged
		{
			add
			{
				this.Events.AddHandler(_switchPanelSizeChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_switchPanelSizeChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="SwitchPanelSizeChanged"/> event.
		/// </summary>
		protected virtual void OnSwitchPanelSizeChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_switchPanelSizeChanged, e);
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
		 * SelectedIndex
		 */

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int SelectedIndex
		{
			get
			{
				NuGenSwitchPage switchPage = this.SelectedSwitchPage;

				if (switchPage != null)
				{
					return this.SwitchPages.IndexOf(switchPage);
				}

				return -1;
			}
			set
			{
				if (
					this.SelectedSwitchPage == null
					|| this.SelectedIndex != value
					)
				{
					this.SelectedSwitchPage = this.SwitchPages[value];
				}
			}
		}

		/*
		 * SelectedSwitchPage
		 */

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public NuGenSwitchPage SelectedSwitchPage
		{
			get
			{
				NuGenSwitchButton switchButton = _switchPanel.SelectedSwitchButton;

				if (switchButton != null)
				{
					Debug.Assert(_buttonPageDictionary != null, "_buttonPageDictionary != null");
					NuGenSwitchPage selectedSwitchPage = _buttonPageDictionary[switchButton];
					Debug.Assert(selectedSwitchPage != null, "selectedSwitchPage != null");
					return selectedSwitchPage;
				}

				return null;
			}
			set
			{
				if (value == null)
				{
					_switchPanel.SelectedSwitchButton = null;
				}
				else
				{
					Debug.Assert(_pageButtonDictionary != null, "_pageButtonDictionary != null");

					if (!_pageButtonDictionary.ContainsKey(value))
					{
						throw new ArgumentException(
							string.Format(
								Resources.Argument_InvalidSwitchPage,
								typeof(NuGenSwitchPage).Name,
								typeof(NuGenSwitcher).Name
							)
						);
					}

					_switchPanel.SelectedSwitchButton = _pageButtonDictionary[value];
				}
			}
		}

		private static readonly object _selectedSwitchPageChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="SelectedSwitchPage"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Switcher_SelectedSwitchPageChanged")]
		public event EventHandler SelectedSwitchPageChanged
		{
			add
			{
				this.Events.AddHandler(_selectedSwitchPageChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_selectedSwitchPageChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="SelectedSwitchPageChanged"/> event.
		/// </summary>
		protected virtual void OnSelectedSwitchPageChanged(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(_selectedSwitchPageChanged, e);
		}

		/*
		 * SwitchPages
		 */

		private SwitchPageCollection _switchPages;

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public SwitchPageCollection SwitchPages
		{
			get
			{
				if (_switchPages == null)
				{
					_switchPages = new SwitchPageCollection(this);
				}

				return _switchPages;
			}
		}

		#endregion

		#region Properties.Public.Overridden

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

		#region Methods.Public

		/// <summary>
		/// </summary>
		/// <param name="pointToTest">Screen coordinates expected.</param>
		public NuGenSwitcherHitResult HitTest(Point pointToTest)
		{
			Point cp = this.PointToClient(pointToTest);

			if (this.ClientRectangle.Contains(cp))
			{
				if (_switchPanel.Bounds.Contains(cp))
				{
					return _switchPanel.HitTest(pointToTest);
				}

				return NuGenSwitcherHitResult.Body;
			}

			return NuGenSwitcherHitResult.Nowhere;
		}

		#endregion

		#region Methods.Protected.Overridden

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.ControlRemoved"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.ControlEventArgs"></see> that contains the event data.</param>
		protected override void OnControlRemoved(ControlEventArgs e)
		{
			base.OnControlRemoved(e);

			if (e.Control is NuGenSwitchPage)
			{
				NuGenSwitchPage switchPage = (NuGenSwitchPage)e.Control;
				int index = this.SwitchPages.IndexOf(switchPage);
				this.RemoveSwitchPage(switchPage);

				if (this.SwitchPages.ListInternal.Remove(switchPage))
				{
					this.OnSwitchPageRemoved(new NuGenCollectionEventArgs<NuGenSwitchPage>(index, switchPage));
				}
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="e">The <see cref="System.Windows.Forms.LayoutEventArgs"/> instance containing the event data.</param>
		protected override void OnLayout(LayoutEventArgs e)
		{
			this.RebuildLayout();
			base.OnLayout(e);
		}

		#endregion

		#region Methods.Private.Layout

		private void RebuildLayout()
		{
			_switchPanel.Dock = DockStyle.None;

			switch (this.Alignment)
			{
				case NuGenSwitcherAlignment.Bottom:
				{
					_switchPanel.Orientation = NuGenOrientationStyle.Horizontal;
					_switchPanel.Dock = DockStyle.Bottom;
					break;
				}
				case NuGenSwitcherAlignment.Left:
				{
					_switchPanel.Orientation = NuGenOrientationStyle.Vertical;
					_switchPanel.Dock = DockStyle.Left;
					break;
				}
				case NuGenSwitcherAlignment.Right:
				{
					_switchPanel.Orientation = NuGenOrientationStyle.Vertical;
					_switchPanel.Dock = DockStyle.Right;
					break;
				}
				default:
				{
					_switchPanel.Orientation = NuGenOrientationStyle.Horizontal;
					_switchPanel.Dock = DockStyle.Top;
					break;
				}
			}

			Size switchPanelSize = this.SwitchPanelSize;

			if (_switchPanel.Orientation == NuGenOrientationStyle.Horizontal)
			{
				_switchPanel.Height = switchPanelSize.Height;
			}
			else
			{
				_switchPanel.Width = switchPanelSize.Width;
			}
		}

		#endregion

		#region Methods.Private.SwitchButton

		/*
		 * InitializeSwitchButton
		 */

		private void InitializeSwitchButton(NuGenSwitchButton switchButtonToInitialize)
		{
			Debug.Assert(switchButtonToInitialize != null, "switchButtonToInitialize != null");

			switchButtonToInitialize.Click += _switchButton_Click;
			switchButtonToInitialize.DoubleClick += _switchButton_DoubleClick;
			switchButtonToInitialize.DragDrop += _switchButton_DragDrop;
			switchButtonToInitialize.DragEnter += _switchButton_DragEnter;
			switchButtonToInitialize.DragLeave += _switchButton_DragLeave;
			switchButtonToInitialize.MouseDown += _switchButton_MouseDown;
			switchButtonToInitialize.MouseEnter += _switchButton_MouseEnter;
			switchButtonToInitialize.MouseHover += _switchButton_MouseHover;
			switchButtonToInitialize.MouseLeave += _switchButton_MouseLeave;
			switchButtonToInitialize.MouseUp += _switchButton_MouseUp;
		}

		/*
		 * AddSwitchButton
		 */

		private void AddSwitchButton(NuGenSwitchButton switchButtonToAdd)
		{
			Debug.Assert(switchButtonToAdd != null, "switchButtonToAdd != null");
			Debug.Assert(_switchPanel != null, "_switchPanel != null");

			_switchPanel.Controls.Add(switchButtonToAdd);
			this.InitializeSwitchButton(switchButtonToAdd);
		}

		/*
		 * InsertSwitchButton
		 */

		private void InsertSwitchButton(int index, NuGenSwitchButton switchButtonToInsert)
		{
			Debug.Assert(switchButtonToInsert != null, "switchButtonToInsert != null");
			this.Controls.Add(switchButtonToInsert);
			this.InitializeSwitchButton(switchButtonToInsert);
		}

		/*
		 * RemoveSwitchButton
		 */

		private void RemoveSwitchButton(NuGenSwitchButton switchButtonToRemove)
		{
			Debug.Assert(switchButtonToRemove != null, "switchButtonToRemove != null");
			Debug.Assert(_switchPanel != null, "_switchPanel != null");

			switchButtonToRemove.Click -= _switchButton_Click;
			switchButtonToRemove.DoubleClick -= _switchButton_DoubleClick;
			switchButtonToRemove.DragDrop -= _switchButton_DragDrop;
			switchButtonToRemove.DragEnter -= _switchButton_DragEnter;
			switchButtonToRemove.DragLeave -= _switchButton_DragLeave;
			switchButtonToRemove.MouseDown -= _switchButton_MouseDown;
			switchButtonToRemove.MouseEnter -= _switchButton_MouseEnter;
			switchButtonToRemove.MouseHover -= _switchButton_MouseHover;
			switchButtonToRemove.MouseLeave -= _switchButton_MouseLeave;
			switchButtonToRemove.MouseUp -= _switchButton_MouseUp;

			_switchPanel.Controls.Remove(switchButtonToRemove);
		}

		#endregion

		#region Methods.Private.SwitchPage

		/*
		 * InitializeSwitchPage
		 */

		private NuGenSwitchButton InitializeSwitchPage(NuGenSwitchPage switchPageToInitialize)
		{
			Debug.Assert(switchPageToInitialize != null, "switchPageToInitialize != null");
			Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");

			NuGenSwitchButton switchButtonToAssociate = new NuGenSwitchButton(this.ServiceProvider);
			switchButtonToAssociate.Image = switchPageToInitialize.SwitchButtonImage;
			switchButtonToAssociate.Text = switchPageToInitialize.Text;

			Debug.Assert(_buttonPageDictionary != null, "_buttonPageDictionary != null");
			Debug.Assert(_pageButtonDictionary != null, "_pageButtonDictionary != null");
			_buttonPageDictionary.Add(switchButtonToAssociate, switchPageToInitialize);
			_pageButtonDictionary.Add(switchPageToInitialize, switchButtonToAssociate);

			switchPageToInitialize.EnabledChanged += _switchPage_EnabledChanged;
			switchPageToInitialize.SwitchButtonImageChanged += _switchPage_SwitchButtonImageChanged;
			switchPageToInitialize.TextChanged += _switchPage_TextChanged;

			this.Controls.Add(switchPageToInitialize);
			switchPageToInitialize.BringToFront();

			return switchButtonToAssociate;
		}

		/*
		 * AddSwitchPage
		 */

		private void AddSwitchPage(NuGenSwitchPage switchPageToAdd)
		{
			Debug.Assert(switchPageToAdd != null, "switchPageToAdd != null");
			this.AddSwitchButton(this.InitializeSwitchPage(switchPageToAdd));
			this.OnSwitchPageAdded(
				new NuGenCollectionEventArgs<NuGenSwitchPage>(this.SwitchPages.Count - 1, switchPageToAdd)
			);
		}

		/*
		 * InsertSwitchPage
		 */

		private void InsertSwitchPage(int index, NuGenSwitchPage switchPageToInsert)
		{
			Debug.Assert(switchPageToInsert != null, "switchPageToInsert != null");
			this.InsertSwitchButton(index, this.InitializeSwitchPage(switchPageToInsert));
			this.OnSwitchPageAdded(
				new NuGenCollectionEventArgs<NuGenSwitchPage>(index, switchPageToInsert)
			);
		}

		/*
		 * RemoveSwitchPage
		 */

		private void RemoveSwitchPage(NuGenSwitchPage switchPageToRemove)
		{
			Debug.Assert(switchPageToRemove != null, "switchPageToRemove != null");
			Debug.Assert(_buttonPageDictionary != null, "_buttonPageDictionary != null");
			Debug.Assert(_pageButtonDictionary != null, "_pageButtonDictionary != null");

			NuGenSwitchButton associatedSwitchButton = _pageButtonDictionary[switchPageToRemove];
			Debug.Assert(associatedSwitchButton != null, "associatedSwitchButton != null");

			switchPageToRemove.EnabledChanged -= _switchPage_EnabledChanged;
			switchPageToRemove.SwitchButtonImageChanged -= _switchPage_SwitchButtonImageChanged;
			switchPageToRemove.TextChanged -= _switchPage_TextChanged;

			_buttonPageDictionary.Remove(associatedSwitchButton);
			_pageButtonDictionary.Remove(switchPageToRemove);

			this.RemoveSwitchButton(associatedSwitchButton);
		}

		#endregion

		#region EventHandlers.SwitchButton

		private void _switchButton_Click(object sender, EventArgs e)
		{
			this.OnSwitchButtonClick(sender, e);
		}

		private void _switchButton_DoubleClick(object sender, EventArgs e)
		{
			this.OnSwitchButtonDoubleClick(sender, e);
		}

		private void _switchButton_DragDrop(object sender, DragEventArgs e)
		{
			this.OnSwitchButtonDragDrop(sender, e);
		}

		private void _switchButton_DragEnter(object sender, DragEventArgs e)
		{
			this.OnSwitchButtonDragEnter(sender, e);
		}

		private void _switchButton_DragLeave(object sender, EventArgs e)
		{
			this.OnSwitchButtonDragLeave(sender, e);
		}

		private void _switchButton_MouseDown(object sender, MouseEventArgs e)
		{
			this.OnSwitchButtonMouseDown(sender, e);
		}

		private void _switchButton_MouseEnter(object sender, EventArgs e)
		{
			this.OnSwitchButtonMouseEnter(sender, e);
		}

		private void _switchButton_MouseHover(object sender, EventArgs e)
		{
			this.OnSwitchButtonMouseHover(sender, e);
		}

		private void _switchButton_MouseLeave(object sender, EventArgs e)
		{
			this.OnSwitchButtonMouseLeave(sender, e);
		}

		private void _switchButton_MouseUp(object sender, MouseEventArgs e)
		{
			this.OnSwitchButtonMouseUp(sender, e);
		}

		#endregion

		#region EventHandlers.SwitchPage

		private void _switchPage_EnabledChanged(object sender, EventArgs e)
		{
			Debug.Assert(sender is NuGenSwitchPage, "sender is NuGenSwitchPage");
			NuGenSwitchPage switchPage = (NuGenSwitchPage)sender;

			Debug.Assert(_pageButtonDictionary != null, "_pageButtonDictionary != null");
			Debug.Assert(_pageButtonDictionary.ContainsKey(switchPage), "_pageButtonDictionary.ContainsKey(switchPage)");
			NuGenSwitchButton switchButton = _pageButtonDictionary[switchPage];

			switchButton.Enabled = switchPage.Enabled;
			switchButton.Invalidate();

			if (
				!switchPage.Enabled
				&& switchPage == this.SelectedSwitchPage
				)
			{
				NuGenSwitchPage newSelectedSwitchPage = null;

				for (int i = this.SelectedIndex - 1; i > -1; i--)
				{
					if (this.SwitchPages[i].Enabled)
					{
						newSelectedSwitchPage = this.SwitchPages[i];
					}
				}

				if (newSelectedSwitchPage == null)
				{
					for (int i = this.SelectedIndex + 1; i < this.SwitchPages.Count; i++)
					{
						if (this.SwitchPages[i].Enabled)
						{
							newSelectedSwitchPage = this.SwitchPages[i];
						}
					}
				}

				this.SelectedSwitchPage = newSelectedSwitchPage;
			}
		}

		private void _switchPage_SwitchButtonImageChanged(object sender, EventArgs e)
		{
			Debug.Assert(sender is NuGenSwitchPage, "sender is NuGenSwitchPage");
			NuGenSwitchPage switchPage = (NuGenSwitchPage)sender;

			Debug.Assert(_pageButtonDictionary != null, "_pageButtonDictionary != null");
			Debug.Assert(_pageButtonDictionary.ContainsKey(switchPage), "_pageButtonDictionary.ContainsKey(switchPage)");
			_pageButtonDictionary[switchPage].Image = switchPage.SwitchButtonImage;
		}

		private void _switchPage_TextChanged(object sender, EventArgs e)
		{
			Debug.Assert(sender is NuGenSwitchPage, "sender is NuGenSwitchPage");
			NuGenSwitchPage switchPage = (NuGenSwitchPage)sender;

			Debug.Assert(_pageButtonDictionary != null, "_pageButtonDictionary != null");
			Debug.Assert(_pageButtonDictionary.ContainsKey(switchPage), "_pageButtonDictionary.ContainsKey(switchPage)");
			_pageButtonDictionary[switchPage].Text = switchPage.Text;
		}

		#endregion

		#region EventHandlers.SwitchPanel

		private void _switchPanel_SelectedSwitchButtonChanged(object sender, EventArgs e)
		{
			if (_switchPanel.SelectedSwitchButton != null)
			{
				Debug.Assert(_buttonPageDictionary != null, "_buttonPageDictionary != null");
				NuGenSwitchPage activeSwitchPage = _buttonPageDictionary[_switchPanel.SelectedSwitchButton];
				Debug.Assert(activeSwitchPage != null, "activeSwitchPage != null");
				activeSwitchPage.BringToFront();
			}

			this.OnSelectedSwitchPageChanged(e);
		}

		private void _switchPanel_SwitchButtonImageAlignChanged(object sender, EventArgs e)
		{
			this.OnSwitchButtonImageAlignChanged(e);
		}

		private void _switchPanel_SwitchButtonTextAlignChanged(object sender, EventArgs e)
		{
			this.OnSwitchButtonTextAlignChanged(e);
		}

		private void _switchPanel_SwitchButtonSizeChanged(object sender, EventArgs e)
		{
			this.OnSwitchButtonSizeChanged(e);
		}

		#endregion

		private Dictionary<NuGenSwitchButton, NuGenSwitchPage> _buttonPageDictionary;
		private Dictionary<NuGenSwitchPage, NuGenSwitchButton> _pageButtonDictionary;
		private NuGenSwitchPanel _switchPanel;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSwitcher"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenButtonStateService"/></para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		///		<para><see cref="INuGenSwitchButtonLayoutManager"/></para>
		///		<para><see cref="INuGenSwitchButtonRenderer"/></para>
		///		<para><see cref="INuGenPanelRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSwitcher(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			_buttonPageDictionary = new Dictionary<NuGenSwitchButton, NuGenSwitchPage>();
			_pageButtonDictionary = new Dictionary<NuGenSwitchPage, NuGenSwitchButton>();

			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.Opaque, false);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.Selectable, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.UserPaint, true);

			this.BackColor = Color.Transparent;

			_switchPanel = new NuGenSwitchPanel(serviceProvider);
			_switchPanel.BackColor = Color.Blue;
			this.RebuildLayout();
			_switchPanel.Parent = this;
			_switchPanel.SelectedSwitchButtonChanged += _switchPanel_SelectedSwitchButtonChanged;
			_switchPanel.SwitchButtonImageAlignChanged += _switchPanel_SwitchButtonImageAlignChanged;
			_switchPanel.SwitchButtonTextAlignChanged += _switchPanel_SwitchButtonTextAlignChanged;
			_switchPanel.SwitchButtonSizeChanged += _switchPanel_SwitchButtonSizeChanged;
		}

		/// <summary>
		/// </summary>
		/// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_switchPanel != null)
				{
					_switchPanel.SelectedSwitchButtonChanged -= _switchPanel_SelectedSwitchButtonChanged;
					_switchPanel.SwitchButtonImageAlignChanged -= _switchPanel_SwitchButtonImageAlignChanged;
					_switchPanel.SwitchButtonTextAlignChanged -= _switchPanel_SwitchButtonTextAlignChanged;
					_switchPanel.SwitchButtonSizeChanged -= _switchPanel_SwitchButtonSizeChanged;
					_switchPanel.Dispose();
					_switchPanel = null;
				}
			}

			base.Dispose(disposing);
		}
	}
}
