/* -----------------------------------------------
 * NuGenSmoothTabControl.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.TabControlInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// <seealso cref="TabControl"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothTabControl), "Resources.NuGenIcon.png")]
	[Designer("Genetibase.SmoothControls.Design.NuGenSmoothTabControlDesigner")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenSmoothTabControl : NuGenTabControl
	{
		#region Properties.Hidden

		/*
		 * FlatStyle
		 */

		/// <summary>
		/// Do not use this property. Any value will not affect the appearance.
		/// </summary>
		/// <value></value>
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
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

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothTabControl"/> class.
		/// </summary>
		public NuGenSmoothTabControl()
			: this(NuGenSmoothServiceManager.TabControlServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothTabControl"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenButtonStateTracker"/><para/>
		/// <see cref="INuGenControlStateTracker"/><para/>
		/// <see cref="INuGenSmoothColorManager"/><para/>
		/// <see cref="INuGenTabRenderer"/><para/>
		/// <see cref="INuGenTabStateTracker"/><para/>
		/// <see cref="INuGenTabLayoutManager"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="serviceProvider"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public NuGenSmoothTabControl(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
