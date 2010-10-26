/* -----------------------------------------------
 * NuGenPrintPreview.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.PrintPreviewInternals;
using Genetibase.Shared.Controls.Properties;
using Genetibase.Shared.Controls.SpinInternals;
using Genetibase.Shared.Controls.ToolStripInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Represents a print preview dialog which supports custom renderers and icon sets for tool bar commands.
	/// </summary>
	[ToolboxItem(false)]
	[Designer("Genetibase.Shared.Controls.Design.NuGenPrintPreviewDesigner")]
	[DesignTimeVisible(true)]
	public partial class NuGenPrintPreview : NuGenForm
	{
		#region Events.Hidden

		/// <summary>
		/// </summary>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler AutoSizeChanged
		{
			add
			{
				base.AutoSizeChanged += value;
			}
			remove
			{
				base.AutoSizeChanged -= value;
			}
		}

		/// <summary>
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public new event EventHandler AutoValidateChanged
		{
			add
			{
				base.AutoValidateChanged += value;
			}
			remove
			{
				base.AutoValidateChanged -= value;
			}
		}

		/// <summary>
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public new event EventHandler BackColorChanged
		{
			add
			{
				base.BackColorChanged += value;
			}
			remove
			{
				base.BackColorChanged -= value;
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackgroundImageChanged
		{
			add
			{
				base.BackgroundImageChanged += value;
			}
			remove
			{
				base.BackgroundImageChanged -= value;
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler BackgroundImageLayoutChanged
		{
			add
			{
				base.BackgroundImageLayoutChanged += value;
			}
			remove
			{
				base.BackgroundImageLayoutChanged -= value;
			}
		}

		/// <summary>
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public new event EventHandler CausesValidationChanged
		{
			add
			{
				base.CausesValidationChanged += value;
			}
			remove
			{
				base.CausesValidationChanged -= value;
			}
		}

		/// <summary>
		/// </summary>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler ContextMenuChanged
		{
			add
			{
				base.ContextMenuChanged += value;
			}
			remove
			{
				base.ContextMenuChanged -= value;
			}
		}

		/// <summary>
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public new event EventHandler ContextMenuStripChanged
		{
			add
			{
				base.ContextMenuStripChanged += value;
			}
			remove
			{
				base.ContextMenuStripChanged -= value;
			}
		}

		/// <summary>
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public new event EventHandler CursorChanged
		{
			add
			{
				base.CursorChanged += value;
			}
			remove
			{
				base.CursorChanged -= value;
			}
		}

		/// <summary>
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public new event EventHandler DockChanged
		{
			add
			{
				base.DockChanged += value;
			}
			remove
			{
				base.DockChanged -= value;
			}
		}

		/// <summary>
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public new event EventHandler ImeModeChanged
		{
			add
			{
				base.ImeModeChanged += value;
			}
			remove
			{
				base.ImeModeChanged -= value;
			}
		}

		/// <summary>
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public new event EventHandler MarginChanged
		{
			add
			{
				base.MarginChanged += value;
			}
			remove
			{
				base.MarginChanged -= value;
			}
		}

		/// <summary>
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public new event EventHandler PaddingChanged
		{
			add
			{
				base.PaddingChanged += value;
			}
			remove
			{
				base.PaddingChanged -= value;
			}
		}

		/// <summary>
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public new event EventHandler RightToLeftChanged
		{
			add
			{
				base.RightToLeftChanged += value;
			}
			remove
			{
				base.RightToLeftChanged -= value;
			}
		}

		/// <summary>
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public new event EventHandler RightToLeftLayoutChanged
		{
			add
			{
				base.RightToLeftLayoutChanged += value;
			}
			remove
			{
				base.RightToLeftLayoutChanged -= value;
			}
		}

		/// <summary>
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public new event EventHandler TabStopChanged
		{
			add
			{
				base.TabStopChanged += value;
			}
			remove
			{
				base.TabStopChanged -= value;
			}
		}

		/// <summary>
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public new event EventHandler VisibleChanged
		{
			add
			{
				base.VisibleChanged += value;
			}
			remove
			{
				base.VisibleChanged -= value;
			}
		}

		#endregion

		#region Properties.Behavior

		/*
		 * Document
		 */

		/// <summary>
		/// Gets or sets the document to preview.
		/// </summary>
		[Browsable(true)]
		[DefaultValue(null)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_PrintPreview_Document")]
		public PrintDocument Document
		{
			get
			{
				return _previewControl.Document;
			}
			set
			{
				_previewControl.Document = value;
				_pageSpin.Enabled = value != null;
				_pageSpin.Value = _previewControl.StartPage + 1;
			}
		}

		#endregion

		#region Properties.Public

		/*
		 * MinimizeBox
		 */

		/// <summary>
		/// Gets or sets a value indicating whether the Minimize button is displayed in the caption bar of the form.
		/// </summary>
		/// <value></value>
		/// <returns>true to display a Minimize button for the form; otherwise, false. The default is true.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[DefaultValue(false)]
		public new bool MinimizeBox
		{
			get
			{
				return base.MinimizeBox;
			}
			set
			{
				base.MinimizeBox = value;
			}
		}

		/*
		 * PrintPreviewControl
		 */

		/// <summary>
		/// Gets the <see cref="PrintPreviewControl"/> associated with this <see cref="NuGenPrintPreview"/>.
		/// </summary>
		[Browsable(false)]
		public PrintPreviewControl PrintPreviewControl
		{
			get
			{
				return _previewControl;
			}
		}

		/*
		 * ShowInTaskBar
		 */

		/// <summary>
		/// Gets or sets a value indicating whether the form is displayed in the Windows taskbar.
		/// </summary>
		/// <value></value>
		/// <returns>true to display the form in the Windows taskbar at run time; otherwise, false. The default is true.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[DefaultValue(false)]
		public new bool ShowInTaskbar
		{
			get
			{
				return base.ShowInTaskbar;
			}
			set
			{
				base.ShowInTaskbar = value;
			}
		}

		#endregion

		#region Properties.Hidden

		/// <summary>
		/// Gets or sets the button on the form that is clicked when the user presses the ENTER key.
		/// </summary>
		/// <value></value>
		/// <returns>An <see cref="T:System.Windows.Forms.IButtonControl"></see> that represents the button to use as the accept button for the form.</returns>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new IButtonControl AcceptButton
		{
			get
			{
				return base.AcceptButton;
			}
			set
			{
				base.AcceptButton = value;
			}
		}

		/// <summary>
		/// Gets or sets the description of the control used by accessibility client applications.
		/// </summary>
		/// <value></value>
		/// <returns>The description of the control used by accessibility client applications. The default is null.</returns>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new string AccessibleDescription
		{
			get
			{
				return base.AccessibleDescription;
			}
			set
			{
				base.AccessibleDescription = value;
			}
		}

		/// <summary>
		/// Gets or sets the name of the control used by accessibility client applications.
		/// </summary>
		/// <value></value>
		/// <returns>The name of the control used by accessibility client applications. The default is null.</returns>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public new string AccessibleName
		{
			get
			{
				return base.AccessibleName;
			}
			set
			{
				base.AccessibleName = value;
			}
		}

		/// <summary>
		/// Gets or sets the accessible role of the control
		/// </summary>
		/// <value></value>
		/// <returns>One of the values of <see cref="T:System.Windows.Forms.AccessibleRole"></see>. The default is Default.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Windows.Forms.AccessibleRole"></see> values. </exception>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new AccessibleRole AccessibleRole
		{
			get
			{
				return base.AccessibleRole;
			}
			set
			{
				base.AccessibleRole = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the control can accept data that the user drags onto it.
		/// </summary>
		/// <value></value>
		/// <returns>true if drag-and-drop operations are allowed in the control; otherwise, false. The default is false.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public override bool AllowDrop
		{
			get
			{
				return base.AllowDrop;
			}
			set
			{
				base.AllowDrop = value;
			}
		}

		/// <summary>
		/// Gets or sets the edges of the container to which a control is bound and determines how a control is resized with its parent.
		/// </summary>
		/// <value></value>
		/// <returns>A bitwise combination of the <see cref="T:System.Windows.Forms.AnchorStyles"></see> values. The default is Top and Left.</returns>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override AnchorStyles Anchor
		{
			get
			{
				return base.Anchor;
			}
			set
			{
				base.Anchor = value;
			}
		}

		/// <summary>
		/// Gets or sets the base size used for autoscaling of the form.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Size"></see> that represents the base size that this form uses for autoscaling.</returns>
		[Obsolete("This property has been deprecated. Use the AutoScaleDimensions property instead.  http://go.microsoft.com/fwlink/?linkid=14202"), EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public override Size AutoScaleBaseSize
		{
			get
			{
				return base.AutoScaleBaseSize;
			}
			set
			{
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the form enables autoscrolling.
		/// </summary>
		/// <value></value>
		/// <returns>true to enable autoscrolling on the form; otherwise, false. The default is false.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override bool AutoScroll
		{
			get
			{
				return base.AutoScroll;
			}
			set
			{
				base.AutoScroll = value;
			}
		}

		/// <summary>
		/// Gets or sets the size of the auto-scroll margin.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Size"></see> that represents the height and width of the auto-scroll margin in pixels.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The <see cref="P:System.Drawing.Size.Height"></see> or <see cref="P:System.Drawing.Size.Width"></see> value assigned is less than 0. </exception>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new Size AutoScrollMargin
		{
			get
			{
				return base.AutoScrollMargin;
			}
			set
			{
				base.AutoScrollMargin = value;
			}
		}

		/// <summary>
		/// Gets or sets the minimum size of the auto-scroll.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Size"></see> that determines the minimum size of the virtual area through which the user can scroll.</returns>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new Size AutoScrollMinSize
		{
			get
			{
				return base.AutoScrollMinSize;
			}
			set
			{
				base.AutoScrollMinSize = value;
			}
		}

		/// <summary>
		/// </summary>
		/// <value></value>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override AutoValidate AutoValidate
		{
			get
			{
				return base.AutoValidate;
			}
			set
			{
				base.AutoValidate = value;
			}
		}

		/// <summary>
		/// </summary>
		/// <value></value>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
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

		/// <summary>
		/// Gets or sets the background image displayed in the control.
		/// </summary>
		/// <value></value>
		/// <returns>An <see cref="T:System.Drawing.Image"></see> that represents the image to display in the background of the control.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
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
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
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
		/// Gets or sets the button control that is clicked when the user presses the ESC key.
		/// </summary>
		/// <value></value>
		/// <returns>An <see cref="T:System.Windows.Forms.IButtonControl"></see> that represents the cancel button for the form.</returns>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public new IButtonControl CancelButton
		{
			get
			{
				return base.CancelButton;
			}
			set
			{
				base.CancelButton = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the control causes validation to be performed on any controls that require validation when it receives focus.
		/// </summary>
		/// <value></value>
		/// <returns>true if the control causes validation to be performed on any controls requiring validation when it receives focus; otherwise, false. The default is true.</returns>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new bool CausesValidation
		{
			get
			{
				return base.CausesValidation;
			}
			set
			{
				base.CausesValidation = value;
			}
		}

		/// <summary>
		/// Gets or sets the shortcut menu associated with the control.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Windows.Forms.ContextMenu"></see> that represents the shortcut menu associated with the control.</returns>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override ContextMenu ContextMenu
		{
			get
			{
				return base.ContextMenu;
			}
			set
			{
				base.ContextMenu = value;
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="T:System.Windows.Forms.ContextMenuStrip"></see> associated with this control.
		/// </summary>
		/// <value></value>
		/// <returns>The <see cref="T:System.Windows.Forms.ContextMenuStrip"></see> for this control, or null if there is no <see cref="T:System.Windows.Forms.ContextMenuStrip"></see>. The default is null.</returns>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public override ContextMenuStrip ContextMenuStrip
		{
			get
			{
				return base.ContextMenuStrip;
			}
			set
			{
				base.ContextMenuStrip = value;
			}
		}

		/// <summary>
		/// Gets or sets the cursor that is displayed when the mouse pointer is over the control.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Windows.Forms.Cursor"></see> that represents the cursor to display when the mouse pointer is over the control.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public override Cursor Cursor
		{
			get
			{
				return base.Cursor;
			}
			set
			{
				base.Cursor = value;
			}
		}

		/// <summary>
		/// Gets the data bindings for the control.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Windows.Forms.ControlBindingsCollection"></see> that contains the <see cref="T:System.Windows.Forms.Binding"></see> objects for the control.</returns>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new ControlBindingsCollection DataBindings
		{
			get
			{
				return base.DataBindings;
			}
		}

		/// <summary>
		/// Gets or sets which control borders are docked to its parent control and determines how a control is resized with its parent.
		/// </summary>
		/// <value></value>
		/// <returns>One of the <see cref="T:System.Windows.Forms.DockStyle"></see> values. The default is <see cref="F:System.Windows.Forms.DockStyle.None"></see>.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Windows.Forms.DockStyle"></see> values. </exception>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public override DockStyle Dock
		{
			get
			{
				return base.Dock;
			}
			set
			{
				base.Dock = value;
			}
		}

		/// <summary>
		/// Gets the dock padding settings for all edges of the control.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Windows.Forms.ScrollableControl.DockPaddingEdges"></see> that represents the padding for all the edges of a docked control.</returns>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new ScrollableControl.DockPaddingEdges DockPadding
		{
			get
			{
				return base.DockPadding;
			}
		}

		/// <summary>
		/// Gets or sets the border style of the form.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Windows.Forms.FormBorderStyle"></see> that represents the style of border to display for the form. The default is FormBorderStyle.Sizable.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value specified is outside the range of valid values. </exception>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public new FormBorderStyle FormBorderStyle
		{
			get
			{
				return base.FormBorderStyle;
			}
			set
			{
				base.FormBorderStyle = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether a Help button should be displayed in the caption box of the form.
		/// </summary>
		/// <value></value>
		/// <returns>true to display a Help button in the form's caption bar; otherwise, false. The default is false.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new bool HelpButton
		{
			get
			{
				return base.HelpButton;
			}
			set
			{
				base.HelpButton = value;
			}
		}

		/// <summary>
		/// Gets or sets the Input Method Editor (IME) mode of the control.
		/// </summary>
		/// <value></value>
		/// <returns>One of the <see cref="T:System.Windows.Forms.ImeMode"></see> values. The default is <see cref="F:System.Windows.Forms.ImeMode.Inherit"></see>.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:System.Windows.Forms.ImeMode"></see> enumeration values. </exception>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new ImeMode ImeMode
		{
			get
			{
				return base.ImeMode;
			}
			set
			{
				base.ImeMode = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the form is a container for multiple-document interface (MDI) child forms.
		/// </summary>
		/// <value></value>
		/// <returns>true if the form is a container for MDI child forms; otherwise, false. The default is false.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new bool IsMdiContainer
		{
			get
			{
				return base.IsMdiContainer;
			}
			set
			{
				base.IsMdiContainer = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the form will receive key events before the event is passed to the control that has focus.
		/// </summary>
		/// <value></value>
		/// <returns>true if the form will receive all key events; false if the currently selected control on the form receives key events. The default is false.</returns>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new bool KeyPreview
		{
			get
			{
				return base.KeyPreview;
			}
			set
			{
				base.KeyPreview = value;
			}
		}

		/// <summary>
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public new Padding Margin
		{
			get
			{
				return base.Margin;
			}
			set
			{
				base.Margin = value;
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="T:System.Windows.Forms.MainMenu"></see> that is displayed in the form.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Windows.Forms.MainMenu"></see> that represents the menu to display in the form.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public new MainMenu Menu
		{
			get
			{
				return base.Menu;
			}
			set
			{
				base.Menu = value;
			}
		}

		/// <summary>
		/// Gets or sets padding within the control.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Windows.Forms.Padding"></see> representing the control's internal spacing characteristics.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
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

		/// <summary>
		/// Gets or sets a value indicating whether control's elements are aligned to support locales using right-to-left fonts.
		/// </summary>
		/// <value></value>
		/// <returns>One of the <see cref="T:System.Windows.Forms.RightToLeft"></see> values. The default is <see cref="F:System.Windows.Forms.RightToLeft.Inherit"></see>.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The assigned value is not one of the <see cref="T:System.Windows.Forms.RightToLeft"></see> values. </exception>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override RightToLeft RightToLeft
		{
			get
			{
				return base.RightToLeft;
			}
			set
			{
				base.RightToLeft = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether right-to-left mirror placement is turned on.
		/// </summary>
		/// <value></value>
		/// <returns>true if right-to-left mirror placement is turned on; otherwise, false for standard child control placement. The default is false.</returns>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public override bool RightToLeftLayout
		{
			get
			{
				return base.RightToLeftLayout;
			}
			set
			{
				base.RightToLeftLayout = value;
			}
		}

		/// <summary>
		/// Gets or sets the style of the size grip to display in the lower-right corner of the form.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Windows.Forms.SizeGripStyle"></see> that represents the style of the size grip to display. The default is <see cref="F:System.Windows.Forms.SizeGripStyle.Auto"></see></returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value specified is outside the range of valid values. </exception>
		/// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DefaultValue(2)]
		public new SizeGripStyle SizeGripStyle
		{
			get
			{
				return base.SizeGripStyle;
			}
			set
			{
				base.SizeGripStyle = value;
			}
		}

		/// <summary>
		/// </summary>
		/// <value></value>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public new bool TabStop
		{
			get
			{
				return base.TabStop;
			}
			set
			{
				base.TabStop = value;
			}
		}

		/// <summary>
		/// Gets or sets the object that contains data about the control.
		/// </summary>
		/// <value></value>
		/// <returns>An <see cref="T:System.Object"></see> that contains data about the control. The default is null.</returns>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public new object Tag
		{
			get
			{
				return base.Tag;
			}
			set
			{
				base.Tag = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the form should be displayed as a topmost form.
		/// </summary>
		/// <value></value>
		/// <returns>true to display the form as a topmost form; otherwise, false. The default is false.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new bool TopMost
		{
			get
			{
				return base.TopMost;
			}
			set
			{
				base.TopMost = value;
			}
		}

		/// <summary>
		/// Gets or sets the color that will represent transparent areas of the form.
		/// </summary>
		/// <value></value>
		/// <returns>A <see cref="T:System.Drawing.Color"></see> that represents the color to display transparently on the form.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new Color TransparencyKey
		{
			get
			{
				return base.TransparencyKey;
			}
			set
			{
				base.TransparencyKey = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether to use the wait cursor for the current control and all child controls.
		/// </summary>
		/// <value></value>
		/// <returns>true to use the wait cursor for the current control and all child controls; otherwise, false. The default is false.</returns>
		[Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
		public new bool UseWaitCursor
		{
			get
			{
				return base.UseWaitCursor;
			}
			set
			{
				base.UseWaitCursor = value;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the control and all its parent controls are displayed.
		/// </summary>
		/// <value></value>
		/// <returns>true if the control and all its parent controls are displayed; otherwise, false. The default is true.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public new bool Visible
		{
			get
			{
				return base.Visible;
			}
			set
			{
				base.Visible = value;
			}
		}

		#endregion

		#region Properties.Services

		/*
		 * MenuItemCheckedTracker
		 */

		private INuGenMenuItemCheckedTracker _menuItemCheckedTracker;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenMenuItemCheckedTracker MenuItemCheckedTracker
		{
			get
			{
				if (_menuItemCheckedTracker == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_menuItemCheckedTracker = this.ServiceProvider.GetService<INuGenMenuItemCheckedTracker>();

					if (_menuItemCheckedTracker == null)
					{
						throw new NuGenServiceNotFoundException<INuGenMenuItemCheckedTracker>();
					}
				}

				return _menuItemCheckedTracker;
			}
		}

		/*
		 * ToolStripManager
		 */

		private INuGenPrintPreviewToolStripManager _toolstripManager;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenPrintPreviewToolStripManager ToolStripManager
		{
			get
			{
				if (_toolstripManager == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_toolstripManager = this.ServiceProvider.GetService<INuGenPrintPreviewToolStripManager>();

					if (_toolstripManager == null)
					{
						throw new NuGenServiceNotFoundException<INuGenPrintPreviewToolStripManager>();
					}
				}

				return _toolstripManager;
			}
		}

		#endregion

		#region Properties.Private

		private INuGenMenuItemGroup _zoomGroup;

		private INuGenMenuItemGroup ZoomGroup
		{
			get
			{
				if (_zoomGroup == null)
				{
					_zoomGroup = this.MenuItemCheckedTracker.CreateGroup(
						new ToolStripMenuItem[]
						{
							_autoZoomItem
							, _500ZoomItem
							, _250ZoomItem
							, _150ZoomItem
							, _100ZoomItem
							, _75ZoomItem
							, _50ZoomItem
							, _25ZoomItem
							, _10ZoomItem
						}
					);
				}

				return _zoomGroup;
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		/// <summary>
		/// Creates the handle for the form. If a derived class overrides this function, it must call the base implementation.
		/// </summary>
		protected override void CreateHandle()
		{
			if ((this.Document != null) && !this.Document.PrinterSettings.IsValid)
			{
				throw new InvalidPrinterException(this.Document.PrinterSettings);
			}

			base.CreateHandle();
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Form.Closing"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs"></see> that contains the event data.</param>
		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);
			_previewControl.InvalidatePreview();
		}

		/// <summary>
		/// Processes a dialog box key.
		/// </summary>
		/// <param name="keyData">One of the <see cref="T:System.Windows.Forms.Keys"></see> values that represents the key to process.</param>
		/// <returns>
		/// true if the keystroke was processed and consumed by the control; otherwise, false to allow further processing.
		/// </returns>
		[SecurityPermission(SecurityAction.LinkDemand)]
		protected override bool ProcessDialogKey(Keys keyData)
		{
			if ((keyData & (Keys.Alt | Keys.Control)) == Keys.None)
			{
				switch ((keyData & Keys.KeyCode))
				{
					case Keys.Left:
					case Keys.Up:
					case Keys.Right:
					case Keys.Down:
					{
						return false;
					}
				}
			}

			return base.ProcessDialogKey(keyData);
		}

		/// <summary>
		/// Processes the tab key.
		/// </summary>
		/// <param name="forward">if set to <c>true</c> [forward].</param>
		/// <returns></returns>
		protected override bool ProcessTabKey(bool forward)
		{
			if (base.ActiveControl == _previewControl)
			{
				_pageSpin.Focus();
				return true;
			}

			return false;
		}

		#endregion

		#region Methods.Private

		private void CheckZoomMenuItem(ZoomMenuItem zoomMenuItem)
		{
			this.MenuItemCheckedTracker.CheckedChanged(this.ZoomGroup, zoomMenuItem);
			_previewControl.Zoom = zoomMenuItem.ZoomFactor;
			_previewControl.AutoZoom = zoomMenuItem.AutoZoom;
		}

		#endregion

		#region EventHandlers.Controls

		private void _previewControl_StartPageChanged(object sender, EventArgs e)
		{
			_pageSpin.Value = _previewControl.StartPage + 1;
		}

		#endregion

		#region EventHandlers.MenuStrip

		private void _closeButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void _fourPagesButton_Click(object sender, EventArgs e)
		{
			_previewControl.Rows = _previewControl.Columns = 2;
		}

		private void _pageSpin_ValueChanged(object sender, EventArgs e)
		{
			int currentPage = _pageSpin.Value - 1;
			
			if (currentPage >= 0)
			{
				_previewControl.StartPage = currentPage;
			}
			else
			{
				_pageSpin.Value = _previewControl.StartPage + 1;
			}
		}

		private void _printButton_Click(object sender, EventArgs e)
		{
			if (_previewControl.Document != null)
			{
				_previewControl.Document.Print();
			}
		}

		private void _singlePageButton_Click(object sender, EventArgs e)
		{
			_previewControl.Rows = _previewControl.Columns = 1;
		}

		private void _twoPagesButton_Click(object sender, EventArgs e)
		{
			_previewControl.Columns = 2;
			_previewControl.Rows = 1;
		}

		private void _zoomPercent_Click(object sender, EventArgs e)
		{
			this.CheckZoomMenuItem((ZoomMenuItem)sender);
		}

		#endregion

		private NuGenToolStrip _toolStrip;
		private PrintPreviewControl _previewControl;

		private ZoomMenuItem _autoZoomItem;
		private ZoomMenuItem _500ZoomItem;
		private ZoomMenuItem _250ZoomItem;
		private ZoomMenuItem _150ZoomItem;
		private ZoomMenuItem _100ZoomItem;
		private ZoomMenuItem _75ZoomItem;
		private ZoomMenuItem _50ZoomItem;
		private ZoomMenuItem _25ZoomItem;
		private ZoomMenuItem _10ZoomItem;

		private NuGenToolStripSpin _pageSpin;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPrintPreview"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// <para>Requires:</para>
		/// <para><see cref="INuGenButtonStateTracker"/></para>
		/// <para><see cref="INuGenControlStateTracker"/></para>
		/// <para><see cref="INuGenInt32ValueConverter"/></para>
		/// <para><see cref="INuGenMenuItemCheckedTracker"/></para>
		/// <para><see cref="INuGenPrintPreviewToolStripManager"/></para>
		/// <para><see cref="INuGenSpinRenderer"/></para>
		/// <para><see cref="INuGenToolStripRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenPrintPreview(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
			_toolStrip = new NuGenToolStrip(serviceProvider);
			_pageSpin = new NuGenToolStripSpin(serviceProvider);
			_previewControl = new PrintPreviewControl();

			_previewControl.Dock = DockStyle.Fill;
			_previewControl.StartPageChanged += _previewControl_StartPageChanged;

			_pageSpin.Alignment = ToolStripItemAlignment.Right;
			_pageSpin.Enabled = false;
			_pageSpin.Minimum = 0;
			_pageSpin.Maximum = 1000;
			_pageSpin.Width = 50;
			_pageSpin.ValueChanged += _pageSpin_ValueChanged;
			_pageSpin.Value = 1;

			ToolStripButton closeButton = this.ToolStripManager.GetCloseToolStripButton();
			ToolStripButton fourPagesButton = this.ToolStripManager.GetFourPagesToolStripButton();
			ToolStripLabel pageLabel = this.ToolStripManager.GetPageToolStripLabel();
			ToolStripButton printButton = this.ToolStripManager.GetPrintToolStripButton();
			ToolStripButton singlePageButton = this.ToolStripManager.GetSinglePageToolStripButton();
			ToolStripButton twoPagesButton = this.ToolStripManager.GetTwoPagesToolStripButton();
			ToolStripDropDownButton zoomButton = this.ToolStripManager.GetZoomToolStripDropDownButton();

			closeButton.Click += _closeButton_Click;
			fourPagesButton.Click += _fourPagesButton_Click;
			printButton.Click += _printButton_Click;
			singlePageButton.Click += _singlePageButton_Click;
			twoPagesButton.Click += _twoPagesButton_Click;

			pageLabel.Alignment = ToolStripItemAlignment.Right;

			zoomButton.DropDownItems.AddRange(
				new ToolStripItem[]
			    {
			        _autoZoomItem = new ZoomMenuItem(Resources.Text_ZoomButton_Auto, true)
			        , _500ZoomItem = new ZoomMenuItem(Resources.Text_ZoomButton_500Percent, 5.00f)
			        , _250ZoomItem = new ZoomMenuItem(Resources.Text_ZoomButton_250Percent, 2.50f)
			        , _150ZoomItem = new ZoomMenuItem(Resources.Text_ZoomButton_150Percent, 1.50f)
			        , _100ZoomItem = new ZoomMenuItem(Resources.Text_ZoomButton_100Percent, 1.00f)
			        , _75ZoomItem = new ZoomMenuItem(Resources.Text_ZoomButton_75Percent, 0.75f)
			        , _50ZoomItem = new ZoomMenuItem(Resources.Text_ZoomButton_50Percent, 0.50f)
			        , _25ZoomItem = new ZoomMenuItem(Resources.Text_ZoomButton_25Percent, 0.25f)
			        , _10ZoomItem = new ZoomMenuItem(Resources.Text_ZoomButton_10Percent, 0.10f)
			    }
			);

			foreach (ToolStripMenuItem menuItem in zoomButton.DropDownItems)
			{
				menuItem.Click += _zoomPercent_Click;
			}

			this.CheckZoomMenuItem(_autoZoomItem);

			_toolStrip.Items.AddRange(
				new ToolStripItem[]
			    {
			        printButton
			        , zoomButton
			        , new ToolStripSeparator()
			        , singlePageButton
			        , twoPagesButton
			        , fourPagesButton
			        , new ToolStripSeparator()
			        , closeButton
			        , _pageSpin
			        , pageLabel
			    }
			);

			this.Controls.Add(_previewControl);
			this.Controls.Add(_toolStrip);
			this.MinimizeBox = false;
			this.Size = new Size(640, 480);
			this.ShowInTaskbar = false;
		}
	}
}
