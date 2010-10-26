/* -----------------------------------------------
 * NuGenSmoothTabLayoutBuilder.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.TabControlInternals;

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Genetibase.SmoothControls.TabControlInternals
{
	/// <summary>
	/// </summary>
	public class NuGenSmoothTabLayoutBuilder : NuGenTabLayoutBuilder
	{
		private static readonly Rectangle _selectedTabButtonOffset = new Rectangle(-2, -2, 4, 2);

		/*
		 * SelectedTabButtonOffset
		 */

		/// <summary>
		/// Gets the <see cref="Rectangle"/> that determines the offset for the location and size
		/// of the selected tab button.
		/// </summary>
		/// <value></value>
		protected override Rectangle SelectedTabButtonOffset
		{
			get
			{
				return _selectedTabButtonOffset;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothTabLayoutBuilder"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="tabButtons"/> is <see langword="null"/>.</exception>
		public NuGenSmoothTabLayoutBuilder(List<NuGenTabButton> tabButtons, Size defaultTabButtonSize)
			: base(tabButtons, defaultTabButtonSize)
		{
		}
	}
}
