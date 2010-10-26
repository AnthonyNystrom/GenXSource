/* -----------------------------------------------
 * NuGenPageHostDesignerBase.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides basic functionality for such designers as <see cref="NuGenTabControlDesigner"/> and the like.
	/// </summary>
	public abstract class NuGenPageHostDesignerBase : ParentControlDesigner
	{
		/*
		 * AllowControlLasso
		 */

		/// <summary>
		/// Gets a value indicating whether selected controls will be re-parented.
		/// </summary>
		/// <value></value>
		/// <returns>true if the controls that were selected by lassoing on the designer's surface will be re-parented to this designer's control.</returns>
		protected override bool AllowControlLasso
		{
			get
			{
				return false;
			}
		}
	}
}
