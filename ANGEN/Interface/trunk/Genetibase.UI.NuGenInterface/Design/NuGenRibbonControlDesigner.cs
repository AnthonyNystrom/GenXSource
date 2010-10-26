/* -----------------------------------------------
 * NuGenRibbonControlDesigner.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.UI.NuGenInterface.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenRibbonControl"/>.
	/// </summary>
	internal class NuGenRibbonControlDesigner : ParentControlDesigner
	{
		#region Properties.Public.Overrriden

		/*
		 * SelectionRules
		 */

		/// <summary>
		/// Gets the selection rules that indicate the movement capabilities of a component.
		/// </summary>
		/// <value></value>
		/// <returns>A bitwise combination of <see cref="T:System.Windows.Forms.Design.SelectionRules"></see> values.</returns>
		public override SelectionRules SelectionRules
		{
			get
			{
				return base.SelectionRules & ~(SelectionRules.BottomSizeable | SelectionRules.TopSizeable);
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenRibbonControlDesigner"/> class.
		/// </summary>
		public NuGenRibbonControlDesigner()
		{
		}

		#endregion
	}
}
