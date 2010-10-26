/* -----------------------------------------------
 * NuGenPinpointWindow.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using res = Genetibase.Shared.Controls.Properties.Resources;

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.PinpointInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Collections.Specialized;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	[ToolboxItem(false)]
	[DefaultEvent("SelectedIndexChanged")]
	[DefaultProperty("Items")]
	[Designer("Genetibase.Shared.Controls.Design.NuGenPinpointWindowDesigner")]
	[NuGenSRDescription("Description_PinpointWindow")]
	public partial class NuGenPinpointWindow : NuGenControl
	{
		#region Properties.Appearance

		/*
		 * DesiredFocusLength
		 */

		private int _desiredFocusLength;

		/// <summary>
		/// Gets or sets the preferred fish eye height.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(11)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_Pinpoint_DesiredFocusLength")]
		public int DesiredFocusLength
		{
			get
			{
				return _desiredFocusLength;
			}
			set
			{
				if (_desiredFocusLength != value)
				{
					_desiredFocusLength = value;
					this.CalculateSizes();
				}
			}
		}

		#endregion

		#region Properties.Data

		/*
		 * Items
		 */

		private ObjectCollection _items;

		/// <summary>
		/// Gets a list of items contained within this <see cref="NuGenPinpointWindow"/>.
		/// </summary>
		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor("Genetibase.Shared.Controls.Design.NuGenPinpointListEditor", typeof(UITypeEditor))]
		[MergableProperty(false)]
		[NuGenSRCategory("Category_Data")]
		[NuGenSRDescription("Description_Pinpoint_Items")]
		public ObjectCollection Items
		{
			get
			{
				if (_items == null)
				{
					_items = new ObjectCollection(this);
				}

				return _items;
			}
		}

		#endregion

		#region Properties.NonBrowsable

		/*
		 * SelectedIndex
		 */

		private int _selectedIndex;

		/// <summary>
		/// Gets or sets the index of the currently selected item.
		/// </summary>
		/// <exception cref="IndexOutOfRangeException">
		/// <para><see cref="SelectedIndex"/> is a zero-based index that should not exceed <see cref="P:Genetibase.Shared.Controls.NuGenPinpointWindow.Items.Count"/>.</para>
		/// </exception>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int SelectedIndex
		{
			get
			{
				return _selectedIndex;
			}
			set
			{
				if (_selectedIndex != value)
				{
					if (value >= this.Items.Count)
					{
						throw new IndexOutOfRangeException(res.IndexOutOfRange_SelectedIndex);
					}

					_selectedIndex = value;
					this.OnSelectedIndexChanged(EventArgs.Empty);
					this.Invalidate();
				}
			}
		}

		private static readonly object _selectedIndexChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="SelectedIndex"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_Pinpoint_SelectedIndexChanged")]
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
		/// Raises the <see cref="E:Genetibase.Shared.Controls.NuGenPinpointWindow.SelectedIndexChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnSelectedIndexChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_selectedIndexChanged, e);
		}

		/*
		 * SelectedItem
		 */

		/// <summary>
		/// Gets or sets the currently selected item.
		/// </summary>
		/// <exception cref="ArgumentException">
		/// The assigned value should be contained within <see cref="Items"/>.
		/// </exception>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public object SelectedItem
		{
			get
			{
				return this.Items[_selectedIndex];
			}
			set
			{
				int index = this.Items.IndexOf(value);

				if (index > -1)
				{
					this.SelectedIndex = index;
				}
				else
				{
					throw new ArgumentException(res.Argument_SelectedItem);
				}
			}
		}

		#endregion

		#region Properties.Public.Overridden

		/// <summary>
		/// Indicates whether the size of the control is calculated automatically according to its content.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(false)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_Pinpoint_AutoSize")]
		public override bool AutoSize
		{
			get
			{
				return base.AutoSize;
			}
			set
			{
				base.AutoSize = value;
				this.Refresh();
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

		private new Color DefaultBackColor
		{
			get
			{
				return Color.Transparent;
			}
		}

		private new void ResetBackColor()
		{
			this.BackColor = this.DefaultBackColor;
		}

		private bool ShouldSerializeBackColor()
		{
			return this.BackColor != this.DefaultBackColor;
		}

		/*
		 * Font
		 */

		private Font _font;

		/// <summary>
		/// Gets or sets the font of the text displayed by the control.
		/// </summary>
		/// <value></value>
		/// <returns>The <see cref="T:System.Drawing.Font"></see> to apply to the text displayed by the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultFont"></see> property.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		public override Font Font
		{
			get
			{
				if (_font == null)
				{
					return this.DefaultFont;
				}

				return _font;
			}
			set
			{
				if (_font != value)
				{
					_font = value;
					this.OnFontChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly Font _defaultFont = new Font("Verdana", 12, FontStyle.Regular);

		private new Font DefaultFont
		{
			get
			{
				return _defaultFont;
			}
		}

		private new void ResetFont()
		{
			this.Font = this.DefaultFont;
		}

		private bool ShouldSerializeFont()
		{
			return this.Font != this.DefaultFont;
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.FontChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.DesiredMaxFontSize = (int)this.Font.Size;
			this.Refresh();
		}

		/*
		 * MinimumSize
		 */

		/// <summary>
		/// Gets or sets the size that is the lower limit that <see cref="M:System.Windows.Forms.Control.GetPreferredSize(System.Drawing.Size)"></see> can specify.
		/// </summary>
		/// <value></value>
		/// <returns>An ordered pair of type <see cref="T:System.Drawing.Size"></see> representing the width and height of a rectangle.</returns>
		public override Size MinimumSize
		{
			get
			{
				return base.MinimumSize;
			}
			set
			{
				base.MinimumSize = value;
			}
		}

		private static readonly Size _defaultMinimumSize = new Size(10, 10);

		private new Size DefaultMinimumSize
		{
			get
			{
				return _defaultMinimumSize;
			}
		}

		private void ResetMinimumSize()
		{
			this.MinimumSize = this.DefaultSize;
		}

		private bool ShouldSerializeMinimumSize()
		{
			return this.MinimumSize != this.DefaultSize;
		}

		#endregion

		#region Properties.Private

		private int _desiredMaxFontSize;

		private int DesiredMaxFontSize
		{
			get
			{
				return _desiredMaxFontSize;
			}
			set
			{
				Font[] newFonts = new Font[value];
				Font[] newBoldFonts = new Font[value];

				this.CreateRegularFontCollection(value).CopyTo(newFonts, 0);
				this.CreateBoldFontCollection(value).CopyTo(newBoldFonts, 0);

				_fonts = newFonts;
				_boldFonts = newBoldFonts;

				_desiredMaxFontSize = value;
				_desiredSpace = (int)(value * _desiredSpacing);
				_borderLeft = this.GetBorderLeft(value);

				this.Invalidate();
			}
		}

		#endregion

		#region Properties.Services

		private INuGenPinpointLayoutManager _layoutManager;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenPinpointLayoutManager LayoutManager
		{
			get
			{
				if (_layoutManager == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_layoutManager = this.ServiceProvider.GetService<INuGenPinpointLayoutManager>();

					if (_layoutManager == null)
					{
						throw new NuGenServiceNotFoundException<INuGenPinpointLayoutManager>();
					}
				}

				return _layoutManager;
			}
		}

		private INuGenPinpointRenderer _renderer;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenPinpointRenderer Renderer
		{
			get
			{
				if (_renderer == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_renderer = this.ServiceProvider.GetService<INuGenPinpointRenderer>();

					if (_renderer == null)
					{
						throw new NuGenServiceNotFoundException<INuGenPinpointRenderer>();
					}
				}

				return _renderer;
			}
		}

		#endregion

		#region Methods.Public.Virtual

		/// <summary>
		/// Returns the text representation of the specified item.
		/// </summary>
		/// <param name="item"></param>
		/// <returns>
		/// If the specified <paramref name="item"/> is <see langword="null"/>, an empty string is returned.
		/// Otherwise, the value of the item's ToString method is returned.
		/// </returns>
		public virtual string GetItemText(object item)
		{
			if (item == null)
			{
				return string.Empty;
			}

			return item.ToString();
		}

		#endregion

		#region Methods.Public.Overridden

		/// <summary>
		/// Forces the control to invalidate its client area and immediately redraw itself and any child controls.
		/// </summary>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		public override void Refresh()
		{
			if (this.Visible)
			{
				_focusIndex = 0;
				this.SelectedIndex = 0;
				_mouseY = 0;
				_focusLock = false;
				this.CalculateSizes();
			}

			base.Refresh();
		}

		#endregion

		#region Methods.Protected.Overridden

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseMove"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"></see> that contains the event data.</param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			if (e.X < this.Width / 2 || _noFishEye)
			{
				if (_focusLock)
				{
					this.Invalidate();
				}

				_focusLock = false;
			}
			else
			{
				if (!_focusLock)
				{
					// Just entered focus lock mode, so update look up tables, and calculate focus item position.
					_flFocusPosition = _borderY;

					for (int i = 0; i < this.Items.Count; i++)
					{
						_flSizeLUT[i] = _sizeLUT[_focusIndex][i];
						_flSpaceLUT[i] = _spaceLUT[_focusIndex][i];

						if (i < _focusIndex)
						{
							_flFocusPosition += _flSizeLUT[i] + _flSpaceLUT[i];
						}
					}
				}

				_focusLock = true;
			}

			_mouseY = e.Y;

			if (_focusLock)
			{
				// FOCUS LOCK mode
				if (_mouseY > _flFocusPosition)
				{
					int i;
					int y = _flFocusPosition;
					this.SelectedIndex = _focusIndex;

					// First, set the items between the focus and the cursor to full size
					for (i = _focusIndex; i < this.Items.Count; i++)
					{
						if (_flSizeLUT[i] < _maxFontSize)
						{
							_flSizeLUT[i] = _maxFontSize;
							_flSpaceLUT[i] = _desiredSpace;
						}

						y += _flSizeLUT[i] + _flSpaceLUT[i];
						this.SelectedIndex = i;

						if (_mouseY < y)
						{
							break;
						}
					}

					i++;

					// Then, add the fisheye size decrease
					if ((i < this.Items.Count) && (_flSizeLUT[i] < _maxFontSize))
					{
						bool done = false;

						while ((i < this.Items.Count) && !done)
						{
							if (_flSizeLUT[i] == _minFontSize)
							{
								done = true;
							}

							if (_flSizeLUT[i] < _maxFontSize)
							{
								_flSizeLUT[i]++;
								if (_flSizeLUT[i] == _maxFontSize)
								{
									_flSpaceLUT[i] = _desiredSpace;
								}
							}

							i++;
						}
					}
				}
				else if (_focusIndex > 0)
				{
					int i;
					int y = _flFocusPosition - _flSizeLUT[_focusIndex - 1] - _flSpaceLUT[_focusIndex - 1];
					this.SelectedIndex = _focusIndex;

					for (i = _focusIndex - 1; i >= 0; i--)
					{
						if (_flSizeLUT[i] < _maxFontSize)
						{
							_flSizeLUT[i] = _maxFontSize;
							_flSpaceLUT[i] = _desiredSpace;
						}

						this.SelectedIndex = i;

						if (_mouseY > y)
						{
							break;
						}

						y -= _flSizeLUT[i] + _flSpaceLUT[i];
					}

					i--;

					if ((i >= 0) && (_flSizeLUT[i] < _maxFontSize))
					{
						bool done = false;

						while ((i >= 0) && !done)
						{
							if (_flSizeLUT[i] == _minFontSize)
							{
								done = true;
							}

							if (_flSizeLUT[i] < _maxFontSize)
							{
								_flSizeLUT[i]++;

								if (_flSizeLUT[i] == _maxFontSize)
								{
									_flSpaceLUT[i] = _desiredSpace;
								}
							}

							i--;
						}
					}
				}

				this.Invalidate();
			}
			else
			{
				// Not FOCUS LOCK mode
				// Calculate index of focus item based on pointer position
				int prevFocusIndex = _focusIndex;
				_focusIndex = _focusIndexLUT[_mouseY];
				this.SelectedIndex = _focusIndex;
				this.Invalidate();
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"></see> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			if (!_shouldRender)
			{
				return;
			}

			int i;
			int x;
			int y;
			int space;
			int size;
			int width = this.Width;
			int height = this.Height;

			Graphics g = e.Graphics;
			g.TextRenderingHint = TextRenderingHint.AntiAlias;

			/*
			 * Fisheye
			 */

			y = _borderY;
			int y1 = 0;
			int y2 = height - _borderY;
			bool start;
			size = 0;

			if (_focusLock)
			{
				start = true;
				y = _flFocusPosition;
				for (i = _focusIndex - 1; i >= 0; i--)
				{
					size = _flSizeLUT[i];
					space = _flSpaceLUT[i];

					if (size == _minFontSize)
					{
						y1 = y + size + space;
						break;
					}

					y -= size + space;
				}

				y = _flFocusPosition;

				for (i = _focusIndex; i < this.Items.Count; i++)
				{
					size = _flSizeLUT[i];
					space = _flSpaceLUT[i];

					if (size == _minFontSize)
					{
						y2 = y;
						break;
					}

					y += size + space;
				}

				if (y1 < _borderY)
				{
					y1 = _borderY;
				}
			}
			else
			{
				start = true;

				for (i = 0; i < this.Items.Count; i++)
				{
					size = _sizeLUT[_focusIndex][i];
					space = _spaceLUT[_focusIndex][i];

					if (start)
					{
						if (size > _minFontSize)
						{
							y1 = y;
							start = false;
						}
					}
					else
					{
						if (size == _minFontSize)
						{
							y2 = y - size;
							break;
						}
					}

					y += size + space;
				}
			}

			if (this.Items.Count > 0 && !_noFishEye)
			{
				int expanderLeft = width / 2;
				int expanderTop = y1;
				int expanderWidth = width / 2 - _borderRight;
				int expanderHeight = y2 - y1;

				if (expanderWidth > 0 && expanderHeight > 0)
				{
					NuGenPaintParams expanderPaintParams = new NuGenPaintParams(g);
					expanderPaintParams.Bounds = new Rectangle(
						expanderLeft
						, expanderTop
						, expanderWidth
						, expanderHeight
						);

					if (_focusLock)
					{
						expanderPaintParams.State = NuGenControlState.Hot;
					}
					else
					{
						expanderPaintParams.State = NuGenControlState.Normal;
					}

					this.Renderer.DrawFisheyeExpander(expanderPaintParams);
				}
			}

			/*
			 * Items
			 */

			x = _borderLeft;
			y = _borderY;

			NuGenTextPaintParams itemPaintParams = new NuGenTextPaintParams(g);
			itemPaintParams.ForeColor = this.ForeColor;
			itemPaintParams.State = this.StateTracker.GetControlState();
			itemPaintParams.TextAlign = ContentAlignment.MiddleLeft;

			List<string> sortedItems = this.GetSortedItems(this.Items);
			Debug.Assert(sortedItems != null, "sortedItems != null");

			if (!_focusLock /* Normal mode. */)
			{
				for (i = 0; i < sortedItems.Count; i++)
				{
					size = _sizeLUT[_focusIndex][i];
					space = _spaceLUT[_focusIndex][i];

					// Draw focused element with a background hilight
					if (_selectedIndex == i)
					{
						NuGenPaintParams selPaintParams = new NuGenPaintParams(itemPaintParams);
						selPaintParams.Bounds = this.LayoutManager.GetSelectionFrameBounds(
							_borderLeft, y, width - (_borderLeft + _borderRight) - 1, size + space
						);
						selPaintParams.State = NuGenControlState.Hot;

						this.Renderer.DrawSelectionFrame(selPaintParams);
					} // if

					itemPaintParams.Text = sortedItems[i];
					itemPaintParams.Font = _fonts[size - 1];
					itemPaintParams.Bounds = this.GetTextBounds(g, itemPaintParams.Text, itemPaintParams.Font, x, y);

					this.Renderer.DrawText(itemPaintParams);
					y += space + size;
				} // for

				foreach (KeyValuePair<string, int> pair in _labelLUT)
				{
					itemPaintParams.Text = pair.Key;
					itemPaintParams.Font = _boldFonts[_maxFontSize - 1];
					itemPaintParams.Bounds = this.GetTextBounds(
						g
						, itemPaintParams.Text
						, itemPaintParams.Font
						, _labelBorderLeft
						, pair.Value - _sizeLUT[0][0]
					);

					this.Renderer.DrawText(itemPaintParams);
				} // foreach
			}
			else /* Fisheye mode. */
			{
				if (_focusIndex > 0 /* First render from focus up. */)
				{
					y = _flFocusPosition - _flSizeLUT[_focusIndex - 1] - _flSpaceLUT[_focusIndex - 1];

					for (i = _focusIndex - 1; i >= 0; i--)
					{
						if (y < _borderY)
						{
							break;
						}

						size = _flSizeLUT[i];
						space = _flSpaceLUT[i];

						// Draw focused element with a background hilight.
						if (_selectedIndex == i)
						{
							NuGenPaintParams selPaintParams = new NuGenPaintParams(itemPaintParams);
							selPaintParams.Bounds = this.LayoutManager.GetSelectionFrameBounds(
								_borderLeft, y, width - (_borderLeft + _borderRight), size + space
							);
							selPaintParams.State = NuGenControlState.Hot;

							this.Renderer.DrawSelectionFrame(selPaintParams);
						} // if

						itemPaintParams.Text = sortedItems[i];
						itemPaintParams.Font = _fonts[size - 1];
						itemPaintParams.Bounds = this.GetTextBounds(g, itemPaintParams.Text, itemPaintParams.Font, x, y);

						this.Renderer.DrawText(itemPaintParams);

						if (i > 0)
						{
							y -= _flSizeLUT[i - 1] + _flSpaceLUT[i - 1];
						}
					} // for
				} // if

				/* Then render from focus down. */

				y = _flFocusPosition;

				for (i = _focusIndex; i < sortedItems.Count; i++)
				{
					if (y > height - _borderY)
					{
						break;
					}

					size = _flSizeLUT[i];
					space = _flSpaceLUT[i];

					// Draw focused element with a background hilight
					if (_selectedIndex == i)
					{
						NuGenPaintParams selPaintParams = new NuGenPaintParams(itemPaintParams);
						selPaintParams.Bounds = this.LayoutManager.GetSelectionFrameBounds(
							_borderLeft, y, width - (_borderLeft + _borderRight), size + space
						);
						selPaintParams.State = NuGenControlState.Hot;

						this.Renderer.DrawSelectionFrame(selPaintParams);
					}

					itemPaintParams.Text = sortedItems[i];
					itemPaintParams.Font = _fonts[size - 1];
					itemPaintParams.Bounds = this.GetTextBounds(g, itemPaintParams.Text, itemPaintParams.Font, x, y);

					this.Renderer.DrawText(itemPaintParams);

					y += space + size;
				} // for
			}

			base.OnPaint(e);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.ParentChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			this.Refresh();
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Resize"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			this.Refresh();
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.VisibleChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected override void OnVisibleChanged(EventArgs e)
		{
			this.Refresh();
			base.OnVisibleChanged(e);
		}

		#endregion

		#region Methods.Private

		/// <summary>
		/// Calculate the sizes of the window, the max and min font sizes, and the focus length.
		/// It uses the following algorithm:
		/// Try to follow desired max font size and focus length by first computing
		/// the biggest minimum font size that will fit in the available space.
		/// If this is not satisfied for a minimum font size of 1, then first
		/// the focus length, and then the max font size is reduced until it fits.
		/// </summary>
		private void CalculateSizes()
		{
			if (this.Parent == null)
			{
				_shouldRender = false;
				return;
			}

			_noFishEye = false;
			_shouldRender = true;
			int i;
			int width = 0;
			int height = 0;
			int maxHeight = 0;
			_maxFontSize = _desiredMaxFontSize;
			_focusLength = _desiredFocusLength;

			if (this.AutoSize)
			{
				using (Graphics g = this.CreateGraphics())
				{
					float stringWidth;

					for (i = 0; i < this.Items.Count; i++)
					{
						stringWidth = g.MeasureString(this.Items[i].ToString(), _fonts[_maxFontSize - 1]).Width;

						if (stringWidth + _borderLeft + _borderRight > width)
						{
							width = (int)Math.Ceiling(stringWidth) + _borderLeft + _borderRight;
						}
					}
				}

				width += 6;
				maxHeight = this.Parent.Height - this.Top;
			}
			else
			{
				width = this.Width;
				maxHeight = this.Height;
			}

			int size;
			bool done = false;
			// Calculate minimum font size, and actual window height
			do
			{
				for (size = _maxFontSize; size >= 1; size--)
				{
					int tmpHeight = this.CalculateWindowHeight(size);

					if (tmpHeight <= maxHeight)
					{
						height = tmpHeight;
						_minFontSize = size;

						if (size == _maxFontSize)
						{
							// If whole window at max size, then no compressed spacing
							_minSpace = _desiredSpace;
						}
						else
						{
							_minSpace = (int)(_minFontSize * _minDesiredSpacing);
						}

						done = true;
						break;
					}
				}

				if (!done)
				{
					// If not done, there wasn't enough room, so try a smaller focus or max font size
					if (_focusLength > 2)
					{
						_focusLength -= 2;
					}
					else
					{
						_maxFontSize--;

						if (_maxFontSize == 1)
						{
							_shouldRender = false;
							return;
						}
					}
				}
			} while (!done);

			// If entire menu is full size, then just use requested space.
			// Else, use all the available space.
			if (size < _maxFontSize)
			{
				height = maxHeight;
			}

			if (this.AutoSize)
			{
				height += 6;
				// Set the size of the window
				this.Size = new Size(width, height);
			}

			int itemCount = this.Items.Count;

			// Now, calculate layout of all items for each focus
			_sizeLUT = new int[itemCount][];
			_spaceLUT = new int[itemCount][];

			for (i = 0; i < itemCount; i++)
			{
				_sizeLUT[i] = new int[itemCount];
				_spaceLUT[i] = new int[itemCount];
			}

			_flSizeLUT = new int[itemCount];
			_flSpaceLUT = new int[itemCount];

			int d;
			int j, k;
			int fl2;
			int y;
			int space;

			for (i = 0; i < itemCount; i++)
			{
				fl2 = _focusLength / 2;
				y = _borderY;

				for (j = 0; j < itemCount; j++)
				{
					// Calculate size and spacing for the current item
					d = Math.Abs(j - i);

					if (d > fl2)
					{
						d -= fl2;
						size = _maxFontSize - d;
						space = (int)(size * _desiredSpacing);
						if (size <= _minFontSize)
						{
							size = _minFontSize;
							space = _minSpace;
						}
					}
					else
					{
						size = _maxFontSize;
						space = _desiredSpace;
					}

					_sizeLUT[i][j] = size;
					_spaceLUT[i][j] = space;

					y += size + space;
				}

				// If extra space, then grow items to fill it
				int extraSpace = this.Height - y - 2 * _borderY;
				if (extraSpace > 0)
				{
					int j1, j2;
					int i1, i2;

					i1 = i - fl2 - 1;
					i2 = i + fl2 + 1;

					while ((extraSpace > 0) && ((i1 >= 0) || (i2 < itemCount)))
					{
						j1 = i1;
						j2 = i2;
						for (k = 0; k < (_maxFontSize - _minFontSize); k++)
						{
							if (j1 >= 0)
							{
								if (extraSpace > 0)
								{
									if (_sizeLUT[i][j1] < _maxFontSize)
									{
										_sizeLUT[i][j1]++;
										extraSpace--;
									}
								}
								if (extraSpace > 0)
								{
									if (_spaceLUT[i][j1] < _desiredSpace)
									{
										_spaceLUT[i][j1]++;
										extraSpace--;
									}
								}
								j1--;
							}

							if (extraSpace > 0)
							{
								if (j2 < itemCount)
								{
									if (_sizeLUT[i][j2] < _maxFontSize)
									{
										_sizeLUT[i][j2]++;
										extraSpace--;
									}
									if (extraSpace > 0)
									{
										if (_spaceLUT[i][j2] < _desiredSpace)
										{
											_spaceLUT[i][j2]++;
											extraSpace--;
										}
									}

									j2++;
								}
							}
						}
						i1--;
						i2++;
					}
				}
			}

			// Calculate look-up-table of mouse position => focus index
			_focusIndexLUT = new int[this.Height];

			for (i = 0; i < itemCount; i++)
			{
				y = _borderY;

				for (j = 0; j < i; j++)
				{
					y += _sizeLUT[i][j] + _spaceLUT[i][j];
				}

				if (y >= _focusIndexLUT.Length)
				{
					break;
				}

				_focusIndexLUT[y] = i;
			}

			for (i = 1; i < this.Height; i++)
			{
				if (_focusIndexLUT[i] == 0)
				{
					_focusIndexLUT[i] = _focusIndexLUT[i - 1];
				}
			}

			// Then, calculate label positions
			char currentLabel = ' ';
			char label;
			int labelY = 0;
			_labelLUT = new List<KeyValuePair<string, int>>();

			List<string> sortedItems = this.GetSortedItems(this.Items);
			Debug.Assert(sortedItems != null, "sortedItems != null");

			for (i = 0; i < sortedItems.Count; i++)
			{
				string text = sortedItems[i];

				if (!string.IsNullOrEmpty(text))
				{
					label = char.ToUpper(text[0]);
				}
				else
				{
					label = ' ';
				}

				if (currentLabel != label)
				{
					y = _borderY;

					for (j = 0; j < i; j++)
					{
						y += _sizeLUT[i][j] + _spaceLUT[i][j];
					}

					y += _sizeLUT[i][i];

					if (y > labelY + _maxFontSize + 1)
					{
						_labelLUT.Add(new KeyValuePair<string, int>(label.ToString(), y));
						labelY = y;
						currentLabel = label;
					}
				}
			}
		}

		/// <summary>
		/// Calculate the height of the window needed to render the items with 
		/// fisheye distortion using the class variables such as # of elements,
		/// max font size, spacing, and focus length, and the 
		/// specified minimum font size.
		/// </summary>
		/// <param name="minFontSize"></param>
		/// <returns></returns>
		private int CalculateWindowHeight(int minFontSize)
		{
			int height;
			int fl = _focusLength;
			int size;
			int space;
			int itemCount = this.Items.Count;

			if (_minFontSize == _maxFontSize)
			{
				// If whole window in max font, then no fisheye
				_noFishEye = true;
				height = itemCount * (_maxFontSize + _desiredSpace);
				return height;
			}

			// Start with focus area
			if (itemCount < fl)
			{
				fl = itemCount;
			}

			height = fl * (_maxFontSize + _desiredSpace);
			itemCount -= fl;

			// Then, calculate distortion area
			for (size = (_maxFontSize - 1); size > minFontSize; size--)
			{
				space = (int)(size * _desiredSpacing);
				height += 2 * (size + space);
				itemCount -= 2;

				if (itemCount <= 0)
				{
					break;
				}
			}

			// Finally, add minimum size area
			height += itemCount * (minFontSize + (int)(minFontSize * _minDesiredSpacing));
			height += 2 * _borderY;
			height += 6;

			return height;
		}

		private Font[] CreateRegularFontCollection(int itemCount)
		{
			return this.CreateFontCollection(itemCount, FontStyle.Regular);
		}

		private Font[] CreateBoldFontCollection(int itemCount)
		{
			return this.CreateFontCollection(itemCount, FontStyle.Bold);
		}

		private Font[] CreateFontCollection(int itemCount, FontStyle fontStyle)
		{
			Font[] fonts = new Font[Math.Max(0, itemCount)];
			FontFamily ff = this.Font.FontFamily;

			for (int i = 0; i < itemCount; i++)
			{
				fonts[i] = new Font(ff, i + 1, fontStyle);
			}

			return fonts;
		}

		private int GetBorderLeft(int fontSize)
		{
			return _labelBorderLeft + fontSize + 8;
		}

		private List<string> GetSortedItems(ObjectCollection unsortedItems)
		{
			Debug.Assert(unsortedItems != null, "unsortedItems != null");

			List<string> sortedItems = new List<string>();

			foreach (object item in unsortedItems)
			{
				sortedItems.Add(this.GetItemText(item));
			}

			sortedItems.Sort(StringComparer.CurrentCultureIgnoreCase);
			return sortedItems;
		}

		private Rectangle GetTextBounds(Graphics g, string text, Font font, int x, int y)
		{
			Debug.Assert(g != null, "g !=null");
			Debug.Assert(font != null, "font != null");

			return new Rectangle(
				new Point(x, y)
				, NuGenControlPaint.SizeFToSize(g.MeasureString(text, font))
			);
		}

		#endregion

		private Font[] _fonts;
		private Font[] _boldFonts;
		private int _focusLength;
		private int _maxFontSize;
		private int _minFontSize;

		/// <summary>
		/// Percentage of font size.
		/// </summary>
		private float _desiredSpacing;

		/// <summary>
		/// Percentage of font size.
		/// </summary>
		private float _minDesiredSpacing;

		private int _desiredSpace;
		private int _minSpace;
		private int _borderLeft;
		private int _borderRight;
		private int _labelBorderLeft;
		private int _borderY;
		private int _mouseY;

		/// <summary>
		/// The index of the item that is centered in the focus.
		/// </summary>
		private int _focusIndex;

		private int[] _focusIndexLUT;
		private List<KeyValuePair<string, int>> _labelLUT;
		private int[][] _sizeLUT;
		private int[][] _spaceLUT;
		private int _flFocusPosition;
		private int[] _flSizeLUT;
		private int[] _flSpaceLUT;
		private bool _focusLock;

		/// <summary>
		/// Indicates whether the parameters calculated in CalculateSize method are suitable to render
		/// the control.
		/// </summary>
		private bool _shouldRender;

		/// <summary>
		/// Indicates whether the fisheye extender should be rendered.
		/// </summary>
		private bool _noFishEye;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPinpointWindow"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		///		<para><see cref="INuGenPinpointLayoutManager"/></para>
		///		<para><see cref="INuGenPinpointRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenPinpointWindow(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.SetStyle(ControlStyles.Selectable, true);
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.SetStyle(ControlStyles.UserPaint, true);

			this.AutoSize = false;
			this.BackColor = this.DefaultBackColor;
			this.MinimumSize = new Size(10, 10);

			_desiredMaxFontSize = 12;
			_desiredFocusLength = 11;
			_desiredSpacing = 0.5f;
			_minDesiredSpacing = 0.1f;
			_borderRight = 3;
			_labelBorderLeft = 3;
			_borderY = 3;
			_focusLength = _desiredFocusLength;
			_minFontSize = 3;
			_maxFontSize = _desiredMaxFontSize;

			_desiredSpace = (int)(_maxFontSize * _desiredSpacing);
			_borderLeft = this.GetBorderLeft(_maxFontSize);

			_fonts = new Font[_maxFontSize];
			this.CreateRegularFontCollection(_maxFontSize).CopyTo(_fonts, 0);

			_boldFonts = new Font[_maxFontSize];
			this.CreateBoldFontCollection(_maxFontSize).CopyTo(_boldFonts, 0);
		}
	}
}
