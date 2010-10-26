/* -----------------------------------------------
 * NuGenSmoothToolTip.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ToolTipInternals;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// <seealso cref="NuGenToolTip"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothToolTip), "Resources.NuGenIcon.png")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenSmoothToolTip : NuGenToolTip
	{
		/// <summary>
		/// </summary>
		/// <param name="targetControl"></param>
		/// <returns></returns>
		[Editor("Genetibase.SmoothControls.Design.NuGenSmoothToolTipInfoEditor", typeof(UITypeEditor))]
		public new NuGenToolTipInfo GetToolTip(Control targetControl)
		{
			return base.GetToolTip(targetControl);
		}

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothToolTip"/> class.
		/// </summary>
		public NuGenSmoothToolTip()
			: this(NuGenSmoothServiceManager.ToolTipServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothToolTip"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenToolTipRenderer"/></para></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothToolTip(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		#endregion
	}
}
