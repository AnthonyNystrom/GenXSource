/* -----------------------------------------------
 * NuGenSmoothTabLayoutManager.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.TabControlInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.SmoothControls.TabControlInternals
{
	/// <summary>
	/// </summary>
	public class NuGenSmoothTabLayoutManager : NuGenTabLayoutManager
	{
		#region INuGenTabLayoutManager.RegisterLayoutBuilder

		/// <summary>
		/// </summary>
		/// <param name="tabButtons"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="tabButtons"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public override NuGenTabLayoutBuilder RegisterLayoutBuilder(List<NuGenTabButton> tabButtons)
		{
			if (tabButtons == null)
			{
				throw new ArgumentNullException("tabButtons");
			}

			return new NuGenSmoothTabLayoutBuilder(tabButtons, new Size(250, 24));
		}

		#endregion

		#region INuGenTabLayoutManager.GetTabPageBounds

		/// <summary>
		/// </summary>
		/// <param name="tabControlBounds"></param>
		/// <param name="tabStripBounds"></param>
		public override Rectangle GetTabPageBounds(Rectangle tabControlBounds, Rectangle tabStripBounds)
		{
			return new Rectangle(
				tabControlBounds.Left,
				tabControlBounds.Top + tabStripBounds.Height + 2,
				tabControlBounds.Width,
				tabControlBounds.Height - tabStripBounds.Bottom + 1
			);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothTabLayoutManager"/> class.
		/// </summary>
		public NuGenSmoothTabLayoutManager()
		{

		}

		#endregion
	}
}
