/* -----------------------------------------------
 * NuGenServiceManager.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ControlReflectorInternals;
using Genetibase.Shared.Controls.LabelInternals;
using Genetibase.Shared.Controls.ListBoxInternals;
using Genetibase.Shared.Controls.MatrixLabelInternals;
using Genetibase.Shared.Controls.PictureBoxInternals;
using Genetibase.Shared.Controls.PropertyGridInternals;
using Genetibase.Shared.Controls.TabControlInternals;
using Genetibase.Shared.Controls.TreeViewInternals;

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Controls
{
	internal static class NuGenServiceManager
	{
		public static readonly INuGenServiceProvider ControlReflectorServiceProvider = new NuGenControlReflectorServiceProvider();
		public static readonly INuGenServiceProvider LabelServiceProvider = new NuGenLabelServiceProvider();
		public static readonly INuGenServiceProvider ListBoxServiceProvider = new NuGenListBoxServiceProvider();
		public static readonly INuGenServiceProvider MatrixLabelServiceProvider = new NuGenMatrixLabelServiceProvider();
		public static readonly INuGenServiceProvider PictureBoxServiceProvider = new NuGenPictureBoxServiceProvider();
		public static readonly INuGenServiceProvider PropertyGridServiceProvider = new NuGenPropertyGridServiceProvider();
		public static readonly INuGenServiceProvider TabControlServiceProvider = new NuGenTabControlServiceProvider();
		public static readonly INuGenServiceProvider TreeViewServiceProvider = new NuGenTreeViewServiceProvider();
	}
}
