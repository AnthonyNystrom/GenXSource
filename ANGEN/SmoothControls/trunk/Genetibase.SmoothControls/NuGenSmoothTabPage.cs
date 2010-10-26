/* -----------------------------------------------
 * NuGenSmoothTabPage.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
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


namespace Genetibase.SmoothControls
{
	/// <summary>
	/// </summary>
	[ToolboxItem(false)]
	public class NuGenSmoothTabPage : NuGenTabPage
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothTabPage"/> class.
		/// </summary>
		public NuGenSmoothTabPage()
			: this(NuGenSmoothServiceManager.TabControlServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothTabPage"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:<para/>
		/// <see cref="INuGenTabRenderer"/>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="serviceProvider"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public NuGenSmoothTabPage(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		#endregion
	}
}
