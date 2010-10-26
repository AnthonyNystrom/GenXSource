/* -----------------------------------------------
 * NuGenSmoothButton.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// <seealso cref="Button"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothButton), "Resources.NuGenIcon.png")]
	public class NuGenSmoothButton : NuGenButton
	{
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
		 * BackgrounImageLayout
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
		 * TextImageRelation
		 */

		/// <summary>
		/// Do not use this property. Any value will not affect the appearance.
		/// </summary>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new TextImageRelation TextImageRelation
		{
			get
			{
				return base.TextImageRelation;
			}
			set
			{
				base.TextImageRelation = value;
			}
		}

		#endregion

		#region Properties.Public.New

		/*
		 * FlatStyle
		 */

		/// <summary>
		/// Gets or sets the flat style appearance of the button control.
		/// </summary>
		/// <value></value>
		/// <returns>One of the <see cref="T:System.Windows.Forms.FlatStyle"></see> values. The default value is Standard.</returns>
		/// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">The value assigned is not one of the <see cref="T:System.Windows.Forms.FlatStyle"></see> values. </exception>
		/// <PermissionSet><IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence"/><IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/></PermissionSet>
		[DefaultValue(FlatStyle.Standard)]
		public new FlatStyle FlatStyle
		{
			get
			{
				return base.FlatStyle;
			}
			set
			{
				base.FlatStyle = value;
			}
		}

		#endregion

		#region Properties.Protected.Overridden

		private static readonly Size _defaultSize = new Size(133, 30);

		/// <summary>
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				return _defaultSize;
			}
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothButton"/> class.
		/// </summary>
		public NuGenSmoothButton()
			: this(NuGenSmoothServiceManager.ButtonServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothButton"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenButtonStateTracker"/><para/>
		/// <see cref="INuGenSmoothColorManager"/><para/>
		/// </param>
		public NuGenSmoothButton(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
