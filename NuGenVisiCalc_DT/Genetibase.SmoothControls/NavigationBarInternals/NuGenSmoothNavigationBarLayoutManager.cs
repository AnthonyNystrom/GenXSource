/* -----------------------------------------------
 * NuGenSmoothNavigationBarLayoutManager.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.NavigationBarInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.NavigationBarInternals
{
	/// <summary>
	/// </summary>
	internal sealed class NuGenSmoothNavigationBarLayoutManager : INuGenNavigationBarLayoutManager
	{
		#region INuGenNavigationBarLayoutManager Members

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public int GetButtonHeight()
		{
			return 32;
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public int GetBottomContainerLeftMargin()
		{
			return 15;
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public int GetGripHeight()
		{
			return 6;
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		public int GetSmallButtonWidth()
		{
			return 22;
		}

		public int GetTitleHeight()
		{
			return 32;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothNavigationBarLayoutManager"/> class.
		/// </summary>
		public NuGenSmoothNavigationBarLayoutManager()
		{

		}
		
		#endregion
	}
}
