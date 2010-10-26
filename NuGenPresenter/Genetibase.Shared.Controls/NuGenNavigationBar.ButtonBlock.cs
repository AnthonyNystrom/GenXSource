/* -----------------------------------------------
 * NuGenNavigationBar.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.Collections;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.NavigationBarInternals;
using Genetibase.Shared.Controls.Properties;
using Genetibase.Shared.Controls.ToolStripInternals;
using Genetibase.Shared.Controls.ToolTipInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	partial class NuGenNavigationBar
	{
		private sealed class ButtonBlock : NuGenControl
		{
			#region Events

			/*
			 * NavigationButtonAdded
			 */

			private static readonly object _navigationButtonAdded = new object();

			public event EventHandler<NuGenCollectionEventArgs<NuGenNavigationButton>> NavigationButtonAdded
			{
				add
				{
					this.Events.AddHandler(_navigationButtonAdded, value);
				}
				remove
				{
					this.Events.RemoveHandler(_navigationButtonAdded, value);
				}
			}

			internal void OnNavigationButtonAdded(NuGenCollectionEventArgs<NuGenNavigationButton> e)
			{
				this.Invalidate();
				this.Initiator.InvokeEventHandlerT<NuGenCollectionEventArgs<NuGenNavigationButton>>(_navigationButtonAdded, e);
			}

			/*
			 * NavigationButtonRemoved
			 */

			private static readonly object _navigationButtonRemoved = new object();

			public event EventHandler<NuGenCollectionEventArgs<NuGenNavigationButton>> NavigationButtonRemoved
			{
				add
				{
					this.Events.AddHandler(_navigationButtonRemoved, value);
				}
				remove
				{
					this.Events.RemoveHandler(_navigationButtonRemoved, value);
				}
			}

			internal void OnNavigationButtonRemoved(NuGenCollectionEventArgs<NuGenNavigationButton> e)
			{
				this.Invalidate();
				this.Initiator.InvokeEventHandlerT<NuGenCollectionEventArgs<NuGenNavigationButton>>(_navigationButtonRemoved, e);
			}

			#endregion

			#region Properties.Public

			/*
			 * SelectedButton
			 */

			private NuGenNavigationButton _selectedButton;

			/// <summary>
			/// </summary>
			[Browsable(false)]
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			public NuGenNavigationButton SelectedButton
			{
				get
				{
					return _selectedButton;
				}
				set
				{
					if (_selectedButton != value)
					{
						_selectedButton = value;
						this.Invalidate();
						this.OnSelectedButtonChanged(EventArgs.Empty);
					}
				}
			}

			private static readonly object _selectedButtonChanged = new object();

			/// <summary>
			/// </summary>
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
			/// Will bubble the <see cref="SelectedButtonChanged"/> event.
			/// </summary>
			/// <param name="e"></param>
			private void OnSelectedButtonChanged(EventArgs e)
			{
				this.Initiator.InvokeEventHandler(_selectedButtonChanged, e);
			}

			#endregion

			#region Properties.Services

			/*
			 * LayoutManager
			 */

			private INuGenNavigationBarLayoutManager _layoutManager;

			private INuGenNavigationBarLayoutManager LayoutManager
			{
				get
				{
					if (_layoutManager == null)
					{
						Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
						_layoutManager = this.ServiceProvider.GetService<INuGenNavigationBarLayoutManager>();

						if (_layoutManager == null)
						{
							throw new NuGenServiceNotFoundException<INuGenNavigationBarLayoutManager>();
						}
					}

					return _layoutManager;
				}
			}

			/*
			 * Renderer
			 */

			private INuGenNavigationBarRenderer _renderer;

			/// <summary>
			/// </summary>
			/// <exception cref="NuGenServiceNotFoundException"/>
			private INuGenNavigationBarRenderer Renderer
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

			public void AddNavigationButton(NuGenNavigationButton navigationButtonToAdd)
			{
				Debug.Assert(navigationButtonToAdd != null, "navigationButtonToAdd != null");
				
				navigationButtonToAdd.AllowedChanged += _navigationButton_Refresh;
				navigationButtonToAdd.ImageChanged += _navigationButton_Refresh;
				navigationButtonToAdd.TextChanged += _navigationButton_Refresh;
				navigationButtonToAdd.VisibleChanged += _navigationButton_Refresh;
				
				_buttons.Add(navigationButtonToAdd);
			}

			public void RemoveNavigationButton(NuGenNavigationButton navigationButtonToRemove)
			{
				Debug.Assert(navigationButtonToRemove != null, "navigationButtonToRemove != null");
				
				navigationButtonToRemove.AllowedChanged -= _navigationButton_Refresh;
				navigationButtonToRemove.ImageChanged -= _navigationButton_Refresh;
				navigationButtonToRemove.TextChanged -= _navigationButton_Refresh;
				navigationButtonToRemove.VisibleChanged -= _navigationButton_Refresh;

				int selectedIndex = _buttons.IndexOf(navigationButtonToRemove);
				_buttons.Remove(navigationButtonToRemove);

				if (navigationButtonToRemove == this.SelectedButton)
				{
					if (_buttons.Count < 1)
					{
						this.SelectedButton = null;
						return;
					}

					if (selectedIndex >= _buttons.Count)
					{
						selectedIndex = _buttons.Count - 1;
					}

					this.SelectedButton = _buttons[selectedIndex];
				}
			}
			
			/// <summary>
			/// </summary>
			/// <param name="pointToTest">Screen coordinates expected.</param>
			public NuGenNavigationBarHitResult HitTest(Point pointToTest)
			{
				Point pt = this.PointToClient(pointToTest);

				if (this.ClientRectangle.Contains(pt))
				{
					if (this.GetGripRectangle().Contains(pt))
					{
						return NuGenNavigationBarHitResult.Grip;
					}

					NuGenNavigationButton button = _buttons[pt];

					if (button != null)
					{
						return NuGenNavigationBarHitResult.Buttons;
					}

					return NuGenNavigationBarHitResult.Body;
				}

				return NuGenNavigationBarHitResult.Nowhere;
			}

			#endregion

			#region Methods.Protected.Overridden

			/*
			 * OnMouseClick
			 */

			/// <summary>
			/// Raises the <see cref="E:System.Windows.Forms.Control.MouseClick"></see> event.
			/// </summary>
			/// <param name="e">An <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
			protected override void OnMouseClick(MouseEventArgs e)
			{
				base.OnMouseClick(e);

				_rightClickedButton = null;
				NuGenNavigationButton currentButton = _buttons[e.Location];

				if (currentButton != null)
				{
					if (e.Button == MouseButtons.Left)
					{
						this.SelectedButton = currentButton;
					}
					else if (e.Button == MouseButtons.Right)
					{
						_rightClickedButton = currentButton;
						this.Invalidate();
					}
				}
				else
				{
					if (this.GetDropDownRectangle().Contains(e.Location))
					{
						this.CreateContextMenu();
					}
				}
			}

			/*
			 * OnMouseDown
			 */

			/// <summary>
			/// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown"></see> event.
			/// </summary>
			/// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
			protected override void OnMouseDown(MouseEventArgs e)
			{
				base.OnMouseDown(e);
				_isResizing = this.GetGripRectangle().Contains(e.Location);
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
				base.OnMouseLeave(e);

				if (_rightClickedButton == null)
				{
					_hoveringButton = null;
					_dropDownHovering = false;
					this.Invalidate();
				}

				_tooltip.Hide();
			}

			/*
			 * OnMouseMove
			 */

			/// <summary>
			/// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove"></see> event.
			/// </summary>
			/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
			protected override void OnMouseMove(MouseEventArgs e)
			{
				base.OnMouseMove(e);

				_hoveringButton = null;
				_dropDownHovering = false;

				int buttonHeight = this.GetButtonHeight();

				if (_isResizing)
				{
					if (e.Y < -buttonHeight && _canGrow)
					{
						this.Height += buttonHeight;
					}
					else if (e.Y > buttonHeight && _canShrink)
					{
						this.Height -= buttonHeight;
					}
				}
				else
				{
					NuGenNavigationButton buttonUnderCursor = null;
					NuGenToolTipInfo tooltipInfo = null;

					if (this.GetGripRectangle().Contains(e.Location))
					{
						this.Cursor = Cursors.SizeNS;
					}
					else if (this.GetDropDownRectangle().Contains(e.Location))
					{
						this.Cursor = Cursors.Hand;
						_dropDownHovering = true;
						this.Invalidate();

						tooltipInfo = new NuGenToolTipInfo(null, null, Resources.Text_NavigationBar_ConfigureButtons);
					}
					else if ((buttonUnderCursor = _buttons[e.Location]) != null)
					{
						this.Cursor = Cursors.Hand;
						_hoveringButton = buttonUnderCursor;
						this.Invalidate();

						if (!buttonUnderCursor.IsLarge)
						{
							tooltipInfo = new NuGenToolTipInfo(null, null, buttonUnderCursor.Text);
						}
					}
					else
					{
						this.Cursor = Cursors.Default;
					}

					if (tooltipInfo != null)
					{
						_tooltip.Show(this, tooltipInfo);
					}
					else
					{
						_tooltip.Hide();
					}
				}
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
				_isResizing = false;
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
				int bottomContainerHeight = this.GetBottomContainerRectangle().Height;
				int buttonHeight = this.GetButtonHeight();
				int gripHeight = this.GetGripRectangle().Height;

				_maxLargeButtonCount = (int)Math.Floor(
					(double)((this.Height - bottomContainerHeight - gripHeight) / buttonHeight)
				);

				int visibleCount = _buttons.GetVisibleCount();

				if (visibleCount < _maxLargeButtonCount)
				{
					_maxLargeButtonCount = visibleCount;
				}

				_canShrink = _maxLargeButtonCount != 0;
				_canGrow = _maxLargeButtonCount < visibleCount;

				this.Height = _maxLargeButtonCount * buttonHeight + gripHeight + bottomContainerHeight;

				int bottomContainerLeftMargin = this.GetBottomContainerLeftMargin();
				int dropDownWidth = this.GetDropDownRectangle().Width;
				int smallButtonWidth = this.GetSmallButtonWidth();

				_maxSmallButtonCount = (int)Math.Floor(
					(double)((this.Width - dropDownWidth - bottomContainerLeftMargin) / smallButtonWidth)
				);

				if (visibleCount - _maxLargeButtonCount <= 0)
				{
					_maxSmallButtonCount = 0;
				}

				if (_maxSmallButtonCount > (visibleCount - _maxLargeButtonCount))
				{
					_maxSmallButtonCount = visibleCount - _maxLargeButtonCount;
				}

				Graphics g = e.Graphics;
				Rectangle bounds = this.ClientRectangle;
				NuGenControlState currentState = this.StateTracker.GetControlState();
				NuGenPaintParams paintParams = new NuGenPaintParams(g);
				paintParams.State = currentState;

				/* Border */

				paintParams.Bounds = bounds;
				this.Renderer.DrawBorder(paintParams);

				/* Grip */

				paintParams.Bounds = this.GetGripRectangle();
				this.Renderer.DrawGrip(paintParams);

				/* Large buttons */

				int syncLargeButtons = 0;
				int iterateLargeButtons;

				for (iterateLargeButtons = 0; iterateLargeButtons < _buttons.Count; iterateLargeButtons++)
				{
					NuGenNavigationButton button = _buttons[iterateLargeButtons];

					if (button.Visible)
					{
						Rectangle rect = new Rectangle(0, syncLargeButtons * this.GetButtonHeight() + this.GetGripHeight(), this.ClientRectangle.Width, this.GetButtonHeight());
						button.Bounds = rect;
						button.IsLarge = true;
						this.DrawButton(g, button);

						if (syncLargeButtons == _maxLargeButtonCount)
						{
							break;
						}

						syncLargeButtons++;
					}
				}

				/* Bottom container */

				paintParams.Bounds = this.GetBottomContainerRectangle();
				this.Renderer.DrawBackground(paintParams);

				/* Small buttons */

				int startX = this.ClientRectangle.Width - this.GetDropDownRectangle().Width - _maxSmallButtonCount * this.GetSmallButtonWidth();
				int syncSmallButtons = 0;
				int iterateSmallButtons;

				for (iterateSmallButtons = iterateLargeButtons; iterateSmallButtons < _buttons.Count; iterateSmallButtons++)
				{
					if (syncSmallButtons == _maxSmallButtonCount)
					{
						break;
					}

					NuGenNavigationButton button = _buttons[iterateSmallButtons];

					if (button.Visible)
					{
						Rectangle rect = new Rectangle(startX + (syncSmallButtons * this.GetSmallButtonWidth()), this.GetBottomContainerRectangle().Y, this.GetSmallButtonWidth(), this.GetBottomContainerRectangle().Height);
						button.Bounds = rect;
						button.IsLarge = false;
						this.DrawButton(g, button);
						syncSmallButtons++;
					}
				}

				for (int i = iterateSmallButtons; i < _buttons.Count; i++)
				{
					_buttons[i].Bounds = Rectangle.Empty;
				}

				/* DropDown */

				this.DrawDropDown(g);

				/* Bottom container border */

				this.Renderer.DrawBorder(paintParams);
			}

			#endregion

			#region Methods.Drawing

			/*
			 * DrawButton
			 */

			private void DrawButton(Graphics g, NuGenNavigationButton button)
			{
				Debug.Assert(g != null, "g != null");
				Debug.Assert(button != null, "button != null");

				NuGenControlState buttonState = this.StateTracker.GetControlState();

				if (button == _hoveringButton)
				{
					buttonState = button == _selectedButton ? NuGenControlState.Pressed : NuGenControlState.Hot;
				}
				else
				{
					if (button == _selectedButton)
					{
						buttonState = NuGenControlState.Pressed;
					}
				}

				NuGenPaintParams paintParams = new NuGenPaintParams(g);
				paintParams.Bounds = button.Bounds;
				paintParams.State = buttonState;

				this.Renderer.DrawBackground(paintParams);

				if (button.IsLarge)
				{
					NuGenItemPaintParams itemPaintParams = new NuGenItemPaintParams(paintParams);

					itemPaintParams.ContentAlign = ContentAlignment.MiddleLeft;
					itemPaintParams.Font = this.Font;
					itemPaintParams.ForeColor = this.ForeColor;
					itemPaintParams.Image = button.Image;
					itemPaintParams.Text = button.Text;

					this.Renderer.DrawLargeButtonBody(itemPaintParams);
					this.Renderer.DrawButtonBorder(paintParams);
				}
				else
				{
					NuGenImagePaintParams imagePaintParams = new NuGenImagePaintParams(paintParams);
					imagePaintParams.Image = button.Image;

					this.Renderer.DrawSmallButtonBody(imagePaintParams);
				}
			}

			/*
			 * DrawDropDown
			 */

			private void DrawDropDown(Graphics g)
			{
				Debug.Assert(g != null, "g != null");
				NuGenControlState state = this.StateTracker.GetControlState();

				if (_dropDownHovering)
				{
					state = NuGenControlState.Hot;
				}

				NuGenPaintParams paintParams = new NuGenPaintParams(g);
				paintParams.Bounds = this.GetDropDownRectangle();
				paintParams.State = state;

				this.Renderer.DrawBackground(paintParams);
				this.Renderer.DrawDropDownArrow(paintParams);
			}

			#endregion

			#region Methods.Layout

			/*
			 * GetBottomContainerLeftMargin
			 */

			private int GetBottomContainerLeftMargin()
			{
				return this.LayoutManager.GetBottomContainerLeftMargin();
			}

			/*
			 * GetBottomContainerRectangle
			 */

			private Rectangle GetBottomContainerRectangle()
			{
				int buttonHeight = this.GetButtonHeight();

				return new Rectangle(
					0,
					this.ClientRectangle.Bottom - buttonHeight,
					this.ClientRectangle.Width,
					buttonHeight
				);
			}

			/*
			 * GetButtonHeight
			 */

			private int GetButtonHeight()
			{
				return this.LayoutManager.GetButtonHeight();
			}

			/*
			 * GetDropDownRectangle
			 */

			private Rectangle GetDropDownRectangle()
			{
				int buttonHeight = this.GetButtonHeight();
				int smallButtonWidth = this.GetSmallButtonWidth();

				return new Rectangle(
					this.ClientRectangle.Width - smallButtonWidth,
					this.ClientRectangle.Height - buttonHeight,
					smallButtonWidth,
					buttonHeight
				);
			}

			/*
			 * GetGripHeight
			 */

			private int GetGripHeight()
			{
				return this.LayoutManager.GetGripHeight();
			}

			/*
			 * GetGripRectangle
			 */

			private Rectangle GetGripRectangle()
			{
				return new Rectangle(
					this.ClientRectangle.Left,
					this.ClientRectangle.Top,
					this.ClientRectangle.Width,
					this.GetGripHeight()
				);
			}

			/*
			 * GetSmallButtonWidth
			 */

			private int GetSmallButtonWidth()
			{
				return this.LayoutManager.GetSmallButtonWidth();
			}

			#endregion

			#region Methods.ContextMenu

			private void CreateContextMenu()
			{
				_contextMenuStrip.Items.Clear();
				_addRemoveButtons.DropDownItems.Clear();
				_contextMenuStrip.Items.Add(_addRemoveButtons);
				bool overflowSeparatorAdded = false;

				foreach (NuGenNavigationButton navigationButton in _buttons)
				{
					if (navigationButton.Allowed)
					{
						ToolStripMenuItem addRemoveButtonMenuItem = new ToolStripMenuItem(
							navigationButton.Text,
							navigationButton.Image,
							_addRemoveButtonsMenuItem_Click
						);

						addRemoveButtonMenuItem.CheckOnClick = true;
						addRemoveButtonMenuItem.Checked = navigationButton.Visible;
						addRemoveButtonMenuItem.Tag = navigationButton;

						_addRemoveButtons.DropDownItems.Add(addRemoveButtonMenuItem);

						if (navigationButton.Bounds == Rectangle.Empty)
						{
							if (!overflowSeparatorAdded)
							{
								_contextMenuStrip.Items.Add(_overflowSeparator);
								overflowSeparatorAdded = true;
							}

							ToolStripMenuItem overflowMenuItem = new ToolStripMenuItem(
								navigationButton.Text,
								navigationButton.Image,
								_overflowMenuItem_Click
							);

							overflowMenuItem.CheckOnClick = true;
							overflowMenuItem.Checked = _selectedButton == navigationButton;
							overflowMenuItem.Tag = navigationButton;

							_contextMenuStrip.Items.Add(overflowMenuItem);
						}
					}
				}

				_contextMenuStrip.Show(this, new Point(this.Width, this.Height - this.GetButtonHeight() / 2));
			}

			private void _addRemoveButtonsMenuItem_Click(object sender, EventArgs e)
			{
				NuGenNavigationButton navigationButton = (NuGenNavigationButton)((ToolStripMenuItem)sender).Tag;
				navigationButton.Visible = !navigationButton.Visible;
				this.Invalidate();
			}

			private void _overflowMenuItem_Click(object sender, EventArgs e)
			{
				this.SelectedButton = (NuGenNavigationButton)((ToolStripMenuItem)sender).Tag;
			}

			#endregion

			#region EventHandlers.NavigationButton

			private void _navigationButton_Refresh(object sender, EventArgs e)
			{
				this.Invalidate();
			}

			#endregion

			private ButtonCollection _buttons;

			private NuGenNavigationButton _hoveringButton;
			private NuGenNavigationButton _rightClickedButton;

			private bool _canGrow;
			private bool _canShrink;
			private bool _dropDownHovering;
			private bool _isResizing;

			private int _maxLargeButtonCount;
			private int _maxSmallButtonCount;

			private NuGenContextMenuStrip _contextMenuStrip;
			private NuGenToolTip _tooltip;
			private ToolStripMenuItem _addRemoveButtons;
			private ToolStripSeparator _overflowSeparator;

			/// <summary>
			/// Initializes a new instance of the <see cref="NuGenNavigationBar"/> class.
			/// </summary>
			/// <param name="serviceProvider">
			/// <para>Requires:</para>
			/// <para><see cref="INuGenControlStateTracker"/></para>
			/// <para><see cref="INuGenNavigationBarRenderer"/></para>
			/// <para><see cref="INuGenNavigationBarLayoutManager"/></para>
			/// <para><see cref="INuGenToolStripRenderer"/></para>
			/// <para><see cref="INuGenToolTipLayoutManager"/></para>
			/// <para><see cref="INuGenToolTipRenderer"/></para>
			/// </param>
			/// <exception cref="ArgumentNullException">
			/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
			/// </exception>
			public ButtonBlock(INuGenServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				_buttons = new ButtonCollection(this);

				_contextMenuStrip = new NuGenContextMenuStrip(serviceProvider);
				_addRemoveButtons = new ToolStripMenuItem(Resources.Text_NavigationBar_AddRemoveButtons);
				_contextMenuStrip.Items.Add(_addRemoveButtons);
				_overflowSeparator = new ToolStripSeparator();
				_tooltip = new NuGenToolTip(serviceProvider);

				this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
				this.SetStyle(ControlStyles.Opaque, true);
				this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
				this.SetStyle(ControlStyles.ResizeRedraw, true);
			}
		}
	}
}
