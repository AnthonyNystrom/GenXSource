/* -----------------------------------------------
 * NuGenTextBoxDesigner.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for the <see cref="NuGenTextBox"/>.
	/// </summary>
	public class NuGenTextBoxDesigner : ControlDesigner
	{
		#region Properties.Public.Overridden

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
		/// Initializes a new instance of the <see cref="NuGenTextBoxDesigner"/> class.
		/// </summary>
		public NuGenTextBoxDesigner()
		{
		}

		#endregion
	}
}
