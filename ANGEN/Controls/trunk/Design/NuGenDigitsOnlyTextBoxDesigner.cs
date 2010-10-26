/* -----------------------------------------------
 * NuGenDigitsOnlyTextBoxDesigner.cs
 * Author: Alex Nesterov
 * --------------------------------------------- */

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Controls.Design
{
	/// <summary>
	/// Provides additional design-time functionality for this <see cref="NuGenDigitsOnlyTextBox"/>.
	/// </summary>
	class NuGenDigitsOnlyTextBoxDesigner : ControlDesigner
	{
		#region Properties.Public.Overriden

		/// <summary>
		/// Gets the selection rules that indicate the movement capabilities of a component.
		/// </summary>
		/// <value></value>
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
		/// Initializes a new instance of the <see cref="NuGenDigitsOnlyTextBoxDesigner"/> class.
		/// </summary>
		public NuGenDigitsOnlyTextBoxDesigner()
		{
		}

		#endregion
	}
}
