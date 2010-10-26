/* -----------------------------------------------
 * NuGenSpacer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.ComponentModel;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Use this control to specify location offset when docking controls.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSpacer), "Resources.NuGenIcon.png")]
	[DefaultEvent("Click")]
	[DefaultProperty("Dock")]
	[Designer("Genetibase.Shared.Controls.Design.NuGenSpacerDesigner")]
	[NuGenSRDescription("Description_Spacer")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenSpacer : Control
	{
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

		private new void ResetBackColor()
		{
			this.BackColor = Color.Transparent;
		}

		private bool ShouldSerializeBackColor()
		{
			return this.BackColor != Color.Transparent;
		}

		private static readonly Size _defaultSize = new Size(10, 10);

		/// <summary>
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				return _defaultSize;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSpacer"/> class.
		/// </summary>
		public NuGenSpacer()
		{
			this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.BackColor = Color.Transparent;
			this.TabStop = false;
		}
	}
}
