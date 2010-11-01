/* -----------------------------------------------
 * NuGenTabCloseButtonDesigner.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Windows.Forms.Design;

namespace Genetibase.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenTabCloseButton"/>.
	/// </summary>
	class NuGenTabCloseButtonDesigner : ControlDesigner
	{
		#region Properties.Public.Overriden

		/// <summary>
		/// Gets the selection rules that indicate the movement capabilities of a component.
		/// </summary>
		/// <value></value>
		/// <returns>A bitwise combination of <see cref="T:System.Windows.Forms.Design.SelectionRules"></see> values.</returns>
		public override SelectionRules SelectionRules
		{
			get
			{
				return base.SelectionRules & ~(SelectionRules.AllSizeable);
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTabCloseButtonDesigner"/> class.
		/// </summary>
		public NuGenTabCloseButtonDesigner()
		{
		}

		#endregion
	}
}
