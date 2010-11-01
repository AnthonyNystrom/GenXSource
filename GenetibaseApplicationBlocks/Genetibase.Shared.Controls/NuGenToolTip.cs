/* -----------------------------------------------
 * NuGenToolTip.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.ToolTipInternals;
using Genetibase.WinApi;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Represents a Microsoft Office 2007 like tooltip.
	/// </summary>
	[ToolboxItem(false)]
	[ProvideProperty("ToolTip", typeof(Control))]
	[System.ComponentModel.DesignerCategory("Code")]
	public abstract partial class NuGenToolTip : Component, IExtenderProvider
	{
		#region Declarations.Fields

		private IContainer _components;
		private ToolTipControl _tooltip;
		private Timer _autoPopDelayTimer;

		private static readonly int _tooltipOffset = 3;

		#endregion

		#region IExtenderProvider Members

		/// <summary>
		/// Specifies whether this object can provide its extender properties to the specified object.
		/// </summary>
		/// <param name="extendee">The <see cref="T:System.Object"></see> to receive the extender properties.</param>
		/// <returns>
		/// true if this object can provide extender properties to the specified object; otherwise, false.
		/// </returns>
		public bool CanExtend(object extendee)
		{
			return extendee is Control && !(extendee is Form || extendee is UserControl);
		}

		#endregion

		#region Properties.Appearance

		/*
		 * Font
		 */

		private Font _font;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_ToolTip_Font")]
		public Font Font
		{
			get
			{
				return _font;
			}
			set
			{
				if (_font != value)
				{
					_font = value;

					if (_tooltip.Visible)
					{
						_tooltip.Invalidate();
					}
				}
			}
		}

		/*
		 * MinimumTooltipSize
		 */

		private Size _minimumTooltipSize;

		[Browsable(true)]
		[NuGenSRCategory("Category_Appearance")]
		[NuGenSRDescription("Description_ToolTip_MinimumTooltipSize")]
		public Size MinimumTooltipSize
		{
			get
			{
				return _minimumTooltipSize;
			}
			set
			{
				_minimumTooltipSize = value;
			}
		}

		#endregion

		#region Properties.Behavior

		/*
		 * AutoPopDelay
		 */

		private int _autoPopDelay = 5000;

		/// <summary>
		/// Gets or sets the period of time the tooltip remains visible if the pointer is stationary on a control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(5000)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_ToolTip_AutoPopDelay")]
		public int AutoPopDelay
		{
			get
			{
				return _autoPopDelay;
			}
			set
			{
				_autoPopDelay = value;
			}
		}

		/*
		 * PlaceBelowControl
		 */

		private bool _placeBelowControl = true;

		/// <summary>
		/// </summary>
		[Browsable(true)]
		[DefaultValue(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_ToolTip_PlaceBelowControl")]
		public bool PlaceBelowControl
		{
			get
			{
				return _placeBelowControl;
			}
			set
			{
				_placeBelowControl = value;
			}
		}

		#endregion

		#region Properties.Extended.Tooltip

		/// <summary>
		/// </summary>
		/// <param name="targetControl"></param>
		/// <returns></returns>
		[DefaultValue(null)]
		public NuGenToolTipInfo GetToolTip(Control targetControl)
		{
			if (this.Tooltips.ContainsKey(targetControl))
			{
				return this.Tooltips[targetControl];
			}

			return null;
		}

		/// <summary>
		/// Associates a tooltip with the specified <see cref="Control"/>.
		/// </summary>
		/// <param name="targetControl"></param>
		/// <param name="tooltipInfo">
		/// Specify <see langword="null"/> to remove a tooltip for the specified <paramref name="targetControl"/>.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="targetControl"/> is <see langword="null"/>.</para>
		/// </exception>
		public void SetToolTip(Control targetControl, NuGenToolTipInfo tooltipInfo)
		{
			if (targetControl == null)
			{
				throw new ArgumentNullException("targetControl");
			}

			if (tooltipInfo == null)
			{
				this.RemoveTooltip(targetControl);
			}

			if (this.Tooltips.ContainsKey(targetControl))
			{
				this.Tooltips[targetControl] = tooltipInfo;
			}
			else
			{
				this.AddTooltip(targetControl, tooltipInfo);
			}
		}

		#endregion

		#region Properties.Services

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

		#region Properties.Private

		/*
		 * Tooltips
		 */

		private Dictionary<Control, NuGenToolTipInfo> _tooltips;

		private Dictionary<Control, NuGenToolTipInfo> Tooltips
		{
			get
			{
				if (_tooltips == null)
				{
					_tooltips = new Dictionary<Control, NuGenToolTipInfo>();
				}

				return _tooltips;
			}
		}

		#endregion

		#region Methods.Public

		public void Hide()
		{
			_tooltip.Hide();
		}

		public void ShowToolTip(Control targetControl, Point location)
		{
			NuGenToolTipInfo tooltipInfo = null;

			if (this.Tooltips.ContainsKey(targetControl))
			{
				tooltipInfo = this.Tooltips[targetControl];
			}

			if (tooltipInfo != null)
			{
				_tooltip.Show(tooltipInfo, location);
			}
		}

		#endregion

		#region Methods.Private

		private void AddTooltip(Control targetControl, NuGenToolTipInfo tooltipInfo)
		{
			Debug.Assert(targetControl != null, "targetControl != null");
			targetControl.MouseHover += _targetControl_MouseHover;
			targetControl.MouseLeave += _targetControl_MouseLeave;
			this.Tooltips.Add(targetControl, tooltipInfo);
		}

		private void RemoveTooltip(Control targetControl)
		{
			Debug.Assert(targetControl != null, "targetControl != null");
			targetControl.MouseHover -= _targetControl_MouseHover;
			targetControl.MouseLeave -= _targetControl_MouseLeave;
			this.Tooltips.Remove(targetControl);
		}

		private void ResetHover(Control targetControl)
		{
			if (targetControl == null)
			{
				return;
			}

			TRACKMOUSEEVENT eventTrack = new TRACKMOUSEEVENT();
			eventTrack.dwFlags = WinUser.TME_QUERY;
			eventTrack.hwndTrack = targetControl.Handle.ToInt32();
			eventTrack.cbSize = Marshal.SizeOf(eventTrack);
			User32.TrackMouseEvent(ref eventTrack);
			eventTrack.dwFlags |= WinUser.TME_HOVER;
			User32.TrackMouseEvent(ref eventTrack);
		}

		#endregion

		#region EventHandlers.TargetControl

		private int _hoverCount;

		private void _targetControl_MouseHover(object sender, EventArgs e)
		{
			_hoverCount++;

			if (_hoverCount < 2)
			{
				this.ResetHover((Control)sender);
			}
			else
			{
				Control ctrl = (Control)sender;
				Point tooltipLocation;

				if (this.PlaceBelowControl)
				{
					Rectangle ctrlScreenBounds = ctrl.RectangleToScreen(ctrl.ClientRectangle);
					tooltipLocation = new Point(Cursor.Position.X, ctrlScreenBounds.Bottom + _tooltipOffset);
				}
				else
				{
					tooltipLocation = Cursor.Position;
					Size cursorSize = Cursor.Current.Size;
					tooltipLocation.Offset(cursorSize.Width / 2, cursorSize.Height / 2);
				}

				this.ShowToolTip(ctrl, tooltipLocation);
				_autoPopDelayTimer.Start();
			}
		}
		
		private void _targetControl_MouseLeave(object sender, EventArgs e)
		{
			_hoverCount = 0;
			this.Hide();
		}

		#endregion

		#region EventHandlers.Timers

		private void _autoPopDelayTimer_Tick(object sender, EventArgs e)
		{
			_autoPopDelayTimer.Stop();
			_tooltip.Hide();
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenToolTip"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// <para>Requires:</para>
		/// <para><see cref="INuGenToolTipRenderer"/></para>
		/// <para><see cref="INuGenToolTipLayoutManager"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenToolTip(INuGenServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			_serviceProvider = serviceProvider;
			
			_components = new Container();
			_tooltip = new ToolTipControl(serviceProvider);
			_components.Add(_tooltip);
			
			_autoPopDelayTimer = new Timer(_components);
			_autoPopDelayTimer.Interval = this.AutoPopDelay;
			_autoPopDelayTimer.Tick += _autoPopDelayTimer_Tick;
		}

		#endregion

		#region Dispose

		private bool _isDisposed;

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.Component"></see> and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && !_isDisposed)
			{
				if (_components != null)
				{
					_components.Dispose();
				}

				_isDisposed = true;
			}

			base.Dispose(disposing);
		}

		#endregion
	}
}
