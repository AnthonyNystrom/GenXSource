/* -----------------------------------------------
 * NuGenSmoothComboBox.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ComboBoxInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;
using Genetibase.SmoothControls.ComponentModel;
using Genetibase.SmoothControls.Properties;
using Genetibase.WinApi;

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
	/// <seealso cref="ComboBox"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothComboBox), "Resources.NuGenIcon.png")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenSmoothComboBox : NuGenComboBox
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothComboBox"/> class.
		/// </summary>
		public NuGenSmoothComboBox()
			: this(NuGenSmoothServiceManager.ComboBoxServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothComboBox"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenComboBoxRenderer"/><para/>
		/// <see cref="INuGenButtonStateTracker"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="serviceProvider"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public NuGenSmoothComboBox(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		#endregion
	}
}
