/* -----------------------------------------------
 * NuGenTabPageDesigner.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenTabPage"/>.
	/// </summary>
	public class NuGenTabPageDesigner : NuGenPageDesignerBase
	{
		/*
		 * CanBeParentedTo
		 */

		/// <summary>
		/// Indicates if this designer's control can be parented by the control of the specified designer.
		/// </summary>
		/// <param name="parentDesigner">The <see cref="T:System.ComponentModel.Design.IDesigner"></see> that manages the control to check.</param>
		/// <returns>
		/// true if the control managed by the specified designer can parent the control managed by this designer; otherwise, false.
		/// </returns>
		public override bool CanBeParentedTo(IDesigner parentDesigner)
		{
			if (parentDesigner != null)
			{
				return parentDesigner.Component is NuGenTabControl;
			}

			return false;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTabPageDesigner"/> class.
		/// </summary>
		public NuGenTabPageDesigner()
		{
		}
	}
}
