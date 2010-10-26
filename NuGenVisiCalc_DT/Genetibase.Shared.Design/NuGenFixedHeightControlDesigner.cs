/* -----------------------------------------------
 * NuGenFixedHeightControlDesigner.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms.Design;

namespace Genetibase.Shared.Design
{
	/// <summary>
	/// Represents a <see cref="ControlDesigner"/> that excludes BottomSizable and TopSizable selection rules.
	/// </summary>
	public class NuGenFixedHeightControlDesigner : ControlDesigner
	{
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

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenFixedHeightControlDesigner"/> class.
		/// </summary>
		public NuGenFixedHeightControlDesigner()
		{
		}
	}
}
