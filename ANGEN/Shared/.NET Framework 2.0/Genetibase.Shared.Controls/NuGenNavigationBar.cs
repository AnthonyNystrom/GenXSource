/* -----------------------------------------------
 * NuGenNavigationBar.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Collections;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.NavigationBarInternals;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.TitleInternals;
using Genetibase.Shared.Controls.ToolStripInternals;
using Genetibase.Shared.Controls.ToolTipInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Windows.Forms;
using Genetibase.Shared.Controls.Properties;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Microsoft Outlook like navigation bar.
	/// </summary>
	[ToolboxItem(false)]
	[DefaultEvent("SelectedButtonChanged")]
	[DefaultProperty("Dock")]
	[Designer("Genetibase.Shared.Controls.Design.NuGenNavigationBarDesigner")]
	[NuGenSRDescription("Description_NavigationBar")]
	[System.ComponentModel.DesignerCategory("Code")]
	public partial class NuGenNavigationBar : NuGenControl
	{
		#region Events

		/*
		 * NavigationPaneAdded
		 */

		private static readonly object _navigationPaneAdded = new object();

		/// <summary>
		/// Occurs when a new navigation pane is added.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		[NuGenSRDescription("Description_NavigationBar_NavigationPaneAdded")]
		public event EventHandler<NuGenCollectionEventArgs<NuGenNavigationPane>> NavigationPaneAdded
		{
			add
			{
				this.Events.AddHandler(_navigationPaneAdded, value);
			}
			remove
			{
				this.Events.RemoveHandler(_navigationPaneAdded, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenNavigationBar.NavigationPaneAdded"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnNavigationPaneAdded(NuGenCollectionEventArgs<NuGenNavigationPane> e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokeEventHandlerT<NuGenCollectionEventArgs<NuGenNavigationPane>>(_navigationPaneAdded, e);
		}

		/*
		 * NavigationPaneRemoved
		 */

		private static readonly object _navigationPaneRemoved = new object();

		/// <summary>
		/// Occurs when a navigation pane is removed.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Action")]
		[NuGenSRDescription("Description_NavigationBar_NavigationPaneRemoved")]
		public event EventHandler<NuGenCollectionEventArgs<NuGenNavigationPane>> NavigationPaneRemoved
		{
			add
			{
				this.Events.AddHandler(_navigationPaneRemoved, value);
			}
			remove
			{
				this.Events.RemoveHandler(_navigationPaneRemoved, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenNavigationBar.NavigationPaneRemoved"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnNavigationPaneRemoved(NuGenCollectionEventArgs<NuGenNavigationPane> e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokeEventHandlerT<NuGenCollectionEventArgs<NuGenNavigationPane>>(_navigationPaneRemoved, e);
		}

		/*
		 * SelectedButtonChanged
		 */

		private static readonly object _selectedButtonChanged = new object();

		/// <summary>
		/// Occurs when a new button is selected.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_NavigationBar_SelectedButtonChanged")]
		public event EventHandler SelectedButtonChanged
		{
			add
			{
				this.Events.AddHandler(_selectedButtonChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_selectedButtonChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="E:Genetibase.Shared.Controls.NuGenNavigationBar.SelectedButtonChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnSelectedButtonChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_selectedButtonChanged, e);
		}

		#endregion

		#region Properties.NonBrowsable

		/*
		 * GripDistance
		 */

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public int GripDistance
		{
			get
			{
				return _buttonBlock.Height;
			}
			set
			{
				_buttonBlock.Height = value;
			}
		}

		/*
		 * NavigationPanes
		 */

		private NavigationPaneCollection _navigationPanes;

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public NavigationPaneCollection NavigationPanes
		{
			get
			{
				if (_navigationPanes == null)
				{
					_navigationPanes = new NavigationPaneCollection(this);
				}

				return _navigationPanes;
			}
		}

		/*
		 * SelectedNavigationPane
		 */

		/// <summary>
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public NuGenNavigationPane SelectedNavigationPane
		{
			get
			{
				Debug.Assert(_buttonPaneDictionary != null, "_buttonPaneDictionary != null");

				if (
					_buttonBlock.SelectedButton != null
					&& _buttonPaneDictionary.ContainsKey(_buttonBlock.SelectedButton)
					)
				{
					return _buttonPaneDictionary[_buttonBlock.SelectedButton];
				}

				return null;
			}
			set
			{
				if (value != null)
				{
					Debug.Assert(_paneButtonDictionary != null, "_paneButtonDictionary != null");

					if (!_paneButtonDictionary.ContainsKey(value))
					{
						throw new ArgumentException(
							string.Format(
								CultureInfo.InvariantCulture,
								Resources.Argument_InvalidNavigationPane,
								typeof(NuGenNavigationPane).Name,
								typeof(NuGenNavigationBar).Name
								)
							);
					}

					_buttonBlock.SelectedButton = _paneButtonDictionary[value];
				}
				else
				{
					_buttonBlock.SelectedButton = null;
				}
			}
		}

		#endregion

		#region Properties.Hidden

		/// <summary>
		/// Gets or sets the background image displayed in the control.
		/// </summary>
		/// <value></value>
		/// <returns>An <see cref="T:System.Drawing.Image"></see> that represents the image to display in the background of the control.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
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

		/// <summary>
		/// Gets or sets the background image layout as defined in the <see cref="T:System.Windows.Forms.ImageLayout"></see> enumeration.
		/// </summary>
		/// <value></value>
		/// <returns>One of the values of <see cref="T:System.Windows.Forms.ImageLayout"></see> (<see cref="F:System.Windows.Forms.ImageLayout.Center"></see> , <see cref="F:System.Windows.Forms.ImageLayout.None"></see>, <see cref="F:System.Windows.Forms.ImageLayout.Stretch"></see>, <see cref="F:System.Windows.Forms.ImageLayout.Tile"></see>, or <see cref="F:System.Windows.Forms.ImageLayout.Zoom"></see>). <see cref="F:System.Windows.Forms.ImageLayout.Tile"></see> is the default value.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The specified enumeration value does not exist. </exception>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
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

		/// <summary>
		/// Gets the collection of controls contained within the control.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Windows.Forms.Control.ControlCollection"></see> representing the collection of controls contained within the control.</returns>
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ControlCollection Controls
		{
			get
			{
				return base.Controls;
			}
		}

		#endregion

		#region Properties.Services

		/*
		 * Renderer
		 */

		private INuGenNavigationBarRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenNavigationBarRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_renderer = this.ServiceProvider.GetService<INuGenNavigationBarRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenNavigationBarRenderer>();
					}
				}

				return _renderer;
			}
		}

		#endregion

		#region Methods.Public

		/// <summary>
		/// </summary>
		/// <param name="pointToTest">Screen coordinates expected.</param>
		/// <returns></returns>
		public NuGenNavigationBarHitResult HitTest(Point pointToTest)
		{
			NuGenNavigationBarHitResult buttonBlockHitResults = _buttonBlock.HitTest(pointToTest);

			if (buttonBlockHitResults != NuGenNavigationBarHitResult.Nowhere)
			{
				return buttonBlockHitResults;
			}
			else
			{
				Point pt = this.PointToClient(pointToTest);

				if (this.ClientRectangle.Contains(pt))
				{
					return NuGenNavigationBarHitResult.Body;
				}
			}

			return NuGenNavigationBarHitResult.Nowhere;
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

			if (e.Control is NuGenNavigationPane)
			{
				NuGenNavigationPane navigationPane = e.Control as NuGenNavigationPane;
				int index = this.NavigationPanes.IndexOf(navigationPane);
				this.RemoveNavigationPane(navigationPane);

				if (this.NavigationPanes.ListInternal.Remove(navigationPane))
				{
					this.OnNavigationPaneRemoved(
						new NuGenCollectionEventArgs<NuGenNavigationPane>(index, navigationPane)
					);
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
			NuGenPaintParams paintParams = new NuGenPaintParams(e.Graphics);
			paintParams.Bounds = this.ClientRectangle;
			paintParams.State = this.StateTracker.GetControlState();
			this.Renderer.DrawBackground(paintParams);
			this.Renderer.DrawBorder(paintParams);
		}

		#endregion

		#region Methods.Private.NavigationButton

		/*
		 * InitializeNavigationButton
		 */

		private void InitializeNavigationButton(NuGenNavigationButton navigationButtonToInitialize)
		{
			Debug.Assert(navigationButtonToInitialize != null, "navigationButtonToInitialize != null");
			_buttonBlock.SelectedButton = navigationButtonToInitialize;
		}

		/*
		 * AddNavigationButton
		 */

		private void AddNavigationButton(NuGenNavigationButton navigationButtonToAdd)
		{
			Debug.Assert(navigationButtonToAdd != null, "navigationButtonToAdd != null");
			_buttonBlock.AddNavigationButton(navigationButtonToAdd);
			this.InitializeNavigationButton(navigationButtonToAdd);
		}

		/*
		 * InsertNavigationButton
		 */

		private void InsertNavigationButton(int index, NuGenNavigationButton navigationButtonToInsert)
		{
			Debug.Assert(navigationButtonToInsert != null, "navigationButtonToInsert != null");
			_buttonBlock.AddNavigationButton(navigationButtonToInsert);
			this.InitializeNavigationButton(navigationButtonToInsert);
		}

		/*
		 * RemoveNavigationButton
		 */

		private void RemoveNavigationButton(NuGenNavigationButton navigationButtonToRemove)
		{
			Debug.Assert(navigationButtonToRemove != null, "navigationButtonToRemove != null");
			_buttonBlock.RemoveNavigationButton(navigationButtonToRemove);
		}

		#endregion

		#region Methods.Private.NavigationPane

		/*
		 * InitializeNavigationPane
		 */

		private NuGenNavigationButton InitializeNavigationPane(NuGenNavigationPane navigationPaneToInitialize)
		{
			Debug.Assert(navigationPaneToInitialize != null, "navigationPaneToInitialize != null");
			NuGenNavigationButton navigationButtonToAssociate = new NuGenNavigationButton();
			navigationButtonToAssociate.Image = navigationPaneToInitialize.NavigationButtonImage;
			navigationButtonToAssociate.Text = navigationPaneToInitialize.Text;

			Debug.Assert(_buttonPaneDictionary != null, "_buttonPaneDictionary != null");
			Debug.Assert(_paneButtonDictionary != null, "_paneButtonDictionary != null");
			_buttonPaneDictionary.Add(navigationButtonToAssociate, navigationPaneToInitialize);
			_paneButtonDictionary.Add(navigationPaneToInitialize, navigationButtonToAssociate);

			navigationPaneToInitialize.NavigationButtonImageChanged += _navigationPane_NavigationButtonImageChanged;
			navigationPaneToInitialize.TextChanged += _navigationPane_TextChanged;

			this.Controls.Add(navigationPaneToInitialize);
			navigationPaneToInitialize.BringToFront();

			return navigationButtonToAssociate;
		}

		/*
		 * AddNavigationPane
		 */

		private void AddNavigationPane(NuGenNavigationPane navigationPaneToAdd)
		{
			Debug.Assert(navigationPaneToAdd != null, "navigationPaneToAdd != null");
			this.AddNavigationButton(this.InitializeNavigationPane(navigationPaneToAdd));
			this.OnNavigationPaneAdded(
				new NuGenCollectionEventArgs<NuGenNavigationPane>(this.NavigationPanes.Count - 1, navigationPaneToAdd)
			);
		}

		/*
		 * InsertNavigationPane
		 */

		private void InsertNavigationPane(int index, NuGenNavigationPane navigationPaneToInsert)
		{
			Debug.Assert(navigationPaneToInsert != null, "navigationPaneToInsert != null");
			this.InsertNavigationButton(index, this.InitializeNavigationPane(navigationPaneToInsert));
			this.OnNavigationPaneAdded(
				new NuGenCollectionEventArgs<NuGenNavigationPane>(index, navigationPaneToInsert)
			);
		}

		/*
		 * RemoveNavigationPane
		 */

		private void RemoveNavigationPane(NuGenNavigationPane navigationPaneToRemove)
		{
			Debug.Assert(navigationPaneToRemove != null, "navigationPaneToRemove != null");
			Debug.Assert(_buttonPaneDictionary != null, "_buttonPaneDictionary != null");
			Debug.Assert(_paneButtonDictionary != null, "_paneButtonDictionary != null");

			NuGenNavigationButton associatedButton = _paneButtonDictionary[navigationPaneToRemove];
			Debug.Assert(associatedButton != null, "associatedButton != null");

			navigationPaneToRemove.NavigationButtonImageChanged -= _navigationPane_NavigationButtonImageChanged;
			navigationPaneToRemove.TextChanged -= _navigationPane_TextChanged;

			_buttonPaneDictionary.Remove(associatedButton);
			_paneButtonDictionary.Remove(navigationPaneToRemove);

			this.RemoveNavigationButton(associatedButton);
		}

		#endregion

		#region EventHandlers.ButtonBlock

		private void _buttonBlock_SelectedButtonChanged(object sender, EventArgs e)
		{
			NuGenNavigationButton selectedButton = _buttonBlock.SelectedButton;
			_title.SetSelectedButton(selectedButton);

			if (selectedButton != null)
			{
				Debug.Assert(_buttonPaneDictionary != null, "_buttonPaneDictionary != null");
				Debug.Assert(_buttonPaneDictionary.ContainsKey(selectedButton), "_buttonPaneDictionary.ContainsKey(selectedButton)");
				NuGenNavigationPane activeNavigationPane = _buttonPaneDictionary[selectedButton];
				Debug.Assert(activeNavigationPane != null, "activeNavigationPane != null");
				activeNavigationPane.BringToFront();
			}

			this.OnSelectedButtonChanged(e);
		}

		#endregion

		#region EventHandlers.NavigationPane

		private void _navigationPane_NavigationButtonImageChanged(object sender, EventArgs e)
		{
			Debug.Assert(sender is NuGenNavigationPane, "sender is NuGenNavigationPane");
			NuGenNavigationPane navigationPane = (NuGenNavigationPane)sender;

			Debug.Assert(_paneButtonDictionary != null, "_paneButtonDictionary != null");
			Debug.Assert(_paneButtonDictionary.ContainsKey(navigationPane), "_paneButtonDictionary.ContainsKey(navigationPane)");
			_paneButtonDictionary[navigationPane].Image = navigationPane.NavigationButtonImage;
		}

		private void _navigationPane_TextChanged(object sender, EventArgs e)
		{
			Debug.Assert(sender is NuGenNavigationPane, "sender is NuGenNavigationPane");
			NuGenNavigationPane navigationPane = (NuGenNavigationPane)sender;

			Debug.Assert(_paneButtonDictionary != null, "_paneButtonDictionary != null");
			Debug.Assert(_paneButtonDictionary.ContainsKey(navigationPane), "_paneButtonDictionary.ContainsKey(navigationPane)");
			NuGenNavigationButton navigationButton = _paneButtonDictionary[navigationPane];
			navigationButton.Text = navigationPane.Text;

			if (navigationButton == _buttonBlock.SelectedButton)
			{
				_title.SetSelectedButton(navigationButton);
			}
		}

		#endregion

		private ButtonBlock _buttonBlock;
		private Title _title;

		private Dictionary<NuGenNavigationButton, NuGenNavigationPane> _buttonPaneDictionary;
		private Dictionary<NuGenNavigationPane, NuGenNavigationButton> _paneButtonDictionary;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenNavigationBar"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// <para>Requires:</para>
		/// <para><see cref="INuGenControlStateTracker"/></para>
		/// <para><see cref="INuGenNavigationBarRenderer"/></para>
		/// <para><see cref="INuGenNavigationBarLayoutManager"/></para>
		/// <para><see cref="INuGenTitleRenderer"/></para>
		/// <para><see cref="INuGenToolStripRenderer"/></para>
		/// <para><see cref="INuGenToolTipLayoutManager"/></para>
		/// <para><see cref="INuGenToolTipRenderer"/></para>
		/// <para><see cref="INuGenControlImageManager"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenNavigationBar(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			_buttonPaneDictionary = new Dictionary<NuGenNavigationButton, NuGenNavigationPane>();
			_paneButtonDictionary = new Dictionary<NuGenNavigationPane, NuGenNavigationButton>();

			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SuspendLayout();

			_buttonBlock = new ButtonBlock(serviceProvider);
			_buttonBlock.Dock = DockStyle.Bottom;
			_buttonBlock.Parent = this;
			_buttonBlock.SelectedButtonChanged += _buttonBlock_SelectedButtonChanged;

			_title = new Title(serviceProvider);
			_title.Dock = DockStyle.Top;
			_title.Parent = this;

			this.ResumeLayout(false);
		}

		/// <summary>
		/// </summary>
		/// <param name="disposing">
		/// <see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.
		/// </param>
		protected override void Dispose(bool disposing)
		{
			if (_buttonBlock != null)
			{
				_buttonBlock.SelectedButtonChanged -= _buttonBlock_SelectedButtonChanged;
				_buttonBlock.Dispose();
			}

			if (_title != null)
			{
				_title.Dispose();
			}

			base.Dispose(disposing);
		}
	}
}
