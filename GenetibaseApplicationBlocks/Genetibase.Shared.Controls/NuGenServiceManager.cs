/* -----------------------------------------------
 * NuGenServiceManager.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ControlReflectorInternals;
using Genetibase.Shared.Controls.LinkLabelInternals;
using Genetibase.Shared.Controls.PictureBoxInternals;
using Genetibase.Shared.Controls.PropertyGridInternals;
using Genetibase.Shared.Controls.TabControlInternals;
using Genetibase.Shared.Controls.TreeViewInternals;

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Service provider manager.
	/// </summary>
	internal static class NuGenServiceManager
	{
		#region Declarations.Fields

		/// <summary>
		/// </summary>
		public static readonly INuGenServiceProvider ControlReflectorServiceProvider;

		/// <summary>
		/// </summary>
		public static readonly INuGenServiceProvider LinkLabelServiceProvider;

		/// <summary>
		/// </summary>
		public static readonly INuGenServiceProvider PictureBoxServiceProvider;

		/// <summary>
		/// </summary>
		public static readonly INuGenServiceProvider PropertyGridServiceProvider;

		/// <summary>
		/// </summary>
		public static readonly INuGenServiceProvider TabControlServiceProvider;

		/// <summary>
		/// </summary>
		public static readonly INuGenServiceProvider TreeViewServiceProvider;

		#endregion

		#region Constructors

		static NuGenServiceManager()
		{
			ControlReflectorServiceProvider = new NuGenControlReflectorServiceProvider();
			LinkLabelServiceProvider = new NuGenLinkLabelServiceProvider();
			PictureBoxServiceProvider = new NuGenPictureBoxServiceProvider();
			PropertyGridServiceProvider = new NuGenPropertyGridServiceProvider();
			TabControlServiceProvider = new NuGenTabControlServiceProvider();
			TreeViewServiceProvider = new NuGenTreeViewServiceProvider();
		}

		#endregion
	}
}
