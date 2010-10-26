/* -----------------------------------------------
 * NuGenGraphBase.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using Genetibase.UI.NuGenMeters.Design;

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.UI.NuGenMeters
{
	/// <summary>
	/// Defines a flexible graph control.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[Designer(typeof(NuGenGraphBaseDesigner))]
	[ToolboxItem(false)]
	public class NuGenGraphBase : NuGenGraphGeneric
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenGraphBase"/> class.
		/// </summary>
		public NuGenGraphBase()
		{
		}

		#endregion
	}
}
